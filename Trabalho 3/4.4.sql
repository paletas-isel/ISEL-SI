USE Escola

CREATE ROLE Professor

EXEC sp_addrolemember 'Professor', 'Joao'
EXEC sp_addrolemember 'Professor', 'Maria'

GO

CREATE VIEW ProfessorsData AS SELECT Professor.Nome AS Professor, Inscricao.Disciplina, Aluno.Nome AS Aluno, Inscricao.Nota FROM 
Professor INNER JOIN Disciplina INNER JOIN Inscricao INNER JOIN Aluno
			ON Inscricao.Numero_Aluno = Aluno.Numero
		ON Disciplina.Nome = Inscricao.Disciplina
	ON Professor.Numero = Disciplina.Responsavel

GO

GRANT Select, Update (Nota) ON ProfessorsData TO Professor

EXEC AS USER = 'Joao'

UPDATE ProfessorsData SET Nota = 19 WHERE Aluno = 'Ana' AND Disciplina = 'PG'

UPDATE ProfessorsData SET Aluno = 'Ana' WHERE Aluno = 'Ana1' /* Tem que falhar! */

REVERT