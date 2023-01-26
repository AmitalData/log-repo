import { Pipe, PipeTransform } from "@angular/core";
import { Entities } from "Workflow/Utilities/Entities";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {

            let parentEntity = Entities.parents().find(e => e.Code === entity);
            if (parentEntity) {
                return parentEntity.Name;
            }

            let childEntity = Entities.children().find(e => e.Code === entity);
            if (childEntity) {
                return childEntity.Name;
            }

            return entity ? entity : null;
        }
        return null;
    }

}