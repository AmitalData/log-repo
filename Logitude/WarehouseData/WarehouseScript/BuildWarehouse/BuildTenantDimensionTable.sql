

    declare @Id as int
    declare @Company as varchar(100)
    declare @Country as varchar(100)

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR

	SELECT dw_Tenants.Id, dw_Tenants.Company,dw_Countries.EnglishName
	From dw_Tenants
	INNER JOIN dw_Addresses ON dw_Tenants.AddressId = dw_Addresses.Id
	INNER JOIN dw_Countries ON dw_Addresses.CountryId = dw_Countries.Id

	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Id , @Company, @Country
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_TenantsTemp ([Tenant Number] , [Tenant Name] , [Country]) values(@Id,@Company,@Country)

	FETCH NEXT FROM TenantsCursor  INTO @Id , @Company, @Country
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	
