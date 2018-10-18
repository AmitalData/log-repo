using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentTypeCustomField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentTypeId { get; set; }
        public string FieldCode { get; set; }
        public string FieldDataTypeCode { get; set; }
        public bool InActive { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public bool MultiLine { get; set; }
        public string Name { get; set; }
        //[Include]
        //[Association("DocumentTypeDocumentTypeCustomField","DocumentTypeId","Id",IsForeignKey=true)]
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }
        //[ExternalReference]
        //[Association("DocumentTypeCustomFieldFieldDatatype", "FieldDataTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("FieldDataTypeCode")]
        public FieldDataType FieldDataType { get; set; }

        public int IndexOrder { get; set; }

    }
}