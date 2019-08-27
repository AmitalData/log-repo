"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var StockNewWizardComponent_1 = require("./Components/MessagingStock/StockNewWizardComponent");
var StockGeneralTabComponent_1 = require("./Components/MessagingStock/StockGeneralTabComponent");
var StockWindowComponent_1 = require("./Components/MessagingStock/StockWindowComponent");
var StockHistoryComponent_1 = require("./Components/MessagingStock/StockHistoryComponent");
var FBLStockFieldComponent_1 = require("./Components/FBLStock/FBLStockFieldComponent");
var FBLStockMainComponent_1 = require("./Components/FBLStock/FBLStockMainComponent");
var NewFBLStockComponent_1 = require("./Components/FBLStock/NewFBLStockComponent");
var FBLStackSelectionComponent_1 = require("./Components/FBLStock/FBLStackSelectionComponent");
var AddEditAWBStockComponent_1 = require("./Components/Maintenance/AddEditAWBStockComponent");
var TenantManagementAWBStockTabComponent_1 = require("./Components/Maintenance/TenantManagementAWBStockTabComponent");
exports.Components = [
    StockNewWizardComponent_1.StockNewWizardComponent,
    StockGeneralTabComponent_1.StockGeneralTabComponent,
    StockWindowComponent_1.StockWindowComponent,
    StockHistoryComponent_1.StockHistoryComponent,
    FBLStockFieldComponent_1.FBLStockFieldComponent,
    FBLStockMainComponent_1.FBLStockMainComponent,
    NewFBLStockComponent_1.NewFBLStockComponent,
    FBLStackSelectionComponent_1.FBLStackSelectionComponent,
    AddEditAWBStockComponent_1.AddEditAWBStockComponent,
    TenantManagementAWBStockTabComponent_1.TenantManagementAWBStockTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "StockNewWizardComponent": {
                myResult = StockNewWizardComponent_1.StockNewWizardComponent;
                break;
            }
            case "StockGeneralTabComponent": {
                myResult = StockGeneralTabComponent_1.StockGeneralTabComponent;
                break;
            }
            case "StockWindowComponent": {
                myResult = StockWindowComponent_1.StockWindowComponent;
                break;
            }
            case "StockHistoryComponent": {
                myResult = StockHistoryComponent_1.StockHistoryComponent;
                break;
            }
            case "FBLStockFieldComponent": {
                myResult = FBLStockFieldComponent_1.FBLStockFieldComponent;
                break;
            }
            case "FBLStockMainComponent": {
                myResult = FBLStockMainComponent_1.FBLStockMainComponent;
                break;
            }
            case "NewFBLStockComponent": {
                myResult = NewFBLStockComponent_1.NewFBLStockComponent;
                break;
            }
            case "FBLStackSelectionComponent": {
                myResult = FBLStackSelectionComponent_1.FBLStackSelectionComponent;
                break;
            }
            case "AddEditAWBStockComponent": {
                myResult = AddEditAWBStockComponent_1.AddEditAWBStockComponent;
                break;
            }
            case "TenantManagementAWBStockTabComponent": {
                myResult = TenantManagementAWBStockTabComponent_1.TenantManagementAWBStockTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map