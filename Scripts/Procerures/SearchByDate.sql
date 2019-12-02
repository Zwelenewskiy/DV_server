CREATE PROCEDURE [dbo].[SearchByDate]
	@from datetime,
	@to datetime
AS
BEGIN
	SELECT 
        em.[id], 
        em.[from], 
        em.[date], 
        em.[content], 
        em.[name],
        a.[to],
        b.[copy],
        c.[hidden_copy],
        d.[tags]
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
    ) a([to])
    CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [copy] 
		WHERE em.ID = [copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) b([copy])
    CROSS APPLY
    (
        SELECT 
            [user_id] AS 'int'
        FROM 
            [hidden_copy] 
		WHERE em.ID = [hidden_copy].email_id 
 
        FOR XML PATH(''), ROOT('ArrayOfInt'), TYPE
    ) c([hidden_copy])
    CROSS APPLY
    (
        SELECT 
            t.ID AS 'id', t.name AS 'name'
        FROM 
            [tag] t
        JOIN email_tag et ON em.ID = et.email_id
        WHERE et.tag_id = t.ID
 
        FOR XML PATH('Tag'), ROOT('ArrayOfTag'), TYPE
    ) d([tags])
	WHERE 
		em.date BETWEEN @from AND @to
END;