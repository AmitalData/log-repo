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
var CustomsDocumentMetaDataValuePM_1 = require("../../EntityPMs/CustomsDocumentMetaDataValuePM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustDocMetaDataValuesWebService = /** @class */ (function () {
    function CustDocMetaDataValuesWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustDocMetaDataValuesWebService';
    }
    CustDocMetaDataValuesWebService.prototype.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds = function (customsDocumentFilingsIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        var params = encodeURIComponent(customsDocumentFilingsIds);
        return Rx_1.Observable.defer(function () {
            var x = 10;
            var custDocMetadataValue = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(null);
            custDocMetadataValue.CustomsDocumentId = customsDocumentFilingsIds;
            ;
            custDocMetadataValue.MetaDataTypeCode = '1';
            return _this._http.post(_this._apiUrl, JSON.stringify(custDocMetadataValue), { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityPMs(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustDocMetaDataValuesWebService.prototype.GetCustomsDocumentMetaDataValuesByConnectedEntity = function (entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var params = encodeURIComponent(entityId);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomsDocumentMetaDataValuesByConnectedEntity?' + 'entityId=' + params, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToEntityPMs(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustDocMetaDataValuesWebService.prototype.MapJsonToEntityPMs = function (jsonPM) {
        var entityPM;
        entityPM = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(null);
        var jsonListKeys = Object.keys(jsonPM);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CustDocMetaDataValuesWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustDocMetaDataValuesWebService);
    return CustDocMetaDataValuesWebService;
}());
exports.CustDocMetaDataValuesWebService = CustDocMetaDataValuesWebService;
//# sourceMappingURL=CustDocMetaDataValuesWebService.js.map