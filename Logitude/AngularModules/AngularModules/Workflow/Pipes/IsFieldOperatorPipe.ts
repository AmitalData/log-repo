import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "IsFieldOperatorPipe"
})

export class IsFieldOperatorPipe implements PipeTransform {

    transform(operatorCode: string) {
        if (operatorCode) {
            return operatorCode.toString().endsWith("<field>") || operatorCode.toString().endsWith("<record>");// || operatorCode.toString().endsWith("<collection>");
        }
        return false;
    }

}