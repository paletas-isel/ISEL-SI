-- Drop Database if exists

use master
go

if exists(select * from sys.databases where name='Escola')
  drop database Escola
go

-- Create database 'Escola'

create database Escola
go

use Escola
go

-- Create tables

CREATE TABLE dbo.Aluno(
	Numero int IDENTITY(1,1) NOT NULL,
	Nome nchar(10) NOT NULL,
	Sexo char(1) NOT NULL
	PRIMARY KEY (Numero)
)

CREATE TABLE dbo.Professor(
	Numero int IDENTITY(1,1) NOT NULL,
	Nome nchar(10) NOT NULL,
	Sexo char(1) NOT NULL,
	PRIMARY KEY (Numero)
)

CREATE TABLE dbo.Disciplina(
	Nome nchar(10) NOT NULL,
	Responsavel int NOT NULL,
	PRIMARY KEY (Nome),
	FOREIGN KEY(Responsavel) REFERENCES dbo.Professor (Numero)
)

CREATE TABLE dbo.Inscricao(
	Numero_Aluno int NOT NULL,
	Disciplina nchar(10) NOT NULL,
	Nota int NULL,
	PRIMARY KEY (Numero_Aluno, Disciplina),
	FOREIGN KEY(Numero_Aluno) REFERENCES dbo.Aluno (Numero),
	FOREIGN KEY(Disciplina) REFERENCES dbo.Disciplina (Nome)
)

-- Insert data

INSERT INTO dbo.Aluno VALUES ('Ana', 'F')
INSERT INTO dbo.Aluno VALUES ('Bernardo', 'M')
INSERT INTO dbo.Aluno VALUES ('Carlos', 'M')
INSERT INTO dbo.Aluno VALUES ('Diogo', 'M')
INSERT INTO dbo.Aluno VALUES ('Elsa', 'F')
INSERT INTO dbo.Aluno VALUES ('Fernanda', 'F')

INSERT INTO dbo.Professor VALUES ('Joao', 'M')
INSERT INTO dbo.Professor VALUES ('Maria', 'F')

INSERT INTO dbo.Disciplina VALUES ('LSD', '2')
INSERT INTO dbo.Disciplina VALUES ('PG', '1')

INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (1, 'PG')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (2, 'PG')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (3, 'LSD')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (3, 'PG')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (5, 'LSD')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (4, 'PG')
INSERT INTO dbo.Inscricao(Numero_Aluno, Disciplina) VALUES (6, 'LSD')

-- View inserted data

select * from Aluno
select * from Disciplina
select * from Professor
select * from Inscricao

-- Create users

CREATE USER Maria FOR LOGIN Maria WITH DEFAULT_SCHEMA=dbo
CREATE USER Joao FOR LOGIN Joao WITH DEFAULT_SCHEMA=dbo
CREATE USER Fernanda FOR LOGIN Fernanda WITH DEFAULT_SCHEMA=dbo
CREATE USER Elsa FOR LOGIN Elsa WITH DEFAULT_SCHEMA=dbo
CREATE USER Diogo FOR LOGIN Diogo WITH DEFAULT_SCHEMA=dbo
CREATE USER Carlos FOR LOGIN Carlos WITH DEFAULT_SCHEMA=dbo
CREATE USER Bernardo FOR LOGIN Bernardo WITH DEFAULT_SCHEMA=dbo
CREATE USER Ana FOR LOGIN Ana WITH DEFAULT_SCHEMA=dbo

-- Create Logins for users (execute after previous step)

--CREATE LOGIN Maria WITH PASSWORD = 'changeit'
--CREATE LOGIN Joao WITH PASSWORD = 'changeit'
--CREATE LOGIN Fernanda WITH PASSWORD = 'changeit'
--CREATE LOGIN Elsa WITH PASSWORD = 'changeit'
--CREATE LOGIN Diogo WITH PASSWORD = 'changeit'
--CREATE LOGIN Carlos WITH PASSWORD = 'changeit'
--CREATE LOGIN Bernardo WITH PASSWORD = 'changeit'
--CREATE LOGIN Ana WITH PASSWORD = 'changeit'