declare @DocumentFilingInbox varchar(100) 
declare @Tenant as int
declare @Id as varchar(15)
declare @Check as int
declare @COUNTER as int
declare @usersCount as int 
declare @ContactEmail as varchar(100)
declare @FilingInboxName varchar(100) 
declare @EmailIndex as int = 0

BEGIN
		DECLARE UsersCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM Users
		OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @Id, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @ContactEmail = (select Email from Contacts where Id = @Id and Tenant = @Tenant)
		set @FilingInboxName = Substring(@ContactEmail, 1,Charindex('@', @ContactEmail)-1)+'.'+Substring(Substring(@ContactEmail, Charindex('@', @ContactEmail)+1,LEN(@ContactEmail)-1), 1,Charindex('.', Substring(@ContactEmail, Charindex('@', @ContactEmail)+1,LEN(@ContactEmail)-1))-1) 
		set @Check = 0
		set @EmailIndex = 0
		set @COUNTER = 1
		set @usersCount = (select count(*) from users)
		set @DocumentFilingInbox = @FilingInboxName
		while (@COUNTER <= @usersCount and @Check = 0)
		begin
			set @EmailIndex = @EmailIndex + 1
			if not exists(select * from users where DocumentFilingInbox = @DocumentFilingInbox)
				begin
					set @Check = 1
					SET @COUNTER = @COUNTER + 1
					update Users set  DocumentFilingInbox = @DocumentFilingInbox  where Id = @Id AND Tenant = @Tenant
					break
				end
			else 
				begin 
					set @DocumentFilingInbox = @FilingInboxName + '' + CONVERT(varchar(10),@EmailIndex)
				end
		end

		FETCH NEXT FROM UsersCursor INTO @Id, @Tenant
		END				
		CLOSE UsersCursor
		DEALLOCATE UsersCursor
END