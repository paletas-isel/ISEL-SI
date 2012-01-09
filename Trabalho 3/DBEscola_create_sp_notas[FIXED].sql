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
	DECLARE @ParmDef nvarchar(max)
	
	SET @query = N'SELECT dbo.Inscricao.Nota ' +
				 N'FROM dbo.Inscricao INNER JOIN dbo.Aluno ON dbo.Aluno.Numero = dbo.Inscricao.Numero_Aluno ' +
				 N'WHERE 1=1'
				 
	SET @ParmDef = N''
	
	IF @disciplina IS NOT NULL
	BEGIN
		SET @query = @query + N' AND dbo.Inscricao.Disciplina=@disciplina'
		SET @ParmDef = @ParmDef + N'@disciplina nchar(10)'
	END
	IF @sexo IS NOT NULL
	BEGIN
		SET @query = @query + N' AND dbo.Aluno.Sexo=@sexo'
		IF @disciplina IS NOT NULL
			SET @ParmDef = @ParmDef + N', '		
		SET @ParmDef = @ParmDef + N'@sexo char(1)'
	END
	
	IF @disciplina IS NOT NULL AND @sexo IS NOT NULL	
		EXECUTE sp_executesql @query, @ParmDef, @disciplina, @sexo 
	ELSE IF @disciplina IS NOT NULL
		EXECUTE sp_executesql @query, @ParmDef, @disciplina
	ELSE IF @sexo IS NOT NULL	
		EXECUTE sp_executesql @query, @ParmDef, @sexo
	ELSE
		EXECUTE sp_executesql @query, @ParmDef
END
GO

