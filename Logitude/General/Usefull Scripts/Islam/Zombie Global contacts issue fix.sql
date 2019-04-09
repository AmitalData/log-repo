--select * from contacts where id = '1-251631'
--select * from users where id = '1-251631'
--select  * from [Global].[dbo].[GlobalContacts] where id = '1-251631'
--select * from contacts where email = 'system@tenant998.com'
--select * from [Global].[dbo].[GlobalContacts] where email = 'orders@modotec.co.il'


declare @Id  as varchar(15)
declare @Email as varchar(70)
declare @GlobalTenantId as int
declare @Count as int
set @Count = 0;
BEGIN 
		DECLARE globalContactsCursor CURSOR READ_ONLY
		FOR
		select Id,Email,GlobalTenantId
		FROM [Global].[dbo].[GlobalContacts]
		 
       OPEN globalContactsCursor FETCH NEXT FROM globalContactsCursor INTO @Id,@Email,@GlobalTenantId
		
		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				BEGIN
					If not exists (select Id from [Main].[dbo].[Contacts] where id = @Id and tenant = @GlobalTenantId)
					 Begin
					  
					  print 'Id:'+@Id +' ,Email:'+ @Email+' ,Tenant:'+ CAST(@GlobalTenantId as varchar)
					  set @Count +=1;
					  --delete from [LogitudeGlobal-Test2].[dbo].[GlobalContacts] where Id = @Id and GlobalTenantId = @GlobalTenantId
					 End 
				 
				END
             
		    FETCH NEXT FROM globalContactsCursor INTO @Id,@Email,@GlobalTenantId
		    END
		CLOSE globalContactsCursor
		DEALLOCATE globalContactsCursor
		print '------------------------------------------------------------'
		print 'Total Global Zombies Count:' + CAST(@Count as varchar)
END


--ALTER TABLE [LogitudeGlobal-Test2].[dbo].[GlobalContacts]
--ADD CONSTRAINT UQ_Tenant_Email UNIQUE (Email,GlobalTenantId)


