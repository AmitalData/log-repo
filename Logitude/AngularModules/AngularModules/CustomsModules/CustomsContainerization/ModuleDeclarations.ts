
import { ContainerizationGeneralComponent } from './Components/EditTabs/ContainerizationGeneralComponent';


export const Components =
    [
        
        ContainerizationGeneralComponent,
        
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
        
            case "ContainerizationGeneralComponent": { myResult = ContainerizationGeneralComponent; break; }
        
        }

        return myResult;
    }
}
