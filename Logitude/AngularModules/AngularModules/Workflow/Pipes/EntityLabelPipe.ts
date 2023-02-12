import { Pipe, PipeTransform } from "@angular/core";
import { Entities } from "Workflow/Utilities/Entities";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {
            let parentEntity = Entities.getParents().find(e => e.Code === entity);
            if (parentEntity) {
                return parentEntity.Name;
            }

            let childEntity = Entities.getChildren().find(e => e.Code === entity);
            if (childEntity) {
                return childEntity.Name;
            }

            return entity;
        }
        return null;
    }

}