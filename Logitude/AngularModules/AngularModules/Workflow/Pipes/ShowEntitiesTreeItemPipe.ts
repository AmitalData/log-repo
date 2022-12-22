import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowEntitiesTreeItemPipe"
})

export class ShowEntitiesTreeItemPipe implements PipeTransform {

    transform(excludedEntities: string[], checkItemKey: boolean = true) {
        if (excludedEntities && excludedEntities.length > 0) {
            return (item: TreeSelectItem) => checkItemKey ? this.showItemByKey(excludedEntities, item) : this.showItemByData(excludedEntities, item);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

    private showItemByKey(excludedEntities: string[], item: TreeSelectItem) {
        return excludedEntities.indexOf(item.key) === -1;
    }

    private showItemByData(excludedEntities: string[], item: TreeSelectItem) {
        let dataEntity = item.data ? (item.data["entity"] || null) : null;
        return dataEntity === null || (dataEntity && excludedEntities.indexOf(dataEntity) === -1);
    }

}