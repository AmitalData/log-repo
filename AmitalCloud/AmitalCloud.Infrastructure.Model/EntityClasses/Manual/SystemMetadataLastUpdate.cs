using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class SystemMetadataLastUpdate
    {
        [Key]
        public string Id { get; set; }
        public DateTime TranslationsUpdateDateGMT { get; set; }
        public DateTime ObjectFieldsUpdateDateGMT { get; set; }
    }
}
