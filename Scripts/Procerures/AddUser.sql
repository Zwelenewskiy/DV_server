CREATE PROCEDURE AddUser
	@users xml
AS
BEGIN
	INSERT INTO [user](name, patronymic, lastname, email)
	SELECT 
			t.n.value('(name)[1]', 'nvarchar(20)') AS 'name',
			t.n.value('(patronymic)[1]', 'nvarchar(15)') AS 'patronymic',
			t.n.value('(lastname)[1]', 'nvarchar(20)') AS 'lastname',
			t.n.value('(email)[1]', 'nvarchar(20)') AS 'email'
	FROM
		@users.nodes('/ArrayOfUser/User') t(n)
END;