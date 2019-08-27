"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DeclarationPaymentComponent_1 = require("./Components/DeclarationPayment/DeclarationPaymentComponent");
var DeclarationSplitComponent_1 = require("./Components/DeclarationSplitComponent");
var DocumentsPanelComponent_1 = require("./Components/DocumentsPanel/DocumentsPanelComponent");
var VehicleModificationsComponent_1 = require("./Components/VehicleModificationsComponent");
var NewDeclarationComponent_1 = require("./Components/NewEntity/NewDeclarationComponent");
var SendManifestComponent_1 = require("./Components/SendDeclaration/SendManifestComponent");
var SendDeclarationComponent_1 = require("./Components/SendDeclaration/SendDeclarationComponent");
var SupplierInvoiceSelectionComponent_1 = require("./Components/DeclarationPayment/SupplierInvoiceSelectionComponent");
var PointersFromInvoicesSelectionComponent_1 = require("./Components/Documents/PointersFromInvoicesSelectionComponent");
var DeclarationQueryComponent_1 = require("./Components/DeclarationQueryComponent");
exports.Components = [
    DeclarationPaymentComponent_1.DeclarationPaymentComponent,
    DeclarationSplitComponent_1.DeclarationSplitComponent,
    DocumentsPanelComponent_1.DocumentsPanelComponent,
    VehicleModificationsComponent_1.VehicleModificationsComponent,
    NewDeclarationComponent_1.NewDeclarationComponent,
    SendDeclarationComponent_1.SendDeclarationComponent,
    SupplierInvoiceSelectionComponent_1.SupplierInvoiceSelectionComponent,
    PointersFromInvoicesSelectionComponent_1.PointersFromInvoicesSelectionComponent,
    DeclarationQueryComponent_1.DeclarationQueryComponent,
    SendManifestComponent_1.SendManifestComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "DeclarationPaymentComponent": {
                myResult = DeclarationPaymentComponent_1.DeclarationPaymentComponent;
                break;
            }
            case "DeclarationSplitComponent": {
                myResult = DeclarationSplitComponent_1.DeclarationSplitComponent;
                break;
            }
            case "DocumentsPanelComponent": {
                myResult = DocumentsPanelComponent_1.DocumentsPanelComponent;
                break;
            }
            case "VehicleModificationsComponent": {
                myResult = VehicleModificationsComponent_1.VehicleModificationsComponent;
                break;
            }
            case "NewDeclarationComponent": {
                myResult = NewDeclarationComponent_1.NewDeclarationComponent;
                break;
            }
            case "SendDeclarationComponent": {
                myResult = SendDeclarationComponent_1.SendDeclarationComponent;
                break;
            }
            case "SupplierInvoiceSelectionComponent": {
                myResult = SupplierInvoiceSelectionComponent_1.SupplierInvoiceSelectionComponent;
                break;
            }
            case "PointersFromInvoicesSelectionComponent": {
                myResult = PointersFromInvoicesSelectionComponent_1.PointersFromInvoicesSelectionComponent;
                break;
            }
            case "DeclarationQueryComponent": {
                myResult = DeclarationQueryComponent_1.DeclarationQueryComponent;
                break;
            }
            case "SendManifestComponent": {
                myResult = SendManifestComponent_1.SendManifestComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map