
import { ContainerizationGeneralComponent } from './Components/EditTabs/ContainerizationGeneralComponent';
import { NewContainerizationComponent } from './Components/NewEntity/NewContainerizationComponent';
import { AgentStatementContainerization } from './Components/Other/AgentStatementContainerization';


export const Components =
    [
        
        ContainerizationGeneralComponent,
        NewContainerizationComponent,
        AgentStatementContainerization,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
        
            case "ContainerizationGeneralComponent": { myResult = ContainerizationGeneralComponent; break; }
            case "NewContainerizationComponent": { myResult = NewContainerizationComponent; break; }
            case "AgentStatementContainerization": { myResult = AgentStatementContainerization; break; }

        }

        return myResult;
    }
}
