import { Pipe, PipeTransform } from "@angular/core";
import { ListItem } from "Workflow/Models/ListItem";

@Pipe({
    name: "ListItemPipe"
})

export class ListItemPipe implements PipeTransform {

    transform(itemCode: string, items: ListItem[] | null = null) {
        if (itemCode) {
            return items ? items.find(i => i.Code === itemCode) : new ListItem(itemCode);
        }
        return null;
    }

}