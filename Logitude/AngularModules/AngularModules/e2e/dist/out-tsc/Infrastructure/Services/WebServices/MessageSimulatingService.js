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
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var MessageSimulatingService = /** @class */ (function () {
    function MessageSimulatingService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/MessageSimulating';
    }
    MessageSimulatingService.prototype.Simulate = function (args) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapSimulatorArgs(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = new SimulatorResult();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    MessageSimulatingService.prototype.MapSimulatorArgs = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new SimulatorArgs();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    MessageSimulatingService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], MessageSimulatingService);
    return MessageSimulatingService;
}());
exports.MessageSimulatingService = MessageSimulatingService;
var SimulatorArgs = /** @class */ (function () {
    function SimulatorArgs() {
    }
    return SimulatorArgs;
}());
exports.SimulatorArgs = SimulatorArgs;
var SimulatorFVA = /** @class */ (function () {
    function SimulatorFVA() {
        this.ScheduleInformations = [];
    }
    return SimulatorFVA;
}());
exports.SimulatorFVA = SimulatorFVA;
var FVASimulatorScheduleInformation = /** @class */ (function () {
    function FVASimulatorScheduleInformation() {
    }
    return FVASimulatorScheduleInformation;
}());
exports.FVASimulatorScheduleInformation = FVASimulatorScheduleInformation;
var SimulatorResult = /** @class */ (function () {
    function SimulatorResult() {
    }
    return SimulatorResult;
}());
exports.SimulatorResult = SimulatorResult;
//# sourceMappingURL=MessageSimulatingService.js.map