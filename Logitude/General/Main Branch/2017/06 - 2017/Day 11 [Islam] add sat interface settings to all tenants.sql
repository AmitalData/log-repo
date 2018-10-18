   
    
    declare @Tenant as int
   declare @SATCode as varchar(4)
   BEGIN 
   DECLARE TenantCursor CURSOR READ_ONLY
   FOR
   SELECT Id
   FROM Tenants  
   OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant   
   WHILE @@FETCH_STATUS = 0

   BEGIN         
	set @SATCode = ( select SATInterfaceCode from SATInterfaceSettings where Tenant = @Tenant) 
	if(@SATCode is  null)
	begin
	   
	   insert into SATInterfaceSettings(Tenant,SATInterfaceCode,Token) values(@Tenant,'NONE',NULL)

	end 

	FETCH NEXT FROM TenantCursor   INTO @Tenant  
    END 
	 
    CLOSE TenantCursor
    DEALLOCATE TenantCursor

	END
