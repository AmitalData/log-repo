import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from '../../Services/Others/CargoTrackingSearchService';
import { Component, ViewChild, ElementRef, AfterViewInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';



@Component({
    selector: 'UserDashboard',
    templateUrl: './UserDashboardComponent.html',
    styleUrls: ['./UserDashboardComponent.css']
})
export class UserDashboardComponent implements AfterViewInit
{

    @ViewChild('input') input: ElementRef;
    isLoading: boolean = false;
    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    Shipments: CargoTrackingShipmentList[] = [];
    UserName:string;
    ConnectedCustomers: string[] = [];
    IsBrandingDataLoaded:boolean = false;
    get tenant(){
         return CargoTrackingBrandingData.Tenant;
    }
    set tenant(val:number){
          CargoTrackingBrandingData.Tenant = val;
    }
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,private router: Router,@Inject('BASE_URL') baseUrl: string )
    {
         
         document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
         this.GetcargoTrackingData(baseUrl);
         this.InitComponent();

    }
 
    private InitComponent()
    {

        this.Authenticate();
        this.GetLoggedUserNameFromLoggedEmail();
    }

    private GetLoggedUserNameFromLoggedEmail()
    {
        var loggedEmail = sessionStorage.getItem("LoggedUserEmail");
        this.UserName = loggedEmail?.split('@')[0] || 'Saitama Con';
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
            console.log("[Invited Customers]",this.ConnectedCustomers);            
    }

  

    private Authenticate()
    {
        var loggedEmail = sessionStorage.getItem("LoggedUserEmail");
        if (!loggedEmail) 
            this.router.navigate(["Cargo-Tracking", "login"]);        
    }

    isNavOpened = false;

    openNav(){
        this.isNavOpened = !this.isNavOpened;
    }


    SignOutClicked(){
        this.tenant = +sessionStorage.getItem("LoggedUserTenant");
        sessionStorage.clear();
        if(this.tenant)
            this.router.navigate(["Cargo-Tracking/login"]);//,{ queryParams: {tenant: this.tenant}}
        else
            this.router.navigate(["Cargo-Tracking/login"]);
    }


    public GoToError401(){
        this.router.navigate(['Error401']);
    }

 
    private GetcargoTrackingData(baseUrl:string)
    {   if(this.tenant) 
        this.IsBrandingDataLoaded = true;
        this.cargoTrackingDataExtendedService.GetCargoTrackingBrandingDataForPrivateSite(ServiceHelper.GetcargoTrackingDataRequest(baseUrl)).subscribe((response: ServiceResponse) =>
        { if(response.Result){
            ServiceHelper.SetCargoTrackingDate(response.Result,baseUrl);
            this.IsBrandingDataLoaded = true;
        }
        else{
            this.GoToError401();
        }
          
        });
    }
    
    ngAfterViewInit()
    { 

    }
    get ComapnyLogo(){
        return CargoTrackingBrandingData.ComapnylogoURL;
    } 
    get BrowserIcon(){
        return CargoTrackingBrandingData.BrowserIconURL;
    } 
    get BackGroundImg(){
        return CargoTrackingBrandingData.BackgroundURL;
    } 
  
    
}
