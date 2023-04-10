import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { Entity } from "Workflow/Types";

export class Entities {

    private static DefaultParents: string[] = [
        "Shipment",
        "Container",
        "Task",
        "Customer",
        "User",
        "Opportunity",
        "ShipmentStoragePricing",
        "Card",
        "Contact",
        "Country",
        "Currency",
        "CustomerTeam",
        "Department",
        "DocumentType",
        "EntityStatus",
        "PackageType",
        "Port",
        "SpecialServicesType",
        "Vessel"
    ];

    private static DefaultChildren: string[] = [
        "ShipmentPackage",
        "ARInvoice",
        "APInvoice",
        "ShipmentReceivable",
        "ShipmentPayable"
    ];

    public static getParents(): Entity[] {
        return ObjectTables.getAll()
            .filter(o => this.DefaultParents.includes(o.Name) || (o.IsCustom && !o.ParentObjectTableId))
            .sort((a, b) => this.getEntityOrder(a) - this.getEntityOrder(b))
            .map(objectTable => {
                return {
                    Code: objectTable.Name,
                    Name: this.getEntityDisplayName(objectTable),
                    IsCustom: objectTable.IsCustom,
                    IsChild: false,
                    ParentEntity: null
                };
            });
    }

    public static getChildren(parentEntity: string | null = null, excludeInvoiceEntity: boolean = true): Entity[] {
        return ObjectTables.getAll()
            .filter(o => !excludeInvoiceEntity || (o.Name !== "ARInvoice" && o.Name !== "APInvoice"))
            .filter(o => !parentEntity || (this.getParentEntity(o) === parentEntity))
            .filter(o => this.DefaultChildren.includes(o.Name) || (o.IsCustom && o.ParentObjectTableId))
            .sort((a, b) => this.getEntityOrder(a, true) - this.getEntityOrder(b, true))
            .map(objectTable => {
                return {
                    Code: objectTable.Name,
                    Name: this.getEntityDisplayName(objectTable),
                    IsCustom: objectTable.IsCustom,
                    IsChild: true,
                    ParentEntity: this.getParentEntity(objectTable)
                };
            });
    }

    private static getEntityOrder(objectTable: ObjectTableList, isChildren: boolean = false): number {
        let entityOrder = (isChildren ? this.DefaultChildren.length : this.DefaultParents.length) + 1;
        let objectTableName = objectTable ? objectTable.Name : null;
        if (objectTableName) {
            let entityIndex = (isChildren ? this.DefaultChildren : this.DefaultParents).indexOf(objectTableName);
            return entityIndex === -1 ? entityOrder : entityIndex;
        }
        return entityOrder;
    }

    private static getEntityDisplayName(objectTable: ObjectTableList): string | null {
        if (objectTable) {
            return objectTable.FullNameTextCodeDefaultText || objectTable.Name;
        }
        return null;
    }

    private static getParentEntity(objectTable: ObjectTableList): string | null {
        let objectTableName = objectTable ? objectTable.Name : null;
        switch (objectTableName) {
            case "ShipmentPackage":
            case "ARInvoice":
            case "APInvoice":
            case "ShipmentReceivable":
            case "ShipmentPayable":
                return "Shipment";
            default:
                return ObjectTables.getNameById(objectTable ? objectTable.ParentObjectTableId : null);
        }
    }

}