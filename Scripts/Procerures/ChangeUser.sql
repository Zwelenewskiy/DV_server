CREATE PROCEDURE ChangeUser
		@id int,
		@name nvarchar(20),
		@patronymic nvarchar(15),
		@lastname varchar(20),
		@email nvarchar(20)
AS
BEGIN
	UPDATE [user]
	SET
		name = @name,
		patronymic = @patronymic,
		lastname = @lastname,
		email = @email
	WHERE
		[user].id = @id
END;