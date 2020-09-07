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
FROM Shipments where $LastCounterWhere$
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
FROM Shipments where $LastCounterWhere$
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
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), '0ffd1cb3665a8b1f4103b4f4b8032215', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007201431_FillWarehouseWeightClosedTables.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from WarehouseWeightMeasurements where Code = 'GRWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('GRWT', 'Gross Weight', 'GRWT,Gross Weight')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = 'CHWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('CHWT', 'Chargeable Weight', 'CHWT,Chargeable Weight')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'NON')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('NON', 'None', 'NON,None', 'None')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'HAF')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('HAF', 'Half', 'HAF,Half', '0.5')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'ONE')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('ONE', 'One', 'ONE,One', '1')
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201431_FillWarehouseWeightClosedTables.sxml', GETDATE(), 'if not exists (select Code from WarehouseWeightMeasurements where Code = ''GRWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''GRWT'', ''Gross Weight'', ''GRWT,Gross Weight'')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = ''CHWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''CHWT'', ''Chargeable Weight'', ''CHWT,Chargeable Weight'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''NON'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''NON'', ''None'', ''NON,None'', ''None'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''HAF'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''HAF'', ''Half'', ''HAF,Half'', ''0.5'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''ONE'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''ONE'', ''One'', ''ONE,One'', ''1'')
end', DATEDIFF(MS,@StartTime,@EndTime), '5a31949cce6467cce78c87ba8d9dff83', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007201436_FillWarehouseFieldsDefaultValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Warehouses set AirWeightMeasurementCode = 'GRWT'
update Warehouses set OceanWeightMeasurementCode = 'GRWT'
update Warehouses set InlandWeightMeasurementCode = 'GRWT'
update Warehouses set AirWeightRoundingCode = 'NON'
update Warehouses set OceanWeightRoundingCode = 'NON'
update Warehouses set InlandWeightRoundingCode = 'NON'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201436_FillWarehouseFieldsDefaultValues.sxml', GETDATE(), 'update Warehouses set AirWeightMeasurementCode = ''GRWT''
update Warehouses set OceanWeightMeasurementCode = ''GRWT''
update Warehouses set InlandWeightMeasurementCode = ''GRWT''
update Warehouses set AirWeightRoundingCode = ''NON''
update Warehouses set OceanWeightRoundingCode = ''NON''
update Warehouses set InlandWeightRoundingCode = ''NON''', DATEDIFF(MS,@StartTime,@EndTime), 'b17d9a558fa92bab0d7aece163282d61', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008051018_AddNewMeasurementAndChargesType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = 'STFE')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('STFE', 'Storage Fee', 'Storage Fee', @MeasurementId, @Tenant, 0, 0, 0, 'STFE,Storage Fee,Storage Fee', 'Storage Fee')
end
--ChargesType
set @ChargesGroupCode = 'HNDCH'
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = 'HNDCH')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = 'STD')
set @IATACodeId = (select Id from IATACodes where Code = 'SO')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = 'ISTOR')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,'ChargesType'
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values('ISTOR', 'Import Storage', 'Import Storage', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, 'AG',
0, @MeasurementId, NULL, 70,'ISTOR,Import Storage,Import Storage', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'Measurement')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008051018_AddNewMeasurementAndChargesType.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = ''STFE'')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''STFE'', ''Storage Fee'', ''Storage Fee'', @MeasurementId, @Tenant, 0, 0, 0, ''STFE,Storage Fee,Storage Fee'', ''Storage Fee'')
end
--ChargesType
set @ChargesGroupCode = ''HNDCH''
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = ''HNDCH'')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = ''STD'')
set @IATACodeId = (select Id from IATACodes where Code = ''SO'')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = ''ISTOR'')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,''ChargesType''
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values(''ISTOR'', ''Import Storage'', ''Import Storage'', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, ''AG'',
0, @MeasurementId, NULL, 70,''ISTOR,Import Storage,Import Storage'', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''Measurement'')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), 'b252e3857be84e566299715d80b06883', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

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

-- General Script From 202008261425_SetToggleCodeForHorsesFeature.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from Toggles where Code = 'HRS')
begin
insert into Toggles (Code, Name, SearchFields)
values ('HRS', 'Horse', 'HRS,Horse')
end
update Features
set ToggleCode = 'HRS'
where Code = 'Horse.M.Horses'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008261425_SetToggleCodeForHorsesFeature.sxml', GETDATE(), 'if not exists (select Code from Toggles where Code = ''HRS'')
begin
insert into Toggles (Code, Name, SearchFields)
values (''HRS'', ''Horse'', ''HRS,Horse'')
end
update Features
set ToggleCode = ''HRS''
where Code = ''Horse.M.Horses''', DATEDIFF(MS,@StartTime,@EndTime), '3e01d0de48c070160eed409222a814c8', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008300915_AddHorsesEventTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = 'QuoteClosingReason')
set @Code = 'HRIN'
set @Name = 'Set as Inactive'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = 'HRRC'
set @Name = 'Reactivated'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008300915_AddHorsesEventTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = ''QuoteClosingReason'')
set @Code = ''HRIN''
set @Name = ''Set as Inactive''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = ''HRRC''
set @Name = ''Reactivated''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '5f18abe5124698d5536097a0706ca9fb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009011450_ImportToUSADropMaman.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete from CustomsInterfaces where code = 'CMN'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009011450_ImportToUSADropMaman.sxml', GETDATE(), 'delete from CustomsInterfaces where code = ''CMN''', DATEDIFF(MS,@StartTime,@EndTime), '830638ae226d14b30e478d0b45d1f45b', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009031302_FillDataFromShipmentPackagesToShipmentComputedFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#temp') Is Not Null)
Begin
Drop Table #temp
End
If(OBJECT_ID('tempdb..#tempGrouped') Is Not Null)
Begin
Drop Table #tempGrouped
End
If(OBJECT_ID('tempdb..#temp_ShipmentComputedFields') Is Not Null)
Begin
Drop Table #temp_ShipmentComputedFields
End
CREATE TABLE #temp_ShipmentComputedFields
(
Id varchar(15) not null,
ContainersNumbersandTypesArray varchar(1000) null
)
select
Shipments.Id, ShipmentPackages.ContainerNumber as ContainerNumber, PackageTypes.Code as PackageTypeCode
into #temp
from Shipments
left outer join ShipmentPackages on Shipments.Id = ShipmentPackages.ShipmentId
left outer join PackageTypes on ShipmentPackages.PackageTypeId = PackageTypes.Id
SELECT DISTINCT temp3.Id,
SUBSTRING(
(
SELECT ','+temp1.ContainerNumber+'['+temp1.PackageTypeCode+']'
FROM #temp temp1
WHERE temp1.Id = temp3.Id
ORDER BY temp1.Id
FOR XML PATH ('')
), 2, 1000) ContainersNumbersandTypesArray
into #tempGrouped
FROM #temp temp3
declare @Id as varchar(15)
declare @ContainersNumbersandTypesArray as varchar(1000)
declare @Count as int
set @Count = 0;
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, ContainersNumbersandTypesArray
FROM #tempGrouped
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_ShipmentComputedFields(Id, ContainersNumbersandTypesArray) values(@Id, @ContainersNumbersandTypesArray)
set @Count = @Count + 1;
if(@Count = 1000)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
truncate table #temp_ShipmentComputedFields
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
END
CLOSE DataCursor
DEALLOCATE DataCursor
if (@Count > 0)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
end
drop table #temp
drop table #tempGrouped
drop table #temp_ShipmentComputedFields
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009031302_FillDataFromShipmentPackagesToShipmentComputedFields.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#temp'') Is Not Null)
Begin
Drop Table #temp
End
If(OBJECT_ID(''tempdb..#tempGrouped'') Is Not Null)
Begin
Drop Table #tempGrouped
End
If(OBJECT_ID(''tempdb..#temp_ShipmentComputedFields'') Is Not Null)
Begin
Drop Table #temp_ShipmentComputedFields
End
CREATE TABLE #temp_ShipmentComputedFields
(
Id varchar(15) not null,
ContainersNumbersandTypesArray varchar(1000) null
)
select
Shipments.Id, ShipmentPackages.ContainerNumber as ContainerNumber, PackageTypes.Code as PackageTypeCode
into #temp
from Shipments
left outer join ShipmentPackages on Shipments.Id = ShipmentPackages.ShipmentId
left outer join PackageTypes on ShipmentPackages.PackageTypeId = PackageTypes.Id
SELECT DISTINCT temp3.Id,
SUBSTRING(
(
SELECT '',''+temp1.ContainerNumber+''[''+temp1.PackageTypeCode+'']''
FROM #temp temp1
WHERE temp1.Id = temp3.Id
ORDER BY temp1.Id
FOR XML PATH ('''')
), 2, 1000) ContainersNumbersandTypesArray
into #tempGrouped
FROM #temp temp3
declare @Id as varchar(15)
declare @ContainersNumbersandTypesArray as varchar(1000)
declare @Count as int
set @Count = 0;
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, ContainersNumbersandTypesArray
FROM #tempGrouped
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_ShipmentComputedFields(Id, ContainersNumbersandTypesArray) values(@Id, @ContainersNumbersandTypesArray)
set @Count = @Count + 1;
if(@Count = 1000)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
truncate table #temp_ShipmentComputedFields
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id, @ContainersNumbersandTypesArray
END
CLOSE DataCursor
DEALLOCATE DataCursor
if (@Count > 0)
begin
update ShipmentComputedFields
set ContainersNumbersandTypesArray = #temp_ShipmentComputedFields.ContainersNumbersandTypesArray
FROM ShipmentComputedFields
INNER JOIN #temp_ShipmentComputedFields
on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
end
drop table #temp
drop table #tempGrouped
drop table #temp_ShipmentComputedFields', DATEDIFF(MS,@StartTime,@EndTime), '1a2368963f979ff3a8286b129ace299f', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

