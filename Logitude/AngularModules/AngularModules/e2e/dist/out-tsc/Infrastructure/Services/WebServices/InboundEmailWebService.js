"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var InboundEmailWebService = /** @class */ (function () {
    function InboundEmailWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InboundEmailWebService';
    }
    InboundEmailWebService.prototype.SendInboundEmailAsync = function (Recepient, Tenant, Subject, Body, entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMessageResult?recepient=' + Recepient + '&tenant=' + Tenant + '&subject=' + Subject + '&body=' + Body + '&entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var mappedResult = response.json();
                //if (myJsonResult) {
                //    var jsonListKeys = Object.keys(myJsonResult);
                //    for (var key in jsonListKeys) {
                //        var property = jsonListKeys[key];
                //        mappedResult[property] = myJsonResult[property];
                //    }
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return InboundEmailWebService;
}());
exports.InboundEmailWebService = InboundEmailWebService;
var InboundEmailResult = /** @class */ (function () {
    function InboundEmailResult() {
    }
    return InboundEmailResult;
}());
exports.InboundEmailResult = InboundEmailResult;
//# sourceMappingURL=InboundEmailWebService.js.map