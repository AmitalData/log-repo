using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARInvoiceEntity
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARInvoiceId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityReference { get; set; }
        [ForeignKey("ARInvoiceId")]
        public virtual ARInvoice ARInvoice { get; set; }

          [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }


    }
}