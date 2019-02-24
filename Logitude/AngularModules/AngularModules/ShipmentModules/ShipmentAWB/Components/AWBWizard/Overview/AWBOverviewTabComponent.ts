import {Component, ViewChildren, QueryList} from '@angular/core';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {LocationDirective} from '../../../../../Infrastructure/Utilities/LocationDirective';
import {ShipmentDomainService, ShipmentCarrierStatusList} from '../../../../../Shipment/Services/ShipmentDomainService';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {FSRWizardComponent} from '../../FSRWizard/FSRWizardComponent';
import {SendFSRArgs} from '../../FSRWizard/SendFSRComponent';
import {AWBHelper, AWBCCSValidator} from '../../../../../Shipment/Tools';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    selector: 'OverviewTabComponent',
    templateUrl: './AWBOverviewTabComponent.html',
})

export class AWBOverviewTabComponent {
    public EntityPM: ShipmentPM;
    public DataContext: AWBOverviewTabComponent = this;
    public ObjectTableName: string;
    public ItemsSource: StatusLineItem[] = [];
    public IsFullWizard: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor() {

    }

    private AWBWizard: AWBWizardComponent = null;
    private FSRWizard: FSRWizardComponent = null;
    InitTab(entityPM: ShipmentPM, wizard: any) {

        if (wizard instanceof AWBWizardComponent) {
            this.AWBWizard = wizard;
            this.IsFullWizard = true;
        }

        if (wizard instanceof FSRWizardComponent) {
            this.FSRWizard = wizard;
            this.IsFullWizard = false;
        }

        this.EntityPM = entityPM;
        this.ObjectTableName = entityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetFSRStatus();
        this.SetMessagingStatus();
        this.LoadCarrierStatuses();
        this.Listen();
    }

    RefreshTab() {
        this.SetFSRStatus();
        this.SetMessagingStatus();        
    }

    public IsFHLStatusVisible: boolean = false;
    public IsCargonautFWBStatusVisible: boolean = false;
    public IsCargonautFHLStatusVisible: boolean = false;
    public IsFNAReasonStatusVisible: boolean = false;
    public IsDemoTenantStatusVisible: boolean = false;
    public IsBookingStatusVisible: boolean = false;    
    private SetMessagingStatus() {

        this.SetMessagingLables();
        this.IsFHLStatusVisible = false;
        this.IsCargonautFWBStatusVisible = false;
        this.IsCargonautFHLStatusVisible = false;
        this.IsFNAReasonStatusVisible = false;
        this.IsDemoTenantStatusVisible = false;
        this.IsBookingStatusVisible = false;

        if (this.EntityPM.ShipmentLevelCode != "D") {
            this.IsFHLStatusVisible = true;
        }

        switch (this.EntityPM.MainCarriageFromPortCode) {
            case "SPL":
            case "AMS":
            case "RTM":
            case "MST":
            case "LGG":
            case "BRU":
                {
                    this.IsCargonautFWBStatusVisible = true;

                    if (this.EntityPM.ShipmentLevelCode != "D") {
                        this.IsCargonautFHLStatusVisible = true;
                    }

                    break;
                }
        }
       
        if (!AppTool.IsNullOrEmpty(this.EntityPM.FNAReason)) {
            this.IsFNAReasonStatusVisible = true;
        }

        if (SessionLocator.TenantPM.Id == 65 || SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            this.IsDemoTenantStatusVisible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
            this.IsBookingStatusVisible = true;
        }
    }

    public CargonautDEXXFWBLabel: string = null;
    public CargonautDEXXFHLLabel: string = null;
    private SetMessagingLables() {
        var FWBLabel = "Cargonaut FWB";
        var FHLLabel = "Cargonaut FHL";

        switch (this.EntityPM.MainCarriageFromPortCode) {
            case "LGG":
            case "BRU":
                {
                    FWBLabel = "DEXX FWB";
                    FHLLabel = "DEXX FWB";
                    break;
                }
        }

        this.CargonautDEXXFWBLabel = FWBLabel;
        this.CargonautDEXXFHLLabel = FHLLabel;
    }

    // Messaging Status
    get FWBStatusName() { return this.EntityPM.FWBStatusName; }
    get FWBStatusDate() { return this.EntityPM.FWBStatusDate; }

    get FHLStatusName() { return this.EntityPM.FHLStatusName; }
    get FHLStatusDate() { return this.EntityPM.FHLStatusDate; }
    
    get CargonautFWBStatusName() { return this.EntityPM.CargonautFWBStatusName; }
    get CargonautFWBStatusDate() { return this.EntityPM.CargonautFWBStatusDate; }
    
    get CargonautFHLStatusName() { return this.EntityPM.CargonautFHLStatusName; }
    get CargonautFHLStatusDate() { return this.EntityPM.CargonautFHLStatusDate; }

    get CarrierLastStatusCode() { return this.EntityPM.CarrierLastStatusCode; }
    get CarrierLastStatusName() { return this.EntityPM.CarrierLastStatusName; }
    get CarrierLastStatusDate() { return this.EntityPM.CarrierLastStatusDate; }

    get FNAReason() { return this.EntityPM.FNAReason; }
    get BookingId() { return this.EntityPM.BookingId; }
    get BookingNumber() { return this.EntityPM.BookingNumber; }

    OpenBooking() {
        if (!AppTool.IsNullOrEmpty(this.BookingId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "Edit Booking Title";
            logWindow.WindowArgs = this.BookingId;
            logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');
        }
    }

    // Cargo Information
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    get Quantity() {
        var myResult = 0;

        if (AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId)) {
            this.EntityPM.ShipmentPackages.forEach(item => {
                if (!AppTool.IsNullOrZero(item.Quantity)) {
                    myResult += item.Quantity;
                }                
            });
        }

        else {
            myResult = this.EntityPM.ShipmentPackages.length;
        }

        if (AppTool.IsNullOrZero(myResult)) {
            myResult = 0;
        }

        return myResult;
    }

    // Tabs
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
                        this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(response=> {
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

    // Statuses
    public IsNoStatus: boolean = false;
    private myDomainService: ShipmentDomainService;
    LoadCarrierStatuses() {
        this.ItemsSource = [];
        this.IsNoStatus = false;

        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService();
        }
       
        this.myDomainService.GetShipmentCarrierStatuses(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {

                    myResponse.Result.forEach(item => {
                        this.ItemsSource.push(new StatusLineItem(item));
                    });

                    if (myResponse.Result.length == 0) {
                        this.IsNoStatus = true;
                    }
                }
            }
        });
    }

    // Refresh
    RefreshClicked() {
        this.ReloadEntity();
    }

    // FSR
    private isSendWindowOpen: boolean = false;
    public IsFSRButtonVisible: boolean = false;
    public IsFSRButtonEnabled: boolean = false;
    private SetFSRStatus() {
        var isFSRButtonVisible = false;
        var isFSRButtonEnabled = false;

        if (FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST")) {
            isFSRButtonVisible = true;
        }

        if (this.IsFullWizard) {
            if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
                if (this.EntityPM.TenantZeroAirlineGLSHKFSRFSA) {
                    isFSRButtonEnabled = true;
                }
            }

            else {
                if (this.EntityPM.TenantZeroAirlineChampFSRFSA) {
                    isFSRButtonEnabled = true;
                }
            }
        }

        else {

            if (SessionLocator.TenantPM.Id == 0) {
                isFSRButtonEnabled = true;
            }

            else if (FeatureLocator.IsPackage_DVMT()) {
                isFSRButtonEnabled = true;
            }

            else if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                isFSRButtonEnabled = true;
            }
        }

        this.IsFSRButtonVisible = isFSRButtonVisible;
        this.IsFSRButtonEnabled = isFSRButtonEnabled;
    }
    SendFSRClicked() {

        var ShowConfirmRCS: boolean = false;

        if (this.ItemsSource) {
            if (this.ItemsSource.filter(d => d.Status.toUpperCase() == "RCS").length > 0) {
                ShowConfirmRCS = true;                
            }
        }

        if (ShowConfirmRCS) {
            var myConfirmWindow = new ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show("The airline may not receive your update after RCS status received, send anyway?");
            myConfirmWindow.WindowClosed.subscribe((event: any) => {
                if (myConfirmWindow.Yes) {
                    this.SendFSR();
                }
            });
        }

        else {
            this.SendFSR();
        }
    }
    private SendFSR() {

        var isValidFSR = false;

        if (this.AWBWizard != null) {
            isValidFSR = this.AWBWizard.ValidateShipment();
            if (isValidFSR) {
                isValidFSR = this.AWBWizard.ValidateFSR();
            }
        }

        else if (this.FSRWizard != null) {
            isValidFSR = this.FSRWizard.ValidateFSR();          
        }

        if (isValidFSR) {
            var myCCSValidator: AWBCCSValidator = AWBHelper.ValidateAWBCCS(this.EntityPM);

            if (myCCSValidator.FSRFSA) {

                if (myCCSValidator.IsValid) {

                    if (this.AWBWizard != null) {
                        this.AWBWizard.SaveFSR();
                    }

                    else if (this.FSRWizard != null) {
                        this.RunSendWindow();
                    }
                }

                else {
                    if (myCCSValidator.TenantManagementFieldHasError) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show(myCCSValidator.TenantManagementFieldErrorMessage);
                    }

                    else if (myCCSValidator.AirlineFieldHasError) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show(myCCSValidator.AirlineFieldErrorMessage);
                    }
                }
            }

            else {
                var messageWindow = new MessageWindow();
                messageWindow.Show("This airline does not support FSR/FSA messages");
            }
        }
    }
    private RunSendWindow() {
        if (!this.isSendWindowOpen) {

            this.isSendWindowOpen = true;

            var args = new SendFSRArgs();
            args.EntityPM = this.EntityPM;
            args.OverviewTab = this;

            var logWindow = new LogitudeWindow();
            logWindow.Title = "FSR Request";
            logWindow.WindowArgs = args;
            logWindow.Show("./ShipmentModules/ShipmentAWB/Components/FSRWizard/SendFSRComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.isSendWindowOpen = false;
            });
        }
    }

    public ReloadEntity() {
        if (this.AWBWizard != null) {
            this.AWBWizard.ReloadEntity();
        }

        else if (this.FSRWizard != null) {
            this.FSRWizard.ReloadEntity();
        }
    }

    private Listen() {
        if (this.AWBWizard != null) {
            this.AWBWizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.AWBWizard.EntityPM;
                    this.RefreshTab();

                    if (this.AWBWizard.IsFSRRequestButtonClicked) {
                        this.AWBWizard.IsFSRRequestButtonClicked = false;
                        this.RunSendWindow();
                    }
                }
            });

            this.AWBWizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.AWBWizard.EntityPM;
                    this.LoadCarrierStatuses();
                    this.RefreshTab();

                    if (this.AWBWizard.IsFSRRequestButtonClicked) {
                        this.AWBWizard.IsFSRRequestButtonClicked = false;
                        this.RunSendWindow();
                    }
                }
            });
        }

        else if (this.FSRWizard != null) {
            this.FSRWizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.FSRWizard.EntityPM;
                    this.LoadCarrierStatuses();
                    this.RefreshTab();
                }
            });
        }
    }
}

class StatusLineItem {
    public Name: string = "LOL";
    public LegHeight: number = 50;
    public IsDatesRowVisible: boolean = false;
    constructor(private item: ShipmentCarrierStatusList) {
        if (item.DepartureDate != null || item.ArrivalDate != null) {
            this.LegHeight = 75;
            this.IsDatesRowVisible = true;
            this.SetDatesProperties();
        }
    }

    get Status() { return this.item.Status; }
    get StatusName() { return this.item.StatusName; }
    get Details() { return this.item.Details; }

    get EventDate() { return this.item.EventDate; }
    get ReceivingDate() { return this.item.ReceivingDate; }
    get Weight() { return this.item.Weight; }
    get Pieces() { return this.item.Pieces; }
    get Partial() { return this.item.Partial; }

    get AirlineName() { return this.item.AirlineName; }
    get FlightNumber() { return this.item.FlightNumber; }
    get LocationCode() { return this.item.LocationCode; }
    get LocationName() { return this.item.LocationName; }

    public DepartureLabel: string;
    get DepartureDate() { return this.item.DepartureDate; }

    public ArrivalLabel: string;
    get ArrivalDate() { return this.item.ArrivalDate; }
    private SetDatesProperties() {

        if (this.item.DepartureDate != null) {
        
            if (!AppTool.IsNullOrEmpty(this.item.TimeOfDepartureInfo)) {
                switch (this.item.TimeOfDepartureInfo.toUpperCase()) {
                    case "A": { this.DepartureLabel = "Actual Departure: "; break; }
                    case "E": { this.DepartureLabel = "Expected Departure: "; break; }
                    case "S": { this.DepartureLabel = "Scheduled Departure: "; break; }
                    default: { this.DepartureLabel = "Departure: "; break; };
                }
            }
        }

        if (this.item.ArrivalDate != null) {
            if (!AppTool.IsNullOrEmpty(this.item.TimeOfArrivalInfo)) {
                switch (this.item.TimeOfArrivalInfo.toUpperCase()) {
                    case "A": { this.ArrivalLabel = "Actual Arrival: "; break; }
                    case "E": { this.ArrivalLabel = "Expected Arrival: "; break; }
                    case "S": { this.ArrivalLabel = "Scheduled Arrival: "; break; }
                    default: { this.ArrivalLabel = "Arrival: "; break; };
                }
            }
        }
    }
}
