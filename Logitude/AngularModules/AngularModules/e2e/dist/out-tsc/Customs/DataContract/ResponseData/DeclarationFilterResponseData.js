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
var DeclarationFilterResponseData = /** @class */ (function (_super) {
    __extends(DeclarationFilterResponseData, _super);
    function DeclarationFilterResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return DeclarationFilterResponseData;
}(ResponseDataBase_1.ResponseDataBase));
exports.DeclarationFilterResponseData = DeclarationFilterResponseData;
var GeneralDetails = /** @class */ (function () {
    function GeneralDetails() {
    }
    return GeneralDetails;
}());
exports.GeneralDetails = GeneralDetails;
var Deficit = /** @class */ (function () {
    function Deficit() {
    }
    return Deficit;
}());
exports.Deficit = Deficit;
var Guarantee = /** @class */ (function () {
    function Guarantee() {
    }
    return Guarantee;
}());
exports.Guarantee = Guarantee;
var Claim = /** @class */ (function () {
    function Claim() {
    }
    return Claim;
}());
exports.Claim = Claim;
//# sourceMappingURL=DeclarationFilterResponseData.js.map