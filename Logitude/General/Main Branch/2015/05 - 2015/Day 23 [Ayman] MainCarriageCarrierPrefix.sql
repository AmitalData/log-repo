
declare @Tenant as int
declare @EntityId as varchar(15)
declare @MainCarriageCarrierPrefix as varchar(2)
declare @Transshipment1CarrierPrefix as varchar(2)
declare @Transshipment2CarrierPrefix as varchar(2)
declare @Transshipment3CarrierPrefix as varchar(2)
declare @ShipmentLevelCode as varchar(1)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, MainCarriageCarrierPrefix, Transshipment1CarrierPrefix, Transshipment2CarrierPrefix, Transshipment3CarrierPrefix, ShipmentLevelCode
	FROM Shipments
	where MasterShipmentDataId is not null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MainCarriageCarrierPrefix, @Transshipment1CarrierPrefix, @Transshipment2CarrierPrefix, @Transshipment3CarrierPrefix, @ShipmentLevelCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@ShipmentLevelCode = 'H')
		begin
			select
			@MainCarriageCarrierPrefix = MainCarriageCarrierPrefix,
			@Transshipment1CarrierPrefix = Transshipment1CarrierPrefix,
			@Transshipment2CarrierPrefix = Transshipment2CarrierPrefix,
			@Transshipment3CarrierPrefix = Transshipment3CarrierPrefix
			from Shipments
			where Tenant = @Tenant and Id = @EntityId
		end
		
		update ShipmentMasterDatas
		set
		MainCarriageCarrierPrefix = @MainCarriageCarrierPrefix,
		Transshipment1CarrierPrefix = @Transshipment1CarrierPrefix,
		Transshipment2CarrierPrefix = @Transshipment2CarrierPrefix,
		Transshipment3CarrierPrefix = @Transshipment3CarrierPrefix
		where Tenant = @Tenant and Id = @EntityId 

	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @MainCarriageCarrierPrefix, @Transshipment1CarrierPrefix, @Transshipment2CarrierPrefix, @Transshipment3CarrierPrefix, @ShipmentLevelCode
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END