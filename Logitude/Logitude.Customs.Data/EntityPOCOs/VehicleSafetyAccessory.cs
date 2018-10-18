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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class VehicleSafetyAccessory
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Vehicle")]
        [Column("VehicleId" ,Order = 1)]
	    public string VehicleId { get; set; }
	      
        public virtual Vehicle Vehicle { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 2)]
	    public int LineNumber { get; set; }
        [ForeignKey("VehicleSafetyAccessoryType")]
        [Column("VehicleSafetyAccessoryCode")]
	    public string VehicleSafetyAccessoryCode { get; set; }
	      
        public virtual VehicleSafetyAccessoryType VehicleSafetyAccessoryType { get; set; }
        [ForeignKey("VehicleSafeAccessoryInstlType")]
        [Column("VehicleSafAccessoryInstlTypCod")]
	    public string VehicleSafAccessoryInstlTypCod { get; set; }
	      
        public virtual VehicleSafeAccessoryInstlType VehicleSafeAccessoryInstlType { get; set; }
    }
}
	 