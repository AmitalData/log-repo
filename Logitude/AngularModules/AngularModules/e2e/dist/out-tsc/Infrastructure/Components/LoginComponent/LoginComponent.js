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
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var InfraSettings_1 = require("../../Utilities/InfraSettings");
var IndexedDbService_1 = require("../../Services/IndexedDbService");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var Tools_1 = require("../../Tools");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var LastFilterClass_1 = require("../../Utilities/LastFilterClass");
var ApplicationTimersManager_1 = require("../../Utilities/ApplicationTimersManager");
var CachedDataManager_1 = require("../../Utilities/CachedDataManager");
var EntityListService_1 = require("../../Services/EntityListService");
var LoginService_1 = require("../../Services/LoginService");
var UserPMService_1 = require("../../../Common/Services/StandardPMs/UserPMService");
var TenantPMService_1 = require("../../../Common/Services/StandardPMs/TenantPMService");
var AccountingSettingPMService_1 = require("../../../Common/Services/StandardPMs/AccountingSettingPMService");
var CustomsInterfaceSettingPMService_1 = require("../../../Common/Services/StandardPMs/CustomsInterfaceSettingPMService");
var SharedLogisticsSettingPMService_1 = require("../../Services/StandardPMs/SharedLogisticsSettingPMService");
var CreditLimitSettingPMService_1 = require("../../../Common/Services/StandardPMs/CreditLimitSettingPMService");
var LogitudeApplicationService_1 = require("../../Services/WebServices/LogitudeApplicationService");
var ObjectTableRulePMService_1 = require("../../Services/StandardPMs/ObjectTableRulePMService");
var ObjectTableRuleFieldPMService_1 = require("../../Services/StandardPMs/ObjectTableRuleFieldPMService");
var UserLastLoginPMService_1 = require("../../../Common/Services/StandardPMs/UserLastLoginPMService");
var InfrastructureDomainService_1 = require("../../Services/InfrastructureDomainService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var GlobalDomainService_1 = require("../../../Common/Services/GlobalDomainService");
var SATInterfaceSettingPMService_1 = require("../../../Invoice/Services/StandardPMs/SATInterfaceSettingPMService");
var Tools_2 = require("../../Tools");
var Guid_1 = require("../../Utilities/Guid");
var AmitalGatewayUtil_1 = require("../../Utilities/AmitalGatewayUtil");
var RulesValidator_1 = require("../../Validators/RulesValidator");
var Environment_1 = require("../../Locators/Environment");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var ObjectsUpdater_1 = require("../../Locators/ObjectsUpdater");
//import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
var UserExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var LoginComponent = /** @class */ (function () {
    function LoginComponent(logitudeApplicationService, loginService, IndexedDbService, entityResourceService, _applicationTimersManager, entityListService, _userLastLoginPMService) {
        this.logitudeApplicationService = logitudeApplicationService;
        this.loginService = loginService;
        this.IndexedDbService = IndexedDbService;
        this.entityResourceService = entityResourceService;
        this._applicationTimersManager = _applicationTimersManager;
        this.entityListService = entityListService;
        this._userLastLoginPMService = _userLastLoginPMService;
        this.Blocking = new core_1.EventEmitter();
        this.LoginCompleted = new core_1.EventEmitter();
        this.IsShowTenantList = false;
        this.IsProduction = false;
        this.LogoURL = "./Images/LoginScreen/header.jpg";
        this.SampleLogoURL = "./Images/ApplicationLogo/Angular/AngularLogo.png";
        this.blocked = false;
        this.dataLoaded = false;
        this.ShowTwoFactorAuthenScreen = false;
        this.InvalidVerificationCode = false;
        //public LogoURL: string = "./Images/ApplicationLogo/UnifreightLogo.jpg";
        //public SampleLogoURL: string = "./Images/ApplicationLogo/UnifreightLogo.jpg";
        this._objectTableRulePMService = new ObjectTableRulePMService_1.ObjectTableRulePMService();
        this._objectTableRuleFieldPMService = new ObjectTableRuleFieldPMService_1.ObjectTableRuleFieldPMService();
        this.IsShowLoginForm = false;
        this.ShowTenantList = false;
        this.LoggedUserData = null;
        this.TotalNumberOfLoads = 0;
        this.LoadSize = 0;
        this.LastLoadSize = 0;
        this.LoadingCounter = 0;
        this.CompletedLoadsCount = 0;
        this.DefultText = "test";
        this.IsProduction = SessionLocator_1.SessionLocator.IsProduction;
        this.ShowLoginBusyIndicator = false;
        this.HideLoginForm = false;
        this.HideTenantForm = true;
        this.LoginFailed = false;
        this.LoginParams = new LoginService_1.LoginParameters();
        this.HidePendingLoading = true;
        var temp = window.sessionStorage.getItem("LogoURL");
        var LogoCode = window.sessionStorage.getItem("LogoCode");
        if (temp) {
            this.LogoURL = temp;
            this.SampleLogoURL = temp;
        }
        else if (LogoCode) {
            this.LogoURL = Tools_1.AppTool.GetEnvironmentLogo(LogoCode);
            this.SampleLogoURL = Tools_1.AppTool.GetEnvironmentLogo(LogoCode);
        }
        window.Statuses = [];
        window.Ports = [];
        window.TransportModes = [];
        window.Directions = [];
        window.Cards = [];
        window.MenusTables = [];
        window.TextCodesTranslations = [];
        window.Screens = [];
        window.ScreenFields = [];
        window.ObjectTableTabs = [];
        window.ObjectFields = [];
        window.PreDefinedFilters = [];
        window.TenantTranslations = [];
        window.TenantLanguageTranslations = [];
        window.ObjectTableRules = [];
        window.ObjectTableRuleFields = [];
        window.ObjectTableRules = [];
        window.ObjectTables = [];
        window.CachedTables = [];
        window.TranslationsCache = [];
        window.TextCodes = [];
        window.TextCodesCache = [];
        window.ObjectFieldsCache = [];
        window.Tips = [];
        window.TipsVisibilities = [];
        window.DWObjectFields = [];
        this.myInfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this.sATInterfaceSettingPMService = new SATInterfaceSettingPMService_1.SATInterfaceSettingPMService();
        this.UserExtendedPMService = new UserExtendedPMService_1.UserExtendedPMService();
        //FileLoader.LoadFroalaResources();
    }
    LoginComponent.prototype.ngOnInit = function () {
        this.StartLoginProcess();
    };
    LoginComponent.prototype.StartLoginProcess = function () {
        var url = window.location.href;
        if (url && url.indexOf('localhost') > -1) {
            this.Email = "angular@fnarsoft.com";
            this.Password = "1";
            this.IsShowLoginForm = true;
        }
        this.authHeader = new http_1.Headers();
        this.authHeader.append('Content-Type', 'application/json');
        this.authHeader.append('Accept', 'application/json');
        this.loginService.AuthHeader = this.authHeader;
        var isUseDefultLogin = false;
        window.indexedDB.deleteDatabase("MyDatabase");
        var data = window.sessionStorage.getItem('userdata');
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            if (SessionLocator_1.SessionLocator.ExternalParams) {
                if (SessionLocator_1.SessionLocator.ExternalParams.Menu) {
                    var menuName = SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase();
                    if (menuName == "logbox" || menuName == "dapp" || menuName == "protractor" || menuName == "preq") {
                        if (menuName == "preq") {
                            this.LoginCompleted.emit("IgnoreTerms");
                            return;
                        }
                        isUseDefultLogin = true;
                    }
                    else if (SessionLocator_1.SessionLocator.ExternalParams.OneTimePasswordId) {
                        this.HideLoginForm = true;
                        this.HideTenantForm = true;
                        this.ShowLoginBusyIndicator = true;
                        this.OneUsePasswordMethod();
                    }
                    else
                        document.location.href = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "Login.aspx";
                }
            }
            else {
                document.location.href = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "Login.aspx";
            }
        }
        if (!SessionLocator_1.SessionLocator.IsExternalParams || isUseDefultLogin) {
            if (data) {
                this.HideLoginForm = true;
                this.HideTenantForm = true;
                this.HidePendingLoading = true;
                this.ShowLoginBusyIndicator = true;
                var userData = JSON.parse(data);
                this.StartLoading(userData);
            }
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
    LoginComponent.prototype.StartLoading = function (userData) {
        var _this = this;
        if (userData) {
            this.HideLoginForm = true;
            this.HideTenantForm = true;
            this.HidePendingLoading = true;
            this.ShowLoginBusyIndicator = true;
            SessionInfo_1.SessionInfo.LoggedUserEmail = userData.UserName;
            SessionInfo_1.SessionInfo.LoggedUserId = userData.Id;
            SessionInfo_1.SessionInfo.Token = userData.Token;
            SessionInfo_1.SessionInfo.DocumentDownloadToken = userData.DocumentDownloadToken;
            SessionInfo_1.SessionInfo.SessionTimeout = userData.SessionTimeout;
            SessionInfo_1.SessionInfo.WebTokenExpirationWarningInMinutes = userData.WebTokenExpirationWarningInMinutes;
            SessionInfo_1.SessionInfo.WebTokenLifeTimeInMinutes = userData.WebTokenLifeTimeInMinutes;
            SessionInfo_1.SessionInfo.KeepUserLoggedIn = userData.KeepUserLoggedIn;
            SessionInfo_1.SessionInfo.LastLoginDateTime = userData.LastLoginDateTime;
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse = userData.AmitalBrowserInUse;
            this.authHeader.append('token', userData.Token);
            if (userData.TwoFactorkey) {
                window.localStorage.setItem('TwoFactorkey', userData.TwoFactorkey);
            }
            if (userData.CurrentTenant != null) {
                SessionInfo_1.SessionInfo.LoggedUserTenant = Number(userData.CurrentTenant + "");
            }
            if (SessionInfo_1.SessionInfo.LoggedUserTenant != null) {
                this.loginService.AuthHeader = this.authHeader;
                this.loginService.CurrentTenant = userData.CurrentTenant;
                this.loginService.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                this.loginService.LoggedUserEmail = SessionInfo_1.SessionInfo.LoggedUserEmail;
                this.loginService.GetLoggedUser().subscribe(function (myResult) {
                    var iGlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
                    iGlobalDomainService.GetTenantManagementJS(SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (myResponse) {
                        ObjectsUpdater_1.ObjectsUpdater.UpdateLoggedUserPM(myResult);
                        ObjectsUpdater_1.ObjectsUpdater.UpdateTenantManagementJS(myResponse.Result);
                        SessionInfo_1.SessionInfo.LoggedUserPM = myResult;
                        if (ObjectsLocator_1.ObjectsLocator.LoggedUserPM.ExpirationDate != null && Tools_2.DateTool.GetDateParts(ObjectsLocator_1.ObjectsLocator.LoggedUserPM.ExpirationDate).DateTicks < Tools_2.DateTool.GetCurrentDateAsUtc().valueOf()) {
                            SessionLocator_1.SessionLocator.BlockType = "user";
                        }
                        //   else {
                        _this.CheckTenantBlocking(userData);
                        //   }
                    });
                });
            }
        }
        window.sessionStorage.setItem("userdata", "");
    };
    LoginComponent.prototype.OneUsePasswordMethod = function () {
        var _this = this;
        this.loginService.GetOneUsePassword().subscribe(function (userData) {
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                var message = "Can't use this key (" + SessionLocator_1.SessionLocator.ExternalParams.OneTimePasswordId + ") again because you used it before ";
                if (userData && userData.ExceptionMessage)
                    message = userData.ExceptionMessage;
                alert(message);
                document.location.href = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "Login.aspx";
                SessionLocator_1.SessionLocator.ExternalParams.OneTimePasswordId = null;
            }
            else {
                _this.StartLoading(userData);
            }
            _this.HidePendingLoading = true;
        });
    };
    LoginComponent.prototype.LoginClicked = function () {
        if (this.Email != null && this.Password != null) {
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
                ClientType: "Web",
            };
            this.HidePendingLoading = false;
            this.PostUserValidation(this.LoginParams);
        }
    };
    LoginComponent.prototype.PostUserValidation = function (loginParameters) {
        var _this = this;
        this.loginService.PostUserValidation(loginParameters).subscribe(function (userData) {
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                _this.LoginFailed = true;
                _this.HidePendingLoading = true;
                if (userData)
                    alert(userData.ExceptionMessage);
            }
            else {
                _this.TenantList = userData.CompanyLogins;
                _this.HideLoginForm = true;
                _this.HideTenantForm = false;
                _this.HidePendingLoading = false;
                _this.LoginParams = loginParameters;
                if (_this.TenantList.length === 1) {
                    _this.HideTenantForm = true;
                    _this.Tenant = _this.TenantList[0].Tenant;
                    _this.loginService.CurrentTenant = _this.Tenant;
                    var f = { valid: true };
                    _this.ChooseTenant(f, null);
                }
                else {
                    var i = 0;
                    _this.TenantList.forEach(function (item) {
                        i += 1;
                        item.Id = i;
                    });
                    _this.SelectedCompany = _this.TenantList[0];
                    _this.ShowTenantList = true;
                    _this.HidePendingLoading = true;
                    _this.HideTenantForm = true;
                }
            }
            _this.HidePendingLoading = true;
        });
    };
    LoginComponent.prototype.TenantListChangeSelected = function (value) {
        this.SelectedCompany = this.TenantList.filter(function (d) { return d.Id == value; })[0];
    };
    LoginComponent.prototype.ContinueClicked = function () {
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
                ClientType: "Web",
            };
            this.loginService.CurrentTenant = this.Tenant;
            var f = { valid: true };
            this.ChooseTenant(f, null);
            SessionInfo_1.SessionInfo.LoggedUserCardId = this.SelectedCompany.CardId;
            SessionInfo_1.SessionInfo.LoggedUserCardType = this.SelectedCompany.CardType;
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
            if (userData.TwoFactorkey) {
                window.localStorage.setItem('TwoFactorkey', userData.TwoFactorkey);
            }
            if (!userData.IsTwoFactorAuthenticationRequired || userData.IsTwoFactorAuthenticationRequired == false) {
                _this.StartLoading(userData);
            }
            else {
                _this.LoggedUserData = userData;
                _this.UserMobileNumber = userData.UserMobileNumber;
                _this.ShowTwoFactorAuthenScreen = true;
                //alert('Two factor authentication');
            }
        });
    };
    LoginComponent.prototype.VerifyClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VerficationCode)) {
            this.HidePendingLoading = false;
            this.loginService.PostAuthenticationDeviceVerificationCode(this.LoggedUserData.TwoFactorkey, this.VerficationCode, Number(this.LoggedUserData.CurrentTenant + "")).subscribe(function (res) {
                _this.HidePendingLoading = true;
                if (res == true) {
                    _this.ShowTwoFactorAuthenScreen = false;
                    //this.StartLoading(this.LoggedUserData);
                    _this.PostLoginData();
                }
                else {
                    _this.InvalidVerificationCode = true;
                }
            });
        }
    };
    LoginComponent.prototype.ResendVerificationCodeClicked = function () {
        var _this = this;
        this.HidePendingLoading = false;
        this.loginService.PostResendAuthenticationDeviceVerificationCode(this.LoggedUserData.TwoFactorkey, this.LoggedUserData.Id, Number(this.LoggedUserData.CurrentTenant + "")).subscribe(function (res) {
            _this.HidePendingLoading = true;
            if (res) {
            }
            //if (res == true) {
            //    this.ShowTwoFactorAuthenScreen = false;
            //    this.StartLoading(this.LoggedUserData);
            //}
            //else {
            //    this.InvalidVerificationCode = true;
            //}
        });
    };
    LoginComponent.prototype.LoadClosedTablesToWindow = function (CurrentTenant) {
        var _this = this;
        this.IndexedDbService.InitializeIndexedDB().subscribe(function (response) {
            InfraSettings_1.InfraSettings.IndexedDbService = IndexedDbService_1.IndexedDbService;
            _this.loginService.AuthHeader = _this.authHeader;
            _this.loginService.CurrentTenant = CurrentTenant;
            _this.loginService.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserId;
            _this.loginService.LoggedUserEmail = SessionInfo_1.SessionInfo.LoggedUserEmail;
            // UserPM
            _this.loginService.GetLoggedUser().subscribe(function (myResult) {
                var myUserPMService = new UserPMService_1.UserPMService();
                SessionInfo_1.SessionInfo.LoggedUserPM = myUserPMService.MapJsonToEntityPM(myResult);
                if (SessionInfo_1.SessionInfo.LoggedUserPM) {
                    _this.loginService.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserPM.Id;
                    SessionInfo_1.SessionInfo.LoggedUserId = SessionInfo_1.SessionInfo.LoggedUserPM.Id;
                }
                _this.IncreaseProgressBar();
                //1
                // TenantPM
                _this.loginService.GetLoggedTenant().subscribe(function (myResult) {
                    var myTenantPMService = new TenantPMService_1.TenantPMService();
                    InfraSettings_1.InfraSettings.TenantPM = myTenantPMService.MapJsonToEntityPM(myResult);
                    _this.IncreaseProgressBar();
                    //2
                });
                CachedDataManager_1.CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(function (response) {
                    _this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(function (response) {
                        _this.IncreaseProgressBar("General Resources");
                        //26
                    });
                });
                // LastFilters
                _this.loginService.GetLastFilters().subscribe(function (myResult) {
                    LastFilterClass_1.LastFilterClass.MapJSON(myResult);
                    _this.IncreaseProgressBar();
                    //3
                });
                // TenantManagementPM
                var iGlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
                iGlobalDomainService.GetTenantManagementJS(SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (myResponse) {
                    ObjectsUpdater_1.ObjectsUpdater.UpdateTenantManagementJS(myResponse.Result);
                    _this.IncreaseProgressBar();
                    //4
                    _this.loginService.GetPrivateLableById(SessionLocator_1.SessionLocator.TenantManagementJS.PrivateLabelId).subscribe(function (Result) {
                        ObjectsLocator_1.ObjectsLocator.UpdatePrivateLableSettings(Result);
                        SessionLocator_1.SessionLocator.PrivateLableSettings = Result;
                        _this.IncreaseProgressBar();
                        Environment_1.Environment.SetFavIconAndTitle();
                        //5
                    });
                });
                //this.loginService.GetTenantManagement().subscribe(myResult => {
                //});
                _this.myInfrastructureDomainService.GetAllowedFeaturesForLoggedUser().subscribe(function (myResponse) {
                    _this.IncreaseProgressBar();
                    //6
                    // Ayman: please don't modify this (24)
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "READ") && ObjectsLocator_1.ObjectsLocator.GlobalSetting && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment != 'customs') {
                        var myCreditLimitSettingPMService = new CreditLimitSettingPMService_1.CreditLimitSettingPMService();
                        myCreditLimitSettingPMService.get(CurrentTenant + "").subscribe(function (myResponse) {
                            ObjectsLocator_1.ObjectsLocator.UpdateCreditLimitSettingPM(myResponse.Result);
                            _this.IncreaseProgressBar();
                            //24
                        });
                    }
                    else {
                        _this.IncreaseProgressBar();
                        //24
                    }
                });
                _this.loginService.GetQueries().subscribe(function (myResult) {
                    window.Queries = myResult;
                    _this.IncreaseProgressBar();
                    //7
                });
                _this.loginService.GetStatuses().subscribe(function (myResult) {
                    window.Statuses = myResult;
                    _this.IncreaseProgressBar();
                    //8
                });
                _this.loginService.GetPreDefinedFilters().subscribe(function (myResult) {
                    window.PreDefinedFilters = myResult;
                    _this.IncreaseProgressBar();
                    //9
                });
                _this.loginService.GetTenantTranslations().subscribe(function (myResult) {
                    window.TenantTranslations = myResult;
                    _this.IncreaseProgressBar();
                    //10
                });
                _this.loginService.GetTransportModes().subscribe(function (myResult) {
                    window.TransportModes = myResult;
                    _this.IncreaseProgressBar();
                    //11
                });
                _this.loginService.GetDirections().subscribe(function (myResult) {
                    window.Directions = myResult;
                    _this.IncreaseProgressBar();
                    //12
                });
                _this.loginService.GetMenusTables().subscribe(function (myResult) {
                    window.MenusTables = myResult;
                    _this.IncreaseProgressBar();
                    //13
                });
                _this.loginService.GetObjectTables().subscribe(function (myResult) {
                    window.ObjectTables = myResult;
                    _this.IncreaseProgressBar();
                    //14
                });
                _this.loginService.GetScreens().subscribe(function (myResult) {
                    window.Screens = myResult;
                    _this.IncreaseProgressBar();
                    //15
                });
                _this.loginService.GetScreenFields().subscribe(function (myResult) {
                    window.ScreenFields = myResult;
                    _this.IncreaseProgressBar();
                    //16
                });
                _this.loginService.GetObjectTableTabs().subscribe(function (myResult) {
                    window.ObjectTableTabs = myResult;
                    _this.IncreaseProgressBar();
                    //17
                });
                _this.loginService.GetAccountingSetting().subscribe(function (myResult) {
                    var myAccountingSettingPM = null;
                    if (myResult) {
                        var myAccountingSettingPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
                        myAccountingSettingPM = myAccountingSettingPMService.MapJsonToEntityPM(myResult);
                    }
                    ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(myAccountingSettingPM);
                    _this.IncreaseProgressBar();
                    //18
                    var myAccountingSystemCode = null;
                    if (ObjectsLocator_1.ObjectsLocator.AccountingSettingPM) {
                        myAccountingSystemCode = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.AccountingSystemCode;
                        if (ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                            var myCommonDomain = new CommonDomainService_1.CommonDomainService();
                            myCommonDomain.GetAllVatTypesGroups().subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    SessionLocator_1.SessionLocator.AllVatTypesGroups = myResponse.Result;
                                }
                            });
                        }
                    }
                    _this.loginService.GetAccountingSystem(myAccountingSystemCode).subscribe(function (myResult2) {
                        SessionLocator_1.SessionLocator.AccountingSystemPM = myResult2;
                        _this.IncreaseProgressBar();
                        //19
                    });
                });
                _this.loginService.GetCustomsInterfaceSetting().subscribe(function (myResult) {
                    if (myResult) {
                        var myCustomsInterfaceSettingPMService = new CustomsInterfaceSettingPMService_1.CustomsInterfaceSettingPMService();
                        ObjectsUpdater_1.ObjectsUpdater.UpdateCustomsInterfaceSettingPM(myCustomsInterfaceSettingPMService.MapJsonToEntityPM(myResult));
                    }
                    _this.IncreaseProgressBar();
                });
                _this.loginService.GetSharedLogisticsSetting().subscribe(function (myResult) {
                    if (myResult) {
                        var mySharedLogisticsSettingPMService = new SharedLogisticsSettingPMService_1.SharedLogisticsSettingPMService();
                        ObjectsUpdater_1.ObjectsUpdater.UpdateSharedLogisticsSettingPM(mySharedLogisticsSettingPMService.MapJsonToEntityPM(myResult));
                    }
                    _this.IncreaseProgressBar();
                });
                _this.loginService.GetGlobalSetting().subscribe(function (myResult) {
                    // Accounting - Abdullah
                    if (InfraSettings_1.InfraSettings.TenantPM) {
                        myResult.LayoutDirection = InfraSettings_1.InfraSettings.TenantPM.LayoutDirection ? InfraSettings_1.InfraSettings.TenantPM.LayoutDirection.toLowerCase() : InfraSettings_1.InfraSettings.TenantPM.LayoutDirection;
                    }
                    //
                    ObjectsLocator_1.ObjectsLocator.UpdateGlobalSetting(myResult);
                    _this.IncreaseProgressBar();
                    Environment_1.Environment.SetFavIconAndTitle();
                    //20
                });
                _this.loginService.GetTenantSetting().subscribe(function (myResult) {
                    SessionLocator_1.SessionLocator.TenantSettings = myResult;
                    _this.IncreaseProgressBar();
                    //21
                });
                _this.loginService.GetTips().subscribe(function (myResult) {
                    window.Tips = myResult;
                    _this.IncreaseProgressBar();
                    //22
                });
                _this.loginService.GetTipsVisibilities().subscribe(function (myResult) {
                    window.TipsVisibilities = myResult;
                    _this.IncreaseProgressBar();
                    //23
                });
                if (!SessionLocator_1.SessionLocator.UseCachedData) {
                    _this.loginService.GetObjectFields().subscribe(function (myResult) {
                        if (!SessionLocator_1.SessionLocator.UseCachedData) {
                            window.ObjectFields = myResult;
                        }
                        _this.IncreaseProgressBar();
                    });
                    _this.loginService.GetTextCodesTranslations().subscribe(function (myResult) {
                        if (!SessionLocator_1.SessionLocator.UseCachedData) {
                            window.TextCodesTranslations = myResult;
                        }
                        window.TranslationsCache = [];
                        _this.IncreaseProgressBar();
                    });
                }
                else {
                    _this.loginService.GetTenantTextCode().subscribe(function (myResult) {
                        if (myResult) {
                            window.TextCodes = window.TextCodes.concat(myResult);
                            _this.IncreaseProgressBar();
                            //25
                        }
                    });
                }
                //CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(response => {
                //    this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(response => {
                //        this.IncreaseProgressBar();
                //        //26
                //    });
                //});
            });
        });
        this._objectTableRulePMService.getAllByTenant(CurrentTenant).subscribe(function (response) {
            if (response) {
                window.ObjectTableRules = response.Result;
            }
            _this.IncreaseProgressBar();
            //27
        });
        this._objectTableRuleFieldPMService.getAllByTenant(CurrentTenant).subscribe(function (response) {
            if (response) {
                window.ObjectTableRuleFields = response.Result;
            }
            _this.IncreaseProgressBar();
            //28
        });
        this._userLastLoginPMService.GetUserLastLogin(SessionInfo_1.SessionInfo.LoggedUserId, CurrentTenant).subscribe(function (response) {
            if (!response.HasError && response.Result) {
                var lastloginPM = response.Result;
                var computerId = SessionLocator_1.SessionLocator.GetComputerIdFromStorage();
                if (Tools_1.AppTool.IsNullOrEmpty(computerId)) {
                    computerId = Guid_1.Guid.newGuid();
                    SessionLocator_1.SessionLocator.StoreLogedComputerId(computerId);
                }
                lastloginPM.ComputerId = computerId;
                _this._userLastLoginPMService.update(lastloginPM).subscribe(function (response) {
                    _this.IncreaseProgressBar();
                    //29
                });
            }
            else {
                _this.IncreaseProgressBar();
                //29
            }
        });
        this.loginService.GeLoggedTenantObjectFields().subscribe(function (response) {
            if (response) {
                window.ObjectFields = window.ObjectFields.concat(response);
            }
            _this.IncreaseProgressBar();
            //30
        });
        this.loginService.GetTenantLanguageTranslations().subscribe(function (myResult) {
            window.TenantLanguageTranslations = myResult;
            _this.IncreaseProgressBar();
            //31
        });
        this.sATInterfaceSettingPMService.get(CurrentTenant).subscribe(function (myResult) {
            SessionLocator_1.SessionLocator.SATInterfaceSettings = myResult.Result;
            _this.IncreaseProgressBar();
            //32
        });
        this.myInfrastructureDomainService.GetFeatureToggles().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                SessionLocator_1.SessionLocator.FeatureToggles = myResponse.Result;
                _this.IncreaseProgressBar();
                //33
            }
        });
        this.UserExtendedPMService.CheckUserReleaseNotesToolTip(SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                SessionLocator_1.SessionLocator.ShowUserNewReleaseToolTip = myResponse.Result;
                _this.IncreaseProgressBar();
                //34
            }
        });
        this.myInfrastructureDomainService.getDWObjectFieldsWithChildrenByDWTableId("Fact_Shipments").subscribe(function (Result) {
            //var ObsList = [];
            if (!Result.HasError) {
                window.DWObjectFields = Result.Result;
                _this.IncreaseProgressBar();
                //Result.Result.forEach((field) => {
                //    if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
                //        var view = new DWObjectFieldsDetails(field, null);
                //        view.ParentDataTypeCode = field.DataTypeCode;
                //        ObsList.push(view);
                //    }
                //});
            }
        });
        //this._objectTableRuleFieldPMService.getAllByTenant(CurrentTenant).subscribe(myResult => {
        //    window.ObjectTableRulePMs = myResult;
        //    this.IncreaseProgressBar();
        //    //11
        //});
    };
    LoginComponent.prototype.CheckTenantBlocking = function (userData) {
        var isSystemBlocked = false;
        var todayDateTicks = Tools_2.DateTool.GetCurrentDateAsUtc().valueOf();
        if (SessionLocator_1.SessionLocator.TenantManagementJS.PaymentFailure) {
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDate)) {
                isSystemBlocked = true;
                SessionLocator_1.SessionLocator.BlockType = "company";
            }
            else if (Tools_2.DateTool.GetDateParts(SessionLocator_1.SessionLocator.TenantManagementJS.SuspendDate).DateTicks < todayDateTicks) {
                isSystemBlocked = true;
                SessionLocator_1.SessionLocator.BlockType = "suspend";
            }
        }
        if (!isSystemBlocked) {
            if (SessionLocator_1.SessionLocator.TenantManagementJS.IsTrial) {
                if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TrialEndDate)) {
                    isSystemBlocked = true;
                    SessionLocator_1.SessionLocator.BlockType = "company";
                }
                else if (Tools_2.DateTool.GetDateParts(SessionLocator_1.SessionLocator.TenantManagementJS.TrialEndDate).DateTicks < todayDateTicks) {
                    isSystemBlocked = true;
                    SessionLocator_1.SessionLocator.BlockType = "company";
                }
            }
        }
        if (!isSystemBlocked) {
            if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PaidUntilDate)) {
                if (Tools_2.DateTool.GetDateParts(SessionLocator_1.SessionLocator.TenantManagementJS.PaidUntilDate).DateTicks < todayDateTicks && !SessionLocator_1.SessionLocator.TenantManagementJS.IsRecurring) {
                    isSystemBlocked = true;
                    SessionLocator_1.SessionLocator.BlockType = "company";
                }
            }
        }
        this.LoadClosedTablesToWindow(userData.CurrentTenant);
    };
    LoginComponent.prototype.IncreaseProgressBar = function (loadOPName) {
        var _this = this;
        if (loadOPName === void 0) { loadOPName = ""; }
        console.log(loadOPName + "==>Completed Login Loads Count: " + this.CompletedLoadsCount);
        if (this.TotalNumberOfLoads == 0) {
            this.TotalNumberOfLoads = 36;
            if (!SessionLocator_1.SessionLocator.UseCachedData) {
                this.TotalNumberOfLoads += 1;
            }
            this.LoadSize = 100 / this.TotalNumberOfLoads;
            if (this.LoadSize.toString().indexOf(".") > -1) {
                this.LoadSize = +this.LoadSize.toString().split(".")[0];
                this.LastLoadSize = 100 - ((this.TotalNumberOfLoads - 1) * this.LoadSize);
            }
            else {
                this.LastLoadSize = this.LoadSize;
            }
        }
        this.CompletedLoadsCount++;
        if (this.CompletedLoadsCount <= this.TotalNumberOfLoads) {
            var elem = document.getElementById("myBar");
            var length = this.LoadSize;
            if (this.CompletedLoadsCount == this.TotalNumberOfLoads) {
                length = this.LastLoadSize;
            }
            for (var i = 1; i <= length; i++) {
                if (this.LoadingCounter < 100) {
                    this.LoadingCounter = this.LoadingCounter + 1;
                    elem.style.width = this.LoadingCounter + '%';
                }
            }
            if (this.CompletedLoadsCount == this.TotalNumberOfLoads) {
                console.log("===============>Changing Page<==================");
                ServiceLocator_1.ServiceLocator.RulesValidator = new RulesValidator_1.RulesValidator();
                this.timerToken = setTimeout(function () { return _this.ChangePage(); }, 1000);
            }
        }
        //console.log("login load count:" + this.CompletedLoadsCount);
    };
    LoginComponent.prototype.ChangePage = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        this._applicationTimersManager.StartApplicationTimers();
        this.LoginCompleted.emit("event");
        if (SessionLocator_1.SessionLocator.UseCachedData && SessionInfo_1.SessionInfo.LoggedUserTenant != 0) {
            CachedDataManager_1.CachedDataManager.GetCacheOnClientTablesData(this.entityListService);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LoginComponent.prototype, "Blocking", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LoginComponent.prototype, "LoginCompleted", void 0);
    LoginComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LoginComponent.html',
            providers: [ApplicationTimersManager_1.ApplicationTimersManager, LogitudeApplicationService_1.LogitudeApplicationService, UserLastLoginPMService_1.UserLastLoginPMService]
        }),
        __metadata("design:paramtypes", [LogitudeApplicationService_1.LogitudeApplicationService, LoginService_1.LoginService, IndexedDbService_1.IndexedDbService, EntityResourceService_1.EntityResourceService, ApplicationTimersManager_1.ApplicationTimersManager, EntityListService_1.EntityListService,
            UserLastLoginPMService_1.UserLastLoginPMService])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=LoginComponent.js.map