using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ChargesTypeList
    {
        public string Code { get; set; }

        public string EnglishName { get; set; }

        public string LocalName { get; set; }
        public int Tenant { get; set; }
        [Key]
        public string Id { get; set; }

        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }

        public string AccountingCard { get; set; }

        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string ChargesGroupId { get; set; }
        public string ChargesGroupCode { get; set; }
        public string ChargesGroupName { get; set; }
        
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double VatTypePercentage { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        public bool IsReceivable { get; set; }

        public bool IsPayable { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
        public bool IsAutoDisplayInShipment { get; set; }
        public bool IsAutoDisplayInConsolidation { get; set; }

        public bool AWBPrintDescription { get; set; }
        public string Description { get; set; }
        public string IATACodeId { get; set; }

        public string DueTypeCode { get; set; }
        public string DueTypeName { get; set; }

        public bool IsAutoDisplayInQuote { get; set; }

        public string ContainerMeasurementId { get; set; }
        public string ContainerMeasurementCode { get; set; }
        public int ViewOrder { get; set; }
        public string SearchFields { get; set; }
        public string ReceivableAccountId { get; set; }
        public string PayableAccountId { get; set; }

        public bool AccountingVATSplit { get; set; }
        public string ReceivableCreditAccount { get; set; }
        public string PayableDebitAccount { get; set; }
        public string ReceivablesChargesTypeExternalCode { get; set; }
        public string PayablesChargesTypeExternalCode { get; set; }
        public string PayableDebitGLAcountId { get; set; }
        public string ReceivableCreditGLAccountId { get; set; }
        public bool IsBackToBack { get; set; }
        public bool IsAutoDisplayInCustoms { get; set; }
        public bool IsCustoms { get; set; }
        public string SATExternalId { get; set; }
        public bool IsExpense { get; set; }

        public bool IsImport { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsExport { get; set; }
        public bool IsDrop { get; set; }

        public string ReceivablesDefaultCurrencyId { get; set; }
        public string PayablesDefaultCurrencyId { get; set; }
    }
}
