create procedure [dbo].[GetTo]
as
begin
	SELECT email_id, user_id FROM [to]
end;