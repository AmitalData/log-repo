"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewStackComponent_1 = require("./Components/Partners/AWBStock/NewStackComponent");
var NewCurrencyComponent_1 = require("./Components/Maintenance/Currency/NewCurrencyComponent");
var NewChargesTypeComponent_1 = require("./Components/Maintenance/ChargesType/NewChargesTypeComponent");
var NewVatTypePercentageComponent_1 = require("./Components/Maintenance/VatType/NewVatTypePercentageComponent");
var CustomerHelperComponent_1 = require("./Components/Helpers/CustomerHelperComponent");
var CustomerShortTitleComponent_1 = require("./Components/ShortTitles/CustomerShortTitleComponent");
var AssignComponent_1 = require("./Components/Partners/AWBStock/AssignComponent");
var StackSelectionComponent_1 = require("./Components/Partners/AWBStock/StackSelectionComponent");
var AssignToShipperComponent_1 = require("./Components/Partners/AWBStock/AssignToShipperComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var EventsTabComponent_1 = require("./Components/Events/EventsTabComponent");
var AddEditEventComponent_1 = require("./Components/Events/AddEditEventComponent");
var RatesHistoryComponent_1 = require("./Components/Maintenance/RatesHistoryComponent");
var RatesMainTabComponent_1 = require("./Components/Maintenance/RatesMainTabComponent");
var EditLastRateComponent_1 = require("./Components/Maintenance/EditLastRateComponent");
var LogBoxSettings_1 = require("../Common/Components/Maintenance/LogBoxSettings");
var TenantImportComponent_1 = require("./Components/Maintenance/TenantImportComponent");
var MeasurementGeneralComponent_1 = require("./Components/Maintenance/Measurement/MeasurementGeneralComponent");
var ChargesTypeGeneralTabComponent_1 = require("./Components/Maintenance/ChargesType/ChargesTypeGeneralTabComponent");
var CreditLimitSettingsComponent_1 = require("./Components/Maintenance/CreditLimit/CreditLimitSettingsComponent");
var CustomerActivationSettingsComponent_1 = require("../Common/Components/Maintenance/CustomerActivationSettingsComponent");
var VATSettingsComponent_1 = require("../Common/Components/Maintenance/VATSettingsComponent");
var CompetitorComponent_1 = require("../Common/Components/Maintenance/CompetitorComponent");
var NewBusinessUnitComponent_1 = require("./Components/Maintenance/BusinessUnit/NewBusinessUnitComponent");
var BusinessUnitGeneralTabComponent_1 = require("./Components/Maintenance/BusinessUnit/BusinessUnitGeneralTabComponent");
var AccountingTabComponent_1 = require("./Components/AccountingTab/AccountingTabComponent");
var AccountingTab_ChargesType_1 = require("./Components/AccountingTab/AccountingTab_ChargesType");
var AccountingTab_Currency_1 = require("./Components/AccountingTab/AccountingTab_Currency");
var AccountingTab_Full_1 = require("./Components/AccountingTab/AccountingTab_Full");
var AccountingTab_Partners_1 = require("./Components/AccountingTab/AccountingTab_Partners");
var AccountingTab_PaymentTerm_1 = require("./Components/AccountingTab/AccountingTab_PaymentTerm");
var AccountingTab_QuickBooksOnline_1 = require("./Components/AccountingTab/AccountingTab_QuickBooksOnline");
var AccountingTab_VatType_1 = require("./Components/AccountingTab/AccountingTab_VatType");
var ExternalAccountsByProductsComponent_1 = require("./Components/Partners/AddEdit/ExternalAccountsByProductsComponent");
var CustomsInterfaceSettingsComponent_1 = require("./Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent");
var CustomsInterfaceCredintialsComponent_1 = require("./Components/Maintenance/CustomsInterface/CustomsInterfaceCredintialsComponent");
var FTPDetailComponent_1 = require("./Components/Maintenance/CustomsInterface/FTPDetailComponent");
var ArtemusSettingsComponent_1 = require("./Components/Maintenance/CustomsInterface/ArtemusSettingsComponent");
var VatTypeGeneralTabComponent_1 = require("./Components/Maintenance/VatType/VatTypeGeneralTabComponent");
var VatTypePercentagesTabComponent_1 = require("./Components/Maintenance/VatType/VatTypePercentagesTabComponent");
var AccountingTab_Branch_1 = require("./Components/AccountingTab/AccountingTab_Branch");
var AddEditCustomerFieldsUpdateSettingComponent_1 = require("./Components/Maintenance/CustomerFieldsUpdateSetting/AddEditCustomerFieldsUpdateSettingComponent");
var BranchGeneralTabComponent_1 = require("./Components/Maintenance/Branch/BranchGeneralTabComponent");
var AddEditBranchAddressComponent_1 = require("./Components/Maintenance/Branch/AddEditBranchAddressComponent");
var QuoteStageGeneralTabComponent_1 = require("./Components/Maintenance/QuoteStage/QuoteStageGeneralTabComponent");
var AddEditCustomerSalesNoteComponent_1 = require("./Components/Helpers/AddEditCustomerSalesNoteComponent");
var CustomerTenantAccessFiltersMenuComponent_1 = require("./Components/FiltersMenu/CustomerTenantAccessFiltersMenuComponent");
var DocumentsFilingShortTitleComponent_1 = require("./Components/ShortTitles/DocumentsFilingShortTitleComponent");
var ChargesExternalByProductsComponent_1 = require("../Common/Components/AccountingTab/Advanced/ChargesExternalByProductsComponent");
var DocumentFilingEmailSettingsComponent_1 = require("./Components/Maintenance/DocumentFilingEmailSettings/DocumentFilingEmailSettingsComponent");
var CountryCityGeneralTabComponent_1 = require("./Components/Maintenance/CountryCity/CountryCityGeneralTabComponent");
var VesselGeneralTabComponent_1 = require("./Components/Maintenance/Vessel/VesselGeneralTabComponent");
var CustomsShipperFiltersMenuComponent_1 = require("./Components/FiltersMenu/CustomsShipperFiltersMenuComponent");
exports.Components = [
    CustomerHelperComponent_1.CustomerHelperComponent,
    CustomerShortTitleComponent_1.CustomerShortTitleComponent,
    AssignComponent_1.AssignComponent,
    NewStackComponent_1.NewStackComponent,
    StackSelectionComponent_1.StackSelectionComponent,
    AssignToShipperComponent_1.AssignToShipperComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    EventsTabComponent_1.EventsTabComponent,
    AddEditEventComponent_1.AddEditEventComponent,
    RatesHistoryComponent_1.RatesHistoryComponent,
    RatesMainTabComponent_1.RatesMainTabComponent,
    EditLastRateComponent_1.EditLastRateComponent,
    LogBoxSettings_1.LogBoxSettings,
    TenantImportComponent_1.TenantImportComponent,
    MeasurementGeneralComponent_1.MeasurementGeneralComponent,
    NewCurrencyComponent_1.NewCurrencyComponent,
    NewChargesTypeComponent_1.NewChargesTypeComponent,
    ChargesTypeGeneralTabComponent_1.ChargesTypeGeneralTabComponent,
    NewVatTypePercentageComponent_1.NewVatTypePercentageComponent,
    CreditLimitSettingsComponent_1.CreditLimitSettingsComponent,
    CompetitorComponent_1.CompetitorComponent,
    NewBusinessUnitComponent_1.NewBusinessUnitComponent,
    BusinessUnitGeneralTabComponent_1.BusinessUnitGeneralTabComponent,
    CustomerActivationSettingsComponent_1.CustomerActivationSettingsComponent,
    VATSettingsComponent_1.VATSettingsComponent,
    AccountingTabComponent_1.AccountingTabComponent,
    AccountingTab_ChargesType_1.AccountingTab_ChargesType,
    AccountingTab_Currency_1.AccountingTab_Currency,
    AccountingTab_Full_1.AccountingTab_Full,
    AccountingTab_Partners_1.AccountingTab_Partners,
    AccountingTab_PaymentTerm_1.AccountingTab_PaymentTerm,
    AccountingTab_QuickBooksOnline_1.AccountingTab_QuickBooksOnline,
    AccountingTab_VatType_1.AccountingTab_VatType,
    ExternalAccountsByProductsComponent_1.ExternalAccountsByProductsComponent,
    CustomsInterfaceSettingsComponent_1.CustomsInterfaceSettingsComponent,
    CustomsInterfaceCredintialsComponent_1.CustomsInterfaceCredintialsComponent,
    FTPDetailComponent_1.FTPDetailComponent,
    ArtemusSettingsComponent_1.ArtemusSettingsComponent,
    VatTypeGeneralTabComponent_1.VatTypeGeneralTabComponent,
    VatTypePercentagesTabComponent_1.VatTypePercentagesTabComponent,
    AccountingTab_Branch_1.AccountingTab_Branch,
    AddEditCustomerFieldsUpdateSettingComponent_1.AddEditCustomerFieldsUpdateSettingComponent,
    BranchGeneralTabComponent_1.BranchGeneralTabComponent,
    VesselGeneralTabComponent_1.VesselGeneralTabComponent,
    AddEditBranchAddressComponent_1.AddEditBranchAddressComponent,
    QuoteStageGeneralTabComponent_1.QuoteStageGeneralTabComponent,
    AddEditCustomerSalesNoteComponent_1.AddEditCustomerSalesNoteComponent,
    CustomerTenantAccessFiltersMenuComponent_1.CustomerTenantAccessFiltersMenuComponent,
    DocumentsFilingShortTitleComponent_1.DocumentsFilingShortTitleComponent,
    ChargesExternalByProductsComponent_1.ChargesExternalByProductsComponent,
    CountryCityGeneralTabComponent_1.CountryCityGeneralTabComponent,
    DocumentFilingEmailSettingsComponent_1.DocumentFilingEmailSettingsComponent,
    CustomsShipperFiltersMenuComponent_1.CustomsShipperFiltersMenuComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomerHelperComponent": {
                myResult = CustomerHelperComponent_1.CustomerHelperComponent;
                break;
            }
            case "CustomerShortTitleComponent": {
                myResult = CustomerShortTitleComponent_1.CustomerShortTitleComponent;
                break;
            }
            case "AssignComponent": {
                myResult = AssignComponent_1.AssignComponent;
                break;
            }
            case "NewStackComponent": {
                myResult = NewStackComponent_1.NewStackComponent;
                break;
            }
            case "StackSelectionComponent": {
                myResult = StackSelectionComponent_1.StackSelectionComponent;
                break;
            }
            case "AssignToShipperComponent": {
                myResult = AssignToShipperComponent_1.AssignToShipperComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "EventsTabComponent": {
                myResult = EventsTabComponent_1.EventsTabComponent;
                break;
            }
            case "AddEditEventComponent": {
                myResult = AddEditEventComponent_1.AddEditEventComponent;
                break;
            }
            case "RatesHistoryComponent": {
                myResult = RatesHistoryComponent_1.RatesHistoryComponent;
                break;
            }
            case "RatesMainTabComponent": {
                myResult = RatesMainTabComponent_1.RatesMainTabComponent;
                break;
            }
            case "EditLastRateComponent": {
                myResult = EditLastRateComponent_1.EditLastRateComponent;
                break;
            }
            case "LogBoxSettings": {
                myResult = LogBoxSettings_1.LogBoxSettings;
                break;
            }
            case "TenantImportComponent": {
                myResult = TenantImportComponent_1.TenantImportComponent;
                break;
            }
            case "MeasurementGeneralComponent": {
                myResult = MeasurementGeneralComponent_1.MeasurementGeneralComponent;
                break;
            }
            case "NewCurrencyComponent": {
                myResult = NewCurrencyComponent_1.NewCurrencyComponent;
                break;
            }
            case "NewChargesTypeComponent": {
                myResult = NewChargesTypeComponent_1.NewChargesTypeComponent;
                break;
            }
            case "ChargesTypeGeneralTabComponent": {
                myResult = ChargesTypeGeneralTabComponent_1.ChargesTypeGeneralTabComponent;
                break;
            }
            case "NewVatTypePercentageComponent": {
                myResult = NewVatTypePercentageComponent_1.NewVatTypePercentageComponent;
                break;
            }
            case "CreditLimitSettingsComponent": {
                myResult = CreditLimitSettingsComponent_1.CreditLimitSettingsComponent;
                break;
            }
            case "CustomerActivationSettingsComponent": {
                myResult = CustomerActivationSettingsComponent_1.CustomerActivationSettingsComponent;
                break;
            }
            case "VATSettingsComponent": {
                myResult = VATSettingsComponent_1.VATSettingsComponent;
                break;
            }
            case "CompetitorComponent": {
                myResult = CompetitorComponent_1.CompetitorComponent;
                break;
            }
            case "CreditLimitSettingsComponent": {
                myResult = CreditLimitSettingsComponent_1.CreditLimitSettingsComponent;
                break;
            }
            case "NewBusinessUnitComponent": {
                myResult = NewBusinessUnitComponent_1.NewBusinessUnitComponent;
                break;
            }
            case "BusinessUnitGeneralTabComponent": {
                myResult = BusinessUnitGeneralTabComponent_1.BusinessUnitGeneralTabComponent;
                break;
            }
            case "CreditLimitSettingsComponent": {
                myResult = CreditLimitSettingsComponent_1.CreditLimitSettingsComponent;
                break;
            }
            case "AccountingTabComponent": {
                myResult = AccountingTabComponent_1.AccountingTabComponent;
                break;
            }
            case "AccountingTab_ChargesType": {
                myResult = AccountingTab_ChargesType_1.AccountingTab_ChargesType;
                break;
            }
            case "AccountingTab_Currency": {
                myResult = AccountingTab_Currency_1.AccountingTab_Currency;
                break;
            }
            case "AccountingTab_Full": {
                myResult = AccountingTab_Full_1.AccountingTab_Full;
                break;
            }
            case "AccountingTab_Partners": {
                myResult = AccountingTab_Partners_1.AccountingTab_Partners;
                break;
            }
            case "AccountingTab_PaymentTerm": {
                myResult = AccountingTab_PaymentTerm_1.AccountingTab_PaymentTerm;
                break;
            }
            case "AccountingTab_QuickBooksOnline": {
                myResult = AccountingTab_QuickBooksOnline_1.AccountingTab_QuickBooksOnline;
                break;
            }
            case "AccountingTab_VatType": {
                myResult = AccountingTab_VatType_1.AccountingTab_VatType;
                break;
            }
            case "ExternalAccountsByProductsComponent": {
                myResult = ExternalAccountsByProductsComponent_1.ExternalAccountsByProductsComponent;
                break;
            }
            case "CustomsInterfaceSettingsComponent": {
                myResult = CustomsInterfaceSettingsComponent_1.CustomsInterfaceSettingsComponent;
                break;
            }
            case "CustomsInterfaceCredintialsComponent": {
                myResult = CustomsInterfaceCredintialsComponent_1.CustomsInterfaceCredintialsComponent;
                break;
            }
            case "FTPDetailComponent": {
                myResult = FTPDetailComponent_1.FTPDetailComponent;
                break;
            }
            case "ArtemusSettingsComponent": {
                myResult = ArtemusSettingsComponent_1.ArtemusSettingsComponent;
                break;
            }
            case "VatTypeGeneralTabComponent": {
                myResult = VatTypeGeneralTabComponent_1.VatTypeGeneralTabComponent;
                break;
            }
            case "VatTypePercentagesTabComponent": {
                myResult = VatTypePercentagesTabComponent_1.VatTypePercentagesTabComponent;
                break;
            }
            case "AccountingTab_Branch": {
                myResult = AccountingTab_Branch_1.AccountingTab_Branch;
                break;
            }
            case "AddEditCustomerFieldsUpdateSettingComponent": {
                myResult = AddEditCustomerFieldsUpdateSettingComponent_1.AddEditCustomerFieldsUpdateSettingComponent;
                break;
            }
            case "BranchGeneralTabComponent": {
                myResult = BranchGeneralTabComponent_1.BranchGeneralTabComponent;
                break;
            }
            case "AddEditBranchAddressComponent": {
                myResult = AddEditBranchAddressComponent_1.AddEditBranchAddressComponent;
                break;
            }
            case "VesselGeneralTabComponent": {
                myResult = VesselGeneralTabComponent_1.VesselGeneralTabComponent;
                break;
            }
            case "QuoteStageGeneralTabComponent": {
                myResult = QuoteStageGeneralTabComponent_1.QuoteStageGeneralTabComponent;
                break;
            }
            case "AddEditCustomerSalesNoteComponent": {
                myResult = AddEditCustomerSalesNoteComponent_1.AddEditCustomerSalesNoteComponent;
                break;
            }
            case "CustomerTenantAccessFiltersMenuComponent": {
                myResult = CustomerTenantAccessFiltersMenuComponent_1.CustomerTenantAccessFiltersMenuComponent;
                break;
            }
            case "DocumentsFilingShortTitleComponent": {
                myResult = DocumentsFilingShortTitleComponent_1.DocumentsFilingShortTitleComponent;
                break;
            }
            case "ChargesExternalByProductsComponent": {
                myResult = ChargesExternalByProductsComponent_1.ChargesExternalByProductsComponent;
                break;
            }
            case "CountryCityGeneralTabComponent": {
                myResult = CountryCityGeneralTabComponent_1.CountryCityGeneralTabComponent;
                break;
            }
            case "DocumentFilingEmailSettingsComponent": {
                myResult = DocumentFilingEmailSettingsComponent_1.DocumentFilingEmailSettingsComponent;
                break;
            }
            case "CustomsShipperFiltersMenuComponent": {
                myResult = CustomsShipperFiltersMenuComponent_1.CustomsShipperFiltersMenuComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map