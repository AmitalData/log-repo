declare @TenantId varchar(15)
declare @AddOnTenantId varchar(15)
declare @LicensesTenantId varchar(15)
declare @AddOnTenantPackageCode varchar(60)
declare @LicensesPackageCode varchar(60)
declare @PackageCodeSearchField AS varchar(250)

DECLARE PackageCodeSearchFieldCursor CURSOR READ_ONLY
	FOR	
	SELECT distinct Id
	FROM TenantManagements 
	OPEN PackageCodeSearchFieldCursor FETCH NEXT FROM PackageCodeSearchFieldCursor INTO @TenantId 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	    set @PackageCodeSearchField = '';
		DECLARE PackageAddOnsCursor CURSOR READ_ONLY
		FOR	
			SELECT  Id, PackageCode
			FROM TenantAddOns 
			where Tenant = @TenantId
			OPEN PackageAddOnsCursor FETCH NEXT FROM PackageAddOnsCursor INTO @AddOnTenantId, @AddOnTenantPackageCode  
			WHILE @@FETCH_STATUS = 0
			BEGIN
				set @PackageCodeSearchField = COALESCE(@PackageCodeSearchField  + ',', '') + @AddOnTenantPackageCode
			FETCH NEXT FROM PackageAddOnsCursor INTO @AddOnTenantId, @AddOnTenantPackageCode  
			END
		CLOSE PackageAddOnsCursor	
		DEALLOCATE PackageAddOnsCursor

		DECLARE TenantManagementLicenseCursor CURSOR READ_ONLY
		FOR	
			SELECT  Id, PackageCode
			FROM TenantManagementLicenses 
			where Tenant = @TenantId
			OPEN TenantManagementLicenseCursor FETCH NEXT FROM TenantManagementLicenseCursor INTO @LicensesTenantId, @AddOnTenantPackageCode  
			WHILE @@FETCH_STATUS = 0
			BEGIN
				set @PackageCodeSearchField = COALESCE(@PackageCodeSearchField  + ',', '') + @AddOnTenantPackageCode
			FETCH NEXT FROM TenantManagementLicenseCursor INTO @LicensesTenantId, @AddOnTenantPackageCode  
			END
		CLOSE TenantManagementLicenseCursor	
		DEALLOCATE TenantManagementLicenseCursor

		set @PackageCodeSearchField = COALESCE(@PackageCodeSearchField  + ',', '') + (SELECT PackageCode FROM TenantManagements where Id = @TenantId)

		update TenantManagements set PackageCodeSearchField=@PackageCodeSearchField where Id=@TenantId

	FETCH NEXT FROM PackageCodeSearchFieldCursor INTO @TenantId
	END

CLOSE PackageCodeSearchFieldCursor	
DEALLOCATE PackageCodeSearchFieldCursor	


