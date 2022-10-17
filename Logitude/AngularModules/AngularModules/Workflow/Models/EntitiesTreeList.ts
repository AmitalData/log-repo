import { TreeSelectItem } from "./TreeSelectItem";

type Entity = { Code: string, Name: string };
type ChildEntity = { Code: string, Name: string, ParentEntityCode: string, ChildField: string };

export class EntitiesTreeList {
    public Items: TreeSelectItem[] = [];

    private OnlyParentEntities: boolean = false;

    private Entities: Entity[] = [
        { Code: "Shipment", Name: "Shipment" },
        { Code: "Customer", Name: "Customer" },
        { Code: "Opportunity", Name: "Opportunity" }
    ];

    private ChildEntities: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ARInvoice", Name: "AR Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
        { Code: "APInvoice", Name: "AP Invoice", ParentEntityCode: "Shipment", ChildField: "MainEntityId" },
    ];

    constructor(excludedParentEntities: string[], onlyParentEntities: boolean = false) {
        this.OnlyParentEntities = onlyParentEntities;
        this.setEntitiesTreeItems(excludedParentEntities);
    }

    public getChildField(parentEntityCode: string, childEntityCode: string) {
        if (this.OnlyParentEntities) {
            return null;
        }
        let childEntity = this.ChildEntities.find(e => e.ParentEntityCode === parentEntityCode && e.Code === childEntityCode);
        return childEntity ? childEntity.ChildField : null;
    }

    private setEntitiesTreeItems(excludedParentEntities: string[]) {
        let entities = excludedParentEntities && excludedParentEntities.length > 0 ? this.Entities.filter(e => excludedParentEntities.indexOf(e.Code) === -1) : this.Entities;
        entities.forEach(entity => {
            let childrenItems = this.getChildrenItems(entity.Code);
            let entityItem = new TreeSelectItem(entity.Code, entity.Name, this.OnlyParentEntities, true, !this.OnlyParentEntities, false, childrenItems);
            this.Items.push(entityItem);
        });
    }

    private getChildrenItems(parentEntityCode: string) {
        if (this.OnlyParentEntities) {
            return [];
        }
        let childrenItems = [];
        let childEntities = this.ChildEntities.filter(e => e.ParentEntityCode === parentEntityCode);
        childEntities.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(parentEntityCode + "." + childEntity.Code, childEntity.Name, true, true, false, false, []);
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }
}