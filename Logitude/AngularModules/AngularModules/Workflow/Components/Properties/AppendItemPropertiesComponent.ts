import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { SingleEditableEntitiesTreeList } from "Workflow/TreeLists/SingleEditableEntitiesTreeList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { SetValue } from "Workflow/Models/SetValue";
import { SetRecordFieldsTypes } from "Workflow/Constants/SetRecordFieldsTypes";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { IsObjectTypePipe } from "Workflow/Pipes/IsObjectTypePipe";
import { EntityLabelPipe } from "Workflow/Pipes/EntityLabelPipe";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./AppendItemPropertiesComponent.html"
})

export class AppendItemPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public FlowObject: any;
    public CurrentNodeId: string;
    public SingleEditableEntitiesTreeItems: TreeSelectItem[];
    public VariablesTreeItems: TreeSelectItem[];
    public FlowVariablesTreeItems: TreeSelectItem[];
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public DeclareType: string = null;
    public Collection: string;
    public SetValues: SetValue[];
    public Value: string;
    public Variable: string;
    public SetRecordFieldsType: string;
    public IsValidSetValues: boolean = true;
    public ValidationErrorsList: string[];
    public CollectionChanged: boolean = false;
    public SetRecordFieldsTypes = SetRecordFieldsTypes;
    public CurrentSession = SessionLocator.SelectedSession;
    public SetValuesTitleText: string = null;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initialize();
        this.initializeTreeLists();
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
        this.SetValues = this.Data["setValues"] || [];
        this.SetRecordFieldsType = this.Data["setRecordFieldsType"] || SetRecordFieldsTypes.UseRecord;
        this.Variable = this.Data["variable"] || this.Data["record"] || null;
        this.Value = this.Data["value"] || null;

        this.Data["setRecordFieldsType"] = this.SetRecordFieldsType;

        this.initializeSetValues();

        if (this.isEntityCollection(this.Entity)) {
            this.setEntity()
        } else {
            this.setDeclareType()
        }

        this.setUIProperties();
    }

    initializeSetValues(reset: boolean = false) {
        if (reset) {
            this.SetValues = [];
        }
        if (this.SetValues.length === 0 && this.SetRecordFieldsType === SetRecordFieldsTypes.SetValues) {
            let setValue = new SetValue();
            this.SetValues.push(setValue);
            this.IsValidSetValues = false;
        }
    }

    initializeTreeLists() {
        this.SingleEditableEntitiesTreeItems = new SingleEditableEntitiesTreeList(this.FlowObject, this.CurrentNodeId).Items;
        this.initializeVariablesToAppeandTree();
        this.initializeDeclaredCollectionVariables();
        this.initializeFlowVariablesTree();
    }


    initializeVariablesToAppeandTree() {
        let props = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            IsNoChildrenObjectVariables: true
        };
        this.VariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
    }

    initializeDeclaredCollectionVariables() {
        let props = {
            ShowDeclaredCollectionVariables: true
        };
        let flowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
        this.SingleEditableEntitiesTreeItems = this.SingleEditableEntitiesTreeItems.concat(flowVariablesTreeItems);
    }

    initializeFlowVariablesTree() {
        let props = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            ShowDeclaredCollectionVariables: true,
            ShowGlobalVariables: true,
            IsObjectVariableSelectable: true
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
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

        let isCollectionChanged = this.Data["collection"] !== collectionName;

        this.Collection = collectionName;
        this.Entity = collectionEntity;

        if (this.isEntityCollection(this.Entity)) {
            this.setEntity()
        } else {
            this.setDeclareType()
        }

        this.Data["collection"] = collectionName;
        this.Data["entity"] = collectionEntity;
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(collectionEntity);

        this.Data["collectionUsedFrom"] = collectionItem && collectionItem.data && collectionItem.data["nodeId"] ? collectionItem.data["nodeId"] : null;

        if (isCollectionChanged) {
            this.resetFields();
            this.updateSetRecordFieldsType(SetRecordFieldsTypes.UseRecord);
            this.CollectionChanged = !this.CollectionChanged;
        }

        this.setUIProperties();
    }

    getCollectionEntity(collectionItem: TreeSelectItem) {
        let isDeclared = collectionItem ? collectionItem.data["isDeclaredCollectionVariable"] : false;
        let collectionEntity = null;
        if (isDeclared) {
            var type = collectionItem ? collectionItem.data["type"] : null;
            var typeArray = type?.replace('[]', '').split('.')
            collectionEntity = typeArray.length > 1 ? typeArray[1] : typeArray[0]
        } else {
            collectionEntity = collectionItem ? collectionItem.data["entity"] : null;
        }
        return collectionEntity;
    }

    resetFields() {
        this.Variable = null
        this.Data["variable"] = null;
        this.Value = null
        this.Data["value"] = null;
        this.SetValues = []
    }

    updateSetRecordFieldsType(setRecordFieldsType: string) {
        this.Data["setRecordFieldsType"] = setRecordFieldsType;
        this.SetRecordFieldsType = setRecordFieldsType;

        this.initializeSetValues(true);
        this.updateVariable(null);
        this.setIsValidSetValues(setRecordFieldsType === SetRecordFieldsTypes.UseRecord);

        this.setUIProperties();
    }

    updateVariable(recordItem: TreeSelectItem) {
        let variable = recordItem ? recordItem.key : null;
        this.Data["variable"] = variable;
        this.Variable = variable;

        this.Data["variableUsedFrom"] = recordItem && recordItem.data && recordItem.data["nodeId"] ? recordItem.data["nodeId"] : null;

        this.setUIProperties();
    }

    updateValue(value: string) {
        this.Data["value"] = value;
        this.Value = value;
        this.SetValues = [];

        this.setUIProperties();
    }

    setIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Collection", null, AppTool.IsNullOrEmpty(this.Collection));
        this.UIProperties.SetRequired("Variable", null, (this.SetRecordFieldsType === SetRecordFieldsTypes.UseRecord && AppTool.IsNullOrEmpty(this.Variable)));
        if (this.Entity && !this.isEntityCollection(this.Entity)) {
            this.IsValidSetValues = true;
            this.UIProperties.SetRequired("Value", null, (this.SetRecordFieldsType === SetRecordFieldsTypes.SetValues && AppTool.IsNullOrEmpty(this.Value)));
        } else if (this.Entity && this.isEntityCollection(this.Entity)) {
            this.UIProperties.SetRequired("Value", null, false);
        }
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && this.IsValidSetValues && isValidName) {
            this.setSetValuesData();
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

    setSetValuesData() {
        this.Data["setValues"] = this.SetValues;
    }

    setEntity() {
        this.EntityId = ObjectTables.getIdByName(this.Entity);
        this.DeclareType = null;
        this.SetValuesTitleText = "Set Fields Values of " + this.getEntityLabel()
    }

    setDeclareType() {
        this.DeclareType = this.Entity
        this.EntityId = null;
        this.SetValuesTitleText = "Set Value to Append"
    }

    isEntityCollection(type: string) {
        return new IsObjectTypePipe().transform(type);
    }

    getEntityLabel() {
        return new EntityLabelPipe().transform(this.Entity);
    }
}