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
   
    public class CargoTrackingShipmentMaster
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Master")]
	    public string Master { get; set; }
        [Column("MainCarriageATD")]
	    public DateTime? MainCarriageATD { get; set; }
        [Column("MainCarriageETD")]
	    public DateTime? MainCarriageETD { get; set; }
        [Column("MainCarriageATA")]
	    public DateTime? MainCarriageATA { get; set; }
        [Column("MainCarriageETA")]
	    public DateTime? MainCarriageETA { get; set; }
    }
}
	 