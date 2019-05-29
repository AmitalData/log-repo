
	declare @Id as varchar(15)
	declare @Subject as varchar(50)
	declare @AWBNumber as varchar(50)
	declare @EntityId as varchar(15)
	declare @ObjectTableId as varchar(15)
	declare @ObjectTableName as varchar(50)
	declare @EntityPrefix as varchar(50)
	declare @EntityMaster as varchar(50)

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT top 100 Id, EntityId, ObjectTableId, AWBNumber, [Subject]
	FROM CommunicationLogs
	WHERE EntityId is not null AND ObjectTableId is not null 
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

		else if (@ObjectTableName = 'Shipment')
		select @EntityPrefix = AirlinePrefix, @EntityMaster = Master from ShipmentMasterDatas where Id = @EntityId

			
		if (@EntityPrefix is not null)
			begin
				if(@AWBNumber = @EntityMaster)
				--update CommunicationLogs set AWBNumber = (@EntityPrefix + '-' + @EntityMaster) where Id = @Id

				print '(' + @Subject + '):(' + @AWBNumber + '):(' + @EntityPrefix + '-' + @EntityMaster + ')'

				--else
				--print 'huh shipment (' + @Subject + '):(' + @AWBNumber + '):(' + @EntityPrefix + '-' + @EntityMaster + ')'
			end

	FETCH NEXT FROM DataCursor INTO @Id, @EntityId, @ObjectTableId, @AWBNumber,@Subject
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor