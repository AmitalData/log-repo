import { Component } from '@angular/core';
import { CustomsInterfaceSettingPM } from '../../../EntityPMs/CustomsInterfaceSettingPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsInterfaceStartDatesComponent.html',
})

export class CustomsInterfaceStartDatesComponent extends BaseComponent {
    public EntityPM: CustomsInterfaceSettingPM;
    public ObjectTableName: string;
    public DataContext: CustomsInterfaceStartDatesComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: CustomsInterfaceSettingPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "CustomsInterfaceSetting";
        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("AMCAirStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AMCOceanStartDate", this.ObjectTableName, false);
    }

    get AMCAirStartDate() { return this.EntityPM.AMCAirStartDate; }
    set AMCAirStartDate(value: Date) {
        if (this.EntityPM.AMCAirStartDate != value) {
            this.EntityPM.AMCAirStartDate = value;
        }
    }

    get AMCOceanStartDate() { return this.EntityPM.AMCOceanStartDate; }
    set AMCOceanStartDate(value: Date) {
        if (this.EntityPM.AMCOceanStartDate != value) {
            this.EntityPM.AMCOceanStartDate = value;
        }
    }

    EditStartDate(type: string) {

    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('AMCAirStartDate');
        this.myCloner.AddField('AMCOceanStartDate');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
