"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConnectionsTabComponent_1 = require("./Components/Connections/ConnectionsTabComponent");
var QuoteDocsInTabComponent_1 = require("./Components/DocsIn/QuoteDocsInTabComponent");
var QuoteDocsOutTabComponent_1 = require("./Components/DocsOut/QuoteDocsOutTabComponent");
var OrdersTabComponent_1 = require("./Components/Orders/OrdersTabComponent");
var OverviewTabComponent_1 = require("./Components/Overview/OverviewTabComponent");
var AddEditPackageComponent_1 = require("./Components/Packages/AddEditPackageComponent");
var PackagesTabComponent_1 = require("./Components/Packages/PackagesTabComponent");
var AddEditPartnerComponent_1 = require("./Components/Partners/AddEditPartnerComponent");
var PartnersTabComponent_1 = require("./Components/Partners/PartnersTabComponent");
var InlandDomesticRoutingsComponent_1 = require("./Components/Routings/InlandDomesticRoutingsComponent");
var OrdinaryRoutingsComponent_1 = require("./Components/Routings/OrdinaryRoutingsComponent");
var RoutingsAddEditAddressComponent_1 = require("./Components/Routings/RoutingsAddEditAddressComponent");
var RoutingsTabComponent_1 = require("./Components/Routings/RoutingsTabComponent");
var TariffsComponent_1 = require("./Components/Tariffs/TariffsComponent");
exports.Components = [
    ConnectionsTabComponent_1.ConnectionsTabComponent,
    QuoteDocsInTabComponent_1.QuoteDocsInTabComponent,
    QuoteDocsOutTabComponent_1.QuoteDocsOutTabComponent,
    OrdersTabComponent_1.OrdersTabComponent,
    OverviewTabComponent_1.OverviewTabComponent,
    AddEditPackageComponent_1.AddEditPackageComponent,
    PackagesTabComponent_1.PackagesTabComponent,
    AddEditPartnerComponent_1.AddEditPartnerComponent,
    PartnersTabComponent_1.PartnersTabComponent,
    InlandDomesticRoutingsComponent_1.InlandDomesticRoutingsComponent,
    OrdinaryRoutingsComponent_1.OrdinaryRoutingsComponent,
    RoutingsAddEditAddressComponent_1.RoutingsAddEditAddressComponent,
    RoutingsTabComponent_1.RoutingsTabComponent,
    TariffsComponent_1.TariffsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "ConnectionsTabComponent": {
                myResult = ConnectionsTabComponent_1.ConnectionsTabComponent;
                break;
            }
            case "QuoteDocsInTabComponent": {
                myResult = QuoteDocsInTabComponent_1.QuoteDocsInTabComponent;
                break;
            }
            case "QuoteDocsOutTabComponent": {
                myResult = QuoteDocsOutTabComponent_1.QuoteDocsOutTabComponent;
                break;
            }
            case "OrdersTabComponent": {
                myResult = OrdersTabComponent_1.OrdersTabComponent;
                break;
            }
            case "OverviewTabComponent": {
                myResult = OverviewTabComponent_1.OverviewTabComponent;
                break;
            }
            case "AddEditPackageComponent": {
                myResult = AddEditPackageComponent_1.AddEditPackageComponent;
                break;
            }
            case "PackagesTabComponent": {
                myResult = PackagesTabComponent_1.PackagesTabComponent;
                break;
            }
            case "AddEditPartnerComponent": {
                myResult = AddEditPartnerComponent_1.AddEditPartnerComponent;
                break;
            }
            case "PartnersTabComponent": {
                myResult = PartnersTabComponent_1.PartnersTabComponent;
                break;
            }
            case "InlandDomesticRoutingsComponent": {
                myResult = InlandDomesticRoutingsComponent_1.InlandDomesticRoutingsComponent;
                break;
            }
            case "OrdinaryRoutingsComponent": {
                myResult = OrdinaryRoutingsComponent_1.OrdinaryRoutingsComponent;
                break;
            }
            case "RoutingsAddEditAddressComponent": {
                myResult = RoutingsAddEditAddressComponent_1.RoutingsAddEditAddressComponent;
                break;
            }
            case "RoutingsTabComponent": {
                myResult = RoutingsTabComponent_1.RoutingsTabComponent;
                break;
            }
            case "TariffsComponent": {
                myResult = TariffsComponent_1.TariffsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map