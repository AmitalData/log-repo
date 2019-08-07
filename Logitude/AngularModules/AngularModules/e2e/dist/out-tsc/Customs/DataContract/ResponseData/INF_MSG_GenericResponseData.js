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
var ResponseDataBase_1 = require("./ResponseDataBase");
var INF_MSG_GenericResponseData = /** @class */ (function (_super) {
    __extends(INF_MSG_GenericResponseData, _super);
    function INF_MSG_GenericResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return INF_MSG_GenericResponseData;
}(ResponseDataBase_1.ResponseDataBase));
exports.INF_MSG_GenericResponseData = INF_MSG_GenericResponseData;
//# sourceMappingURL=INF_MSG_GenericResponseData.js.map