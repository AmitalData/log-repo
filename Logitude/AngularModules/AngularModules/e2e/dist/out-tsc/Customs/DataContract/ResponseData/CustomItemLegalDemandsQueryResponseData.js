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
//import { ResponseDataBase } from './ResponseDataBase';
var INF_MSG_GenericResponseData_1 = require("./INF_MSG_GenericResponseData");
var CustomItemLegalDemandsQueryResponseData = /** @class */ (function (_super) {
    __extends(CustomItemLegalDemandsQueryResponseData, _super);
    function CustomItemLegalDemandsQueryResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CustomItemLegalDemandsQueryResponseData;
}(INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData));
exports.CustomItemLegalDemandsQueryResponseData = CustomItemLegalDemandsQueryResponseData;
var CustomItemLegalDemandsQueryResult = /** @class */ (function () {
    function CustomItemLegalDemandsQueryResult() {
    }
    return CustomItemLegalDemandsQueryResult;
}());
exports.CustomItemLegalDemandsQueryResult = CustomItemLegalDemandsQueryResult;
var ConfirmationWebAddressResult = /** @class */ (function () {
    function ConfirmationWebAddressResult() {
    }
    return ConfirmationWebAddressResult;
}());
exports.ConfirmationWebAddressResult = ConfirmationWebAddressResult;
var CountriesExclusionResult = /** @class */ (function () {
    function CountriesExclusionResult() {
    }
    return CountriesExclusionResult;
}());
exports.CountriesExclusionResult = CountriesExclusionResult;
//# sourceMappingURL=CustomItemLegalDemandsQueryResponseData.js.map