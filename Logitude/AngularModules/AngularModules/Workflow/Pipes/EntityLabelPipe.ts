import { Pipe, PipeTransform } from "@angular/core";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {
            return ObjectTables.getDisplayNameByName(entity);
        }
        return null;
    }

}