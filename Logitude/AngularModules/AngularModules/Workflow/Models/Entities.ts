import { Entity, ChildEntity } from "./Types";

export class Entities {

    public static readonly Parents: Entity[] = [
        { Code: "Shipment", Name: "Shipment" },
        { Code: "Customer", Name: "Customer" },
        { Code: "User", Name: "User" },
        { Code: "Opportunity", Name: "Opportunity" },
        { Code: "ShipmentStoragePricing", Name: "Storage Pricing" }
    ];

    public static readonly Children: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ARInvoice", Name: "AR Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
        { Code: "APInvoice", Name: "AP Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
        { Code: "ShipmentReceivable", Name: "Receivable", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPayable", Name: "Payable", ParentEntityCode: "Shipment", ChildField: "ShipmentId" }
    ];

    public static getChildField(parentEntityCode: string, childEntityCode: string) {
        if (parentEntityCode && childEntityCode) {
            let childEntity = this.Children.find(e => e.ParentEntityCode === parentEntityCode && e.Code === childEntityCode);
            return childEntity ? childEntity.ChildField : null;
        }
        return null;
    }

}