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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var QuoteTemplateDetailsFieldPM_1 = require("../../EntityPMs/QuoteTemplateDetailsFieldPM");
var QuoteTemplateDetailsFieldExtendedPMService = /** @class */ (function () {
    function QuoteTemplateDetailsFieldExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateDetailsFieldExtended';
    }
    QuoteTemplateDetailsFieldExtendedPMService.prototype.GetQuoteTemplateDetailsFieldByQuoteTemplateId = function (quoteTemplateId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateDetailsFieldByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var quoteTemplateDetailsFieldPMLists;
            quoteTemplateDetailsFieldPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                quoteTemplateDetailsFieldPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = quoteTemplateDetailsFieldPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateDetailsFieldExtendedPMService.prototype.updateDetailsFields = function (quoteTemplateDetailsFields) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.put(_this._apiUrl + '/PutQuoteTemplateDetailsFields', JSON.stringify(quoteTemplateDetailsFields), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QuoteTemplateDetailsFieldExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new QuoteTemplateDetailsFieldPM_1.QuoteTemplateDetailsFieldPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    QuoteTemplateDetailsFieldExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateDetailsFieldExtendedPMService);
    return QuoteTemplateDetailsFieldExtendedPMService;
}());
exports.QuoteTemplateDetailsFieldExtendedPMService = QuoteTemplateDetailsFieldExtendedPMService;
//# sourceMappingURL=QuoteTemplateDetailsFieldExtendedPMService.js.map