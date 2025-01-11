using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


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
        public virtual Contact Contact { get; set; }

        [Column("ObjectTableId")]
        [ForeignKey("ObjectTable")]
        public string ObjectTableId { get; set; }
        public virtual ObjectTable ObjectTable { get; set; }

        [Column("FilterName")]
        public string FilterName { get; set; }
        [Column("FilterCode")]
        public string FilterCode { get; set; }
        [Column("IsChecked")]
        public bool IsChecked { get; set; }
    }  
}
