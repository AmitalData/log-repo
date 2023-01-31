import { Component, OnInit, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from 'Shipment/EntityPMs/ContainerPM';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    templateUrl: './RoutingsTabComponent.html',
})

export class RoutingsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ContainerPM;
    public ObjectTableName: string = "Container";    
    private CurrentSession = SessionLocator.SelectedSession;
    private ItemsSource: RoutingItem[];
    public SelectedLegTitle: string;
    public SelectedLegCode: string;
    public DataContext: ContainerPM;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.DataContext = entityArgs.EntityPM;

        this.BuildItemsSource();
        this.SetUIProperties();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    
                }
            });

        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    ngOnInit() {

    }

    private SetUIProperties() {

    }

    public ItemsSource1: RoutingItem[];
    public ItemsSource2: RoutingItem[];
    public ItemsSource3: RoutingItem[];
    public ItemsSource4: RoutingItem[];
    private BuildItemsSource() {
        this.ItemsSource = [];
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource3 = [];
        this.ItemsSource4 = [];

        this.AddRoutingItem("PICK");
        this.AddRoutingItemLine("PREC");

        this.AddRoutingItem("PREC");
        this.AddRoutingItemLine("POL");

        this.AddRoutingItem("POL");
        this.AddRoutingItemLine("TS1");

        this.AddRoutingItem("TSS");
        this.AddRoutingItemLine("POD");

        this.AddRoutingItem("TS1", true, true);
        this.AddRoutingItemLine("TS2", true, true);

        this.AddRoutingItem("TS2", true, true);
        this.AddRoutingItemLine("TS3", true, true);

        this.AddRoutingItem("TS3", true, true);
        this.AddRoutingItemLine("POD", true, true);

        this.AddRoutingItem("POD");
        this.AddRoutingItemLine("ONC");

        this.AddRoutingItem("ONC");
        this.AddRoutingItemLine("DELV");

        this.AddRoutingItem("DELV");
        this.AddRoutingItemLine("EMRT");

        this.AddRoutingItem("EMRT");
    }

    private AddRoutingItem(code: string, isHidden: boolean = false, isTransshipment: boolean = false) {
        var item: RoutingItem = new RoutingItem(code, this.EntityPM);
        item.IsHidden = isHidden;
        item.IsTransshipment = isTransshipment;
        this.ItemsSource.push(item);
    }   
    private AddRoutingItemLine(nextLegCode: string, isHidden: boolean = false, isTransshipment: boolean = false) {
        var item: RoutingItem = new RoutingItem("Line", this.EntityPM);
        item.IsLine = true;
        item.IsHidden = isHidden;
        item.IsTransshipment = isTransshipment;
        item.NextLegCode = nextLegCode;
        item.IsContinuousLine = this.CheckNextLegDates(nextLegCode);
        item.IsDashedLine = !item.IsContinuousLine;
        this.ItemsSource.push(item);
    }

    private CheckNextLegDates(nextLegCode: string): boolean {
        switch (nextLegCode) {
            case "PREC": {
                return this.CheckDates_PreCarriage();
            }

            case "POL": {
                return this.CheckDates_POL();
            }

            case "TS1": {
                return this.CheckDates_Transshipment1();
            }

            case "POD": {
                return this.CheckDates_POD();
            }

            case "ONC": {
                return this.CheckDates_OnCarriage();
            }

            case "DELV": {
                return this.CheckDates_Delivery();
            }

            case "EMRT": {
                return this.CheckDates_EmptyReturn();
            }

            default: {
                return false;
            }
        }
    }
    private CheckDates_PreCarriage(): boolean {
        if (this.EntityPM.PreCarriageATD != null)
            return true;

        else if (this.EntityPM.PreCarriageETD != null)
            return true;

        else if (this.EntityPM.PreCarriageGateIn != null)
            return true;

        else
            return false;
    }
    private CheckDates_POL(): boolean {
        if (this.EntityPM.ActualPOLVesselDeparture != null)
            return true;

        else if (this.EntityPM.EstimatedPOLVesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualPOLLoaded != null)
            return true;

        else if (this.EntityPM.EstimatedPOLLoaded != null)
            return true;

        else if (this.EntityPM.GateIn != null)
            return true;
        else
            return false;
    }
    private CheckDates_Transshipment1(): boolean {
        if (this.EntityPM.ActualTrans1VesselDeparture != null)
            return true;

        else if (this.EntityPM.EstimatedTrans1VesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualTransshipment1VesselArrival != null)
            return true;

        else if (this.EntityPM.EstimatedTrans1VesselArrival != null)
            return true;

        else if (this.EntityPM.ActualTransshipment1Loaded != null)
            return true;

        else if (this.EntityPM.EstimatedTransshipment1Loaded != null)
            return true;
        else
            return false;
    }
    private CheckDates_POD(): boolean {
        if (this.EntityPM.GateOut != null)
            return true;

        else if (this.EntityPM.ActualPODDischarge != null)
            return true;

        else if (this.EntityPM.EstimatedPODDischarge != null)
            return true;

        else if (this.EntityPM.ActualPODVesselArrival != null)
            return true;

        else if (this.EntityPM.EstimatedPODVesselArrival != null)
            return true;

        else
            return false;
    }
    private CheckDates_OnCarriage(): boolean {
        if (this.EntityPM.OnCarriageGateOut != null)
            return true;

        else if (this.EntityPM.OnCarriageATD != null)
            return true;

        else if (this.EntityPM.OnCarriageETD != null)
            return true;

        else if (this.EntityPM.OnCarriageATA != null)
            return true;

        else if (this.EntityPM.OnCarriageETA != null)
            return true;

        else
            return false;
    }
    private CheckDates_Delivery(): boolean {
        if (this.EntityPM.ShipmentDeliveryATA != null)
            return true;

        else if (this.EntityPM.ShipmentDeliveryETA != null)
            return true;

        else if (this.EntityPM.ShipmentDeliveryATD != null)
            return true;

        else if (this.EntityPM.ShipmentDeliveryETD != null)
            return true;

        else
            return false;
    }
    private CheckDates_EmptyReturn(): boolean {
        if (this.EntityPM.ActualEmptyReturn != null)
            return true;

        else if (this.EntityPM.EstimatedEmptyReturn != null)
            return true;

        else
            return false;
    }

    RoutingItemClicked(code: string) {
        if (code == "TSS") {
            this.SelectedLegCode = "TS1";
            this.SelectedLegTitle = this.ItemsSource.filter(d => d.Code == "TS1")[0]?.Name;

            this.ItemsSource.forEach(item => {
                if (item.Code == "TSS")
                    item.IsHidden = true;

                else if (item.Code == "Line" && !item.IsTransshipment && item.NextLegCode == "POD")
                    item.IsHidden = true;

                else if (item.IsTransshipment)
                    item.IsHidden = false;

                else if (item.Code == "Line" && item.IsTransshipment && item.NextLegCode == "POD")
                    item.IsHidden = false;
            });
        }

        else {
            this.SelectedLegCode = code;
            this.SelectedLegTitle = this.ItemsSource.filter(d => d.Code == code)[0]?.Name;
        }
    }
}

export class RoutingItem {
    private Container: ContainerPM;
    constructor(code: string, container: ContainerPM) {
        this.Code = code;
        this.Container = container;
        this.Calculate();
    }

    public IsTransshipment: boolean;
    public Code: string;
    public NextLegCode: string;
    public Name: string;
    public Location: string;
    public IsLine: boolean = false;
    public IsHidden: boolean = false;
    public IsDashedLine: boolean = false;
    public IsContinuousLine: boolean = false;

    private Calculate() {
        var name: string = null;
        var location: string = null;

        switch (this.Code) {
            case "PICK": {
                name = "Empty Pickup";
                location = this.Container.EmptyPickupLocationName;
                break;
            }

            case "PREC": {
                name = "Pre Carriage";
                location = this.Container.PreCarriageLocationName;
                break;
            }

            case "POL": {
                name = "POL";
                location = this.Container.POLLocationName;
                break;
            }

            case "TSS": {
                name = "Transshipments";
                location = this.GetTrnasshipmentLocation();
                break;
            }

            case "TS1": {
                name = "Transshipment 1";
                location = this.Container.Transshipment1LocationName;
                break;
            }

            case "TS2": {
                name = "Transshipment 2";
                location = this.Container.Transshipment2LocationName;
                break;
            }

            case "TS3": {
                name = "Transshipment 3";
                location = this.Container.Transshipment3LocationName;
                break;
            }

            case "POD": {
                name = "POD";
                location = this.Container.PODLocationName;
                break;
            }

            case "ONC": {
                name = "On Carriage";
                location = this.Container.OnCarriageLocationName;
                break;
            }

            case "DELV": {
                name = "Delivery";
                //location = this.Container.delivery;
                break;
            }

            case "EMRT": {
                name = "Empty Return";
                location = this.Container.EmptyReturnLocationName;
                break;
            }
        }

        this.Name = name;
        this.Location = location;
    }

    private GetTrnasshipmentLocation(): string {
        if (!AppTool.IsNullOrEmpty(this.Container.Transshipment3LocationName))
            return this.Container.Transshipment3LocationName;        

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment2LocationName))
            return this.Container.Transshipment2LocationName;        

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment1LocationName)) 
            return this.Container.Transshipment1LocationName;        

        else
            return null;
    }
}
