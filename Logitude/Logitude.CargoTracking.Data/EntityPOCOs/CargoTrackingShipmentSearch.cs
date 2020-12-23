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
   
    public class CargoTrackingShipmentSearch
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ShipmentDate")]
	    public DateTime ShipmentDate { get; set; }
     [Key]
        [Column("Id")]
	    public int Id { get; set; }
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
        [Column("IsPublic")]
	    public bool? IsPublic { get; set; }
        [Column("ReferenceType")]
	    public string ReferenceType { get; set; }
    }
}
	 