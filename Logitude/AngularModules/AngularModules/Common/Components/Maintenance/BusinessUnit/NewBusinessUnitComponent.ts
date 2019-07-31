import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BusinessUnitPM} from '../../../EntityPMs/BusinessUnitPM'; 
import {BusinessUnitPMService} from '../../../Services/StandardPMs/BusinessUnitPMService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'NewBusinessUnitComponent',
    moduleId: module.id,
    templateUrl: './NewBusinessUnitComponent.html',
})

export class NewBusinessUnitComponent extends BaseComponent {

    public EntityPM: BusinessUnitPM;
    public ObjectTableName: string = "BusinessUnit";
    public DataContext: NewBusinessUnitComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new BusinessUnitPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.SetUIProperties();
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("ParentId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ParentId));
    }

    //Properties
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get ParentId() { return this.EntityPM.ParentId; }
    set ParentId(value: string) {
        if (this.EntityPM.ParentId != value) {
            this.EntityPM.ParentId = value;
            this.SetUIProperties();
        }
    }

    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.ParentId)) {
            errors.push(msg.replace("%FieldName", "Parent"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var service: BusinessUnitPMService = new BusinessUnitPMService();
            service.insert(this.EntityPM).subscribe((myResult: ServiceResponse) => {
                if (myResult) {
                    if (!myResult.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    }
}
