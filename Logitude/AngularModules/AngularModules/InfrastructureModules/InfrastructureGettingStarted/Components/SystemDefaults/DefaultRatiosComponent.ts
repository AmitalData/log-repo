import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'DefaultRatiosComponent',
    templateUrl: './DefaultRatiosComponent.html',
})

export class DefaultRatiosComponent extends BaseComponent {
    public EntityPM: TenantPM;
    public ObjectTableName: string = "Tenant";
    public DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    public IsHybrid: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private fields: Map<string, string> = new Map();
    constructor() {
        super();

        this.fields.set('AirRatio', TextCodeTranslator.Translate("Tenant.F.AirRatio"));
        this.fields.set('FCLRatio', TextCodeTranslator.Translate("Tenant.F.FCLRatio"));
        this.fields.set('LCLRatio', TextCodeTranslator.Translate("Tenant.F.LCLRatio"));
        this.fields.set('FTLRatio', TextCodeTranslator.Translate("Tenant.F.FTLRatio"));
        this.fields.set('LTLRatio', TextCodeTranslator.Translate("Tenant.F.LTLRatio"));
    }

    SetDataContext(dataContext: any) {        
        this.EntityPM = dataContext.TenantPm;
        this.IsHybrid = this.EntityPM.IsHybrid;
        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.SetFieldUIProperties('AirRatio');
        this.SetFieldUIProperties('FCLRatio');
        this.SetFieldUIProperties('LCLRatio');
        this.SetFieldUIProperties('FTLRatio');
        this.SetFieldUIProperties('LTLRatio');
    }

    SetFieldUIProperties(fieldName: string) {

        this.UIProperties.SetRequired(fieldName, this.ObjectTableName, false);
        this.UIProperties.SetValidity(fieldName, this.ObjectTableName, true, null);

        let value = this.EntityPM[fieldName];
        let isRequired = AppTool.IsNullOrEmpty(value);
        let isValid = (value >= 1 && value <= 10);
        let translatedField = this.fields.get(fieldName);
        let message = isValid ? null : `${translatedField} must be between 1-10`;

        if (isRequired) {
            this.UIProperties.SetRequired(fieldName, this.ObjectTableName, isRequired);
        }

        else if (!isValid) {
            this.UIProperties.SetValidity(fieldName, this.ObjectTableName, isValid, message);
        }
    }

    get AirRatio() { return this.EntityPM.AirRatio; }
    set AirRatio(value: number) {
        if (this.EntityPM.AirRatio != value) {
            this.EntityPM.AirRatio = AppTool.Round(value, 3);
            this.SetFieldUIProperties('AirRatio');
        }
    }

    get FCLRatio() { return this.EntityPM.FCLRatio; }
    set FCLRatio(value: number) {
        if (this.EntityPM.FCLRatio != value) {
            this.EntityPM.FCLRatio = AppTool.Round(value, 3);
            this.SetFieldUIProperties('FCLRatio');
        }
    }

    get LCLRatio() { return this.EntityPM.LCLRatio; }
    set LCLRatio(value: number) {
        if (this.EntityPM.LCLRatio != value) {
            this.EntityPM.LCLRatio = AppTool.Round(value, 3);
            this.SetFieldUIProperties('LCLRatio');
        }
    }

    get FTLRatio() { return this.EntityPM.FTLRatio; }
    set FTLRatio(value: number) {
        if (this.EntityPM.FTLRatio != value) {
            this.EntityPM.FTLRatio = AppTool.Round(value, 3);
            this.SetFieldUIProperties('FTLRatio');
        }
    }

    get LTLRatio() { return this.EntityPM.LTLRatio; }
    set LTLRatio(value: number) {
        if (this.EntityPM.LTLRatio != value) {
            this.EntityPM.LTLRatio = AppTool.Round(value, 3);
            this.SetFieldUIProperties('LTLRatio');
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.Validate();

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    Validate() {
        this.ValidationErrorsList = [];
        this.ValidateField('AirRatio');
        this.ValidateField('FCLRatio');
        this.ValidateField('LCLRatio');
        this.ValidateField('FTLRatio');
        this.ValidateField('LTLRatio');
    }

    private ValidateField(fieldName: string) {
        let fieldValue = this.EntityPM[fieldName];
        let translatedField = this.fields.get(fieldName);

        if (AppTool.IsNullOrEmpty(fieldValue)) {
            this.ValidationErrorsList.push(`${translatedField} Field is required`);
        }

        else if (fieldValue < 1 || fieldValue > 10) {
            this.ValidationErrorsList.push(`${translatedField} must be between 1-10`);
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('AirRatio');
        this.myCloner.AddField('LCLRatio');
        this.myCloner.AddField('FCLRatio');
        this.myCloner.AddField('LTLRatio');
        this.myCloner.AddField('FTLRatio');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
