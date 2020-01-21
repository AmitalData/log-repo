using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ObjectTableRuleMapping
    {
        public static void MapEntity(ObjectTableRulePM rulePM, ObjectTableRule rule, bool isNewState)
        {
            rule.Tenant = rulePM.Tenant;
            rule.RuleCode = rulePM.RuleCode;
            rule.RuleTypeCode = rulePM.RuleTypeCode;
            rule.Name = rulePM.Name;
            rule.ObjectTableId = rulePM.ObjectTableId;
            rule.OutputMessage = rulePM.OutputMessage;
            rule.SystemLevel = rulePM.SystemLevel;
            rule.InActive = rulePM.InActive;
            rule.Condition = rulePM.Condition;
            rule.TriggerTypeCode = rulePM.TriggerTypeCode;
            rule.TriggerFieldId = rulePM.TriggerFieldId;
            rule.ActiveForUpdate = rulePM.ActiveForUpdate;
            rule.ActiveForNew = rulePM.ActiveForNew;
            rule.RuleNotificationTypeCode = rulePM.RuleNotificationTypeCode;
            rule.Internal = rulePM.Internal;
            rule.AdvancedCondition = rulePM.AdvancedCondition;
            rule.TriggerFieldCode = rulePM.TriggerFieldCode;
        }
    }
}