
IF OBJECT_ID('[dbo].[usp_DeleteBusinessRecords]', 'P') IS NOT NULL
drop PROCEDURE [dbo].usp_DeleteBusinessRecords
GO

Create PROCEDURE [dbo].usp_DeleteBusinessRecords
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

END