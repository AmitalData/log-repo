import {CRMWorkspaceComponent} from './Components/Workspaces/CRMWorkspaceComponent';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {TicketsComponent} from './Components/Workspaces/TicketsComponent';
import {OverviewWorkspaceComponent} from './Components/Workspaces/OverviewWorkspaceComponent';
import {CustomerWorkspaceComponent} from './Components/Workspaces/CustomerWorkspaceComponent';
import {ActivityWorkspaceComponent} from './Components/Workspaces/ActivityWorkspaceComponent';
import {OpportunityWorkspaceComponent} from './Components/Workspaces/OpportunityWorkspaceComponent';
import {ContactWorkspaceComponent} from './Components/Workspaces/ContactWorkspaceComponent';
import {DashboardWorkspaceComponent} from './Components/Workspaces/DashboardWorkspaceComponent';
import {CloseAsWonOrLostComponent} from './Components/MenuButtons/CloseAsWonOrLostComponent';
import {ReOpen_StageComponent} from './Components/MenuButtons/ReOpen_StageComponent';
import {EditClosedOpportunityComponent} from './Components/MenuButtons/EditClosedOpportunityComponent';
import {ByCreateDateComponent} from './Components/Workspaces/DashboardTabComponents/ByCreateDateComponent';
import {ByInProgressComponent} from './Components/Workspaces/DashboardTabComponents/ByInProgressComponent';
import {CompanyPerformanceComponent} from './Components/Workspaces/DashboardTabComponents/CompanyPerformanceComponent';
import {TicketsWorkspaceComponent} from './Components/Workspaces/TicketsWorkspaceComponent';
import {TicketDashboardComponent} from './Components/Workspaces/TicketDashboardComponent';
import {ByOpenedTicketComponent} from './Components/Workspaces/TicketDashboardTabComponents/ByOpenedTicketComponent';
import {TicketClassificationMaintenanceComponent} from './Components/Workspaces/TicketClassificationMaintenanceComponent';
import {AddEditClassificationComponent} from './Components/Workspaces/AddEditClassificationComponent'; 
import {ByFirstResolveTicketComponent} from './Components/Workspaces/TicketDashboardTabComponents/ByFirstResolveTicketComponent';
import { OccasionWorkspaceComponent } from './Components/Workspaces/OccasionWorkspaceComponent';

// Helpers
import {TicketHelperComponent} from './Components/Helpers/TicketHelperComponent';
import {OpportunityHelperComponent} from './Components/Helpers/OpportunityHelperComponent';
import {ActivityHelperComponent} from './Components/Helpers/ActivityHelperComponent';

// Short Titles
import {TicketShortTitleComponent} from './Components/ShortTitles/TicketShortTitleComponent';
import {ActivityShortTitleComponent} from './Components/ShortTitles/ActivityShortTitleComponent';
import {OpportunityShortTitleComponent} from './Components/ShortTitles/OpportunityShortTitleComponent';
import { ClassificationsTree } from './Controls/ClassificationsTree';

export const Components =
    [
        CRMWorkspaceComponent,
        FieldTemplateComponent,
        TicketsComponent,
        OverviewWorkspaceComponent,
        CustomerWorkspaceComponent,
        ActivityWorkspaceComponent,
        OpportunityWorkspaceComponent,
        ContactWorkspaceComponent,
        DashboardWorkspaceComponent,
        TicketHelperComponent,
        TicketShortTitleComponent,
        ActivityShortTitleComponent,
        OpportunityShortTitleComponent,
        CloseAsWonOrLostComponent,
        ReOpen_StageComponent,
        EditClosedOpportunityComponent,
        OpportunityHelperComponent,
        ActivityHelperComponent,
        ByCreateDateComponent,
        ByInProgressComponent,
        CompanyPerformanceComponent,
        TicketsWorkspaceComponent,
        TicketDashboardComponent,
        ByOpenedTicketComponent,
        TicketClassificationMaintenanceComponent,
        AddEditClassificationComponent,
        ByFirstResolveTicketComponent,
        OccasionWorkspaceComponent,
    ];
export const ControlsComponents =
    [
        ClassificationsTree,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CRMWorkspaceComponent": { myResult = CRMWorkspaceComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "TicketsComponent": { myResult = TicketsComponent; break; }
            case "OverviewWorkspaceComponent": { myResult = OverviewWorkspaceComponent; break; }
            case "CustomerWorkspaceComponent": { myResult = CustomerWorkspaceComponent; break; }
            case "ActivityWorkspaceComponent": { myResult = ActivityWorkspaceComponent; break; }
            case "OpportunityWorkspaceComponent": { myResult = OpportunityWorkspaceComponent; break; }
            case "ContactWorkspaceComponent": { myResult = ContactWorkspaceComponent; break; }
            case "DashboardWorkspaceComponent": { myResult = DashboardWorkspaceComponent; break; }
            case "ReOpen_StageComponent": { myResult = ReOpen_StageComponent; break; } 
            case "EditClosedOpportunityComponent": { myResult = EditClosedOpportunityComponent; break; } 
            case "CloseAsWonOrLostComponent": { myResult = CloseAsWonOrLostComponent; break; } 
            case "ByCreateDateComponent": { myResult = ByCreateDateComponent; break; } 
            case "TicketsWorkspaceComponent": { myResult = TicketsWorkspaceComponent; break; }
            case "TicketDashboardComponent": { myResult = TicketDashboardComponent; break; }
            case "ByOpenedTicketComponent": { myResult = ByOpenedTicketComponent; break; }
            case "TicketClassificationMaintenanceComponent": { myResult = TicketClassificationMaintenanceComponent; break; }
            case "AddEditClassificationComponent": { myResult = AddEditClassificationComponent; break; }
            case "ByFirstResolveTicketComponent": { myResult = ByFirstResolveTicketComponent; break; }    
            case "ByInProgressComponent": { myResult = ByInProgressComponent; break; } 
            case "CompanyPerformanceComponent": { myResult = CompanyPerformanceComponent; break; } 

            // Helpers
            case "TicketHelperComponent": { myResult = TicketHelperComponent; break; }      
            case "OpportunityHelperComponent": { myResult = OpportunityHelperComponent; break; }      
            case "ActivityHelperComponent": { myResult = ActivityHelperComponent; break; }      

            // Short Titles 
            case "TicketShortTitleComponent": { myResult = TicketShortTitleComponent; break; }      
            case "ActivityShortTitleComponent": { myResult = ActivityShortTitleComponent; break; }   
            case "OpportunityShortTitleComponent": { myResult = OpportunityShortTitleComponent; break; }
            case "OccasionWorkspaceComponent": { myResult = OccasionWorkspaceComponent; break; }
                
        }

        return myResult;
    }
}
