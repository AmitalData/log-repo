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
   
    public class CustomsHouseTypeAdditional
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CustomsHouseType")]
        [Column("Code")]
	    public string Code { get; set; }
	      
        public virtual CustomsHouseType CustomsHouseType { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("UnloadingSiteType")]
        [Column("UnloadPortCode")]
	    public string UnloadPortCode { get; set; }
	      
        public virtual UnloadingSiteType UnloadingSiteType { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
    }
}
	 