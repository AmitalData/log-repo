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
var CreateClientRequestParams = /** @class */ (function (_super) {
    __extends(CreateClientRequestParams, _super);
    function CreateClientRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CreateClientRequestParams;
}(GenericRequestParams_1.GenericRequestParams));
exports.CreateClientRequestParams = CreateClientRequestParams;
var ClientAdressParams = /** @class */ (function () {
    function ClientAdressParams() {
    }
    return ClientAdressParams;
}());
exports.ClientAdressParams = ClientAdressParams;
var ClientAddressCommunicationType = /** @class */ (function () {
    function ClientAddressCommunicationType() {
    }
    return ClientAddressCommunicationType;
}());
exports.ClientAddressCommunicationType = ClientAddressCommunicationType;
var ClientDrivingLicenseParams = /** @class */ (function () {
    function ClientDrivingLicenseParams() {
    }
    return ClientDrivingLicenseParams;
}());
exports.ClientDrivingLicenseParams = ClientDrivingLicenseParams;
var ClientDrivingLicenseTypeParams = /** @class */ (function () {
    function ClientDrivingLicenseTypeParams() {
    }
    return ClientDrivingLicenseTypeParams;
}());
exports.ClientDrivingLicenseTypeParams = ClientDrivingLicenseTypeParams;
//# sourceMappingURL=CreateClientRequestParams.js.map