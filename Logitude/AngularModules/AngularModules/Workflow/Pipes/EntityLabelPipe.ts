import { Pipe, PipeTransform } from "@angular/core";
import { Entities } from "Workflow/Models/Entities";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {
            entity = Formatter.getEntity(entity);

            let parentEntity = Entities.Parents.find(e => e.Code.toLowerCase() === entity?.toLowerCase());
            if (parentEntity) {
                return parentEntity.Name;
            }

            let childEntity = Entities.Children.find(e => e.Code.toLowerCase() === entity?.toLowerCase());
            if (childEntity) {
                return childEntity.Name;
            }

            return entity ? entity : null;
        }
        return null;
    }

}