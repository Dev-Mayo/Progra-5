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

ALTER TABLE cliente
ADD fecha_nacimiento Date NOT NULL DEFAULT '2002-01-15';

ALTER TABLE cuenta
ADD TipoCuenta VARCHAR(20) NOT NULL DEFAULT 'Corriente';

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

-----------------------------------------------------------------------
----------------------------SA11---------------------------------------
-----------------------------------------------------------------------
create PROCEDURE sp_CrearCuenta
(
    @Identificacion INT, 
    @TipoCuenta VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CuentaId INT;
    DECLARE @NumeroCuenta VARCHAR(20);
    DECLARE @UltimoNumero INT;
    DECLARE @NuevoNumero INT;
    DECLARE @ClienteId INT;
    DECLARE @Prefix VARCHAR(2);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1 
            FROM cliente 
            WHERE identificacion = @Identificacion 
              AND estado = 1
        )
        BEGIN
            THROW 50070, 'El cliente no existe o está inactivo', 1;
        END

        IF @TipoCuenta NOT IN ('Corriente', 'Ahorros')
        BEGIN
            THROW 50071, 'Tipo de cuenta inválido. Debe ser Corriente o Ahorros', 1;
        END

        IF @TipoCuenta = 'Corriente'
            SET @Prefix = 'CR';
        ELSE
            SET @Prefix = 'CR'; 

        SELECT TOP 1 @UltimoNumero = CAST(SUBSTRING(numero_cuenta, LEN(@Prefix) + 1, LEN(numero_cuenta) - LEN(@Prefix)) AS INT)
        FROM cuenta WITH (UPDLOCK, ROWLOCK)
        WHERE numero_cuenta LIKE @Prefix + '%'
        ORDER BY cuenta_id DESC;

        IF @UltimoNumero IS NULL
            SET @NuevoNumero = 1;
        ELSE
            SET @NuevoNumero = @UltimoNumero + 1;

        SET @NumeroCuenta = @Prefix + RIGHT('100000' + CAST(@NuevoNumero AS VARCHAR(10)), 5);

        SELECT @ClienteId = cliente_id 
        FROM cliente 
        WHERE identificacion = @Identificacion 
          AND estado = 1;

        INSERT INTO cuenta 
        (
            numero_cuenta,
            cliente_id,
            saldo,
            estado,
            fecha_creacion,
            TipoCuenta
        )
        VALUES
        (
            @NumeroCuenta,
            @ClienteId,
            0.00,
            1,
            GETDATE(),
            @TipoCuenta
        );

        SET @CuentaId = SCOPE_IDENTITY();
        SELECT @CuentaId AS CuentaId, @NumeroCuenta AS NumeroCuenta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

create PROCEDURE sp_EditarCuenta
(
    @ClienteId INT,
    @NumeroCuenta VARCHAR(20),
    @TipoCuenta VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ClienteId2 INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar que el tipo de cuenta sea válido
        IF @TipoCuenta NOT IN ('Corriente', 'Ahorros')
        BEGIN
            THROW 50030, 'Tipo de cuenta inválido. Debe ser Corriente o Ahorros', 1;
        END

        SELECT @ClienteId2 = cliente_id 
        FROM cliente 
        WHERE identificacion = @ClienteId 
          AND estado = 1;

        -- Validar que la cuenta exista y pertenezca al cliente
        IF NOT EXISTS (
            SELECT 1 
            FROM cuenta c
            INNER JOIN cliente cl ON c.cliente_id = cl.cliente_id
            WHERE c.numero_cuenta = @NumeroCuenta
              AND c.cliente_id = @ClienteId2
              AND c.estado = 1
              AND cl.estado = 1
        )
        IF NOT EXISTS (
            SELECT 1 
            FROM cliente 
            WHERE identificacion = @ClienteId 
              AND estado = 1
        )
        BEGIN
            THROW 50031, 'La cuenta no existe o no pertenece al cliente', 1;
        END

        -- Actualizar el tipo de cuenta
        UPDATE cuenta
        SET TipoCuenta = @TipoCuenta
        WHERE numero_cuenta = @NumeroCuenta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

create PROCEDURE sp_EliminarCuenta
(
    @ClienteId INT,
    @NumeroCuenta VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ClienteId2 INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @ClienteId2 = cliente_id 
        FROM cliente 
        WHERE identificacion = @ClienteId 
          AND estado = 1;

        -- Validar que la cuenta exista y pertenezca al cliente
        IF NOT EXISTS (
            SELECT 1 
            FROM cuenta c
            INNER JOIN cliente cl ON c.cliente_id = cl.cliente_id
            WHERE c.numero_cuenta = @NumeroCuenta
              AND c.cliente_id = @ClienteId2
              AND c.estado = 1
              AND cl.estado = 1
        )
        IF NOT EXISTS (
            SELECT 1 
            FROM cliente 
            WHERE identificacion = @ClienteId 
              AND estado = 1
        )
        BEGIN
            THROW 50040, 'La cuenta no existe o no pertenece al cliente', 1;
        END

        -- Validar que no tenga movimientos pendientes (opcional, según negocio)
        IF EXISTS (
            SELECT 1 
            FROM movimiento m
            WHERE m.cuenta_id = (SELECT cuenta_id FROM cuenta WHERE numero_cuenta = @NumeroCuenta)
        )
        BEGIN
            THROW 50041, 'No se puede eliminar una cuenta con movimientos registrados', 1;
        END

        -- Soft delete (marcar como inactiva)
        UPDATE cuenta
        SET estado = 0
        WHERE numero_cuenta = @NumeroCuenta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

alter PROCEDURE sp_ListarTodas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        numero_cuenta AS NumeroCuenta,
        saldo AS Saldo,
        TipoCuenta AS TipoCuenta,
        estado AS Estado,
        fecha_creacion AS FechaCreacion,
        cliente_id AS ClienteId
    FROM cuenta
    WHERE estado = 1
    ORDER BY fecha_creacion DESC;
END;
GO

create PROCEDURE sp_ListarPorLlavePrimaria
(
    @NumeroCuenta VARCHAR(20)
)
AS
BEGIN

    Declare @ClienteId INT;
    SET NOCOUNT ON;

    SELECT @ClienteId = cliente_id 
        FROM cuenta 
        WHERE numero_cuenta = @NumeroCuenta 
          AND estado = 1;

    IF NOT EXISTS (
            SELECT 1 
            FROM cliente 
            WHERE cliente_id = @ClienteId 
              AND estado = 1
        )
    BEGIN
        THROW 50071, 'La cuenta no existe', 1;
    END

    SELECT
        numero_cuenta AS NumeroCuenta,
        saldo AS Saldo,
        TipoCuenta AS TipoCuenta,
        estado AS Estado,
        fecha_creacion AS FechaCreacion,
        cliente_id AS ClienteId
    FROM cuenta
    WHERE numero_cuenta = @NumeroCuenta
      AND estado = 1

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50050, 'Cuenta no encontrada', 1;
    END
END;
GO

create PROCEDURE sp_ListarPorCliente
(
    @ClienteId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ClienteId2 INT;


    SELECT @ClienteId2 = cliente_id 
        FROM cliente 
        WHERE identificacion = @ClienteId 
          AND estado = 1;

    IF NOT EXISTS (
            SELECT 1 
            FROM cliente 
            WHERE cliente_id = @ClienteId2 
              AND estado = 1
        )
    BEGIN
        THROW 50081, ' El cliente consultado no existe o no tiene cuentas asociadas', 1;
    END

    SELECT 
        numero_cuenta AS NumeroCuenta,
        saldo AS Saldo,
        TipoCuenta AS TipoCuenta,
        estado AS Estado,
        fecha_creacion AS FechaCreacion,
        cliente_id AS ClienteId
    FROM cuenta
    WHERE cliente_id = @ClienteId2
      AND estado = 1
    ORDER BY fecha_creacion DESC;

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50060, 'Cliente no encontrado o no tiene cuentas activas', 1;
    END
END;
GO
