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
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var LocationDirective_1 = require("../../Utilities/LocationDirective");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LoginService_1 = require("../../Services/LoginService");
var http_1 = require("@angular/http");
var AmitalGatewayUtil_1 = require("../../Utilities/AmitalGatewayUtil");
var Rx_1 = require("rxjs/Rx");
var NotificationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/NotificationExtendedListService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var Environment_1 = require("../../Locators/Environment");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var DetectUserInActivity_1 = require("../../Helpers/DetectUserInActivity");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var BluesnapContractPMService_1 = require("../../Services/StandardPMs/BluesnapContractPMService");
var UserExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var HomeComponent = /** @class */ (function () {
    function HomeComponent() {
        this.DataContext = this;
        this.ChangeHeaderColor = false;
        this.SignoutCompleted = new core_1.EventEmitter();
        this.SettingBtnVisibility = false;
        this.IsShowLastSuccessfulLoginComponent = true;
        this.IfBlueSnapContracts = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.BluesnapContractService = new BluesnapContractPMService_1.BluesnapContractPMService();
        this.ShowNewReleaseToolTip = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // InitializeComponent
        this.IsTenant65 = false;
        this.IsLogBox = false;
        this.IsNewSignupTenant = false;
        this.LayoutDirection = 'ltr';
        //public SystemFontFamily: string = 'Lucida Sans Unicode';
        this.SystemFontFamily = "'Lucida Sans Unicode', 'Lucida Grande', sans-serif";
        // InitializeAppHeader
        this.EnvironmentUrl = null;
        this.EnvironmentSRC = null;
        this.EnvironmentName = null;
        this.IsBellVisible = false;
        this.IsCustomizationVisible = false;
        this.IsSignatureVisible = false;
        this.IsChangePasswordVisible = false;
        this.IsDataBackupVisible = false;
        this.IsFillLocalStorageVisible = false;
        this.IsDocumentsBackupVisible = false;
        this.IsCurrenciesRatesVisible = false;
        this.IsBluesnapAccount = false;
        this.IsCountryIsrael = false;
        this.TrialMessage = null;
        this.messageWindow = new MessageWindow_1.MessageWindow();
        // AmitalBrowserInUse
        this._AmitalBrowserInUse = false;
        this.height = 16;
        this.width = 16;
        this.IsControlVisibile = false;
        this.showLockIndicator = false;
        this.notificationExtendedListService = new NotificationExtendedListService_1.NotificationExtendedListService();
        // RunComponent
        this.Retries = 0;
        this.SaveCompletedEvent = null;
        this.IsApplicationBlocked = false;
        this.Tenant = SessionLocator_1.SessionLocator.Tenant;
        SessionLocator_1.SessionLocator.Index = 0;
        SessionLocator_1.SessionLocator.AllSessions = new Array();
        SessionLocator_1.SessionLocator.HomeComponent = this;
        this.ChangeHeaderColor = ObjectsLocator_1.ObjectsLocator.TenantManagementJS.ChangeHeaderColor;
        this.Tabs = [];
        this.Tabs.push(new SessionTabItem());
        this.InitializeComponent();
        if (!this.IsNewSignupTenant) {
            this.InitializeAppHeader();
            this.CheckAmitalBrowserInUse();
        }
        if (!SessionInfo_1.SessionInfo.KeepUserLoggedIn) {
            // sessionTimeout
            var sessionTimeout = new DetectUserInActivity_1.DetectUserInActivity();
            sessionTimeout.Start(SessionInfo_1.SessionInfo.SessionTimeout);
            // tokenExpiration
            var tokenExpiration = new DetectUserInActivity_1.DetectUserInActivity(true);
            tokenExpiration.Start(SessionInfo_1.SessionInfo.WebTokenLifeTimeInMinutes, SessionInfo_1.SessionInfo.WebTokenExpirationWarningInMinutes, "M"); //(3, 1, "M")
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(ObjectsLocator_1.ObjectsLocator.GlobalSetting.ReleaseNotesURL) && SessionLocator_1.SessionLocator.ShowUserNewReleaseToolTip && !SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.ShowNewReleaseToolTip = true;
        }
    }
    HomeComponent.prototype.OnSessionMouseUp = function ($event) {
        this.CurrentSession.MouseUpEvent.emit($event);
    };
    HomeComponent.prototype.InitializeComponent = function () {
        var _this = this;
        this.IsBluesnapAccount = !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount);
        this.IsCountryIsrael = SessionLocator_1.SessionLocator.TenantManagementJS.CountryName == "Israel";
        var isNewSignupTenant = false;
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.CurrencyId) || Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId) || Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.AddressId) || Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.AgentId)) {
            isNewSignupTenant = true;
        }
        this.IsNewSignupTenant = SessionLocator_1.SessionLocator.IsNewSignupTenant = isNewSignupTenant;
        this.IsTenant65 = SessionLocator_1.SessionLocator.Tenant == 65 ? true : false;
        this.IsLogBox = SessionLocator_1.SessionLocator.TenantPM.IsDocumentsArchive == true ? true : false;
        // Layout Direction
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            // if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
            if (!ObjectsLocator_1.ObjectsLocator.LoggedUserPM.DontShowLocal) {
                this.SystemFontFamily = 'Arial'; //'OpenSans-Regular';
                isNewSignupTenant = false;
            }
        }
        this.table = window.ObjectTables.filter(function (d) { return d.Name === 'General'; })[0];
        var SettingBtnFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "AppSettingsBtn") && f.ObjectTableId == _this.table.Id; })[0];
        this.SettingBtnVisibility = !Tools_1.AppTool.IsNullOrEmpty(SettingBtnFeature) || SessionLocator_1.SessionLocator.Tenant == 0;
    };
    HomeComponent.prototype.InitializeAppHeader = function () {
        this.EnvironmentUrl = Environment_1.Environment.GetEnvironmentUrl();
        this.EnvironmentSRC = Environment_1.Environment.GetEnvironmentIcon();
        this.EnvironmentName = Environment_1.Environment.GetEnvironmentName();
        this.Company = SessionLocator_1.SessionLocator.TenantPM.Company;
        this.LoggedUser = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        if (SessionLocator_1.SessionLocator.Tenant == 261) {
            this.IsCustomizationVisible = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.Customization")) {
            this.IsCustomizationVisible = true;
        }
        if (!this.IsLogBox && FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsCurrenciesRatesVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SIGNATURESETTING")) {
            this.IsSignatureVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CHANGEPASSWORDSETTING")) {
            this.IsChangePasswordVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.DataBackup")) {
            this.IsDataBackupVisible = true;
        }
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            this.IsFillLocalStorageVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "NOTIFICATIONBELL")) {
            this.IsBellVisible = true;
            this.GetBadjCount();
            this.StartApplicationTimers();
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.DocumentsBackup")) {
            this.IsDocumentsBackupVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("BluesnapContract", "PaymentSettingButton")) {
            this.IfBlueSnapContracts = true;
        }
    };
    HomeComponent.prototype.GetUserSetting = function () {
        var _this = this;
        if (this.loginService == null) {
            this.loginService = new LoginService_1.LoginService();
            this.authHeader = new http_1.Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator_1.SessionLocator.Tenant;
            this.loginService.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserId;
            this.loginService.LoggedUserEmail = SessionInfo_1.SessionInfo.LoggedUserEmail;
        }
        //this.loginService.CurrentTenant = SessionLocator.TenantPM.Id;
        this.loginService.CheckTenantMangmnt(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (myResult2) {
            SessionLocator_1.SessionLocator.BlockType = null;
            var tt = myResult2;
            _this.TrialMessage = "";
            _this.messageWindow.Close();
            var user = SessionLocator_1.SessionLocator.LoggedUserPM;
            user.ExpirationDaysLeft = tt.ExpirationDaysLeft;
            user.ExpirationDate = tt.ExpirationDate;
            SessionLocator_1.SessionLocator.TenantManagementJS.TrailDaysLeft = tt.TrailDaysLeft;
            SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft = tt.PaidDaysLeft;
            SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDaysLeft = tt.SuspendDaysLeft;
            var stopTimer = false;
            if (tt.DoBlocking) {
                SessionLocator_1.SessionLocator.BlockType = tt.BlockType;
                _this.SignoutClickedToBlockScreen();
                //BlockScreen
                stopTimer = true;
            }
            else {
                if (tt.ExpirationDate != null) {
                    if (tt.ExpirationDaysLeft >= 0) {
                        _this.CheckUserExpiration(user);
                    }
                    else {
                        SessionLocator_1.SessionLocator.BlockType = "company";
                        _this.SignoutClickedToBlockScreen();
                        stopTimer = true;
                    }
                }
                if (tt.PaymentFailure) {
                    if (tt.TrailDaysLeft >= 0) {
                        _this.CheckPaymentFailure();
                    }
                    else {
                        SessionLocator_1.SessionLocator.BlockType = "suspend";
                        _this.SignoutClickedToBlockScreen();
                        stopTimer = true;
                    }
                }
                else if (tt.IsTrial) {
                    if (tt.TrailDaysLeft >= 0) {
                        _this.CheckTrialDays();
                    }
                    else {
                        SessionLocator_1.SessionLocator.BlockType = "company";
                        _this.SignoutClickedToBlockScreen();
                        stopTimer = true;
                    }
                }
                else if (!tt.IsRecurring && tt.PaidUntilDate != null) {
                    if (tt.PaidDaysLeft >= 0) {
                        _this.CheckPaidUntilDays();
                    }
                    else {
                        SessionLocator_1.SessionLocator.BlockType = "company";
                        _this.SignoutClickedToBlockScreen();
                        stopTimer = true;
                    }
                }
            }
            if (!stopTimer)
                _this.RunComponentTimerTrial();
        });
    };
    HomeComponent.prototype.CheckPaymentFailure = function () {
        var HeaderMessage = "";
        var WindowMessage = "";
        this.messageWindow.Width = 600;
        this.messageWindow.Height = 150;
        var email = Environment_1.Environment.GetContactUsEmail();
        if (SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDate == null) {
            WindowMessage = "Your Bluesnap payment is failing, \nplease contact Bluesnap to fix the problem";
            HeaderMessage = "Your Bluesnap payment is failing";
        }
        else {
            HeaderMessage = "Your company subscription will expire in " + SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDaysLeft + " days.";
            WindowMessage = "Your company subscription will expire in " + SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDaysLeft + " days due to credit \ncard failure. \nPlease contact your e-commerce vendor or " + email;
        }
        this.messageWindow.Title = HeaderMessage;
        this.messageWindow.Message = WindowMessage;
        this.messageWindow.Show(this.messageWindow.Message);
    };
    HomeComponent.prototype.CheckTrialDays = function () {
        var HeaderMessage = "";
        var WindowMessage = "";
        this.messageWindow.Width = 380;
        this.messageWindow.Height = 150;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.TrailDaysLeft <= 0) {
            HeaderMessage = "0 Trial Days Left !";
        }
        else {
            HeaderMessage = SessionLocator_1.SessionLocator.TenantManagementJS.TrailDaysLeft + " Trial Days Left !";
        }
        this.messageWindow.Message = HeaderMessage;
        this.messageWindow.Show(this.messageWindow.Message);
        this.TrialMessage = HeaderMessage;
    };
    HomeComponent.prototype.CheckPaidUntilDays = function () {
        var HeaderMessage = "";
        var WindowMessage = "";
        var email = Environment_1.Environment.GetContactUsEmail();
        if (SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft <= 31) {
            this.messageWindow.Width = 600;
            this.messageWindow.Height = 150;
            if (SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft == 0) {
                WindowMessage = "Your company subscription will expire in 0 days. \nTo renew please contact " + email;
                HeaderMessage = "Your company subscription will expire in 0 days.";
            }
            else {
                var AbsuluteValue;
                if (SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft >= 0)
                    AbsuluteValue = SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft;
                else
                    AbsuluteValue = SessionLocator_1.SessionLocator.TenantManagementJS.PaidDaysLeft * -1;
                WindowMessage = "Your company subscription will expire in " + AbsuluteValue + " days. \nTo renew please contact " + email;
                HeaderMessage = "Your company subscription will expire in " + AbsuluteValue + " days.";
            }
            this.messageWindow.Title = HeaderMessage;
            this.messageWindow.Message = WindowMessage;
            this.messageWindow.Show(this.messageWindow.Message);
        }
    };
    HomeComponent.prototype.AccountAndTenantExpiration = function () {
        if (this.trialTimer != null) {
            this.GetUserSetting();
        }
        else {
            this.AccountAndTenantExpirationFunction();
        }
    };
    HomeComponent.prototype.AccountAndTenantExpirationFunction = function () {
        this.TrialMessage = "";
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.BlockType)) {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.ExpirationDate != null) {
                if (SessionLocator_1.SessionLocator.LoggedUserPM.ExpirationDaysLeft <= 7) {
                    this.CheckUserExpiration(SessionLocator_1.SessionLocator.LoggedUserPM);
                }
                else {
                    if (SessionLocator_1.SessionLocator.TenantManagementJS.PaymentFailure) {
                        this.CheckPaymentFailure();
                    }
                    else if (SessionLocator_1.SessionLocator.TenantManagementJS.IsTrial) {
                        this.CheckTrialDays();
                    }
                    else if (!SessionLocator_1.SessionLocator.TenantManagementJS.IsRecurring && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PaidUntilDate + "")) {
                        this.CheckPaidUntilDays();
                    }
                }
            }
            else {
                if (SessionLocator_1.SessionLocator.TenantManagementJS.PaymentFailure) {
                    this.CheckPaymentFailure();
                }
                else if (SessionLocator_1.SessionLocator.TenantManagementJS.IsTrial) {
                    this.CheckTrialDays();
                }
                else if (!SessionLocator_1.SessionLocator.TenantManagementJS.IsRecurring && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PaidUntilDate)) {
                    this.CheckPaidUntilDays();
                }
            }
        }
        this.RunComponentTimerTrial();
    };
    HomeComponent.prototype.CheckUserExpiration = function (loggedUser) {
        var email = Environment_1.Environment.GetContactUsEmail();
        //if (SessionLocator.PrivateLableSettings) {
        //    email = SessionLocator.PrivateLableSettings.ContactUsEmail;
        //}
        var HeaderMessage = "";
        var WindowMessage = "";
        if (loggedUser.ExpirationDaysLeft <= 7) {
            this.messageWindow.Width = 380;
            this.messageWindow.Height = 150;
            if (loggedUser.ExpirationDaysLeft < 0) {
                HeaderMessage = "Your temporary access has expired.";
                WindowMessage = "Your temporary access has expired. \nTo renew please contact " + email;
            }
            else {
                HeaderMessage = "Your Access will be expired in " + loggedUser.ExpirationDaysLeft + " days";
                WindowMessage = "Your Access will be expired in " + loggedUser.ExpirationDaysLeft + " days. \nTo renew please contact " + email;
            }
            this.messageWindow.Title = HeaderMessage;
            this.messageWindow.Message = WindowMessage;
            this.messageWindow.Show(this.messageWindow.Message);
            this.TrialMessage = HeaderMessage;
        }
    };
    HomeComponent.prototype.RunComponentTimerTrial = function () {
        var _this = this;
        if (this.trialTimer) {
            clearTimeout(this.trialTimer);
        }
        this.trialTimer = setTimeout(function () { return _this.AccountAndTenantExpiration(); }, 43200000);
    };
    HomeComponent.prototype.CheckAmitalBrowserInUse = function () {
        var _this = this;
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            this.AddTab(); //this.Tabs.push(new SessionTabItem());
            this._AmitalBrowserInUse = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse;
            var myCA23EditTab_1 = this.Tabs[1];
            this.SelectionChanged(myCA23EditTab_1);
            var timerToken_1 = //setTimeout(() => this.RunComponent(), 1);
             setTimeout(function () {
                //if (tabItem.IsSelected) {
                clearTimeout(timerToken_1);
                if (!myCA23EditTab_1.IsSessionLoaded) {
                    var locs = _this.AllLocations.toArray().filter(function (f) { return f.Code == 'SessionLocation'; });
                    var myLocation_1 = locs.filter(function (f) { return f.Index == myCA23EditTab_1.Index; })[0];
                    //let viewContainerRef = myCA23EditTab.SessionComponent.viewContainerRef
                    if (myLocation_1 != null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Session/SessionComponent", myLocation_1.viewContainerRef).then(function (cmpRef) {
                            cmpRef.instance.SessionIndex = myCA23EditTab_1.Index;
                            cmpRef.instance.SessionTabItem = myCA23EditTab_1;
                            cmpRef.instance.SessionLocation = myLocation_1;
                            cmpRef.instance.ComponentRef = cmpRef;
                            SessionLocator_1.SessionLocator.AddSession(cmpRef.instance);
                            myCA23EditTab_1.IsSessionLoaded = true;
                            myCA23EditTab_1.SessionComponent = cmpRef.instance;
                            _this.CurrentSession = myCA23EditTab_1.SessionComponent;
                            cmpRef.instance.RunComponent();
                            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.NoteUnifreightIamReady();
                        });
                    }
                }
                else {
                    AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.NoteUnifreightIamReady();
                }
            }, 500);
        }
    };
    Object.defineProperty(HomeComponent.prototype, "IsAmitalBackButtonDisable", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(AmitalGatewayUtil_1.AmitalGatewayUtil)) {
                return true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(AmitalGatewayUtil_1.AmitalGatewayUtil.Instance)) {
                return true;
            }
            return AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable;
        },
        enumerable: true,
        configurable: true
    });
    HomeComponent.prototype.AmitalBackButtonClicked = function () {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable) {
            return;
        }
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
    };
    HomeComponent.prototype.UnifaceRequest = function (event) {
        var _this = this;
        var myParam = event.detail;
        var myEditTab = this.Tabs[0];
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequest(myParam, myEditTab, function () {
            myEditTab.Header = myParam.formtitle;
            _this.SelectionChanged(myEditTab);
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsTabCA23 = false;
        }, function () {
            _this.SelectionChanged(_this.Tabs[1]);
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsTabCA23 = true;
        });
    };
    Object.defineProperty(HomeComponent.prototype, "ShowLockIndicator", {
        get: function () { return this.showLockIndicator; },
        set: function (newValue) {
            if (this.showLockIndicator != newValue) {
                this.showLockIndicator = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    HomeComponent.prototype.GetBadjCount = function () {
        var _this = this;
        this.notificationExtendedListService.GetNotificationsBadjCount(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.badjCount = response.Result;
                    if (_this.badjCount > 0) {
                        _this.IsBadjCountVisibile = true;
                        if (_this.badjCount < 10) {
                            _this.top = 2;
                            _this.right = 0;
                            _this.left = 0;
                        }
                        else if (_this.badjCount > 10 && _this.badjCount < 100) {
                            _this.left = 0;
                            _this.top = 2;
                            _this.right = 0;
                        }
                        else if (_this.badjCount > 100) {
                            _this.left = 0;
                            _this.top = 2;
                            _this.right = 0;
                            _this.height = 17;
                            _this.width = 17;
                        }
                    }
                    else {
                        _this.IsBadjCountVisibile = false;
                    }
                }
            }
        });
    };
    HomeComponent.prototype.StartApplicationTimers = function () {
        var _this = this;
        var belltimer = this.initializeBadjCountTimer().subscribe(function (res) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "NOTIFICATIONBELL")) {
                _this.GetBadjCount();
            }
        });
    };
    HomeComponent.prototype.initializeBadjCountTimer = function () {
        return Rx_1.Observable.interval(60000).timeInterval();
    };
    HomeComponent.prototype.onBellButtonClicked = function () {
        this.BellClicked = true;
        if (this.IsControlVisibile) {
            this.IsControlVisibile = false;
        }
        else {
            this.IsControlVisibile = true;
            this.IsBadjCountVisibile = false;
        }
    };
    HomeComponent.prototype.OnClickOutSide = function () {
        if (!this.BellClicked && !this.MouseInArea) {
            if (this.IsControlVisibile) {
                this.IsControlVisibile = false;
            }
        }
        this.BellClicked = false;
    };
    HomeComponent.prototype.OnControlMouseOver = function () {
        this.MouseInArea = true;
        var input = document.getElementById("111Bell");
        input.focus();
    };
    HomeComponent.prototype.OnControlMouseOut = function () {
        this.MouseInArea = false;
    };
    HomeComponent.prototype.OnBellBlur = function () {
        if (!this.MouseInArea) {
            this.IsControlVisibile = false;
        }
    };
    HomeComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else if (!this.ApplicationLocation) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                SessionLocator_1.SessionLocator.ApplicationLocation = this.ApplicationLocation;
                if (SessionLocator_1.SessionLocator.BlockType) {
                    this.IsApplicationBlocked = true;
                }
                if (this.SelectedTabItem == null) {
                    this.SelectionChanged(this.Tabs[0]);
                }
                else {
                    if (this.SelectedTabItem.IsSelected) {
                        if (!this.SelectedTabItem.IsSessionLoaded) {
                            var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'SessionLocation'; });
                            var location_1 = locs.filter(function (f) { return f.Index == _this.SelectedTabItem.Index; })[0];
                            if (location_1 == null) {
                                this.RunComponentTimer();
                            }
                            else if (!this.IsApplicationBlocked) {
                                this.CreateSession(this.SelectedTabItem);
                            }
                        }
                    }
                }
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    HomeComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    HomeComponent.prototype.AddTab = function () {
        SessionLocator_1.SessionLocator.Index += 1;
        this.Tabs.push(new SessionTabItem());
        this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
        this.Retries = 0;
        this.RunComponentTimer();
        if (this.Tabs.length > 4)
            this.IsShowLastSuccessfulLoginComponent = false;
    };
    HomeComponent.prototype.SelectionChanged = function (clickdTab) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;
                this.Tabs.forEach(function (item) {
                    item.IsSelected = false;
                    if (item.SessionComponent) {
                        item.SessionComponent.StopChangeDetection();
                    }
                });
                this.SelectedTabItem.IsSelected = true;
                if (this.SelectedTabItem.SessionComponent) {
                    this.SelectedTabItem.SessionComponent.StartChangeDetection();
                }
            }
            if (this.SelectedTabItem.IsSessionLoaded) {
                SessionLocator_1.SessionLocator.SelectedSession = this.SelectedTabItem.SessionComponent;
                this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
                this.CurrentSession.SessionSeleced.emit(true);
            }
            else {
                this.CreateSession(clickdTab);
            }
        }
    };
    HomeComponent.prototype.CreateSession = function (tabItem) {
        var _this = this;
        if (this.isLoaderReady) {
            if (tabItem.IsSelected) {
                if (!tabItem.IsSessionLoaded) {
                    var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'SessionLocation'; });
                    var myLocation = locs.filter(function (f) { return f.Index == tabItem.Index; })[0];
                    if (myLocation != null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Session/SessionComponent", myLocation.viewContainerRef).then(function (cmpRef) {
                            cmpRef.instance.SessionIndex = tabItem.Index;
                            cmpRef.instance.SessionTabItem = tabItem;
                            cmpRef.instance.ComponentRef = cmpRef;
                            SessionLocator_1.SessionLocator.AddSession(cmpRef.instance);
                            tabItem.IsSessionLoaded = true;
                            tabItem.SessionComponent = cmpRef.instance;
                            SessionLocator_1.SessionLocator.SelectedSession = tabItem.SessionComponent;
                            _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
                            cmpRef.instance.RunComponent();
                            if (tabItem.Index == 0) {
                                if (_this.IsNewSignupTenant) {
                                    cmpRef.instance.SessionInitialize.subscribe(function (s) {
                                        _this.RunSignupWizard();
                                    });
                                }
                                else {
                                    _this.AccountAndTenantExpiration();
                                }
                            }
                        });
                    }
                }
            }
        }
    };
    HomeComponent.prototype.CloseTab = function (tabItem) {
        var _this = this;
        var ClosedTabEditComponent = tabItem.SessionComponent.CurrentEditComponent;
        if (ClosedTabEditComponent) {
            if (ClosedTabEditComponent.NeedCloseConfirmation()) {
                this.SelectionChanged(tabItem);
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.IsOverAll = true;
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(ClosedTabEditComponent.ObjectTableName)));
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        if (!_this.SaveCompletedEvent) {
                            _this.SaveCompletedEvent = ClosedTabEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                if (isSaveSuccess) {
                                    _this.Close(tabItem);
                                }
                                Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                                _this.SaveCompletedEvent = null;
                            });
                        }
                        ClosedTabEditComponent.SaveChanges();
                    }
                    else if (confirmWindow.No) {
                        _this.Close(tabItem);
                    }
                });
            }
            else {
                this.Close(tabItem);
            }
        }
        else {
            this.Close(tabItem);
        }
    };
    HomeComponent.prototype.Close = function (tabItem) {
        var itemIndex = this.Tabs.indexOf(tabItem);
        if (itemIndex > -1) {
            this.Tabs.splice(itemIndex, 1);
            if (tabItem.IsSelected) {
                this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
            }
            tabItem.SessionComponent.DestroySession();
            tabItem = null;
            if (this.Tabs.length <= 4) {
                if (!this.IsShowLastSuccessfulLoginComponent)
                    this.IsShowLastSuccessfulLoginComponent = true;
            }
        }
    };
    HomeComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;
    };
    HomeComponent.prototype.RunSignupWizard = function () {
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Maintenance/Wizard/WizardBaseComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            //cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.RunComponent();
            cmpRef.instance.SignOutCompleted.subscribe(function (s) {
                SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
            });
            cmpRef.instance.SaveCompleted.subscribe(function (s) {
                SessionLocator_1.SessionLocator.RootComponent.ViewHomeComponent();
            });
        });
    };
    // App Header Commands
    HomeComponent.prototype.CustomizationClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Object Names";
        logWindow.IsShowCloseButton = true;
        logWindow.Width = 800;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationMainComponent');
    };
    HomeComponent.prototype.TranslateLabelsClicked = function () {
        var windowTitle = "Select Translation Language";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/TranslationLabels/SelectLanguagesComponent');
    };
    HomeComponent.prototype.SignatureClicked = function () {
        var _this = this;
        if (!this.CurrentSession.IsOpenSignatureWindowFromSetting) {
            this.CurrentSession.IsOpenSignatureWindowFromSetting = true;
            this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                var windowArgs = {};
                windowArgs.DataViewModel = _this;
                windowArgs.PageType = "Signature";
                windowArgs.TemplateId = SessionLocator_1.SessionLocator.LoggedUserId;
                windowArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Html Template";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
                logWindow.WindowClosed.subscribe(function ($event) {
                    _this.CurrentSession.IsOpenSignatureWindowFromSetting = false;
                });
            });
        }
    };
    HomeComponent.prototype.ChangePasswordClicked = function () {
        var _this = this;
        if (!this.CurrentSession.IsOpenChangePasswordWindowFromSetting) {
            this.CurrentSession.IsOpenChangePasswordWindowFromSetting = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "Change User Password";
            this._entityResourceService.getEntityResourceByTableName("User").subscribe(function (response) {
                logWindow.DataContext = _this;
                logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
            });
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.CurrentSession.IsOpenChangePasswordWindowFromSetting = false;
            });
        }
    };
    HomeComponent.prototype.CurrenciesRatesClicked = function () {
        var windowTitle = "Edit exchange rates";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(function (response) {
            logWindow.Show('./Common/Components/Maintenance/RatesMainTabComponent');
        });
    };
    HomeComponent.prototype.DataBackupClicked = function () {
        var _this = this;
        if (!this.CurrentSession.IsOpenDatabaseBackupWindowFromSetting) {
            this.CurrentSession.IsOpenDatabaseBackupWindowFromSetting = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 140;
            logWindow.Title = "Database Backup";
            logWindow.DataContext = this;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/DatabaseBackupComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.CurrentSession.IsOpenDatabaseBackupWindowFromSetting = false;
            });
        }
    };
    HomeComponent.prototype.DocumentsBackupClicked = function () {
        var _this = this;
        if (!this.CurrentSession.IsOpenDocumentBackupWindowFromSetting) {
            this.CurrentSession.IsOpenDocumentBackupWindowFromSetting = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1360;
            logWindow.Height = 600;
            logWindow.Title = "Documents Backup";
            logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/DocumentFilingBackupBatchesComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.CurrentSession.IsOpenDocumentBackupWindowFromSetting = false;
            });
        }
    };
    HomeComponent.prototype.SubscribeToLogitude = function (EmptyOrError, contractId, temp) {
        var storeid = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeid = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            if (SessionLocator_1.SessionLocator.TenantManagementJS.CountryName == "Israel") {
                contractId = "3256464";
            }
            else {
                contractId = "3148346";
            }
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
                ;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
                ;
                ;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.SubscribeToAWB = function (EmptyOrError, contractId, temp) {
        var storeId = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            contractId = "3285402";
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.BuyToAWB = function (EmptyOrError, contractId, temp) {
        var storeId = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            contractId = "3233898";
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.OneTimeBuy = function (EmptyOrError, contractId, temp) {
        var storeId = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            if (SessionLocator_1.SessionLocator.TenantManagementJS.CountryName == "Israel") {
                contractId = "3529380";
            }
            else {
                contractId = "3300952";
            }
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapOneTimeContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapOneTimeContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.SubscribeToCRM = function (EmptyOrError, contractId, temp) {
        var storeId = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            if (SessionLocator_1.SessionLocator.TenantManagementJS.CountryName == "Israel") {
                contractId = "3529378";
            }
            else {
                contractId = "3280846";
            }
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapCRMContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapCRMContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.SubscribeToInttra = function (EmptyOrError, contractId, temp) {
        var storeId = "543002";
        var isSandbox = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || Tools_1.AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            contractId = "3542118";
        }
        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers = Tools_1.AppTool.IsNullOrZero(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapInttraStockContractQTY) ? 1 : SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapInttraStockContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://ws.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo_1.SessionInfo.LoggedUserTenant;
            }
        }
        var win = window.open(link, '_blank');
        win.focus();
    };
    HomeComponent.prototype.SubscribeClicked = function (code) {
        var _this = this;
        var link = "";
        var EmptyOrError = true;
        switch (code) {
            case "LOG":
                {
                    var myService = new CommonDomainService_1.CommonDomainService();
                    this.CurrentSession.StartBusyIndicatorLoading();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapContractId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            _this.BluesnapContractService.get(contractId).subscribe(function (res) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (res) {
                                    if (!res.HasError) {
                                        contractId = res.Result.ContractId;
                                        EmptyOrError = false;
                                        _this.SubscribeToLogitude(EmptyOrError, contractId, temp);
                                    }
                                    else {
                                        _this.SubscribeToLogitude(EmptyOrError, null, temp);
                                    }
                                }
                                else {
                                    _this.SubscribeToLogitude(EmptyOrError, null, temp);
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.SubscribeToLogitude(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
            case "AWB":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new CommonDomainService_1.CommonDomainService();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBContractId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            _this.BluesnapContractService.get(contractId).subscribe(function (res) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (res) {
                                    if (!res.HasError) {
                                        contractId = res.Result.ContractId;
                                        EmptyOrError = false;
                                        _this.SubscribeToAWB(EmptyOrError, contractId, temp);
                                    }
                                    else {
                                        _this.SubscribeToAWB(EmptyOrError, null, temp);
                                    }
                                }
                                else {
                                    _this.SubscribeToAWB(EmptyOrError, null, temp);
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.SubscribeToAWB(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
            case "BUY":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new CommonDomainService_1.CommonDomainService();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapEAWBSContractId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            _this.BluesnapContractService.get(contractId).subscribe(function (res) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (res) {
                                    if (!res.HasError) {
                                        contractId = res.Result.ContractId;
                                        EmptyOrError = false;
                                        _this.BuyToAWB(EmptyOrError, contractId, temp);
                                    }
                                    else {
                                        _this.BuyToAWB(EmptyOrError, null, temp);
                                    }
                                }
                                else {
                                    _this.BuyToAWB(EmptyOrError, null, temp);
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.BuyToAWB(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
            case "CRM":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new CommonDomainService_1.CommonDomainService();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapCRMContractId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            _this.BluesnapContractService.get(contractId).subscribe(function (res) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (res) {
                                    if (!res.HasError) {
                                        contractId = res.Result.ContractId;
                                        EmptyOrError = false;
                                        _this.SubscribeToCRM(EmptyOrError, contractId, temp);
                                    }
                                    else {
                                        _this.SubscribeToCRM(EmptyOrError, null, temp);
                                    }
                                }
                                else {
                                    _this.SubscribeToCRM(EmptyOrError, null, temp);
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.SubscribeToCRM(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
            case "OTP":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new CommonDomainService_1.CommonDomainService();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapOneTimeContract;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            EmptyOrError = false;
                            _this.OneTimeBuy(EmptyOrError, contractId, temp);
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.OneTimeBuy(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
            case "INT":
                {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    var myService = new CommonDomainService_1.CommonDomainService();
                    myService.GetBlueSnapSecretToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
                        var temp = myResult.Result;
                        _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
                        var contractId = SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapInttraStockContractId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(contractId)) {
                            _this.BluesnapContractService.get(contractId).subscribe(function (res) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (res) {
                                    if (!res.HasError) {
                                        contractId = res.Result.ContractId;
                                        EmptyOrError = false;
                                        _this.SubscribeToInttra(EmptyOrError, contractId, temp);
                                    }
                                    else {
                                        _this.SubscribeToInttra(EmptyOrError, null, temp);
                                    }
                                }
                                else {
                                    _this.SubscribeToInttra(EmptyOrError, null, temp);
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.SubscribeToInttra(EmptyOrError, contractId, temp);
                        }
                    });
                    break;
                }
        }
    };
    HomeComponent.prototype.ManageBluesnapAccountClicked = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetBlueSnapToken(SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator_1.SessionLocator.TenantManagementJS.CountryName).subscribe(function (myResult) {
            var temp = myResult.Result;
            temp = temp.Token;
            _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
            var link = "https://checkout.bluesnap.com/jsp/account_login.jsp";
            if (!Tools_1.AppTool.IsNullOrEmpty(temp)) {
                link = "https://ws.bluesnap.com/jsp/entrance.jsp?target=cp&token=" + temp + "&pageToShow=my_account.jsp";
            }
            var win = window.open(link, '_blank');
            win.focus();
        });
    };
    HomeComponent.prototype.HelpButtonClicked = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Help Center", "Help Icon");
        window.open(Tools_1.AppTool.GetLogitudeURL() + 'TrainingResourcesHTML/TrainingResourcesMainPage.aspx?tempId=' + SessionInfo_1.SessionInfo.DocumentDownloadToken, '_blank');
    };
    HomeComponent.prototype.SignoutClicked = function () {
        SessionLocator_1.SessionLocator.Index = 0;
        SessionLocator_1.SessionLocator.AllSessions.forEach(function (item) {
            item.DestroySession();
        });
        this.SignoutCompleted.emit("event from child");
    };
    HomeComponent.prototype.SignoutClickedToBlockScreen = function () {
        this.IsApplicationBlocked = true;
        //SessionLocator.Index = 0;
        //SessionLocator.AllSessions.forEach((item) => {
        //    item.DestroyMenuReferences();
        //    item.DestroyListComponentReferences();
        //    item.MainMenuComponent.BlockScreenLoad();
        //});
        if (this.Tabs.length > 1) {
            for (var i = this.Tabs.length - 1; i > 0; i--) {
                var item = this.Tabs[i];
                this.Tabs.splice(i, 1);
                item.SessionComponent.DestroySession();
            }
        }
        this.SelectionChanged(this.Tabs[0]);
        this.Tabs[0].SessionComponent.MainMenuComponent.BlockScreenLoad();
        // this.SignoutCompleted.emit('Block');
    };
    HomeComponent.prototype.Clos555e = function (tabItem) {
        var itemIndex = this.Tabs.indexOf(tabItem);
        if (itemIndex > -1) {
            this.Tabs.splice(itemIndex, 1);
            if (tabItem.IsSelected) {
                this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
            }
            tabItem.SessionComponent.DestroySession();
            tabItem = null;
            if (this.Tabs.length <= 4) {
                if (!this.IsShowLastSuccessfulLoginComponent)
                    this.IsShowLastSuccessfulLoginComponent = true;
            }
        }
    };
    HomeComponent.prototype.OnClearCache = function () {
        window.ObjectFields = [];
        window.TextCodes = [];
        window.CachedTables = [];
        console.log("cache cleared!");
        SessionLocator_1.SessionLocator.StopApplicationTimers();
    };
    HomeComponent.prototype.TranslationClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.Title = "Translation";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Translations/TranslationComponent');
    };
    HomeComponent.prototype.DefaultTranslationClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.ShowHeaderButtons = true;
        logWindow.Title = "Default Translation";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Translations/DefaultTranslationComponent');
    };
    HomeComponent.prototype.LanguageSettingsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Language Settings";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/LanguageSettings/LanguageSettingsComponent');
    };
    HomeComponent.prototype.ConnectToDropBox = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetDropBoxAuthURI(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            var temp = myResult.Result;
            _this.setCookie("CurrentTenant", SessionLocator_1.SessionLocator.Tenant.toString(), 1);
            window.open(temp, 'Authenticate with Dropbox', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=1300,height=650');
        });
    };
    HomeComponent.prototype.CreateCommLogForDropBox = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetDropBoxComLog(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            var temp = myResult.Result;
            _this.messageWindow.Width = 300;
            _this.messageWindow.Height = 200;
            _this.messageWindow.Title = "DropBox Communicaiton Log";
            _this.messageWindow.Message = "Communicaiton Log Created For DropBox Successfully";
            _this.messageWindow.Show(_this.messageWindow.Message);
        });
    };
    HomeComponent.prototype.LoadSampleDataClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 470;
        logWindow.Height = 480;
        logWindow.Title = "Load Sample Data";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/LoadSampleDataComponent');
    };
    HomeComponent.prototype.ShowQueryBuilderClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
    };
    HomeComponent.prototype.FillStorageClicked = function () {
        try {
            var i;
            for (i = 1; i <= 10000; i++) {
                localStorage.setItem('test', new Array(i * 100000).join('a'));
            }
        }
        catch (error) {
            //console.log("test stopped at i: " + i);
            try {
                var j;
                for (j = 1; j <= 100; j++) {
                    localStorage.setItem('test2', new Array(j * 1000).join('a'));
                }
            }
            catch (error) {
                //console.log("test2 stopped at j: " + j);
                try {
                    var k;
                    for (k = 1; k <= 1000; k++) {
                        localStorage.setItem('test3', new Array(k).join('a'));
                    }
                }
                catch (error) {
                    //console.log("test3 stopped at k: " + k);
                    console.log("Local Storage is Full!");
                    //console.log("total storage: " + (i * 100000 + j * 1000 + k));
                    var total = 0;
                    for (var x in localStorage) {
                        var amount = (localStorage[x].length * 2) / 1024 / 1024;
                        if (amount)
                            total += amount;
                        console.log(x + " = " + amount.toFixed(2) + " MB");
                    }
                    console.log("Total: " + total.toFixed(2) + " MB");
                    // var used = Object.keys(window.localStorage).map(function (key) { return localStorage[key].length; }).reduce(function (a, b) { return a + b; });
                    //console.log("Used: " + used);
                }
            }
        }
    };
    HomeComponent.prototype.ShowDocTypesDefScreen = function () {
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 350;
        newWindow.Title = "Define document types for digital sign";
        //var windowArgs: any = {};
        //windowArgs.IsNew = true;
        //newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control);
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DigitalSignDocTypeComponent');
        newWindow.WindowClosed.subscribe(function ($event) {
            //if ($event == "MyShipmentAdded") {
            //    this.SelectedFilter = "My Shipments";
            //    this.LoadImporterShipments();
            //}
        });
    };
    HomeComponent.prototype.setCookie = function (name, value, expireDays, path) {
        if (path === void 0) { path = ''; }
        var d = new Date();
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        var expires = "expires=" + d.toUTCString();
        var cpath = path ? "; path=" + path : '';
        document.cookie = name + "=" + value + "; " + expires + cpath;
    };
    HomeComponent.prototype.ViewReleaseNotes = function () {
        window.open(ObjectsLocator_1.ObjectsLocator.GlobalSetting.ReleaseNotesURL);
    };
    HomeComponent.prototype.HideReleaseMessageClicked = function () {
        this.ShowNewReleaseToolTip = false;
        var service = new UserExtendedPMService_1.UserExtendedPMService();
        service.AddUserToReleaseNotesUsers(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                }
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], HomeComponent.prototype, "SignoutCompleted", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], HomeComponent.prototype, "AllLocations", void 0);
    __decorate([
        core_1.ViewChild("ApplicationLocation", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], HomeComponent.prototype, "ApplicationLocation", void 0);
    __decorate([
        core_1.HostListener('window:UnifaceRequestEvent', ['$event']),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], HomeComponent.prototype, "UnifaceRequest", null);
    HomeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './HomeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], HomeComponent);
    return HomeComponent;
}());
exports.HomeComponent = HomeComponent;
var SessionTabItem = /** @class */ (function () {
    function SessionTabItem() {
        this.IsSelected = false;
        this.IsSessionLoaded = false;
        this.Header = null;
        this.IconSource = null;
        this.IconSourceGray = null;
        this.DirectionId = null;
        this.TransportId = null;
        this.Index = SessionLocator_1.SessionLocator.Index;
    }
    SessionTabItem.prototype.ChangeSessionHeader = function (args) {
        if (args) {
            var Icon = args['Icon'];
            var Text = args['Text'];
            var TextCode = args['TextCode'];
            var MenuTextCode = args['MenuTextCode'];
            if (Text) {
                this.SetSessionTabHeader(Text);
            }
            if (MenuTextCode) {
                this.SetSessionTabIcon(Tools_1.AppTool.GetMainMenuIconCode(MenuTextCode));
                this.SetSessionTabHeader(TextCodeTranslator_1.TextCodeTranslator.Translate(MenuTextCode));
                this.DirectionId = null;
                this.TransportId = null;
            }
            if (args['DirectionId']) {
                this.DirectionId = args['DirectionId'] == "All" ? null : args['DirectionId'];
            }
            if (args['TransportId']) {
                this.TransportId = args['TransportId'] == "All" ? null : args['TransportId'];
            }
        }
    };
    SessionTabItem.prototype.SetSessionTabIcon = function (myIcon) {
        this.IconSource = "./Images/Menu/" + myIcon + ".png";
        this.IconSourceGray = "./Images/Menu/" + myIcon + ".Gray.png";
    };
    SessionTabItem.prototype.SetSessionTabHeader = function (myHeader) {
        this.Header = myHeader;
    };
    return SessionTabItem;
}());
exports.SessionTabItem = SessionTabItem;
var BluesnapParameters = /** @class */ (function () {
    function BluesnapParameters() {
    }
    Object.defineProperty(BluesnapParameters.prototype, "Token", {
        get: function () { return this.token; },
        set: function (value) { this.token = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BluesnapParameters.prototype, "Storeid", {
        get: function () { return this.storeid; },
        set: function (value) { this.storeid = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BluesnapParameters.prototype, "ContractId", {
        get: function () { return this.contractId; },
        set: function (value) { this.contractId = value; },
        enumerable: true,
        configurable: true
    });
    return BluesnapParameters;
}());
exports.BluesnapParameters = BluesnapParameters;
var TenantUserDataClass = /** @class */ (function () {
    function TenantUserDataClass() {
    }
    Object.defineProperty(TenantUserDataClass.prototype, "Id", {
        get: function () { return this.id; },
        set: function (value) { this.Id = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "DoBlocking", {
        get: function () { return this.doBlocking; },
        set: function (value) { this.doBlocking = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "TrailDaysLeft", {
        get: function () { return this.trailDaysLeft; },
        set: function (value) { this.trailDaysLeft = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "PaidDaysLeft", {
        get: function () { return this.paidDaysLeft; },
        set: function (value) { this.paidDaysLeft = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "ExpirationDaysLeft", {
        get: function () { return this.expirationDaysLeft; },
        set: function (value) { this.expirationDaysLeft = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "IsTrial", {
        get: function () { return this.isTrial; },
        set: function (value) { this.isTrial = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "PaymentFailure", {
        get: function () { return this.paymentFailure; },
        set: function (value) { this.paymentFailure = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "IsRecurring", {
        get: function () { return this.isRecurring; },
        set: function (value) { this.isRecurring = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "SuspendDaysLeft", {
        get: function () { return this.suspendDaysLeft; },
        set: function (value) { this.suspendDaysLeft = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "BlockType", {
        get: function () { return this.blockType; },
        set: function (value) { this.blockType = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "PaidUntilDate", {
        get: function () { return this.paidUntilDate; },
        set: function (value) { this.paidUntilDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantUserDataClass.prototype, "ExpirationDate", {
        get: function () { return this.expirationDate; },
        set: function (value) { this.expirationDate = value; },
        enumerable: true,
        configurable: true
    });
    return TenantUserDataClass;
}());
exports.TenantUserDataClass = TenantUserDataClass;
//# sourceMappingURL=HomeComponent.js.map