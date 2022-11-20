import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { GetRecordLimits } from "Workflow/Constants/GetRecordLimits";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { SortDirections } from "Workflow/Constants/SortDirections";
import { Condition } from "Workflow/Models/Condition";
import { SortDirectionList } from "Workflow/Models/SortDirectionList";
import { ReturnedField } from "Workflow/Models/ReturnedField";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { EntitiesTreeList } from "Workflow/Models/EntitiesTreeList";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";

@Component({
    templateUrl: "./GetRecordPropertiesComponent.html"
})

export class GetRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public RecordsLimit: string = null;
    public RecordsType: string = null;
    public ReturnedFields: ReturnedField[];
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public OrderBy: string;
    public SortBy: string;

    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public IsValidReturnedFields: boolean = true;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public CurrentSession = SessionLocator.SelectedSession;
    public SortDirectionListItems = new SortDirectionList().Items;

    public EntitiesTreeList: EntitiesTreeList;
    public EntitiesTreeItems: TreeSelectItem[];

    public ExcludedEntities: string[];
    public EnableAddConditions: boolean;
    public ShowConditionsOperation: boolean;
    public RecordsTypeChanged: boolean = false;

    public GetRecordLimits = GetRecordLimits;
    public GetRecordTypes = GetRecordTypes;
    public SortDirections = SortDirections;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.initializeEntitiesTreeItems();
        this.initialize();
    }

    ngOnChanges() {
        this.SortDirectionListItems = new SortDirectionList().Items;
    }

    initializeEntitiesTreeItems() {
        this.EntitiesTreeList = new EntitiesTreeList(true);
        this.EntitiesTreeItems = this.EntitiesTreeList.Items;
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Entity = this.Data["entity"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : GetRecordLimits.FirstRecord;
        this.RecordsType = this.Data["recordsType"] ? this.Data["recordsType"] : null;
        this.OrderBy = this.Data["orderBy"] ? this.Data["orderBy"] : null;
        this.SortBy = this.Data["sortBy"] ? this.Data["sortBy"] : null;

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;
        this.ReturnedFields = this.Data["returnedFields"] || [];

        this.Data["recordsLimit"] = this.RecordsLimit;
        this.Data["recordsType"] = this.RecordsType;
        this.Data["orderBy"] = this.OrderBy;
        this.Data["sortBy"] = this.SortBy;

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.initializeConditions(this.RecordsType);
        this.initializeReturnedFields(this.RecordsType);
        this.handleRecordsType(this.RecordsType);

        this.setUIProperties();
    }

    // initializeConditions(reset: boolean = false) {
    //     if (reset) {
    //         this.Conditions = [];
    //         this.ConditionsOperation = ConditionOperations.And;
    //     }
    //     if (this.Conditions.length === 0) {
    //         let condition = new Condition();

    //         let isChildEntity = this.Entity ? (this.Entity.indexOf(".") !== -1) : false;
    //         if (isChildEntity) {
    //             let entities = this.Entity.split(".");
    //             let parentEntity = entities[0];
    //             let childEntity = entities[1];
    //             let childField = this.EntitiesTreeList.getChildField(parentEntity, childEntity);
    //             let fieldCode = childEntity + "." + childField;
    //             let objectField = this.FlowObjectFields.find(o => o.FieldCode === fieldCode);
    //             let parentEntityObjectTable = ObjectTables.getByName(parentEntity);

    //             condition.field = childField;
    //             condition.fieldCode = fieldCode;
    //             condition.type = objectField ? objectField.DataTypeCode : null;
    //             condition.lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
    //             condition.picklistType = objectField && objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
    //             condition.value = "triggeringrecord_" + parentEntity + "." + (parentEntityObjectTable ? parentEntityObjectTable.KeyPropertyPath : "Id");
    //             condition.operator = ConditionOperators.EqualsField;
    //             condition.isDisabled = true;
    //         }

    //         this.Conditions.push(condition);
    //         this.IsValidConditions = false;
    //     }
    // }

    initializeConditions(recordsType: string, reset: boolean = false) {
        if (reset) {
            this.Conditions = [];
            this.ConditionsOperation = ConditionOperations.And;
        }
        if (recordsType === GetRecordTypes.ReadOnly) {
            if (this.Conditions.length === 0) {
                let condition = new Condition();
                this.Conditions.push(condition);
                this.IsValidConditions = false;
            }
        } else {
            if (this.EntityId && this.Conditions.length === 0) {
                let condition = new Condition();
                let entityKeyPropertyPath = ObjectTables.getKeyPropertyPathByName(this.Entity);
                let primaryObjectField = this.FlowObjectFields.find(o => o.ObjectTableId === this.EntityId && o.FieldName === entityKeyPropertyPath);
                condition.field = primaryObjectField ? primaryObjectField.FieldName : null;
                condition.fieldCode = primaryObjectField ? primaryObjectField.FieldCode : null;
                condition.type = primaryObjectField ? primaryObjectField.DataTypeCode : null;
                condition.operator = ConditionOperators.EqualsField;
                condition.disabled = "d,f,o";
                this.Conditions.push(condition);
                this.IsValidConditions = false;
            }
        }
    }

    initializeReturnedFields(recordsType: string, reset: boolean = false) {
        if (recordsType === GetRecordTypes.ReadOnly) {
            if (reset) {
                this.ReturnedFields = [];
            }
            if (this.EntityId && this.ReturnedFields.length === 0) {
                let entityKeyPropertyPath = ObjectTables.getKeyPropertyPathByName(this.Entity);
                let primaryObjectField = this.FlowObjectFields.find(o => o.ObjectTableId === this.EntityId && o.FieldName === entityKeyPropertyPath);
                if (primaryObjectField) {
                    let field = new ReturnedField();
                    field.fieldCode = primaryObjectField.FieldCode;
                    field.type = primaryObjectField.DataTypeCode;
                    this.ReturnedFields.push(field);
                }
                this.ReturnedFields.push(new ReturnedField());
                this.IsValidReturnedFields = false;
            }
        } else {
            this.ReturnedFields = null;
            this.IsValidReturnedFields = true;
        }
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;

        this.setUIProperties();
    }

    updateEntity(entity: string) {
        let isEntityChanged = this.Data["entity"] !== entity;
        this.Data["entity"] = entity;
        this.Entity = entity;
        this.EntityId = ObjectTables.getIdByName(entity);

        if (isEntityChanged) {
            this.updateRecordsLimit(GetRecordLimits.FirstRecord);
            this.initializeConditions(this.RecordsType, true);
            this.initializeReturnedFields(this.RecordsType, true);
        }

        this.setUIProperties();
    }

    updateRecordsLimit(recordsLimit: string) {
        this.Data["recordsLimit"] = recordsLimit;
        this.RecordsLimit = recordsLimit;
        if (recordsLimit == GetRecordLimits.FirstRecord) {
            this.updateOrderBy(null);
            this.updateSortBy(null);
        } else {
            this.updateOrderBy(SortDirections.NotSorted);
        }
    }

    updateRecordsType(recordsType: string) {
        this.Data["recordsType"] = recordsType;
        this.RecordsType = recordsType;
        this.handleRecordsType(recordsType);
        this.updateEntity(null);
        this.RecordsTypeChanged = !this.RecordsTypeChanged;
    }

    handleRecordsType(recordsType: string){
        if(recordsType == GetRecordTypes.ReadOnly){
            this.EnableAddConditions = true;
            this.ShowConditionsOperation = true;
            this.ExcludedEntities = ["Opportunity"];
        }
        else if (recordsType == GetRecordTypes.Editable) {
            this.EnableAddConditions = false;
            this.ShowConditionsOperation = false;
            this.ExcludedEntities = ["Customer", "User", "Opportunity"];
        }
    }

    updateOrderBy(orderValue: string) {
        this.Data["orderBy"] = orderValue;
        this.OrderBy = orderValue;
        if (!this.isOrderBy()) {
            this.updateSortBy(null);
        }
        this.setUIProperties();
    }

    updateSortBy(sortValue: ObjectFieldPM) {
        this.Data["sortBy"] = sortValue ? sortValue.FieldCode : null;
        this.SortBy = sortValue ? sortValue.FieldCode : null;
        this.setUIProperties();
    }

    isOrderBy() {
        return this.OrderBy && this.OrderBy != SortDirections.NotSorted;
    }

    updateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
        if (this.isOrderBy()) {
            this.UIProperties.SetRequired("SortBy", null, AppTool.IsNullOrEmpty(this.SortBy));
        } else {
            this.UIProperties.SetRequired("SortBy", null, false);
        }
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidConditions && this.IsValidReturnedFields) {
            this.setData();
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions) {
                this.ValidationErrorsList.push("Invalid Conditions");
            }

            if (!this.IsValidReturnedFields) {
                this.ValidationErrorsList.push("Invalid Selected Fields");
            }
        }
    }

    setData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
        this.Data["returnedFields"] = this.ReturnedFields;
    }

    updateSelectedField(selectedField: ObjectFieldPM, index: number) {
        this.ReturnedFields[index].fieldCode = selectedField ? selectedField.FieldCode : null;
        this.ReturnedFields[index].type = selectedField ? selectedField.DataTypeCode : null;
        this.IsValidReturnedFields = this.ReturnedFields.filter(r => r.fieldCode === null).length === 0;
    }

    addEmptyField() {
        if (this.IsValidReturnedFields) {
            this.ReturnedFields.push(new ReturnedField());
            this.IsValidReturnedFields = false;
        }
    }

    deleteField(index: number) {
        this.ReturnedFields.splice(index, 1);
        this.IsValidReturnedFields = this.ReturnedFields.filter(r => r.fieldCode === null).length === 0;
    }
}