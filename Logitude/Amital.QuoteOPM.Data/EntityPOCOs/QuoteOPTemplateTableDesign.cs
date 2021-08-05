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
   
    public class QuoteOPTemplateTableDesign
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("BorderOPType")]
        [Column("BorderTypeCode")]
	    public string BorderTypeCode { get; set; }
	      
        public virtual BorderOPType BorderOPType { get; set; }
        [Column("BorderColor")]
	    public string BorderColor { get; set; }
        [Column("BorderThickness")]
	    public int BorderThickness { get; set; }
        [ForeignKey("HeaderDesign")]
        [Column("HeaderDesignId")]
	    public string HeaderDesignId { get; set; }
	      
        public virtual QuoteOPTemplateTextDesign HeaderDesign { get; set; }
        [ForeignKey("LinesDesign")]
        [Column("LinesDesignId")]
	    public string LinesDesignId { get; set; }
	      
        public virtual QuoteOPTemplateTextDesign LinesDesign { get; set; }
        [ForeignKey("GroupDesign")]
        [Column("GroupByDesignId")]
	    public string GroupByDesignId { get; set; }
	      
        public virtual QuoteOPTemplateTextDesign GroupDesign { get; set; }
    }
}
	 