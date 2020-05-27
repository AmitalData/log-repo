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
var TermsofUseSignaturePM_1 = require("../../EntityPMs/TermsofUseSignaturePM");
var TermsofUseSignatureExtendedPM = /** @class */ (function () {
    function TermsofUseSignatureExtendedPM() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TermsOfUseSignatures';
    }
    TermsofUseSignatureExtendedPM.prototype.GetTermsofUseSignatures = function (tenant, contactId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&contactId=' + contactId, { headers: authHeader }).map(function (response) {
            // var result = this.MapJsonToEntityPM(response.json()); 
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    TermsofUseSignatureExtendedPM.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new TermsofUseSignaturePM_1.TermsofUseSignaturePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    TermsofUseSignatureExtendedPM = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], TermsofUseSignatureExtendedPM);
    return TermsofUseSignatureExtendedPM;
}());
exports.TermsofUseSignatureExtendedPM = TermsofUseSignatureExtendedPM;
//# sourceMappingURL=TermsofUseSignatureExtendedPM.js.map