import { CargoTrackingShipmentList } from './../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from './../../Services/Others/CargoTrackingSearchService';
import { Component, AfterViewInit, HostListener, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd, NavigationStart, RoutesRecognized, Event } from '@angular/router';
import { Location } from '@angular/common';
import { db } from '../../../app/mem.data';
import { IfStmt } from '@angular/compiler';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { filter } from 'rxjs/operators';
import { AppHelper } from 'src/CargoTracking/Utilities/AppHelper';

@Component({
    selector: 'shipment',
    styleUrls: ['./shipment.component.css'],
    templateUrl: './shipment.component.html'
})
export class ShipmentComponent implements OnInit
{
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentList = null;
    innerWidth: number;
    isLoading: boolean = false;
    isMobileView: boolean = false;
    isTabletView: boolean = false;
    tenant:number;
    previousUrl: string;

    constructor(private route: ActivatedRoute,
        private router: Router,
         private location: Location,
         private searchService: CargoTrackingSearchService) {
        this.GetIdFromURI();
        this.mainColor = CargoTrackingBrandingData.MainColor;
        this.LoadShipment();
        console.log("[referrer]",document.referrer);
        
        this.listenToRouterEvents();
        
        
        

    }
    private listenToRouterEvents()
    {
        this.router.events.subscribe((event: Event) =>
        {
            if (event instanceof RoutesRecognized) {
                // Show loading indicator
                // var url = window.location.pathname;
                

            }

            if (event instanceof NavigationStart) {
                var url = window.location.pathname;
            }

            if (event instanceof NavigationEnd) {
                // Hide loading indicator
            }

        });
    }
    @HostListener('window:resize', ['$event'])
    onResize(event) {
      this.setViews();
    }
    ngOnInit(): void
    {
        this.setViews();
    }

    private setViews()
    {
        this.innerWidth = window.innerWidth;
        this.isMobileView = this.innerWidth <= 479;
        this.isTabletView = this.innerWidth <= 1000;
    }

    // private GetShipmentFromDB()
    // {
    //     this.Shipment = db.Shipments.find(d => d.Id == this.SecurityKey);
    // }

    private GetIdFromURI()
    {      
        
        // var tenant = this.route.snapshot.paramMap.get('SecurityKey');
        if(this.tenant==null){
            this.tenant = Number(this.route.snapshot.parent.paramMap.get('Tenant'));
        }
        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }





    goBack(): void {
        AppHelper.AppBack(this.router,this.location,this.tenant);
        
    }
    GetModeIcon()
    {
        var iconPath = "";
        switch (this.Shipment.TransportModeId) {
            case 'A':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'O':
                iconPath = "./assets/images/misc/ship.svg";
            break;

            default:
                case 'L':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

        }

        return iconPath;
    }
    public mainColor: string;
    public transform: string;
    GetModesvgPath() {
   
        var iconPath = "";
        switch (this.Shipment.TransportModeId) {
            case 'A':
                iconPath = "M28.2010781,26 L44.9721135,32.4312677 C46.8367468,31.1081412 48.7257202,29.7863693 50.5260794,28.5557714 C51.8003996,27.6847188 57.0176923,27.3267788 57.7639616,28.3543079 L57.7639616,28.3543079 L57.8454343,28.4667753 C58.5922106,29.4947934 56.4585397,34.1000925 55.1737396,34.9563124 C53.4750773,36.0893977 51.6720921,37.2654064 49.8681811,38.4256513 L50.4013143,55.8435582 L47.2072029,58 L45.7311918,55.9677976 L46.4843162,55.4596112 L41.7760232,43.5398952 C37.7106852,46.0706551 34.750133,47.8626901 34.750133,47.8626901 L34.750133,47.8626901 L33.3792963,52.8533126 L29.7502134,55.3041258 L28.875312,48.0677395 L28.7522579,47.8982233 L22,44.6307983 L25.629421,42.179985 L30.9638518,42.6479476 C30.9638518,42.6479476 33.557413,40.7024661 37.2302404,38.0156431 L27.2355389,29.6802767 L26.4826023,30.1888253 L25.0069668,28.1571663 L28.2010781,26 Z";
                break;

            case 'O':
                iconPath = "M64.0004222,40.059772 L55.4342944,51 L20.0331393,51 L17.0004222,40.059772 L64.0004222,40.059772 Z M30.3716056,37.1653425 L30.3716056,39.1572031 L20.6380712,39.1572031 L20.6380712,37.1653425 L30.3716056,37.1653425 Z M57.1715124,35.1574036 L57.1715124,39.1411248 L51.6342935,39.1411248 L51.6342935,35.1574036 L57.1715124,35.1574036 Z M51.0698938,35.1574036 L51.0698938,39.1411248 L45.532675,39.1411248 L45.532675,35.1574036 L51.0698938,35.1574036 Z M41.4126297,36.4234621 L41.4126297,39.0792762 L31.6790953,39.0792762 L31.6790953,36.4234621 L41.4126297,36.4234621 Z M25.1815979,34.4120812 L25.1815979,36.7067047 L20.8232989,36.7067047 L20.8232989,34.4120812 L25.1815979,34.4120812 Z M30.1863779,34.4120812 L30.1863779,36.7067047 L25.8280789,36.7067047 L25.8280789,34.4120812 L30.1863779,34.4120812 Z M54.0862725,31 L54.0862725,34.6441488 L48.5490537,34.6441488 L48.5490537,31 L54.0862725,31 Z";
                break;

            default:
            case 'L':
                iconPath = "M29.6731686,46.9283885 C31.3325599,46.9283885 32.677691,48.2875099 32.677691,49.9642285 C32.677691,51.6408786 31.3325599,53 29.6731686,53 C28.0137095,53 26.6685784,51.6408786 26.6685784,49.9642285 C26.6685784,48.2875099 28.0137095,46.9283885 29.6731686,46.9283885 Z M52.7712947,46.9283885 C54.4306859,46.9283885 55.775885,48.2875099 55.775885,49.9642285 C55.775885,51.6408786 54.4306859,53 52.7712947,53 C51.1118356,53 49.7667045,51.6408786 49.7667045,49.9642285 C49.7667045,48.2875099 51.1118356,46.9283885 52.7712947,46.9283885 Z M59.5103151,29.000045 C60.3331132,28.9936856 61,29.6623235 61,30.4935267 L61,30.4935267 L61,48.094433 C61,48.9256362 60.3362073,49.664958 59.5173334,49.7455545 L59.5173334,49.7455545 L56.7117602,50.0216564 C56.7120621,50.0025176 56.7125149,49.9833788 56.7125149,49.96424 C56.7125149,47.7649559 54.9479854,45.9819979 52.7712645,45.9819979 C50.5946191,45.9819979 48.8300897,47.7649559 48.8300897,49.96424 C48.8300897,50.093179 48.8364287,50.2208219 48.8482013,50.3467109 L48.8482013,50.3467109 L33.596213,50.3467109 C33.608061,50.2208219 33.6144001,50.093179 33.6144001,49.96424 C33.6144001,47.7649559 31.8498707,45.9819979 29.6731498,45.9819979 C27.4965043,45.9819979 25.7319749,47.7649559 25.7319749,49.96424 C25.7319749,50.0006876 25.7325031,50.0371352 25.7334842,50.0734302 L25.7334842,50.0734302 L23.395494,49.6050254 C22.5884681,49.443375 21.9644456,48.639088 22.0015746,47.8087235 L22.001651,47.7970977 C22.0037397,47.5436283 22.0604663,43.2216537 23.5033339,42.3380778 C24.3290751,41.8324629 25.2657503,41.5066458 25.9354293,41.3168591 C26.3464133,41.2004251 26.7834328,40.7948503 26.9041773,40.3811168 L26.9041773,40.3811168 L28.6373886,34.4390163 C28.9325334,33.4271763 29.5955715,32.453919 30.5043245,31.6986609 C31.4130775,30.9434028 32.486194,30.4737017 33.5258793,30.3764064 L33.5258793,30.3764064 L35.9259775,30.1515447 C36.1341862,29.5682307 36.6859884,29.1476347 37.3324996,29.1426022 L37.3324996,29.1426022 Z M35.4019465,31.7518073 L33.6669994,31.9144489 C32.140563,32.057418 30.5370764,33.3899662 30.1036038,34.8758536 L30.1036038,34.8758536 L28.4251048,40.6306075 C28.3768824,40.7960704 28.4752137,40.9146394 28.6448597,40.8954243 L28.6448597,40.8954243 L34.2584964,40.2599553 C34.4990043,40.232734 34.7174763,40.013896 34.7465304,39.7711154 L34.7465304,39.7711154 L35.672565,32.0330942 C35.6931671,31.8618363 35.5718943,31.7359472 35.4019465,31.7518073 L35.4019465,31.7518073 Z";
                this.transform = "translate(41.500000, 41.000000) scale(-1, 1) translate(-41.500000, -41.000000)";
                break;

        }

        return iconPath;
    }

    Delivered: boolean = false;
    DileveredIconColor: string;
    //GetDeliveredIconColor() {
    //    var iconPath = "";

    //    if (this.Shipment.CurrentMilestoneCode == "11") {

    //        this.DileveredIconColor = this.mainColor;
          
    //    }
    //    else {
    //        this.DileveredIconColor ="#B5B5B5";
    //    }
          
    //    return iconPath;
    //}

    SelectedTab: string = 'steps';
    TabToggleClicked(tabName: string)
    {
        this.SelectedTab = tabName;
    }

    LoadShipment(){
        this.isLoading = true;
        this.searchService.getShipment(this.SecurityKey, this.tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getShipment]", result);
            this.ShipmentWithMilestones = result;
            if(this.ShipmentWithMilestones){
                this.Shipment = result.ShipmentList;
                if (this.Shipment.CurrentMilestoneCode == "11") {
                    this.Delivered = true;
                    this.DileveredIconColor = this.mainColor;
                }
                else {
                    this.Delivered = false;
                    this.DileveredIconColor = "#B5B5B5";
                }
                this.SetMilestonesFields(result);
            }
            

        });
    }
    public Date: Date;
    SetMilestonesFields(result: CargoTrackingShipmentWithMilestones) {
     
        this.AllMilestoneFields = result.Milestones;
        if(this.AllMilestoneFields){
            this.AllMilestoneFields.forEach(S=>{
                
                    if(S.IsEstimation){
                         this.FuturesMilestoneFields.push(S);
                    }
                    else if(!S.IsCurrent){
                        this.CompletedMilestoneFields.push(S);
                       
                    }
                    else{
                        this.CurrentMilestoneField = S;
                    }
            });
        }

        if (this.Shipment.CurrentMilestoneCode == "11") {
            this.Date = this.CompletedMilestoneFields[0].Date;
        }
        else {
            this.Date = this.CurrentMilestoneField.Date;

        }
    }

    public ShipmentWithMilestones:CargoTrackingShipmentWithMilestones;
    public AllMilestoneFields:Milestone[];
    public CompletedMilestoneFields:Milestone[]=[];
    public FuturesMilestoneFields:Milestone[]=[];
    public CurrentMilestoneField:Milestone = new Milestone();
}



export class Milestone {
   
    public	Code: string;
    public	Name: string;
	public  Notes: string;
	public  Date: Date;
	public  EstimationDate: Date;
    public  Done: boolean;
    public  IsEstimation: boolean;
    public  IsCurrent: boolean;
}


export class CargoTrackingShipmentWithMilestones
{
    public Milestones: Milestone[]  ;
    public ShipmentList:CargoTrackingShipmentList   ;

}
