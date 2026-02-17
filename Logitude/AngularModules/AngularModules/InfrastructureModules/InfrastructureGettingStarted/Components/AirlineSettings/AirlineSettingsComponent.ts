import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {TenantManagementPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'AirlineSettingsComponent',
    moduleId: module.id,
    templateUrl: './AirlineSettingsComponent.html',
})

export class AirlineSettingsComponent extends BaseComponent {
    public DataContext: AirlineSettingsComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM = null;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private iService: TenantManagementPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.iService = new TenantManagementPMService();
        this.iService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.EntityPM = myResponse.Result;
                this.IsResourcesReady = true;
            }
        });
    }

    get SignupRequestRecipients() { return this.EntityPM.SignupRequestRecipients; }
    set SignupRequestRecipients(value: string)
    {
        if (this.EntityPM.SignupRequestRecipients != value) {
            this.EntityPM.SignupRequestRecipients = value;
        }
    }

    get LoginPageNotes() { return this.EntityPM.LoginPageNotes; }
    set LoginPageNotes(value:string)
    {
        if (this.EntityPM.LoginPageNotes != value) {
            this.EntityPM.LoginPageNotes = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicatorSaving();

        this.iService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        });
    }

}
