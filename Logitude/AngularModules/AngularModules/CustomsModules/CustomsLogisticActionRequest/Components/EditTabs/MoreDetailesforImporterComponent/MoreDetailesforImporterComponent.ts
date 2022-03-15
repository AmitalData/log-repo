import { Component } from "@angular/core";
import { LogisticActionRequestPM } from "Customs/EntityPMs/LogisticActionRequestPM";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Component({
    templateUrl: './MoreDetailesforImporterComponent.html',
})
export class MoreDetailesforImporterComponent extends BaseComponent {
    public DataContext: any = this;
    entityPM: LogisticActionRequestPM;
    public ObjectTableName: string = "Customs.LogisticActionRequestGeneralTabComponent";
    public OriginalEntityPM: LogisticActionRequestPM;
    public ClonedEntityPM: LogisticActionRequestPM;
    FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    requierdFieldsList: string[] = []
    ValidationErrorsList: string[] = []

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
        this.invalidate()
    }

    public get PassportCountry() { return this.entityPM.PassportCountry; }
    public set PassportCountry(newValue: string) {
        this.entityPM.PassportCountry = newValue;
        this.setRequiredField("PassportCountry", this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportCountry);
        this.invalidate()
    }

    public get PassportNumber() { return this.entityPM.PassportNumber; }
    public set PassportNumber(newValue: string) {
        this.entityPM.PassportNumber = newValue;
        this.setRequiredField("PassportNumber", this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportNumber);
        this.invalidate()
    }


    setRequiredField(name: string, fieldIsRequired: boolean) {
        this.UIProperties.SetRequired(name, this.ObjectTableName, fieldIsRequired);

        if (fieldIsRequired) {
            if (this.requierdFieldsList.every(x => x != name))
                this.requierdFieldsList.push(name)
        } else
            this.removeFromArray(this.requierdFieldsList, name)
    }


    private removeFromArray(arr: string[], val: string) {
        const index = arr.indexOf(val);
        if (index !== -1)
            arr.splice(index, 1);
    }


    SetScreenFieldsEditability() {
        const isPassport: boolean = this.entityPM.ExporterIdentifierType == "2";
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, isPassport);
        this.UIProperties.SetEnabled("PassportCountry", this.ObjectTableName, isPassport);
        this.setRequiredField("ExporterIdentifierType", !this.entityPM.ExporterIdentifierType);
        this.setRequiredField("PassportNumber", this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportCountry);
        this.setRequiredField("PassportCountry", this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportCountry);
    }


    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("cancel");
    }


    invalidate(): boolean {
        this.ValidationErrorsList = []
        this.requierdFieldsList
            .filter(filed => !this[filed])
            .forEach(filed =>
                this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.LogisticActionRequest.F." + filed))));

        return !!this.ValidationErrorsList.length;
    }

    
    OkButtonClicked() {
        if (this.invalidate()) return;

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