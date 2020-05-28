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
var DynamicLoader_1 = require("./Utilities/DynamicLoader");
var ServiceHelper_1 = require("./Utilities/ServiceHelper");
var SessionLocator_1 = require("./Utilities/SessionLocator");
var ExternalParams_1 = require("./Utilities/ExternalParams");
var TermsofUseService_1 = require("./Services/WebServices/TermsofUseService");
var SessionInfo_1 = require("./Utilities/SessionInfo");
var environment_1 = require("../environments/environment");
var LoginService_1 = require("./Services/LoginService");
var Tools_1 = require("./Tools");
var RootComponent = /** @class */ (function () {
    function RootComponent() {
        this.isComponentBooted = false;
        this.isComponentInited = false;
        this.isDSV = false;
        this._FinishLogin = false;
        var data = window.sessionStorage.getItem('userdata');
        if (data != "SignOut") {
            this.BuildExternalParams();
        }
    }
    RootComponent.prototype.Boot = function (args) {
        ServiceHelper_1.ServiceHelper.Http = args["Http"];
        SessionLocator_1.SessionLocator.Http = args["Http"];
        DynamicLoader_1.DynamicLoader.Compiler = args["Compiler"];
        DynamicLoader_1.DynamicLoader.Resolver = args["Resolver"];
        DynamicLoader_1.DynamicLoader.Injector = args["Injector"];
        DynamicLoader_1.DynamicLoader.ModuleLoader = args["ModuleLoader"];
        SessionLocator_1.SessionLocator.DynamicLoader = DynamicLoader_1.DynamicLoader;
        SessionLocator_1.SessionLocator.RootComponent = this;
        if (environment_1.environment.production) {
            SessionLocator_1.SessionLocator.IsProduction = true;
        }
        this.isComponentBooted = true;
        this.RunComponent();
    };
    RootComponent.prototype.ngOnInit = function () {
        this.isComponentInited = true;
        this.RunComponent();
    };
    RootComponent.prototype.RunComponent = function () {
        if (this.isComponentBooted && this.isComponentInited) {
            var url = window.location.href;
            this.isDSV = url.toLowerCase().indexOf(".dsv.") > -1 ? true : false;
            var data = window.sessionStorage.getItem('userdata');
            if ((data && data == "SignOut") || (!data && !SessionLocator_1.SessionLocator.IsExternalParams && url.indexOf('localhost') == -1)) {
                document.location.href = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "Login.aspx";
            }
            else {
                this.LoadLoginPage();
                var IsPREQ = SessionLocator_1.SessionLocator.IsExternalParams && SessionLocator_1.SessionLocator.ExternalParams && SessionLocator_1.SessionLocator.ExternalParams.Menu && SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq";
                if (url && IsPREQ == false && url.indexOf('localhost') == -1) {
                    window.onbeforeunload = function (e) {
                        var message = "";
                        if (SessionLocator_1.SessionLocator.ExternalParams && SessionLocator_1.SessionLocator.ExternalParams.OneTimePasswordId) {
                            message = "when you leave this site can't not be used the key agin";
                        }
                        else if (!SessionLocator_1.SessionLocator.IsSiguOut) {
                            message = "Are you sure you want to leave this page ?";
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(message)) {
                            e.returnValue = message;
                            return message;
                        }
                    };
                }
            }
        }
    };
    RootComponent.prototype.ClearLocation = function () {
        if (this.location) {
            this.location.clear();
        }
    };
    RootComponent.prototype.BuildExternalParams = function () {
        //var url = 'login.aspx?Menu=Tickets&Action=Query&Parmters=[{"CompanyId":"1-720","ShipmentId":"1-26027", "ShipmentNumber" : "bdf22789-46e3-4"}]';
        var url = window.location.href;
        if (url) {
            var keys = url.split('?');
            if (keys[1]) {
                var vars = keys[1].split('&');
                if (vars) {
                    SessionLocator_1.SessionLocator.ExternalParams = new ExternalParams_1.ExternalParams();
                    SessionLocator_1.SessionLocator.IsExternalParams = true;
                    for (var i = 0; i < vars.length; i++) {
                        var pair = vars[i].split('=');
                        SessionLocator_1.SessionLocator.ExternalParams[decodeURIComponent(pair[0])] = pair[1];
                        if (decodeURIComponent(pair[0]) == "Parmters") {
                            var str1 = pair[1];
                            var str = decodeURIComponent(str1);
                            var jsonPMKeys = JSON.parse(str);
                            for (var key in jsonPMKeys) {
                                var arg = new ExternalParams_1.ExternalParamsArg();
                                arg.FieldName = key;
                                arg.FieldValue = jsonPMKeys[key];
                                SessionLocator_1.SessionLocator.ExternalParams.Args.push(arg);
                            }
                        }
                    }
                }
            }
        }
    };
    RootComponent.prototype.LoadLoginPage = function () {
        var _this = this;
        this.ClearLocation();
        //this.isDSV = true;
        if (this.isDSV == true) {
            if (SessionLocator_1.SessionLocator.IsExternalParams && SessionLocator_1.SessionLocator.ExternalParams && SessionLocator_1.SessionLocator.ExternalParams.Menu && SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/CustomLoginComponents/DSVMobileLoginProcessComponent", this.location)
                    .then(function (cmpRef) {
                    cmpRef.instance.Blocking.subscribe(function (s) {
                        SessionLocator_1.SessionLocator.BlockType = s;
                        _this.LoadBlockingScreen();
                    });
                    cmpRef.instance.LoginCompleted.subscribe(function (s) {
                        _this.OnLoginCompleted();
                    });
                });
            }
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/CustomLoginComponents/DSVLoginProcessComponent", this.location)
                    .then(function (cmpRef) {
                    cmpRef.instance.Blocking.subscribe(function (s) {
                        SessionLocator_1.SessionLocator.BlockType = s;
                        _this.LoadBlockingScreen();
                    });
                    cmpRef.instance.LoginCompleted.subscribe(function (s) {
                        _this.OnLoginCompleted();
                    });
                });
            }
        }
        else {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/LoginComponent", this.location)
                .then(function (cmpRef) {
                cmpRef.instance.Blocking.subscribe(function (s) {
                    //SessionLocator.BlockType = s;
                    //this.LoadBlockingScreen();
                });
                cmpRef.instance.LoginCompleted.subscribe(function (s) {
                    _this.OnLoginCompleted(s);
                });
            });
        }
    };
    RootComponent.prototype.LoadBlockingScreen = function () {
        var _this = this;
        this.ClearLocation();
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/BlockScreenComponent", this.location)
            .then(function (cmpRefBlocked) {
            cmpRefBlocked.instance.BackToLoginCompleted.subscribe(function (r) {
                _this.SignOutCompleted();
            });
        });
    };
    RootComponent.prototype.ViewHomeComponent = function () {
        var _this = this;
        this.ClearLocation();
        //SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.location)
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/HomeComponent/HomeComponent", this.location)
            .then(function (cmpRef) {
            cmpRef.instance.RunComponent();
            cmpRef.instance.SignoutCompleted.subscribe(function (s) {
                if (s == "Block") {
                    _this.LoadBlockingScreen();
                }
                else {
                    _this.SignOutCompleted();
                }
            });
        });
    };
    RootComponent.prototype.ViewDeclarationApprovalComponent = function () {
        this.ClearLocation();
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.location)
            .then(function (cmpRef) {
            cmpRef.instance.RunComponent();
            //cmpRef.instance.SignoutCompleted.subscribe(s => {
            //    if (s == "Block") {
            //        this.LoadBlockingScreen();
            //    }
            //    else {
            //        this.SignOutCompleted();
            //    }
            //});
        });
    };
    RootComponent.prototype.ViewEComercePaymentRequestComponent = function () {
        this.ClearLocation();
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/ECommercePaymentRequestMobileComponent", this.location)
            .then(function (cmpRef) {
            cmpRef.instance.RunComponent();
            //cmpRef.instance.SignoutCompleted.subscribe(s => {
            //    if (s == "Block") {
            //        this.LoadBlockingScreen();
            //    }
            //    else {
            //        this.SignOutCompleted();
            //    }
            //});
        });
    };
    RootComponent.prototype.OnLoginCompleted = function (Param) {
        var _this = this;
        if (Param === void 0) { Param = null; }
        if (Param == "IgnoreTerms") {
            this.ViewEComercePaymentRequestComponent();
            return;
        }
        this._FinishLogin = true;
        var termsofUseService = new TermsofUseService_1.TermsofUseService();
        termsofUseService.GetCheckIfGoToTermUseComponent(SessionLocator_1.SessionLocator.Tenant, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    if (myResult.IsTermOfUse) {
                        _this.ClearLocation();
                        if (_this.isDSV == true) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/CustomTermsOfUse/DSVTermsOfUseStartupComponent", _this.location)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Load(myResult.Version);
                                cmpRef.instance.TermsOfUseCompleted.subscribe(function ($event) {
                                    if ($event == "Accept") {
                                        _this.ViewHomeComponent();
                                    }
                                    else if ($event == "Decline") {
                                        _this.SignOutCompleted();
                                    }
                                });
                            });
                        }
                        else {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsOfUseStartupComponent", _this.location)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Load(myResult.Version);
                                cmpRef.instance.TermsOfUseCompleted.subscribe(function ($event) {
                                    if ($event == "Accept") {
                                        _this.ViewHomeComponent();
                                    }
                                    else if ($event == "Decline") {
                                        _this.SignOutCompleted();
                                    }
                                });
                            });
                        }
                    }
                    else {
                        if (SessionLocator_1.SessionLocator.IsExternalParams && SessionLocator_1.SessionLocator.ExternalParams && SessionLocator_1.SessionLocator.ExternalParams.Menu && SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
                            _this.ViewDeclarationApprovalComponent();
                        }
                        else if (SessionLocator_1.SessionLocator.IsExternalParams && SessionLocator_1.SessionLocator.ExternalParams && SessionLocator_1.SessionLocator.ExternalParams.Menu && SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq" && IsMobileDetected() == true) {
                            _this.ViewEComercePaymentRequestComponent();
                        }
                        else {
                            _this.ViewHomeComponent();
                        }
                    }
                }
            }
        });
    };
    RootComponent.prototype.SignOutCompleted = function () {
        var loginService = new LoginService_1.LoginService();
        loginService.GetSignOut().subscribe(function (res) {
        });
        window.sessionStorage.setItem("userdata", "SignOut");
        SessionLocator_1.SessionLocator.ClearLocalStorage();
        SessionLocator_1.SessionLocator.IsSiguOut = true;
        SessionInfo_1.SessionInfo.LoggedUserEmail = "";
        SessionInfo_1.SessionInfo.LoggedUserId = "";
        SessionInfo_1.SessionInfo.Token = "";
        this.ClearLocation();
        document.location.href = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "Login.aspx";
        //SignOutAut
        //window.sessionStorage.setItem("Token", "");
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], RootComponent.prototype, "location", void 0);
    RootComponent = __decorate([
        core_1.Component({
            selector: 'RootComponent',
            template: "\n        <div class=\"MediaFillRelative\">\n            <div #Child></div>\n        </div>\n    ",
        }),
        __metadata("design:paramtypes", [])
    ], RootComponent);
    return RootComponent;
}());
exports.RootComponent = RootComponent;
//# sourceMappingURL=RootComponent.js.map