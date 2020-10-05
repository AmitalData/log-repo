-- General Script From 202009231117_UpdateActiveForInterestFieldOnGLAccount.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update GLAccounts set ActiveForInterest = 0 where ActiveForInterest is null
update GLAccounts set ActiveForInterestCreditInvoice = 0 where ActiveForInterestCreditInvoice is null
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009231117_UpdateActiveForInterestFieldOnGLAccount.sxml', GETDATE(), 'update GLAccounts set ActiveForInterest = 0 where ActiveForInterest is null
update GLAccounts set ActiveForInterestCreditInvoice = 0 where ActiveForInterestCreditInvoice is null', DATEDIFF(MS,@StartTime,@EndTime), 'b06a251f158db248920db0342229802c', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Change Type From varchar To nvarchar For Column InvoiceFailureReason
ALTER TABLE [dbo].[InterestReports] ALTER COLUMN [InvoiceFailureReason] NVARCHAR(1024);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('469c0d17-ac7b-4b2e-9345-ca92be4f5620', 'InterestReport.dxml', 'InterestReports', 'InvoiceFailureReason', 'Alter Column Type', GETDATE(), '-- Change Type From varchar To nvarchar For Column InvoiceFailureReasonALTER TABLE [dbo].[InterestReports] ALTER COLUMN [InvoiceFailureReason] NVARCHAR(1024);');


-- Add New Column With Name SearchFields
ALTER TABLE [dbo].[TicketEscalations] ADD [SearchFields] NVARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c609b0fd-4c36-478d-b260-fec2affd5ea5', 'TicketEscalation.dxml', 'TicketEscalations', 'SearchFields', 'Add Column', GETDATE(), '-- Add New Column With Name SearchFieldsALTER TABLE [dbo].[TicketEscalations] ADD [SearchFields] NVARCHAR(1000) NULL;');


-- Create Index On CardSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId_Weight] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId],[Weight]) INCLUDE([CardId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fd42c57e-1668-4608-8110-9a4c0177b6f1', 'CardSearch.dxml', 'CardSearches', 'Tenant,InActive,Keyword,PartnerTypeId,Weight', 'Create Index', GETDATE(), '-- Create Index On CardSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CardSearches_Tenant_InActive_Keyword_PartnerTypeId_Weight] ON [dbo].[CardSearches]([Tenant],[InActive],[Keyword],[PartnerTypeId],[Weight]) INCLUDE([CardId])'');');


-- Change Type From varchar To nvarchar For Column DescriptionOfGoods
ALTER TABLE [dbo].[Quotes] ALTER COLUMN [DescriptionOfGoods] NVARCHAR(512);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2ef187b1-9a44-4726-9872-9178550952ab', 'Quote.dxml', 'Quotes', 'DescriptionOfGoods', 'Alter Column Type', GETDATE(), '-- Change Type From varchar To nvarchar For Column DescriptionOfGoodsALTER TABLE [dbo].[Quotes] ALTER COLUMN [DescriptionOfGoods] NVARCHAR(512);');

-- Add New Column With Name DescriptionRightToLeft
ALTER TABLE [dbo].[Quotes] ADD [DescriptionRightToLeft] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c1deaec6-eb15-4ff6-bd98-1d58230e2da0', 'Quote.dxml', 'Quotes', 'DescriptionRightToLeft', 'Add Column', GETDATE(), '-- Add New Column With Name DescriptionRightToLeftALTER TABLE [dbo].[Quotes] ADD [DescriptionRightToLeft] BIT DEFAULT(0) NOT NULL;');


-- Add New Column With Name IsCFSWarehouse
ALTER TABLE [dbo].[Shipments] ADD [IsCFSWarehouse] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3d231179-33c6-4f0d-b623-e328da45351a', 'Shipment.dxml', 'Shipments', 'IsCFSWarehouse', 'Add Column', GETDATE(), '-- Add New Column With Name IsCFSWarehouseALTER TABLE [dbo].[Shipments] ADD [IsCFSWarehouse] BIT DEFAULT(0) NOT NULL;');

-- Add New Column With Name IsCFSWarehouseChanged
ALTER TABLE [dbo].[Shipments] ADD [IsCFSWarehouseChanged] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c70db3ae-8a4d-4d4f-9556-bfa89029f1de', 'Shipment.dxml', 'Shipments', 'IsCFSWarehouseChanged', 'Add Column', GETDATE(), '-- Add New Column With Name IsCFSWarehouseChangedALTER TABLE [dbo].[Shipments] ADD [IsCFSWarehouseChanged] BIT DEFAULT(0) NOT NULL;');

-- Create Index On Shipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_ComputedStatusDate] ON [dbo].[Shipments]([Tenant],[ComputedStatusDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('22137cea-a326-4916-8a50-fbc1f59b562a', 'Shipment.dxml', 'Shipments', 'Tenant,ComputedStatusDate', 'Create Index', GETDATE(), '-- Create Index On Shipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_ComputedStatusDate] ON [dbo].[Shipments]([Tenant],[ComputedStatusDate])'');');


-- DataView Script From ShipmentDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentDataView] END');
EXEC('CREATE VIEW [dbo].[ShipmentDataView]
AS
SELECT        dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber,dbo.Shipments.DeclarationNumber,dbo.Shipments.IncludesCustoms,dbo.Shipments.CustomsClearanceDate,dbo.Shipments.DeclarationDate, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,dbo.Shipments.ValueOfGoods,
dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId, dbo.shipments.SLAC,
dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId, dbo.Shipments.FirstARInvoiceApprovalDate,
dbo.Shipments.LastFinalDestination, [dbo].[Shipments].[From], [dbo].[Shipments].[To], dbo.Shipments.Origin, dbo.Shipments.FirstPickupETA, dbo.Shipments.FirstPickupETD,
dbo.Shipments.ExceptionDescription,dbo.Shipments.ValueOfGoodsCurrencyId,dbo.Shipments.ExceptionDate, dbo.Shipments.HasException, dbo.Shipments.IsManifestSentToAgent, dbo.Shipments.ManifestLastSharingDate,
dbo.Shipments.AgentSharedManifestRef , dbo.Shipments.ExceptionResolvedDescription,dbo.Shipments.LastExceptionDescription, dbo.Shipments.OperationalDate, dbo.ShipmentMasterDatas.CutoffDate,
dbo.Shipments.ComputedStatusId, dbo.Shipments.ComputedStatusDate, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
dbo.Shipments.CustomConnectToShipment, dbo.Shipments.ForeignPartnerCountryCode, dbo.Shipments.IsNewARInvoiceBlocked,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId, dbo.ShipmentMasterDatas.Transshipment3CarrierId,
dbo.ShipmentMasterDatas.Transshipment2CarrierId, dbo.ShipmentMasterDatas.Transshipment1CarrierId, dbo.ShipmentMasterDatas.MainCarriageCarrierId,
dbo.ShipmentMasterDatas.MainCarriageIsFromStack, dbo.ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dbo.ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment3VesselId, dbo.ShipmentMasterDatas.Transshipment2VesselId, dbo.ShipmentMasterDatas.Transshipment1VesselId,
dbo.ShipmentMasterDatas.MainCarriageVesselId, dbo.ShipmentMasterDatas.BookingConfirmedBy, dbo.ShipmentMasterDatas.BookingConfirmationNotes,
dbo.ShipmentMasterDatas.BookingConfirmationNumber, dbo.ShipmentMasterDatas.MAWBOBLDate, dbo.ShipmentMasterDatas.Transshipment3CarrierNumber,
dbo.ShipmentMasterDatas.Transshipment3ETA, dbo.ShipmentMasterDatas.Transshipment3ETD, dbo.ShipmentMasterDatas.Transshipment3ATA,
dbo.ShipmentMasterDatas.Transshipment3ATD, dbo.ShipmentMasterDatas.Transshipment3ToPortId, dbo.ShipmentMasterDatas.Transshipment3FromPortId,
dbo.ShipmentMasterDatas.Transshipment2CarrierNumber, dbo.ShipmentMasterDatas.Transshipment2ETA, dbo.ShipmentMasterDatas.Transshipment2ETD,
dbo.ShipmentMasterDatas.Transshipment2ATA, dbo.ShipmentMasterDatas.Transshipment2ATD, dbo.ShipmentMasterDatas.Transshipment2ToPortId,
dbo.ShipmentMasterDatas.Transshipment2FromPortId, dbo.ShipmentMasterDatas.Transshipment1CarrierNumber, dbo.ShipmentMasterDatas.Transshipment1ETA,
dbo.ShipmentMasterDatas.Transshipment1ETD, dbo.ShipmentMasterDatas.Transshipment1ATA, dbo.ShipmentMasterDatas.Transshipment1ATD,
dbo.ShipmentMasterDatas.Transshipment1ToPortId, dbo.ShipmentMasterDatas.Transshipment1FromPortId, dbo.ShipmentMasterDatas.Master,
dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA, dbo.ShipmentMasterDatas.TrailerNumber,
dbo.ShipmentMasterDatas.MainCarriageFromAddressId, dbo.ShipmentMasterDatas.MainCarriageToAddressId,
dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2,
dbo.ShipmentMasterDatas.InterlineId, dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix,
dbo.ShipmentMasterDatas.ImportManifest,
dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails, dbo.Shipments.RegistryDate, dbo.Shipments.IsAssembly, dbo.Shipments.FirstOperationalCloseDate,
dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.FirstAccountingCloseDate,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.EstimateProfitInProfitCurrency, dbo.Shipments.ProfitCurrencyId, dbo.Shipments.SCI,
dbo.Shipments.AWBHandlingInformation, dbo.Shipments.AWBInsurrenceValue, dbo.Shipments.AWBAccountingInformation, dbo.Shipments.ProductCode,
dbo.Shipments.AWBDeclaredValueForCustoms, dbo.Shipments.AWBDeclaredValueForCarriage, dbo.Shipments.AWBCarrierTarrifReference,
dbo.Shipments.FreightForwarderContactId, dbo.Shipments.FreightForwarderAddressId, dbo.Shipments.CustomAgentExportContactId,
dbo.Shipments.CustomAgentExportAddressId, dbo.Shipments.ShipmentCustomerTypeCode, dbo.Shipments.CustomerReference1, dbo.Shipments.CustomerReference2, dbo.Shipments.CustomerContactId,
dbo.Shipments.CustomerAddressId, dbo.Shipments.CustomerId, dbo.Shipments.FreightForwarderReference, dbo.Shipments.FreightForwarderId,
dbo.Shipments.CustomAgentExportReference, dbo.Shipments.CustomAgentExportId, dbo.Shipments.CustomAgentImportReference,
dbo.Shipments.ConsigneeAddressOneTime, dbo.Shipments.ShipperAddressOneTime, dbo.Shipments.EstimateProfitInLocalCurrency, dbo.Shipments.AWBCurrencyId,
dbo.Shipments.OrderChargeableWeight, dbo.Shipments.OnCarriageCarrierId, dbo.Shipments.PreCarriageCarrierId, dbo.Shipments.ProfitInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency, dbo.Shipments.LastUpdateDate,
dbo.Shipments.UpdatedByUserId, dbo.Shipments.IsCancelled, dbo.Shipments.CancelledDate, dbo.Shipments.IsAccountingClosed, dbo.Shipments.ShipmentPayableStatusCode,
dbo.Shipments.ShipmentReceivableStatusCode, dbo.Shipments.QuoteId, dbo.Shipments.ShipmentDeliveryIndex, dbo.Shipments.ShipmentPickUpIndex,
dbo.Shipments.LTCWEdited, dbo.Shipments.OnCarriageVesselId, dbo.Shipments.PreCarriageVesselId, dbo.Shipments.DangerousMaterialDescription,
dbo.Shipments.DangerousPackagingGroup, dbo.Shipments.DangerousClassNumber, dbo.Shipments.DangerousUnNumber, dbo.Shipments.DangerousIMDGCode,
dbo.Shipments.DangerousFlashPoint, dbo.Shipments.AWBFreightAmountPrepaid, dbo.Shipments.AWBFreightAmountCollect, dbo.Shipments.IsDangerous,
dbo.Shipments.MainHarmonize, dbo.Shipments.VolumeUnitCode, dbo.Shipments.ChargeableWeightEdited,
dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, dbo.Shipments.ColoaderId,
dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps,
dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId,
dbo.Shipments.ConsigneeNotImporterContactId, dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId,
dbo.Shipments.ConsigneeNotImporterId, dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId,
dbo.Shipments.OrderIsDangerouseGoods, dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight,
dbo.Shipments.Field10, dbo.Shipments.Field9, dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4,
dbo.Shipments.Field3, dbo.Shipments.Field2, dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight,
dbo.Shipments.Field11, dbo.Shipments.Field12, dbo.Shipments.Field13, dbo.Shipments.Field14, dbo.Shipments.Field15,
dbo.Shipments.Field16, dbo.Shipments.Field17, dbo.Shipments.Field18, dbo.Shipments.Field19, dbo.Shipments.Field20,
dbo.Shipments.Field21, dbo.Shipments.Field22, dbo.Shipments.Field23, dbo.Shipments.Field24, dbo.Shipments.Field25,
dbo.Shipments.Field26, dbo.Shipments.Field27, dbo.Shipments.Field28, dbo.Shipments.Field29, dbo.Shipments.Field30,
dbo.Shipments.Field31, dbo.Shipments.Field32, dbo.Shipments.Field33, dbo.Shipments.Field34, dbo.Shipments.Field35,
dbo.Shipments.Field36, dbo.Shipments.Field37, dbo.Shipments.Field38, dbo.Shipments.Field39, dbo.Shipments.Field40,
dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId, dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId,
dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId, dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId,
dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId, dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD,
dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD, dbo.Shipments.AgentId,dbo.Shipments.AgentComputed, dbo.Shipments.OnCarriageCarrierNumber,
dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD, dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId,
dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber, dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD,
dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId, dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate,
dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId, dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId,
dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id, dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId,
dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId, dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId,
dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House, dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1,
dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
dbo.Shipments.ShipmentSubTypeId,
dbo.Shipments.ARInvoices,
dbo.Shipments.NotInvoicedReceivablesAmount,
dbo.Shipments.ReleasingAgentId,
dbo.Shipments.ContainerLastStatusDate,
dbo.Shipments.Notify1Reference,
dbo.Shipments.Notify2Reference,
dbo.Shipments.ShipperNotExporterReference,
dbo.Shipments.ConsigneeNotImporterReference,
dbo.Shipments.ProjectNumber,
dbo.Shipments.INTTRALastStatusDate,
dbo.Shipments.INTTRASIError,
dbo.Shipments.INTTRASIStatusCode,
dbo.Shipments.INTTRASIStatusDate,
dbo.Shipments.INTTRABookingError,
dbo.Shipments.INTTRALastBookingResponse,
dbo.INTTRASIStatus.Name AS INTTRASIStatusName,
dbo.Shipments.CreatedByPartner AS CreatedByPartner,
dbo.Shipments.INTTRABookingStatusCode,
dbo.INTTRABookingStatuses.Name AS INTTRABookingStatusName,
dbo.Shipments.INTTRABookingTransStatusCode,
dbo.INTTRABookingTransStatuses.Name AS INTTRABookingTransStatusName,
dbo.Shipments.AccountManagerUserId,
dbo.Shipments.CustomsDeclarationNumber,
dbo.Shipments.ShipperName,dbo.Shipments.FBLIsFromStock,
dbo.Shipments.FreightRelease, dbo.Shipments.TerminalAvailable, dbo.Shipments.ISFDate, dbo.Shipments.ISFNumber, dbo.Shipments.ITDate, dbo.Shipments.ITNumber,
dbo.ShipmentMasterDatas.OBLTypeCode, dbo.ShipmentMasterDatas.DocumentsClosingDate,
dbo.Shipments.ShipperName as Shipper, dbo.Shipments.ENSNumber, dbo.Shipments.ENSDate, dbo.Shipments.WarehouseLegWarehouseId, dbo.Shipments.WarehouseLegAddressId, dbo.Shipments.WarehouseLegTerminalCode, dbo.Shipments.WarehouseLegExpectedEntryDate,
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,dbo.Shipments.WarehouseStorageFreeDays,
WarehouseLegCard.EnglishName AS WarehouseLegTerminalName,
dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,
dbo.EventTypes.EnglishName as LastSharedEventName,
WarehouseLegCard.CountryCode AS WarehouseLegAddressCountryCode,
WarehouseLegCard.CountryName AS WarehouseLegAddressCountryName,
--ShipperCards.EnglishName AS ShipperName,
MainCarriageFromPorts.Code AS MainCarriageFromPortCode, MainCarriageToPorts.Code AS MainCarriageToPortCode,
Transshipment1FromPorts.Code AS Transshipment1FromPortCode, Transshipment1ToPorts.Code AS Transshipment1ToPortCode,
Transshipment2FromPorts.Code AS Transshipment2FromPortCode, Transshipment2ToPorts.Code AS Transshipment2ToPortCode,
Transshipment3FromPorts.Code AS Transshipment3FromPortCode, Transshipment3ToPorts.Code AS Transshipment3ToPortCode,
PreCarriageFromPorts.Code AS PreCarriageFromPortCode, PreCarriageToPorts.Code AS PreCarriageToPortCode,
OnCarriageFromPorts.Code AS OnCarriageFromPortCode, OnCarriageToPorts.Code AS OnCarriageToPortCode,
MainCarriageToPorts.EnglishName AS MainCarriageToPortName, MainCarriageFromPorts.EnglishName AS MainCarriageFromPortName,
Transshipment1FromPorts.EnglishName AS Transshipment1FromPortName, Transshipment1ToPorts.EnglishName AS Transshipment1ToPortName,
Transshipment2ToPorts.EnglishName AS Transshipment2ToPortName, Transshipment2FromPorts.EnglishName AS Transshipment2FromPortName,
PreCarriageFromPorts.EnglishName AS PreCarriageFromPortName, OnCarriageFromPorts.EnglishName AS OnCarriageFromPortName,
PreCarriageToPorts.EnglishName AS PreCarriageToPortName, Transshipment3FromPorts.EnglishName AS Transshipment3FromPortName,
Transshipment3ToPorts.EnglishName AS Transshipment3ToPortName, OnCarriageToPorts.EnglishName AS OnCarriageToPortName,
CustomerCards.EnglishName AS CustomerName, CustomerCards.Notes AS CustomerNote,
ConsolidatorCards.EnglishName AS ConsolidatorName, ConsolidatorCards.Notes AS ConsolidatorNote,
FreightForwarderCards.EnglishName AS FreightForwarderName,
FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.Notes AS ShipperNote, ReleasingAgentCards.EnglishName AS ReleasingAgentName,
ConsigneeCards.EnglishName AS ConsigneeName,ConsigneeCards.EnglishName AS Consignee, ConsigneeCards.Notes AS ConsigneeNote, AgentComputedCards.EnglishName AS AgentName,
AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote,
CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote,
Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note,
ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote,
ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote,
ToPorts.Code AS ToPortCode, FromPorts.Code AS FromPortCode,
MainCarriageFromCountries.Code AS MainCarriageFromPortCountryCode, MainCarriageFromCountries.EnglishName AS MainCarriageFromPortCountryName,
MainCarriageToCountries.Code AS MainCarriageToPortCountryCode, MainCarriageToCountries.EnglishName AS MainCarriageToPortCountryName,
Transshipment1FromCountries.Code AS Transshipment1FromPortCountryCode,
Transshipment1FromCountries.EnglishName AS Transshipment1FromPortCountryName,
Transshipment2FromCountries.Code AS Transshipment2FromPortCountryCode,
Transshipment2FromCountries.EnglishName AS Transshipment2FromPortCountryName,
Transshipment3FromCountries.Code AS Transshipment3FromPortCountryCode,
Transshipment3FromCountries.EnglishName AS Transshipment3FromPortCountryName, PreCarriageFromCountries.Code AS PreCarriageFromPortCountryCode,
PreCarriageFromCountries.EnglishName AS PreCarriageFromPortCountryName, OnCarriageFromCountries.Code AS OnCarriageFromPortCountryCode,
OnCarriageFromCountries.EnglishName AS OnCarriageFromPortCountryName, Transshipment1ToCountries.Code AS Transshipment1ToPortCountryCode,
Transshipment1ToCountries.EnglishName AS Transshipment1ToPortCountryName, OnCarriageToCountries.Code AS OnCarriageToPortCountryCode,
OnCarriageToCountries.EnglishName AS OnCarriageToPortCountryName, Transshipment2ToCountries.Code AS Transshipment2ToPortCountryCode,
Transshipment2ToCountries.EnglishName AS Transshipment2ToPortCountryName, PreCarriageToCountries.Code AS PreCarriageToPortCountryCode,
PreCarriageToCountries.EnglishName AS PreCarriageToPortCountryName, Transshipment3ToCountries.Code AS Transshipment3ToPortCountryCode,
Transshipment3ToCountries.EnglishName AS Transshipment3ToPortCountryName, MainCarriageCarrierCards.EnglishName AS MainCarriageCarrierName,
MainCarriageCarrierCards.Code AS MainCarriageCarrierCode, Transshipment1CarrierCards.EnglishName AS Transshipment1CarrierName,
Transshipment1CarrierCards.Code AS Transshipment1CarrierCode, Transshipment2CarrierCards.EnglishName AS Transshipment2CarrierName,
Transshipment2CarrierCards.Code AS Transshipment2CarrierCode, Transshipment3CarrierCards.EnglishName AS Transshipment3CarrierName,
Transshipment3CarrierCards.Code AS Transshipment3CarrierCode, PreCarriageCarrierCards.EnglishName AS PreCarriageCarrierName,
PreCarriageCarrierCards.Code AS PreCarriageCarrierCode, OnCarriageCarrierCards.EnglishName AS OnCarriageCarrierName,
OnCarriageCarrierCards.Code AS OnCarriageCarrierCode, dbo.NextLegs.Name AS NextLegName, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.ShipmentReceivableStatus.Name AS ShipmentReceivableStatusName, dbo.ShipmentPayableStatus.Name AS ShipmentPayableStatusName,
AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName,dbo.MoveTypes.MoveTypeEnglishName AS MoveTypeName ,
dbo.ShipmentLevels.Name AS ShipmentLevelName,
dbo.ShipmentSubTypes.Name AS ShipmentSubTypeName,
dbo.EntityStatus.Id AS ShipmentStatusId,
dbo.EntityStatus.Name AS ShipmentStatusName,
dbo.EntityStatus.StatusWeight AS ShipmentStatusWeight,
dbo.Shipments.StatusDate as ShipmentStatusDate,
dbo.Shipments.StatusLocation as ShipmentStatusLocation,
ShipmentMasterDataEntityStatus.Id AS ShipmentMasterDataStatusId,
ShipmentMasterDataEntityStatus.Name AS ShipmentMasterDataStatusName,
ShipmentMasterDataEntityStatus.StatusWeight AS ShipmentMasterDataStatusWeight,
dbo.ShipmentMasterDatas.StatusDate As ShipmentMasterDataStatusDate,
dbo.ShipmentMasterDatas.StatusLocation as ShipmentMasterDataStatusLocation,
MainCarriageFinalDestinationPorts.Code AS MainCarriageFinalDestinationPortCode,
MainCarriageFinalDestinationPorts.EnglishName AS MainCarriageFinalDestinationPortName,
FinalDestinationCountries.Code AS MainCarriageFinalDestinationCountryCode,
FinalDestinationCountries.EnglishName AS MainCarriageFinalDestinationCountryName,
dbo.ShipmentMasterDatas.MasterShipmentNumber,
dbo.ShipmentMasterDatas.CarrierTransportDocumentNumber,
dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,dbo.Shipments.OperationalClosedByUserId,
dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency,
dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.GrossWeightPerStorageDays,dbo.Shipments.GrossWeightPerTon,
dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight,
dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Incoterms.Code AS IncotermCode,
dbo.FHLStatus.Code AS FHLStatusCode,
dbo.FHLStatus.Name AS FHLStatusName,
dbo.Shipments.FHLStatusDate,
dbo.FWBStatus.Code AS FWBStatusCode,
dbo.FWBStatus.Name AS FWBStatusName,
dbo.ShipmentMasterDatas.FWBStatusDate,
dbo.Shipments.LastSentByUserId,
dbo.CustomsTransmissionsStatus.Code AS LocalCustomsTransmissionsStatusCode,
dbo.CustomsTransmissionsStatus.Name AS LocalCustomsTransmissionsStatusName,
dbo.shipments.LocalCustomsTransmissionsStatusError,
dbo.Shipments.LocalCustomsTransmissionsStatusDate,
dbo.Shipments.LocalCustomsSentByUserId,
LocalCustomsSentByUser.EnglishName as LocalCustomsSentByUserName,
CargonautFHLStatus.Code AS CargonautFHLStatusCode,
CargonautFHLStatus.Name AS CargonautFHLStatusName,
dbo.Shipments.CargonautFHLStatusDate,
CargonautFWBStatus.Code AS CargonautFWBStatusCode,
CargonautFWBStatus.Name AS CargonautFWBStatusName,
dbo.ShipmentMasterDatas.CargonautFWBStatusDate,
dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason, dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber,
dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges, dbo.Shipments.AccountNumber,
dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate, dbo.Shipments.LastFSRStatusRequestDate,
dbo.Shipments.FinalArrivalDate, dbo.Shipments.EstimatedFinalArrivalDate, dbo.Shipments.ActualFinalArrivalDate, FromPortCountries.Code AS FromPortCountryCode,
ToPortCountries.Code AS ToPortCountryCode, dbo.Shipments.TEU, MainCarriageFromAddresses.City AS MainCarriageFromCity,
MainCarriageToAddresses.City AS MainCarriageToCity, MainCarriageFromAddressCountries.Code AS MainCarriageFromCountryCode,
MainCarriageToAddressCountries.Code AS MainCarriageToCountryCode, dbo.Shipments.ProfitExchangeRate, dbo.Shipments.CASSCode, dbo.Shipments.AMSBL,
dbo.Shipments.SpecialServicesTypeId, dbo.SpecialServicesTypes.Code AS SpecialServicesTypeCode,
dbo.SpecialServicesTypes.EnglishName AS SpecialServicesTypeName, dbo.Shipments.CustomFileId, dbo.Shipments.CustomFileNumber,
dbo.Shipments.NoFreightFile, ToPortCountries.EnglishName AS ToPortCountryName, FromPortCountries.EnglishName AS FromPortCountryName,
dbo.Vessels.EnglishName AS MainCarriageVesselName,dbo.Shipments.LastStatusLogDate As LastStatusLogDate,
AccountManagerUserContacts.EnglishName as AccountManagerUserName,
SalesmanUserContact.EnglishName as SalesmanUserName,
CreatedByUserContact.EnglishName as CreatedByUserName,
LastSentByUserContact.EnglishName as LastSentByUserName,
ShipmentComputedFields.IsMissingDocuments as IsMissingDocument,
ShipmentComputedFields.DocumentsSearchFields as DocumentsSearchFields,
dbo.Shipments.ForwarderShipmentNumber as ForwarderShipmentNumber,
dbo.Shipments.CustomerShipmentNumber as CustomerShipmentNumber,
ShipmentComputedFields.LastDocumentDateTime as LastDocumentDateTime,
HybridPartner.SmallLogoId as PartnerLogoId,
ShipmentComputedFields.MissingDocumentsCount as MissingDocumentsCount,
ShipmentComputedFields.MissingDocumentsNames as MissingDocsNames,
ShipmentComputedFields.RequestedDocumentsCount as RequestedDocumentsCount,
ShipmentComputedFields.IsRequestedDocuments as IsRequestedDocuments,
ShipmentComputedFields.IsDigitalSignRequired as IsDigitalSignRequired,
ShipmentComputedFields.IsDepositionRequired as IsDepositionRequired,
ShipmentComputedFields.CreatedFromDigital as CreatedFromDigital,
ShipmentComputedFields.ImporterDepositionRequestDetails as ImporterDepositionRequestDetails,
ShipmentComputedFields.NumberOfHouses as NumberOfHouses,
ShipmentAdditionalCloudDatas.DeclarationXmlData as DeclarationXmlData,
ShipmentAdditionalCloudDatas.IsImporterApprovalRequried as IsImporterApprovalRequried,
ShipmentAdditionalCloudDatas.ApprovedByUserName as ApprovedByUserName,
ShipmentAdditionalCloudDatas.ApproveDateTime as ApproveDateTime,
ShipmentAdditionalCloudDatas.VersionApproved as VersionApproved,
HybridPartner.Name as PartnerName,
HybridPartner.Id as ForwarderPartnerId,
dbo.ShipmentMasterDatas.DepartureArrivalFromDate as DepartureArrivalFromDate,
dbo.ShipmentMasterDatas.DepartureArrivalToDate as DepartureArrivalToDate,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationETA as MainCarriageFinalDestinationETA,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationATA as MainCarriageFinalDestinationATA,
CASE WHEN ( NOT ((dbo.ShipmentTypes.Name IS NULL) OR ((LEN(dbo.ShipmentTypes.Name)) = 0))) THEN CASE WHEN (dbo.ShipmentTypes.Name IS NULL) THEN N'''' ELSE dbo.ShipmentTypes.Name END + N'' '' + CASE WHEN (dbo.ShipmentLevels.Name IS NULL) THEN N'''' ELSE dbo.ShipmentLevels.Name END ELSE dbo.ShipmentLevels.Name END AS ShipmentType,
CASE WHEN ( NOT ((MainCarriageFromPortId IS NULL) OR ((LEN(MainCarriageFromPortId)) = 0))) THEN MainCarriageFromPortId ELSE dbo.Shipments.FromPortId END AS FromPortId,
CASE WHEN ( NOT ((MainCarriageToPortId IS NULL) OR ((LEN(MainCarriageToPortId)) = 0))) THEN MainCarriageToPortId ELSE dbo.Shipments.ToPortId END AS ToPortId,
CASE WHEN ( NOT ((MainCarriageFromPorts.Code IS NULL) OR ((LEN(MainCarriageFromPorts.Code)) = 0))) THEN MainCarriageFromPorts.Code ELSE FromPorts.Code END AS FromPort,
CASE WHEN ( NOT ((MainCarriageFromPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFromPorts.EnglishName)) = 0))) THEN MainCarriageFromPorts.EnglishName ELSE FromPorts.EnglishName END AS FromPortName,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.Code IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.Code)) = 0))) THEN MainCarriageFinalDestinationPorts.Code ELSE ToPorts.Code END AS ToPort,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.EnglishName)) = 0))) THEN MainCarriageFinalDestinationPorts.EnglishName ELSE ToPorts.EnglishName END AS ToPortName,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (MainCarriageCarrierCards.Code IS NULL) THEN N'''' ELSE MainCarriageCarrierCards.Code END + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''O'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (dbo.Vessels.EnglishName IS NULL) THEN N'''' ELSE dbo.Vessels.EnglishName END + N''/'' + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''I'' = dbo.Shipments.TransportModeId) THEN MainCarriageCarrierNumber END AS CarrierNumber,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN MainCarriageATA ELSE MainCarriageETA END AS MainCarriageExpectedOrActual,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN N''ATA'' ELSE N''ETA'' END AS MainCarriageETAOrATA,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusDate ELSE dbo.Shipments.StatusDate END ELSE dbo.Shipments.StatusDate END AS StatusDate,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Name ELSE dbo.EntityStatus.Name END ELSE dbo.EntityStatus.Name END AS StatusName,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusLocation ELSE dbo.Shipments.StatusLocation END ELSE dbo.Shipments.StatusLocation END AS StatusLocation,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (( NOT ((AirlinePrefix IS NULL) OR ((LEN(AirlinePrefix)) = 0))) AND ( NOT ((Master IS NULL) OR ((LEN(Master)) = 0)))) THEN CASE WHEN (AirlinePrefix IS NULL) THEN N'''' ELSE AirlinePrefix END + N''-'' + CASE WHEN (Master IS NULL) THEN N'''' ELSE Master END ELSE N'''' END ELSE Master END AS LongMaster,
CAST( MissingDocumentsCount AS nvarchar(max)) + N'' Missing'' AS MissingDocumentsCountWords,
CASE WHEN (IsOperationalClosed = 1) THEN N''Archived'' ELSE N'''' END AS ArchivedText
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageFromAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageToAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
--ELSE MainCarriageFinalDestinationPortsStates.EnglishName END) END AS MainCarriageToState
FROM            dbo.Shipments LEFT OUTER JOIN
dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId LEFT OUTER JOIN
dbo.Ports AS MainCarriageFromPorts ON dbo.ShipmentMasterDatas.MainCarriageFromPortId = MainCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageToPorts ON dbo.ShipmentMasterDatas.MainCarriageToPortId = MainCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1FromPorts ON dbo.ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1ToPorts ON dbo.ShipmentMasterDatas.Transshipment1ToPortId = Transshipment1ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2FromPorts ON dbo.ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2ToPorts ON dbo.ShipmentMasterDatas.Transshipment2ToPortId = Transshipment2ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3FromPorts ON dbo.ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3ToPorts ON dbo.ShipmentMasterDatas.Transshipment3ToPortId = Transshipment3ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageFinalDestinationPorts ON dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageFromPorts ON dbo.Shipments.PreCarriageFromPortId = PreCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageToPorts ON dbo.Shipments.PreCarriageToPortId = PreCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageFromPorts ON dbo.Shipments.OnCarriageFromPortId = OnCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageToPorts ON dbo.Shipments.OnCarriageToPortId = OnCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Shipments.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Shipments.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Cards AS CustomerCards ON dbo.Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsolidatorCards ON dbo.Shipments.ConsolidatorId = ConsolidatorCards.Id LEFT OUTER JOIN
dbo.Cards AS FreightForwarderCards ON dbo.Shipments.FreightForwarderId = FreightForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Shipments.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Shipments.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentCards ON dbo.Shipments.AgentId = AgentCards.Id LEFT OUTER JOIN
dbo.Cards AS ReleasingAgentCards ON dbo.Shipments.ReleasingAgentId = ReleasingAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentComputedCards ON dbo.Shipments.AgentComputed = AgentComputedCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentExportCards ON dbo.Shipments.CustomAgentExportId = CustomAgentExportCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentImportCards ON dbo.Shipments.CustomAgentImportId = CustomAgentImportCards.Id LEFT OUTER JOIN
dbo.Cards AS Notify1Cards ON dbo.Shipments.Notify1Id = Notify1Cards.Id LEFT OUTER JOIN
dbo.Cards AS Notify2Cards ON dbo.Shipments.Notify2Id = Notify2Cards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperNotExporterCards ON dbo.Shipments.ShipperNotExporterId = ShipperNotExporterCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeNotImporterCards ON dbo.Shipments.ConsigneeNotImporterId = ConsigneeNotImporterCards.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromCountries ON MainCarriageFromPorts.CountryId = MainCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToCountries ON MainCarriageToPorts.CountryId = MainCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1FromCountries ON Transshipment1FromPorts.CountryId = Transshipment1FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2FromCountries ON Transshipment2FromPorts.CountryId = Transshipment2FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3FromCountries ON Transshipment3FromPorts.CountryId = Transshipment3FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageFromCountries ON PreCarriageFromPorts.CountryId = PreCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageFromCountries ON OnCarriageFromPorts.CountryId = OnCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1ToCountries ON Transshipment1ToPorts.CountryId = Transshipment1ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2ToCountries ON Transshipment2ToPorts.CountryId = Transshipment2ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3ToCountries ON Transshipment3ToPorts.CountryId = Transshipment3ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageToCountries ON PreCarriageToPorts.CountryId = PreCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageToCountries ON OnCarriageToPorts.CountryId = OnCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS FinalDestinationCountries ON MainCarriageFinalDestinationPorts.CountryId = FinalDestinationCountries.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarrierCards ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment1CarrierCards ON dbo.ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment2CarrierCards ON dbo.ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment3CarrierCards ON dbo.ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS PreCarriageCarrierCards ON dbo.Shipments.PreCarriageCarrierId = PreCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS OnCarriageCarrierCards ON dbo.Shipments.OnCarriageCarrierId = OnCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS WarehouseLegCard ON dbo.Shipments.WarehouseLegWarehouseId = WarehouseLegCard.Id LEFT OUTER JOIN
dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
dbo.ShipmentSubTypes ON dbo.Shipments.ShipmentSubTypeId = dbo.ShipmentSubTypes.Id LEFT OUTER JOIN
dbo.MoveTypes ON dbo.Shipments.MoveTypeId = dbo.MoveTypes.Id LEFT OUTER JOIN
dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.CustomsTransmissionsStatus ON dbo.Shipments.LocalCustomsTransmissionsStatusCode = dbo.CustomsTransmissionsStatus.Code LEFT OUTER JOIN
dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
dbo.INTTRASIStatus ON dbo.Shipments.INTTRASIStatusCode = dbo.INTTRASIStatus.Code LEFT OUTER JOIN
dbo.INTTRABookingTransStatuses ON dbo.Shipments.INTTRABookingTransStatusCode = dbo.INTTRABookingTransStatuses.Code LEFT OUTER JOIN
dbo.INTTRABookingStatuses ON dbo.Shipments.INTTRABookingStatusCode = dbo.INTTRABookingStatuses.Code LEFT OUTER JOIN
dbo.Addresses AS MainCarriageFromAddresses ON dbo.ShipmentMasterDatas.MainCarriageFromAddressId = MainCarriageFromAddresses.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageToAddresses ON dbo.ShipmentMasterDatas.MainCarriageToAddressId = MainCarriageToAddresses.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageToPartners ON dbo.ShipmentMasterDatas.MainCarriageToPartnerId = MainCarriageToPartners.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageFromPartners ON dbo.ShipmentMasterDatas.MainCarriageFromPartnerId = MainCarriageFromPartners.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromAddressCountries ON MainCarriageFromAddresses.CountryId = MainCarriageFromAddressCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToAddressCountries ON MainCarriageToAddresses.CountryId = MainCarriageToAddressCountries.Id LEFT OUTER JOIN
dbo.SpecialServicesTypes ON dbo.Shipments.SpecialServicesTypeId = dbo.SpecialServicesTypes.Id LEFT OUTER JOIN
dbo.Vessels ON dbo.ShipmentMasterDatas.MainCarriageVesselId = dbo.Vessels.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Shipments.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContact ON dbo.Shipments.SalesmanUserId = SalesmanUserContact.Id LEFT OUTER JOIN
dbo.HybridPartners AS HybridPartner ON dbo.Shipments.ForwarderPartnerId = HybridPartner.Id LEFT OUTER JOIN
dbo.EventTypes ON dbo.Shipments.LastSharedEventId = dbo.EventTypes.Id LEFT OUTER JOIN
dbo.Contacts AS LastSentByUserContact ON dbo.Shipments.LastSentByUserId = LastSentByUserContact.Id LEFT OUTER JOIN
dbo.Contacts AS LocalCustomsSentByUser ON dbo.Shipments.LocalCustomsSentByUserId = LocalCustomsSentByUser.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContact ON dbo.Shipments.CreatedByUserId = CreatedByUserContact.Id INNER JOIN
dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id INNER JOIN
dbo.ShipmentAdditionalCloudDatas AS ShipmentAdditionalCloudDatas ON dbo.Shipments.Id = ShipmentAdditionalCloudDatas.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromAddressesStates ON MainCarriageFromAddresses.StateId = MainCarriageFromAddressesStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageToAddressesStates ON MainCarriageToAddresses.StateId = MainCarriageToAddressesStates.Id LEFT OUTER JOIN
dbo.States AS FromPortsStates ON FromPorts.StateId = FromPortsStates.Id  LEFT OUTER JOIN
dbo.States AS ToPortsStates ON ToPorts.StateId = ToPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromPortsStates ON MainCarriageFromPorts.StateId = MainCarriageFromPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id');


-- Procedure Script From usp_DeleteBusinessRecords.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteBusinessRecords]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteBusinessRecords] END');
EXEC('Create PROCEDURE [dbo].usp_DeleteBusinessRecords
(
@Tenant int
)
AS
BEGIN
update Activities set QuoteId = NULL where Tenant = @Tenant
update Shipments set MasterShipmentDataId = NULL where Tenant = @Tenant
delete from FollowUps where Tenant = @Tenant
delete from QuotePriceSteps where Tenant = @Tenant
delete from QuoteCharges where Tenant = @Tenant
delete from QuoteDocumentVersions where Tenant = @Tenant
delete from QuotePackages where Tenant = @Tenant
delete from QuoteTotalVATs where Tenant = @Tenant
delete from QuoteTemplateExcludedSections where Tenant = @Tenant
delete from Quotes where Tenant = @Tenant
delete from ShipmentCustomsTransmissions where Tenant = @Tenant
delete from ShipmentReceivables where Tenant= @Tenant
delete from ShipmentPayables where Tenant= @Tenant
delete from InsideShipmentPackages where Tenant = @Tenant
delete from ShipmentPackageHarmonize where Tenant = @Tenant
delete from ShipmentPackageItems where Tenant = @Tenant
delete from ShipmentPackages where Tenant= @Tenant
delete from ShipmentOrderPackages where Tenant= @Tenant
delete from PickUpDeliveryPackageHarmonizes where Tenant = @Tenant
delete from ShipmentPickUpDeliveryPackages where Tenant= @Tenant
delete from ShipmentPickUpDeliveries where Tenant= @Tenant
delete from ShipmentAWBPrintOnlies where Tenant= @Tenant
delete from ShipmentCarrierStatuses where Tenant= @Tenant
delete from AWBOCIs where Tenant= @Tenant
delete from ShipmentCommodities where Tenant= @Tenant
delete from ShipmentAssemblies where Tenant= @Tenant
delete from ShipmentReceivables where Tenant= @Tenant
delete from MessagingStockUsageHistories where Tenant = @Tenant
delete from ShipmentMasterDatas where Tenant = @Tenant
delete from ShipmentComputedFields where Tenant = @Tenant
delete from ShipmentAdditionalCloudDatas where Tenant = @Tenant
delete from ShipmentStoragePricings where Tenant= @Tenant
delete from WarehouseEntryPackagesReleases where Tenant = @Tenant
delete from WarehouseEntryPackages where Tenant = @Tenant
delete from WarehouseReleasePackages where Tenant = @Tenant
delete from WarehouseEntries where Tenant = @Tenant
delete from WarehouseReleases where Tenant = @Tenant
delete from Shipments where Tenant = @Tenant
delete from ARInvoiceLines where Tenant = @Tenant
delete from ARInvoiceEntities where Tenant = @Tenant
delete from ARInvoicePayments where Tenant = @Tenant
delete from ARInvoiceTotalVATs where Tenant = @Tenant
delete from ARInvoices where Tenant = @Tenant
delete from ARPayments where Tenant = @Tenant
delete from APInvoiceLines where Tenant = @Tenant
delete from APInvoiceEntities where Tenant = @Tenant
delete from APInvoicePayments where Tenant = @Tenant
delete from APInvoiceTotalVATs where Tenant = @Tenant
delete from APInvoices where Tenant = @Tenant
delete from APPayments where Tenant = @Tenant
END');


-- Procedure Script From usp_UpdateCustomerActualData.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCustomerActualData]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCustomerActualData] END');
EXEC('create PROCEDURE [dbo].[usp_UpdateCustomerActualData]
(
@CustomerId_PARAM varchar(15) = null,
@Tenant int
)
AS
-- Select the Firt day of the current date
-- in order to get date of the last month and bellow
declare @DateOfFirstDayOfCurrentDate as datetime
set @DateOfFirstDayOfCurrentDate = DATEADD(m, DATEDIFF(m, 0, GETDATE()), 0)
DECLARE @CustomerId AS varchar(15)
DECLARE @TypeCode AS varchar(2)
DECLARE @CountryId AS varchar(15)
DECLARE @Year AS int
DECLARE @Month AS int
DECLARE @TEU AS float
DECLARE @Revenue AS float
DECLARE @ChargeableWeight AS float
DECLARE @NumberOfShipments AS int
DECLARE @LastDate as datetime
declare @MemoryTable table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null,
CountryId varchar(15) null,
LastShipmentDate datetime not null
)
declare @MemoryTable_Customers table
(
Id varchar(15) not null
)
declare @MemoryTable_ActualData table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null
)
declare @MemoryTable_LocationActualData table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
Year int not null,
Month int not null,
TEU decimal(18, 2) not null,
Revenue decimal(18, 2) not null,
ChargeableWeight decimal(18, 2) not null,
NumberOfShipments int not null,
CountryId varchar(15) not null
)
declare @MemoryTable_LastShipmentDate table
(
CustomerId varchar(15) not null,
ProductCode varchar(2) not null,
LastShipmentDate datetime not null
)
declare @MemoryTable_CustomersLastShipmentDate table
(
CustomerId varchar(15) not null,
LastShipmentDate datetime not null
)
-- 1) Select Memory Data + Reset Actual Data
BEGIN
if (@CustomerId_PARAM is null)
BEGIN
BEGIN
insert into @MemoryTable
SELECT
CustomerId,
ProductCode,
Year(CreateDateTime),
Month(CreateDateTime),
sum(isnull(TEU,0)),
sum(ISNULL(OpenReceivablesInProfitCurrency,0) + ISNULL(AccountedReceivablesInProfitCurrency,0)),
sum(isnull(ChargeableWeightInKG,0)),
count(*),
CountryForStatisticsId,
max(CreateDateTime)
From Shipments
Where CreateDateTime >= ''2020-01-01'' and IsCancelled = 0 AND Tenant = @Tenant AND ProductCode is not null AND CustomerId is not null
group by CustomerId, ProductCode, Month(CreateDateTime), Year(CreateDateTime), CountryForStatisticsId
END
BEGIN
--insert into @MemoryTable_Customers
--SELECT
--Id
--From Customers
--Where Tenant = @Tenant
--END
--BEGIN
update CustomerProductActualDatas
set
TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant and Year = 2020
END
BEGIN
update CustomerProductLocationActualDatas
set
TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant and Year = 2020
END
END
else
BEGIN
BEGIN
insert into @MemoryTable
SELECT
CustomerId,
ProductCode,
Year(CreateDateTime),
Month(CreateDateTime),
sum(isnull(TEU,0)),
sum(ISNULL(OpenReceivablesInProfitCurrency,0) + ISNULL(AccountedReceivablesInProfitCurrency,0)),
sum(isnull(ChargeableWeightInKG,0)),
count(*),
CountryForStatisticsId,
max(CreateDateTime)
From Shipments
Where IsCancelled = 0 AND Tenant = @Tenant AND ProductCode is not null AND CustomerId = @CustomerId_PARAM
group by CustomerId, ProductCode, Month(CreateDateTime), Year(CreateDateTime), CountryForStatisticsId
END
--BEGIN
--insert into @MemoryTable_Customers
--SELECT
--Id
--From Customers
--Where Tenant = @Tenant AND Id = @CustomerId_PARAM
--END
BEGIN
update CustomerProductActualDatas
set TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant AND CustomerId = @CustomerId_PARAM and Year = 2020
END
BEGIN
update CustomerProductLocationActualDatas
set TEU = 0,
Revenue = 0,
ChargeableWeight = 0,
NumberOfShipments = 0
where Tenant = @Tenant AND CustomerId = @CustomerId_PARAM and Year = 2020
END
END
END
-- 2) Select Actual Data
BEGIN
insert into @MemoryTable_ActualData
SELECT
CustomerId,
ProductCode,
Year,
Month,
sum(isnull(TEU,0)),
sum(isnull(Revenue,0)),
sum(isnull(ChargeableWeight,0)),
sum(isnull(NumberOfShipments,0))
From @MemoryTable
where (DATEADD(year, Year-1900, DATEADD(month, Month-1, DATEADD(day, 20-1, 0)))) < @DateOfFirstDayOfCurrentDate
group by CustomerId, ProductCode, Month, Year
END
-- 3) Select Location Actual Data
BEGIN
insert into @MemoryTable_LocationActualData
SELECT
CustomerId,
ProductCode,
Year,
Month,
sum(isnull(TEU,0)),
sum(isnull(Revenue,0)),
sum(isnull(ChargeableWeight,0)),
sum(isnull(NumberOfShipments,0)),
CountryId
From @MemoryTable
Where CountryId is not null AND (DATEADD(year, Year-1900, DATEADD(month, Month-1, DATEADD(day, 20-1, 0)))) < @DateOfFirstDayOfCurrentDate
group by CustomerId, ProductCode, Month, Year, CountryId
END
-- 4) Select Last Date _ Customer Products
BEGIN
insert into @MemoryTable_LastShipmentDate
SELECT
CustomerId,
ProductCode,
max(LastShipmentDate)
From @MemoryTable
group by CustomerId, ProductCode
END
BEGIN
insert into @MemoryTable_CustomersLastShipmentDate
SELECT
CustomerId,
max(LastShipmentDate)
From @MemoryTable
group by CustomerId
END
-- 5) Update Product ActualDatas
BEGIN
DECLARE DataCursor1 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments
From @MemoryTable_ActualData
OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from Customers where Id = @CustomerId)
BEGIN
IF exists (
select * from CustomerProductActualDatas
where
Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
)
BEGIN
UPDATE CustomerProductActualDatas
set
TEU = isnull(@TEU,0),
Revenue = isnull(@Revenue,0),
ChargeableWeight =isnull(@ChargeableWeight,0),
NumberOfShipments = isnull(@NumberOfShipments,0)
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
END
else
BEGIN
INSERT INTO CustomerProductActualDatas(CustomerId, Tenant, ProductTypeCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments)
VALUES
(
@CustomerId,
@Tenant,
@TypeCode,
@Year,
@Month,
isnull(@TEU,0),
isnull(@Revenue,0),
isnull(@ChargeableWeight,0),
isnull(@NumberOfShipments,0)
)
END
if not exists (select * from CustomerProducts where CustomerId = @CustomerId AND Tenant = @Tenant AND ProductTypeCode = @TypeCode)
begin
insert into CustomerProducts(CustomerId, ProductTypeCode,Tenant) values(@CustomerId,@TypeCode,@Tenant)
end
END
FETCH NEXT FROM DataCursor1 INTO @CustomerId, @TypeCode,@Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments
END
CLOSE DataCursor1
DEALLOCATE DataCursor1
END
-- 6) Update Location ActualDatas
BEGIN
DECLARE DataCursor2 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments, CountryId
From @MemoryTable_LocationActualData
OPEN DataCursor2 FETCH NEXT FROM DataCursor2 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments, @CountryId
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from Customers where Id = @CustomerId)
BEGIN
IF EXISTS (
select * from CustomerProductLocationActualDatas
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
AND CountryId = @CountryId
)
BEGIN
UPDATE CustomerProductLocationActualDatas
set
TEU = isnull(@TEU,0),
Revenue = isnull(@Revenue,0),
ChargeableWeight =isnull(@ChargeableWeight,0),
NumberOfShipments = isnull(@NumberOfShipments,0)
where Tenant = @Tenant
AND CustomerId = @CustomerId
AND ProductTypeCode = @TypeCode
AND Year = @Year
AND Month = @Month
AND CountryId = @CountryId
END
else
BEGIN
INSERT INTO CustomerProductLocationActualDatas(CustomerId, Tenant, ProductTypeCode, Year, Month, TEU, Revenue, ChargeableWeight, NumberOfShipments,CountryId)
VALUES
(
@CustomerId,
@Tenant,
@TypeCode,
@Year,
@Month,
isnull(@TEU,0),
isnull(@Revenue,0),
isnull(@ChargeableWeight,0),
isnull(@NumberOfShipments,0),
@CountryId
)
END
if not exists (select * from CustomerProductLocations where CustomerId = @CustomerId and ProductTypeCode = @TypeCode and CountryId  = @CountryId and Tenant = @Tenant)
begin
insert into CustomerProductLocations(CountryId, CustomerId, ProductTypeCode, Tenant) values(@CountryId, @CustomerId, @TypeCode,@Tenant)
end
END
FETCH NEXT FROM DataCursor2 INTO @CustomerId, @TypeCode, @Year, @Month, @TEU, @Revenue, @ChargeableWeight, @NumberOfShipments, @CountryId
END
CLOSE DataCursor2
DEALLOCATE DataCursor2
END
-- 7) Update Products Last Shipment Date
BEGIN
DECLARE DataCursor3 CURSOR READ_ONLY
FOR
SELECT CustomerId, ProductCode, LastShipmentDate
From @MemoryTable_LastShipmentDate
OPEN DataCursor3 FETCH NEXT FROM DataCursor3 INTO @CustomerId, @TypeCode, @LastDate
WHILE @@FETCH_STATUS = 0
BEGIN
if exists (select * from Customers where Id = @CustomerId)
BEGIN
update CustomerProducts
set LastShipmentDate = @LastDate
where Tenant = @Tenant
and CustomerId = @CustomerId
and ProductTypeCode = @TypeCode
END
FETCH NEXT FROM DataCursor3 INTO @CustomerId, @TypeCode, @LastDate
END
CLOSE DataCursor3
DEALLOCATE DataCursor3
END
-- 8) Update Customers Last Shipment Date
BEGIN
DECLARE DataCursor4 CURSOR READ_ONLY
FOR
SELECT CustomerId, LastShipmentDate
From @MemoryTable_CustomersLastShipmentDate
OPEN DataCursor4 FETCH NEXT FROM DataCursor4 INTO @CustomerId, @LastDate
WHILE @@FETCH_STATUS = 0
BEGIN
update Customers
set LastShipmentDate = @LastDate
where Tenant = @Tenant
and Id = @CustomerId
FETCH NEXT FROM DataCursor4 INTO @CustomerId, @LastDate
END
CLOSE DataCursor4
DEALLOCATE DataCursor4
END');


-- General Script From BuildSearchKeywordFunction.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N'[dbo].[BuildSearchKeywordFunction]'))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = 'CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='' '') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('' '', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('' '', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='' '')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='' '')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END'
EXEC(@dateString)
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '598fa43f8677d92057a73ea7ffba534c', [Version] = 2 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051322_UpdateNotAirShipmentsSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
-- If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
--      Begin
--        Drop Table #tempTable
--      End
--      If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
--      Begin
--        Drop Table #temp_Shipments
--      End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    ShipmentSubTypeId varchar(15)  null,
--	ShipmentTypeId varchar(15)  null
--    )
--	select
--	Id,
--	Tenant,
--	TransportModeId,
--	ShipmentTypeId,
--	(
--		CASE
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'O' and ShipmentTypeId = 'MyGO' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGO' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = 'I' and ShipmentTypeId = 'MyGI' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGI' and Tenant = Shipments.Tenant)
--		END
--	 ) as ShipmentSubTypeId
--	into #tempTable
--	FROM Shipments where TransportModeId <> 'A'
--	  declare @Tenant as int
--      declare @EntityId as varchar(15)
--	  declare @ShipmentTypeId as varchar(4)
--	  declare @TransportModeId as varchar(4)
--	  declare @ShipmentSubTypeId as varchar(15)
--	  declare @Count as int
--      set @Count = 0;
--    BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--       		insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--	  drop table #tempTable
--      drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051322_UpdateNotAirShipmentsSubType.sxml', GETDATE(), '-- If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
--      Begin
--        Drop Table #tempTable
--      End
--      If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
--      Begin
--        Drop Table #temp_Shipments
--      End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    ShipmentSubTypeId varchar(15)  null,
--	ShipmentTypeId varchar(15)  null
--    )
--	select
--	Id,
--	Tenant,
--	TransportModeId,
--	ShipmentTypeId,
--	(
--		CASE
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''O'' and ShipmentTypeId = ''MyGO'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGO'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Shipments.Tenant)
--			WHEN TransportModeId = ''I'' and ShipmentTypeId = ''MyGI'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGI'' and Tenant = Shipments.Tenant)
--		END
--	 ) as ShipmentSubTypeId
--	into #tempTable
--	FROM Shipments where TransportModeId <> ''A''
--	  declare @Tenant as int
--      declare @EntityId as varchar(15)
--	  declare @ShipmentTypeId as varchar(4)
--	  declare @TransportModeId as varchar(4)
--	  declare @ShipmentSubTypeId as varchar(15)
--	  declare @Count as int
--      set @Count = 0;
--    BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--       		insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
--				ShipmentTypeId = #temp_Shipments.ShipmentTypeId
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--	  drop table #tempTable
--      drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), 'aa0b6efe01efd811137af38dc287e4ad', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009230840_AddNewSpecialHandlingCode.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Id as varchar(15)
if not exists (select Id from AWBSpecialHandlingCodes where Code = 'NSC')
begin
EXECUTE usp_GetNextTableIdValue @Id OUTPUT,'AWBSpecialHandlingCode'
insert into AWBSpecialHandlingCodes(Code, Name, SearchFields, IsIATA, AirlineId, Id, InActive)
values('NSC', 'Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft', 'NSC,Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft', 1, NULL, @Id, 0)
end
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'AWBSpecialHandlingCode')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009230840_AddNewSpecialHandlingCode.sxml', GETDATE(), 'declare @Id as varchar(15)
if not exists (select Id from AWBSpecialHandlingCodes where Code = ''NSC'')
begin
EXECUTE usp_GetNextTableIdValue @Id OUTPUT,''AWBSpecialHandlingCode''
insert into AWBSpecialHandlingCodes(Code, Name, SearchFields, IsIATA, AirlineId, Id, InActive)
values(''NSC'', ''Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft'', ''NSC,Cargo Has Not Been Secured Yet for Passenger or All-Cargo Aircraft'', 1, NULL, @Id, 0)
end
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''AWBSpecialHandlingCode'')', DATEDIFF(MS,@StartTime,@EndTime), 'd445435d375c617293df430dadb953d2', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009281700_MapValuesFromIsBondedToIsCFS.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009281700_MapValuesFromIsBondedToIsCFS.sxml', GETDATE(), 'update Shipments set IsCFSWarehouse = IsBondedWarehouse, IsCFSWarehouseChanged = IsBondedWarehouseChanged
where IsBondedWarehouse = 1', DATEDIFF(MS,@StartTime,@EndTime), '65f6bddc686b4dc02b7b0ad7d82ce96d', 3);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

