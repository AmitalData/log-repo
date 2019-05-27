import { Component, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './TariffGeneralTabComponent.html',
})

export class TariffGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public DataContext = this;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    get SellerId() { return this.EntityPM.SellerId; }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;
        }
    }

    get ContractNumber() { return this.EntityPM.ContractNumber; }
    set ContractNumber(value: string) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }
}
