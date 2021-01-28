import { AddEditReferantExceptionReasonComponent } from "./Components/ReferantExceptionReason/AddEditReferantExceptionReasonComponent";
import { AddExceptionReasonToUnifreightStatusComponent } from './Components/ReferantExceptionReason/AddExceptionReasonToUnifreightStatusComponent';
 

export const Components =
    [
        AddEditReferantExceptionReasonComponent,
        AddExceptionReasonToUnifreightStatusComponent,
        RemarksPopUp,
    ];
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditReferantExceptionReasonComponent": { myResult = AddEditReferantExceptionReasonComponent; break; }
            case "AddExceptionReasonToUnifreightStatusComponent": { myResult = AddExceptionReasonToUnifreightStatusComponent; break; }
 
        }
        return myResult;
    }
} 
