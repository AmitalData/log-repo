"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var InvoiceComponent_1 = require("./Components/Workspaces/InvoiceComponent");
var AccountReceivablesComponent_1 = require("./Components/Workspaces/AccountReceivablesComponent");
var AccountPayablesComponent_1 = require("./Components/Workspaces/AccountPayablesComponent");
var AccountingTransferComponent_1 = require("./Components/Workspaces/AccountingTransferComponent");
var RecalculateExternalsComponent_1 = require("./Components/Workspaces/Windows/RecalculateExternalsComponent");
var NewTransferComponent_1 = require("./Components/Workspaces/Windows/NewTransferComponent");
var ExportTransferComponent_1 = require("./Components/Workspaces/Windows/ExportTransferComponent");
var TransferSettingsComponent_1 = require("./Components/Workspaces/Windows/TransferSettingsComponent");
var TransferStartDateComponent_1 = require("./Components/Workspaces/Windows/TransferStartDateComponent");
var PrintTaxComponent_1 = require("./Components/Workspaces/Windows/PrintTaxComponent");
var ExternalAccountingSystemComponent_1 = require("./Components/Workspaces/ExternalAccountingSystemComponent");
var QuickBooksLogin_1 = require("./Components/Workspaces/QuickBooksLogin");
var AccountingTab_AccountingPaymentMethod_1 = require("./Components/AccountingTab/AccountingTab_AccountingPaymentMethod");
var AccountingTab_APPaymentMethod_1 = require("./Components/AccountingTab/AccountingTab_APPaymentMethod");
// BankAccountLite
var NewBankAccountLiteComponent_1 = require("./Components/NewEntity/NewBankAccountLiteComponent");
// TransferHeader
var TransferHeaderDetailsTabComponent_1 = require("./Components/EditTabs/TransferHeader/TransferHeaderDetailsTabComponent");
// ShortTitle
var APInvoiceShortTitleComponent_1 = require("./Components/ShortTitles/APInvoiceShortTitleComponent");
var APPaymentShortTitleComponent_1 = require("./Components/ShortTitles/APPaymentShortTitleComponent");
var ARInvoiceShortTitleComponent_1 = require("./Components/ShortTitles/ARInvoiceShortTitleComponent");
var ARPaymentShortTitleComponent_1 = require("./Components/ShortTitles/ARPaymentShortTitleComponent");
// Helper
var APInvoiceHelperComponent_1 = require("./Components/Helpers/APInvoiceHelperComponent");
var APPaymentHelperComponent_1 = require("./Components/Helpers/APPaymentHelperComponent");
var ARInvoiceHelperComponent_1 = require("./Components/Helpers/ARInvoiceHelperComponent");
var ARPaymentHelperComponent_1 = require("./Components/Helpers/ARPaymentHelperComponent");
var AccountingTransferHeaderHelperComponent_1 = require("./Components/Helpers/AccountingTransferHeaderHelperComponent");
// Templates
var ARInvoiceIsPrintedHeaderTemplate_1 = require("./Components/ListHeaderTemplates/ARInvoiceIsPrintedHeaderTemplate");
var ARInvoiceSentHeaderTemplate_1 = require("./Components/ListHeaderTemplates/ARInvoiceSentHeaderTemplate");
var ARInvoiceMenuButtonsComponent_1 = require("./Components/MenuButtonsComponents/ARInvoiceMenuButtonsComponent");
var CreditLimitPopupComponent_1 = require("./Components/NewEntity/CreditLimitPopupComponent");
var SettingsComponent_1 = require("./Components/Workspaces/SettingsComponent");
var SATInterfaceSettingsComponent_1 = require("./Components/Workspaces/SATInterfaceSettingsComponent");
var SendPaymentWindowComponent_1 = require("./Components/SAT/SendPaymentWindowComponent");
exports.Components = [
    FieldTemplateComponent_1.FieldTemplateComponent,
    InvoiceComponent_1.InvoiceComponent,
    AccountReceivablesComponent_1.AccountReceivablesComponent,
    AccountPayablesComponent_1.AccountPayablesComponent,
    AccountingTransferComponent_1.AccountingTransferComponent,
    RecalculateExternalsComponent_1.RecalculateExternalsComponent,
    NewTransferComponent_1.NewTransferComponent,
    ExportTransferComponent_1.ExportTransferComponent,
    TransferSettingsComponent_1.TransferSettingsComponent,
    TransferStartDateComponent_1.TransferStartDateComponent,
    PrintTaxComponent_1.PrintTaxComponent,
    NewBankAccountLiteComponent_1.NewBankAccountLiteComponent,
    APInvoiceShortTitleComponent_1.APInvoiceShortTitleComponent,
    APPaymentShortTitleComponent_1.APPaymentShortTitleComponent,
    ARInvoiceShortTitleComponent_1.ARInvoiceShortTitleComponent,
    ARPaymentShortTitleComponent_1.ARPaymentShortTitleComponent,
    APInvoiceHelperComponent_1.APInvoiceHelperComponent,
    APPaymentHelperComponent_1.APPaymentHelperComponent,
    ARInvoiceHelperComponent_1.ARInvoiceHelperComponent,
    ARPaymentHelperComponent_1.ARPaymentHelperComponent,
    AccountingTransferHeaderHelperComponent_1.AccountingTransferHeaderHelperComponent,
    ARInvoiceIsPrintedHeaderTemplate_1.ARInvoiceIsPrintedHeaderTemplate,
    ARInvoiceSentHeaderTemplate_1.ARInvoiceSentHeaderTemplate,
    ARInvoiceMenuButtonsComponent_1.ARInvoiceMenuButtonsComponent,
    ExternalAccountingSystemComponent_1.ExternalAccountingSystemComponent,
    QuickBooksLogin_1.QuickBooksLogin,
    TransferHeaderDetailsTabComponent_1.TransferHeaderDetailsTabComponent,
    CreditLimitPopupComponent_1.CreditLimitPopupComponent,
    SettingsComponent_1.SettingsComponent,
    SATInterfaceSettingsComponent_1.SATInterfaceSettingsComponent,
    SendPaymentWindowComponent_1.SendPaymentWindowComponent,
    AccountingTab_AccountingPaymentMethod_1.AccountingTab_AccountingPaymentMethod,
    AccountingTab_APPaymentMethod_1.AccountingTab_APPaymentMethod,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "InvoiceComponent": {
                myResult = InvoiceComponent_1.InvoiceComponent;
                break;
            }
            case "AccountReceivablesComponent": {
                myResult = AccountReceivablesComponent_1.AccountReceivablesComponent;
                break;
            }
            case "AccountPayablesComponent": {
                myResult = AccountPayablesComponent_1.AccountPayablesComponent;
                break;
            }
            case "AccountingTransferComponent": {
                myResult = AccountingTransferComponent_1.AccountingTransferComponent;
                break;
            }
            case "RecalculateExternalsComponent": {
                myResult = RecalculateExternalsComponent_1.RecalculateExternalsComponent;
                break;
            }
            case "NewTransferComponent": {
                myResult = NewTransferComponent_1.NewTransferComponent;
                break;
            }
            case "ExportTransferComponent": {
                myResult = ExportTransferComponent_1.ExportTransferComponent;
                break;
            }
            case "TransferSettingsComponent": {
                myResult = TransferSettingsComponent_1.TransferSettingsComponent;
                break;
            }
            case "TransferStartDateComponent": {
                myResult = TransferStartDateComponent_1.TransferStartDateComponent;
                break;
            }
            case "PrintTaxComponent": {
                myResult = PrintTaxComponent_1.PrintTaxComponent;
                break;
            }
            case "NewBankAccountLiteComponent": {
                myResult = NewBankAccountLiteComponent_1.NewBankAccountLiteComponent;
                break;
            }
            case "APInvoiceShortTitleComponent": {
                myResult = APInvoiceShortTitleComponent_1.APInvoiceShortTitleComponent;
                break;
            }
            case "APPaymentShortTitleComponent": {
                myResult = APPaymentShortTitleComponent_1.APPaymentShortTitleComponent;
                break;
            }
            case "ARInvoiceShortTitleComponent": {
                myResult = ARInvoiceShortTitleComponent_1.ARInvoiceShortTitleComponent;
                break;
            }
            case "ARPaymentShortTitleComponent": {
                myResult = ARPaymentShortTitleComponent_1.ARPaymentShortTitleComponent;
                break;
            }
            case "APInvoiceHelperComponent": {
                myResult = APInvoiceHelperComponent_1.APInvoiceHelperComponent;
                break;
            }
            case "APPaymentHelperComponent": {
                myResult = APPaymentHelperComponent_1.APPaymentHelperComponent;
                break;
            }
            case "ARInvoiceHelperComponent": {
                myResult = ARInvoiceHelperComponent_1.ARInvoiceHelperComponent;
                break;
            }
            case "ARPaymentHelperComponent": {
                myResult = ARPaymentHelperComponent_1.ARPaymentHelperComponent;
                break;
            }
            case "AccountingTransferHeaderHelperComponent": {
                myResult = AccountingTransferHeaderHelperComponent_1.AccountingTransferHeaderHelperComponent;
                break;
            }
            case "ARInvoiceIsPrintedHeaderTemplate": {
                myResult = ARInvoiceIsPrintedHeaderTemplate_1.ARInvoiceIsPrintedHeaderTemplate;
                break;
            }
            case "ARInvoiceSentHeaderTemplate": {
                myResult = ARInvoiceSentHeaderTemplate_1.ARInvoiceSentHeaderTemplate;
                break;
            }
            case "ARInvoiceMenuButtonsComponent": {
                myResult = ARInvoiceMenuButtonsComponent_1.ARInvoiceMenuButtonsComponent;
                break;
            }
            case "ExternalAccountingSystemComponent": {
                myResult = ExternalAccountingSystemComponent_1.ExternalAccountingSystemComponent;
                break;
            }
            case "QuickBooksLogin": {
                myResult = QuickBooksLogin_1.QuickBooksLogin;
                break;
            }
            case "TransferHeaderDetailsTabComponent": {
                myResult = TransferHeaderDetailsTabComponent_1.TransferHeaderDetailsTabComponent;
                break;
            }
            case "CreditLimitPopupComponent": {
                myResult = CreditLimitPopupComponent_1.CreditLimitPopupComponent;
                break;
            }
            case "SettingsComponent": {
                myResult = SettingsComponent_1.SettingsComponent;
                break;
            }
            case "SATInterfaceSettingsComponent": {
                myResult = SATInterfaceSettingsComponent_1.SATInterfaceSettingsComponent;
                break;
            }
            case "SendPaymentWindowComponent": {
                myResult = SendPaymentWindowComponent_1.SendPaymentWindowComponent;
                break;
            }
            case "AccountingTab_AccountingPaymentMethod": {
                myResult = AccountingTab_AccountingPaymentMethod_1.AccountingTab_AccountingPaymentMethod;
                break;
            }
            case "AccountingTab_APPaymentMethod": {
                myResult = AccountingTab_APPaymentMethod_1.AccountingTab_APPaymentMethod;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map