CREATE PROCEDURE SaveEmail
	@from int,
	@date datetime,
	@content nvarchar(300),
	@name nvarchar(20),
	@to xml,
	@copy xml,
	@hidden_copy xml,
	@tags xml
AS
BEGIN
	INSERT INTO email([from], [date], content, name) VALUES
	(@from, @date, @content, @name)
	
	DECLARE @current_email_id int = (SELECT MAX(e.id) FROM email AS e)

	INSERT INTO [to](email_id, [user_id])
	SELECT 
		@current_email_id,
		n.t.value('.', 'int')
	FROM
		@to.nodes('/ArrayOfInt/int') n(t)	

	INSERT INTO [copy](email_id, [user_id])
	SELECT 
		@current_email_id,
		n.t.value('.', 'int')
	FROM
		@copy.nodes('/ArrayOfInt/int') n(t)

	INSERT INTO hidden_copy(email_id, [user_id])
	SELECT 
		@current_email_id,
		n.t.value('.', 'int')
	FROM
		@hidden_copy.nodes('/ArrayOfInt/int') n(t)

	INSERT INTO email_tag(email_id, tag_id)
	SELECT		
		@current_email_id,
		n.t.value('.', 'int')
	FROM
		@tags.nodes('/ArrayOfInt/int') n(t)	
END;