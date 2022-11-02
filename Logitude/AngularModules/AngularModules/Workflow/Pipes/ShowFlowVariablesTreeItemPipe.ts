import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { Condition } from "Workflow/Models/Condition";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowFlowVariablesTreeItemPipe"
})

export class ShowFlowVariablesTreeItemPipe implements PipeTransform {

    transform(object: Condition | SetValue, flowVariablesTreeList: FlowVariablesTreeList) {
        if (object && flowVariablesTreeList) {
            let compareWithLookupOrPickListType: string | null = null;
            if (object.type === FieldTypes.LookUp) {
                compareWithLookupOrPickListType = object.lookupType;
            } else if (object.type === FieldTypes.PickList) {
                compareWithLookupOrPickListType = object.picklistType;
            }
            return (item: TreeSelectItem) => flowVariablesTreeList.compareItemType(item, object.type, compareWithLookupOrPickListType);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

}