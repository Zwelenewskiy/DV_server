CREATE PROCEDURE [dbo].[Add_in_email]
	@from int,
	@date datetime,
	@content  nvarchar(300),
	@name nvarchar(20)
AS
BEGIN
	INSERT INTO [dbo].[email] ([from] ,[date] ,[content] ,[name]) 
	VALUES(@from, @date, @content, @name);

	SELECT SCOPE_IDENTITY()
END;

