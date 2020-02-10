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
declare @TotalNumberOfUsers_Packages as int 
declare @TotalFreeUsers_Packages as int 

DECLARE TenantManagementsCursor CURSOR READ_ONLY
	FOR	
	SELECT distinct Id, IsMultiPackage, MainAdditionalPackageApplied, NumberOfUsers, FreeUsers, TotalPrice
	FROM TenantManagements 
	OPEN TenantManagementsCursor FETCH NEXT FROM TenantManagementsCursor INTO @TenantId, @IsMultiPackage,@MainAdditionalPackageApplied,@NumberOfUsers, @FreeUsers, @TotalPrice
	WHILE @@FETCH_STATUS = 0
	BEGIN

	    set @TotalNumberOfUsers_Packages = (select SUM(isnull(NumberOfUsers,0)) from TenantManagementLicenses where Tenant = @TenantId);
		set @TotalFreeUsers_Packages = (select SUM(isnull(FreeUsers,0)) from TenantManagementLicenses where Tenant = @TenantId);
		set @TotalPaymentamount_Packages = (select SUM(isnull(TotalPrice,0)) from TenantManagementLicenses where Tenant = @TenantId);
		set @AveragePrice = 0;

		if (@IsMultiPackage = 0)
		 begin
			set @TotalNumberOfUsers = isnull(@NumberOfUsers,0);
			set @TotalFreeUsers = isnull(@FreeUsers,0);
			set @TotalPaymentamount = isnull(@TotalPrice,0);
		 end
		 else 
		 begin
			set @TotalPaymentamount = isnull(@TotalPaymentamount_Packages,0);
			set @TotalFreeUsers = isnull(@TotalFreeUsers_Packages,0);
			set @TotalNumberOfUsers = isnull(@TotalNumberOfUsers_Packages,0);
		 end

		 if (@MainAdditionalPackageApplied = 1)
		  begin
			set @TotalPaymentamount = isnull(@TotalPaymentamount_Packages,0) + isnull(@TotalPrice,0);
			set @TotalFreeUsers = isnull(@TotalFreeUsers_Packages,0) + isnull(@NumberOfUsers,0);
			set @TotalNumberOfUsers = isnull(@TotalNumberOfUsers_Packages,0) + isnull(@FreeUsers,0);
		 end

		 if (@TotalNumberOfUsers != 0)
		  begin
			 set @AveragePrice = ROUND(@TotalPaymentamount/@TotalNumberOfUsers, 3);
		  end

		update TenantManagements set TotalNumberOfUsers=@TotalNumberOfUsers, TotalFreeUsers = @TotalFreeUsers, AveragePrice = @AveragePrice, TotalPaymentamount =@TotalPaymentamount where Id=@TenantId

	FETCH NEXT FROM TenantManagementsCursor INTO @TenantId, @IsMultiPackage,@MainAdditionalPackageApplied,@NumberOfUsers, @FreeUsers, @TotalPrice
	END

CLOSE TenantManagementsCursor	
DEALLOCATE TenantManagementsCursor	


