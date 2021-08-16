
import { StatusesTabComponent } from './Components/EditTabs/StatusesTabComponent';

export const Components =
    [
        StatusesTabComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "StatusesTabComponent": { myResult = StatusesTabComponent; break; }
        }

        return myResult;
    }
}
