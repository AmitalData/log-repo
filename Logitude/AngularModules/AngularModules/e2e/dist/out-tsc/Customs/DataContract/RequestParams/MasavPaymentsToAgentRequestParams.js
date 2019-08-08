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
var MasavPaymentsToAgentRequestParams = /** @class */ (function (_super) {
    __extends(MasavPaymentsToAgentRequestParams, _super);
    function MasavPaymentsToAgentRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return MasavPaymentsToAgentRequestParams;
}(GenericRequestParams_1.GenericRequestParams));
exports.MasavPaymentsToAgentRequestParams = MasavPaymentsToAgentRequestParams;
//# sourceMappingURL=MasavPaymentsToAgentRequestParams.js.map