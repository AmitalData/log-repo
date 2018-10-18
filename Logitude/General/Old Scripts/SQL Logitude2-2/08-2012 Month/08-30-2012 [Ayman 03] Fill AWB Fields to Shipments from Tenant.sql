
-- just first time
update shipments 
set  IssuingCarrierAgentId = null,IssuingCarrierAddressId=null

DECLARE @EntityTenant AS INT
DECLARE @EntityId AS varchar(15)
DECLARE @EntityFromPortId AS varchar(15)
DECLARE @EntityAWBPlace AS varchar(150)
DECLARE @EntityAWBSignature AS varchar(40)
DECLARE @EntityIssuingCarrierIATACode AS varchar(15)
DECLARE @EntityMasterShipmentDataId AS varchar(15)
DECLARE @EntityIssuingCarrierAgentId AS varchar(15)
DECLARE @EntityIssuingCarrierAddressId AS varchar(15)
DECLARE @ShipmentLevelCode AS varchar(1)
DECLARE @EntityShipperId AS varchar(15)
DECLARE @EntityShipperAddressId AS varchar(15)

DECLARE @TenantIATA AS varchar(15)
DECLARE @TenantAgentId AS varchar(15)
DECLARE @TenantAddressId AS varchar(15)
DECLARE @TenantSignature AS varchar(40)

BEGIN; -- Update Shipments	
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,AWBPlace,AWBSignature,IssuingCarrierIATACode,MasterShipmentDataId,FromPortId,IssuingCarrierAgentId,IssuingCarrierAddressId,ShipmentLevelCode,ShipperId,ShipperAddressId
	FROM Shipments
	Where TransportModeId = 'A' AND DirectionId = 'E'
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId,@EntityTenant,@EntityAWBPlace,@EntityAWBSignature,@EntityIssuingCarrierIATACode,@EntityMasterShipmentDataId,@EntityFromPortId,@EntityIssuingCarrierAgentId,@EntityIssuingCarrierAddressId,@ShipmentLevelCode,@EntityShipperId,@EntityShipperAddressId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @TenantIATA = (SELECT IATA From Tenants where Id = @EntityTenant)
		set @TenantAgentId = (SELECT AgentId From Tenants where Id = @EntityTenant)
		set @TenantAddressId = (SELECT AddressId From Tenants where Id = @EntityTenant)
		set @TenantSignature = (SELECT Signature From Tenants where Id = @EntityTenant)	
	
		if (@EntityIssuingCarrierIATACode is null)
		begin;
		update Shipments set IssuingCarrierIATACode = @TenantIATA where Id = @EntityId
		end

		if (@EntityAWBSignature is null)
		begin;
		update Shipments set AWBSignature = @TenantSignature where Id = @EntityId
		end

		if (@EntityIssuingCarrierAgentId is null)
		begin;
		 update Shipments set IssuingCarrierAgentId = @TenantAgentId where Id = @EntityId
		end

		if (@EntityIssuingCarrierAddressId is null)
		begin;
		 update Shipments set IssuingCarrierAddressId = @TenantAddressId where Id = @EntityId
		end

		if (@ShipmentLevelCode = 'C')
			begin;
				if (@EntityShipperId is null)
				begin;
				 update Shipments set ShipperId = @TenantAgentId where Id = @EntityId
				end

				if (@EntityShipperAddressId is null)
				begin;
				 update Shipments set ShipperAddressId = @TenantAddressId where Id = @EntityId
				end				
			end

		if (@EntityAWBPlace is null)
		begin;
			
			DECLARE @PortId AS varchar(15)
			DECLARE @PortName AS varchar(40)
			DECLARE @CountryId AS varchar(15)
			DECLARE @CountryName AS varchar(120)
			DECLARE @Place AS varchar(165)

			set @PortId = (SELECT MainCarriageFromPortId From ShipmentMasterDatas where Id = @EntityMasterShipmentDataId)

				if (@PortId is null)
				begin;
					set @PortId = @EntityFromPortId
				end

			set @PortName = (SELECT EnglishName From Ports where Id = @PortId)
			set @CountryId = (SELECT CountryId From Ports where Id = @PortId)
			set @CountryName = (SELECT EnglishName From Countries where Id = @CountryId)
			set @Place = @PortName + ',' + @CountryName

			update Shipments set AWBPlace = @Place where Id = @EntityId
		end

	FETCH NEXT FROM ShipmentsCursor INTO @EntityId,@EntityTenant,@EntityAWBPlace,@EntityAWBSignature,@EntityIssuingCarrierIATACode,@EntityMasterShipmentDataId,@EntityFromPortId,@EntityIssuingCarrierAgentId,@EntityIssuingCarrierAddressId,@ShipmentLevelCode,@EntityShipperId,@EntityShipperAddressId
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END