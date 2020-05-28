"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GettingStartedComponent_1 = require("./Components/Workspaces/GettingStartedComponent");
var SystemDefaultsComponent_1 = require("./Components/SystemDefaults/SystemDefaultsComponent");
var CompanyAddressSettingsComponent_1 = require("./Components/CompanyAddress/CompanyAddressSettingsComponent");
var SystemCurrenciesComponent_1 = require("./Components/SystemCurrencies/SystemCurrenciesComponent");
var CurrencyRatesComponent_1 = require("./Components/SystemCurrencies/CurrencyRatesComponent");
var CountersComponent_1 = require("./Components/Counters/CountersComponent");
var CounterHAWBComponent_1 = require("./Components/Counters/EditComponents/CounterHAWBComponent");
var CounterTableComponent_1 = require("./Components/Counters/EditComponents/CounterTableComponent");
var CounterInvoiceComponent_1 = require("./Components/Counters/EditComponents/CounterInvoiceComponent");
var CounterAdvancedComponent_1 = require("./Components/Counters/EditComponents/CounterAdvancedComponent");
var AccountingSettingsComponent_1 = require("./Components/AccountingSettings/AccountingSettingsComponent");
var AccountingAdvancedSettingsComponent_1 = require("./Components/AccountingSettings/AccountingAdvancedSettingsComponent");
var LocalSettingsComponent_1 = require("./Components/LocalSettings/LocalSettingsComponent");
var InvoiceSettingsComponent_1 = require("./Components/InvoiceSettings/InvoiceSettingsComponent");
var AirlineSettingsComponent_1 = require("./Components/AirlineSettings/AirlineSettingsComponent");
var UploadLogoComponent_1 = require("./Components/UploadImage/UploadLogoComponent");
exports.Components = [
    GettingStartedComponent_1.GettingStartedComponent,
    SystemDefaultsComponent_1.SystemDefaultsComponent,
    CompanyAddressSettingsComponent_1.CompanyAddressSettingsComponent,
    SystemCurrenciesComponent_1.SystemCurrenciesComponent,
    CurrencyRatesComponent_1.CurrencyRatesComponent,
    CountersComponent_1.CountersComponent,
    CounterHAWBComponent_1.CounterHAWBComponent,
    CounterTableComponent_1.CounterTableComponent,
    CounterInvoiceComponent_1.CounterInvoiceComponent,
    CounterAdvancedComponent_1.CounterAdvancedComponent,
    AccountingSettingsComponent_1.AccountingSettingsComponent,
    AccountingAdvancedSettingsComponent_1.AccountingAdvancedSettingsComponent,
    LocalSettingsComponent_1.LocalSettingsComponent,
    InvoiceSettingsComponent_1.InvoiceSettingsComponent,
    AirlineSettingsComponent_1.AirlineSettingsComponent,
    UploadLogoComponent_1.UploadLogoComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "GettingStartedComponent": {
                myResult = GettingStartedComponent_1.GettingStartedComponent;
                break;
            }
            case "SystemDefaultsComponent": {
                myResult = SystemDefaultsComponent_1.SystemDefaultsComponent;
                break;
            }
            case "CompanyAddressSettingsComponent": {
                myResult = CompanyAddressSettingsComponent_1.CompanyAddressSettingsComponent;
                break;
            }
            case "SystemCurrenciesComponent": {
                myResult = SystemCurrenciesComponent_1.SystemCurrenciesComponent;
                break;
            }
            case "CurrencyRatesComponent": {
                myResult = CurrencyRatesComponent_1.CurrencyRatesComponent;
                break;
            }
            case "CountersComponent": {
                myResult = CountersComponent_1.CountersComponent;
                break;
            }
            case "CounterHAWBComponent": {
                myResult = CounterHAWBComponent_1.CounterHAWBComponent;
                break;
            }
            case "CounterTableComponent": {
                myResult = CounterTableComponent_1.CounterTableComponent;
                break;
            }
            case "CounterInvoiceComponent": {
                myResult = CounterInvoiceComponent_1.CounterInvoiceComponent;
                break;
            }
            case "CounterAdvancedComponent": {
                myResult = CounterAdvancedComponent_1.CounterAdvancedComponent;
                break;
            }
            case "AccountingSettingsComponent": {
                myResult = AccountingSettingsComponent_1.AccountingSettingsComponent;
                break;
            }
            case "AccountingAdvancedSettingsComponent": {
                myResult = AccountingAdvancedSettingsComponent_1.AccountingAdvancedSettingsComponent;
                break;
            }
            case "LocalSettingsComponent": {
                myResult = LocalSettingsComponent_1.LocalSettingsComponent;
                break;
            }
            case "InvoiceSettingsComponent": {
                myResult = InvoiceSettingsComponent_1.InvoiceSettingsComponent;
                break;
            }
            case "AirlineSettingsComponent": {
                myResult = AirlineSettingsComponent_1.AirlineSettingsComponent;
                break;
            }
            case "UploadLogoComponent": {
                myResult = UploadLogoComponent_1.UploadLogoComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map