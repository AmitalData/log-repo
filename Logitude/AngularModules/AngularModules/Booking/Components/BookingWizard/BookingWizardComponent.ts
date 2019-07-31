import {Component, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import {AWBUtilities, AWBFFRValidator} from '../../Utilities/AWBUtilities';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {BookingPM} from '../../EntityPMs/BookingPM';
import {BookingAnswerPM} from '../../EntityPMs/BookingAnswerPM';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {BookingPMService} from '../../Services/StandardPMs/BookingPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../Common/Services/StandardLists/AirlineListService';
import {StateListService} from '../../../Common/Services/StandardLists/StateListService';
import {PartnersDomainService, AirlineMessagingRuleList} from '../../../Common/Services/PartnersDomainService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../Common/EntityLists/AirlineList';
import {StateList} from '../../../Common/EntityLists/StateList';
import {AirlinePM} from '../../../Common/EntityPMs/AirlinePM';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {FFRWebService, FFRResult} from '../../../Infrastructure/Services/WebServices/FFRWebService';
import {BookingTool} from '../../Tools';
import {BookingWizardArgs} from '../../Args';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardArgs} from '../../../Shipment/Args';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {BookingDomainService, BookingValidatorResultClass} from '../../Services/BookingDomainService';
import {FSRWebService, FSRResultClass} from '../../../Infrastructure/Services/WebServices/FSRWebService';
import {InfrastructureDomainService} from '../../../Infrastructure/Services/InfrastructureDomainService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'BookingWizardComponent',
    moduleId: module.id,
    templateUrl: './BookingWizardComponent.html',
    providers: [EntityArgs]
})

export class BookingWizardComponent implements AfterViewInit {
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    public TenantPM: TenantPM;
    public EntityPM: BookingPM;
    public DataContext: BookingWizardComponent = this;
    public WindowArgs: BookingWizardArgs;
    public IsNewEntity: boolean = false;
    public ObjectTableName: string;
    public IsSendResponseVisible: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private myFFRWebService: FFRWebService;
    private myPartnersDomainService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.TenantPM = InfraSettings.TenantPM;
        this.myPartnersDomainService = new PartnersDomainService();

        if (FeatureLocator.HasFeaturePermession("Booking", "BOOKINGSENDRESPONSE")) {
            this.IsSendResponseVisible = true;
        }
    }

    SetWindowArgs(windowArgs: BookingWizardArgs) {
        this.WindowArgs = windowArgs;
        this.InitializeWizard();
        this.RunComponent();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        //this.isViewInited = true;
        //this.InitializeComponent();
    }

    RunComponent() {


        ServiceLocator.SendTotangoUserActivity("Booking", "Booking Wizard");
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private InitializeWizard() {
        if (this.WindowArgs != null) {

            this.IsNewEntity = this.WindowArgs.IsNewEntity;
            this.ObjectTableName = "Booking";

            if (this.IsNewEntity) {
                this.CreateBooking();
            }

            else {
                this.EntityPM = this.WindowArgs.EntityPM;
            }

            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = this.ObjectTableName;

            this.SetSelectedTab();
            this.SetButtonsProperties();
        }
    }

    public SendFFRContent: string = "";
    public IsCopyButtonEnabled: boolean = false;
    public IsCancelButtonEnabled: boolean = false;
    public IsReactivateButtonEnabled: boolean = false;
    public IsSendCancellationEnabled: boolean = false;
    public IsSaveButtonEnabled: boolean = false;
    public IsBuildShipmentEnabled: boolean = false;
    public IsSendResponseEnabled: boolean = false;
    public IsSendToAirlineTenantVisible: boolean = false;
    public SetButtonsProperties() {
        var isCopyEnabled = true;
        var isCancelEnabled = true;
        var isReacivateEnabled = true;
        var isSendCancellationEnabled = true;
        var isSaveEnabled = true;
        var isBuildShipmentEnabled = false;
        var isSendResponseEnabled = true;

        if (this.EntityPM.IsCancelled) {
            isCopyEnabled = false;
            isSendResponseEnabled = false;
        }
        
        if (this.EntityPM.BookingStatusCode == "CRT"
            || this.EntityPM.BookingStatusCode == "AWB"
            || this.EntityPM.IsCancelled
            || (this.EntityPM.WaitingForResponse && this.EntityPM.FFRStatusCode == "CRS")
            || this.EntityPM.FFRStatusCode == "RBC") {
            isSendCancellationEnabled = false;
        }

        if (this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "AWB" || this.EntityPM.IsCancelled) {
            isCancelEnabled = false;
        }

        if (this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "AWB" || !this.EntityPM.IsCancelled) {
            isReacivateEnabled = false;
        }

        if (this.EntityPM.BookingStatusCode == "AWB" || this.EntityPM.IsCancelled || this.EntityPM.BookingStatusCode == "CNF" || (this.EntityPM.WaitingForResponse && this.EntityPM.FFRStatusCode == "BRQ")) {
            isSaveEnabled = false;
        }

        if (this.EntityPM.BookingStatusCode == "CRT") {
            this.SendFFRContent = "Request Booking";
        }
        else {
            this.SendFFRContent = "Update Booking";
        }

        if (this.CurrentSession.CurrentWindow != null) {
            this.CurrentSession.CurrentWindow.ShowCancelControl(this.EntityPM.IsCancelled);
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if ((this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "WCF") && !this.EntityPM.IsCancelled) {
                isBuildShipmentEnabled = true;
            }
        }

        if (FeatureLocator.HasFeaturePermession("Booking", "Booking.Action.SendToAirlineTenant")) {
            this.IsSendToAirlineTenantVisible = true;
        }

        this.IsCopyButtonEnabled = isCopyEnabled;
        this.IsCancelButtonEnabled = isCancelEnabled;
        this.IsReactivateButtonEnabled = isReacivateEnabled;
        this.IsSendCancellationEnabled = isSendCancellationEnabled;
        this.IsSaveButtonEnabled = isSaveEnabled;
        this.IsBuildShipmentEnabled = isBuildShipmentEnabled;
        this.IsSendResponseEnabled = isSendResponseEnabled;
    }

    get IsSaveButtonVisible() {
        var myResult = false;

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            myResult = true;
        }

        else if (this.EntityPM.BookingStatusCode == "CRT" && this.EntityPM.IsDirty) {
            myResult = true;
        }

        return myResult;
    }

    get IsUpdateBookingVisible() {
        var myResult: boolean = false;

        if (!this.EntityPM.IsCancelled) {
            if (this.EntityPM.BookingStatusCode != "CRT" && this.EntityPM.IsDirty) {
                myResult = true;
            }
        }

        return myResult;
    }

    get IsSendFFREnabled() {
        var myResult = false;

        if (this.EntityPM.BookingStatusCode == "CRT") {
            myResult = true;
        }

        else if (this.EntityPM.BookingStatusCode == "AWB") {
            myResult = false;
        }

        else {
            if (this.EntityPM.IsDirty) {
                myResult = true;
            }
        }

        return myResult;
    }
    
    private InitializeComponent() {
        if (this.WindowArgs != null && this.isViewInited) {

            this.SelectionChanged();
            this.ValidateAllTabs();
            this.LoadAirlineRules(this.EntityPM.MainCarriageCarrierCode);
            this.LoadAllowedAirline();
            this.LoadAllStates();
        }
    }

    public AllStates: StateList[] = [];
    private myStateListService: StateListService;
    private LoadAllStates() {
        if (this.myStateListService == null) {
            this.myStateListService = new StateListService();            
        }

        this.myStateListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllStates = myResponse.Result;
                this.ValidateScreen_PAR();
            }
        });
    }

    private CreateBooking() {
        var todayDate = DateTool.GetCurrentDateTimeAsUtc();

        this.EntityPM = new BookingPM();

        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionInfo.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionInfo.LoggedUserId;
        this.EntityPM.DirectionCode = "E";
        this.EntityPM.TransportModeCode = "A";
        this.EntityPM.SpaceAllocationCode = "NN";
        this.EntityPM.MainCarriageSpaceAllocationCode = "NN";
        this.EntityPM.BookingStatusCode = "CRT";
        this.EntityPM.BookingStatusName = "Created";
        this.EntityPM.FFRStatusCode = "NST";
        this.EntityPM.FFRStatusName = "Not Sent";
        this.EntityPM.DimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        this.EntityPM.VolumeUnitCode = this.TenantPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        this.EntityPM.ChargeableWeightUnitCode = this.TenantPM.ChargeableWeightUnitCode;
        
        if (this.WindowArgs.IsCopyFromBooking || this.WindowArgs.IsBuiltFromSchedule) {
            var oldBooking: BookingPM = this.WindowArgs.EntityPM;

            BookingTool.CopyBooking(this.EntityPM, oldBooking);
            BookingTool.CopyBookingPackages(this.EntityPM, oldBooking);
        }

        else {
            this.EntityPM.CASSCode = this.TenantPM.CASSCode;
            this.EntityPM.IssuingCarrierIATACode = this.TenantPM.IATA;
            this.EntityPM.IssuingCarrierAgentId = this.TenantPM.AgentId;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAgentId)) {
                var myService: CardListService = new CardListService();
                myService.getSingle(this.EntityPM.IssuingCarrierAgentId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        if (list != null) {
                            this.EntityPM.IssuingCarrierAddressId = list.MainAddressId;
                        }

                        this.ValidateScreen_PAR();
                    }
                });
            }
        }
    }
    
    public IsTabVisible_OVE: boolean = false;
    private SetSelectedTab() {

        if (this.IsNewEntity) {
            this.selectedTabCode = "BKD";
        }

        else {
            this.IsTabVisible_OVE = true;
            this.selectedTabCode = "OVE";
        }
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    private PageChild_OVE: any = null;
    public PageChild_BKD: any = null;
    public PageChild_PAR: any = null;
    public PageChild_PAC: any = null;
    public PageChild_GEN: any = null; 
    private PageChild_EVE: any = null;    
    SelectionChanged() {
        if (this.isViewInited) {
            if (this.SelectedTabCode != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                if (myLocation != null) {

                    switch (this.SelectedTabCode) {

                        case "OVE": {
                            if (this.PageChild_OVE == null) {
                                SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Overview/OverviewTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_OVE = cmpRef.instance;
                                        this.PageChild_OVE.InitTab(this);

                                        if (this.runOverview) {
                                            this.PageChild_OVE.SetDataAfterSending();
                                        }
                                    });
                            }

                            else {
                                this.PageChild_OVE.RefreshTab();

                                if (this.runOverview) {
                                    this.PageChild_OVE.SetDataAfterSending();
                                }
                            }

                            break;
                        }

                        case "BKD": {
                            if (this.PageChild_BKD == null) {
                                SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/BookingDetails/BookingDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_BKD = cmpRef.instance;
                                        this.PageChild_BKD.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_BKD.RefreshTab();
                            }

                            break;
                        }

                        case 'PAR': {
                            if (this.PageChild_PAR == null) {
                                SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Partners/PartnersTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_PAR = cmpRef.instance;
                                        this.PageChild_PAR.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_PAR.RefreshTab();
                            }

                            break;
                        }

                        case "PAC": {
                            if (this.PageChild_PAC == null) {
                                SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Packages/PackagesTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_PAC = cmpRef.instance;
                                        this.PageChild_PAC.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_PAC.RefreshTab();
                            }

                            break;
                        }

                        case "GEN": {
                            if (this.PageChild_GEN == null) {
                                SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/GeneralDetails/GeneralDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_GEN = cmpRef.instance;
                                        this.PageChild_GEN.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_GEN.RefreshTab();
                            }

                            break;
                        }

                        case "EVE": {
                            if (this.PageChild_EVE == null) {
                                SessionLocator.DynamicLoader.Load('./Common/Components/Events/EventsTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_EVE = cmpRef.instance;
                                    });
                            }

                            else {
                                this.PageChild_EVE.LoadData();
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

    OverviewDirectiveLoaded() {
        if (this.SelectedTabCode == "OVE") {
            if (this.PageChild_OVE == null) {

                this.SelectionChanged();
            }
        }
    }

    // Allowed Airline
    private LoadAllowedAirline() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.TransportModeCode == "A") {
            if (this.WindowArgs.IsCopyFromBooking || this.WindowArgs.IsBuiltFromSchedule) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    this.GetMainCarriageCarrier();
                }
            }

            else if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
                this.myPartnersDomainService.GetAllowedAirlineId().subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            var allowedAirlineId: string = myResponse.Result;
                            if (!AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                this.EntityPM.MainCarriageCarrierId = allowedAirlineId;
                                this.GetMainCarriageCarrier();
                            }
                        }
                    }
                });
            }            
        }
    }
    private GetMainCarriageCarrier() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            this.EntityPM.AirlinePrefix = null;
            this.EntityPM.Master = null;
            this.EntityPM.AccountNumber = null;
            this.EntityPM.MainCarriageCarrierPrefix = null;
            this.EntityPM.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.LongMaster = null;
            this.EntityPM.TenantZeroAirlineId = null;
            this.EntityPM.TenantZeroAirlineTTY = null;
            this.EntityPM.TenantZeroAirlinePIMA = null;
            this.EntityPM.TenantZeroAirlineChampFFR = false;
            this.EntityPM.TenantZeroAirlineGLSHKFFR = false;
            this.EntityPM.TenantZeroAirlineChampFVR = false;
            this.EntityPM.TenantZeroAirlineGLSHKFVR = false;
            this.EntityPM.ZeroChampNeedsRegistration = false;
            this.EntityPM.ZeroGLSHKNeedsRegistration = false;
            this.EntityPM.TenantZeroIsManagingProduct = false;
            this.EntityPM.TenantZeroIsProductMandatory = false;
            this.EntityPM.ZeroIsDescOfGoodsFromList = false;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;

            this.OnLoadingAllowedAirlineCompleted();
        }

        else {
            this.GetCarrier();
            this.GetAirline();
        }
    }
    private GetCarrier() {
        var myService: CardListService = new CardListService();

        myService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CardList = myResponse.Result;

                if (list != null) {
                    this.EntityPM.MainCarriageCarrierPrefix = list.Code;
                    this.EntityPM.AccountNumber = list.AirlineAccountNumber;
                    this.EntityPM.MainCarriageCarrierCode = list.Code;
                    this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                }

                this.OnLoadingAllowedAirlineCompleted();
            }
        });
    }
    private GetAirline() {
        var myService: AirlineListService = new AirlineListService();

        myService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: AirlineList = myResponse.Result;

                if (list != null) {
                    var myPrefix: string = null;

                    if (!AppTool.IsNullOrEmpty(list.Prefix)) {
                        myPrefix = list.Prefix.toString().trim();
                        myPrefix = AppTool.PadLeft(myPrefix, 3, "0");
                    }

                    this.EntityPM.AirlinePrefix = myPrefix;
                    this.EntityPM.LongMaster = myPrefix + "-";
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
                        this.EntityPM.LongMaster = this.EntityPM.LongMaster + this.EntityPM.Master;
                    }

                    this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                    this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;
                    this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                    this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;

                    this.GetTenantZeroAirline(list.Code);
                }
            }
        });
    }
    private GetTenantZeroAirline(myCode: string) {

        this.myPartnersDomainService.GetAirlineByCode(myCode, 0).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var airlinePM: AirlinePM = myResponse.Result;
                if (airlinePM != null) {
                    this.EntityPM.TenantZeroAirlineId = airlinePM.Id;
                    this.EntityPM.TenantZeroAirlineTTY = airlinePM.TTY;
                    this.EntityPM.TenantZeroAirlinePIMA = airlinePM.GLSHKPIMA;
                    this.EntityPM.TenantZeroAirlineChampFFR = airlinePM.ChampFFRFFA;
                    this.EntityPM.TenantZeroAirlineGLSHKFFR = airlinePM.GLSHKFFRFFA;
                    this.EntityPM.TenantZeroAirlineChampFVR = airlinePM.ChampFVRFVA;
                    this.EntityPM.TenantZeroAirlineGLSHKFVR = airlinePM.GLSHKFVRFVA;
                    this.EntityPM.ZeroChampNeedsRegistration = airlinePM.ChampNeedsRegistration;
                    this.EntityPM.ZeroGLSHKNeedsRegistration = airlinePM.GLSHKNeedsRegistration;
                    this.EntityPM.TenantZeroIsManagingProduct = airlinePM.IsManagingProduct;
                    this.EntityPM.TenantZeroIsProductMandatory = airlinePM.IsProductMandatory;
                    this.EntityPM.ZeroIsDescOfGoodsFromList = airlinePM.IsDescriptionOfGoodsFromList;
                }

                this.OnLoadingAllowedAirlineCompleted();
            }
        });
    }
    private OnLoadingAllowedAirlineCompleted() {
        this.ValidateScreen_BKD();
        this.PageChild_BKD.RefreshTab();
    }

    public AirlineRulesList: AirlineMessagingRuleList[] = [];
    LoadAirlineRules(myAirlineCode: string) {

        if (AppTool.IsNullOrEmpty(myAirlineCode)) {
            this.AirlineRulesList = [];
            this.ValidateAllTabs();
            this.RefreshTab(this.SelectedTabCode);
        }

        else {
            this.myPartnersDomainService.GetAirlineRules(myAirlineCode, "FFR").subscribe(myResult => {
                if (myResult == null) {
                    this.AirlineRulesList = [];
                    this.ValidateAllTabs();
                    this.RefreshTab(this.SelectedTabCode);
                }

                else {
                    var myResponse: ServiceResponse = myResult;
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.AirlineRulesList = myResponse.Result;
                        this.ValidateAllTabs();
                        this.RefreshTab(this.SelectedTabCode);
                    }
                }
            });
        }
    }

    private RefreshTab(tabCode: string) {
        switch (tabCode) {

            case "OVE": {
                if (this.PageChild_OVE != null) {
                    this.PageChild_OVE.RefreshTab();
                }

                break;
            }

            case "BKD": {
                if (this.PageChild_BKD != null) {
                    this.PageChild_BKD.RefreshTab();
                }

                break;
            }

            case "PAR": {
                if (this.PageChild_PAR != null) {
                    this.PageChild_PAR.RefreshTab();
                }

                break;
            }

            case "PAC": {
                if (this.PageChild_PAC != null) {
                    this.PageChild_PAC.RefreshTab();
                }

                break;
            }

            case "GEN": {
                if (this.PageChild_GEN != null) {
                    this.PageChild_GEN.RefreshTab();
                }

                break;
            }

            case "EVE": {
                if (this.PageChild_EVE != null) {
                    this.PageChild_EVE.LoadData();
                }

                break;
            }
        }
    }

    public ValidationText: string;
    public TabErrors_BKD: string[] = [];
    public TabErrors_PAR: string[] = [];
    public TabErrors_PAC: string[] = [];
    public TabErrors_GEN: string[] = [];
    public TabWarnings_BKD: string[] = [];
    public TabWarnings_PAR: string[] = [];
    public TabWarnings_PAC: string[] = [];
    public TabWarnings_GEN: string[] = [];
    private ValidateAllTabs() {

        this.ValidationText = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        this.ValidateScreen_BKD();
        this.ValidateScreen_PAR();
        this.ValidateScreen_PAC();
        this.ValidateScreen_GEN();
    }

    public ValidateScreen_BKD() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Booking.S.Routings.Airline")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Master"));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Departure"));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFinalDestinationPortId)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Destination"));
        }

        //Main carriage
        var codePrefix = AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix) ? this.EntityPM.MainCarriageCarrierPrefix : this.EntityPM.MainCarriageCarrierPrefix.trim();
        if (AppTool.IsNullOrEmpty(codePrefix)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Carrier Prefix"));
        }

        else if (codePrefix.length != 2) {
            screenWarnings.push("Main Carriage Carrier Prefix length must be 2");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Booking.S.Routings.FlightNo")));
        }

        else {
            if (this.isSendFFRButtonClicked || this.isSendCancellationButtonClicked) {
                if (!FormatTool.Validate_FlightNumber(this.EntityPM.MainCarriageCarrierNumber)) {
                    var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".O." + "Routings.FlightNo");
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }
        }

        if (this.EntityPM.MainCarriageETD == null) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage ETD"));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageSpaceAllocationCode)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Space Allocation"));
        }
        else {
            if (this.EntityPM.MainCarriageSpaceAllocationCode == "CA") {
                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageAllotmentIdentification)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Allotment Identification"));
                }
            }
        }

        //Transshipment1
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 2 carrier"));
            }

            var codePrefix1 = AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierPrefix) ? this.EntityPM.Transshipment1CarrierPrefix : this.EntityPM.Transshipment1CarrierPrefix.trim();
            if (AppTool.IsNullOrEmpty(codePrefix1)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Carrier Prefix"));
            }

            else if (codePrefix1.length != 2) {
                screenWarnings.push("Transshipment1 Carrier Prefix length must be 2");
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierNumber)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Flight No."));
            }
            else {
                if (!FormatTool.Validate_FlightNumber(this.EntityPM.Transshipment1CarrierNumber)) {
                    var fieldName = "Transshipment1 Flight No.";
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }

            if (this.EntityPM.Transshipment1ETD == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 ETD"));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1SpaceAllocationCode)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Space Allocation"));
            }
            else {
                if (this.EntityPM.Transshipment1SpaceAllocationCode == "CA") {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1AllotmentIdentification)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Allotment Identification"));
                    }
                }
            }
        }

        //Transshipment2
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 3 carrier"));
            }

            var codePrefix2 = AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierPrefix) ? this.EntityPM.Transshipment2CarrierPrefix : this.EntityPM.Transshipment2CarrierPrefix.trim();
            if (AppTool.IsNullOrEmpty(codePrefix2)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Carrier Prefix"));
            }

            else if (codePrefix2.length != 2) {
                screenWarnings.push("Transshipment2 Carrier Prefix length must be 2");
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierNumber)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Flight No."));
            }
            else {
                if (!FormatTool.Validate_FlightNumber(this.EntityPM.Transshipment2CarrierNumber)) {
                    var fieldName = "Transshipment2 Flight No.";
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }

            if (this.EntityPM.Transshipment2ETD == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 ETD"));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2SpaceAllocationCode)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Space Allocation"));
            }
            else {
                if (this.EntityPM.Transshipment2SpaceAllocationCode == "CA") {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2AllotmentIdentification)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Allotment Identification"));
                    }
                }
            }
        }       
        
        this.TabErrors_BKD = screenErrors; //this.TabErrors_BKD.concat(screenErrors);
        this.TabWarnings_BKD = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "BKD");
    }

    public ValidateScreen_PAR() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        this.ValidateScreen_PAR_Shipper(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_Consignee(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_IssuingAgent(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_AirlineRules(screenErrors, screenWarnings);

        this.TabErrors_PAR = screenErrors;
        this.TabWarnings_PAR = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAR");
    }
    private ValidateScreen_PAR_Shipper(screenErrors: string[], screenWarnings: string[]) {

        if (this.EntityPM.ShipperId != null) {
            if (!AWBUtilities.IsText(this.EntityPM.ShipperName)) {
                screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Shipper Name"));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address"));
            }

            else {
                var myAddress1: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress1) ? null : this.EntityPM.ShipperAddress1.trim();
                var myAddress2: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress2) ? null : this.EntityPM.ShipperAddress2.trim();
                var myZipCode: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperZipCode) ? null : this.EntityPM.ShipperZipCode.trim();
                var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperCity) ? null : this.EntityPM.ShipperCity.trim();

                if (!FormatTool.IsTextFormatted(myAddress1)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Address1"));
                }

                if (!FormatTool.IsTextFormatted(myAddress2)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Address2"));
                }

                if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address1 Or Address2"));
                }

                if (!AppTool.IsNullOrEmpty(myZipCode)) {
                    if (!AWBUtilities.IsText(myZipCode)) {
                        screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Shipper Zip Code"));
                    }
                }

                if (AppTool.IsNullOrEmpty(myCity)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper City"));
                }

                else if (!AWBUtilities.IsText(myCity)) {
                    screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Shipper City"));
                }
            }
        }
    }
    private ValidateScreen_PAR_Consignee(screenErrors: string[], screenWarnings: string[]) {

        if (this.EntityPM.ConsigneeId != null) {
            if (!AWBUtilities.IsText(this.EntityPM.ConsigneeName)) {
                screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Consignee Name"));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address"));
            }

            else {
                var myAddress1: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress1) ? null : this.EntityPM.ConsigneeAddress1.trim();
                var myAddress2: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress2) ? null : this.EntityPM.ConsigneeAddress2.trim();
                var myZipCode: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeZipCode) ? null : this.EntityPM.ConsigneeZipCode.trim();
                var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeCity) ? null : this.EntityPM.ConsigneeCity.trim();

                if (!AWBUtilities.IsText(myAddress1)) {
                    screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Consignee Address1"));
                }

                if (!AWBUtilities.IsText(myAddress2)) {
                    screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Consignee Address2"));
                }

                if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address1 Or Address2"));
                }

                if (!AppTool.IsNullOrEmpty(myZipCode)) {
                    if (!AWBUtilities.IsText(myZipCode)) {
                        screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Consignee Zip Code"));
                    }
                }
                if (AppTool.IsNullOrEmpty(myCity)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee City"));
                }

                else if (!AWBUtilities.IsText(myCity)) {
                    screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Consignee City"));
                }
            }
        }
    }
    private ValidateScreen_PAR_IssuingAgent(screenErrors: string[], screenWarnings: string[]) {

        if (this.EntityPM.IssuingCarrierAgentId == null) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent"));
        }

        else {
            if (!AWBUtilities.IsText(this.EntityPM.IssuingCarrierAgentName)) {
                screenWarnings.push(AWBUtilities.GetWrongTextFormatMessage("Issuing Carrier Agent Name"));
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierIATACode)) {
                if (!AWBUtilities.FormateValidate_IATACode(this.EntityPM.IssuingCarrierIATACode)) {
                    var fieldName: string = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "IssuingCarrierIATACode");
                    screenWarnings.push(fieldName + " wrong format: must be 7 numeric digits max");
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.CASSCode)) {
                if (!AWBUtilities.FormateValidate_CASSCode(this.EntityPM.CASSCode)) {
                    var fieldName: string = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "CASSCode");
                    screenWarnings.push(fieldName + " wrong format: must be 4 numeric digits max");
                }
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent Address"));
            }
        }
    }
    private ValidateScreen_PAR_AirlineRules(screenErrors: string[], screenWarnings: string[]) {
        this.ValidateAirlineRule("ShipperId", this.EntityPM.ShipperId, screenWarnings);
        this.ValidateAirlineRule("ConsigneeId", this.EntityPM.ConsigneeId, screenWarnings);
        this.ValidateAirlineRule("IssuingCarrierIATACode", this.EntityPM.IssuingCarrierIATACode, screenWarnings);
        this.ValidateAirlineRule("CASSCode", this.EntityPM.CASSCode, screenWarnings);
    }

    public ValidateScreen_PAC() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];
        
        if (this.EntityPM.BookingPackages.length > 10) {
            screenErrors.push("You have exceeded the allowable limit of 10 lines of packages");
        }

        if (AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
            var msgField = TextCodeTranslator.Translate("Booking.F.GrossWeight");
            msgField = msgField.replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

            screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
            screenWarnings.push("Description Of Goods field is required");
        }

        this.EntityPM.BookingPackages.forEach((item) => {
            if (AppTool.IsNullOrZero(item.Quantity)) {
                if (AppTool.IsNullOrZero(item.Height) || AppTool.IsNullOrZero(item.Width) || AppTool.IsNullOrZero(item.Length)) {
                    screenWarnings.push("Dimensions field is required");
                }

                if ((item.Weight == null || item.Weight == 0)) {
                    screenWarnings.push("Weight field is required");
                }
            }
        });
    
        this.TabErrors_PAC = screenErrors;
        this.TabWarnings_PAC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAC");
    }

    public ValidateScreen_GEN() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        if (this.EntityPM.TenantZeroIsProductMandatory) {
            if (this.EntityPM.BookingProductId == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Booking Product"));
            }
        }

        if (this.EntityPM.IsTemperatureSensitive) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId1) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId2) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId3)
                && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId4) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId5) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId6)
                && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId7) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId8) && AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId9)) {
                screenWarnings.push("You have to select at least one handling code");
            }
        }

        this.ValidateScreen_GEN_AirlineRules(screenErrors, screenWarnings);

        this.TabErrors_GEN = screenErrors;
        this.TabWarnings_GEN = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "GEN");
    }
    private ValidateScreen_GEN_AirlineRules(screenErrors: string[], screenWarnings: string[]) {

        this.ValidateAirlineRule("AWBCarrierTarrifReference", this.EntityPM.AWBCarrierTarrifReference, screenWarnings);
        this.ValidateAirlineRule("OtherServicesInformation", this.EntityPM.OtherServicesInformation, screenWarnings);
        this.ValidateAirlineRule("SpecialServicesRequest", this.EntityPM.SpecialServicesRequest, screenWarnings);
        this.ValidateAirlineRule("BookingProductId", this.EntityPM.BookingProductId, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId1", this.EntityPM.AWBSpecialHandlingCodeId1, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId2", this.EntityPM.AWBSpecialHandlingCodeId2, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId3", this.EntityPM.AWBSpecialHandlingCodeId3, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId4", this.EntityPM.AWBSpecialHandlingCodeId4, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId5", this.EntityPM.AWBSpecialHandlingCodeId5, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId6", this.EntityPM.AWBSpecialHandlingCodeId6, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId7", this.EntityPM.AWBSpecialHandlingCodeId7, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId8", this.EntityPM.AWBSpecialHandlingCodeId8, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId9", this.EntityPM.AWBSpecialHandlingCodeId9, screenWarnings);
    }

    ValidateAirlineRule(myFieldName: string, myFieldValue: any, validationList: string[]) {
        if (this.AirlineRulesList != null) {
            var myRule = this.AirlineRulesList.filter(d => d.RuleFieldName == myFieldName)[0];
            if (myRule != null) {

                var myFieldLabel = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + myFieldName);

                if (myFieldValue == null || isNaN(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }

                else if (typeof (myFieldValue) == "string") {
                    if (AppTool.IsNullOrEmpty(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }

                    else if (myRule.MaxSize > 0) {
                        if (myFieldValue.length > myRule.MaxSize) {
                            validationList.push(myFieldLabel + " exceeds max size (" + myRule.MaxSize + ")");
                        }
                    }
                }

                else if (typeof (myFieldValue) == "number") {
                    if (AppTool.IsNullOrZero(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }
                }
            }
        }
    }
    
    public Fill_BKD: string = null;
    public Fill_PAR: string = null;
    public Fill_PAC: string = null;
    public Fill_GEN: string = null;
    private ApplyStyle(hasErrors: boolean, hasWarnings: boolean, screenCode: string) {
        if (hasErrors) {
            switch (screenCode) {
                case "BKD": { this.Fill_BKD = "#E45A26"; break; }
                case "PAR": { this.Fill_PAR = "#E45A26"; break; }
                case "PAC": { this.Fill_PAC = "#E45A26"; break; }
                case "GEN": { this.Fill_GEN = "#E45A26"; break; }
                default: { break; }
            }
        }

        else if (hasWarnings) {
            switch (screenCode) {
                case "BKD": { this.Fill_BKD = "#FFCB00"; break; }
                case "PAR": { this.Fill_PAR = "#FFCB00"; break; }
                case "PAC": { this.Fill_PAC = "#FFCB00"; break; }
                case "GEN": { this.Fill_GEN = "#FFCB00"; break; }
                default: { break; }
            }
        }

        else {
            switch (screenCode) {
                case "BKD": { this.Fill_BKD = null; break; }
                case "PAR": { this.Fill_PAR = null; break; }
                case "PAC": { this.Fill_PAC = null; break; }
                case "GEN": { this.Fill_GEN = null; break; }
                default: { break; }
            }
        }

        this.ValidateFFR();
    }
    
    ValidateFFR(): boolean {

        this.ValidationWarningsList = [];

        if (this.isSendFFRButtonClicked || this.isSendCancellationButtonClicked) {
            if (this.ValidationErrorsList.length == 0) {

                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_BKD);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAR);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_GEN);
            }
        }

        return this.ValidationWarningsList.length == 0 ? true : false;
    }
    
    public ValidationErrorsList: string[];
    public ValidationWarningsList: string[];
    ValidateBooking(): boolean {

        this.ValidationErrorsList = [];

        var myMasterFieldError = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeCode, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);

        if (!AppTool.IsNullOrEmpty(myMasterFieldError)) {
            this.ValidationErrorsList.push(myMasterFieldError);
        }

        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_BKD);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAR);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_GEN);

        return this.ValidationErrorsList.length == 0 ? true : false;
    }
    
    CloseButtonClicked() {
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;

            if (this.EntityPM.BookingStatusCode != "CRT") {
                confirmWindow.YesButtonText = "Update";
                confirmWindow.NoButtonText = "Don't Update";
            }

            else {
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
            }
            
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.isConfirmCloseClicked = true;

                    var isValid: boolean = this.ValidateBooking();

                    if (isValid) {
                        this.Save();
                    }
                }

                else if (confirmWindow.No) {
                    this.CloseWizardWindow();
                }
            });
        }

        else {
            this.CloseWizardWindow();
        }
    }
    private CloseWizardWindow() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private isSaveButtonClicked: boolean = false;
    private isSendFFRButtonClicked: boolean = false;
    private isSendCancellationButtonClicked: boolean = false;
    private isConfirmCloseClicked: boolean = false;
    private isCopyBookingButtonClicked: boolean = false;
    private isCancelBookingButtonClicked: boolean = false;
    private isReactivateButtonClicked: boolean = false;
    private isBuildButtonClicked: boolean = false;
    private isFSRRequestButtonClicked: boolean = false;
    private isManualStatusClicked: boolean = false;
    private isSendToAirlineTenantButtonClicked: boolean = false;
    private InitFlags() {
        this.isSendFFRButtonClicked = false;
        this.isSendCancellationButtonClicked = false;
        this.isSaveButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.isCopyBookingButtonClicked = false;
        this.isCancelBookingButtonClicked = false;
        this.isReactivateButtonClicked = false;
        this.isBuildButtonClicked = false;
        this.isFSRRequestButtonClicked = false;
        this.isManualStatusClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
    }

    SaveManualStatus() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        this.InitFlags();
        this.isManualStatusClicked = true;
        var isValid = this.ValidateBooking();

        if (isValid) {
            this.Save();
        }
        else {
            this.StopBusyIndicator();
        }
    }

    SaveClicked() {
        this.InitFlags();
        this.isSaveButtonClicked = true;

        var isValid: boolean = this.ValidateBooking();

        if (isValid) {
            this.Save();
        }
    }
    
    private Save() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        if (this.EntityPM.IsDirty) {
            if (this.EntityPM.Id == null) {
                this.isReloadingOnSave = true;
                this.SubmitCreatingBooking();
            }

            else {
                this.SubmitUpdatingBooking();
            }
        }

        else {
            this.StopBusyIndicator();
            this.OnSaveCompletedSuccessfully();
            this.SaveCompleted.emit(true);
        }
    }

    private SubmitCreatingBooking() {
        var myService: BookingPMService = new BookingPMService();
        myService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.OnSaveCompletedSuccessfully();
                this.SaveCompleted.emit(true);
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.StopBusyIndicator();
                this.SaveCompleted.emit(false);

            }
        }
            , error => {
                this.StopBusyIndicator();
                this.SaveCompleted.emit(false);
            });
    }
    private SubmitUpdatingBooking() {

        var myService: BookingPMService = new BookingPMService();

        myService.update(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.OnSaveCompletedSuccessfully();
                this.SaveCompleted.emit(true);
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.StopBusyIndicator();
                this.SaveCompleted.emit(false);

            }
        }
            , error => {
                this.StopBusyIndicator();
                this.SaveCompleted.emit(false);
            });
    }
    private OnSaveCompletedSuccessfully() {        
        if (this.isReloadingOnSave) {
            this.isExecutingMethod = true;
            this.ReloadBooking();
        }

        else {
            this.isExecutingMethod = true;
            this.ExecuteRequestedMethod();
        }
    }

    private isReloadingOnSave: boolean;
    private isExecutingMethod: boolean;
    private ExecuteRequestedMethod() {
        if (this.isExecutingMethod) {

            if (this.isSaveButtonClicked) {
                this.StopBusyIndicator();
            }

            else if (this.isSendFFRButtonClicked) {
                this.Send(false);
            }

            else if (this.isSendCancellationButtonClicked) {
                this.Send(true);
            }

            else if (this.isConfirmCloseClicked) {
                this.CloseWizardWindow();
            }

            else if (this.isCopyBookingButtonClicked) {
                this.StopBusyIndicator();
                this.CopyBooking();
            }

            else if (this.isCancelBookingButtonClicked) {
                this.StopBusyIndicator();
                this.CancelBooking();
            }

            else if (this.isReactivateButtonClicked) {
                this.StopBusyIndicator();
                this.ReactivateBooking();
            }

            else if (this.isBuildButtonClicked) {
                this.StopBusyIndicator();
                this.BuildShipment();
            }

            else if (this.isFSRRequestButtonClicked) {
                this.FSRRequest();
            }

            else if (this.isManualStatusClicked) {
                this.StopBusyIndicator();
                this.ReloadBooking();
            }

            else if (this.isSendToAirlineTenantButtonClicked) {
                this.StopBusyIndicator();
                this.SendToAirlineTenant();
            }

            this.isSaveButtonClicked = false;
            this.isExecutingMethod = false;
            this.isReloadingOnSave = false;
        }
    }

    OnSendFFRButtonClicked() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendFFRButtonClicked = true;

        var isValid = this.ValidateBooking();

        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();

            if (isAwbValid) {
                this.SetSendingData(false);
                this.Save();
            }

            else {
                this.StopBusyIndicator();
            }
        }

        else {
            this.StopBusyIndicator();
        }
    }
    OnSendCancellationButtonClicked() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendCancellationButtonClicked = true;

        var isValid = this.ValidateBooking();

        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();

            if (isAwbValid) {
                this.SetSendingData(true);
                this.Save();
            }

            else {
                this.StopBusyIndicator();
            }
        }

        else {
            this.StopBusyIndicator();
        }
    }

    private SetSendingData(cancellationSent: boolean) {
        if (cancellationSent) {
            //this.EntityPM.IsCancellationButtonClicked = true;
        }
        else {
            //this.EntityPM.IsFFRButtonClicked = true;
        }

        this.EntityPM.FNAReason = null;
        this.EntityPM.FMAAcknowledgementReason = null;
        this.EntityPM.AnswerOtherServicesInformation = null;
        
        if (this.EntityPM.BookingAnswers != null) {
            if (this.EntityPM.BookingAnswers.length > 0) {
                this.EntityPM.BookingAnswers.forEach((item) => {
                    this.EntityPM.RemoveBookingAnswer(item);
                });
            }
        }       
    }

    private isCancellationSent: boolean = false;
    private Send(cancellationSent: boolean) {
        this.isCancellationSent = cancellationSent;
        var isValidForSending = true;
        var validationErrorMessage = "";
        
        var myAWBFFRValidator: AWBFFRValidator = AWBUtilities.ValidateAWBFFR(this.EntityPM);

        if (isValidForSending) {
            if (myAWBFFRValidator.AirlineFieldHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.AirlineFieldErrorMessage;
            }
        }

        if (isValidForSending) {
            if (myAWBFFRValidator.TenantManagementFieldHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.TenantManagementFieldErrorMessage;
            }
        }

        if (isValidForSending) {
            if (myAWBFFRValidator.AirlineRegistrationHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.AirlineRegistrationErrorMessage;
            }
        }

        if (!cancellationSent) {
            if (isValidForSending) {
                if (myAWBFFRValidator.ETDFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myAWBFFRValidator.ETDFieldErrorMessage;
                }
            }
        }

        if (isValidForSending) {
            if (!myAWBFFRValidator.FFR) {
                isValidForSending = false;
                validationErrorMessage = "This airline will not receive FFR";
            }
        }
        
        if (!isValidForSending) {
            this.StopBusyIndicator();
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show(validationErrorMessage);
        }

        else {
            this.ValidateOnline();
        }
    }

    private ValidateOnline() {
        var myBookingDomainService = new BookingDomainService();

        myBookingDomainService.ValidateBookingForSending(this.EntityPM.Id, this.isCancellationSent).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                var myResultClass: BookingValidatorResultClass = myResponse.Result;

                if (myResultClass != null) {
                    this.ValidationErrorsList = [];

                    if (myResultClass.IsValid) {
                        this.StopBusyIndicator(); 
                        this.StartSending(this.isCancellationSent);
                    }

                    else if (myResultClass.HasStockErrors) {
                        this.StopBusyIndicator(); 

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 150;
                        logWindow.Title = "AWB Stock error";
                        logWindow.Show('./Booking/Components/BookingWizard/NoRemainingStockComponent');
                    }

                    else {
                        myResultClass.ErrorsList.forEach((item) => {
                            this.ValidationErrorsList.push(item);
                        });

                        this.StopBusyIndicator();
                    }
                }
            }

            else {
                this.StopBusyIndicator();
            }
        });
    }

    private mySendingResultClass: FFRResult = new FFRResult();
    private runOverview: boolean = false;
    private StartSending(isCancellationSent: boolean) {               
        this.StartBusyIndicator("Sending...");

        if (this.myFFRWebService == null) {
            this.myFFRWebService = new FFRWebService();
        }
                
        this.myFFRWebService.Send(this.EntityPM.Id, this.EntityPM.Tenant, this.EntityPM.TenantZeroAirlineTTY, isCancellationSent).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.mySendingResultClass = myResponse.Result;

                if (this.mySendingResultClass == null) {
                    this.StopBusyIndicator();
                }

                else {
                    if (this.mySendingResultClass.IsValid) {
                        this.runOverview = true;
                        this.ReloadBookingAfterSending();                       
                        this.SetDemoMessage();
                    }

                    else if (this.mySendingResultClass.HasStockError) {
                        this.StopBusyIndicator(); 

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 150;
                        logWindow.Title = "AWB Stock error";
                        logWindow.Show('./Booking/Components/BookingWizard/NoRemainingStockComponent');
                    }

                    else {
                        this.StopBusyIndicator();
                    }
                }
            }
        });
    }

    private RunOverviewTab() {
        this.IsTabVisible_OVE = true;
        this.SelectedTabCode = "OVE";
    }

    private SetDemoMessage() {
        if (InfraSettings.TenantPM.Id == 65 || SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Please note that this message will not be sent to the airline since it is a demo environment. You can still review the built message");
        }
    }
    
    private shipmentLevelCode: string = null;
    private shipperPartnerType: string = null;
    private consigneePartnerType: string = null;
    BuildShipmentClicked(shipmentLevel: string) {
        this.shipmentLevelCode = shipmentLevel;
        this.InitFlags();

        if (this.EntityPM.BookingStatusCode == "WCF") {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Please notice that the booking is not confirmed yet, confirm manually ?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.BookingStatusCode = "CNF";
                    this.EntityPM.FFRStatusCode = "CFM";

                    this.isBuildButtonClicked = true;
                    var isValid = this.ValidateBooking();

                    if (isValid) {
                        this.Save();
                    }
                }
            });
        }

        else {
            this.isBuildButtonClicked = true;
            var isValid = this.ValidateBooking();

            if (isValid) {
                this.Save();
            }
        }
    }
    private BuildShipment() {
        var confirmMsg = "";
        if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperId) && AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
            confirmMsg = null;
            this.StartBuildingShipment();
        }

        else {
            var myService: CardListService = new CardListService();

            myService.getSingle(this.EntityPM.ShipperId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var shipper: CardList = myResponse.Result;

                    myService.getSingle(this.EntityPM.ConsigneeId).subscribe(myResult => {
                        var myResponse: ServiceResponse = myResult;

                        if (!myResponse.HasError) {
                            var consignee: CardList = myResponse.Result; 

                            this.SetPartnerType(shipper, consignee, confirmMsg);
                        }
                    }); 
                }
            });                       
        }        
    }
    private SetPartnerType(shipper: CardList, consignee: CardList, confirmMsg: string) {
        if (shipper != null) {
            this.shipperPartnerType = shipper == null ? "" : shipper.PartnerTypeId;
        }

        if (consignee != null) {
            this.consigneePartnerType = consignee == null ? "" : consignee.PartnerTypeId;
        }

        if (this.shipmentLevelCode == "D") {
            if (this.shipperPartnerType == "AG" || this.consigneePartnerType == "AG") {
                confirmMsg = "Please confirm creating a direct AWB, note that you'll lose the shipper/consignee details if filled";
            }
        }

        else if (this.shipmentLevelCode == "C") {
            if (this.shipperPartnerType == "CS" || this.consigneePartnerType == "CS") {
                confirmMsg = "Please confirm creating a consol AWB, note that you'll lose the shipper/consignee details if filled";
            }
        }

        if (AppTool.IsNullOrEmpty(confirmMsg)) {
            this.StartBuildingShipment();
        }

        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show(confirmMsg);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.StartBuildingShipment();
                }
            });
        }
    }
    private StartBuildingShipment() {
        var shipmentPM: ShipmentPM = BookingTool.BuildShipment(this.EntityPM);
        shipmentPM.ShipmentLevelCode = this.shipmentLevelCode;

        if (this.shipmentLevelCode == "D") {
            if (this.shipperPartnerType == "AG") {
                shipmentPM.ShipperId = null;
                shipmentPM.ShipperAddressId = null;
                shipmentPM.ShipperName = null;
                shipmentPM.ShipperReference1 = null;
            }

            if (this.consigneePartnerType == "AG") {
                shipmentPM.ConsigneeId = null;
                shipmentPM.ConsigneeAddressId = null;
                shipmentPM.ConsigneeName = null;
                shipmentPM.ConsigneeReference1 = null;
            }
        }

        else if (this.shipmentLevelCode == "C") {
            if (this.shipperPartnerType == "CS") {
                shipmentPM.ShipperId = null;
                shipmentPM.ShipperAddressId = null;
                shipmentPM.ShipperName = null;
                shipmentPM.ShipperReference1 = null;
            }

            if (this.consigneePartnerType == "CS") {
                shipmentPM.ConsigneeId = null;
                shipmentPM.ConsigneeAddressId = null;
                shipmentPM.ConsigneeName = null;
                shipmentPM.ConsigneeReference1 = null;
            }
        }

        var windowArgs: AWBWizardArgs = new AWBWizardArgs();
        windowArgs.IsNewEntity = true;
        windowArgs.ShipmentLevelCode = this.shipmentLevelCode;
        windowArgs.EntityPM = shipmentPM;
        windowArgs.IsBuildFromBooking = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "New AWB Shipment";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            this.ReloadBooking();
        });
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }   
        
    private ReloadBookingAfterSending() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        var myService: BookingPMService = new BookingPMService();

        myService.get(this.EntityPM.Id).subscribe(myResult => {
            var mm: ServiceResponse = myResult;

            if (!mm.HasError) {
                this.EntityPM = mm.Result;
                this.LoadCompleted.emit(true);
                this.SetButtonsProperties();
                this.RefreshTabs();           

                if (this.runOverview) {
                    this.RunOverviewTab();                    
                }
                
                this.StopBusyIndicator();
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.LoadCompleted.emit(false);
                this.StopBusyIndicator();
            }
        }
            , error => {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push('server error on reload');
                this.LoadCompleted.emit(false);
                this.StopBusyIndicator();
            });
    }      
    private ReloadBooking() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        var myService: BookingPMService = new BookingPMService();
        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                }

                else {
                    this.EntityPM = myResponse.Result;
                    this.LoadCompleted.emit(true);
                }
            }

            this.StopBusyIndicator();

            this.SetButtonsProperties();
            this.ExecuteRequestedMethod();
        });
    }
    public ReloadEntity() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        var myService: BookingPMService = new BookingPMService();
        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.LoadCompleted.emit(true);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                }

                this.StopBusyIndicator();
            }
        });
    }

    private RefreshTabs() {
        if (this.PageChild_OVE != null) {
            this.PageChild_OVE.RefreshTab();
        }

        if (this.PageChild_BKD != null) {
            this.PageChild_BKD.RefreshTab();
        }

        if (this.PageChild_GEN != null) {
            this.PageChild_GEN.RefreshTab();
        }

        if (this.PageChild_PAC != null) {
            this.PageChild_PAC.RefreshTab();
        }

        if (this.PageChild_PAR != null) {
            this.PageChild_PAR.RefreshTab();
        }
    }  

    //More Button
    CopyBookingClicked() {
        this.InitFlags();
        this.isCopyBookingButtonClicked = true;

        var isValid = this.ValidateBooking();

        if (isValid) {
            this.Save();
        }
    }
    CancelBookingClicked() {
        var confirmMsg = "Are you sure you want to cancel this Booking?";

        var confirmWindow = new ConfirmWindow();

        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var isValid: boolean = this.ValidateBooking();

                if (isValid) {
                    this.InitFlags();
                    this.isCancelBookingButtonClicked = true;
                    this.Save();  
                }
            }
        });
    }
    ReactivateBookingClicked() {
        this.InitFlags();

        this.isReactivateButtonClicked = true;
        var isValid = this.ValidateBooking();

        if (isValid) {
            this.Save();
        }
    }
    SendToAirlineTenantClicked() {
        this.InitFlags();
        this.isSendToAirlineTenantButtonClicked = true;

        var isValid: boolean = this.ValidateBooking();

        if (isValid) {
            this.Save();
        }
    }

    private CopyBooking() {
        var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsCopyFromBooking = true;
        windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "Copy Booking";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    }
    private CancelBooking() {
        this.EntityPM.IsCancelled = true;

        this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
        }

        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            this.EntityPM.MAWBReturnedToStack = true;
            this.EntityPM.MAWBReturnedToStackWithCancel = true;
            this.EntityPM.MAWBStackNumber = this.EntityPM.Master;

            this.SaveClicked();
        }
        else {
            this.isCancelBookingButtonClicked = false;
            this.Save();
        }

        this.SetButtonsProperties();
        this.RefreshTabs();
        this.StopBusyIndicator();    
    }
    private ReactivateBooking() {
        this.InitFlags();
        this.EntityPM.IsCancelled = false;
        
        this.Save();
        this.SetButtonsProperties();
        this.StopBusyIndicator();  
    }
    private SendToAirlineTenant() {

        this.StartBusyIndicator(TextCodeTranslator.Translate("Sending..."));

        var myService = new InfrastructureDomainService();
        myService.SendEntityToAirlineTenant(this.EntityPM.Id, "Booking", this.EntityPM.MainCarriageCarrierCode).subscribe((myResponse: ServiceResponse) => {
            this.StopBusyIndicator();

            if (myResponse.HasError) {
                var messageWindow = new MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    public IsResponseProgressVisible: boolean = false;
    public TimerStoppedByUser: boolean = false;
    CloseResponseProgressClicked() {
        this.IsResponseProgressVisible = false;
        this.TimerStoppedByUser = true;

        if (this.PageChild_OVE != null) {
            this.PageChild_OVE.StopTimer();
        }
    }

    public FSRRequestMethod() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.TenantZeroAirlineTTY)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("This Airline doesn't support transmitting messages");           
        }

        else {
            this.SendFSR();
        }
    }
    private SendFSR() {
        this.InitFlags();
        this.isFSRRequestButtonClicked = true;

        var isValid = this.ValidateBooking();

        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();

            if (isAwbValid) {
                this.Save();
            }

            else {
                this.StopBusyIndicator();
            }
        }

        else {
            this.StopBusyIndicator();
        }
    }
    private FSRRequest() {
        this.StopBusyIndicator();

        var myValidator: AWBFFRValidator = AWBUtilities.ValidateAWBFSR(this.EntityPM);
        
        if (myValidator.FSR) {
            if (myValidator.IsValid) {
                this.StartSendingFSR();
            }

            else {
                this.StopBusyIndicator();

                var errorMessage: string = "";

                if (myValidator.TenantManagementFieldHasError) {
                    errorMessage = myValidator.TenantManagementFieldErrorMessage;
                }

                else if (myValidator.AirlineFieldHasError) {
                    errorMessage = myValidator.AirlineFieldErrorMessage;
                }

                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show(errorMessage);
            }
        }

        else {
            this.ValidationErrorsList.push("This airline does not support FSR/FSA messages");
        }
    }
    private StartSendingFSR() {
        this.StartBusyIndicator("Sending in Progress..");
        var myService: FSRWebService = new FSRWebService();   

        myService.SendBookingFSR(this.EntityPM.Id, "Booking", this.EntityPM.TenantZeroAirlineTTY).subscribe((myResponse: ServiceResponse) => {
            this.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                if (myResponse.Result instanceof FSRResultClass) {
                    this.ReloadBooking();
                    this.PageChild_OVE.SetDataAfterSending();
                    this.SetDemoMessage();
                }

                else {
                    var errors: string[] = [];
                    errors.push("Sending FSR failed");
                    this.ValidationErrorsList = errors;
                }
            }
        });
    }
}
