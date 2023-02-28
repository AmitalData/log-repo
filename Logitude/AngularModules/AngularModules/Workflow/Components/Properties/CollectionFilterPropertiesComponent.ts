import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { SingleEditableEntitiesTreeList } from "Workflow/TreeLists/SingleEditableEntitiesTreeList";
import { Condition } from "Workflow/Models/Condition";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { FieldType } from "Workflow/Utilities/FieldType";
import { IsDateTimeTypePipe } from "Workflow/Pipes/IsDateTimeTypePipe";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./CollectionFilterPropertiesComponent.html"
})

export class CollectionFilterPropertiesComponent extends BaseComponent {

    public DataContext: any = this;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowVariablesTreeList: FlowVariablesTreeList;
    public CollectionItems: TreeSelectItem[];
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public Type: string = null;
    public IsPrimitiveType: boolean = false;
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
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeCollectionTreeItems();
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

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.Collection = this.Data["collection"] || null;
        this.Entity = this.Data["entity"] || null;
        this.Type = this.Data["type"] || null;
        this.IsPrimitiveType = this.Data["isPrimitiveType"] || false;

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

            if (this.IsPrimitiveType && this.Type) {
                let collectionItem = this.FlowVariablesTreeList.getItem(this.Collection);
                condition.field = "Item in " + (collectionItem ? collectionItem.title : (this.Collection || "Collection"));
                condition.fieldCode = "item";
                condition.type = this.Type;
                condition.valueExpression = new IsDateTimeTypePipe().transform(this.Type) ? DateTimeValueExpressions.Date : null;
            }

            this.CollectionFilters.push(condition);
            this.IsValidConditions = false;
        }
    }

    initializeCollectionTreeItems() {
        let props = {
            ShowDeclaredCollectionVariables: true
        };

        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props);
        let flowVariablesTreeItems = this.FlowVariablesTreeList.Items;
        let singleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId).Items;
        this.CollectionItems = singleEditableEntitiesTreeItems.concat(flowVariablesTreeItems);
    }

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateCollection(collectionItem: TreeSelectItem) {
        let collectionName = collectionItem ? collectionItem.key : null;
        let collectionEntity = this.getCollectionEntity(collectionItem);
        let collectionType = this.getCollectionType(collectionItem);
        let isPrimitiveTypeCollection = this.getIsPrimitiveTypeCollection(collectionItem);

        let isCollectionChanged = this.Data["collection"] !== collectionName;

        this.Data["collection"] = collectionName;
        this.Data["entity"] = collectionEntity;
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(collectionEntity);
        this.Data["type"] = collectionType;
        this.Data["isPrimitiveType"] = isPrimitiveTypeCollection;
        this.Data["collectionUsedFrom"] = collectionItem && collectionItem.data && collectionItem.data["nodeId"] ? collectionItem.data["nodeId"] : null;

        this.Collection = collectionName;
        this.Entity = collectionEntity;
        this.EntityId = ObjectTables.getIdByName(collectionEntity);
        this.Type = collectionType;
        this.IsPrimitiveType = isPrimitiveTypeCollection;

        if (isCollectionChanged) {
            this.initializeCollectionFilters(true);
        }

        this.setUIProperties();
    }

    getCollectionEntity(collectionItem: TreeSelectItem) {
        if (collectionItem) {
            let isEditableEntity = collectionItem.data["isEditableEntity"] || false;
            let isDeclaredCollectionVariable = collectionItem.data["isDeclaredCollectionVariable"] || false;
            if (isEditableEntity) {
                return collectionItem.data["entity"] || null;
            } else if (isDeclaredCollectionVariable) {
                let collectionVariableType = collectionItem.data["type"];
                let type = collectionVariableType ? collectionVariableType.replace("[]", "") : null;
                let isPrimitiveType = FieldType.isPrimitive(type);
                return isPrimitiveType ? null : type;
            } else {
                return null;
            }
        }
        return null;
    }

    getCollectionType(collectionItem: TreeSelectItem) {
        if (collectionItem) {
            let isEditableEntity = collectionItem.data["isEditableEntity"] || false;
            let isDeclaredCollectionVariable = collectionItem.data["isDeclaredCollectionVariable"] || false;
            if (isEditableEntity) {
                return null;
            } else if (isDeclaredCollectionVariable) {
                let collectionVariableType = collectionItem.data["type"];
                let type = collectionVariableType ? collectionVariableType.replace("[]", "") : null;
                let isPrimitiveType = FieldType.isPrimitive(type);
                return isPrimitiveType ? type : null;
            } else {
                return null;
            }
        }
        return null;
    }

    getIsPrimitiveTypeCollection(collectionItem: TreeSelectItem) {
        if (collectionItem) {
            let isEditableEntity = collectionItem.data["isEditableEntity"] || false;
            let isDeclaredCollectionVariable = collectionItem.data["isDeclaredCollectionVariable"] || false;
            if (isEditableEntity) {
                return false;
            } else if (isDeclaredCollectionVariable) {
                return FieldType.isPrimitive(collectionItem.data["type"]);
            } else {
                return null;
            }
        }
        return null;
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
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && this.IsValidConditions && isValidName) {
            this.setConditionsData();
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions) {
                this.ValidationErrorsList.push("Invalid Conditions");
            }

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }

    setConditionsData() {
        this.Data["collectionFilters"] = this.CollectionFilters;
        this.Data["collectionFiltersOperation"] = this.CollectionFilters.length === 0 ? null : this.CollectionFiltersOperation;
    }
}