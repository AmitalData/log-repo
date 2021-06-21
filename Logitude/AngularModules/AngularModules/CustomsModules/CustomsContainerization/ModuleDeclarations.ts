import { ContainerizationGeneralComponent } from './Components/EditTabs/ContainerizationGeneralComponent';
import { NewContainerizationComponent } from './Components/NewEntity/NewContainerizationComponent';
import { AgentStatementContainerization } from './Components/Other/AgentStatementContainerization';
import { SendContainerization } from './Components/SendContainerization/SendContainerization';


export const Components =
    [
        
        ContainerizationGeneralComponent,
        NewContainerizationComponent,
        AgentStatementContainerization,
        SendContainerization,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;
        debugger;
        switch (name) {
        
            case "ContainerizationGeneralComponent": { myResult = ContainerizationGeneralComponent; break; }
            case "NewContainerizationComponent": { myResult = NewContainerizationComponent; break; }
            case "AgentStatementContainerization": { myResult = AgentStatementContainerization; break; }
            case "SendContainerization": { myResult = SendContainerization; break; }
        }

        return myResult;
    }
}
