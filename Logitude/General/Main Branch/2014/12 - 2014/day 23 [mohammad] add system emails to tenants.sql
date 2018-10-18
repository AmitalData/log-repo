
BEGIN;
declare @Id as int
declare @ContactId as varchar(15)
declare @departmentId as varchar(15)
declare @branchId as varchar(15)
declare @RoleId as varchar(15)

	DECLARE tenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	OPEN tenantsCursor FETCH NEXT FROM tenantsCursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN

	declare @email varchar(100)
	declare @tenantString as varchar(50)
	set @tenantString=CONVERT(varchar(50), @Id)
	set @email ='system@tenant'+@tenantString+'.com'
	
	set @ContactId= (select id from Contacts where email =@email)
	
	if(@ContactId is null)
	begin

	set @RoleId =(select top 1 Id from Roles where Code='ADMN')
	set @branchId=(select top 1 Id from Branches where Tenant=@Id)
	set @departmentId=(select top 1 Id from Departments where Tenant=@Id)
	declare @createdDate as datetime
    set @createdDate =GETDATE()
    EXECUTE usp_GetNextTableIdValue @ContactId OUTPUT,'Contact'
	insert into Contacts (Id,Tenant,Email,EnglishName,UserType,InActive,DisplayGettingStarted,DontShowLocalLabels,SearchFields) values (@ContactId,@Id,@email,'System','S',0,0,1,@email)
	insert into Users (Id,Tenant,DepartmentId,BranchId,CreateDate,BusinessUnitId,IsBranchRestricted,IsDistributor,IsFreelancer,IsProductRestricted,IsSalesman,LicencedUser,SearchFields) values (@ContactId,@Id,@departmentId,@branchId,@createdDate,@Id,0,0,0,0,0,0,@email)
	declare @contactTenantId varchar(15)
	EXECUTE usp_GetNextTableIdValue @contactTenantId OUTPUT,'ContactTenant'

	insert into ContactTenants (Id,ContactId,TenantId) values (@contactTenantId,@ContactId,@Id)
	declare @contactTenantRoleId varchar(15)
	EXECUTE usp_GetNextTableIdValue @contactTenantRoleId OUTPUT,'ContactTenantRole'

	insert into ContactTenantRoleSet (Id,ContactTenantId,RoleId,Tenant) values(@contactTenantRoleId,@contactTenantId,@RoleId,@Id)

	end

	FETCH NEXT FROM tenantsCursor INTO @Id
	END
	CLOSE tenantsCursor
	DEALLOCATE tenantsCursor
END
