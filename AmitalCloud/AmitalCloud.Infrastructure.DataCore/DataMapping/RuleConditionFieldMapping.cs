using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
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
            ruleConditionField.ObjectFieldCode = ruleConditionFieldPM.ObjectFieldCode;
        }
    }
}