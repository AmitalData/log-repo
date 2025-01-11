using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class BlobFile
    {
        [Key]
        public string Id { get; set; }
        public byte[] Blob { get; set; }
        public bool IsCompressed { get; set; }
    }
}
