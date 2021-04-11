import {NewAgentComponent} from './Components/NewEntity/NewAgentComponent';
import {AgentGeneralTabComponent} from './Components/EditTabs/AgentGeneralTabComponent';
import {AgentSharedLogisticsTabComponent} from './Components/EditTabs/AgentSharedLogisticsTabComponent';
import { AgentShareInvitaionComponent } from './Components/EditTabs/AgentShareInvitaionComponent';
import { AcceptAgentInvitaionComponent } from './Components/EditTabs/AcceptAgentInvitaionComponent';
import { AgentDocsInTabComponent } from './Components/EditTabs/AgentDocsInTabComponent';
import { AgentDocsOutTabComponent } from './Components/EditTabs/AgentDocsOutTabComponent';
import {AgentBillingTabComponent} from './Components/EditTabs/AgentBillingTabComponent';


export const Components =
    [
        NewAgentComponent,
        AgentGeneralTabComponent,
        AgentSharedLogisticsTabComponent,
        AgentShareInvitaionComponent,
        AcceptAgentInvitaionComponent,
        AgentBillingTabComponent,
        AgentDocsInTabComponent,
        AgentDocsOutTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewAgentComponent": { myResult = NewAgentComponent; break; }
            case "AgentGeneralTabComponent": { myResult = AgentGeneralTabComponent; break; }
            case "AgentSharedLogisticsTabComponent": { myResult = AgentSharedLogisticsTabComponent; break; }
            case "AgentShareInvitaionComponent": { myResult = AgentShareInvitaionComponent; break; }
            case "AcceptAgentInvitaionComponent": { myResult = AcceptAgentInvitaionComponent; break; }
            case "AgentBillingTabComponent": { myResult = AgentBillingTabComponent; break; }
            case "AgentDocsInTabComponent": { myResult = AgentDocsInTabComponent; break; }
            case "AgentDocsOutTabComponent": { myResult = AgentDocsOutTabComponent; break; }
        }

        return myResult;
    }
}
