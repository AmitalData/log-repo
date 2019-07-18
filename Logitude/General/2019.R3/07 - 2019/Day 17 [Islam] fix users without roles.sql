select count(*)
		FROM  users where id  in (select  ContactId from ContactTenants where id not in (select ContactTenantId from ContactTenantRoleSet))
		 

declare @UserId  as varchar(15)
declare @Tenant as int
declare @AdminRoleId as varchar(15)

set @AdminRoleId = (select id from Roles where Code = 'ADMN')


declare @Count as int
set @Count = 0;
BEGIN 
		DECLARE usersCursor CURSOR READ_ONLY
		FOR
		select Id,Tenant
		FROM  users where id  in (select  ContactId from ContactTenants where id not in (select ContactTenantId from ContactTenantRoleSet))
		 
       OPEN usersCursor FETCH NEXT FROM usersCursor INTO @UserId,@Tenant
		
		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				BEGIN
				declare @ContactTenantId as varchar(15)
				set @ContactTenantId = (select id from ContactTenants where ContactId = @UserId and TenantId = @Tenant)

					If @ContactTenantId is null
					 Begin
					  --declare @NewId varchar(15)
    			   	  execute usp_GetNextTableIdValue @ContactTenantId OUTPUT,'@ContactTenant'
					  insert into ContactTenants(Id,TenantId,ContactId) values(@ContactTenantId,@Tenant,@UserId)

					  print 'adding contact tenant for UserId:'+@UserId +' ,Tenant:'+ CAST(@Tenant as varchar)
					
					 End 

					 declare @ContactTenantRoleSetNewId varchar(15)
    			   	 execute usp_GetNextTableIdValue @ContactTenantRoleSetNewId OUTPUT,'ContactTenantRole'

					 print 'new id = ' + @ContactTenantRoleSetNewId
					 insert into ContactTenantRoleSet(Id,Tenant,ContactTenantId,RoleId) values(@ContactTenantRoleSetNewId,@Tenant,@ContactTenantId,@AdminRoleId)
				 
				END

				  set @Count +=1;
             
		    FETCH NEXT FROM usersCursor INTO @UserId,@Tenant
		    END
		CLOSE usersCursor
		DEALLOCATE usersCursor
		print '------------------------------------------------------------'
		print 'Total Fixed Users:' + CAST(@Count as varchar)
END


--ALTER TABLE [LogitudeGlobal-Test2].[dbo].[GlobalContacts]
--ADD CONSTRAINT UQ_Tenant_Email UNIQUE (Email,GlobalTenantId)


