ALTER PROCEDURE SearchByRecipientOrSender
	@recipients xml = NULL,--получатели [to]
	@senders xml = NULL--отправители [from]
AS
BEGIN

	IF ((@recipients IS NOT NULL) AND (@senders IS NOT NULL))
		SELECT 
			em.[id], 
			em.[from], 
			em.[date], 
			em.[content], 
			em.[name]
		FROM
			email em
		WHERE em.[from] IN 
		(
			SELECT
				n.id
			FROM
			(
				SELECT 
					t.n.value('(id)[1]', 'int') AS 'id',
					t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
					t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
				FROM
					@senders.nodes('/ArrayOfUser/User') t(n) 
			) n
			JOIN [user] ON n.email = [user].email--сделать на user.email индекс
		)
		
		UNION

		SELECT
			em.[id], 
			em.[from], 
			em.[date], 
			em.[content], 
			em.[name]
		FROM
			email em
		JOIN [to] t ON em.id = t.email_id
		WHERE t.[user_id] IN
		(
			SELECT
				n.id
			FROM
			(
				SELECT 
					t.n.value('(id)[1]', 'int') AS 'id',
					t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
					t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
				FROM
					@recipients.nodes('/ArrayOfUser/User') t(n) 
			) n
			JOIN [user] ON n.email = [user].email--сделать на user.email индекс
		)	
		ORDER BY em.id
	ELSE IF(@recipients IS NOT NULL)
		SELECT
			em.[id], 
			em.[from], 
			em.[date], 
			em.[content], 
			em.[name]
		FROM
			email em
		JOIN [to] t ON em.id = t.email_id
		WHERE t.[user_id] IN
		(
			SELECT
				n.id
			FROM
			(
				SELECT 
					t.n.value('(id)[1]', 'int') AS 'id',
					t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
					t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
				FROM
					@recipients.nodes('/ArrayOfUser/User') t(n) 
			) n
			JOIN [user] ON n.email = [user].email--сделать на user.email индекс
		)
	ELSE IF(@senders IS NOT NULL)--COALESCE(...) - возвращает первое не null-значение: поиск по имени ИЛИ email
		SELECT  
			*
		FROM
			email em
		WHERE em.[from] IN
		(
			SELECT
				n.id
			FROM
			(
				SELECT 
					t.n.value('(id)[1]', 'int') AS 'id',
					t.n.value('(email)[1]', 'nvarchar(20)') AS 'email',
					t.n.value('(name)[1]', 'nvarchar(20)') AS 'name'
				FROM
					@senders.nodes('/ArrayOfUser/User') t(n) 
			) n
			JOIN [user] ON n.email = [user].email--сделать на user.email индекс
		)
END;

exec SearchByRecipientOrSender NULL, --'<ArrayOfUser><User><id>0</id><name>Иван</name><patronymic>Иванович</patronymic><lastname>Иванов</lastname><email>iva@tt.yy</email></User></ArrayOfUser>',
							   '<ArrayOfUser><User><id>5</id><name>Иван</name><patronymic>Иванович</patronymic><lastname>Иванов</lastname><email>ivPetr@dfg.tt</email></User></ArrayOfUser>'