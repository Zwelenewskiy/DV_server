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

CREATE PROCEDURE [dbo].[SearchByDate]
	@from datetime,
	@to datetime
AS
BEGIN
	CREATE TABLE #result_table
	(
		[id] int, 
		[from] int, 
		[date] datetime, 
		[content] nvarchar(300), 
		[name] nvarchar(20),
		[new_to] xml,
		[new_copy] xml,
		[new_hidden_copy] xml,
		[new_tags] xml,
	)

	CREATE TABLE #result_to (email_id int, [new_to] xml)
	CREATE TABLE #result_copy (email_id int, [new_copy] xml)
	CREATE TABLE #result_hidden_copy (email_id int, [new_hidden_copy] xml)
	CREATE TABLE #result_tags (id int, [new_tags] xml)

	INSERT INTO #result_table(id, [from], [date], content, name)
	SELECT 
		[id], 
		[from], 
		[date], 
		[content], 
		[name]
	FROM 
		email
	WHERE 
		email.date BETWEEN @from AND @to
		



	INSERT INTO #result_to([email_id], [new_to])
	SELECT 
		em.ID, b.[To]
	FROM 
		email em
	CROSS APPLY
	(
		SELECT 
			[user_id] AS 'int'
		FROM 
			[to] 
		WHERE em.ID IN (SELECT ID FROM #result_table ) AND em.ID = [to].email_id 

		FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
	) b([To])

	UPDATE #result_table
	SET
		#result_table.new_to = #result_to.new_to
	FROM #result_table rt
	JOIN #result_to ON rt.ID = #result_to.email_id
	



	INSERT INTO #result_copy([email_id], [new_copy])
	SELECT 
		em.ID, b.[Copy]
	FROM 
		email em
	CROSS APPLY
	(
		SELECT 
			[user_id] AS 'int'
		FROM 
			[copy] 
		WHERE em.ID IN (SELECT ID FROM #result_table ) AND em.ID = [copy].email_id 

		FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
	) b([Copy])

	UPDATE #result_table
	SET
		#result_table.new_copy = #result_copy.new_copy
	FROM #result_table rt
	JOIN #result_copy ON rt.ID = #result_copy.email_id




	INSERT INTO #result_hidden_copy([email_id], [new_hidden_copy])
	SELECT 
		em.ID, b.[Hidden_Copy]
	FROM 
		email em
	CROSS APPLY
	(
		SELECT 
			[user_id] AS 'int'
		FROM 
			[hidden_copy] 
		WHERE em.ID IN (SELECT ID FROM #result_table ) AND em.ID = [hidden_copy].email_id 

		FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
	) b([Hidden_Copy])

	UPDATE #result_table
	SET
		#result_table.new_hidden_copy = #result_hidden_copy.new_hidden_copy
	FROM #result_table rt
	JOIN #result_hidden_copy ON rt.ID = #result_hidden_copy.email_id



	INSERT INTO #result_tags([id], [new_tags])
	SELECT 
		em.ID, b.tag_xml
	FROM 
		email em	
	CROSS APPLY
	(
		SELECT 
			t.ID AS 'id', t.name AS 'name'
		FROM 
			[tag] t
		JOIN email_tag et ON em.ID = et.email_id
		WHERE et.tag_id = t.ID

		FOR XML PATH('Tag'), ROOT('ArrayOfTag'), TYPE
	) b([tag_xml])
	WHERE em.ID IN (SELECT ID FROM #result_table) 

	UPDATE #result_table
	SET
		#result_table.new_tags = #result_tags.new_tags
	FROM #result_table rt
	JOIN #result_tags ON rt.ID = #result_tags.id

	SELECT
		[id], 
		[from], 
		[date], 
		[content], 
		[name],
		[new_to],
		[new_copy],
		[new_hidden_copy],
		[new_tags]
	FROM 
		#result_table
END;