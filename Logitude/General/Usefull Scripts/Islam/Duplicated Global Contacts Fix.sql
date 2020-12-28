declare @GlobalTenantId as int
declare @Email as varchar(70)
declare @Count as int
 
BEGIN 
		DECLARE globalContactsCursor CURSOR READ_ONLY
		FOR
		
		select Email,GlobalTenantId,count(*)
		from [Global].[dbo].[GlobalContacts]
		group by Email,GlobalTenantId
		having count(*) > 1
  OPEN globalContactsCursor FETCH NEXT FROM globalContactsCursor INTO @Email,@GlobalTenantId,@Count
		
		
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
							
							print @Email+ ', Tenant:'+CAST(@GlobalTenantId as varchar)+ ', Count:'+CAST(@Count as varchar)
			
			------------------------ not valid emails cursor -----------------------------------------
			
declare @InternalTenant as int
declare @CurrentContactId as varchar(15)
 declare @IsUser as bit
declare @RowsCount as int
set @RowsCount = 0
BEGIN 
		DECLARE gContactsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,GlobalTenantId,IsUser
		FROM [Global].[dbo].[GlobalContacts]
		where Email = @Email and GlobalTenantId =  @GlobalTenantId
		order by IsUser desc 
    	OPEN gContactsCursor FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant,@IsUser	
		WHILE @@FETCH_STATUS = 0
			BEGIN
 
 if(@RowsCount <> 0)
		begin
		declare @NewEmail as varchar(70)
	    set @NewEmail = @emailSplit1 + CAST(@RowsCount as varchar) + @emailSplit2
		 
		
		update [Global].[dbo].[GlobalContacts] set Email = @NewEmail where id = @CurrentContactId and GlobalTenantId = @InternalTenant
		update [Main].[dbo].[Contacts] set Email = @NewEmail,ComputedKey= @NewEmail where id = @CurrentContactId and Tenant = @InternalTenant
		
		print'This contact will be modified:' + @CurrentContactId + ' to Email:' +@NewEmail
		
	
			end--print 'hello'
	    else
			begin
 print  @CurrentContactId+' - '+ + @Email + '------ IsUser:' + CAST(@IsUser as varchar)
		    

end
 set @RowsCount = @RowsCount + 1
		   FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant,@IsUser		
			END
		CLOSE gContactsCursor
		DEALLOCATE gContactsCursor
END

			-----------------------------------------------------------------------------------------
			
			
			 print '-------------------------------------------------------------------------------------'
			end
             --ELSE
             --BEGIN
             --END
		   FETCH NEXT FROM globalContactsCursor INTO @Email,@GlobalTenantId,@Count
			
			END
		CLOSE globalContactsCursor
		DEALLOCATE globalContactsCursor
END



		--delete
		--from [LogitudeGlobal-Test2].[dbo].[GlobalContacts]
		--where email is null or email = ''
		
		--ALTER TABLE [LogitudeGlobal-Test2].[dbo].[GlobalContacts]
		--ADD CONSTRAINT UQ_Tenant_Email UNIQUE (Email,GlobalTenantId)



