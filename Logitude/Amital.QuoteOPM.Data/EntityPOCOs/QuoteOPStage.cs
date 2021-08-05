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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPStage
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Code")]
	    public string Code { get; set; }
        [Column("Name")]
	    public string Name { get; set; }
        [Column("MaxDays")]
	    public int? MaxDays { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("Rank")]
	    public int Rank { get; set; }
        [Column("InActive")]
	    public bool InActive { get; set; }
        [Column("AutomaticLastUpdateDate")]
	    public DateTime? AutomaticLastUpdateDate { get; set; }
    }
}
	 