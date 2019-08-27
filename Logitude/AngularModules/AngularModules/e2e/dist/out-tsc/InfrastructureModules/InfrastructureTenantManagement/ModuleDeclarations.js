"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BrandingTabComponent_1 = require("./Components/TenantManagement/BrandingTabComponent");
var TenantManagementGeneralTabComponent_1 = require("./Components/TenantManagement/TenantManagementGeneralTabComponent");
var TenantManagementStatisticsTabComponent_1 = require("./Components/TenantManagement/TenantManagementStatisticsTabComponent");
var CCSSettingsTabComponent_1 = require("./Components/TenantManagement/CCSSettingsTabComponent");
var SupportTabComponent_1 = require("./Components/TenantManagement/SupportTabComponent");
var AddEditAddOnComponent_1 = require("./Components/TenantManagement/AddEditAddOnComponent");
var AddEditLicenceComponent_1 = require("./Components/TenantManagement/AddEditLicenceComponent");
var AddEditPrivateLabelsComponent_1 = require("./Components/TenantManagement/AddEditPrivateLabelsComponent");
var PrivateLabelLoadComponent_1 = require("./Components/TenantManagement/PrivateLabelLoadComponent");
exports.Components = [
    BrandingTabComponent_1.BrandingTabComponent,
    TenantManagementGeneralTabComponent_1.TenantManagementGeneralTabComponent,
    TenantManagementStatisticsTabComponent_1.TenantManagementStatisticsTabComponent,
    CCSSettingsTabComponent_1.CCSSettingsTabComponent,
    SupportTabComponent_1.SupportTabComponent,
    AddEditAddOnComponent_1.AddEditAddOnComponent,
    AddEditLicenceComponent_1.AddEditLicenceComponent,
    AddEditPrivateLabelsComponent_1.AddEditPrivateLabelsComponent,
    PrivateLabelLoadComponent_1.PrivateLabelLoadComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "BrandingTabComponent": {
                myResult = BrandingTabComponent_1.BrandingTabComponent;
                break;
            }
            case "TenantManagementGeneralTabComponent": {
                myResult = TenantManagementGeneralTabComponent_1.TenantManagementGeneralTabComponent;
                break;
            }
            case "TenantManagementStatisticsTabComponent": {
                myResult = TenantManagementStatisticsTabComponent_1.TenantManagementStatisticsTabComponent;
                break;
            }
            case "CCSSettingsTabComponent": {
                myResult = CCSSettingsTabComponent_1.CCSSettingsTabComponent;
                break;
            }
            case "SupportTabComponent": {
                myResult = SupportTabComponent_1.SupportTabComponent;
                break;
            }
            case "AddEditAddOnComponent": {
                myResult = AddEditAddOnComponent_1.AddEditAddOnComponent;
                break;
            }
            case "AddEditLicenceComponent": {
                myResult = AddEditLicenceComponent_1.AddEditLicenceComponent;
                break;
            }
            case "AddEditPrivateLabelsComponent": {
                myResult = AddEditPrivateLabelsComponent_1.AddEditPrivateLabelsComponent;
                break;
            }
            case "PrivateLabelLoadComponent": {
                myResult = PrivateLabelLoadComponent_1.PrivateLabelLoadComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map