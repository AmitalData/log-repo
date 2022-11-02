import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    transform(entity: string) {
        if (entity) {
            return entity.indexOf(".") === -1 ? entity : entity.split(".")[1];
        }
        return null;
    }

}