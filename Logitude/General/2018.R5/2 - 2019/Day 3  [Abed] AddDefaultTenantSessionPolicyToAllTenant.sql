
    declare @Id as int
	declare @Tenant as int
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants

	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Id 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Tenant = (select Tenant from TenantLoginPolicies where Tenant =@Id)
	if(@Tenant is null)
	begin 

	insert into TenantLoginPolicies (Tenant , LoginPolicyCode,IsEnabledForSpecificUsers,TwoFactorInternalIPs , KeepUserLoggedIn  ,ExcludeInternalIPs,AllowedIPs, SessionTimeout) values(@Id, 'NOREST' ,0 ,'' , 0,0, '',999)

	end

	FETCH NEXT FROM TenantsCursor  INTO @Id 
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	

	


