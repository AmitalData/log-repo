import {PhysicalCheckGeneralTabComponent} from './Components/EditTabs/General/PhysicalCheckGeneralTabComponent';


export const Components =
    [
        PhysicalCheckGeneralTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "PhysicalCheckGeneralTabComponent": { myResult = PhysicalCheckGeneralTabComponent; break; }
        }

        return myResult;
    }
}