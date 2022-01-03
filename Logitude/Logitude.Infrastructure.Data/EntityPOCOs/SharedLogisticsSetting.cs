using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class SharedLogisticsSetting
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("IsAgentShared")]
	    public bool IsAgentShared { get; set; }
        [Column("IsShipperNotExporterShared")]
	    public bool IsShipperNotExporterShared { get; set; }
        [Column("IsNotify1Shared")]
	    public bool IsNotify1Shared { get; set; }
        [Column("IsNotify2Shared")]
	    public bool IsNotify2Shared { get; set; }
        [Column("IsFreightForwarderShared")]
	    public bool IsFreightForwarderShared { get; set; }
        [Column("IsColoaderShared")]
	    public bool IsColoaderShared { get; set; }
        [Column("IsConsigneeNotImporterShared")]
	    public bool IsConsigneeNotImporterShared { get; set; }
        [Column("IsMainCarrierShared")]
	    public bool IsMainCarrierShared { get; set; }
        [Column("IsPickDelivCarriesShared")]
	    public bool IsPickDelivCarriesShared { get; set; }
        [Column("IsInvoicesMenuEnabled")]
	    public bool IsInvoicesMenuEnabled { get; set; }
        [Column("IsMoneyTabEnabled")]
	    public bool IsMoneyTabEnabled { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("IsIssuingCarrierAgentShared")]
	    public bool IsIssuingCarrierAgentShared { get; set; }
        [Column("IsCustomsAgentExportShared")]
	    public bool IsCustomsAgentExportShared { get; set; }
        [Column("IsCustomsAgentImportShared")]
	    public bool IsCustomsAgentImportShared { get; set; }
        [Column("IsCustomClearancePoinShared")]
	    public bool IsCustomClearancePoinShared { get; set; }
        [Column("IsConsolidatorShared")]
	    public bool IsConsolidatorShared { get; set; }
        [Column("IsReleasingAgentShared")]
	    public bool IsReleasingAgentShared { get; set; }
        [Column("IsShipperShared")]
	    public bool IsShipperShared { get; set; }
        [Column("IsConsigneeShared")]
	    public bool IsConsigneeShared { get; set; }
        [Column("IsShowAmountLocalCurrency")]
	    public bool IsShowAmountLocalCurrency { get; set; }
        [Column("IsShipperShowContactTS")]
	    public bool IsShipperShowContactTS { get; set; }
        [Column("IsConsigneeShowContactTS")]
	    public bool IsConsigneeShowContactTS { get; set; }
        [Column("IsAgentShowContactTS")]
	    public bool IsAgentShowContactTS { get; set; }
        [Column("IsShipperNotExShowContactTS")]
	    public bool IsShipperNotExShowContactTS { get; set; }
        [Column("IsConsigneeNotImShowContactTS")]
	    public bool IsConsigneeNotImShowContactTS { get; set; }
        [Column("IsNotify1ShowContactTS")]
	    public bool IsNotify1ShowContactTS { get; set; }
        [Column("IsNotify2ShowContactTS")]
	    public bool IsNotify2ShowContactTS { get; set; }
        [Column("IsFreightForwardShowContactTS")]
	    public bool IsFreightForwardShowContactTS { get; set; }
        [Column("IsColoaderShowContactTS")]
	    public bool IsColoaderShowContactTS { get; set; }
        [Column("IsCustomAgentExShowContactTS")]
	    public bool IsCustomAgentExShowContactTS { get; set; }
        [Column("IsCustomAgentImShowContactTS")]
	    public bool IsCustomAgentImShowContactTS { get; set; }
        [Column("IsCustomCleaPointShowContactTS")]
	    public bool IsCustomCleaPointShowContactTS { get; set; }
        [Column("IsConsolidatorShowContactTS")]
	    public bool IsConsolidatorShowContactTS { get; set; }
        [Column("IsReleasingAgentShowContactTS")]
	    public bool IsReleasingAgentShowContactTS { get; set; }
        [Column("IsIssuingCarAgentShowContactTS")]
	    public bool IsIssuingCarAgentShowContactTS { get; set; }
    }
}
	 