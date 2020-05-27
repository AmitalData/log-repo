"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var INF_MSG_GenericResponseData_1 = require("./INF_MSG_GenericResponseData");
var PaymentOrderReplyResponseData = /** @class */ (function (_super) {
    __extends(PaymentOrderReplyResponseData, _super);
    function PaymentOrderReplyResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return PaymentOrderReplyResponseData;
}(INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData));
exports.PaymentOrderReplyResponseData = PaymentOrderReplyResponseData;
var PaymentDetailData = /** @class */ (function () {
    function PaymentDetailData() {
    }
    return PaymentDetailData;
}());
exports.PaymentDetailData = PaymentDetailData;
var PaymentMethodData = /** @class */ (function () {
    function PaymentMethodData() {
    }
    return PaymentMethodData;
}());
exports.PaymentMethodData = PaymentMethodData;
var ConnectedEntityData = /** @class */ (function () {
    function ConnectedEntityData() {
    }
    return ConnectedEntityData;
}());
exports.ConnectedEntityData = ConnectedEntityData;
var TaxParagraphData = /** @class */ (function () {
    function TaxParagraphData() {
    }
    return TaxParagraphData;
}());
exports.TaxParagraphData = TaxParagraphData;
//# sourceMappingURL=PaymentOrderReplyResponseData.js.map