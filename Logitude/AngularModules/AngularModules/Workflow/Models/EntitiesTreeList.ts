import { TreeSelectItem } from "./TreeSelectItem";

export class EntitiesTreeList {
    public Items: TreeSelectItem[] = [];

    private ShipmentChildEntities: { Key: string, Title: string }[] = [
        { Key: "Container", Title: "Container" },
        { Key: "ShipmentPackage", Title: "Package" },
        { Key: "ARInvoice", Title: "AR Invoice" },
        { Key: "APInvoice", Title: "AP Invoice" }
    ];

    constructor() {
        this.setEntitiesTreeItems();
    }

    private setEntitiesTreeItems() {
        let shipmentItemChildren = this.getShipmentItemChildren();

        let shipmentItem = new TreeSelectItem("Shipment", "Shipment", false, true, true, false, shipmentItemChildren);
        let customerItem = new TreeSelectItem("Customer", "Customer", false, true, false, false, []);

        this.Items.push(shipmentItem);
        this.Items.push(customerItem);
    }

    private getShipmentItemChildren() {
        let shipmentItemChildren = [];

        this.ShipmentChildEntities.forEach(shipmentChildEntity => {
            let treeSelectItem = new TreeSelectItem(shipmentChildEntity.Key, shipmentChildEntity.Title, true, true, false, false, []);
            shipmentItemChildren.push(treeSelectItem);
        });

        return shipmentItemChildren;
    }
}