"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CRMWorkspaceComponent_1 = require("./Components/Workspaces/CRMWorkspaceComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var TicketsComponent_1 = require("./Components/Workspaces/TicketsComponent");
var OverviewWorkspaceComponent_1 = require("./Components/Workspaces/OverviewWorkspaceComponent");
var CustomerWorkspaceComponent_1 = require("./Components/Workspaces/CustomerWorkspaceComponent");
var ActivityWorkspaceComponent_1 = require("./Components/Workspaces/ActivityWorkspaceComponent");
var OpportunityWorkspaceComponent_1 = require("./Components/Workspaces/OpportunityWorkspaceComponent");
var ContactWorkspaceComponent_1 = require("./Components/Workspaces/ContactWorkspaceComponent");
var DashboardWorkspaceComponent_1 = require("./Components/Workspaces/DashboardWorkspaceComponent");
var CloseAsWonOrLostComponent_1 = require("./Components/MenuButtons/CloseAsWonOrLostComponent");
var ReOpen_StageComponent_1 = require("./Components/MenuButtons/ReOpen_StageComponent");
var EditClosedOpportunityComponent_1 = require("./Components/MenuButtons/EditClosedOpportunityComponent");
var ByCreateDateComponent_1 = require("./Components/Workspaces/DashboardTabComponents/ByCreateDateComponent");
var ByInProgressComponent_1 = require("./Components/Workspaces/DashboardTabComponents/ByInProgressComponent");
var CompanyPerformanceComponent_1 = require("./Components/Workspaces/DashboardTabComponents/CompanyPerformanceComponent");
var TicketsWorkspaceComponent_1 = require("./Components/Workspaces/TicketsWorkspaceComponent");
var TicketDashboardComponent_1 = require("./Components/Workspaces/TicketDashboardComponent");
var ByOpenedTicketComponent_1 = require("./Components/Workspaces/TicketDashboardTabComponents/ByOpenedTicketComponent");
var TicketClassificationMaintenanceComponent_1 = require("./Components/Workspaces/TicketClassificationMaintenanceComponent");
var AddEditClassificationComponent_1 = require("./Components/Workspaces/AddEditClassificationComponent");
var ByFirstResolveTicketComponent_1 = require("./Components/Workspaces/TicketDashboardTabComponents/ByFirstResolveTicketComponent");
var OccasionWorkspaceComponent_1 = require("./Components/Workspaces/OccasionWorkspaceComponent");
// Helpers
var TicketHelperComponent_1 = require("./Components/Helpers/TicketHelperComponent");
var OpportunityHelperComponent_1 = require("./Components/Helpers/OpportunityHelperComponent");
var ActivityHelperComponent_1 = require("./Components/Helpers/ActivityHelperComponent");
// Short Titles
var TicketShortTitleComponent_1 = require("./Components/ShortTitles/TicketShortTitleComponent");
var ActivityShortTitleComponent_1 = require("./Components/ShortTitles/ActivityShortTitleComponent");
var OpportunityShortTitleComponent_1 = require("./Components/ShortTitles/OpportunityShortTitleComponent");
var ClassificationsTree_1 = require("./Controls/ClassificationsTree");
exports.Components = [
    CRMWorkspaceComponent_1.CRMWorkspaceComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    TicketsComponent_1.TicketsComponent,
    OverviewWorkspaceComponent_1.OverviewWorkspaceComponent,
    CustomerWorkspaceComponent_1.CustomerWorkspaceComponent,
    ActivityWorkspaceComponent_1.ActivityWorkspaceComponent,
    OpportunityWorkspaceComponent_1.OpportunityWorkspaceComponent,
    ContactWorkspaceComponent_1.ContactWorkspaceComponent,
    DashboardWorkspaceComponent_1.DashboardWorkspaceComponent,
    TicketHelperComponent_1.TicketHelperComponent,
    TicketShortTitleComponent_1.TicketShortTitleComponent,
    ActivityShortTitleComponent_1.ActivityShortTitleComponent,
    OpportunityShortTitleComponent_1.OpportunityShortTitleComponent,
    CloseAsWonOrLostComponent_1.CloseAsWonOrLostComponent,
    ReOpen_StageComponent_1.ReOpen_StageComponent,
    EditClosedOpportunityComponent_1.EditClosedOpportunityComponent,
    OpportunityHelperComponent_1.OpportunityHelperComponent,
    ActivityHelperComponent_1.ActivityHelperComponent,
    ByCreateDateComponent_1.ByCreateDateComponent,
    ByInProgressComponent_1.ByInProgressComponent,
    CompanyPerformanceComponent_1.CompanyPerformanceComponent,
    TicketsWorkspaceComponent_1.TicketsWorkspaceComponent,
    TicketDashboardComponent_1.TicketDashboardComponent,
    ByOpenedTicketComponent_1.ByOpenedTicketComponent,
    TicketClassificationMaintenanceComponent_1.TicketClassificationMaintenanceComponent,
    AddEditClassificationComponent_1.AddEditClassificationComponent,
    ByFirstResolveTicketComponent_1.ByFirstResolveTicketComponent,
    OccasionWorkspaceComponent_1.OccasionWorkspaceComponent,
];
exports.ControlsComponents = [
    ClassificationsTree_1.ClassificationsTree,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CRMWorkspaceComponent": {
                myResult = CRMWorkspaceComponent_1.CRMWorkspaceComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "TicketsComponent": {
                myResult = TicketsComponent_1.TicketsComponent;
                break;
            }
            case "OverviewWorkspaceComponent": {
                myResult = OverviewWorkspaceComponent_1.OverviewWorkspaceComponent;
                break;
            }
            case "CustomerWorkspaceComponent": {
                myResult = CustomerWorkspaceComponent_1.CustomerWorkspaceComponent;
                break;
            }
            case "ActivityWorkspaceComponent": {
                myResult = ActivityWorkspaceComponent_1.ActivityWorkspaceComponent;
                break;
            }
            case "OpportunityWorkspaceComponent": {
                myResult = OpportunityWorkspaceComponent_1.OpportunityWorkspaceComponent;
                break;
            }
            case "ContactWorkspaceComponent": {
                myResult = ContactWorkspaceComponent_1.ContactWorkspaceComponent;
                break;
            }
            case "DashboardWorkspaceComponent": {
                myResult = DashboardWorkspaceComponent_1.DashboardWorkspaceComponent;
                break;
            }
            case "ReOpen_StageComponent": {
                myResult = ReOpen_StageComponent_1.ReOpen_StageComponent;
                break;
            }
            case "EditClosedOpportunityComponent": {
                myResult = EditClosedOpportunityComponent_1.EditClosedOpportunityComponent;
                break;
            }
            case "CloseAsWonOrLostComponent": {
                myResult = CloseAsWonOrLostComponent_1.CloseAsWonOrLostComponent;
                break;
            }
            case "ByCreateDateComponent": {
                myResult = ByCreateDateComponent_1.ByCreateDateComponent;
                break;
            }
            case "TicketsWorkspaceComponent": {
                myResult = TicketsWorkspaceComponent_1.TicketsWorkspaceComponent;
                break;
            }
            case "TicketDashboardComponent": {
                myResult = TicketDashboardComponent_1.TicketDashboardComponent;
                break;
            }
            case "ByOpenedTicketComponent": {
                myResult = ByOpenedTicketComponent_1.ByOpenedTicketComponent;
                break;
            }
            case "TicketClassificationMaintenanceComponent": {
                myResult = TicketClassificationMaintenanceComponent_1.TicketClassificationMaintenanceComponent;
                break;
            }
            case "AddEditClassificationComponent": {
                myResult = AddEditClassificationComponent_1.AddEditClassificationComponent;
                break;
            }
            case "ByFirstResolveTicketComponent": {
                myResult = ByFirstResolveTicketComponent_1.ByFirstResolveTicketComponent;
                break;
            }
            case "ByInProgressComponent": {
                myResult = ByInProgressComponent_1.ByInProgressComponent;
                break;
            }
            case "CompanyPerformanceComponent": {
                myResult = CompanyPerformanceComponent_1.CompanyPerformanceComponent;
                break;
            }
            // Helpers
            case "TicketHelperComponent": {
                myResult = TicketHelperComponent_1.TicketHelperComponent;
                break;
            }
            case "OpportunityHelperComponent": {
                myResult = OpportunityHelperComponent_1.OpportunityHelperComponent;
                break;
            }
            case "ActivityHelperComponent": {
                myResult = ActivityHelperComponent_1.ActivityHelperComponent;
                break;
            }
            // Short Titles 
            case "TicketShortTitleComponent": {
                myResult = TicketShortTitleComponent_1.TicketShortTitleComponent;
                break;
            }
            case "ActivityShortTitleComponent": {
                myResult = ActivityShortTitleComponent_1.ActivityShortTitleComponent;
                break;
            }
            case "OpportunityShortTitleComponent": {
                myResult = OpportunityShortTitleComponent_1.OpportunityShortTitleComponent;
                break;
            }
            case "OccasionWorkspaceComponent": {
                myResult = OccasionWorkspaceComponent_1.OccasionWorkspaceComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map