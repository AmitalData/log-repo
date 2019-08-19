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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TenantLoginPolicyPMService_1 = require("../../../../Common/Services/StandardPMs/TenantLoginPolicyPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var UserExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var TenantLoginPolicyComponent = /** @class */ (function (_super) {
    __extends(TenantLoginPolicyComponent, _super);
    function TenantLoginPolicyComponent(entityResourceService, TenantLoginPolicyPMService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.TenantLoginPolicyPMService = TenantLoginPolicyPMService;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantLoginPolicy";
        _this.IsResourcesReady = false;
        _this.EntityPM = null;
        _this.IsNew = false;
        _this.OnCloseSendToContactsEvent = new core_1.EventEmitter();
        _this.EnabledForUsersCount = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.loginPolicyCode = "";
        _this.keepUserLoggedIn = false;
        _this.IsSessionTimeoutValueHasError = false;
        _this.InternalIps = [];
        _this.twoFactorInternalIPs = "";
        _this.LoginAllowdedIps = [];
        _this.allowedIPs = "";
        _this.userExtendedPMService = new UserExtendedPMService_1.UserExtendedPMService();
        _this.EnabledForTypesList = [];
        var type1 = new EnabledForType("ALL", "All Users");
        _this.EnabledForTypesList.push(type1);
        var type2 = new EnabledForType("SPCF", "Specific Users");
        _this.EnabledForTypesList.push(type2);
        _this.LoadData();
        return _this;
    }
    TenantLoginPolicyComponent.prototype.LoadData = function () {
        var _this = this;
        this.TenantLoginPolicyPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (!response.HasError) {
                if (response.Result) {
                    _this.EntityPM = response.Result;
                    if (_this.EntityPM.IsEnabledForSpecificUsers)
                        _this.GetEnabledForUsersCount();
                    else
                        _this.IsResourcesReady = true;
                }
                else {
                    _this.EntityPM = _this.TenantLoginPolicyPMService.GetNewEntityPM();
                    _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.EntityPM.LoginPolicyCode = "NOREST";
                    _this.EntityPM.IsEnabledForSpecificUsers = false;
                    _this.EntityPM.TwoFactorInternalIPs = "";
                    _this.EntityPM.AllowedIPs = "";
                    _this.EntityPM.ExcludeInternalIPs = false;
                    _this.EntityPM.SessionTimeout = 999;
                    _this.SelectedEnabledFor = _this.EnabledForTypesList.filter(function (t) { return t.Code === "ALL"; })[0];
                    _this.IsNew = true;
                    _this.IsResourcesReady = true;
                }
            }
        });
    };
    TenantLoginPolicyComponent.prototype.GetEnabledForUsersCount = function () {
        var _this = this;
        this.userExtendedPMService.GetUsersTwoFactorAuthenticationEnabled(SessionLocator_1.SessionLocator.Tenant).subscribe(function (resp) {
            if (resp.Result) {
                _this.EnabledForUsersCount = resp.Result.length;
            }
            _this.IsResourcesReady = true;
        });
    };
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "SelectedEnabledFor", {
        get: function () {
            if (this.EntityPM) {
                if (this.EntityPM.IsEnabledForSpecificUsers) {
                    return this.EnabledForTypesList.filter(function (t) { return t.Code === "SPCF"; })[0];
                }
                else {
                    return this.EnabledForTypesList.filter(function (t) { return t.Code === "ALL"; })[0];
                }
            }
        },
        set: function (newValue) {
            var isSpecific = false;
            if (newValue.Code === "SPCF") {
                isSpecific = true;
            }
            else {
                isSpecific = false;
            }
            if (this.EntityPM.IsEnabledForSpecificUsers != isSpecific) {
                this.EntityPM.IsEnabledForSpecificUsers = isSpecific;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "LoginPolicyCode", {
        get: function () {
            if (this.EntityPM) {
                this.loginPolicyCode = this.EntityPM.LoginPolicyCode;
            }
            return this.loginPolicyCode;
        },
        set: function (newValue) {
            if (this.loginPolicyCode != newValue) {
                this.loginPolicyCode = newValue;
                this.EntityPM.LoginPolicyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "KeepUserLoggedIn", {
        get: function () {
            if (this.EntityPM) {
                this.keepUserLoggedIn = this.EntityPM.KeepUserLoggedIn;
            }
            return this.keepUserLoggedIn;
        },
        set: function (newValue) {
            if (this.keepUserLoggedIn != newValue) {
                this.keepUserLoggedIn = newValue;
                this.EntityPM.KeepUserLoggedIn = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "SessionTimeout", {
        get: function () {
            if (this.EntityPM) {
                this.sessionTimeout = this.EntityPM.SessionTimeout;
            }
            return this.sessionTimeout;
        },
        set: function (newValue) {
            if (this.sessionTimeout != newValue) {
                this.sessionTimeout = newValue;
                this.EntityPM.SessionTimeout = newValue;
                this.IsSessionTimeoutValueHasError = false;
                if (newValue > 8) {
                    this.IsSessionTimeoutValueHasError = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "TwoFactorInternalIPs", {
        get: function () {
            var value = "";
            if (this.EntityPM) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
                    this.InternalIps = this.EntityPM.TwoFactorInternalIPs.split(',');
                    this.InternalIps.forEach(function (p) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(p)) {
                            value += p + '\n';
                        }
                    });
                }
                //else
                //this.twoFactorInternalIPs = "";
            }
            return value;
        },
        set: function (newValue) {
            if (this.twoFactorInternalIPs != newValue) {
                this.twoFactorInternalIPs = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "AllowedIPs", {
        get: function () {
            var value = "";
            if (this.EntityPM) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {
                    this.LoginAllowdedIps = this.EntityPM.AllowedIPs.split(',');
                    this.LoginAllowdedIps.forEach(function (p) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(p)) {
                            value += p + '\n';
                        }
                    });
                }
            }
            return value;
        },
        set: function (newValue) {
            if (this.allowedIPs != newValue) {
                this.allowedIPs = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "IPField", {
        get: function () { return this.iPField; },
        set: function (newValue) { if (this.iPField != newValue) {
            this.iPField = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantLoginPolicyComponent.prototype, "AllowedIPField", {
        get: function () { return this.allowedIPField; },
        set: function (newValue) { if (this.allowedIPField != newValue) {
            this.allowedIPField = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    TenantLoginPolicyComponent.prototype.DefineUsers = function () {
        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 725;
        //logWindow.Height = 520;
        //logWindow.WindowArgs = this;
        //logWindow.Title = TextCodeTranslator.TranslateTablePlural("Contact") + " Search";;
        //logWindow.Show('./Common/Components/UsersSearch/SearchContactsComponent');
        var _this = this;
        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Contacts List";
        //logWindow.Width = window.innerWidth - 100;
        //logWindow.Height = window.innerHeight - 100;
        ///logWindow.WindowArgs = windowArgs;
        //logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        //logWindow.WindowClosed.subscribe(($event: any) => {
        //    //this.IsOpenWidnow = false;
        //});
        //var windowArgs: any = {};
        //windowArgs.PartnersObslist = this.PartnersObslist;
        //windowArgs.ToEmail = this.ToEmail;
        //windowArgs.Cc = this.Cc;
        //windowArgs.Bcc = this.Bcc;
        //windowArgs.EntityId = this.EntityId;
        //windowArgs.ObjectTableId = this.ObjectTableId;
        //windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = 725;
        logWindow.Height = 520;
        //logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureUser/Components/UsersSearch/UserSearchComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.GetEnabledForUsersCount();
            //this.IsOpenWidnow = false;
        });
    };
    TenantLoginPolicyComponent.prototype.AddInternalIP = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.IPField)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.IPField.match(ipformat)) {
                if (this.TwoFactorInternalIPs.indexOf(this.IPField.trim()) < 0) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
                        this.EntityPM.TwoFactorInternalIPs += ',' + this.IPField.trim();
                    }
                    else
                        this.EntityPM.TwoFactorInternalIPs = this.IPField.trim();
                    this.EntityPM.TwoFactorInternalIPs = this.TrimEndAndStart(this.EntityPM.TwoFactorInternalIPs, ',');
                    this.TwoFactorInternalIPs = this.EntityPM.TwoFactorInternalIPs;
                }
                this.IPField = null;
            }
            else {
                this.ValidationErrorsList.push("You have entered an invalid IP address!");
            }
        }
        else {
            //this.ValidationErrorsList.push("You have entered an invalid IP address!");
        }
    };
    TenantLoginPolicyComponent.prototype.RemoveIP = function (ip) {
        if (this.TwoFactorInternalIPs.indexOf(ip.trim()) >= 0) {
            var ipsString = "";
            var ipsArray = this.EntityPM.TwoFactorInternalIPs.split(',');
            ipsArray.forEach(function (s) {
                if (ip != s && !Tools_1.AppTool.IsNullOrEmpty(s)) {
                    ipsString += s + ",";
                }
            });
            ipsString = this.TrimEndAndStart(ipsString, ',');
            this.EntityPM.TwoFactorInternalIPs = ipsString;
            this.TwoFactorInternalIPs = this.EntityPM.TwoFactorInternalIPs;
        }
    };
    TenantLoginPolicyComponent.prototype.AddAllowedIP = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AllowedIPField)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.AllowedIPField.match(ipformat)) {
                if (this.AllowedIPs.indexOf(this.AllowedIPField.trim()) < 0) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {
                        this.EntityPM.AllowedIPs += ',' + this.AllowedIPField.trim();
                    }
                    else
                        this.EntityPM.AllowedIPs = this.AllowedIPField.trim();
                    this.EntityPM.AllowedIPs = this.TrimEndAndStart(this.EntityPM.AllowedIPs, ',');
                    this.AllowedIPs = this.EntityPM.AllowedIPs;
                }
                this.AllowedIPField = null;
            }
            else {
                this.ValidationErrorsList.push("You have entered an invalid IP address!");
            }
        }
        else {
            //this.ValidationErrorsList.push("You have entered an invalid IP address!");
        }
    };
    TenantLoginPolicyComponent.prototype.RemoveAllowedIP = function (ip) {
        if (this.AllowedIPs.indexOf(ip.trim()) >= 0) {
            var ipsString = "";
            var ipsArray = this.EntityPM.AllowedIPs.split(',');
            ipsArray.forEach(function (s) {
                if (ip != s && !Tools_1.AppTool.IsNullOrEmpty(s)) {
                    ipsString += s + ",";
                }
            });
            ipsString = this.TrimEndAndStart(ipsString, ',');
            this.EntityPM.AllowedIPs = ipsString;
            this.AllowedIPs = this.EntityPM.AllowedIPs;
        }
    };
    TenantLoginPolicyComponent.prototype.TrimEndAndStart = function (myString, ch) {
        if (!Tools_1.AppTool.IsNullOrEmpty(myString)) {
            if (myString.charAt(myString.length - 1) == ch) {
                myString = myString.substr(0, myString.length - 1);
            }
            if (myString.charAt(0) == ch) {
                myString = myString.substr(0, 1);
            }
        }
        return myString;
    };
    //Commands 
    TenantLoginPolicyComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TenantLoginPolicyComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LoginPolicyCode)) {
            // this.ObjectTableName + ".F." + myFieldName
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TenantLoginPolicy.F.LoginPolicyCode") + " field is required!");
        }
        if ((this.EntityPM.LoginPolicyCode === "TFAUTH") && this.EntityPM.ExcludeInternalIPs == true && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TenantLoginPolicy.F.TwoFactorInternalIPs") + " field is required!");
        }
        if ((this.EntityPM.LoginPolicyCode === "COMPIP") && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("TenantLoginPolicy.F.AllowedIPs") + " field is required!");
        }
        if (this.IsSessionTimeoutValueHasError) {
            this.ValidationErrorsList.push("Maximum session timeout 8 hours");
        }
        if (this.ValidationErrorsList.length > 0)
            return;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (this.IsNew) {
            this.TenantLoginPolicyPMService.insert(this.EntityPM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else
                    _this.ValidationErrorsList = response.ErrorsArray;
            });
        }
        else {
            this.TenantLoginPolicyPMService.update(this.EntityPM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else
                    _this.ValidationErrorsList = response.ErrorsArray;
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], TenantLoginPolicyComponent.prototype, "OnCloseSendToContactsEvent", void 0);
    TenantLoginPolicyComponent = __decorate([
        core_1.Component({
            selector: 'TenantLoginPolicyComponent',
            moduleId: module.id,
            templateUrl: './TenantLoginPolicyComponent.html',
            providers: [TenantLoginPolicyPMService_1.TenantLoginPolicyPMService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, TenantLoginPolicyPMService_1.TenantLoginPolicyPMService])
    ], TenantLoginPolicyComponent);
    return TenantLoginPolicyComponent;
}(BaseComponent_1.BaseComponent));
exports.TenantLoginPolicyComponent = TenantLoginPolicyComponent;
var EnabledForType = /** @class */ (function () {
    function EnabledForType(Code, Name) {
        this.Code = Code;
        this.Name = Name;
    }
    return EnabledForType;
}());
exports.EnabledForType = EnabledForType;
//# sourceMappingURL=TenantLoginPolicyComponent.js.map