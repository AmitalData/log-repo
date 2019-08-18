"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewTicketComponent_1 = require("./Components/NewEntity/NewTicketComponent");
var ChooseShipmentComponent_1 = require("./Components/NewEntity/ChooseShipmentComponent");
var TicketDetailsTabComponent_1 = require("./Components/EditTabs/Details/TicketDetailsTabComponent");
var TicketMainTabComponent_1 = require("./Components/EditTabs/MainTab/TicketMainTabComponent");
var DetailsTabComponent_1 = require("./Components/EditTabs/MainTab/DetailsTabComponent");
var ActivitiesTabComponent_1 = require("./Components/EditTabs/MainTab/ActivitiesTabComponent");
var SendEmailComponent_1 = require("./Components/EditTabs/MainTab/SendEmailComponent");
var PostsTabComponent_1 = require("./Components/EditTabs/MainTab/PostsTabComponent");
var TicketAuditTabComponent_1 = require("./Components/EditTabs/Audit/TicketAuditTabComponent");
var TicketDocsInTabComponent_1 = require("./Components/EditTabs/DocsIn/TicketDocsInTabComponent");
var TicketDocsOutTabComponent_1 = require("./Components/EditTabs/DocsOut/TicketDocsOutTabComponent");
var TicketEscalationTabComponent_1 = require("./Components/EditTabs/Escalation/TicketEscalationTabComponent");
var TicketOverviewTabComponent_1 = require("./Components/EditTabs/Overview/TicketOverviewTabComponent");
var TicketClosureComponent_1 = require("./Components/EditTabs/Others/TicketClosureComponent");
exports.Components = [
    NewTicketComponent_1.NewTicketComponent,
    ChooseShipmentComponent_1.ChooseShipmentComponent,
    TicketDetailsTabComponent_1.TicketDetailsTabComponent,
    TicketMainTabComponent_1.TicketMainTabComponent,
    DetailsTabComponent_1.DetailsTabComponent,
    ActivitiesTabComponent_1.ActivitiesTabComponent,
    SendEmailComponent_1.SendEmailComponent,
    PostsTabComponent_1.PostsTabComponent,
    TicketAuditTabComponent_1.TicketAuditTabComponent,
    TicketDocsInTabComponent_1.TicketDocsInTabComponent,
    TicketDocsOutTabComponent_1.TicketDocsOutTabComponent,
    TicketEscalationTabComponent_1.TicketEscalationTabComponent,
    TicketOverviewTabComponent_1.TicketOverviewTabComponent,
    TicketClosureComponent_1.TicketClosureComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewTicketComponent": {
                myResult = NewTicketComponent_1.NewTicketComponent;
                break;
            }
            case "ChooseShipmentComponent": {
                myResult = ChooseShipmentComponent_1.ChooseShipmentComponent;
                break;
            }
            case "TicketDetailsTabComponent": {
                myResult = TicketDetailsTabComponent_1.TicketDetailsTabComponent;
                break;
            }
            case "TicketMainTabComponent": {
                myResult = TicketMainTabComponent_1.TicketMainTabComponent;
                break;
            }
            case "DetailsTabComponent": {
                myResult = DetailsTabComponent_1.DetailsTabComponent;
                break;
            }
            case "ActivitiesTabComponent": {
                myResult = ActivitiesTabComponent_1.ActivitiesTabComponent;
                break;
            }
            case "SendEmailComponent": {
                myResult = SendEmailComponent_1.SendEmailComponent;
                break;
            }
            case "PostsTabComponent": {
                myResult = PostsTabComponent_1.PostsTabComponent;
                break;
            }
            case "TicketAuditTabComponent": {
                myResult = TicketAuditTabComponent_1.TicketAuditTabComponent;
                break;
            }
            case "TicketDocsInTabComponent": {
                myResult = TicketDocsInTabComponent_1.TicketDocsInTabComponent;
                break;
            }
            case "TicketDocsOutTabComponent": {
                myResult = TicketDocsOutTabComponent_1.TicketDocsOutTabComponent;
                break;
            }
            case "TicketEscalationTabComponent": {
                myResult = TicketEscalationTabComponent_1.TicketEscalationTabComponent;
                break;
            }
            case "TicketOverviewTabComponent": {
                myResult = TicketOverviewTabComponent_1.TicketOverviewTabComponent;
                break;
            }
            case "TicketClosureComponent": {
                myResult = TicketClosureComponent_1.TicketClosureComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map