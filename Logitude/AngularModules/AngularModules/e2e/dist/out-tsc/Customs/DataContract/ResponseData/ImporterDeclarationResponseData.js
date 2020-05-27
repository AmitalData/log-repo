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
var ImporterDeclarationResponseData = /** @class */ (function (_super) {
    __extends(ImporterDeclarationResponseData, _super);
    function ImporterDeclarationResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ImporterDeclarationResponseData;
}(INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData));
exports.ImporterDeclarationResponseData = ImporterDeclarationResponseData;
var PeriodDeclarationResult = /** @class */ (function () {
    function PeriodDeclarationResult() {
    }
    return PeriodDeclarationResult;
}());
exports.PeriodDeclarationResult = PeriodDeclarationResult;
var LoiDeclarationResult = /** @class */ (function () {
    function LoiDeclarationResult() {
    }
    return LoiDeclarationResult;
}());
exports.LoiDeclarationResult = LoiDeclarationResult;
var SecurityDeclarationResult = /** @class */ (function () {
    function SecurityDeclarationResult() {
    }
    return SecurityDeclarationResult;
}());
exports.SecurityDeclarationResult = SecurityDeclarationResult;
//# sourceMappingURL=ImporterDeclarationResponseData.js.map