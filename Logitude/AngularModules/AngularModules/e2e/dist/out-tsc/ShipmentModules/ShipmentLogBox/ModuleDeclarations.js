"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LogBoxDocumentsComponent_1 = require("./Components/LogBox/LogBoxDocumentsComponent");
var LogBoxMainComponent_1 = require("./Components/LogBox/LogBoxMainComponent");
var EditLogBoxShipmentComponent_1 = require("./Components/LogBox/EditLogBoxShipmentComponent");
var DigitalSignDocTypeComponent_1 = require("./Components/LogBox/DigitalSignDocTypeComponent");
var AddEditImporterDocumentComponent_1 = require("./Components/LogBox/AddEditImporterDocumentComponent");
var LogboxUploaderComponent_1 = require("./Components/Logbox/LogboxUploaderComponent");
var AddEditImporterShipmentComponent_1 = require("./Components/Logbox/AddEditImporterShipmentComponent");
var ForwarderShipmentsComponent_1 = require("./Components/Logbox/ForwarderShipmentsComponent");
var MultiArchiveShipmentsComponent_1 = require("./Components/Logbox/MultiArchiveShipmentsComponent");
var DownloadAllFilesComponent_1 = require("./Components/LogBox/DownloadAllFilesComponent");
var AddEditPrivateLabelShipmentComponent_1 = require("./Components/Logbox/AddEditPrivateLabelShipmentComponent");
var PrivateLabelApprovePaymentComponent_1 = require("./Components/Logbox/PrivateLabelApprovePaymentComponent");
var PrivateLabelApprovebyMobileComponent_1 = require("./Components/Logbox/PrivateLabelApprovebyMobileComponent");
var ECommercePaymentRequestMobileComponent_1 = require("./Components/Logbox/ECommercePaymentRequestMobileComponent");
var TaxScreenComponent_1 = require("./Components/Logbox/TaxScreenComponent");
var GoodsValueComponent_1 = require("./Components/Logbox/GoodsValueComponent");
var DenyReasonComponent_1 = require("./Components/Logbox/DenyReasonComponent");
var LogBoxPackagesComponent_1 = require("./Components/Logbox/LogBoxPackagesComponent");
var DepositionRequestComponent_1 = require("./Components/Logbox/DepositionRequestComponent");
exports.Components = [
    LogBoxDocumentsComponent_1.LogBoxDocumentsComponent,
    LogBoxMainComponent_1.LogBoxMainComponent,
    AddEditImporterDocumentComponent_1.AddEditImporterDocumentComponent,
    LogboxUploaderComponent_1.LogboxUploaderComponent,
    AddEditImporterShipmentComponent_1.AddEditImporterShipmentComponent,
    ForwarderShipmentsComponent_1.ForwarderShipmentsComponent,
    MultiArchiveShipmentsComponent_1.MultiArchiveShipmentsComponent,
    DownloadAllFilesComponent_1.DownloadAllFilesComponent,
    AddEditPrivateLabelShipmentComponent_1.AddEditPrivateLabelShipmentComponent,
    PrivateLabelApprovePaymentComponent_1.PrivateLabelApprovePaymentComponent,
    PrivateLabelApprovebyMobileComponent_1.PrivateLabelApprovebyMobileComponent,
    TaxScreenComponent_1.TaxScreenComponent,
    GoodsValueComponent_1.GoodsValueComponent,
    DenyReasonComponent_1.DenyReasonComponent,
    EditLogBoxShipmentComponent_1.EditLogBoxShipmentComponent,
    DigitalSignDocTypeComponent_1.DigitalSignDocTypeComponent,
    LogBoxPackagesComponent_1.LogBoxPackagesComponent,
    ECommercePaymentRequestMobileComponent_1.ECommercePaymentRequestMobileComponent,
    DepositionRequestComponent_1.DepositionRequestComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "LogBoxDocumentsComponent": {
                myResult = LogBoxDocumentsComponent_1.LogBoxDocumentsComponent;
                break;
            }
            case "LogBoxMainComponent": {
                myResult = LogBoxMainComponent_1.LogBoxMainComponent;
                break;
            }
            case "AddEditImporterDocumentComponent": {
                myResult = AddEditImporterDocumentComponent_1.AddEditImporterDocumentComponent;
                break;
            }
            case "LogboxUploaderComponent": {
                myResult = LogboxUploaderComponent_1.LogboxUploaderComponent;
                break;
            }
            case "AddEditImporterShipmentComponent": {
                myResult = AddEditImporterShipmentComponent_1.AddEditImporterShipmentComponent;
                break;
            }
            case "ForwarderShipmentsComponent": {
                myResult = ForwarderShipmentsComponent_1.ForwarderShipmentsComponent;
                break;
            }
            case "MultiArchiveShipmentsComponent":
                {
                    myResult = MultiArchiveShipmentsComponent_1.MultiArchiveShipmentsComponent;
                    break;
                }
                ;
            case "DownloadAllFilesComponent": {
                myResult = DownloadAllFilesComponent_1.DownloadAllFilesComponent;
                break;
            }
            case "AddEditPrivateLabelShipmentComponent": {
                myResult = AddEditPrivateLabelShipmentComponent_1.AddEditPrivateLabelShipmentComponent;
                break;
            }
            case "PrivateLabelApprovePaymentComponent": {
                myResult = PrivateLabelApprovePaymentComponent_1.PrivateLabelApprovePaymentComponent;
                break;
            }
            case "PrivateLabelApprovebyMobileComponent": {
                myResult = PrivateLabelApprovebyMobileComponent_1.PrivateLabelApprovebyMobileComponent;
                break;
            }
            case "TaxScreenComponent": {
                myResult = TaxScreenComponent_1.TaxScreenComponent;
                break;
            }
            case "GoodsValueComponent": {
                myResult = GoodsValueComponent_1.GoodsValueComponent;
                break;
            }
            case "DenyReasonComponent": {
                myResult = DenyReasonComponent_1.DenyReasonComponent;
                break;
            }
            case "EditLogBoxShipmentComponent": {
                myResult = EditLogBoxShipmentComponent_1.EditLogBoxShipmentComponent;
                break;
            }
            case "DigitalSignDocTypeComponent": {
                myResult = DigitalSignDocTypeComponent_1.DigitalSignDocTypeComponent;
                break;
            }
            case "LogBoxPackagesComponent": {
                myResult = LogBoxPackagesComponent_1.LogBoxPackagesComponent;
                break;
            }
            case "ECommercePaymentRequestMobileComponent": {
                myResult = ECommercePaymentRequestMobileComponent_1.ECommercePaymentRequestMobileComponent;
                break;
            }
            case "DepositionRequestComponent": {
                myResult = DepositionRequestComponent_1.DepositionRequestComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map