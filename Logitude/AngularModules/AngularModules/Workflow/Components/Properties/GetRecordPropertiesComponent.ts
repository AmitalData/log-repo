import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { GetRecordsLimit } from "Workflow/Constants/GetRecordsLimit";
import { SortDirections } from "Workflow/Constants/SortDirections";
import { Condition } from "Workflow/Models/Condition";
import { ListItem } from "Workflow/Models/ListItem";
import { SortDirectionList } from "Workflow/Models/SortDirectionList";

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
    public ReturnedColumns: string[];
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public OrderBy: string;
    public SortBy: string;

    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public IsValidSelectedCoulmns: boolean = true;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];

    public CurrentSession = SessionLocator.SelectedSession;
    public SortDirectionListItems = new SortDirectionList().Items;
    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.initialize();
    }

    ngOnChanges() {
        this.SortDirectionListItems = new SortDirectionList().Items;
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Entity = this.Data["entity"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : GetRecordsLimit.FirstRecord
        this.OrderBy = this.Data["orderBy"] ? this.Data["orderBy"] : null;
        this.SortBy = this.Data["sortBy"] ? this.Data["sortBy"] : null;

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;

        if (this.Conditions.length == 0) {
            this.IsValidConditions = false;
            let condition = new Condition();
            this.Conditions.push(condition);
        }

        this.ReturnedColumns = this.Data["returnedColumns"] || [];

        if (this.ReturnedColumns.length == 0) {
            this.IsValidSelectedCoulmns = false;
            this.ReturnedColumns.push(null);
        }

        this.Data["recordsLimit"] = this.RecordsLimit;
        this.Data["orderBy"] = this.OrderBy;
        this.Data["sortBy"] = this.SortBy;

        this.EntityId = this.getEntityId(this.Entity);

        this.setUIProperties();
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;

        this.setUIProperties();
    }

    updateEntity(entity: any) {
        this.Data["entity"] = entity ? entity.Name : null;
        this.Entity = entity ? entity.Name : null;
        this.EntityId = this.getEntityId(entity.Name);
        this.setUIProperties();
    }

    updateRecordsLimit(recordsLimit: string) {
        this.Data["recordsLimit"] = recordsLimit;
        this.RecordsLimit = recordsLimit;
        if (recordsLimit == GetRecordsLimit.FirstRecord) {
            this.updateOrderBy(null)
            this.updateSortBy(null)
        } else {
            this.updateOrderBy(SortDirections.NotSorted)
        }
    }

    updateOrderBy(orderValue: string) {
        this.Data["orderBy"] = orderValue;
        this.OrderBy = orderValue;
        if (!this.isOrderBy()) {
            this.updateSortBy(null)
        }
        this.setUIProperties();
    }

    updateSortBy(sortValue: ObjectFieldPM) {
        this.Data["sortBy"] = sortValue ? sortValue.FieldCode : null;
        this.SortBy = sortValue ? sortValue.FieldCode : null;
        this.setUIProperties();
    }

    isAllRecords() {
        return this.RecordsLimit == GetRecordsLimit.AllRecords
    }

    isOrderBy() {
        return this.OrderBy && this.OrderBy != SortDirections.NotSorted
    }

    getObjectTablesQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("Name", "Shipment", null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }

    UpdateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
        if (this.isOrderBy()) {
            this.UIProperties.SetRequired("SortBy", null, AppTool.IsNullOrEmpty(this.SortBy));
        }else{
            this.UIProperties.SetRequired("SortBy", null, false);
        }
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidConditions && this.IsValidSelectedCoulmns) {
            this.setConditionsData();
            this.Data["returnedColumns"] = this.ReturnedColumns;

            //console.log(this.Data);

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions)
                this.ValidationErrorsList.push("Invalid Conditions");

            if (!this.IsValidSelectedCoulmns)
                this.ValidationErrorsList.push("Invalid Selected Fields");
        }
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }

    getEntityId(entity: string) {
        if (entity) {
            let entityObjectTable = (window as any).ObjectTables.filter((o: any) => o.Name === entity)[0];
            return entityObjectTable ? entityObjectTable.Id : null;
        }
        return null;
    }

    getObjectFieldsValueQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("ObjectTableId", this.EntityId, null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }

    updateSelectedColumn(selectedColumn: ObjectFieldPM, index: number) {
        this.ReturnedColumns[index] = selectedColumn ? selectedColumn.FieldCode : null;
        this.IsValidSelectedCoulmns = !this.ReturnedColumns.includes(null);
    }

    addEmptyColumn() {
        if (this.IsValidSelectedCoulmns) {
            this.ReturnedColumns.push(null);
            this.IsValidSelectedCoulmns = false;
        }
    }

    deletecoulmn(index: number) {
        this.ReturnedColumns.splice(index, 1);
        this.IsValidSelectedCoulmns = !this.ReturnedColumns.includes(null);
    }
}