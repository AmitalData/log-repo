begin
declare @Id as varchar(15)
declare @Tenant as int
declare @CustomerId as varchar(15)
declare @WarehouseId as varchar(15)
declare @MasterNumber as varchar(30)
declare @HouseNumber as varchar(30)
declare @EntryNumber as varchar(15)
declare @StatusCode as varchar(100)
declare @ReceivedBy as varchar(15)
declare @SpecialInstruction as varchar(250)
declare @Notes as varchar(1000)
declare @SearchFields as varchar(1000)
declare @CustomerRef1 as varchar(100)
declare @CustomerRef2 as varchar(100)
declare @EntryReference as varchar(15)

	DECLARE WarehouseEntriesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,CustomerId,WarehouseId,MasterNumber,HouseNumber,EntryNumber,StatusCode,Tenant,ReceivedBy, SpecialInstruction, Notes,CustomerRef1,CustomerRef2,EntryReference
	From WarehouseEntries

	OPEN WarehouseEntriesCursor FETCH NEXT FROM WarehouseEntriesCursor INTO @Id,@CustomerId,@WarehouseId,@MasterNumber,@HouseNumber,@EntryNumber,@StatusCode, @Tenant,@ReceivedBy,@SpecialInstruction,@Notes,@CustomerRef1,@CustomerRef1,@EntryReference
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @SearchFields = @EntryNumber;

	if(@Notes is not null)
	BEGIN
	set @SearchFields += (',' + @Notes);
	end

	if(@SpecialInstruction is not null)
	BEGIN
	set @SearchFields += (',' + @SpecialInstruction);
	end

	if(@ReceivedBy is not null)
	BEGIN
	set @SearchFields += (',' + @ReceivedBy);
	end

	if(@MasterNumber is not null)
	BEGIN
	set @SearchFields += (',' + @MasterNumber);
	end

	if(@HouseNumber is not null)
	BEGIN
	set @SearchFields += (',' + @HouseNumber);
	end

	if(@StatusCode is not null)
	BEGIN
	set @SearchFields += ( ',' + (select Name from WarehouseEntryStatuses where Code = @StatusCode))
	end

	if(@CustomerId is not null)
	BEGIN
	set @SearchFields +=( ',' + (select EnglishName from Cards where Id = @CustomerId))
	end

	if(@WarehouseId is not null)
	BEGIN
	set @SearchFields +=( ',' + (select EnglishName from Cards where Id = @WarehouseId))
	end


	if(@CustomerRef1 is not null)
	BEGIN
	set @SearchFields += (',' + @CustomerRef1);
	end

	if(@CustomerRef2 is not null)
	BEGIN
	set @SearchFields += (',' + @CustomerRef2);
	end

	update WarehouseEntries set SearchFields = @SearchFields where Id = @Id;

	FETCH NEXT FROM WarehouseEntriesCursor INTO @Id,@CustomerId,@WarehouseId,@MasterNumber,@HouseNumber,@EntryNumber,@StatusCode, @Tenant,@ReceivedBy,@SpecialInstruction,@Notes,@CustomerRef1,@CustomerRef1,@EntryReference
	END
	CLOSE WarehouseEntriesCursor
	DEALLOCATE WarehouseEntriesCursor
END

