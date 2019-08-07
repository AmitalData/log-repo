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
require("rxjs/add/operator/map");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var CommunicationLogStepListService = /** @class */ (function () {
    function CommunicationLogStepListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep';
    }
    CommunicationLogStepListService.prototype.getCommunicationLogStepsListsByLogId = function (logId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?logId=' + logId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService.prototype.getCommunicationLogStepsRequestParamResponseData = function (mainInterfaceCode, logId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCommunicationLogStepsRequestParamResponseData/' + '?mainInterfaceCode=' + mainInterfaceCode + '&logId=' + logId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService.prototype.GetExportExcelByLogId = function (mainInterfaceCode, logId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetExportExcelByLogId/' + '?mainInterfaceCode=' + mainInterfaceCode + '&logId=' + logId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService.prototype.GetExportExcelByRequestId = function (mainInterfaceCode, requestId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetExportExcelByRequestId/' + '?mainInterfaceCode=' + mainInterfaceCode + '&requestId=' + requestId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService.prototype.GetCommunicationLogStepsDocumentDataBystringStepFilter = function (mainInterfaceCode, communicationLogId, tenant, stepFilter, suppressHugeData) {
        var authHeader = new http_1.Headers();
        var $stepFilter = "";
        for (var a in stepFilter) {
            if ($stepFilter) {
                $stepFilter += ",";
            }
            $stepFilter += stepFilter[a];
        }
        var suppressHugeDataValue = false;
        if (suppressHugeData) {
            suppressHugeDataValue = true;
        }
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(
        //GetCommunicationLogStepsDocumentDataBystringStepFilter(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter)
        this._apiUrl + '/GetCommunicationLogStepsDocumentDataBystringStepFilter/' + '?mainInterfaceCode=' + mainInterfaceCode + '&communicationLogId=' + communicationLogId + '&tenant=' + tenant + '&stringStepFilter=' + $stepFilter + "&suppressHugeData=" + suppressHugeDataValue, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService.prototype.GetRequestComminicationIdByEntityId2 = function (tenant, InterfaceTypeCode, RequestStatusCode, ObjectTableId2, EntityId2) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetRequestComminicationIdByEntityId2/' +
            //tenant: number,                 InterfaceTypeCode: string,                  RequestStatusCode: string,                  ObjectTableId2: string,               EntityId2: string
            '?tenant=' + tenant.toString() + '&InterfaceTypeCode=' + InterfaceTypeCode + '&RequestStatusCode=' + RequestStatusCode + '&ObjectTableId2=' + ObjectTableId2 + '&EntityId2=' + encodeURIComponent(EntityId2), { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationLogStepListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CommunicationLogStepListService);
    return CommunicationLogStepListService;
}());
exports.CommunicationLogStepListService = CommunicationLogStepListService;
//# sourceMappingURL=CommunicationLogStepListService.js.map