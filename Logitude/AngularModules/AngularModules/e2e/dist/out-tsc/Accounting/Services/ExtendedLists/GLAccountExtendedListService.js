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
var GLAccountList_1 = require("../../EntityLists/GLAccountList");
var GLAccountTotalByMonthList_1 = require("../../EntityLists/GLAccountTotalByMonthList");
var PeriodM_1 = require("../../DataContracts/PeriodM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var GLAccountExtendedListService = /** @class */ (function () {
    function GLAccountExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/glaccountviews';
    }
    GLAccountExtendedListService.prototype.GetRecentGLAccounts = function (accountTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetRecentGLAccounts?accountTypeCode=' + accountTypeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.GetChildrenGLAccounts = function (GLAccountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetChildrenGLAccounts?GLAccountId=' + GLAccountId, {
                headers: authHeader
            }).map(function (response) {
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
            });
        });
    };
    GLAccountExtendedListService.prototype.GetSplittedByCurrencyGLAccounts = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSplittedByCurrencyGLAccounts?accountId=' + accountId, {
                headers: authHeader
            }).map(function (response) {
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
            });
        });
    };
    GLAccountExtendedListService.prototype.CheckIfHasLedgerTransactions = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetAccountTransactions?accountId=' + accountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var res = response.json();
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.CheckIfSplitted = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/CheckIfSplitted?accountId=' + accountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var res = response.json();
                if (res && res != null) {
                    if (res.length > 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.GetGLAccountsSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetGLAccountsSummary?', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    GLAccountExtendedListService.prototype.CalculateFututreCheques = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCalculateFututreCheques?', {
                headers: authHeader
            }).map(function (response) {
                return response.json();
            });
        });
    };
    GLAccountExtendedListService.prototype.GetAccountCurrencies = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetAccountCurrencies?accountId=' + accountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.GetAccountReconcilesCount = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAccountReconcilesCount?glAccountId=' + accountId, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                return res;
            });
        });
    };
    GLAccountExtendedListService.prototype.GetAgingReport = function (args) {
        var _this = this;
        var urlParameters = "";
        urlParameters += "agingForDate=" + args.AgingForDate.toISOString();
        urlParameters += ("&numberOfmonthsbackwards=" + args.NumberOfmonthsbackwards);
        urlParameters += "&vendorCustomerId=" + args.VendorCustomerId;
        urlParameters += "&category1Id=" + args.Category1Id;
        urlParameters += "&category2Id=" + args.Category2Id;
        urlParameters += "&category3Id=" + args.Category3Id;
        urlParameters += "&category4Id=" + args.Category4Id;
        urlParameters += "&category5Id=" + args.Category5Id;
        urlParameters += "&collectorId=" + args.CollectorId;
        urlParameters += "&salesmanId=" + args.SalesmanId;
        urlParameters += "&isCustomer=" + args.IsCustomer;
        urlParameters += "&groupByDate=" + args.GroupByDate;
        urlParameters += "&forceUseMonthMethod=" + args.ForceUseMonthMethod;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetGLAccountsAgingReport?' + urlParameters, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToPeriodMList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            });
        });
    };
    GLAccountExtendedListService.prototype.GetTopDeptors = function (filter, accountTypeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTopDeptors?filterString=' + filter + '&accountTypeCode=' + accountTypeCode, {
                headers: authHeader
            }).map(function (response) {
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
            });
        });
    };
    GLAccountExtendedListService.prototype.SetParentAccountId = function (id, parentId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetParentAccountId?' + 'id=' + id + '&' + 'parentId=' + parentId, {
                headers: authHeader
            }).map(function (response) {
                var list = response.json();
                var entity;
                if (list) {
                    entity = _this.MapJsonToEntityList(list);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.GetAccountOpenTransactionsCount = function (accountId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetAccountOpenTransactionsCount?accountId=' + accountId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var result = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GLAccountExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new GLAccountList_1.GLAccountList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GLAccountExtendedListService.prototype.MapJsonToPeriodMList = function (jsonList) {
        var entityList;
        entityList = new PeriodM_1.PeriodM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GLAccountExtendedListService.prototype.MapJsonToGLAccountTotalByMonthEntityList = function (jsonList) {
        var entityList;
        entityList = new GLAccountTotalByMonthList_1.GLAccountTotalByMonthList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GLAccountExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GLAccountExtendedListService);
    return GLAccountExtendedListService;
}());
exports.GLAccountExtendedListService = GLAccountExtendedListService;
//# sourceMappingURL=GLAccountExtendedListService.js.map