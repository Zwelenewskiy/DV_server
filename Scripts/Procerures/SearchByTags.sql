CREATE PROCEDURE SearchByTags
	@tags xml
AS
BEGIN	
	SELECT 
        em.[id], 
        em.[from], 
        em.[date], 
        em.[content], 
        em.[name],
        a.new_to,
        b.[new_copy],
        c.[new_hidden_copy],
        d.[new_tags]
	FROM 
		email em
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [to] 
		WHERE em.ID = [to].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) a([new_to])
	CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [copy] 
		WHERE em.ID = [copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) b([new_copy])
    CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [hidden_copy] 
		WHERE em.ID = [hidden_copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) c([new_hidden_copy])
	CROSS APPLY
    (
        SELECT 
            t.ID AS 'id', t.name AS 'name'
        FROM 
            [tag] t
        JOIN email_tag et ON em.ID = et.email_id
        WHERE 
			et.tag_id = t.ID 
 
        FOR XML PATH('Tag'), ROOT('ArrayOfTag'), TYPE
    ) d([new_tags])
	WHERE em.id IN
	(
		SELECT DISTINCT 
			e.id
		FROM 
			tag t
		JOIN email_tag em ON t.ID = em.tag_id
		JOIN email e ON em.email_id = e.ID
		WHERE t.ID IN 
		(
			SELECT 
				n.id 
			FROM 
			(
				SELECT 
					t.n.value('(id)[1]', 'int') AS 'id'
				FROM
					@tags.nodes('/ArrayOfTag/Tag') t(n)
			) n
		)
	)
END;