using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class RuleConditionFieldMapping
    {
        public static void MapEntity(RuleConditionFieldPM ruleConditionFieldPM, RuleConditionField ruleConditionField, bool isNewState)
        {
            ruleConditionField.Tenant = ruleConditionFieldPM.Tenant;
            ruleConditionField.ObjectFieldId = ruleConditionFieldPM.ObjectFieldId;
            ruleConditionField.ObjectTableRuleId = ruleConditionFieldPM.ObjectTableRuleId;
            ruleConditionField.Operator = ruleConditionFieldPM.Operator;
            ruleConditionField.Value = ruleConditionFieldPM.Value;
        }
    }
}