import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { Component } from '@angular/core';
import { FeatureTogglePM } from '../../../../Infrastructure/EntityPMs/FeatureTogglePM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureTogglePMService } from '../../../../Infrastructure/Services/StandardPMs/FeatureTogglePMService';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({

    templateUrl: './FeatureToggleGeneralTabComponent.html',
})

export class FeatureToggleGeneralTabComponent extends BaseComponent {

    public EntityPM: FeatureTogglePM;
    public DataContext: FeatureToggleGeneralTabComponent = this;
    public ObjectTableName: string = "FeatureToggle";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public FeatureTogglePMService: FeatureTogglePMService;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
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

}
