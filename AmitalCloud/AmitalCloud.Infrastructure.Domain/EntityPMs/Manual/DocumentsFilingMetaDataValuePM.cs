using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class DocumentsFilingMetaDataValuePM : BaseEntityPM
    {
        [Key]
        public string Id { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentsMetaDataTypeId { get; set; }
        public string MetaDataValue { get; set; }
        public string DocumentsMetaDataTypeCode { get; set; }

    }
}