import { AddEditReferantExceptionReasonComponent } from "./Components/ReferantExceptionReason/AddEditReferantExceptionReasonComponent";
import { AddExceptionReasonToUnifreightStatusComponent } from './Components/ReferantExceptionReason/AddExceptionReasonToUnifreightStatusComponent';
 
import { ReferantWorkspaceComponent } from './Components/ReferantWorkspaces/ReferantWorkspaceComponent';
import { DeclarationReferantDataFiltersMenuComponent } from './Components/FiltersMenu/DeclarationReferantDataFiltersMenuComponent';
 
export const Components =
    [
        AddEditReferantExceptionReasonComponent,
        AddExceptionReasonToUnifreightStatusComponent,
 
        ReferantWorkspaceComponent,
        DeclarationReferantDataFiltersMenuComponent,
    ];
 
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;
         
        switch (name) {
            case "AddEditReferantExceptionReasonComponent": { myResult = AddEditReferantExceptionReasonComponent; break; }
            case "AddExceptionReasonToUnifreightStatusComponent": { myResult = AddExceptionReasonToUnifreightStatusComponent; break; }
 
            case "ReferantWorkspaceComponent": { myResult = ReferantWorkspaceComponent; break; }
            case "DeclarationReferantDataFiltersMenuComponent": { myResult = DeclarationReferantDataFiltersMenuComponent; break; }
 
        }
        return myResult;
    }
} 
