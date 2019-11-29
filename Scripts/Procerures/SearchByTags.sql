CREATE PROCEDURE SearchByTags
	@tags xml
AS
BEGIN	
	With CTE1(TagId) AS
	(
		SELECT 
			t.n.value('.', 'int') AS 'id'
		FROM
			@tags.nodes('/ArrayOfInt/int') t(n)
	),
	EmailIds(MailId) AS
	(
		SELECT DISTINCT 
			em.email_id
		FROM 
			email_tag em
		JOIN CTE1 tHelp ON em.tag_id = tHelp.TagId
	)

	SELECT
        e.[id], 
        e.[from], 
        e.[date], 
        e.[content], 
        e.[name],
        a.new_to,
        b.[new_copy],
        c.[new_hidden_copy],
        d.[new_tags]
	FROM 
		email e
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [to] 
		WHERE e.ID = [to].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) a([new_to])
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [copy] 
		WHERE e.ID = [copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) b([new_copy])
    CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [hidden_copy] 
		WHERE e.ID = [hidden_copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) c([new_hidden_copy])
	CROSS APPLY
    (
        SELECT 
            t.ID AS 'id', t.name AS 'name'
        FROM 
            [tag] t
        JOIN email_tag et ON e.ID = et.email_id
        WHERE 
			et.tag_id = t.ID 
		
        FOR XML PATH('Tag'), ROOT('ArrayOfTag'), TYPE
    ) d([new_tags])	
	JOIN email_tag em ON e.id = em.email_id
	JOIN tag t ON em.tag_id = t.id
	WHERE e.id IN (SELECT MailId FROM EmailIds) 
	AND 
	t.id IN
	(
		SELECT 
				n.id 
		FROM 
		(
			SELECT 
				t.n.value('.', 'int') AS 'id'
			FROM
				@tags.nodes('/ArrayOfInt/int') t(n)
		) n
	)
END;