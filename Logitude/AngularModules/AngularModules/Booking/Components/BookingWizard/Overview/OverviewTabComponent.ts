import {Component, ViewChildren, QueryList} from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BookingWizardComponent} from '../BookingWizardComponent';
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {BookingAnswerPM} from '../../../EntityPMs/BookingAnswerPM';
import {BookingPMService} from '../../../Services/StandardPMs/BookingPMService';
import {BookingDomainService} from '../../../Services/BookingDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'OverviewTabComponent',
    moduleId: module.id,
    templateUrl: './OverviewTabComponent.html',
})

export class OverviewTabComponent {
    public Wizard: BookingWizardComponent;
    public EntityPM: BookingPM;
    public ObjectTableName: string;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public AnswersList: BookingAnswerItem[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    InitTab(wizard: BookingWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.InitializeTimer();
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction();
    }
    
    RefreshTab() {
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction();          
    }

    SetDataAfterSending() {
        this.IsFirstTimeLoaded = true;
        this.EntityPM = this.Wizard.EntityPM;
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction(); 
        this.StopTimer();
        this.StartTimer();
    }

    private isSaveRequested: boolean = false;
    private isReloadRequested: boolean = false;
    private Save() {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    }
    private Reload() {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    }
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties_ManualAction();
                }

                if (this.isSaveRequested) {
                    this.isSaveRequested = false;

                    if (isSaveSuccess) {
                        this.Reload();
                    }
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;

                    if (this.isReloadRequested) {
                        this.isReloadRequested = false;

                        this.SetUIProperties_ManualAction();
                    }
                }
            });
        }
    }

    OpenShipment() {
        if (!AppTool.IsNullOrEmpty(this.ShipmentId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "Edit AWB Wizard";
            logWindow.WindowArgs = this.ShipmentId;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
        }
    }
    
    public IsConfirmManuallyVisibile: boolean = false;
    public IsCancelManuallyVisible: boolean = false;
    public IsConfirmManuallyEnabled: boolean = false;
    public SetUIProperties_ManualAction() {
        var isConfirmVisible = false;
        var isCancelVisible = false;
        var isConfirmEnabled = true;

        if (this.EntityPM.FFRStatusCode == "BRQ" || this.EntityPM.FFRStatusCode == "RBA") {
            isConfirmVisible = true;
        }

        else if (this.EntityPM.FFRStatusCode == "CRS" || this.EntityPM.FFRStatusCode == "RBC") {
            isCancelVisible = true;
        }

        if (this.EntityPM.IsCancelled) {
            isConfirmEnabled = false;
        }

        this.IsConfirmManuallyVisibile = isConfirmVisible;
        this.IsCancelManuallyVisible = isCancelVisible;
        this.IsConfirmManuallyEnabled = isConfirmEnabled;
    }

    private CommunicationsPage: any = null;
    private selectedTabCode: string = "ST";
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }
    private SelectionChanged() {
        switch (this.SelectedTabCode) {
            case "ST": {

                break;
            }

            case "CM": {
                if (this.CommunicationsPage == null) {
                    let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                    if (myLocation != null) {
                        this._entityResourceService.getEntityResourceByTableName("CommunicationLog", 0).subscribe((resp: any) => {

                            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.CommunicationsPage = cmpRef.instance;
                                    this.CommunicationsPage.IsTitleHidden = true;
                                    //this.CommunicationsPage.Run(this.EntityPM.Id, this.ObjectTableName);
                                });
                        });
                    }
                }

                else {
                    //this.CommunicationsPage.Load();
                }

                break;
            }
        }
    }

    // Status
    get FFRStatusName() { return this.EntityPM.FFRStatusName; }
    get BookingStatusName() { return this.EntityPM.BookingStatusName; }
    get ShipmentId() { return this.EntityPM.ShipmentId; }
    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }

    // Cardo Info
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    get Quantity() {
        var myResult = 0;

        if (this.EntityPM != null) {
            this.EntityPM.BookingPackages.forEach(item => {
                if (!AppTool.IsNullOrZero(item.Quantity)) {
                    myResult += item.Quantity;
                }
            });
        }
        
        if (AppTool.IsNullOrZero(myResult)) {
            myResult = 0;
        }

        return myResult;
    }

    // Requests

    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    get MainCarriageFlightNumber() {
        var result = this.EntityPM.MainCarriageCarrierPrefix;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
            result = result + this.EntityPM.MainCarriageCarrierNumber;
        }

        return result;
    }

    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    get MainCarriageSpaceAllocationCode() { return this.EntityPM.MainCarriageSpaceAllocationCode; }
    get MainCarriageAllotmentId() { return this.EntityPM.MainCarriageAllotmentIdentification; }

    get Transshipment1FromPortId() { return this.EntityPM.Transshipment1FromPortId; }
    get Transshipment1CarrierName() { return this.EntityPM.Transshipment1CarrierName; }

    get Transshipment1FlightNumber() {
        var result = this.EntityPM.Transshipment1CarrierPrefix;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierNumber)) {
            result = result + this.EntityPM.Transshipment1CarrierNumber;
        }

        return result;
    }

    get Transshipment1ETD() { return this.EntityPM.Transshipment1ETD; }
    get Transshipment1SpaceAllocationCode() { return this.EntityPM.Transshipment1SpaceAllocationCode; }
    get Transshipment1AllotmentId() { return this.EntityPM.Transshipment1AllotmentIdentification; }

    get Transshipment2FromPortId() { return this.EntityPM.Transshipment2FromPortId; }
    get Transshipment2CarrierName() { return this.EntityPM.Transshipment2CarrierName; }

    get Transshipment2FlightNumber() {
        var result = this.EntityPM.Transshipment2CarrierPrefix;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierNumber)) {
            result = result + this.EntityPM.Transshipment2CarrierNumber;
        }

        return result;
    }

    get Transshipment2ETD() { return this.EntityPM.Transshipment2ETD; }
    get Transshipment2SpaceAllocationCode() { return this.EntityPM.Transshipment2SpaceAllocationCode; }
    get Transshipment2AllotmentId() { return this.EntityPM.Transshipment2AllotmentIdentification; }

    get FFRStatusDate() { return this.EntityPM.FFRStatusDate; }
    get FMAAcknowledgement() { return this.EntityPM.FMAAcknowledgementReason; }
    get FNAReason() { return this.EntityPM.FNAReason; }
    get OSI() { return this.EntityPM.AnswerOtherServicesInformation; }

    public SameDataText: string = "";
    private sameAnswersData: boolean = true;
    public AllAnswersHaveSameData: boolean = false;
    private CheckSameData() {
        this.AnswersList.forEach((item) => {
            var answerDate = null;
            var date1 = null;
            var date2 = null;
            var date3 = null;

            if (item.ETD != null) {
                answerDate = DateTool.TruncateTime(item.ETD);
            }

            if (this.EntityPM.MainCarriageETD != null) {
                date1 = DateTool.TruncateTime(this.EntityPM.MainCarriageETD);
            }

            if (this.EntityPM.Transshipment1ETD != null) {
                date2 = DateTool.TruncateTime(this.EntityPM.Transshipment1ETD);
            }

            if (this.EntityPM.Transshipment2ETD != null) {
                date3 = DateTool.TruncateTime(this.EntityPM.Transshipment2ETD);
            }

            if (item.LegText == "Leg 1") {
                if (item.FlightNumber != this.MainCarriageFlightNumber || answerDate.valueOf() != date1.valueOf() || item.CarrierId != this.EntityPM.MainCarriageCarrierId) {
                    this.sameAnswersData = false;
                    return;
                }
            }

            else if (item.LegText == "Leg 2") {
                if (this.sameAnswersData) {
                    if (item.FlightNumber != this.Transshipment1FlightNumber || answerDate.valueOf() != date2.valueOf() || item.CarrierId != this.EntityPM.Transshipment1CarrierId) {
                        this.sameAnswersData = false;
                        return;
                    }
                }
            }

            else if (item.LegText == "Leg 3") {
                if (this.sameAnswersData) {
                    if (item.FlightNumber != this.Transshipment2FlightNumber || answerDate.valueOf() != date3.valueOf() || item.CarrierId != this.EntityPM.Transshipment2CarrierId) {
                        this.sameAnswersData = false;
                        return;
                    }
                }
            }
        });

        //this.SameAnswersData = true;
        if (this.AnswersList.length > 0 && this.sameAnswersData) {
            this.AllAnswersHaveSameData = true;

            if (this.AnswersList.every(elem => elem.SpaceAllocationCode == "KK")) {
                this.SameDataText = "Booking Confirmed";
            }
            else if (this.AnswersList.every(elem => elem.SpaceAllocationCode == "UU")) {
                this.SameDataText = "Unable";
            }
            else if (this.AnswersList.every(elem => elem.SpaceAllocationCode == "CN")) {
                this.SameDataText = "Cancellation Noted";
            }
        }
    }

    //Answers    
    public BuildAnswersData(list: BookingAnswerPM[]) {
        if (this.AnswersList == null) {
            this.AnswersList = new Array<BookingAnswerItem>();
        }

        else {
            this.AnswersList = [];
        }

        var i = 1;
        list.sort((a, b) => { return (a === b) ? 0 : a ? -1 : 1 }).forEach((item) => {

            var text = "Leg " + i++;
            var itemViewModel: BookingAnswerItem = new BookingAnswerItem(item, this, text);
            this.AnswersList.push(itemViewModel);
        })

        this.CheckSameData();
    }
    private LoadAnswers() {
        var myBookingDomainService: BookingDomainService = new BookingDomainService();

        myBookingDomainService.GetBookingAnswerPMs(this.EntityPM.Id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                var list: BookingAnswerPM[] = myResponse.Result;
                this.BuildAnswersData(list);
            }
        });
    }
    RefreshAnswersClicked() {
        this.LoadBooking();
    }

    // Manual Actions
    SetStatusManually(type: string) {
        this.EntityPM.WaitingForResponse = false;

        if (type == "Confirmed") {
            this.EntityPM.BookingStatusCode = "CNF";
            this.EntityPM.FFRStatusCode = "CFM";
        }

        else if (type == "Cancelled") {
            this.EntityPM.BookingStatusCode = "CRT";
            this.EntityPM.FFRStatusCode = "CNM";
        }

        this.Wizard.SaveManualStatus();
    }

    // Timer
    private Retries: number = 0;
    private timerToken: any;
    private timerSeconds: number = 1;
    private IsLoading: boolean = false;
    private InitializeTimer() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if (this.EntityPM.WaitingForResponse) {
                this.StartTimer();
            }
        }
    }
    public StopTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        //this.Wizard.SaveCompleted.emit(false);
        this.Wizard.IsResponseProgressVisible = false;
    }
    public StartTimer() {
        this.Retries = 0;
        this.timerToken = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
        this.Wizard.IsResponseProgressVisible = true;
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
            this.LoadBooking();
            this.AdjustTimerSpeed();
        }
    }

    public IsFirstTimeLoaded: boolean = false;
    private LoadBooking() {
        var myService: BookingPMService = new BookingPMService();

        myService.get(this.EntityPM.Id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;
                this.Wizard.EntityPM = myResponse.Result;

                if (!this.IsFirstTimeLoaded) {
                    this.IsFirstTimeLoaded = true;

                    this.LoadAnswers();
                    this.SetUIProperties_ManualAction();
                }

                if (this.EntityPM.HasResponse && this.EntityPM.WaitingForResponse) {
                    this.StopTimer();
                    this.LoadAnswers();
                    this.SetUIProperties_ManualAction();
                }

                if (!this.EntityPM.WaitingForResponse) {
                    this.StopTimer();
                    this.LoadAnswers();
                    this.SetUIProperties_ManualAction();
                }

                this.Wizard.SetButtonsProperties();

                if (this.Wizard.PageChild_BKD != null) {
                    this.Wizard.PageChild_BKD.RefreshTab();
                }

                if (this.Wizard.PageChild_GEN != null) {
                    this.Wizard.PageChild_GEN.RefreshTab();
                }

                if (this.Wizard.PageChild_PAC != null) {
                    this.Wizard.PageChild_PAC.RefreshTab();
                }

                if (this.Wizard.PageChild_PAR != null) {
                    this.Wizard.PageChild_PAR.RefreshTab();
                }
            }            
        }
            , error => {
                this.CurrentSession.StopBusyIndicator();
            });
    }

    //FSR
    get IsSendFSREnabled() {
        var myResult = true;

        if (this.EntityPM.IsCancelled) {
            myResult = false;
        }
        else if (this.EntityPM.BookingStatusCode == "AWB") {
            myResult = false;
        }
        else {
            if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
                if (!this.EntityPM.TenantZeroAirlineGLSHKFSRFSA) {
                    myResult = false;
                }
            }

            else {
                if (!this.EntityPM.TenantZeroAirlineChampFSRFSA) {
                    myResult = false;
                }
            }
        }

        return myResult;
    }

    get LastFSRStatusRequestDate() { return this.EntityPM.LastFSRStatusRequestDate; }

    SendFSRButtonClicked() {
        this.Wizard.FSRRequestMethod();
    }
}

export class BookingAnswerItem {
    public EntityPM: BookingAnswerPM;
    public BookingPM: BookingPM;
    constructor(entityPM: BookingAnswerPM, public fatherComponent: OverviewTabComponent, public legText: string) {
        this.EntityPM = entityPM;
        this.BookingPM = fatherComponent.EntityPM;
    }

    get LegText() { return this.legText; }
    get CarrierId() { return this.EntityPM.CarrierId; }
    get CarrierName() { return this.EntityPM.CarrierName; }
    get FlightNumber() { return this.EntityPM.FlightNumber; }
    get ETD() { return this.EntityPM.ETD; }
    get Origin() { return this.EntityPM.Origin; }
    get OriginCountryName() { return this.EntityPM.OriginCountryName; }
    get OriginCountryCode() { return this.EntityPM.OriginCountryCode; }
    get Destination() { return this.EntityPM.Destination; }
    get DestinationCountryName() { return this.EntityPM.DestinationCountryName; }
    get DestinationCountryCode() { return this.EntityPM.DestinationCountryCode; }
    get SpaceAllocationCode() { return this.EntityPM.BookingSpaceAllocationCode; }
}
