using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteCustomerType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool ShowInLOV { get; set; }
    }
}