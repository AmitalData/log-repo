"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewBIReportFolderComponent_1 = require("./Components/NewEntity/NewBIReportFolderComponent");
var NewBIReport_1 = require("./Components/NewEntity/NewBIReport");
var BIReportGeneralTabComponent_1 = require("./Components/EditTabs/BIReportGeneralTabComponent");
var BIReportPreviewComponent_1 = require("./Components/Workspaces/BIReportPreviewComponent");
var AGGridCustomHeader_1 = require("./Components/TemplateRenderer/AGGridCustomHeader");
var EditShipmentLinkRendererComponent_1 = require("./Components/TemplateRenderer/EditShipmentLinkRendererComponent");
var AgGridColumnsOperations_1 = require("./Components/NewEntity/AgGridColumnsOperations");
var DWAskUserFiltersComponent_1 = require("./Components/Workspaces/DWAskUserFiltersComponent");
var DateSampleComponent_1 = require("./Components/Workspaces/DateSampleComponent");
exports.Components = [
    NewBIReportFolderComponent_1.NewBIReportFolderComponent,
    NewBIReport_1.NewBIReport,
    BIReportGeneralTabComponent_1.BIReportGeneralTabComponent,
    BIReportPreviewComponent_1.BIReportPreviewComponent,
    AgGridColumnsOperations_1.AgGridColumnsOperations,
    DWAskUserFiltersComponent_1.DWAskUserFiltersComponent,
    AGGridCustomHeader_1.AGGridCustomHeader,
    EditShipmentLinkRendererComponent_1.EditShipmentLinkRendererComponent,
    DateSampleComponent_1.DateSampleComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewBIReportFolderComponent": {
                myResult = NewBIReportFolderComponent_1.NewBIReportFolderComponent;
                break;
            }
            case "NewBIReport": {
                myResult = NewBIReport_1.NewBIReport;
                break;
            }
            case "BIReportGeneralTabComponent": {
                myResult = BIReportGeneralTabComponent_1.BIReportGeneralTabComponent;
                break;
            }
            case "BIReportPreviewComponent": {
                myResult = BIReportPreviewComponent_1.BIReportPreviewComponent;
                break;
            }
            case "AgGridColumnsOperations": {
                myResult = AgGridColumnsOperations_1.AgGridColumnsOperations;
                break;
            }
            case "DWAskUserFiltersComponent": {
                myResult = DWAskUserFiltersComponent_1.DWAskUserFiltersComponent;
                break;
            }
            case "AGGridCustomHeader": {
                myResult = AGGridCustomHeader_1.AGGridCustomHeader;
                break;
            }
            case "EditShipmentLinkRendererComponent": {
                myResult = EditShipmentLinkRendererComponent_1.EditShipmentLinkRendererComponent;
                break;
            }
            case "DateSampleComponent": {
                myResult = DateSampleComponent_1.DateSampleComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map