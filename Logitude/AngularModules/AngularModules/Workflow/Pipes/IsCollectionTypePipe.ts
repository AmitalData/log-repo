import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "IsCollectionTypePipe"
})

export class IsCollectionTypePipe implements PipeTransform {

    transform(type: string) {
        if (type) {
            return type.toString().endsWith("[]");
        }
        return false;
    }

}