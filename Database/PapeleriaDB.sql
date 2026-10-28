IF DB_ID('PapeleriaDB') IS NULL
    CREATE DATABASE PapeleriaDB;
GO

USE PapeleriaDB;
GO

-- Tablas (solo se crean si no existen)
IF OBJECT_ID('dbo.Categorias', 'U') IS NULL
BEGIN
    CREATE TABLE Categorias (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Proveedores', 'U') IS NULL
BEGIN
    CREATE TABLE Proveedores (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Telefono NVARCHAR(20) NULL
    );
END
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE Productos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(150) NOT NULL,
        Precio DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,
        CategoriaId INT NOT NULL,
        ProveedorId INT NOT NULL,
        CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id),
        CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (ProveedorId) REFERENCES Proveedores(Id)
    );
END
GO

-- Datos de ejemplo (solo se agregan si no existen)
INSERT INTO Categorias (Nombre)
SELECT v.Nombre
FROM (VALUES (N'Cuadernos'), (N'Escolar'), (N'Papel'), (N'Plumas y lápices')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Categorias c WHERE c.Nombre = v.Nombre);
GO

INSERT INTO Proveedores (Nombre, Telefono)
SELECT N'Distribuidora Chiapas', N'9611234567'
WHERE NOT EXISTS (SELECT 1 FROM Proveedores WHERE Nombre = N'Distribuidora Chiapas');
GO

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId, ProveedorId)
SELECT N'Bolígrafo azul', 8.50, 100,
       (SELECT TOP 1 Id FROM Categorias WHERE Nombre = N'Plumas y lápices'),
       (SELECT TOP 1 Id FROM Proveedores WHERE Nombre = N'Distribuidora Chiapas')
WHERE NOT EXISTS (SELECT 1 FROM Productos WHERE Nombre = N'Bolígrafo azul');
GO

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId, ProveedorId)
SELECT N'Cuaderno profesional 100 hojas', 45.00, 30,
       (SELECT TOP 1 Id FROM Categorias WHERE Nombre = N'Cuadernos'),
       (SELECT TOP 1 Id FROM Proveedores WHERE Nombre = N'Distribuidora Chiapas')
WHERE NOT EXISTS (SELECT 1 FROM Productos WHERE Nombre = N'Cuaderno profesional 100 hojas');
GO

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId, ProveedorId)
SELECT N'Paquete de hojas blancas', 95.00, 20,
       (SELECT TOP 1 Id FROM Categorias WHERE Nombre = N'Papel'),
       (SELECT TOP 1 Id FROM Proveedores WHERE Nombre = N'Distribuidora Chiapas')
WHERE NOT EXISTS (SELECT 1 FROM Productos WHERE Nombre = N'Paquete de hojas blancas');
GO