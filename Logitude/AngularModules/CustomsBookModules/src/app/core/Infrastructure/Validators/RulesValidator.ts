import { ObjectTableRulePM } from '../EntityPMs/ObjectTableRulePM';
import { RuleConditionFieldPM } from '../EntityPMs/RuleConditionFieldPM';
import { ObjectTableRuleFieldPM } from '../EntityPMs/ObjectTableRuleFieldPM';
import { Settings } from '../Settings';
import { SessionInfo } from '../Utilities/SessionInfo';
import { ObjectFieldPM } from '../EntityPMs/ObjectFieldPM';
import { TextCodeTranslator } from '../Utilities/TextCodeTranslator';
import { AppTool } from '../Tools';
import { FieldValueResolver } from '../Utilities/FieldValueResolver';
import { ObjectTablePM } from '../EntityPMs/ObjectTablePM';
// import { UIProperty, UIProperties, UIPropertyArgs } from '../Components/LogitudeComponents/UIProperties';
import { CustomFieldClass } from '../DataContracts/CustomFieldClass';
import { EntityListService } from '../Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../DataContracts/ApiQueryFilters';
import { DateTool } from '../Tools';

declare var window: any;

export class RulesValidator {
	public IsNewEntity: boolean;
	private _requiredFieldRules: Array<ObjectTableRulePM> = [];
	private _entityLevelRules: Array<ObjectTableRulePM> = [];
	private _blockRules: Array<ObjectTableRulePM> = [];
	private _tenantRules: Array<ObjectTableRulePM> = [];
	private _tenantObjectFields: Array<ObjectFieldPM> = [];
	private _objectTableRuleFields: Array<ObjectTableRuleFieldPM> = [];
	private _objectTables: Array<ObjectTablePM> = [];

	private _unConditionalsetFieldValueRules: Array<ObjectTableRulePM> = [];
	private _conditionalSetFieldValueRules: Array<ObjectTableRulePM> = [];
	private _triggeredSetFieldValueRules: Array<ObjectTableRulePM> = [];
	private entityListService: EntityListService;
	constructor() {
		this.Initizialize();
	}

	public Initizialize(entity: any = null) {
		this.SetIsNewEntity(entity);

		if (window.ObjectTableRules != null && window.ObjectTableRules != undefined) {
			this._tenantRules = window.ObjectTableRules;

			this._objectTables = window.ObjectTables;

			this._objectTableRuleFields = window.ObjectTableRuleFields;

			this._tenantObjectFields = window.ObjectFields;

			if (this.IsNewEntity) {
				this._requiredFieldRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'REQ' && r.InActive == false && r.InActive == false && r.ActiveForNew == this.IsNewEntity,
				);

				this._entityLevelRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'EVAL' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForNew == this.IsNewEntity,
				);

				this._blockRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'BLCK' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForNew == this.IsNewEntity,
				);

				this._unConditionalsetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'ALLW' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForNew == this.IsNewEntity,
				);

				this._conditionalSetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'COND' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForNew == this.IsNewEntity,
				);

				this._triggeredSetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'FLDC' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForNew == this.IsNewEntity,
				);
			} else {
				this._requiredFieldRules = this._tenantRules.filter(
					(r) => r.RuleTypeCode == 'REQ' && r.InActive == false && r.InActive == false && r.ActiveForUpdate,
				);

				this._entityLevelRules = this._tenantRules.filter(
					(r) => r.RuleTypeCode == 'EVAL' && r.InActive == false && r.InActive == false && r.ActiveForUpdate,
				);

				this._blockRules = this._tenantRules.filter(
					(r) => r.RuleTypeCode == 'BLCK' && r.InActive == false && r.InActive == false && r.ActiveForUpdate,
				);

				this._unConditionalsetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'ALLW' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForUpdate,
				);

				this._conditionalSetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'COND' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForUpdate,
				);

				this._triggeredSetFieldValueRules = this._tenantRules.filter(
					(r) =>
						r.RuleTypeCode == 'SETV' &&
						r.TriggerTypeCode == 'FLDC' &&
						r.InActive == false &&
						r.InActive == false &&
						r.ActiveForUpdate,
				);
			}

			this.entityListService = new EntityListService();

			//this._triggeredSetFieldValueRules = (from r in TenantContext.Current.ObjectTableRules
			//where(r.RuleTypeCode == "SETV") && (r.TriggerTypeCode == "FLDC") && r.InActive == false && r.InActive == false && (r.ActiveForNew == this.IsNewEntity || r.ActiveForUpdate)
			//select r).ToList();//TenantContext.Current.ObjectTableRules.
		}
	}
	public ValidateAllTableRules(entity: any, objectTableId: string, errorsArray: Array<string>) {
		this.ValidateAllRequiredFieldRules(entity, objectTableId, errorsArray);
		this.ValidateEntityRules(entity, objectTableId, errorsArray);
		//this.val
		return errorsArray;
	}

	public ApplyEntityChangedRules(propertyName: string, entity: any, objectTableName: string) {
		//if (this.CurrentSession && this.CurrentSession.CurrentEditComponent) {
		//    this.CurrentSession.CurrentEditComponent.ChangeDetectorRef.detach();
		//}

		this.Initizialize(entity);
		this.ApplyRequiredFieldRules(propertyName, entity, objectTableName);
		this.ApplyConditionalBlockFieldRules(propertyName, entity, objectTableName, true);

		this.ApplyTriggeredSetFieldValueRules(propertyName, entity, objectTableName);
		this.ApplyConditionalSetFieldRules(propertyName, entity, objectTableName, true);
		//if (this.CurrentSession && this.CurrentSession.CurrentEditComponent) {
		//    this.CurrentSession.CurrentEditComponent.ChangeDetectorRef.detectChanges();
		//}
	}

	private SetIsNewEntity(entity: any) {
		if (entity) {
			this.IsNewEntity =
				(entity.OldEntityPM === null || entity.OldEntityPM === undefined) &&
				(entity.Id === null || entity.Id === undefined);
		}
	}

	//***********************************************************************************************
	public ApplyTriggeredSetFieldValueRules(propertyName: string, entity: any, objectTableName: string): void {
		if (Settings.DisableRuleValidation) {
			return;
		}

		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var objectTableId: string = table.Id;
		var propertyValue: Object = null;
		var entityTableRules: Array<ObjectTableRulePM> = this._triggeredSetFieldValueRules.filter(
			(o) => o.ObjectTableId == objectTableId,
		);
		for (var k in entityTableRules) {
			var rule: ObjectTableRulePM = entityTableRules[k];

			var field: ObjectFieldPM = this._tenantObjectFields.filter((x) => x.FieldCode === rule.TriggerFieldCode)[0];

			if (field && field.FieldName == propertyName && entity.OldEntityPM) {
				this.ExecuteSetFieldsRule(rule, entity, objectTableName);
			}
		}
	}

	private ExecuteSetFieldsRule(rule: ObjectTableRulePM, entity: Object, objectTableName: string) {
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
			(rf) => rf.ObjectTableRuleId == rule.Id,
		);
		for (var key in ruleFields) {
			var ruleField: ObjectTableRuleFieldPM = ruleFields[key];

			var expression: string = ruleField.Expression;
			if (expression != null) {
				var FieldValue: string;
				if (expression.indexOf('[') >= 0) {
					//for (var m in expression.split('')) {
					//    var ch = expression[m];
					//    if(ch !=
					//}

					var fieldName: string = expression.substring(expression.lastIndexOf('[') + 1, expression.lastIndexOf(']'));
					if (fieldName.indexOf('.') === -1) {
						this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, entity[fieldName], table.Id);
					} else {
						this.SetInsideEntityFieldValue(entity, entity, fieldName, ruleField, table);
					}
				} else {
					//ConditionFieldsDic.Add(ruleField.ObjectFieldName, expression);
					//object value = expressionValidation.ExecuteValueExpression(ruleField.ObjectFieldName, ConditionFieldsDic);
					this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, expression, table.Id);
				}
			} else {
				this.SetEntityFieldValue(entity, ruleField.ObjectFieldName, null, table.Id);
			}
		}
	}

	private SetInsideEntityFieldValue(
		parentEntity: Object,
		entity: Object,
		fieldName: string,
		ruleField: ObjectTableRuleFieldPM,
		table: ObjectTablePM,
	) {
		var apiFilters: ApiQueryFilters = new ApiQueryFilters();

		var currentEntity = entity;
		var fieldsAray: string[] = fieldName.split('.');
		// while (true) { start
		var i: number = 0;
		var f1: string = fieldsAray[0]; //"IncotermId.PerpaidCollect.Id"
		var currentValue: string = currentEntity[fieldsAray[i]];
		var objectField: ObjectFieldPM = this._tenantObjectFields.filter(
			(x) => x.FieldName === fieldsAray[i] && x.ObjectTableId === table.Id,
		)[0];
		if (objectField && objectField.DataTypeCode == 'DateTime') {
			if (fieldsAray[i + 1] == 'Date') {
				var datetimevalue: Date = currentEntity[fieldsAray[i]];
				var datevalue = DateTool.TruncateTime(datetimevalue);
				this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, datevalue, table.Id);
			} else this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, currentValue, table.Id);
		}
		if (objectField && currentValue && objectField.DataTypeCode == 'LookUp') {
			var insideEntityName: string = objectField.ObjectTable_LookUpTableName;
			var insideTable: ObjectTablePM = this._objectTables.filter(
				(t) => t.Name == insideEntityName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
			)[0];

			var insideObjectField: ObjectFieldPM = this._tenantObjectFields.filter(
				(f) => f.FieldName === fieldsAray[i + 1] && f.ObjectTableId === insideTable.Id,
			)[0];

			if (insideTable.CacheOnClient) {
				this.entityListService.getSingleFromCache(currentValue, insideEntityName, apiFilters).then((res: any) => {
					res.subscribe((response: any) => {
						var insideEntity = response.Result;
						if (insideEntity) {
							if (i + 1 < fieldsAray.length) {
								var insideValue: any = insideEntity[fieldsAray[i + 1]];
								if (insideObjectField && insideObjectField.DataTypeCode == 'LookUp') {
									var insideEntityName2: string = insideObjectField.ObjectTable_LookUpTableName;
									var insideTable2: ObjectTablePM = this._objectTables.filter(
										(t) => t.Name == insideEntityName2 && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
									)[0];

									var insideFields = fieldName.replace(fieldsAray[i] + '.', '');
									this.SetInsideEntityFieldValue(parentEntity, insideEntity, insideFields, ruleField, insideTable);
								} else {
									this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, insideValue, table.Id);
								}
							} else {
							}
						} else {
							// break;
						}
					});
				});
			} else {
				this.entityListService.getSingle(currentValue, insideEntityName).then((res: any) => {
					res.subscribe((response: any) => {
						var insideEntity = response.Result;
						if (insideEntity) {
							if (i + 1 < fieldsAray.length) {
								var insideValue: any = insideEntity[fieldsAray[i + 1]];
								if (insideObjectField && insideObjectField.DataTypeCode == 'LookUp') {
									var insideEntityName2: string = insideObjectField.ObjectTable_LookUpTableName;
									var insideTable2: ObjectTablePM = this._objectTables.filter(
										(t) => t.Name == insideEntityName2 && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
									)[0];

									var insideFields = fieldName.replace(fieldsAray[i] + '.', '');
									this.SetInsideEntityFieldValue(parentEntity, insideEntity, insideFields, ruleField, insideTable);
								} else {
									this.SetEntityFieldValue(parentEntity, ruleField.ObjectFieldName, insideValue, table.Id);
								}
							} else {
							}
						} else {
							// break;
						}
					});
				});
			}
		} else {
			// break;
		}

		//} end
	}

	private SetEntityFieldValue(entity: Object, propertyName: string, value: any, objectTableId: string): void {
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Id == objectTableId && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		var field: ObjectFieldPM = this._tenantObjectFields.filter(
			(a) => a.FieldName == propertyName && a.ObjectTableId == objectTableId,
		)[0];
		var resultValue: Object = ' ';
		if (value != undefined && value != null && value instanceof CustomFieldClass) {
			value = value.ResolvedValue;
		}
		if (field !== null) {
			const textValue = value + '';
			if (value && field.MaxLength < textValue.length) {
				value = textValue.substring(0, field.MaxLength);
			}
			if (field.IsCustom) {
				const classvalue: CustomFieldClass = entity[propertyName];
				if (classvalue != null) {
					if (classvalue.Value != value) {
						const customFieldClass: CustomFieldClass = new CustomFieldClass(value, propertyName, table.Name);
						entity[propertyName] = customFieldClass;
					}
				} else {
					const customFieldClass: CustomFieldClass = new CustomFieldClass(value, propertyName, table.Name);
					entity[propertyName] = customFieldClass;
				}
			} else {
				if (entity[propertyName] != value) {
					entity[propertyName] = value;
				}
			}
		} else {
			if (entity[propertyName] != value) {
				entity[propertyName] = value;
			}
		}
	}

	public ApplyAllEntityStaticRules(entity: any, objectTableName: string) {
		this.ApplyStaticConditionalSetFieldRules(entity, objectTableName);
		this.ApplyStaticUnConditionalSetFieldRules(entity, objectTableName);
	}

	public ApplyStaticConditionalSetFieldRules(entity: any, objectTableName: string): void {
		if (Settings.DisableRuleValidation) {
			return;
		}
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var advancedConditionalTableSetValueRules: Array<ObjectTableRulePM> = this._conditionalSetFieldValueRules.filter(
			(r) =>
				r.Condition != null &&
				r.AdvancedCondition == true &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		var conditionalTableSetValueRules: Array<ObjectTableRulePM> = this._conditionalSetFieldValueRules.filter(
			(r) =>
				r.AdvancedCondition == false &&
				r.RuleConditionFields.length > 0 &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		for (var k in conditionalTableSetValueRules) {
			var rule = conditionalTableSetValueRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);

			var validcondition: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);

			if (validcondition) {
				this.ExecuteSetFieldsRule(rule, entity, objectTableName);
			}
		}
	}

	public ApplyStaticUnConditionalSetFieldRules(entity: any, objectTableName: string): void {
		if (Settings.DisableRuleValidation) {
			return;
		}
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var tableSetValueRules: Array<ObjectTableRulePM> = this._unConditionalsetFieldValueRules.filter(
			(r) => r.ObjectTableId == table.Id,
		);

		for (var k in tableSetValueRules) {
			var rule = tableSetValueRules[k];
			this.ExecuteSetFieldsRule(rule, entity, objectTableName);
		}
	}
	//=================================================================================================================
	public ApplyConditionalSetFieldRules(
		propertyName: string,
		entity: any,
		objectTableName: string,
		onPropertyChange: boolean,
	): void {
		if (Settings.DisableRuleValidation) {
			return;
		}
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}
		var fieldFormat: string = '[' + propertyName + ']';
		var advancedConditionalTableSetValueRules: Array<ObjectTableRulePM> = this._conditionalSetFieldValueRules.filter(
			(r) =>
				r.Condition != null &&
				r.AdvancedCondition == true &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		var conditionalTableSetValueRules: Array<ObjectTableRulePM> = this._conditionalSetFieldValueRules.filter(
			(r) =>
				r.AdvancedCondition == false &&
				r.RuleConditionFields.length > 0 &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		for (var k in conditionalTableSetValueRules) {
			var rule = conditionalTableSetValueRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			if (
				rule.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName) ||
				(ruleFields.some((f) => f.ObjectFieldName == propertyName) && !onPropertyChange)
			) {
				//var canRun: boolean = CanRunRule(rule, table.Id, entity);
				//if (canRun) {
				var validcondition: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
				//if (rule.RuleConditionFields.some(f => f.ObjectFieldName == propertyName)) {
				//    this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, uiPoperty);
				//}
				if (validcondition) {
					this.ExecuteSetFieldsRule(rule, entity, objectTableName);
				}
			}
		}
		//type1 = entity.GetType();
		//propertyInf = _type1.GetProperty(propertyName);
		//if (propertyInf != null) {
		propertyValue = entity[propertyName]; //propertyInf.GetValue(entity, null);
		for (var k in advancedConditionalTableSetValueRules) {
			rule = advancedConditionalTableSetValueRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			if (rule.Condition.indexOf(fieldFormat) != -1 || ruleFields.some((f) => f.ObjectFieldName == propertyName)) {
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
	}

	public ApplyUnConditionalSetFieldRules(propertyName: string, entity: any, objectTableName: string): void {
		if (Settings.DisableRuleValidation) {
			return;
		}
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var tableSetValueRules: Array<ObjectTableRulePM> = this._unConditionalsetFieldValueRules.filter(
			(r) => r.ObjectTableId == table.Id,
		);

		for (var k in tableSetValueRules) {
			var rule = tableSetValueRules[k];
			this.ExecuteSetFieldsRule(rule, entity, objectTableName);
		}
	}

	//***********************************************************************************************

	public ValidateAllRequiredFieldRules(entity: any, objectTableId: string, errorsArray: Array<string>) {
		var requiredFields: Array<ObjectTableRuleFieldPM> = [];

		if (Settings.DisableRuleValidation) {
			return requiredFields;
		}

		var tableRules = this._requiredFieldRules.filter(
			(a) => a.ObjectTableId == objectTableId && (a.Tenant == SessionInfo.LoggedUserTenant || a.Tenant == 0),
		);

		// tableRules.forEach(function (rule) {
		//    this.ValidateRequierdFieldRule(rule.RuleCode, entity);
		//  });

		for (var k in tableRules) {
			this.ExecuteRequierdFieldRule(tableRules[k].RuleCode, entity, requiredFields);
		}

		if (requiredFields.length != 0) {
			for (var k in requiredFields) {
				var field = requiredFields[k];
				var obField = this._tenantObjectFields.filter((x) => x.FieldCode === field.ObjectFieldCode)[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
				var requiredError = TextCodeTranslator.Translate('General.M.FieldIsRequired');
				var fieldTrans = TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
				requiredError = requiredError.replace('%FieldName', fieldTrans);
				errorsArray.push(requiredError);
			}
		}

		//return requiredFields;
	}

	public ValidateEntityRules(entity: Object, objectTableId: string, errorsArray: Array<string>): boolean {
		if (Settings.DisableRuleValidation) {
			return true;
		}
		//var outputMessage1: string = "";
		var isValid: boolean = true;
		//var expressionValidation: ExpressionValidation = null;
		var tableRules: Array<ObjectTableRulePM> = this._entityLevelRules.filter((r) => r.ObjectTableId == objectTableId);
		for (var k in tableRules) {
			var rule = tableRules[k];
			//if (CanRunRule(rule, rule.ObjectTableId, entity)) {
			// if (expressionValidation == null) {
			//   expressionValidation = new ExpressionValidation();
			// }
			var haserror: boolean = false;
			if (rule.AdvancedCondition && rule.Condition != null) {
				//haserror = ValidateConditionExpression(rule.Condition, entity, rule.ObjectTableId);
			}
			if (rule.AdvancedCondition == false && rule.RuleConditionFields.length > 0) {
				haserror = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
			}
			if (haserror) {
				if (!AppTool.IsNullOrEmpty(rule.OutputMessage)) {
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
	}

	public ExecuteRequierdFieldRule(ruleCode: string, entity: any, requiredFields: Array<ObjectTableRuleFieldPM>) {
		var rule: ObjectTableRulePM = this._requiredFieldRules.filter(
			(a) =>
				a.RuleCode == ruleCode && (a.Tenant == SessionInfo.LoggedUserTenant || a.Tenant == 0) && a.InActive == false,
		)[0];
		if (rule != null) {
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id && rf.RuleNotificationTypeCode == 'ERR',
			);

			// if (CanRunRule(rule, rule.ObjectTableId, entity)) {
			if (rule.AdvancedCondition && rule.Condition != null) {
				//bool required = ValidateConditionExpression(rule.Condition, entity, rule.ObjectTableId);
				//if (required) {
				// GenerateRuleErrors(ruleFields, entity, requiredFields);
				//}
			}
			if (rule.TriggerTypeCode == 'ALLW') {
				this.GenerateRuleErrors(ruleFields, entity, requiredFields);
			} else {
				if (rule.AdvancedCondition == false && rule.RuleConditionFields.length > 0) {
					var required: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);

					if (required) {
						this.GenerateRuleErrors(ruleFields, entity, requiredFields);
					}
				}
			}
		}

		return requiredFields;
	}

	public ApplyRequiredFieldRules(
		propertyName: string,
		entity: Object,
		objectTableName: string,
		viewModelBase: any = entity,
	): void {
		if (Settings.DisableRuleValidation) {
			return;
		}

		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}
		var fieldFormat: string = '[' + propertyName + ']';
		var advancedConditionalRules: Array<ObjectTableRulePM> = this._requiredFieldRules.filter(
			(r) => r.Condition != null && r.AdvancedCondition == true && r.ObjectTableId == table.Id,
		);
		var fieldsConditionalRules: Array<ObjectTableRulePM> = this._requiredFieldRules.filter(
			(r) =>
				r.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName) &&
				r.AdvancedCondition == false &&
				r.ObjectTableId == table.Id,
		);
		var unConditionalRules: Array<ObjectTableRulePM> = this._requiredFieldRules.filter(
			(r) => r.Condition == null && r.TriggerTypeCode == 'ALLW' && r.ObjectTableId == table.Id,
		);
		//var type1: Type = entity.GetType();
		//------------------ Advanced Conditional rules
		var advancedRulesToExecute: Array<ObjectTableRulePM> = advancedConditionalRules.filter(
			(r) => r.Condition.indexOf(propertyName) != -1 && r.ObjectTableId == table.Id,
		);

		var targetRuleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
			(rf) => rf.ObjectFieldName == propertyName && rf.ObjectTableRuleTypeCode == 'REQ',
		);
		for (var k in targetRuleFields) {
			var field = targetRuleFields[k];
			var currField: ObjectFieldPM = this._tenantObjectFields.filter((f) => f.FieldCode == field.ObjectFieldCode)[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
			if (currField != null) {
				if (table.Id == currField.ObjectTableId) {
					var rule: ObjectTableRulePM = this._requiredFieldRules.filter((r) => r.Id == field.ObjectTableRuleId)[0];
					if (rule != null) {
						if (!advancedRulesToExecute.some((r) => r.Id == field.ObjectTableRuleId)) {
							if (rule.AdvancedCondition && rule.Condition != null) {
								advancedRulesToExecute.push(rule);
							}
						}
						if (!fieldsConditionalRules.some((r) => r.Id == field.ObjectTableRuleId)) {
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
			if (rule.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName)) {
				// if (CanRunRule(rule, table.Id, entity)) {
				var validcondition: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
				var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
					(rf) => rf.ObjectTableRuleId == rule.Id,
				);
				this.SetRequiredFields(ruleFields, validcondition, objectTableName, entity, viewModelBase);
			}
			//}
		}
		for (var k in unConditionalRules) {
			var rule = unConditionalRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			this.SetRequiredFields(ruleFields, true, objectTableName, entity, viewModelBase);
		}
	}
	public SetRequiredFields(
		ruleFields: Array<ObjectTableRuleFieldPM>,
		required: boolean,
		objectTableName: string,
		entity: any,
		viewModelBase: any,
	): void {
		//var type1: Type = entity.GetType();
		//var propertyInf: PropertyInfo = null;
		var propertyValue: Object = null;
		for (var k in ruleFields) {
			var ruleField = ruleFields[k];
			var objectField: ObjectFieldPM = this._tenantObjectFields.filter(
				(f) => f.FieldCode == ruleField.ObjectFieldCode,
			)[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
			if (objectField != null) {
				if (this.HasProperty(entity, objectField.FieldName)) {
					if (required && entity.UIProperties) {
						propertyValue = entity[objectField.FieldName];
						if (propertyValue != null) {
							if (AppTool.IsNullOrEmpty(propertyValue.toString())) {
								entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
							}
						} else {
							entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
						}
					} else if (!objectField.IsRequiered && entity.UIProperties) {
						entity.UIProperties.SetRequired(objectField.FieldName, objectTableName, false);
					}
				} else {
					if (required && viewModelBase.UIProperties) {
						propertyValue = viewModelBase[objectField.FieldName];
						if (propertyValue != null) {
							if (AppTool.IsNullOrEmpty(propertyValue.toString())) {
								viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
							}
						} else {
							viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, required);
						}
					} else if (!objectField.IsRequiered && viewModelBase.UIProperties) {
						viewModelBase.UIProperties.SetRequired(objectField.FieldName, objectTableName, false);
					}
				}
			}
		}
	}

	public ApplyAllConditionalBlockFieldRules(entity: any, objectTableName: string): void {
		this.Initizialize(entity);
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var advancedConditionalTableBlockRules: Array<ObjectTableRulePM> = this._blockRules.filter(
			(r) =>
				r.Condition != null &&
				r.AdvancedCondition == true &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		var conditionalTableBlockRules: Array<ObjectTableRulePM> = this._blockRules.filter(
			(r) =>
				r.AdvancedCondition == false &&
				r.RuleConditionFields.length > 0 &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		for (var k in conditionalTableBlockRules) {
			var rule = conditionalTableBlockRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			var validcondition: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);

			this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, null);
		}
	}

	public ApplyConditionalBlockFieldRules(
		propertyName: string,
		entity: any,
		objectTableName: string,
		onPropertyChange: boolean,
	): void {
		//if (TenantContext.Current.CurrentSession == null) {
		//    return
		//}
		//var _type1: Type = entity.GetType();
		// var propertyInf: PropertyInfo = null;
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}
		var fieldFormat: string = '[' + propertyName + ']';
		var advancedConditionalTableBlockRules: Array<ObjectTableRulePM> = this._blockRules.filter(
			(r) =>
				r.Condition != null &&
				r.AdvancedCondition == true &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		var conditionalTableBlockRules: Array<ObjectTableRulePM> = this._blockRules.filter(
			(r) =>
				r.AdvancedCondition == false &&
				r.RuleConditionFields.length > 0 &&
				r.TriggerTypeCode == 'COND' &&
				r.ObjectTableId == table.Id,
		);
		for (var k in conditionalTableBlockRules) {
			var rule = conditionalTableBlockRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			if (
				rule.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName) ||
				ruleFields.some((f) => f.ObjectFieldName == propertyName)
			) {
				//var canRun: boolean = CanRunRule(rule, table.Id, entity);
				//if (canRun) {
				var validcondition: boolean = this.ValidateConditionFieldsRule(entity, rule.RuleConditionFields);
				if (rule.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName)) {
					this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, null);
				} else {
					this.SetFieldsAccessibility(ruleFields, validcondition, objectTableName, entity, null);
				}
				//}
			}
		}
		//type1 = entity.GetType();
		//propertyInf = _type1.GetProperty(propertyName);
		//if (propertyInf != null) {
		propertyValue = entity[propertyName]; //propertyInf.GetValue(entity, null);
		for (var k in advancedConditionalTableBlockRules) {
			rule = advancedConditionalTableBlockRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			if (rule.Condition.indexOf(fieldFormat) != -1 || ruleFields.some((f) => f.ObjectFieldName == propertyName)) {
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
	}

	public ApplyUnConditionalBlockFieldRules(
		propertyName: string,
		entity: any,
		objectTableName: string,
		onPropertyChange: boolean,
		// uiPoperty: UIProperty = null,
	): void {
		var propertyValue: Object = null;
		var table: ObjectTablePM = this._objectTables.filter(
			(t) => t.Name == objectTableName && (t.Tenant == SessionInfo.LoggedUserTenant || t.Tenant == 0),
		)[0];
		if (table == null || table == undefined) {
			return;
		}

		var fieldFormat: string = '[' + propertyName + ']';
		var unconditionalTableBlockRules: Array<ObjectTableRulePM> = this._blockRules.filter(
			(r) => r.TriggerTypeCode == 'ALLW' && r.ObjectTableId == table.Id,
		);
		for (var k in unconditionalTableBlockRules) {
			var rule = unconditionalTableBlockRules[k];
			var ruleFields: Array<ObjectTableRuleFieldPM> = this._objectTableRuleFields.filter(
				(rf) => rf.ObjectTableRuleId == rule.Id,
			);
			if (
				rule.RuleConditionFields.some((f) => f.ObjectFieldName == propertyName) ||
				ruleFields.some((f) => f.ObjectFieldName == propertyName)
			) {
				this.SetFieldsAccessibility(ruleFields, true, objectTableName, entity, null);
			}
		}
	}
	private SetFieldsAccessibility(
		ruleFields: Array<ObjectTableRuleFieldPM>,
		validcondition: boolean,
		objectTableName: string,
		entity: any,
		uiPoperty: null,
	): void {
		if (uiPoperty) {
			// var ruleField = ruleFields.filter((f) => f.ObjectFieldName == uiPoperty.FieldName)[0];
			if (ruleField) {
				//if (objectField.FieldName == uiPoperty.FieldName) {
				//if (entity != null) {
				if (validcondition) {
					//entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, false);
					// uiPoperty.IsEnabled = false;
				} else {
					//entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, true);
					// uiPoperty.IsEnabled = true;
				}
				// }
				//}
			}
		} else {
			for (var k in ruleFields) {
				var ruleField = ruleFields[k];
				var objectField: ObjectFieldPM = this._tenantObjectFields.filter(
					(f) => f.FieldCode == ruleField.ObjectFieldCode,
				)[0];
				if (objectField != null && objectField.AutomaticField == false) {
					if (entity != null) {
						if (validcondition) {
							entity.UIProperties.SetEnabled(objectField.FieldName, objectTableName, false);
						} else {
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
	}

	public ValidateConditionFieldsRule(entity: Object, ruleConditionFields: Array<RuleConditionFieldPM>) {
		var validcondition: boolean = true;
		// var type1: Type = entity.GetType();
		// var propertyInf: PropertyInfo = null;
		let fieldCurrentValue = null;
		let conditionValue = null;
		for (var k in ruleConditionFields) {
			var condfield = ruleConditionFields[k];
			var fieldPM: ObjectFieldPM = this._tenantObjectFields.filter((f) => f.FieldCode == condfield.ObjectFieldCode)[0];
			fieldCurrentValue = entity[condfield.ObjectFieldName];

			if (
				fieldPM &&
				(fieldPM.DataTypeCode === 'Integer' || fieldPM.DataTypeCode === 'Double' || fieldPM.DataTypeCode === 'Decimal')
			) {
				conditionValue = FieldValueResolver.GetFieldDataValue(fieldPM, condfield.Value);
				fieldCurrentValue = entity[condfield.ObjectFieldName];
			} else {
				fieldCurrentValue = FieldValueResolver.GetFieldStringValue(fieldPM, fieldCurrentValue);
				conditionValue = condfield.Value;
				if (fieldCurrentValue) fieldCurrentValue = fieldCurrentValue.toLowerCase();

				if (conditionValue) conditionValue = conditionValue.toLowerCase();
			}

			validcondition = this.IsValidCondition(condfield, validcondition, fieldCurrentValue, conditionValue);

			if (validcondition === false) break;
		}

		return validcondition;
	}

	private IsValidCondition(
		condfield: RuleConditionFieldPM,
		validcondition: boolean,
		fieldCurrentValue: any,
		conditionValue: any,
	) {
		switch (condfield.Operator) {
			case 'Equals':
				validcondition = fieldCurrentValue == conditionValue;
				break;
			case 'NotEqual':
				validcondition = fieldCurrentValue != conditionValue;
				break;
			case 'LargerThan':
				validcondition = fieldCurrentValue > conditionValue;
				break;
			case 'GreaterThanOrEqual':
				validcondition = fieldCurrentValue >= conditionValue;
				break;
			case 'LessThan':
				validcondition = fieldCurrentValue < conditionValue;
				break;
			case 'LessThanOrEqual':
				validcondition = fieldCurrentValue <= conditionValue;
				break;
			case 'StartsWith':
				validcondition = fieldCurrentValue?.startsWith(conditionValue);
				break;
			default:
				validcondition = fieldCurrentValue == conditionValue;
				break;
		}
		return validcondition;
	}

	private GenerateRuleErrors(
		ruleFields: Array<ObjectTableRuleFieldPM>,
		entity: Object,
		requiredFields: Array<ObjectTableRuleFieldPM>,
	): void {
		//var type1: Type = entity.GetType();
		// var propertyInf: PropertyInfo = null;
		var propertyValue: any = null;
		for (var k in ruleFields) {
			var ruleField = ruleFields[k];
			//var objectField: ObjectFieldPM = ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
			var objectField: ObjectFieldPM = this._tenantObjectFields.filter(
				(f) => f.FieldCode == ruleField.ObjectFieldCode,
			)[0];
			if (objectField != null) {
				// ObjectFieldsCachedDataProvider.GetObjectFieldById(ruleField.ObjectFieldId);
				if (ruleField.RuleNotificationTypeCode == 'ERR') {
					//propertyInf = type1.GetProperty(objectField.FieldName);
					//if (propertyInf != null) {
					//propertyValue = propertyInf.GetValue(entity, null);
					propertyValue = entity[objectField.FieldName];
					if (objectField.IsCustom && propertyValue != null && propertyValue != undefined) {
						propertyValue = propertyValue.Value;
					}
					if (objectField.DataTypeCode == 'Text') {
						if (propertyValue == null) {
							if (!requiredFields.some((r) => r.Id == ruleField.Id)) {
								requiredFields.push(ruleField);
							}
						} else {
							if (propertyValue.toString().trim() == '') {
								if (!requiredFields.some((r) => r.Id == ruleField.Id)) {
									requiredFields.push(ruleField);
								}
							}
						}
					} else {
						if (propertyValue == null) {
							if (!requiredFields.some((r) => r.Id == ruleField.Id)) {
								requiredFields.push(ruleField);
							}
						}
					}
					// }
				}
			}
		}
	}

	private HasProperty(entity: any, property: string) {
		var entityKeys: Array<string> = Object.keys(entity);
		return entityKeys.some((k) => k == property);
	}
}

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
