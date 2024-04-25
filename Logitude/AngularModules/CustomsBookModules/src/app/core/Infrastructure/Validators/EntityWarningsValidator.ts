import {ObjectTableRulePM} from '../EntityPMs/ObjectTableRulePM';
import {RuleConditionFieldPM} from '../EntityPMs/RuleConditionFieldPM';
import {ObjectTableRuleFieldPM} from '../EntityPMs/ObjectTableRuleFieldPM';
import {SessionLocator} from '../Utilities/SessionLocator';
import {SessionInfo} from '../Utilities/SessionInfo';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {AppTool} from '../Tools';
import {FieldValueResolver} from '../Utilities/FieldValueResolver';
import {ObjectTablePM} from '../EntityPMs/ObjectTablePM';
import {UIProperty, UIProperties, UIPropertyArgs} from '../Components/LogitudeComponents/UIProperties';
import {RulesValidator} from './RulesValidator';
declare var window: any;

export class EntityWarningsValidator {

    private requiredFieldRules: Array<ObjectTableRulePM> = [];
    private ruleValidator:RulesValidator;
    constructor() {
        if (window.ObjectTableRules) {
            this.requiredFieldRules = window.ObjectTableRules.filter(r => r.RuleTypeCode == "REQ" && r.InActive == false);
        }
        this.ruleValidator = new RulesValidator();
    }

     
    public  ValidateEntityWarnings(entity: Object, objectTableName: string): Array<string> {
        var table: ObjectTablePM =window.ObjectTables.filter(t => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0)).FirstOrDefault();
        var requiredFieldWarnings: Array<string> = new Array<string>();
        var tableRules: Array<ObjectTableRulePM> = this.requiredFieldRules.filter(a=> a.ObjectTableId == table.Id && (a.Tenant == SessionInfo.LoggedUserTenant || a.Tenant == 0));
        
        tableRules.forEach(rule => {
            this.ValidateRequiedFieldRule(rule.RuleCode, entity, requiredFieldWarnings);
        });

        return requiredFieldWarnings;
    }
    public  ValidateRequiedFieldRule(ruleCode: string, entity: Object, requiredFieldWarnings: Array<string>) {
       // var type1: Type = entity.GetType();
       // var propertyInf: PropertyInfo = null;
       // var propertyValue: Object = null;
        var rule: ObjectTableRulePM = (this.requiredFieldRules.filter(a=>a.RuleCode == ruleCode && (a.Tenant == SessionInfo.LoggedUserTenant || a.Tenant == 0) && a.InActive == false))[0];
        if (rule != null) {
            var ruleFields: Array<ObjectTableRuleFieldPM> = window.ObjectTableRuleFields.filter(rf => rf.ObjectTableRuleId == rule.Id && rf.RuleNotificationTypeCode == "WAR");
            if (!AppTool.IsNullOrEmpty(rule.Condition)) {
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
    }
    private  GenerateRuleWarnings(rule: ObjectTableRulePM, ruleFields: Array<ObjectTableRuleFieldPM>, entity: Object, requiredFieldWarnings: Array<string>): void {
        // var type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        // var propertyValue: Object = null;
        for (var k in ruleFields) {
            var ruleField: ObjectTableRuleFieldPM = ruleFields[k];
            var objectField: ObjectFieldPM = window.ObjectFields.filter(f => f.FieldCode === ruleField.ObjectFieldCode)[0];//ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
            //  propertyInf = type1.GetProperty(objectField.FieldName);
            //if (propertyInf != null) {
            var propertyValue = entity[objectField.FieldName];//propertyInf.GetValue(entity, null);
            if (objectField.DataTypeCode == "Text") {
                if (propertyValue == null) {
                    var warningMessage: string = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                    if (requiredFieldWarnings.some(r => r == warningMessage) == false) {
                        requiredFieldWarnings.push(warningMessage);
                    }
                }
                else {
                    if (AppTool.IsNullOrEmpty(propertyValue.toString().trim())) {
                        var warningMessage: string = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                        if (requiredFieldWarnings.some(r => r == warningMessage) == false) {
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
                        var warningMessage: string = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);

                        if (!requiredFieldWarnings.some(r => r == warningMessage)) {
                            requiredFieldWarnings.push(warningMessage);
                        }
                    }
                }
                else {
                    if (propertyValue == null) {
                        var warningMessage: string = EntityWarningsValidator.GetFieldWarningMessage(objectField.FullNameTextCodeCode);
                        if (!requiredFieldWarnings.some(r => r == warningMessage)) {
                            requiredFieldWarnings.push(warningMessage);
                        }
                    }
                }
            }
            //}
        }
    }

    public static GetFieldWarningMessage(FullNameTextCodeCode:string) {
        var warningMessage = TextCodeTranslator.Translate("General.M.FieldWarning");
        var fieldTrans = TextCodeTranslator.Translate(FullNameTextCodeCode);
        warningMessage =  warningMessage.replace("%FieldName", fieldTrans);

        return warningMessage;

    }
}

