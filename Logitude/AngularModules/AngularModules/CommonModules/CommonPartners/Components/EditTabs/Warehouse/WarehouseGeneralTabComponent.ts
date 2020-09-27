import {Component} from '@angular/core';
import {WarehousePM} from '../../../../../Common/EntityPMs/WarehousePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { FeatureToggleList } from '../../../../../Infrastructure/EntityLists/FeatureToggleList';

@Component({    
    templateUrl: './WarehouseGeneralTabComponent.html',
})

export class WarehouseGeneralTabComponent extends BaseComponent {
    public EntityPM: WarehousePM;
    public ObjectTableName: string = "Warehouse";
    public DataContext: WarehouseGeneralTabComponent = this;
    public IsWarehouseFirmCodeVisible = false;
    public IsStoragePricingVisible: boolean = false;
    private pricingFeatureToggle: FeatureToggleList;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;

        if (SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
            this.IsWarehouseFirmCodeVisible = true;
        }

        this.pricingFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "STR" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (this.pricingFeatureToggle && this.TypeCode == "CFS") {
            this.IsStoragePricingVisible = true;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get MyWarehouse() { return this.EntityPM.MyWarehouse; }
    set MyWarehouse(newValue: boolean) {
        if (this.EntityPM.MyWarehouse != newValue) {
            this.EntityPM.MyWarehouse = newValue;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(newValue: string) {
        if (this.EntityPM.TypeCode != newValue) {
            this.EntityPM.TypeCode = newValue;

            if (newValue == "CFS") {
                this.EntityPM.AirWeightMeasurementCode = "GRWT";
                this.EntityPM.OceanWeightMeasurementCode = "GRWT";
                this.EntityPM.InlandWeightMeasurementCode = "GRWT";

                this.EntityPM.AirWeightRoundingCode = "NON";
                this.EntityPM.OceanWeightRoundingCode = "NON";
                this.EntityPM.InlandWeightRoundingCode = "NON";

                if (this.pricingFeatureToggle) {
                    this.IsStoragePricingVisible = true;
                }
            }

            else {
                this.IsStoragePricingVisible = false;
            }
        }
    }

    get FirmCode() { return this.EntityPM.FirmCode; }
    set FirmCode(newValue: string) {
        if (this.EntityPM.FirmCode != newValue) {
            this.EntityPM.FirmCode = newValue;
        }
    }

    StorageDefaultsClicked() {
        var entityResourceService: EntityResourceService = new EntityResourceService();
        entityResourceService.getEntityResourceByTableName("WarehouseStoragePricing").subscribe((res1: any) => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "Storage Defaults";
            logitudeWindow.WindowArgs = this.EntityPM;
            logitudeWindow.Show("./CommonModules/CommonPartners/Components/EditTabs/Warehouse/StorageDefaultsComponents");
        });
    }
}
