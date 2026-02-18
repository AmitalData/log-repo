using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentsFilingMetaDataValue
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentsMetaDataTypeId { get; set; }
        public string MetaDataValue { get; set; }

        [ForeignKey("DocumentsFilingId")]
        public virtual DocumentsFiling DocumentsFiling { get; set; }

        [ForeignKey("DocumentsMetaDataTypeId")]
        public virtual DocumentsMetaDataType DocumentsMetaDataType { get; set; }

    }
}
