import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { Condition } from "Workflow/Models/Condition";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { CompareVariableItemType } from "Workflow/Utilities/CompareVariableItemType";

@Pipe({
    name: "ShowFlowVariablesTreeItemPipe"
})

export class ShowFlowVariablesTreeItemPipe implements PipeTransform {

    transform(object: Condition | SetValue) {
        if (object) {
            let compareWithLookupOrPickListType: string | null = null;
            if (object.type === FieldTypes.LookUp) {
                compareWithLookupOrPickListType = object.lookupType;
            } else if (object.type === FieldTypes.PickList) {
                compareWithLookupOrPickListType = object.picklistType;
            }

            // let type = object.type;
            // if (object.operator === SetValueOperators.Add || object.operator === SetValueOperators.AddField) {
            //     type = type ? type.replace("[]", "") : null;
            // }

            return (item: TreeSelectItem) => CompareVariableItemType.compare(item, object.type, compareWithLookupOrPickListType);
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

}