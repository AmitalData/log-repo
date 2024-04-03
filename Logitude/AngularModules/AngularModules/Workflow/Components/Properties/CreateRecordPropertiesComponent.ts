import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { EntitiesTreeList } from "Workflow/TreeLists/EntitiesTreeList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./CreateRecordPropertiesComponent.html"
})

export class CreateRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public RecordsLimit: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public SetValues: SetValue[];

    public FlowObject: any;
    public CurrentNodeId: string;

    public EntitiesTreeItems: TreeSelectItem[];

    public ValidationErrorsList: string[];

    public IsValidSetValues: boolean = true;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeEntitiesTreeItems();
        this.initialize();
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

    initializeEntitiesTreeItems() {
        this.EntitiesTreeItems = new EntitiesTreeList().Items;
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : "One";
        this.Entity = this.Data["entity"] || null;

        this.Data["recordsLimit"] = this.RecordsLimit;

        this.SetValues = this.Data["setValues"] || [];

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.initializeSetValues();

        this.setUIProperties();
    }

    initializeSetValues(reset: boolean = false) {
        if (reset) {
            this.SetValues = [];
        }
        if (this.SetValues.length === 0) {
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

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

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
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && this.IsValidSetValues && isValidName) {
            this.setValuesData();
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidSetValues) {
                this.ValidationErrorsList.push("Invalid Set Values");
            }

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }

    setValuesData() {
        this.Data["setValues"] = this.SetValues;
    }
}