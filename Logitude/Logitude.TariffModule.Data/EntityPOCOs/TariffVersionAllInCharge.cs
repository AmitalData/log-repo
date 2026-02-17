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

namespace Logitude.TariffModule.Data.EntityPOCOs
{
   
    public class TariffVersionAllInCharge
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Version")]
	    public int Version { get; set; }
        [ForeignKey("ChargesType")]
        [Column("ChargesTypeId")]
	    public string ChargesTypeId { get; set; }
	      
        public virtual ChargesType ChargesType { get; set; }
        [ForeignKey("AddedByUser")]
        [Column("AddedByUserId")]
	    public string AddedByUserId { get; set; }
	      
        public virtual User AddedByUser { get; set; }
        [Column("AddDate")]
	    public DateTime? AddDate { get; set; }
        [Column("TariffId")]
	    public string TariffId { get; set; }
    }
}
	 