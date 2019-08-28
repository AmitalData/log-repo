import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {TenantManagementPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'PaymentGatewayComponent',
    moduleId: module.id,
    templateUrl: './PaymentGatewayComponent.html',
})

export class PaymentGatewayComponent extends BaseComponent {
    public DataContext: PaymentGatewayComponent = this;
    public ObjectTableName: string = "TenantAdditionalData";
    //public EntityPM: TenantManagementPM = null;
    //public IsResourcesReady: boolean = false;
    //public ValidationErrorsList: string[] = [];
    //private iService: TenantManagementPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        //this.iService = new TenantManagementPMService();
        //this.iService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
        //    if (myResponse.HasError) {
        //        this.ValidationErrorsList = myResponse.ErrorsArray;
        //    }

        //    else {
        //        this.EntityPM = myResponse.Result;
        //        this.IsResourcesReady = true;
        //    }
        //});
    }

  

    //get PaymentGatewayPartnerCode() { return this.EntityPM.PaymentGatewayPartnerCode; }
    //set PaymentGatewayPartnerCode(value:string)
    //{
    //    if (this.EntityPM.PaymentGatewayPartnerCode != value) {
    //        this.EntityPM.PaymentGatewayPartnerCode = value;
    //    }
    //}

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        //this.ValidationErrorsList = [];
        //this.CurrentSession.StartBusyIndicatorSaving();

        //this.iService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

        //    this.CurrentSession.StopBusyIndicator();

        //    if (myResponse.HasError) {
        //        this.ValidationErrorsList = myResponse.ErrorsArray;
        //    }

        //    else {
        //        this.CurrentSession.CloseCurrentWindow();
        //    }
        //});
    }

}
