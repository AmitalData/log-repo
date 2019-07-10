
	declare @Id as varchar(15)
	declare @Subject as varchar(50)
	declare @AWBNumber as varchar(50)
	declare @EntityId as varchar(15)
	declare @ObjectTableId as varchar(15)
	declare @ObjectTableName as varchar(50)
	declare @EntityPrefix as varchar(50)
	declare @EntityMaster as varchar(50)

	declare @ShipmentObjectTableId as varchar(15)
	declare @BookingObjectTableId as varchar(15)
	declare @MasterObjectTableId as varchar(15)
	set @ShipmentObjectTableId = (select Id from ObjectTables where Name = 'Shipment' and Tenant = 0)
	set @BookingObjectTableId = (select Id from ObjectTables where Name = 'Booking' and Tenant = 0)
	set @MasterObjectTableId = (select Id from ObjectTables where Name = 'Master' and Tenant = 0)

	if (@ShipmentObjectTableId is null OR @BookingObjectTableId is null OR @MasterObjectTableId is null)
	print 'Missing Object Table'

	else
	begin
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, EntityId, ObjectTableId, AWBNumber, [Subject]
		FROM CommunicationLogs
		WHERE EntityId is not null
		AND ObjectTableId in (@ShipmentObjectTableId, @BookingObjectTableId, @MasterObjectTableId)
		and 
		(
		(InOut = 'i' and Subject in ('FNA', 'FMA', 'FSA', 'FSU', 'FFA'))
		OR
		(InOut = 'o' and Subject in ('FWB', 'FHL', 'FFR', 'Cargonaut FWB', 'Cargonaut FHL', 'DEXX FWB', 'DEXX FHL'))
		)
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @EntityId, @ObjectTableId, @AWBNumber, @Subject
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @ObjectTableName = (select [Name] from ObjectTables where Id = @ObjectTableId)

			if (@ObjectTableName = 'Booking')		
			select @EntityPrefix = AirlinePrefix, @EntityMaster = Master from Bookings where Id = @EntityId		

			else if (@ObjectTableName = 'Shipment' OR @ObjectTableName = 'Master')
			select @EntityPrefix = AirlinePrefix, @EntityMaster = Master from ShipmentMasterDatas where Id = @EntityId


			if (@EntityPrefix is not null)
				begin
					if(@AWBNumber = @EntityMaster)
					--update CommunicationLogs set AWBNumber = (@EntityPrefix + '-' + @EntityMaster) where Id = @Id

					print '(' + @Subject + ':' + @Id +'):(' + @AWBNumber + '):(' + @EntityPrefix + '-' + @EntityMaster + ')'

					--else
					--print 'huh shipment (' + @Subject + '):(' + @AWBNumber + '):(' + @EntityPrefix + '-' + @EntityMaster + ')'
				end

				
		FETCH NEXT FROM DataCursor INTO @Id, @EntityId, @ObjectTableId, @AWBNumber,@Subject
		END
		CLOSE DataCursor
		DEALLOCATE DataCursor
	end

