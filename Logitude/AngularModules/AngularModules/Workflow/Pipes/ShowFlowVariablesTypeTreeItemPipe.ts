import { Pipe, PipeTransform } from "@angular/core";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowFlowVariablesTypeTreeItemPipe"
})

export class ShowFlowVariablesTypeTreeItemPipe implements PipeTransform {

    transform(type:string, flowVariablesTreeList: FlowVariablesTreeList) {
        if (type && flowVariablesTreeList) {
            let compareWithLookupOrPickListType: string | null = null;
            return (item: TreeSelectItem) => flowVariablesTreeList.compareItemType(item, type, compareWithLookupOrPickListType);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }
}