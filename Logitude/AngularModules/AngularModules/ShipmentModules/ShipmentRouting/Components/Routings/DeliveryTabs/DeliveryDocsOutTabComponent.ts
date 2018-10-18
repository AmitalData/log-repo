declare var window: any;
import {Component} from '@angular/core';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';

@Component({
    moduleId: module.id,
    templateUrl: './DeliveryDocsOutTabComponent.html',
})

export class DeliveryDocsOutTabComponent {
    public EntityPM: ShipmentDeliveryPM = null;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName = "ShipmentPickUpDelivery";
    public DataContext = this;
    public ObjectTableId: string;
    public EntityId: string;
    public ChildEntityId: string;
    public EntityReference: string
    public ChildObjectTableId: string;
    public ChildEntityReference: string;
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
        this.EntityReference = this.ShipmentPM.ShipmentNumber;
    }
}