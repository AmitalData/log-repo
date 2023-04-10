import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { GetRecordLimits } from "Workflow/Constants/GetRecordLimits";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { SortDirections } from "Workflow/Constants/SortDirections";
import { Condition } from "Workflow/Models/Condition";
import { SortDirectionList } from "Workflow/Lists/SortDirectionList";
import { ReturnedField } from "Workflow/Models/ReturnedField";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { EntitiesTreeList } from "Workflow/TreeLists/EntitiesTreeList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectFieldsTreeList } from "Workflow/TreeLists/ObjectFieldsTreeList";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./GetRecordPropertiesComponent.html"
})

export class GetRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
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

    public CurrentSession = SessionLocator.SelectedSession;
    public SortDirectionListItems = new SortDirectionList().Items;

    public EntitiesTreeItems: TreeSelectItem[];
    public ObjectFieldsTreeItems: TreeSelectItem[];

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
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initialize();
        this.initializeEntitiesTreeItems();
        this.initializeObjectFieldsTreeItems();
    }

    initializeWindowEvents() {
        this.CurrentSession.CurrentWindow.FooterButtonsClicked.subscribe((e: any) => {
            if (e === "submit") {
                this.saveButtonClicked();
            } else {
                this.cancelButtonClicked();
            }
        });
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
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
                let primaryObjectField = ObjectFields.getByObjectTableId(this.EntityId).find(o => o.FieldName === entityKeyPropertyPath);
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
                let primaryObjectField = ObjectFields.getByObjectTableId(this.EntityId).find(o => o.FieldName === entityKeyPropertyPath);
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

    initializeEntitiesTreeItems() {
        this.EntitiesTreeItems = new EntitiesTreeList("parent").Items;
    }

    initializeObjectFieldsTreeItems() {
        this.ObjectFieldsTreeItems = new ObjectFieldsTreeList(this.EntityId).Items;
    }

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateEntity(entity: string) {
        let isEntityChanged = this.Data["entity"] !== entity;
        this.Data["entity"] = entity;
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(entity);
        this.Entity = entity;
        this.EntityId = ObjectTables.getIdByName(entity);

        if (isEntityChanged) {
            this.updateRecordsLimit(GetRecordLimits.FirstRecord);
            this.initializeConditions(this.RecordsType, true);
            this.initializeReturnedFields(this.RecordsType, true);
            this.initializeObjectFieldsTreeItems();
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

    handleRecordsType(recordsType: string) {
        if (recordsType === GetRecordTypes.ReadOnly) {
            this.EnableAddConditions = true;
            this.ShowConditionsOperation = true;
        }
        else if (recordsType === GetRecordTypes.Editable) {
            this.EnableAddConditions = false;
            this.ShowConditionsOperation = false;
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

    updateSortBy(fieldCode: string) {
        this.Data["sortBy"] = fieldCode || null;
        this.SortBy = fieldCode || null;
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
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && this.IsValidConditions && this.IsValidReturnedFields && isValidName) {
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

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }

    setData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
        this.Data["returnedFields"] = this.ReturnedFields;
    }

    updateSelectedField(objectFieldItem: TreeSelectItem, index: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        this.ReturnedFields[index].fieldCode = objectField ? objectField.FieldCode : null;
        this.ReturnedFields[index].type = objectField ? objectField.DataTypeCode : null;
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