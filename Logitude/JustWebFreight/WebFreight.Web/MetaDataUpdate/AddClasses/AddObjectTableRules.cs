using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddObjectTableRules
    {
        public static ObjectTableRule AddObjectTableRule(ObjectTableRuleDetails objectTableRuleDetails, ObjectTableRuleRepository objectTableRuleRepository,Dictionary<string,ObjectTableRule> tenantObjectTableRules)
        {
            if (tenantObjectTableRules.Keys.Contains(objectTableRuleDetails.RuleCode))
            {
                ObjectTableRule objectTableRule = tenantObjectTableRules[objectTableRuleDetails.RuleCode];
                objectTableRule.ActiveForNew = objectTableRuleDetails.ActiveForNew;
                objectTableRule.ActiveForUpdate = objectTableRuleDetails.ActiveForUpdate;
                objectTableRule.Condition = objectTableRuleDetails.Condition;
                objectTableRule.InActive = objectTableRuleDetails.InActive;
                objectTableRule.Name = objectTableRuleDetails.Name;
                objectTableRule.ObjectTableId = objectTableRuleDetails.ObjectTableId;
                objectTableRule.OutputMessage = objectTableRuleDetails.OutputMessage;
                objectTableRule.RuleNotificationTypeCode = objectTableRuleDetails.RuleNotificationTypeCode;
                objectTableRule.RuleTypeCode = objectTableRuleDetails.RuleTypeCode;
                objectTableRule.SystemLevel = objectTableRuleDetails.SystemLevel;
                objectTableRule.Tenant = objectTableRuleDetails.Tenant;
                objectTableRule.TriggerFieldId = objectTableRuleDetails.TriggerFieldId;
                objectTableRule.TriggerTypeCode = objectTableRuleDetails.TriggerTypeCode;
                objectTableRule.Internal = objectTableRuleDetails.Internal;
                objectTableRule.AdvancedCondition = objectTableRuleDetails.AdvancedCondition;
                objectTableRuleRepository.Update(objectTableRule);
                return objectTableRule;
            }
            else
            {
                ObjectTableRule newObjectTableRule = new ObjectTableRule()
                {
                    TriggerTypeCode = objectTableRuleDetails.TriggerTypeCode,
                    TriggerFieldId = objectTableRuleDetails.TriggerFieldId,
                    Tenant = objectTableRuleDetails.Tenant,
                    SystemLevel = objectTableRuleDetails.SystemLevel,
                    RuleTypeCode = objectTableRuleDetails.RuleTypeCode,
                    RuleNotificationTypeCode = objectTableRuleDetails.RuleNotificationTypeCode,
                    OutputMessage = objectTableRuleDetails.OutputMessage,
                    ObjectTableId = objectTableRuleDetails.ObjectTableId,
                    Name = objectTableRuleDetails.Name,
                    InActive = objectTableRuleDetails.InActive,
                    Condition = objectTableRuleDetails.Condition,
                    ActiveForNew = objectTableRuleDetails.ActiveForNew,
                    ActiveForUpdate = objectTableRuleDetails.ActiveForUpdate,
                    Id = IdCounter.GetNumber("ObjectTableRule",objectTableRuleDetails.Tenant).ToString(),
                    RuleCode = objectTableRuleDetails.RuleCode,
                    Internal = objectTableRuleDetails.Internal,
                    AdvancedCondition = objectTableRuleDetails.AdvancedCondition,
                };
                objectTableRuleRepository.Add(newObjectTableRule);
                return newObjectTableRule;
            }
        }

        public static ObjectTableRuleField AddObjectTableRuleField(ObjectTableRuleFieldDetails objectTableRuleFieldDetails, ObjectTableRuleFieldRepository objectTableRuleRepository, Dictionary<string, ObjectTableRuleField> tenantObjectTableRuleFields)
        {
            if (tenantObjectTableRuleFields.Keys.Contains(objectTableRuleFieldDetails.ObjectTableRuleId + objectTableRuleFieldDetails.ObjectFieldCode))
            {
                ObjectTableRuleField objectTableRuleField = tenantObjectTableRuleFields[objectTableRuleFieldDetails.ObjectTableRuleId + objectTableRuleFieldDetails.ObjectFieldCode];
                objectTableRuleField.Expression = objectTableRuleFieldDetails.Expression;
                objectTableRuleField.SystemLevel = objectTableRuleFieldDetails.SystemLevel;
                objectTableRuleField.RuleNotificationTypeCode = objectTableRuleFieldDetails.RuleNotificationTypeCode;
                objectTableRuleRepository.Update(objectTableRuleField);
                return objectTableRuleField;
            }
            else
            {
                ObjectTableRuleField newObjectTableRuleField = new ObjectTableRuleField()
                {
                    SystemLevel = objectTableRuleFieldDetails.SystemLevel,
                    Expression = objectTableRuleFieldDetails.Expression,
                    Id = IdCounter.GetNumber("ObjectTableRuleField", objectTableRuleFieldDetails.Tenant).ToString(),
                    ObjectFieldId = objectTableRuleFieldDetails.ObjectFieldId,
                    ObjectTableRuleId = objectTableRuleFieldDetails.ObjectTableRuleId,
                    Tenant = objectTableRuleFieldDetails.Tenant,
                    RuleNotificationTypeCode = objectTableRuleFieldDetails.RuleNotificationTypeCode,
                    ObjectFieldCode = objectTableRuleFieldDetails.ObjectFieldCode,
                };
                objectTableRuleRepository.Add(newObjectTableRuleField);
                return newObjectTableRuleField;
            }
        }




        public static RuleConditionField AddRuleConditionField(RuleConditionFieldDetails ruleConditionFieldDetails, RuleConditionFieldRepository ruleConditionFieldRepository, Dictionary<string, RuleConditionField> tenantRuleConditionFields)
        {
            if (tenantRuleConditionFields.Keys.Contains(ruleConditionFieldDetails.ObjectTableRuleId + ruleConditionFieldDetails.ObjectFieldCode))
            {
                RuleConditionField ruleConditionField = tenantRuleConditionFields[ruleConditionFieldDetails.ObjectTableRuleId + ruleConditionFieldDetails.ObjectFieldCode];
                ruleConditionField.Operator = ruleConditionFieldDetails.Operator;
                ruleConditionField.Value = ruleConditionFieldDetails.Value;

                ruleConditionFieldRepository.Update(ruleConditionField);
                return ruleConditionField;
            }
            else
            {
                RuleConditionField newRuleConditionField = new RuleConditionField()
                {

                    Id = IdCounter.GetNumber("RuleConditionField", ruleConditionFieldDetails.Tenant).ToString(),
                    ObjectFieldId = ruleConditionFieldDetails.ObjectFieldId,
                    ObjectFieldCode = ruleConditionFieldDetails.ObjectFieldCode,
                    ObjectTableRuleId = ruleConditionFieldDetails.ObjectTableRuleId,
                    Tenant = ruleConditionFieldDetails.Tenant,
                    Value = ruleConditionFieldDetails.Value,
                    Operator = ruleConditionFieldDetails.Operator,

                };

                ruleConditionFieldRepository.Add(newRuleConditionField);
                return newRuleConditionField;
            }
        }
    
    }
}