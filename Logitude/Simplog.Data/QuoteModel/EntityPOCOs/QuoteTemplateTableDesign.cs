using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteTemplateTableDesign
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BorderTypeCode { get; set; }
        public string BorderColor { get; set; }
        public int BorderThickness { get; set; }


        public string HeaderDesignId { get; set; }
        public string LinesDesignId { get; set; }
        public string GroupByDesignId { get; set; }

        [ForeignKey("HeaderDesignId")]
        public virtual QuoteTemplateTextDesign HeaderDesign { get; set; }


        [ForeignKey("LinesDesignId")]
        public virtual QuoteTemplateTextDesign LinesDesign { get; set; }

        [ForeignKey("GroupByDesignId")]
        public virtual QuoteTemplateTextDesign GroupDesign { get; set; }



        [ForeignKey("BorderTypeCode")]
        public virtual BorderType BorderType { get; set; }
    }
}
