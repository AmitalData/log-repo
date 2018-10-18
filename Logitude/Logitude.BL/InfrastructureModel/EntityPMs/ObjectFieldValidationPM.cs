using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InfrastructureModel.EntityPMs
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
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}