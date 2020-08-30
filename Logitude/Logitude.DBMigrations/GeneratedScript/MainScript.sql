-- Add New Column With Name PickupDeliveryCWeightUnitCode
ALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryCWeightUnitCode] VARCHAR(3) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c3f15735-566d-498f-9c5f-e23ae453ccbc', 'Quote.dxml', 'Quotes', 'PickupDeliveryCWeightUnitCode', 'Add Column', GETDATE(), '-- Add New Column With Name PickupDeliveryCWeightUnitCodeALTER TABLE [dbo].[Quotes] ADD [PickupDeliveryCWeightUnitCode] VARCHAR(3) NULL;');


-- Drop Column PickupDeliveryVolumetricWeight
EXEC SP_RENAME 'dbo.QuotePackages.PickupDeliveryVolumetricWeight', 'Drop_PickupDeliveryVolumetricWeight', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2c033783-4f73-4cd0-898d-06b1e3aa1f1e', 'QuotePackage.dxml', 'QuotePackages', 'PickupDeliveryVolumetricWeight', 'Drop Column', GETDATE(), '-- Drop Column PickupDeliveryVolumetricWeightEXEC SP_RENAME ''dbo.QuotePackages.PickupDeliveryVolumetricWeight'', ''Drop_PickupDeliveryVolumetricWeight'', ''COLUMN'';');

-- Drop Column PickupDeliveryVolume
EXEC SP_RENAME 'dbo.QuotePackages.PickupDeliveryVolume', 'Drop_PickupDeliveryVolume', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1e7886e7-34ac-460c-a41a-36bce11bb5cf', 'QuotePackage.dxml', 'QuotePackages', 'PickupDeliveryVolume', 'Drop Column', GETDATE(), '-- Drop Column PickupDeliveryVolumeEXEC SP_RENAME ''dbo.QuotePackages.PickupDeliveryVolume'', ''Drop_PickupDeliveryVolume'', ''COLUMN'';');


-- Procedure Script From usp_UpdateCardSearchFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCardSearchFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCardSearchFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCardSearchFunction]
(
@CardId varchar(15)
)
AS
declare  @Tenant int
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
Declare @SearchField nvarchar(max)
if (@CardId is not null)
begin
delete CardSearches where CardId = @CardId
select
@Tenant = Tenant,
@Code = Code,
@EnglishName = EnglishName,
@LocalName = LocalName,
@VatNumber = VatNumber,
@CityName = CityName,
@CountryName =CountryName,
@ReceivablesAccountingCard = ReceivablesAccountingCard,
@PayablesAccountingCard = PayablesAccountingCard,
@CreateDate = CreateDate,
@UpdateDate = UpdateDate,
@PartnerTypeId = PartnerTypeId,
@InActive = InActive
from Cards
where Id = @CardId
set @Weight = 0
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
set @SearchField = @Code;
if(@EnglishName is not null) set @SearchField += ('' '' + @EnglishName);
if(@LocalName is not null) set @SearchField += ('' '' + @LocalName);
if(@VatNumber is not null) set @SearchField += ('' '' + @VatNumber);
if(@CountryName is not null) set @SearchField += ('' '' + @CountryName);
if(@CityName is not null) set @SearchField += ('' '' + @CityName);
if(@ReceivablesAccountingCard is not null) set @SearchField += ('' '' + @ReceivablesAccountingCard);
if(@PayablesAccountingCard is not null) set @SearchField += ('' '' + @PayablesAccountingCard);
insert into CardSearches (Tenant, CardId  , RecordDate , Weight , PartnerTypeId,InActive , Keyword) select  @Tenant, @CardId , @RecordDate , @Weight , @PartnerTypeId,@InActive ,  Name from dbo.SplitBySpaceFunction(@SearchField) where Name !='' ''
end');


-- General Script From 202008110941_AddNewMeasurement.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'PDCW')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('PDCW', 'Pickup/Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight', @MeasurementId, @Tenant, 0, 0, 0, 'PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008110941_AddNewMeasurement.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = ''PDCW'')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''PDCW'', ''Pickup/Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'', @MeasurementId, @Tenant, 0, 0, 0, ''PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '34b9d9902a50650ec4ac4fa4aa87bfe1', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 20200812_SetIATACodeToSRForImportStorageCharge.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = 'SR')
where Code = 'ISTOR'
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('20200812_SetIATACodeToSRForImportStorageCharge.sxml', GETDATE(), 'update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = ''SR'')
where Code = ''ISTOR''
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), '60d427a5e1ea664ef4e7d5766423a171', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008211829_FillPickupDeliveryCWeightUnitCodeQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Quotes') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null ,PickupDeliveryCWeightUnitCode varchar(3))
select
Id,
Tenant,
TransportModeId
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @TransportModeId as varchar(4)
declare @PickupDelivaryChargeableWeightUnitCode as varchar(3)
declare @TenantChargeableWeightUnitCode as varchar(3)
declare @TenantPMWeightMeasurementUnitCode as varchar(3)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantChargeableWeightUnitCode = (select ChargeableWeightUnitCode from Tenants where Id = @Tenant)
set @TenantPMWeightMeasurementUnitCode = (select WeightMeasurementUnitCode  from Tenants where Id = @Tenant)
if (@TransportModeId = 'A') set @PickupDelivaryChargeableWeightUnitCode = @TenantChargeableWeightUnitCode
else if (@TransportModeId = 'O') set @PickupDelivaryChargeableWeightUnitCode = @TenantPMWeightMeasurementUnitCode
insert into #temp_Quotes(Id, PickupDeliveryCWeightUnitCode) values (@EntityId, @PickupDelivaryChargeableWeightUnitCode)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryCWeightUnitCode = #temp_Quotes.PickupDeliveryCWeightUnitCode
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY '00:00:01'
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryCWeightUnitCode = #temp_Quotes.PickupDeliveryCWeightUnitCode
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008211829_FillPickupDeliveryCWeightUnitCodeQuoteField.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Quotes'') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null ,PickupDeliveryCWeightUnitCode varchar(3))
select
Id,
Tenant,
TransportModeId
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @TransportModeId as varchar(4)
declare @PickupDelivaryChargeableWeightUnitCode as varchar(3)
declare @TenantChargeableWeightUnitCode as varchar(3)
declare @TenantPMWeightMeasurementUnitCode as varchar(3)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantChargeableWeightUnitCode = (select ChargeableWeightUnitCode from Tenants where Id = @Tenant)
set @TenantPMWeightMeasurementUnitCode = (select WeightMeasurementUnitCode  from Tenants where Id = @Tenant)
if (@TransportModeId = ''A'') set @PickupDelivaryChargeableWeightUnitCode = @TenantChargeableWeightUnitCode
else if (@TransportModeId = ''O'') set @PickupDelivaryChargeableWeightUnitCode = @TenantPMWeightMeasurementUnitCode
insert into #temp_Quotes(Id, PickupDeliveryCWeightUnitCode) values (@EntityId, @PickupDelivaryChargeableWeightUnitCode)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryCWeightUnitCode = #temp_Quotes.PickupDeliveryCWeightUnitCode
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY ''00:00:01''
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryCWeightUnitCode = #temp_Quotes.PickupDeliveryCWeightUnitCode
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes', DATEDIFF(MS,@StartTime,@EndTime), '5c5cad66b8c2d0efd07e2e3b6c37843e', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008221040_FillPickupDeliveryRatioQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Quotes') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null , PickupDelivaryRatio float)
select
Id,
Tenant,
ShipmentTypeId
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @PickupDelivaryRatio as float
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ShipmentTypeId = 'Air' or @ShipmentTypeId = 'LCL' or @ShipmentTypeId = 'LCLD' or @ShipmentTypeId = 'LTL') set @PickupDelivaryRatio = '3.3'
else if (@ShipmentTypeId = 'FCL' or @ShipmentTypeId = 'FCLD' or @ShipmentTypeId = 'FTL') set @PickupDelivaryRatio ='1'
insert into #temp_Quotes(Id, PickupDelivaryRatio) values (@EntityId, @PickupDelivaryRatio)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryRatio = #temp_Quotes.PickupDelivaryRatio
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY '00:00:01'
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryRatio = #temp_Quotes.PickupDelivaryRatio
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008221040_FillPickupDeliveryRatioQuoteField.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Quotes'') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null , PickupDelivaryRatio float)
select
Id,
Tenant,
ShipmentTypeId
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @PickupDelivaryRatio as float
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ShipmentTypeId = ''Air'' or @ShipmentTypeId = ''LCL'' or @ShipmentTypeId = ''LCLD'' or @ShipmentTypeId = ''LTL'') set @PickupDelivaryRatio = ''3.3''
else if (@ShipmentTypeId = ''FCL'' or @ShipmentTypeId = ''FCLD'' or @ShipmentTypeId = ''FTL'') set @PickupDelivaryRatio =''1''
insert into #temp_Quotes(Id, PickupDelivaryRatio) values (@EntityId, @PickupDelivaryRatio)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryRatio = #temp_Quotes.PickupDelivaryRatio
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY ''00:00:01''
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryRatio = #temp_Quotes.PickupDelivaryRatio
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes', DATEDIFF(MS,@StartTime,@EndTime), '354d8c82d79ba86abb3e766f129ad515', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008221130_FillPickupDeliveryChargeableWeightQuoteField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Quotes') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null , PickupDeliveryVolumetricWeight float, PickupDeliveryChargeableWeight float)
select
Id,
Tenant,
Volume,
VolumeUnitCode,
GrossWeight,
GrossWeightUnitCode,
PickupDeliveryCWeightUnitCode,
PickupDeliveryRatio
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @PickupDelivaryRatio as float
declare @Count as int
DECLARE @Volume as FLoat
DECLARE @Weight as FLoat
DECLARE @VolumeCBM as FLoat
DECLARE @VolumeKG as FLoat
DECLARE @Volumetric as FLoat
DECLARE @PickupDeliveryChargeableWeight as FLoat
DECLARE @VolumeUnitCode as varchar(3)
DECLARE @GrossWeightUnitCode as varchar(3)
DECLARE @PickupDeliveryCWeightUnitCode as varchar(3)
DECLARE @TransportModeId as varchar(1)
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, Volume,VolumeUnitCode, GrossWeight, GrossWeightUnitCode, PickupDeliveryCWeightUnitCode, PickupDeliveryRatio
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Volume,@VolumeUnitCode, @Weight,@GrossWeightUnitCode, @PickupDeliveryCWeightUnitCode, @PickupDelivaryRatio
WHILE @@FETCH_STATUS = 0
BEGIN
-- Compute VolumeKG
if (@Volume is NULL)
begin
if (@GrossWeightUnitCode = 'KG')
begin
set @VolumeKG = Round(@Weight,3)
end
else if (@GrossWeightUnitCode = 'LB')
begin
set @VolumeKG = Round(@Weight / 2.20458,3)
end
else if (@GrossWeightUnitCode = 'MT')
begin
set @VolumeKG = Round(@Weight * 1000,3)
end
end
else
begin
if (@VolumeUnitCode = 'CBM')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = 'CBF')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = 'CBI')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @PickupDelivaryRatio,3)
end
-- Compute PickupDeliveryVolumetric
if (@PickupDeliveryCWeightUnitCode = 'KG')
begin
set @Volumetric = Round(@VolumeKG,3)
end
else if (@PickupDeliveryCWeightUnitCode = 'LB')
begin
set @Volumetric = Round(@VolumeKG * 2.20458,3)
end
else if (@PickupDeliveryCWeightUnitCode = 'MT')
begin
set @Volumetric = Round(@VolumeKG / 1000,3)
end
-- Compute PickupDeliveryVolumetricWeight
-- insert values in temp table
insert into #temp_Quotes(Id, PickupDeliveryVolumetricWeight, PickupDeliveryChargeableWeight) values (@EntityId, @Volumetric, @PickupDeliveryChargeableWeight)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryVolumetricWeight = #temp_Quotes.PickupDeliveryVolumetricWeight
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY '00:00:01'
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Volume,@VolumeUnitCode, @Weight,@GrossWeightUnitCode, @PickupDeliveryCWeightUnitCode, @PickupDelivaryRatio
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryVolumetricWeight = #temp_Quotes.PickupDeliveryVolumetricWeight
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008221130_FillPickupDeliveryChargeableWeightQuoteField.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Quotes'') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (Id varchar(15) not null , PickupDeliveryVolumetricWeight float, PickupDeliveryChargeableWeight float)
select
Id,
Tenant,
Volume,
VolumeUnitCode,
GrossWeight,
GrossWeightUnitCode,
PickupDeliveryCWeightUnitCode,
PickupDeliveryRatio
into #tempTable
FROM Quotes
declare @Tenant as int
declare @EntityId as varchar(15)
declare @PickupDelivaryRatio as float
declare @Count as int
DECLARE @Volume as FLoat
DECLARE @Weight as FLoat
DECLARE @VolumeCBM as FLoat
DECLARE @VolumeKG as FLoat
DECLARE @Volumetric as FLoat
DECLARE @PickupDeliveryChargeableWeight as FLoat
DECLARE @VolumeUnitCode as varchar(3)
DECLARE @GrossWeightUnitCode as varchar(3)
DECLARE @PickupDeliveryCWeightUnitCode as varchar(3)
DECLARE @TransportModeId as varchar(1)
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, Volume,VolumeUnitCode, GrossWeight, GrossWeightUnitCode, PickupDeliveryCWeightUnitCode, PickupDeliveryRatio
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Volume,@VolumeUnitCode, @Weight,@GrossWeightUnitCode, @PickupDeliveryCWeightUnitCode, @PickupDelivaryRatio
WHILE @@FETCH_STATUS = 0
BEGIN
-- Compute VolumeKG
if (@Volume is NULL)
begin
if (@GrossWeightUnitCode = ''KG'')
begin
set @VolumeKG = Round(@Weight,3)
end
else if (@GrossWeightUnitCode = ''LB'')
begin
set @VolumeKG = Round(@Weight / 2.20458,3)
end
else if (@GrossWeightUnitCode = ''MT'')
begin
set @VolumeKG = Round(@Weight * 1000,3)
end
end
else
begin
if (@VolumeUnitCode = ''CBM'')
begin
set @VolumeCBM = Round(@Volume,3)
end
else if (@VolumeUnitCode = ''CBF'')
begin
set @VolumeCBM = Round(@Volume / 35.31466,3)
end
else if (@VolumeUnitCode = ''CBI'')
begin
set @VolumeCBM = Round(@Volume / 61023.74409,3)
end
set @VolumeKG = Round(@VolumeCBM * 1000 / @PickupDelivaryRatio,3)
end
-- Compute PickupDeliveryVolumetric
if (@PickupDeliveryCWeightUnitCode = ''KG'')
begin
set @Volumetric = Round(@VolumeKG,3)
end
else if (@PickupDeliveryCWeightUnitCode = ''LB'')
begin
set @Volumetric = Round(@VolumeKG * 2.20458,3)
end
else if (@PickupDeliveryCWeightUnitCode = ''MT'')
begin
set @Volumetric = Round(@VolumeKG / 1000,3)
end
-- Compute PickupDeliveryVolumetricWeight
-- insert values in temp table
insert into #temp_Quotes(Id, PickupDeliveryVolumetricWeight, PickupDeliveryChargeableWeight) values (@EntityId, @Volumetric, @PickupDeliveryChargeableWeight)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
PickupDeliveryVolumetricWeight = #temp_Quotes.PickupDeliveryVolumetricWeight
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
truncate table #temp_Quotes
WAITFOR DELAY ''00:00:01''
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Volume,@VolumeUnitCode, @Weight,@GrossWeightUnitCode, @PickupDeliveryCWeightUnitCode, @PickupDelivaryRatio
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
PickupDeliveryVolumetricWeight = #temp_Quotes.PickupDeliveryVolumetricWeight
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id = #temp_Quotes.Id
end
drop table #tempTable
drop table #temp_Quotes', DATEDIFF(MS,@StartTime,@EndTime), 'e3221e15f6b1b70811a35a0684e8d7f4', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

