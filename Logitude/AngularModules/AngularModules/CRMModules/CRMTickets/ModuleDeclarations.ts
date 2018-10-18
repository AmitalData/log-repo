import {NewTicketComponent} from './Components/NewEntity/NewTicketComponent';
import {ChooseShipmentComponent} from './Components/NewEntity/ChooseShipmentComponent';
import {TicketDetailsTabComponent} from './Components/EditTabs/Details/TicketDetailsTabComponent';
import {TicketMainTabComponent} from './Components/EditTabs/MainTab/TicketMainTabComponent';
import {DetailsTabComponent} from './Components/EditTabs/MainTab/DetailsTabComponent';
import {ActivitiesTabComponent} from './Components/EditTabs/MainTab/ActivitiesTabComponent';
import {SendEmailComponent} from './Components/EditTabs/MainTab/SendEmailComponent';
import {PostsTabComponent} from './Components/EditTabs/MainTab/PostsTabComponent';
import {TicketAuditTabComponent} from './Components/EditTabs/Audit/TicketAuditTabComponent';
import {TicketDocsInTabComponent} from './Components/EditTabs/DocsIn/TicketDocsInTabComponent';
import {TicketDocsOutTabComponent} from './Components/EditTabs/DocsOut/TicketDocsOutTabComponent';
import {TicketEscalationTabComponent} from './Components/EditTabs/Escalation/TicketEscalationTabComponent';
import {TicketOverviewTabComponent} from './Components/EditTabs/Overview/TicketOverviewTabComponent';
import {TicketClosureComponent} from './Components/EditTabs/Others/TicketClosureComponent';

export const Components =
    [
        NewTicketComponent,
        ChooseShipmentComponent,
        TicketDetailsTabComponent,
        TicketMainTabComponent,
        DetailsTabComponent,
        ActivitiesTabComponent,
        SendEmailComponent,

        PostsTabComponent,
        TicketAuditTabComponent,
        TicketDocsInTabComponent,
        TicketDocsOutTabComponent,
        TicketEscalationTabComponent,
        TicketOverviewTabComponent,
        TicketClosureComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewTicketComponent": { myResult = NewTicketComponent; break; }
            case "ChooseShipmentComponent": { myResult = ChooseShipmentComponent; break; }
            case "TicketDetailsTabComponent": { myResult = TicketDetailsTabComponent; break; }
            case "TicketMainTabComponent": { myResult = TicketMainTabComponent; break; }
            case "DetailsTabComponent": { myResult = DetailsTabComponent; break; }
            case "ActivitiesTabComponent": { myResult = ActivitiesTabComponent; break; }
            case "SendEmailComponent": { myResult = SendEmailComponent; break; }
            case "PostsTabComponent": { myResult = PostsTabComponent; break; }
            case "TicketAuditTabComponent": { myResult = TicketAuditTabComponent; break; }
            case "TicketDocsInTabComponent": { myResult = TicketDocsInTabComponent; break; }
            case "TicketDocsOutTabComponent": { myResult = TicketDocsOutTabComponent; break; }
            case "TicketEscalationTabComponent": { myResult = TicketEscalationTabComponent; break; }
            case "TicketOverviewTabComponent": { myResult = TicketOverviewTabComponent; break; }
            case "TicketClosureComponent": { myResult = TicketClosureComponent; break; } 
        }

        return myResult;
    }
}