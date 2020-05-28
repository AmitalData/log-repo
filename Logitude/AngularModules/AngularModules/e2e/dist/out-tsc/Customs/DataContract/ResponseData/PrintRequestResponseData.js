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
var PrintRequestResponseData = /** @class */ (function (_super) {
    __extends(PrintRequestResponseData, _super);
    function PrintRequestResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return PrintRequestResponseData;
}(INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData));
exports.PrintRequestResponseData = PrintRequestResponseData;
var PrintRequestResultList = /** @class */ (function () {
    function PrintRequestResultList() {
    }
    return PrintRequestResultList;
}());
exports.PrintRequestResultList = PrintRequestResultList;
//# sourceMappingURL=PrintRequestResponseData.js.map