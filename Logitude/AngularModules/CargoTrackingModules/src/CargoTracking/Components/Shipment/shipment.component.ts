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
    ShipmentId: string = "";
    Shipment;
    innerWidth: number;
    isMobileView: boolean = false;
    isTabletView: boolean = false;

    constructor(private route: ActivatedRoute,private router: Router, private location: Location) {
        this.GetIdFromURI();

        this.GetShipmentFromDB();

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

    private GetShipmentFromDB()
    {
        this.Shipment = db.Shipments.find(d => d.Id == this.ShipmentId);
    }

    private GetIdFromURI()
    {
        let _id = this.route.snapshot.paramMap.get('shipmentId');
        this.ShipmentId = _id;
        return _id;
    }





    goBack(): void {
        // this.location.back();
        this.router.navigate(['search']);
    }
    GetModeIcon()
    {
        var iconPath = "";
        switch (this.Shipment.Mode) {
            case 'Air':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'Ocean':
                iconPath = "./assets/images/misc/ship.svg";
            break;

            default:
                case 'Land':
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
}
