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
   
    public class QuoteOPTemplateTextCode
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("TextCode")]
	    public string TextCode { get; set; }
        [Column("EnglishName")]
	    public string EnglishName { get; set; }
        [Column("LocalName")]
	    public string LocalName { get; set; }
        [ForeignKey("QuoteOPTemplate")]
        [Column("QuoteTemplateId")]
	    public string QuoteTemplateId { get; set; }
	      
        public virtual QuoteOPTemplate QuoteOPTemplate { get; set; }
        [Column("Area")]
	    public string Area { get; set; }
        [Column("OriginalEnglishName")]
	    public string OriginalEnglishName { get; set; }
        [Column("OriginalLocalName")]
	    public string OriginalLocalName { get; set; }
    }
}
	 