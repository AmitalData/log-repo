
--select * from contacts where ComputedKey is null order by createdate desc
--select * from users where id in (select id from contacts where ComputedKey is null )order by createdate desc

--select Email,tenant,count(*)
--  from contacts
--  	where email is not null
--  group by Email,tenant
  
--  having count(*) > 1
  
  --select * from contacts where id = '1-253859' or id ='1-253860'
  
  --select * from users where id = '1-253859' or id ='1-253860'
  
declare @Tenant as int
declare @Email as varchar(70)
declare @Count as int
BEGIN 
		DECLARE mainContactsCursor CURSOR READ_ONLY
		FOR
		select Email,Tenant,count(*)
		from [Main].[dbo].[Contacts]
		where email is not null and LTRIM(RTRIM(email)) <> ''
		group by Email,Tenant
		having count(*) > 1
		OPEN mainContactsCursor FETCH NEXT FROM mainContactsCursor INTO @Email,@Tenant,@Count
			WHILE @@FETCH_STATUS = 0
				BEGIN
					declare @delimiter as varchar(1)
					declare @emailSplit1 as varchar(70)
					declare @emailSplit2 as varchar(70)
					set @delimiter = '@'
					if @Email <> ''  and @Email is not null and len(@Email) > 0 ---and @Email like('@') and len(@Email) > 1
						BEGIN
							set @emailSplit1 = @Email
							set @emailSplit2 =''
							if  CHARINDEX('@',@Email) > 0
							Begin 
								set @emailSplit1 = SUBSTRING(@Email, 1, CHARINDEX(@delimiter, @Email) - 1)
		        				set @emailSplit2 = SUBSTRING(@Email, CHARINDEX(@delimiter, @Email), len(@Email))
							End
							
							print @Email+ ', Tenant:'+CAST(@Tenant as varchar)+ ', Count:'+CAST(@Count as varchar)
					
							
		        			
		        			declare @InternalTenant as int
							declare @CurrentContactId as varchar(15)
							declare @IsUser as bit
							declare @RowsCount as int
							set @RowsCount = 0
		        			
		        			BEGIN
		        				DECLARE gContactsCursor CURSOR READ_ONLY
								FOR
								SELECT Id,Tenant
								FROM [Main].[dbo].[Contacts]
								where Email = @Email and Tenant =  @Tenant
								--order by IsUser desc 
	    						OPEN gContactsCursor FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant	
								WHILE @@FETCH_STATUS = 0
								
								BEGIN
									print @CurrentContactId
									if(@RowsCount <> 0)
										BEGIN
										    declare @NewEmail as varchar(70)
										    set @NewEmail = @emailSplit1 + CAST(@RowsCount as varchar) + @emailSplit2
										    
										    print'This contact will be modified:' + @CurrentContactId + ' to Email:' +@NewEmail
										    update [Global].[dbo].[GlobalContacts] set Email = @NewEmail where id = @CurrentContactId and GlobalTenantId = @InternalTenant
  											update [Main].[dbo].[Contacts] set Email = @NewEmail,ComputedKey= @NewEmail where id = @CurrentContactId and Tenant = @InternalTenant
										END
								    set @RowsCount = @RowsCount + 1
							        FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant	
								END
							
							CLOSE gContactsCursor
							DEALLOCATE gContactsCursor
		        			END
		        			 
		        			 
						END
             
					FETCH NEXT FROM mainContactsCursor INTO @Email,@Tenant,@Count
				End
		CLOSE mainContactsCursor
		DEALLOCATE mainContactsCursor
END
