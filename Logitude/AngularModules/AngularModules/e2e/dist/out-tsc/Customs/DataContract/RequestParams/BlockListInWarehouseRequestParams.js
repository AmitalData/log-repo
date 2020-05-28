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
var RequestParamsBase_1 = require("./RequestParamsBase");
var BlockListInWarehouseRequestParams = /** @class */ (function (_super) {
    __extends(BlockListInWarehouseRequestParams, _super);
    function BlockListInWarehouseRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return BlockListInWarehouseRequestParams;
}(RequestParamsBase_1.RequestParamsBase));
exports.BlockListInWarehouseRequestParams = BlockListInWarehouseRequestParams;
var ShowResetBlocksTypesEnum;
(function (ShowResetBlocksTypesEnum) {
    ShowResetBlocksTypesEnum[ShowResetBlocksTypesEnum["No"] = 0] = "No";
    ShowResetBlocksTypesEnum[ShowResetBlocksTypesEnum["Yes"] = 1] = "Yes";
    ShowResetBlocksTypesEnum[ShowResetBlocksTypesEnum["All"] = 2] = "All";
})(ShowResetBlocksTypesEnum = exports.ShowResetBlocksTypesEnum || (exports.ShowResetBlocksTypesEnum = {}));
//# sourceMappingURL=BlockListInWarehouseRequestParams.js.map