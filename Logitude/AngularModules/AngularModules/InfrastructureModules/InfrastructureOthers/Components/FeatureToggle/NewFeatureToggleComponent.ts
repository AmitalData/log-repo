import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { Component } from '@angular/core';
import { FeatureTogglePM } from '../../../../Infrastructure/EntityPMs/FeatureTogglePM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureTogglePMService } from '../../../../Infrastructure/Services/StandardPMs/FeatureTogglePMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { DateTool } from '../../../../Infrastructure/Tools';

@Component({

    templateUrl: './NewFeatureToggleComponent.html',
})


export class NewFeatureToggleComponent extends BaseComponent {

    public EntityPM: FeatureTogglePM;
    public DataContext: NewFeatureToggleComponent = this;
    public ObjectTableName: string = "FeatureToggle";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public FeatureTogglePMService: FeatureTogglePMService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.Initialize();
    }

    Initialize() {
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM = new FeatureTogglePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.FeatureTogglePMService = new FeatureTogglePMService();
    }

    get ToggleCode() { return this.EntityPM.ToggleCode; }
    set ToggleCode(value: string) {
        if (this.EntityPM.ToggleCode != value) {
            this.EntityPM.ToggleCode = value;
        }
    }

    get TenantNumber() { return this.EntityPM.TenantNumber; }
    set TenantNumber(value: number) {
        if (this.EntityPM.TenantNumber != value) {
            this.EntityPM.TenantNumber = value;
        }
    }


    get FromTenantNumber() { return this.EntityPM.FromTenantNumber; }
    set FromTenantNumber(value: number) {
        if (this.EntityPM.FromTenantNumber != value) {
            this.EntityPM.FromTenantNumber = value;
        }
    }

    get ToTenantNumber() { return this.EntityPM.ToTenantNumber; }
    set ToTenantNumber(value: number) {
        if (this.EntityPM.ToTenantNumber != value) {
            this.EntityPM.ToTenantNumber = value;
        }
    }


    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    get IsMultiTenant() { return this.EntityPM.IsMultiTenant; }
    set IsMultiTenant(value: boolean) {
        if (this.EntityPM.IsMultiTenant != value) {
            this.EntityPM.IsMultiTenant = value;
            this.ResetTenantFields();
        }
    }
    public SetIsMultiTenant(isMulti: boolean) {
        this.IsMultiTenant = isMulti;
    }
    private ResetTenantFields() {
        if (this.IsMultiTenant) {
            this.TenantNumber = null;
        }
        else {
            this.FromTenantNumber = null;
            this.ToTenantNumber = null;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.IsMultiTenant) {
            if (this.FromTenantNumber > this.ToTenantNumber) {
                errors.push("From Tenant cannot be greater than the To Tenant");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.FeatureTogglePMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
