using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APPaymentMethodList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool AddedManually { get; set; }
        public bool Inactive { get; set; }
        public string AccountingExternalId { get; set; }

    }
}