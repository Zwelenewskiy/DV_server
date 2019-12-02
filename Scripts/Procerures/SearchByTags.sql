CREATE PROCEDURE SearchByTags
	@tags xml
AS
BEGIN	
	With TagIds(TagId) AS
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
		JOIN TagIds tTids ON em.tag_id = tTids.TagId
	)

	SELECT
        e.[id], 
        e.[from], 
        e.[date], 
        e.[content], 
        e.[name],
        a.[to],
        b.[copy],
        c.[hidden_copy],
        d.[tags]
	FROM 
		email e
	JOIN EmailIds tIds ON tIds.MailId = e.id
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [to] 
		WHERE e.ID = [to].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) a([to])
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [copy] 
		WHERE e.ID = [copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) b([copy])
    CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [hidden_copy] 
		WHERE e.ID = [hidden_copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) c([hidden_copy])
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
    ) d([tags])	
END;