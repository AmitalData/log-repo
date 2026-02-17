import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../EntityPMs/TariffPM';
@Component({
    moduleId: module.id,
    templateUrl: "TariffHelperComponent.html",
})
export class TariffHelperComponent {
    public EntityPM: TariffPM;
    public ObjectTableName = "Tariff";
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
    }
    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }
}
