-- General Script From FillShipmentSearchFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
SearchFields nvarchar(4000)  null,
)
select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
into #tempTable
FROM Shipments
SET NOCOUNT ON
declare @MySearchFields as nvarchar(4000)
declare @PortsTable table
(
Id varchar(15) not null
)
declare @PartnersTable table
(
Id varchar(15) not null
)
declare @ReferencesTable table
(
Reference varchar(50) not null
)
-- Shipment fields
BEGIN
declare @Id as varchar(15)
declare @Tenant as int
declare @ShipmentNumber as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @TransportModeId as varchar(1)
declare @DirectionId as varchar(1)
declare @StatusId as varchar(15)
declare @StatusCode as varchar(4)
declare @StatusName as varchar(40)
declare @QuoteId as varchar(15)
declare @QuoteNumber as varchar(15)
declare @SalesmanUserId as varchar(15)
declare @SalesmanUserName as varchar(60)
declare @MasterShipmentDataId as varchar(15)
declare @House as varchar(20)
declare @CustomFileNumber as varchar(15)
declare @AWBCarrierTarrifReference as varchar(25)
declare @CustomsDeclarationNumber as varchar(35)
declare @ForwarderShipmentNumber as varchar(15)
declare @TransportDocumentNumber as varchar(50)
declare @ImportManifest as varchar(50)
declare @BookingConfirmationNumber as varchar(25)
declare @CarrierTransportDocumentNumber as varchar(50)
declare @ProjectNumber as varchar(100)
declare @AMSBL as nvarchar(17)
declare @WarehouseLegReference as nvarchar(50)
END
-- Ports Firlds
BEGIN
declare @PortId as varchar(15)
declare @PortCode as varchar(3)
declare @PortName as varchar(40)
declare @PortCountryCode as varchar(2)
declare @PortCountryName as varchar(120)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @PreCarriageFromPortId as varchar(15)
declare @PreCarriageToPortId as varchar(15)
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @MainCarriageFinalDestinationPortId as varchar(15)
END
-- Partners Fields
BEGIN
declare @PartnerId as varchar(15)
declare @PartnerName as varchar(60)
declare @CityName as varchar(120)
declare @PartnerReference1 as varchar(50)
declare @PartnerReference2 as varchar(50)
declare @AgentId as varchar(15)
declare @AgentReference1 as varchar(50)
declare @AgentReference2 as varchar(50)
declare @ShipperId as varchar(15)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)
declare @ConsigneeId as varchar(15)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)
declare @CustomerId as varchar(15)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)
declare @Notify1Id as varchar(15)
declare @Notify2Id as varchar(15)
declare @IssuingCarrierAgentId as varchar(15)
declare @CustomAgentImportId as varchar(15)
declare @CustomAgentImportReference as varchar(50)
declare @CustomAgentExportId as varchar(15)
declare @CustomAgentExportReference as varchar(50)
declare @ShipperNotExporterId as varchar(15)
declare @ConsigneeNotImporterId as varchar(15)
declare @FreightForwarderId as varchar(15)
declare @FreightForwarderReference as varchar(50)
declare @ConsolidatorId as varchar(15)
declare @ConsolidatorReference as varchar(50)
declare @ReleasingAgentId as varchar(15)
declare @ReleasingAgentReference1 as varchar(50)
declare @ReleasingAgentReference2 as varchar(50)
declare @MainCarriageFromAddressId as varchar(50)
declare @MainCarriageToAddressId as varchar(50)
END
-- MasterData Fields
BEGIN
declare @Master as varchar(20)
declare @LongMaster as varchar(30)
declare @MasterShipmentNumber as varchar(15)
declare @MainCarriageVesselId as varchar(15)
declare @MainCarriageVesselCode as varchar(5)
declare @MainCarriageVesselName as varchar(40)
declare @MainCarriageCarrierId as varchar(15)
declare @MainCarriageCarrierCode as varchar(15)
declare @MainCarriageCarrierName as varchar(60)
declare @MainCarriageCarrierPrefix as varchar(3)
declare @MainCarriageCarrierNumber as varchar(15)
declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
END
-- Custom Fields
BEGIN
declare @Field nvarchar(250)
declare @FieldName varchar(10)
declare @FieldDataTypeCode as varchar(10)
declare @Field1 nvarchar(250)
declare @Field2 nvarchar(250)
declare @Field3 nvarchar(250)
declare @Field4 nvarchar(250)
declare @Field5 nvarchar(250)
declare @Field6 nvarchar(250)
declare @Field7 nvarchar(250)
declare @Field8 nvarchar(250)
declare @Field9 nvarchar(250)
declare @Field10 nvarchar(250)
END
declare @ARInvoiceId as varchar(20)
declare @ARInvoiceNumber as varchar(20)
declare @ARInvoiceDraftNumber as varchar(20)
declare @ContainerNumber as varchar(20)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
WHILE @@FETCH_STATUS = 0
BEGIN
set @MySearchFields = ''
delete from @PortsTable
delete from @PartnersTable
delete from @ReferencesTable
-- Master Data
BEGIN
if (@MasterShipmentDataId is not null)
BEGIN
select
@Master = Master,
@MasterShipmentNumber = MasterShipmentNumber,
@MainCarriageVesselId = MainCarriageVesselId,
@MainCarriageCarrierId  = MainCarriageCarrierId,
@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
@MainCarriageFromPortId = MainCarriageFromPortId,
@MainCarriageToPortId = MainCarriageToPortId,
@Transshipment1FromPortId = Transshipment1FromPortId,
@Transshipment2FromPortId = Transshipment2FromPortId,
@Transshipment3FromPortId = Transshipment3FromPortId,
@Transshipment1ToPortId = Transshipment1ToPortId,
@Transshipment2ToPortId = Transshipment2ToPortId,
@Transshipment3ToPortId = Transshipment3ToPortId,
@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
@ImportManifest = ImportManifest,
@BookingConfirmationNumber = BookingConfirmationNumber,
@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
@MainCarriageFromAddressId = MainCarriageFromAddressId,
@MainCarriageToAddressId = MainCarriageToAddressId
from ShipmentMasterDatas
where Id = @MasterShipmentDataId AND Tenant = @Tenant
END
END
-- Fields
BEGIN
if (@ProjectNumber is not null AND @ProjectNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ProjectNumber
else set @MySearchFields = @MySearchFields + ',' + @ProjectNumber
end
if (@House is not null AND @House <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @House
else set @MySearchFields = @MySearchFields + ',' + @House
end
if (@Master is not null AND @Master <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Master
else set @MySearchFields = @MySearchFields + ',' + @Master
end
if (@ShipmentNumber is not null AND @ShipmentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ShipmentNumber
else set @MySearchFields = @MySearchFields + ',' + @ShipmentNumber
end
if (@CustomFileNumber is not null AND @CustomFileNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CustomFileNumber
else set @MySearchFields = @MySearchFields + ',' + @CustomFileNumber
end
if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @AWBCarrierTarrifReference
else set @MySearchFields = @MySearchFields + ',' + @AWBCarrierTarrifReference
end
if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CustomsDeclarationNumber
else set @MySearchFields = @MySearchFields + ',' + @CustomsDeclarationNumber
end
if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ForwarderShipmentNumber
else set @MySearchFields = @MySearchFields + ',' + @ForwarderShipmentNumber
end
if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @TransportDocumentNumber
else set @MySearchFields = @MySearchFields + ',' + @TransportDocumentNumber
end
if (@ImportManifest is not null AND @ImportManifest <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @ImportManifest
else set @MySearchFields = @MySearchFields + ',' + @ImportManifest
end
if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @BookingConfirmationNumber
else set @MySearchFields = @MySearchFields + ',' + @BookingConfirmationNumber
end
if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @CarrierTransportDocumentNumber
else set @MySearchFields = @MySearchFields + ',' + @CarrierTransportDocumentNumber
end
if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment1AdditionalMAWBOBLBL
end
if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment2AdditionalMAWBOBLBL
end
if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + ',' + @Transshipment3AdditionalMAWBOBLBL
end
if (@QuoteId is not null)
begin
set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
if (@QuoteNumber is not null AND @QuoteNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @QuoteNumber
else set @MySearchFields = @MySearchFields + ',' + @QuoteNumber
end
end
if (@StatusId is not null)
begin
select
@StatusCode = Code,
@StatusName = Name
from EntityStatus
where Id = @StatusId AND Tenant = @Tenant
if (@StatusCode is not null AND @StatusCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @StatusCode
else set @MySearchFields = @MySearchFields + ',' + @StatusCode
end
if (@StatusName is not null AND @StatusName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @StatusName
else set @MySearchFields = @MySearchFields + ',' + @StatusName
end
end
if (@MainCarriageVesselId is not null)
begin
select
@MainCarriageVesselCode = Code,
@MainCarriageVesselName = EnglishName
from Vessels
where Id = @MainCarriageVesselId AND Tenant = @Tenant
if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselCode
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselCode
end
if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselName
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselName
end
end
if (@MainCarriageCarrierId is not null)
begin
select
@MainCarriageCarrierCode = Cards.Code,
@MainCarriageCarrierName = Cards.EnglishName,
@MainCarriageCarrierPrefix = Airlines.Prefix
from Airlines join Cards on Airlines.Id = Cards.Id
where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
if (@TransportModeId = 'A' AND @Master is not null AND @Master <> '')
begin
set @LongMaster = @MainCarriageCarrierPrefix + '-' + @Master
if (@LongMaster is not null AND @LongMaster <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @LongMaster
else set @MySearchFields = @MySearchFields + ',' + @LongMaster
end
end
if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierCode
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierCode
end
if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierName
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierName
end
if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierNumber
else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierNumber
end
end
if (@SalesmanUserId is not null)
begin
set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
if (@SalesmanUserName is not null AND @SalesmanUserName <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @SalesmanUserName
else set @MySearchFields = @MySearchFields + ',' + @SalesmanUserName
end
end
if (@AMSBL is not null AND @AMSBL <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @AMSBL
else set @MySearchFields = @MySearchFields + ',' + @AMSBL
end
if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '')
begin
if (@MySearchFields = '') set @MySearchFields = @WarehouseLegReference
else set @MySearchFields = @MySearchFields + ',' + @WarehouseLegReference
end
END
-- Ports
BEGIN
set @PortId = @FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @PreCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @PreCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @OnCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @OnCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment1FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment1ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment2FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment2ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment3FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @Transshipment3ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
set @PortId = @MainCarriageFinalDestinationPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + ',' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + ',' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
end
end
END
-- Partners
BEGIN
set @PartnerId = @AgentId
set @PartnerReference1 = @AgentReference1
set @PartnerReference2 = @AgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ShipperId
set @PartnerReference1 = @ShipperReference1
set @PartnerReference2 = @ShipperReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeId
set @PartnerReference1 = @ConsigneeReference1
set @PartnerReference2 = @ConsigneeReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomerId
set @PartnerReference1 = @CustomerReference1
set @PartnerReference2 = @CustomerReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @Notify1Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @Notify2Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @IssuingCarrierAgentId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentImportId
set @PartnerReference1 = @CustomAgentImportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentExportId
set @PartnerReference1 = @CustomAgentExportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ShipperNotExporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeNotImporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @FreightForwarderId
set @PartnerReference1 = @FreightForwarderReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ConsolidatorId
set @PartnerReference1 = @ConsolidatorReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
set @PartnerId = @ReleasingAgentId
set @PartnerReference1 = @ReleasingAgentReference1
set @PartnerReference2 = @ReleasingAgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + ',' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
end
end
if(@TransportModeId = 'I' and @DirectionId ='D' and @MasterShipmentDataId is not null)
begin
set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '')
begin
if (@MySearchFields = '') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + ',' + @CityName
end
set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '')
begin
if (@MySearchFields = '') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + ',' + @CityName
end
end
END
-- Invoices
if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
BEGIN
DECLARE ARInvoicesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
begin
if (@MySearchFields = '') set @MySearchFields = @ARInvoiceNumber
else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceNumber
end
else if (@ARInvoiceDraftNumber is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @ARInvoiceDraftNumber
else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceDraftNumber
end
FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
END
CLOSE ARInvoicesCursor
DEALLOCATE ARInvoicesCursor
END
-- Packages Containers
if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
BEGIN
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT ContainerNumber
FROM ShipmentPackages
Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''
group by ContainerNumber
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ContainerNumber is not null)
begin
if (@MySearchFields = '') set @MySearchFields = @ContainerNumber
else set @MySearchFields = @MySearchFields + ',' + @ContainerNumber
end
FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor
END
-- Custom Fields
BEGIN
set @Field = @Field1
set @FieldName = 'Field1'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field2
set @FieldName = 'Field2'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field3
set @FieldName = 'Field3'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field4
set @FieldName = 'Field4'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field5
set @FieldName = 'Field5'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field6
set @FieldName = 'Field6'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field7
set @FieldName = 'Field7'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field8
set @FieldName = 'Field8'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field9
set @FieldName = 'Field9'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
set @Field = @Field10
set @FieldName = 'Field10'
if (@Field is not null AND @Field <> '')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
begin
if (@MySearchFields = '') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + ',' + @Field
end
end
END
insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
SET NOCOUNT OFF
drop table #tempTable
drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FillShipmentSearchFields.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
SearchFields nvarchar(4000)  null,
)
select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
into #tempTable
FROM Shipments
SET NOCOUNT ON
declare @MySearchFields as nvarchar(4000)
declare @PortsTable table
(
Id varchar(15) not null
)
declare @PartnersTable table
(
Id varchar(15) not null
)
declare @ReferencesTable table
(
Reference varchar(50) not null
)
-- Shipment fields
BEGIN
declare @Id as varchar(15)
declare @Tenant as int
declare @ShipmentNumber as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @TransportModeId as varchar(1)
declare @DirectionId as varchar(1)
declare @StatusId as varchar(15)
declare @StatusCode as varchar(4)
declare @StatusName as varchar(40)
declare @QuoteId as varchar(15)
declare @QuoteNumber as varchar(15)
declare @SalesmanUserId as varchar(15)
declare @SalesmanUserName as varchar(60)
declare @MasterShipmentDataId as varchar(15)
declare @House as varchar(20)
declare @CustomFileNumber as varchar(15)
declare @AWBCarrierTarrifReference as varchar(25)
declare @CustomsDeclarationNumber as varchar(35)
declare @ForwarderShipmentNumber as varchar(15)
declare @TransportDocumentNumber as varchar(50)
declare @ImportManifest as varchar(50)
declare @BookingConfirmationNumber as varchar(25)
declare @CarrierTransportDocumentNumber as varchar(50)
declare @ProjectNumber as varchar(100)
declare @AMSBL as nvarchar(17)
declare @WarehouseLegReference as nvarchar(50)
END
-- Ports Firlds
BEGIN
declare @PortId as varchar(15)
declare @PortCode as varchar(3)
declare @PortName as varchar(40)
declare @PortCountryCode as varchar(2)
declare @PortCountryName as varchar(120)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @PreCarriageFromPortId as varchar(15)
declare @PreCarriageToPortId as varchar(15)
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @MainCarriageFinalDestinationPortId as varchar(15)
END
-- Partners Fields
BEGIN
declare @PartnerId as varchar(15)
declare @PartnerName as varchar(60)
declare @CityName as varchar(120)
declare @PartnerReference1 as varchar(50)
declare @PartnerReference2 as varchar(50)
declare @AgentId as varchar(15)
declare @AgentReference1 as varchar(50)
declare @AgentReference2 as varchar(50)
declare @ShipperId as varchar(15)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)
declare @ConsigneeId as varchar(15)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)
declare @CustomerId as varchar(15)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)
declare @Notify1Id as varchar(15)
declare @Notify2Id as varchar(15)
declare @IssuingCarrierAgentId as varchar(15)
declare @CustomAgentImportId as varchar(15)
declare @CustomAgentImportReference as varchar(50)
declare @CustomAgentExportId as varchar(15)
declare @CustomAgentExportReference as varchar(50)
declare @ShipperNotExporterId as varchar(15)
declare @ConsigneeNotImporterId as varchar(15)
declare @FreightForwarderId as varchar(15)
declare @FreightForwarderReference as varchar(50)
declare @ConsolidatorId as varchar(15)
declare @ConsolidatorReference as varchar(50)
declare @ReleasingAgentId as varchar(15)
declare @ReleasingAgentReference1 as varchar(50)
declare @ReleasingAgentReference2 as varchar(50)
declare @MainCarriageFromAddressId as varchar(50)
declare @MainCarriageToAddressId as varchar(50)
END
-- MasterData Fields
BEGIN
declare @Master as varchar(20)
declare @LongMaster as varchar(30)
declare @MasterShipmentNumber as varchar(15)
declare @MainCarriageVesselId as varchar(15)
declare @MainCarriageVesselCode as varchar(5)
declare @MainCarriageVesselName as varchar(40)
declare @MainCarriageCarrierId as varchar(15)
declare @MainCarriageCarrierCode as varchar(15)
declare @MainCarriageCarrierName as varchar(60)
declare @MainCarriageCarrierPrefix as varchar(3)
declare @MainCarriageCarrierNumber as varchar(15)
declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
END
-- Custom Fields
BEGIN
declare @Field nvarchar(250)
declare @FieldName varchar(10)
declare @FieldDataTypeCode as varchar(10)
declare @Field1 nvarchar(250)
declare @Field2 nvarchar(250)
declare @Field3 nvarchar(250)
declare @Field4 nvarchar(250)
declare @Field5 nvarchar(250)
declare @Field6 nvarchar(250)
declare @Field7 nvarchar(250)
declare @Field8 nvarchar(250)
declare @Field9 nvarchar(250)
declare @Field10 nvarchar(250)
END
declare @ARInvoiceId as varchar(20)
declare @ARInvoiceNumber as varchar(20)
declare @ARInvoiceDraftNumber as varchar(20)
declare @ContainerNumber as varchar(20)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
OnCarriageFromPortId, OnCarriageToPortId,
House, CustomFileNumber, AWBCarrierTarrifReference,
AgentId, AgentReference1, AgentReference2,
ShipperId, ShipperReference1, ShipperReference2,
ConsigneeId, ConsigneeReference1, ConsigneeReference2,
CustomerId, CustomerReference1, CustomerReference2,
Notify1Id, Notify2Id, IssuingCarrierAgentId,
CustomAgentImportId, CustomAgentImportReference,
CustomAgentExportId, CustomAgentExportReference,
ShipperNotExporterId, ConsigneeNotImporterId,
FreightForwarderId, FreightForwarderReference,
ConsolidatorId, ConsolidatorReference,
Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
AMSBL, WarehouseLegReference
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
WHILE @@FETCH_STATUS = 0
BEGIN
set @MySearchFields = ''''
delete from @PortsTable
delete from @PartnersTable
delete from @ReferencesTable
-- Master Data
BEGIN
if (@MasterShipmentDataId is not null)
BEGIN
select
@Master = Master,
@MasterShipmentNumber = MasterShipmentNumber,
@MainCarriageVesselId = MainCarriageVesselId,
@MainCarriageCarrierId  = MainCarriageCarrierId,
@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
@MainCarriageFromPortId = MainCarriageFromPortId,
@MainCarriageToPortId = MainCarriageToPortId,
@Transshipment1FromPortId = Transshipment1FromPortId,
@Transshipment2FromPortId = Transshipment2FromPortId,
@Transshipment3FromPortId = Transshipment3FromPortId,
@Transshipment1ToPortId = Transshipment1ToPortId,
@Transshipment2ToPortId = Transshipment2ToPortId,
@Transshipment3ToPortId = Transshipment3ToPortId,
@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
@ImportManifest = ImportManifest,
@BookingConfirmationNumber = BookingConfirmationNumber,
@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
@MainCarriageFromAddressId = MainCarriageFromAddressId,
@MainCarriageToAddressId = MainCarriageToAddressId
from ShipmentMasterDatas
where Id = @MasterShipmentDataId AND Tenant = @Tenant
END
END
-- Fields
BEGIN
if (@ProjectNumber is not null AND @ProjectNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ProjectNumber
else set @MySearchFields = @MySearchFields + '','' + @ProjectNumber
end
if (@House is not null AND @House <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @House
else set @MySearchFields = @MySearchFields + '','' + @House
end
if (@Master is not null AND @Master <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Master
else set @MySearchFields = @MySearchFields + '','' + @Master
end
if (@ShipmentNumber is not null AND @ShipmentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ShipmentNumber
else set @MySearchFields = @MySearchFields + '','' + @ShipmentNumber
end
if (@CustomFileNumber is not null AND @CustomFileNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CustomFileNumber
else set @MySearchFields = @MySearchFields + '','' + @CustomFileNumber
end
if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @AWBCarrierTarrifReference
else set @MySearchFields = @MySearchFields + '','' + @AWBCarrierTarrifReference
end
if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CustomsDeclarationNumber
else set @MySearchFields = @MySearchFields + '','' + @CustomsDeclarationNumber
end
if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ForwarderShipmentNumber
else set @MySearchFields = @MySearchFields + '','' + @ForwarderShipmentNumber
end
if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @TransportDocumentNumber
else set @MySearchFields = @MySearchFields + '','' + @TransportDocumentNumber
end
if (@ImportManifest is not null AND @ImportManifest <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @ImportManifest
else set @MySearchFields = @MySearchFields + '','' + @ImportManifest
end
if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @BookingConfirmationNumber
else set @MySearchFields = @MySearchFields + '','' + @BookingConfirmationNumber
end
if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CarrierTransportDocumentNumber
else set @MySearchFields = @MySearchFields + '','' + @CarrierTransportDocumentNumber
end
if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment1AdditionalMAWBOBLBL
end
if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment2AdditionalMAWBOBLBL
end
if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
else set @MySearchFields = @MySearchFields + '','' + @Transshipment3AdditionalMAWBOBLBL
end
if (@QuoteId is not null)
begin
set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
if (@QuoteNumber is not null AND @QuoteNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @QuoteNumber
else set @MySearchFields = @MySearchFields + '','' + @QuoteNumber
end
end
if (@StatusId is not null)
begin
select
@StatusCode = Code,
@StatusName = Name
from EntityStatus
where Id = @StatusId AND Tenant = @Tenant
if (@StatusCode is not null AND @StatusCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @StatusCode
else set @MySearchFields = @MySearchFields + '','' + @StatusCode
end
if (@StatusName is not null AND @StatusName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @StatusName
else set @MySearchFields = @MySearchFields + '','' + @StatusName
end
end
if (@MainCarriageVesselId is not null)
begin
select
@MainCarriageVesselCode = Code,
@MainCarriageVesselName = EnglishName
from Vessels
where Id = @MainCarriageVesselId AND Tenant = @Tenant
if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselCode
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselCode
end
if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselName
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselName
end
end
if (@MainCarriageCarrierId is not null)
begin
select
@MainCarriageCarrierCode = Cards.Code,
@MainCarriageCarrierName = Cards.EnglishName,
@MainCarriageCarrierPrefix = Airlines.Prefix
from Airlines join Cards on Airlines.Id = Cards.Id
where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
if (@TransportModeId = ''A'' AND @Master is not null AND @Master <> '''')
begin
set @LongMaster = @MainCarriageCarrierPrefix + ''-'' + @Master
if (@LongMaster is not null AND @LongMaster <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @LongMaster
else set @MySearchFields = @MySearchFields + '','' + @LongMaster
end
end
if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierCode
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierCode
end
if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierName
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierName
end
if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierNumber
else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierNumber
end
end
if (@SalesmanUserId is not null)
begin
set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
if (@SalesmanUserName is not null AND @SalesmanUserName <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @SalesmanUserName
else set @MySearchFields = @MySearchFields + '','' + @SalesmanUserName
end
end
if (@AMSBL is not null AND @AMSBL <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @AMSBL
else set @MySearchFields = @MySearchFields + '','' + @AMSBL
end
if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @WarehouseLegReference
else set @MySearchFields = @MySearchFields + '','' + @WarehouseLegReference
end
END
-- Ports
BEGIN
set @PortId = @FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @PreCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @PreCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @OnCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @OnCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageFromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment1FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment1ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment2FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment2ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment3FromPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @Transshipment3ToPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
set @PortId = @MainCarriageFinalDestinationPortId
if (@PortId is not null)
if not exists (select * from @PortsTable where Id = @PortId)
begin
insert into @PortsTable(Id) values (@PortId)
select
@PortCode = Ports.Code,
@PortName = Ports.EnglishName,
@PortCountryCode = Countries.Code,
@PortCountryName  = Countries.EnglishName
from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
where Ports.Id = @PortId AND Ports.Tenant = @Tenant
if (@PortCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCode
else set @MySearchFields = @MySearchFields + '','' + @PortCode
end
if (@PortName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortName
else set @MySearchFields = @MySearchFields + '','' + @PortName
end
if (@PortCountryCode is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
end
if (@PortCountryName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
end
end
END
-- Partners
BEGIN
set @PartnerId = @AgentId
set @PartnerReference1 = @AgentReference1
set @PartnerReference2 = @AgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ShipperId
set @PartnerReference1 = @ShipperReference1
set @PartnerReference2 = @ShipperReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeId
set @PartnerReference1 = @ConsigneeReference1
set @PartnerReference2 = @ConsigneeReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomerId
set @PartnerReference1 = @CustomerReference1
set @PartnerReference2 = @CustomerReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @Notify1Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @Notify2Id
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @IssuingCarrierAgentId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentImportId
set @PartnerReference1 = @CustomAgentImportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @CustomAgentExportId
set @PartnerReference1 = @CustomAgentExportReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ShipperNotExporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsigneeNotImporterId
set @PartnerReference1 = null
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @FreightForwarderId
set @PartnerReference1 = @FreightForwarderReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ConsolidatorId
set @PartnerReference1 = @ConsolidatorReference
set @PartnerReference2 = null
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
set @PartnerId = @ReleasingAgentId
set @PartnerReference1 = @ReleasingAgentReference1
set @PartnerReference2 = @ReleasingAgentReference2
if (@PartnerId is not null)
begin
if not exists (select * from @PartnersTable where Id = @PartnerId)
begin
insert into @PartnersTable(Id) values (@PartnerId)
set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
if (@PartnerName is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @PartnerName
else set @MySearchFields = @MySearchFields + '','' + @PartnerName
end
end
if (@PartnerReference1 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference1)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
end
if (@PartnerReference2 is not null)
if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
begin
insert into @ReferencesTable(Reference) values (@PartnerReference2)
if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
end
end
if(@TransportModeId = ''I'' and @DirectionId =''D'' and @MasterShipmentDataId is not null)
begin
set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + '','' + @CityName
end
set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
if(@CityName is not null or @CityName != '''')
begin
if (@MySearchFields = '''') set @MySearchFields = @CityName
else set @MySearchFields = @MySearchFields + '','' + @CityName
end
end
END
-- Invoices
if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
BEGIN
DECLARE ARInvoicesCursor CURSOR READ_ONLY
FOR
SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
begin
if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceNumber
else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceNumber
end
else if (@ARInvoiceDraftNumber is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceDraftNumber
else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceDraftNumber
end
FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
END
CLOSE ARInvoicesCursor
DEALLOCATE ARInvoicesCursor
END
-- Packages Containers
if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
BEGIN
DECLARE PackagesCursor CURSOR READ_ONLY
FOR
SELECT ContainerNumber
FROM ShipmentPackages
Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''''
group by ContainerNumber
OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
WHILE @@FETCH_STATUS = 0
BEGIN
if (@ContainerNumber is not null)
begin
if (@MySearchFields = '''') set @MySearchFields = @ContainerNumber
else set @MySearchFields = @MySearchFields + '','' + @ContainerNumber
end
FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
END
CLOSE PackagesCursor
DEALLOCATE PackagesCursor
END
-- Custom Fields
BEGIN
set @Field = @Field1
set @FieldName = ''Field1''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field2
set @FieldName = ''Field2''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field3
set @FieldName = ''Field3''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field4
set @FieldName = ''Field4''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field5
set @FieldName = ''Field5''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field6
set @FieldName = ''Field6''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field7
set @FieldName = ''Field7''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field8
set @FieldName = ''Field8''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field9
set @FieldName = ''Field9''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
set @Field = @Field10
set @FieldName = ''Field10''
if (@Field is not null AND @Field <> '''')
begin
select
@FieldDataTypeCode = ObjectFields.DataTypeCode
from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
begin
if (@MySearchFields = '''') set @MySearchFields = @Field
else set @MySearchFields = @MySearchFields + '','' + @Field
end
end
END
insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO
@Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
@OnCarriageFromPortId, @OnCarriageToPortId,
@House, @CustomFileNumber, @AWBCarrierTarrifReference,
@AgentId, @AgentReference1, @AgentReference2,
@ShipperId, @ShipperReference1, @ShipperReference2,
@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
@CustomerId, @CustomerReference1, @CustomerReference2,
@Notify1Id,
@Notify2Id,
@IssuingCarrierAgentId,
@CustomAgentImportId, @CustomAgentImportReference,
@CustomAgentExportId, @CustomAgentExportReference,
@ShipperNotExporterId,
@ConsigneeNotImporterId,
@FreightForwarderId, @FreightForwarderReference,
@ConsolidatorId, @ConsolidatorReference,
@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
@AMSBL, @WarehouseLegReference
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
SearchFields = #temp_Shipments.SearchFields
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
SET NOCOUNT OFF
drop table #tempTable
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), 'fd337cc7a59cdf89641a7fbe65fe51de', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007091330_UpdateNotAirQuotesSubType.sxml File
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
CREATE TABLE #temp_Quotes (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Quotes.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Quotes.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Quotes where TransportModeId <> 'A'
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Quotes(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Quotes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Quotes
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007091330_UpdateNotAirQuotesSubType.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Quotes'') Is Not Null)
Begin
Drop Table #temp_Quotes
End
CREATE TABLE #temp_Quotes (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Quotes.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Quotes.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Quotes where TransportModeId <> ''A''
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Quotes(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Quotes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Quotes
set
ShipmentSubTypeId = #temp_Quotes.ShipmentSubTypeId,
ShipmentTypeId = #temp_Quotes.ShipmentTypeId
FROM Quotes
INNER JOIN #temp_Quotes
on Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Quotes.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Quotes', DATEDIFF(MS,@StartTime,@EndTime), '6a180068f0f1f9a2d54f22b583fb4a0e', 2);
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
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'MyGO' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGO' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'MyGI' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGI' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> 'A'
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051322_UpdateNotAirShipmentsSubType.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''MyGO'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGO'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''MyGI'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGI'' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> ''A''
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), 'd83b69dccd89385b36d4782d9c487e31', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

