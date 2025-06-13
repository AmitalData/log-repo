using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ObjectTableRuleFieldMapping
    {
        public static void MapEntity(ObjectTableRuleFieldPM ruleFieldPM, ObjectTableRuleField ruleField, bool isNewState)
        {
            ruleField.Tenant = ruleFieldPM.Tenant;
            ruleField.ObjectFieldId = ruleFieldPM.ObjectFieldId;
            ruleField.ObjectTableRuleId = ruleFieldPM.ObjectTableRuleId;
            ruleField.SystemLevel = ruleFieldPM.SystemLevel;
            ruleField.Expression = ruleFieldPM.Expression;
            ruleField.RuleNotificationTypeCode = ruleFieldPM.RuleNotificationTypeCode;
            ruleField.ObjectFieldCode = ruleFieldPM.ObjectFieldCode;
        }
    }
}