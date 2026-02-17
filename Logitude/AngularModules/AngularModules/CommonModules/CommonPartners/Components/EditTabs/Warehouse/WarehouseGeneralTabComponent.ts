import {Component} from '@angular/core';
import {WarehousePM} from '../../../../../Common/EntityPMs/WarehousePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './WarehouseGeneralTabComponent.html',
})

export class WarehouseGeneralTabComponent extends BaseComponent {
    public EntityPM: WarehousePM;
    public ObjectTableName: string = "Warehouse";
    public DataContext: WarehouseGeneralTabComponent = this;
    public IsWarehouseFirmCodeVisible = false;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        if (SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
            this.IsWarehouseFirmCodeVisible = true;
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
        }
    }

    get FirmCode() { return this.EntityPM.FirmCode; }
    set FirmCode(newValue: string) {
        if (this.EntityPM.FirmCode != newValue) {
            this.EntityPM.FirmCode = newValue;
        }
    }
}