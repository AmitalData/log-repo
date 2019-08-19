"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewOpportunityComponent_1 = require("./Components/NewEntity/NewOpportunityComponent");
var OpportunityOverviewTabComponent_1 = require("./Components/EditTabs/OpportunityOverviewTabComponent");
var QuotesWindowComponent_1 = require("./Components/EditTabs/QuotesWindowComponent");
var OpportunityDocsOutTabComponent_1 = require("./Components/EditTabs/OpportunityDocsOutTabComponent");
var OpportunityDocsInTabComponent_1 = require("./Components/EditTabs/OpportunityDocsInTabComponent");
var OpportunityProductsTabComponent_1 = require("./Components/EditTabs/OpportunityProductsTabComponent");
var EditProductComponent_1 = require("./Components/EditTabs/EditProductComponent");
var ProductHistoryDetailsComponent_1 = require("./Components/EditTabs/ProductHistoryDetailsComponent");
var OpportunityGeneralTabComponent_1 = require("./Components/EditTabs/OpportunityGeneralTabComponent");
exports.Components = [
    NewOpportunityComponent_1.NewOpportunityComponent,
    OpportunityGeneralTabComponent_1.OpportunityGeneralTabComponent,
    OpportunityOverviewTabComponent_1.OpportunityOverviewTabComponent,
    QuotesWindowComponent_1.QuotesWindowComponent,
    OpportunityDocsOutTabComponent_1.OpportunityDocsOutTabComponent,
    OpportunityDocsInTabComponent_1.OpportunityDocsInTabComponent,
    OpportunityProductsTabComponent_1.OpportunityProductsTabComponent,
    EditProductComponent_1.EditProductComponent,
    ProductHistoryDetailsComponent_1.ProductHistoryDetailsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewOpportunityComponent": {
                myResult = NewOpportunityComponent_1.NewOpportunityComponent;
                break;
            }
            case "OpportunityGeneralTabComponent": {
                myResult = OpportunityGeneralTabComponent_1.OpportunityGeneralTabComponent;
                break;
            }
            case "OpportunityOverviewTabComponent": {
                myResult = OpportunityOverviewTabComponent_1.OpportunityOverviewTabComponent;
                break;
            }
            case "QuotesWindowComponent": {
                myResult = QuotesWindowComponent_1.QuotesWindowComponent;
                break;
            }
            case "OpportunityDocsOutTabComponent": {
                myResult = OpportunityDocsOutTabComponent_1.OpportunityDocsOutTabComponent;
                break;
            }
            case "OpportunityDocsInTabComponent": {
                myResult = OpportunityDocsInTabComponent_1.OpportunityDocsInTabComponent;
                break;
            }
            case "OpportunityProductsTabComponent": {
                myResult = OpportunityProductsTabComponent_1.OpportunityProductsTabComponent;
                break;
            }
            case "EditProductComponent": {
                myResult = EditProductComponent_1.EditProductComponent;
                break;
            }
            case "ProductHistoryDetailsComponent": {
                myResult = ProductHistoryDetailsComponent_1.ProductHistoryDetailsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map