
IF OBJECT_ID('[dbo].[usp_UpdateReceivablesData]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateReceivablesData]
GO

Create PROCEDURE [dbo].[usp_UpdateReceivablesData]
(
	@ShipmentId_PARAM varchar(15),
	@IsInvoiceUpdated_PARAM  bit
)
AS

if exists (select * from Shipments where Id = @ShipmentId_PARAM and ShipmentLevelCode != 'D')
BEGIN

	declare @Tenant as int
	declare @MasterId as varchar(15)
	declare @ProrateReceivables as bit

	select @Tenant = Tenant, 
	@MasterId = MasterShipmentDataId
	from Shipments where Id = @ShipmentId_PARAM

	if (@MasterId is not null)
	BEGIN
		
		set @ProrateReceivables = (select ProrateReceivables from ShipmentMasterDatas where Tenant = @Tenant AND Id = @MasterId)
		if (@ProrateReceivables = 0)
		BEGIN
			declare @ShipmentHouseId as varchar(15)

			DECLARE Houses1Cursor CURSOR READ_ONLY
			FOR
			SELECT Id
			FROM Shipments
			WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
			OPEN Houses1Cursor FETCH NEXT FROM Houses1Cursor INTO @ShipmentHouseId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				delete from ShipmentReceivables
				where Tenant = @Tenant
				AND ShipmentId = @ShipmentHouseId
				AND ShipmentReceivableParentId is not null

			FETCH NEXT FROM Houses1Cursor INTO @ShipmentHouseId
			END
			CLOSE Houses1Cursor
			DEALLOCATE Houses1Cursor
		END

		else
		BEGIN
			-- Global Variables
			BEGIN

				-- 01
				declare @GRWT_Id as varchar(15)
				declare @CHWT_Id as varchar(15)
				declare @FIXD_Id as varchar(15)
				declare @VOLU_Id as varchar(15)
				declare @BTEU_Id as varchar(15)
				declare @GWTN_Id as varchar(15)
				declare @PRVL_Id as varchar(15)
				declare @PRFR_Id as varchar(15)	
				declare @QTY_Id as varchar(15)	
				declare @GWKG_Id as varchar(15)
				declare @CWKG_Id as varchar(15)
				declare @VCBM_Id as varchar(15)
				declare @SCGW_Id as varchar(15)
				set @GRWT_Id = (select Id from Measurements where Code = 'GRWT' AND Tenant = @Tenant)
				set @CHWT_Id = (select Id from Measurements where Code = 'CHWT' AND Tenant = @Tenant)
				set @FIXD_Id = (select Id from Measurements where Code = 'FIXD' AND Tenant = @Tenant)
				set @VOLU_Id = (select Id from Measurements where Code = 'VOLU' AND Tenant = @Tenant)
				set @BTEU_Id = (select Id from Measurements where Code = 'BTEU' AND Tenant = @Tenant)
				set @GWTN_Id = (select Id from Measurements where Code = 'GWTN' AND Tenant = @Tenant)
				set @PRVL_Id = (select Id from Measurements where Code = 'PRVL' AND Tenant = @Tenant)
				set @PRFR_Id = (select Id from Measurements where Code = 'PRFR' AND Tenant = @Tenant)
				set @QTY_Id = (select Id from Measurements where Code = 'QTY' AND Tenant = @Tenant)				
				set @GWKG_Id = (select Id from Measurements where Code = 'GWKG' AND Tenant = @Tenant)
				set @CWKG_Id = (select Id from Measurements where Code = 'CWKG' AND Tenant = @Tenant)
				set @VCBM_Id = (select Id from Measurements where Code = 'VCBM' AND Tenant = @Tenant)
				set @SCGW_Id = (select Id from Measurements where Code = 'SCGW' AND Tenant = @Tenant)

				-- 02
				declare @AllHousesCount as float
				declare @AllHousesTotalTEU as float
				declare @AllHousesTotalVolume as float
				declare @AllHousesTotalGrossWeight as float
				declare @AllHousesTotalVolumetrics as float
				declare @AllHousesTotalChargeables as float
				declare @AllHousesTotalGrossWeightPerTon as float
				declare @AllHousesTotalNumberOfPackages as float
				declare @AllHousesTotalNumberOfContainers as float
				declare @AllHousesTotalGrossWeightInKG as float
				declare @AllHousesTotalChargeableWeightInKG as float
				declare @AllHousesTotalVolumeInCBM as float
				declare @AllHousesGrossWeightPerStorageDays as float;

				if exists (select * from Shipments where ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId)
				begin
					select
					@AllHousesCount = count(*),
					@AllHousesTotalTEU = sum(isnull(TEU,0)),
					@AllHousesTotalVolume = sum(isnull(Volume,0)),
					@AllHousesTotalGrossWeight = sum(isnull(GrossWeight,0)),
					@AllHousesTotalVolumetrics = sum(isnull(VolumetricWeight,0)),
					@AllHousesTotalChargeables = sum(isnull(ChargeableWeight,0)),
					@AllHousesTotalGrossWeightPerTon = sum(isnull(GrossWeightPerTon,0)),
					@AllHousesTotalNumberOfPackages = sum(isnull(NumberOfPackages,0)),
					@AllHousesTotalNumberOfContainers = sum(isnull(NumberOfContainers,0)),
					@AllHousesTotalGrossWeightInKG = sum(isnull(GrossWeightInKG,0)),
					@AllHousesTotalChargeableWeightInKG = sum(isnull(ChargeableWeightInKG,0)),
					@AllHousesTotalVolumeInCBM = sum(isnull(VolumeInCBM,0)),
					@AllHousesGrossWeightPerStorageDays = sum(isnull(GrossWeightPerStorageDays,0))
					from Shipments
					where ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId 
				end
				else
				begin
					set @AllHousesTotalTEU = 0
					set @AllHousesTotalVolume = 0
					set @AllHousesTotalGrossWeight = 0
					set @AllHousesTotalVolumetrics = 0
					set @AllHousesTotalChargeables = 0
					set @AllHousesTotalGrossWeightPerTon = 0
					set @AllHousesTotalNumberOfPackages = 0
					set @AllHousesTotalNumberOfContainers = 0
					set @AllHousesTotalGrossWeightInKG = 0
					set @AllHousesTotalChargeableWeightInKG = 0
					set @AllHousesTotalVolumeInCBM = 0
					set @AllHousesGrossWeightPerStorageDays = 0
				end

				-- 03
				declare @MasterTypeId as varchar(5)
				declare @MasterTransportModeId as varchar(1)		
				select
				@MasterTypeId = ShipmentTypeId,
				@MasterTransportModeId = TransportModeId
				from Shipments where Tenant = @Tenant and Id = @MasterId
		
			END

			-- Master Receivable Variables
			BEGIN
				declare @MasterReceivableId as varchar(15)
				declare @MasterReceivableQuantity as float
				declare @MasterReceivableUnitPrice as float
				declare @MasterReceivableChargesTypeId as varchar(15)
				declare @MasterReceivableMeasurementId as varchar(15)
				declare @MasterReceivablePrepaidCollectId as varchar(1)
				declare @MasterReceivableDueTypeCode as varchar(2)
				declare @MasterReceivableIATACodeId as varchar(15)
				declare @MasterReceivableAWBPrint as int
				declare @MasterReceivableCurrencyId as varchar(15)
				declare @MasterReceivableRate as float
				declare @MasterReceivableProfitRate as float			
				declare @MasterReceivableLineStatusCode as varchar(4)
				declare @MasterReceivableCreatedByUserId as varchar(15)
				declare @MasterReceivableUpdatedByUserId as varchar(15)
				declare @MasterReceivableCreateDate as datetime
				declare @MasterReceivableUpdateDate as datetime
				declare @MasterReceivablePackageTypeId as varchar(15)
				declare @MasterReceivableAmount as float
				declare @MasterReceivableARInvoiceId as varchar(15)
				declare @MasterReceivableARInvoiceLineId as varchar(15)
				declare @MasterReceivableIsFixedPrice as bit
				declare @MasterReceivableIsExchangeRateFixed as bit
			END

			-- House Receivable Variables
			BEGIN
				declare @IsCreatingReceivable as bit
				declare @IsUpdatingReceivable as bit
				declare @NewId as varchar(15)
				declare @HouseId as varchar(15)
				declare @HouseTEU as float
				declare @HouseVolume as float
				declare @HouseGrossWeight as float
				declare @HouseChargeableWeight as float
				declare @HouseGrossWeightPerTon as float
				declare @HouseValueOfGoods as float
				declare @HouseFreightAmount as float
				declare @HouseGrossWeightInKG as float
				declare @HouseChargeableWeightInKG as float
				declare @HouseVolumeInCBM as float
				declare @HouseGrossWeightPerStorageDays as float
				declare @HouseReceivableId as varchar(15)
				declare @HouseReceivableMeasurementId as varchar(15)
				declare @HouseReceivableARInvoiceId as varchar(15)			
				declare @HouseNumberOfPackages as int
				declare @HouseNumberOfContainers as int
				declare @HouseTypeId as varchar(5)
				declare @HouseTransportModeId as varchar(1)

				declare @Ratio as float
				declare @Quantity as float
				declare @UnitPrice as float
				declare @Amount as float
				declare @AmountLocal as float
				declare @AmountInProfitCurrency as float
				declare @AmountRatio as float
			END

			-- Loop Master Receivables (1: Not PRFR)
			BEGIN
				DECLARE MasterReceivables1Cursor CURSOR READ_ONLY
				FOR
				SELECT Id, Quantity, UnitPrice, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentReceivableLineStatusCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, TotalAmount, ARInvoiceId, ARInvoiceLineId, IsFixedPrice, IsExchangeRateFixed, IATACodeId
				FROM ShipmentReceivables
				WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND MeasurementId != @PRFR_Id
				OPEN MasterReceivables1Cursor FETCH NEXT FROM MasterReceivables1Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
				WHILE @@FETCH_STATUS = 0
				BEGIN

					-- Loop Houses
					BEGIN
						DECLARE Houses2Cursor CURSOR READ_ONLY
						FOR
						SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods, NumberOfPackages, NumberOfContainers, TransportModeId, ShipmentTypeId, GrossWeightInKG, ChargeableWeightInKG, VolumeInCBM, GrossWeightPerStorageDays
						FROM Shipments
						WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
						OPEN Houses2Cursor FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId,@HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
						WHILE @@FETCH_STATUS = 0
						BEGIN

						set @IsCreatingReceivable = 0
						set @HouseReceivableMeasurementId = @MasterReceivableMeasurementId

						-- IsCreatingReceivable
						BEGIN

							if (@MasterReceivableMeasurementId = @FIXD_Id
								OR @MasterReceivableMeasurementId = @BTEU_Id
								OR @MasterReceivableMeasurementId = @VOLU_Id
								OR @MasterReceivableMeasurementId = @GRWT_Id
								OR @MasterReceivableMeasurementId = @CHWT_Id
								OR @MasterReceivableMeasurementId = @GWTN_Id
								OR @MasterReceivableMeasurementId = @PRVL_Id
								OR @MasterReceivableMeasurementId = @QTY_Id
								OR @MasterReceivableMeasurementId = @GWKG_Id
								OR @MasterReceivableMeasurementId = @CWKG_Id
								OR @MasterReceivableMeasurementId = @VCBM_Id
								OR @MasterReceivableMeasurementId = @SCGW_Id
								)
							BEGIN
								if not exists (select * from ShipmentReceivables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentReceivableParentId = @MasterReceivableId and ChargesTypeId = @MasterReceivableChargesTypeId)
								set @IsCreatingReceivable = 1
							END

							-- If Master Is FCL | FTL
							else if ((@MasterTransportModeId = 'O' AND @MasterTypeId = 'FCLD') OR (@MasterTransportModeId = 'I' AND @MasterTypeId = 'FTL'))
							BEGIN
								set @MasterReceivablePackageTypeId = (select Id from PackageTypes where MeasurementId = @MasterReceivableMeasurementId AND Tenant = @Tenant)
								if exists (select * from ShipmentPackages where Tenant = @Tenant AND ShipmentId = @HouseId AND PackageTypeId = @MasterReceivablePackageTypeId)
								begin
									if not exists (select Id from ShipmentReceivables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentReceivableParentId = @MasterReceivableId AND ChargesTypeId = @MasterReceivableChargesTypeId AND MeasurementId = @MasterReceivableMeasurementId)
									set @IsCreatingReceivable = 1
								end
							END

							else if ((@MasterTransportModeId = 'O' AND @MasterTypeId = 'MyGO') OR (@MasterTransportModeId = 'I' AND @MasterTypeId = 'MyGI'))
							BEGIN
								if not exists (select Id from ShipmentReceivables where Tenant = @Tenant AND ShipmentId = @HouseId AND ShipmentReceivableParentId = @MasterReceivableId AND ChargesTypeId = @MasterReceivableChargesTypeId AND MeasurementId = @CHWT_Id)
								set @IsCreatingReceivable = 1
								set @HouseReceivableMeasurementId = @CHWT_Id
							END
				
							if (@IsCreatingReceivable = 1)
							BEGIN

								SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
								BEGIN TRAN T1;
								EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'ShipmentReceivable'
								COMMIT TRAN T1; 

								INSERT INTO ShipmentReceivables
								(
								Id,
								Tenant,
								ShipmentId,
								ShipmentReceivableParentId,
								ShipmentReceivableLineStatusCode,
								ChargesTypeId,
								MeasurementId,
								PrepaidCollectId,
								DueTypeCode,
								AWBPrint,
								CurrencyId,
								Rate,
								ProfitCurrencyExchangeRate,
								CreateDate,
								UpdateDate,
								CreatedByUserId,
								UpdateByUserId,
								IsFromQuote,
								PayableLocal,
								IsFixedPrice,
								IsExchangeRateFixed,						
								ARInvoiceId,
								ARInvoiceLineId,
								IATACodeId
								)
								VALUES
								(
								@NewId,
								@Tenant,
								@HouseId,
								@MasterReceivableId,
								@MasterReceivableLineStatusCode,
								@MasterReceivableChargesTypeId,
								@HouseReceivableMeasurementId,
								@MasterReceivablePrepaidCollectId,
								@MasterReceivableDueTypeCode,
								@MasterReceivableAWBPrint,
								@MasterReceivableCurrencyId,
								@MasterReceivableRate,
								@MasterReceivableProfitRate,
								@MasterReceivableCreateDate,
								@MasterReceivableUpdateDate,
								@MasterReceivableCreatedByUserId,
								@MasterReceivableUpdatedByUserId,
								0,
								0,
								@MasterReceivableIsFixedPrice,
								@MasterReceivableIsExchangeRateFixed,
								@MasterReceivableARInvoiceId,
								@MasterReceivableARInvoiceLineId,
								@MasterReceivableIATACodeId
								)				
							END
						END

						-- Loop House Receivables / Amount Calculating & Updating
						BEGIN
							DECLARE HouseReceivables1Cursor CURSOR READ_ONLY
							FOR
							SELECT Id, ARInvoiceId
							FROM ShipmentReceivables
							WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentReceivableParentId = @MasterReceivableId
							OPEN HouseReceivables1Cursor FETCH NEXT FROM HouseReceivables1Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
							WHILE @@FETCH_STATUS = 0
							BEGIN

								set @IsUpdatingReceivable = 0

								if (@IsCreatingReceivable = 1)
								begin
									set @IsUpdatingReceivable = 1
								end

								else if (@IsInvoiceUpdated_PARAM = 1)
								begin
									set @IsUpdatingReceivable = 1
								end

								else
								begin									
									if (@HouseReceivableARInvoiceId is not null AND @MasterReceivableARInvoiceId is not null)
									begin
										set @IsUpdatingReceivable = 0
									end

									else
									begin
										set @IsUpdatingReceivable = 1
									end
								end

								--set @IsUpdatingReceivable = 1
								if (@IsUpdatingReceivable = 1)
								BEGIN
			
									-- Reset Variables
									BEGIN
										set @Ratio = 0
										set @Quantity = 0
										set @UnitPrice = 0
										set @Amount = 0
										set @AmountLocal = 0
										set @AmountInProfitCurrency = 0
									END
						
									-- Compute Ration, Quantity, UnitPrice
									BEGIN

										-- Fixed
										if (@MasterReceivableMeasurementId = @FIXD_Id)
										BEGIN
											set @Ratio = @MasterReceivableQuantity / @AllHousesCount
											set @Quantity = 1
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- By TEU
										else if (@MasterReceivableMeasurementId = @BTEU_Id)
										BEGIN
											if (@AllHousesTotalTEU <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalTEU
											end

											set @Quantity = @HouseTEU
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- Volume
										else if (@MasterReceivableMeasurementId = @VOLU_Id)
										BEGIN
											if (@AllHousesTotalVolume <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalVolume
											end

											set @Quantity = @HouseVolume
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- Gross Weight
										else if (@MasterReceivableMeasurementId = @GRWT_Id)
										BEGIN
											if (@AllHousesTotalGrossWeight <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeight
											end

											set @Quantity = @HouseGrossWeight
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- Chargeable Weight
										else if (@MasterReceivableMeasurementId = @CHWT_Id)
										BEGIN
											if (@AllHousesTotalChargeables <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeables
											end

											set @Quantity = @HouseChargeableWeight
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- Gross Weight Per Ton
										else if (@MasterReceivableMeasurementId = @GWTN_Id)
										BEGIN
											if (@AllHousesTotalGrossWeightPerTon <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeightPerTon
											end

											set @Quantity = @HouseGrossWeightPerTon
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- GrossWeightPerStorageDays
										else if (@MasterReceivableMeasurementId = @SCGW_Id)
										BEGIN
											if (@AllHousesGrossWeightPerStorageDays <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesGrossWeightPerStorageDays
											end

											set @Quantity = @HouseGrossWeightPerStorageDays
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- GWKG: Gross Weight in Kg
										else if (@MasterReceivableMeasurementId = @GWKG_Id)
										BEGIN
											if (@AllHousesTotalGrossWeightInKG <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalGrossWeightInKG
											end

											set @Quantity = @HouseGrossWeightInKG
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- CWKG: Chargeable Weight in Kg
										else if (@MasterReceivableMeasurementId = @CWKG_Id)
										BEGIN
											if (@AllHousesTotalChargeableWeightInKG <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeableWeightInKG
											end

											set @Quantity = @HouseChargeableWeightInKG
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- VCBM: Volume in CBM
										else if (@MasterReceivableMeasurementId = @VCBM_Id)
										BEGIN
											if (@AllHousesTotalVolumeInCBM <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalVolumeInCBM
											end

											set @Quantity = @HouseVolumeInCBM
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

										-- Percent of Value
										else if (@MasterReceivableMeasurementId = @PRVL_Id)
										BEGIN
											set @Quantity = @HouseValueOfGoods
											set @UnitPrice = @MasterReceivableUnitPrice
										END

										-- Quantity
										else if (@MasterReceivableMeasurementId = @QTY_Id)
										BEGIN
											if(@HouseTransportModeId = 'A' OR (@HouseTransportModeId = 'O' AND @HouseTypeId = 'LCLD') OR (@HouseTransportModeId = 'I' AND @HouseTypeId = 'LTL'))
											begin
												if (@AllHousesTotalNumberOfPackages <> 0)
												begin
													set @Ratio = @MasterReceivableQuantity / @AllHousesTotalNumberOfPackages
												end

												set @Quantity = @HouseNumberOfPackages
												set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
											end

											else 
											begin
												if (@AllHousesTotalNumberOfContainers <> 0)
												begin
													set @Ratio = @MasterReceivableQuantity / @AllHousesTotalNumberOfContainers
												end

												set @Quantity = @HouseNumberOfContainers
												set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
											end											
										END
										----------

										else if ((@MasterTransportModeId = 'O' AND @MasterTypeId = 'FCLD') OR (@MasterTransportModeId = 'I' AND @MasterTypeId = 'FTL'))
										BEGIN
											set @Quantity = (select COUNT(Id) from ShipmentPackages where ShipmentId = @HouseId AND PackageTypeId = @MasterReceivablePackageTypeId)
											set @UnitPrice = @MasterReceivableUnitPrice	
										END

										else if ((@MasterTransportModeId = 'O' AND @MasterTypeId = 'MyGO') OR (@MasterTransportModeId = 'I' AND @MasterTypeId = 'MyGI'))
										BEGIN
											if (@AllHousesTotalChargeables <> 0)
											begin
												set @Ratio = @MasterReceivableQuantity / @AllHousesTotalChargeables
											end

											set @Quantity = @HouseChargeableWeight
											set @UnitPrice = @Ratio * @MasterReceivableUnitPrice
										END

									END
						
									-- Compute Amounts
									BEGIN
										set @Amount = @Quantity * @UnitPrice

										if (@MasterReceivableMeasurementId = @PRVL_Id)
										BEGIN
											set @Amount = @Quantity * @UnitPrice / 100
										END

										set @AmountLocal = @Amount * @MasterReceivableRate
										set @AmountInProfitCurrency = @AmountLocal / @MasterReceivableProfitRate
									END

									-- Update Receivable
									BEGIN
										update ShipmentReceivables
										set
										MeasurementId = @HouseReceivableMeasurementId,
										PrepaidCollectId = @MasterReceivablePrepaidCollectId,
										DueTypeCode = @MasterReceivableDueTypeCode,
										--AWBPrint = 0, --@MasterReceivableAWBPrint,						
										UpdateDate = @MasterReceivableUpdateDate,
										UpdateByUserId = @MasterReceivableUpdatedByUserId,
										ShipmentReceivableLineStatusCode = @MasterReceivableLineStatusCode,
										CurrencyId = @MasterReceivableCurrencyId,
										Rate = @MasterReceivableRate,
										ProfitCurrencyExchangeRate = @MasterReceivableProfitRate,
										Quantity = isnull(ROUND(@Quantity,3),0),
										UnitPrice = isnull(Round(@UnitPrice,3),0),							
										TotalAmount = isnull(Round(@Amount,3),0),
										TotalAmountLocal = isnull(Round(@AmountLocal,3),0),
										AmountInProfitCurrency = isnull(Round(@AmountInProfitCurrency,3),0),
										IsFixedPrice = @MasterReceivableIsFixedPrice,
										IsExchangeRateFixed = @MasterReceivableIsExchangeRateFixed,
										ARInvoiceId = @MasterReceivableARInvoiceId,
										ARInvoiceLineId = @MasterReceivableARInvoiceLineId,
										IATACodeId = @MasterReceivableIATACodeId
										where Id = @HouseReceivableId and Tenant = @Tenant
									END

								END

							FETCH NEXT FROM HouseReceivables1Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
							END
							CLOSE HouseReceivables1Cursor
							DEALLOCATE HouseReceivables1Cursor
						END

						FETCH NEXT FROM Houses2Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods, @HouseNumberOfPackages, @HouseNumberOfContainers, @HouseTransportModeId, @HouseTypeId, @HouseGrossWeightInKG, @HouseChargeableWeightInKG, @HouseVolumeInCBM, @HouseGrossWeightPerStorageDays
						END
						CLOSE Houses2Cursor
						DEALLOCATE Houses2Cursor
					END

				FETCH NEXT FROM MasterReceivables1Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
				END
				CLOSE MasterReceivables1Cursor
				DEALLOCATE MasterReceivables1Cursor
			END

			-- Loop Master Receivables (2: PRFR)
			BEGIN
				DECLARE MasterReceivables2Cursor CURSOR READ_ONLY
				FOR
				SELECT Id, Quantity, UnitPrice, ChargesTypeId, MeasurementId, PrepaidCollectId, DueTypeCode, AWBPrint, CurrencyId, Rate, ProfitCurrencyExchangeRate, ShipmentReceivableLineStatusCode, CreatedByUserId, UpdateByUserId, CreateDate, UpdateDate, TotalAmount, ARInvoiceId, ARInvoiceLineId, IsFixedPrice, IsExchangeRateFixed, IATACodeId
				FROM ShipmentReceivables
				WHERE Tenant = @Tenant AND ShipmentId = @MasterId AND MeasurementId = @PRFR_Id
				OPEN MasterReceivables2Cursor FETCH NEXT FROM MasterReceivables2Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
				WHILE @@FETCH_STATUS = 0
				BEGIN

					-- Loop Houses
					BEGIN
						DECLARE Houses3Cursor CURSOR READ_ONLY
						FOR
						SELECT Id, TEU, Volume, GrossWeight, ChargeableWeight, GrossWeightPerTon, ValueOfGoods
						FROM Shipments
						WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId and Tenant = @Tenant
						OPEN Houses3Cursor FETCH NEXT FROM Houses3Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
						WHILE @@FETCH_STATUS = 0
						BEGIN

						set @IsCreatingReceivable = 0
						set @HouseReceivableMeasurementId = @MasterReceivableMeasurementId
						set @HouseFreightAmount = (select sum(isnull(TotalAmount,0)) from ShipmentReceivables where ShipmentId = @HouseId AND ShipmentReceivableParentId is not null AND ChargesTypeId in (select Id from ChargesTypes where ChargesGroupCode = 'FRT' AND Tenant = @Tenant))			

						-- IsCreatingReceivable
						BEGIN

							if (@MasterReceivableMeasurementId = @PRFR_Id)
							BEGIN
								if not exists (select * from ShipmentReceivables where Tenant = @Tenant and ShipmentId = @HouseId and ShipmentReceivableParentId = @MasterReceivableId and ChargesTypeId = @MasterReceivableChargesTypeId)
								set @IsCreatingReceivable = 1
							END
				
							if (@IsCreatingReceivable = 1)
							BEGIN

								SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
								BEGIN TRAN T1;
								EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'ShipmentReceivable'
								COMMIT TRAN T1; 

								INSERT INTO ShipmentReceivables
								(
								Id,
								Tenant,
								ShipmentId,
								ShipmentReceivableParentId,
								ShipmentReceivableLineStatusCode,
								ChargesTypeId,
								MeasurementId,
								PrepaidCollectId,
								DueTypeCode,
								AWBPrint,
								CurrencyId,
								Rate,
								ProfitCurrencyExchangeRate,
								CreateDate,
								UpdateDate,
								CreatedByUserId,
								UpdateByUserId,
								IsFromQuote,
								PayableLocal,
								IsFixedPrice,
								IsExchangeRateFixed,						
								ARInvoiceId,
								ARInvoiceLineId,
								IATACodeId
								)
								VALUES
								(
								@NewId,
								@Tenant,
								@HouseId,
								@MasterReceivableId,
								@MasterReceivableLineStatusCode,
								@MasterReceivableChargesTypeId,
								@HouseReceivableMeasurementId,
								@MasterReceivablePrepaidCollectId,
								@MasterReceivableDueTypeCode,
								@MasterReceivableAWBPrint,
								@MasterReceivableCurrencyId,
								@MasterReceivableRate,
								@MasterReceivableProfitRate,
								@MasterReceivableCreateDate,
								@MasterReceivableUpdateDate,
								@MasterReceivableCreatedByUserId,
								@MasterReceivableUpdatedByUserId,
								0,
								0,
								@MasterReceivableIsFixedPrice,
								@MasterReceivableIsExchangeRateFixed,
								@MasterReceivableARInvoiceId,
								@MasterReceivableARInvoiceLineId,
								@MasterReceivableIATACodeId
								)				
							END
						END

						-- Loop House Receivables / Amount Calculating & Updating
						BEGIN
							DECLARE HouseReceivables2Cursor CURSOR READ_ONLY
							FOR
							SELECT Id, ARInvoiceId
							FROM ShipmentReceivables
							WHERE ShipmentId = @HouseId AND Tenant = @Tenant AND ShipmentReceivableParentId = @MasterReceivableId
							OPEN HouseReceivables2Cursor FETCH NEXT FROM HouseReceivables2Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
							WHILE @@FETCH_STATUS = 0
							BEGIN
		
								set @IsUpdatingReceivable = 0

								if (@IsCreatingReceivable = 1)
								begin
									set @IsUpdatingReceivable = 1
								end

								else if (@IsInvoiceUpdated_PARAM = 1)
								begin
									set @IsUpdatingReceivable = 1
								end

								else
								begin									
									if (@HouseReceivableARInvoiceId is not null AND @MasterReceivableARInvoiceId is not null)
									begin
										set @IsUpdatingReceivable = 0
									end

									else
									begin
										set @IsUpdatingReceivable = 1
									end
								end

								--set @IsUpdatingReceivable = 1
								if (@IsUpdatingReceivable = 1)
								BEGIN
									set @Quantity = @HouseFreightAmount
									set @UnitPrice = @MasterReceivableUnitPrice
									set @Amount = @Quantity * @UnitPrice / 100
									set @AmountLocal = @Amount * @MasterReceivableRate
									set @AmountInProfitCurrency = @AmountLocal / @MasterReceivableProfitRate

									-- Update Receivable
									BEGIN
										update ShipmentReceivables
										set
										MeasurementId = @HouseReceivableMeasurementId,
										PrepaidCollectId = @MasterReceivablePrepaidCollectId,
										DueTypeCode = @MasterReceivableDueTypeCode,
										--AWBPrint = 0, --@MasterReceivableAWBPrint,						
										UpdateDate = @MasterReceivableUpdateDate,
										UpdateByUserId = @MasterReceivableUpdatedByUserId,
										ShipmentReceivableLineStatusCode = @MasterReceivableLineStatusCode,
										CurrencyId = @MasterReceivableCurrencyId,
										Rate = @MasterReceivableRate,
										ProfitCurrencyExchangeRate = @MasterReceivableProfitRate,
										Quantity = isnull(ROUND(@Quantity,3),0),
										UnitPrice = isnull(Round(@UnitPrice,3),0),							
										TotalAmount = isnull(Round(@Amount,3),0),
										TotalAmountLocal = isnull(Round(@AmountLocal,3),0),
										AmountInProfitCurrency = isnull(Round(@AmountInProfitCurrency,3),0),
										IsFixedPrice = @MasterReceivableIsFixedPrice,
										IsExchangeRateFixed = @MasterReceivableIsExchangeRateFixed,
										ARInvoiceId = @MasterReceivableARInvoiceId,
										ARInvoiceLineId = @MasterReceivableARInvoiceLineId,
										IATACodeId = @MasterReceivableIATACodeId
										where Id = @HouseReceivableId and Tenant = @Tenant
									END
								END
							FETCH NEXT FROM HouseReceivables2Cursor INTO @HouseReceivableId, @HouseReceivableARInvoiceId
							END
							CLOSE HouseReceivables2Cursor
							DEALLOCATE HouseReceivables2Cursor
						END

						FETCH NEXT FROM Houses3Cursor INTO @HouseId, @HouseTEU, @HouseVolume, @HouseGrossWeight, @HouseChargeableWeight, @HouseGrossWeightPerTon, @HouseValueOfGoods
						END
						CLOSE Houses3Cursor
						DEALLOCATE Houses3Cursor
					END

				FETCH NEXT FROM MasterReceivables2Cursor INTO @MasterReceivableId, @MasterReceivableQuantity, @MasterReceivableUnitPrice, @MasterReceivableChargesTypeId, @MasterReceivableMeasurementId, @MasterReceivablePrepaidCollectId, @MasterReceivableDueTypeCode, @MasterReceivableAWBPrint, @MasterReceivableCurrencyId, @MasterReceivableRate, @MasterReceivableProfitRate, @MasterReceivableLineStatusCode,  @MasterReceivableCreatedByUserId, @MasterReceivableUpdatedByUserId, @MasterReceivableCreateDate, @MasterReceivableUpdateDate, @MasterReceivableAmount, @MasterReceivableARInvoiceId, @MasterReceivableARInvoiceLineId, @MasterReceivableIsFixedPrice, @MasterReceivableIsExchangeRateFixed, @MasterReceivableIATACodeId
				END
				CLOSE MasterReceivables2Cursor
				DEALLOCATE MasterReceivables2Cursor
			END

		END				
	END
END