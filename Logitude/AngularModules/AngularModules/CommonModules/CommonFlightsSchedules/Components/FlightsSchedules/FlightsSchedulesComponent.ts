import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {FlightsSchedulesRequestPM} from '../../../../Booking/EntityPMs/FlightsSchedulesRequestPM';
import {FlightsSchedulesResponsePM} from '../../../../Booking/EntityPMs/FlightsSchedulesResponsePM';
import {BookingPM} from '../../../../Booking/EntityPMs/BookingPM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {PartnersDomainService, AirlineMessagingRuleList} from '../../../../Common/Services/PartnersDomainService';
import {FVRWebService, FVRResultClass, FVASimulatorResult, FlightSchedulePort} from '../../../../Infrastructure/Services/WebServices/FVRWebService';
import {FlightsSchedulesRequestPMService} from '../../../../Booking/Services/StandardPMs/FlightsSchedulesRequestPMService';
import {FlightsSchedulesArgs, FVASimulatorWindowArgs, XMLSimulatorWindowArgs} from '../../Args';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BookingWizardArgs} from '../../../../Booking/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './FlightsSchedulesComponent.html',
})

export class FlightsSchedulesComponent extends BaseComponent {
    private myRequestId: string;
    private myBookingPM: BookingPM;
    private myShipmentPM: ShipmentPM;
    public EntityPM: FlightsSchedulesRequestPM;
    public ObjectTableName: string = "FlightsSchedulesRequest";
    public DataContext: FlightsSchedulesComponent = this;
    public RequestSent: boolean = false;
    public FlightSelected: boolean = false;
    public ItemsSource: FlightsSchedulesResponeViewModel[];
    public ValidationErrorsList: string[] = [];
    private myAirlineService: AirlineListService;
    private myPartnersDomainService: PartnersDomainService;
    public myFVRWebService: FVRWebService;
    public myFlightsSchedulesRequestPMService: FlightsSchedulesRequestPMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsResourcesReady: boolean = false;
    public IsSimulateVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = [];
       
        this.EntityPM = new FlightsSchedulesRequestPM();
        this.EntityPM.StatusCode = "W";
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;

        this.myAirlineService = new AirlineListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.myFVRWebService = new FVRWebService();

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "FlightsSchedules.Simulator")) {
            this.IsSimulateVisible = true;
        }
    }

    public args: FlightsSchedulesArgs;
    SetWindowArgs(args: FlightsSchedulesArgs) {
        this.args = args; 

        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(response => {
            this.IsResourcesReady = true;

            if (args.BookingPM != null) {
                this.myBookingPM = args.BookingPM;
                this.InitFromBooking();
            }

            else if (args.ShipmentPM != null) {
                this.myShipmentPM = args.ShipmentPM;
                this.InitFromShipment();
            }

            this.InitializeComponent();
        });
    }

    private InitFromBooking() {
        this.EntityPM.BookingId = this.myBookingPM.Id;
        this.EntityPM.AirlineId = this.myBookingPM.MainCarriageCarrierId;
        this.EntityPM.FromPortId = this.myBookingPM.MainCarriageFromPortId;
        this.EntityPM.ToPortId = this.myBookingPM.MainCarriageFinalDestinationPortId;
        this.EntityPM.ETD = this.myBookingPM.MainCarriageETD;
        this.EntityPM.ETA = null;
        this.EntityPM.Volume = this.myBookingPM.Volume;
        this.EntityPM.GrossWeight = this.myBookingPM.GrossWeight;
        this.EntityPM.VolumeUnitCode = this.myBookingPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.myBookingPM.GrossWeightUnitCode;

        if (this.EntityPM.ToPortId == null) {
            this.EntityPM.ToPortId = this.myBookingPM.MainCarriageToPortId;
        }

        if (!this.args.IsMainLeg) {
            this.EntityPM.AirlineId = this.myBookingPM.Transshipment1CarrierId;
            this.EntityPM.FromPortId = this.myBookingPM.Transshipment1FromPortId;
            this.EntityPM.ToPortId = this.myBookingPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ETD = this.myBookingPM.Transshipment1ETD;
        }
    }

    private InitFromShipment() {
        this.EntityPM.ShipmentId = this.myShipmentPM.Id;
        this.EntityPM.AirlineId = this.myShipmentPM.MainCarriageCarrierId;
        this.EntityPM.FromPortId = this.myShipmentPM.MainCarriageFromPortId;
        this.EntityPM.ToPortId = this.myShipmentPM.MainCarriageFinalDestinationPortId;
        this.EntityPM.ETD = this.myShipmentPM.MainCarriageETD;
        this.EntityPM.ETA = null;
        this.EntityPM.Volume = this.myShipmentPM.Volume;
        this.EntityPM.GrossWeight = this.myShipmentPM.GrossWeight;
        this.EntityPM.VolumeUnitCode = this.myShipmentPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.myShipmentPM.GrossWeightUnitCode;

        if (this.EntityPM.FromPortId == null) {
            this.EntityPM.FromPortId = this.myShipmentPM.FromPortId;
        }

        if (this.EntityPM.ToPortId == null) {
            this.EntityPM.ToPortId = this.myShipmentPM.ToPortId;
        }

        if (!this.args.IsMainLeg) {
            this.EntityPM.AirlineId = this.myShipmentPM.Transshipment1CarrierId;
            this.EntityPM.FromPortId = this.myShipmentPM.Transshipment1FromPortId;
            this.EntityPM.ToPortId = this.myShipmentPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ETD = this.myShipmentPM.Transshipment1ETD;
        }
    }

    private InitializeComponent() {
        //if (this.EntityPM.ETD != null) {
        //    this.EntityPM.ETD = DateTool.TruncateTime(this.EntityPM.ETD);
        //}

        if (AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            this.GrossWeightUnitCode = InfraSettings.TenantPM.GrossWeightUnitCode;
        }

        if (AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            this.VolumeUnitCode = InfraSettings.TenantPM.VolumeUnitCode;
        }

        this.SetUIProperties();
        this.SetUIProperties_Airline();

        if (AppTool.IsNullOrEmpty(this.AirlineId)) {
            this.LoadAllowedAirline();
        }

        else {
            this.LoadTenantZeroAirline();
        }
    }

    public OnFlightSelected() {
        this.args.IsFlightSelected = true;
        this.FlightSelected = true;
        this.Close();
    }
    public UpdateMissingPorts(myResult: FlightSchedulePort[]) {
        myResult.forEach(item0 => {
            this.ItemsSource.forEach(item1 => {
                item1.ItemsSource.forEach(item2 => {
                    item2.UpdateMissingPorts(item0);
                });
            });
        });
    }

    // SetUIProperties

    private SetUIProperties() {
        this.UIProperties.SetRequired("ETD", this.ObjectTableName, (this.ETD == null ? true : false));

        var isWeightRequired = false;
        if (this.GrossWeight > 0) {
            if (AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                isWeightRequired = true;
            }
        }

        var isVolumeRequired = false;
        if (this.Volume > 0) {
            if (AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
                isVolumeRequired = true;
            }
        }

        this.UIProperties.SetRequired("Volume", this.ObjectTableName, isVolumeRequired);
        this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isWeightRequired);
    }
    private SetUIProperties_Airline() {
        var isFieldEnabled = true;

        if (this.Airline != null) {
            if (this.Airline.NoAvailabilityInFVAMessages) {
                isFieldEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isFieldEnabled);
    }

    // Load Airline
    private myTenantZeroAirlineTTY: string = null;
    private IsAirlinepNeedsRegistration: boolean = false;
    private IsAirlineRegistered: boolean = false;
    private LoadAllowedAirline() {
        if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (AppTool.IsNullOrEmpty(this.AirlineId)) {
                this.myPartnersDomainService.GetAllowedAirlineId().subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            var allowedAirlineId: string = myResponse.Result;
                            if (!AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                this.AirlineId = allowedAirlineId;
                            }
                        }
                    }
                });
            }
        }
    }
    private LoadTenantZeroAirline() {
        this.myTenantZeroAirlineTTY = null;

        if (!AppTool.IsNullOrEmpty(this.AirlineId)) {
            this.myAirlineService.getSingle(this.AirlineId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    this.Airline = myResponse.Result;

                    this.SetResponseHelpMessage();
                    this.SetUIProperties_Airline();

                    if (this.Airline != null) {

                        this.IsAirlineRegistered = this.Airline.IsChampRegistered;

                        this.myPartnersDomainService.GetAirlineByCode(this.Airline.Code, 0).subscribe((myResponse2: ServiceResponse) => {
                            if (!myResponse2.HasError) {
                                var airlinePM: AirlinePM = myResponse2.Result;
                                if (airlinePM != null) {
                                    this.myTenantZeroAirlineTTY = airlinePM.TTY;
                                    this.IsAirlinepNeedsRegistration = airlinePM.ChampNeedsRegistration;
                                }
                            }
                        });
                    }
                }
            });
        }
    }

    // Properties
    private Airline: AirlineList;
    get AirlineId() { return this.EntityPM.AirlineId; }
    set AirlineId(newValue: string) {
        if (this.EntityPM.AirlineId != newValue) {
            this.EntityPM.AirlineId = newValue;
            this.SetUIProperties_Airline();
            this.LoadTenantZeroAirline();
        }
    }

    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(newValue: string) {
        if (this.EntityPM.FromPortId != newValue) {
            this.EntityPM.FromPortId = newValue;
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(newValue: string) {
        if (this.EntityPM.ToPortId != newValue) {
            this.EntityPM.ToPortId = newValue;
        }
    }

    get ETD() { return this.EntityPM.ETD; }
    set ETD(newValue: Date) {
        if (this.EntityPM.ETD != newValue) {
            this.EntityPM.ETD = newValue;
            this.SetUIProperties();
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(newValue: Date) {
        if (this.EntityPM.ETA != newValue) {
            this.EntityPM.ETA = newValue;
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = newValue;
            this.SetUIProperties();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = newValue;
            this.SetUIProperties();
        }
    }

    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    set VolumeUnitCode(newValue: string) {
        if (this.EntityPM.VolumeUnitCode != newValue) {
            this.EntityPM.VolumeUnitCode = newValue;
            this.SetUIProperties();
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;
            this.SetUIProperties();
        }
    }

    public ResponseHelpMessage: string;
    private SetResponseHelpMessage() {
        var myResult = "Flights displayed are in the specific date of departure only";

        if (this.Airline != null) {
            if (this.Airline.ScheduleDays > 0) {
                var myParam = this.Airline.ScheduleDays + " days";
                if (this.Airline.ScheduleDays == 1) {
                    myParam = this.Airline.ScheduleDays + " day";
                }

                myResult = "Flights displayed are " + myParam + " from the departure date";
            }
        }       

        this.ResponseHelpMessage = myResult;
    }

    // Commands
    FindFlightsClicked() {

        this.CurrentSession.StartBusyIndicator("Sending Request...");

        var isValid: boolean = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }

        else {
            if (AppTool.IsNullOrEmpty(this.myTenantZeroAirlineTTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "This Airline doesn't support transmitting messages";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.TTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Tenant communication parameter (TTY) is missing";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else if (this.IsAirlinepNeedsRegistration && !this.IsAirlineRegistered) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else {
                this.myPartnersDomainService.GetAirlineRules(this.Airline.Code, "FVR").subscribe((myResponse: ServiceResponse) => {

                    var isValid = true;

                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            isValid = false;
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            var items: AirlineMessagingRuleList[] = myResponse.Result;
                            if (items != null) {
                                if (items.length > 0) {
                                    var errors: string[] = this.ValidateRules(items);
                                    if (errors.length > 0) {
                                        isValid = false;
                                        this.ValidationErrorsList = errors;
                                    }
                                }
                            }
                        }
                    }

                    if (isValid) {
                        this.SendRequest();
                    }

                    else {                        
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    }
    BuildXMLClicked() {
        var myBookingId: string = null;
        var myShipmentId: string = null;
        if (this.myBookingPM != null) {
            myBookingId = this.myBookingPM.Id;
        }

        if (this.myShipmentPM != null) {
            myShipmentId = this.myShipmentPM.Id;
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = { ShipmentId: myShipmentId, BookingId: myBookingId, FatherComponent: this };
        logWindow.Title = "Build FVA Response";
        logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/XMLFlightsSimulatorComponent');
    }
    public UpgradeFromXML(myResult: FVASimulatorResult) {
        this.ETD = myResult.ETD;
        this.FromPortId = myResult.FromPortId;
        this.ToPortId = myResult.ToPortId;
        this.AirlineId = myResult.AirlineId;
        this.myRequestId = myResult.RequestId;
        this.StartTimer();
        this.RequestSent = true;
    }

    SendFVAClicked() {
        var isValid: boolean = this.Validate();
        if (isValid) {
            if (AppTool.IsNullOrEmpty(this.myTenantZeroAirlineTTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "This Airline doesn't support transmitting messages";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.TTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Tenant communication parameter (TTY) is missing";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else if (this.IsAirlinepNeedsRegistration && !this.IsAirlineRegistered) {
                this.CurrentSession.StopBusyIndicator();               
                var validationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
                var messageWindow = new MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }

            else {
                var args = new FVASimulatorWindowArgs();
                args.EntityPM = this.EntityPM;
                args.AirlineList = this.Airline;
                args.Recipient = this.myTenantZeroAirlineTTY;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 920;
                logWindow.Height = 530;
                logWindow.WindowArgs = args;
                logWindow.Title = "Flights Schedules";
                logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FVASimulatorComponent');
            }
        }
    }
    CloseClicked() {
        this.Close();
    }

    private ValidationText: string = null;
    private Validate() {
        var isValid = true;
        var errors: string[] = [];

        if (this.ValidationText == null) {
            this.ValidationText = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        }

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.ETD == null) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.ETD");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        if (this.Volume != null && AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.VolumeUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        if (this.GrossWeight != null && AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeightUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        this.ValidationErrorsList = errors;
        isValid = errors.length == 0 ? true : false;
        return isValid;
    }
    private ValidateRules(items: AirlineMessagingRuleList[]) {
        var errors: string[] = [];

        var allKeys = Object.keys(this.EntityPM);
        var list: string[] = [];
        allKeys.forEach(item => {
            list.push(item.toLowerCase());
        });

        items.forEach(item => {
            if (list.indexOf(item.RuleFieldName.toLowerCase()) > -1) {
                var value = this.EntityPM[item.RuleFieldName];
                this.ValidateAirlineRule(item, value, errors);
            }
        });

        return errors;
    }
    private ValidateAirlineRule(myRule: AirlineMessagingRuleList, myFieldValue: any, validationList: string[]) {
        if (myRule != null) {

            var myFieldLabel = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + myRule.RuleFieldName);

            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                }
            }

            else if (typeof (myFieldValue) == "string") {
                if (AppTool.IsNullOrEmpty(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }

                else if (myRule.MaxSize > 0) {
                    if (myFieldValue.length > myRule.MaxSize) {
                        validationList.push("Rule: " + myFieldLabel + " exceeds max size (" + myRule.MaxSize + ")");
                    }
                }
            }

            else if (typeof (myFieldValue) == "number") {
                if (AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }
            }
        }
    }

    // Send
    private SendRequest() {

        this.myFVRWebService.SendFVR(this.AirlineId, this.FromPortId, this.ToPortId, this.ETD, this.ETA, this.Volume, this.GrossWeight, this.VolumeUnitCode, this.GrossWeightUnitCode, this.EntityPM.ShipmentId, this.EntityPM.BookingId, this.myTenantZeroAirlineTTY).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator(); 

            if (myResponse != null) {

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myResult: FVRResultClass = myResponse.Result;

                    if (myResult.IsValid) {
                        this.myRequestId = myResult.RequestId;
                        this.StartTimer();
                        this.RequestSent = true;
                    }

                    else {
                        this.ValidationErrorsList = myResult.Errors;
                    }
                }
            }            
        });               
    }

    // Timer
    private timerToken: any;
    private timerSeconds: number = 1;
    private Retries: number = 0;
    private IsLoading: boolean = false;
    public IsSimulatorEnabled: boolean = false;
    public IsResponseProgressVisible: boolean = false;
    private StopTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        this.IsSimulatorEnabled = false;
        this.IsResponseProgressVisible = false;
    }
    private StartTimer() {
        this.Retries = 0;
        this.timerToken = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
        this.IsSimulatorEnabled = true;
        this.IsResponseProgressVisible = true;
    }
    private IncreaseTimer() {
        clearTimeout(this.timerToken);
        this.timerToken = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
    }
    private AdjustTimerSpeed() {
        if (this.Retries <= 60) {
            if (this.timerSeconds != 1) {
                this.timerSeconds = 1;
                this.IncreaseTimer();
            }
        }

        else if (this.Retries <= 120) {
            if (this.timerSeconds != 5) {
                this.timerSeconds = 5;
                this.IncreaseTimer();
            }
        }

        else if (this.Retries <= 180) {
            if (this.timerSeconds != 60) {
                this.timerSeconds = 60;
                this.IncreaseTimer();
            }
        }

        else {
            this.StopTimer();
        }
    }
    private RunTimerFunction() {
        if (!this.IsLoading) {
            this.Retries++;
            this.LoadData();
            this.AdjustTimerSpeed();
        }
    }

    public IsNoFlightsResult: boolean = false;
    private LoadData() {
        this.IsLoading = true;
        this.IsNoFlightsResult = false;

        if (this.myFlightsSchedulesRequestPMService == null) {
            this.myFlightsSchedulesRequestPMService = new FlightsSchedulesRequestPMService();
        }

        this.myFlightsSchedulesRequestPMService.get(this.myRequestId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var entity: FlightsSchedulesRequestPM = myResponse.Result;

                    if (entity != null) {
                        if (entity.ResponseDate != null) {
                            this.StopTimer();
                            this.BuildObslist(entity);
                        }
                    }
                }
            }

            this.IsLoading = false;
        });
    }
    private BuildObslist(myResult: FlightsSchedulesRequestPM) {
        this.ItemsSource = [];

        var Responses = myResult.Responses.filter(f => f.FromPortId == this.FromPortId).sort((a, b) => { return a.ResultNumber - b.ResultNumber });

        Responses.forEach((item: FlightsSchedulesResponsePM) => {
            var itemsList: FlightsSchedulesResponsePM[] = [];

            if (item.ToPortId == this.ToPortId) {
                itemsList.push(item);
            }

            else {
                var insideResponses = myResult.Responses.filter(f => f.ResultNumber == item.ResultNumber).sort((a, b) => { return a.LineNumber - b.LineNumber });
                insideResponses.forEach(insideItem => {
                    itemsList.push(insideItem);
                });
            }

            var args = new ResponseItemArgs();
            args.Code = "Leg";
            args.Shipment = this.myShipmentPM;
            args.Booking = this.myBookingPM;
            args.Items = itemsList;
            this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, this));
        });

        if (!AppTool.IsNullOrEmpty(myResult.AnswerOSI)) {
            var args = new ResponseItemArgs();
            args.Code = "OSI";
            args.DisplayText = myResult.AnswerOSI;
            this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, this));
        }

        if (!AppTool.IsNullOrEmpty(myResult.AnswerReasonForNoReply)) {
            var args = new ResponseItemArgs();
            args.Code = "NRP";
            args.DisplayText = myResult.AnswerReasonForNoReply;
            this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, this));

            if (myResult.AnswerReasonForNoReply.toUpperCase() == "NO FLIGHTS" || myResult.AnswerReasonForNoReply.toUpperCase() == "FLIGHT NOT FOUND") {
                this.IsNoFlightsResult = true;
            }
        }
    }

    CancelResponseProgressClicked() {
        this.args.IsCancelledFomProgress = true;
        this.StopTimer();
    }

    CloseResponseProgressClicked() {
        this.args.IsClosedFomProgress = true;
        this.Close();
    }

    private Close() {
        this.StopTimer();
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class FlightsSchedulesResponeViewModel {
    private myBookingPM: BookingPM;
    private myShipmentPM: ShipmentPM;
    private legTypeCode: string;
    public IsFlightsLeg: boolean;
    public IsFromOutSide: boolean;
    public LegHeader: string;
    public LegDescription: string;
    public ItemsSource: FlightItemViewModel[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(args: ResponseItemArgs, private fatherComponent: FlightsSchedulesComponent) {
        this.ItemsSource = [];
        this.myBookingPM = args.Booking;
        this.myShipmentPM = args.Shipment;
        this.legTypeCode = args.Code;
        this.LegDescription = args.DisplayText;

        if (args.Booking == null && args.Shipment == null) {
            this.IsFromOutSide = true;
        }

        if (args.Code == "Leg") {
            this.IsFlightsLeg = true;

            args.Items.sort((a, b) => { return a.LineNumber - b.LineNumber }).forEach(item => {
                this.ItemsSource.push(new FlightItemViewModel(item));
            });
        }

        else {
            if (this.legTypeCode == "OSI") {
                this.LegHeader = "Other Service Information: ";
            }

            else if (this.legTypeCode == "NRP") {
                this.LegHeader = "Reason For No Reply: ";
            }
        }
    }

    BookButtonClicked() {
        this.selectedCommandCode = "B";

        if (this.ItemsSource.filter(f => f.MissingPort == true).length > 0) {
            this.CopyMissingPorts();
        }

        else {
            this.Book();
        }
    }
    SelectButtonClicked() {
        this.selectedCommandCode = "S";

        if (this.ItemsSource.filter(f => f.MissingPort == true).length > 0) {
            this.CopyMissingPorts();
        }

        else {
            this.Select();
        }
    }

    // CopyMissingPorts
    private selectedCommandCode: string;
    private CopyMissingPorts() {
        this.CurrentSession.StartBusyIndicator("Copy missing ports..");

        var myResponsesIds: string = null;

        this.ItemsSource.forEach(item => {
            if (myResponsesIds == null) {
                myResponsesIds = item.Id;
            }

            else {
                myResponsesIds += ":" + item.Id;
            }
        });

        if (myResponsesIds.length == 0) {
            this.CurrentSession.StopBusyIndicator();

            if (this.selectedCommandCode == "B") {
                this.Book;
            }

            else {
                this.Select();
            }
        }

        else {
            this.fatherComponent.myFVRWebService.GetCopyFlightsSchedulesPorts(myResponsesIds).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (myResponse != null) {

                    if (myResponse.HasError) {
                        this.fatherComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        var myResult: FlightSchedulePort[] = myResponse.Result;

                        this.fatherComponent.UpdateMissingPorts(myResult);

                        if (this.selectedCommandCode == "B") {
                            this.Book();
                        }

                        else {
                            this.Select();
                        }
                    }
                } 
            });
        }
    }

    private Book() {

    // NewEntityPMService(tablename)

        var newBookingPM: BookingPM = new BookingPM();
        newBookingPM.IsCopyMode = true;
        newBookingPM.Tenant = InfraSettings.TenantPM.Id;
        newBookingPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        newBookingPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newBookingPM.CreatedByUserId = SessionLocator.LoggedUserId;
        newBookingPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newBookingPM.DirectionCode = "E";
        newBookingPM.TransportModeCode = "A";
        newBookingPM.SpaceAllocationCode = "NN";
        newBookingPM.MainCarriageSpaceAllocationCode = "NN";
        newBookingPM.BookingStatusCode = "CRT";
        newBookingPM.BookingStatusName = "Created";
        newBookingPM.FFRStatusCode = "NST";
        newBookingPM.FFRStatusName = "Not Sent";
        newBookingPM.DimensionsUnitCode = InfraSettings.TenantPM.DimensionsUnitCode;
        newBookingPM.VolumeUnitCode = InfraSettings.TenantPM.VolumeUnitCode;
        newBookingPM.GrossWeightUnitCode = InfraSettings.TenantPM.GrossWeightUnitCode;
        newBookingPM.ChargeableWeightUnitCode = InfraSettings.TenantPM.ChargeableWeightUnitCode;
        newBookingPM.CASSCode = InfraSettings.TenantPM.CASSCode;
        newBookingPM.IssuingCarrierIATACode = InfraSettings.TenantPM.IATA;
        newBookingPM.IssuingCarrierAgentId = InfraSettings.TenantPM.AgentId;
        newBookingPM.IssuingCarrierAddressId = InfraSettings.TenantPM.AddressId;
        newBookingPM.Ratio = AppTool.GetRatio(newBookingPM.DirectionCode, newBookingPM.TransportModeCode, null, InfraSettings.TenantPM.CountryCode);
        newBookingPM.DimFactor = AppTool.GetDimFactorFromRatio(newBookingPM.Ratio, newBookingPM.DimensionsUnitCode, newBookingPM.ChargeableWeightUnitCode);

        var myFinalDestinationPortId: string = null;

        for (var i = 0; i < this.ItemsSource.length; i++) {
            var item: FlightItemViewModel = this.ItemsSource[i];

            if (item != null) {

                var itemETD: Date = DateTool.GetDateParts(item.ETD).DateObject;

                switch (i) {
                    case 0:
                        {
                            newBookingPM.MainCarriageETD = itemETD;
                            newBookingPM.MainCarriageCarrierId = item.AirlineId;
                            newBookingPM.MainCarriageCarrierCode = item.AirlineCode;
                            newBookingPM.MainCarriageCarrierName = item.AirlineName;
                            newBookingPM.MainCarriageCarrierNumber = item.FlightNumber;
                            newBookingPM.MainCarriageCarrierPrefix = item.AirlineCode;
                            newBookingPM.MainCarriageFromPortId = item.FromPortId;
                            newBookingPM.MainFromPortCode = item.FromPortCode;
                            newBookingPM.MainFromPortName = item.FromPortName;
                            newBookingPM.MainFromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.MainFromPortCountryName = item.FromPortCountryName;
                            newBookingPM.MainCarriageToPortId = item.ToPortId;
                            newBookingPM.MainToPortCode = item.ToPortCode;
                            newBookingPM.MainToPortName = item.ToPortName;
                            newBookingPM.MainToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.MainToPortCountryName = item.ToPortCountryName;
                            break;
                        }

                    case 1:
                        {
                            newBookingPM.Transshipment1ETD = itemETD;
                            newBookingPM.Transshipment1CarrierId = item.AirlineId;
                            newBookingPM.Transshipment1CarrierName = item.AirlineName;
                            newBookingPM.Transshipment1CarrierNumber = item.FlightNumber;
                            newBookingPM.Transshipment1CarrierPrefix = item.AirlineCode;
                            newBookingPM.Transshipment1FromPortId = item.FromPortId;
                            newBookingPM.Trans1FromPortCode = item.FromPortCode;
                            newBookingPM.Trans1FromPortName = item.FromPortName;
                            newBookingPM.Trans1FromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.Trans1FromPortCountryName = item.FromPortCountryName;
                            newBookingPM.Transshipment1ToPortId = item.ToPortId;
                            newBookingPM.Trans1ToPortCode = item.ToPortCode;
                            newBookingPM.Trans1ToPortName = item.ToPortName;
                            newBookingPM.Trans1ToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.Trans1ToPortCountryName = item.ToPortCountryName;
                            break;
                        }

                    case 2:
                        {
                            newBookingPM.Transshipment2ETD = itemETD;
                            newBookingPM.Transshipment2CarrierId = item.AirlineId;
                            newBookingPM.Transshipment2CarrierName = item.AirlineName;
                            newBookingPM.Transshipment2CarrierNumber = item.FlightNumber;
                            newBookingPM.Transshipment2CarrierPrefix = item.AirlineCode;
                            newBookingPM.Transshipment2FromPortId = item.FromPortId;
                            newBookingPM.Trans2FromPortCode = item.FromPortCode;
                            newBookingPM.Trans2FromPortName = item.FromPortName;
                            newBookingPM.Trans2FromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.Trans2FromPortCountryName = item.FromPortCountryName;
                            newBookingPM.Transshipment2ToPortId = item.ToPortId;
                            newBookingPM.Trans2ToPortCode = item.ToPortCode;
                            newBookingPM.Trans2ToPortName = item.ToPortName;
                            newBookingPM.Trans2ToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.Trans2ToPortCountryName = item.ToPortCountryName;
                            break;
                        }
                }

                myFinalDestinationPortId = item.ToPortId;
            }
        }

        newBookingPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;

        var windowTitle = "New Booking Wizard";
        var windowArgs: BookingWizardArgs = new BookingWizardArgs();

        windowArgs.IsNewEntity = true;
        windowArgs.IsBuiltFromSchedule = true;
        windowArgs.EntityPM = newBookingPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    }
    private Select() {

        var isMainLeg = this.fatherComponent.args.IsMainLeg;

        var myFinalDestinationPortId: string = null;

        if (this.myShipmentPM != null) {

            if (isMainLeg) {
                this.myShipmentPM.MainCarriageETD = null;
                this.myShipmentPM.MainCarriageCarrierNumber = null;
                this.myShipmentPM.MainCarriageFromPortId = null;
                this.myShipmentPM.MainCarriageFromPortCode = null;
                this.myShipmentPM.MainCarriageFromPortName = null;
                this.myShipmentPM.MainCarriageFromPortCountryCode = null;
                this.myShipmentPM.MainCarriageFromPortCountryName = null;
            }

            this.myShipmentPM.MainCarriageToPortId = null;
            this.myShipmentPM.MainCarriageToPortCode = null;
            this.myShipmentPM.MainCarriageToPortName = null;
            this.myShipmentPM.MainCarriageToPortCountryCode = null;
            this.myShipmentPM.MainCarriageToPortCountryName = null;
            //**********

            this.myShipmentPM.Transshipment1ETD = null;
            this.myShipmentPM.Transshipment1CarrierId = null;
            this.myShipmentPM.Transshipment1CarrierCode = null;
            this.myShipmentPM.Transshipment1CarrierName = null;
            this.myShipmentPM.Transshipment1CarrierNumber = null;
            this.myShipmentPM.Transshipment1CarrierPrefix = null;
            this.myShipmentPM.Transshipment1FromPortId = null;
            this.myShipmentPM.Transshipment1FromPortCode = null;
            this.myShipmentPM.Transshipment1FromPortName = null;
            this.myShipmentPM.Transshipment1FromPortCountryCode = null;
            this.myShipmentPM.Transshipment1FromPortCountryName = null;
            this.myShipmentPM.Transshipment1ToPortId = null;
            this.myShipmentPM.Transshipment1ToPortCode = null;
            this.myShipmentPM.Transshipment1ToPortName = null;
            this.myShipmentPM.Transshipment1ToPortCountryCode = null;
            this.myShipmentPM.Transshipment1ToPortCountryName = null;
            //**********
            this.myShipmentPM.Transshipment2ETD = null;
            this.myShipmentPM.Transshipment2CarrierId = null;
            this.myShipmentPM.Transshipment2CarrierCode = null;
            this.myShipmentPM.Transshipment2CarrierName = null;
            this.myShipmentPM.Transshipment2CarrierNumber = null;
            this.myShipmentPM.Transshipment2CarrierPrefix = null;
            this.myShipmentPM.Transshipment2FromPortId = null;
            this.myShipmentPM.Transshipment2FromPortCode = null;
            this.myShipmentPM.Transshipment2FromPortName = null;
            this.myShipmentPM.Transshipment2FromPortCountryCode = null;
            this.myShipmentPM.Transshipment2FromPortCountryName = null;
            this.myShipmentPM.Transshipment2ToPortId = null;
            this.myShipmentPM.Transshipment2ToPortCode = null;
            this.myShipmentPM.Transshipment2ToPortName = null;
            this.myShipmentPM.Transshipment2ToPortCountryCode = null;
            this.myShipmentPM.Transshipment2ToPortCountryName = null;
        }

        else if (this.myBookingPM != null) {

            if (isMainLeg) {
                this.myBookingPM.MainCarriageETD = null;
                this.myBookingPM.MainCarriageCarrierNumber = null;
                this.myBookingPM.MainCarriageFromPortId = null;
                this.myBookingPM.MainFromPortCode = null;
                this.myBookingPM.MainFromPortName = null;
                this.myBookingPM.MainFromPortCountryCode = null;
                this.myBookingPM.MainFromPortCountryName = null;
            }

            this.myBookingPM.MainCarriageToPortId = null;
            this.myBookingPM.MainToPortCode = null;
            this.myBookingPM.MainToPortName = null;
            this.myBookingPM.MainToPortCountryCode = null;
            this.myBookingPM.MainToPortCountryName = null;
            //**********

            this.myBookingPM.Transshipment1ETD = null;
            this.myBookingPM.Transshipment1CarrierId = null;
            this.myBookingPM.Transshipment1CarrierName = null;
            this.myBookingPM.Transshipment1CarrierNumber = null;
            this.myBookingPM.Transshipment1CarrierPrefix = null;
            this.myBookingPM.Transshipment1FromPortId = null;
            this.myBookingPM.Trans1FromPortCode = null;
            this.myBookingPM.Trans1FromPortName = null;
            this.myBookingPM.Trans1FromPortCountryCode = null;
            this.myBookingPM.Trans1FromPortCountryName = null;
            this.myBookingPM.Transshipment1ToPortId = null;
            this.myBookingPM.Trans1ToPortCode = null;
            this.myBookingPM.Trans1ToPortName = null;
            this.myBookingPM.Trans1ToPortCountryCode = null;
            this.myBookingPM.Trans1ToPortCountryName = null;
            //**********
            this.myBookingPM.Transshipment2ETD = null;
            this.myBookingPM.Transshipment2CarrierId = null;
            this.myBookingPM.Transshipment2CarrierName = null;
            this.myBookingPM.Transshipment2CarrierNumber = null;
            this.myBookingPM.Transshipment2CarrierPrefix = null;
            this.myBookingPM.Transshipment2FromPortId = null;
            this.myBookingPM.Trans2FromPortCode = null;
            this.myBookingPM.Trans2FromPortName = null;
            this.myBookingPM.Trans2FromPortCountryCode = null;
            this.myBookingPM.Trans2FromPortCountryName = null;
            this.myBookingPM.Transshipment2ToPortId = null;
            this.myBookingPM.Trans2ToPortCode = null;
            this.myBookingPM.Trans2ToPortName = null;
            this.myBookingPM.Trans2ToPortCountryCode = null;
            this.myBookingPM.Trans2ToPortCountryName = null;
        }

        for (var i = 0; i < this.ItemsSource.length; i++) {
            var item: FlightItemViewModel = this.ItemsSource[i];
            if (item != null) {
                switch (i) {
                    case 0: {

                        if (isMainLeg) {
                            this.MapLeg_MN(item);
                        }

                        else {
                            if (this.myShipmentPM != null) {
                                this.myShipmentPM.MainCarriageToPortId = item.FromPortId;
                                this.myShipmentPM.MainCarriageToPortCode = item.FromPortCode;
                                this.myShipmentPM.MainCarriageToPortName = item.FromPortName;
                                this.myShipmentPM.MainCarriageToPortCountryCode = item.FromPortCountryCode;
                                this.myShipmentPM.MainCarriageToPortCountryName = item.FromPortCountryName;
                            }

                            else if (this.myBookingPM != null) {
                                this.myBookingPM.MainCarriageToPortId = item.FromPortId;
                                this.myBookingPM.MainToPortCode = item.FromPortCode;
                                this.myBookingPM.MainToPortName = item.FromPortName;
                                this.myBookingPM.MainToPortCountryCode = item.FromPortCountryCode;
                                this.myBookingPM.MainToPortCountryName = item.FromPortCountryName;
                            }

                            this.MapLeg_T1(item);
                        }

                        break;
                    }

                    case 1: {

                        if (isMainLeg) {
                            this.MapLeg_T1(item);
                        }

                        else {
                            this.MapLeg_T2(item);
                        }

                        break;
                    }

                    case 2: {

                        if (isMainLeg) {
                            this.MapLeg_T2(item);
                        }

                        else {
                            //this.MapLeg_T3(item);
                        }

                        break;
                    }
                }

                myFinalDestinationPortId = item.ToPortId;
            }
        }

        if (this.myShipmentPM != null) {
            this.myShipmentPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
        }

        else if (this.myBookingPM != null) {
            this.myBookingPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
        }

        if (this.fatherComponent != null) {
            this.fatherComponent.OnFlightSelected();
        }
    }

    private MapLeg_MN(item: FlightItemViewModel) {
        var itemETD: Date = DateTool.GetDateParts(item.ETD).DateObject;

        if (this.myShipmentPM != null) {
            this.myShipmentPM.MainCarriageETD = itemETD;
            this.myShipmentPM.MainCarriageCarrierNumber = item.FlightNumber;
            this.myShipmentPM.MainCarriageFromPortId = item.FromPortId;
            this.myShipmentPM.MainCarriageFromPortCode = item.FromPortCode;
            this.myShipmentPM.MainCarriageFromPortName = item.FromPortName;
            this.myShipmentPM.MainCarriageFromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.MainCarriageFromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.MainCarriageToPortId = item.ToPortId;
            this.myShipmentPM.MainCarriageToPortCode = item.ToPortCode;
            this.myShipmentPM.MainCarriageToPortName = item.ToPortName;
            this.myShipmentPM.MainCarriageToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.MainCarriageToPortCountryName = item.ToPortCountryName;
        }

        else if (this.myBookingPM != null) {
            this.myBookingPM.MainCarriageETD = itemETD;
            this.myBookingPM.MainCarriageCarrierNumber = item.FlightNumber;
            this.myBookingPM.MainCarriageFromPortId = item.FromPortId;
            this.myBookingPM.MainFromPortCode = item.FromPortCode;
            this.myBookingPM.MainFromPortName = item.FromPortName;
            this.myBookingPM.MainFromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.MainFromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.MainCarriageToPortId = item.ToPortId;
            this.myBookingPM.MainToPortCode = item.ToPortCode;
            this.myBookingPM.MainToPortName = item.ToPortName;
            this.myBookingPM.MainToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.MainToPortCountryName = item.ToPortCountryName;
        }
    }
    private MapLeg_T1(item: FlightItemViewModel) {
        var itemETD: Date = DateTool.GetDateParts(item.ETD).DateObject;

        if (this.myShipmentPM != null) {
            this.myShipmentPM.Transshipment1ETD = DateTool.GetDateParts(itemETD).DateObject;
            this.myShipmentPM.Transshipment1CarrierId = item.AirlineId;
            this.myShipmentPM.Transshipment1CarrierCode = item.AirlineCode;
            this.myShipmentPM.Transshipment1CarrierName = item.AirlineName;
            this.myShipmentPM.Transshipment1CarrierNumber = item.FlightNumber;
            this.myShipmentPM.Transshipment1CarrierPrefix = item.AirlineCode;
            this.myShipmentPM.Transshipment1FromPortId = item.FromPortId;
            this.myShipmentPM.Transshipment1FromPortCode = item.FromPortCode;
            this.myShipmentPM.Transshipment1FromPortName = item.FromPortName;
            this.myShipmentPM.Transshipment1FromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.Transshipment1FromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.Transshipment1ToPortId = item.ToPortId;
            this.myShipmentPM.Transshipment1ToPortCode = item.ToPortCode;
            this.myShipmentPM.Transshipment1ToPortName = item.ToPortName;
            this.myShipmentPM.Transshipment1ToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.Transshipment1ToPortCountryName = item.ToPortCountryName;
        }

        else if (this.myBookingPM != null) {
            this.myBookingPM.Transshipment1ETD = itemETD;
            this.myBookingPM.Transshipment1CarrierId = item.AirlineId;
            this.myBookingPM.Transshipment1CarrierName = item.AirlineName;
            this.myBookingPM.Transshipment1CarrierNumber = item.FlightNumber;
            this.myBookingPM.Transshipment1CarrierPrefix = item.AirlineCode;
            this.myBookingPM.Transshipment1FromPortId = item.FromPortId;
            this.myBookingPM.Trans1FromPortCode = item.FromPortCode;
            this.myBookingPM.Trans1FromPortName = item.FromPortName;
            this.myBookingPM.Trans1FromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.Trans1FromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.Transshipment1ToPortId = item.ToPortId;
            this.myBookingPM.Trans1ToPortCode = item.ToPortCode;
            this.myBookingPM.Trans1ToPortName = item.ToPortName;
            this.myBookingPM.Trans1ToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.Trans1ToPortCountryName = item.ToPortCountryName;
        }
    }
    private MapLeg_T2(item: FlightItemViewModel) {
        var itemETD: Date = DateTool.GetDateParts(item.ETD).DateObject;

        if (this.myShipmentPM != null) {
            this.myShipmentPM.Transshipment2ETD = itemETD;
            this.myShipmentPM.Transshipment2CarrierId = item.AirlineId;
            this.myShipmentPM.Transshipment2CarrierCode = item.AirlineCode;
            this.myShipmentPM.Transshipment2CarrierName = item.AirlineName;
            this.myShipmentPM.Transshipment2CarrierNumber = item.FlightNumber;
            this.myShipmentPM.Transshipment2CarrierPrefix = item.AirlineCode;
            this.myShipmentPM.Transshipment2FromPortId = item.FromPortId;
            this.myShipmentPM.Transshipment2FromPortCode = item.FromPortCode;
            this.myShipmentPM.Transshipment2FromPortName = item.FromPortName;
            this.myShipmentPM.Transshipment2FromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.Transshipment2FromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.Transshipment2ToPortId = item.ToPortId;
            this.myShipmentPM.Transshipment2ToPortCode = item.ToPortCode;
            this.myShipmentPM.Transshipment2ToPortName = item.ToPortName;
            this.myShipmentPM.Transshipment2ToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.Transshipment2ToPortCountryName = item.ToPortCountryName;
        }

        else if (this.myBookingPM != null) {
            this.myBookingPM.Transshipment2ETD = itemETD;
            this.myBookingPM.Transshipment2CarrierId = item.AirlineId;
            this.myBookingPM.Transshipment2CarrierName = item.AirlineName;
            this.myBookingPM.Transshipment2CarrierNumber = item.FlightNumber;
            this.myBookingPM.Transshipment2CarrierPrefix = item.AirlineCode;
            this.myBookingPM.Transshipment2FromPortId = item.FromPortId;
            this.myBookingPM.Trans2FromPortCode = item.FromPortCode;
            this.myBookingPM.Trans2FromPortName = item.FromPortName;
            this.myBookingPM.Trans2FromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.Trans2FromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.Transshipment2ToPortId = item.ToPortId;
            this.myBookingPM.Trans2ToPortCode = item.ToPortCode;
            this.myBookingPM.Trans2ToPortName = item.ToPortName;
            this.myBookingPM.Trans2ToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.Trans2ToPortCountryName = item.ToPortCountryName;
        }
    }
    private MapLeg_T3(item: FlightItemViewModel) {

    }
}

export class FlightItemViewModel {
    constructor(private entityPM: FlightsSchedulesResponsePM) {

    }

    get Id() { return this.entityPM.Id; }
    get LineNumber() { return this.entityPM.LineNumber; }
    get AirlineId() { return this.entityPM.AirlineId; }
    get AirlineCode() { return this.entityPM.AirlineCode; }
    get AirlineName() { return this.entityPM.AirlineName; }
    get AirplaneType() { return this.entityPM.AirplaneType; }
    get FlightNumber() { return this.entityPM.FlightNumber; }
    get Carrier() { return this.AirlineCode + " " + this.FlightNumber; }
    get NumberOfStops() { return this.entityPM.NumberOfStops; }
    get FromPortId() { return this.entityPM.FromPortId; }
    get FromPortCode() { return this.entityPM.FromPortCode; }
    get FromPortName() { return this.entityPM.FromPortName; }
    get FromPortCountryCode() { return this.entityPM.FromPortCountryCode; }
    get FromPortCountryName() { return this.entityPM.FromPortCountryName; }
    get ToPortId() { return this.entityPM.ToPortId; }
    get ToPortCode() { return this.entityPM.ToPortCode; }
    get ToPortName() { return this.entityPM.ToPortName; }
    get ToPortCountryCode() { return this.entityPM.ToPortCountryCode; }
    get ToPortCountryName() { return this.entityPM.ToPortCountryName; }
    get MissingPort() { return this.entityPM.MissingPort; }
    get ETD() { return this.entityPM.ETD; }
    get ETA() { return this.entityPM.ETA; }
    get IsLineVisible() {
        var myResult = false;

        if (this.LineNumber > 0) {
            myResult = true;
        }

        return myResult;
    }

    public UpdateMissingPorts(item: FlightSchedulePort) {
        if (this.entityPM.FromPortId == null) {
            if (this.entityPM.FromPortCode == item.PortCode) {
                this.entityPM.FromPortId = item.PortId;
                this.entityPM.FromPortCode = item.PortCode;
                this.entityPM.FromPortName = item.PortName;
                this.entityPM.FromPortCountryCode = item.PortCountryCode;
                this.entityPM.FromPortCountryName = item.PortCountryName;
            }
        }

        if (this.entityPM.ToPortId == null) {
            if (this.entityPM.ToPortCode == item.PortCode) {
                this.entityPM.ToPortId = item.PortId;
                this.entityPM.ToPortCode = item.PortCode;
                this.entityPM.ToPortName = item.PortName;
                this.entityPM.ToPortCountryCode = item.PortCountryCode;
                this.entityPM.ToPortCountryName = item.PortCountryName;
            }
        }

        if (this.entityPM.FromPortId != null && this.entityPM.ToPortId != null) {
            this.entityPM.MissingPort = false;
        }
    }
}

class ResponseItemArgs {
    public Code: string;
    public Booking: BookingPM;
    public Shipment: ShipmentPM;
    public DisplayText: string;
    public Items: FlightsSchedulesResponsePM[]
}
