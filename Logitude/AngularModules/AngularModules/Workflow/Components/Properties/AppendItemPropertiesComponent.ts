import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { SingleEditableEntitiesTreeList } from "Workflow/Models/SingleEditableEntitiesTreeList";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { SetValue } from "Workflow/Models/SetValue";
import { DeclaredRecordsTreeList } from "Workflow/Models/DeclaredRecordsTreeList";
import { SetRecordFieldsTypes } from "Workflow/Constants/SetRecordFieldsTypes";
import { Formatter } from "Workflow/Models/Formatter";

@Component({
    templateUrl: "./AppendItemPropertiesComponent.html"
})

export class AppendItemPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];
    public SingleEditableEntitiesTreeItems: TreeSelectItem[];
    public DeclaredRecordsTreeItems: TreeSelectItem[];
    public Data: any;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public Collection: string;
    public SetValues: SetValue[];
    public Record: string;
    public SetRecordFieldsType: string;
    public IsValidSetValues: boolean = true;
    public ValidationErrorsList: string[];
    public CollectionChanged: boolean = false;
    public ExcludedEntities: string[] = ["Container"];
    public SetRecordFieldsTypes = SetRecordFieldsTypes;
    public CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
    }

    ngOnInit() {
        this.initialize();
        this.initializeTreeLists();
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || null;
        this.Collection = this.Data["collection"] || null;
        this.Entity = this.Data["entity"] || null;
        this.SetValues = this.Data["setValues"] || [];
        this.SetRecordFieldsType = this.Data["setRecordFieldsType"] || SetRecordFieldsTypes.UseRecord;
        this.Record = this.Data["record"] || null;

        this.Data["setRecordFieldsType"] = this.SetRecordFieldsType;

        this.initializeSetValues();
        this.EntityId = ObjectTables.getIdByName(this.Entity);

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
        this.DeclaredRecordsTreeItems = new DeclaredRecordsTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    updateName(name: string) {
        if(this.IsNew){
            this.Data["name"] = Formatter.getCodeFromName(name);
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
            this.updateSetRecordFieldsType(SetRecordFieldsTypes.UseRecord);
            this.CollectionChanged = !this.CollectionChanged;
        }

        this.setUIProperties();
    }

    updateSetRecordFieldsType(setRecordFieldsType: string) {
        this.Data["setRecordFieldsType"] = setRecordFieldsType;
        this.SetRecordFieldsType = setRecordFieldsType;

        this.initializeSetValues(true);
        this.updateRecord(null);
        this.setIsValidSetValues(setRecordFieldsType === SetRecordFieldsTypes.UseRecord);

        this.setUIProperties();
    }

    updateRecord(record: string) {
        this.Data["record"] = record;
        this.Record = record;

        this.setUIProperties();
    }

    setIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Collection", null, AppTool.IsNullOrEmpty(this.Collection));
        this.UIProperties.SetRequired("Record", null, (this.SetRecordFieldsType === SetRecordFieldsTypes.UseRecord && AppTool.IsNullOrEmpty(this.Record)));
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