import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { SingleEditableEntitiesTreeList } from "Workflow/Models/SingleEditableEntitiesTreeList";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { SetValue } from "Workflow/Models/SetValue";

@Component({
    templateUrl: "./AppendItemPropertiesComponent.html"
})

export class AppendItemPropertiesComponent extends BaseComponent {

    public DataContext: any = this;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public SingleEditableEntitiesTreeItems: TreeSelectItem[];

    public Data: any;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public Collection: string;
    public SetValues: SetValue[];
    public IsValidSetValues: boolean = true;
    public ValidationErrorsList: string[];

    public ExcludedEntities: string[] = ["Container"];

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
    }

    ngOnInit() {
        this.initialize();
        this.initializeSingleEditableEntitiesTree();
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Collection = this.Data["collection"] || null;
        this.Entity = this.Data["entity"] || null;

        this.SetValues = this.Data["setValues"] || [];

        this.initializeSetValues();

        this.EntityId = ObjectTables.getIdByName(this.Entity);

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

    initializeSingleEditableEntitiesTree() {
        this.SingleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    updateName(name: string) {
        this.Data["name"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateCollection(collectionItem: TreeSelectItem) {
        let collectionName = collectionItem ? collectionItem.key : null;
        let collectionEntity = collectionItem ? collectionItem.data["entity"] : null;
        let isCollectionChanged = this.Data["collection"] !== collectionName;

        this.Collection = collectionName;
        this.Entity = collectionEntity;
        this.EntityId = ObjectTables.getIdByName(collectionEntity);

        this.Data["collection"] = collectionName;
        this.Data["entity"] = collectionEntity;

        if (isCollectionChanged) {
            this.initializeSetValues(true);
        }

        this.setUIProperties();
    }

    setIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Collection", null, AppTool.IsNullOrEmpty(this.Collection));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidSetValues) {
            this.setSetValuesData();
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidSetValues) {
                this.ValidationErrorsList.push("Invalid Set Values");
            }
        }
    }

    setSetValuesData() {
        this.Data["setValues"] = this.SetValues;
    }
}