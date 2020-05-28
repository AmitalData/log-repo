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
var GenericRequestParams_1 = require("./GenericRequestParams");
var GuaranteeFileFilterRequestParams = /** @class */ (function (_super) {
    __extends(GuaranteeFileFilterRequestParams, _super);
    function GuaranteeFileFilterRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return GuaranteeFileFilterRequestParams;
}(GenericRequestParams_1.GenericRequestParams));
exports.GuaranteeFileFilterRequestParams = GuaranteeFileFilterRequestParams;
//# sourceMappingURL=GuaranteeFileFilterRequestParams.js.map