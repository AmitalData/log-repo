import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { EntitiesTreeList } from "Workflow/Models/EntitiesTreeList";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./CreateRecordPropertiesComponent.html"
})

export class CreateRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public OnlyParentEntities: boolean = true;
    public Data: any;
    public Name: string = null;
    public RecordsLimit: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public SetValues: SetValue[];

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];

    public EntitiesTreeList: EntitiesTreeList;
    public EntitiesTreeItems: TreeSelectItem[];

    public ValidationErrorsList: string[];

    public IsValidSetValues: boolean = true;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.initializeEntitiesTreeItems();
        this.initialize();
    }

    initializeEntitiesTreeItems() {
        this.EntitiesTreeList = new EntitiesTreeList(this.OnlyParentEntities);
        this.EntitiesTreeItems = this.EntitiesTreeList.Items;
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : "One";
        this.Entity = this.Data["entity"] || null;

        this.Data["recordsLimit"] = this.RecordsLimit;

        this.SetValues = this.Data["setValues"] || [];

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.initializeSetValues();

        this.setUIProperties();
    }

    initializeSetValues(reset: boolean = false) {
        if(reset){
            this.SetValues = [];
        }
        if (this.SetValues.length == 0) {
            let setValue = new SetValue();
            this.SetValues.push(setValue);
            this.IsValidSetValues = false;
        }
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
    }

    updateIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;

        this.setUIProperties();
    }

    updateRecordsLimit(recordsLimit: string) {
        this.Data["recordsLimit"] = recordsLimit;
        this.RecordsLimit = recordsLimit;
    }

    updateEntity(entity: string) {
        let isEntityChanged = this.Data["entity"] !== entity;
        this.Data["entity"] = entity;
        this.Entity = entity;
        this.EntityId = ObjectTables.getIdByName(entity);

        if (isEntityChanged) {
            this.initializeSetValues(true);
        }

        this.setUIProperties();
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidSetValues) {

            this.setValuesData();

            //console.log(this.Data);

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidSetValues){
                this.ValidationErrorsList.push("Invalid Set Values");
            }
        }
    }

    setValuesData() {
        this.Data["setValues"] = this.SetValues;
    }

    showEntitiesTreeItem() {
        return (item: TreeSelectItem) => item.key !== "Customer" && item.key !== "User";
    }
}