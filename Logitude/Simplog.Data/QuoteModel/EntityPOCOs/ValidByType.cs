using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class ValidByType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

    }
}
