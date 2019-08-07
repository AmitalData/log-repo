"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DeclarationVehicleModificationListService = /** @class */ (function () {
    function DeclarationVehicleModificationListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DeclarationVehicleModification';
    }
    DeclarationVehicleModificationListService.prototype.GetDeclarationVehicleModification = function (declarationId, chassisNumber, adjustmentTypeCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetDeclarationVehicleModification';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDeclarationVehicleModification/?'
                + '&declarationId=' + declarationId
                + '&chassisNumber=' + chassisNumber
                + '&adjustmentTypeCode=' + adjustmentTypeCode
                + '&tenant=' + tenant.toString(), { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationVehicleModificationListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new DeclarationVehicleModificationList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    return DeclarationVehicleModificationListService;
}());
exports.DeclarationVehicleModificationListService = DeclarationVehicleModificationListService;
var DeclarationVehicleModificationList = /** @class */ (function () {
    function DeclarationVehicleModificationList() {
    }
    return DeclarationVehicleModificationList;
}());
exports.DeclarationVehicleModificationList = DeclarationVehicleModificationList;
//# sourceMappingURL=DeclarationVehicleModificationListService.js.map