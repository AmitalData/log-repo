import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {TenantManagementPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';

@Component({
    selector: 'AirlineSettingsComponent',
    moduleId: module.id,
    templateUrl: './AirlineSettingsComponent.html',
})

export class AirlineSettingsComponent extends BaseComponent {
    public DataContext: AirlineSettingsComponent = this;
    public ObjectTableName: string = "TenantManagement";
    private entityPM: TenantManagementPM;

    constructor() {
        super();
        this.entityPM = SessionLocator.TenantManagementPM;
    }

    // Props
    get SignupRequestRecipients() { return this.entityPM.SignupRequestRecipients; }
    set SignupRequestRecipients(value: string)
    {
        if (this.entityPM.SignupRequestRecipients != value) {
            this.entityPM.SignupRequestRecipients = value;
        }
    }

    get LoginPageNotes() { return this.entityPM.LoginPageNotes; }
    set LoginPageNotes(value:string)
    {
        if (this.entityPM.LoginPageNotes != value) {
            this.entityPM.LoginPageNotes = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        var service = new TenantManagementPMService();
        service.update(this.entityPM).subscribe((res:ServiceResponse) => {
            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                SessionLocator.CurrentSession.CloseCurrentWindow();
                InfraSettings.TenantManagementPM = this.entityPM;
            }
            else {
                SessionLocator.CurrentSession.CloseCurrentWindow();
            }
        });
    }

}