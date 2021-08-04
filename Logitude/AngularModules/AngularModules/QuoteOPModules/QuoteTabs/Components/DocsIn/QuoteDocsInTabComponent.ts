
declare var System: any;
declare var window: any;

import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';

@Component({
    
    templateUrl: './QuoteDocsInTabComponent.html',
})

export class QuoteDocsInTabComponent implements OnInit {

    public EntityPM: QuotePM;
    public EntityId: string;
    public ObjectTableId: string;
    public TransportModeId: string;
    public ShipmentlevelCode: string;


    constructor(public entityArgs: EntityArgs) {


    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d=> d.Name == "QuoteOP")[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.TransportModeId = this.EntityPM.TransportModeId;
        }
    }




}