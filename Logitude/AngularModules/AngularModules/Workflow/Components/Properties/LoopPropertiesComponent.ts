import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { SingleEditableEntitiesTreeList } from "Workflow/TreeLists/SingleEditableEntitiesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./LoopPropertiesComponent.html"
})

export class LoopPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public CollectionVariable: string = null;
    public Direction: string = null;
    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowVariablesTreeItems: TreeSelectItem[];
    public SingleEditableEntitiesTreeItems: TreeSelectItem[];
    public ValidationErrorsList: string[];
    public FirstToLastDirection = { Code: "FirstToLast", Name: "First item to last item" };
    public LastToFirstDirection = { Code: "LastToFirst", Name: "Last item to first item" };
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeFlowVariablesTree();
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

    initializeFlowVariablesTree() {
        let props = {
            ShowRecordsCollectionVariables: true,
            ShowDeclaredCollectionVariables: true
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
        this.SingleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId).Items;
        this.FlowVariablesTreeItems = this.SingleEditableEntitiesTreeItems.concat(this.FlowVariablesTreeItems);
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.CollectionVariable = this.Data["collectionVariable"] || null;
        this.Direction = this.Data["direction"] ? this.Data["direction"] : this.FirstToLastDirection.Code;

        this.Data["direction"] = this.Direction;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Collection_Variable", null, AppTool.IsNullOrEmpty(this.CollectionVariable));
    }

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateCollectionVariable(collectionVariableItem: TreeSelectItem) {
        let collectionVariable = collectionVariableItem ? collectionVariableItem.key : null;
        let isEditableEntity = collectionVariableItem ? (collectionVariableItem.data["isEditableEntity"] || false) : null;
        let isCollectionFilterVariable = collectionVariableItem ? (collectionVariableItem.data["isCollectionFilterVariable"] || false) : null;
        let isDeclaredCollectionVariable = collectionVariableItem ? (collectionVariableItem.data["isDeclaredCollectionVariable"] || false) : null;

        this.Data["collectionVariable"] = collectionVariable;
        this.Data["isEditableEntity"] = isEditableEntity;
        this.Data["isCollectionFilterVariable"] = isCollectionFilterVariable;
        this.Data["isDeclaredCollectionVariable"] = isDeclaredCollectionVariable;
        this.Data["isCustomEntity"] = isEditableEntity ? ObjectTables.getIsCustomByName(collectionVariable ? collectionVariable.split("_")[1] : null) : null;
        this.CollectionVariable = collectionVariable;

        this.Data["collectionUsedFrom"] = collectionVariableItem && collectionVariableItem.data && collectionVariableItem.data["nodeId"] ? collectionVariableItem.data["nodeId"] : null;

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
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && isValidName) {
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError ? t.ValidationError.replace(/\_/gi, " ") : null });
            this.ValidationErrorsList = validationErrors;

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }
}