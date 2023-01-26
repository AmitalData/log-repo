import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { Entity, ChildEntity } from "Workflow/Types";

export class Entities {

    private static readonly FlowEntities: string[] = [
        "Shipment",
        "Container",
        "Customer",
        "User",
        "Opportunity",
        "ShipmentStoragePricing"
    ];

    private static readonly FlowChildEntities: string[] = [
        "ShipmentPackage",
        "ARInvoice",
        "APInvoice",
        "ShipmentReceivable",
        "ShipmentPayable"
    ];

    public static parents(): Entity[] {
        return ObjectTables.getAll()
            .filter(o => this.FlowEntities.includes(o.Name)).map(objectTable => {
                return { Code: objectTable.Name, Name: this.getEntityDisplayName(objectTable) };
            });
    }

    public static children(): ChildEntity[] {
        return ObjectTables.getAll()
            .filter(o => this.FlowChildEntities.includes(o.Name) || o.IsCustom)
            .sort((a, b) => Number(a.IsCustom) - Number(b.IsCustom))
            .map(objectTable => {
                return { Code: objectTable.Name, Name: this.getEntityDisplayName(objectTable), ParentEntityCode: this.getParentEntityCode(objectTable) };
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

}