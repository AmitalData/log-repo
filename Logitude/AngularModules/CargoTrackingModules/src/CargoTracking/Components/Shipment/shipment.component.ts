import { CargoTrackingShipmentList } from './../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from './../../Services/Others/CargoTrackingSearchService';
import { Component, AfterViewInit, HostListener, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { db } from '../../../app/mem.data';
import { IfStmt } from '@angular/compiler';

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
    _Tenant:number;
    constructor(private route: ActivatedRoute,
        private router: Router,
         private location: Location,
         private searchService: CargoTrackingSearchService) {
        this.GetIdFromURI();

        this.LoadShipment();

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
        if(this._Tenant==null){
            this._Tenant = Number(this.route.snapshot.parent.paramMap.get('Tenant'));
        }
        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }





    goBack(): void {
        // this.location.back();
        this.router.navigate([this._Tenant,'search']);
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

    SelectedTab: string = 'steps';
    TabToggleClicked(tabName: string)
    {
        this.SelectedTab = tabName;
    }

    LoadShipment(){
        this.isLoading = true;
        this.searchService.getShipment(this.SecurityKey, this._Tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getShipment]", result);
            this.ShipmentWithMilestones = result;
            if(this.ShipmentWithMilestones){
                this.Shipment = result.ShipmentList;
                this.SetMilestonesFields(result);
            }
            

        });
    }

    SetMilestonesFields(result:CargoTrackingShipmentWithMilestones){

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