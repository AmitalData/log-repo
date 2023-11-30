import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { CompareVariableItemType } from "Workflow/Utilities/CompareVariableItemType";

@Pipe({
    name: "ShowFlowVariablesTypeTreeItemPipe"
})

export class ShowFlowVariablesTypeTreeItemPipe implements PipeTransform {

    transform(type: string) {
        if (type) {
            return (item: TreeSelectItem) => CompareVariableItemType.compare(item, type, null);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }
}