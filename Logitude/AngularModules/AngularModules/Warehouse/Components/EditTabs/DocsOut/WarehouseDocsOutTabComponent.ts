
declare var System: any;
declare var window: any;
import {Component, OnInit, ElementRef, Output, EventEmitter}  from '@angular/core';

import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';

import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    selector: 'WarehouseDocsOutTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseDocsOutTabComponent.html',
    providers: [DocumentTypeListService]
})


export class WarehouseDocsOutTabComponent implements OnInit {


    public ObjectTableId: string;
    public EntityId: string;
    public TransportModeId: string;
    public EntityReference: string
    public EntityPM: any;

    constructor(
        public entityArgs: EntityArgs
    ) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.entityArgs.ObjectTableName)[0];

            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
          ///  this.TransportModeId = this.EntityPM.TransportModeId;
            this.EntityReference = this.entityArgs.ObjectTableName == "WarehouseEntry" ? this.EntityPM.EntryNumber : this.EntityPM.ReleaseNumber;

        }

    }





}