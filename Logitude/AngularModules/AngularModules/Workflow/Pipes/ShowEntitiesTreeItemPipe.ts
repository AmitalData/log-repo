import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowEntitiesTreeItemPipe"
})

export class ShowEntitiesTreeItemPipe implements PipeTransform {

    transform(excludedEntities: string[]) {
        if (excludedEntities && excludedEntities.length > 0) {
            return (item: TreeSelectItem) => excludedEntities.indexOf(item.key) === -1;
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

}