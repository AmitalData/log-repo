begin
declare @Id as varchar(15)
declare @Tenant as int
declare @CustomerId as varchar(15)
declare @WarehouseId as varchar(15)
declare @MasterNumber as varchar(30)
declare @HouseNumber as varchar(30)
declare @ReleaseNumber as varchar(15)
declare @StatusCode as varchar(100)
declare @ReleaseBy as varchar(15)
declare @SpecialInstruction as varchar(250)
declare @Notes as varchar(1000)
declare @SearchFields as varchar(1000)
declare @CustomerRef1 as varchar(100)
declare @CustomerRef2 as varchar(100)
	DECLARE WarehouseReleasesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,CustomerId,WarehouseId,MasterNumber,HouseNumber,ReleaseNumber,StatusCode,Tenant,ReleaseBy, SpecialInstruction, Notes,CustomerRef1,CustomerRef2
	From WarehouseReleases

	OPEN WarehouseReleasesCursor FETCH NEXT FROM WarehouseReleasesCursor INTO @Id,@CustomerId,@WarehouseId,@MasterNumber,@HouseNumber,@ReleaseNumber,@StatusCode, @Tenant,@ReleaseBy,@SpecialInstruction,@Notes,@CustomerRef1,@CustomerRef1
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @SearchFields = @ReleaseNumber;

	if(@Notes is not null)
	BEGIN
	set @SearchFields += (',' + @Notes);
	end

	if(@SpecialInstruction is not null)
	BEGIN
	set @SearchFields += (',' + @SpecialInstruction);
	end

	if(@ReleaseBy is not null)
	BEGIN
	set @SearchFields += (',' + @ReleaseBy);
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
	set @SearchFields += ( ',' + (select Name from WarehouseReleaseStatuses where Code = @StatusCode))
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

	update WarehouseReleases set SearchFields = @SearchFields where Id = @Id;

	FETCH NEXT FROM WarehouseReleasesCursor INTO @Id,@CustomerId,@WarehouseId,@MasterNumber,@HouseNumber,@ReleaseNumber,@StatusCode, @Tenant,@ReleaseBy,@SpecialInstruction,@Notes,@CustomerRef1,@CustomerRef1
	END
	CLOSE WarehouseReleasesCursor
	DEALLOCATE WarehouseReleasesCursor
END

