import {Component} from '@angular/core';
import {CustomsInterfaceSettingPM} from '../../../EntityPMs/CustomsInterfaceSettingPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsInterfaceCredintialsComponent.html',
})

export class CustomsInterfaceCredintialsComponent extends BaseComponent {
    public EntityPM: CustomsInterfaceSettingPM;
    public ObjectTableName: string;
    public DataContext: CustomsInterfaceCredintialsComponent = this;
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
        if (!AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {

            this.UIProperties.SetRequired("LocalCompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId));
            this.UIProperties.SetRequired("LocalUserId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId));
            this.UIProperties.SetRequired("LocalPassword", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword));
        }
    }

    get LocalCompanyId() { return this.EntityPM.LocalCompanyId; }
    set LocalCompanyId(value: string) {
        if (this.EntityPM.LocalCompanyId != value) {
            this.EntityPM.LocalCompanyId = value;

            this.SetUIProperties();
        }
    }

    get LocalUserId() { return this.EntityPM.LocalUserId; }
    set LocalUserId(value: string) {
        if (this.EntityPM.LocalUserId != value) {
            this.EntityPM.LocalUserId = value;

            this.SetUIProperties();
        }
    }

    get LocalPassword() { return this.EntityPM.LocalPassword; }
    set LocalPassword(value: string) {
        if (this.EntityPM.LocalPassword != value) {
            this.EntityPM.LocalPassword = value;

            this.SetUIProperties();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                if (AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId)) {
                    this.ValidationErrorsList.push("Company field is required");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId)) {
                    this.ValidationErrorsList.push("User field is required");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword)) {
                    this.ValidationErrorsList.push("Password field is required");
                }
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('LocalCompanyId');
        this.myCloner.AddField('LocalUserId');
        this.myCloner.AddField('LocalPassword');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
