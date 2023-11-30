import { Pipe, PipeTransform } from "@angular/core";
import { SetValueDisabled } from "Workflow/Types";

@Pipe({
    name: "SetValueDisabledPipe"
})

export class SetValueDisabledPipe implements PipeTransform {

    transform(setValueDisabled: SetValueDisabled, setValueSectionCode: string) {
        if (setValueDisabled && setValueSectionCode) {
            if (setValueDisabled.toString().indexOf(",") !== -1) {
                return setValueDisabled.toString().toLowerCase().split(",").indexOf(setValueSectionCode.toString().toLowerCase()) !== -1;
            }
            return setValueDisabled.toString().toLowerCase() === setValueSectionCode.toString().toLowerCase();
        }
        return false;
    }

}