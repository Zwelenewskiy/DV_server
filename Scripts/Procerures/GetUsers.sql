create procedure [dbo].[GetUsers]
as
begin
	SELECT id, lastname, name, patronymic, email FROM [user]
end;