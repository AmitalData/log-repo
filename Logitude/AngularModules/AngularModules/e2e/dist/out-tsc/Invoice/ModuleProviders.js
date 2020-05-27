"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AccountingSystemsSettingListService_1 = require("./Services/StandardLists/AccountingSystemsSettingListService");
var AccountingSystemsSyncStatusListService_1 = require("./Services/StandardLists/AccountingSystemsSyncStatusListService");
var AccountingTransferHeaderListService_1 = require("./Services/StandardLists/AccountingTransferHeaderListService");
var AccountListService_1 = require("./Services/StandardLists/AccountListService");
var AccountTypeListService_1 = require("./Services/StandardLists/AccountTypeListService");
var APInvoiceListService_1 = require("./Services/StandardLists/APInvoiceListService");
var APInvoiceStatusListService_1 = require("./Services/StandardLists/APInvoiceStatusListService");
var APInvoiceTransferStatusListService_1 = require("./Services/StandardLists/APInvoiceTransferStatusListService");
var APInvoiceTypeListService_1 = require("./Services/StandardLists/APInvoiceTypeListService");
var APPaymentListService_1 = require("./Services/StandardLists/APPaymentListService");
var APPaymentMethodListService_1 = require("./Services/StandardLists/APPaymentMethodListService");
var APPaymentStatusListService_1 = require("./Services/StandardLists/APPaymentStatusListService");
var ARInvoiceListService_1 = require("./Services/StandardLists/ARInvoiceListService");
var ARInvoiceStatusListService_1 = require("./Services/StandardLists/ARInvoiceStatusListService");
var ARInvoiceTransferStatusListService_1 = require("./Services/StandardLists/ARInvoiceTransferStatusListService");
var ARInvoiceTypeListService_1 = require("./Services/StandardLists/ARInvoiceTypeListService");
var ARPaymentListService_1 = require("./Services/StandardLists/ARPaymentListService");
var ARPaymentStatusListService_1 = require("./Services/StandardLists/ARPaymentStatusListService");
var CreditCardTypeListService_1 = require("./Services/StandardLists/CreditCardTypeListService");
var ExternalSystemsTablesCodeListService_1 = require("./Services/StandardLists/ExternalSystemsTablesCodeListService");
var ARPaymentTransferStatusListService_1 = require("./Services/StandardLists/ARPaymentTransferStatusListService");
var APPaymentTransferStatusListService_1 = require("./Services/StandardLists/APPaymentTransferStatusListService");
var SATInterfaceListService_1 = require("./Services/StandardLists/SATInterfaceListService");
var SATPaymentMethodListService_1 = require("./Services/StandardLists/SATPaymentMethodListService");
var SATTransferStatusListService_1 = require("./Services/StandardLists/SATTransferStatusListService");
var SATInvoiceStatusListService_1 = require("./Services/StandardLists/SATInvoiceStatusListService");
var AccountingPaymentMethodPMService_1 = require("./Services/StandardPMs/AccountingPaymentMethodPMService");
var AccountingPaymentMethodListService_1 = require("./Services/StandardLists/AccountingPaymentMethodListService");
var ARInvoiceStockPMService_1 = require("./Services/StandardPMs/ARInvoiceStockPMService");
var AccountingTransferHeaderPMService_1 = require("./Services/StandardPMs/AccountingTransferHeaderPMService");
var APInvoicePMService_1 = require("./Services/StandardPMs/APInvoicePMService");
var APPaymentPMService_1 = require("./Services/StandardPMs/APPaymentPMService");
var ARInvoicePMService_1 = require("./Services/StandardPMs/ARInvoicePMService");
var ARPaymentPMService_1 = require("./Services/StandardPMs/ARPaymentPMService");
var CreditCardTypePMService_1 = require("./Services/StandardPMs/CreditCardTypePMService");
var SATInterfaceSettingPMService_1 = require("./Services/StandardPMs/SATInterfaceSettingPMService");
var BankAccountLitePMService_1 = require("./Services/StandardPMs/BankAccountLitePMService");
var BankAccountLiteListService_1 = require("./Services/StandardLists/BankAccountLiteListService");
var APPaymentMethodPMService_1 = require("./Services/StandardPMs/APPaymentMethodPMService");
var ARPaymentMethodPMService_1 = require("./Services/StandardPMs/ARPaymentMethodPMService");
var APInvoiceMenuButtonsHandler_1 = require("./Components/MenuButtons/APInvoiceMenuButtonsHandler");
var APPaymentMenuButtonsHandler_1 = require("./Components/MenuButtons/APPaymentMenuButtonsHandler");
var ARInvoiceMenuButtonsHandler_1 = require("./Components/MenuButtons/ARInvoiceMenuButtonsHandler");
var ARPaymentMenuButtonsHandler_1 = require("./Components/MenuButtons/ARPaymentMenuButtonsHandler");
var ARInvoiceStockMenuButtonsHandler_1 = require("./Components/MenuButtons/ARInvoiceStockMenuButtonsHandler");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "AccountingSystemsSettingListService": {
                myResult = new AccountingSystemsSettingListService_1.AccountingSystemsSettingListService();
                break;
            }
            case "AccountingSystemsSyncStatusListService": {
                myResult = new AccountingSystemsSyncStatusListService_1.AccountingSystemsSyncStatusListService();
                break;
            }
            case "AccountingTransferHeaderListService": {
                myResult = new AccountingTransferHeaderListService_1.AccountingTransferHeaderListService();
                break;
            }
            case "AccountListService": {
                myResult = new AccountListService_1.AccountListService();
                break;
            }
            case "AccountTypeListService": {
                myResult = new AccountTypeListService_1.AccountTypeListService();
                break;
            }
            case "APInvoiceListService": {
                myResult = new APInvoiceListService_1.APInvoiceListService();
                break;
            }
            case "APInvoiceStatusListService": {
                myResult = new APInvoiceStatusListService_1.APInvoiceStatusListService();
                break;
            }
            case "APInvoiceTransferStatusListService": {
                myResult = new APInvoiceTransferStatusListService_1.APInvoiceTransferStatusListService();
                break;
            }
            case "ARPaymentTransferStatusListService": {
                myResult = new ARPaymentTransferStatusListService_1.ARPaymentTransferStatusListService();
                break;
            }
            case "APInvoiceTypeListService": {
                myResult = new APInvoiceTypeListService_1.APInvoiceTypeListService();
                break;
            }
            case "APPaymentListService": {
                myResult = new APPaymentListService_1.APPaymentListService();
                break;
            }
            case "APPaymentMethodListService": {
                myResult = new APPaymentMethodListService_1.APPaymentMethodListService();
                break;
            }
            case "APPaymentStatusListService": {
                myResult = new APPaymentStatusListService_1.APPaymentStatusListService();
                break;
            }
            case "ARInvoiceListService": {
                myResult = new ARInvoiceListService_1.ARInvoiceListService();
                break;
            }
            case "ARInvoiceStatusListService": {
                myResult = new ARInvoiceStatusListService_1.ARInvoiceStatusListService();
                break;
            }
            case "ARInvoiceTransferStatusListService": {
                myResult = new ARInvoiceTransferStatusListService_1.ARInvoiceTransferStatusListService();
                break;
            }
            case "ARInvoiceTypeListService": {
                myResult = new ARInvoiceTypeListService_1.ARInvoiceTypeListService();
                break;
            }
            case "ARPaymentListService": {
                myResult = new ARPaymentListService_1.ARPaymentListService();
                break;
            }
            case "ARPaymentStatusListService": {
                myResult = new ARPaymentStatusListService_1.ARPaymentStatusListService();
                break;
            }
            case "CreditCardTypeListService": {
                myResult = new CreditCardTypeListService_1.CreditCardTypeListService();
                break;
            }
            case "ExternalSystemsTablesCodeListService": {
                myResult = new ExternalSystemsTablesCodeListService_1.ExternalSystemsTablesCodeListService();
                break;
            }
            case "AccountingTransferHeaderPMService": {
                myResult = new AccountingTransferHeaderPMService_1.AccountingTransferHeaderPMService();
                break;
            }
            case "APInvoicePMService": {
                myResult = new APInvoicePMService_1.APInvoicePMService();
                break;
            }
            case "APPaymentPMService": {
                myResult = new APPaymentPMService_1.APPaymentPMService();
                break;
            }
            case "ARInvoicePMService": {
                myResult = new ARInvoicePMService_1.ARInvoicePMService();
                break;
            }
            case "ARPaymentPMService": {
                myResult = new ARPaymentPMService_1.ARPaymentPMService();
                break;
            }
            case "CreditCardTypePMService": {
                myResult = new CreditCardTypePMService_1.CreditCardTypePMService();
                break;
            }
            case "SATInterfaceSettingPMService": {
                myResult = new SATInterfaceSettingPMService_1.SATInterfaceSettingPMService();
                break;
            }
            case "SATInterfaceListService": {
                myResult = new SATInterfaceListService_1.SATInterfaceListService();
                break;
            }
            case "SATPaymentMethodListService": {
                myResult = new SATPaymentMethodListService_1.SATPaymentMethodListService();
                break;
            }
            case "SATTransferStatusListService": {
                myResult = new SATTransferStatusListService_1.SATTransferStatusListService();
                break;
            }
            case "BankAccountLiteListService": {
                myResult = new BankAccountLiteListService_1.BankAccountLiteListService();
                break;
            }
            case "BankAccountLitePMService": {
                myResult = new BankAccountLitePMService_1.BankAccountLitePMService();
                break;
            }
            case "SATInvoiceStatusListService": {
                myResult = new SATInvoiceStatusListService_1.SATInvoiceStatusListService();
                break;
            }
            case "APPaymentMethodPMService": {
                myResult = new APPaymentMethodPMService_1.APPaymentMethodPMService();
                break;
            }
            case "ARPaymentMethodPMService": {
                myResult = new ARPaymentMethodPMService_1.ARPaymentMethodPMService();
                break;
            }
            case "AccountingPaymentMethodPMService": {
                myResult = new AccountingPaymentMethodPMService_1.AccountingPaymentMethodPMService();
                break;
            }
            case "AccountingPaymentMethodListService": {
                myResult = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
                break;
            }
            case "APPaymentTransferStatusListService": {
                myResult = new APPaymentTransferStatusListService_1.APPaymentTransferStatusListService();
                break;
            }
            case "ARInvoiceStockPMService": {
                myResult = new ARInvoiceStockPMService_1.ARInvoiceStockPMService();
                break;
            }
            case "APInvoiceMenuButtonsHandler": {
                myResult = new APInvoiceMenuButtonsHandler_1.APInvoiceMenuButtonsHandler();
                break;
            }
            case "APPaymentMenuButtonsHandler": {
                myResult = new APPaymentMenuButtonsHandler_1.APPaymentMenuButtonsHandler();
                break;
            }
            case "ARInvoiceMenuButtonsHandler": {
                myResult = new ARInvoiceMenuButtonsHandler_1.ARInvoiceMenuButtonsHandler();
                break;
            }
            case "ARPaymentMenuButtonsHandler": {
                myResult = new ARPaymentMenuButtonsHandler_1.ARPaymentMenuButtonsHandler();
                break;
            }
            case "ARInvoiceStockMenuButtonsHandler": {
                myResult = new ARInvoiceStockMenuButtonsHandler_1.ARInvoiceStockMenuButtonsHandler();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map