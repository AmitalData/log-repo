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
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var TimeManagementDomainService = /** @class */ (function () {
    function TimeManagementDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TimeManagementDomain';
    }
    TimeManagementDomainService.prototype.GetWeeklyTimeSheetList = function (employeeUserId, locationCode, periodStartDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetWeeklyTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper_1.ServiceHelper.GetDateString(periodStartDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var args = new TimeManagementAPIHelper();
                var mappedResult = _this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetPeriodTimeSheetList = function (employeeUserId, locationCode, startDate, endDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDataEntryTimeSheetList?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&startDate=" + ServiceHelper_1.ServiceHelper.GetDateString(startDate) + "&endDate=" + ServiceHelper_1.ServiceHelper.GetDateString(endDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                //var args = new TimeManagementAPIHelper();
                //var mappedResult: TimeManagementAPIHelper = this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetTMProjects = function (employeeUserId, locationCode, periodStartDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTMProjects?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper_1.ServiceHelper.GetDateString(periodStartDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var args = new TimeManagementAPIHelper();
                var mappedResult = _this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, args);
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetTMProjectsByBatchTask = function (employeeUserId, fromDate, toDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTMProjectsByBatchTask?employeeUserId=' + employeeUserId + "&fromDate=" + ServiceHelper_1.ServiceHelper.GetDateString(fromDate) + "&toDate=" + ServiceHelper_1.ServiceHelper.GetDateString(toDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetNewTMProjectConnect = function (MainId, ConnectedId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNewTMProjectConnect?MainId=' + MainId + "&id=" + ConnectedId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    TimeManagementDomainService.prototype.UpdateTimeSheetList = function (helper) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToTimeManagementAPIHelper(helper, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, helper);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetProjectsCounts = function (loggedUserId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetProjectsCounts?loggedUserId=' + loggedUserId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    TimeManagementDomainService.prototype.DeleteTimeSheetItem = function (Id, employeeUserId, locationCode, periodStartDate, exitDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdatedTimeSheetList?Id=' + Id + "&employeeUserId=" + employeeUserId + " &locationCode=" + locationCode + "&periodStartDate=" + ServiceHelper_1.ServiceHelper.GetDateString(periodStartDate) + "&exitDate=" + ServiceHelper_1.ServiceHelper.GetDateString(exitDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = _this.MapJsonToTimeManagementAPIHelper(myJsonResult, true, new TimeManagementAPIHelper());
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetCalculationCompleteWork = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCalculationCompleteWork?';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.MapJsonToTimeManagementAPIHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TimeManagementAPIHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    TimeManagementDomainService.prototype.Prorate = function (EmployeeUserId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetProrate?EmployeeUserId=' + EmployeeUserId, { headers: authHeader }).map(function (response) {
                var iResponse = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = iResponse;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetVacationsSummary = function (Year) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetVacationsSummary?Year=' + Year;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.GetVacationsDetails = function (Year, Type) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetVacationsDetails?Year=' + Year + '&Type=' + Type;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService.prototype.DownloadEmployeesTimesToExcel = function (employeeUserId, locationCode, startDate, endDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDownloadEmployeesTimesToExcel?employeeUserId=' + employeeUserId + "&locationCode=" + locationCode + "&startDate=" + ServiceHelper_1.ServiceHelper.GetDateString(startDate) + "&endDate=" + ServiceHelper_1.ServiceHelper.GetDateString(endDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TimeManagementDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], TimeManagementDomainService);
    return TimeManagementDomainService;
}());
exports.TimeManagementDomainService = TimeManagementDomainService;
var TimeManagementAPIHelper = /** @class */ (function () {
    function TimeManagementAPIHelper() {
        this.Items = [];
        this.ItemsPM = [];
        this.OfficeClockDays = [];
    }
    return TimeManagementAPIHelper;
}());
exports.TimeManagementAPIHelper = TimeManagementAPIHelper;
var TimeSheetItem = /** @class */ (function () {
    function TimeSheetItem() {
        this.Days = [];
    }
    return TimeSheetItem;
}());
exports.TimeSheetItem = TimeSheetItem;
var TimeSheetItemDay = /** @class */ (function () {
    function TimeSheetItemDay() {
    }
    return TimeSheetItemDay;
}());
exports.TimeSheetItemDay = TimeSheetItemDay;
//# sourceMappingURL=TimeManagementDomainService.js.map