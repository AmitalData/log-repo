import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "IsDeclaredVariablePipe"
})

export class IsDeclaredVariablePipe implements PipeTransform {

    transform(field: string) {
        if (field) {
            return field.toString().startsWith("declaredvariables_");
        }
        return false;
    }

}