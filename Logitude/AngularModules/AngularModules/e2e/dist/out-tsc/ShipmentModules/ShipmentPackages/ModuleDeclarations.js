"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PackagesTabComponent_1 = require("./Components/Packages/PackagesTabComponent");
var AddEditAirPackageComponent_1 = require("./Components/Packages/AddEditAirPackageComponent");
var AddEditOceanPackageComponent_1 = require("./Components/Packages/AddEditOceanPackageComponent");
var AddEditInsidePackageComponent_1 = require("./Components/Packages/AddEditInsidePackageComponent");
var AdvancedDangerousGoodsComponent_1 = require("./Components/Packages/AdvancedDangerousGoodsComponent");
var ContainerFollowupActionsComponent_1 = require("./Components/Packages/ContainerFU/ContainerFollowupActionsComponent");
var ContainerFollowupWindowComponent_1 = require("./Components/Packages/ContainerFU/ContainerFollowupWindowComponent");
var ContainerFollowupWindowTemplate_1 = require("./Components/Packages/ContainerFU/ContainerFollowupWindowTemplate");
var ContainerFollowupWizardComponent_1 = require("./Components/Packages/ContainerFU/ContainerFollowupWizardComponent");
var ContainerFollowupWizardTemplate_1 = require("./Components/Packages/ContainerFU/ContainerFollowupWizardTemplate");
var LastStatusComponent_1 = require("./Components/Packages/LastStatusComponent");
var AddEditPackageHarmonizeComponent_1 = require("./Components/Packages/AddEditPackageHarmonizeComponent");
var DownloadPackagesFileComponent_1 = require("./Components/Packages/DownloadPackagesFileComponent");
exports.Components = [
    PackagesTabComponent_1.PackagesTabComponent,
    AddEditAirPackageComponent_1.AddEditAirPackageComponent,
    AddEditOceanPackageComponent_1.AddEditOceanPackageComponent,
    AddEditInsidePackageComponent_1.AddEditInsidePackageComponent,
    AdvancedDangerousGoodsComponent_1.AdvancedDangerousGoodsComponent,
    ContainerFollowupActionsComponent_1.ContainerFollowupActionsComponent,
    ContainerFollowupWindowComponent_1.ContainerFollowupWindowComponent,
    ContainerFollowupWindowTemplate_1.ContainerFollowupWindowTemplate,
    ContainerFollowupWizardComponent_1.ContainerFollowupWizardComponent,
    ContainerFollowupWizardTemplate_1.ContainerFollowupWizardTemplate,
    LastStatusComponent_1.LastStatusComponent,
    AddEditPackageHarmonizeComponent_1.AddEditPackageHarmonizeComponent,
    DownloadPackagesFileComponent_1.DownloadPackagesFileComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "PackagesTabComponent": {
                myResult = PackagesTabComponent_1.PackagesTabComponent;
                break;
            }
            case "AddEditAirPackageComponent": {
                myResult = AddEditAirPackageComponent_1.AddEditAirPackageComponent;
                break;
            }
            case "AddEditOceanPackageComponent": {
                myResult = AddEditOceanPackageComponent_1.AddEditOceanPackageComponent;
                break;
            }
            case "AddEditInsidePackageComponent": {
                myResult = AddEditInsidePackageComponent_1.AddEditInsidePackageComponent;
                break;
            }
            case "AdvancedDangerousGoodsComponent": {
                myResult = AdvancedDangerousGoodsComponent_1.AdvancedDangerousGoodsComponent;
                break;
            }
            case "ContainerFollowupActionsComponent": {
                myResult = ContainerFollowupActionsComponent_1.ContainerFollowupActionsComponent;
                break;
            }
            case "ContainerFollowupWindowComponent": {
                myResult = ContainerFollowupWindowComponent_1.ContainerFollowupWindowComponent;
                break;
            }
            case "ContainerFollowupWindowTemplate": {
                myResult = ContainerFollowupWindowTemplate_1.ContainerFollowupWindowTemplate;
                break;
            }
            case "ContainerFollowupWizardComponent": {
                myResult = ContainerFollowupWizardComponent_1.ContainerFollowupWizardComponent;
                break;
            }
            case "ContainerFollowupWizardTemplate": {
                myResult = ContainerFollowupWizardTemplate_1.ContainerFollowupWizardTemplate;
                break;
            }
            case "LastStatusComponent": {
                myResult = LastStatusComponent_1.LastStatusComponent;
                break;
            }
            case "AddEditPackageHarmonizeComponent": {
                myResult = AddEditPackageHarmonizeComponent_1.AddEditPackageHarmonizeComponent;
                break;
            }
            case "DownloadPackagesFileComponent": {
                myResult = DownloadPackagesFileComponent_1.DownloadPackagesFileComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map