--1104
--select MainCarriageIsFromStack, * from ShipmentMasterDatas where Master = '12541001'
--select MainCarriageIsFromStack, * from Bookings where Master = '12541454'



	declare @Tenant as int
	declare @StackId as varchar(15)
	declare @StackMaster as varchar(50)
	declare @StackAirlineId as varchar(15)
	declare @StackIsUsed as bit

	declare @ShipmentNumber as varchar(50)
	declare @BookingNumber as varchar(50)

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, Number, AirlineId, IsUsed
	FROM MAWBStacks
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @StackId, @Tenant, @StackMaster, @StackAirlineId, @StackIsUsed
	WHILE @@FETCH_STATUS = 0
	BEGIN

	if(@StackIsUsed = 0)
	begin
		
		set @ShipmentNumber = (select top 1 Shipments.ShipmentNumber from Shipments join ShipmentMasterDatas on Shipments.Id = ShipmentMasterDatas.Id
						  where Shipments.Tenant = @Tenant
						  and Shipments.IsCancelled = 0
						  and Shipments.TransportModeId = 'A'
						  and Shipments.DirectionId = 'E'
						  and (Shipments.ShipmentLevelCode = 'D' OR Shipments.ShipmentLevelCode = 'C')
						  and ShipmentMasterDatas.Master = @StackMaster
						  and ShipmentMasterDatas.MainCarriageIsFromStack = 1
						  and
							  (
								(ShipmentMasterDatas.InterlineId is not null and ShipmentMasterDatas.InterlineId = @StackAirlineId)
								OR
								(ShipmentMasterDatas.MainCarriageCarrierId is not null and ShipmentMasterDatas.MainCarriageCarrierId = @StackAirlineId)
							  )
						  )

		if (@ShipmentNumber is not null)
		begin
			print '(Tenant:' + convert(varchar,@Tenant) + ')' + '(Shipment:' + @ShipmentNumber + ')'
		end

		else
		begin
			set @BookingNumber = (select top 1 BookingNumber from Bookings
							 where Tenant = @Tenant
							 and IsCancelled = 0
							 and TransportModeCode = 'A'
							 and DirectionCode = 'E'
							 and Master = @StackMaster
							 and MainCarriageIsFromStack = 1
							 and
								 (
									(InterlineId is not null and InterlineId = @StackAirlineId)
									OR
									(MainCarriageCarrierId is not null and MainCarriageCarrierId = @StackAirlineId)
								 )
							 )

			if(@BookingNumber is not null)
			begin				
				print '(Tenant:' + convert(varchar,@Tenant) + ')' + '(Booking:' + @BookingNumber + ')'
			end
		end

	end

	FETCH NEXT FROM DataCursor INTO @StackId, @Tenant, @StackMaster, @StackAirlineId, @StackIsUsed
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


