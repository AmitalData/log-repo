using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DigitalContactLastSetting
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }
        [Column("Tenant")]
        public int Tenant { get; set; }

        [ForeignKey("Contact")]
        [Column("ContactId")]
        public string ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        [Column("ObjectTableId")]
        public string ObjectTableId { get; set; }
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [Column("FilterName")]
        public string FilterName { get; set; }
        [Column("FilterCode")]
        public string FilterCode { get; set; }
        [Column("IsChecked")]
        public bool IsChecked { get; set; }
    }
}
