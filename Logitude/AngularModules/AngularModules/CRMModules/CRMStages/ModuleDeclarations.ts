import {StageGeneralTabComponent} from './Components/StageGeneralTabComponent';

export const Components =
    [
        StageGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "StageGeneralTabComponent": { myResult = StageGeneralTabComponent; break; } 

        }

        return myResult;
    }
}