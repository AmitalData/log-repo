import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from '../../Services/Others/CargoTrackingSearchService';
import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';


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
    searchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    _Tenant:number;
    UserName:string;

    ConnectedCustomers: string[] = [];

    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService)
    {
        document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
        
        this.InitComponent();

    }

    private InitComponent()
    {
        this.GetTenantFromURL();
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
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this._Tenant)
            .map(d => d.CardId);
            console.log("[Invited Customers]",this.ConnectedCustomers);            
    }

    private GetTenantFromURL()
    {
        var tenant = Number(this.route.snapshot.paramMap.get('Tenant'));
        if (tenant) 
            this._Tenant = tenant;
        else
            console.error("Tenant not provided in URL");            
    }

    private Authenticate()
    {
        var loggedEmail = sessionStorage.getItem("LoggedUserEmail");
        if (!loggedEmail) 
            this.router.navigate([this._Tenant, "login"]);        
    }

    isNavOpened = false;

    openNav(){
        this.isNavOpened = !this.isNavOpened;
    }

    SignOutClicked(){
        this._Tenant = +sessionStorage.getItem("LoggedUserTenant");
        sessionStorage.clear();
        if(!this._Tenant) this._Tenant = 0;
        this.router.navigate([this._Tenant, 'login'])
    }

    ngAfterViewInit()
    { 
    }

  




    private _SearchText: string;
    public get SearchText(): string
    {
        return this._SearchText;
    }
    public set SearchText(v: string)
    {
        this._SearchText = v;
        if (!this.SearchText)
            this.Search();
    }

    Clear()
    {
        this.SearchText = '';
        this.Search();
    }
    Search()
    {
        if(this._Tenant){
            this.router.navigate([this._Tenant,'search', this.SearchText]);
            this.LoadShipments();
        }
            
    }
    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;

        this.router.navigate([this._Tenant,'shipment', SecurityKey]);

    }
    LoadShipments()
    {

        this.noResult = false;
        var searchText = this._SearchText.trim().toLowerCase();
        if (searchText) {
            this.isLoading = true;
            this.searchService.getShipments(searchText, this._Tenant).subscribe((result: any) =>
            {
                this.isLoading = false;
                console.log("[getShipments]", result);
                this.Shipments = result;
                this.noResult = this.Shipments.length == 0 && !!this.SearchText;

            });
        }else{
            this.Shipments = [];
        }

    }
    references: string[];
    SplitReference(reference: string){
      this.references =reference!= null?  reference.split(','): null;
    
    }

    GetModeIcon(mode: string)
    {
        var iconPath = "";
        switch (mode) {
            case 'A':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'O':
                iconPath = "./assets/images/misc/ship.svg";
                break;

            case 'I':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

            default:
                break;
        }

        return iconPath;
    }
}
