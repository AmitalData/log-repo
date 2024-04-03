import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowEntitiesTreeItemPipe"
})

export class ShowEntitiesTreeItemPipe implements PipeTransform {

    private ExcludeEntitiesDictionary = {
        "Customer": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "User": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "ShipmentStoragePricing": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Opportunity": { Start: true, GetReadOnlyRecord: true, GetEditableRecord: true },
        "Card": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Contact": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Country": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Currency": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "CustomerTeam": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Department": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "DocumentType": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "EntityStatus": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "PackageType": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Port": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "SpecialServicesType": { Start: true, CreateRecord: true, GetEditableRecord: true },
        "Vessel": { Start: true, CreateRecord: true, GetEditableRecord: true }
    };

    transform(sectionCode: string, excludeCustomEntities: boolean = false) {
        return (item: TreeSelectItem) => this.showItem(item, sectionCode, excludeCustomEntities);
    }

    private showItem(item: TreeSelectItem, sectionCode: string, excludeCustomEntities: boolean) {
        if (excludeCustomEntities && this.isCustomEntityItem(item)) {
            return false;
        }

        if (this.excludeEntityItem(item, sectionCode)) {
            return false;
        }

        return true;
    }

    private isCustomEntityItem(item: TreeSelectItem) {
        let isCustom = item && item.data ? (item.data["isCustom"] || false) : false;
        return isCustom;
    }

    private excludeEntityItem(item: TreeSelectItem, sectionCode: string) {
        let entity = item && item.data ? (item.data["entity"] || null) : null;
        if (entity && sectionCode) {
            let excludeEntity = this.ExcludeEntitiesDictionary[entity];
            if (excludeEntity) {
                return excludeEntity[sectionCode] || false;
            }
        }
        return false;
    }

}