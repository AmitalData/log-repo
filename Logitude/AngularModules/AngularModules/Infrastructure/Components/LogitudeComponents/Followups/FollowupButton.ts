import {Component, OnInit, OnDestroy} from '@angular/core';
import {AppTool, DateTool} from '../../../Tools';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {FeatureLocator} from '../../../Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../Utilities/TextCodeTranslator';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import {ShipmentDeliveryPM} from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Services/EntityResourceService';
import {EventTypeList} from '../../../EntityLists/EventTypeList';
import {EventTypeListService} from '../../../Services/StandardLists/EventTypeListService';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './FollowupButton.html',
    selector: "FollowupButton",
    inputs: ['QuotePM', 'ShipmentPM', 'PickUpPM', 'DeliveryPM', 'LegType', 'IsEnabled', 'IsAutomatic'],
})

export class FollowupButton implements OnInit, OnDestroy {
    public Source: string;
    public Tootip: string;
    public LegType: string = null;
    public QuotePM: QuotePM = null;
    public ShipmentPM: ShipmentPM = null;
    public PickUpPM: ShipmentPickUpPM;
    public DeliveryPM: ShipmentDeliveryPM;
    public ObjectTableName: string = null;
    public FollowupLegType: string = null;
    public HasFollowup: boolean = false;
    public IsFeatureExists: boolean = false;
    public IsResourceReady: boolean = false;
    public IsComponentVisible: boolean = false;
    public IsComponentInitited: boolean = false;
    public IsAutomatic: boolean = false;
    public ExpDate: Date = null;
    public ActDate: Date = null;
    public ExpDateName: string = null;
    public ActDateName: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.entityResourceService.getEntityResourceByTableName("FollowUp").subscribe((res: any) => {
            this.IsResourceReady = true;

            this.Listen();
            this.InitComponent();
        });
    }

    private PropertyChangedEvent: any = null;
    private FollowupsChangedEvent: any = null;
    Listen() {
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "FollowupDeleted") {
                    this.SetComponent();
                }
            });
        }
    }

    ngOnInit() {
        this.IsComponentInitited = true;
        this.InitComponent();
    }

    InitComponent() {
        if (this.IsResourceReady && this.IsComponentInitited) {
            if (!AppTool.IsNullOrEmpty(this.LegType)) {
                this.FollowupLegType = this.LegType.replace(" ", "");
                this.SetDateFieldsName();
            }

            if (this.QuotePM) {
                this.ObjectTableName = "Quote";
                if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Quote.Followups")) {
                    this.IsFeatureExists = true;
                }
            }

            else if (this.ShipmentPM) {
                this.ObjectTableName = "Shipment";
                if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Shipment.Followups")) {
                    this.IsFeatureExists = true;

                    if (!this.PropertyChangedEvent) {
                        if (this.PickUpPM) {
                            this.PropertyChangedEvent = this.PickUpPM.PropertyChanged.subscribe(s => {
                                if (s) {
                                    if (s.PropertyName == this.ActDateName) {
                                        this.SetComponent();
                                        this.ActDate = DateTool.GetDateParts(this.PickUpPM[this.ActDateName]).DateObject;
                                        if (this.ActDate != null) {
                                            this.DeleteCurrentFollowup();
                                        }
                                    }
                                }
                            });
                        }

                        else if (this.DeliveryPM) {
                            this.PropertyChangedEvent = this.DeliveryPM.PropertyChanged.subscribe(s => {
                                if (s) {
                                    if (s.PropertyName == this.ActDateName) {
                                        this.SetComponent();
                                        this.ActDate = DateTool.GetDateParts(this.DeliveryPM[this.ActDateName]).DateObject;
                                        if (this.ActDate != null) {
                                            this.DeleteCurrentFollowup();
                                        }
                                    }
                                }
                            });
                        }

                        else {
                            this.PropertyChangedEvent = this.ShipmentPM.PropertyChanged.subscribe(s => {
                                if (s) {
                                    if (s.PropertyName == this.ActDateName) {

                                        this.SetComponent();

                                        this.ActDate = DateTool.GetDateParts(this.ShipmentPM[this.ActDateName]).DateObject;
                                        if (this.ActDate != null) {
                                            this.DeleteCurrentFollowup();
                                        }
                                        this.SetSource();
                                        this.SetTooltip();
                                    }
                                }
                            });
                        }
                    }
                }
            }

            this.SetComponent();
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.FollowupsChangedEvent);
        AppTool.KillEventEmitter(this.PropertyChangedEvent);
    }

    DeleteCurrentFollowup() {
        var myFollowup: ShipmentFollowUpPM = this.ShipmentPM.FollowUps.filter(f => f.LegType == this.FollowupLegType)[0];
        if (myFollowup) {
            this.ShipmentPM.RemoveShipmentFollowUp(myFollowup);
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }

    SetComponent() {
        if (this.IsFeatureExists) {
            this.HasFollowup = false;
            this.SetDateFields();

            if (this.QuotePM) {

            }

            else if (this.ShipmentPM) {
                if (this.ShipmentPM.FollowUps.filter(f => f.LegType == this.FollowupLegType && f.Done != true).length > 0) {
                    this.HasFollowup = true;
                }
            }

            this.SetSource();
            this.SetTooltip();

        }
    }
    SetSource() {
        var src = "./_Resources/Images/Icons/Followups/Followup.png";

        if (this.HasFollowup) {
            src = "./_Resources/Images/Icons/Followups/Followup_Black.png";
        }

        else if (this.IsMouseOver) {
            src = "./_Resources/Images/Icons/Followups/Followup_Black.png";
        }

        this.Source = src;
    }
    SetTooltip() {
        if (this.HasFollowup) {
            this.Tootip = TextCodeTranslator.Translate("FollowUp.M.DeleteFollowUp");
        }

        else {
            this.Tootip = TextCodeTranslator.Translate("FollowUp.M.AddFollowUp");
        }
    }
    SetDateFields() {
        var myExpDate: Date = null;
        var myActDate: Date = null;

        var legType = this.FollowupLegType;
        if (legType == null) {
            legType = "";
        }

        if (this.QuotePM) {

        }

        else if (this.ShipmentPM) {

            if (legType.indexOf("PickUp") > -1) {
                if (this.PickUpPM) {
                    myExpDate = DateTool.GetDateParts(this.PickUpPM[this.ExpDateName]).DateObject;
                    myActDate = DateTool.GetDateParts(this.PickUpPM[this.ActDateName]).DateObject;
                }
            }

            else if (legType.indexOf("Delivery") > -1) {
                if (this.DeliveryPM) {
                    myExpDate = DateTool.GetDateParts(this.DeliveryPM[this.ExpDateName]).DateObject;
                    myActDate = DateTool.GetDateParts(this.DeliveryPM[this.ActDateName]).DateObject;
                }
            }

            else if (legType.indexOf("EmptyCR") > -1) {
                if (this.DeliveryPM) {
                    myExpDate = DateTool.GetDateParts(this.DeliveryPM[this.ExpDateName]).DateObject;
                    myActDate = DateTool.GetDateParts(this.DeliveryPM[this.ActDateName]).DateObject;
                }

            }
            else if (legType.indexOf("CustomsClearanceDate") > -1 || legType.indexOf("FreightRelease") > -1 || legType.indexOf("TerminalAvailable") > -1
                || legType.indexOf("MAWBOBLDate") > -1 || legType.indexOf("CutoffDate") > -1 ) {
                if (this.ShipmentPM) {
                    myExpDate = DateTool.GetDateParts(this.ShipmentPM[this.ExpDateName]).DateObject;
                    myActDate = DateTool.GetDateParts(this.ShipmentPM[this.ActDateName]).DateObject;
                }

            }
          

            else {
                myExpDate = DateTool.GetDateParts(this.ShipmentPM[this.ExpDateName]).DateObject;
                myActDate = DateTool.GetDateParts(this.ShipmentPM[this.ActDateName]).DateObject;
            }
        }

        this.ExpDate = myExpDate;
        this.ActDate = myActDate;
        this.IsComponentVisible = AppTool.IsNullOrEmpty(this.ActDate) ? true : false;

    }
    SetDateFieldsName() {
        if (this.QuotePM) {

        }

        else if (this.ShipmentPM) {
            switch (this.FollowupLegType) {

                // Pre
                case "PreCarriageDeparture": {
                    this.ExpDateName = "PreCarriageETD";
                    this.ActDateName = "PreCarriageATD";
                    break;
                }
                case "PreCarriageArrival": {
                    this.ExpDateName = "PreCarriageETA";
                    this.ActDateName = "PreCarriageATA";
                    break;
                }

                // Main
                case "MainCarriageDeparture": {
                    this.ExpDateName = "MainCarriageETD";
                    this.ActDateName = "MainCarriageATD";
                    break;
                }
                case "MainCarriageArrival": {
                    this.ExpDateName = "MainCarriageETA";
                    this.ActDateName = "MainCarriageATA";
                    break;
                }

                // TR1
                case "Transshipment1Departure": {
                    this.ExpDateName = "Transshipment1ETD";
                    this.ActDateName = "Transshipment1ATD";
                    break;
                }
                case "Transshipment1Arrival": {
                    this.ExpDateName = "Transshipment1ETA";
                    this.ActDateName = "Transshipment1ATA";
                    break;
                }

                // TR2
                case "Transshipment2Departure": {
                    this.ExpDateName = "Transshipment2ETD";
                    this.ActDateName = "Transshipment2ATD";
                    break;
                }
                case "Transshipment2Arrival": {
                    this.ExpDateName = "Transshipment2ETA";
                    this.ActDateName = "Transshipment2ATA";
                    break;
                }

                // TR3
                case "Transshipment3Departure": {
                    this.ExpDateName = "Transshipment3ETD";
                    this.ActDateName = "Transshipment3ATD";
                    break;
                }
                case "Transshipment3Arrival": {
                    this.ExpDateName = "Transshipment3ETA";
                    this.ActDateName = "Transshipment3ATA";
                    break;
                }

                // On
                case "OnCarriageDeparture": {
                    this.ExpDateName = "OnCarriageETD";
                    this.ActDateName = "OnCarriageATD";
                    break;
                }
                case "OnCarriageArrival": {
                    this.ExpDateName = "OnCarriageETA";
                    this.ActDateName = "OnCarriageATA";
                    break;
                }

                case "CustomsClearanceDate": {
                    this.ExpDateName = "CustomsClearanceDate";
                    this.ActDateName = "CustomsClearanceDate";
                    break;
                }

                case "FreightRelease": {
                    this.ExpDateName = "FreightRelease";
                    this.ActDateName = "FreightRelease";

                    break;
                }

                case "TerminalAvailable": {
                    this.ExpDateName = "TerminalAvailable";
                    this.ActDateName = "TerminalAvailable";

                    break;
                }

                case "WarehouseLegEntry": {
                    this.ExpDateName = "WarehouseLegExpectedEntryDate";
                    this.ActDateName = "WarehouseLegActualEntryDate";
                    break;
                }

                case "WarehouseLegRelease": {
                    this.ExpDateName = "WarehouseLegExpectedReleaseDate";
                    this.ActDateName = "WarehouseLegActualReleaseDate";
                    break;
                }

                case "MAWBOBLDate": {
                    this.ExpDateName = "MAWBOBLDate";
                    this.ActDateName = "MAWBOBLDate";
                    break;
                }

                case "CutoffDate": {
                    this.ExpDateName = "CutoffDate";
                    this.ActDateName = "CutoffDate";
                    break;
                }

                default: {
                    if (this.FollowupLegType) {
                        if (this.FollowupLegType.indexOf("Departure") > -1) {
                            this.ExpDateName = "ETD";
                            this.ActDateName = "ATD";
                        }

                        else if (this.FollowupLegType.indexOf("Arrival") > -1) {
                            this.ExpDateName = "ETA";
                            this.ActDateName = "ATA";
                        }
                    }

                    break;
                }
            }
        }
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
        }
    }

    private isMouseOver: boolean = false;
    get IsMouseOver() { return this.isMouseOver; }
    set IsMouseOver(value: boolean) {
        if (this.isMouseOver != value) {
            this.isMouseOver = value;
            this.SetSource();
        }
    }

    ButtonClicked() {
        if (this.QuotePM) {

        }

        else if (this.ShipmentPM) {
            if (!AppTool.IsNullOrEmpty(this.FollowupLegType)) {
                var allFollowups: ShipmentFollowUpPM[] = this.ShipmentPM.FollowUps;
                var myFollowup: ShipmentFollowUpPM = allFollowups.filter(f => f.LegType == this.FollowupLegType && f.Done != true)[0];

                if (myFollowup) {
                    this.ShipmentPM.RemoveShipmentFollowUp(myFollowup);
                    this.CurrentSession.FireEvent("FollowupsChanged");
                    this.SetComponent();
                }

                else {
                    if (this.IsAutomatic) {

                        var eventTypeCode: string = null;
                        var dateTime: Date = null;
                        
                        switch (this.FollowupLegType) {
                            case "MainCarriageDeparture":
                                {
                                    eventTypeCode = "DEP";
                                    dateTime = this.ShipmentPM.MainCarriageETD;
                                    break;
                                }

                            case "Transshipment1Departure":
                                {
                                    eventTypeCode = "T1DP";
                                    dateTime = this.ShipmentPM.Transshipment1ETD;
                                    break;
                                }

                            case "Transshipment2Departure":
                                {
                                    eventTypeCode = "T2DP";
                                    dateTime = this.ShipmentPM.Transshipment2ETD;
                                    break;
                                }

                            case "Transshipment3Departure":
                                {
                                    eventTypeCode = "T3DP";
                                    dateTime = this.ShipmentPM.Transshipment3ETD;
                                    break;
                                }

                            case "MainCarriageArrival":
                                {
                                    eventTypeCode = "ARR";
                                    dateTime = this.ShipmentPM.MainCarriageETA;
                                    break;
                                }

                            case "Transshipment1Arrival":
                                {
                                    eventTypeCode = "T1AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }

                            case "Transshipment2Arrival":
                                {
                                    eventTypeCode = "T2AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }

                            case "Transshipment3Arrival":
                                {
                                    eventTypeCode = "T3AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }

                            case "CustomsClearanceDate":
                                {
                                    if (this.ShipmentPM.DirectionId == "I") {
                                        eventTypeCode = "ICUC"; 
                                    }
                                    else {
                                        eventTypeCode = "ECUC";
                                    }
                                    dateTime = this.ShipmentPM.CustomsClearanceDate;
                                    break;
                                }

                            case "FreightRelease":
                                {
                                    eventTypeCode = "FRRL";
                                    dateTime = this.ShipmentPM.FreightRelease;
                                    break;
                                }

                            case "TerminalAvailable":
                                {
                                    eventTypeCode = "TRAV";
                                    dateTime = this.ShipmentPM.TerminalAvailable;
                                    break;
                                }
                            case "WarehouseLegEntry": {
                                eventTypeCode = "WHED";
                                dateTime = this.ShipmentPM.WarehouseLegExpectedEntryDate;
                                break;
                            }

                            case "WarehouseLegRelease": {
                                eventTypeCode = "WHRD";
                                dateTime = this.ShipmentPM.WarehouseLegExpectedReleaseDate;
                                break;
                            }

                            case "MAWBOBLDate":
                                {
                                    eventTypeCode = "OBLD";                                    
                                    dateTime = this.ShipmentPM.MAWBOBLDate;
                                    break;
                                }

                            case "CutoffDate":
                                {
                                    eventTypeCode = "CUTO";
                                    dateTime = this.ShipmentPM.CutoffDate;
                                    break;
                                }
                        }

                        var myService = new EventTypeListService();
                        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var AllEventTypes: EventTypeList[] = myResponse.Result;

                                var eventType: EventTypeList = AllEventTypes.filter(f => f.Code == eventTypeCode)[0];
                                if (eventType) {
                                    var newFollowUp: ShipmentFollowUpPM = new ShipmentFollowUpPM(null);
                                    newFollowUp.Tenant = SessionLocator.Tenant;
                                    newFollowUp.Date = dateTime;
                                    newFollowUp.IsNew = true;
                                    newFollowUp.Done = false;
                                    newFollowUp.Deleted = false;
                                    newFollowUp.EventTypeId = eventType.Id;
                                    newFollowUp.EventTypeFollowUpName = eventType.FollowUpEnglishName;
                                    newFollowUp.LegType = this.FollowupLegType;
                                    newFollowUp.ShipmentId = this.ShipmentPM.Id;
                                    newFollowUp.OwnerUserId = SessionLocator.LoggedUserId;
                                    this.ShipmentPM.AddShipmentFollowUp(newFollowUp);
                                    this.CurrentSession.FireEvent("FollowupsChanged");
                                    this.SetComponent();
                                }
                            }
                        });
                    }

                    else {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Routings.AddNewFollowUp");
                        logitudeWindow.WindowArgs = { Date: this.ExpDate, ShipmentPM: this.ShipmentPM, FollowupLegType: this.FollowupLegType };
                        logitudeWindow.Width = 350;
                        logitudeWindow.Height = 400;

                        logitudeWindow.WindowClosed.subscribe(s => {
                            if (s) {
                                this.SetComponent();
                            }
                        });

                        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Followups/AddFollowupComponent');
                    }
                }
            }
        }
    }
}
