import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";

@Component({
    templateUrl: "./DeleteItemPropertiesComponent.html"
})

export class DeleteItemPropertiesComponent extends BaseComponent {

    public DataContext: any = this;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public FlowVariablesTreeItems: TreeSelectItem[];

    public Data: any;
    public Name: string = null;
    public Record: string;
    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
    }

    ngOnInit() {
        this.initialize();
        this.initializeFlowVariablesTreeItems();
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Record = this.Data["record"] || null;

        this.setUIProperties();
    }

    initializeFlowVariablesTreeItems() {
        let showVariables = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            ShowRecordsCollectionVariables: false,
            ShowDeclaredCollectionVariables: false
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId, showVariables, true).Items;
    }

    updateName(name: string) {
        this.Data["name"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateRecord(record: string) {
        this.Record = record;
        this.Data["record"] = record;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Record", null, AppTool.IsNullOrEmpty(this.Record));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;
        }
    }
}