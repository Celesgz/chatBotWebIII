CREATE DATABASE Botify;
GO

USE Botify;
GO

CREATE SCHEMA Usuario;
GO

CREATE TABLE Usuario.Usuario(
Id INT PRIMARY KEY IDENTITY (1, 1),
Nombre VARCHAR(20) NOT NULL,
Password VARCHAR(255) NOT NULL,
Email VARCHAR(100) NOT NULL
);
GO

ALTER TABLE Usuario.Usuario
ADD CONSTRAINT UQ_Usuario_Nombre UNIQUE (Nombre);

ALTER TABLE Usuario.Usuario
ADD CONSTRAINT UQ_Usuario_Email UNIQUE (Email);
GO
