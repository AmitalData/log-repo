
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component}  from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {TenantManagementPMService} from '../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {TenantManagementPM} from '../../../Infrastructure/EntityPMs/TenantManagementPM';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,

    selector: 'SupportManagement',
    templateUrl: './SupportManagementComponent.html',
    providers: [TenantManagementPMService]
 

})

export class SupportManagementComponent extends BaseComponent  {
    TenantManagementPM: TenantManagementPM = new TenantManagementPM();

    IsSystemSupportEnabled: boolean = false;
    IsDistributorSupportEnabled: boolean = false;
    IsSystemSupportEnabledCheck: boolean = false;
    IsDistributorSupportEnabledCheck: boolean = false;
    private tenantManagementPMService: TenantManagementPMService;
    public ValidationErrorsList: string[];
    SystemSupportEnabledKey: string = "";
    DistributorSupportEnabledKey: string = "";
    constructor() {
        super();

        if (this.tenantManagementPMService == null) {
            this.tenantManagementPMService = new TenantManagementPMService();
        }
        this.SystemSupportEnabledKey = Guid.newGuid();
        this.DistributorSupportEnabledKey = Guid.newGuid();
        this.LoadData();
    }

    SystemSupportEnabledChecked() {
        this.IsSystemSupportEnabledCheck = this.IsSystemSupportEnabledCheck; 
    }

    DistributorSupportEnabledChecked() {
        this.IsDistributorSupportEnabledCheck = this.IsDistributorSupportEnabledCheck;

    }
    
    LoadData() {
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.tenantManagementPMService.get(SessionLocator.Tenant).subscribe(res=> {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.TenantManagementPM = myResult;
                    this.IsDistributorSupportEnabled = this.TenantManagementPM.IsDistributorSupportEnabled;
                    this.IsSystemSupportEnabled = this.TenantManagementPM.IsSystemSupportEnabled; 
                    this.IsSystemSupportEnabledCheck = this.TenantManagementPM.IsSystemSupportEnabled;
                    this.IsDistributorSupportEnabledCheck = this.TenantManagementPM.IsDistributorSupportEnabled;

                }
            }
            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
        


        });

    }



    CloseButtonClicked() {

        SessionLocator.CurrentSession.CloseCurrentWindow();
    }



    SaveButtonClicked() {

        ServiceLocator.SendTotangoUserActivity("SupportManagement", "Edit");
    
        if ((!this.IsSystemSupportEnabledCheck && this.IsSystemSupportEnabled) || (!this.IsDistributorSupportEnabledCheck && this.IsDistributorSupportEnabled)) {
            this.ShowConfirmationWindow();
        }
        else {
            this.SaveChanges();
        }

     
        
    }


    ShowConfirmationWindow() {

        var confirmMsg: string = "Please note that when the system/distributor support is not allowed, we cannot provide online customer support.\nIn case of a problem, please refer to your Logitude account manager.";
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Customer Care Deactivation";
        confirmWindow.Width = 400;
        confirmWindow.Height = 180;
        confirmWindow.YesButtonText = "OK";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show(confirmMsg);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveChanges();
            }
        });
    }



    SaveChanges() {

        this.ValidationErrorsList = [];
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        this.TenantManagementPM.IsSystemSupportEnabled = this.IsSystemSupportEnabledCheck;
        this.TenantManagementPM.IsDistributorSupportEnabled = this.IsDistributorSupportEnabledCheck;
        this.tenantManagementPMService.update(this.TenantManagementPM).subscribe(res=> {

            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (pmResponse.HasError) {
                pmResponse.ErrorsArray.forEach((item) => {
                    this.ValidationErrorsList.push(item);
                });
            }
            else {
                SessionLocator.TenantManagementPM = this.TenantManagementPM;
                SessionLocator.CurrentSession.CloseCurrentWindow();
            }
            
      

        });
    }


    



}