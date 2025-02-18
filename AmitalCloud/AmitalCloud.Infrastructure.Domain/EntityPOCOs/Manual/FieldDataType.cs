using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class FieldDataType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        ////[Include]
        ////[Association("ObjectFieldDataType", "Code", "DataTypeCode")]
        //public virtual List<ObjectField> ObjectFields { get; set; }
        //    public List<DocumentTypeCustomField> DocumentTypeCustomFields { get; set; }
    }
}