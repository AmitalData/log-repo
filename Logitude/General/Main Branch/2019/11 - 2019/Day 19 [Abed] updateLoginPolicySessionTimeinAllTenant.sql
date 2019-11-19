
declare @TenantLoginPoliy as int
declare @Tenant as int

	 DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	 OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	 WHILE @@FETCH_STATUS = 0
	BEGIN

	set @TenantLoginPoliy = (select Tenant from TenantLoginPolicies where Tenant = @Tenant)
	if(@TenantLoginPoliy is  null)
	begin

	insert into TenantLoginPolicies (Tenant,LoginPolicyCode,SessionTimeout,IsEnabledForSpecificUsers,ExcludeInternalIPs) values (@Tenant,'NOREST' ,8,0,0 )
	end

	FETCH NEXT FROM TenantCursor     INTO @Tenant
	END
	 CLOSE TenantCursor
	 DEALLOCATE TenantCursor


update TenantLoginPolicies set SessionTimeout = 8
