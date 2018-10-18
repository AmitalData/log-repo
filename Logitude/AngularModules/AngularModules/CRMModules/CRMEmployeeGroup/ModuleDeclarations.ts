import {EmployeeGroupGeneralTabComponent} from './Components/EditTabs/EmployeeGroupGeneralTabComponent'; 
import {NewEmployeeComponent} from './Components/NewEntity/NewEmployeeComponent';

export const Components =
    [
        NewEmployeeComponent,
        EmployeeGroupGeneralTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewEmployeeComponent": { myResult = NewEmployeeComponent; break; }
            case "EmployeeGroupGeneralTabComponent": { myResult = EmployeeGroupGeneralTabComponent; break; }

        }

        return myResult;
    }
}