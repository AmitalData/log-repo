import { Pipe, PipeTransform } from "@angular/core";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Pipe({
    name: "ShowExpressionTreeItemPipe"
})

export class ShowExpressionTreeItemPipe implements PipeTransform {

    transform(categoryCode: string) {
        if(categoryCode && categoryCode !== "ALL"){
            return (item: TreeSelectItem) => { return item.data.CategoryCode === categoryCode; };
        }
        return (_treeSelectItem: TreeSelectItem) => { return true };
    }

}