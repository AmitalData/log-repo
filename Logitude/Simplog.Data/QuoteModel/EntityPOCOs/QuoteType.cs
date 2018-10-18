using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<Quote> Quotes { get; set; }

    }
}