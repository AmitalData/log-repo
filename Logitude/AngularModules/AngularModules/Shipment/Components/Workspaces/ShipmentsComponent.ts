import {Component, EventEmitter, Output} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../Infrastructure/Tools';
import {ShipmentDomainService, ShipmentsSummary, FlightSummary} from '../../Services/ShipmentDomainService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentList} from '../../EntityLists/ShipmentList';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {ShipmentTool} from '../../Tools';
import {AWBWizardArgs, NewShipmentComponentArgs} from '../../Args';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
declare var window: any;

@Component({
    
    templateUrl: './ShipmentsComponent.html',
})

export class ShipmentsComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    public IsCloudDeployment: boolean = false;
    private myShipmentDomainService: ShipmentDomainService;
    public TestToggleIsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myShipmentDomainService = new ShipmentDomainService();

        if (ObjectsLocator.GlobalSetting) {
            if (ObjectsLocator.GlobalSetting.DeploymentStage) {
                if (ObjectsLocator.GlobalSetting.DeploymentStage.toLowerCase() == "amitalstorage") {
                    this.IsCloudDeployment = true;
                }
            }
        }

        var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "TST")[0];
        if (FeatureToggle) {
            this.TestToggleIsVisible = true;
        }
    }

    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }
    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }
    LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.LoadRecentShipments();
        this.LoadDeparturesArrivals(); 
        this.ReloadUsersQuery();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }


    // Queries Features
    public IsNewButtonVisible: boolean = false;
    public IsMessagingStockVisible: boolean = false;
    public IsQueryVisible_OperationalOpenGroup: boolean = false;
    public IsQueryVisible_Shipments: boolean = false;
    public IsQueryVisible_ImportShipments: boolean = false;
    public IsQueryVisible_Masters: boolean = false;
    public IsQueryVisible_AccountinglOpenGroup: boolean = false;
    public IsQueryVisible_OpenReceivables: boolean = false;
    public IsQueryVisible_OpenPayables: boolean = false;
    public IsQueryVisible_EAWBGroup: boolean = false;
    public IsQueryVisible_ExpectedDepartures: boolean = false;
    public IsQueryVisible_AirlinesUpdates: boolean = false;
    public IsQueryVisible_OthersGroup: boolean = false;
    public IsQueryVisible_AllFollowUps: boolean = false;
    public IsQueryVisible_MyFollowUps: boolean = false;
    public IsQueryVisible_AllShipments: boolean = false;
    public IsQueryVisible_AllMasters: boolean = false;
    public IsQueryVisible_CanceledShipments: boolean = false;
    public IsQueryVisible_CreditLimitBlocked: boolean = false;
    public IsQueryVisible_FSRGroup: boolean = false;
    public IsQueryVisible_MyViewsGroup: boolean = false;
    public IsQueryVisible_INTTRAGroup: boolean = false;
    public IsQueryVisible_ExpectedDeparturesNotTransmitted: boolean = false;
    public IsQueryVisible_ShippingInstructionsLast7Days: boolean = false;
    public IsQueryVisible_ContainerStatusLast7Days: boolean = false;
    public IsQueryVisible_EBookingInProgress: boolean = false;

    private SetQueriesVisibility() {

        this.IsNewButtonVisible = false;
        if (!FeatureLocator.IsPackage_EAWB() && !SessionLocator.TenantPM.IsHybrid) {
            if (FeatureLocator.HasFeaturePermession("Shipment", "NEW")) {
                this.IsNewButtonVisible = true;
            }        
        }

        this.IsQueryVisible_OperationalOpenGroup = FeatureLocator.HasFeaturePermession("Shipment", "SHIPMENTS") || FeatureLocator.HasFeaturePermession("Shipment", "MASTERS") ? true : false;
        this.IsQueryVisible_Shipments = FeatureLocator.HasFeaturePermession("Shipment", "SHIPMENTS") ? true : false;
        this.IsQueryVisible_ImportShipments = FeatureLocator.HasFeaturePermession("Shipment", "IMPORTSHIPMETNS") ? true : false;
        this.IsQueryVisible_Masters = FeatureLocator.HasFeaturePermession("Shipment", "MASTERS") ? true : false;
        this.IsQueryVisible_AccountinglOpenGroup = FeatureLocator.HasFeaturePermession("Shipment", "OPENRECEIVABLESSHIPMENTS") || FeatureLocator.HasFeaturePermession("Shipment", "OPENPAYABLESMASTERS") ? true : false;
        this.IsQueryVisible_OpenReceivables = FeatureLocator.HasFeaturePermession("Shipment", "OPENRECEIVABLESSHIPMENTS") ? true : false;
        this.IsQueryVisible_OpenPayables = FeatureLocator.HasFeaturePermession("Shipment", "OPENPAYABLESMASTERS") ? true : false;
        this.IsQueryVisible_EAWBGroup = FeatureLocator.HasFeaturePermession("Shipment", "EXPECTEDDEPATURE") || FeatureLocator.HasFeaturePermession("Shipment", "AIRLINESUPDATES") ? true : false;
        this.IsQueryVisible_ExpectedDepartures = FeatureLocator.HasFeaturePermession("Shipment", "EXPECTEDDEPATURE") ? true : false;
        this.IsQueryVisible_AirlinesUpdates = FeatureLocator.HasFeaturePermession("Shipment", "AIRLINESUPDATES") ? true : false;
        this.IsMessagingStockVisible = SessionLocator.TenantManagementJS.IsAWBStockPrepaid;

        // Others
        this.IsQueryVisible_AllFollowUps = FeatureLocator.HasFeaturePermession("Shipment", "ALLFOLLOWUPS") ? true : false;
        this.IsQueryVisible_MyFollowUps = FeatureLocator.HasFeaturePermession("Shipment", "MYFOLLOWUPS") ? true : false;
        this.IsQueryVisible_AllShipments = FeatureLocator.HasFeaturePermession("Shipment", "ALLSHIPMENTS") ? true : false;
        this.IsQueryVisible_AllMasters = FeatureLocator.HasFeaturePermession("Shipment", "ALLMASTERS") ? true : false;
        this.IsQueryVisible_CanceledShipments = FeatureLocator.HasFeaturePermession("Shipment", "CANCELLEDSHIPMENTS") ? true : false;
        if (this.IsQueryVisible_AllFollowUps || this.IsQueryVisible_MyFollowUps || this.IsQueryVisible_AllShipments || this.IsQueryVisible_AllMasters || this.IsQueryVisible_CanceledShipments) {
            this.IsQueryVisible_OthersGroup = true;
        }

        this.IsQueryVisible_CreditLimitBlocked = FeatureLocator.HasFeaturePermession("Shipment", "CreditLimitBlockedShipments") ? true : false;
        this.IsQueryVisible_FSRGroup = FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST") ? true : false;
        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;

        // INNTRA
        this.IsQueryVisible_ExpectedDeparturesNotTransmitted = FeatureLocator.HasFeaturePermession("Shipment", "ExpectedDeparturesNotTransmitted") ? true : false;
        this.IsQueryVisible_ShippingInstructionsLast7Days = FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructionsLast7Days") ? true : false;
        this.IsQueryVisible_ContainerStatusLast7Days = FeatureLocator.HasFeaturePermession("Shipment", "ContainerStatusLast7Days") ? true : false;
        this.IsQueryVisible_EBookingInProgress = FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Q.EBookingInProgress") ? true : false;
        if (this.IsQueryVisible_ExpectedDeparturesNotTransmitted || this.IsQueryVisible_ShippingInstructionsLast7Days || this.IsQueryVisible_ContainerStatusLast7Days || this.IsQueryVisible_EBookingInProgress) {
            this.IsQueryVisible_INTTRAGroup = true;
        }
    }

    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(value: string) {
        if (this.mySelectedDirectionFilter != value) {
            this.mySelectedDirectionFilter = value;

            this.CurrentSession.ChangeSessionHeader({ DirectionId: value });

            this.LoadQueriesCounts();
            this.LoadDeparturesArrivals();
        }
    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;

            this.CurrentSession.ChangeSessionHeader({ TransportId: value });

            this.LoadQueriesCounts();
            this.LoadDeparturesArrivals();
        }
    }

    public OperationalOpenCount_DH: string;
    public OperationalOpenCount_DC: string;
    public AccountingOpenCount_DH: string;
    public AccountingOpenCount_DC: string;
    public AllFollowUpsCount: string;
    public MyFollowUpsCount: string;
    public OperationalOpenCount_ETD: string;
    public OperationalOpenCount_LWU: string;
    public LastSentFSRCount: string;
    public ImportShipmentsCount: string;
    public CreditLimitBlockedCount: string;
    public ExpectedDeparturesNotTransmittedCount: string;
    public ShippingInstructionsLast7DaysCount: string;
    public ContainerStatusLast7DaysCount: string;
    public EBookingInProgressCount: string;

    LoadQueriesCounts() {
        if (this.IsCloudDeployment == false) {
            this.myShipmentDomainService.GetShipmentsCounts(this.SelectedDirectionFilter, this.SelectedTransportFilter).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult: ShipmentsSummary = myResponse.Result;

                        if (myResult != null) {
                            this.OperationalOpenCount_DH = myResult.OperationalOpenCount_DH > 1000 ? "1000+" : myResult.OperationalOpenCount_DH.toString();
                            this.OperationalOpenCount_DC = myResult.OperationalOpenCount_DC > 1000 ? "1000+" : myResult.OperationalOpenCount_DC.toString();
                            this.AccountingOpenCount_DH = myResult.AccountingOpenCount_DH > 1000 ? "1000+" : myResult.AccountingOpenCount_DH.toString();
                            this.AccountingOpenCount_DC = myResult.AccountingOpenCount_DC > 1000 ? "1000+" : myResult.AccountingOpenCount_DC.toString();
                            this.AllFollowUpsCount = myResult.AllFollowUpsCount > 1000 ? "1000+" : myResult.AllFollowUpsCount.toString();
                            this.MyFollowUpsCount = myResult.MyFollowUpsCount >= 1000 ? "1000+" : myResult.MyFollowUpsCount.toString();
                            this.OperationalOpenCount_ETD = myResult.OperationalOpenCount_ETD > 1000 ? "1000+" : myResult.OperationalOpenCount_ETD.toString();
                            this.OperationalOpenCount_LWU = myResult.OperationalOpenCount_LWU > 1000 ? "1000+" : myResult.OperationalOpenCount_LWU.toString();
                            this.LastSentFSRCount = myResult.LastSentFSRCount > 1000 ? "1000+" : myResult.LastSentFSRCount.toString();
                            this.ImportShipmentsCount = myResult.ImportShipmentsCount > 1000 ? "1000+" : myResult.ImportShipmentsCount.toString();
                            this.CreditLimitBlockedCount = myResult.CreditLimitBlockedCount > 1000 ? "1000+" : myResult.CreditLimitBlockedCount.toString();
                            this.ExpectedDeparturesNotTransmittedCount = myResult.ExpectedDeparturesNotTransmittedCount > 1000 ? "1000+" : myResult.ExpectedDeparturesNotTransmittedCount.toString();
                            this.ShippingInstructionsLast7DaysCount = myResult.ShippingInstructionsLast7DaysCount > 1000 ? "1000+" : myResult.ShippingInstructionsLast7DaysCount.toString();
                            this.ContainerStatusLast7DaysCount = myResult.ContainerStatusLast7DaysCount > 1000 ? "1000+" : myResult.ContainerStatusLast7DaysCount.toString();
                            this.EBookingInProgressCount = myResult.EBookingInProgressCount > 1000 ? "1000+" : myResult.EBookingInProgressCount.toString();
                        }
                    }
                }
            });
        }
    }

    public RecentShipmentsList: ShipmentList[] = [];
    public IsNoDataVisible_RecentShipments: boolean = false;
    LoadRecentShipments() {
        this.RecentShipmentsList = [];
        this.IsNoDataVisible_RecentShipments = false;

        this.myShipmentDomainService.GetRecentShipments().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.RecentShipmentsList = myResponse.Result;

                    if (this.RecentShipmentsList.length == 0) {
                        this.IsNoDataVisible_RecentShipments = true;
                    }
                }
            }
        });
    }

    public FlightSummaryList: FlightSummary[];
    public DepartureArrivalList: DepartureArrivalItem[];
    public IsNoDataVisible_DeparturesArrivals: boolean = false;
    private isLoadingDeparturesArrivals: boolean = false;
    LoadDeparturesArrivals() {
        if (!this.isLoadingDeparturesArrivals) {

            this.isLoadingDeparturesArrivals = true;

            this.FlightSummaryList = [];
            this.DepartureArrivalList = [];
            this.IsNoDataVisible_DeparturesArrivals = false;

            this.myShipmentDomainService.GetDeparturesArrivals(this.SelectedDirectionFilter, this.SelectedTransportFilter).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult = myResponse.Result;

                        this.FlightSummaryList = myResult;
                        this.BuildDeparturesArrivals();
                    }
                }

                this.isLoadingDeparturesArrivals = false;
            });
        }
    }
    BuildDeparturesArrivals() {

        var myResult: DepartureArrivalItem[] = [];

        if (this.FlightSummaryList != null) {

            var dataGroup: DepartureArrivalItem[] = [];

            this.FlightSummaryList.forEach((item) => {
                var dataResultItem = dataGroup.filter(d => d.CarrierId == item.CarrierId && d.CarrierNumber == item.CarrierNumber && d.DirectionId == item.DirectionId && d.TransportModeId == item.TransportModeId)[0];
                if (dataResultItem == null) {
                    dataResultItem = new DepartureArrivalItem(item.DirectionId, item.TransportModeId);
                    dataResultItem.CarrierId = item.CarrierId;
                    dataResultItem.CarrierNumber = item.CarrierNumber;
                    dataGroup.push(dataResultItem);
                }
            })

            if (dataGroup.length > 0) {

                dataGroup.forEach((item) => {
                    var myRecord: FlightSummary = this.FlightSummaryList.filter(f => f.CarrierId == item.CarrierId && f.CarrierNumber == item.CarrierNumber && f.DirectionId == item.DirectionId && f.TransportModeId == item.TransportModeId)[0];
                    if (myRecord != null) {

                        item.LineBackground = "#FFFFFFFF";
                        item.CarrierCode = myRecord.CarrierCode;
                        item.CarrierNumber = myRecord.CarrierNumber;
                        item.CarrierCodeCellText = myRecord.CarrierCode == "No_Data" ? "-" : myRecord.CarrierCode;
                        item.CarrierNumberCellText = myRecord.CarrierNumber == "No_Data" ? "-" : myRecord.CarrierNumber;
                        item.CarrierCodeFontSize = myRecord.CarrierCode == "No_Data" ? 10 : 12;
                        item.CarrierCodeForeground = myRecord.CarrierCode == "No_Data" ? "Gray" : "#FF282E30";
                        item.CarrierNumberFontSize = myRecord.CarrierNumber == "No_Data" ? 10 : 12;
                        item.CarrierNumberForeground = myRecord.CarrierNumber == "No_Data" ? "Gray" : "#FF282E30";

                        var myCarrierList: FlightSummary[] = this.FlightSummaryList.filter(f => f.CarrierId == item.CarrierId && f.CarrierNumber == item.CarrierNumber && f.DirectionId == item.DirectionId && f.TransportModeId == item.TransportModeId);
                        item.LastWeek.Build(myCarrierList);
                        item.Today.Build(myCarrierList);
                        item.Tomorrow.Build(myCarrierList);
                        item.NextWeek.Build(myCarrierList);

                        item.TotalsCount = item.LastWeek.Count + item.Today.Count + item.Tomorrow.Count + item.NextWeek.Count;
                        if (item.TotalsCount > 0) {
                            myResult.push(item);
                        }
                    }
                })
            }
        }

        // OrderByDescending
        myResult.sort((a, b) => { return (b.Today.Count) - (a.Today.Count) }).forEach((myResultItem) => {
            this.DepartureArrivalList.push(myResultItem);
        })

        this.IsNoDataVisible_DeparturesArrivals = myResult.length == 0 ? true : false;
    }

    // New Commands AWBWizardComponent
    private isWindowOpened: boolean = false;
    RunNewShipmentWizard(levelCode: string) {
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;

            var windowTitle = "";
            switch (levelCode) {
                case "D": { windowTitle = "New Direct shipment"; break; }
                case "H": { windowTitle = "New House Shipment"; break; }
                case "C": { windowTitle = "New Master"; break; }
                default: { break; }
            }

            if (levelCode == "C") {
                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.Title = windowTitle;
                logWindow.Show('./Shipment/Components/NewShipment/NewMasterComponent');

                logWindow.WindowClosed.subscribe(s => {
                    this.isWindowOpened = false;

                    if (s) {
                        this.LoadAllScreenData();
                    }
                });
            }

            else {
                var args = new NewShipmentComponentArgs();
                args.ShipmentLevelCode = levelCode;
                args.IsShipmentLevelFixed = true;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.WindowArgs = args;
                logWindow.Title = windowTitle;
                logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');

                logWindow.WindowClosed.subscribe(s => {
                    this.isWindowOpened = false;

                    if (s) {
                        this.LoadAllScreenData();
                    }
                });
            }
        }
    }
    RunNewAWBWizard(levelCode: string) {
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;

            var windowTitle = "";
            switch (levelCode) {
                case "D": { windowTitle = "Direct AWB Wizard"; break; }
                case "H": { windowTitle = "House AWB Wizard"; break; }
                case "C": { windowTitle = "Master AWB Wizard"; break; }
                default: { break; }
            }

            var windowArgs: AWBWizardArgs = new AWBWizardArgs();
            windowArgs.IsNewEntity = true;
            windowArgs.ShipmentLevelCode = levelCode;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');

            logWindow.WindowClosed.subscribe(s => {
                this.LoadAllScreenData();
                this.isWindowOpened = false;               
            });
        }
    }

    // Edit Commands
    EditShipment(entity: ShipmentList) {
        if (entity != null) {

            if (!this.isWindowOpened) {
                this.isWindowOpened = true;

                var myCodes: string[] = [];
                myCodes.push("EAWB");
                myCodes.push("BUBK");

                if (FeatureLocator.IsPackageOneOf(myCodes)) {
                    this.AWBEditShipment(entity);
                }

                else {
                    this.FullEditShipment(entity);
                }
            }
        }
    }
    private AWBEditShipment(entity: ShipmentList) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = ShipmentTool.GetAWBWizardHeader(entity.ShipmentLevelCode, entity.DirectionId);
        logWindow.WindowArgs = entity.Id;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.LoadAllScreenData();
            this.isWindowOpened = false;
        });
    }
    private FullEditShipment(entity: ShipmentList) {   
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Shipment', BackButtonLabel: "Operations" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                    this.isWindowOpened = false;                    
                });
            });
    }

    // Query Commands
    filterAgrs: ApiQueryFilters;
    ViewShipmentQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var queryCode = myQueryCode;
            var objectTableName = "Shipment";
            var MethodName = null;
            var displayTitle = "";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Operations");
            this.filterAgrs = new ApiQueryFilters();

            switch (myQueryCode) {
                case "Shipments":
                    {
                        displayTitle = "Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Masters":
                    {
                        displayTitle = "Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Open Receivables Shipments":
                    {
                        displayTitle = "Open Receivables Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Open Payables Masters":
                    {
                        displayTitle = "Open Payables Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Expected Departures":
                    {
                        displayTitle = "Expected Departures Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Airlines Updates":
                    {
                        displayTitle = "Airlines Updates Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }


                case "All Follow Ups":
                    {
                        displayTitle = "All Follow Ups";
                        MethodName = "ShipmentFollowUp";
                        this.SetDirectionTransportFilter();
                        break;
                    }


                case "My Follow Ups":
                    {
                        displayTitle = "My Follow Ups";
                        MethodName = "ShipmentFollowUp";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "All Shipments":
                    {
                        displayTitle = "All Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "All Masters":
                    {
                        displayTitle = "All Masters";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "Cancelled Shipments":
                    {
                        displayTitle = "Canceled Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "SentFSR":
                    {
                        displayTitle = "Sent FSR (last 7 days)";
                        break;
                    }

                case "ImportShipments":
                    {
                        displayTitle = "Import Shipments";
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "CreditLimitBlockedShipments":
                    {
                        displayTitle = TextCodeTranslator.Translate("Shipment.Q.CreditLimitBlockedShipment");
                        this.SetDirectionTransportFilter();
                        break;
                    }

                case "ExpDepNotTransmitted":
                    {
                        displayTitle = TextCodeTranslator.Translate("Shipment.Q.ExpectedDeparturesNotTransmitted");
                        this.SetDirectionTransportFilter();
                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName; //"Shipment";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Operations";
            listArgs.MethodName = MethodName;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {

                        var filtersBar: any = null;
                        cmpRef.instance.FiltersBarLoaded.subscribe((myBar: any) => {
                            filtersBar = myBar;

                            if (filtersBar) {
                                if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                    filtersBar.SetTransport(this.SelectedTransportFilter);
                                }

                                if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                    filtersBar.SetDirection(this.SelectedDirectionFilter);
                                }
                            }
                        });

                        cmpRef.instance.BackCompleted.subscribe(($event: any) =>
                        {
                            if (filtersBar) {
                                if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                    this.mySelectedTransportFilter = filtersBar.SelectedValue;
                                }

                                if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                    this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                                }
                            }
                            //this.CurrentSession.ChangeSessionHeader({ DirectionId: this.SelectedDirectionFilter });
                            //this.CurrentSession.ChangeSessionHeader({ TransportId: this.SelectedTransportFilter });
                            this.LoadAllScreenData()
                        });
                        listArgs.SelectedDirection = this.mySelectedDirectionFilter;
                        listArgs.SelectedTransportMode = this.mySelectedTransportFilter;
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }
    ViewDepartureArrivalQuery(item: DepartureArrivalItem, myCode: string) {
        if (myCode != null) {

            var displayName = ""; 
            this.filterAgrs = new ApiQueryFilters();

            switch (myCode) {
                case "LSW_EXP": {
                    displayName = "Last week expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.LastWeek.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }

                case "LSW_ACT": {
                    displayName = "Last week actual shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.LastWeek.ActualShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }

                case "TOD_EXP": {
                    displayName = "Today expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Today.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }

                case "TOD_ACT": {
                    displayName = "Today actual shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Today.ActualShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }

                case "TOM_EXP": {
                    displayName = "Tomorrow expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.Tomorrow.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }

                case "NXW_EXP": {
                    displayName = "Next week expected shipments";
                    this.filterAgrs.addAdditionalFilter("DeparturesArrivalsFilter", item.NextWeek.ExpectedShipmentsIds, null, null, "Equals", true, false, false, "string");
                    break;
                }
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Masters";
            listArgs.ObjectTableName = "Shipment";
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "Operations";
            listArgs.ShowViews = false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {

                        var filtersBar: any = null;
                        cmpRef.instance.FiltersBarLoaded.subscribe((myBar: any) => {
                            filtersBar = myBar;

                            if (filtersBar) {
                                if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                    filtersBar.SetTransport(this.SelectedTransportFilter);
                                }

                                if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                    filtersBar.SetDirection(this.SelectedDirectionFilter);
                                }
                            }
                        });

                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            if (filtersBar) {
                                if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                    this.mySelectedTransportFilter = filtersBar.SelectedValue;
                                }

                                if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                    this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                                }
                            }

                            this.LoadAllScreenData()
                        });

                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }
    private SetDirectionTransportFilter() {
        if (!AppTool.IsNullOrEmpty(this.SelectedDirectionFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, true, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedTransportFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string");
        }
    }

    MessagingStockClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 550;
        logWindow.Title = "Messaging Stock";
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/MessagingStock/StockWindowComponent');
    }
    SendShipmentFSRClicked() {
        this._entityResourceService.getEntityResourceByTableName("Master").subscribe(response=> {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Send FSR";
            logWindow.Width = 600;
            logWindow.Height = 370;
            logWindow.WindowArgs = this;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/FSRWizard/SendShipmentFSRComponent');
        });
    }
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    CreateMissingMastersClicked() {
        this.CurrentSession.StartBusyIndicator("Creating Masters");

        this.myShipmentDomainService.CreateMissingMasters().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}

class DepartureArrivalItem {
    LineBackground: string;
    DirectionId: string;
    DirectionName: string;
    TransportModeId: string;
    TransportModeName: string;
    CarrierId: string;
    CarrierCode: string;
    CarrierNumber: string;
    CarrierCodeCellText: string;
    CarrierNumberCellText: string;
    CarrierCodeFontSize: number;
    CarrierCodeForeground: string;
    CarrierNumberFontSize: number;
    CarrierNumberForeground: string;
    TotalsCount: number;
    LastWeek: DepartureArrival;
    Today: DepartureArrival;
    Tomorrow: DepartureArrival;
    NextWeek: DepartureArrival;
    constructor(myDirectionId: string, myTransportModeId: string) {
        this.DirectionId = myDirectionId;
        this.TransportModeId = myTransportModeId;

        switch (this.DirectionId) {
            case "E": { this.DirectionName = "Export"; break; }
            case "I": { this.DirectionName = "Import"; break; }
            case "R": { this.DirectionName = "Drop"; break; }
            case "D": { this.DirectionName = "Domestic"; break; }
        }

        switch (this.TransportModeId) {
            case "A": { this.TransportModeName = "Air"; break; }
            case "O": { this.TransportModeName = "Ocean"; break; }
            case "I": { this.TransportModeName = "Inland"; break; }
        }

        this.LastWeek = new DepartureArrival("LSW");
        this.Today = new DepartureArrival("TOD");
        this.Tomorrow = new DepartureArrival("TOM");
        this.NextWeek = new DepartureArrival("NXW");
    }    
}
class DepartureArrival {
    public Code: string;
    public Count: number = 0;
    public ExpectedCount: number = 0;
    public ExpectedShipmentsIds: string;
    public ActualCount: number = 0;
    public ActualShipmentsIds: string;
    constructor(myCode: string) {
        this.Code = myCode;
    }

    private ActualItems: FlightSummary[] = [];
    private ExpectedItems: FlightSummary[] = [];
    public Build(list: FlightSummary[]) {
        if (list) {
            this.ActualItems = list.filter(d => d.ActualDate != null);
            this.ExpectedItems = list.filter(d => d.ActualDate == null && d.ExpectedDate != null);

            switch (this.Code) {
                case "LSW": { this.Build_LSW(); break; }
                case "TOD": { this.Build_TOD(); break; }
                case "TOM": { this.Build_TOM(); break; }
                case "NXW": { this.Build_NXW(); break; }
            }

            if (AppTool.IsNullOrEmpty(this.ExpectedCount)) {
                this.ExpectedCount = 0;
            }

            if (AppTool.IsNullOrEmpty(this.ActualCount)) {
                this.ActualCount = 0;
            }

            this.Count = this.ExpectedCount + this.ActualCount;
        }
    }
    private Build_LSW() {
        var list_EXP: FlightSummary[] = this.ExpectedItems.filter(d => d.ExpectedDateCode == "LSW");
        if (list_EXP.length > 0) {
            var ids = this.GetIdsList(list_EXP);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = AppTool.GetIdsArrayText(ids);
        }

        var list_ACT: FlightSummary[] = this.ActualItems.filter(d => d.ActualDateCode == "LSW");
        if (list_ACT.length > 0) {
            var ids = this.GetIdsList(list_ACT);
            this.ActualCount = ids.length;
            this.ActualShipmentsIds = AppTool.GetIdsArrayText(ids);
        }
    }
    private Build_TOD() {
        var list_EXP: FlightSummary[] = this.ExpectedItems.filter(d => d.ExpectedDateCode == "TOD");
        if (list_EXP.length > 0) {
            var ids = this.GetIdsList(list_EXP);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = AppTool.GetIdsArrayText(ids);
        }

        var list_ACT: FlightSummary[] = this.ActualItems.filter(d => d.ActualDateCode == "TOD");
        if (list_ACT.length > 0) {
            var ids = this.GetIdsList(list_ACT);
            this.ActualCount = ids.length;
            this.ActualShipmentsIds = AppTool.GetIdsArrayText(ids);
        }
    }
    private Build_TOM() {
        var list: FlightSummary[] = this.ExpectedItems.filter(d => d.ExpectedDateCode == "TOM");
        if (list.length > 0) {
            var ids = this.GetIdsList(list);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = AppTool.GetIdsArrayText(ids);
        }
    }
    private Build_NXW() {
        var list: FlightSummary[] = this.ExpectedItems.filter(d => d.ExpectedDateCode == "NXW");
        if (list.length > 0) {
            var ids = this.GetIdsList(list);
            this.ExpectedCount = ids.length;
            this.ExpectedShipmentsIds = AppTool.GetIdsArrayText(ids);
        }
    }
    private GetIdsList(list: FlightSummary[]) {
        var myResult: string[] = [];

        list.forEach(item => {
            if (myResult.indexOf(item.ShipmentId) == -1) {
                myResult.push(item.ShipmentId);
            }
        });

        return myResult;
    }
}
