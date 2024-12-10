using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.CargoTracking.Data.EntityPOCOs
{
   
    public class CargoTrackingShipmentComputed
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("FirstPickupATD")]
	    public DateTime? FirstPickupATD { get; set; }
        [Column("FinalDeliveryATA")]
	    public DateTime? FinalDeliveryATA { get; set; }
        [Column("FinalDeliveryETA")]
	    public DateTime? FinalDeliveryETA { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 