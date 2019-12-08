import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../../Customs/EntityPMs/ClaimPM';


@Component({

    moduleId: module.id,
    templateUrl: './PassportDetailsComponent.html',
    selector: 'PassportDetailsComponent',
})

export class PassportDetailsComponent extends BaseComponent {
    public EntityPM: ClaimPM;
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.Claim";
    public IsDisplayOnly: boolean = false;

    constructor() {
        super();

    }


    //#region properties
    public get PassportTypeCode() { return this.EntityPM.PassportTypeCode; }
    public set PassportTypeCode(newValue: string) {
        this.EntityPM.PassportTypeCode = newValue;
        this.PassportNumber = null;
        this.PassportCountryTypeCode = null;
        if (newValue) {
            this.UIProperties.SetRequired("PassportTypeCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("PassportTypeCode", this.ObjectTableName, true);
        }
    }

    public get PassportTypeName() { return this.EntityPM.PassportTypeName; }
    public set PassportTypeName(newValue: string) { this.EntityPM.PassportTypeName = newValue;  }

    public get PassportCountryTypeCode() { return this.EntityPM.PassportCountryTypeCode; }
    public set PassportCountryTypeCode(newValue: string) {
        this.EntityPM.PassportCountryTypeCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("PassportCountryTypeCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("PassportCountryTypeCode", this.ObjectTableName, true);
        }
    }

    get PassportNumber() { return this.EntityPM.PassportNumber; }
    set PassportNumber(value: string) {
        this.EntityPM.PassportNumber = value;
        if (value) {
            this.UIProperties.SetRequired("PassportNumber", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("PassportNumber", this.ObjectTableName, true);
        }
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.IsDisplayOnly = args.IsDisplayOnly;

            //Disable fields
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.PassportTypeCode)) {
                this.UIProperties.SetRequired("PassportTypeCode", this.ObjectTableName, true);
            }
            if (AppTool.IsNullOrEmpty(this.EntityPM.PassportNumber)) {
                this.UIProperties.SetRequired("PassportNumber", this.ObjectTableName, true);
            }
            if (AppTool.IsNullOrEmpty(this.EntityPM.PassportCountryTypeCode)) {
                this.UIProperties.SetRequired("PassportCountryTypeCode", this.ObjectTableName, true);
            }
        }
    }

    PassportNumberLostFocus(item: any) {
        this.PassportNumber = item;
    }

    PassportNumberTextChanged(item: any) {
        this.PassportNumber = item;
    }

    PassportNumberClicked(item: any) {
        this.PassportNumber = item;
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("PassportTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PassportCountryTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
    }

    CloneEntity(entityToClone: ClaimPM) {

        var clonedEntity: ClaimPM;
        clonedEntity = new ClaimPM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }

    RejectChanges() {
        //this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {

        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        //if (this.doDisable) {
        //    SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        //}
        //else {
        //    SessionLocator.SelectedSession.CloseCurrentWindowEmit("!ok");
        //}

    }
}
