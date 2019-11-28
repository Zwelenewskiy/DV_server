CREATE PROCEDURE [dbo].[Add_in_hidden_copy]
	@email_id int,
	@user_id int
AS
BEGIN
	INSERT INTO [dbo].hidden_copy ([email_id],[user_id]) 
	VALUES(@email_id, @user_id);
END

