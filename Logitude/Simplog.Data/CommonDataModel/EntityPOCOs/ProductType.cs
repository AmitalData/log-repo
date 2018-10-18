using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ProductType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    
        public string SearchFields { get; set; }
        public string QuotationDefaultTemplateId { get; set; }


        [ForeignKey("QuotationDefaultTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }




    }
}
