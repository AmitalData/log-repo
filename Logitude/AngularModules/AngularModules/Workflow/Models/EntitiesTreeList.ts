import { TreeSelectItem } from "./TreeSelectItem";

type Entity = { Code: string, Name: string };
type ChildEntity = { Code: string, Name: string, ParentEntityCode: string, ChildField: string };

export class EntitiesTreeList {
    public Items: TreeSelectItem[] = [];

    private Entities: Entity[] = [
        { Code: "Shipment", Name: "Shipment" },
        { Code: "Customer", Name: "Customer" },
        { Code: "User", Name: "User" }
    ];

    private ChildEntities: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ARInvoice", Name: "AR Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
        { Code: "APInvoice", Name: "AP Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
    ];

    constructor() {
        this.setEntitiesTreeItems();
    }

    public getChildField(parentEntityCode: string, childEntityCode: string) {
        let childEntity = this.ChildEntities.find(e => e.ParentEntityCode === parentEntityCode && e.Code === childEntityCode);
        return childEntity ? childEntity.ChildField : null;
    }

    private setEntitiesTreeItems() {
        this.Entities.forEach(entity => {
            let childrenItems = this.getChildrenItems(entity.Code);
            let entityItem = new TreeSelectItem(entity.Code, entity.Name, false, true, true, false, childrenItems);
            this.Items.push(entityItem);
        });
    }

    private getChildrenItems(parentEntityCode: string) {
        let childrenItems = [];
        let childEntities = this.ChildEntities.filter(e => e.ParentEntityCode === parentEntityCode);
        childEntities.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(parentEntityCode + "." + childEntity.Code, childEntity.Name, true, true, false, false, []);
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }
}