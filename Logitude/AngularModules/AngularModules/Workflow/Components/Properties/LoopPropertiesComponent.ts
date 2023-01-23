import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { EntitiesTreeList } from "Workflow/Models/EntitiesTreeList";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { SingleEditableEntitiesTreeList } from "Workflow/Models/SingleEditableEntitiesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

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
    public FlowObjectFields: ObjectFieldList[];
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
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
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
            ShowRecordsVariables: false,
            ShowDeclaredVariables: false,
            ShowRecordsCollectionVariables: true,
            ShowDeclaredCollectionVariables: true,
            OnlyCurrentLoopItemVariables: false,
            IsObjectVariableSelectable: false
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId, props).Items;
        this.SingleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId, true).Items;
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
        if(this.IsNew){
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateCollectionVariable(collectionVariableItem: TreeSelectItem) {
        let collectionVariable = collectionVariableItem ? collectionVariableItem.key : null;
        let isCollectionFilterVariable = collectionVariableItem ? (collectionVariableItem.data["isCollectionFilterVariable"] || false) : null;
        let isDeclaredCollectionVariable = collectionVariableItem ? (collectionVariableItem.data["isDeclaredCollectionVariable"] || false) : null;
        this.Data["collectionVariable"] = collectionVariable;
        this.Data["isCollectionFilterVariable"] = isCollectionFilterVariable;
        this.Data["isDeclaredCollectionVariable"] = isDeclaredCollectionVariable;
        this.CollectionVariable = collectionVariable;
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