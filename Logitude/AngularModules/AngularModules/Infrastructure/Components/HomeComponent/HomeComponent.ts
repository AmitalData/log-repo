declare var window: any;
import { HostListener, Component, ViewContainerRef, ViewChild, ViewChildren, QueryList, Output, EventEmitter, OnDestroy} from '@angular/core';
import {AppTool} from '../../Tools';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {LocationDirective} from '../../Utilities/LocationDirective';
import {SessionComponent} from '../Session/SessionComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../Services/EntityResourceService';
import {UserPM} from '../../../Common/EntityPMs/UserPM';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LoginService, LoginParameters} from '../../Services/LoginService';
import {Headers} from '@angular/http';
import {AmitalGatewayUtil, UnifreightMessageM} from '../../Utilities/AmitalGatewayUtil';
import {Observable}     from 'rxjs/Rx';
import {NotificationExtendedListService} from '../../../Customs/Services/ExtendedLists/NotificationExtendedListService';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {Environment} from '../../Locators/Environment';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import {ServiceLocator} from '../../Locators/ServiceLocator';
import { DetectUserInActivity } from '../../Helpers/DetectUserInActivity';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { BluesnapContractPMService } from '../../Services/StandardPMs/BluesnapContractPMService';
import { BluesnapContractPM } from '../../EntityPMs/BluesnapContractPM';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
@Component({
    moduleId: module.id,
    templateUrl: './HomeComponent.html',
})

export class HomeComponent implements OnDestroy{
    public Tenant: number;
    public DataContext = this;
    public Tabs: Array<SessionTabItem>;
    public SelectedTabItem: SessionTabItem;
    public ChangeHeaderColor: boolean = false;
    @Output() SignoutCompleted = new EventEmitter();
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("ApplicationLocation", { read: ViewContainerRef }) ApplicationLocation: ViewContainerRef;
    SettingBtnVisibility: boolean = false;
    IsShowLastSuccessfulLoginComponent: boolean = true;
    public IfBlueSnapContracts: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
private BluesnapContractService: BluesnapContractPMService= new BluesnapContractPMService();
    constructor() {
        this.Tenant = SessionLocator.Tenant;
        SessionLocator.Index = 0;
        SessionLocator.AllSessions = new Array<SessionComponent>();
        SessionLocator.HomeComponent = this;
        this.ChangeHeaderColor = ObjectsLocator.TenantManagementJS.ChangeHeaderColor;

        this.Tabs = [];
        this.Tabs.push(new SessionTabItem());

        this.InitializeComponent();

        if (!this.IsNewSignupTenant) {
            this.InitializeAppHeader();
            this.CheckAmitalBrowserInUse();
        }   


        if (!SessionInfo.KeepUserLoggedIn) {
            // sessionTimeout
            var sessionTimeout: DetectUserInActivity = new DetectUserInActivity();
            sessionTimeout.Start(SessionInfo.SessionTimeout);

            // tokenExpiration
            var tokenExpiration: DetectUserInActivity = new DetectUserInActivity(true);
            tokenExpiration.Start(SessionInfo.WebTokenLifeTimeInMinutes, SessionInfo.WebTokenExpirationWarningInMinutes , "M");//(3, 1, "M")


        }

    }

    OnSessionMouseUp($event) {
        SessionLocator.CurrentSession.MouseUpEvent.emit($event);
    }

    // InitializeComponent
    public IsTenant65: boolean = false;
    public IsLogBox: boolean = false;
    public IsNewSignupTenant: boolean = false;
    public LayoutDirection: string = 'ltr';
    //public SystemFontFamily: string = 'Lucida Sans Unicode';
    public SystemFontFamily: string = "'Lucida Sans Unicode', 'Lucida Grande', sans-serif";
    table: any;
    InitializeComponent() {
        this.IsBluesnapAccount = !AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.BluesnapAccount);
        this.IsCountryIsrael = SessionLocator.TenantManagementJS.CountryName == "Israel";


        
        
        var isNewSignupTenant = false;

        if (AppTool.IsNullOrEmpty(SessionLocator.TenantPM.CurrencyId) || AppTool.IsNullOrEmpty(SessionLocator.TenantPM.ProfitCurrencyId) || AppTool.IsNullOrEmpty(SessionLocator.TenantPM.AddressId) || AppTool.IsNullOrEmpty(SessionLocator.TenantPM.AgentId)) {
            isNewSignupTenant = true;
        }

        this.IsNewSignupTenant = SessionLocator.IsNewSignupTenant = isNewSignupTenant;

        this.IsTenant65 = SessionLocator.Tenant == 65 ? true : false;
        this.IsLogBox = SessionLocator.TenantPM.IsDocumentsArchive == true ? true : false;

        // Layout Direction
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

        if (ObjectsLocator.GlobalSetting) {
          // if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
          if (!ObjectsLocator.LoggedUserPM.DontShowLocal) {
                this.SystemFontFamily = 'Arial'; //'OpenSans-Regular';
                isNewSignupTenant = false;
            }
        }

        this.table = window.ObjectTables.filter(d => d.Name === 'General')[0];
        var SettingBtnFeature = FeatureLocator.Features.filter(f => (f.Code == "AppSettingsBtn") && f.ObjectTableId == this.table.Id)[0];
        this.SettingBtnVisibility = !AppTool.IsNullOrEmpty(SettingBtnFeature) || SessionLocator.Tenant == 0;
    }

    // InitializeAppHeader
    public EnvironmentUrl: string = null;
    public EnvironmentSRC: string = null;
    public EnvironmentName: string = null;
    public Company: string;
    public LoggedUser: string;
    public IsBellVisible: boolean = false;
    public IsCustomizationVisible: boolean = false;
    public IsSignatureVisible: boolean = false;
    public IsChangePasswordVisible: boolean = false;
    public IsDataBackupVisible: boolean = false;
    public IsFillLocalStorageVisible: boolean = false;
    public IsDocumentsBackupVisible: boolean = false;
    public IsCurrenciesRatesVisible: boolean = false;
    public IsBluesnapAccount: boolean = false;
    public IsCountryIsrael: boolean = false;
    
    
    InitializeAppHeader() {
        this.EnvironmentUrl = Environment.GetEnvironmentUrl();
        this.EnvironmentSRC = Environment.GetEnvironmentIcon();
        this.EnvironmentName = Environment.GetEnvironmentName();
        this.Company = SessionLocator.TenantPM.Company;
        this.LoggedUser = SessionLocator.LoggedUserPM.EnglishName;

        if (SessionLocator.Tenant == 261) {
            this.IsCustomizationVisible = true;
        }

        else if (FeatureLocator.HasFeaturePermession("General", "General.Features.Customization")) {
            this.IsCustomizationVisible = true;
        }
                
        if (!this.IsLogBox && FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsCurrenciesRatesVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "SIGNATURESETTING")) {
            this.IsSignatureVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "CHANGEPASSWORDSETTING")) {
            this.IsChangePasswordVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.DataBackup")) {
            this.IsDataBackupVisible = true;
        }

        if (SessionLocator.LoggedUserPM.IsCustomerCare || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            this.IsFillLocalStorageVisible = true;
        }
        
        if (FeatureLocator.HasFeaturePermession("General", "NOTIFICATIONBELL")) {
            this.IsBellVisible = true;
            this.GetBadjCount();
            this.StartApplicationTimers();
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.DocumentsBackup")) {
            this.IsDocumentsBackupVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("BluesnapContract", "PaymentSettingButton")) {
            this.IfBlueSnapContracts = true;
        }


    }

    // UserSettings
    public authHeader;   
    public TrialMessage: string = null;
    private trialTimer: any;
    private loginService: LoginService
    private messageWindow: MessageWindow = new MessageWindow();
    GetUserSetting() {


        if (this.loginService == null) {
            this.loginService = new LoginService();
            this.authHeader = new Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.authHeader.append('Token', SessionInfo.Token);
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator.Tenant;
            this.loginService.LoggedUserId = SessionInfo.LoggedUserId;
            this.loginService.LoggedUserEmail = SessionInfo.LoggedUserEmail;
        }
        //this.loginService.CurrentTenant = SessionLocator.TenantPM.Id;
        this.loginService.CheckTenantMangmnt(SessionLocator.LoggedUserId).subscribe(myResult2 => {
            SessionLocator.BlockType = null;
            var tt: TenantUserDataClass = myResult2;
            this.TrialMessage = "";
            this.messageWindow.Close();
            var user: UserPM = SessionLocator.LoggedUserPM;
            user.ExpirationDaysLeft = tt.ExpirationDaysLeft;
            user.ExpirationDate = tt.ExpirationDate;

            SessionLocator.TenantManagementJS.TrailDaysLeft = tt.TrailDaysLeft;
            SessionLocator.TenantManagementJS.PaidDaysLeft = tt.PaidDaysLeft;
            SessionLocator.TenantManagementJS.SuspendDaysLeft = tt.SuspendDaysLeft;
            var stopTimer = false;
            if (tt.DoBlocking) {

                SessionLocator.BlockType = tt.BlockType;
                this.SignoutClickedToBlockScreen();
                //BlockScreen
                stopTimer = true;

            }
            else {

                if (tt.ExpirationDate != null) {
                    if (tt.ExpirationDaysLeft >= 0) {
                        this.CheckUserExpiration(user);
                    }
                    else {
                        SessionLocator.BlockType = "company";

                        this.SignoutClickedToBlockScreen();

                        stopTimer = true;
                    }
                }


                if (tt.PaymentFailure) {
                    if (tt.TrailDaysLeft >= 0) {
                        this.CheckPaymentFailure();
                    }
                    else {
                        SessionLocator.BlockType = "suspend";
                        this.SignoutClickedToBlockScreen();
                        stopTimer = true;

                    }
                }

                else if (tt.IsTrial) {
                    if (tt.TrailDaysLeft >= 0) {
                        this.CheckTrialDays();
                    }
                    else {
                        SessionLocator.BlockType = "company";
                        this.SignoutClickedToBlockScreen();

                        stopTimer = true;
                    }
                }

                else if (!tt.IsRecurring && tt.PaidUntilDate != null) {
                    if (tt.PaidDaysLeft >= 0) {
                        this.CheckPaidUntilDays();
                    }
                    else {
                        SessionLocator.BlockType = "company";
                        this.SignoutClickedToBlockScreen();

                        stopTimer = true;

                    }
                }

             


            }


            if (!stopTimer)
                this.RunComponentTimerTrial();




        });





    }
    CheckPaymentFailure() {

        var HeaderMessage: string = "";
        var WindowMessage: string = "";
        this.messageWindow.Width = 600;
        this.messageWindow.Height = 150;
        var email = Environment.GetContactUsEmail();
        if (SessionLocator.TenantManagementJS.SuspendDate == null) {
            WindowMessage = "Your Bluesnap payment is failing, \nplease contact Bluesnap to fix the problem";
            HeaderMessage = "Your Bluesnap payment is failing";
        }
        else {
            HeaderMessage = "Your company subscription will expire in " + SessionLocator.TenantManagementJS.SuspendDaysLeft + " days.";
            WindowMessage = "Your company subscription will expire in " + SessionLocator.TenantManagementJS.SuspendDaysLeft + " days due to credit \ncard failure. \nPlease contact your e-commerce vendor or " + email;
        }

        this.messageWindow.Title = HeaderMessage;
        this.messageWindow.Message = WindowMessage;
        this.messageWindow.Show(this.messageWindow.Message);
    }
    CheckTrialDays() {
        var HeaderMessage: string = "";
        var WindowMessage: string = "";
        this.messageWindow.Width = 380;
        this.messageWindow.Height = 150;

        if (SessionLocator.TenantManagementJS.TrailDaysLeft <= 0) {
            HeaderMessage = "0 Trial Days Left !";
        }
        else {
            HeaderMessage = SessionLocator.TenantManagementJS.TrailDaysLeft + " Trial Days Left !";
        }

        this.messageWindow.Message = HeaderMessage;
        this.messageWindow.Show(this.messageWindow.Message);
        this.TrialMessage = HeaderMessage;
    }
    CheckPaidUntilDays() {
        var HeaderMessage: string = "";
        var WindowMessage: string = "";
        var email = Environment.GetContactUsEmail();

        if (SessionLocator.TenantManagementJS.PaidDaysLeft <= 31) {
            this.messageWindow.Width = 600;
            this.messageWindow.Height = 150;

            if (SessionLocator.TenantManagementJS.PaidDaysLeft == 0) {
                WindowMessage = "Your company subscription will expire in 0 days. \nTo renew please contact " + email;
                HeaderMessage = "Your company subscription will expire in 0 days.";
            }
            else {
                var AbsuluteValue;
                if (SessionLocator.TenantManagementJS.PaidDaysLeft >= 0)
                    AbsuluteValue = SessionLocator.TenantManagementJS.PaidDaysLeft;
                else AbsuluteValue = SessionLocator.TenantManagementJS.PaidDaysLeft * -1;

                WindowMessage = "Your company subscription will expire in " + AbsuluteValue + " days. \nTo renew please contact " + email;
                HeaderMessage = "Your company subscription will expire in " + AbsuluteValue + " days.";
            }


            this.messageWindow.Title = HeaderMessage;
            this.messageWindow.Message = WindowMessage;
            this.messageWindow.Show(this.messageWindow.Message);

        }

    }
    AccountAndTenantExpiration() {
        if (this.trialTimer != null) {
            this.GetUserSetting();
        }
        else {
            this.AccountAndTenantExpirationFunction();
        }


    }
    AccountAndTenantExpirationFunction() {
        this.TrialMessage = "";
        if (AppTool.IsNullOrEmpty(SessionLocator.BlockType)) {
            if (SessionLocator.LoggedUserPM.ExpirationDate != null) {
                if (SessionLocator.LoggedUserPM.ExpirationDaysLeft <= 7) {
                    this.CheckUserExpiration(SessionLocator.LoggedUserPM);


                }
                else {
                    if (SessionLocator.TenantManagementJS.PaymentFailure) {
                        this.CheckPaymentFailure();
                    }

                    else if (SessionLocator.TenantManagementJS.IsTrial) {
                        this.CheckTrialDays();
                    }

                    else if (!SessionLocator.TenantManagementJS.IsRecurring && !AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.PaidUntilDate + "")) {
                        this.CheckPaidUntilDays();
                    }


                }

            }
            else {
                if (SessionLocator.TenantManagementJS.PaymentFailure) {
                    this.CheckPaymentFailure();
                }

                else if (SessionLocator.TenantManagementJS.IsTrial) {
                    this.CheckTrialDays();
                }

                else if (!SessionLocator.TenantManagementJS.IsRecurring && !AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.PaidUntilDate)) {
                    this.CheckPaidUntilDays();
                }


            }

         
        }

        this.RunComponentTimerTrial();

    }
    CheckUserExpiration(loggedUser: UserPM) {

        var email = Environment.GetContactUsEmail();
        //if (SessionLocator.PrivateLableSettings) {
        //    email = SessionLocator.PrivateLableSettings.ContactUsEmail;
        //}
        var HeaderMessage: string = "";
        var WindowMessage: string = "";
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

    }
    RunComponentTimerTrial() {

        if (this.trialTimer) {
            clearTimeout(this.trialTimer);
        }

        this.trialTimer = setTimeout(() => this.AccountAndTenantExpiration(), 43200000);

    }

    // AmitalBrowserInUse
    _AmitalBrowserInUse: boolean = false;
    CheckAmitalBrowserInUse() {
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            this.AddTab();//this.Tabs.push(new SessionTabItem());
            this._AmitalBrowserInUse = AmitalGatewayUtil.Instance.AmitalBrowserInUse;
            let myCA23EditTab: SessionTabItem = this.Tabs[1];
            this.SelectionChanged(myCA23EditTab);
            let timerToken = //setTimeout(() => this.RunComponent(), 1);
                setTimeout(() => {
                    //if (tabItem.IsSelected) {
                    clearTimeout(timerToken);
                    if (!myCA23EditTab.IsSessionLoaded) {

                        let locs = this.AllLocations.toArray().filter(f => f.Code == 'SessionLocation');
                        let myLocation: LocationDirective = locs.filter(f => f.Index == myCA23EditTab.Index)[0];

                        //let viewContainerRef = myCA23EditTab.SessionComponent.viewContainerRef
                        if (myLocation != null) {
                            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Session/SessionComponent", myLocation.viewContainerRef).then(cmpRef => {
                                cmpRef.instance.SessionIndex = myCA23EditTab.Index;
                                cmpRef.instance.SessionTabItem = myCA23EditTab;
                                cmpRef.instance.SessionLocation = myLocation;
                                cmpRef.instance.ComponentRef = cmpRef;
                                SessionLocator.AddSession(cmpRef.instance);

                                myCA23EditTab.IsSessionLoaded = true;
                                myCA23EditTab.SessionComponent = cmpRef.instance;
                                SessionLocator.CurrentSession = myCA23EditTab.SessionComponent;
                                cmpRef.instance.RunComponent();
                                AmitalGatewayUtil.Instance.NoteUnifreightIamReady();

                            });
                        }
                    }
                    else {
                        AmitalGatewayUtil.Instance.NoteUnifreightIamReady();
                    }
                }, 500);
        }
    }
    public get IsAmitalBackButtonDisable() {

        if (AppTool.IsNullOrEmpty(AmitalGatewayUtil))
        { 
            return true;
        }
        if (AppTool.IsNullOrEmpty( AmitalGatewayUtil.Instance))
        { 
            return true;
        }
        return AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable;
    }

    AmitalBackButtonClicked() {

        if (AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable){
            return;
        }
        AmitalGatewayUtil.Instance.AmitalBackButtonClicked();
    }
    @HostListener('window:UnifaceRequestEvent', ['$event'])
    UnifaceRequest(event) {
        let myParam = event.detail;
        let myEditTab: SessionTabItem = this.Tabs[0];

        AmitalGatewayUtil.Instance.UnifaceRequest(myParam, myEditTab,
            () => {
                myEditTab.Header = myParam.formtitle;
                this.SelectionChanged(myEditTab);
                AmitalGatewayUtil.Instance.IsTabCA23 = false;
            },
            () => {
                this.SelectionChanged(this.Tabs[1]);
                AmitalGatewayUtil.Instance.IsTabCA23 = true;
            }
        );

    }

    // Notification Bell
    badjCount: number;
    IsBadjCountVisibile: boolean;
    left: number;
    top: number;
    right: number;
    height: number = 16;
    width: number = 16;
    BellClicked: boolean;
    IsControlVisibile: boolean = false;
    MouseInArea: boolean;
    private showLockIndicator: boolean = false;
    get ShowLockIndicator() { return this.showLockIndicator; }
    set ShowLockIndicator(newValue: boolean) {
        if (this.showLockIndicator != newValue) {
            this.showLockIndicator = newValue;
        }
    }

    notificationExtendedListService: NotificationExtendedListService = new NotificationExtendedListService();
    GetBadjCount() {
        this.notificationExtendedListService.GetNotificationsBadjCount(SessionLocator.LoggedUserId).subscribe(response => {
            if (response) {
                if (!response.HasError) {
                    this.badjCount = response.Result;
                    if (this.badjCount > 0) {
                        this.IsBadjCountVisibile = true;
                        if (this.badjCount < 10) {
                            this.top = 2;
                            this.right = 0;
                            this.left = 0;
                        }
                        else if (this.badjCount > 10 && this.badjCount < 100) {
                            this.left = 0;
                            this.top = 2;
                            this.right = 0;

                        }
                        else if (this.badjCount > 100) {
                            this.left = 0;
                            this.top = 2;
                            this.right = 0;
                            this.height = 17;
                            this.width = 17;
                        }

                    }

                    else {
                        this.IsBadjCountVisibile = false;
                    }

                }
            }
        });
    }
    StartApplicationTimers() {     
        var belltimer = this.initializeBadjCountTimer().subscribe(res => {

            if (FeatureLocator.HasFeaturePermession("General", "NOTIFICATIONBELL")) {
                this.GetBadjCount();
            }       
        });
    }
    initializeBadjCountTimer() {
        return Observable.interval(60000).timeInterval();
    }
    onBellButtonClicked() {
        this.BellClicked = true;
        if (this.IsControlVisibile) {
            this.IsControlVisibile = false;
        }
        else {
            this.IsControlVisibile = true;
            this.IsBadjCountVisibile = false;
          
        }
    }
    OnClickOutSide() {
        if (!this.BellClicked && !this.MouseInArea) {
            if (this.IsControlVisibile) {
                this.IsControlVisibile = false;

            }
        }
        this.BellClicked = false;
    }   
    OnControlMouseOver() {
        this.MouseInArea = true;

        var input = document.getElementById("111Bell");
        input.focus();
    }
    OnControlMouseOut() {

        this.MouseInArea = false;
    }
    OnBellBlur() {
        if (!this.MouseInArea) {
            this.IsControlVisibile = false;
        }

    }

    // RunComponent
    private Retries: number = 0;
    private timerToken: any;
    private isLoaderReady: boolean;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else if (!this.ApplicationLocation) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;

                SessionLocator.ApplicationLocation = this.ApplicationLocation;

                if (SessionLocator.BlockType) {
                    this.IsApplicationBlocked = true;
                }

                if (this.SelectedTabItem == null) {
                    this.SelectionChanged(this.Tabs[0]);
                }

                else {
                    if (this.SelectedTabItem.IsSelected) {
                        if (!this.SelectedTabItem.IsSessionLoaded) {

                            let locs = this.AllLocations.toArray().filter(f => f.Code == 'SessionLocation');
                            let location: LocationDirective = locs.filter(f => f.Index == this.SelectedTabItem.Index)[0];
                            if (location == null) {
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
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    AddTab() {
        SessionLocator.Index += 1;
        this.Tabs.push(new SessionTabItem());
        this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
        this.Retries = 0;
        this.RunComponentTimer();

        if (this.Tabs.length > 4) this.IsShowLastSuccessfulLoginComponent = false;
  
    }
    SelectionChanged(clickdTab: SessionTabItem) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;
                
                    this.Tabs.forEach((item) => {
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
                SessionLocator.CurrentSession = this.SelectedTabItem.SessionComponent;              
            }

            else {
                this.CreateSession(clickdTab);
            }
        }
    }
    CreateSession(tabItem: SessionTabItem) {
        if (this.isLoaderReady) {
            if (tabItem.IsSelected) {
                if (!tabItem.IsSessionLoaded) {

                    let locs = this.AllLocations.toArray().filter(f => f.Code == 'SessionLocation');
                    let myLocation: LocationDirective = locs.filter(f => f.Index == tabItem.Index)[0];

                    if (myLocation != null) {
                        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Session/SessionComponent", myLocation.viewContainerRef).then(cmpRef => {
                            cmpRef.instance.SessionIndex = tabItem.Index;
                            cmpRef.instance.SessionTabItem = tabItem;                            
                            cmpRef.instance.ComponentRef = cmpRef;
                            SessionLocator.AddSession(cmpRef.instance);

                            tabItem.IsSessionLoaded = true;
                            tabItem.SessionComponent = cmpRef.instance;
                            SessionLocator.CurrentSession = tabItem.SessionComponent;                           
                            cmpRef.instance.RunComponent();

                            if (tabItem.Index == 0) {

                                if (this.IsNewSignupTenant) {
                                    cmpRef.instance.SessionInitialize.subscribe(s => {
                                        this.RunSignupWizard();
                                    });
                                }

                                else{
                                    this.AccountAndTenantExpiration();
                                }
                            }                            
                        });
                    }
                }
            }
        }      
    }

    //CloseTab(tabItem: SessionTabItem) {
    //    var isNeedingConfirmation = false;
    //    var tab = SessionLocator.CurrentSession.CurrentEditComponent; 
    //    if (tabItem.SessionComponent.CurrentEditComponent != null) {
    //        tab = tabItem.SessionComponent.CurrentEditComponent;
    //        SessionLocator.CurrentSession = tabItem.SessionComponent;
    //    }

    //    if (tab) {
    //        isNeedingConfirmation = tab.NeedCloseConfirmation();
    //    }

    //    if (isNeedingConfirmation) {
    //        var confirmWindow = new ConfirmWindow();
    //        confirmWindow.Width = 450;
    //        confirmWindow.Height = 190;
    //        confirmWindow.ShowCancelButton = true;
    //        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
    //        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
    //        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
    //        confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator.Translate(tab.ObjectTableName)));
    //        confirmWindow.WindowClosed.subscribe((event: any) => {
    //            if (confirmWindow.Yes) {


    //                if (!this.SaveCompletedEvent) {
    //                    this.SaveCompletedEvent = tab.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
    //                        if (isSaveSuccess) {
    //                            this.Close(tabItem);
    //                        }

    //                        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    //                        this.SaveCompletedEvent = null;
    //                    });
    //                }

    //                tab.SaveChanges();
    //            }

    //            else if (confirmWindow.No) {
    //                this.Close(tabItem);
    //            }
    //        });
    //    }

    //    else {
    //        this.Close(tabItem);
    //    }
    //}

    CloseTab(tabItem: SessionTabItem) {

        var ClosedTabEditComponent = tabItem.SessionComponent.CurrentEditComponent;

        if (ClosedTabEditComponent) {
            if (ClosedTabEditComponent.NeedCloseConfirmation()) {

                this.SelectionChanged(tabItem);

                var confirmWindow = new ConfirmWindow();
                confirmWindow.IsOverAll = true;
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator.Translate(ClosedTabEditComponent.ObjectTableName)));
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        if (!this.SaveCompletedEvent) {
                            this.SaveCompletedEvent = ClosedTabEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                if (isSaveSuccess) {
                                    this.Close(tabItem);
                                }

                                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                                this.SaveCompletedEvent = null;
                            });
                        }

                        ClosedTabEditComponent.SaveChanges();
                    }

                    else if (confirmWindow.No) {
                        this.Close(tabItem);
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
    }

    Close(tabItem: SessionTabItem) {

        var itemIndex = this.Tabs.indexOf(tabItem);
        if (itemIndex > -1) {

            this.Tabs.splice(itemIndex, 1);

            if (tabItem.IsSelected) {

                this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
            }

            tabItem.SessionComponent.DestroySession();
            tabItem = null;

            if (this.Tabs.length <= 4) {
                if (!this.IsShowLastSuccessfulLoginComponent) this.IsShowLastSuccessfulLoginComponent = true;
            }
        }
    }

    private SaveCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;

    }

    RunSignupWizard() {
        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/Maintenance/Wizard/WizardBaseComponent", SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                //cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.RunComponent();

                cmpRef.instance.SignOutCompleted.subscribe(s => {
                    SessionLocator.HomeComponent.SignoutClicked();
                });

                cmpRef.instance.SaveCompleted.subscribe(s => {
                    SessionLocator.RootComponent.ViewHomeComponent();
                });
            });
    }

    // App Header Commands
    CustomizationClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Object Names";
        logWindow.IsShowCloseButton = true;
        logWindow.Width = 800;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationMainComponent');
    }
    TranslateLabelsClicked() {
        var windowTitle = "Select Translation Language";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/TranslationLabels/SelectLanguagesComponent');
    }
    SignatureClicked() {
        if (!SessionLocator.CurrentSession.IsOpenSignatureWindowFromSetting) {
            SessionLocator.CurrentSession.IsOpenSignatureWindowFromSetting = true;
            this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response => {

                var windowArgs: any = {};
                windowArgs.DataViewModel = this;
                windowArgs.PageType = "Signature";
                windowArgs.TemplateId = SessionLocator.LoggedUserId;
                windowArgs.Tenant = SessionLocator.Tenant;

                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;

                var logWindow = new LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Html Template";


                logWindow.WindowArgs = windowArgs;

                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");

                logWindow.WindowClosed.subscribe(($event: any) => {
                    SessionLocator.CurrentSession.IsOpenSignatureWindowFromSetting = false;

                });
            });



        }
    }
    ChangePasswordClicked() {
        if (!SessionLocator.CurrentSession.IsOpenChangePasswordWindowFromSetting) {
            SessionLocator.CurrentSession.IsOpenChangePasswordWindowFromSetting = true;
            var logWindow = new LogitudeWindow();
                logWindow.Width = 600;
                logWindow.Height = 400;
            logWindow.Title = "Change User Password";
            this._entityResourceService.getEntityResourceByTableName("User").subscribe(response => {
                logWindow.DataContext = this;
                logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
            });

            logWindow.WindowClosed.subscribe(($event: any) => {
                SessionLocator.CurrentSession.IsOpenChangePasswordWindowFromSetting = false;

            });


        }

    }
    CurrenciesRatesClicked() {
        var windowTitle = "Edit exchange rates";
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(response => {
            logWindow.Show('./Common/Components/Maintenance/RatesMainTabComponent');
        });
    }
    DataBackupClicked() {


        if (!SessionLocator.CurrentSession.IsOpenDatabaseBackupWindowFromSetting) {
            SessionLocator.CurrentSession.IsOpenDatabaseBackupWindowFromSetting = true;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 140;
            logWindow.Title = "Database Backup";
            logWindow.DataContext = this;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/DatabaseBackupComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                SessionLocator.CurrentSession.IsOpenDatabaseBackupWindowFromSetting = false;

            });
        }


    }
    

    DocumentsBackupClicked() {

        if (!SessionLocator.CurrentSession.IsOpenDocumentBackupWindowFromSetting) {
            SessionLocator.CurrentSession.IsOpenDocumentBackupWindowFromSetting = true;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 1360;
            logWindow.Height = 600;
            logWindow.Title = "Documents Backup";
            logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/DocumentFilingBackupBatchesComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                SessionLocator.CurrentSession.IsOpenDocumentBackupWindowFromSetting = false;

            });
        }

    }


    private SubscribeToLogitude(EmptyOrError: boolean, contractId: string, temp) {

        var storeid: string = "543002";
        var isSandbox = false;
        if (!AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeid = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }

        else if (EmptyOrError || AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;

            if (SessionLocator.TenantManagementJS.CountryName == "Israel") {
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
        var numberofUsers: number = AppTool.IsNullOrZero(SessionLocator.TenantManagementJS.BluesnapContractQTY) ? 1 : SessionLocator.TenantManagementJS.BluesnapContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;

        }
        if (AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;;
            }
            else {
                link = "https://bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;;;
            }
        }

        var win = window.open(link, '_blank');
        win.focus();
    }


    private SubscribeToAWB(EmptyOrError: boolean, contractId: string, temp) {

        var storeId: string = "543002";
        var isSandbox = false;
        if (!AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || AppTool.IsNullOrEmpty(contractId) )) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }

        else if (EmptyOrError || AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;

            contractId = "3285402";
        }

        else {
            temp = temp.Token;
        }

        var link = "";
        var numberofUsers: number = AppTool.IsNullOrZero(SessionLocator.TenantManagementJS.BluesnapEAWBContractQTY) ? 1 : SessionLocator.TenantManagementJS.BluesnapEAWBContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;

        }
        if (AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
        }

        var win = window.open(link, '_blank');
        win.focus();
    }



    private BuyToAWB(EmptyOrError: boolean, contractId: string, temp) {

        var storeId: string = "543002";
        var isSandbox = false;
        if (!AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }


        else if (EmptyOrError || AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;

            contractId = "3233898";
        }

        else {
            temp = temp.Token;
        }
        var link = "";
        var numberofUsers: number = AppTool.IsNullOrZero(SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY) ? 1 : SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;

        }
        if (AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
        }

        var win = window.open(link, '_blank');
        win.focus();
    }


    private OneTimeBuy(EmptyOrError: boolean, contractId: string, temp) {
        var storeId: string = "543002";
        var isSandbox = false;
        if (!AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }

        else if (EmptyOrError || AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;
            if (SessionLocator.TenantManagementJS.CountryName == "Israel") {
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
        var numberofUsers: number = AppTool.IsNullOrZero(SessionLocator.TenantManagementJS.BluesnapOneTimeContractQTY) ? 1 : SessionLocator.TenantManagementJS.BluesnapOneTimeContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;

        }
        if (AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://bluesnap.com/buynow/checkout?sku" + contractId  + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
        }

        var win = window.open(link, '_blank');
        win.focus();
    }


    private SubscribeToCRM(EmptyOrError: boolean, contractId: string, temp) {
        var storeId: string = "543002";
        var isSandbox = false;
        if (!AppTool.IsNullOrEmpty(temp.ContractId) && (EmptyOrError || AppTool.IsNullOrEmpty(contractId))) {
            contractId = temp.ContractId;
            storeId = temp.Storeid;
            temp = temp.Token;
            isSandbox = true;
        }
        else if (EmptyOrError || AppTool.IsNullOrEmpty(contractId)) {
            temp = temp.Token;

            if (SessionLocator.TenantManagementJS.CountryName == "Israel") {
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
        var numberofUsers: number = AppTool.IsNullOrZero(SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY) ? 1 : SessionLocator.TenantManagementJS.BluesnapEAWBSContractQTY;
        if (isSandbox) {
            var link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
        }
        else {
            var link = "https://checkout.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&currency=USD&enc=" + temp + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;

        }
        if (AppTool.IsNullOrEmpty(temp)) {
            if (isSandbox) {
                link = "https://sandbox.bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
            else {
                link = "https://bluesnap.com/buynow/checkout?sku" + contractId + "=" + numberofUsers + "&language=ENGLISH&currency=USD&custom1=" + SessionInfo.LoggedUserTenant;
            }
        }

        var win = window.open(link, '_blank');
        win.focus();
    }

    SubscribeClicked(code: string) {
        var link: string = "";
        var EmptyOrError: boolean = true;

            switch (code) {
                case "LOG":
                    {
                        var myService: CommonDomainService = new CommonDomainService();
                        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

                        myService.GetBlueSnapSecretToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
                            var temp: BluesnapParameters = myResult.Result;
                            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
                            var contractId: string = SessionLocator.TenantManagementJS.BluesnapContractId;
                            if (!AppTool.IsNullOrEmpty(contractId)) {
                                this.BluesnapContractService.get(contractId).subscribe((res: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    if (res) {
                                        if (!res.HasError) {
                                            contractId = res.Result.ContractId;
                                            EmptyOrError = false;
                                            this.SubscribeToLogitude(EmptyOrError, contractId, temp);
                                        }
                                        else {
                                            this.SubscribeToLogitude(EmptyOrError, null, temp);

                                        }
                                    }
                                    else {
                                        this.SubscribeToLogitude(EmptyOrError, null, temp);
                                    }
                                });
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.SubscribeToLogitude(EmptyOrError, contractId,temp);
                            }
                         
                        });
                        break;
                    }

                case "AWB":
                    {
                        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

                        var myService: CommonDomainService = new CommonDomainService();
                        myService.GetBlueSnapSecretToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
                            var temp: BluesnapParameters = myResult.Result;
                            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
                            var contractId: string = SessionLocator.TenantManagementJS.BluesnapEAWBContractId;
                            if (!AppTool.IsNullOrEmpty(contractId)) {
                                this.BluesnapContractService.get(contractId).subscribe((res: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    if (res) {
                                        if (!res.HasError) {
                                            contractId = res.Result.ContractId;
                                            EmptyOrError = false;
                                            this.SubscribeToAWB(EmptyOrError, contractId, temp);
                                        }
                                        else {
                                            this.SubscribeToAWB(EmptyOrError, null, temp);

                                        }
                                    }
                                    else {
                                        this.SubscribeToAWB(EmptyOrError, null, temp);
                                    }
                                });
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.SubscribeToAWB(EmptyOrError, contractId, temp);
                            }
                        });
                        break;                        
                    }

                case "BUY":
                    {
                        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

                        var myService: CommonDomainService = new CommonDomainService();
                        myService.GetBlueSnapSecretToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
                            var temp: BluesnapParameters = myResult.Result;
                            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
                            var contractId: string = SessionLocator.TenantManagementJS.BluesnapEAWBSContractId;

                            if (!AppTool.IsNullOrEmpty(contractId)) {
                                this.BluesnapContractService.get(contractId).subscribe((res: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    if (res) {
                                        if (!res.HasError) {
                                            contractId = res.Result.ContractId;
                                            EmptyOrError = false;
                                            this.BuyToAWB(EmptyOrError, contractId, temp);

                                        }
                                        else {
                                            this.BuyToAWB(EmptyOrError, null, temp);

                                        }
                                    }
                                    else {
                                        this.BuyToAWB(EmptyOrError, null, temp);
                                    }
                                });
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.BuyToAWB(EmptyOrError, contractId, temp);

                            }

                          
                        });
                        break;
                    }



                case "CRM":
                    {
                        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

                        var myService: CommonDomainService = new CommonDomainService();
                        myService.GetBlueSnapSecretToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
                            var temp: BluesnapParameters= myResult.Result;
                            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
                            var contractId: string = SessionLocator.TenantManagementJS.BluesnapCRMContractId;
                            if (!AppTool.IsNullOrEmpty(contractId)) {
                                this.BluesnapContractService.get(contractId).subscribe((res: ServiceResponse) => {
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                    if (res) {
                                        if (!res.HasError) {
                                            contractId = res.Result.ContractId;
                                            EmptyOrError = false;
                                            this.SubscribeToCRM(EmptyOrError, contractId, temp);
                                        }
                                        else {
                                            this.SubscribeToCRM(EmptyOrError, null, temp);

                                        }
                                    }                                        
                                        else {
                                        this.SubscribeToCRM(EmptyOrError, null, temp);
                                        }
                                    
                                });
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.SubscribeToCRM(EmptyOrError, contractId, temp);

                            }                          
                        });
                        break;
                    }



                case "OTP":
                    {
                        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

                        var myService: CommonDomainService = new CommonDomainService();
                        myService.GetBlueSnapSecretToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
                            SessionLocator.CurrentSession.StopBusyIndicator();
                            var temp: BluesnapParameters = myResult.Result;
                            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
                            var contractId: string = SessionLocator.TenantManagementJS.BluesnapOneTimeContract;
                            if (!AppTool.IsNullOrEmpty(contractId)) {
                                EmptyOrError = false;
                                this.OneTimeBuy(EmptyOrError, contractId, temp);                                    
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.OneTimeBuy(EmptyOrError, contractId, temp);
                            }
                         
                        });
                        break;
                    }
            
        }
    }
    public ManageBluesnapAccountClicked() {


        var myService: CommonDomainService = new CommonDomainService();
        myService.GetBlueSnapToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
            var temp = myResult.Result;
            temp = temp.Token;
            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
            var link = "https://checkout.bluesnap.com/jsp/account_login.jsp";
            if (!AppTool.IsNullOrEmpty(temp)) {         
                link = "https://www.bluesnap.com/jsp/entrance.jsp?target=cp&token=" + temp + "&pageToShow=my_account.jsp"
            }
            var win = window.open(link, '_blank');
            win.focus();
        });        
    }
    HelpButtonClicked() {
        ServiceLocator.SendTotangoUserActivity("Help Center", "Help Icon");
      
        window.open(AppTool.GetLogitudeURL() + 'TrainingResourcesHTML/TrainingResourcesMainPage.aspx?tempId='+ SessionInfo.DocumentDownloadToken, '_blank');
    }
    SignoutClicked() {
        SessionLocator.Index = 0;

        SessionLocator.AllSessions.forEach((item) => {
            item.DestroySession();
        });

        this.SignoutCompleted.emit("event from child");
    }

    public IsApplicationBlocked: boolean = false;
    SignoutClickedToBlockScreen() {
        this.IsApplicationBlocked = true;
        //SessionLocator.Index = 0;

        //SessionLocator.AllSessions.forEach((item) => {
        //    item.DestroyMenuReferences();
        //    item.DestroyListComponentReferences();
        //    item.MainMenuComponent.BlockScreenLoad();
        //});

        if (this.Tabs.length > 1) {
            for (var i = this.Tabs.length - 1; i > 0; i--) {

                var item: SessionTabItem = this.Tabs[i];
                this.Tabs.splice(i, 1);
                item.SessionComponent.DestroySession();
            }
        }

        this.SelectionChanged(this.Tabs[0]);

        this.Tabs[0].SessionComponent.MainMenuComponent.BlockScreenLoad();
        

       // this.SignoutCompleted.emit('Block');

    }



    Clos555e(tabItem: SessionTabItem) {

        var itemIndex = this.Tabs.indexOf(tabItem);
        if (itemIndex > -1) {

            this.Tabs.splice(itemIndex, 1);

            if (tabItem.IsSelected) {

                this.SelectionChanged(this.Tabs[this.Tabs.length - 1]);
            }

            tabItem.SessionComponent.DestroySession();
            tabItem = null;

            if (this.Tabs.length <= 4) {
                if (!this.IsShowLastSuccessfulLoginComponent) this.IsShowLastSuccessfulLoginComponent = true;
            }
        }
    }

    OnClearCache() {
        window.ObjectFields = [];
        window.TextCodes = [];
        window.CachedTables = [];
        console.log("cache cleared!");
        SessionLocator.StopApplicationTimers();
    }
    TranslationClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.Title = "Translation";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Translations/TranslationComponent');
    }
    DefaultTranslationClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.ShowHeaderButtons = true;
        logWindow.Title = "Default Translation";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Translations/DefaultTranslationComponent');
    }
    LanguageSettingsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Language Settings";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/LanguageSettings/LanguageSettingsComponent');
    }
    ConnectToDropBox() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetDropBoxAuthURI(SessionLocator.Tenant).subscribe((myResult) => {
            var temp = myResult.Result;
            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
            window.open(temp, 'Authenticate with Dropbox', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=1300,height=650');
        });
    }
    CreateCommLogForDropBox() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetDropBoxComLog(SessionLocator.Tenant).subscribe((myResult) => {
            var temp = myResult.Result;
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 200;
            this.messageWindow.Title = "DropBox Communicaiton Log";
            this.messageWindow.Message = "Communicaiton Log Created For DropBox Successfully";
            this.messageWindow.Show(this.messageWindow.Message);

        });
    }
    LoadSampleDataClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 470;
        logWindow.Height = 480;
        logWindow.Title = "Load Sample Data";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/LoadSampleDataComponent');
    }

    ShowQueryBuilderClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
    }

    FillStorageClicked() {
        try {
            var i;
            for (i = 1; i <= 10000; i++) {
                localStorage.setItem('test', new Array(i * 100000).join('a'));
            }
        } catch (error) {
            //console.log("test stopped at i: " + i);
            try {
                var j;
                for (j = 1; j <= 100; j++) {
                    localStorage.setItem('test2', new Array(j * 1000).join('a'));
                }
            } catch (error) {
                //console.log("test2 stopped at j: " + j);
                try {
                    var k;
                    for (k = 1; k <= 1000; k++) {
                        localStorage.setItem('test3', new Array(k).join('a'));
                    }
                } catch (error) {
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
    }

    ShowDocTypesDefScreen() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 350;
        newWindow.Title = "Define document types for digital sign";
        //var windowArgs: any = {};
        //windowArgs.IsNew = true;
        //newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control);

        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DigitalSignDocTypeComponent');

        newWindow.WindowClosed.subscribe(($event: any) => {
            //if ($event == "MyShipmentAdded") {
            //    this.SelectedFilter = "My Shipments";
            //    this.LoadImporterShipments();
            //}
        });
    }

    private setCookie(name: string, value: string, expireDays: number, path: string = '') {
        let d: Date = new Date();
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        let expires: string = `expires=${d.toUTCString()}`;
        let cpath: string = path ? `; path=${path}` : '';
        document.cookie = `${name}=${value}; ${expires}${cpath}`;
    }





}

export class SessionTabItem {
    public Index: number;
    public IsSelected: boolean = false;
    public IsSessionLoaded: boolean = false;
    public SessionComponent: SessionComponent;
    constructor() {
        this.Index = SessionLocator.Index;
    }

    ChangeSessionHeader(args: any) {
        if (args) {
            var Icon: string = args['Icon'];
            var Text: string = args['Text'];
            var TextCode: string = args['TextCode'];
            var MenuTextCode: string = args['MenuTextCode'];
            
            if (Text) {
                this.SetSessionTabHeader(Text);
            }

            if (MenuTextCode) {
                this.SetSessionTabIcon(AppTool.GetMainMenuIconCode(MenuTextCode));
                this.SetSessionTabHeader(TextCodeTranslator.Translate(MenuTextCode));
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
    }

    public Header: string = null;
    public IconSource: string = null;
    public IconSourceGray: string = null;
    public DirectionId: string = null;
    public TransportId: string = null;
    SetSessionTabIcon(myIcon: string) {
        this.IconSource = "./Images/Menu/" + myIcon + ".png";
        this.IconSourceGray = "./Images/Menu/" + myIcon + ".Gray.png";
    }
    SetSessionTabHeader(myHeader: string) {
        this.Header = myHeader;
    }
}

export class BluesnapParameters {
    private token: string;
    public get Token() { return this.token; }
    public set Token(value: string) { this.token = value; }

    private storeid: string;
    public get Storeid() { return this.storeid; }
    public set Storeid(value: string) { this.storeid = value; }


    private contractId: string;
    public get ContractId() { return this.contractId; }
    public set ContractId(value: string) { this.contractId = value; }

}
export class TenantUserDataClass {
    private id: string;
    public get Id() { return this.id; }
    public set Id(value: string) { this.Id = value; }
    private doBlocking: boolean;
    public get DoBlocking() { return this.doBlocking; }
    public set DoBlocking(value: boolean) { this.doBlocking = value; }

    private trailDaysLeft: number;
    public get TrailDaysLeft() { return this.trailDaysLeft; }
    public set TrailDaysLeft(value: number) { this.trailDaysLeft = value; }


    private paidDaysLeft: number;
    public get PaidDaysLeft() { return this.paidDaysLeft; }
    public set PaidDaysLeft(value: number) { this.paidDaysLeft = value; }


    private expirationDaysLeft: number;
    public get ExpirationDaysLeft() { return this.expirationDaysLeft; }
    public set ExpirationDaysLeft(value: number) { this.expirationDaysLeft = value; }


    private isTrial: boolean;
    public get IsTrial() { return this.isTrial; }
    public set IsTrial(value: boolean) { this.isTrial = value; }

    private paymentFailure: boolean;
    public get PaymentFailure() { return this.paymentFailure; }
    public set PaymentFailure(value: boolean) { this.paymentFailure = value; }

    private isRecurring: boolean;
    public get IsRecurring() { return this.isRecurring; }
    public set IsRecurring(value: boolean) { this.isRecurring = value; }

    private suspendDaysLeft: number;
    public get SuspendDaysLeft() { return this.suspendDaysLeft; }
    public set SuspendDaysLeft(value: number) { this.suspendDaysLeft = value; }


    private blockType: string;
    public get BlockType() { return this.blockType; }
    public set BlockType(value: string) { this.blockType = value; }

    private paidUntilDate: Date;
    public get PaidUntilDate() { return this.paidUntilDate; }
    public set PaidUntilDate(value: Date) { this.paidUntilDate = value; }


    private expirationDate: Date;
    public get ExpirationDate() { return this.expirationDate; }
    public set ExpirationDate(value: Date) { this.expirationDate = value; }




}
