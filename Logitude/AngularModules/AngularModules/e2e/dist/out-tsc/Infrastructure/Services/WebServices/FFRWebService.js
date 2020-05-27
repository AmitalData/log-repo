"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var FFRWebService = /** @class */ (function () {
    function FFRWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/FFRWebService';
    }
    FFRWebService.prototype.Send = function (myBookingId, myTenant, myRecipient, isCancellationSent) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMessageResult?myBookingId=' + myBookingId + '&myTenant=' + myTenant + '&myRecipient=' + myRecipient + '&isCancellationSent=' + isCancellationSent;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new FFRResult();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return FFRWebService;
}());
exports.FFRWebService = FFRWebService;
var FFRResult = /** @class */ (function () {
    function FFRResult() {
    }
    return FFRResult;
}());
exports.FFRResult = FFRResult;
//# sourceMappingURL=FFRWebService.js.map