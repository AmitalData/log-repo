import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { Component, ViewChild, ElementRef, AfterViewInit, Inject, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { filter } from 'rxjs/operators';



@Component({
    selector: 'UserDashboard',
    templateUrl: './UserDashboardComponent.html',
    styleUrls: ['./UserDashboardComponent.css']
})
export class UserDashboardComponent implements AfterViewInit, OnInit
{

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

    get tenant()
    {
        return CargoTrackingBrandingData.Tenant;
    }
    set tenant(val: number)
    {
        CargoTrackingBrandingData.Tenant = val;
    }

    constructor(
        private brandingService: CargoTrackingBrandingDataExtendedService,
        private loginService: LoginExtendedService,
        private router: Router,
        @Inject('BASE_URL') baseUrl: string) {
        this.baseURL = baseUrl;
        this.InitComponent();
    }

    private SetDefaultBackgroundColor()
    {
        document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
    }

    private InitComponent()
    {

        this.SetDefaultBackgroundColor();
        this.GetBrandingData();
        this.GetLoggedUserIfNotSet();
        //this.Authenticate();
        this.LoggedUserData();
    }
    LoggedUserData() {
        SessionInfo.Token = sessionStorage.getItem("Token");
    }


    private GetLoggedUserIfNotSet()
    {
        if(SessionInfo.LoggedContact){
            this.UserName = SessionInfo.LoggedContact.EnglishName;
            this.SetFirstUserLetters(SessionInfo.LoggedContact.EnglishName);
        }else if (SessionInfo.LoggedUserPM) {
            this.UserName = SessionInfo.LoggedUserPM.EnglishName;
            this.SetFirstUserLetters(SessionInfo.LoggedUserPM.EnglishName);
        } else{
            var tenant = sessionStorage.getItem("LoggedUserTenant");
            var email = sessionStorage.getItem("LoggedUserEmail");
            this.GetLoggedUserPM(email, tenant);
        }

    }

    private GetLoggedUserPM(email: any, tenant: any)
    {
        this.loginService.GetLoggedUser(email, tenant).subscribe((loggedUserPM: any) =>
        {
            if (loggedUserPM) {
                SessionInfo.LoggedUserPM = loggedUserPM;
                this.UserName = SessionInfo.LoggedUserPM.EnglishName;
                this.SetFirstUserLetters(SessionInfo.LoggedUserPM.EnglishName);
            }else{
                this.GetLoggedContact();
            }
        });
    }

    private GetLoggedContact()
    {
        this.brandingService.GetLoggedContact().subscribe((loggedContact: any) =>
        {
            if (loggedContact) {
                SessionInfo.LoggedContact = loggedContact;
                this.UserName = SessionInfo.LoggedContact.EnglishName;
                this.SetFirstUserLetters(SessionInfo.LoggedContact.EnglishName);

            }
        });
    }

    private SetFirstUserLetters(userName: string)
    {
        if (userName) {
            var splitted = userName.split(" ");
            if (splitted.length == 1)
                this.UserNameFirstLetters = splitted[0][0];
            else if (splitted.length >= 2)
                this.UserNameFirstLetters = splitted[0][0] + splitted[1][0];
            else if (splitted.length == 0)
                this.UserNameFirstLetters = "Aa";

        }
    }

    private GetCompanyLoginsFromCache()
    {
        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        console.log("[LoggedUserCompanyLogins]", SessionInfo.LoggedUserCompanyLogins);
        this.GetInvitedCustomers();
    }

    private GetInvitedCustomers()
    {
        this.ConnectedCustomers = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => d.CardId);
        console.log("[Invited Customers]", this.ConnectedCustomers);
    }



    private Authenticate()
    {
        let token = sessionStorage.getItem("Token");
        if (!token)
            this.router.navigate(["cargo-tracking", "login"]);
    }

    isNavOpened = false;

    openNav()
    {
        this.isNavOpened = !this.isNavOpened;
    }


    SignOutClicked()
    {
        this.tenant = +sessionStorage.getItem("LoggedUserTenant");
        sessionStorage.clear();
        if (this.tenant)

            this.router.navigate(["cargo-tracking/login"]);//,{ queryParams: {tenant: this.tenant}}
        else
            this.router.navigate(["cargo-tracking/login"]);
    }


    public get InvertedLogoURL()
    {
        return CargoTrackingBrandingData.InvertedLogoURL;
    }

    public RedirectTo401Page()
    {
        this.router.navigate(['Error401']);
    }


    private GetBrandingData()
    {
        if (this.tenant)
            this.IsBrandingDataLoaded = true;


        this.brandingService.GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(this.baseURL))
        .subscribe((response: ServiceResponse) =>
        {
            if (response.Result) {
                ServiceHelper.SetCargoTrackingDate(response.Result, this.baseURL);

                this.IsBrandingDataLoaded = true;
            }
            else {
                this.RedirectTo401Page();
            }

        });
    }
    ngOnInit(): void {
        this.currentRoute = this.router.url;
        this.SubscribeRoutingEvents();
    }
    private SubscribeRoutingEvents() {
        this.router.events.pipe(
            filter((e: any): e is NavigationEnd => e instanceof NavigationEnd)
         ).subscribe((e: NavigationEnd) => {
             this.currentRoute = e.url;
         });
    }
    ngAfterViewInit()
    {
    }
    get ComapnyLogo()
    {
        return CargoTrackingBrandingData.ComapnylogoURL;
    }
    get BrowserIcon()
    {
        return CargoTrackingBrandingData.BrowserIconURL;
    }
    get BackGroundImg()
    {
        return CargoTrackingBrandingData.BackgroundURL;
    }
    get ShipmentHeaderImage(){
        return CargoTrackingBrandingData.ShipmentHeaderURL;
    }

    BackLinkClicked() {
        this.router.navigate(['cargo-tracking', 'shipments']);
    }

}
