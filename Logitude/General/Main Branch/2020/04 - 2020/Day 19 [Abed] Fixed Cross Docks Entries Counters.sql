
   declare @Tenant as int
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	where Id = ANY (SELECT Tenant FROM WarehouseEntries);
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN


    declare @LastNumber as int = 1000;
	set @LastNumber = 1000;
    declare @WarehouseEntryId as varchar(15)


	DECLARE WarehouseEntryCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From WarehouseEntries
	where tenant = @Tenant order by CreateDate
	OPEN WarehouseEntryCursor FETCH NEXT FROM WarehouseEntryCursor INTO @WarehouseEntryId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	update WarehouseEntries set EntryNumber = @LastNumber where id =@WarehouseEntryId;
    set @LastNumber +=1;

	FETCH NEXT FROM WarehouseEntryCursor INTO  @WarehouseEntryId
		End
	CLOSE WarehouseEntryCursor
	DEALLOCATE WarehouseEntryCursor

  if(@LastNumber!=1000)begin set @LastNumber  = @LastNumber -1 end

  update CounterStats set LastValue = @LastNumber where CounterId  = (select id from Counters where Code = 'WAEC' and tenant =@Tenant )

	FETCH NEXT FROM TenantCursor INTO  @Tenant
		End
	CLOSE TenantCursor
	DEALLOCATE TenantCursor
