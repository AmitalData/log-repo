"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Settings_1 = require("../Settings");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var Tools_1 = require("../Tools");
var FieldValueResolver_1 = require("../Utilities/FieldValueResolver");
var CustomFieldClass_1 = require("../DataContracts/CustomFieldClass");
var EntityListService_1 = require("../Services/EntityListService");
var ApiQueryFilters_1 = require("../DataContracts/ApiQueryFilters");
var Tools_2 = require("../Tools");
var RulesValidator = /** @class */ (function () {
    function RulesValidator() {
        this._requiredFieldRules = [];
        this._entityLevelRules = [];
        this._blockRules = [];
        this._tenantRules = [];
        this._tenantObjectFields = [];
        this._objectTableRuleFields = [];
        this._objectTables = [];
        this._unConditionalsetFieldValueRules = [];
        this._conditionalSetFieldValueRules = [];
        this._triggeredSetFieldValueRules = [];
        this.Initizialize();
    }
    RulesValidator.prototype.Initizialize = function () {
        var _this = this;
        if (window.ObjectTableRules != null && window.ObjectTableRules != undefined) {
            this._tenantRules = window.ObjectTableRules;
            this._objectTables = window.ObjectTables;
            this._objectTableRuleFields = window.ObjectTableRuleFields;
            this._tenantObjectFields = window.ObjectFields;
            if (this.IsNewEntity) {
                this._requiredFieldRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "REQ" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity); });
                this._entityLevelRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "EVAL" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity); });
                this._blockRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "BLCK" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity); });
                this._unConditionalsetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "ALLW" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity);
                });
                this._conditionalSetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "COND" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity);
                });
                this._triggeredSetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "FLDC" && r.InActive == false && r.InActive == false && (r.ActiveForNew == _this.IsNewEntity);
                });
            }
            else {
                this._requiredFieldRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "REQ" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate); });
                this._entityLevelRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "EVAL" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate); });
                this._blockRules = this._tenantRules.filter(function (r) { return r.RuleTypeCode == "BLCK" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate); });
                this._unConditionalsetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "ALLW" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate);
                });
                this._conditionalSetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "COND" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate);
                });
                this._triggeredSetFieldValueRules = this._tenantRules.filter(function (r) {
                    return r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "FLDC" && r.InActive == false && r.InActive == false && (r.ActiveForUpdate);
                });
            }
            this.entityListService = new EntityListService_1.EntityListService();
            //this._triggeredSetFieldValueRules = (from r in TenantContext.Current.ObjectTableRules
            //where(r.RuleTypeCode == "SETV") && (r.TriggerTypeCode == "FLDC") && r.InActive == false && r.InActive == false && (r.ActiveForNew == this.IsNewEntity || r.ActiveForUpdate)
            //select r).ToList();//TenantContext.Current.ObjectTableRules.
        }
    };
    RulesValidator.prototype.ValidateAllTableRules = function (entity, objectTableId, errorsArray) {
        this.ValidateAllRequiredFieldRules(entity, objectTableId, errorsArray);
        this.ValidateEntityRules(entity, objectTableId, errorsArray);
        //this.val
        return errorsArray;
    };
    RulesValidator.prototype.ApplyEntityChangedRules = function (propertyName, entity, objectTableName) {
        //if (this.CurrentSession && this.CurrentSession.CurrentEditComponent) {
        //    this.CurrentSession.CurrentEditComponent.ChangeDetectorRef.detach();
        //}
        if (entity) {
            this.IsNewEntity = (entity.OldEntityPM === null || entity.OldEntityPM === undefined);
        }
        this.Initizialize();
        this.ApplyRequiredFieldRules(propertyName, entity, objectTableName);
        this.ApplyConditionalBlockFieldRules(propertyName, entity, objectTableName, true);
        this.ApplyTriggeredSetFieldValueRules(propertyName, entity, objectTableName);
        this.ApplyConditionalSetFieldRules(propertyName, entity, objectTableName, true);
        //if (this.CurrentSession && this.CurrentSession.CurrentEditComponent) {
        //    this.CurrentSession.CurrentEditComponent.ChangeDetectorRef.detectChanges();
        //}
    };
    //***********************************************************************************************
    RulesValidator.prototype.ApplyTriggeredSetFieldValueRules = function (propertyName, entity, objectTableName) {
        if (Settings_1.Settings.DisableRuleValidation) {
            return;
        }
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var objectTableId = table.Id;
        var propertyValue = null;
        var entityTableRules = this._triggeredSetFieldValueRules.filter(function (o) { return o.ObjectTableId == objectTableId; });
        for (var k in entityTableRules) {
            var rule = entityTableRules[k];
            var field = this._tenantObjectFields.filter(function (x) { return x.Id === rule.TriggerFieldId; })[0];
            if (field && field.FieldName == propertyName && entity.OldEntityPM) {
                this.ExecuteSetFieldsRule(rule, entity, objectTableName);
            }
        }
    };
    RulesValidator.prototype.ExecuteSetFieldsRule = function (rule, entity, objectTableName) {
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
        for (var key in ruleFields) {
            var ruleField = ruleFields[key];
            var expression = ruleField.Expression;
            if (expression != null) {
                var FieldValue;
                if (expression.indexOf("[") >= 0) {
                    //for (var m in expression.split('')) {
                    //    var ch = expression[m];
                    //    if(ch != 
                    //}
                    var fieldName = expression.substring(expression.lastIndexOf("[") + 1, expression.lastIndexOf("]"));
                    if (fieldName.indexOf(".") === -1) {
                        this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, entity[fieldName], table.Id);
                    }
                    else {
                        this.SetInsideEntityFieldValue(entity, entity, fieldName, ruleField, table);
                    }
                }
                else {
                    //ConditionFieldsDic.Add(ruleField.ObjectFieldName, expression);
                    //object value = expressionValidation.ExecuteValueExpression(ruleField.ObjectFieldName, ConditionFieldsDic);
                    this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, expression, table.Id);
                }
            }
            else {
                this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, null, table.Id);
            }
        }
    };
    RulesValidator.prototype.SetInsideEntityFieldValue = function (parentEntity, entity, fieldName, ruleField, table) {
        var _this = this;
        var apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
        var currentEntity = entity;
        var fieldsAray = fieldName.split('.');
        // while (true) { start
        var i = 0;
        var f1 = fieldsAray[0]; //"IncotermId.PerpaidCollect.Id"
        var currentValue = currentEntity[fieldsAray[i]];
        var objectField = this._tenantObjectFields.filter(function (x) { return x.FieldName === fieldsAray[i] && x.ObjectTableId === table.Id; })[0];
        if (objectField && objectField.DataTypeCode == "DateTime") {
            if (fieldsAray[i + 1] == "Date") {
                var datetimevalue = currentEntity[fieldsAray[i]];
                var datevalue = Tools_2.DateTool.TruncateTime(datetimevalue);
                this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, datevalue, table.Id);
            }
            else
                this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, currentValue, table.Id);
        }
        if (objectField && currentValue && objectField.DataTypeCode == "LookUp") {
            var insideEntityName = objectField.ObjectTable_LookUpTableName;
            var insideTable = this._objectTables.filter(function (t) { return t.Name == insideEntityName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
            var insideObjectField = this._tenantObjectFields.filter(function (f) { return f.FieldName === fieldsAray[i + 1] && f.ObjectTableId === insideTable.Id; })[0];
            if (insideTable.CacheOnClient) {
                this.entityListService.getSingleFromCache(currentValue, insideEntityName, apiFilters).then(function (res) {
                    res.subscribe(function (response) {
                        var insideEntity = response.Result;
                        if (insideEntity) {
                            if ((i + 1) < fieldsAray.length) {
                                var insideValue = insideEntity[fieldsAray[i + 1]];
                                if (insideObjectField && insideObjectField.DataTypeCode == "LookUp") {
                                    var insideEntityName2 = insideObjectField.ObjectTable_LookUpTableName;
                                    var insideTable2 = _this._objectTables.filter(function (t) { return t.Name == insideEntityName2 && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
                                    var insideFields = fieldName.replace(fieldsAray[i] + ".", "");
                                    _this.SetInsideEntityFieldValue(parentEntity, insideEntity, insideFields, ruleField, insideTable);
                                }
                                else {
                                    _this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, insideValue, table.Id);
                                }
                            }
                            else {
                            }
                        }
                        else {
                            // break;
                        }
                    });
                });
            }
            else {
                this.entityListService.getSingle(currentValue, insideEntityName).then(function (res) {
                    res.subscribe(function (response) {
                        var insideEntity = response.Result;
                        if (insideEntity) {
                            if ((i + 1) < fieldsAray.length) {
                                var insideValue = insideEntity[fieldsAray[i + 1]];
                                if (insideObjectField && insideObjectField.DataTypeCode == "LookUp") {
                                    var insideEntityName2 = insideObjectField.ObjectTable_LookUpTableName;
                                    var insideTable2 = _this._objectTables.filter(function (t) { return t.Name == insideEntityName2 && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
                                    var insideFields = fieldName.replace(fieldsAray[i] + ".", "");
                                    _this.SetInsideEntityFieldValue(parentEntity, insideEntity, insideFields, ruleField, insideTable);
                                }
                                else {
                                    _this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, insideValue, table.Id);
                                }
                            }
                            else {
                            }
                        }
                        else {
                            // break;
                        }
                    });
                });
            }
        }
        else {
            // break;
        }
        //} end
    };
    RulesValidator.prototype.SetEntityFieldValue = function (entity, propertyName, value, objectTableId) {
        var table = this._objectTables.filter(function (t) { return t.Id == objectTableId && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        var field = this._tenantObjectFields.filter(function (a) { return a.FieldName == propertyName && a.ObjectTableId == objectTableId; })[0];
        var resultValue = " ";
        if (value != undefined && value != null && (value instanceof CustomFieldClass_1.CustomFieldClass)) {
            value = value.ResolvedValue;
        }
        if (field != null) {
            if (field.IsCustom) {
                var classvalue = entity[propertyName];
                if (classvalue != null) {
                    if (classvalue.Value != value) {
                        var customFieldClass = new CustomFieldClass_1.CustomFieldClass(value, propertyName, table.Name);
                        entity[propertyName] = customFieldClass;
                    }
                }
                else {
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(value, propertyName, table.Name);
                    entity[propertyName] = customFieldClass;
                }
            }
            else {
                if (entity[propertyName] != value) {
                    entity[propertyName] = value;
                }
            }
        }
        else {
            if (entity[propertyName] != value) {
                entity[propertyName] = value;
            }
        }
    };
    RulesValidator.prototype.ApplyConditionalSetFieldRules = function (propertyName, entity, objectTableName, onPropertyChange) {
        if (Settings_1.Settings.DisableRuleValidation) {
            return;
        }
        var propertyValue = null;
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var fieldFormat = "[" + propertyName + "]";
        var advancedConditionalTableSetValueRules = this._conditionalSetFieldValueRules.filter(function (r) { return (r.Condition != null) && r.AdvancedCondition == true && r.TriggerTypeCode == "COND" && r.ObjectTableId == table.Id; });
        var conditionalTableSetValueRules = this._conditionalSetFieldValueRules.filter(function (r) { return r.AdvancedCondition == false && r.RuleConditionFields.length > 0 && r.TriggerTypeCode == "COND" && r.ObjectTableId == table.Id; });
        for (var k in conditionalTableSetValueRules) {
            var rule = conditionalTableSetValueRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            if (rule.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; }) || (ruleFields.some(function (f) { return f.ObjectFieldName == propertyName; }) && !onPropertyChange)) {
                //var canRun: boolean = CanRunRule(rule, table.Id, entity);
                //if (canRun) {
                var validcondition = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
                //if (rule.RuleConditionFields.some(f => f.ObjectFieldName == propertyName)) {
                //    this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, uiPoperty);
                //}
                if (validcondition) {
                    this.ExecuteSetFieldsRule(rule, entity, objectTableName);
                }
            }
        }
        ;
        //type1 = entity.GetType();
        //propertyInf = _type1.GetProperty(propertyName);
        //if (propertyInf != null) {
        propertyValue = entity[propertyName]; //propertyInf.GetValue(entity, null);
        for (var k in advancedConditionalTableSetValueRules) {
            rule = advancedConditionalTableSetValueRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            if (rule.Condition.indexOf(fieldFormat) != -1 || ruleFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                //// var canRun: boolean = CanRunRule(rule, table.Id, entity);
                // //if (canRun) {
                //     var ruleCondition: string = rule.Condition;
                //     var blocked: boolean = ValidateConditionExpression(ruleCondition, entity, rule.ObjectTableId);
                //     if (onPropertyChange) {
                //         var cancel: boolean = false;
                //         ruleFields.forEach(function (f) {
                //             if (ruleCondition.Contains(f.ObjectFieldName)) {
                //                 cancel = true;
                //                 break;
                //             }
                //         });
                //         if (cancel) {
                //             return
                //         }
                //     }
                //     if (!ruleFields.some(f => f.ObjectFieldName == propertyName)) {
                //         SetFieldsAccessibility(ruleFields, viewModelBase, blocked, objectTableName, entity);
                //     }
                //     else {
                //         SetFieldsAccessibility(ruleFields.filter(f => f.ObjectFieldName == propertyName), viewModelBase, blocked, objectTableName, entity);
                //     }
                // //}
            }
        } //);
        //}
    };
    RulesValidator.prototype.ApplyUnConditionalSetFieldRules = function (propertyName, entity, objectTableName) {
        if (Settings_1.Settings.DisableRuleValidation) {
            return;
        }
        var propertyValue = null;
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var tableSetValueRules = this._unConditionalsetFieldValueRules.filter(function (r) { return r.ObjectTableId == table.Id; });
        for (var k in tableSetValueRules) {
            var rule = tableSetValueRules[k];
            this.ExecuteSetFieldsRule(rule, entity, objectTableName);
        }
        ;
    };
    //***********************************************************************************************
    RulesValidator.prototype.ValidateAllRequiredFieldRules = function (entity, objectTableId, errorsArray) {
        var requiredFields = [];
        if (Settings_1.Settings.DisableRuleValidation) {
            return requiredFields;
        }
        var tableRules = this._requiredFieldRules.filter(function (a) { return a.ObjectTableId == objectTableId && (a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || a.Tenant == 0); });
        // tableRules.forEach(function (rule) {
        //    this.ValidateRequierdFieldRule(rule.RuleCode, entity);
        //  });
        for (var k in tableRules) {
            this.ExecuteRequierdFieldRule(tableRules[k].RuleCode, entity, requiredFields);
        }
        if (requiredFields.length != 0) {
            for (var k in requiredFields) {
                var field = requiredFields[k];
                var obField = this._tenantObjectFields.filter(function (x) { return x.Id === field.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
                var requiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldTrans = TextCodeTranslator_1.TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                requiredError = requiredError.replace("%FieldName", fieldTrans);
                errorsArray.push(requiredError);
            }
        }
        //return requiredFields;
    };
    RulesValidator.prototype.ValidateEntityRules = function (entity, objectTableId, errorsArray) {
        if (Settings_1.Settings.DisableRuleValidation) {
            return true;
        }
        //var outputMessage1: string = "";
        var isValid = true;
        //var expressionValidation: ExpressionValidation = null;
        var tableRules = this._entityLevelRules.filter(function (r) { return r.ObjectTableId == objectTableId; });
        for (var k in tableRules) {
            var rule = tableRules[k];
            //if (CanRunRule(rule, rule.ObjectTableId, entity)) {
            // if (expressionValidation == null) {
            //   expressionValidation = new ExpressionValidation();
            // }
            var haserror = false;
            if (rule.AdvancedCondition && rule.Condition != null) {
                //haserror = ValidateConditionExpression(rule.Condition, entity, rule.ObjectTableId);
            }
            if (rule.AdvancedCondition == false && rule.RuleConditionFields.length > 0) {
                haserror = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
            }
            if (haserror) {
                if (!Tools_1.AppTool.IsNullOrEmpty(rule.OutputMessage)) {
                    errorsArray.push(rule.OutputMessage);
                }
                isValid = false;
                //break;
            }
            //}
        }
        //if (isValid) {
        //outputMessage = "";
        //    return true;
        //}
        //else {
        // outputMessage = outputMessage1;
        //   return false;
        //}
        return isValid;
    };
    RulesValidator.prototype.ExecuteRequierdFieldRule = function (ruleCode, entity, requiredFields) {
        var rule = this._requiredFieldRules.filter(function (a) { return a.RuleCode == ruleCode && (a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || a.Tenant == 0) && a.InActive == false; })[0];
        if (rule != null) {
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id && rf.RuleNotificationTypeCode == "ERR"; });
            // if (CanRunRule(rule, rule.ObjectTableId, entity)) {
            if (rule.AdvancedCondition && rule.Condition != null) {
                //bool required = ValidateConditionExpression(rule.Condition, entity, rule.ObjectTableId);
                //if (required) {
                // GenerateRuleErrors(ruleFields, entity, requiredFields);
                //}
            }
            if (rule.TriggerTypeCode == "ALLW") {
                this.GenerateRuleErrors(ruleFields, entity, requiredFields);
            }
            else {
                if (rule.AdvancedCondition == false && rule.RuleConditionFields.length > 0) {
                    var required = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
                    if (required) {
                        this.GenerateRuleErrors(ruleFields, entity, requiredFields);
                    }
                }
            }
        }
        return requiredFields;
    };
    RulesValidator.prototype.ApplyRequiredFieldRules = function (propertyName, entity, objectTableName, viewModelBase) {
        if (viewModelBase === void 0) { viewModelBase = entity; }
        if (Settings_1.Settings.DisableRuleValidation) {
            return;
        }
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var fieldFormat = "[" + propertyName + "]";
        var advancedConditionalRules = this._requiredFieldRules.filter(function (r) { return r.Condition != null && r.AdvancedCondition == true && r.ObjectTableId == table.Id; });
        var fieldsConditionalRules = this._requiredFieldRules.filter(function (r) { return r.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; }) && r.AdvancedCondition == false && r.ObjectTableId == table.Id; });
        var unConditionalRules = this._requiredFieldRules.filter(function (r) { return r.Condition == null && r.TriggerTypeCode == "ALLW" && r.ObjectTableId == table.Id; });
        //var type1: Type = entity.GetType();
        //------------------ Advanced Conditional rules
        var advancedRulesToExecute = advancedConditionalRules.filter(function (r) { return r.Condition.indexOf(propertyName) != -1 && r.ObjectTableId == table.Id; });
        var targetRuleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectFieldName == propertyName && rf.ObjectTableRuleTypeCode == "REQ"; });
        for (var k in targetRuleFields) {
            var field = targetRuleFields[k];
            var currField = this._tenantObjectFields.filter(function (f) { return f.Id == field.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
            if (currField != null) {
                if (table.Id == currField.ObjectTableId) {
                    var rule = this._requiredFieldRules.filter(function (r) { return r.Id == field.ObjectTableRuleId; })[0];
                    if (rule != null) {
                        if (!advancedRulesToExecute.some(function (r) { return r.Id == field.ObjectTableRuleId; })) {
                            if (rule.AdvancedCondition && rule.Condition != null) {
                                advancedRulesToExecute.push(rule);
                            }
                        }
                        if (!fieldsConditionalRules.some(function (r) { return r.Id == field.ObjectTableRuleId; })) {
                            if (!rule.AdvancedCondition && rule.RuleConditionFields.length > 0) {
                                fieldsConditionalRules.push(rule);
                            }
                        }
                    }
                }
            }
        }
        //  if (requiredFieldsRules.FirstOrDefault() != null) {
        for (var k in advancedRulesToExecute) {
            var rule = advancedRulesToExecute[k];
            //if (CanRunRule(rule, table.Id, entity)) {
            //var required: boolean = ValidateConditionExpression(rule.Condition, entity, rule.ObjectTableId);
            //var ruleFields: List<ObjectTableRuleFieldPM> = TenantContext.Current.ObjectTableRuleFields.filter(rf => rf.ObjectTableRuleId == rule.Id);
            // SetRequiredFields(ruleFields, viewModelBase, required, objectTableName, entity);
        }
        //});
        //}
        //--------------------------------------------------
        for (var k in fieldsConditionalRules) {
            var rule = fieldsConditionalRules[k];
            if (rule.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                // if (CanRunRule(rule, table.Id, entity)) {
                var validcondition = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
                var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
                this.SetRequiredFields(ruleFields, validcondition, objectTableName, entity, viewModelBase);
            }
            //}
        }
        for (var k in unConditionalRules) {
            var rule = unConditionalRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            this.SetRequiredFields(ruleFields, true, objectTableName, entity, viewModelBase);
        }
    };
    RulesValidator.prototype.SetRequiredFields = function (ruleFields, required, objectTableName, entity, viewModelBase) {
        //var type1: Type = entity.GetType();
        //var propertyInf: PropertyInfo = null;
        var propertyValue = null;
        for (var k in ruleFields) {
            var ruleField = ruleFields[k];
            var objectField = this._tenantObjectFields.filter(function (f) { return f.Id == ruleField.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
            if (objectField != null) {
                if (this.HasProperty(entity, objectField.FieldName)) {
                    if (required && entity.UIProperties) {
                        propertyValue = entity[objectField.FieldName];
                        if (propertyValue != null) {
                            if (Tools_1.AppTool.IsNullOrEmpty(propertyValue.toString())) {
                                entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
                            }
                        }
                        else {
                            entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
                        }
                    }
                    else if (!objectField.IsRequiered && entity.UIProperties) {
                        entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, false);
                    }
                }
                else {
                    if (required && viewModelBase.UIProperties) {
                        propertyValue = viewModelBase[objectField.FieldName];
                        if (propertyValue != null) {
                            if (Tools_1.AppTool.IsNullOrEmpty(propertyValue.toString())) {
                                viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
                            }
                        }
                        else {
                            viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
                        }
                    }
                    else if (!objectField.IsRequiered && viewModelBase.UIProperties) {
                        viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, false);
                    }
                }
            }
        }
    };
    RulesValidator.prototype.ApplyConditionalBlockFieldRules = function (propertyName, entity, objectTableName, onPropertyChange, uiPoperty) {
        if (uiPoperty === void 0) { uiPoperty = null; }
        //if (TenantContext.Current.CurrentSession == null) {
        //    return
        //}
        //var _type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        var propertyValue = null;
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var fieldFormat = "[" + propertyName + "]";
        var advancedConditionalTableBlockRules = this._blockRules.filter(function (r) { return (r.Condition != null) && r.AdvancedCondition == true && r.TriggerTypeCode == "COND" && r.ObjectTableId == table.Id; });
        var conditionalTableBlockRules = this._blockRules.filter(function (r) { return r.AdvancedCondition == false && r.RuleConditionFields.length > 0 && r.TriggerTypeCode == "COND" && r.ObjectTableId == table.Id; });
        for (var k in conditionalTableBlockRules) {
            var rule = conditionalTableBlockRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            if (rule.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; }) || ruleFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                //var canRun: boolean = CanRunRule(rule, table.Id, entity);
                //if (canRun) {
                var validcondition = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
                if (rule.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                    this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, uiPoperty);
                }
                else {
                    this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, uiPoperty);
                }
                //}
            }
        }
        ;
        //type1 = entity.GetType();
        //propertyInf = _type1.GetProperty(propertyName);
        //if (propertyInf != null) {
        propertyValue = entity[propertyName]; //propertyInf.GetValue(entity, null);
        for (var k in advancedConditionalTableBlockRules) {
            rule = advancedConditionalTableBlockRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            if (rule.Condition.indexOf(fieldFormat) != -1 || ruleFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                //// var canRun: boolean = CanRunRule(rule, table.Id, entity);
                // //if (canRun) {
                //     var ruleCondition: string = rule.Condition;
                //     var blocked: boolean = ValidateConditionExpression(ruleCondition, entity, rule.ObjectTableId);
                //     if (onPropertyChange) {
                //         var cancel: boolean = false;
                //         ruleFields.forEach(function (f) {
                //             if (ruleCondition.Contains(f.ObjectFieldName)) {
                //                 cancel = true;
                //                 break;
                //             }
                //         });
                //         if (cancel) {
                //             return
                //         }
                //     }
                //     if (!ruleFields.some(f => f.ObjectFieldName == propertyName)) {
                //         SetFieldsAccessibility(ruleFields, viewModelBase, blocked, objectTableName, entity);
                //     }
                //     else {
                //         SetFieldsAccessibility(ruleFields.filter(f => f.ObjectFieldName == propertyName), viewModelBase, blocked, objectTableName, entity);
                //     }
                // //}
            }
        } //);
        //}
    };
    RulesValidator.prototype.ApplyUnConditionalBlockFieldRules = function (propertyName, entity, objectTableName, onPropertyChange, uiPoperty) {
        if (uiPoperty === void 0) { uiPoperty = null; }
        var propertyValue = null;
        var table = this._objectTables.filter(function (t) { return t.Name == objectTableName && (t.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || t.Tenant == 0); })[0];
        if (table == null || table == undefined) {
            return;
        }
        var fieldFormat = "[" + propertyName + "]";
        var unconditionalTableBlockRules = this._blockRules.filter(function (r) { return r.TriggerTypeCode == "ALLW" && r.ObjectTableId == table.Id; });
        for (var k in unconditionalTableBlockRules) {
            var rule = unconditionalTableBlockRules[k];
            var ruleFields = this._objectTableRuleFields.filter(function (rf) { return rf.ObjectTableRuleId == rule.Id; });
            if (rule.RuleConditionFields.some(function (f) { return f.ObjectFieldName == propertyName; }) || ruleFields.some(function (f) { return f.ObjectFieldName == propertyName; })) {
                this.SetFieldsAccessibility(ruleFields, true, objectTableName, entity, uiPoperty);
            }
        }
        ;
    };
    RulesValidator.prototype.SetFieldsAccessibility = function (ruleFields, validcondition, objectTableName, entity, uiPoperty) {
        if (uiPoperty) {
            var ruleField = ruleFields.filter(function (f) { return f.ObjectFieldName == uiPoperty.FieldName; })[0];
            if (ruleField) {
                //if (objectField.FieldName == uiPoperty.FieldName) {
                //if (entity != null) {
                if (validcondition) {
                    //entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, false);
                    uiPoperty.IsEnabled = false;
                }
                else {
                    //entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, true);
                    uiPoperty.IsEnabled = true;
                }
                // }
                //}
            }
        }
        else {
            for (var k in ruleFields) {
                var ruleField = ruleFields[k];
                var objectField = this._tenantObjectFields.filter(function (f) { return f.Id == ruleField.ObjectFieldId; })[0];
                if (objectField != null && objectField.AutomaticField == false) {
                    if (entity != null) {
                        if (validcondition) {
                            entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, false);
                        }
                        else {
                            entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, true);
                        }
                    }
                }
            }
        }
        //ruleFields.forEach(function (ruleField) {
        //    ruleField.ObjectFieldName
        //    var objectField: ObjectFieldPM = this._tenantObjectFields.filter(f=> f.Id == ruleField.ObjectFieldId)[0];
        //    if (objectField.FieldName == uiPoperty.FieldName) {
        //        //if (entity != null) {
        //        if (validcondition) {
        //            //entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, false);
        //            uiPoperty.IsEnabled = false;
        //        }
        //        else {
        //            //entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, true);
        //            uiPoperty.IsEnabled = true;
        //        }
        //        // }
        //    }
        //});
    };
    RulesValidator.prototype.ValidateConditionFieldsRule = function (entity, ruleConditionFields) {
        var validcondition = true;
        // var type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        for (var k in ruleConditionFields) {
            var condfield = ruleConditionFields[k];
            var fieldPM = this._tenantObjectFields.filter(function (f) { return f.Id == condfield.ObjectFieldId; })[0];
            var value = entity[condfield.ObjectFieldName];
            var valueString = FieldValueResolver_1.FieldValueResolver.GetFieldStringValue(fieldPM, value);
            var fieldValue = condfield.Value;
            if (valueString)
                valueString = valueString.toLowerCase();
            if (fieldValue)
                fieldValue = fieldValue.toLowerCase();
            switch (condfield.Operator) {
                case "Equals":
                    validcondition = (fieldValue == valueString);
                    break;
                case "NotEqual":
                    validcondition = (fieldValue != valueString);
                    break;
                default:
                    validcondition = (fieldValue == valueString);
                    break;
            }
            if (validcondition === false)
                break;
        }
        return validcondition;
    };
    RulesValidator.prototype.GenerateRuleErrors = function (ruleFields, entity, requiredFields) {
        //var type1: Type = entity.GetType();
        // var propertyInf: PropertyInfo = null;
        var propertyValue = null;
        for (var k in ruleFields) {
            var ruleField = ruleFields[k];
            //var objectField: ObjectFieldPM = ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
            var objectField = this._tenantObjectFields.filter(function (f) { return f.Id == ruleField.ObjectFieldId; })[0];
            if (objectField != null) { // ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
                if (ruleField.RuleNotificationTypeCode == "ERR") {
                    //propertyInf = type1.GetProperty(objectField.FieldName);
                    //if (propertyInf != null) {
                    //propertyValue = propertyInf.GetValue(entity, null);
                    propertyValue = entity[objectField.FieldName];
                    if (objectField.IsCustom && propertyValue != null && propertyValue != undefined) {
                        propertyValue = propertyValue.Value;
                    }
                    if (objectField.DataTypeCode == "Text") {
                        if (propertyValue == null) {
                            if (!requiredFields.some(function (r) { return r.Id == ruleField.Id; })) {
                                requiredFields.push(ruleField);
                            }
                        }
                        else {
                            if (propertyValue.toString().trim() == "") {
                                if (!requiredFields.some(function (r) { return r.Id == ruleField.Id; })) {
                                    requiredFields.push(ruleField);
                                }
                            }
                        }
                    }
                    else {
                        if (propertyValue == null) {
                            if (!requiredFields.some(function (r) { return r.Id == ruleField.Id; })) {
                                requiredFields.push(ruleField);
                            }
                        }
                    }
                    // }
                }
            }
        }
    };
    RulesValidator.prototype.HasProperty = function (entity, property) {
        var entityKeys = Object.keys(entity);
        return (entityKeys.some(function (k) { return k == property; }));
    };
    return RulesValidator;
}());
exports.RulesValidator = RulesValidator;
//public static ZipFilesDictionary: { [TableName: string]: Array<ZipFileDetails>; } = {};
//AppliedRules: Dictionary<string, Dictionary<string, Object>> = new Dictionary<string, Dictionary<string, Object>>();
//private CanRunRule(rule: ObjectTableRulePM, objectTableId: string, entity: Object): boolean {
//    var enableRun: boolean = false;
//    if (!AppliedRules.Keys.Contains(rule.RuleCode)) {
//        var currentConditionFieldsValues: Dictionary<string, Object> = GetRuleConditionFieldCurrentValues(rule, objectTableId, entity);
//        AppliedRules.Add(rule.RuleCode, currentConditionFieldsValues);
//        enableRun = true;
//    }
//    else {
//        var oldConditionFieldsValues: Dictionary<string, Object> = AppliedRules[rule.RuleCode];
//        var currentConditionFieldsValues: Dictionary<string, Object> = GetRuleConditionFieldCurrentValues(rule, objectTableId, entity);
//        var equals: boolean = new DictionaryComparer<string, Object>().Equals(oldConditionFieldsValues, currentConditionFieldsValues);
//        if (!equals) {
//            AppliedRules[rule.RuleCode] = currentConditionFieldsValues;
//        }
//        enableRun = !equals;
//    }
//    return enableRun;
//}
//}
//# sourceMappingURL=RulesValidator.js.map