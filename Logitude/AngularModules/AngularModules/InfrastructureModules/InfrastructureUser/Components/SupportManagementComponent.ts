import { Component } from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantManagementPM } from '../../../Infrastructure/EntityPMs/TenantManagementPM';
import { TenantManagementPMService } from '../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,
    selector: 'SupportManagement',
    templateUrl: './SupportManagementComponent.html',
})

export class SupportManagementComponent extends BaseComponent  {
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM = null;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private iService: TenantManagementPMService;
    SystemSupportEnabledKey: string = "";
    DistributorSupportEnabledKey: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.CurrentSession.StartBusyIndicatorLoading();

        this.iService = new TenantManagementPMService();
        this.iService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.EntityPM = myResponse.Result;
                this.IsSystemSupportEnabledCheck = this.EntityPM.IsSystemSupportEnabled;
                this.IsDistributorSupportEnabledCheck = this.EntityPM.IsDistributorSupportEnabled;
                this.IsResourcesReady = true;
            }

            this.CurrentSession.StopBusyIndicator();
        });

        
        this.SystemSupportEnabledKey = Guid.newGuid();
        this.DistributorSupportEnabledKey = Guid.newGuid();
    }

    get IsSystemSupportEnabled() { return this.EntityPM.IsSystemSupportEnabled; }
    set IsSystemSupportEnabled(value: boolean) {
        if (this.EntityPM.IsSystemSupportEnabled != value) {
            this.EntityPM.IsSystemSupportEnabled = value;
        }
    }

    get IsDistributorSupportEnabled() { return this.EntityPM.IsDistributorSupportEnabled; }
    set IsDistributorSupportEnabled(value: boolean) {
        if (this.EntityPM.IsDistributorSupportEnabled != value) {
            this.EntityPM.IsDistributorSupportEnabled = value;
        }
    }


    IsSystemSupportEnabledCheck: boolean = false;
    IsDistributorSupportEnabledCheck: boolean = false;
    SystemSupportEnabledChecked() {
        this.IsSystemSupportEnabledCheck = this.IsSystemSupportEnabledCheck; 
    }

    DistributorSupportEnabledChecked() {
        this.IsDistributorSupportEnabledCheck = this.IsDistributorSupportEnabledCheck;

    }
    
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
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

        this.CurrentSession.StartBusyIndicatorSaving();

        this.EntityPM.IsSystemSupportEnabled = this.IsSystemSupportEnabledCheck;
        this.EntityPM.IsDistributorSupportEnabled = this.IsDistributorSupportEnabledCheck;

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
