import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'DocumentFilingEmailSettingsComponent',
    moduleId: module.id,
    templateUrl: './DocumentFilingEmailSettingsComponent.html',
})

export class DocumentFilingEmailSettingsComponent extends BaseComponent  {

    public DataContext = this;
    public ValidationErrorsList = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    private isDocumentFilingByEmailEnabled = false;
    get IsDocumentFilingByEmailEnabled() {
        return this.isDocumentFilingByEmailEnabled;
    }
    set IsDocumentFilingByEmailEnabled(value: boolean) {
        if (this.isDocumentFilingByEmailEnabled != value) {

            this.isDocumentFilingByEmailEnabled = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var myService = new InfrastructureDomainService();
        myService.UpdateTenantSettings(this.IsDocumentFilingByEmailEnabled).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        });
    }
}
