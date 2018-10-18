---------------------------------------------------------------
-- DO NOT RUN ON PRODUCTION DB
---------------------------------------------------------------

	DECLARE @CurrentTenant AS INT
	DECLARE @CurrentShipmentId AS varchar(15)
	DECLARE @MasterShipmentDataId AS varchar(15)

	DECLARE @MainCarriageCarrierId AS varchar(15)
	DECLARE @Transshipment1CarrierId AS varchar(15)
	DECLARE @Transshipment2CarrierId AS varchar(15)
	DECLARE @Transshipment3CarrierId AS varchar(15)
	DECLARE @MainCarriageCarrierPrefix AS char(2)
	DECLARE @Transshipment1CarrierPrefix AS char(2)
	DECLARE @Transshipment2CarrierPrefix AS char(2)
	DECLARE @Transshipment3CarrierPrefix AS char(2)

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,MasterShipmentDataId
	FROM Shipments
	WHERE MasterShipmentDataId is not null
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @CurrentShipmentId,@CurrentTenant, @MasterShipmentDataId
	WHILE @@FETCH_STATUS = 0 
	BEGIN

	if(@MasterShipmentDataId is not null)
		BEGIN

		set @MainCarriageCarrierPrefix = NULL
		set @Transshipment1CarrierPrefix = NULL
		set @Transshipment2CarrierPrefix = NULL
		set @Transshipment3CarrierPrefix = NULL
		set @MainCarriageCarrierId = (select MainCarriageCarrierId from ShipmentMasterDatas where Id = @MasterShipmentDataId)
		set @Transshipment1CarrierId = (select Transshipment1CarrierId from ShipmentMasterDatas where Id = @MasterShipmentDataId)
		set @Transshipment2CarrierId = (select Transshipment2CarrierId from ShipmentMasterDatas where Id = @MasterShipmentDataId)
		set @Transshipment3CarrierId = (select Transshipment3CarrierId from ShipmentMasterDatas where Id = @MasterShipmentDataId)

		if(@MainCarriageCarrierId is not null)
		set @MainCarriageCarrierPrefix = (Select Code from Cards where Id = @MainCarriageCarrierId)

		if(@Transshipment1CarrierId is not null)
		set @Transshipment1CarrierPrefix = (Select Code from Cards where Id = @Transshipment1CarrierId)

		if(@Transshipment2CarrierId is not null)
		set @Transshipment2CarrierPrefix = (Select Code from Cards where Id = @Transshipment2CarrierId)

		if(@Transshipment3CarrierId is not null)
		set @Transshipment3CarrierPrefix = (Select Code from Cards where Id = @Transshipment3CarrierId)

		Update Shipments 
		set
		MainCarriageCarrierPrefix = @MainCarriageCarrierPrefix,
		Transshipment1CarrierPrefix = @Transshipment1CarrierPrefix,
		Transshipment2CarrierPrefix = @Transshipment2CarrierPrefix,
		Transshipment3CarrierPrefix = @Transshipment3CarrierPrefix		
		where Id = @CurrentShipmentId AND Tenant = @CurrentTenant

		END

	FETCH NEXT FROM ShipmentsCursor INTO @CurrentShipmentId,@CurrentTenant, @MasterShipmentDataId
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
