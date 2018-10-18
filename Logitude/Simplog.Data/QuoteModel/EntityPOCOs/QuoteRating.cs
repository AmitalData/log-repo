using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteRating
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int IndexOrder { get; set; }
        public string SearchFields { get; set; }
    }
}
