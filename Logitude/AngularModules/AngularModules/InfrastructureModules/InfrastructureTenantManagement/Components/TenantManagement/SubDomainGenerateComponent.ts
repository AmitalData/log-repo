import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { WebFreightDomainService } from '../../../../Infrastructure/Services/WebFreightDomainService'
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'SubDomainGenerateComponent',
    templateUrl: './SubDomainGenerateComponent.html',
})

export class SubDomainGenerateComponent extends BaseComponent  {
    public DataContext: SubDomainGenerateComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public Domain: string;

    constructor() {
        super();
        this.Domain = ObjectsLocator.GlobalSetting.DNSZone;
    }

    SetWindowArgs(args) {
        this.EntityPM = args;
    }

    get CustomerURL() {
        return this.EntityPM.CustomerURL;
    }
    set CustomerURL(value: string) {
        if (this.EntityPM.CustomerURL != value) {
            this.EntityPM.CustomerURL = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Generating ..");
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.GetGenerateDigitalPortalDomain(this.CustomerURL).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        });
    }
}
