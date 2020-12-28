--select * from contacts where id = '1-100003'
--select * from contacts where ComputedKey  = 'marcelo.hinojosa@pak2go.com.mx'


--select Email,tenant,count(*)
--  from contacts
--  	where email is not null
--  group by Email,tenant
  
--  having count(*) > 1
  
  --select * from contacts where id = '1-253859' or id ='1-253860'
  
  --select * from users where id = '1-253859' or id ='1-253860'
  
  --select * from contacts where  LTRIM(RTRIM(computedkey)) = ''

--  ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [UQ_Tenant_ComputedKey_Contacts] UNIQUE NONCLUSTERED 
--(
--	[Tenant] ASC,
--	[ComputedKey] ASC
--)


declare @Id  as varchar(15)  
declare @Tenant as int
declare @Email as varchar(70)
declare @Count as int
declare @ComputedKey as varchar(70)
BEGIN 
		DECLARE mainContactsCursor CURSOR READ_ONLY
		FOR
		select Id,Email,Tenant,ComputedKey
		from [Main].[dbo].[Contacts]
		where (ComputedKey <> Email and ComputedKey is not  null and LTRIM(RTRIM(email)) <> '') or (computedkey is null)
		OPEN mainContactsCursor FETCH NEXT FROM mainContactsCursor INTO @Id,@Email,@Tenant,@ComputedKey
			WHILE @@FETCH_STATUS = 0
				BEGIN
					 
						BEGIN
							if(@Email is null or @Email = '')
								BEGIN
									set @ComputedKey = @Id
								END
							else
								BEGIN
									set @ComputedKey = @Email
								END
							
							print 'Computed key for ContactId:'+ @Id  +', Tenant:' + CAST(@Tenant as varchar) + ' is:'+@ComputedKey
		        			update [Main].[dbo].[Contacts] set ComputedKey =@ComputedKey where id = @Id and tenant = @Tenant
						END
             
					FETCH NEXT FROM mainContactsCursor INTO @Id,@Email,@Tenant,@ComputedKey
				End
		CLOSE mainContactsCursor
		DEALLOCATE mainContactsCursor
END

  --ALTER TABLE [LogitudeMain-Test2].[dbo].[Contacts]
  --ADD CONSTRAINT ComputedKey_Contact UNIQUE (ComputedKey,Tenant);

