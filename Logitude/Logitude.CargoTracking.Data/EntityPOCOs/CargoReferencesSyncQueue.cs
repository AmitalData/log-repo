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

namespace Logitude.CargoTracking.Data.EntityPOCOs
{
   
    public class CargoReferencesSyncQueue
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public int Id { get; set; }
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
        [Column("ShipmentType")]
	    public string ShipmentType { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SyncTo")]
	    public string SyncTo { get; set; }
    }
}
	 