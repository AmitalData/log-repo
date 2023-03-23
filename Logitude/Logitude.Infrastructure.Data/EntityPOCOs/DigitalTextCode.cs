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

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
   
    public class DigitalTextCode
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("Labels")]
	    public string Labels { get; set; }
        [ForeignKey("DigitalProfile")]
        [Column("ProfileId")]
	    public string ProfileId { get; set; }
	      
        public virtual DigitalProfile DigitalProfile { get; set; }
        [ForeignKey("DigitalPortalLanguage")]
        [Column("LanguageCode")]
	    public string LanguageCode { get; set; }
	      
        public virtual DigitalPortalLanguage DigitalPortalLanguage { get; set; }
    }
}
	 