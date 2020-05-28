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
var MorningMessageResponseData = /** @class */ (function (_super) {
    __extends(MorningMessageResponseData, _super);
    function MorningMessageResponseData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return MorningMessageResponseData;
}(ResponseDataBase_1.ResponseDataBase));
exports.MorningMessageResponseData = MorningMessageResponseData;
var MorningMessageResult = /** @class */ (function () {
    function MorningMessageResult() {
        this.NeedExpandaple = false;
        this.Toggle = false;
        this.Indicator = "-";
    }
    MorningMessageResult.prototype.ToggleIt = function () {
        this.Toggle = !this.Toggle;
        if (this.Toggle) {
            this.Indicator = "+++";
        }
        else {
            this.Indicator = "---";
        }
    };
    return MorningMessageResult;
}());
exports.MorningMessageResult = MorningMessageResult;
//# sourceMappingURL=MorningMessageResponseData.js.map