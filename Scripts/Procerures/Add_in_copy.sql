CREATE PROCEDURE [dbo].[Add_in_copy]
	@email_id int,
	@user_id int
AS
BEGIN
	INSERT INTO [dbo].[copy] ([email_id],[user_id]) 
	VALUES(@email_id, @user_id);
END;