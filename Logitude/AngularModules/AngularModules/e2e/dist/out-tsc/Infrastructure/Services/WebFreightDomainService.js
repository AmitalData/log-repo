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
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var TraceEventPM_1 = require("../EntityPMs/TraceEventPM");
var BIReportPM_1 = require("../EntityPMs/BIReportPM");
var WebFreightDomainService = /** @class */ (function () {
    function WebFreightDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/WebFreightDomain';
    }
    WebFreightDomainService.prototype.GetTraceEventsForEntity = function (objectTableId, entityId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain/GetTraceEventsForEntity?objectTableId=' + objectTableId + '&entityId=' + entityId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapTraceEventPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    WebFreightDomainService.prototype.InsertTraceEvent = function (entityId, objectTableId, eventTypeId, eventDate, notes) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var args = new TraceEventsServiceArgs();
        args.EntityId = entityId;
        args.ObjectTableId = objectTableId;
        args.EventTypeId = eventTypeId;
        args.EventDate = eventDate;
        args.Notes = notes;
        var mappedArgs = this.MapJsonTraceEventArgs(args);
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.post(url, JSON.stringify(mappedArgs), { headers: authHeader }).map(function (response) {
                var myJason = response.json();
                var myResult = _this.MapNewTraceEventResult(myJason);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    WebFreightDomainService.prototype.DeleteTraceEvent = function (entityId, objectTableId, traceEventId, isExternal) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var args = new TraceEventsServiceArgs();
        args.EntityId = entityId;
        args.ObjectTableId = objectTableId;
        args.TraceEventId = traceEventId;
        args.IsExternal = isExternal;
        var mappedArgs = this.MapJsonTraceEventArgs(args);
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TraceEventsDomain';
        return Rx_1.Observable.defer(function () {
            return _this._http.put(url + "/PutDeleteTraceEvent", JSON.stringify(mappedArgs), { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    WebFreightDomainService.prototype.MapJsonTraceEventArgs = function (json, args) {
        if (args === void 0) { args = null; }
        if (!args) {
            args = new TraceEventsServiceArgs();
        }
        var jsonPMKeys = Object.keys(json);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                args[property] = json[property];
            }
        }
        return args;
    };
    WebFreightDomainService.prototype.MapTraceEventPM = function (jsonList) {
        var entityPM;
        entityPM = new TraceEventPM_1.TraceEventPM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonList[property];
        }
        return entityPM;
    };
    WebFreightDomainService.prototype.MapNewTraceEventResult = function (jsonList) {
        var entityPM;
        entityPM = new NewTraceEventResult();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityPM[property] = jsonList[property];
        }
        return entityPM;
    };
    WebFreightDomainService.prototype.getExcelData = function (filters, queryId, tenant, userid, ObjectTableName) {
        var _this = this;
        if (filters == null) {
            filters.GetCount = true;
            filters.PageIndex = 0;
            filters.PageSize = 30;
            filters.SortBy = "";
            filters.SortDirection = "";
        }
        filters.queryId = queryId;
        filters.Tenant = tenant;
        filters.userid = userid;
        filters.ObjectTableName = ObjectTableName;
        var urlparameters = '/getquerytoexceldata?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var j = 1;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            //if (urlparameters != "?") {
            //    urlparameters = urlparameters.concat('&');
            //}
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            if (j < mykeys.length && !ignoreFilter) {
                urlparameters = urlparameters.concat('&');
            }
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
            j++;
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                if (response.ok == true) {
                    var viewResponse = response.json();
                    if (viewResponse != "Faild") {
                        return viewResponse;
                    }
                    else {
                        return "Faild";
                    }
                }
                else {
                    return "Faild";
                }
            });
        });
    };
    WebFreightDomainService.prototype.GetExportBIReportToExcel = function (QueryData) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(QueryData.BIReportPM, false);
                var temp = _this.deepClone(QueryData.DWQueryData);
                QueryData.BIReportPM = mappedEntity;
                QueryData.DWQueryData = temp;
                var temp2 = _this.deepClone(QueryData);
                /////////////////////////////////////////////////////
                return _this._http.put(_this._apiUrl + "/PutExportBIReportToExcel", JSON.stringify(temp2), { headers: authHeader }).map(function (res) {
                    var entity = res.json();
                    var serviceResponse;
                    serviceResponse = new ServiceResponse_1.ServiceResponse();
                    serviceResponse.Result = entity;
                    var servertime = res.headers.get('ServerExecutionTime');
                    return serviceResponse;
                });
            }
            else {
                return null;
            }
        });
    };
    WebFreightDomainService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new BIReportPM_1.BIReportPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    WebFreightDomainService.prototype.deepClone = function (obj, hash) {
        var _this = this;
        if (hash === void 0) { hash = new WeakMap(); }
        // Do not try to clone primitives or functions
        if (Object(obj) !== obj || obj instanceof Function) {
            return obj;
        }
        if (hash.has(obj)) {
            //return hash.get(obj); // Cyclic reference
            return;
        }
        try { // Try to run constructor (without arguments, as we don't know them)
            var result = new obj.constructor();
        }
        catch (e) { // Constructor failed, create object without running the constructor
            result = Object.create(Object.getPrototypeOf(obj));
        }
        // Optional: support for some standard constructors (extend as desired)
        if (obj instanceof Map) {
            Array.from(obj, function (_a) {
                var key = _a[0], val = _a[1];
                return result.set(_this.deepClone(key, hash), _this.deepClone(val, hash));
            });
        }
        else if (obj instanceof Set) {
            Array.from(obj, function (key) { return result.add(_this.deepClone(key, hash)); });
        }
        // Register in hash    
        hash.set(obj, result);
        // Clone and assign enumerable own properties recursively
        return Object.assign.apply(Object, [result].concat(Object.keys(obj).map(function (key) {
            var _a;
            return (_a = {},
                _a[key] = key != "UIProperties" && key != "MyParentClass" ? _this.deepClone(obj[key], hash) : true,
                _a);
        })));
    };
    WebFreightDomainService.prototype.getHypridPartnerLogo = function (logoId) {
        var _this = this;
        var urlparameters = '/GetHypridPartnerLogo?LogoId=' + logoId;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                return response.json();
            });
        });
    };
    WebFreightDomainService.prototype.DownLoadAllFilesForShipments = function (ShipmentId, ObjectTableId, tenant) {
        var _this = this;
        var urlparameters = '/DownLoadAllFilesForShipments?ShipmentId=' + ShipmentId + "&&ObjectTableId=" + ObjectTableId + "&&Tenant=" + tenant;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters); //
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                return response.json();
            });
        });
    };
    WebFreightDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], WebFreightDomainService);
    return WebFreightDomainService;
}());
exports.WebFreightDomainService = WebFreightDomainService;
var TraceEventsServiceArgs = /** @class */ (function () {
    function TraceEventsServiceArgs() {
        this.EventDate = null;
        this.IsExternal = false;
    }
    return TraceEventsServiceArgs;
}());
exports.TraceEventsServiceArgs = TraceEventsServiceArgs;
var NewTraceEventResult = /** @class */ (function () {
    function NewTraceEventResult() {
    }
    return NewTraceEventResult;
}());
exports.NewTraceEventResult = NewTraceEventResult;
//# sourceMappingURL=WebFreightDomainService.js.map