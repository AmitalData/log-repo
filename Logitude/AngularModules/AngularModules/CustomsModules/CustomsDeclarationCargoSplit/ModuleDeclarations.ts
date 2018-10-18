
import { DeclarationCargoSplitEditComponent } from './Components/EditTabs/DeclarationCargoSplitEditComponent';
import { NewDeclarationCargoSplitComponent } from './Components/NewEntity/NewDeclarationCargoSplitComponent';
import { DecCargoSplitConComponent } from './Components/EditTabs/DecCargoSplitConComponent';
import { DecCargoSplitConsPackDetComponent } from './Components/EditTabs/DecCargoSplitConsPackDetComponent';

import { CargoSplitGeneralTabComponent } from './Components/EditTabs/General/CargoSplitGeneralTabComponent';




export const Components =
    [
    DeclarationCargoSplitEditComponent,
    NewDeclarationCargoSplitComponent,
    DecCargoSplitConComponent,
    DecCargoSplitConsPackDetComponent,
    CargoSplitGeneralTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

      switch (name) {
        case "NewDeclarationCargoSplitComponent": { myResult = NewDeclarationCargoSplitComponent; break; }
        case "CargoSplitGeneralTabComponent": { myResult = CargoSplitGeneralTabComponent; break; }
        case "DeclarationCargoSplitEditComponent": { myResult = DeclarationCargoSplitEditComponent; break; }

        case "DecCargoSplitConComponent": { myResult = DecCargoSplitConComponent; break; }
        case "DecCargoSplitConsPackDetComponent": { myResult = DecCargoSplitConsPackDetComponent; break; }      
          
        }

        return myResult;
    }
}
