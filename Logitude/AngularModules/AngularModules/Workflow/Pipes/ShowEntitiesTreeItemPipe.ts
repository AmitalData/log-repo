import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowEntitiesTreeItemPipe"
})

export class ShowEntitiesTreeItemPipe implements PipeTransform {

    transform(excludedEntities: string[], checkItemKey: boolean = true, excludeCustomEntities: boolean = true) {
        if (excludedEntities && excludedEntities.length > 0) {
            return (item: TreeSelectItem) => checkItemKey ?
                this.showItemByKey(excludedEntities, item, excludeCustomEntities) :
                this.showItemByData(excludedEntities, item, excludeCustomEntities);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

    private showItemByKey(excludedEntities: string[], item: TreeSelectItem, excludeCustomEntities: boolean) {
        if (excludeCustomEntities && this.isCustomEntityItem(item)) {
            return false;
        }
        return excludedEntities.indexOf(item.key) === -1;
    }

    private showItemByData(excludedEntities: string[], item: TreeSelectItem, excludeCustomEntities: boolean) {
        if (excludeCustomEntities && this.isCustomEntityItem(item)) {
            return false;
        }
        let dataEntity = item.data ? (item.data["entity"] || null) : null;
        return dataEntity === null || (dataEntity && excludedEntities.indexOf(dataEntity) === -1);
    }

    private isCustomEntityItem(item: TreeSelectItem) {
        let isCustomData = item.data ? (item.data["isCustom"] || null) : null;
        return isCustomData === true;
    }

}