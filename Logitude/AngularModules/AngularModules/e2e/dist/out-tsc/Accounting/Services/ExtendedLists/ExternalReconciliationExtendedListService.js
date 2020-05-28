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
var LedgerTransactionList_1 = require("../../EntityLists/LedgerTransactionList");
//import {ReconcileExternalPageLineList} from '../../EntityLists/ReconcileExternalPageLineList';
var ExternalReconciliationExtendedListService = /** @class */ (function () {
    function ExternalReconciliationExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExternalReconciliation';
    }
    ExternalReconciliationExtendedListService.prototype.getExternalAutomaticReconcilationsByFilter = function (amountReconcile, referenceReconcile, refDateReconcile, bankAccountId, glAccountId, filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + "/GetExternalAutomaticReconcilationsByFilter";
        var urlparameters = '?glAccountId=' + glAccountId
            + '&bankAccountId=' + bankAccountId
            + '&amountReconcile=' + amountReconcile
            + '&referenceReconcile=' + referenceReconcile
            + '&refDateReconcile=' + refDateReconcile;
        //#region Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        //#endregion End Parse
        var callUrl = url.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var result = new AutoSelectedExternalReconciliationLines();
                result = serviceResponse.Result.Result;
                //
                //var mappedTransactions: Array<LedgerTransactionList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result.transactionLines) {
                //        var list: LedgerTransactionList;
                //        list = this.MapJsonToEntityLedgerTransactionList(serviceResponse.Result[key]);
                //        mappedTransactions.push(list);
                //    }
                //}
                //result.transactionLines = mappedTransactions;
                //
                //var mappedPageLines: Array<ReconcileExternalPageLineList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result.pageLines) {
                //        var list: ReconcileExternalPageLineList;
                //        list = this.MapJsonToEntityReconcileExternalBankPageLineList(serviceResponse.Result[key]);
                //        mappedPageLines.push(list);
                //    }
                //}
                //result.pageLines = mappedPageLines;
                //
                result.Count = serviceResponse.Result.Count;
                console.log("[Result]", result);
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ExternalReconciliationExtendedListService.prototype.getGenerateTestRecordsForExternalReco = function (bankAccountId, glAccountId, type) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + "/GetGenerateTestRecordsForExternalReco";
        var urlparameters = '?glAccountId=' + glAccountId + '&bankAccountId=' + bankAccountId + '&type=' + type;
        var callUrl = url.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ExternalReconciliationExtendedListService.prototype.MapJsonToEntityLedgerTransactionList = function (jsonList) {
        var entityList;
        entityList = new LedgerTransactionList_1.LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ExternalReconciliationExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ExternalReconciliationExtendedListService);
    return ExternalReconciliationExtendedListService;
}());
exports.ExternalReconciliationExtendedListService = ExternalReconciliationExtendedListService;
var AutoSelectedExternalReconciliationLines = /** @class */ (function () {
    function AutoSelectedExternalReconciliationLines() {
    }
    return AutoSelectedExternalReconciliationLines;
}());
exports.AutoSelectedExternalReconciliationLines = AutoSelectedExternalReconciliationLines;
//# sourceMappingURL=ExternalReconciliationExtendedListService.js.map