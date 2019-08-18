"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var IndexedDbService_1 = require("../../../Services/IndexedDbService");
var EntityResourceService_1 = require("../../../Services/EntityResourceService");
var ApplicationTimersManager_1 = require("../../../Utilities/ApplicationTimersManager");
var EntityListService_1 = require("../../../Services/EntityListService");
var LoginService_1 = require("../../../Services/LoginService");
var LogitudeApplicationService_1 = require("../../../Services/WebServices/LogitudeApplicationService");
var UserLastLoginPMService_1 = require("../../../../Common/Services/StandardPMs/UserLastLoginPMService");
var LoginComponent_1 = require("../LoginComponent");
var DSVMobileLoginProcessComponent = /** @class */ (function (_super) {
    __extends(DSVMobileLoginProcessComponent, _super);
    function DSVMobileLoginProcessComponent(mylogitudeApplicationService, myloginService, myIndexedDbService, myentityResourceService, _myapplicationTimersManager, myentityListService, _myuserLastLoginPMService) {
        var _this = _super.call(this, mylogitudeApplicationService, myloginService, myIndexedDbService, myentityResourceService, _myapplicationTimersManager, myentityListService, _myuserLastLoginPMService) || this;
        _this.mylogitudeApplicationService = mylogitudeApplicationService;
        _this.myloginService = myloginService;
        _this.myIndexedDbService = myIndexedDbService;
        _this.myentityResourceService = myentityResourceService;
        _this._myapplicationTimersManager = _myapplicationTimersManager;
        _this.myentityListService = myentityListService;
        _this._myuserLastLoginPMService = _myuserLastLoginPMService;
        return _this;
    }
    DSVMobileLoginProcessComponent.prototype.ngOnInit = function () {
        this.StartLoginProcess();
    };
    DSVMobileLoginProcessComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DSVMobileLoginProcessComponent.html',
            providers: [ApplicationTimersManager_1.ApplicationTimersManager, LogitudeApplicationService_1.LogitudeApplicationService, UserLastLoginPMService_1.UserLastLoginPMService]
        }),
        __metadata("design:paramtypes", [LogitudeApplicationService_1.LogitudeApplicationService, LoginService_1.LoginService, IndexedDbService_1.IndexedDbService, EntityResourceService_1.EntityResourceService, ApplicationTimersManager_1.ApplicationTimersManager, EntityListService_1.EntityListService,
            UserLastLoginPMService_1.UserLastLoginPMService])
    ], DSVMobileLoginProcessComponent);
    return DSVMobileLoginProcessComponent;
}(LoginComponent_1.LoginComponent));
exports.DSVMobileLoginProcessComponent = DSVMobileLoginProcessComponent;
//# sourceMappingURL=DSVMobileLoginProcessComponent.js.map