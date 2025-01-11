using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DocumentTypeMetaData
    {
        [Key]
        public string Id { get; set; }

        public string DocumentTypeId { get; set; }

        public string DocumentsMetaDataTypeId { get; set; }

        public bool Mandatory { get; set; }

        public int Tenant { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey("DocumentsMetaDataType")]
        public virtual DocumentsMetaDataType DocumentsMetaDataType { get; set; }
    }
}
