import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { BooleanValues } from "Workflow/Constants/BooleanValues";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Lists/BooleanValuesList";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Lists/ConditionOperationsList";
import { DateTimeValueExpressionsList } from "Workflow/Lists/DateTimeValueExpressionsList";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ConditionDisabledPipe } from "Workflow/Pipes/ConditionDisabledPipe";
import { ObjectFieldPipe } from "Workflow/Pipes/ObjectFieldPipe";
import { IsDateTimeTypePipe } from "Workflow/Pipes/IsDateTimeTypePipe";
import { IsNoObjectFieldVariablePipe } from "Workflow/Pipes/IsNoObjectFieldVariablePipe";
import { IsFieldOperatorPipe } from "Workflow/Pipes/IsFieldOperatorPipe";
import { IsNoValueOperatorPipe } from "Workflow/Pipes/IsNoValueOperatorPipe";
import { ObjectFieldsTreeList } from "Workflow/TreeLists/ObjectFieldsTreeList";

@Component({
    selector: "ConditionGroups",
    templateUrl: "./ConditionGroupsComponent.html"
})

export class ConditionGroupsComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() EntityId: string;
    @Input() ShowChangedOperator: boolean = true;
    @Input() Conditions: Condition[];
    @Input() IsRootConditions: boolean = true;
    @Input() IsValidConditions: boolean = true;
    @Input() ConditionsCounter: number = 1;
    @Input() AtLeastOneCondition: boolean = false;
    @Input() IsEntityField: boolean = false;
    @Input() IsEntityFieldValue: boolean = false;
    @Input() FlowObject: any;
    @Input() CurrentNodeId: string;
    @Input() EnableAdd: boolean = true;
    @Input() IsOneLevelConditions: boolean = false;
    @Input() IsDummyField: boolean = false;
    @Input() GetFieldFromFirstCondition: boolean = false;

    @Output() ConditionsChangedEvent = new EventEmitter();

    public DataContext: any = this;
    public DateTimeValueExpressions = DateTimeValueExpressions;

    public ComboBoxMaxHeight: number = 120;

    public FlowVariablesTreeItems: TreeSelectItem[];
    public ObjectFieldsTreeItems: TreeSelectItem[];

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;
    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;
    public DateTimeValueExpressionsItems: ListItem[] = new DateTimeValueExpressionsList().Items;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeFlowVariablesTree();
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes && changes.EntityId && changes.EntityId.currentValue !== changes.EntityId.previousValue && (this.IsEntityField || this.IsEntityFieldValue)) {
            this.ObjectFieldsTreeItems = new ObjectFieldsTreeList(changes.EntityId.currentValue).Items;
        }
    }

    initializeFlowVariablesTree() {
        if (!this.IsEntityField || !this.IsEntityFieldValue) {
            let props = {
                ShowRecordsVariables: true,
                ShowDeclaredVariables: true,
                ShowGlobalVariables: true
            };
            this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
        }
    }

    updateConditionGroupOperation(operationCode: string, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;
            this.emitConditionsChanged();
        }
    }

    updateConditionEntityField(objectFieldItem: TreeSelectItem, conditionIndex: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.fieldCode) {
            if (objectField) {
                this.updateConditionFieldByObjectField(objectField, conditionIndex);
            } else {
                this.resetConditionField(conditionIndex);
            }
            this.emitConditionsChanged();
        }
    }

    updateConditionField(fieldItem: TreeSelectItem, conditionIndex: number) {
        let field = fieldItem ? fieldItem.key : null;
        if (field !== this.Conditions[conditionIndex]?.field) {
            if (field) {
                if (this.isNoObjectFieldVariable(field)) {
                    let fieldType = fieldItem.data["type"] || null;
                    let lookupType = fieldItem.data["lookupType"] || null;
                    let picklistType = fieldItem.data["picklistType"] || null;
                    this.updateConditionFieldByDeclaredVariableField(field, fieldType, lookupType, picklistType, conditionIndex);
                } else {
                    let objectField = this.getObjectField(field);
                    this.updateConditionFieldByObjectField(objectField, conditionIndex, field);
                }

                if (fieldItem && fieldItem.data && fieldItem.data["nodeId"]) {
                    this.Conditions[conditionIndex].fieldUsedFrom = fieldItem.data["nodeId"];
                } else {
                    this.Conditions[conditionIndex].fieldUsedFrom = null;
                }
            } else {
                this.resetConditionField(conditionIndex);
            }
            this.emitConditionsChanged();
        }
    }

    updateConditionFieldByObjectField(objectField: ObjectFieldList, conditionIndex: number, field: string | null = null) {
        this.Conditions[conditionIndex].fieldCode = field ? field : (objectField ? objectField.FieldCode : null);
        this.Conditions[conditionIndex].field = field ? field : (objectField ? objectField.FieldName : null);
        this.Conditions[conditionIndex].type = objectField ? objectField.DataTypeCode : null;
        this.Conditions[conditionIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
        this.Conditions[conditionIndex].picklistType = objectField && objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;

        if (this.Conditions[conditionIndex].operator !== ConditionOperators.Changed && this.Conditions[conditionIndex].operator !== ConditionOperators.IsEmpty) {
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            this.Conditions[conditionIndex].valueExpression = objectField ? (this.isDateTimeType(objectField.DataTypeCode) ? DateTimeValueExpressions.Date : null) : null;
        }

        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
    }

    updateConditionFieldByDeclaredVariableField(field: string, fieldType: string, lookupType: string, picklistType: string, conditionIndex: number) {
        this.Conditions[conditionIndex].fieldCode = field ? field : null;
        this.Conditions[conditionIndex].field = field ? field : null;
        this.Conditions[conditionIndex].type = fieldType;
        this.Conditions[conditionIndex].lookupType = lookupType;
        this.Conditions[conditionIndex].picklistType = picklistType;

        if (this.Conditions[conditionIndex].operator !== ConditionOperators.Changed && this.Conditions[conditionIndex].operator !== ConditionOperators.IsEmpty) {
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            this.Conditions[conditionIndex].valueExpression = this.isDateTimeType(fieldType) ? DateTimeValueExpressions.Date : null;
        }

        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
    }

    resetConditionField(conditionIndex: number) {
        this.Conditions[conditionIndex].fieldCode = null;
        this.Conditions[conditionIndex].field = null;
        this.Conditions[conditionIndex].type = null;
        this.Conditions[conditionIndex].lookupType = null;
        this.Conditions[conditionIndex].picklistType = null;

        if (this.Conditions[conditionIndex].operator !== ConditionOperators.Changed && this.Conditions[conditionIndex].operator !== ConditionOperators.IsEmpty) {
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            this.Conditions[conditionIndex].valueExpression = null;
            this.Conditions[conditionIndex].valueUsedFrom = null;
        }

        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
        this.Conditions[conditionIndex].fieldUsedFrom = null;
    }

    updateConditionOperator(operatorCode: string, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {
            this.updateConditionValueAccordingToUpdateOperator(operatorCode, conditionIndex);
        }
    }

    updateConditionValueAccordingToUpdateOperator(operatorCode: string, conditionIndex: number) {
        if (this.isNoValueOperator(operatorCode)) {
            let value = this.isNoValueOperator(operatorCode) ? BooleanValues.True : null;
            let valueExpression = this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null;
            this.updateConditionValue(value, conditionIndex);
            this.updateConditionValueExpression(valueExpression, conditionIndex, false);
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
        }
        else if ((this.isFieldCompareOperator(operatorCode) && !this.isFieldCompareOperator(this.Conditions[conditionIndex]?.operator)) ||
            (!this.isFieldCompareOperator(operatorCode) && this.isFieldCompareOperator(this.Conditions[conditionIndex]?.operator))) {
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            let valueExpression = this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null;
            this.updateConditionValueExpression(valueExpression, conditionIndex, false);
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
        } else if (!this.isNoValueOperator(operatorCode) && this.isNoValueOperator(this.Conditions[conditionIndex]?.operator)) {
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
        }

        this.Conditions[conditionIndex].operator = operatorCode;
        this.emitConditionsChanged();
    }

    updateConditionValue(value: string, conditionIndex: number, nodeId: string | null = null) {
        if (value !== this.Conditions[conditionIndex]?.value) {
            this.Conditions[conditionIndex].value = value ? value.toString() : null;

            this.Conditions[conditionIndex].valueUsedFrom = nodeId;

            this.emitConditionsChanged();
        }
    }

    updateConditionFieldValue(objectFieldItem: TreeSelectItem, conditionIndex: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.valueCode) {
            this.Conditions[conditionIndex].value = objectField ? objectField.FieldName : null;
            this.Conditions[conditionIndex].valueCode = objectField ? objectField.FieldCode : null;
            this.emitConditionsChanged();
        }
    }

    updateConditionValueExpression(valueExpressionCode: string, conditionIndex: number, resetValue: boolean = true) {
        if (valueExpressionCode !== this.Conditions[conditionIndex]?.valueExpression) {
            if (resetValue) {
                let value = this.isDateTimeType(this.Conditions[conditionIndex]?.type) && valueExpressionCode === DateTimeValueExpressions.PlusMinusToday ? "0" : null;
                this.updateConditionValue(value, conditionIndex);
            }
            this.Conditions[conditionIndex].valueExpression = valueExpressionCode;
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
            this.emitConditionsChanged();
        }
    }

    addCondition(conditionIndex: number, isGroup: boolean) {
        if (isGroup && this.IsOneLevelConditions) {
            return;
        }

        if (this.IsValidConditions && this.EnableAdd) {
            let condition = new Condition(isGroup);

            if (this.GetFieldFromFirstCondition && this.Conditions && this.Conditions.length > 0) {
                let firstCondition = this.Conditions[0];
                condition.field = firstCondition.field;
                condition.fieldCode = firstCondition.fieldCode;
                condition.type = firstCondition.type;
                condition.lookupType = firstCondition.lookupType;
                condition.picklistType = firstCondition.picklistType;
                condition.valueExpression = new IsDateTimeTypePipe().transform(firstCondition.type) ? DateTimeValueExpressions.Date : null;
            }

            condition.id = this.ConditionsCounter;
            if (conditionIndex === null) {
                this.Conditions.push(condition);
            } else {
                this.Conditions[conditionIndex].conditions.push(condition);
            }
            this.emitConditionsChanged("add");
        }
    }

    deleteCondition(conditionIndex: number, isGroup: boolean) {
        let condition = this.Conditions[conditionIndex];
        let isDeleteDisabled = condition ? (new ConditionDisabledPipe().transform(condition.disabled, "d")) : false;
        if (condition && !isDeleteDisabled) {
            if (isGroup && condition.conditions.length > 0) {
                let firstChildCondition = condition.conditions[0];
                if (!firstChildCondition.isGroup) {
                    firstChildCondition.isGroup = true;
                    firstChildCondition.groupOperation = condition.groupOperation;
                    firstChildCondition.conditions = condition.conditions.slice(1);
                }
                this.Conditions[conditionIndex] = firstChildCondition;
            } else {
                this.Conditions.splice(conditionIndex, 1);
            }
            this.emitConditionsChanged();
        }
    }

    emitConditionsChanged(event: any = null) {
        this.ConditionsChangedEvent.emit(event);
    }

    isNoObjectFieldVariable(field: string) {
        return new IsNoObjectFieldVariablePipe().transform(field);
    }

    isFieldCompareOperator(operatorCode: string) {
        return new IsFieldOperatorPipe().transform(operatorCode);
    }

    isNoValueOperator(operatorCode: string) {
        return new IsNoValueOperatorPipe().transform(operatorCode);
    }

    isDateTimeType(type: string) {
        return new IsDateTimeTypePipe().transform(type);
    }

    getObjectField(field: string) {
        return new ObjectFieldPipe().transform(field);
    }
}