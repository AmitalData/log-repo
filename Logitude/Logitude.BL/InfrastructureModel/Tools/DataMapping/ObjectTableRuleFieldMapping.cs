using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
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
        }
    }
}