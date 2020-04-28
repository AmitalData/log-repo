
   declare @Tenant as int
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	where Id = ANY (SELECT Tenant FROM WarehouseReleases);
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN


    declare @LastNumber as int = 1000;
	set @LastNumber = 1000;
    declare @WarehouseReleaseId as varchar(15)


	DECLARE WarehouseReleaseCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From WarehouseReleases
	where tenant = @Tenant order by CreateDate
	OPEN WarehouseReleaseCursor FETCH NEXT FROM WarehouseReleaseCursor INTO @WarehouseReleaseId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	update WarehouseReleases set ReleaseNumber = @LastNumber where id =@WarehouseReleaseId;
    set @LastNumber +=1;

	FETCH NEXT FROM WarehouseReleaseCursor INTO  @WarehouseReleaseId
		End
	CLOSE WarehouseReleaseCursor
	DEALLOCATE WarehouseReleaseCursor

  if(@LastNumber!=1000)  begin set @LastNumber  = @LastNumber -1 end

  update CounterStats set LastValue = @LastNumber where CounterId  = (select id from Counters where Code = 'WARC' and tenant =@Tenant )
  print @LastNumber
	FETCH NEXT FROM TenantCursor INTO  @Tenant
		End
	CLOSE TenantCursor
	DEALLOCATE TenantCursor
