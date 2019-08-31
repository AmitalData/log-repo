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
var AddAddressContactForClientRequestParams = /** @class */ (function (_super) {
    __extends(AddAddressContactForClientRequestParams, _super);
    function AddAddressContactForClientRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return AddAddressContactForClientRequestParams;
}(GenericRequestParams_1.GenericRequestParams));
exports.AddAddressContactForClientRequestParams = AddAddressContactForClientRequestParams;
var OperationTypes;
(function (OperationTypes) {
    OperationTypes[OperationTypes["Add"] = 1] = "Add";
    OperationTypes[OperationTypes["Update"] = 2] = "Update";
    OperationTypes[OperationTypes["Delete"] = 3] = "Delete";
})(OperationTypes = exports.OperationTypes || (exports.OperationTypes = {}));
var ClientAddress = /** @class */ (function () {
    function ClientAddress() {
    }
    return ClientAddress;
}());
exports.ClientAddress = ClientAddress;
var ClientsAddressCommunicationResult = /** @class */ (function () {
    function ClientsAddressCommunicationResult() {
    }
    return ClientsAddressCommunicationResult;
}());
exports.ClientsAddressCommunicationResult = ClientsAddressCommunicationResult;
//# sourceMappingURL=AddAddressContactForClientRequestParams.js.map