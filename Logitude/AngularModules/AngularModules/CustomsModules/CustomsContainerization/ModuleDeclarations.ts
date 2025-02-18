import { ContainerizationGeneralComponent } from './Components/EditTabs/ContainerizationGeneralComponent';
import { ContainerizationFiltersMenuComponent } from './Components/FiltersMenu/ContainerizationFiltersMenuComponent';
import { NewContainerizationComponent } from './Components/NewEntity/NewContainerizationComponent';
import { AgentStatementContainerization } from './Components/Other/AgentStatementContainerization';
import { ContainerizationListComponent } from './Components/EditTabs/ContainerizationListComponent';
import { SendContainerization } from './Components/SendContainerization/SendContainerization';


export const Components =
    [

        ContainerizationGeneralComponent,
        NewContainerizationComponent,
        AgentStatementContainerization,
        SendContainerization,
        ContainerizationFiltersMenuComponent,
        ContainerizationListComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;
        switch (name) {

            case "ContainerizationGeneralComponent": { myResult = ContainerizationGeneralComponent; break; }
            case "NewContainerizationComponent": { myResult = NewContainerizationComponent; break; }
            case "AgentStatementContainerization": { myResult = AgentStatementContainerization; break; }
            case "SendContainerization": { myResult = SendContainerization; break; }
            case "ContainerizationFiltersMenuComponent": { myResult = ContainerizationFiltersMenuComponent; break; }
            case "ContainerizationListComponent": { myResult = ContainerizationListComponent; break; }

        }

        return myResult;
    }
}
