import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TenantAdditionalDataPM } from '../../../../Common/EntityPMs/TenantAdditionalDataPM';
import { TenantAdditionalDataPMService } from '../../../../Common/Services/StandardPMs/TenantAdditionalDataPMService';
import { TenantAdditionalDataPMServiceExtended } from '../../../../Common/Services/StandardPMs/TenantAdditionalDataPMService Extended';

@Component({
    selector: 'PaymentGatewayComponent',
    moduleId: module.id,
    templateUrl: './PaymentGatewayComponent.html',
})

export class PaymentGatewayComponent extends BaseComponent {
    public DataContext: PaymentGatewayComponent = this;
    public ObjectTableName: string = "TenantAdditionalData";
    public EntityPM: TenantAdditionalDataPM = null;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private iService: TenantAdditionalDataPMService;
    private ExtenedService: TenantAdditionalDataPMServiceExtended;
    private CurrentSession = SessionLocator.SelectedSession;
    Existed: boolean;
    constructor() {
        super();
        this.EntityPM = new TenantAdditionalDataPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.iService = new TenantAdditionalDataPMService();
        this.ExtenedService = new TenantAdditionalDataPMServiceExtended();

        this.ExtenedService.get(this.EntityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
              
                if (!AppTool.IsNullOrEmpty(myResponse.Result) ) {
                    this.EntityPM = myResponse.Result;
                    this.Existed = true;
                }
                else {
                    this.Existed = false;
                }
            }
           
        });
        
    }

  
    get PaymentGatewayConnectionString() { return this.EntityPM.PaymentGatewayConnectionString; }
    set PaymentGatewayConnectionString(value: string) {
        if (this.EntityPM.PaymentGatewayConnectionString != value) {
            this.EntityPM.PaymentGatewayConnectionString = value;
        }
    }

    get PaymentGatewayPartnerCode() { return this.EntityPM.PaymentGatewayPartnerCode; }
    set PaymentGatewayPartnerCode(value:string)
    {
        if (this.EntityPM.PaymentGatewayPartnerCode != value) {
            this.EntityPM.PaymentGatewayPartnerCode = value;
        }
    }



    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
      

        if (!this.Existed) {

            if (this.PaymentGatewayPartnerCode == null) {
                this.ValidationErrorsList.push("Partner Code is required");
            }
            if (this.PaymentGatewayConnectionString == null) {
                this.ValidationErrorsList.push("Connection String is required");
            }
            if (this.ValidationErrorsList.length == 0) {
                this.iService.insert(this.EntityPM).subscribe(myResult => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.CloseCurrentWindow();
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;

                    }
                });
            }
        }


        else {
            this.ValidationErrorsList = [];
            if (this.PaymentGatewayPartnerCode == null) {
                this.ValidationErrorsList.push("Partner Code is required");
            }
            if (this.PaymentGatewayConnectionString == null) {
                this.ValidationErrorsList.push("Connection String is required");
            }
            if (this.ValidationErrorsList.length == 0) {
                this.iService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {



                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                });
            }
        }
    }

}
