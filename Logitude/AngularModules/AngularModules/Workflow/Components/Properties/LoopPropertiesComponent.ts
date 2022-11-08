import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./LoopPropertiesComponent.html"
})

export class LoopPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public Name: string = null;
    public CollectionVariable: string = null;
    public Direction: string = null;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];

    public ValidationErrorsList: string[];

    public FirstToLastDirection = { Code: "FirstToLast", Name: "First item to last item" };
    public LastToFirstDirection = { Code: "LastToFirst", Name: "Last item to first item" };

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.initializeFlowVariablesTree();
        this.initialize();
    }

    initializeFlowVariablesTree() {
        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId, true);
        this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.CollectionVariable = this.Data["collectionVariable"] || null;
        this.Direction = this.Data["direction"] ? this.Data["direction"] : this.FirstToLastDirection.Code;

        this.Data["direction"] = this.Direction;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Collection_Variable", null, AppTool.IsNullOrEmpty(this.CollectionVariable));
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;
        this.setUIProperties();
    }

    updateCollectionVariable(collectionVariable: string) {
        this.Data["collectionVariable"] = collectionVariable || null;
        this.CollectionVariable = collectionVariable || null;
        this.setUIProperties();
    }

    updateDirection(direction: string) {
        this.Data["direction"] = direction;
        this.Direction = direction;
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
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError ? t.ValidationError.replace(/\_/gi, " ") : null });
            this.ValidationErrorsList = validationErrors;
        }
    }
}