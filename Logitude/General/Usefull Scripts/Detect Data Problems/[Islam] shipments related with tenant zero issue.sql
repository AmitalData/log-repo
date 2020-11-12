



	-- SELECT count(*)
	--From Shipments where (DepartmentId in (select id from Departments  where Tenant = 0)or BranchId in (select id from Branches  where Tenant = 0)
	--or CreatedByUserId in (select id from users where Tenant = 0)  or UpdatedByUserId in (select id from users where Tenant = 0)
	--or SalesmanUserId in (select id from users where Tenant = 0)
	--or AccountManagerUserId in (select id from users where Tenant = 0)) and Tenant <> 0




	

	select a.id as addressId,a.address1, a.tenant as addressTenant,a.AddressTypeId,c.id as cardid,c.englishname,c.tenant as cardtenant,t.company 
	from addresses a 
	inner join cards c on c.id = a.cardid
	inner join tenants t on t.id = a.tenant
	where a.tenant <> c.tenant

	update Addresses set CardId = null where cardid in (
	select c.id
	from addresses a 
	inner join cards c on c.id = a.cardid
	inner join tenants t on t.id = a.tenant
	where a.tenant <> c.tenant
	)

	select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as cardId,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.consigneeid
	where sh.tenant <> c.tenant


	select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as cardId,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.customerid
	where sh.tenant <> c.tenant

	update shipments set CustomerId = (select top 1 id from Customers where shipments.Tenant = Tenant) 
	where id in (
	select sh.id
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.customerid
	where sh.tenant <> c.tenant
	)



    select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as cardId,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.shipperid
	where sh.tenant <> c.tenant

   update shipments set shipperid = (select top 1 id from Customers where shipments.Tenant = Tenant) 
	where id in (
	select sh.id
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.shipperid
	where sh.tenant <> c.tenant
	)


	select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as cardId,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join cards c on c.id = sh.agentid
	inner join tenants t on t.id = sh.tenant
	where sh.tenant <> c.tenant


	   update shipments set agentid = (select top 1 id from Agents where shipments.Tenant = Tenant) 
	where id in (
	select sh.id
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join cards c on c.id = sh.agentid
	where sh.tenant <> c.tenant
	)

	select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as departmentID,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join Departments c on c.id = sh.departmentid
	inner join tenants t on t.id = sh.tenant
	where sh.tenant <> c.tenant

	   update shipments set DepartmentId = (select top 1 id from Departments where shipments.Tenant = Tenant) 
	where id in (
	select sh.id
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join Departments c on c.id = sh.DepartmentId
	where sh.tenant <> c.tenant
	)


    select sh.id as sipmentId,sh.tenant as shipmentTenant,t.company,c.id as brancId,c.englishname,c.tenant as cardTenant 
	from shipments sh
	inner join Branches c on c.id = sh.branchid
	inner join tenants t on t.id = sh.tenant
	where sh.tenant <> c.tenant

	   update shipments set BranchId = (select top 1 id from Branches where shipments.Tenant = Tenant) 
	where id in (
	select sh.id
	from shipments sh
	inner join tenants t on t.id = sh.tenant
	inner join Branches c on c.id = sh.BranchId
	where sh.tenant <> c.tenant
	)
	






-- select count(*) as Zero_Deprtments from Shipments where (DepartmentId in (select id from Departments where Tenant = 0)
--or BranchId in (select id from Branches where Tenant = 0) or (UpdatedByUserId in (select id from users where Tenant = 0) )or (CreatedByUserId in (select id from users where Tenant = 0) )) and Tenant <> 0

BEGIN;
declare @Id as varchar(15)
declare @Tenant as int

declare @DepartmentId as varchar(15)
declare @BranchId as varchar(15)
declare @CreatedByUserId as varchar(15)
declare @UpdatedByUserId as varchar(15)
declare @SalesmanUserId as varchar(15)
declare @AccountManagerUserId as varchar(15)

declare @ZeroDepartmentId as varchar(15)
declare @ZeroBranchId as varchar(15)
declare @ZeroContactemail varchar(100)
declare @ZeroContactId as varchar(15)
set @ZeroContactemail ='system@tenant0.com'
set @ZeroContactId= (select id from Contacts where email =@ZeroContactemail)
--set @ZeroDepartmentId= (select departmentid from Users where id =@ZeroContactId)
--set @ZeroBranchId= (select branchid from Users where id =@ZeroContactId)
--drop table #@ZeroDepartments
--drop table #@ZeroBranches
--select id into #@ZeroDepartments from  Departments where tenant = 0
--select id into #@ZeroBranches from  Departments where tenant = 0
--SELECT count(*)
--	From Shipments where (DepartmentId in (select id from #@ZeroDepartments)or BranchId in (select id from #@ZeroBranches)
--	or CreatedByUserId in (select id from users where Tenant = 0)  or UpdatedByUserId in (select id from users where Tenant = 0)) and Tenant <> 0
	 



 
	 
print @ZeroContactemail +' ' + @ZeroContactId + ' ' +@ZeroDepartmentId + ' '+  @ZeroBranchId


	DECLARE shipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,BranchId,DepartmentId,CreatedByUserId,UpdatedByUserId,SalesmanUserId,AccountManagerUserId
	From Shipments where (DepartmentId in (select id from Departments  where Tenant = 0)or BranchId in (select id from Branches  where Tenant = 0)
	or CreatedByUserId in (select id from users where Tenant = 0)  or UpdatedByUserId in (select id from users where Tenant = 0)
	or SalesmanUserId in (select id from users where Tenant = 0)
	or AccountManagerUserId in (select id from users where Tenant = 0)) and Tenant <> 0
	 
	OPEN shipmentsCursor FETCH NEXT FROM shipmentsCursor INTO @Id,@Tenant,@BranchId,@DepartmentId,@CreatedByUserId,@UpdatedByUserId,@SalesmanUserId,@AccountManagerUserId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	declare @email varchar(100)
	declare @tenantString as varchar(50)
	set @tenantString=CONVERT(varchar(50), @Tenant)
	set @email ='system@tenant'+@tenantString+'.com'
	declare @TenantContactId as varchar(15)

	declare @IsZeroDepartment as bit
	set @IsZeroDepartment = case when exists (select id from Departments where id = @DepartmentId and tenant = 0) then 1 else 0 end
	if @IsZeroDepartment = 1
	begin
		declare @TenantDepartmentId as varchar(15)
		set @TenantDepartmentId = (select top 1 id from departments where tenant = @Tenant)
		update shipments set departmentid = @TenantDepartmentId where id = @Id and tenant = @Tenant
		
		print 'zero department ' + @DepartmentId
	end
	
	declare @IsZeroBranch as bit
	set @IsZeroBranch = case when exists (select id from Branches where id = @BranchId and tenant = 0) then 1 else 0 end
	if @IsZeroBranch = 1
	begin
		declare @TenantBranchId as varchar(15)
		set @TenantBranchId = (select top 1 id from Branches where tenant = @Tenant)
		update shipments set branchid = @TenantBranchId where id = @Id and tenant = @Tenant
		
		print 'zero branch ' + @BranchId
	end
	
	declare @IsZeroCreatedByUserId as bit
	set @IsZeroCreatedByUserId = case when exists (select id from Users where id = @CreatedByUserId and tenant = 0) then 1 else 0 end
	if @IsZeroCreatedByUserId = 1
	begin
		set @TenantContactId= (select id from Contacts where email =@email)
		update shipments set createdbyuserid = @TenantContactId where id = @Id and tenant = @Tenant
		
		print 'zero created by ' + @CreatedByUserId
	end

	declare @IsZeroUpdatedByUserId as bit
	set @IsZeroUpdatedByUserId = case when exists (select id from Users where id = @UpdatedByUserId and tenant = 0) then 1 else 0 end
	if @IsZeroUpdatedByUserId = 1
	begin
		set @TenantContactId= (select id from Contacts where email =@email)
		update shipments set updatedbyuserid = @TenantContactId where id = @Id and tenant = @Tenant
		
		print 'zero updated by ' + @UpdatedByUserId
	end
	
	declare @IsZeroSalesmanByUserId as bit
	set @IsZeroSalesmanByUserId = case when exists (select id from Users where id = @SalesmanUserId and tenant = 0) then 1 else 0 end
	if @IsZeroSalesmanByUserId = 1
	begin
		set @TenantContactId= (select id from Contacts where email =@email)
		update shipments set SalesmanUserId = @TenantContactId where id = @Id and tenant = @Tenant
		
		print 'zero salesman by ' + @SalesmanUserId
	end
	
	declare @IsZeroAccountManagerUserId as bit
	set @IsZeroAccountManagerUserId = case when exists (select id from Users where id = @AccountManagerUserId and tenant = 0) then 1 else 0 end
	if @IsZeroAccountManagerUserId = 1
	begin
		set @TenantContactId= (select id from Contacts where email =@email)
		update shipments set AccountManagerUserId = @TenantContactId where id = @Id and tenant = @Tenant
		
		print 'zero account manager ' + @AccountManagerUserId
	end

	FETCH NEXT FROM shipmentsCursor INTO  @Id,@Tenant,@BranchId,@DepartmentId,@CreatedByUserId,@UpdatedByUserId,@SalesmanUserId,@AccountManagerUserId
	END
	CLOSE shipmentsCursor
	DEALLOCATE shipmentsCursor
END
