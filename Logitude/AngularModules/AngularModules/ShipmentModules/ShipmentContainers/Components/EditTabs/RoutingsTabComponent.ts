import { Component, OnInit, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from 'Shipment/EntityPMs/ContainerPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTimeToDatePipe } from '../../../../Controls/Pipes/DateTimeToDatePipe';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { PropertyChangedArgs } from '../../../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

@Component({
    templateUrl: './RoutingsTabComponent.html',
})

export class RoutingsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ContainerPM;
    public ObjectTableName: string = "Container";    
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: RoutingItem[];
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
    private PropertyChangedEvent: any = null;
    private Listen() {
        if (this.PropertyChangedEvent) {
            AppTool.KillEventEmitter(this.PropertyChangedEvent);
            this.PropertyChangedEvent = null;
        }

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

            this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe((fieldChanged: PropertyChangedArgs) => {
                if (fieldChanged) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.Refresh();
                }
            });
        }
    }
    private GetChangedLegIndex(fieldChanged: string): number {
        if (AppTool.IsNullOrEmpty(fieldChanged))
            return 0;

        else {
            if (fieldChanged.toLowerCase().indexOf("pickup") > -1)
                return 1;

            else if (fieldChanged.toLowerCase().indexOf("precarriage") > -1)
                return 2;

            else if (fieldChanged.toLowerCase().indexOf("pol") > -1)
                return 3;

            else if (fieldChanged.toLowerCase().indexOf("trans") > -1)
                return 4;

            else if (fieldChanged.toLowerCase().indexOf("pod") > -1)
                return 5;

            else if (fieldChanged.toLowerCase().indexOf("oncarriage") > -1)
                return 6;

            else if (fieldChanged.toLowerCase().indexOf("delivery") > -1)
                return 7;

            else if (fieldChanged.toLowerCase().indexOf("return") > -1)
                return 8;
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.PropertyChangedEvent);
    }

    ngOnInit() {

    }

    private Refresh() {
        this.ItemsSource.forEach(item => {
            item.IsContinuousLine = this.CheckNextLegDates(item.NextLegCode);
            item.IsDashedLine = !item.IsContinuousLine;
            item.Calculate();
        });

        this.SetPreviousLegProperites();
    }
    private SetUIProperties() {

    }

    private lastLegCode: string = null;
    private BuildItemsSource() {
        this.ItemsSource = [];

        this.AddRoutingItem("PICK", "PREC", 1);
        this.AddRoutingItem("PREC", "POL", 2);
        this.AddRoutingItem("POL", "TSS", 3);
        this.AddRoutingItem("TSS", "POD", 4);
        this.AddRoutingItem("POD", "ONC", 5);
        this.AddRoutingItem("ONC", "DELV", 6);
        this.AddRoutingItem("DELV", "EMRT", 7);
        this.AddRoutingItem("EMRT", null, 8);

        this.SetPreviousLegProperites();
        this.RoutingItemClicked(this.lastLegCode);
    }
    private SetPreviousLegProperites() {
        this.ItemsSource.forEach(item => {
            this.ItemsSource.filter(d => d.Index < item.Index).forEach(previousItem => {
                if (item.IsGreenCircle) {
                    previousItem.IsGreenCircle = item.IsGreenCircle;
                    previousItem.IsOrangeCircle = item.IsOrangeCircle;
                }

                else if (item.IsOrangeCircle) {
                    previousItem.IsGreenCircle = true;
                    previousItem.IsOrangeCircle = false;
                }
            });
        });
    }

    private AddRoutingItem(code: string, nextLegCode: string, index: number) {
        var item: RoutingItem = new RoutingItem(code, this.EntityPM);
        item.NextLegCode = nextLegCode;
        item.Index = index;
        item.IsContinuousLine = this.CheckNextLegDates(nextLegCode);
        item.IsDashedLine = !item.IsContinuousLine;
        item.Calculate();
        this.ItemsSource.push(item);

        if (item.IsGreenCircle || item.IsOrangeCircle) {
            this.lastLegCode = item.Code;
        }
    }

    private CheckNextLegDates(nextLegCode: string): boolean {
        switch (nextLegCode) {
            case "PREC": {
                return this.CheckNextDates_PreCarriage();
            }

            case "POL": {
                return this.CheckNextDates_POL();
            }

            case "TSS": {
                return this.CheckNextDates_Transshipments();
            }

            case "POD": {
                return this.CheckNextDates_POD();
            }

            case "ONC": {
                return this.CheckNextDates_OnCarriage();
            }

            case "DELV": {
                return this.CheckNextDates_Delivery();
            }

            case "EMRT": {
                return this.CheckNextDates_EmptyReturn();
            }

            default: {
                return false;
            }
        }
    }
    private CheckNextDates_PreCarriage(): boolean {
        if (this.EntityPM.PreCarriageATD != null)
            return true;

        else if (this.EntityPM.PreCarriageGateIn != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_POL(): boolean {
        if (this.EntityPM.ActualPOLVesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualPOLLoaded != null)
            return true;

        else if (this.EntityPM.GateIn != null)
            return true;
        else
            return false;
    }
    private CheckNextDates_Transshipments(): boolean {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3LocationName))
            return this.CheckNextDates_TS3();

        else if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2LocationName))
            return this.CheckNextDates_TS2();

        else if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1LocationName))
            return this.CheckNextDates_TS1();        
    }
    private CheckNextDates_TS3(): boolean {
        if (this.EntityPM.ActualTrans3VesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualTransshipment3VesselArrival != null)
            return true;

        else if (this.EntityPM.ActualTransshipment3Loaded != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_TS2(): boolean {
        if (this.EntityPM.ActualTrans2VesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualTransshipment2VesselArrival != null)
            return true;

        else if (this.EntityPM.ActualTransshipment2Loaded != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_TS1(): boolean {
        if (this.EntityPM.ActualTrans1VesselDeparture != null)
            return true;

        else if (this.EntityPM.ActualTransshipment1VesselArrival != null)
            return true;

        else if (this.EntityPM.ActualTransshipment1Loaded != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_POD(): boolean {
        if (this.EntityPM.GateOut != null)
            return true;

        else if (this.EntityPM.ActualPODDischarge != null)
            return true;

        else if (this.EntityPM.ActualPODVesselArrival != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_OnCarriage(): boolean {
        if (this.EntityPM.OnCarriageGateOut != null)
            return true;

        else if (this.EntityPM.OnCarriageATD != null)
            return true;

        else if (this.EntityPM.OnCarriageATA != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_Delivery(): boolean {
        if (this.EntityPM.ShipmentDeliveryATA != null)
            return true;

        else if (this.EntityPM.ShipmentDeliveryATD != null)
            return true;

        else
            return false;
    }
    private CheckNextDates_EmptyReturn(): boolean {
        if (this.EntityPM.ActualEmptyReturn != null)
            return true;

        else
            return false;
    }

    RoutingItemClicked(code: string) {
        //if (code == "TSS") {
        //    this.SelectedLegCode = "TS1";
        //    this.SelectedLegTitle = this.ItemsSource.filter(d => d.Code == "TS1")[0]?.Name;

        //    this.ItemsSource.forEach(item => {
        //        if (item.Code == "TSS")
        //            item.IsHidden = true;

        //        else if (item.Code == "Line" && !item.IsTransshipment && item.NextLegCode == "POD")
        //            item.IsHidden = true;

        //        else if (item.IsTransshipment)
        //            item.IsHidden = false;

        //        else if (item.Code == "Line" && item.IsTransshipment && item.NextLegCode == "POD")
        //            item.IsHidden = false;
        //    });
        //}

        //else {
            this.SelectedLegCode = code;
            this.SelectedLegTitle = this.ItemsSource.filter(d => d.Code == code)[0]?.Name;
        //}
    }

    private emptyPickupLocationPort: PortList;
    get EmptyPickupLocationPort() { return this.emptyPickupLocationPort; }
    set EmptyPickupLocationPort(value: PortList) {
        if (this.emptyPickupLocationPort != value) {
            this.emptyPickupLocationPort = value;
            this.EntityPM.EmptyPickupLocationName = value?.EnglishName;
            this.EntityPM.EmptyPickupLocation = value?.CombinedCode;
        }
    }

    private preCarriageLocationPort: PortList;
    get PreCarriageLocationPort() { return this.preCarriageLocationPort; }
    set PreCarriageLocationPort(value: PortList) {
        if (this.preCarriageLocationPort != value) {
            this.preCarriageLocationPort = value;
            this.EntityPM.PreCarriageLocationName = value?.EnglishName;
            this.EntityPM.PreCarriageLocation = value?.CombinedCode;
        }
    }

    private pOLLocationPort: PortList;
    get POLLocationPort() { return this.pOLLocationPort; }
    set POLLocationPort(value: PortList) {
        if (this.pOLLocationPort != value) {
            this.pOLLocationPort = value;
            this.EntityPM.POLLocationName = value?.EnglishName;
            this.EntityPM.POLLocation = value?.CombinedCode;
        }
    }

    private transshipment1LocationPort: PortList;
    get Transshipment1LocationPort() { return this.transshipment1LocationPort; }
    set Transshipment1LocationPort(value: PortList) {
        if (this.transshipment1LocationPort != value) {
            this.transshipment1LocationPort = value;
            this.EntityPM.Transshipment1LocationName = value?.EnglishName;
            this.EntityPM.Transshipment1Location = value?.CombinedCode;
        }
    }

    private transshipment2LocationPort: PortList;
    get Transshipment2LocationPort() { return this.transshipment2LocationPort; }
    set Transshipment2LocationPort(value: PortList) {
        if (this.transshipment2LocationPort != value) {
            this.transshipment2LocationPort = value;
            this.EntityPM.Transshipment2LocationName = value?.EnglishName;
            this.EntityPM.Transshipment2Location = value?.CombinedCode;
        }
    }

    private transshipment3LocationPort: PortList;
    get Transshipment3LocationPort() { return this.transshipment3LocationPort; }
    set Transshipment3LocationPort(value: PortList) {
        if (this.transshipment3LocationPort != value) {
            this.transshipment3LocationPort = value;
            this.EntityPM.Transshipment3LocationName = value?.EnglishName;
            this.EntityPM.Transshipment3Location = value?.CombinedCode;
        }
    }

    private pODLocationPort: PortList;
    get PODLocationPort() { return this.pODLocationPort; }
    set PODLocationPort(value: PortList) {
        if (this.pODLocationPort != value) {
            this.pODLocationPort = value;
            this.EntityPM.PODLocationName = value?.EnglishName;
            this.EntityPM.PODLocation = value?.CombinedCode;
        }
    }

    private onCarriageLocationPort: PortList;
    get OnCarriageLocationPort() { return this.onCarriageLocationPort; }
    set OnCarriageLocationPort(value: PortList) {
        if (this.onCarriageLocationPort != value) {
            this.onCarriageLocationPort = value;
            this.EntityPM.OnCarriageLocationName = value?.EnglishName;
            this.EntityPM.OnCarriageLocation = value?.CombinedCode;
        }
    }

    private emptyReturnLocationPort: PortList;
    get EmptyReturnLocationPort() { return this.emptyReturnLocationPort; }
    set EmptyReturnLocationPort(value: PortList) {
        if (this.emptyReturnLocationPort != value) {
            this.emptyReturnLocationPort = value;
            this.EntityPM.EmptyReturnLocationName = value?.EnglishName;
            this.EntityPM.EmptyReturnLocation = value?.CombinedCode;
        }
    }
}

export class RoutingItem {
    private Container: ContainerPM;
    constructor(code: string, container: ContainerPM) {
        this.Code = code;
        this.Container = container;     
    }

    public IsTransshipment: boolean;
    public Code: string;
    public NextLegCode: string;
    public PreviousLegCode: string;
    public Index: number;
    public Name: string;
    public Location: string;
    public Date: string;
    public IsLine: boolean = false;
    public IsHidden: boolean = false;
    public IsDashedLine: boolean = false;
    public IsContinuousLine: boolean = false;
    public IsGreenCircle: boolean = false;
    public IsOrangeCircle: boolean = false;

    public Calculate() {
        var name: string = null;
        var location: string = null;
        var date: string = null;

        switch (this.Code) {
            case "PICK": {
                name = "Empty Pickup";
                location = !AppTool.IsNullOrEmpty(this.Container.EmptyPickupLocationName) ? this.Container.EmptyPickupLocationName : (!AppTool.IsNullOrEmpty(this.Container.ShipmentPickupFrom) ? this.Container.ShipmentPickupFrom + " , " + this.Container.ShipmentPickupTo : null);
                date = this.GetDate_EmptyPickup();
                break;
            }

            case "PREC": {
                name = "Pre Carriage";
                location = this.Container.PreCarriageLocationName ?? this.Container.ShipmentPreCarriageFromName;
                date = this.GetDate_PreCarriage();
                break;
            }

            case "POL": {
                name = "POL";
                location = this.Container.POLLocationName ?? this.Container.ShipmentMainCarriageFromName;
                date = this.GetDate_POL();
                break;
            }

            case "TSS": {
                name = "Transshipments";
                location = this.GetTrnasshipmentLocation();
                date = this.GetTrnasshipmentDate();
                break;
            }

            case "POD": {
                name = "POD";
                location = this.Container.PODLocationName ?? this.Container.ShipmentMainCarriageToName;
                date = this.GetDate_POD();
                break;
            }

            case "ONC": {
                name = "On Carriage";
                location = this.Container.OnCarriageLocationName ?? this.Container.ShipmentOnCarriageToName;
                date = this.GetDate_OnCarriage();
                break;
            }

            case "DELV": {
                name = "Delivery";
                location = !AppTool.IsNullOrEmpty(this.Container.ShipmentDeliveryFrom) ? this.Container.ShipmentDeliveryFrom + " , " + this.Container.ShipmentDeliveryTo : null;
                date = this.GetDate_Delivery();
                break;
            }

            case "EMRT": {
                name = "Empty Return";
                location = !AppTool.IsNullOrEmpty(this.Container.EmptyReturnLocationName) ? this.Container.EmptyReturnLocationName :  (!AppTool.IsNullOrEmpty(this.Container.EmptyContainerReturnFrom) ? this.Container.EmptyContainerReturnFrom + " , " + this.Container.EmptyContainerReturnTo : null);
                date = this.GetDate_EmptyReturn();
                break;
            }
        }

        this.Name = name;
        this.Location = location;
        this.Date = date;

        this.CheckActualDates();
    }

    private GetDate_EmptyPickup(): string {
        if (this.Container.ActualEmptyPickupDate != null)
            return this.GetDateString("Act.", this.Container.ActualEmptyPickupDate);

        else if (this.Container.ShipmentPickupATD != null)
            return this.GetDateString("Act.", this.Container.ShipmentPickupATD);

        else if (this.Container.EstimatedEmptyPickupDate != null)
            return this.GetDateString("Est.", this.Container.EstimatedEmptyPickupDate);

        else if (this.Container.ShipmentPickupETD != null)
            return this.GetDateString("Est.", this.Container.ShipmentPickupETD);

        else
            return null;
    }
    private GetDate_PreCarriage(): string {
        if (this.Container.PreCarriageATD != null)
            return this.GetDateString("ATD", this.Container.PreCarriageATD);

        else if (this.Container.ShipmentPreCarriageATD != null)
            return this.GetDateString("ATD", this.Container.ShipmentPreCarriageATD);

        else if (this.Container.PreCarriageETD != null)
            return this.GetDateString("ETD", this.Container.PreCarriageETD);

        else if (this.Container.ShipmentPreCarriageETD != null)
            return this.GetDateString("ETD", this.Container.ShipmentPreCarriageETD);

        else if (this.Container.PreCarriageGateIn != null)
            return this.GetDateString("Gate In", this.Container.PreCarriageGateIn);

        else
            return null;
    }
    private GetDate_POL(): string {
        if (this.Container.ActualPOLVesselDeparture != null)
            return this.GetDateString("Vessel ATD", this.Container.ActualPOLVesselDeparture);

        else if (this.Container.ShipmentMainCarriageATD != null)
            return this.GetDateString("Vessel ATD", this.Container.ShipmentMainCarriageATD);

        else if (this.Container.EstimatedPOLVesselDeparture != null)
            return this.GetDateString("Vessel ETD", this.Container.EstimatedPOLVesselDeparture);

        else if (this.Container.ShipmentMainCarriageETD != null)
            return this.GetDateString("Vessel ETD", this.Container.ShipmentMainCarriageETD);

        else if (this.Container.ActualPOLLoaded != null)
            return this.GetDateString("Loaded Actual", this.Container.ActualPOLLoaded);

        else if (this.Container.EstimatedPOLLoaded != null)
            return this.GetDateString("Loaded Est.", this.Container.EstimatedPOLLoaded);

        else if (this.Container.GateIn != null)
            return this.GetDateString("Gate In", this.Container.GateIn);

        else
            return null;
    }
    private GetDate_TS1(): string {
        if (this.Container.ActualTrans1VesselDeparture != null)
            return this.GetDateString("Vessel ATD", this.Container.ActualTrans1VesselDeparture);

        else if (this.Container.ShipmentTransshipment1ATD != null)
            return this.GetDateString("Vessel ATD", this.Container.ShipmentTransshipment1ATD);

        else if (this.Container.EstimatedTrans1VesselDeparture != null)
            return this.GetDateString("Vessel ETD", this.Container.EstimatedTrans1VesselDeparture);

        else if (this.Container.ShipmentTransshipment1ETD != null)
            return this.GetDateString("Vessel ETD", this.Container.ShipmentTransshipment1ETD);

        else if (this.Container.ActualTransshipment1VesselArrival != null)
            return this.GetDateString("Vessel ATA", this.Container.ActualTransshipment1VesselArrival);

        else if (this.Container.EstimatedTrans1VesselArrival != null)
            return this.GetDateString("Vessel ETA", this.Container.EstimatedTrans1VesselArrival);

        else if (this.Container.ActualTransshipment1Loaded != null)
            return this.GetDateString("Loaded Actual", this.Container.ActualTransshipment1Loaded);

        else if (this.Container.EstimatedTransshipment1Loaded != null)
            return this.GetDateString("Loaded Est.", this.Container.EstimatedTransshipment1Loaded);

        else
            return null;
    }
    private GetDate_TS2(): string {
        if (this.Container.ActualTrans2VesselDeparture != null)
            return this.GetDateString("Vessel ATD", this.Container.ActualTrans2VesselDeparture);

        else if (this.Container.ShipmentTransshipment2ATD != null)
            return this.GetDateString("Vessel ATD", this.Container.ShipmentTransshipment2ATD);

        else if (this.Container.EstimatedTrans2VesselDeparture != null)
            return this.GetDateString("Vessel ETD", this.Container.EstimatedTrans2VesselDeparture);

        else if (this.Container.ShipmentTransshipment2ETD != null)
            return this.GetDateString("Vessel ETD", this.Container.ShipmentTransshipment2ETD);

        else if (this.Container.ActualTransshipment2VesselArrival != null)
            return this.GetDateString("Vessel ATA", this.Container.ActualTransshipment2VesselArrival);

        else if (this.Container.EstimatedTrans2VesselArrival != null)
            return this.GetDateString("Vessel ETA", this.Container.EstimatedTrans2VesselArrival);

        else if (this.Container.ActualTransshipment2Loaded != null)
            return this.GetDateString("Loaded Actual", this.Container.ActualTransshipment2Loaded);

        else if (this.Container.EstimatedTransshipment2Loaded != null)
            return this.GetDateString("Loaded Est.", this.Container.EstimatedTransshipment2Loaded);

        else
            return null;
    }
    private GetDate_TS3(): string {
        if (this.Container.ActualTrans3VesselDeparture != null)
            return this.GetDateString("Vessel ATD", this.Container.ActualTrans3VesselDeparture);

        else if (this.Container.ShipmentTransshipment3ATD != null)
            return this.GetDateString("Vessel ATD", this.Container.ShipmentTransshipment3ATD);

        else if (this.Container.EstimatedTrans3VesselDeparture != null)
            return this.GetDateString("Vessel ETD", this.Container.EstimatedTrans3VesselDeparture);

        else if (this.Container.ShipmentTransshipment3ETD != null)
            return this.GetDateString("Vessel ETD", this.Container.ShipmentTransshipment3ETD);

        else if (this.Container.ActualTransshipment3VesselArrival != null)
            return this.GetDateString("Vessel ATA", this.Container.ActualTransshipment3VesselArrival);

        else if (this.Container.EstimatedTrans3VesselArrival != null)
            return this.GetDateString("Vessel ETA", this.Container.EstimatedTrans3VesselArrival);

        else if (this.Container.ActualTransshipment3Loaded != null)
            return this.GetDateString("Loaded Actual", this.Container.ActualTransshipment3Loaded);

        else if (this.Container.EstimatedTransshipment3Loaded != null)
            return this.GetDateString("Loaded Est.", this.Container.EstimatedTransshipment3Loaded);

        else
            return null;
    }
    private GetDate_POD(): string {
        if (this.Container.GateOut != null)
            return this.GetDateString("Gate Out", this.Container.GateOut);

        else if (this.Container.ActualPODDischarge != null)
            return this.GetDateString("Vessel ATD", this.Container.ActualPODDischarge);

        else if (this.Container.EstimatedPODDischarge != null)
            return this.GetDateString("Vessel ETD", this.Container.EstimatedPODDischarge);

        else if (this.Container.ActualPODVesselArrival != null)
            return this.GetDateString("Vessel ATA", this.Container.ActualPODVesselArrival);

        else if (this.Container.ShipmentLastLegATA != null)
            return this.GetDateString("Vessel ATA", this.Container.ShipmentLastLegATA);

        else if (this.Container.EstimatedPODVesselArrival != null)
            return this.GetDateString("Vessel ETA", this.Container.EstimatedPODVesselArrival);

        else if (this.Container.ShipmentLastLegETA != null)
            return this.GetDateString("Vessel ETA", this.Container.ShipmentLastLegETA);

        else
            return null;
    }
    private GetDate_OnCarriage(): string {
        if (this.Container.OnCarriageGateOut != null)
            return this.GetDateString("Gate Out", this.Container.OnCarriageGateOut);

        else if (this.Container.OnCarriageATD != null)
            return this.GetDateString("ATD", this.Container.OnCarriageATD);

        else if (this.Container.OnCarriageETD != null)
            return this.GetDateString("ETD", this.Container.OnCarriageETD);

        else if (this.Container.OnCarriageATA != null)
            return this.GetDateString("ATA", this.Container.OnCarriageATA);

        else if (this.Container.ShipmentOnCarriageATA != null)
            return this.GetDateString("ATA", this.Container.ShipmentOnCarriageATA);

        else if (this.Container.OnCarriageETA != null)
            return this.GetDateString("ETA", this.Container.OnCarriageETA);

        else if (this.Container.ShipmentOnCarriageETA != null)
            return this.GetDateString("ETA", this.Container.ShipmentOnCarriageETA);

        else
            return null;
    }
    private GetDate_Delivery(): string {
        if (this.Container.ShipmentDeliveryATA != null)
            return this.GetDateString("ATA", this.Container.ShipmentDeliveryATA);

        else if (this.Container.ShipmentDeliveryETA != null)
            return this.GetDateString("ETA", this.Container.ShipmentDeliveryETA);

        else if (this.Container.ShipmentDeliveryATD != null)
            return this.GetDateString("ATD", this.Container.ShipmentDeliveryATD);

        else if (this.Container.ShipmentDeliveryETD != null)
            return this.GetDateString("ETD", this.Container.ShipmentDeliveryETD);

        else
            return null;
    }
    private GetDate_EmptyReturn(): string {
        if (this.Container.ActualEmptyReturn != null)
            return this.GetDateString("Act.", this.Container.ActualEmptyReturn);

        else if (this.Container.EmptyContainerReturnATA != null)
            return this.GetDateString("Act.", this.Container.EmptyContainerReturnATA);

        else if (this.Container.EstimatedEmptyReturn != null)
            return this.GetDateString("Est.", this.Container.EstimatedEmptyReturn);

        else if (this.Container.EmptyContainerReturnETA != null)
            return this.GetDateString("Est.", this.Container.EmptyContainerReturnETA);

        else
            return null;
    }

    private GetDateString(field: string, fieldValue: Date): string {
        return field + ": " + DateTimeToDatePipe.Pipe(fieldValue);
    }

    private GetTrnasshipmentLocation(): string {
        if (!AppTool.IsNullOrEmpty(this.Container.Transshipment3LocationName) || !AppTool.IsNullOrEmpty(this.Container.ShipmentTransshipment3FromName))
            return this.Container.Transshipment3LocationName ?? this.Container.ShipmentTransshipment3FromName;

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment2LocationName) || !AppTool.IsNullOrEmpty(this.Container.ShipmentTransshipment2FromName))
            return this.Container.Transshipment2LocationName ?? this.Container.ShipmentTransshipment2FromName;

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment1LocationName) || !AppTool.IsNullOrEmpty(this.Container.ShipmentTransshipment1FromName))
            return this.Container.Transshipment1LocationName ?? this.Container.ShipmentTransshipment1FromName;

        else
            return null;
    }

    private GetTrnasshipmentDate(): string {
        if (!AppTool.IsNullOrEmpty(this.Container.Transshipment3LocationName))
            return this.GetDate_TS3();

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment2LocationName))
            return this.GetDate_TS2();

        else if (!AppTool.IsNullOrEmpty(this.Container.Transshipment1LocationName))
            return this.GetDate_TS1();

        else
            return null;
    }

    CheckActualDates() {
        switch (this.Code) {
            case "PICK": {
                this.CheckActualDates_EmptyPickup();
                break;
            }

            case "PREC": {
                this.CheckActualDates_PreCarriage();
                break;
            }

            case "POL": {
                this.CheckActualDates_POL();
                break;
            }

            case "TSS": {
                this.CheckActualDates_Transshipments();
                break;
            }

            case "POD": {
                this.CheckActualDates_POD();
                break;
            }

            case "ONC": {
                this.CheckActualDates_OnCarriage();
                break;
            }

            case "DELV": {
                this.CheckActualDates_Delivery();
                break;
            }

            case "EMRT": {
                this.CheckActualDates_EmptyReturn();
                break;
            }
        }
    }
    CheckActualDates_EmptyPickup() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = false;

        if (this.Container.ActualEmptyPickupDate != null || this.Container.ShipmentPickupATD != null)
            this.IsGreenCircle = true;
    }
    CheckActualDates_PreCarriage() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = true;

        if ((this.Container.PreCarriageATD != null || this.Container.ShipmentPreCarriageATD != null) && this.Container.PreCarriageGateIn != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.PreCarriageATD == null && this.Container.ShipmentPreCarriageATD == null && this.Container.PreCarriageGateIn == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_POL() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = true;

        if ((this.Container.ActualPOLVesselDeparture != null || this.Container.ShipmentMainCarriageATD != null) && this.Container.ActualPOLLoaded != null && this.Container.GateIn != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.ActualPOLVesselDeparture == null && this.Container.ShipmentMainCarriageATD == null && this.Container.ActualPOLLoaded == null && this.Container.GateIn == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_Transshipments() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = false;

        if (this.Container.ActualTrans3VesselDeparture != null || this.Container.ShipmentTransshipment3ATD != null || this.Container.ActualTransshipment3VesselArrival != null || this.Container.ActualTransshipment3Loaded != null)
            this.CheckActualDates_TS3();

        else if (this.Container.ActualTrans2VesselDeparture != null || this.Container.ShipmentTransshipment2ATD != null || this.Container.ActualTransshipment2VesselArrival != null || this.Container.ActualTransshipment2Loaded != null)
            this.CheckActualDates_TS2();

        else if (this.Container.ActualTrans1VesselDeparture != null || this.Container.ShipmentTransshipment1ATD != null || this.Container.ActualTransshipment1VesselArrival != null || this.Container.ActualTransshipment1Loaded != null)
            this.CheckActualDates_TS1();
    }
    CheckActualDates_TS3() {
        this.IsOrangeCircle = true;

        if ((this.Container.ActualTrans3VesselDeparture != null || this.Container.ShipmentTransshipment3ATD != null) && this.Container.ActualTransshipment3VesselArrival != null && this.Container.ActualTransshipment3Loaded != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.ActualTrans3VesselDeparture == null && this.Container.ShipmentTransshipment3ATD == null && this.Container.ActualTransshipment3VesselArrival == null && this.Container.ActualTransshipment3Loaded == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_TS2() {
        this.IsOrangeCircle = true;

        if ((this.Container.ActualTrans2VesselDeparture != null || this.Container.ShipmentTransshipment2ATD != null) && this.Container.ActualTransshipment2VesselArrival != null && this.Container.ActualTransshipment2Loaded != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.ActualTrans2VesselDeparture == null && this.Container.ShipmentTransshipment2ATD == null && this.Container.ActualTransshipment2VesselArrival == null && this.Container.ActualTransshipment2Loaded == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_TS1() {
        this.IsOrangeCircle = true;

        if ((this.Container.ActualTrans1VesselDeparture != null || this.Container.ShipmentTransshipment1ATD != null) && this.Container.ActualTransshipment1VesselArrival != null && this.Container.ActualTransshipment1Loaded != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.ActualTrans1VesselDeparture == null && this.Container.ShipmentTransshipment1ATD == null && this.Container.ActualTransshipment1VesselArrival == null && this.Container.ActualTransshipment1Loaded == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_POD() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = true;

        if (this.Container.GateOut != null && this.Container.ActualPODDischarge != null && (this.Container.ActualPODVesselArrival != null || this.Container.ShipmentLastLegATA != null)) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.GateOut == null && this.Container.ActualPODDischarge == null && this.Container.ActualPODVesselArrival == null && this.Container.ShipmentLastLegATA == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_OnCarriage() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = true;

        if (this.Container.OnCarriageGateOut != null && this.Container.OnCarriageATD != null && (this.Container.OnCarriageATA != null || this.Container.ShipmentOnCarriageATA != null)) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.OnCarriageGateOut == null && this.Container.OnCarriageATD == null && this.Container.OnCarriageATA == null && this.Container.ShipmentOnCarriageATA == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_Delivery() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = true;

        if (this.Container.ShipmentDeliveryATA != null && this.Container.ShipmentDeliveryATD != null) {
            this.IsGreenCircle = true;
            this.IsOrangeCircle = false;
        }

        else if (this.Container.ShipmentDeliveryATA == null && this.Container.ShipmentDeliveryATD == null)
            this.IsOrangeCircle = false;
    }
    CheckActualDates_EmptyReturn() {
        this.IsGreenCircle = false;
        this.IsOrangeCircle = false;

        if (this.Container.ActualEmptyReturn != null || this.Container.EmptyContainerReturnATA != null)
            this.IsGreenCircle = true;
    }
}
