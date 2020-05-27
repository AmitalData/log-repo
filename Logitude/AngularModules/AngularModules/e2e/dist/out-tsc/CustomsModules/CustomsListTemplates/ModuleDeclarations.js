"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsRequestsSheetsListTemplate_1 = require("./Components/CustomsRequestsSheetsListTemplate");
var DeclarationSupplierInvoiceListTemplate_1 = require("./Components/DeclarationSupplierInvoiceListTemplate");
var CustomsClosedTablesListTemplate_1 = require("./Components/CustomsClosedTablesListTemplate");
var CertificateCheckBoxComponent_1 = require("./Components/CertificateCheckBoxComponent");
var CertificateTextBoxComponent_1 = require("./Components/CertificateTextBoxComponent");
var NotificationListTemplate_1 = require("./Components/NotificationListTemplate");
var SupplierInvoiceItemsTaxListTemplate_1 = require("./Components/SupplierInvoiceItemsTaxListTemplate");
var InterfaceManagementsListTemplate_1 = require("./Components/InterfaceManagementsListTemplate");
var CourierConnectedDeclarationListTemplate_1 = require("./Components/CourierConnectedDeclarationListTemplate");
var SignStationListTemplate_1 = require("./Components/SignStationListTemplate");
var CourierWorksheetListTemplate_1 = require("./Components/CourierWorksheetListTemplate");
var DeclarationQueryListTemplate_1 = require("./Components/DeclarationQueryListTemplate");
exports.Components = [
    CustomsRequestsSheetsListTemplate_1.CustomsRequestsSheetsListTemplate,
    CustomsClosedTablesListTemplate_1.CustomsClosedTablesListTemplate,
    CertificateCheckBoxComponent_1.CertificateCheckBoxComponent,
    CertificateTextBoxComponent_1.CertificateTextBoxComponent,
    InterfaceManagementsListTemplate_1.InterfaceManagementsListTemplate,
    SignStationListTemplate_1.SignStationListTemplate,
    CourierConnectedDeclarationListTemplate_1.CourierConnectedDeclarationListTemplate,
    CourierWorksheetListTemplate_1.CourierWorksheetListTemplate,
    DeclarationSupplierInvoiceListTemplate_1.DeclarationSupplierInvoiceListTemplate,
    DeclarationQueryListTemplate_1.DeclarationQueryListTemplate,
    NotificationListTemplate_1.NotificationListTemplate,
    SupplierInvoiceItemsTaxListTemplate_1.SupplierInvoiceItemsTaxListTemplate,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsRequestsSheetsListTemplate": {
                myResult = CustomsRequestsSheetsListTemplate_1.CustomsRequestsSheetsListTemplate;
                break;
            }
            case "DeclarationSupplierInvoiceListTemplate": {
                myResult = DeclarationSupplierInvoiceListTemplate_1.DeclarationSupplierInvoiceListTemplate;
                break;
            }
            case "CustomsClosedTablesListTemplate": {
                myResult = CustomsClosedTablesListTemplate_1.CustomsClosedTablesListTemplate;
                break;
            }
            case "CertificateCheckBoxComponent": {
                myResult = CertificateCheckBoxComponent_1.CertificateCheckBoxComponent;
                break;
            }
            case "CertificateTextBoxComponent": {
                myResult = CertificateTextBoxComponent_1.CertificateTextBoxComponent;
                break;
            }
            case "NotificationListTemplate": {
                myResult = NotificationListTemplate_1.NotificationListTemplate;
                break;
            }
            case "SupplierInvoiceItemsTaxListTemplate": {
                myResult = SupplierInvoiceItemsTaxListTemplate_1.SupplierInvoiceItemsTaxListTemplate;
                break;
            }
            case "InterfaceManagementsListTemplate": {
                myResult = SupplierInvoiceItemsTaxListTemplate_1.SupplierInvoiceItemsTaxListTemplate;
                break;
            }
            case "SignStationListTemplate": {
                myResult = SignStationListTemplate_1.SignStationListTemplate;
                break;
            }
            case "CourierWorksheetListTemplate": {
                myResult = CourierWorksheetListTemplate_1.CourierWorksheetListTemplate;
                break;
            }
            case "CourierConnectedDeclarationListTemplate": {
                myResult = CourierConnectedDeclarationListTemplate_1.CourierConnectedDeclarationListTemplate;
                break;
            }
            case "DeclarationQueryListTemplate": {
                myResult = DeclarationQueryListTemplate_1.DeclarationQueryListTemplate;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map