import {PhysicalCheckGeneralTabComponent} from './Components/EditTabs/General/PhysicalCheckGeneralTabComponent';
import { PhysicalCheckSearchReasultTabComponent } from './Components/EditTabs/SearchReasult/PhysicalCheckSearchReasultTabComponent';
import { PhysicalCheckFiltersMenuComponent } from './Components/FiltersMenu/PhysicalCheckFiltersMenuComponent';


export const Components =
    [
        PhysicalCheckGeneralTabComponent,
        PhysicalCheckSearchReasultTabComponent,
        PhysicalCheckFiltersMenuComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "PhysicalCheckGeneralTabComponent": { myResult = PhysicalCheckGeneralTabComponent; break; }
            case "PhysicalCheckSearchReasultTabComponent": { myResult = PhysicalCheckSearchReasultTabComponent; break; }
            case "PhysicalCheckFiltersMenuComponent": { myResult = PhysicalCheckFiltersMenuComponent; break; }

        }

        return myResult;
    }
}
