import {Component} from 'angular2/core';
import {EntityArgs} from '../../infrastructure/data-contracts/entity-args';
import {ShipmentPM} from '../EntityPMs/ShipmentPM';

@Component({
    templateUrl: "app/shipment/short-title/shipment-short-title.html",
    //`
    //    <h1>Shipment Short Title</h1><input [(ngModel)]="entityPM.ShipperName" type="text" />
    //`
    //,
})

export class ShipmentShortTitleComponent {

    public EntityPM: ShipmentPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.BuildData();
        }
    }

    public Background: string;
    public DirectionImageSRC: string;
    public TransportModeImageSRC: string;
    public PartnerName: string;
    public RankCode: string;
    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    public IsRankVisible: boolean = false;
    public IsCancelled: boolean = false;

    BuildData() {

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.Background = "rgba(35, 172, 214, 0.25)";
            this.PartnerName = this.EntityPM.AgentName;
        }

        else {
            this.Background = "rgba(242, 242, 242, 1)";
            this.PartnerName = this.EntityPM.CustomerName;
            this.RankName = this.EntityPM.CustomerRankName;

            if (this.RankName != null) {
                switch (this.RankName.toLowerCase()) {
                    case "silver": {
                        this.RankCode = "1";
                        this.RankSource1 = "images/Icons/StarOrange.png";
                        this.RankSource2 = "images/Icons/StarGray.png";
                        this.RankSource3 = "images/Icons/StarGray.png";
                        break;
                    }

                    case "gold": {
                        this.RankCode = "2";
                        this.RankSource1 = "images/Icons/StarOrange.png";
                        this.RankSource2 = "images/Icons/StarOrange.png";
                        this.RankSource3 = "images/Icons/StarGray.png";
                        break;
                    }

                    case "platinum": {
                        this.RankCode = "3";
                        this.RankSource1 = "images/Icons/StarOrange.png";
                        this.RankSource2 = "images/Icons/StarOrange.png";
                        this.RankSource3 = "images/Icons/StarOrange.png";
                        break;
                    }
                }
            }

            this.IsRankVisible = true;
        }

        this.DirectionImageSRC = "images/Directions/" + this.EntityPM.DirectionId + ".png";
        this.TransportModeImageSRC = "images/Icons/" + this.EntityPM.TransportModeId + ".png";

        this.IsCancelled = this.EntityPM.IsCancelled;
    }

    //}

    //EntityPM: ShipmentPM;
    //public setParameters(entityPM: ShipmentPM) {
    //    //console.log("short title entity id: ", entityPM.Id);
    //    this.EntityPM = entityPM;
    //    //this._shipmentsService.getSingleEntityPM(this.entityPM.Id, 1).subscribe((res: ShipmentPM) => { this.entityPM = res; console.log(res as ShipmentPM); });
    //}

}