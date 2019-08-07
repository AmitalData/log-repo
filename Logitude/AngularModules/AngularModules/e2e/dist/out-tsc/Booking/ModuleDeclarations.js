"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BookingsComponent_1 = require("./Components/Workspaces/BookingsComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var BookingWizardComponent_1 = require("./Components/BookingWizard/BookingWizardComponent");
var BookingWizardLoadComponent_1 = require("./Components/BookingWizard/BookingWizardLoadComponent");
var OverviewTabComponent_1 = require("./Components/BookingWizard/Overview/OverviewTabComponent");
var BookingDetailsTabComponent_1 = require("./Components/BookingWizard/BookingDetails/BookingDetailsTabComponent");
var PartnersTabComponent_1 = require("./Components/BookingWizard/Partners/PartnersTabComponent");
var AddEditPartnerComponent_1 = require("./Components/BookingWizard/Partners/AddEditPartnerComponent");
var PackagesTabComponent_1 = require("./Components/BookingWizard/Packages/PackagesTabComponent");
var AddEditPackageComponent_1 = require("./Components/BookingWizard/Packages/AddEditPackageComponent");
var ChooseDescriptionOfGoodsComponent_1 = require("./Components/BookingWizard/Packages/ChooseDescriptionOfGoodsComponent");
var DangerousPackageComponent_1 = require("./Components/BookingWizard/Packages/DangerousPackageComponent");
var GeneralDetailsTabComponent_1 = require("./Components/BookingWizard/GeneralDetails/GeneralDetailsTabComponent");
var NoRemainingStockComponent_1 = require("./Components/BookingWizard/NoRemainingStockComponent");
exports.Components = [
    BookingsComponent_1.BookingsComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    BookingWizardComponent_1.BookingWizardComponent,
    BookingWizardLoadComponent_1.BookingWizardLoadComponent,
    OverviewTabComponent_1.OverviewTabComponent,
    BookingDetailsTabComponent_1.BookingDetailsTabComponent,
    PartnersTabComponent_1.PartnersTabComponent,
    AddEditPartnerComponent_1.AddEditPartnerComponent,
    PackagesTabComponent_1.PackagesTabComponent,
    AddEditPackageComponent_1.AddEditPackageComponent,
    ChooseDescriptionOfGoodsComponent_1.ChooseDescriptionOfGoodsComponent,
    DangerousPackageComponent_1.DangerousPackageComponent,
    GeneralDetailsTabComponent_1.GeneralDetailsTabComponent,
    NoRemainingStockComponent_1.NoRemainingStockComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "BookingsComponent": {
                myResult = BookingsComponent_1.BookingsComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "BookingWizardComponent": {
                myResult = BookingWizardComponent_1.BookingWizardComponent;
                break;
            }
            case "BookingWizardLoadComponent": {
                myResult = BookingWizardLoadComponent_1.BookingWizardLoadComponent;
                break;
            }
            case "OverviewTabComponent": {
                myResult = OverviewTabComponent_1.OverviewTabComponent;
                break;
            }
            case "BookingDetailsTabComponent": {
                myResult = BookingDetailsTabComponent_1.BookingDetailsTabComponent;
                break;
            }
            case "PartnersTabComponent": {
                myResult = PartnersTabComponent_1.PartnersTabComponent;
                break;
            }
            case "AddEditPartnerComponent": {
                myResult = AddEditPartnerComponent_1.AddEditPartnerComponent;
                break;
            }
            case "PackagesTabComponent": {
                myResult = PackagesTabComponent_1.PackagesTabComponent;
                break;
            }
            case "AddEditPackageComponent": {
                myResult = AddEditPackageComponent_1.AddEditPackageComponent;
                break;
            }
            case "ChooseDescriptionOfGoodsComponent": {
                myResult = ChooseDescriptionOfGoodsComponent_1.ChooseDescriptionOfGoodsComponent;
                break;
            }
            case "DangerousPackageComponent": {
                myResult = DangerousPackageComponent_1.DangerousPackageComponent;
                break;
            }
            case "GeneralDetailsTabComponent": {
                myResult = GeneralDetailsTabComponent_1.GeneralDetailsTabComponent;
                break;
            }
            case "NoRemainingStockComponent": {
                myResult = NoRemainingStockComponent_1.NoRemainingStockComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map