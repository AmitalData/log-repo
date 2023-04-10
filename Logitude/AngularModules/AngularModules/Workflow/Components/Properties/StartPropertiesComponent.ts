import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { Condition } from "Workflow/Models/Condition";
import { EntitiesTreeList } from "Workflow/TreeLists/EntitiesTreeList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { StartTriggerTypes } from "Workflow/Constants/StartTriggerTypes";

@Component({
    templateUrl: "./StartPropertiesComponent.html"
})

export class StartPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public Entity: string = null;
    public EntityId: string = null;
    public Trigger: string = null;
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public CreateTrigger: string = "create";
    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public EntitiesTreeItems: TreeSelectItem[];
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeEntitiesTreeItems();
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

    initializeEntitiesTreeItems() {
        this.EntitiesTreeItems = new EntitiesTreeList("parent").Items;
    }

    initialize() {
        this.Data["triggerType"] = StartTriggerTypes.RecordTriggered;
        
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Entity = this.Data["entity"] || null;
        this.Trigger = this.Data["trigger"] || this.CreateTrigger;

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;

        this.EntityId = ObjectTables.getIdByName(this.Entity);

        this.initializeConditions();

        this.setUIProperties();
    }

    initializeConditions(reset: boolean = false, forceAdd: boolean = false, isGroup: boolean = true) {
        if (reset) {
            this.Conditions = [];
            this.ConditionsOperation = ConditionOperations.And;
        }

        if (this.Conditions.length === 0 && (forceAdd || this.Trigger !== this.CreateTrigger)) {
            let condition = new Condition(isGroup);
            if (this.Trigger !== this.CreateTrigger) {
                condition.operator = ConditionOperators.Changed;
                condition.value = "True";
                condition.disabled = "d,o,v";
            }
            this.Conditions.push(condition);
            this.IsValidConditions = false;
        }

        if (this.Conditions.length === 0 && this.Trigger === this.CreateTrigger) {
            this.IsValidConditions = true;
        }
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

            if (!this.IsValidConditions) {
                this.ValidationErrorsList.push("Invalid Conditions");
            }
        }
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }

    updateEntity(entity: string) {
        let isEntityChanged = this.Data["entity"] !== entity;
        this.Data["entity"] = entity;
        this.Data["entityLabel"] = ObjectTables.getDisplayNameByName(entity);
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(entity);
        this.Entity = entity;
        this.EntityId = ObjectTables.getIdByName(entity);

        if (isEntityChanged) {
            this.initializeConditions(true);
        }

        this.setUIProperties();
    }

    updateTrigger(trigger: string) {
        let shouldInitializeConditions = (this.Data["trigger"] === this.CreateTrigger && trigger !== this.CreateTrigger) || (this.Data["trigger"] !== this.CreateTrigger && trigger === this.CreateTrigger);

        this.Data["trigger"] = trigger;
        this.Trigger = trigger;

        if (shouldInitializeConditions) {
            this.initializeConditions(true);
        }

        this.setUIProperties();
    }

    addCondition(isGroup: boolean = false) {
        if (this.EntityId) {
            this.initializeConditions(true, true, isGroup);
        }
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.Entity));
        this.UIProperties.SetRequired("Trigger", null, AppTool.IsNullOrEmpty(this.Trigger));
    }

    updateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }
}