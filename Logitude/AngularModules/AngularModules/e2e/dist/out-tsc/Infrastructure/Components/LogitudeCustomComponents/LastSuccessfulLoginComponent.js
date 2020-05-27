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
/// <reference path="../../../common/services/standardpms/userlastloginpmservice.ts" />
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var core_1 = require("@angular/core");
var UserLastLoginPMService_1 = require("../../../Common/Services/StandardPMs/UserLastLoginPMService");
var LastSuccessfulLoginComponent = /** @class */ (function () {
    function LastSuccessfulLoginComponent(_userLastLoginPMService) {
        this._userLastLoginPMService = _userLastLoginPMService;
    }
    LastSuccessfulLoginComponent.prototype.ngOnInit = function () {
        this.LastLoginData = SessionInfo_1.SessionInfo.LastLoginDateTime;
    };
    LastSuccessfulLoginComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LastSuccessfulLoginComponent',
            templateUrl: './LastSuccessfulLoginComponent.html',
            providers: [UserLastLoginPMService_1.UserLastLoginPMService]
        }),
        __metadata("design:paramtypes", [UserLastLoginPMService_1.UserLastLoginPMService])
    ], LastSuccessfulLoginComponent);
    return LastSuccessfulLoginComponent;
}());
exports.LastSuccessfulLoginComponent = LastSuccessfulLoginComponent;
//# sourceMappingURL=LastSuccessfulLoginComponent.js.map