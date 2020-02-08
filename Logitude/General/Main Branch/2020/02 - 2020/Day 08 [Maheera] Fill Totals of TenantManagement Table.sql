declare @TenantId varchar(15)
declare @IsMultiPackage as bit
declare @MainAdditionalPackageApplied as bit
declare @TotalNumberOfUsers as int 
declare @TotalFreeUsers as int 
declare @AveragePrice as decimal 
declare @TotalPaymentamount as decimal 
declare @NumberOfUsers as int 
declare @FreeUsers as int 
declare @TotalPrice as decimal 
declare @TotalPaymentamount_Packages as decimal 

DECLARE TenantManagementsCursor CURSOR READ_ONLY
	FOR	
	SELECT distinct Id, IsMultiPackage, MainAdditionalPackageApplied, NumberOfUsers, FreeUsers, TotalPrice
	FROM TenantManagements 
	OPEN TenantManagementsCursor FETCH NEXT FROM TenantManagementsCursor INTO @TenantId, @IsMultiPackage,@MainAdditionalPackageApplied,@NumberOfUsers, @FreeUsers, @TotalPrice
	WHILE @@FETCH_STATUS = 0
	BEGIN

	    set @TotalNumberOfUsers = (select SUM(isnull(NumberOfUsers,0)) from TenantManagementLicenses where Tenant = @TenantId);
		set @TotalFreeUsers = (select SUM(isnull(FreeUsers,0)) from TenantManagementLicenses where Tenant = @TenantId);
		set @TotalPaymentamount_Packages = (select SUM(isnull(TotalPrice,0)) from TenantManagementLicenses where Tenant = @TenantId);
		
		if (@IsMultiPackage = 0)
		 begin
			set @TotalNumberOfUsers += isnull(@NumberOfUsers,0);
			set @TotalFreeUsers += isnull(@FreeUsers,0);
			set @TotalPaymentamount = isnull(@TotalPrice,0);
		 end
		 else 
		 begin
			set @TotalPaymentamount = isnull(@TotalPaymentamount_Packages,0);
		 end

		 if (@MainAdditionalPackageApplied = 1)
		  begin
			set @TotalPaymentamount = isnull(@TotalPaymentamount_Packages,0) + isnull(@TotalPrice,0);
		 end

		set @AveragePrice = ROUND(@TotalPaymentamount/@TotalNumberOfUsers, 3);
		update TenantManagements set TotalNumberOfUsers=@TotalNumberOfUsers, TotalFreeUsers = @TotalFreeUsers, AveragePrice = @AveragePrice, TotalPaymentamount =@TotalPaymentamount where Id=@TenantId

	FETCH NEXT FROM TenantManagementsCursor INTO @TenantId, @IsMultiPackage,@MainAdditionalPackageApplied,@NumberOfUsers, @FreeUsers, @TotalPrice
	END

CLOSE TenantManagementsCursor	
DEALLOCATE TenantManagementsCursor	


