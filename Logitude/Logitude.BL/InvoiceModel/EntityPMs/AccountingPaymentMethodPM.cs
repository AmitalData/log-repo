using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class AccountingPaymentMethodPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool AddedManually { get; set; }
        public string ARExternalId { get; set; }
        public string APExternalId { get; set; }
        public bool Inactive { get; set; }
        public bool IsAR { get; set; }
        public bool IsAP { get; set; }
    }
}