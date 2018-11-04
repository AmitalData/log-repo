import { Component } from '@angular/core';
import {LoginService, LoginParameters, LoginTokenParameter} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {Tools} from '../Utilities/Tools';
declare var showTenantsCombo, getselectedcompany, IsBrowserSupported, IsMobileDetected;

@Component({
    selector: 'LoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'LoginComponent.html',
    styleUrls: ['LoginComponent.css']
})
export class LoginComponent {
    public Email: string;
    public Password: string;
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
    public ShowLoadingIndicator: boolean = false;
    public UserData: any;
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public SampleLogoURL: string = "./Images/ApplicationLogo/Angular/AngularLogo.png";
    PasswordExpirationDateMessage: string;
    PasswordExpirationDateMessage2: string;

    IsShowAreaCaptcha: boolean;
    CaptchaImageUrl: string;
    CaptchaTextValue: string;
    CaptchaKey: string;

    public IsShowPasswordExpirationDateArea: boolean = false;

    //private _objectTableRulePMService: ObjectTableRulePMService = new ObjectTableRulePMService();
    //private _objectTableRuleFieldPMService: ObjectTableRuleFieldPMService = new ObjectTableRuleFieldPMService();
    constructor(private loginService: LoginService//, public IndexedDbService: IndexedDbService, private entityResourceService: EntityResourceService, private _applicationTimersManager: ApplicationTimersManager, public entityListService: EntityListService,
        //private _userLastLoginPMService: UserLastLoginPMService
    ) {

        //this.IsProduction = SessionLocator.IsProduction;
        //this.authHeader = new Headers();
        //this.authHeader.append('Content-Type', 'application/json');
        //this.authHeader.append('Accept', 'application/json');
        //this.loginService.AuthHeader = this.authHeader;
        this.ShowLoginBusyIndicator = false;
        this.HideLoginForm = false;
        this.HideTenantForm = true;
        this.LoginFailed = false;
        this.LoginParams = new LoginParameters();
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

    idxdb: IDBOpenDBRequest;
    public authHeader;
    ngOnInit() {

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
    }

    IsShowFormLogin: boolean = false;
    IsHaveTenantInUrl: boolean = false;
    LoginWithToken() {
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

                var loginTokenParameter = new LoginTokenParameter();
                loginTokenParameter.Token = token;

                this.loginService.LoginUsingAuthenticaionToken(loginTokenParameter).subscribe(userData => {
                    if (!userData.HasError) {
                        var data = JSON.stringify(userData);
                        window.sessionStorage.setItem("userdata", data);
                        var mypageUrl = window.location.href;
                        var AngularURL = "";
                        var urlMenu = "";


                        if (userData.HtmlVersion) {
                            var version = userData.HtmlVersion;
                            AngularURL = SessionInfo.GetLogitudeURL() + "Angular" + version + "/index.html";
                        }
                        else {
                            AngularURL = SessionInfo.GetLogitudeURL() + "Angular/index.html";
                        }
                        if (mypageUrl && mypageUrl.indexOf("Menu=") > -1) {
                            urlMenu = mypageUrl.split("Menu=")[1];
                            AngularURL += ("?Menu=" + urlMenu);
                        }

                        document.location.href = AngularURL;


                    } else {
                        this.IsShowFormLogin = true;
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
    }







    OnMouseDownEvt(event) {
        var key = event.keyCode;
        var ENTER = 13;
        if (key == ENTER) {
            this.LoginClicked();
        }
    }

    StartLoading(userData: any) {
        console.log("StartLoading");
        if (userData) {
            this.HideLoginForm = true;
            this.HideTenantForm = true;
            this.HidePendingLoading = true;
            this.ShowLoginBusyIndicator = true;
            SessionInfo.LoggedUserEmail = userData.UserName;
            SessionInfo.LoggedUserId = userData.Id;
            SessionInfo.Token = userData.Token;
            this.authHeader.append('token', userData.Token);

            if (userData.CurrentTenant != null) {
                SessionInfo.LoggedUserTenant = Number(userData.CurrentTenant + "");
            }

            if (SessionInfo.LoggedUserTenant != null) {
                this.loginService.AuthHeader = this.authHeader;
                this.loginService.CurrentTenant = userData.CurrentTenant;
                this.loginService.LoggedUserId = SessionInfo.LoggedUserId;
                this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;

                //this.loginService.GetLoggedUser().subscribe(myResult => {
                //    this.loginService.GetTenantManagement().subscribe(myResult2 => {

                //        SessionInfo.LoggedUserPM = myResult;
                //        InfraSettings.TenantManagementPM = myResult2;

                //        if (SessionInfo.LoggedUserPM.ExpirationDate != null && DateTool.GetDateParts(SessionInfo.LoggedUserPM.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                //            this.Blocking.emit("user");
                //        }

                //        else {
                //            this.CheckTenantBlocking(userData);
                //        }
                //    });
                //});
            }
        }
        window.sessionStorage.setItem("userdata", "");
    }
    //OneUsePasswordMethod() {
    //    this.loginService.GetOneUsePassword().subscribe(userData => {

    //        if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
    //            var message = "Can't use this key (" + SessionLocator.ExternalParams.OneTimePasswordId + ") again because you used it before ";
    //            if (userData && userData.ExceptionMessage) message = userData.ExceptionMessage;
    //            alert(message);
    //            document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
    //            SessionLocator.ExternalParams.OneTimePasswordId = null;
    //        }
    //        else {
    //            this.StartLoading(userData);
    //        }
    //        this.HidePendingLoading = true;
    //    });

    //}

    onEmailBlur(email) {
        if (email != this.Email) {
            this.IsShowAreaCaptcha = false;
            this.CaptchaKey = null;
            this.CaptchaTextValue = null;
            if (this.errorMessage == "Please re-enter the characters you see in the image above") {
                this.errorMessage = "";
            }

        }
    }

    PasswordExpirationButtomClicked(type: string) {

        if (type == "Yes") {
            SessionInfo.LoggedUserEmail = this.UserDataPrompt.UserName;
            if (SessionInfo.MainLocation) {
                SessionInfo.MainLocation.clear();
            }
            Tools.DynamicLoader.Load("./Login/Components/" + SessionInfo.PlShortName + "ChangePasswordComponent", SessionInfo.MainLocation)
                .then(cmpRef => {
                });
        }
        else if (type == "No") {
            this.ComplateProcessLogin(this.UserDataPrompt, this.LoginParameters);
        }
    }


    ComplateProcessLogin(userData: any, loginParameters: any) {
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
    }







    LoginClicked() {

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
            this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;
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
    }


    LoginParameters: any;
    UserDataPrompt: any;

    ShowTenantList: boolean = false;
    errorMessage: string = "";

    PostUserValidation(loginParameters) {
        this.errorMessage = "";
        this.loginService.PostUserValidation(loginParameters).subscribe(userData => {
            this.ShowLoadingIndicator = false;
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                this.LoginFailed = true;
                this.HidePendingLoading = true;
                
                this.CaptchaKey = userData ? userData.CaptchaKey:"";

                if (userData && userData.ExceptionMessage) {
                    alert(userData.ExceptionMessage);
                }


              
                 if (userData.MustChangePassword) {
                    SessionInfo.LoggedUserEmail = userData.UserName;
                    if (SessionInfo.MainLocation) {
                        SessionInfo.MainLocation.clear();
                    }
                    Tools.DynamicLoader.Load("./Login/Components/" + SessionInfo.PlShortName + "ChangePasswordComponent", SessionInfo.MainLocation)
                        .then(cmpRef => {
                        });
                } else if (userData.PasswordExpirationDateMessage) {

                    if (userData.PasswordExpirationDateMessage.indexOf('days. Do') > -1) {
                        this.PasswordExpirationDateMessage = userData.PasswordExpirationDateMessage.split('days. Do')[0] + "days. Do";
                        this.PasswordExpirationDateMessage2 = userData.PasswordExpirationDateMessage.split('days. Do')[1];
                    } else {
                        this.PasswordExpirationDateMessage = userData.PasswordExpirationDateMessagel
                    }

                    this.UserDataPrompt = userData;
                    this.LoginParameters = loginParameters;
                    this.IsShowPasswordExpirationDateArea = true;
                }
                else {
                    this.errorMessage = "";

                     if (userData.InValidCaptcha) {
                         if (this.IsShowAreaCaptcha) {
                             this.CaptchaTextValue = "";
                             this.errorMessage = "Please re-enter the characters you see in the image above";
                         }

                         this.IsShowAreaCaptcha = true;
                         this.CaptchaImageUrl = userData.CaptchaImage;
                     }

                     else {
                         this.errorMessage = "";

                         if (userData.InValidCaptcha) {
                             if (this.IsShowAreaCaptcha) {
                                 this.CaptchaTextValue = "";
                             }

                             this.IsShowAreaCaptcha = true;
                             this.CaptchaImageUrl = userData.CaptchaImage;
                         }



                         if (userData.IpRestricted) this.errorMessage = "Trying to log in from unauthorised station!" + " (The IP address you are trying to " + " log in from is restricted for this user)";//
                         if (userData.InActive) this.errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                         if (userData.Unlicensed) this.errorMessage = "Your account is unlicensed!" + " please contact your administrator.";
                         if (userData.InValidCaptcha) this.errorMessage = "Please re-enter the characters you see in the image above";
                     

                     }


             
                }

            }

            else {

                this.ComplateProcessLogin(userData, loginParameters);

            }

            this.HidePendingLoading = true;
        });
    }
    SelectedCompany: any;
    TenantListChangeSelected(value) {
        this.SelectedCompany = this.TenantList.filter(d => d.Id == value)[0];


    }
    ContinueClicked() {
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

    }
    ChooseTenant(f, values) {
        if (f.valid) {
            this.PostLoginData();
            this.HidePendingLoading = false;
        }
    }
    PostLoginData() {
        this.loginService.PostLoginData(this.LoginParams).subscribe(userData => {
            var data = JSON.stringify(userData);
            window.sessionStorage.setItem("userdata", data);
            var mypageUrl = window.location.href;
            var AngularURL = "";
            var urlMenu = "";
            //userData.KeepUserLoggedIn == true &&
            if (userData.Token && this.IsHaveTenantInUrl) {
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
                AngularURL = SessionInfo.GetLogitudeURL() + "Angular" + version + "/index.html";
            }
            else {
                AngularURL = SessionInfo.GetLogitudeURL() + "Angular/index.html";
            }


            if (mypageUrl && mypageUrl.indexOf("Menu=") > -1) {
                urlMenu = mypageUrl.split("Menu=")[1];
                AngularURL += ("?Menu=" + urlMenu);

                if (externalTenant) {
                    AngularURL = AngularURL.replace("&Tenant=" + externalTenant, "");
                }

            }

            document.location.href = AngularURL;

        });
    }
    cookie_name: string = "email_cookie" // added 
    expdays: number = 365
    SaveDataToCookie() {

        var expdate = new Date();
        expdate.setTime(expdate.getTime() + (this.expdays * 24 * 60 * 60 * 1000)); // expiry date 

        if (this.Email == "") { return }
        this.set_cookie(this.cookie_name, this.Email, expdate)
    }
    set_cookie(name, value, expires) {

        if (!expires) { expires = new Date() }
        document.cookie = name + "=" + encodeURI(value) +
            ((expires == null) ? "" : "; expires=" + expires.toGMTString())

    }

    get_cookie(name) {

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
            if (i == 0) break;
        }
        return null;
    }

    get_cookie_val(offset) {

        var endstr = document.cookie.indexOf(";", offset);
        if (endstr == -1)
            endstr = document.cookie.length;
        return decodeURI(document.cookie.substring(offset, endstr));
    }
    get_cookie_data() {

        var inf = this.get_cookie(this.cookie_name)
        if (!inf) { return }
        this.Email = inf;

        //get_update_date();
    }
    BackToLoginClicked() {
        document.location.href = SessionInfo.GetLogitudeURL() + "Login.aspx";
    }

    //ForgotPasswordClicked() {

    //}
    //LoadClosedTablesToWindow(CurrentTenant: number) {

    //    this.IndexedDbService.InitializeIndexedDB().subscribe(response => {

    //        InfraSettings.IndexedDbService = IndexedDbService;

    //        this.loginService.AuthHeader = this.authHeader;
    //        this.loginService.CurrentTenant = CurrentTenant;
    //        this.loginService.LoggedUserId = SessionInfo.LoggedUserId;
    //        this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;

    //        // UserPM
    //        this.loginService.GetLoggedUser().subscribe(myResult => {
    //            var myUserPMService = new UserPMService();
    //            SessionInfo.LoggedUserPM = myUserPMService.MapJsonToEntityPM(myResult);

    //            if (SessionInfo.LoggedUserPM) {
    //                this.loginService.LoggedUserId = SessionInfo.LoggedUserPM.Id;
    //                SessionInfo.LoggedUserId = SessionInfo.LoggedUserPM.Id;
    //            }

    //            this.IncreaseProgressBar();
    //            //1

    //            // TenantPM
    //            this.loginService.GetLoggedTenant().subscribe(myResult => {
    //                var myTenantPMService = new TenantPMService();
    //                InfraSettings.TenantPM = myTenantPMService.MapJsonToEntityPM(myResult);
    //                this.IncreaseProgressBar();
    //                //2
    //            });

    //            // LastFilters
    //            this.loginService.GetLastFilters().subscribe(myResult => {
    //                LastFilterClass.MapJSON(myResult);
    //                this.IncreaseProgressBar();
    //                //3
    //            });

    //            // TenantManagementPM
    //            this.loginService.GetTenantManagement().subscribe(myResult => {
    //                var myTenantManagementPMService = new TenantManagementPMService();
    //                InfraSettings.TenantManagementPM = myTenantManagementPMService.MapJsonToEntityPM(myResult);
    //                this.IncreaseProgressBar();
    //                //4

    //                this.loginService.GetPrivateLableById(SessionLocator.TenantManagementPM.PrivateLabelId).subscribe(Result => {
    //                    SessionLocator.PrivateLableSettings = Result;
    //                    this.IncreaseProgressBar();
    //                    AppTool.SetFavIconAndTitle();
    //                    //5
    //                });
    //            });

    //            this.loginService.GetFeatures().subscribe(myResult => {
    //                FeatureLocator.MapFeatures(myResult);
    //                this.IncreaseProgressBar();
    //                //6


    //                // Ayman: please don't modify this (24)
    //                if (FeatureLocator.HasFeaturePermession("CreditLimitSetting", "READ")) {
    //                    var myCreditLimitSettingPMService = new CreditLimitSettingPMService();
    //                    myCreditLimitSettingPMService.get(CurrentTenant + "").subscribe((myResponse: ServiceResponse) => {
    //                        SessionLocatorrrrr.CreditLimitSettingPM = myResponse.Result;
    //                        this.IncreaseProgressBar();
    //                        //24
    //                    });
    //                }

    //                else {
    //                    this.IncreaseProgressBar();
    //                    //24
    //                }
    //            });

    //            this.loginService.GetQueries().subscribe(myResult => {
    //                window.Queries = myResult;
    //                this.IncreaseProgressBar();
    //                //7
    //            });

    //            this.loginService.GetStatuses().subscribe(myResult => {
    //                window.Statuses = myResult;
    //                this.IncreaseProgressBar();
    //                //8
    //            });

    //            this.loginService.GetPreDefinedFilters().subscribe(myResult => {
    //                window.PreDefinedFilters = myResult;
    //                this.IncreaseProgressBar();
    //                //9
    //            });

    //            this.loginService.GetTenantTranslations().subscribe(myResult => {
    //                window.TenantTranslations = myResult;
    //                this.IncreaseProgressBar();
    //                //10
    //            });

    //            this.loginService.GetTransportModes().subscribe(myResult => {
    //                window.TransportModes = myResult;
    //                this.IncreaseProgressBar();
    //                //11
    //            });

    //            this.loginService.GetDirections().subscribe(myResult => {
    //                window.Directions = myResult;
    //                this.IncreaseProgressBar();
    //                //12
    //            });

    //            this.loginService.GetMenusTables().subscribe(myResult => {
    //                window.MenusTables = myResult;
    //                this.IncreaseProgressBar();
    //                //13
    //            });

    //            this.loginService.GetObjectTables().subscribe(myResult => {
    //                window.ObjectTables = myResult;
    //                this.IncreaseProgressBar();
    //                //14
    //            });

    //            this.loginService.GetScreens().subscribe(myResult => {
    //                window.Screens = myResult;
    //                this.IncreaseProgressBar();
    //                //15
    //            });

    //            this.loginService.GetScreenFields().subscribe(myResult => {
    //                window.ScreenFields = myResult;
    //                this.IncreaseProgressBar();
    //                //16
    //            });

    //            this.loginService.GetObjectTableTabs().subscribe(myResult => {
    //                window.ObjectTableTabs = myResult;
    //                this.IncreaseProgressBar();
    //                //17
    //            });

    //            this.loginService.GetAccountingSetting().subscribe(myResult => {
    //                var myAccountingSettingPMService = new AccountingSettingPMService();
    //                if (myResult) {
    //                    InfraSettings.AccountingSettingPM = myAccountingSettingPMService.MapJsonToEntityPM(myResult);
    //                }
    //                this.IncreaseProgressBar();
    //                //18

    //                var myAccountingSystemCode = null;
    //                if (InfraSettings.AccountingSettingPM) {
    //                    myAccountingSystemCode = InfraSettings.AccountingSettingPM.AccountingSystemCode;
    //                }

    //                this.loginService.GetAccountingSystem(myAccountingSystemCode).subscribe(myResult2 => {
    //                    SessionLocator.AccountingSystemPM = myResult2;
    //                    this.IncreaseProgressBar();
    //                    //19
    //                });
    //            });

    //            this.loginService.GetGlobalSetting().subscribe(myResult => {
    //                SessionLocatorrrrr.GlobalSetting = myResult;
    //                this.IncreaseProgressBar();
    //                AppTool.SetFavIconAndTitle();
    //                //20
    //            });

    //            this.loginService.GetTenantSetting().subscribe(myResult => {
    //                SessionLocator.TenantSettings = myResult;
    //                this.IncreaseProgressBar();
    //                //21
    //            });

    //            this.loginService.GetTips().subscribe(myResult => {
    //                window.Tips = myResult;
    //                this.IncreaseProgressBar();
    //                //22
    //            });

    //            this.loginService.GetTipsVisibilities().subscribe(myResult => {
    //                window.TipsVisibilities = myResult;
    //                this.IncreaseProgressBar();
    //                //23
    //            });

    //            if (!SessionLocator.UseCachedData) {

    //                this.loginService.GetObjectFields().subscribe(myResult => {

    //                    if (!SessionLocator.UseCachedData) {
    //                        window.ObjectFields = myResult;
    //                    }

    //                    this.IncreaseProgressBar();
    //                });

    //                this.loginService.GetTextCodesTranslations().subscribe(myResult => {

    //                    if (!SessionLocator.UseCachedData) {
    //                        window.TextCodesTranslations = myResult;
    //                    }

    //                    window.TranslationsCache = [];
    //                    this.IncreaseProgressBar();
    //                });
    //            }

    //            else {
    //                this.loginService.GetTenantTextCode().subscribe(myResult => {
    //                    if (myResult) {
    //                        window.TextCodes = window.TextCodes.concat(myResult);
    //                        this.IncreaseProgressBar();
    //                        //25
    //                    }
    //                });
    //            }

    //            CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(response => {
    //                this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(response => {
    //                    this.IncreaseProgressBar();
    //                    //26
    //                });
    //            });
    //        });
    //    });


    //    this._objectTableRulePMService.getAllByTenant(CurrentTenant).subscribe(response => {
    //        if (response) {
    //            window.ObjectTableRules = response.Result;
    //        }

    //        this.IncreaseProgressBar();
    //        //27
    //    });

    //    this._objectTableRuleFieldPMService.getAllByTenant(CurrentTenant).subscribe(response => {
    //        if (response) {
    //            window.ObjectTableRuleFields = response.Result;
    //        }

    //        this.IncreaseProgressBar();
    //        //28
    //    });

    //    this._userLastLoginPMService.GetUserLastLogin(SessionInfo.LoggedUserId, CurrentTenant).subscribe(response => {
    //        if (!response.HasError && response.Result) {

    //            var lastloginPM: UserLastLoginPM = response.Result;

    //            var computerId: string = SessionLocator.GetComputerIdFromStorage();
    //            if (AppTool.IsNullOrEmpty(computerId)) {
    //                computerId = Guid.newGuid();
    //                SessionLocator.StoreLogedComputerId(computerId);
    //            }

    //            lastloginPM.ComputerId = computerId;
    //            this._userLastLoginPMService.update(lastloginPM).subscribe(response => {
    //                this.IncreaseProgressBar();
    //                //29
    //            });
    //        }

    //        else {
    //            this.IncreaseProgressBar();
    //            //29
    //        }
    //    });

    //    this.loginService.GeLoggedTenantObjectFields().subscribe(response => {
    //        if (response) {
    //            window.ObjectFields = window.ObjectFields.concat(response);
    //        }

    //        this.IncreaseProgressBar();
    //        //30
    //    });


    //    this.loginService.GetTenantLanguageTranslations().subscribe(myResult => {
    //        window.TenantLanguageTranslations = myResult;
    //        this.IncreaseProgressBar();
    //        //31
    //    });

    //    //this._objectTableRuleFieldPMService.getAllByTenant(CurrentTenant).subscribe(myResult => {
    //    //    window.ObjectTableRulePMs = myResult;
    //    //    this.IncreaseProgressBar();
    //    //    //11
    //    //});       
    //}


    //private CheckTenantBlocking(userData: any) {
    //    var isCheckedCompleted = false;
    //    var todayDateTicks = DateTool.GetCurrentDateAsUtc().valueOf();

    //    if (InfraSettings.TenantManagementPM.PaymentFailure) {

    //        if (AppTool.IsNullOrEmpty(InfraSettings.TenantManagementPM.SuspendDate)) {
    //            isCheckedCompleted = true;
    //            this.Blocking.emit("company");
    //        }

    //        else if (DateTool.GetDateParts(InfraSettings.TenantManagementPM.SuspendDate).DateTicks < todayDateTicks) {
    //            isCheckedCompleted = true;
    //            this.Blocking.emit("suspend");
    //        }

    //        else {
    //            isCheckedCompleted = true;
    //            this.LoadClosedTablesToWindow(userData.CurrentTenant);
    //        }
    //    }

    //    if (!isCheckedCompleted) {
    //        if (InfraSettings.TenantManagementPM.IsTrial) {

    //            if (AppTool.IsNullOrEmpty(InfraSettings.TenantManagementPM.TrialEndDate)) {
    //                isCheckedCompleted = true;
    //                this.Blocking.emit("company");
    //            }

    //            else if (DateTool.GetDateParts(InfraSettings.TenantManagementPM.TrialEndDate).DateTicks < todayDateTicks) {
    //                isCheckedCompleted = true;
    //                this.Blocking.emit("company");
    //            }

    //            else {
    //                isCheckedCompleted = true;
    //                this.LoadClosedTablesToWindow(userData.CurrentTenant);
    //            }
    //        }
    //    }

    //    if (!isCheckedCompleted) {
    //        if (!AppTool.IsNullOrEmpty(InfraSettings.TenantManagementPM.PaidUntilDate)) {

    //            if (DateTool.GetDateParts(InfraSettings.TenantManagementPM.PaidUntilDate).DateTicks < todayDateTicks && !InfraSettings.TenantManagementPM.IsRecurring) {
    //                isCheckedCompleted = true;
    //                this.Blocking.emit("company");
    //            }

    //            else {
    //                isCheckedCompleted = true;
    //                this.LoadClosedTablesToWindow(userData.CurrentTenant);
    //            }
    //        }
    //    }

    //    if (!isCheckedCompleted) {
    //        this.LoadClosedTablesToWindow(userData.CurrentTenant);
    //    }
    //}

    private timerToken: any;
    private TotalNumberOfLoads: number = 0;
    private LoadSize: number = 0;
    private LastLoadSize: number = 0;
    public LoadingCounter: number = 0;
    public CompletedLoadsCount = 0;
    //IncreaseProgressBar() {

    //    if (this.TotalNumberOfLoads == 0) {
    //        this.TotalNumberOfLoads = 31;

    //        if (!SessionLocator.UseCachedData) {
    //            this.TotalNumberOfLoads += 1;
    //        }

    //        this.LoadSize = 100 / this.TotalNumberOfLoads;

    //        if (this.LoadSize.toString().indexOf(".") > -1) {
    //            this.LoadSize = +this.LoadSize.toString().split(".")[0];
    //            this.LastLoadSize = 100 - ((this.TotalNumberOfLoads - 1) * this.LoadSize);
    //        }

    //        else {
    //            this.LastLoadSize = this.LoadSize;
    //        }
    //    }

    //    this.CompletedLoadsCount++;

    //    if (this.CompletedLoadsCount <= this.TotalNumberOfLoads) {

    //        var elem = document.getElementById("myBar");

    //        var length = this.LoadSize;
    //        if (this.CompletedLoadsCount == this.TotalNumberOfLoads) {
    //            length = this.LastLoadSize;
    //        }

    //        for (var i = 1; i <= length; i++) {
    //            if (this.LoadingCounter < 100) {
    //                this.LoadingCounter = this.LoadingCounter + 1;
    //                elem.style.width = this.LoadingCounter + '%';
    //            }
    //        }

    //        if (this.CompletedLoadsCount == this.TotalNumberOfLoads) {
    //            this.timerToken = setTimeout(() => this.ChangePage(), 1000);
    //        }
    //    }
    //}
    //private ChangePage() {
    //    if (this.timerToken) {
    //        clearTimeout(this.timerToken);
    //    }

    //    this._applicationTimersManager.StartApplicationTimers();

    //    this.LoginCompleted.emit("event");

    //    if (SessionLocator.UseCachedData && SessionInfo.LoggedUserTenant != 0) {
    //        CachedDataManager.GetCacheOnClientTablesData(this.entityListService);
    //    }
    //}



}
