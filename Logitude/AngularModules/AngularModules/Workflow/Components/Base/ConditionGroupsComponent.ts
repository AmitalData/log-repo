import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { BooleanValues } from "Workflow/Constants/BooleanValues";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ConditionOperatorsListsDictionary } from "Workflow/Models/ConditionOperatorsListsDictionary";
import { DateTimeValueExpressionsList } from "Workflow/Models/DateTimeValueExpressionsList";
import { FlowReader } from "Workflow/Models/FlowReader";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { Formatter } from "Workflow/Models/Formatter";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { GetObjectFieldPipe } from "Workflow/Pipes/GetObjectFieldPipe";
import { IsDateTimeTypePipe } from "Workflow/Pipes/IsDateTimeTypePipe";
import { IsDeclaredVariablePipe } from "Workflow/Pipes/IsDeclaredVariablePipe";
import { IsFieldOperatorPipe } from "Workflow/Pipes/IsFieldOperatorPipe";
import { IsNoValueOperatorPipe } from "Workflow/Pipes/IsNoValueOperatorPipe";

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
    @Input() FlowObjectFields: ObjectFieldList[];
    @Input() CurrentNodeId: string;

    @Output() ConditionsChangedEvent = new EventEmitter();

    public DataContext: any = this;
    public DateTimeValueExpressions = DateTimeValueExpressions;

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;
    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;
    public DateTimeValueExpressionsItems: ListItem[] = new DateTimeValueExpressionsList().Items;
    public ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeFlowVariablesTree();
    }

    ngOnChanges() {
        this.ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;
    }

    initializeFlowVariablesTree() {
        if (!this.IsEntityField || !this.IsEntityFieldValue) {
            this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId);
            this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
        }
    }

    updateConditionGroupOperation(operationCode: string, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;
            this.emitConditionsChanged();
        }
    }

    updateConditionEntityField(objectField: ObjectFieldPM, conditionIndex: number) {
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.fieldCode) {
            if (objectField) {
                this.updateConditionFieldByObjectField(objectField, conditionIndex);
            } else {
                this.resetConditionField(conditionIndex);
            }
            this.emitConditionsChanged();
        }
    }

    updateConditionField(field: string, conditionIndex: number) {
        if (field !== this.Conditions[conditionIndex]?.field) {
            if (field) {
                if (this.isDeclaredVariable(field)) {
                    this.updateConditionFieldByDeclaredVariableField(field, conditionIndex);
                } else {
                    let objectField = this.getObjectField(field);
                    this.updateConditionFieldByObjectField(objectField, conditionIndex, field);
                }
            } else {
                this.resetConditionField(conditionIndex);
            }
            this.emitConditionsChanged();
        }
    }

    updateConditionFieldByObjectField(objectField: ObjectFieldPM | ObjectFieldList, conditionIndex: number, field: string | null = null) {
        this.Conditions[conditionIndex].fieldCode = field ? field : (objectField ? objectField.FieldCode : null);
        this.Conditions[conditionIndex].field = field ? field : (objectField ? objectField.FieldName : null);
        this.Conditions[conditionIndex].type = objectField ? objectField.DataTypeCode : null;
        this.Conditions[conditionIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
        this.Conditions[conditionIndex].picklistType = objectField && objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
        this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
        this.Conditions[conditionIndex].value = null;
        this.Conditions[conditionIndex].valueCode = null;
        this.Conditions[conditionIndex].valueExpression = objectField ? (this.isDateTimeType(objectField.DataTypeCode) ? DateTimeValueExpressions.Date : null) : null;
        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
    }

    updateConditionFieldByDeclaredVariableField(field: string, conditionIndex: number) {
        let fieldType = field ? this.getDeclaredVariableFieldType(field) : null;
        this.Conditions[conditionIndex].fieldCode = field ? field : null;
        this.Conditions[conditionIndex].field = field ? field : null;
        this.Conditions[conditionIndex].type = fieldType;
        this.Conditions[conditionIndex].lookupType = null;
        this.Conditions[conditionIndex].picklistType = null;
        this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
        this.Conditions[conditionIndex].value = null;
        this.Conditions[conditionIndex].valueCode = null;
        this.Conditions[conditionIndex].valueExpression = this.isDateTimeType(fieldType) ? DateTimeValueExpressions.Date : null;
        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
    }

    resetConditionField(conditionIndex: number) {
        this.Conditions[conditionIndex].fieldCode = null;
        this.Conditions[conditionIndex].field = null;
        this.Conditions[conditionIndex].type = null;
        this.Conditions[conditionIndex].lookupType = null;
        this.Conditions[conditionIndex].picklistType = null;
        this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
        this.Conditions[conditionIndex].value = null;
        this.Conditions[conditionIndex].valueCode = null;
        this.Conditions[conditionIndex].valueExpression = null;
        this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
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

    updateConditionValue(value: string, conditionIndex: number) {
        if (value !== this.Conditions[conditionIndex]?.value) {
            this.Conditions[conditionIndex].value = value ? value.toString() : null;
            this.emitConditionsChanged();
        }
    }

    updateConditionFieldValue(objectField: ObjectFieldPM, conditionIndex: number) {
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
        if (this.IsValidConditions) {
            let condition = new Condition(isGroup);
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
        if (!condition.isDisabled) {
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

    showFlowVariablesTreeItem(conditionIndex: number) {
        let condition = this.Conditions[conditionIndex];
        let compareWithLookupOrPickListType: string | null = null;
        if (condition.type === FieldTypes.LookUp) {
            compareWithLookupOrPickListType = condition.lookupType;
        } else if (condition.type === FieldTypes.PickList) {
            compareWithLookupOrPickListType = condition.picklistType;
        }
        return (item: TreeSelectItem) => this.FlowVariablesTreeList.compareItemType(item, condition.type, compareWithLookupOrPickListType);
    }

    getDeclaredVariableFieldType(field: string) {
        if (field) {
            let fieldCode = Formatter.getFieldCode(field);
            let declareVariableNode = fieldCode ? FlowReader.getNodes(this.FlowObject, "declareVariableNode").find((n: any) => n.data["variableCode"] === fieldCode) : null;
            return declareVariableNode ? declareVariableNode.data["variableType"] : null;
        }
        return null;
    }

    isDeclaredVariable(field: string) {
        return new IsDeclaredVariablePipe().transform(field);
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
        return new GetObjectFieldPipe().transform(field, this.FlowObjectFields);
    }
}