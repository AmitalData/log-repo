"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AWBWizardComponent_1 = require("./Components/AWBWizard/AWBWizardComponent");
var AWBWizardLoadComponent_1 = require("./Components/AWBWizard/AWBWizardLoadComponent");
var AWBOverviewTabComponent_1 = require("./Components/AWBWizard/Overview/AWBOverviewTabComponent");
var AWBPartnersTabComponent_1 = require("./Components/AWBWizard/Partners/AWBPartnersTabComponent");
var AWBAddEditPartnerComponent_1 = require("./Components/AWBWizard/Partners/AWBAddEditPartnerComponent");
var AWBRoutingsTabComponent_1 = require("./Components/AWBWizard/Routings/AWBRoutingsTabComponent");
var AWBHouseRoutingsTabComponent_1 = require("./Components/AWBWizard/Routings/AWBHouseRoutingsTabComponent");
var AWBPackagesTabComponent_1 = require("./Components/AWBWizard/Packages/AWBPackagesTabComponent");
var AWBAddEditPackageComponent_1 = require("./Components/AWBWizard/Packages/AWBAddEditPackageComponent");
var AWBChooseCommodityComponent_1 = require("./Components/AWBWizard/Packages/AWBChooseCommodityComponent");
var AWBDangerousPackageComponent_1 = require("./Components/AWBWizard/Packages/AWBDangerousPackageComponent");
var FreightChargesTabComponent_1 = require("./Components/AWBWizard/FreightCharges/FreightChargesTabComponent");
var GeneralDetailsTabComponent_1 = require("./Components/AWBWizard/GeneralDetails/GeneralDetailsTabComponent");
var AdvancedAccountingComponent_1 = require("./Components/AWBWizard/GeneralDetails/AdvancedAccountingComponent");
var AdvancedCommentsComponent_1 = require("./Components/AWBWizard/GeneralDetails/AdvancedCommentsComponent");
var HAWBTabComponent_1 = require("./Components/AWBWizard/HAWB/HAWBTabComponent");
var OCITabComponent_1 = require("./Components/AWBWizard/OCI/OCITabComponent");
var AddEditOCIComponent_1 = require("./Components/AWBWizard/OCI/AddEditOCIComponent");
var OtherChargesTabComponent_1 = require("./Components/AWBWizard/OtherCharges/OtherChargesTabComponent");
var ManageDefaultsComponent_1 = require("./Components/AWBWizard/OtherCharges/ManageDefaultsComponent");
var AddEditOtherChargeComponent_1 = require("./Components/AWBWizard/OtherCharges/AddEditOtherChargeComponent");
var OtherPartnersTabComponent_1 = require("./Components/AWBWizard/OtherPartners/OtherPartnersTabComponent");
var RADetailsTabComponent_1 = require("./Components/AWBWizard/RADetails/RADetailsTabComponent");
var SendWindowComponent_1 = require("./Components/AWBWizard/SendWindowComponent");
var PurchaseStockComponent_1 = require("./Components/AWBWizard/Others/PurchaseStockComponent");
var SendFSRComponent_1 = require("./Components/FSRWizard/SendFSRComponent");
var FSRWizardComponent_1 = require("./Components/FSRWizard/FSRWizardComponent");
var SendShipmentFSRComponent_1 = require("./Components/FSRWizard/SendShipmentFSRComponent");
exports.Components = [
    AWBWizardComponent_1.AWBWizardComponent,
    AWBWizardLoadComponent_1.AWBWizardLoadComponent,
    AWBOverviewTabComponent_1.AWBOverviewTabComponent,
    AWBPartnersTabComponent_1.AWBPartnersTabComponent,
    AWBAddEditPartnerComponent_1.AWBAddEditPartnerComponent,
    AWBRoutingsTabComponent_1.AWBRoutingsTabComponent,
    AWBHouseRoutingsTabComponent_1.AWBHouseRoutingsTabComponent,
    AWBPackagesTabComponent_1.AWBPackagesTabComponent,
    AWBAddEditPackageComponent_1.AWBAddEditPackageComponent,
    AWBChooseCommodityComponent_1.AWBChooseCommodityComponent,
    AWBDangerousPackageComponent_1.AWBDangerousPackageComponent,
    FreightChargesTabComponent_1.FreightChargesTabComponent,
    GeneralDetailsTabComponent_1.GeneralDetailsTabComponent,
    AdvancedAccountingComponent_1.AdvancedAccountingComponent,
    AdvancedCommentsComponent_1.AdvancedCommentsComponent,
    HAWBTabComponent_1.HAWBTabComponent,
    OCITabComponent_1.OCITabComponent,
    AddEditOCIComponent_1.AddEditOCIComponent,
    OtherChargesTabComponent_1.OtherChargesTabComponent,
    ManageDefaultsComponent_1.ManageDefaultsComponent,
    AddEditOtherChargeComponent_1.AddEditOtherChargeComponent,
    OtherPartnersTabComponent_1.OtherPartnersTabComponent,
    RADetailsTabComponent_1.RADetailsTabComponent,
    SendWindowComponent_1.SendWindowComponent,
    PurchaseStockComponent_1.PurchaseStockComponent,
    SendFSRComponent_1.SendFSRComponent,
    FSRWizardComponent_1.FSRWizardComponent,
    SendShipmentFSRComponent_1.SendShipmentFSRComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "AWBWizardComponent": {
                myResult = AWBWizardComponent_1.AWBWizardComponent;
                break;
            }
            case "AWBWizardLoadComponent": {
                myResult = AWBWizardLoadComponent_1.AWBWizardLoadComponent;
                break;
            }
            case "AWBOverviewTabComponent": {
                myResult = AWBOverviewTabComponent_1.AWBOverviewTabComponent;
                break;
            }
            case "AWBPartnersTabComponent": {
                myResult = AWBPartnersTabComponent_1.AWBPartnersTabComponent;
                break;
            }
            case "AWBAddEditPartnerComponent": {
                myResult = AWBAddEditPartnerComponent_1.AWBAddEditPartnerComponent;
                break;
            }
            case "AWBRoutingsTabComponent": {
                myResult = AWBRoutingsTabComponent_1.AWBRoutingsTabComponent;
                break;
            }
            case "AWBHouseRoutingsTabComponent": {
                myResult = AWBHouseRoutingsTabComponent_1.AWBHouseRoutingsTabComponent;
                break;
            }
            case "AWBPackagesTabComponent": {
                myResult = AWBPackagesTabComponent_1.AWBPackagesTabComponent;
                break;
            }
            case "AWBAddEditPackageComponent": {
                myResult = AWBAddEditPackageComponent_1.AWBAddEditPackageComponent;
                break;
            }
            case "AWBChooseCommodityComponent": {
                myResult = AWBChooseCommodityComponent_1.AWBChooseCommodityComponent;
                break;
            }
            case "AWBDangerousPackageComponent": {
                myResult = AWBDangerousPackageComponent_1.AWBDangerousPackageComponent;
                break;
            }
            case "FreightChargesTabComponent": {
                myResult = FreightChargesTabComponent_1.FreightChargesTabComponent;
                break;
            }
            case "GeneralDetailsTabComponent": {
                myResult = GeneralDetailsTabComponent_1.GeneralDetailsTabComponent;
                break;
            }
            case "AdvancedAccountingComponent": {
                myResult = AdvancedAccountingComponent_1.AdvancedAccountingComponent;
                break;
            }
            case "AdvancedCommentsComponent": {
                myResult = AdvancedCommentsComponent_1.AdvancedCommentsComponent;
                break;
            }
            case "HAWBTabComponent": {
                myResult = HAWBTabComponent_1.HAWBTabComponent;
                break;
            }
            case "OCITabComponent": {
                myResult = OCITabComponent_1.OCITabComponent;
                break;
            }
            case "AddEditOCIComponent": {
                myResult = AddEditOCIComponent_1.AddEditOCIComponent;
                break;
            }
            case "OtherChargesTabComponent": {
                myResult = OtherChargesTabComponent_1.OtherChargesTabComponent;
                break;
            }
            case "ManageDefaultsComponent": {
                myResult = ManageDefaultsComponent_1.ManageDefaultsComponent;
                break;
            }
            case "AddEditOtherChargeComponent": {
                myResult = AddEditOtherChargeComponent_1.AddEditOtherChargeComponent;
                break;
            }
            case "OtherPartnersTabComponent": {
                myResult = OtherPartnersTabComponent_1.OtherPartnersTabComponent;
                break;
            }
            case "RADetailsTabComponent": {
                myResult = RADetailsTabComponent_1.RADetailsTabComponent;
                break;
            }
            case "SendWindowComponent": {
                myResult = SendWindowComponent_1.SendWindowComponent;
                break;
            }
            case "PurchaseStockComponent": {
                myResult = PurchaseStockComponent_1.PurchaseStockComponent;
                break;
            }
            case "SendFSRComponent": {
                myResult = SendFSRComponent_1.SendFSRComponent;
                break;
            }
            case "FSRWizardComponent": {
                myResult = FSRWizardComponent_1.FSRWizardComponent;
                break;
            }
            case "SendShipmentFSRComponent": {
                myResult = SendShipmentFSRComponent_1.SendShipmentFSRComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map