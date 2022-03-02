import { Component } from "@angular/core";
import { LogisticActionRequestPM } from "Customs/EntityPMs/LogisticActionRequestPM";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    templateUrl: './MoreDetailesforImporterComponent.html',
})
export class MoreDetailesforImporterComponent extends BaseComponent {
    public DataContext: any = this;
    entityPM: LogisticActionRequestPM;
    public ObjectTableName: string = "Customs.LogisticActionRequestGeneralTabComponent";
    public OriginalEntityPM: LogisticActionRequestPM;
    public ClonedEntityPM: LogisticActionRequestPM;


    constructor() {
        super();
    }


    SetWindowArgs(args: any) {
        this.entityPM = args.EntityPM;

        this.SetScreenFieldsEditability()
        this.OriginalEntityPM = args.EntityPM;
        this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
    }
    

    public get ExporterIdentifierType() { return this.entityPM.ExporterIdentifierType; }
    public set ExporterIdentifierType(newValue: string) {
        this.entityPM.ExporterIdentifierType = newValue;
        this.SetScreenFieldsEditability()
    }

    public get PassportCountry() { return this.entityPM.PassportCountry; }
    public set PassportCountry(newValue: string) { this.entityPM.PassportCountry = newValue; }

    public get PassportNumber() { return this.entityPM.PassportNumber; }
    public set PassportNumber(newValue: string) { this.entityPM.PassportNumber = newValue; }


    SetScreenFieldsEditability() {
        const isPassport: boolean = this.entityPM.ExporterIdentifierType == "2" || this.entityPM.ExporterIdentifierType == "3"
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, isPassport);
        this.UIProperties.SetEnabled("PassportCountry", this.ObjectTableName, isPassport);
    }


    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("cancel");
    }


    OkButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
    }


    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
    }


    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        const keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }


    CloneEntity(entityToClone: LogisticActionRequestPM) {
        const clonedEntity = new LogisticActionRequestPM(); 
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }
}