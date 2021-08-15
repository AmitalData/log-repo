
declare var System: any;
declare var window: any;
import {Component, OnInit, ElementRef, Output, EventEmitter}  from '@angular/core';

import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypeListService} from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import {QuoteOPPM} from '../../../../QuoteOPM/EntityPMs/QuoteOPPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    selector: 'QuoteDocsOutTabComponent',
    
    templateUrl: './QuoteDocsOutTabComponent.html',
    providers: [DocumentTypeListService]
})


export class QuoteDocsOutTabComponent implements OnInit {


    public ObjectTableId: string;
    public EntityId: string;
    public TransportModeId: string;
    public EntityReference: string
    //IdentityKey: string;
    public EntityPM: QuoteOPPM;

    constructor(
        public entityArgs: EntityArgs
    ) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d=> d.Name == "QuoteOP")[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.EntityReference = this.EntityPM.QuoteNumber;

        }

    }





}