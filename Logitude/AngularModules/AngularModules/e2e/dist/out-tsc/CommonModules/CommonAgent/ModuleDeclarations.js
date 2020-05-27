"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewAgentComponent_1 = require("./Components/NewEntity/NewAgentComponent");
var AgentGeneralTabComponent_1 = require("./Components/EditTabs/AgentGeneralTabComponent");
var AgentSharedLogisticsTabComponent_1 = require("./Components/EditTabs/AgentSharedLogisticsTabComponent");
var AgentShareInvitaionComponent_1 = require("./Components/EditTabs/AgentShareInvitaionComponent");
var AcceptAgentInvitaionComponent_1 = require("./Components/EditTabs/AcceptAgentInvitaionComponent");
var AgentDocsInTabComponent_1 = require("./Components/EditTabs/AgentDocsInTabComponent");
var AgentDocsOutTabComponent_1 = require("./Components/EditTabs/AgentDocsOutTabComponent");
var AgentBillingTabComponent_1 = require("./Components/EditTabs/AgentBillingTabComponent");
exports.Components = [
    NewAgentComponent_1.NewAgentComponent,
    AgentGeneralTabComponent_1.AgentGeneralTabComponent,
    AgentSharedLogisticsTabComponent_1.AgentSharedLogisticsTabComponent,
    AgentShareInvitaionComponent_1.AgentShareInvitaionComponent,
    AcceptAgentInvitaionComponent_1.AcceptAgentInvitaionComponent,
    AgentBillingTabComponent_1.AgentBillingTabComponent,
    AgentDocsInTabComponent_1.AgentDocsInTabComponent,
    AgentDocsOutTabComponent_1.AgentDocsOutTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewAgentComponent": {
                myResult = NewAgentComponent_1.NewAgentComponent;
                break;
            }
            case "AgentGeneralTabComponent": {
                myResult = AgentGeneralTabComponent_1.AgentGeneralTabComponent;
                break;
            }
            case "AgentSharedLogisticsTabComponent": {
                myResult = AgentSharedLogisticsTabComponent_1.AgentSharedLogisticsTabComponent;
                break;
            }
            case "AgentShareInvitaionComponent": {
                myResult = AgentShareInvitaionComponent_1.AgentShareInvitaionComponent;
                break;
            }
            case "AcceptAgentInvitaionComponent": {
                myResult = AcceptAgentInvitaionComponent_1.AcceptAgentInvitaionComponent;
                break;
            }
            case "AgentBillingTabComponent": {
                myResult = AgentBillingTabComponent_1.AgentBillingTabComponent;
                break;
            }
            case "AgentDocsInTabComponent": {
                myResult = AgentDocsInTabComponent_1.AgentDocsInTabComponent;
                break;
            }
            case "AgentDocsOutTabComponent": {
                myResult = AgentDocsOutTabComponent_1.AgentDocsOutTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map