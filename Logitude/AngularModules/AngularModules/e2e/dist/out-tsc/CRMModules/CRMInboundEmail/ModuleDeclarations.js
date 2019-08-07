"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var InboundEmailGeneralTabComponent_1 = require("./Components/EditTabs/InboundEmailGeneralTabComponent");
var ViewInboundLineBodyComponent_1 = require("./Components/EditTabs/ViewInboundLineBodyComponent");
var NewInboundEmailComponent_1 = require("./Components/NewEntity/NewInboundEmailComponent");
exports.Components = [
    InboundEmailGeneralTabComponent_1.InboundEmailGeneralTabComponent,
    ViewInboundLineBodyComponent_1.ViewInboundLineBodyComponent,
    NewInboundEmailComponent_1.NewInboundEmailComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "InboundEmailGeneralTabComponent": {
                myResult = InboundEmailGeneralTabComponent_1.InboundEmailGeneralTabComponent;
                break;
            }
            case "ViewInboundLineBodyComponent": {
                myResult = ViewInboundLineBodyComponent_1.ViewInboundLineBodyComponent;
                break;
            }
            case "NewInboundEmailComponent": {
                myResult = NewInboundEmailComponent_1.NewInboundEmailComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map