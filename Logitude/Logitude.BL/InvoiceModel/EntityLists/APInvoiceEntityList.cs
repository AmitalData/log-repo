using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APInvoiceEntityList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string EntityReference { get; set; }
        public string ObjectTableId { get; set; }
        public string APInvoiceId { get; set; }
        public int IndexOrder { get; set; }
    }
}