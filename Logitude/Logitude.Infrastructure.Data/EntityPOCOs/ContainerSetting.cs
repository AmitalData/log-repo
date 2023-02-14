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
   
    public class ContainerSetting
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("EmptyReturnClosingDays")]
	    public int? EmptyReturnClosingDays { get; set; }
        [Column("ShipmentATAClosingDays")]
	    public int? ShipmentATAClosingDays { get; set; }
        [Column("ShipmentATADateIndicator")]
	    public string ShipmentATADateIndicator { get; set; }
        [Column("IsExport")]
	    public bool IsExport { get; set; }
        [Column("IsDomestic")]
	    public bool IsDomestic { get; set; }
        [Column("IsImport")]
	    public bool IsImport { get; set; }
        [Column("IsDrop")]
	    public bool IsDrop { get; set; }
        [Column("AddedManually")]
	    public bool AddedManually { get; set; }
        [Column("ActivationDate")]
	    public DateTime? ActivationDate { get; set; }
    }
}
	 