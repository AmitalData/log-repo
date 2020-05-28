"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var EditSupplierInvoiceItem_1 = require("./Components/SupplierInvoices/SupplierInvoiceItem/EditSupplierInvoiceItem");
var SupplierInvoiceItemCertificatesComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent");
var DeclarationSupplierInvoiceTabComponent_1 = require("./Components/SupplierInvoices/DeclarationSupplierInvoiceTabComponent");
var VendorExtendedSearchComponent_1 = require("./Components/SupplierInvoices/VendorExtendedSearchComponent");
var UpdateProcessCodeComponent_1 = require("./Components/SupplierInvoices/UpdateProcessCodeComponent");
var AddEditSupplierInvoiceComponent_1 = require("./Components/SupplierInvoices/AddEditSupplierInvoiceComponent");
var SupplierInvoiceGeneralTabComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceGeneralTabComponent");
var SupplierInvoiceMoreTabComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceMoreTabComponent");
var PartnersItemsSelectionComponent_1 = require("./Components/SupplierInvoices/PartnersItemsSelectionComponent");
var MultiCertificateUpdateComponent_1 = require("./Components/SupplierInvoices/MultiCertificateUpdate/MultiCertificateUpdateComponent");
var SupplierInvoiceItemVehicleComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemVehicleComponent");
var VehiclesSearchComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceItem/VehiclesSearchComponent");
var AddEditActualLinesComponent_1 = require("./Components/SupplierInvoices/SupplierInvoiceItem/AddEditActualLinesComponent");
exports.Components = [
    EditSupplierInvoiceItem_1.EditSupplierInvoiceItem,
    DeclarationSupplierInvoiceTabComponent_1.DeclarationSupplierInvoiceTabComponent,
    SupplierInvoiceItemCertificatesComponent_1.SupplierInvoiceItemCertificatesComponent,
    VendorExtendedSearchComponent_1.VendorExtendedSearchComponent,
    UpdateProcessCodeComponent_1.UpdateProcessCodeComponent,
    AddEditSupplierInvoiceComponent_1.AddEditSupplierInvoiceComponent,
    SupplierInvoiceGeneralTabComponent_1.SupplierInvoiceGeneralTabComponent,
    SupplierInvoiceMoreTabComponent_1.SupplierInvoiceMoreTabComponent,
    PartnersItemsSelectionComponent_1.PartnersItemsSelectionComponent,
    MultiCertificateUpdateComponent_1.MultiCertificateUpdateComponent,
    SupplierInvoiceItemVehicleComponent_1.SupplierInvoiceItemVehicleComponent,
    VehiclesSearchComponent_1.VehiclesSearchComponent,
    AddEditActualLinesComponent_1.AddEditActualLinesComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "DeclarationSupplierInvoiceTabComponent": {
                myResult = DeclarationSupplierInvoiceTabComponent_1.DeclarationSupplierInvoiceTabComponent;
                break;
            }
            case "EditSupplierInvoiceItem": {
                myResult = EditSupplierInvoiceItem_1.EditSupplierInvoiceItem;
                break;
            }
            case "SupplierInvoiceItemCertificatesComponent": {
                myResult = SupplierInvoiceItemCertificatesComponent_1.SupplierInvoiceItemCertificatesComponent;
                break;
            }
            case "VendorExtendedSearchComponent": {
                myResult = VendorExtendedSearchComponent_1.VendorExtendedSearchComponent;
                break;
            }
            case "UpdateProcessCodeComponent": {
                myResult = UpdateProcessCodeComponent_1.UpdateProcessCodeComponent;
                break;
            }
            case "AddEditSupplierInvoiceComponent": {
                myResult = AddEditSupplierInvoiceComponent_1.AddEditSupplierInvoiceComponent;
                break;
            }
            case "SupplierInvoiceGeneralTabComponent": {
                myResult = SupplierInvoiceGeneralTabComponent_1.SupplierInvoiceGeneralTabComponent;
                break;
            }
            case "SupplierInvoiceMoreTabComponent": {
                myResult = SupplierInvoiceMoreTabComponent_1.SupplierInvoiceMoreTabComponent;
                break;
            }
            case "PartnersItemsSelectionComponent": {
                myResult = PartnersItemsSelectionComponent_1.PartnersItemsSelectionComponent;
                break;
            }
            case "SupplierInvoiceItemVehicleComponent": {
                myResult = SupplierInvoiceItemVehicleComponent_1.SupplierInvoiceItemVehicleComponent;
                break;
            }
            case "VehiclesSearchComponent": {
                myResult = VehiclesSearchComponent_1.VehiclesSearchComponent;
                break;
            }
            case "AddEditActualLinesComponent": {
                myResult = AddEditActualLinesComponent_1.AddEditActualLinesComponent;
                break;
            }
            case "MultiCertificateUpdateComponent": {
                myResult = MultiCertificateUpdateComponent_1.MultiCertificateUpdateComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map