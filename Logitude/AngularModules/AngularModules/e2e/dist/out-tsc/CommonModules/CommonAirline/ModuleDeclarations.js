"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewAirlineComponent_1 = require("./Components/NewEntity/NewAirlineComponent");
var AirlineAdaptationsTabComponent_1 = require("./Components/EditTabs/AirlineAdaptationsTabComponent");
var AirlineAWBStockTabComponent_1 = require("./Components/EditTabs/AirlineAWBStockTabComponent");
var AirlineCCSSettingsTabComponent_1 = require("./Components/EditTabs/AirlineCCSSettingsTabComponent");
var AirlineSurchargeTabComponent_1 = require("./Components/EditTabs/AirlineSurchargeTabComponent");
var AddEditAirlineAdaptationItemComponent_1 = require("./Components/AddEdit/AddEditAirlineAdaptationItemComponent");
var AddEditAirlineMessagingRuleComponent_1 = require("./Components/AddEdit/AddEditAirlineMessagingRuleComponent");
var AddEditTarrifHeaderComponent_1 = require("./Components/AddEdit/AddEditTarrifHeaderComponent");
var AddEditTariffChargeComponent_1 = require("./Components/AddEdit/AddEditTariffChargeComponent");
var AirlineDocsInTabComponent_1 = require("./Components/EditTabs/AirlineDocsInTabComponent");
var AirlineGeneralTabComponent_1 = require("./Components/EditTabs/AirlineGeneralTabComponent");
var AreasTabComponent_1 = require("./Components/EditTabs/AreasTabComponent");
var AddEditAirlineAreaComponent_1 = require("./Components/AddEdit/AddEditAirlineAreaComponent");
var ChoosePortComponent_1 = require("./Components/AddEdit/ChoosePortComponent");
exports.Components = [
    NewAirlineComponent_1.NewAirlineComponent,
    AirlineAdaptationsTabComponent_1.AirlineAdaptationsTabComponent,
    AirlineAWBStockTabComponent_1.AirlineAWBStockTabComponent,
    AirlineCCSSettingsTabComponent_1.AirlineCCSSettingsTabComponent,
    AreasTabComponent_1.AreasTabComponent,
    AddEditAirlineAdaptationItemComponent_1.AddEditAirlineAdaptationItemComponent,
    AddEditAirlineMessagingRuleComponent_1.AddEditAirlineMessagingRuleComponent,
    AddEditTarrifHeaderComponent_1.AddEditTarrifHeaderComponent,
    AddEditTariffChargeComponent_1.AddEditTariffChargeComponent,
    AirlineDocsInTabComponent_1.AirlineDocsInTabComponent,
    AirlineGeneralTabComponent_1.AirlineGeneralTabComponent,
    AirlineSurchargeTabComponent_1.AirlineSurchargeTabComponent,
    AddEditAirlineAreaComponent_1.AddEditAirlineAreaComponent,
    ChoosePortComponent_1.ChoosePortComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewAirlineComponent": {
                myResult = NewAirlineComponent_1.NewAirlineComponent;
                break;
            }
            case "AirlineAdaptationsTabComponent": {
                myResult = AirlineAdaptationsTabComponent_1.AirlineAdaptationsTabComponent;
                break;
            }
            case "AirlineAWBStockTabComponent": {
                myResult = AirlineAWBStockTabComponent_1.AirlineAWBStockTabComponent;
                break;
            }
            case "AirlineCCSSettingsTabComponent": {
                myResult = AirlineCCSSettingsTabComponent_1.AirlineCCSSettingsTabComponent;
                break;
            }
            case "AreasTabComponent": {
                myResult = AreasTabComponent_1.AreasTabComponent;
                break;
            }
            case "AddEditAirlineAdaptationItemComponent": {
                myResult = AddEditAirlineAdaptationItemComponent_1.AddEditAirlineAdaptationItemComponent;
                break;
            }
            case "AddEditAirlineMessagingRuleComponent": {
                myResult = AddEditAirlineMessagingRuleComponent_1.AddEditAirlineMessagingRuleComponent;
                break;
            }
            case "AddEditTarrifHeaderComponent": {
                myResult = AddEditTarrifHeaderComponent_1.AddEditTarrifHeaderComponent;
                break;
            }
            case "AddEditTariffChargeComponent": {
                myResult = AddEditTariffChargeComponent_1.AddEditTariffChargeComponent;
                break;
            }
            case "AirlineDocsInTabComponent": {
                myResult = AirlineDocsInTabComponent_1.AirlineDocsInTabComponent;
                break;
            }
            case "AirlineGeneralTabComponent": {
                myResult = AirlineGeneralTabComponent_1.AirlineGeneralTabComponent;
                break;
            }
            case "AirlineSurchargeTabComponent": {
                myResult = AirlineSurchargeTabComponent_1.AirlineSurchargeTabComponent;
                break;
            }
            case "AddEditAirlineAreaComponent": {
                myResult = AddEditAirlineAreaComponent_1.AddEditAirlineAreaComponent;
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