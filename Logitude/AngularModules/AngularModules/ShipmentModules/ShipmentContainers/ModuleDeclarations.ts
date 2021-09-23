
import { StatusesTabComponent } from './Components/EditTabs/StatusesTabComponent';
import { GeneralTabComponent } from './Components/EditTabs/GeneralTabComponent';
export const Components =
    [
        StatusesTabComponent,
        GeneralTabComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "StatusesTabComponent": { myResult = StatusesTabComponent; break; }
            case "GeneralTabComponent": { myResult = GeneralTabComponent; break; }
        }

        return myResult;
    }
}
