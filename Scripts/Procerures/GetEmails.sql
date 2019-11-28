create procedure [dbo].[GetEmails]
as
begin
	SELECT [id], name, [date], [from], content FROM email
end;

