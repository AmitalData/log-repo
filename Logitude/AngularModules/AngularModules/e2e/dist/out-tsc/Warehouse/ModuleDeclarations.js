"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewWarehouseEntryComponent_1 = require("./Components/NewWarehouseEntryComponent");
var NewWarehouseReleaseComponent_1 = require("./Components/NewWarehouseReleaseComponent");
var AddEditWarehouseEntryPackagesAndContainers_1 = require("./Components/AddEditWarehouseEntryPackagesAndContainers");
var WarehouseReleaseChoosePackagesComponent_1 = require("./Components/WarehouseReleaseChoosePackagesComponent");
var EditWarehouseEntryComponent_1 = require("./Components/EditWarehouseEntryComponent");
var EditWarehouseReleaseComponent_1 = require("./Components/EditWarehouseReleaseComponent");
var WarehouseEntryShortTitleComponent_1 = require("./Components/ShortTitles/WarehouseEntryShortTitleComponent");
var WarehouseReleaseShortTitleComponent_1 = require("./Components/ShortTitles/WarehouseReleaseShortTitleComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var CopyFromReleasesPackagesComponent_1 = require("./Components/CopyFromReleasesPackagesComponent");
var WarehouseDocsOutTabComponent_1 = require("./Components/EditTabs/DocsOut/WarehouseDocsOutTabComponent");
var WarehouseDocsInTabComponent_1 = require("./Components/EditTabs/DocsIn/WarehouseDocsInTabComponent");
var WarehouseWorkspaceComponent_1 = require("./Components/Workspaces/WarehouseWorkspaceComponent");
var NewFullWarehouseEntryComponent_1 = require("./Components/NewEntity/NewFullWarehouseEntryComponent");
var WarehouseEntryPackagesDetailsComponent_1 = require("./Components/WarehouseEntryPackagesDetailsComponent");
var WarehouseEntryFiltersMenuComponent_1 = require("./Components/FiltersMenu/WarehouseEntryFiltersMenuComponent");
var WarehouseEntryPartnersTabComponent_1 = require("./Components/EditTabs/PartnersTab/WarehouseEntryPartnersTabComponent");
var AddEditPartnerComponent_1 = require("./Components/EditTabs/PartnersTab/AddEditPartnerComponent");
var WarehouseEntryRoutingsTabComponent_1 = require("./Components/EditTabs/RoutingsTab/WarehouseEntryRoutingsTabComponent");
var WarehouseConnectionsTabComponent_1 = require("./Components/EditTabs/ConnectionsTab/WarehouseConnectionsTabComponent");
var WarehouseEntryPackagesTabComponent_1 = require("./Components/EditTabs/PackagesTab/WarehouseEntryPackagesTabComponent");
var WarehouseReleaseFiltersMenuComponent_1 = require("./Components/FiltersMenu/WarehouseReleaseFiltersMenuComponent");
exports.Components = [
    NewWarehouseEntryComponent_1.NewWarehouseEntryComponent,
    NewWarehouseReleaseComponent_1.NewWarehouseReleaseComponent,
    AddEditWarehouseEntryPackagesAndContainers_1.AddEditWarehouseEntryPackagesAndContainers,
    WarehouseReleaseChoosePackagesComponent_1.WarehouseReleaseChoosePackagesComponent,
    CopyFromReleasesPackagesComponent_1.CopyFromReleasesPackagesComponent,
    EditWarehouseEntryComponent_1.EditWarehouseEntryComponent,
    EditWarehouseReleaseComponent_1.EditWarehouseReleaseComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    WarehouseEntryShortTitleComponent_1.WarehouseEntryShortTitleComponent,
    WarehouseReleaseShortTitleComponent_1.WarehouseReleaseShortTitleComponent,
    WarehouseDocsOutTabComponent_1.WarehouseDocsOutTabComponent,
    WarehouseDocsInTabComponent_1.WarehouseDocsInTabComponent,
    WarehouseWorkspaceComponent_1.WarehouseWorkspaceComponent,
    NewFullWarehouseEntryComponent_1.NewFullWarehouseEntryComponent,
    WarehouseEntryPackagesDetailsComponent_1.WarehouseEntryPackagesDetailsComponent,
    WarehouseEntryFiltersMenuComponent_1.WarehouseEntryFiltersMenuComponent,
    WarehouseEntryPartnersTabComponent_1.WarehouseEntryPartnersTabComponent,
    AddEditPartnerComponent_1.AddEditPartnerComponent,
    WarehouseEntryRoutingsTabComponent_1.WarehouseEntryRoutingsTabComponent,
    WarehouseEntryPackagesTabComponent_1.WarehouseEntryPackagesTabComponent,
    WarehouseReleaseFiltersMenuComponent_1.WarehouseReleaseFiltersMenuComponent,
    WarehouseConnectionsTabComponent_1.WarehouseConnectionsTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewWarehouseEntryComponent": {
                myResult = NewWarehouseEntryComponent_1.NewWarehouseEntryComponent;
                break;
            }
            case "NewWarehouseReleaseComponent": {
                myResult = NewWarehouseReleaseComponent_1.NewWarehouseReleaseComponent;
                break;
            }
            case "AddEditWarehouseEntryPackagesAndContainers": {
                myResult = AddEditWarehouseEntryPackagesAndContainers_1.AddEditWarehouseEntryPackagesAndContainers;
                break;
            }
            case "WarehouseReleaseChoosePackagesComponent": {
                myResult = WarehouseReleaseChoosePackagesComponent_1.WarehouseReleaseChoosePackagesComponent;
                break;
            }
            case "EditWarehouseEntryComponent": {
                myResult = EditWarehouseEntryComponent_1.EditWarehouseEntryComponent;
                break;
            }
            case "EditWarehouseReleaseComponent": {
                myResult = EditWarehouseReleaseComponent_1.EditWarehouseReleaseComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "WarehouseEntryShortTitleComponent": {
                myResult = WarehouseEntryShortTitleComponent_1.WarehouseEntryShortTitleComponent;
                break;
            }
            case "WarehouseReleaseShortTitleComponent": {
                myResult = WarehouseReleaseShortTitleComponent_1.WarehouseReleaseShortTitleComponent;
                break;
            }
            case "CopyFromReleasesPackagesComponent": {
                myResult = CopyFromReleasesPackagesComponent_1.CopyFromReleasesPackagesComponent;
                break;
            }
            case "WarehouseDocsOutTabComponent": {
                myResult = WarehouseDocsOutTabComponent_1.WarehouseDocsOutTabComponent;
                break;
            }
            case "WarehouseDocsInTabComponent": {
                myResult = WarehouseDocsInTabComponent_1.WarehouseDocsInTabComponent;
                break;
            }
            case "WarehouseWorkspaceComponent": {
                myResult = WarehouseWorkspaceComponent_1.WarehouseWorkspaceComponent;
                break;
            }
            case "NewFullWarehouseEntryComponent": {
                myResult = NewFullWarehouseEntryComponent_1.NewFullWarehouseEntryComponent;
                break;
            }
            case "WarehouseEntryPackagesDetailsComponent": {
                myResult = WarehouseEntryPackagesDetailsComponent_1.WarehouseEntryPackagesDetailsComponent;
                break;
            }
            case "WarehouseEntryFiltersMenuComponent": {
                myResult = WarehouseEntryFiltersMenuComponent_1.WarehouseEntryFiltersMenuComponent;
                break;
            }
            case "WarehouseEntryPartnersTabComponent": {
                myResult = WarehouseEntryPartnersTabComponent_1.WarehouseEntryPartnersTabComponent;
                break;
            }
            case "AddEditPartnerComponent": {
                myResult = AddEditPartnerComponent_1.AddEditPartnerComponent;
                break;
            }
            case "WarehouseEntryRoutingsTabComponent": {
                myResult = WarehouseEntryRoutingsTabComponent_1.WarehouseEntryRoutingsTabComponent;
                break;
            }
            case "WarehouseEntryPackagesTabComponent": {
                myResult = WarehouseEntryPackagesTabComponent_1.WarehouseEntryPackagesTabComponent;
                break;
            }
            case "WarehouseReleaseFiltersMenuComponent": {
                myResult = WarehouseReleaseFiltersMenuComponent_1.WarehouseReleaseFiltersMenuComponent;
                break;
            }
            case "WarehouseConnectionsTabComponent": {
                myResult = WarehouseConnectionsTabComponent_1.WarehouseConnectionsTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map