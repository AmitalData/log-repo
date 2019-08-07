"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FilingInboxWorkspaceComponent_1 = require("./Components/FilingInboxWorkspaceComponent");
var ChooseEntityComponent_1 = require("./Components/ChooseEntityComponent");
var ForwarderChooseShipmentsComponent_1 = require("./Components/ForwarderChooseShipmentsComponent");
exports.Components = [
    FilingInboxWorkspaceComponent_1.FilingInboxWorkspaceComponent,
    ChooseEntityComponent_1.ChooseEntityComponent,
    ForwarderChooseShipmentsComponent_1.ForwarderChooseShipmentsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "FilingInboxWorkspaceComponent": {
                myResult = FilingInboxWorkspaceComponent_1.FilingInboxWorkspaceComponent;
                break;
            }
            case "ChooseEntityComponent": {
                myResult = ChooseEntityComponent_1.ChooseEntityComponent;
                break;
            }
            case "ForwarderChooseShipmentsComponent": {
                myResult = ForwarderChooseShipmentsComponent_1.ForwarderChooseShipmentsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map