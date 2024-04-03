import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowTreeItemPipe"
})

export class ShowTreeItemPipe implements PipeTransform {

    transform(value: string, dataKey: string) {
        if (value && dataKey) {
            return (item: TreeSelectItem) => {
                let data = item.data ? (item.data[dataKey] || null) : null;
                let dataToCompare = data ? data.toString().toLowerCase() : null;
                return value.toString().toLowerCase() === dataToCompare;
            };
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }
    
}