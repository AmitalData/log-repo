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
var LoginService_1 = require("../LoginService");
var SessionInfo_1 = require("../SessionInfo");
var Tools_1 = require("../Utilities/Tools");
var LoginComponent = /** @class */ (function () {
    //private _objectTableRulePMService: ObjectTableRulePMService = new ObjectTableRulePMService();
    //private _objectTableRuleFieldPMService: ObjectTableRuleFieldPMService = new ObjectTableRuleFieldPMService();
    function LoginComponent(loginService //, public IndexedDbService: IndexedDbService, private entityResourceService: EntityResourceService, private _applicationTimersManager: ApplicationTimersManager, public entityListService: EntityListService,
    //private _userLastLoginPMService: UserLastLoginPMService
    ) {
        this.loginService = loginService;
        this.IsShowTenantList = false;
        this.IsProduction = false;
        this.ShowLoadingIndicator = false;
        this.LogoURL = "./Images/LoginScreen/header.jpg";
        this.SampleLogoURL = "./Images/ApplicationLogo/Angular/AngularLogo.png";
        this.PasswordImage = "./Images/LoginScreen/password_eye_closed.png";
        this.PasswordTitle = "Show";
        this.PasswordWidth = 280;
        this.IsShowPasswordExpirationDateArea = false;
        this.IsShowFormLogin = false;
        this.IsHaveTenantInUrl = false;
        this.InputPasswordType = "password";
        this.ShowTenantList = false;
        this.errorMessage = "";
        this.cookie_name = "email_cookie"; // added 
        this.expdays = 365;
        this.TotalNumberOfLoads = 0;
        this.LoadSize = 0;
        this.LastLoadSize = 0;
        this.LoadingCounter = 0;
        this.CompletedLoadsCount = 0;
        //this.IsProduction = SessionLocator.IsProduction;
        //this.authHeader = new Headers();
        //this.authHeader.append('Content-Type', 'application/json');
        //this.authHeader.append('Accept', 'application/json');
        //this.loginService.AuthHeader = this.authHeader;
        this.ShowLoginBusyIndicator = false;
        this.HideLoginForm = false;
        this.HideTenantForm = true;
        this.LoginFailed = false;
        this.LoginParams = new LoginService_1.LoginParameters();
        this.HidePendingLoading = true;
        //var temp = window.sessionStorage.getItem("LogoURL");
        //var LogoCode = window.sessionStorage.getItem("LogoCode");
        //if (temp) {
        //    this.LogoURL = temp;
        //    this.SampleLogoURL = temp;
        //}
        //else if (LogoCode) {
        //    this.LogoURL = AppTool.GetEnvironmentLogo(LogoCode);
        //    this.SampleLogoURL = AppTool.GetEnvironmentLogo(LogoCode);
        //}
        //window.Statuses = [];
        //window.Ports = [];
        //window.TransportModes = [];
        //window.Directions = [];
        //window.Cards = [];
        //window.MenusTables = [];
        //window.TextCodesTranslations = [];
        //window.ObjectTables = [];
        //window.Screens = [];
        //window.ScreenFields = [];
        //window.ObjectTableTabs = [];
        //window.ObjectFields = [];
        //window.PreDefinedFilters = [];
        //window.TenantTranslations = [];
        //window.TenantLanguageTranslations = [];
        //window.ObjectTableRules = [];
        //window.ObjectTableRuleFields = [];
        //window.ObjectTableRules = [];
        //window.ObjectTables = [];
        //window.CachedTables = [];
        //window.TranslationsCache = [];
        //window.TextCodes = [];
        //window.TextCodesCache = [];
        //window.ObjectFieldsCache = [];
        //window.Tips = [];
        //window.TipsVisibilities = [];
        this.LoginWithToken();
    }
    LoginComponent.prototype.ngOnInit = function () {
        //this.Email = "angular@fnarsoft.com";
        //this.Password = "1";
        //this.authHeader = new Headers();
        //this.authHeader.append('Content-Type', 'application/json');
        //this.authHeader.append('Accept', 'application/json');
        //this.loginService.AuthHeader = this.authHeader;
        window.indexedDB.deleteDatabase("MyDatabase");
        var data = window.sessionStorage.getItem('userdata');
        console.log("ngOnInit");
        this.get_cookie_data();
        //if (SessionLocator.IsExternalParams) {
        //    if (SessionLocator.ExternalParams && SessionLocator.ExternalParams.OneTimePasswordId) {
        //        this.HideLoginForm = true;
        //        this.HideTenantForm = true;
        //        this.ShowLoginBusyIndicator = true;
        //        this.OneUsePasswordMethod();
        //    }
        //    else {
        //        document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
        //    }
        //}
        //else
        if (data) {
            this.HideLoginForm = true;
            this.HideTenantForm = true;
            this.HidePendingLoading = true;
            this.ShowLoginBusyIndicator = true;
            var userData = JSON.parse(data);
            this.StartLoading(userData);
        }
        //this.idxdb = window.indexedDB.open("mydb", 1);
        //this.indexDB = new AngularIndexedDB("mydb", 2);
        //this.indexDB.createStore(2, (evt) => {
        //    let objectStore = evt.currentTarget.result.createObjectStore(
        //        'ObjectFields', { keyPath: "Id", unique: true });
        //    objectStore.createIndex("Id", "Id", { unique: true });
        //});
        //this.indexDB.add("ObjectFields", "3242", { Id: "3242", FieldName: "Name", IsRequired: true }).then((res) => { console.log("successfully", res); });
    };
    LoginComponent.prototype.LoginWithToken = function () {
        var _this = this;
        var externalTenant = null;
        var url = window.location.href;
        if (url) {
            var args = url.split('&');
            if (args[1] && args[1].indexOf('Tenant=') != -1) {
                externalTenant = args[1].split('=')[1];
            }
        }
        if (externalTenant) {
            this.IsHaveTenantInUrl = true;
            var tokenKey = externalTenant ? "Token_" + externalTenant : "Token";
            var token = window.localStorage.getItem(tokenKey);
            if (token) {
                var loginTokenParameter = new LoginService_1.LoginTokenParameter();
                loginTokenParameter.Token = token;
                this.loginService.LoginUsingAuthenticaionToken(loginTokenParameter).subscribe(function (userData) {
                    if (!userData.HasError) {
                        var data = JSON.stringify(userData);
                        window.sessionStorage.setItem("userdata", data);
                        var mypageUrl = window.location.href;
                        var AngularURL = "";
                        var urlMenu = "";
                        if (userData.HtmlVersion) {
                            var version = userData.HtmlVersion;
                            AngularURL = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Angular" + version + "/index.html";
                        }
                        else {
                            AngularURL = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Angular/index.html";
                        }
                        if (mypageUrl && mypageUrl.indexOf("Menu=") > -1) {
                            urlMenu = mypageUrl.split("Menu=")[1];
                            AngularURL += ("?Menu=" + urlMenu);
                        }
                        document.location.href = AngularURL;
                    }
                    else {
                        _this.IsShowFormLogin = true;
                    }
                });
            }
            else {
                this.IsShowFormLogin = true;
            }
        }
        else {
            this.IsShowFormLogin = true;
            this.IsHaveTenantInUrl = false;
        }
    };
    LoginComponent.prototype.OnMouseDownEvt = function (event) {
        var key = event.keyCode;
        var ENTER = 13;
        if (key == ENTER) {
            this.LoginClicked();
        }
    };
    LoginComponent.prototype.StartLoading = function (userData) {
        console.log("StartLoading");
        if (userData) {
            this.HideLoginForm = true;
            this.HideTenantForm = true;
            this.HidePendingLoading = true;
            this.ShowLoginBusyIndicator = true;
            SessionInfo_1.SessionInfo.LoggedUserEmail = userData.UserName;
            SessionInfo_1.SessionInfo.LoggedUserId = userData.Id;
            SessionInfo_1.SessionInfo.Token = userData.Token;
            this.authHeader.append('token', userData.Token);
            if (userData.CurrentTenant != null) {
                SessionInfo_1.SessionInfo.LoggedUserTenant = Number(userData.CurrentTenant + "");
            }
            if (SessionInfo_1.SessionInfo.LoggedUserTenant != null) {
                this.loginService.AuthHeader = this.authHeader;
                this.loginService.CurrentTenant = userData.CurrentTenant;
                this.loginService.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                this.loginService.LoggedUserEmail = SessionInfo_1.SessionInfo.LoggedUserEmail;
            }
        }
        window.sessionStorage.setItem("userdata", "");
    };
    LoginComponent.prototype.onEmailBlur = function (email) {
        if (email != this.Email) {
            this.IsShowAreaCaptcha = false;
            this.CaptchaKey = null;
            this.CaptchaTextValue = null;
            if (this.errorMessage == "Please re-enter the characters you see in the image above") {
                this.errorMessage = "";
            }
        }
    };
    LoginComponent.prototype.ShowHidePasswordClick = function () {
        var showHidePasswordImage = document.getElementById("ShowHidePasswordImageId");
        if (showHidePasswordImage) {
            this.PasswordImage = this.PasswordImage == "./Images/LoginScreen/password_eye.png" ? "./Images/LoginScreen/password_eye_closed.png" : "./Images/LoginScreen/password_eye.png";
            this.InputPasswordType = this.InputPasswordType == "password" ? "text" : "password";
            this.PasswordTitle = this.PasswordTitle == "Show" ? "Hide" : "Show";
        }
    };
    LoginComponent.prototype.PasswordExpirationButtomClicked = function (type) {
        if (type == "Yes") {
            SessionInfo_1.SessionInfo.LoggedUserEmail = this.UserDataPrompt.UserName;
            if (SessionInfo_1.SessionInfo.MainLocation) {
                SessionInfo_1.SessionInfo.MainLocation.clear();
            }
            Tools_1.Tools.DynamicLoader.Load("./Login/Components/" + SessionInfo_1.SessionInfo.PlShortName + "ChangePasswordComponent", SessionInfo_1.SessionInfo.MainLocation)
                .then(function (cmpRef) {
            });
        }
        else if (type == "No") {
            this.ComplateProcessLogin(this.UserDataPrompt, this.LoginParameters);
        }
    };
    LoginComponent.prototype.ComplateProcessLogin = function (userData, loginParameters) {
        this.TenantList = userData.CompanyLogins;
        this.HideLoginForm = true;
        this.HideTenantForm = false;
        this.HidePendingLoading = false;
        this.LoginParams = loginParameters;
        if (this.TenantList.length === 1) {
            this.HideTenantForm = true;
            this.Tenant = this.TenantList[0].Tenant;
            this.loginService.CurrentTenant = this.Tenant;
            var f = { valid: true };
            this.ChooseTenant(f, null);
        }
        else {
            //var i = 0;
            //this.TenantList.forEach((item) => {
            //    i += 1;
            //    item.Id = i;
            //});
            //this.SelectedCompany = this.TenantList[0];
            this.ShowTenantList = true;
            this.HidePendingLoading = true;
            this.HideTenantForm = true;
            showTenantsCombo(userData.CompanyLogins);
        }
    };
    LoginComponent.prototype.LoginClicked = function () {
        if (!this.Email || !this.Password) {
            this.errorMessage = "Login failed! invalid user name or password.";
            return;
        }
        if (this.IsShowAreaCaptcha && !this.CaptchaTextValue) {
            this.errorMessage = "Please re-enter the characters you see in the image above";
            return;
        }
        if (IsBrowserSupported() == false) {
            alert("This Browser is not supported in HTML5 version, please use Chrome, Firefox or Opera.");
        }
        else {
            this.SaveDataToCookie();
            this.loginService.LoggedUserEmail = SessionInfo_1.SessionInfo.LoggedUserEmail;
            this.ShowLoadingIndicator = true;
            this.LoginParams = {
                Email: this.Email,
                Password: this.Password,
                ByToken: false,
                CardId: "",
                CardType: "",
                IsMobileLogin: false,
                IsUser: true,
                GetToken: true,
                IsAngularLogin: true,
                MobileVersion: "",
                ClientType: "Web",
                CaptchaKey: this.CaptchaKey,
                CaptchaCode: this.CaptchaTextValue,
            };
            this.HidePendingLoading = false;
            this.PostUserValidation(this.LoginParams);
        }
    };
    LoginComponent.prototype.PostUserValidation = function (loginParameters) {
        var _this = this;
        this.errorMessage = "";
        this.loginService.PostUserValidation(loginParameters).subscribe(function (userData) {
            _this.ShowLoadingIndicator = false;
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                _this.LoginFailed = true;
                _this.HidePendingLoading = true;
                _this.CaptchaKey = userData ? userData.CaptchaKey : "";
                if (userData && userData.ExceptionMessage) {
                    alert(userData.ExceptionMessage);
                }
                if (userData.MustChangePassword) {
                    SessionInfo_1.SessionInfo.LoggedUserEmail = userData.UserName;
                    if (SessionInfo_1.SessionInfo.MainLocation) {
                        SessionInfo_1.SessionInfo.MainLocation.clear();
                    }
                    Tools_1.Tools.DynamicLoader.Load("./Login/Components/" + SessionInfo_1.SessionInfo.PlShortName + "ChangePasswordComponent", SessionInfo_1.SessionInfo.MainLocation)
                        .then(function (cmpRef) {
                    });
                }
                else if (userData.PasswordExpirationDateMessage) {
                    if (userData.PasswordExpirationDateMessage.indexOf('days. Do') > -1) {
                        _this.PasswordExpirationDateMessage = userData.PasswordExpirationDateMessage.split('days. Do')[0] + "days. Do";
                        _this.PasswordExpirationDateMessage2 = userData.PasswordExpirationDateMessage.split('days. Do')[1];
                    }
                    else {
                        _this.PasswordExpirationDateMessage = userData.PasswordExpirationDateMessagel;
                    }
                    _this.UserDataPrompt = userData;
                    _this.LoginParameters = loginParameters;
                    _this.IsShowPasswordExpirationDateArea = true;
                }
                else {
                    _this.errorMessage = "";
                    if (userData.InValidCaptcha) {
                        if (_this.IsShowAreaCaptcha) {
                            _this.CaptchaTextValue = "";
                        }
                        _this.IsShowAreaCaptcha = true;
                        _this.CaptchaImageUrl = userData.CaptchaImage;
                    }
                    _this.errorMessage = "";
                    if (userData.IpRestricted)
                        _this.errorMessage = "Trying to log in from unauthorised station!" + " (The IP address you are trying to " + " log in from is restricted for this user)";
                    else if (userData.InActive)
                        _this.errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                    else if (userData.Unlicensed)
                        _this.errorMessage = "Your account is unlicensed!" + " please contact your administrator.";
                    else if (userData.InValidMailOrPassword)
                        _this.errorMessage = "Login failed! invalid user name or password.";
                    else if (userData.InValidCaptcha && userData.CaptchaImage)
                        _this.errorMessage = "Please re-enter the characters you see in the image above";
                    else
                        _this.errorMessage = "Login failed! invalid user name or password." + "<br/>";
                }
            }
            else {
                _this.ComplateProcessLogin(userData, loginParameters);
            }
            _this.HidePendingLoading = true;
        });
    };
    LoginComponent.prototype.TenantListChangeSelected = function (value) {
        this.SelectedCompany = this.TenantList.filter(function (d) { return d.Id == value; })[0];
    };
    LoginComponent.prototype.ContinueClicked = function () {
        this.SelectedCompany = getselectedcompany();
        if (this.SelectedCompany) {
            this.Tenant = this.SelectedCompany.Tenant;
            this.LoginParams = {
                Email: this.SelectedCompany.Email,
                Password: this.Password,
                ByToken: false,
                CardId: this.SelectedCompany.CardId,
                CardType: this.SelectedCompany.CardType,
                IsMobileLogin: false,
                IsUser: this.SelectedCompany.IsUser,
                GetToken: true,
                IsAngularLogin: true,
                MobileVersion: "",
                ClientType: "Web",
                CaptchaKey: this.CaptchaKey,
                CaptchaCode: this.CaptchaTextValue,
            };
            this.loginService.CurrentTenant = this.Tenant;
            var f = { valid: true };
            this.ChooseTenant(f, null);
        }
    };
    LoginComponent.prototype.ChooseTenant = function (f, values) {
        if (f.valid) {
            this.PostLoginData();
            this.HidePendingLoading = false;
        }
    };
    LoginComponent.prototype.PostLoginData = function () {
        var _this = this;
        this.loginService.PostLoginData(this.LoginParams).subscribe(function (userData) {
            if (userData && !userData.HasError) {
                var data = JSON.stringify(userData);
                window.sessionStorage.setItem("userdata", data);
                var mypageUrl = window.location.href;
                var AngularURL = "";
                var urlMenu = "";
                //userData.KeepUserLoggedIn == true &&
                if (userData.Token && _this.IsHaveTenantInUrl) {
                    window.localStorage.setItem("Token_" + userData.CurrentTenant, userData.Token);
                }
                var pageUrl = window.location.href;
                var externalTenant = null;
                if (pageUrl) {
                    var args = pageUrl.split('&');
                    if (args[1] && args[1].indexOf('Tenant=') != -1) {
                        externalTenant = args[1].split('=')[1];
                    }
                }
                if (userData.HtmlVersion) {
                    var version = userData.HtmlVersion;
                    AngularURL = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Angular" + version + "/index.html";
                }
                else {
                    AngularURL = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Angular/index.html";
                }
                if (mypageUrl && mypageUrl.indexOf("Menu=") > -1) {
                    urlMenu = mypageUrl.split("Menu=")[1];
                    AngularURL += ("?Menu=" + urlMenu);
                    if (externalTenant) {
                        AngularURL = AngularURL.replace("&Tenant=" + externalTenant, "");
                    }
                }
                document.location.href = AngularURL;
            }
        });
    };
    LoginComponent.prototype.SaveDataToCookie = function () {
        var expdate = new Date();
        expdate.setTime(expdate.getTime() + (this.expdays * 24 * 60 * 60 * 1000)); // expiry date 
        if (this.Email == "") {
            return;
        }
        this.set_cookie(this.cookie_name, this.Email, expdate);
    };
    LoginComponent.prototype.set_cookie = function (name, value, expires) {
        if (!expires) {
            expires = new Date();
        }
        document.cookie = name + "=" + encodeURI(value) +
            ((expires == null) ? "" : "; expires=" + expires.toGMTString());
    };
    LoginComponent.prototype.get_cookie = function (name) {
        var arg = name + "=";
        var alen = arg.length;
        var clen = document.cookie.length;
        var i = 0;
        while (i < clen) {
            var j = i + alen;
            if (document.cookie.substring(i, j) == arg) {
                return this.get_cookie_val(j);
            }
            i = document.cookie.indexOf(" ", i) + 1;
            if (i == 0)
                break;
        }
        return null;
    };
    LoginComponent.prototype.get_cookie_val = function (offset) {
        var endstr = document.cookie.indexOf(";", offset);
        if (endstr == -1)
            endstr = document.cookie.length;
        return decodeURI(document.cookie.substring(offset, endstr));
    };
    LoginComponent.prototype.get_cookie_data = function () {
        var inf = this.get_cookie(this.cookie_name);
        if (!inf) {
            return;
        }
        this.Email = inf;
        //get_update_date();
    };
    LoginComponent.prototype.BackToLoginClicked = function () {
        document.location.href = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Login.aspx";
    };
    LoginComponent = __decorate([
        core_1.Component({
            selector: 'LoginComponent',
            moduleId: './Login/Components/',
            templateUrl: 'LoginComponent.html',
            styleUrls: ['LoginComponent.css']
        }),
        __metadata("design:paramtypes", [LoginService_1.LoginService //, public IndexedDbService: IndexedDbService, private entityResourceService: EntityResourceService, private _applicationTimersManager: ApplicationTimersManager, public entityListService: EntityListService,
            //private _userLastLoginPMService: UserLastLoginPMService
        ])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=LoginComponent.js.map