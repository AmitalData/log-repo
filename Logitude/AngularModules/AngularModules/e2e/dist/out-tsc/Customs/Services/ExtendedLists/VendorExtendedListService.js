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
var ImporterDespositionClass_1 = require("../../DataContract/ImporterDespositionClass");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var VendorExtendedListService = /** @class */ (function () {
    function VendorExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/VendorExtended';
    }
    VendorExtendedListService.prototype.GetVendorsWithImporterDespositions = function (vendorId, importerId, ShowOnlyValid, useImporterFilter, searchText) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetVendorsWithImporterDespositions';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetVendorsWithImporterDespositions/?' + 'vendorId=' + vendorId + '&importerId=' + importerId + '&ShowOnlyValid=' + ShowOnlyValid + '&useImporterFilter=' + useImporterFilter + '&searchText=' + searchText, { headers: authHeader }).map(function (response) {
                var serviceResponse = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToImporterDesposition(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VendorExtendedListService.prototype.MapJsonToImporterDesposition = function (json, mapParent, entity) {
        if (mapParent === void 0) { mapParent = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new ImporterDespositionClass_1.ImporterDespositionClass();
        }
        var jsonPMKeys = Object.keys(json);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }
        //  entity.IsDirty = false;
        return entity;
    };
    VendorExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], VendorExtendedListService);
    return VendorExtendedListService;
}());
exports.VendorExtendedListService = VendorExtendedListService;
//# sourceMappingURL=VendorExtendedListService.js.map