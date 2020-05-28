"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var GLAccountWithholdingTaxExtendedPMService = /** @class */ (function () {
    function GLAccountWithholdingTaxExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/GLAccountingWithholdingTax';
    }
    GLAccountWithholdingTaxExtendedPMService.prototype.GetDeductionPercentage = function (vendorId, date) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetDeductionPercentage?vendorId=' + vendorId + '&registerDate=' + ServiceHelper_1.ServiceHelper.GetDateString(date), { headers: authHeader })
                    .map(function (response) {
                    var myResult = response.json();
                    var serviceResponse;
                    serviceResponse = new ServiceResponse_1.ServiceResponse();
                    serviceResponse.Result = myResult;
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    return GLAccountWithholdingTaxExtendedPMService;
}());
exports.GLAccountWithholdingTaxExtendedPMService = GLAccountWithholdingTaxExtendedPMService;
var GLAccountingWithholdingItem = /** @class */ (function () {
    function GLAccountingWithholdingItem() {
    }
    return GLAccountingWithholdingItem;
}());
exports.GLAccountingWithholdingItem = GLAccountingWithholdingItem;
//# sourceMappingURL=GLAccountWithholdingTaxExtendedService.js.map