create procedure [dbo].[GetCopy]
as
begin
	SELECT email_id, user_id FROM [copy]
end;
