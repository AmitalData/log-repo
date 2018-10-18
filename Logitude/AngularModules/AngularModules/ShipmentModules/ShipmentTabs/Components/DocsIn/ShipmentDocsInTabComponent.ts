
declare var System: any;
declare var window: any;


import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';

@Component({
    moduleId: module.id,

    templateUrl: './ShipmentDocsInTabComponent.html',
  

})

export class ShipmentDocsInTabComponent implements OnInit {

    public EntityPM: ShipmentPM;
    public EntityId: string ;
    public ObjectTableId: string;
    public TransportModeId: string ;
    public ShipmentlevelCode: string;


    constructor( public entityArgs: EntityArgs) {


    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
                var table = window.ObjectTables.filter(d=> d.Name == "Shipment")[0];
                if (table) this.ObjectTableId = table.Id;
                this.EntityId = this.EntityPM.Id;
                this.TransportModeId = this.EntityPM.TransportModeId;
                this.ShipmentlevelCode = this.EntityPM.ShipmentLevelCode;
        }
    }




}