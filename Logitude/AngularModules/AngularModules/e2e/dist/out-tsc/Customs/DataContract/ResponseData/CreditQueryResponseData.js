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
var CreditQueryResponseData = /** @class */ (function (_super) {
    __extends(CreditQueryResponseData, _super);
    function CreditQueryResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CreditQueryResponseData;
}(INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData));
exports.CreditQueryResponseData = CreditQueryResponseData;
var BalanceDetailsResult = /** @class */ (function () {
    function BalanceDetailsResult() {
    }
    return BalanceDetailsResult;
}());
exports.BalanceDetailsResult = BalanceDetailsResult;
var BankAccountsResult = /** @class */ (function () {
    function BankAccountsResult() {
    }
    return BankAccountsResult;
}());
exports.BankAccountsResult = BankAccountsResult;
//# sourceMappingURL=CreditQueryResponseData.js.map