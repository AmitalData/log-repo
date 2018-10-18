using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ObjectFieldValidation
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string ValidationExpression { get; set; }
        public string ErrorMessage { get; set; }

        public string ObjectFieldId { get; set; }
        public int ValidationOrder { get; set; }
        public string Condition { get; set; }
        //[Include]
        //[Association("ObjectFieldValidationObjectField", "ObjectFieldId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectFieldId")]
        public virtual ObjectField ObjectField { get; set; }
    }
}