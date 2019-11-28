create procedure [dbo].[SaveEmailTag]
	@t_id int,
	@em_id int
as
begin
	insert into email_tag(tag_id, email_id) values
	(@t_id, @em_id)
end;