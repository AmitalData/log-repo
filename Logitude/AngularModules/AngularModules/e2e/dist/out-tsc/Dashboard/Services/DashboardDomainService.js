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
var List_1 = require("../../Infrastructure/DataContracts/Dashboard/List");
var DashBoardClass_1 = require("../../Infrastructure/DataContracts/Dashboard/DashBoardClass");
var DailySpotlightClass_1 = require("../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass");
var ChartingDataClass_1 = require("../../Infrastructure/DataContracts/Dashboard/ChartingDataClass");
var DashboardDomainService = /** @class */ (function () {
    function DashboardDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }
    DashboardDomainService.prototype.GetActivityStatus = function (ActivityType, lastMonths, lastDays, currentTenant, customerid) {
        var _this = this;
        if (customerid === void 0) { customerid = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityStatus?type=' + ActivityType + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetActivityStatusByType = function (ActivityType, fromDate, toDate, currentTenant, directionId, transportmodeId, customerid) {
        var _this = this;
        if (customerid === void 0) { customerid = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityStatusByType?type=' + ActivityType + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(toDate) + '&currentTenant=' + currentTenant + '&customerid=' + customerid + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetActivityStatusByMessagesLogs = function (lastDays, showType) {
        var _this = this;
        if (showType === void 0) { showType = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityStatusByMessagesLogs?lastDays=' + lastDays + '&showType=' + showType, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetDashboardSpotlightCounts = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDashboardSpotlightCounts?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var Object = response.json();
                var mappedEntity = new DailySpotlightClass_1.DailySpotlightClass();
                mappedEntity = _this.MapJsonToEntityListDailySpotlightClass(Object);
                return mappedEntity;
            });
        });
    };
    DashboardDomainService.prototype.GetAirlineDashboardSpotlightCounts = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAirlineDashboardSpotlightCounts?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var Object = response.json();
                var mappedEntity = new DailySpotlightClass_1.DailySpotlightClass();
                mappedEntity = _this.MapJsonToEntityListDailySpotlightClass(Object);
                return mappedEntity;
            });
        });
    };
    DashboardDomainService.prototype.GetShipmentByDirectionAndTransmode = function (type, lastMonths, lastDays, currentTenant, customerid) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentByDirectionAndTransmode?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetShipmentByDirectionAndTransmodeCustom = function (ActivityType, fromDate, toDate, customerid) {
        var _this = this;
        if (customerid === void 0) { customerid = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentByDirectionAndTransmodeCustom?type=' + ActivityType + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(toDate) + '&customerid=' + customerid, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetShipmentsByTop10CountriesDashBoard = function (type, lastMonths, lastDays, measurment, currentTenant, top, includeOthers, customerid, directionId, transmodeId) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentsByTop10CountriesDashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetShipmentsByTop10CountriesDashBoardCustom = function (type, FromDate, ToDate, measurment, currentTenant, top, includeOthers, customerid, directionId, transmodeId) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentsByTop10CountriesDashBoardCustom?type=' + type + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&measurment=' + measurment + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetTop10DashBoard = function (type, lastMonths, lastDays, measurment, currentTenant, top, includeOthers, directionId, transportmodeId) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTop10DashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetTop10DashBoardCustom = function (type, FromDate, ToDate, measurment, currentTenant, top, includeOthers, directionId, transportmodeId) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTop10DashBoardCustom?type=' + type + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(FromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(ToDate) + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.GetMoneyStatusForTenant = function (ActivityType, months, days, tenant, index, currency) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyStatusForTenant?type=' + ActivityType + '&months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    DashboardDomainService.prototype.GetMoneyStatusForTenantCustom = function (ActivityType, fromDate, toDate) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyStatusForTenantCustom?type=' + ActivityType + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(toDate) + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(fromDate), {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    DashboardDomainService.prototype.GetMoneyOutStatusForTenant = function (months, days, tenant, index, currency) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyOutStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    DashboardDomainService.prototype.GetDebrotExposure = function (tenant, currency) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDebrotExposure?tenant=' + tenant + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    DashboardDomainService.prototype.GetDashBoardBookings = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDashBoardBookings?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    DashboardDomainService.prototype.GetTopParticipantsDashBoard = function (lastDays) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTopParticipantsDashBoard?lastDays=' + lastDays, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToChartingDataEntityList(allLists[key]);
                    myList.push(entity);
                }
                return myList;
            });
        });
    };
    DashboardDomainService.prototype.MapJsonToEntityListDailySpotlightClass = function (jsonList) {
        var entityList;
        entityList = new DailySpotlightClass_1.DailySpotlightClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    DashboardDomainService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new DashBoardClass_1.DashBoardClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    DashboardDomainService.prototype.MapJsonToChartingDataEntityList = function (jsonList) {
        var entityList;
        entityList = new ChartingDataClass_1.ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    DashboardDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DashboardDomainService);
    return DashboardDomainService;
}());
exports.DashboardDomainService = DashboardDomainService;
//# sourceMappingURL=DashboardDomainService.js.map