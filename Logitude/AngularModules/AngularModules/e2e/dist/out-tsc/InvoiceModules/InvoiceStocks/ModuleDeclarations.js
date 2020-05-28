"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ManageStocksComponent_1 = require("./Components/ManageStocksComponent");
var NewARInvoiceStockComponent_1 = require("./Components/NewEntity/NewARInvoiceStockComponent");
var ARInvoiceStockInputTemplate_1 = require("./Components/ARInvoiceStockInputTemplate");
var ARInvoiceStockGeneralTabComponent_1 = require("./Components/EditTabs/ARInvoiceStockGeneralTabComponent");
var NewARInvoiceStockLinesComponent_1 = require("./Components/NewEntity/NewARInvoiceStockLinesComponent");
var ARInvoiceStockSelectionComponent_1 = require("./Components/StockSelection/ARInvoiceStockSelectionComponent");
exports.Components = [
    ManageStocksComponent_1.ManageStocksComponent,
    NewARInvoiceStockComponent_1.NewARInvoiceStockComponent,
    ARInvoiceStockInputTemplate_1.ARInvoiceStockInputTemplate,
    ARInvoiceStockGeneralTabComponent_1.ARInvoiceStockGeneralTabComponent,
    NewARInvoiceStockLinesComponent_1.NewARInvoiceStockLinesComponent,
    ARInvoiceStockSelectionComponent_1.ARInvoiceStockSelectionComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "ManageStocksComponent": {
                myResult = ManageStocksComponent_1.ManageStocksComponent;
                break;
            }
            case "NewARInvoiceStockComponent": {
                myResult = NewARInvoiceStockComponent_1.NewARInvoiceStockComponent;
                break;
            }
            case "ARInvoiceStockInputTemplate": {
                myResult = ARInvoiceStockInputTemplate_1.ARInvoiceStockInputTemplate;
                break;
            }
            case "ARInvoiceStockGeneralTabComponent": {
                myResult = ARInvoiceStockGeneralTabComponent_1.ARInvoiceStockGeneralTabComponent;
                break;
            }
            case "NewARInvoiceStockLinesComponent": {
                myResult = NewARInvoiceStockLinesComponent_1.NewARInvoiceStockLinesComponent;
                break;
            }
            case "ARInvoiceStockSelectionComponent": {
                myResult = ARInvoiceStockSelectionComponent_1.ARInvoiceStockSelectionComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map