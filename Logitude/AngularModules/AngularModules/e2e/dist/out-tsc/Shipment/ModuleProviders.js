"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AccountingInformationIdentifierListService_1 = require("./Services/StandardLists/AccountingInformationIdentifierListService");
var AWBChargesCodeListService_1 = require("./Services/StandardLists/AWBChargesCodeListService");
var AWBCustomsInformationListService_1 = require("./Services/StandardLists/AWBCustomsInformationListService");
var AWBInformationListService_1 = require("./Services/StandardLists/AWBInformationListService");
var MessagingStockListService_1 = require("./Services/StandardLists/MessagingStockListService");
var AWBSpecialHandlingCodeListService_1 = require("./Services/StandardLists/AWBSpecialHandlingCodeListService");
var ManifestStatusListService_1 = require("./Services/StandardLists/ManifestStatusListService");
var OtherParticipantIdListService_1 = require("./Services/StandardLists/OtherParticipantIdListService");
var ShipmentCustomerTypeListService_1 = require("./Services/StandardLists/ShipmentCustomerTypeListService");
var ShipmentLevelListService_1 = require("./Services/StandardLists/ShipmentLevelListService");
var ShipmentListService_1 = require("./Services/StandardLists/ShipmentListService");
var ShipmentPayableStatusListService_1 = require("./Services/StandardLists/ShipmentPayableStatusListService");
var ShipmentReceivableStatusListService_1 = require("./Services/StandardLists/ShipmentReceivableStatusListService");
var ShipmentTypeListService_1 = require("./Services/StandardLists/ShipmentTypeListService");
var SpecialServicesTypeListService_1 = require("./Services/StandardLists/SpecialServicesTypeListService");
var ShipmentFollowUpListService_1 = require("./Services/StandardLists/ShipmentFollowUpListService");
var MessagingStockPMService_1 = require("./Services/StandardPMs/MessagingStockPMService");
var ShipmentPMService_1 = require("./Services/StandardPMs/ShipmentPMService");
var SpecialServicesTypePMService_1 = require("./Services/StandardPMs/SpecialServicesTypePMService");
var MessagingStockMenuButtonsHandler_1 = require("./Components/MenuButtons/MessagingStockMenuButtonsHandler");
var ShipmentMenuButtonsHandler_1 = require("./Components/MenuButtons/ShipmentMenuButtonsHandler");
var FBLStockExtenedPMService_1 = require("./Services/ExtendedPMs/FBLStockExtenedPMService");
var ContainerFollowUpPMService_1 = require("./Services/StandardPMs/ContainerFollowUpPMService");
var ContainerFollowUpListService_1 = require("./Services/StandardLists/ContainerFollowUpListService");
var CustomsTransmissionsStatusListService_1 = require("./Services/StandardLists/CustomsTransmissionsStatusListService");
var OBLTypeListService_1 = require("./Services/StandardLists/OBLTypeListService");
var PickUpDeliveryTransportModeListService_1 = require("./Services/StandardLists/PickUpDeliveryTransportModeListService");
var INTTRADocumentTypeListService_1 = require("./Services/StandardLists/INTTRADocumentTypeListService");
var HarmonizeCodeListService_1 = require("./Services/StandardLists/HarmonizeCodeListService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            // List
            case "AccountingInformationIdentifierListService": {
                myResult = new AccountingInformationIdentifierListService_1.AccountingInformationIdentifierListService();
                break;
            }
            case "AWBChargesCodeListService": {
                myResult = new AWBChargesCodeListService_1.AWBChargesCodeListService();
                break;
            }
            case "AWBCustomsInformationListService": {
                myResult = new AWBCustomsInformationListService_1.AWBCustomsInformationListService();
                break;
            }
            case "AWBInformationListService": {
                myResult = new AWBInformationListService_1.AWBInformationListService();
                break;
            }
            case "MessagingStockListService": {
                myResult = new MessagingStockListService_1.MessagingStockListService();
                break;
            }
            case "AWBSpecialHandlingCodeListService": {
                myResult = new AWBSpecialHandlingCodeListService_1.AWBSpecialHandlingCodeListService();
                break;
            }
            case "ManifestStatusListService": {
                myResult = new ManifestStatusListService_1.ManifestStatusListService();
                break;
            }
            case "OtherParticipantIdListService": {
                myResult = new OtherParticipantIdListService_1.OtherParticipantIdListService();
                break;
            }
            case "ShipmentCustomerTypeListService": {
                myResult = new ShipmentCustomerTypeListService_1.ShipmentCustomerTypeListService();
                break;
            }
            case "ShipmentLevelListService": {
                myResult = new ShipmentLevelListService_1.ShipmentLevelListService();
                break;
            }
            case "ShipmentListService": {
                myResult = new ShipmentListService_1.ShipmentListService();
                break;
            }
            case "ShipmentPayableStatusListService": {
                myResult = new ShipmentPayableStatusListService_1.ShipmentPayableStatusListService();
                break;
            }
            case "ShipmentReceivableStatusListService": {
                myResult = new ShipmentReceivableStatusListService_1.ShipmentReceivableStatusListService();
                break;
            }
            case "ShipmentTypeListService": {
                myResult = new ShipmentTypeListService_1.ShipmentTypeListService();
                break;
            }
            case "SpecialServicesTypeListService": {
                myResult = new SpecialServicesTypeListService_1.SpecialServicesTypeListService();
                break;
            }
            case "ShipmentFollowUpListService": {
                myResult = new ShipmentFollowUpListService_1.ShipmentFollowUpListService();
                break;
            }
            case "ContainerFollowUpPMService": {
                myResult = new ContainerFollowUpPMService_1.ContainerFollowUpPMService();
                break;
            }
            case "ContainerFollowUpListService": {
                myResult = new ContainerFollowUpListService_1.ContainerFollowUpListService();
                break;
            }
            case "CustomsTransmissionsStatusListService": {
                myResult = new CustomsTransmissionsStatusListService_1.CustomsTransmissionsStatusListService();
                break;
            }
            case "OBLTypeListService": {
                myResult = new OBLTypeListService_1.OBLTypeListService();
                break;
            }
            case "PickUpDeliveryTransportModeListService": {
                myResult = new PickUpDeliveryTransportModeListService_1.PickUpDeliveryTransportModeListService();
                break;
            }
            case "INTTRADocumentTypeListService": {
                myResult = new INTTRADocumentTypeListService_1.INTTRADocumentTypeListService();
                break;
            }
            case "HarmonizeCodeListService": {
                myResult = new HarmonizeCodeListService_1.HarmonizeCodeListService();
                break;
            }
            // PM
            case "MessagingStockPMService": {
                myResult = new MessagingStockPMService_1.MessagingStockPMService();
                break;
            }
            case "ShipmentPMService": {
                myResult = new ShipmentPMService_1.ShipmentPMService();
                break;
            }
            case "SpecialServicesTypePMService": {
                myResult = new SpecialServicesTypePMService_1.SpecialServicesTypePMService();
                break;
            }
            case "FBLStockExtenedPMService": {
                myResult = new FBLStockExtenedPMService_1.FBLStockExtenedPMService();
                break;
            }
            // Handler
            case "MessagingStockMenuButtonsHandler": {
                myResult = new MessagingStockMenuButtonsHandler_1.MessagingStockMenuButtonsHandler();
                break;
            }
            case "ShipmentMenuButtonsHandler": {
                myResult = new ShipmentMenuButtonsHandler_1.ShipmentMenuButtonsHandler();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map