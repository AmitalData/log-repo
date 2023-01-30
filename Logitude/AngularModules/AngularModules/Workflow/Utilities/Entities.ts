import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { Entity, ChildEntity } from "Workflow/Types";

export class Entities {

    private static DefaultParents: string[] = [
        "Shipment",
        "Container",
        "Customer",
        "User",
        "Opportunity",
        "ShipmentStoragePricing"
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
            .filter(o => this.DefaultParents.includes(o.Name))
            .sort((a, b) => this.getEntityOrder(a) - this.getEntityOrder(b))
            .map(objectTable => {
                return {
                    Code: objectTable.Name,
                    Name: this.getEntityDisplayName(objectTable)
                };
            });
    }

    public static getChildren(): ChildEntity[] {
        return ObjectTables.getAll()
            .filter(o => this.DefaultChildren.includes(o.Name) || (o.IsCustom && o.ParentObjectTableId))
            .sort((a, b) => this.getEntityOrder(a, true) - this.getEntityOrder(b, true))
            .map(objectTable => {
                return {
                    Code: objectTable.Name,
                    Name: this.getEntityDisplayName(objectTable),
                    ParentEntityCode: this.getParentEntityCode(objectTable)
                };
            });
    }

    private static getEntityDisplayName(objectTable: ObjectTableList): string | null {
        let objectTableName = objectTable ? objectTable.Name : null;
        switch (objectTableName) {
            case "ShipmentStoragePricing":
                return "Storage Pricing";
            case "ShipmentPackage":
                return "Package";
            case "ShipmentReceivable":
                return "Receivable";
            case "ShipmentPayable":
                return "Payable";
            default:
                return this.getGeneralEntityDisplayName(objectTable);
        }
    }

    private static getGeneralEntityDisplayName(objectTable: ObjectTableList): string | null {
        let entityDisplayName: string | null = null;
        let objectTableName = objectTable ? objectTable.Name : null;
        if (objectTableName) {
            if (objectTableName.indexOf(".") !== -1) {
                let objectTableNameSections = objectTableName.split(".");
                entityDisplayName = objectTableNameSections[objectTableNameSections.length - 1];
            } else {
                entityDisplayName = objectTableName;
            }

            entityDisplayName = entityDisplayName.replace(/([a-z])([A-Z])/g, "$1 $2");
            entityDisplayName = entityDisplayName.replace(/([A-Z])([A-Z][a-z])/g, "$1 $2");
        }
        return entityDisplayName;
    }

    private static getParentEntityCode(objectTable: ObjectTableList): string | null {
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

    private static getEntityOrder(objectTable: ObjectTableList, isChildren: boolean = false): number {
        let lastOrder = (isChildren ? this.DefaultChildren.length : this.DefaultParents.length) + 1;
        let objectTableName = objectTable ? objectTable.Name : null;
        if (objectTableName) {
            let entityIndex = (isChildren ? this.DefaultChildren : this.DefaultParents).indexOf(objectTableName);
            return entityIndex === -1 ? lastOrder : entityIndex;
        }
        return lastOrder;
    }

}