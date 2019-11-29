CREATE PROCEDURE SearchByRecipientOrSender
	@recipients xml = NULL,--получатели [to]
	@senders xml = NULL--отправители [from]
AS
BEGIN
	WITH recepients(id, email, name)
	AS
	(
		SELECT 
			t.n.value('(id)[1]', 'int') AS 'id',
			t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
			t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
		FROM
			@recipients.nodes('/ArrayOfUser/User') t(n) 
	),
	senders(id, email, name)
	AS
	(
		SELECT 
			t.n.value('(id)[1]', 'int') AS 'id',
			t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
			t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
		FROM
			@senders.nodes('/ArrayOfUser/User') t(n) 
	)

	SELECT 
		e.[id], 
		e.[from], 
		e.[date], 
		e.[content], 
		e.[name]
	FROM
		email e
	JOIN [to] t ON e.id = t.email_id
	JOIN recepients r ON t.[user_id] = r.id
	JOIN [user] u ON r.email = u.email OR r.name = u.name
	
	UNION

	SELECT
		e.[id], 
		e.[from], 
		e.[date], 
		e.[content], 
		e.[name]
	FROM
		email e	
	JOIN senders s ON e.[from] = s.id
	JOIN [user] u ON s.email = u.email OR s.name = u.name
END;

exec SearchByRecipientOrSender NULL,--'<ArrayOfUser><User><id>0</id><name>Иван</name><patronymic>Иванович</patronymic><lastname>Иванов</lastname><email>iva@tt.yy</email></User></ArrayOfUser>',
							   '<ArrayOfUser><User><id>5</id><name>Иван</name><patronymic>Сидорович</patronymic><lastname>Петров</lastname><email></email></User>ivPetr@dfg.tt</ArrayOfUser>'--

select * from email
select * from [user]
select * from [to]
