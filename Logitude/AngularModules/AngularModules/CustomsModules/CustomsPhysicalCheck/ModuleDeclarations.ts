import {PhysicalCheckGeneralTabComponent} from './Components/EditTabs/General/PhysicalCheckGeneralTabComponent';
import { PhysicalCheckSearchReasultTabComponent } from './Components/EditTabs/SearchReasult/PhysicalCheckSearchReasultTabComponent';


export const Components =
    [
        PhysicalCheckGeneralTabComponent,
        PhysicalCheckSearchReasultTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "PhysicalCheckGeneralTabComponent": { myResult = PhysicalCheckGeneralTabComponent; break; }
            case "PhysicalCheckSearchReasultTabComponent": { myResult = PhysicalCheckSearchReasultTabComponent; break; }

        }

        return myResult;
    }
}
