import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ConsoleShipmentPM} from '../../../../Shipment/EntityPMs/ConsoleShipmentPM';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {ShipmentListService} from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {NewShipmentComponentArgs} from '../../../../Shipment/Args';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import { ShipmentSubTypeListService } from '../../../../Shipment/services/standardlists/shipmentsubtypelistservice';
import { ShipmentSubTypeList } from '../../../../Shipment/EntityLists/ShipmentSubTypeList';


@Component({
    
    templateUrl: './ShipmentsTabComponent.html',
})
  // islam: merge test
export class ShipmentsTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public TransportModeId: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ItemsSource1: HAWBItem[];
    public ItemsSource2: HAWBItem[];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    private myService: ShipmentListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.myService = new ShipmentListService();

        this.InitializeComponent();
        this.SetUIProperties();
        this.LoadAllHouses();
        this.LoadShipmentSubTypes();
        this.Listen();
    }
    
    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "RefreshShipmentsTabFromAWBWizard") {
                    this.LoadAllHouses();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateFiltersFields();
                    this.SetUIProperties();

                    if (this.isLoadMasterRequested) {
                        this.isLoadHousesRequested = true;
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }

                    else {
                        switch (this.myRequestedCommandCode) {
                            case "N": {
                                this.RunNewShipment();
                                break;
                            }

                            case "V": {
                                this.RunViewShipment();
                                break;
                            }
                        }
                    }
                }

                this.isInProgress = false;
                this.isLoadMasterRequested = false;
                this.myRequestedCommandCode = null;
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;                 

                    this.UpdateFiltersFields();
                    this.SetUIProperties();

                    if (this.isLoadHousesRequested) {
                        this.LoadAllHouses();
                        this.CurrentSession.SessionEvent.emit("RefreshConnections");
                    }
                }

                this.isLoadHousesRequested = false;
            });
        }

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "ReloadHouses") {
                this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

                this.UpdateFiltersFields();
                this.LoadAllHouses();
            }
        });
    }
    
    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private allShipmentSubTypes: ShipmentSubTypeList[] = [];
    LoadShipmentSubTypes() {
        this.allShipmentSubTypes = [];

        var myShipmentSubTypeListService: ShipmentSubTypeListService = new ShipmentSubTypeListService();
        myShipmentSubTypeListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allShipmentSubTypes = myResponse.Result;
                this.allShipmentSubTypes = this.allShipmentSubTypes.filter(d => !d.Inactive);
            }
        });
    }

    public FromLabel: string = null;
    public ToLabel: string = null;
    InitializeComponent() {
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.Gateway") + ":";
                this.ToLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.Destination") + ":";
                break;
            }

            case "O": {
                this.FromLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.LoadingPort") + ":";
                this.ToLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.DischargePort") + ":";
                break;
            }

            case "I": {
                this.FromLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.From") + ":";
                this.ToLabel = TextCodeTranslator.Translate("Shipment.S.NewShipment.To") + ":";
                break;
            }
        }

        this.UpdateFiltersFields();
    }
    UpdateFiltersFields() {
        this.fromPortId = this.EntityPM.MainCarriageFromPortId;
        this.toPortId = this.EntityPM.MainCarriageFinalDestinationPortId;
        this.branchId = this.EntityPM.BranchId;
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
    }

    RefreshButtonClicked() {
        this.LoadAllHouses();
    }

    public HasFilters: boolean = false;
    private isAdvancedSearchOpened: boolean = false;
    get IsAdvancedSearchOpened() { return this.isAdvancedSearchOpened; }
    set IsAdvancedSearchOpened(value: boolean) {
        if (this.isAdvancedSearchOpened != value) {
            this.isAdvancedSearchOpened = value;
        }
    }
    AdvancedSearchButtonClicked() {
        this.IsAdvancedSearchOpened = !this.IsAdvancedSearchOpened;
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadItemsSource2();
        }
    }

    private fromPortId: string = null;
    get FromPortId() { return this.fromPortId; }
    set FromPortId(newValue: string) {
        if (this.fromPortId != newValue) {
            this.fromPortId = newValue;
            this.LoadItemsSource2();
        }
    }

    private toPortId: string = null;
    get ToPortId() { return this.toPortId; }
    set ToPortId(newValue: string) {
        if (this.toPortId != newValue) {
            this.toPortId = newValue;
            this.LoadItemsSource2();
        }
    }

    private branchId: string = null;
    get BranchId() { return this.branchId; }
    set BranchId(newValue: string) {
        if (this.branchId != newValue) {
            this.branchId = newValue;
            this.LoadItemsSource2();
        }
    }

    private isDirectShipmentsIncluded: boolean = false;
    get IsDirectShipmentsIncluded() { return this.isDirectShipmentsIncluded; }
    set IsDirectShipmentsIncluded(newValue: boolean) {
        if (this.isDirectShipmentsIncluded != newValue) {
            this.isDirectShipmentsIncluded = newValue;
            this.LoadItemsSource2();
        }
    }

    private isConnectedShipmentsIncluded: boolean = false;
    get IsConnectedShipmentsIncluded() { return this.isConnectedShipmentsIncluded; }
    set IsConnectedShipmentsIncluded(newValue: boolean) {
        if (this.isConnectedShipmentsIncluded != newValue) {
            this.isConnectedShipmentsIncluded = newValue;
            this.LoadItemsSource2();
        }
    }

    SetDirectShipmentsIncluded(value: boolean) {
        this.IsDirectShipmentsIncluded = value;
    }

    SetConnectedShipmentsIncluded(value: boolean) {
        this.IsConnectedShipmentsIncluded = value;
    }

    private isLoadHousesRequested: boolean = false;
    private isLoadMasterRequested: boolean = false;    
    private LoadAllHouses() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.LoadItemsSource1();
    }
    private LoadItemsSource1() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        filters.addAdditionalFilter("MasterShipmentDataId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
        filters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {

                    this.ItemsSource1 = [];
                    var list1: HAWBItem[] = [];
                    var list2: HAWBItem[] = [];

                    myResponse.Result.forEach(item => {
                        if (item != null) {
                            if (item.FHLStatusCode == "SENT") {
                                list1.push(new HAWBItem(item, this, true));
                            }

                            else {
                                list2.push(new HAWBItem(item, this, true));
                            }
                        }
                    });

                    list1 = list1.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    list2 = list2.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });

                    if (list1) {
                        this.ItemsSource1 = this.ItemsSource1.concat(list1);
                    }

                    if (list2) {
                        this.ItemsSource1 = this.ItemsSource1.concat(list2);
                    }
                }
            }

            this.BuildSummary();
            this.LoadItemsSource2();
        });
    }
    private LoadItemsSource2() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("DirectionId", this.EntityPM.DirectionId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", this.EntityPM.TransportModeId, null, null, "Equals", false, false, false, "string");

        if (!AppTool.IsNullOrEmpty(this.FromPortId)) {
            filters.addAdditionalFilter("FromPortId", this.FromPortId, null, null, "Equals", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.ToPortId)) {
            filters.addAdditionalFilter("ToPortId", this.ToPortId, null, null, "Equals", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.BranchId)) {
            filters.addAdditionalFilter("BranchId", this.BranchId, null, null, "Equals", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
            var myShipmentTypeId: string = null;

            switch (this.EntityPM.ShipmentTypeId.toUpperCase()) {
                case "MYGO": { myShipmentTypeId = "LCLD"; break; }
                case "MYGI": { myShipmentTypeId = "LTL"; break; }
                default: {
                    myShipmentTypeId = this.EntityPM.ShipmentTypeId;
                    break;
                }
            }

            if (!AppTool.IsNullOrEmpty(myShipmentTypeId)) {
                filters.addAdditionalFilter("ShipmentTypeId", myShipmentTypeId, null, null, "Equals", false, false, false, "string");
            }
        }


        // Custom
        if (!this.IsDirectShipmentsIncluded) {
            filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, false, false, "string");
        }

        filters.addAdditionalFilter("ConnectedToOtherMastersFilter", this.IsConnectedShipmentsIncluded, null, null, "Equals", true, false, false, "Boolean");

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ItemsSource2 = [];
                    var list1: HAWBItem[] = [];
                    var list2: HAWBItem[] = [];

                    myResponse.Result.forEach(item => {
                        if (item != null) {
                            if (item.MasterShipmentDataId != this.EntityPM.Id) {

                                var newItem: HAWBItem = new HAWBItem(item, this, false);

                                if (newItem.IsMatched) {
                                    list1.push(newItem);
                                }

                                else {
                                    list2.push(newItem);
                                }
                            }
                        }
                    });

                    list1 = list1.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });
                    list2 = list2.sort(function (a, b) { return a.ShipmentNumber.toLowerCase() == b.ShipmentNumber.toLowerCase() ? 0 : a.ShipmentNumber.toLowerCase() < b.ShipmentNumber.toLowerCase() ? -1 : 1; });

                    if (list1) {
                        this.ItemsSource2 = this.ItemsSource2.concat(list1);
                    }

                    if (list2) {
                        this.ItemsSource2 = this.ItemsSource2.concat(list2);
                    }
                }
            }

            this.SetCellNotesWidth();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    public CellNotesWidth: number = 0;
    SetCellNotesWidth() {
        var myColumnWidth: number = 0;

        this.ItemsSource2.forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.CellNotes)) {
                var widthOfLabel = AppTool.GetTextWidth(item.CellNotes) + 10;

                if (widthOfLabel > myColumnWidth) {
                    myColumnWidth = widthOfLabel;
                }
            } 
        });

        this.CellNotesWidth = myColumnWidth;
    }

    public SummaryQuantity: number = 0;
    public SummaryGrossWeight: number = 0;
    public SummaryVolumetricWeight: number = 0;
    public SummaryChargeableWeight: number = 0;
    private BuildSummary() {
        this.SummaryQuantity = ArrayTool.Sum(this.ItemsSource1, "Quantity");
        this.SummaryGrossWeight = ArrayTool.Sum(this.ItemsSource1, "GrossWeight");
        this.SummaryChargeableWeight = ArrayTool.Sum(this.ItemsSource1, "ChargeableWeight");
        this.SummaryVolumetricWeight = ArrayTool.Sum(this.ItemsSource1, "VolumetricWeight");
    }

    Save() {
        this.isLoadMasterRequested = true;
        this.entityArgs.EditComponent.SaveChanges();
    }

    private myRequestedCommandCode: string;
    private myRequestedHouseId: string;
    NewShipmentClicked() {
        this.myRequestedCommandCode = "N";
        this.entityArgs.EditComponent.SaveChanges();
    }
    ViewShipmentClicked(item: HAWBItem) {
        if (item) {
            this.myRequestedHouseId = item.Id;
            this.myRequestedCommandCode = "V";
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    RunNewShipment() {
        var args = new NewShipmentComponentArgs();
        args.ShipmentLevelCode = "H";
        args.IsShipmentLevelFixed = true;
        args.IsCreatedFromMasterHouses = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New House Shipment";
        logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {

            var myShipmentTypeId = this.EntityPM.ShipmentTypeId;
            var myShipmentSubTypeId = this.EntityPM.ShipmentSubTypeId;
            var subTypeCode: string = null;

            if (this.EntityPM.ShipmentTypeName) {
                if (this.EntityPM.ShipmentTypeName.toLowerCase().indexOf("my groupage") > -1) {
                    if (this.EntityPM.TransportModeId == "O") {
                        myShipmentTypeId = "LCLD"
                        subTypeCode = "LCL"; 
                    }

                    else {
                        myShipmentTypeId = "LTL";
                        subTypeCode = "LTL"; 
                    }

                    var subType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.Code == subTypeCode)[0];
                    if (subType) {
                        myShipmentSubTypeId = subType.Id;
                    }

                    else {
                        var defaultSubType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.ShipmentTypeCode == myShipmentTypeId)[0];
                        if (defaultSubType) {
                            myShipmentSubTypeId = defaultSubType.Id;
                        }
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
            cmp.EntityPM.MasterShipmentDataId = this.EntityPM.MasterShipmentDataId;
            cmp.EntityPM.CutoffDate = this.EntityPM.CutoffDate;
            cmp.EntityPM.SCI = this.EntityPM.SCI;
            cmp.EntityPM.ShipmentSubTypeId = myShipmentSubTypeId;

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.isLoadHousesRequested = true;
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    }
    RunViewShipment() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.myRequestedHouseId, ObjectTableName: 'Shipment', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });

                let isEditComponentSaved = false;

                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        this.isLoadHousesRequested = true;
                        this.entityArgs.EditComponent.ReloadEntityPM();
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

    private isInProgress: boolean = false;
    ConnectAllClicked() {
        if (!this.isInProgress) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Please confirm connecting all the disconnected shipments to this Master shipment");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.isInProgress = true;

                    var saving: boolean = false;

                    this.ItemsSource2.forEach((item: HAWBItem) => {
                        if (item.IsMatched) {
                            var itemPM = this.EntityPM.ShipmentConsoleShipments.filter(f => f.Id == item.Id)[0];
                            if (itemPM == null) {
                                itemPM = new ConsoleShipmentPM(this.EntityPM);
                                itemPM.Id = item.Id;
                                itemPM.ShipmentNumber = this.EntityPM.ShipmentNumber;
                                itemPM.MasterShipmentDataId = this.EntityPM.Id;
                                this.EntityPM.AddConsoleShipment(itemPM);
                                saving = true;
                            }
                        }
                    });

                    if (saving) {
                        this.Save();
                    }

                    else {
                        this.isInProgress = false;
                    }
                }
            });
        }
    }
    DisconnectAllClicked() {
        if (!this.isInProgress) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Please confirm disconnecting all the connected shipments from this Master shipment");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.isInProgress = true;

                    var saving: boolean = false;

                    this.ItemsSource1.forEach((item: HAWBItem) => {
                        var itemPM = this.EntityPM.ShipmentConsoleShipments.filter(f => f.Id == item.Id)[0];
                        if (itemPM != null) {
                            this.EntityPM.RemoveConsoleShipment(itemPM);
                            saving = true;
                        }
                    });

                    if (saving) {
                        this.Save();
                    }

                    else {
                        this.isInProgress = false;
                    }
                }
            });
        }
    }
}
class HAWBItem {
    public IsMatched: boolean = false;
    public Quantity: number = 0;
    constructor(private item: ShipmentList, private fatherComponent: ShipmentsTabComponent, isConnected: boolean) {
        if (item != null) {
            this.isChecked = isConnected;
            this.SetIsMatched();
            this.SetCellNotes();

            if (fatherComponent.IsFCLEntity) {
                this.Quantity = item.NumberOfContainers;                
            }

            else {
                this.Quantity = item.NumberOfPackages;
            }
        }
    }

    get Id() { return this.item.Id; }
    get ShipmentNumber() { return this.item.ShipmentNumber; }
    get ShipmentType() { return this.item.ShipmentType; }
    get CreateDateTime() { return this.item.CreateDateTime; }
    get StatusName() { return this.item.StatusName; }
    get BranchName() { return this.item.BranchName; }
    get House() { return this.item.House; }
    get CustomerName() { return this.item.CustomerName; }
    get FromPort() { return this.item.FromPort; }
    get ToPort() { return this.item.ToPort; }
    get GrossWeight() { return this.item.GrossWeight; }
    get VolumetricWeight() { return this.item.VolumetricWeight; }
    get ChargeableWeight() { return this.item.ChargeableWeight; }    

    get JobNumber() { return (this.item.ShipmentNumber == this.item.MasterShipmentNumber) ? "" : this.item.MasterShipmentNumber;; }
    public CellNotes: string = null;

    private SetIsMatched() {
        if (AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.FromPortId == this.fatherComponent.EntityPM.MainCarriageFromPortId && this.item.ToPortId == this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId ) {
            this.IsMatched = true;
        }
    }    
    private SetCellNotes() {
        var myResult:string = null;

        if (this.item.ShipmentLevelCode == "D") {
            myResult = TextCodeTranslator.Translate("Master.M.Shipments.OnlyHouseConnected");
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.MasterShipmentDataId != this.fatherComponent.EntityPM.Id) {
                myResult = TextCodeTranslator.Translate("Master.M.Shipments.AlreadyConnected");
            }

            else if (this.item.FromPortId != this.fatherComponent.EntityPM.MainCarriageFromPortId || this.item.ToPortId != this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId) {
                myResult = TextCodeTranslator.Translate("Master.M.Shipments.DoesntMatch");
            }

            else if (this.fatherComponent.EntityPM.ShipmentARInvoices.filter(d => d.InvoiceTypeCode == "MN").length > 0) {
                if (this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(d => d.Id == this.Id).length > 0) {
                    myResult = TextCodeTranslator.Translate("Master.M.Shipments.HasManifestInvoice");
                }
            }
        }

        this.CellNotes = myResult;
    }

    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;
            this.AddRemove();
        }
    }

    private AddRemove() {
        if (this.IsChecked) {
            this.AddConsoleShipment();            
        }

        else {
            this.RemoveConsoleShipment();           
        }
    }
    private AddConsoleShipment() {
        var itemPM = this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(f => f.Id == this.Id)[0];
        if (itemPM == null) {
            itemPM = new ConsoleShipmentPM(this.fatherComponent.EntityPM);
            itemPM.Id = this.Id;
            itemPM.ShipmentNumber = this.fatherComponent.EntityPM.ShipmentNumber;
            itemPM.MasterShipmentDataId = this.fatherComponent.EntityPM.Id;
            this.fatherComponent.EntityPM.AddConsoleShipment(itemPM);
            this.fatherComponent.Save();
        }
    }
    private RemoveConsoleShipment() {
        var itemPM = this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(f => f.Id == this.Id)[0];
        if (itemPM != null) {
            if (!AppTool.IsNullOrEmpty(itemPM.PreForwardingFromPortId) || !AppTool.IsNullOrEmpty(itemPM.OnForwardingFromPortId)) {
                this.ConfirmRemovingConsoleShipment(itemPM);
            }

            else {
                this.fatherComponent.EntityPM.RemoveConsoleShipment(itemPM);
                this.fatherComponent.Save();
            }            
        }
    }
    private ConfirmRemovingConsoleShipment(itemPM: ConsoleShipmentPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Disconnecting this house will change Pre/ On Forwarding Ports, proceed ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.fatherComponent.EntityPM.RemoveConsoleShipment(itemPM);
                this.fatherComponent.Save();
            }
        });
    }
}
