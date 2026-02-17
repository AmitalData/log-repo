import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {AccountingTransferHeaderPM} from '../../EntityPMs/AccountingTransferHeaderPM';

@Component({
    moduleId: module.id,
    templateUrl: "./AccountingTransferHeaderHelperComponent.html",
})

export class AccountingTransferHeaderHelperComponent {
    public EntityPM: AccountingTransferHeaderPM;
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