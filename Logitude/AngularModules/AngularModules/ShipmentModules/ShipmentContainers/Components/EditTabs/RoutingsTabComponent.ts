import { Component, OnInit, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from 'Shipment/EntityPMs/ContainerPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTimeToDatePipe } from '../../../../Controls/Pipes/DateTimeToDatePipe';

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

            this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe(s => {
                if (s) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.Refresh();
                }
            });
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
        });
    }

    private SetUIProperties() {

    }

    private BuildItemsSource() {
        this.ItemsSource = [];

        this.AddRoutingItem("PICK", "PREC");
        this.AddRoutingItem("PREC", "POL");
        this.AddRoutingItem("POL", "TS1");
        this.AddRoutingItem("TSS", "POD");
        this.AddRoutingItem("TS1", "TS2", true, true);
        this.AddRoutingItem("TS2", "TS3", true, true);
        this.AddRoutingItem("TS3", "POD", true, true);
        this.AddRoutingItem("POD", "ONC");
        this.AddRoutingItem("ONC", "DELV");
        this.AddRoutingItem("DELV", "EMRT");
        this.AddRoutingItem("EMRT", null);
    }

    private AddRoutingItem(code: string, nextLegCode: string, isHidden: boolean = false, isTransshipment: boolean = false) {
        var item: RoutingItem = new RoutingItem(code, this.EntityPM);
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
    public Date: string;
    public IsLine: boolean = false;
    public IsHidden: boolean = false;
    public IsDashedLine: boolean = false;
    public IsContinuousLine: boolean = false;

    private Calculate() {
        var name: string = null;
        var location: string = null;
        var date: string = null;

        switch (this.Code) {
            case "PICK": {
                name = "Empty Pickup";
                location = this.Container.EmptyPickupLocationName;
                date = this.GetDate_EmptyPickup();
                break;
            }

            case "PREC": {
                name = "Pre Carriage";
                location = this.Container.PreCarriageLocationName;
                date = this.GetDate_PreCarriage();
                break;
            }

            case "POL": {
                name = "POL";
                location = this.Container.POLLocationName;
                date = this.GetDate_POL();
                break;
            }

            case "TSS": {
                name = "Transshipments";
                location = this.GetTrnasshipmentLocation();
                date = this.GetTrnasshipmentDate();
                break;
            }

            case "TS1": {
                name = "Transshipment 1";
                location = this.Container.Transshipment1LocationName;
                date = this.GetDate_TS1();
                break;
            }

            case "TS2": {
                name = "Transshipment 2";
                location = this.Container.Transshipment2LocationName;
                date = this.GetDate_TS2();
                break;
            }

            case "TS3": {
                name = "Transshipment 3";
                location = this.Container.Transshipment3LocationName;
                date = this.GetDate_TS3();
                break;
            }

            case "POD": {
                name = "POD";
                location = this.Container.PODLocationName;
                date = this.GetDate_POD();
                break;
            }

            case "ONC": {
                name = "On Carriage";
                location = this.Container.OnCarriageLocationName;
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
                location = this.Container.EmptyReturnLocationName;
                date = this.GetDate_EmptyReturn();
                break;
            }
        }

        this.Name = name;
        this.Location = location;
        this.Date = date;
    }

    private GetDate_EmptyPickup(): string {
        if (this.Container.EstimatedEmptyPickupDate != null)
            return this.GetDateString("Container.F.EstimatedEmptyPickupDate", this.Container.EstimatedEmptyPickupDate);

        else if (this.Container.ActualEmptyPickupDate != null)
            return this.GetDateString("Container.F.ActualEmptyPickupDate", this.Container.ActualEmptyPickupDate);

        else
            return null;
    }
    private GetDate_PreCarriage(): string {
        if (this.Container.PreCarriageATD != null)
            return this.GetDateString("Container.F.PreCarriageATD", this.Container.PreCarriageATD);

        else if (this.Container.PreCarriageETD != null)
            return this.GetDateString("Container.F.PreCarriageETD", this.Container.PreCarriageETD);

        else if (this.Container.PreCarriageGateIn != null)
            return this.GetDateString("Container.F.PreCarriageGateIn", this.Container.PreCarriageGateIn);

        else
            return null;
    }
    private GetDate_POL(): string {
        if (this.Container.ActualPOLVesselDeparture != null)
            return this.GetDateString("Container.F.ActualPOLVesselDeparture", this.Container.ActualPOLVesselDeparture);

        else if (this.Container.EstimatedPOLVesselDeparture != null)
            return this.GetDateString("Container.F.EstimatedPOLVesselDeparture", this.Container.EstimatedPOLVesselDeparture);

        else if (this.Container.ActualPOLLoaded != null)
            return this.GetDateString("Container.F.ActualPOLLoaded", this.Container.ActualPOLLoaded);

        else if (this.Container.EstimatedPOLLoaded != null)
            return this.GetDateString("Container.F.EstimatedPOLLoaded", this.Container.EstimatedPOLLoaded);

        else if (this.Container.GateIn != null)
            return this.GetDateString("Container.F.GateIn", this.Container.GateIn);

        else
            return null;
    }
    private GetDate_TS1(): string {
        if (this.Container.ActualTrans1VesselDeparture != null)
            return this.GetDateString("Container.F.ActualTrans1VesselDeparture", this.Container.ActualTrans1VesselDeparture);

        else if (this.Container.EstimatedTrans1VesselDeparture != null)
            return this.GetDateString("Container.F.EstimatedTrans1VesselDeparture", this.Container.EstimatedTrans1VesselDeparture);

        else if (this.Container.ActualTransshipment1VesselArrival != null)
            return this.GetDateString("Container.F.ActualTransshipment1VesselArrival", this.Container.ActualTransshipment1VesselArrival);

        else if (this.Container.EstimatedTrans1VesselArrival != null)
            return this.GetDateString("Container.F.EstimatedTrans1VesselArrival", this.Container.EstimatedTrans1VesselArrival);

        else if (this.Container.ActualTransshipment1Loaded != null)
            return this.GetDateString("Container.F.ActualTransshipment1Loaded", this.Container.ActualTransshipment1Loaded);

        else if (this.Container.EstimatedTransshipment1Loaded != null)
            return this.GetDateString("Container.F.EstimatedTransshipment1Loaded", this.Container.EstimatedTransshipment1Loaded);

        else
            return null;
    }
    private GetDate_TS2(): string {
        if (this.Container.ActualTrans2VesselDeparture != null)
            return this.GetDateString("Container.F.ActualTrans2VesselDeparture", this.Container.ActualTrans2VesselDeparture);

        else if (this.Container.EstimatedTrans2VesselDeparture != null)
            return this.GetDateString("Container.F.EstimatedTrans2VesselDeparture", this.Container.EstimatedTrans2VesselDeparture);

        else if (this.Container.ActualTransshipment2VesselArrival != null)
            return this.GetDateString("Container.F.ActualTransshipment2VesselArrival", this.Container.ActualTransshipment2VesselArrival);

        else if (this.Container.EstimatedTrans2VesselArrival != null)
            return this.GetDateString("Container.F.EstimatedTrans2VesselArrival", this.Container.EstimatedTrans2VesselArrival);

        else if (this.Container.ActualTransshipment2Loaded != null)
            return this.GetDateString("Container.F.ActualTransshipment2Loaded", this.Container.ActualTransshipment2Loaded);

        else if (this.Container.EstimatedTransshipment2Loaded != null)
            return this.GetDateString("Container.F.EstimatedTransshipment2Loaded", this.Container.EstimatedTransshipment2Loaded);

        else
            return null;
    }
    private GetDate_TS3(): string {
        if (this.Container.ActualTrans3VesselDeparture != null)
            return this.GetDateString("Container.F.ActualTrans3VesselDeparture", this.Container.ActualTrans3VesselDeparture);

        else if (this.Container.EstimatedTrans3VesselDeparture != null)
            return this.GetDateString("Container.F.EstimatedTrans3VesselDeparture", this.Container.EstimatedTrans3VesselDeparture);

        else if (this.Container.ActualTransshipment3VesselArrival != null)
            return this.GetDateString("Container.F.ActualTransshipment3VesselArrival", this.Container.ActualTransshipment3VesselArrival);

        else if (this.Container.EstimatedTrans3VesselArrival != null)
            return this.GetDateString("Container.F.EstimatedTrans3VesselArrival", this.Container.EstimatedTrans3VesselArrival);

        else if (this.Container.ActualTransshipment3Loaded != null)
            return this.GetDateString("Container.F.ActualTransshipment3Loaded", this.Container.ActualTransshipment3Loaded);

        else if (this.Container.EstimatedTransshipment3Loaded != null)
            return this.GetDateString("Container.F.EstimatedTransshipment3Loaded", this.Container.EstimatedTransshipment3Loaded);

        else
            return null;
    }
    private GetDate_POD(): string {
        if (this.Container.GateOut != null)
            return this.GetDateString("Container.F.GateOut", this.Container.GateOut);

        else if (this.Container.ActualPODDischarge != null)
            return this.GetDateString("Container.F.ActualPODDischarge", this.Container.ActualPODDischarge);

        else if (this.Container.EstimatedPODDischarge != null)
            return this.GetDateString("Container.F.EstimatedPODDischarge", this.Container.EstimatedPODDischarge);

        else if (this.Container.ActualPODVesselArrival != null)
            return this.GetDateString("Container.F.ActualPODVesselArrival", this.Container.ActualPODVesselArrival);

        else if (this.Container.EstimatedPODVesselArrival != null)
            return this.GetDateString("Container.F.EstimatedPODVesselArrival", this.Container.EstimatedPODVesselArrival);

        else
            return null;
    }
    private GetDate_OnCarriage(): string {
        if (this.Container.OnCarriageGateOut != null)
            return this.GetDateString("Container.F.OnCarriageGateOut", this.Container.OnCarriageGateOut);

        else if (this.Container.OnCarriageATD != null)
            return this.GetDateString("Container.F.OnCarriageATD", this.Container.OnCarriageATD);

        else if (this.Container.OnCarriageETD != null)
            return this.GetDateString("Container.F.OnCarriageETD", this.Container.OnCarriageETD);

        else if (this.Container.OnCarriageATA != null)
            return this.GetDateString("Container.F.OnCarriageATA", this.Container.OnCarriageATA);

        else if (this.Container.OnCarriageETA != null)
            return this.GetDateString("Container.F.OnCarriageETA", this.Container.OnCarriageETA);

        else
            return null;
    }
    private GetDate_Delivery(): string {
        if (this.Container.ShipmentDeliveryATA != null)
            return this.GetDateString("Container.F.ShipmentDeliveryATA", this.Container.ShipmentDeliveryATA);

        else if (this.Container.ShipmentDeliveryETA != null)
            return this.GetDateString("Container.F.ShipmentDeliveryETA", this.Container.ShipmentDeliveryETA);

        else if (this.Container.ShipmentDeliveryATD != null)
            return this.GetDateString("Container.F.ShipmentDeliveryATD", this.Container.ShipmentDeliveryATD);

        else if (this.Container.ShipmentDeliveryETD != null)
            return this.GetDateString("Container.F.ShipmentDeliveryETD", this.Container.ShipmentDeliveryETD);

        else
            return null;
    }
    private GetDate_EmptyReturn(): string {
        if (this.Container.ActualEmptyReturn != null)
            return this.GetDateString("Container.F.ActualEmptyReturn", this.Container.ActualEmptyReturn);

        else if (this.Container.EstimatedEmptyReturn != null)
            return this.GetDateString("Container.F.EstimatedEmptyReturn", this.Container.EstimatedEmptyReturn);

        else
            return null;
    }

    private GetDateString(fieldTextCode: string, fieldValue: Date): string {        
        return TextCodeTranslator.Translate(fieldTextCode) + ": " + DateTimeToDatePipe.Pipe(fieldValue);
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

    private GetTrnasshipmentDate(): string {
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
