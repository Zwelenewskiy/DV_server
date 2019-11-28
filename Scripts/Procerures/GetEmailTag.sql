create procedure [dbo].[GetEmailTag]
as
begin
	SELECT email_id, tag_id FROM email_tag
end;
