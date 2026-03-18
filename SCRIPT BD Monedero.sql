USE PagosMovilesDB;
GO

CREATE TABLE monedero_movil (
    monedero_id         INT IDENTITY(1,1) PRIMARY KEY,
    numero_cuenta       VARCHAR(20)     NOT NULL,
    identificacion      VARCHAR(20)     NOT NULL,
    numero_telefono     VARCHAR(15)     NOT NULL,
    estado              BIT             NOT NULL DEFAULT 1,
    fecha_creacion      DATETIME        NOT NULL DEFAULT GETDATE(),
    fecha_modificacion  DATETIME        NULL
);

CREATE INDEX IX_monedero_telefono ON monedero_movil (numero_telefono);
CREATE INDEX IX_monedero_identificacion ON monedero_movil (identificacion);
CREATE INDEX IX_monedero_cuenta ON monedero_movil (numero_cuenta);
GO