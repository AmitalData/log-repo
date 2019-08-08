"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsRequiredFieldListService_1 = require("../StandardLists/CustomsRequiredFieldListService");
var CustomsRequierdFieldsWebService = /** @class */ (function () {
    function CustomsRequierdFieldsWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsRequierdFields';
    }
    CustomsRequierdFieldsWebService.prototype.GetSomeObjectTables = function () {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSomeObjectTables/", {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsRequierdFieldsWebService.prototype.GetCustomsRequiredFieldListsByObjectTable = function (objectTableId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetCustomsRequiredFieldListsByObjectTable/?objectTableId=" + objectTableId, {
                headers: authHeader
            }).map(function (response) {
                var srv = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = srv.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsRequierdFieldsWebService.prototype.PostRequiredFields = function (fields) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var params = JSON.stringify(fields);
            return _this._http.post(_this._apiUrl + '/PostRequiredFields/', JSON.stringify(fields), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsRequierdFieldsWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustomsRequierdFieldsWebService);
    return CustomsRequierdFieldsWebService;
}());
exports.CustomsRequierdFieldsWebService = CustomsRequierdFieldsWebService;
//# sourceMappingURL=CustomsRequierdFieldsWebService.js.map