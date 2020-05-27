"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var SupplierInvoiceItemList_1 = require("../../EntityLists/Extended/SupplierInvoiceItemList");
var SupplierInvoiceExtendedListService = /** @class */ (function () {
    function SupplierInvoiceExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/SupplierInvoice';
    }
    SupplierInvoiceExtendedListService.prototype.GetSupplierInvoiceItemsForInvoice = function (declarationId, counterkey) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetSupplierInvoiceItemsForInvoice';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSupplierInvoiceItemsForInvoice/?' + 'declarationId=' + declarationId + '&counterkey=' + counterkey, { headers: authHeader }).map(function (response) {
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
    SupplierInvoiceExtendedListService.prototype.GetSupplierInvoiceItemsForInvoices = function (declarationId, supplierInvoiceCounterKeys, skip, take, getCount) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetSupplierInvoiceItemsForInvoices';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSupplierInvoiceItemsForInvoices/?' + 'declarationId=' + declarationId + '&supplierInvoiceCounterKeys=' + supplierInvoiceCounterKeys + '&skip=' + skip + '&take=' + take + '&getCount=' + getCount, { headers: authHeader }).map(function (response) {
                //var serviceResponse: ServiceResponse = new ServiceResponse();
                var serviceResponse = response.json();
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
    SupplierInvoiceExtendedListService.prototype.GetSelectedSupplierInvoiceItemLists = function (declarationId, supplierInvoiceCounterKeys, lineNumbers) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetSelectedSupplierInvoiceItems';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSelectedSupplierInvoiceItems/?' + 'declarationId=' + declarationId + '&supplierInvoiceCounterKeys=' + supplierInvoiceCounterKeys + '&lineNubmers=' + lineNumbers, { headers: authHeader }).map(function (response) {
                //var serviceResponse: ServiceResponse = new ServiceResponse();
                var serviceResponse = response.json();
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
    SupplierInvoiceExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new SupplierInvoiceItemList_1.SupplierInvoiceItemList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    return SupplierInvoiceExtendedListService;
}());
exports.SupplierInvoiceExtendedListService = SupplierInvoiceExtendedListService;
//# sourceMappingURL=SupplierInvoiceExtendedListService.js.map