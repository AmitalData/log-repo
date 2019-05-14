

	--	select Email,Tenant,count(*)
 -- from [Main].[dbo].[Contacts]
 -- 	where email is not null and  LTRIM(RTRIM(email)) <> '' and tenant = 4 
 -- group by Email,Tenant
 -- having count(*) > 1
 

	--	select Code,Tenant,count(*)
 -- from [Main].[dbo].[Users]
 -- 	where Code is not null and  LTRIM(RTRIM(Code)) <> '' and tenant = 4 
 -- group by Code,Tenant
 -- having count(*) > 1
 
 
 --select *
 -- from [Main].[dbo].[Users]
 -- where code is null
 --update users set code = email  where code is null
----------------------------------------------


 declare @Tenant as int
declare @Code as varchar(20)
declare @Count as int
BEGIN 
		DECLARE mainContactsCursor CURSOR READ_ONLY
		FOR
		select Code,Tenant,count(*)
		from [Main].[dbo].[Users]
		where Code is not null and LTRIM(RTRIM(Code)) <> ''
		group by Code,Tenant
		having count(*) > 1
		OPEN mainContactsCursor FETCH NEXT FROM mainContactsCursor INTO @Code,@Tenant,@Count
			WHILE @@FETCH_STATUS = 0
				BEGIN
					 
					if @Code <> ''  and @Code is not null and len(@Code) > 0 ---and @Email like('@') and len(@Email) > 1
						BEGIN
							 
							
							print @Code+ ', Tenant:'+CAST(@Tenant as varchar)+ ', Count:'+CAST(@Count as varchar)
					
							
		        			
		        			declare @InternalTenant as int
							declare @CurrentContactId as varchar(15)
							declare @IsUser as bit
							declare @RowsCount as int
							set @RowsCount = 0
		        			
		        			BEGIN
		        				DECLARE gContactsCursor CURSOR READ_ONLY
								FOR
								SELECT Id,Tenant
								FROM [Main].[dbo].[Users]
								where Code = @Code and Tenant =  @Tenant
								--order by IsUser desc 
	    						OPEN gContactsCursor FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant	
								WHILE @@FETCH_STATUS = 0
								
								BEGIN
									print @CurrentContactId
									if(@RowsCount <> 0)
										BEGIN
										    declare @NewCode as varchar(20)
										    set @NewCode = @Code + CAST(@RowsCount as varchar)
										    
										    print'This user will be modified:' + @CurrentContactId + ' to Code:' +@NewCode
										    
  											update [Main].[dbo].[Users] set Code = @NewCode where id = @CurrentContactId and Tenant = @InternalTenant
										END
								    set @RowsCount = @RowsCount + 1
							        FETCH NEXT FROM gContactsCursor INTO @CurrentContactId,@InternalTenant	
								END
							
							CLOSE gContactsCursor
							DEALLOCATE gContactsCursor
		        			END
		        			 
		        			 
						END
             
					FETCH NEXT FROM mainContactsCursor INTO @Code,@Tenant,@Count
				End
		CLOSE mainContactsCursor
		DEALLOCATE mainContactsCursor
END
