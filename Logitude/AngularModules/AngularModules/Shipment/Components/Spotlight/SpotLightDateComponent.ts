declare var window: any;
declare var SelectingElement: any;
import {Input, Output, Component, OnInit, EventEmitter, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties, UIPropertyArgs} from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogCalendarComponent, DayOfMonth} from '../../../Infrastructure/Components/LogitudeComponents/LogCalendarComponent'
import {ControlsIdCounter} from '../../../Infrastructure/Utilities/ControlsIdCounter';
import {TimeSelectComponent} from '../../../Infrastructure/Components/LogitudeComponents/TimeSelectComponent'
import {FixedPositionDirective} from '../../../Infrastructure/Utilities/FixedPositionDirective';
import {FieldValidator} from '../../../Infrastructure/Validators/FieldValidator';
import {FormControl, FormBuilder, FormGroup, Validators} from '@angular/forms'
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../../Shipment/EntityPMs/ShipmentPickUpPM';
import {ShipmentDeliveryPM} from '../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {RoutingHelper} from '../../../Shipment/Tools';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';

@Component({
    selector: 'SpotLightDate',
    moduleId: module.id,
    templateUrl: './SpotLightDateComponent.html',
    inputs: ['EntityPM', 'legname', 'PickUpPM', 'DeliveryPM', 'State'],
})

export class SpotLightDateComponent extends BaseComponent implements OnInit, AfterViewInit {
    @Output() PopupClosed = new EventEmitter<any>();
    public ShowHelp: boolean = false;
    public ObjectTableName: string = null;
    public legname: string = null;
    public State: string = "Departure";
    public DataContext = this;
    _ShipmentPMService: ShipmentPMService;

    EntityPM: ShipmentPM;
    PickUpPM: ShipmentPickUpPM;
    DeliveryPM: ShipmentDeliveryPM;

    public ControlId: string = null;
    public DropdownId: string = null;
    public IsMouseIn: boolean = false;
    onMouseOver() {
        this.IsMouseIn = true;
    }

    onMouseOut() {
        this.IsMouseIn = false;
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        if (this.CurrentSession == null) {
            this.ControlId = "SpotLightDate_-1_-1";
            this.DropdownId = "SpotLightDateDropdownId_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("SpotLightDate");
            this.ControlId = "SpotLightDate" + idIndex;
            this.DropdownId = "SpotLightDateDropdownId" + idIndex;

        }

        this.CurrentSession.MouseDownEvent.subscribe((res) => {
            if (this.IsMouseIn == false) {
                this.OnLostFocus();
            }
        });

        this._ShipmentPMService = new ShipmentPMService();
    }

    private expectedDepartedDateIsChecked: boolean = false;
    public get ExpectedDepartedDateIsChecked() {
        if (this.DepartedDate_Estimate != null) {
            this.expectedDepartedDateIsChecked = true;
        }

        return this.expectedDepartedDateIsChecked;
    }
    public set ExpectedDepartedDateIsChecked(newValue: boolean) {
        this.expectedDepartedDateIsChecked = newValue;

        if (!newValue) {
            this.DepartedDate_Estimate = null;
        }
    }

    private actualDepartedDateIsChecked: boolean = false;
    public get ActualDepartedDateIsChecked() {
        if (this.DepartedDate_Actual != null) {
            this.actualDepartedDateIsChecked = true;
        }

        return this.actualDepartedDateIsChecked;
    }
    public set ActualDepartedDateIsChecked(newValue: boolean) {
        this.actualDepartedDateIsChecked = newValue;

        if (newValue) {
            this.DepartedDate_Actual = this.DepartedDate_Estimate;
        }

        else {
            this.DepartedDate_Actual = null;
        }
        //this.actualDepartedDateIsChecked = newValue;
    }

    private departedDate_Estimate: Date;
    public get DepartedDate_Estimate() {
        if (this.State == "Departure") {
            switch (this.legname) {
                case "Pick Up": { return this.PickUpPM.ETD; }
                case "Delivery": { return this.DeliveryPM.ETD; }
                case "Pre Carriage": { return this.EntityPM.PreCarriageETD; }
                case "Main Carriage": { return this.EntityPM.MainCarriageETD; }
                case "Transshipment1": { return this.EntityPM.Transshipment1ETD; }
                case "Transshipment2": { return this.EntityPM.Transshipment2ETD; }
                case "Transshipment3": { return this.EntityPM.Transshipment3ETD; }
                case "On Carriage": { return this.EntityPM.OnCarriageETD; }
                case "WarehouseLeg": { return this.EntityPM.WarehouseLegExpectedEntryDate; }

                default: { break; }
            }
        }
        else {
            switch (this.legname) {
                case "Pick Up": { return this.PickUpPM.ETA; }
                case "Delivery": { return this.DeliveryPM.ETA; }
                case "Pre Carriage": { return this.EntityPM.PreCarriageETA; }
                case "Main Carriage": { return this.EntityPM.MainCarriageETA; }
                case "Transshipment1": { return this.EntityPM.Transshipment1ETA; }
                case "Transshipment2": { return this.EntityPM.Transshipment2ETA; }
                case "Transshipment3": { return this.EntityPM.Transshipment3ETA; }
                case "On Carriage": { return this.EntityPM.OnCarriageETA; }
                case "WarehouseLeg": { return this.EntityPM.WarehouseLegExpectedReleaseDate; }

                default: { break; }
            }
        }
    }
    public set DepartedDate_Estimate(newValue: Date) {
        if (this.State == "Departure") {
            switch (this.legname) {
                case "Pick Up": { this.PickUpPM.ETD = newValue; break; }
                case "Delivery": { this.DeliveryPM.ETD = newValue; break; }
                case "Pre Carriage": { this.EntityPM.PreCarriageETD = newValue; break; }
                case "Main Carriage": { this.EntityPM.MainCarriageETD = newValue; break; }
                case "Transshipment1": { this.EntityPM.Transshipment1ETD = newValue; break; }
                case "Transshipment2": { this.EntityPM.Transshipment2ETD = newValue; break; }
                case "Transshipment3": { this.EntityPM.Transshipment3ETD = newValue; break; }
                case "On Carriage": { this.EntityPM.OnCarriageETD = newValue; break; }
                case "WarehouseLeg": { this.EntityPM.WarehouseLegExpectedEntryDate = newValue; break; }

                default: { break; }
            }
        }
        else {
            switch (this.legname) {
                case "Pick Up": { this.PickUpPM.ETA = newValue; break; }
                case "Delivery": { this.DeliveryPM.ETA = newValue; break; }
                case "Pre Carriage": { this.EntityPM.PreCarriageETA = newValue; break; }
                case "Main Carriage": { this.EntityPM.MainCarriageETA = newValue; break; }
                case "Transshipment1": { this.EntityPM.Transshipment1ETA = newValue; break; }
                case "Transshipment2": { this.EntityPM.Transshipment2ETA = newValue; break; }
                case "Transshipment3": { this.EntityPM.Transshipment3ETA = newValue; break; }
                case "On Carriage": { this.EntityPM.OnCarriageETA = newValue; break; }
                case "WarehouseLeg": { this.EntityPM.WarehouseLegExpectedReleaseDate = newValue; break; }

                default: { break; }
            }
        }
    }

    //private departedDate_Actual: Date;
    public get DepartedDate_Actual() {
        if (this.State == "Departure") {
            switch (this.legname) {
                case "Pick Up": { return this.PickUpPM.ATD; }
                case "Delivery": { return this.DeliveryPM.ATD; }
                case "Pre Carriage": { return this.EntityPM.PreCarriageATD; }
                case "Main Carriage": { return this.EntityPM.MainCarriageATD; }
                case "Transshipment1": { return this.EntityPM.Transshipment1ATD; }
                case "Transshipment2": { return this.EntityPM.Transshipment2ATD; }
                case "Transshipment3": { return this.EntityPM.Transshipment3ATD; }
                case "On Carriage": { return this.EntityPM.OnCarriageATD; }
                case "WarehouseLeg": { return this.EntityPM.WarehouseLegActualEntryDate; }

                default: { return null; }
            }
        }
        else {
            switch (this.legname) {
                case "Pick Up": { return this.PickUpPM.ATA; }
                case "Delivery": { return this.DeliveryPM.ATA; }
                case "Pre Carriage": { return this.EntityPM.PreCarriageATA; }
                case "Main Carriage": { return this.EntityPM.MainCarriageATA; }
                case "Transshipment1": { return this.EntityPM.Transshipment1ATA; }
                case "Transshipment2": { return this.EntityPM.Transshipment2ATA; }
                case "Transshipment3": { return this.EntityPM.Transshipment3ATA; }
                case "On Carriage": { return this.EntityPM.OnCarriageATA; }
                case "WarehouseLeg": { return this.EntityPM.WarehouseLegActualReleaseDate; }

                default: { return null; }
            }
        }

    }
    public set DepartedDate_Actual(newValue: Date) {
        if (this.State == "Departure") {
            switch (this.legname) {
                case "Pick Up":
                    {
                        this.PickUpPM.ATD = newValue;

                        //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                        //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber);
                        //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber + PickUpPM.PickUpDeliveryNumber);
                        break;
                    }

                case "Delivery":
                    {
                        this.DeliveryPM.ATD = newValue;

                        //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                        //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber);
                        //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber + DeliveryPM.PickUpDeliveryNumber);
                        break;
                    }

                case "Pre Carriage":
                    {
                        this.EntityPM.PreCarriageATD = newValue;
                        //this.SetActualMethod("PreCarriageDeparture");
                        break;
                    }

                case "Main Carriage":
                    {
                        this.EntityPM.MainCarriageATD = newValue;
                        //this.SetActualMethod("MainCarriageDeparture");
                        break;
                    }

                case "Transshipment1":
                    {
                        this.EntityPM.Transshipment1ATD = newValue;
                        //this.SetActualMethod("Transshipment1Departure");
                        break;
                    }

                case "Transshipment2":
                    {
                        this.EntityPM.Transshipment2ATD = newValue;
                        //this.SetActualMethod("Transshipment2Departure");
                        break;
                    }

                case "Transshipment3":
                    {
                        this.EntityPM.Transshipment3ATD = newValue;
                        //this.SetActualMethod("Transshipment2Departure");
                        break;
                    }

                case "On Carriage":
                    {
                        this.EntityPM.OnCarriageATD = newValue;
                        //this.SetActualMethod("OnCarriageDeparture");
                        break;
                    }

                case "WarehouseLeg":
                    {
                        this.EntityPM.WarehouseLegActualEntryDate = newValue;
                        break;
                    }

                default: { break; }
            }
        }
        else {
            switch (this.legname) {
                case "Pick Up":
                    {
                        this.PickUpPM.ATA = newValue;

                        //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                        //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber);
                        //this.SetActualMethod(followUpLegName + PickUpPM.PickUpDeliveryNumber + PickUpPM.PickUpDeliveryNumber);
                        break;
                    }

                case "Delivery":
                    {
                        this.DeliveryPM.ATA = newValue;

                        //string followUpLegName = LegName.Replace(" ", "") + "Departure";
                        //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber);
                        //this.SetActualMethod(followUpLegName + DeliveryPM.PickUpDeliveryNumber + DeliveryPM.PickUpDeliveryNumber);
                        break;
                    }

                case "Pre Carriage":
                    {
                        this.EntityPM.PreCarriageATA = newValue;
                        //this.SetActualMethod("PreCarriageDeparture");
                        break;
                    }

                case "Main Carriage":
                    {
                        this.EntityPM.MainCarriageATA = newValue;
                        //this.SetActualMethod("MainCarriageDeparture");
                        break;
                    }

                case "Transshipment1":
                    {
                        this.EntityPM.Transshipment1ATA = newValue;
                        //this.SetActualMethod("Transshipment1Departure");
                        break;
                    }

                case "Transshipment2":
                    {
                        this.EntityPM.Transshipment2ATA = newValue;
                        //this.SetActualMethod("Transshipment2Departure");
                        break;
                    }

                case "Transshipment3":
                    {
                        this.EntityPM.Transshipment3ATA = newValue;
                        //this.SetActualMethod("Transshipment2Departure");
                        break;
                    }

                case "On Carriage":
                    {
                        this.EntityPM.OnCarriageATA = newValue;
                        //this.SetActualMethod("OnCarriageDeparture");
                        break;
                    }

                case "WarehouseLeg":
                    {
                        this.EntityPM.WarehouseLegActualReleaseDate = newValue;
                        break;
                    }
                default: { break; }
            }
        }

        //this.departedDate_Actual = newValue;
    }

    private departedTime_Actual: Date;
    public get DepartedTime_Actual() { return this.departedTime_Actual }
    public set DepartedTime_Actual(newValue: Date) { this.departedTime_Actual = newValue; }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.SetControlPosition(), 1);
    }

    private SetControlPosition() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top + 22) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    }

    OnSelectedDepartedDate_ActualChanged(date) {
        this.DepartedDate_Actual = date.SelectedDate;
    }

    OnSelectedDepartedDate_EstimateChanged(date) {
        this.DepartedDate_Estimate = date.SelectedDate;
    }

    ngAfterViewInit() {

    }

    ngOnInit() {

    }
    ValidationErrorsList: string[];
    onOKBtnClick() {
        this.ValidationErrorsList = [];

        this._ShipmentPMService.update(this.EntityPM).subscribe(myResult => {
            if (!myResult.HasError) {
                this.OnLostFocus();
                this.PopupClosed.emit(this);
            }
            else {
                this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });
    }

    onCancelBtnClick() {
        this.OnLostFocus();
    }

    ToggleButtonFocus() {
        var elem = document.getElementById(this.DropdownId);
        if (elem) {
            elem.style.visibility = "visible";
            elem.style.opacity = "1";
            elem.style.pointerEvents = "auto";
        }
        this.RunPositionTimer();
    }

    OnLostFocus() {
        this.StopPositionTimer();
        var elem = document.getElementById(this.DropdownId);
        if (elem) {
            elem.style.visibility = "hidden";
            elem.style.opacity = "0";
            elem.style.pointerEvents = "none";
        }
    }


}
