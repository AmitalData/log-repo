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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var MAWBStackPM_1 = require("../EntityPMs/MAWBStackPM");
var AWBStackDomainService = /** @class */ (function () {
    function AWBStackDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/AWBStackDomain';
    }
    AWBStackDomainService.prototype.GetMAWBStackPMsByAirlineId = function (myCardId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineId?myCardId=' + myCardId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetMAWBStackPMByNumber = function (number) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMAWBStackPMByNumber?number=' + number;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJason = response.json();
                var itemMapped;
                if (itemJason) {
                    itemMapped = _this.MapStackPM(itemJason);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetMAWBStackPMsCountByAirlineIdAndShipperId = function (airlineId, customerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMAWBStackPMsCountByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJason = response.json();
                var itemMapped = +itemJason;
                return itemMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetCardHasAssignedMawbStacks = function (airlineId, customerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCardHasAssignedMawbStacks?airlineId=' + airlineId + '&customerId=' + customerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJason = response.json();
                var itemMapped = itemJason; //== "true" ? true : false;
                return itemMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetMAWBStackPMsByAirlineIdAndShipperId = function (airlineId, customerId, pageSize, pageIndex) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId + '&pageSize=' + pageSize + '&pageIndex=' + pageIndex;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                return listMapped;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetCustomerStockSeries = function (myCustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerStockSeries?myCustomerId=' + myCustomerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetAllAvailableStockSeries = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllAvailableStockSeries';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.GetAirlineAvailableStockSeries = function (airlineId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAirlineAvailableStockSeries?airlineId=' + airlineId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.AssignStockSeriesToCustomer = function (start, end, airlineId, customerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAssignStockSeriesToCustomer?start=' + start + '&end=' + end + '&airlineId=' + airlineId + '&customerId=' + customerId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.UnAssignStockSeriesToUser = function (start, end, airlineId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUnAssignStockSeriesToUser?start=' + start + '&end=' + end + '&airlineId=' + airlineId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.DeleteMAWBStacksOperation = function (myStackId, myAirlineId, isDeletingSeries) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDeleteMAWBStacksOperation?myStackId=' + myStackId + '&myAirlineId=' + myAirlineId + '&isDeletingSeries=' + isDeletingSeries;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.CreateMAWBStacksOperation = function (myAirlineId, myStartNumber, myEndNumber, assignedToId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCreateMAWBStacksOperation?myAirlineId=' + myAirlineId + '&myStartNumber=' + myStartNumber + '&myEndNumber=' + myEndNumber + '&assignedToId=' + assignedToId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    AWBStackDomainService.prototype.MapStackPM = function (jsonList) {
        var entityPM = null;
        if (jsonList) {
            entityPM = new MAWBStackPM_1.MAWBStackPM();
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
            entityPM.IsDirty = false;
        }
        return entityPM;
    };
    AWBStackDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], AWBStackDomainService);
    return AWBStackDomainService;
}());
exports.AWBStackDomainService = AWBStackDomainService;
var StockSeries = /** @class */ (function () {
    function StockSeries() {
    }
    return StockSeries;
}());
exports.StockSeries = StockSeries;
var StockSeriesListClass = /** @class */ (function () {
    function StockSeriesListClass() {
        this.StockSeriesList = [];
    }
    return StockSeriesListClass;
}());
exports.StockSeriesListClass = StockSeriesListClass;
//# sourceMappingURL=AWBStackDomainService.js.map