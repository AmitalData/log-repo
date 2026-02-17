using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FormCustomField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentTypeId { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string FieldCode { get; set; }
        public string Value { get; set; }

        //[Include]
        //[Association("DocumentTypeFormCustomFields","DocumentTypeId","Id",IsForeignKey=true)]
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }


          [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

    }
}