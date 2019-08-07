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
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var LoginService_1 = require("../../Services/LoginService");
var http_1 = require("@angular/http");
var TenantManagementPMService_1 = require("../../Services/StandardPMs/TenantManagementPMService");
var Tools_1 = require("../../Tools");
var Environment_1 = require("../../Locators/Environment");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var BlockScreenComponent = /** @class */ (function () {
    function BlockScreenComponent(loginService) {
        var _this = this;
        this.loginService = loginService;
        this.BackToLoginCompleted = new core_1.EventEmitter();
        this.BlockMessagePart1 = "";
        this.BlockMessagePart2 = "";
        this.BlockMessagePart3 = "";
        this.BlockMessagePart4 = "";
        this.BlockMessagePart5 = "You can also manage your bluesnap account ";
        this.ExistManage = false;
        this.IsProduction = false;
        this.isCompanyAndUser = false;
        this.LogoURL = "./Images/LoginScreen/header.jpg";
        this.SampleLogoURL = "./Images/ApplicationLogo/Angular/AngularLogo.png";
        this.Email = "";
        this.authHeader = new http_1.Headers();
        this.authHeader.append('Content-Type', 'application/json');
        this.authHeader.append('Accept', 'application/json');
        this.authHeader.append('token', SessionInfo_1.SessionInfo.Token);
        this.loginService.AuthHeader = this.authHeader;
        if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount)) {
            this.ExistManage = true;
        }
        else {
            this.ExistManage = false;
        }
        //var temp = window.sessionStorage.getItem("LogoURL");
        //var LogoCode = window.sessionStorage.getItem("LogoCode");
        //if (temp) {
        //    this.LogoURL = temp;
        //    this.SampleLogoURL = temp;
        //}
        //else {
        this.loginService.GetGlobalSetting().subscribe(function (Setting) {
            if (Setting) {
                ObjectsLocator_1.ObjectsLocator.GlobalSetting = Setting;
                _this.loginService.GetTenantManagement().subscribe(function (TenantManagement) {
                    var myTenantManagementPMService = new TenantManagementPMService_1.TenantManagementPMService();
                    if (TenantManagement) {
                        var temptenant = myTenantManagementPMService.MapJsonToEntityPM(TenantManagement);
                        if (temptenant) {
                            _this.loginService.GetPrivateLableById(temptenant.PrivateLabelId).subscribe(function (Result) {
                                SessionLocator_1.SessionLocator.PrivateLableSettings = Result;
                                if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                                    _this.LogoURL = "data:image/JPEG;base64," + SessionLocator_1.SessionLocator.PrivateLableSettings.MainLogo;
                                    _this.SampleLogoURL = "data:image/JPEG;base64," + SessionLocator_1.SessionLocator.PrivateLableSettings.MainLogo;
                                }
                                else {
                                    _this.LogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                                    _this.SampleLogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                                }
                                _this.SetBlockMessage();
                            });
                        }
                        else {
                            _this.LogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                            _this.SampleLogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                            _this.SetBlockMessage();
                        }
                    }
                    else {
                        _this.LogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                        _this.SampleLogoURL = Tools_1.AppTool.GetEnvironmentLogo(Setting.LogoCode);
                        _this.SetBlockMessage();
                    }
                });
            }
            else {
                _this.LogoURL = "./Images/LoginScreen/header.jpg";
                _this.SampleLogoURL = "./Images/ApplicationLogo/Angular/AngularLogo.png";
                _this.SetBlockMessage();
            }
        });
        //}
        //this.SetBlockMessage();
    }
    BlockScreenComponent.prototype.GoToManage = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetBlueSnapToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
            var temp = myResult.Result;
            temp = temp.Token;
            _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
            var link = "https://cp.bluesnap.com/jsp/account_login.jsp";
            if (!Tools_1.AppTool.IsNullOrEmpty(temp)) {
                link = "https://ws.bluesnap.com/jsp/entrance.jsp?target=cp&token=" + temp + "&pageToShow=my_account.jsp";
            }
            var win = window.open(link, '_blank');
            win.focus();
        });
    };
    BlockScreenComponent.prototype.setCookie = function (name, value, expireDays, path) {
        if (path === void 0) { path = ''; }
        var d = new Date();
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        var expires = "expires=" + d.toUTCString();
        var cpath = path ? "; path=" + path : '';
        document.cookie = name + "=" + value + "; " + expires + cpath;
    };
    BlockScreenComponent.prototype.SetBlockMessage = function () {
        this.IsProduction = SessionLocator_1.SessionLocator.IsProduction;
        this.Email = "";
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.Email = SessionLocator_1.SessionLocator.PrivateLableSettings.ContactUsEmail;
        }
        else {
            var tempmail = Environment_1.Environment.GetContactUsEmail();
            if (tempmail) {
                this.Email = tempmail;
            }
            else {
                this.Email = "info@logitudeworld.com";
            }
        }
        if (SessionLocator_1.SessionLocator.BlockType == "company") {
            this.BlockMessagePart1 = "Your company subscription has expired.";
            this.BlockMessagePart2 = "To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "For more information and help please contact ";
        }
        else if (SessionLocator_1.SessionLocator.BlockType == "user") {
            this.BlockMessagePart1 = "Your temporary access has expired";
            this.BlockMessagePart2 = "To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "For more information and help please contact ";
        }
        else if (SessionLocator_1.SessionLocator.BlockType == "suspend") {
            this.BlockMessagePart1 = "Your company subscription has expired. The recurring renew has failed due to credit";
            this.BlockMessagePart2 = "card authorization error.To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "Please contact your e-commerce vendor or ";
        }
    };
    BlockScreenComponent.prototype.BackToLoginClicked = function () {
        this.BackToLoginCompleted.emit('true');
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BlockScreenComponent.prototype, "BackToLoginCompleted", void 0);
    BlockScreenComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BlockScreenComponent.html',
        }),
        __metadata("design:paramtypes", [LoginService_1.LoginService])
    ], BlockScreenComponent);
    return BlockScreenComponent;
}());
exports.BlockScreenComponent = BlockScreenComponent;
//# sourceMappingURL=BlockScreenComponent.js.map