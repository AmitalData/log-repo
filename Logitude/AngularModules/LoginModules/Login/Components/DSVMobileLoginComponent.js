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
var LoginService_1 = require("../LoginService");
var SessionInfo_1 = require("../SessionInfo");
var LoginComponent_1 = require("./LoginComponent");
var Tools_1 = require("../Utilities/Tools");
var DSVMobileLoginComponent = /** @class */ (function (_super) {
    __extends(DSVMobileLoginComponent, _super);
    function DSVMobileLoginComponent(ss) {
        var _this = _super.call(this, ss) || this;
        _this.ss = ss;
        return _this;
    }
    DSVMobileLoginComponent.prototype.ngOnInit = function () {
        console.log("ngOnInit");
        this.get_cookie_data();
    };
    DSVMobileLoginComponent.prototype.ClearLocation = function () {
        if (SessionInfo_1.SessionInfo.MainLocation) {
            SessionInfo_1.SessionInfo.MainLocation.clear();
        }
    };
    DSVMobileLoginComponent.prototype.ForgotPasswordClicked = function () {
        this.ClearLocation();
        Tools_1.Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo_1.SessionInfo.MainLocation)
            .then(function (cmpRef) {
        });
    };
    DSVMobileLoginComponent = __decorate([
        core_1.Component({
            selector: 'DSVMobileLoginComponent',
            moduleId: './Login/Components/',
            templateUrl: 'DSVMobileLoginComponent.html',
            styleUrls: ['DSVMobileLoginComponent.css']
        }),
        __metadata("design:paramtypes", [LoginService_1.LoginService])
    ], DSVMobileLoginComponent);
    return DSVMobileLoginComponent;
}(LoginComponent_1.LoginComponent));
exports.DSVMobileLoginComponent = DSVMobileLoginComponent;
//# sourceMappingURL=DSVMobileLoginComponent.js.map