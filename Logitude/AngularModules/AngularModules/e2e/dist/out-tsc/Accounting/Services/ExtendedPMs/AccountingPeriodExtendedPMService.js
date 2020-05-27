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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
//import {AccountingPeriodLinePM} from '../../EntityPMs/AccountingPeriodLinePM';
//import {AccountingPeriodValidator} from '../../Validators/AccountingPeriodValidator';
var AccountingPeriodExtendedPMService = /** @class */ (function () {
    function AccountingPeriodExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/accountingPeriods';
    }
    AccountingPeriodExtendedPMService.prototype.createDefaultPeriods = function (year) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + "/PostCreatePeriodsForYear?year=" + year, null, { headers: authHeader }).map(function (res) {
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AccountingPeriodPM = null) {
    //    if (!entityPM) {
    //        entityPM = new AccountingPeriodPM();
    //    }
    //    var jsonPMKeys = Object.keys(jsonPM);
    //    for (var key in jsonPMKeys) {
    //        if (jsonPMKeys[key] === "UIProperties") {
    //            continue;
    //        }
    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];
    //    }
    //    return entityPM;
    //}
    AccountingPeriodExtendedPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    AccountingPeriodExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], AccountingPeriodExtendedPMService);
    return AccountingPeriodExtendedPMService;
}());
exports.AccountingPeriodExtendedPMService = AccountingPeriodExtendedPMService;
//# sourceMappingURL=AccountingPeriodExtendedPMService.js.map