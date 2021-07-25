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
    PasswordImage: string = "./Images/LoginScreen/password_eye_closed.png";
    PasswordTitle: string = "Show";
    PasswordWidth: number = 280;
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
            }
        }
        window.sessionStorage.setItem("userdata", "");
    }

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



    InputPasswordType: string = "password";
    ShowHidePasswordClick() {

        var showHidePasswordImage = document.getElementById("ShowHidePasswordImageId");
        if (showHidePasswordImage) {
            this.PasswordImage = this.PasswordImage == "./Images/LoginScreen/password_eye.png" ? "./Images/LoginScreen/password_eye_closed.png" : "./Images/LoginScreen/password_eye.png";
            this.InputPasswordType = this.InputPasswordType == "password" ? "text" : "password";
            this.PasswordTitle = this.PasswordTitle == "Show" ? "Hide" : "Show";
        }


    }

    PasswordExpirationButtomClicked(type: string) {

        if (type == "Yes") {
            SessionInfo.LoggedUserEmail = this.UserDataPrompt.UserName;
            if (SessionInfo.MainLocation) {
                SessionInfo.MainLocation.clear();
            }
            this.LoadChangePasswordComponent();
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
                    this.LoadChangePasswordComponent();
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
                        }
                        this.IsShowAreaCaptcha = true;
                        this.CaptchaImageUrl = userData.CaptchaImage;
                    }

                    this.errorMessage = "";
                    if (userData.IpRestricted) this.errorMessage = "Trying to log in from unauthorised station!" + " (The IP address you are trying to " + " log in from is restricted for this user)";
                    else if (userData.InActive) this.errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                    else if (userData.Unlicensed) this.errorMessage = "Your account is unlicensed!" + " please contact your administrator.";
                    else if (userData.InValidMailOrPassword) this.errorMessage = "Login failed! invalid user name or password.";
                    else if (userData.InValidCaptcha && userData.CaptchaImage) this.errorMessage = "Please re-enter the characters you see in the image above";
                    else this.errorMessage = "Login failed! invalid user name or password." + "<br/>";

                }

            }

            else {

                this.ComplateProcessLogin(userData, loginParameters);

            }

            this.HidePendingLoading = true;
        });
    }

    LoadChangePasswordComponent() {
        Tools.DynamicLoader.Load("./Login/Components/DSVChangePasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
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

            if (userData && !userData.HasError) {
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
                    if (SessionInfo.GetLogitudeURL().indexOf('localhost:9996') > -1) {
                        const isDSV = window.sessionStorage.getItem("IsDSV") == "true";
                        AngularURL = "http://localhost:4200/?" + (isDSV ? "D" : "P") + data;
                    }
                    else {
                        var version = userData.HtmlVersion;
                        AngularURL = SessionInfo.GetLogitudeURL() + "Angular" + version + "/index.html";
                    }
                }
                else {
                    if (SessionInfo.GetLogitudeURL().indexOf('localhost:9996') > -1) {
                        const isDSV = window.sessionStorage.getItem("IsDSV") == "true";
                        AngularURL = "http://localhost:4200/?" + (isDSV ? "D":"P") + data;
                    }
                    else
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
            }




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

    private timerToken: any;
    private TotalNumberOfLoads: number = 0;
    private LoadSize: number = 0;
    private LastLoadSize: number = 0;
    public LoadingCounter: number = 0;
    public CompletedLoadsCount = 0;
}
