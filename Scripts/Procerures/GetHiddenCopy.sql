create procedure [dbo].[GetHiddenCopy]
as
begin
	SELECT email_id, user_id FROM [hidden_copy]
end;
