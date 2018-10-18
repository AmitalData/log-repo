
   go

   declare @Tenant as int
   declare @COMMCode as varchar(5)
   BEGIN 
   DECLARE TenantCursor CURSOR READ_ONLY
   FOR
   SELECT Id
   FROM Tenants  
   OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant   
   WHILE @@FETCH_STATUS = 0

   BEGIN         
	set @COMMCode = ( select Code from ChargesGroups where Tenant = @Tenant AND Code ='COMM') 
	if(@COMMCode is  null)
	begin
	    declare @Id as varchar(15)
        declare @Name as varchar(40)
        declare @Code as varchar(5)
	    declare @SearchFields as nvarchar(1000)
        BEGIN 
              DECLARE ChargesGroupsCursor CURSOR READ_ONLY
              FOR
              SELECT Code ,Name, SearchFields
              FROM ChargesGroups  
			  Where Tenant = 0
			  OPEN ChargesGroupsCursor FETCH NEXT FROM ChargesGroupsCursor INTO @Code ,@Name , @SearchFields
              WHILE @@FETCH_STATUS = 0
                     BEGIN
					 EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'ChargesGroup'

		            Update ChargesGroups set LocalName = @Name where Code = @Code and Tenant = 0 and LocalName is null
   
					INSERT INTO ChargesGroups VALUES (@Code,@Name,@SearchFields, @Id ,@Tenant, @Name );

							FETCH NEXT FROM ChargesGroupsCursor   INTO @Code ,@Name , @SearchFields 
                       END
					 
              CLOSE ChargesGroupsCursor
              DEALLOCATE ChargesGroupsCursor
         END

	end 

	FETCH NEXT FROM TenantCursor   INTO @Tenant  
    END 
	 
    CLOSE TenantCursor
    DEALLOCATE TenantCursor

	END
