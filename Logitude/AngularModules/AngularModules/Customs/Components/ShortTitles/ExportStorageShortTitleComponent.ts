import { Component } from '@angular/core';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';


@Component({
    template: `
        <span *ngIf='EntityPM?.StorageNo'>{{EntityPM.StorageNo}} - </span>
        <span *ngIf='EntityPM?.CargoTypeName'>{{EntityPM.CargoTypeName}} - </span>
        <span>{{EntityPM?.ExporterName}}</span> 
    `,
    styles: [`
        span {
            font-size: 18px;
            color: #1B90CB;
        }`
    ]
})
export class ExportStorageShortTitleComponent {
    public EntityPM: ExportStoragePM;


    constructor(
        public entityArgs: EntityArgs
    ) { }


    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }
}
