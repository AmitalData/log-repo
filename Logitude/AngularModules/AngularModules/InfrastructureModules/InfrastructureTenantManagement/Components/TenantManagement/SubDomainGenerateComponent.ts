import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { WebFreightDomainService } from '../../../../Infrastructure/Services/WebFreightDomainService'
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    selector: 'SubDomainGenerateComponent',
    templateUrl: './SubDomainGenerateComponent.html',
})

export class SubDomainGenerateComponent extends BaseComponent {
    public DataContext: SubDomainGenerateComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public Domain: string;
    public ValidationErrorsList: string[] = [];

    constructor() {
        super();
        this.Domain = ObjectsLocator.GlobalSetting.DNSZone;
    }

    SetWindowArgs(args) {
        this.EntityPM = args;
        this.Clone();
    }

    private customCustomerURL: string;
    get CustomCustomerURL() {
        return this.customCustomerURL;
    }
    set CustomCustomerURL(value: string) {
        if (this.customCustomerURL != value) {
            this.customCustomerURL = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (AppTool.IsNullOrEmpty(this.CustomCustomerURL)) {
            this.ValidationErrorsList.push("Domain is Required");
            return
        }
        this.CurrentSession.StartBusyIndicator("Generating ..");
        var myService: WebFreightDomainService = new WebFreightDomainService();
        var tenant = this.EntityPM.Id;
        var _customerURL = this.CustomCustomerURL.toLowerCase();
        myService.GetGenerateDigitalPortalDomain(_customerURL, tenant).subscribe((myResult: ServiceResponse) => {
            if (!myResult.HasError) {
                this.ValidationErrorsList = [];
                this.EntityPM.CustomerURL = _customerURL + "." + this.Domain;
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                this.ValidationErrorsList = myResult.ErrorsArray;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('CustomerURL');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
