declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Headers } from '@angular/http';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { IndexedDbService } from '../../Services/IndexedDbService';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { AppTool } from '../../Tools';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { LastFilterClass } from '../../Utilities/LastFilterClass';
import { ApplicationTimersManager } from '../../Utilities/ApplicationTimersManager';
import { CachedDataManager } from '../../Utilities/CachedDataManager';
import { EntityListService } from '../../Services/EntityListService';
import { LoginService, LoginParameters } from '../../Services/LoginService';
import { UserPMService } from '../../../Common/Services/StandardPMs/UserPMService';
import { TenantPMService } from '../../../Common/Services/StandardPMs/TenantPMService';
import { AccountingSettingPMService } from '../../../Common/Services/StandardPMs/AccountingSettingPMService';
import { CustomsInterfaceSettingPMService } from '../../../Common/Services/StandardPMs/CustomsInterfaceSettingPMService';
import { SharedLogisticsSettingPMService } from '../../Services/StandardPMs/SharedLogisticsSettingPMService';
import { CreditLimitSettingPMService } from '../../../Common/Services/StandardPMs/CreditLimitSettingPMService';
import { LogitudeApplicationService } from '../../Services/WebServices/LogitudeApplicationService';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ObjectTableRulePMService } from '../../Services/StandardPMs/ObjectTableRulePMService';
import { ObjectTableRuleFieldPMService } from '../../Services/StandardPMs/ObjectTableRuleFieldPMService';
import { UserLastLoginPMService } from '../../../Common/Services/StandardPMs/UserLastLoginPMService';
import { UserLastLoginPM } from '../../../Common/EntityPMs/UserLastLoginPM';
import { InfrastructureDomainService } from '../../Services/InfrastructureDomainService';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { GlobalDomainService } from '../../../Common/Services/GlobalDomainService';
import { SATInterfaceSettingPMService } from '../../../Invoice/Services/StandardPMs/SATInterfaceSettingPMService';
import { DateTool, FileLoader } from '../../Tools';
import { Guid } from '../../Utilities/Guid';
import { AmitalGatewayUtil } from '../../Utilities/AmitalGatewayUtil';
declare var changeFavicon: any;
declare var changeTitle: any;
import { RulesValidator } from '../../Validators/RulesValidator';
import { Environment } from '../../Locators/Environment';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { ServiceLocator } from '../../Locators/ServiceLocator';
import { ObjectsUpdater } from '../../Locators/ObjectsUpdater';
//import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { UserExtendedPMService } from '../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import { GeneralDomainService } from '../../../Infrastructure/Services/GeneralDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './LoginComponent.html',
    providers: [ApplicationTimersManager, LogitudeApplicationService, UserLastLoginPMService]
})

export class LoginComponent implements OnInit {
    @Output() Blocking: EventEmitter<any> = new EventEmitter();
    @Output() LoginCompleted: EventEmitter<any> = new EventEmitter();
    public Email: string;
    public Password: string;
    public VerficationCode: string;
    public LoginParams: LoginParameters;
    public HideLoginForm: boolean;
    public HideTenantForm: boolean;
    public Tenant: number;
    public TenantList: any[];
    public ShowLoginBusyIndicator: boolean;
    public LoginFailed: boolean;
    public HidePendingLoading: boolean;
    public IsShowTenantList: boolean = false;
    public IsProduction: boolean = false;
    public UserData: any;
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public SampleLogoURL: string = "./Images/ApplicationLogo/Angular/AngularLogo.png";
    public blocked: boolean = false;
    public dataLoaded: boolean = false;
    public ShowTwoFactorAuthenScreen: boolean = false;
    public InvalidVerificationCode: boolean = false;
    public DefultText: string;
    //public LogoURL: string = "./Images/ApplicationLogo/UnifreightLogo.jpg";
    //public SampleLogoURL: string = "./Images/ApplicationLogo/UnifreightLogo.jpg";
    private _objectTableRulePMService: ObjectTableRulePMService = new ObjectTableRulePMService();
    private _objectTableRuleFieldPMService: ObjectTableRuleFieldPMService = new ObjectTableRuleFieldPMService();
    private myInfrastructureDomainService: InfrastructureDomainService;
    //public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    private sATInterfaceSettingPMService: SATInterfaceSettingPMService;
    private UserExtendedPMService: UserExtendedPMService;
    private generalDomainService: GeneralDomainService;
    constructor(private logitudeApplicationService: LogitudeApplicationService, private loginService: LoginService, public IndexedDbService: IndexedDbService, private entityResourceService: EntityResourceService, private _applicationTimersManager: ApplicationTimersManager, public entityListService: EntityListService,
        private _userLastLoginPMService: UserLastLoginPMService
    ) {


        this.DefultText = "test";
        this.IsProduction = SessionLocator.IsProduction;
        this.ShowLoginBusyIndicator = false;
        this.HideLoginForm = false;
        this.HideTenantForm = true;
        this.LoginFailed = false;
        this.LoginParams = new LoginParameters();
        this.HidePendingLoading = true;
        var temp = window.sessionStorage.getItem("LogoURL");
        var LogoCode = window.sessionStorage.getItem("LogoCode");
        if (temp) {
            this.LogoURL = temp;
            this.SampleLogoURL = temp;
        }
        else if (LogoCode) {
            this.LogoURL = AppTool.GetEnvironmentLogo(LogoCode);
            this.SampleLogoURL = AppTool.GetEnvironmentLogo(LogoCode);
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
        window.ObjectFieldModifications = [];

        this.myInfrastructureDomainService = new InfrastructureDomainService();
        this.sATInterfaceSettingPMService = new SATInterfaceSettingPMService();
        this.UserExtendedPMService = new UserExtendedPMService();
        this.generalDomainService = new GeneralDomainService();
        //FileLoader.LoadFroalaResources();
    }

    idxdb: IDBOpenDBRequest;
    public authHeader;
    ngOnInit() {
        this.StartLoginProcess();
    }
    IsShowLoginForm: boolean = false;

    StartLoginProcess() {

        var url = window.location.href;
        if (url && url.indexOf('localhost') > -1) {
            this.Email = "angular@fnarsoft.com";
            this.Password = "1";
            this.IsShowLoginForm = true;
        }

        this.authHeader = new Headers();
        this.authHeader.append('Content-Type', 'application/json');
        this.authHeader.append('Accept', 'application/json');
        this.loginService.AuthHeader = this.authHeader;
        var isUseDefultLogin: boolean = false;
        window.indexedDB.deleteDatabase("MyDatabase");

        var data = window.sessionStorage.getItem('userdata');

        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu) {
                    var menuName = SessionLocator.ExternalParams.Menu.toLocaleLowerCase();
                    if (menuName == "logbox" || menuName == "dapp" || menuName == "protractor" || menuName == "preq" || menuName == "uid") {
                        if (menuName == "preq" || menuName == "uid") {
                            this.LoginCompleted.emit("IgnoreTerms");
                            return;
                        }
                        isUseDefultLogin = true;
                    } else if (SessionLocator.ExternalParams.OneTimePasswordId) {
                        this.HideLoginForm = true;
                        this.HideTenantForm = true;
                        this.ShowLoginBusyIndicator = true;
                        this.OneUsePasswordMethod();
                    } else document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
                }
            }
            else {

                document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
            }
        }

        if (!SessionLocator.IsExternalParams || isUseDefultLogin) {
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
    }

    StartLoading(userData: any) {
        if (userData) {
            this.HideLoginForm = true;
            this.HideTenantForm = true;
            this.HidePendingLoading = true;
            this.ShowLoginBusyIndicator = true;
            SessionInfo.LoggedUserEmail = userData.UserName;
            SessionInfo.LoggedUserId = userData.Id;
            SessionInfo.Token = userData.Token;
            SessionInfo.DocumentDownloadToken = userData.DocumentDownloadToken;
            SessionInfo.SessionTimeout = userData.SessionTimeout;
            SessionInfo.WebTokenExpirationWarningInMinutes = userData.WebTokenExpirationWarningInMinutes;
            SessionInfo.WebTokenLifeTimeInMinutes = userData.WebTokenLifeTimeInMinutes;
            SessionInfo.KeepUserLoggedIn = userData.KeepUserLoggedIn;
            SessionInfo.LastLoginDateTime = userData.LastLoginDateTime;
            this.FillProtractorEmails();


            AmitalGatewayUtil.Instance.AmitalBrowserInUse = userData.AmitalBrowserInUse;
            this.authHeader.append('token', userData.Token);

            if (userData.TwoFactorkey) {
                window.localStorage.setItem('TwoFactorkey', userData.TwoFactorkey);
            }

            if (userData.CurrentTenant != null) {
                SessionInfo.LoggedUserTenant = Number(userData.CurrentTenant + "");
            }

            if (SessionInfo.LoggedUserTenant != null) {
                this.loginService.AuthHeader = this.authHeader;
                this.loginService.CurrentTenant = userData.CurrentTenant;
                this.loginService.LoggedUserId = SessionInfo.LoggedUserId;
                this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;

                this.loginService.GetLoggedUser().subscribe(myResult => {

                    var iGlobalDomainService = new GlobalDomainService();

                    iGlobalDomainService.GetTenantManagementJS(SessionInfo.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
                        ObjectsUpdater.UpdateLoggedUserPM(myResult);
                        ObjectsUpdater.UpdateTenantManagementJS(myResponse.Result);

                        SessionInfo.LoggedUserPM = myResult;

                        if (ObjectsLocator.LoggedUserPM.ExpirationDate != null && DateTool.GetDateParts(ObjectsLocator.LoggedUserPM.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                            SessionLocator.BlockType = "user";
                        }

                        //   else {
                        this.CheckTenantBlocking(userData);
                        //   }
                    });
                });
            }
        }
        window.sessionStorage.setItem("userdata", "");
    }

    FillProtractorEmails() {
        SessionLocator.ProtractorEmails.push("razantest@protractor.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("protractor@test.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("ahmadb@logbox.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("ahmadb@test.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("raghad@protractor.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("sgautomation@pro.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("sumaya@cloud.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("sumaya@automation.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("sg1209@test.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("lana3@test.com".toLowerCase());
        SessionLocator.ProtractorEmails.push("protractor2@test.com".toLowerCase());

    }

    OneUsePasswordMethod() {
        this.loginService.GetOneUsePassword().subscribe(userData => {

            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                var message = "Can't use this key (" + SessionLocator.ExternalParams.OneTimePasswordId + ") again because you used it before ";
                if (userData && userData.ExceptionMessage) message = userData.ExceptionMessage;
                alert(message);
                document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
                SessionLocator.ExternalParams.OneTimePasswordId = null;
            }
            else {
                this.StartLoading(userData);
            }
            this.HidePendingLoading = true;
        });

    }

    LoginClicked() {
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
    }



    ShowTenantList: boolean = false;
    PostUserValidation(loginParameters) {
        this.loginService.PostUserValidation(loginParameters).subscribe(userData => {

            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                this.LoginFailed = true;
                this.HidePendingLoading = true;

                if (userData) alert(userData.ExceptionMessage);

            }

            else {
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
                    var i = 0;
                    this.TenantList.forEach((item) => {
                        i += 1;
                        item.Id = i;

                    });

                    this.SelectedCompany = this.TenantList[0];
                    this.ShowTenantList = true;
                    this.HidePendingLoading = true;
                    this.HideTenantForm = true;




                }
            }

            this.HidePendingLoading = true;
        });
    }
    SelectedCompany: any;
    TenantListChangeSelected(value) {
        this.SelectedCompany = this.TenantList.filter(d => d.Id == value)[0];


    }
    ContinueClicked() {

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

            SessionInfo.LoggedUserCardId = this.SelectedCompany.CardId;
            SessionInfo.LoggedUserCardType = this.SelectedCompany.CardType;
        }
    }
    ChooseTenant(f, values) {
        if (f.valid) {
            this.PostLoginData();
            this.HidePendingLoading = false;
        }
    }

    public UserMobileNumber: string;
    public LoggedUserData: any = null;
    PostLoginData() {
        this.loginService.PostLoginData(this.LoginParams).subscribe(userData => {
            if (userData.TwoFactorkey) {
                window.localStorage.setItem('TwoFactorkey', userData.TwoFactorkey);
            }
            if (!userData.IsTwoFactorAuthenticationRequired || userData.IsTwoFactorAuthenticationRequired == false) {
                this.StartLoading(userData);
            } else {

                this.LoggedUserData = userData;
                this.UserMobileNumber = userData.UserMobileNumber;
                this.ShowTwoFactorAuthenScreen = true;

                //alert('Two factor authentication');
            }
        });
    }

    VerifyClicked() {
        if (!AppTool.IsNullOrEmpty(this.VerficationCode)) {
            this.HidePendingLoading = false;
            this.loginService.PostAuthenticationDeviceVerificationCode(this.LoggedUserData.TwoFactorkey, this.VerficationCode, Number(this.LoggedUserData.CurrentTenant + "")).subscribe(res => {
                this.HidePendingLoading = true;
                if (res == true) {
                    this.ShowTwoFactorAuthenScreen = false;
                    //this.StartLoading(this.LoggedUserData);
                    this.PostLoginData();
                }
                else {
                    this.InvalidVerificationCode = true;
                }
            });
        }
    }
    ResendVerificationCodeClicked() {
        this.HidePendingLoading = false;
        this.loginService.PostResendAuthenticationDeviceVerificationCode(this.LoggedUserData.TwoFactorkey, this.LoggedUserData.Id, Number(this.LoggedUserData.CurrentTenant + "")).subscribe(res => {
            this.HidePendingLoading = true;
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

    }
    LoadClosedTablesToWindow(CurrentTenant: number) {

        this.IndexedDbService.InitializeIndexedDB().subscribe(response => {

            InfraSettings.IndexedDbService = IndexedDbService;

            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = CurrentTenant;
            this.loginService.LoggedUserId = SessionInfo.LoggedUserId;
            this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;

            // UserPM
            this.loginService.GetLoggedUser().subscribe(myResult => {
                var myUserPMService = new UserPMService();
                SessionInfo.LoggedUserPM = myUserPMService.MapJsonToEntityPM(myResult);

                if (SessionInfo.LoggedUserPM) {
                    this.loginService.LoggedUserId = SessionInfo.LoggedUserPM.Id;
                    SessionInfo.LoggedUserId = SessionInfo.LoggedUserPM.Id;
                }

                this.IncreaseProgressBar();
                //1

                // TenantPM
                this.loginService.GetLoggedTenant().subscribe(myResult => {
                    var myTenantPMService = new TenantPMService();
                    InfraSettings.TenantPM = myTenantPMService.MapJsonToEntityPM(myResult);
                    this.IncreaseProgressBar();
                    //2
                });

                CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(response => {
                    this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(response => {
                        this.IncreaseProgressBar("General Resources");
                        //26
                    });
                });

                this.generalDomainService.GetObjectFieldModificationForLoggedTenant().subscribe(response => {

                    if (!response.HasError) {
                        window.ObjectFieldModifications = response.Result;
                    }
                    this.IncreaseProgressBar("ObjectField Modifications");
                        //26
                     
                });

                // LastFilters
                this.loginService.GetLastFilters().subscribe(myResult => {
                    LastFilterClass.MapJSON(myResult);
                    this.IncreaseProgressBar();
                    //3
                });

                // TenantManagementPM
                var iGlobalDomainService = new GlobalDomainService();
                iGlobalDomainService.GetTenantManagementJS(SessionInfo.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
                    ObjectsUpdater.UpdateTenantManagementJS(myResponse.Result);
                    this.IncreaseProgressBar();
                    //4

                    this.loginService.GetPrivateLableById(SessionLocator.TenantManagementJS.PrivateLabelId).subscribe(Result => {
                        ObjectsLocator.UpdatePrivateLableSettings(Result);
                        SessionLocator.PrivateLableSettings = Result;
                        this.IncreaseProgressBar();
                        Environment.SetFavIconAndTitle();
                        //5
                    });
                });

                //this.loginService.GetTenantManagement().subscribe(myResult => {

                //});

                this.myInfrastructureDomainService.GetAllowedFeaturesForLoggedUser().subscribe((myResponse: ServiceResponse) => {
                    this.IncreaseProgressBar();
                    //6

                    // Ayman: please don't modify this (24)
                    if (FeatureLocator.HasFeaturePermession("CreditLimitSetting", "READ") && ObjectsLocator.GlobalSetting && ObjectsLocator.GlobalSetting.WorkEnvironment != 'customs') {
                        var myCreditLimitSettingPMService = new CreditLimitSettingPMService();
                        myCreditLimitSettingPMService.get(CurrentTenant + "").subscribe((myResponse: ServiceResponse) => {
                            ObjectsLocator.UpdateCreditLimitSettingPM(myResponse.Result);
                            this.IncreaseProgressBar();
                            //24
                        });
                    }

                    else {
                        this.IncreaseProgressBar();
                        //24
                    }
                });

                this.loginService.GetQueries().subscribe(myResult => {
                    window.Queries = myResult;
                    this.IncreaseProgressBar();
                    //7
                });

                this.loginService.GetStatuses().subscribe(myResult => {
                    window.Statuses = myResult;
                    this.IncreaseProgressBar();
                    //8
                });

                this.loginService.GetPreDefinedFilters().subscribe(myResult => {
                    window.PreDefinedFilters = myResult;
                    this.IncreaseProgressBar();
                    //9
                });

                this.loginService.GetTenantTranslations().subscribe(myResult => {
                    window.TenantTranslations = myResult;
                    this.IncreaseProgressBar();
                    //10
                });

                this.loginService.GetTransportModes().subscribe(myResult => {
                    window.TransportModes = myResult;
                    this.IncreaseProgressBar();
                    //11
                });

                this.loginService.GetDirections().subscribe(myResult => {
                    window.Directions = myResult;
                    this.IncreaseProgressBar();
                    //12
                });

                this.loginService.GetMenusTables().subscribe(myResult => {
                    window.MenusTables = myResult;
                    this.IncreaseProgressBar();
                    //13
                });

                this.loginService.GetObjectTables().subscribe(myResult => {
                    window.ObjectTables = myResult;
                    this.IncreaseProgressBar();
                    //14
                });

                this.loginService.GetScreens().subscribe(myResult => {
                    window.Screens = myResult;
                    this.IncreaseProgressBar();
                    //15
                });

                this.loginService.GetScreenFields().subscribe(myResult => {
                    window.ScreenFields = myResult;
                    this.IncreaseProgressBar();
                    //16
                });

                this.loginService.GetObjectTableTabs().subscribe(myResult => {
                    window.ObjectTableTabs = myResult;
                    this.IncreaseProgressBar();
                    //17
                });

                this.loginService.GetAccountingSetting().subscribe(myResult => {
                    var myAccountingSettingPM: any = null;

                    if (myResult) {
                        var myAccountingSettingPMService = new AccountingSettingPMService();
                        myAccountingSettingPM = myAccountingSettingPMService.MapJsonToEntityPM(myResult);
                    }

                    ObjectsUpdater.UpdateAccountingSettingPM(myAccountingSettingPM);

                    this.IncreaseProgressBar();
                    //18

                    var myAccountingSystemCode = null;
                    if (ObjectsLocator.AccountingSettingPM) {
                        myAccountingSystemCode = ObjectsLocator.AccountingSettingPM.AccountingSystemCode;

                        if (ObjectsLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                            var myCommonDomain = new CommonDomainService();
                            myCommonDomain.GetAllVatTypesGroups().subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    SessionLocator.AllVatTypesGroups = myResponse.Result;
                                }
                            });
                        }
                    }

                    this.loginService.GetAccountingSystem(myAccountingSystemCode).subscribe(myResult2 => {
                        SessionLocator.AccountingSystemPM = myResult2;
                        this.IncreaseProgressBar();
                        //19
                    });
                });

                this.loginService.GetCustomsInterfaceSetting().subscribe(myResult => {
                    if (myResult) {
                        var myCustomsInterfaceSettingPMService = new CustomsInterfaceSettingPMService();
                        ObjectsUpdater.UpdateCustomsInterfaceSettingPM(myCustomsInterfaceSettingPMService.MapJsonToEntityPM(myResult));
                    }

                    this.IncreaseProgressBar();
                });

                this.loginService.GetSharedLogisticsSetting().subscribe(myResult => {
                    if (myResult) {
                        var mySharedLogisticsSettingPMService = new SharedLogisticsSettingPMService();
                        ObjectsUpdater.UpdateSharedLogisticsSettingPM(mySharedLogisticsSettingPMService.MapJsonToEntityPM(myResult));
                    }

                    this.IncreaseProgressBar();
                });

                this.loginService.GetGlobalSetting().subscribe(myResult => {

                    // Accounting - Abdullah
                    if (InfraSettings.TenantPM) {
                        myResult.LayoutDirection = InfraSettings.TenantPM.LayoutDirection ? InfraSettings.TenantPM.LayoutDirection.toLowerCase() : InfraSettings.TenantPM.LayoutDirection;
                    }

                    //

                    ObjectsLocator.UpdateGlobalSetting(myResult);
                    this.IncreaseProgressBar();
                    Environment.SetFavIconAndTitle();
                    //20
                });

                this.loginService.GetTenantSetting().subscribe(myResult => {
                    SessionLocator.TenantSettings = myResult;
                    this.IncreaseProgressBar();
                    //21
                });

                this.loginService.GetTips().subscribe(myResult => {
                    window.Tips = myResult;
                    this.IncreaseProgressBar();
                    //22
                });

                this.loginService.GetTipsVisibilities().subscribe(myResult => {
                    window.TipsVisibilities = myResult;
                    this.IncreaseProgressBar();
                    //23
                });

                if (!SessionLocator.UseCachedData) {

                    this.loginService.GetObjectFields().subscribe(myResult => {

                        if (!SessionLocator.UseCachedData) {
                            window.ObjectFields = myResult;
                        }

                        this.IncreaseProgressBar();
                    });

                    this.loginService.GetTextCodesTranslations().subscribe(myResult => {

                        if (!SessionLocator.UseCachedData) {
                            window.TextCodesTranslations = myResult;
                        }

                        window.TranslationsCache = [];
                        this.IncreaseProgressBar();
                    });
                }

                else {
                    this.loginService.GetTenantTextCode().subscribe(myResult => {
                        if (myResult) {
                            window.TextCodes = window.TextCodes.concat(myResult);
                            this.IncreaseProgressBar();
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


        this._objectTableRulePMService.getAllByTenant(CurrentTenant).subscribe(response => {
            if (response) {
                window.ObjectTableRules = response.Result;
            }

            this.IncreaseProgressBar();
            //27
        });

        this._objectTableRuleFieldPMService.getAllByTenant(CurrentTenant).subscribe(response => {
            if (response) {
                window.ObjectTableRuleFields = response.Result;
            }

            this.IncreaseProgressBar();
            //28
        });

        this._userLastLoginPMService.GetUserLastLogin(SessionInfo.LoggedUserId, CurrentTenant).subscribe(response => {
            if (!response.HasError && response.Result) {

                var lastloginPM: UserLastLoginPM = response.Result;

                var computerId: string = SessionLocator.GetComputerIdFromStorage();
                if (AppTool.IsNullOrEmpty(computerId)) {
                    computerId = Guid.newGuid();
                    SessionLocator.StoreLogedComputerId(computerId);
                }

                lastloginPM.ComputerId = computerId;
                this._userLastLoginPMService.update(lastloginPM).subscribe(response => {
                    this.IncreaseProgressBar();
                    //29
                });
            }

            else {
                this.IncreaseProgressBar();
                //29
            }
        });

        this.loginService.GeLoggedTenantObjectFields().subscribe(response => {
            if (response) {
                window.ObjectFields = window.ObjectFields.concat(response);
            }

            this.IncreaseProgressBar();
            //30
        });


        this.loginService.GetTenantLanguageTranslations().subscribe(myResult => {
            window.TenantLanguageTranslations = myResult;
            this.IncreaseProgressBar();
            //31
        });

        this.sATInterfaceSettingPMService.get(CurrentTenant).subscribe(myResult => {
            SessionLocator.SATInterfaceSettings = myResult.Result;
            this.IncreaseProgressBar();
            //32
        });

        this.myInfrastructureDomainService.GetFeatureToggles().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                SessionLocator.FeatureToggles = myResponse.Result;
                this.IncreaseProgressBar();
                //33
            }
        });

        this.UserExtendedPMService.CheckUserReleaseNotesToolTip(SessionInfo.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                SessionLocator.ShowUserNewReleaseToolTip = myResponse.Result;
                this.IncreaseProgressBar();
                //34
            }
        });

        this.myInfrastructureDomainService.getDWObjectFieldsWithChildrenByDWTableId("Fact_Shipments").subscribe(Result => {
            //var ObsList = [];
            if (!Result.HasError) {
                window.DWObjectFields = Result.Result;
                this.IncreaseProgressBar();
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
            }


    private CheckTenantBlocking(userData: any) {
                var isSystemBlocked = false;
                var todayDateTicks = DateTool.GetCurrentDateAsUtc().valueOf();

                if(SessionLocator.TenantManagementJS.PaymentFailure) {

                    if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.SuspendDate)) {
                        isSystemBlocked = true;
                        SessionLocator.BlockType = "company";
                    }

                    else if (DateTool.GetDateParts(SessionLocator.TenantManagementJS.SuspendDate).DateTicks < todayDateTicks) {
                        isSystemBlocked = true;
                        SessionLocator.BlockType = "suspend";
                    }
                }

        if(!isSystemBlocked) {
                    if (SessionLocator.TenantManagementJS.IsTrial) {

                        if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.TrialEndDate)) {
                            isSystemBlocked = true;
                            SessionLocator.BlockType = "company";
                        }

                        else if (DateTool.GetDateParts(SessionLocator.TenantManagementJS.TrialEndDate).DateTicks < todayDateTicks) {
                            isSystemBlocked = true;
                            SessionLocator.BlockType = "company";
                        }
                    }
                }

        if(!isSystemBlocked) {
                    if (!AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.PaidUntilDate)) {

                        if (DateTool.GetDateParts(SessionLocator.TenantManagementJS.PaidUntilDate).DateTicks < todayDateTicks && !SessionLocator.TenantManagementJS.IsRecurring) {
                            isSystemBlocked = true;
                            SessionLocator.BlockType = "company";
                        }
                    }
                }

        this.LoadClosedTablesToWindow(userData.CurrentTenant);
            }

    private timerToken: any;
    private TotalNumberOfLoads: number = 0;
    private LoadSize: number = 0;
    private LastLoadSize: number = 0;
    public LoadingCounter: number = 0;
    public CompletedLoadsCount = 0;
    IncreaseProgressBar(loadOPName: string = "") {
        console.log(loadOPName + "==>Completed Login Loads Count: " + this.CompletedLoadsCount);
        if (this.TotalNumberOfLoads == 0) {
            this.TotalNumberOfLoads = 37;

            if (!SessionLocator.UseCachedData) {
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
                ServiceLocator.RulesValidator = new RulesValidator();
                this.timerToken = setTimeout(() => this.ChangePage(), 1000);
            }
        }

        //console.log("login load count:" + this.CompletedLoadsCount);
    }
    private ChangePage() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        this._applicationTimersManager.StartApplicationTimers();

        this.LoginCompleted.emit("event");

        if (SessionLocator.UseCachedData && SessionInfo.LoggedUserTenant != 0) {
            CachedDataManager.GetCacheOnClientTablesData(this.entityListService);
        }
    }



    //MapLastTableAndNavigate(objectfields: any[]) {
    //    this.IncreaseProgressBar();

    //    //var indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB || window.shimIndexedDB;
    //    //var open = indexedDB.open("MyDatabase", 2);
    //    //open.onupgradeneeded = function () {
    //    //    var db = open.result;
    //    //    var store = db.createObjectStore("ObjectField", { keyPath: "Id", unique: true });
    //    //    var index = store.createIndex("Id", "Id");
    //    //};
    //    //open.onsuccess = function () {
    //    //    // Start a new transaction
    //    //    var db = open.result;
    //    //    var tx = db.transaction("ObjectField", "readwrite");
    //    //    var store: IDBObjectStore = tx.objectStore("ObjectField");
    //    //    var index: IDBIndex = store.index("Id");

    //    //    var start = new Date().getTime();
    //    //    objectfields.forEach((objfield) => {
    //    //        store.put(objfield);
    //    //    });

    //    //    var end = new Date().getTime();
    //    //    var queryStart = new Date().getTime();
    //    //    var queryEnd;
    //    //    var cursorResult = [];
    //    //    //var getAllObjectFields = store.openCursor();
    //    //    store.openCursor().onsuccess = function (evt: any) {

    //    //        var cursor: IDBCursorWithValue = evt.target.result;
    //    //        if (cursor) {
    //    //            cursorResult.push(cursor.value);
    //    //            cursor.continue(); //["continue"]();
    //    //        }
    //    //        queryEnd = new Date().getTime();
    //    //    };

    //    //    // 1-98 //
    //    //    // 350 - 1-13699
    //    //    // 2650 - 1-16602
    //    //    // 5400 - 1-952
    //    //    var indexStart = new Date().getTime();
    //    //    var indexEnd;
    //    //    var getObjectFieldById = index.get("1-98");
    //    //    getObjectFieldById.onsuccess = function () {
    //    //        indexEnd = new Date().getTime();
    //    //    };

    //    //    //// 1-98 //
    //    //    //var storeStart = new Date().getTime();
    //    //    //var storeEnd;
    //    //    //var getObjectFieldById = store.get("1-98");
    //    //    //getObjectFieldById.onsuccess = function () {
    //    //    //    storeEnd = new Date().getTime();
    //    //    //};

    //    //    // Close the db when the transaction is done
    //    //    tx.oncomplete = function () {
    //    //        db.close();
    //    //    };
    //    //}
    //    //window.ObjectFields = objectfields;

    //    //this.LoginCompleted.emit("event");
    //}

}



