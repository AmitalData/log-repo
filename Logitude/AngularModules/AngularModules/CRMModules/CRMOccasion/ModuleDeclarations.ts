import { NewOccasionComponent } from './Components/NewEntity/NewOccasionComponent';
import { OccasionGeneralTabComponent } from './Components/EditTabs/OccasionGeneralTabComponent';
import { OccasionMainTabComponent } from './Components/EditTabs/OccasionMainTabComponent';
import { AddEditOccasionContactComponent } from './Components/AddEdit/AddEditOccasionContactComponent';

export const Components =
    [
        NewOccasionComponent,
        OccasionGeneralTabComponent,
        OccasionMainTabComponent,
        AddEditOccasionContactComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewOccasionComponent": { myResult = NewOccasionComponent; break; } 
            case "OccasionGeneralTabComponent": { myResult = OccasionGeneralTabComponent; break; }
            case "OccasionMainTabComponent": { myResult = OccasionMainTabComponent; break; }
            case "AddEditOccasionContactComponent": { myResult = AddEditOccasionContactComponent; break; }
        }

        return myResult;
    }
}
