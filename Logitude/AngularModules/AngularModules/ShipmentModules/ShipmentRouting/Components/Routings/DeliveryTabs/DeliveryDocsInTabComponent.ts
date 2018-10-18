declare var window: any;
import {Component} from '@angular/core';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';

@Component({
    moduleId: module.id,
    templateUrl: './DeliveryDocsInTabComponent.html',
})

export class DeliveryDocsInTabComponent {
    public EntityPM: ShipmentDeliveryPM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public DataContext = this;
    public ObjectTableId: string;
    public EntityId: string;
    public ChildEntityId: string;
    public ChildEntityReference: string;
    public ChildObjectTableId: string;
    constructor() {
    }

    InitTab(myEntityPM: ShipmentDeliveryPM, myShipmentPM: ShipmentPM) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        var deliveryobjecttable = window.ObjectTables.filter(d => d.Name == "ShipmentPickUpDelivery")[0];
        var shipmentobjecttable = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
        this.EntityId = this.ShipmentPM.Id;
        this.ChildEntityId = this.EntityPM.Id;
        this.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;
        this.ObjectTableId = shipmentobjecttable.Id;
        this.ChildObjectTableId = deliveryobjecttable.Id;
    }
}