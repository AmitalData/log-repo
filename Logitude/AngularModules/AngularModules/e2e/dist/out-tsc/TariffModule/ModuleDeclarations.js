"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TariffModuleWorkspaceComponent_1 = require("./Components/Workspaces/TariffModuleWorkspaceComponent");
var TariffSettingComponent_1 = require("./Components/Workspaces/TariffSettingComponent");
var NewAirFreightCostComponent_1 = require("./Components/NewEntity/NewAirFreightCostComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var TariffSearchAirFreightPricesComponent_1 = require("./Components/Workspaces/TariffSearchAirFreightPricesComponent");
// Tabs
var TariffDetailsTabComponent_1 = require("./Components/EditTabs/Tariff/TariffDetailsTabComponent");
var VersionTabComponent_1 = require("./Components/EditTabs/Tariff/VersionTabComponent");
var TariffGeneralTabComponent_1 = require("./Components/EditTabs/Tariff/TariffGeneralTabComponent");
var VersionHistoryTabComponent_1 = require("./Components/EditTabs/Tariff/VersionHistoryTabComponent");
var AddEditTariffLineComponent_1 = require("./Components/EditTabs/Tariff/AddEditTariffLineComponent");
var TariffTabsContentComponent_1 = require("./Components/EditTabs/Tariff/TariffTabsContentComponent");
var SurchargeVersionTabComponent_1 = require("./Components/EditTabs/Tariff/SurchargeVersionTabComponent");
var TariffDatesValidationComponent_1 = require("./Components/EditTabs/Tariff/TariffDatesValidationComponent");
var UpdateSurchargesComponent_1 = require("./Components/EditTabs/Tariff/UpdateSurchargesComponent");
var ChoosePortComponent_1 = require("./Components/EditTabs/Tariff/ChoosePortComponent");
exports.Components = [
    TariffModuleWorkspaceComponent_1.TariffModuleWorkspaceComponent,
    TariffSettingComponent_1.TariffSettingComponent,
    NewAirFreightCostComponent_1.NewAirFreightCostComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    TariffDetailsTabComponent_1.TariffDetailsTabComponent,
    VersionTabComponent_1.VersionTabComponent,
    VersionHistoryTabComponent_1.VersionHistoryTabComponent,
    TariffGeneralTabComponent_1.TariffGeneralTabComponent,
    AddEditTariffLineComponent_1.AddEditTariffLineComponent,
    TariffTabsContentComponent_1.TariffTabsContentComponent,
    SurchargeVersionTabComponent_1.SurchargeVersionTabComponent,
    TariffSearchAirFreightPricesComponent_1.TariffSearchAirFreightPricesComponent,
    TariffDatesValidationComponent_1.TariffDatesValidationComponent,
    UpdateSurchargesComponent_1.UpdateSurchargesComponent,
    ChoosePortComponent_1.ChoosePortComponent,
];
exports.ControlsComponents = [];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "TariffModuleWorkspaceComponent": {
                myResult = TariffModuleWorkspaceComponent_1.TariffModuleWorkspaceComponent;
                break;
            }
            case "TariffSettingComponent": {
                myResult = TariffSettingComponent_1.TariffSettingComponent;
                break;
            }
            case "NewAirFreightCostComponent": {
                myResult = NewAirFreightCostComponent_1.NewAirFreightCostComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "TariffDetailsTabComponent": {
                myResult = TariffDetailsTabComponent_1.TariffDetailsTabComponent;
                break;
            }
            case "VersionTabComponent": {
                myResult = VersionTabComponent_1.VersionTabComponent;
                break;
            }
            case "VersionHistoryTabComponent": {
                myResult = VersionHistoryTabComponent_1.VersionHistoryTabComponent;
                break;
            }
            case "TariffGeneralTabComponent": {
                myResult = TariffGeneralTabComponent_1.TariffGeneralTabComponent;
                break;
            }
            case "AddEditTariffLineComponent": {
                myResult = AddEditTariffLineComponent_1.AddEditTariffLineComponent;
                break;
            }
            case "TariffTabsContentComponent": {
                myResult = TariffTabsContentComponent_1.TariffTabsContentComponent;
                break;
            }
            case "SurchargeVersionTabComponent": {
                myResult = SurchargeVersionTabComponent_1.SurchargeVersionTabComponent;
                break;
            }
            case "TariffSearchAirFreightPricesComponent": {
                myResult = TariffSearchAirFreightPricesComponent_1.TariffSearchAirFreightPricesComponent;
                break;
            }
            case "TariffDatesValidationComponent": {
                myResult = TariffDatesValidationComponent_1.TariffDatesValidationComponent;
                break;
            }
            case "UpdateSurchargesComponent": {
                myResult = UpdateSurchargesComponent_1.UpdateSurchargesComponent;
                break;
            }
            case "ChoosePortComponent": {
                myResult = ChoosePortComponent_1.ChoosePortComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map