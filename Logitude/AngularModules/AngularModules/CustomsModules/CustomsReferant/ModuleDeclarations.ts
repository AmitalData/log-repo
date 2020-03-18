import { AddEditReferantExceptionReasonComponent } from "./Components/ReferantExceptionReason/AddEditReferantExceptionReasonComponent";

export const Components =
    [
        AddEditReferantExceptionReasonComponent,
    ];
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditReferantExceptionReasonComponent": { myResult = AddEditReferantExceptionReasonComponent; break; } 
        }
        return myResult;
    }
} 
