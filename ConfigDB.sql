/* ===========================================================
   ConfigDb: creación/ajuste de dbo.Parametros con VARCHAR(10)
   Reglas SRV4:
     - ParametroId: MAYÚSCULAS A–Z, máx 10, sin espacios/guiones/dígitos
     - Valor: NVARCHAR(500) requerido
     - Descripcion: opcional
     - UpdatedAtUtc: default SYSUTCDATETIME()
   =========================================================== */

IF DB_ID(N'ConfigDb') IS NULL
BEGIN
    CREATE DATABASE [ConfigDb];
END
GO

USE [ConfigDb];
GO

/* 1) Crear la tabla si NO existe (ya con VARCHAR(10)) */
IF NOT EXISTS (
    SELECT 1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'[dbo].[Parametros]')
      AND type = 'U'
)
BEGIN
    CREATE TABLE [dbo].[Parametros] (
        [ParametroId]  VARCHAR(10)   NOT NULL,    -- Solo mayúsculas A–Z
        [Valor]        NVARCHAR(500) NOT NULL,
        [Descripcion]  NVARCHAR(250) NULL,
        [UpdatedAtUtc] DATETIME2(3)  NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT [PK_Parametros] PRIMARY KEY ([ParametroId]),

        -- Checks robustos (usan RTRIM para ignorar espacios a la derecha)
        CONSTRAINT [CK_ParametroId_UpperLen]
            CHECK (RTRIM([ParametroId]) = UPPER(RTRIM([ParametroId]))),

        CONSTRAINT [CK_ParametroId_AlphaOnly]
            CHECK (RTRIM([ParametroId]) NOT LIKE '%[^A-Z]%')
    );

    -- Índice útil si consultas por fecha de actualización
    CREATE INDEX [IX_Parametros_UpdatedAtUtc]
        ON [dbo].[Parametros] ([UpdatedAtUtc]);
END
ELSE
BEGIN
    /* 2) La tabla ya existe: asegurar que la columna sea VARCHAR(10)
          y re-crear restricciones con la regla nueva si es necesario. */

    -- 2.1 Detectar tipo actual de la columna
    DECLARE @colType SYSNAME;
    SELECT @colType = t.name
    FROM sys.columns c
    JOIN sys.types   t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'[dbo].[Parametros]')
      AND c.name = N'ParametroId';

    -- 2.2 Si es CHAR(10) u otro distinto a VARCHAR, cambiamos a VARCHAR(10)
    IF (@colType IS NULL OR UPPER(@colType) <> 'VARCHAR')
    BEGIN
        -- Quitar llaves/constraints dependientes para poder alterar la columna
        IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'PK_Parametros')
            ALTER TABLE dbo.Parametros DROP CONSTRAINT PK_Parametros;

        IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_ParametroId_AlphaOnly')
            ALTER TABLE dbo.Parametros DROP CONSTRAINT CK_ParametroId_AlphaOnly;

        IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_ParametroId_UpperLen')
            ALTER TABLE dbo.Parametros DROP CONSTRAINT CK_ParametroId_UpperLen;

        -- Alterar columna a VARCHAR(10) NOT NULL
        ALTER TABLE dbo.Parametros
            ALTER COLUMN ParametroId VARCHAR(10) NOT NULL;

        -- Volver a crear PK y CHECKs
        ALTER TABLE dbo.Parametros
            ADD CONSTRAINT PK_Parametros PRIMARY KEY (ParametroId);

        ALTER TABLE dbo.Parametros
            ADD CONSTRAINT CK_ParametroId_UpperLen
                CHECK (RTRIM([ParametroId]) = UPPER(RTRIM([ParametroId])));

        ALTER TABLE dbo.Parametros
            ADD CONSTRAINT CK_ParametroId_AlphaOnly
                CHECK (RTRIM([ParametroId]) NOT LIKE '%[^A-Z]%');
    END
    ELSE
    BEGIN
        -- 2.3 Si ya es VARCHAR, asegurar que existan los CHECKs correctos; si faltan, crearlos.
        IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_ParametroId_UpperLen')
        BEGIN
            ALTER TABLE dbo.Parametros
                ADD CONSTRAINT CK_ParametroId_UpperLen
                    CHECK (RTRIM([ParametroId]) = UPPER(RTRIM([ParametroId])));
        END

        IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_ParametroId_AlphaOnly')
        BEGIN
            ALTER TABLE dbo.Parametros
                ADD CONSTRAINT CK_ParametroId_AlphaOnly
                    CHECK (RTRIM([ParametroId]) NOT LIKE '%[^A-Z]%');
        END

        -- 2.4 Asegurar que la PK exista
        IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'PK_Parametros')
        BEGIN
            ALTER TABLE dbo.Parametros
                ADD CONSTRAINT PK_Parametros PRIMARY KEY (ParametroId);
        END
    END

    -- 2.5 Asegurar el índice por UpdatedAtUtc
    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes
        WHERE name = N'IX_Parametros_UpdatedAtUtc'
          AND object_id = OBJECT_ID(N'[dbo].[Parametros]')
    )
    BEGIN
        CREATE INDEX [IX_Parametros_UpdatedAtUtc]
            ON [dbo].[Parametros] ([UpdatedAtUtc]);
    END
END
GO

/* ===========================================================
   (Opcional) Limpieza de valores de prueba antiguos
   =========================================================== */
-- Borra posibles residuos de pruebas con sufijo 'X'
DELETE FROM dbo.Parametros WHERE ParametroId IN ('JWTEXPMINX', 'RFREXPMINX');
GO

-- Verificación final
SELECT ParametroId, Valor, Descripcion, UpdatedAtUtc
FROM dbo.Parametros
ORDER BY ParametroId;
GO