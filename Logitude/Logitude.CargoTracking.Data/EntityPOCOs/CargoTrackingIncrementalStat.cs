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
   
    public class CargoTrackingIncrementalStat
    {
	 string dbms;

           [Column("StartDate")]
	    public DateTime StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime EndDate { get; set; }
        [Column("Shipments")]
	    public int Shipments { get; set; }
        [Column("Cards")]
	    public int Cards { get; set; }
        [Column("Ports")]
	    public int Ports { get; set; }
        [Column("Countries")]
	    public int Countries { get; set; }
        [Column("TransportModes")]
	    public int TransportModes { get; set; }
        [Column("ShipmentComputedFields")]
	    public int ShipmentComputedFields { get; set; }
        [Column("ShipmentMasterDatas")]
	    public int ShipmentMasterDatas { get; set; }
     [Key]
        [Column("Id")]
	    public int Id { get; set; }
        [Column("ErrorLog")]
	    public string ErrorLog { get; set; }
    }
}
	 