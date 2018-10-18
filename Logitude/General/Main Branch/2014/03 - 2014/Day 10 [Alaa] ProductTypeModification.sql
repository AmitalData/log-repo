
BEGIN
DECLARE @Code varchar(2)

DECLARE ProdutTypeCursor CURSOR READ_ONLY
	FOR	
	SELECT Code
	FROM ProductTypes
	OPEN ProdutTypeCursor FETCH NEXT FROM ProdutTypeCursor INTO @Code
	

	WHILE @@FETCH_STATUS = 0
	BEGIN
		if (@Code = 'AD' OR @Code = 'AR' OR @Code = 'OD' OR @Code = 'OR' OR @Code = 'ID' OR @Code = 'IR')
		BEGIN
	 DECLARE @Tenant int 
	
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	  
	   print @Tenant
       Insert into ProductTypeModifications values (@Code,@Tenant, 1)

	FETCH NEXT FROM TenantCursor INTO @Tenant
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor

	END
	
	FETCH NEXT FROM ProdutTypeCursor INTO @Code
	END
	CLOSE ProdutTypeCursor
	DEALLOCATE ProdutTypeCursor
	END