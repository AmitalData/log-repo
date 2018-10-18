using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class MarkUpTypeList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}