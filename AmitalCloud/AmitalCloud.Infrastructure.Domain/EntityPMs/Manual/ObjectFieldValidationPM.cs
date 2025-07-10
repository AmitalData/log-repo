using AmitalCloud.Infrastructure.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectFieldValidationPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string ValidationExpression { get; set; }
        public string ErrorMessage { get; set; }
        public int ValidationOrder { get; set; }
        public string ObjectFieldId { get; set; }
        public string Condition { get; set; }
        public string Code { get; set; }
        public string ObjectFieldCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}