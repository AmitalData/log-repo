"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionInfo_1 = require("../Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var Tools_1 = require("../Tools");
var RulesValidator_1 = require("./RulesValidator");
var EntityWarningsValidator = /** @class */ (function () {
    function EntityWarningsValidator() {
        this.requiredFieldRules = [];
        if (window.ObjectTableRules) {
            this.requiredFieldRules = window.ObjectTableRules.filter(function (r) { return r.RuleTypeCode == "REQ" && r.InActive == false; });
        }
        this.ruleValidator = new RulesValidator_1.RulesValidator();
    }
    EntityWarningsValidator.prototype.ValidateEntityWarnings = function (entity, objectTableName) {
        var _this = this;
        var table = window.ObjectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); }).FirstOrDefault();
        var requiredFieldWarnings = new Array();
        var tableRules = this.requiredFieldRules.filter(function (a) { return a.ObjectTableId == table.Id && (a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || a.Tenant == 0); });
        tableRules.forEach(function (rule) {
            _this.ValidateRequiedFieldRule(rule.RuleCode, entity, requiredFieldWarnings);
        });
        return requiredFieldWarnings;
    };
    EntityWarningsValidator.prototype.ValidateRequiedFieldRule = function (ruleCode, entity, requiredFieldWarnings) {
        // var type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        // var propertyValue: Object = null;
        var rule = (this.requiredFieldRules.filter(function (a) { return a.RuleCode == ruleCode && (a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || a.Tenant == 0) && a.InActive == false; }))[0];
        if (rule != null) {
            var ruleFields = window.ObjectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id && rf.RuleNotificationTypeCode == "WAR"; });
            if (!Tools_1.AppTool.IsNullOrEmpty(rule.Condition)) {
                //var conditionFieldsDic: Dictionary<string, Object> = new Dictionary<string, Object>();
                //var fieldNames: string[] = [];
                //var ruleCondition: string = rule.Condition;
                //var level1: string[] = ruleCondition.Split('[');
                //level1.forEach(function (s) {
                //    if (s.Contains("]")) {
                //        var level2: string[] = s.Split(']');
                //        var fieldName: string = level2[0].Trim();
                //        ruleCondition = ruleCondition.Replace("[" + fieldName + "]", fieldName);
                //        propertyInf = type1.GetProperty(fieldName);
                //        if (propertyInf != null) {
                //            propertyValue = propertyInf.GetValue(entity, null);
                //            if (!conditionFieldsDic.Keys.Contains(fieldName)) {
                //                conditionFieldsDic.Add(fieldName, propertyValue);
                //            }
                //        }
                //    }
                //});
                //if (conditionFieldsDic.Count != 0) {
                //    var expressionValidation: ExpressionValidation = new ExpressionValidation();
                //    var required: boolean = expressionValidation.ExecuteExpression(ruleCondition, conditionFieldsDic);
                //    if (required) {
                //        EntityWarningsValidator.GenerateRuleWarnings(rule, ruleFields, entity, requiredFieldWarnings);
                //    }
                //}
            }
            else {
                if (this.ruleValidator.ValidateConditionFieldsRule(entity, rule.RuleConditionFields)) {
                    this.GenerateRuleWarnings(rule, ruleFields, entity, requiredFieldWarnings);
                }
            }
        }
    };
    EntityWarningsValidator.prototype.GenerateRuleWarnings = function (rule, ruleFields, entity, requiredFieldWarnings) {
        // var type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        // var propertyValue: Object = null;
        for (var k in ruleFields) {
            var ruleField = ruleFields[k];
            var objectField = window.ObjectFields.filter(function (f) { return f.Id === ruleField.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
            //  propertyInf = type1.GetProperty(objectField.FieldName);
            //if (propertyInf != null) {
            var propertyValue = entity[objectField.FieldName]; //propertyInf.GetValue(entity, null);
            if (objectField.DataTypeCode == "Text") {
                if (propertyValue == null) {
                    var warningMessage = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                    if (requiredFieldWarnings.some(function (r) { return r == warningMessage; }) == false) {
                        requiredFieldWarnings.push(warningMessage);
                    }
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(propertyValue.toString().trim())) {
                        var warningMessage = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                        if (requiredFieldWarnings.some(function (r) { return r == warningMessage; }) == false) {
                            requiredFieldWarnings.push(warningMessage);
                        }
                    }
                }
            }
            else {
                if (propertyValue != null && (objectField.DataTypeCode == "Integer" || objectField.DataTypeCode == "Double" || objectField.DataTypeCode == "Decimal")) {
                    //var result: number;
                    //Int32.TryParse(propertyValue.ToString(), result);
                    if (propertyValue == 0) {
                        var warningMessage = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                        if (!requiredFieldWarnings.some(function (r) { return r == warningMessage; })) {
                            requiredFieldWarnings.push(warningMessage);
                        }
                    }
                }
                else {
                    if (propertyValue == null) {
                        var warningMessage = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                        if (!requiredFieldWarnings.some(function (r) { return r == warningMessage; })) {
                            requiredFieldWarnings.push(warningMessage);
                        }
                    }
                }
            }
            //}
        }
    };
    EntityWarningsValidator.GetFieldWarningMessage = function (FullNameTextCodeCode) {
        var warningMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldWarning");
        var fieldTrans = TextCodeTranslator_1.TextCodeTranslator.Translate(FullNameTextCodeCode);
        warningMessage = warningMessage.replace("%FieldName", fieldTrans);
        return warningMessage;
    };
    return EntityWarningsValidator;
}());
exports.EntityWarningsValidator = EntityWarningsValidator;
//# sourceMappingURL=EntityWarningsValidator.js.map