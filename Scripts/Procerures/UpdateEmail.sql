CREATE procedure [dbo].[UpdateEmail]
	@ID int,
	@from int,
	@date datetime,
	@content nvarchar(300),
	@name nvarchar(20),
	@new_to xml,
	@new_copy xml,
	@new_hidden_copy xml,
	@new_email_tag xml
AS
begin
	UPDATE email 
	SET
		[from] = @from,
		[date] = @date,
		[content] = @content,
		[name] = @name
	WHERE email.ID = @ID

	DELETE FROM [to] 
	WHERE [to].email_id = @ID

	DELETE FROM [copy] 
	WHERE [copy].email_id = @ID

	DELETE FROM [hidden_copy]
	WHERE [hidden_copy].email_id = @ID

	DELETE FROM [email_tag] 
	WHERE [email_tag].email_id = @ID

	INSERT INTO [to]([email_id], [user_id])
		SELECT 
			@ID as email_id, tmp.n.value('.', 'int') as [user_id]
		FROM 
			@new_to.nodes('/ArrayOfInt/int') tmp(n)

	INSERT INTO [copy]([email_id], [user_id])
		SELECT 
			@ID as email_id, tmp1.n.value('.', 'int') as [user_id]
		FROM 
			@new_copy.nodes('/ArrayOfInt/int') tmp1(n)

	INSERT INTO [hidden_copy]([email_id], [user_id])
		SELECT 
			@ID as email_id, tmp2.n.value('.', 'int') as [user_id]
		FROM 
			@new_hidden_copy.nodes('/ArrayOfInt/int') tmp2(n)

	INSERT INTO [email_tag]([email_id], [tag_id])
		SELECT 
			@ID as [email_id], tmp3.n.value('.', 'int') as [tag_id]
		FROM 
			@new_email_tag.nodes('/ArrayOfInt/int') tmp3(n)
end;