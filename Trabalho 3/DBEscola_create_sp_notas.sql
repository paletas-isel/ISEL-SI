USE Escola
GO

DROP PROCEDURE p_notas_anonimas
GO

CREATE PROCEDURE p_notas_anonimas 
	@disciplina sysname = NULL,
	@sexo	    sysname = NULL
WITH EXECUTE AS OWNER
AS
BEGIN
	DECLARE @query nvarchar(max)
	SET @query = N'SELECT dbo.Inscricao.Nota ' +
	             N'FROM dbo.Inscricao INNER JOIN dbo.Aluno ON dbo.Aluno.Numero = dbo.Inscricao.Numero_Aluno ' +
	             N'WHERE 1=1 '
	IF @disciplina IS NOT NULL
		SET @query = @query + N' AND dbo.Inscricao.Disciplina=''' + @disciplina + ''''
	IF @sexo IS NOT NULL
		SET @query = @query + N' AND dbo.Aluno.Sexo=''' + @sexo + ''''
	EXEC (@query)
END
GO

