using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ChargesType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }        
        public string EnglishName { get; set; }         
        public string LocalName { get; set; }        
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public bool IsReceivable { get; set; }
        public bool IsPayable { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
        public bool IsAutoDisplayInShipment { get; set; }
        public bool IsAutoDisplayInConsolidation { get; set; }
        public bool AWBPrintDescription { get; set; }
        public string Description { get; set; }
        public bool IsAutoDisplayInQuote { get; set; }
        public int ViewOrder { get; set; }
        public string SearchFields { get; set; }
        public bool AccountingVATSplit { get; set; }
        public string ReceivablesChargesTypeExternalCode { get; set; }
        public string PayablesChargesTypeExternalCode { get; set; }
        public string ChargesGroupCode { get; set; }
        public bool IsBackToBack { get; set; }
        public bool IsAutoDisplayInCustoms { get; set; }
        public bool IsCustoms { get; set; }
        public bool IsExpense { get; set; }

        [ForeignKey("PayableAccountId")]
        public Account PayableAccount { get; set; }
        public string PayableAccountId { get; set; }
        public string PayableDebitAccount { get; set; }
        public string PayableDebitGLAcountId { get; set; }

        [ForeignKey("ReceivableAccountId")]
        public Account ReceivableAccount { get; set; }
        public string ReceivableAccountId { get; set; }
        public string ReceivableCreditAccount { get; set; }
        public string ReceivableCreditGLAccountId { get; set; }
        public string SATExternalId { get; set; }

        [ForeignKey("DueTypeCode")]
        public DueType DueType { get; set; }
        public string DueTypeCode { get; set; }

        [ForeignKey("ChargesGroupId")]
        public virtual ChargesGroup ChargesGroup { get; set; }
        public string ChargesGroupId { get; set; }

        [ForeignKey("IATACodeId")]
        public virtual IATACode IATACode { get; set; }
        public string IATACodeId { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }
        public string MeasurementId { get; set; }

        [ForeignKey("ContainerMeasurementId")]
        public virtual Measurement ContainerMeasurement { get; set; }
        public string ContainerMeasurementId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
        public string VatTypeId { get; set; }

        public bool IsImport { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsExport { get; set; }
        public bool IsDrop { get; set; }

        [ForeignKey("ReceivablesDefaultCurrencyId")]
        public virtual Currency ReceivablesDefaultCurrency  { get; set; }
        public string ReceivablesDefaultCurrencyId { get; set; }

        [ForeignKey("PayablesDefaultCurrencyId")]
        public virtual Currency PayablesDefaultCurrency { get; set; }
        public string PayablesDefaultCurrencyId { get; set; }
        public bool ApplyRegionalTax { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public bool HasPickup { get; set; }
        public bool HasDelivery { get; set; }
        public bool IsDirectionRestricted { get; set; }
        public bool IsActiveInExport { get; set; }
        public bool IsActiveInImport { get; set; }
        public bool IsActiveInDomestic { get; set; }
        public bool IsActiveInDrop { get; set; }
    }
}
