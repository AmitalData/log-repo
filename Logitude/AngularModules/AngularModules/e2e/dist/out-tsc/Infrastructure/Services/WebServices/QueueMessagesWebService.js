"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var QueueMessagesWebService = /** @class */ (function () {
    function QueueMessagesWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QueueMessagesWebService';
    }
    QueueMessagesWebService.prototype.UpdateTenantManagementStatistics = function (tenantId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateTenantManagementStatistics?tenantId=' + tenantId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                //var serviceResponse: ServiceResponse;
                //serviceResponse = new ServiceResponse();
                //serviceResponse.Result = myJsonResult;
                //return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return QueueMessagesWebService;
}());
exports.QueueMessagesWebService = QueueMessagesWebService;
//# sourceMappingURL=QueueMessagesWebService.js.map