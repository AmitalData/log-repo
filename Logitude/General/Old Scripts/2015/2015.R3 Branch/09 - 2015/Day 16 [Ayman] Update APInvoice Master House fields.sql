
declare @Tenant as int
declare @EntityId as varchar(15)
declare @MainEntityId as varchar(15)

declare @HouseNumber as nvarchar(20)
declare @MasterNumber as nvarchar(30)
declare @MasterShipmentDataId as varchar(15)
declare @TransportModeId as varchar(1)
declare @AirlinePrefix as char(3)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, MainEntityId
		FROM APInvoices
		where MainEntityId is not null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MainEntityId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @HouseNumber = null
			set @MasterNumber = null

			if exists (select * from Shipments where Tenant = @Tenant AND Id = @MainEntityId)
			BEGIN
			
				set @AirlinePrefix = null
				set @TransportModeId = null
				set @MasterShipmentDataId = null

				select
				@HouseNumber = House,
				@TransportModeId = TransportModeId,
				@MasterShipmentDataId = MasterShipmentDataId
				from Shipments
				where Tenant = @Tenant AND Id = @MainEntityId

				if (@MasterShipmentDataId is not null)
				begin
					if exists (select * from ShipmentMasterDatas where Tenant = @Tenant AND Id = @MasterShipmentDataId)
					begin

						select
						@MasterNumber = Master,
						@AirlinePrefix = AirlinePrefix
						from ShipmentMasterDatas
						where Tenant = @Tenant AND Id = @MasterShipmentDataId
						
						if (@TransportModeId = 'A' AND @MasterNumber is not null AND @MasterNumber != '')
						begin
							set @MasterNumber = @AirlinePrefix + '-' + @MasterNumber
						end
					end
				end
			END
				
			update APInvoices set HouseNumber = @HouseNumber, MasterNumber = @MasterNumber where Id = @EntityId
						
			FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MainEntityId
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

