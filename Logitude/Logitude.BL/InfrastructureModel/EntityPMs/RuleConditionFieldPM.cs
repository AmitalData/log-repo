using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class RuleConditionFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableRuleId { get; set; }
        public string ObjectFieldId { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string ObjectFieldCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        public string ObjectFieldName { get; set; }
    }
}
