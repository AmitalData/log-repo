import { Pipe, PipeTransform } from "@angular/core";
import { ListItem } from "Workflow/Models/ListItem";
import { PrimitiveDataTypes } from "Workflow/Models/PrimitiveDataTypes";

@Pipe({
    name: "IsObjectTypePipe"
})

export class IsObjectTypePipe implements PipeTransform {

    transform(type: string) {
        let DataTypesItems: ListItem[] = new PrimitiveDataTypes().Items;
        if (type) {
            let typedeclared = DataTypesItems.find(i => i.Code === type)
            return typedeclared ? false : true;
        }
        return false;
    }
}