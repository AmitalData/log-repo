import { NewOccasionComponent } from './Components/NewEntity/NewOccasionComponent';
import { OccasionGeneralTabComponent } from './Components/EditTabs/OccasionGeneralTabComponent';
import { OccasionMainTabComponent } from './Components/EditTabs/OccasionMainTabComponent';

export const Components =
    [
        NewOccasionComponent,
        OccasionGeneralTabComponent,
        OccasionMainTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewOccasionComponent": { myResult = NewOccasionComponent; break; } 
            case "OccasionGeneralTabComponent": { myResult = OccasionGeneralTabComponent; break; }
            case "OccasionMainTabComponent": { myResult = OccasionMainTabComponent; break; }
        }

        return myResult;
    }
}
