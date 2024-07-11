import {  Component } from "@angular/core";
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';

@Component({    
    templateUrl: './ShipmentDataTabComponent.html',
})

export class ShipmentDataTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    // this.CustomerId = this.EntityPM.CustomerId;
                    // this.ToCountryId = this.EntityPM.ToCountryId;
                    // this.BuildProductItems();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    // this.CustomerId = this.EntityPM.CustomerId;
                    // this.ToCountryId = this.EntityPM.ToCountryId;
                    // this.BuildProductItems();
                }
            });
        }
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    //#region Properties
    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
        }
    }

    public get DepartmentId() { return this.EntityPM.DepartmentId; }
    public set DepartmentId(newValue: string) {
        if (this.EntityPM.DepartmentId != newValue) {
            this.EntityPM.DepartmentId = newValue;
        }
    }

    // public get ShipmentType() { return this.EntityPM.ShipmentType; }
    // public set ShipmentType(newValue: string) {
    //     if (this.EntityPM.ShipmentType != newValue) {
    //         this.EntityPM.ShipmentType = newValue;
    //     }
    // }

    // public get House() { return this.EntityPM.House; }
    // public set House(newValue: string) {
    //     if (this.EntityPM.House != newValue) {
    //         this.EntityPM.House = newValue;
    //     }
    // }
    public get IskaNumber() { return this.EntityPM.IskaNumber; }
    public set IskaNumber(newValue: string) {
        if (this.EntityPM.IskaNumber != newValue) {
            this.EntityPM.IskaNumber = newValue;
        }
    }
    public get HAWBDate() { return this.EntityPM.HAWBDate; }
    public set HAWBDate(newValue: Date) {
        if (this.EntityPM.HAWBDate != newValue) {
            this.EntityPM.HAWBDate = newValue;
        }
    }
    public get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    public set MainCarriageFromPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFromPortId != newValue) {
            this.EntityPM.MainCarriageFromPortId = newValue;
        }
    }
    public get IncotermId() { return this.EntityPM.IncotermId; }
    public set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;
        }
    }
    public get ReferantUserId() { return this.EntityPM.ReferantUserId; }
    public set ReferantUserId(newValue: string) {
        if (this.EntityPM.ReferantUserId != newValue) {
            this.EntityPM.ReferantUserId = newValue;
        }
    }
    public get GrossWeight() { return this.EntityPM.GrossWeight; }
    public set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = newValue;
        }
    }
    public get ChargeableWeight() { return this.EntityPM.ChargeableWeight; }
    public set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = newValue;
        }
    }
    public get NumberOfPackages() { return this.EntityPM.NumberOfPackages; }
    public set NumberOfPackages(newValue: number) {
        if (this.EntityPM.NumberOfPackages != newValue) {
            this.EntityPM.NumberOfPackages = newValue;
        }
    }
    public get Volume() { return this.EntityPM.Volume; }
    public set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = newValue;
        }
    }
    public get SalesmanUserId() { return this.EntityPM.SalesmanUserId; }
    public set SalesmanUserId(newValue: string) {
        if (this.EntityPM.SalesmanUserId != newValue) {
            this.EntityPM.SalesmanUserId = newValue;
        }
    }
    public get TransportModeId() { return this.EntityPM.TransportModeId; }
    public set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
        }
    }

    // todo:
    public get CarrierCode() { return this.EntityPM.CarrierCode; }
    public set CarrierCode(newValue: string) {
        if (this.EntityPM.CarrierCode != newValue) {
            this.EntityPM.CarrierCode = newValue;
        }
    }
    public get MawbDate() { return this.EntityPM.MawbDate; }
    public set MawbDate(newValue: Date) {
        if (this.EntityPM.MawbDate != newValue) {
            this.EntityPM.MawbDate = newValue;
        }
    }

    public get EstimatedArrivalDate() { return this.EntityPM.EstimatedArrivalDate; }
    public set EstimatedArrivalDate(newValue: Date) {
        if (this.EntityPM.EstimatedArrivalDate != newValue) {
            this.EntityPM.EstimatedArrivalDate = newValue;
        }
    }
    
    public get ArrivalDate() { return this.EntityPM.ArrivalDate; }
    public set ArrivalDate(newValue: Date) {
        if (this.EntityPM.ArrivalDate != newValue) {
            this.EntityPM.ArrivalDate = newValue;
        }
    }

    public get PackageTypeCode() { return this.EntityPM.PackageTypeCode; }
    public set PackageTypeCode(newValue: string) {
        if (this.EntityPM.PackageTypeCode != newValue) {
            this.EntityPM.PackageTypeCode = newValue;
        }
    }

    public get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    public set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    public get Vessel() { return this.EntityPM.Vessel; }
    public set Vessel(newValue: string) {
        if (this.EntityPM.Vessel != newValue) {
            this.EntityPM.Vessel = newValue;
        }
    }

    public get HawbDate() { return this.EntityPM.HAWBDate; }
    public set HawbDate(newValue: Date) {
        if (this.EntityPM.HAWBDate != newValue) {
            this.EntityPM.HAWBDate = newValue;
        }
    }

    public get FlightVoyageNumber() { return this.EntityPM.FlightVoyageNumber; }
    public set FlightVoyageNumber(newValue: string) {
        if (this.EntityPM.FlightVoyageNumber != newValue) {
            this.EntityPM.FlightVoyageNumber = newValue;
        }
    }
    
    public get Commodity() { return this.EntityPM.Commodity; }
    public set Commodity(newValue: string) {
        if (this.EntityPM.Commodity != newValue) {
            this.EntityPM.Commodity = newValue;
        }
    }
}
