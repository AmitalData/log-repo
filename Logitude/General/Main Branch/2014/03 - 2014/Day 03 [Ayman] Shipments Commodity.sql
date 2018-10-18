
declare @NewId as varchar(15)
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @ChargeRate as float
declare @ChargeAmount as float
declare @ChargeableWeight as float
declare @RateClassCode as varchar(3)
declare @CommodityNumber as varchar(7)
declare @DescriptionOfGoods as varchar(215)
declare @Volume as float
declare @VolumetricWeight as float
declare @GrossWeight as float
declare @NumberOfPackages as int

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant, AWBChargeRate, AWBChargeAmount, ChargeableWeight, RateClassCode, AWBCommodityItemNumber, DescriptionOfGoods, Volume, VolumetricWeight,GrossWeight, NumberOfPackages
	FROM Shipments
	where TransportModeId = 'A'
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ChargeRate, @ChargeAmount, @ChargeableWeight, @RateClassCode, @CommodityNumber, @DescriptionOfGoods, @Volume, @VolumetricWeight, @GrossWeight, @NumberOfPackages
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if not exists (select Id from ShipmentCommodities where Tenant = @Tenant and ShipmentId = @ShipmentId)
		begin
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'ShipmentCommodity'
			insert into ShipmentCommodities(Id, ShipmentId, Tenant, ChargeRate, ChargeAmount, ChargeableWeight, RateClassCode, CommodityNumber, DescriptionOfGoods,Volume,VolumetricWeight,GrossWeight,NumberOfPackages)
			values(@NewId,@ShipmentId,@Tenant,@ChargeRate,@ChargeAmount,@ChargeableWeight,@RateClassCode,@CommodityNumber,@DescriptionOfGoods,@Volume,@VolumetricWeight,@GrossWeight,@NumberOfPackages)
		end

	FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ChargeRate, @ChargeAmount, @ChargeableWeight, @RateClassCode, @CommodityNumber, @DescriptionOfGoods, @Volume, @VolumetricWeight, @GrossWeight, @NumberOfPackages
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor



	-- Step(2):
	--	if the Cursor executed without errors
	--	then uncomment this and execute it
	--	after done please comment this back
	------------------------------------------------

	alter table shipments drop FK_ShipmentRateClass
	go

	DROP INDEX Shipments.IX_FK_ShipmentRateClass
	go

	alter table Shipments drop column RateClassCode
	go

	alter table Shipments drop column AWBChargeRate
	go

	alter table Shipments drop column AWBChargeAmount
	go

	alter table Shipments drop column AWBCommodityItemNumber
	go