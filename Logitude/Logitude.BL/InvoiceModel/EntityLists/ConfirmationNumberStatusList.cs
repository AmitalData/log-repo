using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ConfirmationNumberStatusList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }

        public string SearchFields { get; set; }
    }
}