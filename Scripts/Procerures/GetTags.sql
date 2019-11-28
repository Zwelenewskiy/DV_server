create procedure [dbo].[GetTags]
as
begin
	SELECT id, name FROM [tag]
end;