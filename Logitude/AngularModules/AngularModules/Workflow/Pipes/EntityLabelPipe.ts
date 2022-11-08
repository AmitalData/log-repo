import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {
            return Formatter.getEntity(entity);
        }
        return null;
    }

}