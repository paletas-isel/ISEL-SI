USE Escola

GRANT SELECT ON Aluno TO Ana;
GO

REVOKE SELECT ON Aluno TO Ana
GO

CREATE ROLE Aluno
EXEC sp_addrolemember 'Aluno', 'Ana'
EXEC sp_addrolemember 'Aluno', 'Bernardo'
EXEC sp_addrolemember 'Aluno', 'Carlos'
EXEC sp_addrolemember 'Aluno', 'Diogo'
EXEC sp_addrolemember 'Aluno', 'Elsa'
EXEC sp_addrolemember 'Aluno', 'Fernanda'

GRANT SELECT ON Aluno TO Aluno

GO

DENY SELECT ON Aluno TO Ana

GO

GRANT SELECT ON Aluno TO Ana

GO