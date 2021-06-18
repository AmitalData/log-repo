import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { GlobalDomainService, OceanInsightGlobalSetting } from '../../../../Common/Services/GlobalDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    templateUrl: './OceanInsightsSettingsComponent.html',
})

export class OceanInsightsSettingsComponent extends BaseComponent {
    public EntityPM: OceanInsightGlobalSetting;
    public DataContext: OceanInsightsSettingsComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private globalDomainService: GlobalDomainService;

    constructor() {
        super();
        this.globalDomainService = new GlobalDomainService();
        this.GetOceanInsightGlobalSetting();
    }

    GetOceanInsightGlobalSetting() {
        this.globalDomainService.GetOceanInsightGlobalSetting().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.EntityPM = response.Result;
            }
        });
    }

    get OITenantNumber() { return this.EntityPM.OITenantNumber; }
    set OITenantNumber(value: number) {
        if (this.EntityPM.OITenantNumber != value) {
            this.EntityPM.OITenantNumber = value;
        }
    }

    get AmitalCloudLogitudeTenantPrimaryKey() { return this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey; }
    set AmitalCloudLogitudeTenantPrimaryKey(value: string) {
        if (this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey != value) {
            this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey = value;
        }
    }

    get AmitalCloudEnvironmentURL() { return this.EntityPM.AmitalCloudEnvironmentURL; }
    set AmitalCloudEnvironmentURL(value: string) {
        if (this.EntityPM.AmitalCloudEnvironmentURL != value) {
            this.EntityPM.AmitalCloudEnvironmentURL = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        if (AppTool.IsNullOrZero( this.OITenantNumber )) {
            errors.push("Ocean Insight Tenant is required");
        }
        if (AppTool.IsNullOrEmpty(this.AmitalCloudEnvironmentURL)) {
            errors.push("Amital Cloud Environment URL is required");
        }
        if (AppTool.IsNullOrEmpty(this.AmitalCloudLogitudeTenantPrimaryKey)) {
            errors.push("Amital Primary Key is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.globalDomainService.UpdateOceanInsightGlobalSetting(this.EntityPM).subscribe((response: ServiceResponse) => {
                if (response.HasError) {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }
}
