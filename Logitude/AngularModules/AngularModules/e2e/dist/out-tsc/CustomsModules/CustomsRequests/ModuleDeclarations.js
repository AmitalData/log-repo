"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsRequestsComponent_1 = require("./Components/CustomsRequestsComponent");
var DeclarationRestoreComponent_1 = require("./Components/DeclarationRequests/DeclarationRestoreComponent");
var DeclarationStatusComponent_1 = require("./Components/DeclarationRequests/DeclarationStatusComponent");
var GuaranteeCertificateComponent_1 = require("./Components/TapagRequests/GuaranteeCertificateComponent");
var FaultQueryComponent_1 = require("./Components/TapagRequests/FaultQueryComponent");
var WarehouseBlockBalanceComponent_1 = require("./Components/DeclarationRequests/WarehouseBlockBalanceComponent");
var MasavPaymentsToAgentComponent_1 = require("./Components/PaymentOrderRequests/MasavPaymentsToAgentComponent");
var PaymentOrderQueryComponent_1 = require("./Components/PaymentOrderRequests/PaymentOrderQueryComponent");
var PrintRequestComponent_1 = require("./Components/DeclarationRequests/PrintRequestComponent");
var GuaranteeFileFilterQueryComponent_1 = require("./Components/TapagRequests/GuaranteeFileFilterQueryComponent");
var PaymentOrderReplyComponent_1 = require("./Components/PaymentOrderRequests/PaymentOrderReplyComponent");
var ExportDeclarationDataComponent_1 = require("./Components/DeclarationRequests/ExportDeclarationDataComponent");
var DeclarationFilterComponent_1 = require("./Components/TapagRequests/DeclarationFilterComponent");
var ClaimFileFilterComponent_1 = require("./Components/ClaimRequests/ClaimFileFilterComponent");
var VehicleForGoodsItemComponent_1 = require("./Components/DeclarationRequests/VehicleForGoodsItemComponent");
var BlockListInWarehouseComponent_1 = require("./Components/DeclarationRequests/BlockListInWarehouseComponent");
var RequestDetailsComponent_1 = require("./Components/DeclarationRequests/RequestDetailsComponent");
var ReleaseGoodsComponent_1 = require("./Components/DeclarationRequests/ReleaseGoodsComponent");
var StorageEntranceComponent_1 = require("./Components/Courier/StorageEntranceComponent");
exports.Components = [
    CustomsRequestsComponent_1.CustomsRequestsComponent,
    DeclarationRestoreComponent_1.DeclarationRestoreComponent,
    StorageEntranceComponent_1.StorageEntranceComponent,
    DeclarationStatusComponent_1.DeclarationStatusComponent,
    GuaranteeCertificateComponent_1.GuaranteeCertificateComponent,
    FaultQueryComponent_1.FaultQueryComponent,
    WarehouseBlockBalanceComponent_1.WarehouseBlockBalanceComponent,
    MasavPaymentsToAgentComponent_1.MasavPaymentsToAgentComponent,
    PaymentOrderQueryComponent_1.PaymentOrderQueryComponent,
    PrintRequestComponent_1.PrintRequestComponent,
    GuaranteeFileFilterQueryComponent_1.GuaranteeFileFilterQueryComponent,
    PaymentOrderReplyComponent_1.PaymentOrderReplyComponent,
    ExportDeclarationDataComponent_1.ExportDeclarationDataComponent,
    DeclarationFilterComponent_1.DeclarationFilterComponent,
    ClaimFileFilterComponent_1.ClaimFileFilterComponent,
    VehicleForGoodsItemComponent_1.VehicleForGoodsItemComponent,
    BlockListInWarehouseComponent_1.BlockListInWarehouseComponent,
    RequestDetailsComponent_1.RequestDetailsComponent,
    ReleaseGoodsComponent_1.ReleaseGoodsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsRequestsComponent": {
                myResult = CustomsRequestsComponent_1.CustomsRequestsComponent;
                break;
            }
            case "DeclarationRestoreComponent": {
                myResult = DeclarationRestoreComponent_1.DeclarationRestoreComponent;
                break;
            }
            case "StorageEntranceComponent": {
                myResult = StorageEntranceComponent_1.StorageEntranceComponent;
                break;
            }
            case "DeclarationStatusComponent": {
                myResult = DeclarationStatusComponent_1.DeclarationStatusComponent;
                break;
            }
            case "GuaranteeCertificateComponent": {
                myResult = GuaranteeCertificateComponent_1.GuaranteeCertificateComponent;
                break;
            }
            case "FaultQueryComponent": {
                myResult = FaultQueryComponent_1.FaultQueryComponent;
                break;
            }
            case "WarehouseBlockBalanceComponent": {
                myResult = WarehouseBlockBalanceComponent_1.WarehouseBlockBalanceComponent;
                break;
            }
            case "MasavPaymentsToAgentComponent": {
                myResult = MasavPaymentsToAgentComponent_1.MasavPaymentsToAgentComponent;
                break;
            }
            case "PaymentOrderQueryComponent": {
                myResult = PaymentOrderQueryComponent_1.PaymentOrderQueryComponent;
                break;
            }
            case "PrintRequestComponent": {
                myResult = PrintRequestComponent_1.PrintRequestComponent;
                break;
            }
            case "GuaranteeFileFilterQueryComponent": {
                myResult = GuaranteeFileFilterQueryComponent_1.GuaranteeFileFilterQueryComponent;
                break;
            }
            case "PaymentOrderReplyComponent": {
                myResult = PaymentOrderReplyComponent_1.PaymentOrderReplyComponent;
                break;
            }
            case "ExportDeclarationDataComponent": {
                myResult = ExportDeclarationDataComponent_1.ExportDeclarationDataComponent;
                break;
            }
            case "DeclarationFilterComponent": {
                myResult = DeclarationFilterComponent_1.DeclarationFilterComponent;
                break;
            }
            case "ClaimFileFilterComponent": {
                myResult = ClaimFileFilterComponent_1.ClaimFileFilterComponent;
                break;
            }
            case "VehicleForGoodsItemComponent": {
                myResult = VehicleForGoodsItemComponent_1.VehicleForGoodsItemComponent;
                break;
            }
            case "BlockListInWarehouseComponent": {
                myResult = BlockListInWarehouseComponent_1.BlockListInWarehouseComponent;
                break;
            }
            case "RequestDetailsComponent": {
                myResult = RequestDetailsComponent_1.RequestDetailsComponent;
                break;
            }
            case "ReleaseGoodsComponent": {
                myResult = ReleaseGoodsComponent_1.ReleaseGoodsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map