using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
