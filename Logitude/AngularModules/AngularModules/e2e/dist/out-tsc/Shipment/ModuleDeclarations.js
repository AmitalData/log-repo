"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// General
var OperationsComponent_1 = require("./Components/Workspaces/OperationsComponent");
var ShipmentsComponent_1 = require("./Components/Workspaces/ShipmentsComponent");
var ContainersFUsComponent_1 = require("./Components/Workspaces/ContainersFUsComponent");
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var ShipmentHelperComponent_1 = require("./Components/Helpers/ShipmentHelperComponent");
var ShipmentShortTitleComponent_1 = require("./Components/ShortTitles/ShipmentShortTitleComponent");
var ShipmentFiltersMenuComponent_1 = require("./Components/FiltersMenu/ShipmentFiltersMenuComponent");
var TransportModeListHeaderTemplate_1 = require("./Components/ListHeaderTemplates/TransportModeListHeaderTemplate");
var DirectionListHeaderTemplate_1 = require("./Components/ListHeaderTemplates/DirectionListHeaderTemplate");
var ReactivateShipmentComponent_1 = require("./Components/Reactivate/ReactivateShipmentComponent");
var ReferenceNumberCellDisplayListTemplate_1 = require("./Components/ListTemplates/ReferenceNumberCellDisplayListTemplate");
var StatusCellDisplayListTemplate_1 = require("./Components/ListTemplates/StatusCellDisplayListTemplate");
var TaskCellDisplayListTemplate_1 = require("./Components/ListTemplates/TaskCellDisplayListTemplate");
var DateCellDisplayListTemplate_1 = require("./Components/ListTemplates/DateCellDisplayListTemplate");
var ApprovePaymentButtonListTemplate_1 = require("./Components/ListTemplates/ApprovePaymentButtonListTemplate");
var NewShipmentComponent_1 = require("./Components/NewEntity/NewShipmentComponent");
var NewMasterComponent_1 = require("./Components/NewEntity/NewMasterComponent");
var WizardAddEditAddressComponent_1 = require("./Components/NewEntity/WizardAddEditAddressComponent");
var WizardDimensionsComponent_1 = require("./Components/NewEntity/WizardDimensionsComponent");
var WizardAddEditDimensionsComponent_1 = require("./Components/NewEntity/WizardAddEditDimensionsComponent");
var ActionButtonsListTemplate_1 = require("./Components/ListTemplates/ActionButtonsListTemplate");
var CustomReferenceListTemplate_1 = require("./Components/ListTemplates/CustomReferenceListTemplate");
var EditShipmentButtonListTemplate_1 = require("./Components/ListTemplates/EditShipmentButtonListTemplate");
var ConnectButtonsListTemplate_1 = require("./Components/ListTemplates/ConnectButtonsListTemplate");
var ArchiveListTemplate_1 = require("./Components/ListTemplates/ArchiveListTemplate");
var RequestedDocumentsCountListTemplate_1 = require("./Components/ListTemplates/RequestedDocumentsCountListTemplate");
var DocumentSearchResultListTemplate_1 = require("./Components/ListTemplates/DocumentSearchResultListTemplate");
var ActionValidationComponent_1 = require("./Components/ActionValidationComponent/ActionValidationComponent");
var MenuButtonsTemplateComponent_1 = require("./Components/MenuButtons/MenuButtonsTemplateComponent");
var MasterActionConfirmationComponent_1 = require("./Components/MenuButtons/MasterActionConfirmationComponent");
var ShipmentSpotlightComponent_1 = require("./Components/Spotlight/ShipmentSpotlightComponent");
var SpotLightDateComponent_1 = require("./Components/Spotlight/SpotLightDateComponent");
var SplitShipmentComponent_1 = require("./Components/SplitShipment/SplitShipmentComponent");
var SplitPartialPackageComponent_1 = require("./Components/SplitShipment/SplitPartialPackageComponent");
var RemoveTasksButtonListTemplate_1 = require("./Components/ListTemplates/RemoveTasksButtonListTemplate");
var AnalyzeChampXMLComponent_1 = require("./Components/Helpers/AnalyzeChampXMLComponent");
var ShipmenDirectionConvertComponent_1 = require("./Components/MenuButtons/ShipmenDirectionConvertComponent");
exports.Components = [
    OperationsComponent_1.OperationsComponent,
    ShipmentsComponent_1.ShipmentsComponent,
    ContainersFUsComponent_1.ContainersFUsComponent,
    FieldTemplateComponent_1.FieldTemplateComponent,
    ShipmentHelperComponent_1.ShipmentHelperComponent,
    ShipmentShortTitleComponent_1.ShipmentShortTitleComponent,
    ShipmentFiltersMenuComponent_1.ShipmentFiltersMenuComponent,
    TransportModeListHeaderTemplate_1.TransportModeListHeaderTemplate,
    DirectionListHeaderTemplate_1.DirectionListHeaderTemplate,
    ReactivateShipmentComponent_1.ReactivateShipmentComponent,
    ReferenceNumberCellDisplayListTemplate_1.ReferenceNumberCellDisplayListTemplate,
    StatusCellDisplayListTemplate_1.StatusCellDisplayListTemplate,
    TaskCellDisplayListTemplate_1.TaskCellDisplayListTemplate,
    NewShipmentComponent_1.NewShipmentComponent,
    NewMasterComponent_1.NewMasterComponent,
    WizardAddEditAddressComponent_1.WizardAddEditAddressComponent,
    WizardDimensionsComponent_1.WizardDimensionsComponent,
    WizardAddEditDimensionsComponent_1.WizardAddEditDimensionsComponent,
    DateCellDisplayListTemplate_1.DateCellDisplayListTemplate,
    ActionButtonsListTemplate_1.ActionButtonsListTemplate,
    ConnectButtonsListTemplate_1.ConnectButtonsListTemplate,
    ArchiveListTemplate_1.ArchiveListTemplate,
    RequestedDocumentsCountListTemplate_1.RequestedDocumentsCountListTemplate,
    DocumentSearchResultListTemplate_1.DocumentSearchResultListTemplate,
    ActionValidationComponent_1.ActionValidationComponent,
    MenuButtonsTemplateComponent_1.MenuButtonsTemplateComponent,
    MasterActionConfirmationComponent_1.MasterActionConfirmationComponent,
    ShipmentSpotlightComponent_1.ShipmentSpotlightComponent,
    ApprovePaymentButtonListTemplate_1.ApprovePaymentButtonListTemplate,
    EditShipmentButtonListTemplate_1.EditShipmentButtonListTemplate,
    CustomReferenceListTemplate_1.CustomReferenceListTemplate,
    SplitShipmentComponent_1.SplitShipmentComponent,
    SplitPartialPackageComponent_1.SplitPartialPackageComponent,
    RemoveTasksButtonListTemplate_1.RemoveTasksButtonListTemplate,
    AnalyzeChampXMLComponent_1.AnalyzeChampXMLComponent,
    ShipmenDirectionConvertComponent_1.ShipmenDirectionConvertComponent,
];
exports.ControlsComponents = [
    SpotLightDateComponent_1.SpotLightDateComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "OperationsComponent": {
                myResult = OperationsComponent_1.OperationsComponent;
                break;
            }
            case "ShipmentsComponent": {
                myResult = ShipmentsComponent_1.ShipmentsComponent;
                break;
            }
            case "ContainersFUsComponent": {
                myResult = ContainersFUsComponent_1.ContainersFUsComponent;
                break;
            }
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "ShipmentHelperComponent": {
                myResult = ShipmentHelperComponent_1.ShipmentHelperComponent;
                break;
            }
            case "ShipmentShortTitleComponent": {
                myResult = ShipmentShortTitleComponent_1.ShipmentShortTitleComponent;
                break;
            }
            case "ShipmentFiltersMenuComponent": {
                myResult = ShipmentFiltersMenuComponent_1.ShipmentFiltersMenuComponent;
                break;
            }
            case "TransportModeListHeaderTemplate": {
                myResult = TransportModeListHeaderTemplate_1.TransportModeListHeaderTemplate;
                break;
            }
            case "DirectionListHeaderTemplate": {
                myResult = DirectionListHeaderTemplate_1.DirectionListHeaderTemplate;
                break;
            }
            case "ReactivateShipmentComponent": {
                myResult = ReactivateShipmentComponent_1.ReactivateShipmentComponent;
                break;
            }
            case "ReferenceNumberCellDisplayListTemplate": {
                myResult = ReferenceNumberCellDisplayListTemplate_1.ReferenceNumberCellDisplayListTemplate;
                break;
            }
            case "NewShipmentComponent": {
                myResult = NewShipmentComponent_1.NewShipmentComponent;
                break;
            }
            case "NewMasterComponent": {
                myResult = NewMasterComponent_1.NewMasterComponent;
                break;
            }
            case "WizardAddEditAddressComponent": {
                myResult = WizardAddEditAddressComponent_1.WizardAddEditAddressComponent;
                break;
            }
            case "WizardDimensionsComponent": {
                myResult = WizardDimensionsComponent_1.WizardDimensionsComponent;
                break;
            }
            case "WizardAddEditDimensionsComponent": {
                myResult = WizardAddEditDimensionsComponent_1.WizardAddEditDimensionsComponent;
                break;
            }
            case "StatusCellDisplayListTemplate": {
                myResult = StatusCellDisplayListTemplate_1.StatusCellDisplayListTemplate;
                break;
            }
            case "TaskCellDisplayListTemplate": {
                myResult = TaskCellDisplayListTemplate_1.TaskCellDisplayListTemplate;
                break;
            }
            case "DateCellDisplayListTemplate": {
                myResult = DateCellDisplayListTemplate_1.DateCellDisplayListTemplate;
                break;
            }
            case "ActionButtonsListTemplate": {
                myResult = ActionButtonsListTemplate_1.ActionButtonsListTemplate;
                break;
            }
            case "ConnectButtonsListTemplate": {
                myResult = ConnectButtonsListTemplate_1.ConnectButtonsListTemplate;
                break;
            }
            case "ArchiveListTemplate": {
                myResult = ArchiveListTemplate_1.ArchiveListTemplate;
                break;
            }
            case "RequestedDocumentsCountListTemplate": {
                myResult = RequestedDocumentsCountListTemplate_1.RequestedDocumentsCountListTemplate;
                break;
            }
            case "DocumentSearchResultListTemplate": {
                myResult = DocumentSearchResultListTemplate_1.DocumentSearchResultListTemplate;
                break;
            }
            case "ActionValidationComponent": {
                myResult = ActionValidationComponent_1.ActionValidationComponent;
                break;
            }
            case "MenuButtonsTemplateComponent": {
                myResult = MenuButtonsTemplateComponent_1.MenuButtonsTemplateComponent;
                break;
            }
            case "MasterActionConfirmationComponent": {
                myResult = MasterActionConfirmationComponent_1.MasterActionConfirmationComponent;
                break;
            }
            case "ShipmentSpotlightComponent": {
                myResult = ShipmentSpotlightComponent_1.ShipmentSpotlightComponent;
                break;
            }
            case "ApprovePaymentButtonListTemplate": {
                myResult = ApprovePaymentButtonListTemplate_1.ApprovePaymentButtonListTemplate;
                break;
            }
            case "EditShipmentButtonListTemplate": {
                myResult = EditShipmentButtonListTemplate_1.EditShipmentButtonListTemplate;
                break;
            }
            case "CustomReferenceListTemplate": {
                myResult = CustomReferenceListTemplate_1.CustomReferenceListTemplate;
                break;
            }
            case "SpotLightDateComponent": {
                myResult = SpotLightDateComponent_1.SpotLightDateComponent;
                break;
            }
            case "SplitShipmentComponent": {
                myResult = SplitShipmentComponent_1.SplitShipmentComponent;
                break;
            }
            case "SplitPartialPackageComponent": {
                myResult = SplitPartialPackageComponent_1.SplitPartialPackageComponent;
                break;
            }
            case "RemoveTasksButtonListTemplate": {
                myResult = RemoveTasksButtonListTemplate_1.RemoveTasksButtonListTemplate;
                break;
            }
            case "AnalyzeChampXMLComponent": {
                myResult = AnalyzeChampXMLComponent_1.AnalyzeChampXMLComponent;
                break;
            }
            case "ShipmenDirectionConvertComponent": {
                myResult = ShipmenDirectionConvertComponent_1.ShipmenDirectionConvertComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map