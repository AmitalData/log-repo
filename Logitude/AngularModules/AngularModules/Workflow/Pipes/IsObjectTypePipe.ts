import { Pipe, PipeTransform } from "@angular/core";
import { DataTypesList } from "Workflow/Models/DataTypesList";
import { ListItem } from "Workflow/Models/ListItem";

@Pipe({
    name: "IsObjectTypePipe"
})

export class IsObjectTypePipe implements PipeTransform {

    transform(type: string) {
        let DataTypesItems: ListItem[] = new DataTypesList().Items;
        if (type) {
            let typedeclared = DataTypesItems.find(i => i.Code === type)
            return typedeclared ? false : true;
        }
        return false;
    }
}