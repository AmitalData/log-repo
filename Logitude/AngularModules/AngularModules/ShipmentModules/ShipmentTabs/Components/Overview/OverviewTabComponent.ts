declare var window: any;
import { Component, OnDestroy } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './OverviewTabComponent.html',
})

export class OverviewTabComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ObjectTableName: string = null;
    public ContainersList: Container[] = [];
    public FollowupsList: FollowupClass[] = [];
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public entityResourceService: EntityResourceService) {
        entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(response=> {
           
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            this.ObjectTableName = this.entityArgs.ObjectTableName;
            this.Listen();

            if (this.EntityPM != null) {
                this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

                this.BuildCargoData();
                this.InitializeMoneyData();
                this.BuildFollowups();
                this.BuildNotesList();           
            }
        });
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;    
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "FollowupsChanged") {
                    this.BuildFollowups();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHOV" || tabCode == "JHOV") {
                    this.BuildCargoData();
                    this.BuildMoneyData();
                    this.BuildNotesList();
                    this.BuildFollowups();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    // Cargo Information    
    get Volume() { return this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume; }
    get GrossWeight() { return this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight; }
    get ChargeableWeight() { return this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight; }
    get NumberOfPackages() { return this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages; }
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }      
    get LongMaster() { return this.EntityPM.LongMaster; }
    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    get MasterDepartureDate() { return this.EntityPM.MainCarriageATD != null ? this.EntityPM.MainCarriageATD : this.EntityPM.MainCarriageETD; }
    get ConnectedHousesCount() { return this.EntityPM.ShipmentConsoleShipments.length; }

    public MasterLabel: string = "";
    public CarrierLabel: string = "";
    public CarrierNoLabel: string = "";
    public CarrierDateLabel: string = "";
    public MasterDataLableWidth: string = "70px";
    BuildCargoData() {

        if (this.IsFCLEntity) {
            this.ContainersList = [];

            var list: Container[] = [];
            this.EntityPM.ShipmentPackages.filter(f => f.IsContainer == true).forEach((item) => {
                var myContainer: Container = list.filter(f => f.PackageTypeId == item.PackageTypeId)[0];
                if (myContainer == null) {
                    myContainer = new Container();
                    myContainer.Quantity = AppTool.IsNullOrEmpty(item.Quantity) ? 0 : item.Quantity;
                    myContainer.PackageTypeId = item.PackageTypeId;
                    myContainer.PackageTypeName = item.PackageTypeName;
                    list.push(myContainer);
                }

                else {
                    if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                        myContainer.Quantity += item.Quantity;
                    }
                }
            });

            this.ContainersList = list;
        }

        switch (this.EntityPM.TransportModeId) {

            case "A": {
                this.MasterDataLableWidth = "70px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.MAWB");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.Flight");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.FlightNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.FlightDate");
                break;
            }

            case "O": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.OBL");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.ShippingLine");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.VoyageNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.VoyageDate");
                break;
            }

            case "I": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.CMR/RWB#");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.Trucker");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.TruckNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.TruckerDate");
                break;
            }
        }
    }

    // Money Information
    public IsProfitAreaVisible: boolean = false;
    public IsCurrencyFilterVisible: boolean = false;
    public IsByLocalCurrency: boolean = false;
    public SelectedCurrencyCode: string = null;
    get LocalCurrencyCode() { return SessionLocator.LocalCurrencyCode; }
    get ProfitCurrencyCode() { return this.EntityPM.ProfitCurrencyCode; }

    public ARInvoices: number = 0;
    public APInvoices: number = 0;
    public OpenReceivables: number = 0;
    public OpenPayables: number = 0;
    public Profit: number = 0;
    public TotalAR: number = 0;
    public TotalAP: number = 0;
    public ProfitColor: string = "#282E30";
    InitializeMoneyData() {

        if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Profit")) {
            this.IsProfitAreaVisible = true;
        }

        this.IsCurrencyFilterVisible = SessionLocator.LocalCurrencyId == this.EntityPM.ProfitCurrencyId ? false : true;
        this.SelectedCurrencyCode = this.EntityPM.ProfitCurrencyCode;

        this.BuildMoneyData();
    }

    BuildMoneyData() {

        this.ARInvoices = 0;
        this.APInvoices = 0;
        this.OpenReceivables = 0;
        this.OpenPayables = 0;
        this.Profit = 0;

        if (this.EntityPM != null && this.EntityPM !== undefined) {

        // StatusCode
            if (this.IsByLocalCurrency) {
                this.OpenPayables = this.EntityPM.OpenPayablesInLocalCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInLocalCurrency;
                this.ARInvoices = ArrayTool.Sum(this.EntityPM.ShipmentARInvoices.filter(f => f.StatusCode != 'DR'), "AmountInLocalCurrency");

                // Bug 70465: Money Information - open Recievables
                //this.ARInvoices = ArrayTool.Sum(this.EntityPM.ShipmentARInvoices, "AmountInLocalCurrency");
                //this.APInvoices = ArrayTool.Sum(this.EntityPM.ShipmentAPInvoices, "GrandTotalInLocalCurrency");
                this.APInvoices = this.EntityPM.AccountedPayablesInLocalCurrency;            
                this.Profit = this.EntityPM.ProfitInLocalCurrency;                
            }

            else {

                this.OpenPayables = this.EntityPM.OpenPayablesInProfitCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInProfitCurrency;
                this.ARInvoices = ArrayTool.Sum(this.EntityPM.ShipmentARInvoices.filter(f => f.StatusCode != 'DR'), "AmountInProfitCurrency");

                //this.ARInvoices = ArrayTool.Sum(this.EntityPM.ShipmentARInvoices, "AmountInProfitCurrency");
                //this.APInvoices = ArrayTool.Sum(this.EntityPM.ShipmentAPInvoices, "GrandTotalInProfitCurrency");
                this.APInvoices = this.EntityPM.AccountedPayablesInProfitCurrency;
                this.Profit = this.EntityPM.ProfitInProfitCurrency;                
            }

            this.TotalAR = this.ARInvoices + this.OpenReceivables;
            this.TotalAP = this.APInvoices + this.OpenPayables;

            if (this.Profit > 0) {
                this.ProfitColor = "#009161";
            }

            else if (this.Profit < 0) {
                this.ProfitColor = "#E53030";
            }

            else {
                this.ProfitColor = "#282E30";
            }
        }
    }

    // Followup
    public FollowupsIconPath: string = "./_Resources/Images/Icons/Followups/Followup.png";
    BuildFollowups() {
        this.FollowupsList = [];

        this.EntityPM.FollowUps.filter(f => f.Done == false).forEach(item => {
            this.FollowupsList.push(new FollowupClass(item));
        });

        this.SetFollowupsIconPath();
    }
    SetFollowupsIconPath() {
        if (this.FollowupsList.length == 0) {
            this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        }

        else {
            if (this.FollowupsList.filter(f => f.IsOld == true).length > 0) {
                this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup_Red.png";
            }

            else {
                this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup_Black.png";
            }
        }
    }

    // Notes    
    public NotesList: NoteItem[] = [];
    get Notes() { return this.EntityPM.Notes; }
    BuildNotesList() {
        this.NotesList = [];

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.EntityPM.ShipperNote });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.EntityPM.ConsigneeNote });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.AgentNote)) {
            this.NotesList.push({ Header: "Agent", Notes: this.EntityPM.AgentNote });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomAgentExportNote)) {
            this.NotesList.push({ Header: "Custom Agent Export", Notes: this.EntityPM.CustomAgentExportNote });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomAgentImportNote)) {
            this.NotesList.push({ Header: "Custom Agent Import", Notes: this.EntityPM.CustomAgentImportNote });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Notify1Note)) {
            this.NotesList.push({ Header: "Notify1", Notes: this.EntityPM.Notify1Note });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Notify2Note)) {
            this.NotesList.push({ Header: "Notify2", Notes: this.EntityPM.Notify2Note });
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeNotImporterNote)) {
            this.NotesList.push({ Header: "Consignee Not Importer", Notes: this.EntityPM.ConsigneeNotImporterNote });
        }
       
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperNotExporterNote)) {
            this.NotesList.push({ Header: "Shipper Not Exporter", Notes: this.EntityPM.ShipperNotExporterNote });
        }          
    }

    onSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;

        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;

        }

        this.BuildMoneyData();
    }

    ConnectedHousesClicked() {
        if (this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(p => p.Code == "SHCO")[0] != null) {
            this.CurrentSession.CurrentEditComponent.SelectionChanged(this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(p => p.Code == "SHCO")[0]);
        }
    }
}
class Container {
    public Quantity: number;
    public PackageTypeId: string;
    public PackageTypeName: string;
}
class FollowupClass {
    public EntityPM: ShipmentFollowUpPM;
    public ItemId: string = null;
    public ItemTooltipId: string = null;
    public Done: boolean = false;
    public Date: Date = null;
    public Name: string = null;
    public Notes: string = null;
    public IconPath: string = null;
    public TextColor: string = null;
    public IsOld: boolean = false;
    public Background: string = "white";
    public ListItemHeight: number = 40;
    public TooltipHeight: number = 130;
    public TooltipWidth: number = 270;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: ShipmentFollowUpPM) {
        this.EntityPM = entityPM;
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;

        this.Done = entityPM.Done;
        this.Date = entityPM.Date;
        this.Name = entityPM.EventTypeFollowUpName;
        this.Notes = entityPM.Notes;

        if (this.Date) {
            if (DateTool.GetDateParts(this.Date).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }

        this.SetIconPath();
    }

    SetIconPath() {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";

        if (this.Done) {
            iconPath = "./_Resources/Images/Icons/Followups/Done.png";
        }

        else if (this.Name) {
            var name = this.Name.toLowerCase();

            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
            }

            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
            }
        }

        this.IconPath = iconPath;
    }

    public IsShowTooltip: boolean = false;
    ShowTooltip(isShowTooltip: boolean) {
        if (!AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left - this.TooltipWidth + 5) + 'px';
            }

            this.IsShowTooltip = isShowTooltip;
        }
    }
}
interface NoteItem {
    Header: string,
    Notes: string,
}
