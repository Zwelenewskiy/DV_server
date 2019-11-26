create procedure UpdateEmail
	@ID int,
	@from int,
	@date datetime,
	@content nvarchar(300),
	@name nvarchar(20),
	@to xml
AS
begin

	UPDATE email 
	SET
		[from] = @from,
		[date] = @date,
		[content] = @content,
		[name] = @name
	WHERE email.ID = @ID

	CREATE TABLE #to_for_insert_tmp ([email_id] int, [user_id] int)
	
	SELECT @ID as email_id, tmp.n.value('int[1]', 'int') as user_id
	INTO to_for_insert_tmp
	FROM @to.nodes('/ArrayOfInt') tmp(n)
		
	UPDATE [to]
	SET
		[to].user_id = #to_for_insert_tmp.user_id
	FROM [to]
	JOIN #to_for_insert_tmp ON [to].email_id = #to_for_insert_tmp.email_id

	DROP TABLE #to_for_insert_tmp
end;