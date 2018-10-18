
   go

   declare @Tenant as int
   declare @Code as varchar(5)
   declare @ChargesGroupId as varchar(15)
   declare @ChargesTypeId as varchar(15)
   BEGIN 
   DECLARE ChargesTypeCursor CURSOR READ_ONLY
   FOR
   SELECT Id,ChargesGroupCode,Tenant
   FROM ChargesTypes  
   OPEN ChargesTypeCursor FETCH NEXT FROM ChargesTypeCursor INTO @ChargesTypeId, @Code,@Tenant   
   WHILE @@FETCH_STATUS = 0

   BEGIN         
	set @ChargesGroupId = ( select Id from ChargesGroups where Tenant = @Tenant AND Code = @Code) 
	
	update ChargesTypes set ChargesGroupId = @ChargesGroupId  where  Id = @ChargesTypeId

	FETCH NEXT FROM ChargesTypeCursor     INTO @ChargesTypeId ,@Code,@Tenant   
    END 	
			 
    CLOSE ChargesTypeCursor
    DEALLOCATE ChargesTypeCursor

	END
