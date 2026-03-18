CREATE DATABASE Bitacoras
Use Bitacoras
GO
CREATE TABLE Bitacoras (
    IdBitacora INT PRIMARY KEY IDENTITY(1,1),
    UsuarioAccion NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    FechaBitacora DATETIME NOT NULL DEFAULT GETDATE()
);