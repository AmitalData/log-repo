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
var BankDepositExtendedPMService = /** @class */ (function () {
    function BankDepositExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/BankDeposit';
    }
    BankDepositExtendedPMService.prototype.returnCheque = function (bankDepositId, arpChequeId, returnType, notes) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + "/PostReturnCheque?"
                + "&bankDepositId=" + bankDepositId
                + "&arpChequeId=" + arpChequeId
                + "&returnType=" + returnType
                + "&notes=" + notes, "", { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    serviceResponse.Result = pm;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BankDepositExtendedPMService.prototype.cancelDeposit = function (bankDepositId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + "/PostCancelDeposit?"
                + "&bankDepositId=" + bankDepositId, "", { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    serviceResponse.Result = pm;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BankDepositExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], BankDepositExtendedPMService);
    return BankDepositExtendedPMService;
}());
exports.BankDepositExtendedPMService = BankDepositExtendedPMService;
//# sourceMappingURL=BankDepositExtendedPMService.js.map