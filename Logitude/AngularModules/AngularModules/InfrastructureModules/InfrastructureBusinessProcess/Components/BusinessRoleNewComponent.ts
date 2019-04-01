import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools'
import {BusinessRolePMService} from '../../../Infrastructure/Services/StandardPMs/BusinessRolePMService';
import {BusinessRolePM} from '../../../Infrastructure/EntityPMs/BusinessRolePM';
import {BusinessRolePMInitService} from '../../../Infrastructure/EntityPMInitServices/BusinessRolePMInitService';

@Component({
    selector: 'BusinessRoleNewComponent',
    moduleId: module.id,
    templateUrl: './BusinessRoleNewComponent.html',
})

export class BusinessRoleNewComponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: BusinessRolePM;
    public DataContext: BusinessRoleNewComponent = this;
    public ObjectTableName: string = "BusinessRole";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new BusinessRolePM();
        BusinessRolePMInitService.InitValues(this.EntityPM, true);
    }

    // Methods
    OkButtonClicked() {
        var errors: string[] = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            errors.push("Name field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService: BusinessRolePMService = new BusinessRolePMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
  

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    // Properties
    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value;
    }


    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) {
        if (this.EntityPM.Description != value)
            this.EntityPM.Description = value;
    }



}
