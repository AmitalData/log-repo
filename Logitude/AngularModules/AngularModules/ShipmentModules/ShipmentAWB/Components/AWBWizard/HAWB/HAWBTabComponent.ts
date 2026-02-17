import {Component, OnDestroy} from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ConsoleShipmentPM} from '../../../../../Shipment/EntityPMs/ConsoleShipmentPM';
import {ShipmentList} from '../../../../../Shipment/EntityLists/ShipmentList';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {ShipmentListService} from '../../../../../Shipment/Services/StandardLists/ShipmentListService';
import {ApiQueryFilters, FilterItem} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {AWBWizardArgs} from '../../../../../Shipment/Args';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,

    selector: 'HAWBTabComponent',
    templateUrl: './HAWBTabComponent.html',
})

export class HAWBTabComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public ObjectTableName: string;
    public ItemsSource1: HAWBItem[] = [];
    public ItemsSource2: HAWBItem[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public IsEditingEnabled: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
               
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);        
        this.Listen();
        this.LoadAllHouses();
    }

    RefreshTab() {
        this.isNewEntityRequested = false;
    }

    RefreshButtonClicked() {
        this.LoadAllHouses();
    }

    get IsCantConnectTextVisible() {
        var myResult = false;

        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                myResult = true;
            }
        }

        return myResult;
    }

    private myService: ShipmentListService;
    private LoadAllHouses() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.CurrentSession.StartBusyIndicatorLoading();

            if (this.myService == null) {
                this.myService = new ShipmentListService();
            }

            this.ItemsSource1 = [];
            this.ItemsSource2 = [];
            this.LoadItemsSource1();
        }
    }
    private LoadItemsSource1() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;

        filters.Filter1Name = "MasterShipmentDataId";
        filters.Filter1Value = this.EntityPM.Id;
        filters.Filter1Operator = "Equals";

        filters.Filter2Name = "ShipmentLevelCode";
        filters.Filter2Value = "H";
        filters.Filter2Operator = "Equals";

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

                    this.ItemsSource1 = this.ItemsSource1.concat(list1);
                    this.ItemsSource1 = this.ItemsSource1.concat(list2);
                }
            }

            this.BuildSummary();
            this.LoadItemsSource2();
        });
    }
    private LoadItemsSource2() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;

        filters.Filter1Name = "IsCancelled";
        filters.Filter1Value = false;
        filters.Filter1Operator = "Equals";

        filters.Filter2Name = "IsOperationalClosed";
        filters.Filter2Value = false;
        filters.Filter2Operator = "Equals";

        filters.Filter3Name = "DirectionId";
        filters.Filter3Value = this.EntityPM.DirectionId;
        filters.Filter3Operator = "Equals";

        filters.Filter4Name = "TransportModeId";
        filters.Filter4Value = this.EntityPM.TransportModeId;
        filters.Filter4Operator = "Equals";

        filters.Filter5Name = "FromPortId";
        filters.Filter5Value = this.EntityPM.MainCarriageFromPortId;
        filters.Filter5Operator = "Equals";

        filters.Filter6Name = "ToPortId";
        filters.Filter6Value = this.EntityPM.MainCarriageFinalDestinationPortId;
        filters.Filter6Operator = "Equals";

        filters.Filter7Name = "BranchId";
        filters.Filter7Value = this.EntityPM.BranchId;
        filters.Filter7Operator = "Equals";

        filters.Filter8Name = "ShipmentLevelCode";
        filters.Filter8Value = "H";
        filters.Filter8Operator = "Equals";

        // Custom
        filters.addAdditionalFilter("ConnectedToOtherMastersFilter", false, null, null, "Equals", true, false, false, "Boolean");

        if (this.EntityPM.ShipmentTypeId != null) {
            var filterValue: string = this.EntityPM.ShipmentTypeId;

            switch (this.EntityPM.ShipmentTypeId.toUpperCase()) {
                case "MYGO": { filterValue = "LCLD"; break; }
                case "MYGI": { filterValue = "LTL"; break; }
            }

            filters.Filter9Name = "ShipmentTypeId";
            filters.Filter9Value = filterValue;
            filters.Filter9Operator = "Equals";
        }

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

                    this.ItemsSource2 = this.ItemsSource2.concat(list1);
                    this.ItemsSource2 = this.ItemsSource2.concat(list2);
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    public SummaryGrossWeight: number = 0;
    public SummaryVolumetricWeight: number = 0;
    public SummaryChargeableWeight: number = 0;
    private BuildSummary() {

        //var myGrossWeight = 0;
        //var myVolumetricWeight = 0;
        //this.ItemsSource1.forEach(item => {
        //    if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
        //        myGrossWeight += item.GrossWeight;
        //    }

        //    if (!AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
        //        myVolumetricWeight += item.VolumetricWeight;
        //    }
        //});

        //this.SummaryGrossWeight = myGrossWeight;
        //this.SummaryVolumetricWeight = myVolumetricWeight;
        //this.SummaryChargeableWeight = this.SummaryGrossWeight > this.SummaryVolumetricWeight ? this.SummaryGrossWeight : this.SummaryVolumetricWeight;

        this.SummaryGrossWeight = ArrayTool.Sum(this.ItemsSource1, "GrossWeight");
        this.SummaryChargeableWeight = ArrayTool.Sum(this.ItemsSource1, "ChargeableWeight");
        this.SummaryVolumetricWeight = ArrayTool.Sum(this.ItemsSource1, "VolumetricWeight");
    }

    private isNewEntityRequested: boolean = false;
    NewShipment() {
        if (!this.isNewEntityRequested) {
            this.isNewEntityRequested = true;
            this.Wizard.SaveClicked();
        }
    }

    RunNewShipment() {
        this.CurrentSession.StopBusyIndicator();

        var windowTitle = "House AWB Wizard";

        var windowArgs: AWBWizardArgs = new AWBWizardArgs();
        windowArgs.IsNewEntity = true;
        windowArgs.ShipmentLevelCode = "H";
        windowArgs.MasterPM = this.EntityPM;
        windowArgs.IsCreatingHouseFromMaster = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.ReloadEntity());
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent");
    }

    ViewShipment(item: ShipmentList) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = item.Id;
        logWindow.WindowClosed.subscribe(($event: any) => this.ReloadEntity());
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    }

    private isReloadRequested: boolean = false;
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;

                    if (this.isNewEntityRequested) {
                        this.isNewEntityRequested = false;
                        this.RunNewShipment();
                    }

                    if (this.isReloadRequested) {
                        this.ReloadEntity();
                    }
                }

                this.StopListenFlags();
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;

                    if (this.isNewEntityRequested) {
                        this.isNewEntityRequested = false;
                        this.RunNewShipment();
                    }

                    if (this.isReloadRequested) {
                        this.isReloadRequested = false;
                        this.LoadAllHouses();
                    }
                }

                this.StopListenFlags();
            });
        }
    }

    private StopListenFlags() {
        this.isReloadRequested = false;
        this.isNewEntityRequested = false;
    }
    private ReloadEntity() {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    }

    public Save() {
        this.isReloadRequested = true;
        this.Wizard.SaveClicked();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {

    }
}

class HAWBItem {
    public Id: string;
    public ShipmentNumber: string;
    public CustomerName: string;
    public House: string;
    public GrossWeight: number;
    public VolumetricWeight: number;
    public ChargeableWeight: number;
    public FHLStatusName: string;
    public FNAReason: string;
    public IsMatched: boolean = false;
    constructor(private item: ShipmentList, private fatherComponent: HAWBTabComponent, isConnected: boolean) {
        if (item != null) {
            this.Id = item.Id;
            this.ShipmentNumber = item.ShipmentNumber;
            this.CustomerName = item.CustomerName;
            this.House = item.House;
            this.GrossWeight = item.GrossWeight;
            this.VolumetricWeight = item.VolumetricWeight;
            this.ChargeableWeight = item.ChargeableWeight;
            this.FHLStatusName = item.FHLStatusName;
            this.FNAReason = item.FNAReason;
            this.isChecked = isConnected;
            this.SetIsMatched();
        }
    }

    private SetIsMatched() {
        if (AppTool.IsNullOrEmpty(this.item.MasterShipmentDataId) && this.item.FromPortId == this.fatherComponent.EntityPM.MainCarriageFromPortId && this.item.ToPortId == this.fatherComponent.EntityPM.MainCarriageFinalDestinationPortId && this.item.BranchId == this.fatherComponent.EntityPM.BranchId) {
            this.IsMatched = true;
        }
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

        else {
            var itemPM = this.fatherComponent.EntityPM.ShipmentConsoleShipments.filter(f => f.Id == this.Id)[0];
            if (itemPM != null) {
                this.fatherComponent.EntityPM.RemoveConsoleShipment(itemPM);
                this.fatherComponent.Save();
            }
        }
    }
}
