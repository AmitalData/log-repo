import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { SingleEditableEntitiesTreeList } from "Workflow/Models/SingleEditableEntitiesTreeList";
import { Condition } from "Workflow/Models/Condition";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./CollectionFilterPropertiesComponent.html"
})

export class CollectionFilterPropertiesComponent extends BaseComponent {

    public DataContext: any = this;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];
    public SingleEditableEntitiesTreeItems: TreeSelectItem[];
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public Collection: string;
    public CollectionFilters: Condition[];
    public CollectionFiltersOperation: string;
    public IsValidConditions: boolean = true;
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
        this.initializeSingleEditableEntitiesTree();
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.Collection = this.Data["collection"] || null;
        this.Entity = this.Data["entity"] || null;

        this.CollectionFilters = this.Data["collectionFilters"] || [];
        this.CollectionFiltersOperation = this.Data["collectionFiltersOperation"] || ConditionOperations.And;

        this.initializeCollectionFilters();

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.setUIProperties();
    }

    initializeCollectionFilters(reset: boolean = false) {
        if (reset) {
            this.CollectionFilters = [];
            this.CollectionFiltersOperation = ConditionOperations.And;
        }
        if (this.CollectionFilters.length === 0) {
            let condition = new Condition();
            this.CollectionFilters.push(condition);
            this.IsValidConditions = false;
        }
    }

    initializeSingleEditableEntitiesTree() {
        this.SingleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    updateName(name: string) {
        if(this.IsNew){
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
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
            this.initializeCollectionFilters(true);
        }

        this.setUIProperties();
    }

    setIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
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
        if (notValidUIProperties.length === 0 && this.IsValidConditions) {
            this.setConditionsData();
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions)
                this.ValidationErrorsList.push("Invalid Conditions");
        }
    }

    setConditionsData() {
        this.Data["collectionFilters"] = this.CollectionFilters;
        this.Data["collectionFiltersOperation"] = this.CollectionFilters.length === 0 ? null : this.CollectionFiltersOperation;
    }
}