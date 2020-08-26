import { CargoTrackingShipmentList } from './../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from './../../Services/Others/CargoTrackingSearchService';
import { Component, AfterViewInit, HostListener, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { db } from '../../../app/mem.data';

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
        
        var tenant = this.route.snapshot.paramMap.get('SecurityKey');
        if(tenant!=null && tenant!=""){
            this._Tenant = Number(this.route.snapshot.paramMap.get('Tenant'));
        }
        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }





    goBack(): void {
        // this.location.back();
        this.router.navigate(['search']);
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
            this.Shipment = result;

        });
    }
}
