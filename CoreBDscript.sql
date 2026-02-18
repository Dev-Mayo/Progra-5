create database CoreBancario;

use CoreBancario;

--CLIENTES
CREATE TABLE cliente (
    cliente_id INT IDENTITY(1,1) PRIMARY KEY,
    identificacion VARCHAR(20) UNIQUE NOT NULL,
    nombre VARCHAR(15) NOT NULL,
    apellido VARCHAR(25) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    Tipo_Identificacion INT NOT NULL,
    Telefono INT NOT NULL,
    Rol INT NOT NULL,
    ContrasenaHash VARBINARY(64) NOT NULL,
    Estado BIT DEFAULT 1 NOT NULL
);

--CUENTAS
create TABLE cuenta (
    cuenta_id INT IDENTITY(1,1) PRIMARY KEY,
    numero_cuenta VARCHAR(20) UNIQUE NOT NULL,
    cliente_id INT NOT NULL,
    saldo DECIMAL(15,2) NOT NULL DEFAULT 0,
    estado BIT DEFAULT 1,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT fk_cuenta_cliente
        FOREIGN KEY (cliente_id) REFERENCES cliente(cliente_id)
);

--MOVIMIENTOS
create TABLE movimiento (
    movimiento_id INT IDENTITY(1,1) PRIMARY KEY,
    cuenta_id INT NOT NULL,
    tipo_movimiento VARCHAR(10)
        CHECK (tipo_movimiento IN ('DEBITO','CREDITO')),
    monto DECIMAL(15,2) NOT NULL,
    saldo_anterior DECIMAL(15,2) NOT NULL,
    saldo_actual DECIMAL(15,2) NOT NULL,
    descripcion VARCHAR(100),
    fecha_movimiento DATETIME DEFAULT GETDATE(),
    CONSTRAINT fk_movimiento_cuenta
        FOREIGN KEY (cuenta_id) REFERENCES cuenta(cuenta_id)
);

INSERT INTO cliente 
(identificacion, nombre, apellido, Email, Tipo_Identificacion, Telefono, Rol, ContrasenaHash, Estado)
VALUES
('101000001', 'Juan',   'Perez',     'juan.perez@mail.com',   1, 88880001, 1, HASHBYTES('SHA2_256','Password123'), 1),
('101000002', 'Maria',  'Gomez',     'maria.gomez@mail.com',  1, 88880002, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000003', 'Carlos', 'Ramirez',   'carlos.r@mail.com',     1, 88880003, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000004', 'Ana',    'Lopez',     'ana.lopez@mail.com',    1, 88880004, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000005', 'Laura',  'Mora',      'laura.m@mail.com',      1, 88880006, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000006', 'Pedro',  'Soto',      'pedro.s@mail.com',      1, 88880007, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000007', 'Sofia',  'Vargas',    'sofia.v@mail.com',      1, 88880008, 2, HASHBYTES('SHA2_256','Password123'), 1),
('101000008', 'Luis',   'Castro',    'luis.c@mail.com',       1, 88880009, 2, HASHBYTES('SHA2_256','Password123'), 1),
('10100009', 'Elena',  'Rojas',     'elena.r@mail.com',      1, 88880010, 2, HASHBYTES('SHA2_256','Password123'), 1);

INSERT INTO cuenta 
(numero_cuenta, cliente_id, saldo, estado)
VALUES
('CR100001', 1, 250000.00, 1),
('CR100002', 2, 150000.00, 1),
('CR100003', 3, 50000.00,  1),
('CR100004', 4, 300000.00, 1),
('CR100005', 5, 90000.00,  1),
('CR100006', 6, 450000.00, 1),
('CR100007', 7, 80000.00,  1),
('CR100008', 8, 60000.00,  1),
('CR100009',9, 200000.00, 1);

select * from movimiento;
select * from cuenta;
select * from cliente;

INSERT INTO movimiento
(cuenta_id, tipo_movimiento, monto, saldo_anterior, saldo_actual, descripcion)
VALUES
(1, 'CREDITO', 50000.00, 200000.00, 250000.00, 'Deposito inicial'),
(2, 'DEBITO',  20000.00, 170000.00, 150000.00, 'Pago servicio'),
(3, 'CREDITO', 50000.00, 0.00,      50000.00,  'Deposito'),
(4, 'CREDITO', 100000.00,200000.00, 300000.00, 'Transferencia'),
(5, 'CREDITO', 90000.00, 0.00,      90000.00,  'Deposito'),
(6, 'CREDITO', 450000.00,0.00,      450000.00, 'Deposito'),
(7, 'DEBITO',  20000.00, 100000.00, 80000.00,  'Pago movil'),
(8, 'CREDITO', 60000.00, 0.00,      60000.00,  'Deposito'),
(9,'CREDITO', 200000.00,0.00,      200000.00, 'Deposito');


CREATE PROCEDURE sp_AplicarTransaccionPorNumeroCuenta
(
    @NumeroCuenta VARCHAR(20),
    @TipoMovimiento VARCHAR(10), -- 'DEBITO' | 'CREDITO'
    @Monto DECIMAL(15,2),
    @Descripcion VARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CuentaId INT,
        @SaldoAnterior DECIMAL(15,2),
        @SaldoNuevo DECIMAL(15,2);

    BEGIN TRY
        BEGIN TRANSACTION;


        SELECT 
            @CuentaId = cuenta_id,
            @SaldoAnterior = saldo
        FROM cuenta WITH (UPDLOCK, ROWLOCK)
        WHERE numero_cuenta = @NumeroCuenta
          AND estado = 1;

        IF @CuentaId IS NULL
        BEGIN
            THROW 50001, 'La cuenta no existe o está inactiva', 1;
        END

        IF @TipoMovimiento NOT IN ('DEBITO', 'CREDITO')
        BEGIN
            THROW 50002, 'Tipo de movimiento inválido', 1;
        END

        IF @TipoMovimiento = 'DEBITO' AND @SaldoAnterior < @Monto
        BEGIN
            THROW 50003, 'Saldo insuficiente', 1;
        END


        SET @SaldoNuevo =
            CASE
                WHEN @TipoMovimiento = 'DEBITO'
                    THEN @SaldoAnterior - @Monto
                ELSE
                    @SaldoAnterior + @Monto
            END;


        UPDATE cuenta
        SET saldo = @SaldoNuevo
        WHERE cuenta_id = @CuentaId;


        INSERT INTO movimiento
        (
            cuenta_id,
            tipo_movimiento,
            monto,
            saldo_anterior,
            saldo_actual,
            descripcion,
            fecha_movimiento
        )
        VALUES
        (
            @CuentaId,
            @TipoMovimiento,
            @Monto,
            @SaldoAnterior,
            @SaldoNuevo,
            @Descripcion,
            GETDATE()
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

create PROCEDURE sp_ConsultarSaldoPorCuenta
(
    @Identificacion VARCHAR(20),
    @NumeroCuenta VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(c.saldo AS DECIMAL(18,2)) AS saldo
    FROM cuenta c
    INNER JOIN cliente cl
        ON c.cliente_id = cl.cliente_id
    WHERE 
        cl.identificacion = @Identificacion
        AND c.numero_cuenta = @NumeroCuenta
        AND c.estado = 1
        AND cl.estado = 1;

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50010, 'Cuenta no existe o no pertenece al cliente', 1;
    END
END;
GO

create PROCEDURE sp_ConsultarUltimosMovimientos
(
    @Identificacion VARCHAR(20),
    @NumeroCuenta VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM cuenta c
        INNER JOIN cliente cl ON c.cliente_id = cl.cliente_id
        WHERE cl.identificacion = @Identificacion
          AND c.numero_cuenta = @NumeroCuenta
          AND c.estado = 1
          AND cl.estado = 1
    )
    BEGIN
        THROW 50020, 'Cuenta no existe o no pertenece al cliente', 1;
    END

    SELECT TOP 5
        m.movimiento_id     AS MovimientoId,
        m.tipo_movimiento  AS TipoMovimiento,
        m.monto            AS Monto,
        m.saldo_anterior   AS SaldoAnterior,
        m.saldo_actual     AS SaldoActual,
        m.descripcion      AS Descripcion,
        m.fecha_movimiento AS FechaMovimiento
    FROM movimiento m
    INNER JOIN cuenta c ON m.cuenta_id = c.cuenta_id
    INNER JOIN cliente cl ON c.cliente_id = cl.cliente_id
    WHERE cl.identificacion = @Identificacion
      AND c.numero_cuenta = @NumeroCuenta
    ORDER BY m.fecha_movimiento DESC;
END;
GO
