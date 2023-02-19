import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { EditableRecordsTreeList } from "Workflow/TreeLists/EditableRecordsTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./UpdateRecordPropertiesComponent.html"
})

export class UpdateRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public FlowObject: any;
    public CurrentNodeId: string;
    public EditableRecordsTreeItems: TreeSelectItem[];
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public Record: string;
    public ValidationErrorsList: string[];
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initialize();
        this.initializeEditableRecordsTreeItems();
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
        this.Record = this.Data["record"] || null;

        this.setUIProperties();
    }

    initializeEditableRecordsTreeItems() {
        this.EditableRecordsTreeItems = new EditableRecordsTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateRecord(recordItem: TreeSelectItem) {
        let record = recordItem ? recordItem.key : null;
        this.Record = record;
        this.Data["record"] = record;

        this.Data["recordUsedFrom"] = recordItem && recordItem.data && recordItem.data["nodeId"] ? recordItem.data["nodeId"] : null;

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
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        if (notValidUIProperties.length === 0 && isValidName) {
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }
}