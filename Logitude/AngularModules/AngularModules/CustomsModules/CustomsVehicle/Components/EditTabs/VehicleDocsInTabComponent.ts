
declare var System: any;
declare var window: any;

import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';


@Component({  
    templateUrl: './VehicleDocsInTabComponent.html',
})

export class VehicleDocsInTabComponent implements OnInit {

    public EntityPM: any;
    public EntityId: string;
    public ObjectTableId: string;

    constructor(public entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.entityArgs.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
        }
    }




}