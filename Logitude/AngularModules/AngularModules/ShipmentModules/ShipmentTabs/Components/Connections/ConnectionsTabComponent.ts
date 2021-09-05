import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDomainService, ShipmentConnectedEntity} from '../../../../Shipment/Services/ShipmentDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools'
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ShipmentAssemblyPM} from '../../../../Shipment/EntityPMs/ShipmentAssemblyPM';
import { WarehouseHelper } from '../../../../Warehouse/Helpers/WarehouseHelper';
import { NewShipmentComponentArgs } from '../../../../Shipment/Args';
import { WarehouseEntryListExtendedService } from '../../../../Warehouse/Services/ExtendedLists/WarehouseEntryListExtendedService';
import { WarehouseReleaseListExtendedService } from '../../../../Warehouse/Services/ExtendedLists/WarehouseReleaseListExtendedService';
import { FeatureToggleList } from '../../../../Infrastructure/EntityLists/FeatureToggleList';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';

@Component({
    
    templateUrl: './ConnectionsTabComponent.html',
})

export class ConnectionsTabComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    private myDomainService: ShipmentDomainService;
    warehouseEntryListExtendedService: WarehouseEntryListExtendedService;
    warehouseReleaseListExtendedService: WarehouseReleaseListExtendedService;
    public IsNoDataTextVisible: boolean = false;

    public ItemsSource: ShipmentConnectedEntityItem[] = [];
    public WarehouseEntriesItemsSource: ShipmentConnectedEntityItem[];
    public WarehouseReleasesItemsSource: ShipmentConnectedEntityItem[];
    public QuotesItemsSource: ShipmentConnectedEntityItem[];
    public MastersItemsSource: ShipmentConnectedEntityItem[];
    public CustomFilesItemsSource: ShipmentConnectedEntityItem[];
    public TicketsItemsSource: ShipmentConnectedEntityItem[];
    public PickupDeliveryItemsSource: ShipmentConnectedEntityItem[];

    public IsWarehouseEntryVisible: boolean = false;
    public IsNewWarehouseEntryVisible: boolean = false;
    public IsWarehouseReleaseVisible: boolean = false;
    public IsNewWarehouseReleaseVisible: boolean = false;
    public IsAssembliesVisivle: boolean = false;
    public IsDisconnectQuoteVisible: boolean = false;
    public IsMasterGridVisible: boolean = false;
    public IsNewMasterVisible: boolean = false;
    public DisableNewWarehouseEntryButton: boolean = false;
    public DisableNewWarehouseReleaseButton: boolean = false;
    public IsStandaloneShipmentVisible: boolean = false;
    public IsDisconnectindStandAloneShipmentCompleted: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.myDomainService = new ShipmentDomainService();
        this.warehouseEntryListExtendedService = new WarehouseEntryListExtendedService();
        this.warehouseReleaseListExtendedService = new WarehouseReleaseListExtendedService();

        if (FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.IsNewWarehouseEntryVisible = true;
            }

            this.IsWarehouseEntryVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                this.IsNewWarehouseReleaseVisible = true;
            }

            this.IsWarehouseReleaseVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("Shipment", "DisconnectQuote")) {
            this.IsDisconnectQuoteVisible = true;
        }

        if (this.EntityPM.ShipmentLevelCode == "H") {
            this.IsMasterGridVisible = true;

            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "NEWMASTERFROMHOUSE")) {
                this.IsNewMasterVisible = true;
            }
        }

        this.SetIsStandaloneShipmentVisible();
        this.Listen();
        this.LoadData();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("ShipmentAssembly").subscribe((res1: any) => {
                if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                    this.IsAssembliesVisivle = true;

                    this.FillAssemblies();
                }
            });
        }
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.IsNewWarehouseEntryRequested) {
                        this.ShowWarehouseScreen("Entry");
                    }

                    else if (this.IsNewWarehouseReleaseRequested) {
                        this.ShowWarehouseScreen("Release");
                    }

                    else if (this.isNewMasterClicked) {
                        this.RunNewMasterWizard();
                    }

                    else {
                        this.LoadData();
                        this.FillAssemblies();
                    }
                }

                this.IsOpenWarehouseEntryScreen = false;
                this.IsOpenWarehouseReleaseScreen = false;
                this.IsNewWarehouseEntryRequested = false;
                this.IsNewWarehouseReleaseRequested = false;
                this.isNewMasterClicked = false;
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadData();
                    this.FillAssemblies();

                    if (this.IsDisconnectindStandAloneShipmentCompleted) {
                        this.IsDisconnectindStandAloneShipmentCompleted = false;
                        this.CurrentSession.FireEvent("ReloadDisconnectedForwarderShipment");
                    }
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHCN") {
                    this.LoadData();
                    this.FillAssemblies();
                }
            });            
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetShipmentConnectedEntities(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list: ShipmentConnectedEntity[] = myResponse.Result;
                    this.FillItemSources(list);
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
        if (this.EntityPM.IsCFSWarehouse && this.EntityPM.DirectionId == "I") {
            this.SetIsCFSWarehouseProperities();
        }

    }

    SetIsCFSWarehouseProperities() {
        this.DisableNewWarehouseEntryButton = false;
        this.DisableNewWarehouseReleaseButton = false;
        this.warehouseEntryListExtendedService.GetActiveWarehouseEntriesByShipmentId(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
            var warehouseEntries = serviceResponse.Result;
            if (warehouseEntries && warehouseEntries.length > 0) {
                this.DisableNewWarehouseEntryButton = true;
            }
        });
        this.warehouseReleaseListExtendedService.getActiveWarehouseReleaseListsByShipmentId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe((serviceResponse: ServiceResponse) => {
            var warehouseRelease = serviceResponse.Result;
            if (warehouseRelease && warehouseRelease.length > 0) {
                this.DisableNewWarehouseReleaseButton = true;
            }
        });
    }

    SetIsStandaloneShipmentVisible() {
        var featureToggle: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SAS")[0];
        if (featureToggle && this.EntityPM.IsStandalonePickupDelivery) {
            this.IsStandaloneShipmentVisible = true;
        }
    }

    public EntriesGridHeight: number = 90;
    public ReleasesGridHeight: number = 90;
    public AssembliesGridHeight: number = 90;
    public TicketsGridHeight: number = 90;
    public PickupDeliveryGridHeight: number = 90;


    public IsQuoteGridVisible: boolean = false;
    public IsCustomFileGridVisible: boolean = false;
    public IsTicketsGridVisible: boolean = false;
    private FillItemSources(list: ShipmentConnectedEntity[]) {
        this.ItemsSource = [];
        this.WarehouseEntriesItemsSource = [];
        this.WarehouseReleasesItemsSource = [];
        this.QuotesItemsSource = [];
        this.MastersItemsSource = [];
        this.CustomFilesItemsSource = [];
        this.TicketsItemsSource = [];
        this.PickupDeliveryItemsSource = [];

        list.forEach(item => {
            this.ItemsSource.push(new ShipmentConnectedEntityItem(item, this));
        });

        this.WarehouseEntriesItemsSource = this.ItemsSource.filter(d => d.EntityType == "Cross Dock Entry");
        this.WarehouseReleasesItemsSource = this.ItemsSource.filter(d => d.EntityType == "Cross Dock Release");
        this.QuotesItemsSource = this.ItemsSource.filter(d => d.EntityType == "Quote");
        this.MastersItemsSource = this.ItemsSource.filter(d => d.EntityType == "Master");
        this.CustomFilesItemsSource = this.ItemsSource.filter(d => d.EntityType == "Custom File");
        this.TicketsItemsSource = this.ItemsSource.filter(d => d.EntityType == "Ticket");
        this.PickupDeliveryItemsSource = this.ItemsSource.filter(d => d.EntityType == "PickUp" || d.EntityType == "Delivery");

        this.IsQuoteGridVisible = this.QuotesItemsSource.length == 0 ? false : true;
        this.IsCustomFileGridVisible = this.CustomFilesItemsSource.length == 0 ? false : true;
        this.IsTicketsGridVisible = this.TicketsItemsSource.length == 0 ? false : true;

        this.EntriesGridHeight = this.ComputeGridHeight(this.WarehouseEntriesItemsSource);
        this.ReleasesGridHeight = this.ComputeGridHeight(this.WarehouseReleasesItemsSource);
        this.TicketsGridHeight = this.ComputeGridHeight(this.TicketsItemsSource);
        this.PickupDeliveryGridHeight = this.ComputeGridHeight(this.PickupDeliveryItemsSource);
    }

    private ComputeGridHeight(list: any[]): number {
        var height: number = 90;

        if (list.length == 0 || list.length == 1) {
            height = 90;
        }

        else if (list.length == 2) {
            height = 110;
        }

        else if (list.length == 3) {
            height = 130;
        }

        else {
            height = 160;
        }

        return height;
    }

    ViewEntity(item: ShipmentConnectedEntityItem) {
        var myBackButtonLabel = "Shipment: " + this.EntityPM.ShipmentNumber;

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.EntityId, ObjectTableName: item.ObjectTableName, BackButtonLabel: myBackButtonLabel, EntityParentPM: this.EntityPM });

                let isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (item.EntityType == "PickUp" || item.EntityType == "Delivery") {
                        this.entityArgs.EditComponent.EntityId = this.EntityPM.Id;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    else {
                        if (isEditComponentSaved) {
                            if (item.ObjectTableName == "WarehouseRelease" || item.ObjectTableName == "WarehouseEntry") {
                                this.EntityPM.IsDirty = true;
                                this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                            else
                                this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
                cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
    }

    IsNewWarehouseEntryRequested: boolean = false;
    IsOpenWarehouseEntryScreen: boolean = false;
    NewWarehouseEntryButtonClicked() {
        if (!this.IsOpenWarehouseEntryScreen) {
            this.IsOpenWarehouseEntryScreen = true;

            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.IsNewWarehouseEntryRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.ShowWarehouseScreen("Entry");
            }
        }
    }

    IsNewWarehouseReleaseRequested: boolean = false;
    IsOpenWarehouseReleaseScreen: boolean = false;
    NewWarehouseReleaseButtonClicked() {
        if (!this.IsOpenWarehouseReleaseScreen) {
            this.IsOpenWarehouseReleaseScreen = true;

            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.IsNewWarehouseReleaseRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.ShowWarehouseScreen("Release");
            }
        }
    }

    ShowWarehouseScreen(widnowName: string) {
        var windowArgs: any = {};
        windowArgs.ShipmentPM = this.EntityPM;
        windowArgs.ConnectedTo = "Shipment";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1030;
        logWindow.Height = 620;
        logWindow.Title = widnowName == "Release" ? "New Cross Dock Release" : "New Cross Dock Entry";
        if (this.EntityPM) {
            var shipmentPackages: any = this.EntityPM.ShipmentPackages;
            if (shipmentPackages && shipmentPackages.length > 0 && widnowName == "Entry") {
                var wrehouseHelper: WarehouseHelper = new WarehouseHelper();
                windowArgs.WarehouseEntryPackagesLists = wrehouseHelper.FullWarehouseEntryPackagePM(shipmentPackages, this.EntityPM, "ShipmentPackages");
            }
        }

        logWindow.WindowArgs = windowArgs;
        var widnowPath: string = widnowName == "Release" ? "./Warehouse/Components/NewWarehouseReleaseComponent" : "./Warehouse/Components/NewWarehouseEntryComponent";
        logWindow.Show(widnowPath);
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.LoadData();
            }
            this.IsOpenWarehouseReleaseScreen = false;
            this.IsOpenWarehouseEntryScreen = false;

        });
    }

    public AssembliesItemsSource: ShipmentAssemblyPM[];
    private FillAssemblies() {
        this.AssembliesItemsSource = [];
        this.AssembliesItemsSource = this.EntityPM.ShipmentAssemblies;

        this.AssembliesGridHeight = this.ComputeGridHeight(this.AssembliesItemsSource);
    }

    AddAssembly() {
        var itemPM: ShipmentAssemblyPM = new ShipmentAssemblyPM(null);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        itemPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        itemPM.CreatedByUserId = SessionLocator.LoggedUserId;
        itemPM.UpdatedByUserId = SessionLocator.LoggedUserId;

        this.RunAssemblyWindow(itemPM, "Add Shipment Assembly");
    }
    public EditAssembly(itemPM: ShipmentAssemblyPM) {
        this.RunAssemblyWindow(itemPM, "Edit Shipment Assembly");
    }
    public DeleteAssembly(itemPM: ShipmentAssemblyPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this assembly?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var index = this.EntityPM.ShipmentAssemblies.indexOf(itemPM);
                if (index > -1) {
                    this.EntityPM.RemoveAssembly(itemPM);
                    this.FillAssemblies();
                }
            }
        });
    }
    private RunAssemblyWindow(item: ShipmentAssemblyPM, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.WindowArgs = { EntityPM: item, ShipmentPM: this.EntityPM };
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Connections/AddEditShipmentAssemblyComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.FillAssemblies();
            }
        });
    }

    DisconnectQuote() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("After disconnecting the quote from the shipment you will not be able to generate Receivables / Payables from this quote");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.myDomainService.DisconnectQuote(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ReloadEntityPM();
                            this.CurrentSession.FireEvent("LoadConnectedShipments");
                            this.LoadData();
                        }
                    }
                });
            }
        });
    }

    private isNewMasterClicked: boolean = false;
    NewMasterButtonClicked() {
        this.myDomainService.CheckIfHouseConnectedToMaster(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                var result: boolean = myResponse.Result;

                if (result) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("House shipment already connected to a Master, in order to connect to another please disconnect it first");
                }

                else {
                    this.ProceedToNewMaster();
                }                
            }
        }); 
    }
    private ProceedToNewMaster() {
        if (this.EntityPM.IsDirty) {
            this.isNewMasterClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }

        else {
            this.RunNewMasterWizard();
        }
    }
    private RunNewMasterWizard() {
        var args = new NewShipmentComponentArgs();
        args.IsMasterCreatedFromHouse = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New Master";
        logWindow.Show('./Shipment/Components/NewShipment/NewMasterComponent');

        logWindow.ComponentLoaded.subscribe(cmp => {
            var myShipmentTypeId = this.EntityPM.ShipmentTypeId;
            if (this.EntityPM.ShipmentTypeName) {
                if (this.EntityPM.ShipmentTypeName.toLowerCase().indexOf("my groupage") > -1) {
                    if (this.EntityPM.TransportModeId == "O") {
                        myShipmentTypeId = "LCLD"
                    }

                    else {
                        myShipmentTypeId = "LTL"
                    }
                }
            }

            cmp.DirectionId = this.EntityPM.DirectionId;
            cmp.TransportModeId = this.EntityPM.TransportModeId;
            cmp.ShipmentTypeId = myShipmentTypeId;
            cmp.MainCarriageFromPortId = this.EntityPM.FromPortId;
            cmp.MainCarriageToPortId = this.EntityPM.ToPortId;
            cmp.SalesmanUserId = this.EntityPM.SalesmanUserId;
            cmp.EntityPM.BranchId = this.EntityPM.BranchId;
            cmp.EntityPM.DepartmentId = this.EntityPM.DepartmentId;
            cmp.EntityPM.SCI = this.EntityPM.SCI;
            cmp.EntityPM.MasterCreatedFromHouseId = this.EntityPM.Id;
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    //this.isMasterCreated = true;
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    }

    DisconnectStandaloneShipmentClicked() {
        if (this.EntityPM.ShipmentPackages != null && this.EntityPM.ShipmentPackages.length > 0) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Disconnecting this standalone shipment will cause the package(s) to be deleted from the shipment. Please confirm.");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.DisconnectStandaloneShipment();
                }
            });
        }
        else {
            this.DisconnectStandaloneShipment();
        }
    }

    DisconnectStandaloneShipment() {
        this.CurrentSession.StartBusyIndicator("Disconnecting...");
        this.myDomainService.DisconnectStandaloneShipment(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
    
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    this.entityArgs.EditComponent.ReloadEntityPM();
                    this.IsDisconnectindStandAloneShipmentCompleted = true;
                    this.LoadData();
                }
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}

class ShipmentConnectedEntityItem {
    private myEntity: ShipmentConnectedEntity = new ShipmentConnectedEntity();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: ShipmentConnectedEntity, public fatherComponent: ConnectionsTabComponent) {
        this.myEntity = entity;

        this.ComputeDateProperties();
    }

    get EntityId() { return this.myEntity.EntityId; }
    get EntityType() { return this.myEntity.EntityType; }
    get EntityNumber() { return this.myEntity.Reference; }
    get ObjectTableName() { return this.myEntity.ObjectTableName; }
    get EntityStatus() { return this.myEntity.EntityStatus; }
    get OpenDate() { return this.myEntity.OpenDate; }
    get AcceptedDate() { return this.myEntity.AcceptedDate; }
    get Salesman() { return this.myEntity.Salesman; }

    public EntityDate: Date;
    public Foreground: string;
    public DateType: string;
    private ComputeDateProperties() {
        if (this.myEntity.ActualDate != null) {
            this.EntityDate = this.myEntity.ActualDate;
            this.Foreground = FontTool.Green;
            this.DateType = "(actual)";
        }

        else if (this.myEntity.ExpectedDate != null) {
            this.EntityDate = this.myEntity.ExpectedDate;
            this.Foreground = FontTool.Red;
            this.DateType = "(expected)";
        }

        else {
            this.DateType = "";
        }
    }
}
