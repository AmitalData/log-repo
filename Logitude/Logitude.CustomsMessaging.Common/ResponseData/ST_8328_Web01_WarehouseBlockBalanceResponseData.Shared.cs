using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ST_8328_Web01_WarehouseBlockBalanceResponseData : ResponseDataBase
    {
        public string SiteNumber { get; set; }
        public string SiteText { get; set; }
        public string WarehouseBlockNumber { get; set; }
        public string DeclarationNumber { get; set; }
        public string ImporterNumber { get; set; }
        public string ImporterTitle { get; set; }
        public string OpeningDate { get; set; }
        public string OriginalOpeningDate { get; set; }
        public string MaxStorageDate { get; set; }
        public string BlockClosureDate { get; set; }
        public string LogicalPackagesQuantityBalance { get; set; }
        public string PhysicalPackagesQuantityBalance { get; set; }
        public string Value { get; set; }
        public string StorageEntryPortChargeBalance { get; set; }
        public string StorageEntryPortChargeCurrencyType { get; set; }
        public string StorageEntryTransportBalance { get; set; }
        public string StorageEntryTransportCurrencyType { get; set; }
        public string StorageEntryInsuranceBalance { get; set; }
        public string StorageEntryInsuranceCurrencyType { get; set; }
        public List<BlockSpecialActivities> BlockSpecialActivitiesList { get; set; }
        public List<ActionActivities> ActionList { get; set; }
        public List<StorageAction> StorageActionList { get; set; }
        public List<GoodsItemByInvoice> GoodsItemByInvoiceList { get; set; }
    }

    public class BlockSpecialActivities
    {
        public string SpecialActivityTypeName { get; set; }
    }

    public class ActionActivities
    {
        public string ActionDate { get; set; }
        public string ActionPackagesQuantity { get; set; }
        public string PackagesQuantityAfterAction { get; set; }
        public string ActionValue { get; set; }
        public string ValueAfterAction { get; set; }
        public string GovernmentProcedureType { get; set; }
        public string GovernmentProcedureTypeText { get; set; }
        public string DeclarationNumber { get; set; }
    }

    public class StorageAction
    {
        public string StorageActionDate { get; set; }
        public string StorageActionType { get; set; }
        public string StorageActionPackagesQuantity { get; set; }
        public string PackagesQuantityAfterStorageAction { get; set; }
        public string StorageReferenceType { get; set; }
        public string CargoMovementReference { get; set; }
        public List<PackingDetails> PackingDetailsList { get; set; }
        public string StorageUnloadingExceptiontype { get; set; }
        public string StorageUnloadingExceptionTypeText { get; set; }
    }

    public class PackingDetails
    {
        public string PackingType { get; set; }
        public string PackingTypeText { get; set; }
        public string StorageActionPackagesQuantity { get; set; }
        public string PackagesQuantityAfterStorageAction { get; set; }
        public string StorageActionPackagesWeight { get; set; }
        public string PackagesWeightAfterStorageAction { get; set; }
    }

    public class GoodsItemByInvoice
    {
        public string InvoiceSequenceNumber { get; set; }
        public string GoodsItemSequenceNumber { get; set; }
        public string RemainingQuantity { get; set; }
        public string GoodsPriceBase { get; set; }
        public string GoodsPriceMAD { get; set; }
        public string GoodsPriceMADAEF { get; set; }
        public string CurrencyType { get; set; }
        public string ExchangeRate { get; set; }
    }
}
