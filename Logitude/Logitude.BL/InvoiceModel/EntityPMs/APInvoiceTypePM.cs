using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class APInvoiceTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}