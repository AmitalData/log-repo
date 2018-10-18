using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class AccountTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}