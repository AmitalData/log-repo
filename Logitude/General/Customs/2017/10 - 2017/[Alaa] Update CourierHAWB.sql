
declare @Id varchar(15)
declare @Tenant integer
--declare @consignments table (DeclarationId varchar(15))
declare @number varchar(35)


    DECLARE Declaration CURSOR
    FOR
    SELECT Id, Tenant   from customs.Declarations 
    OPEN Declaration FETCH NEXT FROM Declaration INTO @Id, @Tenant
    WHILE @@FETCH_STATUS = 0
	BEGIN
	


	    set  @number = (select Top 1 ManifestNumber from customs.Consignments where DeclarationId = @Id and Tenant = @Tenant )
		
	
		update customs.Declarations set CourierHAWB=@number where Id = @Id and Tenant = @Tenant
	
	FETCH NEXT FROM Declaration INTO @Id, @Tenant
	

	END 
	CLOSE Declaration
	DEALLOCATE Declaration

	

