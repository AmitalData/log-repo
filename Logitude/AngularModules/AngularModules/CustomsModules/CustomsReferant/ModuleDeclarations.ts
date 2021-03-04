import { AddEditReferantExceptionReasonComponent } from "./Components/ReferantExceptionReason/AddEditReferantExceptionReasonComponent";
import { AddExceptionReasonToUnifreightStatusComponent } from './Components/ReferantExceptionReason/AddExceptionReasonToUnifreightStatusComponent';
 
import { ReferantWorkspaceComponent } from './Components/ReferantWorkspaces/ReferantWorkspaceComponent';
import { DeclarationReferantDataFiltersMenuComponent } from './Components/FiltersMenu/DeclarationReferantDataFiltersMenuComponent';
import { AddEditExceptionReasonComponent } from './Components/ReferantExceptionReason/AddEditExceptionReasonComponent';
 
export const Components =
    [
        AddEditReferantExceptionReasonComponent,
        AddExceptionReasonToUnifreightStatusComponent,
        AddEditExceptionReasonComponent,
        ReferantWorkspaceComponent,
        DeclarationReferantDataFiltersMenuComponent,
    ];
 
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;
         
        switch (name) {
            case "AddEditReferantExceptionReasonComponent": { myResult = AddEditReferantExceptionReasonComponent; break; }
            case "AddExceptionReasonToUnifreightStatusComponent": { myResult = AddExceptionReasonToUnifreightStatusComponent; break; }
            case "AddEditExceptionReasonComponent": { myResult = AddEditExceptionReasonComponent; break; }
            case "ReferantWorkspaceComponent": { myResult = ReferantWorkspaceComponent; break; }
            case "DeclarationReferantDataFiltersMenuComponent": { myResult = DeclarationReferantDataFiltersMenuComponent; break; }
 
        }
        return myResult;
    }
} 
