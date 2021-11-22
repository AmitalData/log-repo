using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Linq;
using System.Data.SqlClient;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel
{
    public interface IShipmentsContext : IContext
    {
        IDbSet<Shipment> Shipments { get; }
        IDbSet<ShipmentType> ShipmentTypes { get; }
        IDbSet<ShipmentMasterData> ShipmentMasterDatas { get; }
        IDbSet<ShipmentReceivable> ShipmentReceivables { get; }
        IDbSet<ShipmentPickUpDelivery> ShipmentPickUpDeliveries { get; }
        IDbSet<PackageType> PackageTypes { get; }
        IDbSet<ShipmentPackage> ShipmentPackages { get; }
        IDbSet<InsideShipmentPackage> InsideShipmentPackages { get; }
        IDbSet<ShipmentPickUpDeliveryPackage> ShipmentPickUpDeliveryPackages { get; }
        IDbSet<ShipmentOrderPackage> ShipmentOrderPackages { get; }
        IDbSet<ShipmentPayableLineStatus> ShipmentPayableLineStatus { get; }
        IDbSet<ShipmentPayable> ShipmentPayables { get; }
        IDbSet<ShipmentReceivableLineStatus> ShipmentReceivableLineStatus { get; }
        IDbSet<ShipmentReceivableStatus> ShipmentReceivableStatus { get; }
        IDbSet<ShipmentPayableStatus> ShipmentPayableStatus { get; }
        IDbSet<ShipmentCustomerType> ShipmentCustomerTypes { get; }
        IDbSet<PickUpDeliveryType> PickUpDeliveryTypes { get; }
        IDbSet<PickUpDeliveryFromToType> PickUpDeliveryFromToTypes { get; }
        IDbSet<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies { get; }
        IDbSet<NextLeg> NextLegs { get; }
        IDbSet<ShipmentPayableAmountType> ShipmentPayableAmountTypes { get; }
        IDbSet<ShipmentLevel> ShipmentLevels { get; }
        IDbSet<AWBChargesCode> AWBChargeCodes { get; }
        IDbSet<AWBSpecialHandlingCode> AWBHandlingCodes { get; }
        IDbSet<FWBStatus> FWBStatus { get; }
        IDbSet<FHLStatus> FHLStatus { get; }
        IDbSet<AWBStatus> AWBStatus { get; }
        IDbSet<ShipmentCarrierStatus> ShipmentCarrierStatuses { get; }
        IDbSet<AWBOCI> AWBOCIs { get; }
        IDbSet<AWBCustomsInformation> AWBCustomsInformations { get; }
        IDbSet<AWBInformation> AWBInformations { get; }
        IDbSet<ShipmentPackageItem> ShipmentPackageItems { get; }
        IDbSet<ShipmentCommodity> ShipmentCommodities { get; }
        IDbSet<SpecialServicesType> SpecialServicesTypes { get; }
        IDbSet<MessagingStock> MessagingStocks { get; }
        IDbSet<MessagingStockUsageHistory> MessagingStockUsageHistories { get; }
        IDbSet<AccountingInformationIdentifier> AccountingInformationIdentifiers { get; }
        IDbSet<ManifestStatus> ManifestStatus { get; }
        IDbSet<AWBAdditionalHandlingInfo> AWBAdditionalHandlingInfos { get; }
        IDbSet<ShipmentComputedFields> ShipmentComputedFields { get; }
        IDbSet<OceanInsightsRequest> OceanInsightsRequests { get; }
        IDbSet<LogitudeOceanInsightsRequest> LogitudeOceanInsightsRequests { get; }
        IDbSet<LogitudeOceanInsightsResponse> LogitudeOceanInsightsResponses { get; }
        IDbSet<OceanInsightsRequestsCount> OceanInsightsRequestsCounts { get; }
        IDbSet<OceanInsightsStatuses> OceanInsightsStatuses { get; }
        IDbSet<OtherParticipantId> OtherParticipantIds { get; }
        IDbSet<ShipmentAdditionalCloudData> ShipmentAdditionalCloudDatas { get; }
        IDbSet<FBLStock> FBLStocks { get; set; }
        IDbSet<CustomsTransmissionsStatus> CustomsTransmissionsStatus { get; set; }
        IDbSet<OBLType> OBLTypes { get; set; }
        IDbSet<ShipmentAssembly> ShipmentAssemblies { get; set; }
        IDbSet<ShipmentCustomsMessageType> ShipmentCustomsMessageTypes { get; set; }
        IDbSet<ShipmentCustomsTransmission> ShipmentCustomsTransmissions { get; }
        IDbSet<INTTRAStatus> INTTRAStatuses { get; }
        IDbSet<INTTRASIStatus> INTTRASIStatus { get; }
        IDbSet<INTTRABookingTransStatus> INTTRABookingTransStatuses { get; }
        IDbSet<INTTRABookingStatus> INTTRABookingStatuses { get; }
        IDbSet<ShipmentContainerStatus> ShipmentContainerStatuses { get; }
        IDbSet<PickUpDeliveryTransportMode> PickUpDeliveryTransportModes { get; }
        IDbSet<INTTRADocumentType> INTTRADocumentTypes { get; }
        IDbSet<ShipmentPackageHarmonize> ShipmentPackageHarmonizes { get; }
        IDbSet<PickUpDeliveryPackageHarmonize> PickUpDeliveryPackageHarmonizes { get; }
        IDbSet<HarmonizeCode> HarmonizeCodes { get; }
        IDbSet<CustomsTransferType> CustomsTransferTypes { get; }
        IDbSet<CustomsTransferLine> CustomsTransferLines { get; }
        IDbSet<CustomsTransferHeader> CustomsTransferHeaders { get; }
        IDbSet<ShipmentSubType> ShipmentSubTypes { get; }
        IDbSet<ShipmentStoragePricing> ShipmentStoragePricings { get; set; }
        IDbSet<ShipmentProductItem> ShipmentProductItems { get; set; }
        IDbSet<Container> Containers { get; set; }
        IDbSet<ContainerStatus> ContainerStatuses { get; }
        IDbSet<ContainerStatusSource> ContainerStatusSources { get; }
        IDbSet<ARInvoice> ARInvoicesForReports{ get; }
        IQueryable<TOutput> FunctionTableValue<TOutput>(string functionName, SqlParameter[] parameters);
        IQueryable<ShipmentDataView> ShipmentSearch(string SearchFields);

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}