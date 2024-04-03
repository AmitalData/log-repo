import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowEditableVariablesTreeItemPipe"
})

export class ShowEditableVariablesTreeItemPipe implements PipeTransform {

    transform(_arg: any) {
        return (item: TreeSelectItem) => {
            if (item.data && item.data["isReadOnlyVariable"]) {
                return false;
            }
            return true;
        };
    }

}