import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { CustomsTransferHeaderPM } from '../../EntityPMs/CustomsTransferHeaderPM';

@Component({
    moduleId: module.id,
    templateUrl: "./CustomsTransferHeaderHelperComponent.html",
})

export class CustomsTransferHeaderHelperComponent {
    public EntityPM: CustomsTransferHeaderPM;
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
