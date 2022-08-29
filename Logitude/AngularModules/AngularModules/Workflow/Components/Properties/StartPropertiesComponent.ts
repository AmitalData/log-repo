import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { Entity } from "Workflow/Models/Entity";

@Component({
    templateUrl: "./StartPropertiesComponent.html"
})

export class StartPropertiesComponent extends BaseComponent {

    public Entities: Entity[] = [
        new Entity("Shipment"),
    ];

    public Entity: Entity = null;
    public Trigger: string = null;

    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    DataContext: any = this;
    Data: any;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};

        let entityCode = this.Data["entity"];
        let entity = entityCode ? (this.Entities.filter(e => e.Name === entityCode)[0] || null) : null;

        this.updateEntity(entity, false);
        this.updateTrigger(this.Data["trigger"], false);
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {
            this.CurrentSession.CurrentWindow.Close(this.Data);
        }else{
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;
        }
    }

    updateEntity(entity: Entity, updateData: boolean = true) {
        if (updateData) {
            this.Data["entity"] = entity.Name;
        }
        this.Entity = entity;
        this.setRequiredUIProperty("Object", entity);
    }

    updateTrigger(trigger: string, updateData: boolean = true) {
        if (updateData) {
            this.Data["trigger"] = trigger;
        }
        this.Trigger = trigger;
        this.setRequiredUIProperty("Trigger", trigger);
    }

    setRequiredUIProperty(fieldName: string, fieldValue: any) {
        this.UIProperties.SetRequired(fieldName, null, AppTool.IsNullOrEmpty(fieldValue));
    }
}