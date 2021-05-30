
import { ContainerizationGeneralComponent } from './Components/EditTabs/ContainerizationGeneralComponent';
import { NewContainerizationComponent } from './Components/NewEntity/NewContainerizationComponent';


export const Components =
    [
        
        ContainerizationGeneralComponent,
        NewContainerizationComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
        
            case "ContainerizationGeneralComponent": { myResult = ContainerizationGeneralComponent; break; }
            case "NewContainerizationComponent": { myResult = NewContainerizationComponent; break; }

        }

        return myResult;
    }
}
