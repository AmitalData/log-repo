
import { StatusesTabComponent } from './Components/EditTabs/StatusesTabComponent';
import { GeneralTabComponent } from './Components/EditTabs/GeneralTabComponent';
import { ContainerAuditTabComponent } from './Components/EditTabs/Audit/ContainerAuditTabComponent';
export const Components =
    [
        StatusesTabComponent,
        GeneralTabComponent,
        ContainerAuditTabComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "StatusesTabComponent": { myResult = StatusesTabComponent; break; }
            case "GeneralTabComponent": { myResult = GeneralTabComponent; break; }
            case "ContainerAuditTabComponent": { myResult = ContainerAuditTabComponent; break; }
        }

        return myResult;
    }
}
