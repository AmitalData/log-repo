

   go

   declare @Tenant as int
   declare @Id as int
   BEGIN 
   DECLARE TenantCursor CURSOR READ_ONLY
   FOR
   SELECT Id
   FROM Tenants  
   OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Id   
   WHILE @@FETCH_STATUS = 0

   BEGIN         
	set @Tenant = ( select Tenant from DWHSettings where Tenant = @Id ) 

	if(@Tenant is  null)
	begin
	insert into DWHSettings values (@Id,@Id,null,null,null,null,GETDATE())
	end 

	FETCH NEXT FROM TenantCursor   INTO @Id  
    END 
	 
    CLOSE TenantCursor
    DEALLOCATE TenantCursor

	END
