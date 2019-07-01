import { NewOccasionComponent } from './Components/NewEntity/NewOccasionComponent';
import { OccasionGeneralTabComponent } from './Components/EditTabs/OccasionGeneralTabComponent';

export const Components =
    [
        NewOccasionComponent,
        OccasionGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewOccasionComponent": { myResult = NewOccasionComponent; break; } 
            case "OccasionGeneralTabComponent": { myResult = OccasionGeneralTabComponent; break; } 
        }

        return myResult;
    }
}
