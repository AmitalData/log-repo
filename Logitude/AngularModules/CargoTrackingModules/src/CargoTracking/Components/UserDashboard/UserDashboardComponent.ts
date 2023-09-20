import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { Component, ViewChild, ElementRef, AfterViewInit, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { filter } from 'rxjs/operators';
import { Location } from '@angular/common';
import { SharedService } from 'src/CargoTracking/Services/Others/SharedService';
import { MatDialog } from '@angular/material/dialog';
import { SessionExpirationComponent } from './session-expiration/session-expiration.component';
import { SessionTimeoutServiceService } from 'src/Infrastructure/Services/session-timeout-service.service';
import { Subscription } from 'rxjs';
import { DocumentDownloadTokenUpdateService } from 'src/Infrastructure/Services/document-download-token-update.service';
import { animate, state, style, transition, trigger } from '@angular/animations';



@Component({
    selector: 'UserDashboard',
    templateUrl: './UserDashboardComponent.html',
    styleUrls: ['./UserDashboardComponent.css'],
    animations: [
        trigger('fade', [
          state('void', style({ opacity: 0, transform: 'scale(0.8)' })),
          transition(':enter', [
            animate('500ms cubic-bezier(0.35, 0, 0.25, 1)', style({ opacity: 1, transform: 'scale(1)' }))
          ]),
          transition(':leave', [
            animate('300ms cubic-bezier(0.35, 0, 0.25, 1)', style({ opacity: 0, transform: 'scale(0.8)' }))
          ])
        ])
      ]
})
export class UserDashboardComponent implements AfterViewInit, OnInit, OnDestroy {

    @ViewChild('input') input: ElementRef;
    isLoading: boolean = false;
    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    Shipments: CargoTrackingShipmentList[] = [];
    UserName: string;
    ConnectedCustomers: string[] = [];
    IsBrandingDataLoaded: boolean = false;
    UserNameFirstLetters: string;
    currentRoute: string;
    baseURL;
    displayCookies:boolean=false;
    
    get tenant() {
        return CargoTrackingBrandingData.Tenant;
    }
    set tenant(val: number) {
        CargoTrackingBrandingData.Tenant = val;
    }

    constructor(
        private brandingService: CargoTrackingBrandingDataExtendedService,
        private loginService: LoginExtendedService,
        private location: Location,
        public sessionTimeoutServiceService: SessionTimeoutServiceService,
        public documentDownloadTokenUpdateService: DocumentDownloadTokenUpdateService,
        private router: Router,
        public dialog: MatDialog,
        @Inject('BASE_URL') baseUrl: string,
        public sharedService: SharedService) {
        this.baseURL = baseUrl;
        this.handleSessionTimeOut();
        this.InitComponent();
    }



    private SetDefaultBackgroundColor() {
        document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
    }

    private InitComponent() {

        this.SetDefaultBackgroundColor();
        this.GetBrandingData();
        this.GetLoggedUserIfNotSet();
        //this.Authenticate();
        this.LoggedUserData();
        this.sessionTimeoutServiceService.RunSessionTimeOut();
        this.documentDownloadTokenUpdateService.startIntervalUpdate();

    }
    LoggedUserData() {
        SessionInfo.Token = sessionStorage.getItem("Token");
    }


    private GetLoggedUserIfNotSet() {
        if (SessionInfo.LoggedContact) {
            this.UserName = SessionInfo.LoggedContact.EnglishName;
            this.SetFirstUserLetters(SessionInfo.LoggedContact.EnglishName);
        } else if (SessionInfo.LoggedUserPM) {
            this.UserName = SessionInfo.LoggedUserPM.EnglishName;
            this.SetFirstUserLetters(SessionInfo.LoggedUserPM.EnglishName);
        } else {
            var tenant = sessionStorage.getItem("LoggedUserTenant");
            var email = sessionStorage.getItem("LoggedUserEmail");
            this.GetLoggedUserPM(email, tenant);
        }

    }

    private GetLoggedUserPM(email: any, tenant: any) {
        this.loginService.GetLoggedUser(email, tenant).subscribe((loggedUserPM: any) => {
            if (loggedUserPM) {
                SessionInfo.LoggedUserPM = loggedUserPM;
                this.UserName = SessionInfo.LoggedUserPM.EnglishName;
                this.SetFirstUserLetters(SessionInfo.LoggedUserPM.EnglishName);
            } else {
                this.GetLoggedContact();
            }
        });
    }

    private GetLoggedContact() {
        this.brandingService.GetLoggedContact().subscribe((loggedContact: any) => {
            if (loggedContact) {
                SessionInfo.LoggedContact = loggedContact;
                this.UserName = SessionInfo.LoggedContact.EnglishName;
                this.SetFirstUserLetters(SessionInfo.LoggedContact.EnglishName);

            }
        });
    }

    private SetFirstUserLetters(userName: string) {
        if (userName) {
            var splitted = userName.split(" ");
            if (splitted.length == 1)
                this.UserNameFirstLetters = this.getFirstCharacter(splitted[0]);
            else if (splitted.length >= 2)
                this.UserNameFirstLetters = this.getFirstCharacter(splitted[0]) + this.getFirstCharacter(splitted[1]);

        }
        if (!this.UserNameFirstLetters || this.UserNameFirstLetters.length == 0)
            this.UserNameFirstLetters = "Aa";
    }
    getFirstCharacter(text: string) {
        var result = '';
        if (text && text.length > 0) {
            return text[0];
        }
        return result;
    }

    private Authenticate() {
        let token = sessionStorage.getItem("Token");
        if (!token)
            this.router.navigate(["cargo-tracking", "login"]);
    }

    isNavOpened = false;

    openNav() {
        this.isNavOpened = !this.isNavOpened;
    }


    SignOutClicked() {
        this.tenant = +sessionStorage.getItem("LoggedUserTenant");
        sessionStorage.clear();
        if (this.tenant)

            this.router.navigate(["cargo-tracking/login"]);//,{ queryParams: {tenant: this.tenant}}
        else
            this.router.navigate(["cargo-tracking/login"]);
    }


    public get InvertedLogoURL() {
        return CargoTrackingBrandingData.InvertedLogoURL;
    }

    public RedirectTo401Page() {
        this.router.navigate(['Error401']);
    }


    private GetBrandingData() {
        if (this.tenant)
            this.IsBrandingDataLoaded = true;


        this.brandingService.GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(this.baseURL))
            .subscribe((response: ServiceResponse) => {
                if (response.Result) {
                    if (response?.Result?.ForceHttps)
                        this.RedirectAppToHttps();

                    ServiceHelper.SetCargoTrackingDate(response.Result, this.baseURL);

                    this.IsBrandingDataLoaded = true;
                }
                else {
                    this.RedirectTo401Page();
                }

            });
    }
    RedirectAppToHttps() {
        const isLocally = window.location.origin.indexOf('localhost') > -1;

        if (!isLocally && location.protocol === 'http:') {
            window.location.href = location.href.replace('http', 'https');
        }
    }
    ngOnInit(): void {
        
        this.displayCookies =JSON.parse(sessionStorage.getItem("DisplayCookies"));
        this.SubscribeRoutingEvents();
       

        // this.sharedService.isAdvancedFilterOpened$.subscribe(x => console.log('isAdvancedFilterOpened$', x));
    }
    private SubscribeRoutingEvents() {
        this.router.events.pipe(
            filter((e: any): e is NavigationEnd => e instanceof NavigationEnd)
        ).subscribe((e: NavigationEnd) => {
            this.currentRoute = e.url;
        });
    }
    ngAfterViewInit() {
    }
    get ComapnyLogo() {
        return CargoTrackingBrandingData.ComapnylogoURL;
    }
    get BrowserIcon() {
        return CargoTrackingBrandingData.BrowserIconURL;
    }
    get BackGroundImg() {
        return CargoTrackingBrandingData.BackgroundURL;
    }
    get ShipmentHeaderImage() {
        return CargoTrackingBrandingData.ShipmentHeaderURL;
    }

    BackLinkClicked() {
        this.router.navigate(['cargo-tracking', 'shipments']);
        this.sharedService.updateValue(false);
    }
    handleSessionTimeOutSubscription: Subscription;
    handleSessionTimeOut() {
        this.handleSessionTimeOutSubscription = this.sessionTimeoutServiceService.onExpirToken.subscribe(e => {
            this.openDialog()
        });
    }
    ngOnDestroy(): void {
        this.handleSessionTimeOutSubscription.unsubscribe();
    }
    openDialog() {
        const dialogRef = this.dialog.open(SessionExpirationComponent, { closeOnNavigation: false, disableClose: true });

        dialogRef.afterClosed().subscribe(result => {
            this.SignOutClicked();
        });
    }

    dismissCookieNotice() {
        this.displayCookies = false;
        sessionStorage.setItem("DisplayCookies",JSON.stringify(false));
    }





}
