import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { CollectionFilterEntitiesTreeList } from "Workflow/Models/CollectionFilterEntitiesTreeList";
import { Condition } from "Workflow/Models/Condition";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./CollectionFilterPropertiesComponent.html"
})

export class CollectionFilterPropertiesComponent extends BaseComponent {

    public DataContext: any = this;

    public WorkflowEntity: string;
    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public CollectionFilterEntitiesTreeList: CollectionFilterEntitiesTreeList;
    public CollectionFilterEntitiesTreeItems: TreeSelectItem[];

    public Data: any;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public Collection: string;
    public CollectionFilters: Condition[];
    public CollectionFiltersOperation: string;
    public IsValidCollectionFilters: boolean = true;
    public ValidationErrorsList: string[];


    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.WorkflowEntity = args.WorkflowEntity ? args.WorkflowEntity : null;
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
    }

    ngOnInit() {
        this.initialize();
        this.initializeCollectionFilterEntitiesTree();
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Collection = this.Data["collection"] || null;
        this.Entity = this.Data["entity"] || null;

        this.CollectionFilters = this.Data["collectionFilters"] || [];
        this.CollectionFiltersOperation = this.Data["collectionFiltersOperation"] || ConditionOperations.And;

        if (this.CollectionFilters.length == 0) {
            this.IsValidCollectionFilters = false;
            let condition = new Condition();
            this.CollectionFilters.push(condition);
        }

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.setUIProperties();
    }

    initializeCollectionFilterEntitiesTree() {
        this.CollectionFilterEntitiesTreeList = new CollectionFilterEntitiesTreeList(this.FlowObject, this.CurrentNodeId);
        this.CollectionFilterEntitiesTreeItems = this.CollectionFilterEntitiesTreeList.Items;
    }


    updateName(name: string) {
        this.Data["name"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateCollection(event: any) {
        let value = event.key
        let entity = event.data["entity"]
        let isCollectionChanged = this.Data["collection"] !== value;

        this.Collection = value
        this.Entity  = entity
        this.EntityId = ObjectTables.getIdByName(entity);

        this.Data["collection"] = value
        this.Data["entity"] = entity

        if (isCollectionChanged) {
            let condition = new Condition();
            this.CollectionFilters = []
            this.CollectionFiltersOperation = ConditionOperations.And;
            this.CollectionFilters.push(condition);
        }

        this.setUIProperties();
    }

    UpdateIsValidCollectionFilters(isValidCollectionFilters: boolean) {
        this.IsValidCollectionFilters = isValidCollectionFilters;
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
        if (notValidUIProperties.length === 0 && this.IsValidCollectionFilters) {
            this.setConditionsData();

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidCollectionFilters)
                this.ValidationErrorsList.push("Invalid Conditions");
        }
    }

    setConditionsData() {
        this.Data["collectionFilters"] = this.CollectionFilters;
        this.Data["collectionFiltersOperation"] = this.CollectionFilters.length === 0 ? null : this.CollectionFiltersOperation;
    }
}