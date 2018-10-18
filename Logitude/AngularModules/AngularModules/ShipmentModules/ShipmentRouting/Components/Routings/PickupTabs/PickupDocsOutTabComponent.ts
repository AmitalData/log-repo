declare var window: any;
import {Component} from '@angular/core';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpPM';

@Component({
    moduleId: module.id,
    templateUrl: './PickupDocsOutTabComponent.html',
})

export class PickupDocsOutTabComponent {
    public EntityPM: ShipmentPickUpPM = null;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName = "ShipmentPickUpDelivery";
    public DataContext = this;
    public ObjectTableId: string = null;
    public EntityId: string = null;
    public ChildEntityId: string = null;
    public EntityReference: string = null;
    public ChildObjectTableId: string = null;
    public ChildEntityReference: string = null;
    constructor() {

    }

    InitTab(myEntityPM: ShipmentPickUpPM, myShipmentPM: ShipmentPM) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;

        var deliveryobjecttable = window.ObjectTables.filter(d => d.Name == "ShipmentPickUpDelivery")[0];
        var shipmentobjecttable = window.ObjectTables.filter(d => d.Name == "Shipment")[0];

        this.EntityId = this.ShipmentPM.Id;
        this.ObjectTableId = shipmentobjecttable.Id;
        this.EntityReference = this.ShipmentPM.ShipmentNumber;

        this.ChildEntityId = this.EntityPM.Id;
        this.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;
        this.ChildObjectTableId = deliveryobjecttable.Id;
    }
}