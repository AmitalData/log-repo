
--declare @Tenant as int
--declare @EntityId as varchar(15)
--declare @HouseNumber as nvarchar(20)
--declare @MasterNumber as nvarchar(30)
--declare @MainEntityId as varchar(15)
--declare @MasterShipmentDataId as varchar(15)
--declare @TransportModeId as varchar(1)
--declare @CarrierId as varchar(15)
--declare @Prefix as char(3)

--BEGIN
--		DECLARE DataCursor CURSOR READ_ONLY
--		FOR
--		SELECT Id, Tenant, MainEntityId, HouseNumber, MasterNumber
--		FROM APInvoices
--		where MainEntityId is not null
--		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MainEntityId, @HouseNumber, @MasterNumber
--		WHILE @@FETCH_STATUS = 0
--			BEGIN

--			set @HouseNumber = null
--			set @MasterNumber = null
--			set @CarrierId = null
--			set @Prefix = null
--			set @TransportModeId = null
--			set @MasterShipmentDataId = null

--			if exists (select * from Shipments where Tenant = @Tenant AND Id = @MainEntityId)
--			BEGIN

--				select
--				@HouseNumber = House,
--				@TransportModeId = TransportModeId,
--				@MasterShipmentDataId = MasterShipmentDataId
--				from Shipments
--				where Tenant = @Tenant AND Id = @MainEntityId

--				if (@MasterShipmentDataId is not null)
--				begin
--					if exists (select * from ShipmentMasterDatas where Tenant = @Tenant AND Id = @MasterShipmentDataId)
--					begin

--						select
--						@MasterNumber = Master,
--						@CarrierId = MainCarriageCarrierId
--						from ShipmentMasterDatas
--						where Tenant = @Tenant AND Id = @MasterShipmentDataId
						
--						if (@TransportModeId = 'A' AND @CarrierId is not null)
--						begin
--							set @Prefix = (select Prefix from Airlines where Tenant = @Tenant AND Id = @CarrierId)
--						end
--					end
--				end

--			END
							
--			FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant,@MainEntityId, @HouseNumber, @MasterNumber	
--			END
--		CLOSE DataCursor
--		DEALLOCATE DataCursor
--END

