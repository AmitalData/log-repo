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
   
    public class QuoteOPDocumentVersion
    {
	 string dbms;

        [Key]
        [ForeignKey("QuoteOP")]
        [Column("QuoteOPId")]
	    public string QuoteOPId { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
     [Key]
        [Column("VersionNumber")]
	    public int VersionNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("VersionType")]
	    public string VersionType { get; set; }
        [Column("DocumentId")]
	    public string DocumentId { get; set; }
        [Column("IsSent")]
	    public bool IsSent { get; set; }
        [Column("QuoteOPTemplateId")]
	    public string QuoteOPTemplateId { get; set; }
    }
}
	 