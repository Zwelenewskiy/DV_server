CREATE PROCEDURE [dbo].[Add_in_to]
	@email_id int,
	@user_id int
AS
BEGIN
	INSERT INTO [dbo].[to] ([email_id],[user_id]) 
	VALUES(@email_id, @user_id);
END;
