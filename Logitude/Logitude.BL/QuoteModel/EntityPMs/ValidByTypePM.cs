using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class ValidByTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}
