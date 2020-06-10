import { NewHelpResouceComponent } from './Components/NewHelpResouceComponent';
import { HelpResouceGeneralTabComponent } from './Components/HelpResouceGeneralTabComponent';

export const Components =
    [
        NewHelpResouceComponent,
        HelpResouceGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewHelpResouceComponent": { myResult = NewHelpResouceComponent; break; }
            case "HelpResouceGeneralTabComponent": { myResult = HelpResouceGeneralTabComponent; break; }
        }

        return myResult;
    }
}
