import { NewCustomerTeamComponent } from './Components/NewEntity/NewCustomerTeamComponent';
import { CustomerTeamGeneralTabComponent } from './Components/EditTabs/CustomerTeamGeneralTabComponent';

export const Components =
    [
        NewCustomerTeamComponent,
        CustomerTeamGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewCustomerTeamComponent": { myResult = NewCustomerTeamComponent; break; }
            case "CustomerTeamGeneralTabComponent": { myResult = CustomerTeamGeneralTabComponent; break; }
        }

        return myResult;
    }
}
