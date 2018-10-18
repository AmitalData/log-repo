import { ProceduralFaultsGeneralTabComponent } from './Components/EditTabs/General/ProceduralFaultsGeneralTabComponent';


export const Components =
    [
        ProceduralFaultsGeneralTabComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ProceduralFaultsGeneralTabComponent": { myResult = ProceduralFaultsGeneralTabComponent; break; }
        }

        return myResult;
    }
} 