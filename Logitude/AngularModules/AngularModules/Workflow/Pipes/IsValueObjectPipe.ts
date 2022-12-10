import { Pipe, PipeTransform } from "@angular/core";
import { WorkFlowInstanceVariableList } from "Workflow/EntityLists/WorkFlowInstanceVariableList";
import { ListItem } from "Workflow/Models/ListItem";

@Pipe({
    name: "IsValueObjectPipe"
})

export class IsValueObjectPipe implements PipeTransform {

    transform(type: string, items: ListItem[] | null = null) {
        if (type) {
            let typedeclared = items.find(i => i.Code === type) 
            return typedeclared ? true : false;
        }
        return false;
    }
}