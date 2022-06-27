import { Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {QuotePM} from '../../../../../Quote/EntityPMs/QuotePM';
import {QuoteList} from '../../../../../Quote/EntityLists/QuoteList';
import {QuotePMService} from '../../../../../Quote/Services/StandardPMs/QuotePMService';
import {QuoteListService} from '../../../../../Quote/Services/StandardLists/QuoteListService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LastRate} from '../../../../../Common/Services/CurrencyRatesService';
import {ShipmentGenerator} from '../../../../../Shipment/Tools';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ShipmentDomainService } from '../../../../../Shipment/Services/ShipmentDomainService';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ShipmentPMService } from '../../../../../Shipment/Services/StandardPMs/ShipmentPMService';

@Component({    
    templateUrl: './QuotesComponent.html',
})

export class QuotesComponent {
    public BaseQuote: QuotePM;
    public EntityPM: ShipmentPM;
    public AllRates: LastRate[] = [];
    public ItemsSource: QuoteItem[] = [];
    public IsNoData: boolean = false;
    private myService: QuoteListService;
    private CurrentSession = SessionLocator.SelectedSession;
    private shipmentDomainService: ShipmentDomainService;
    private EntityArgs: EntityArgs;
    public ExpirationDateComparisonDate: Date;
    constructor() {
        this.myService = new QuoteListService();
        this.shipmentDomainService = new ShipmentDomainService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.AllRates = args['AllRates'];
        this.EntityArgs = args['EntityArgs'];
        this.SetExpirationDateComparisonDate();
        this.LoadData();        
    }
    SetExpirationDateComparisonDate() {
        var myExpiredDateFilter: Date;
        if (this.EntityPM.MainCarriageATD != null) {
            myExpiredDateFilter = this.EntityPM.MainCarriageATD;
        }

        else if (this.EntityPM.MainCarriageETD != null) {
            myExpiredDateFilter = this.EntityPM.MainCarriageETD;
        }

        else {
            myExpiredDateFilter = DateTool.GetCurrentDateAsUtc();
        }

        this.ExpirationDateComparisonDate = myExpiredDateFilter;
    }

    private selectedItem: QuoteItem = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: QuoteItem) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
        }
    }

    private isShowingUsedSpotRateQuotes: boolean = false;
    get IsShowingUsedSpotRateQuotes() { return this.isShowingUsedSpotRateQuotes; }
    set IsShowingUsedSpotRateQuotes(value: boolean) {
        if (this.isShowingUsedSpotRateQuotes != value) {
            this.isShowingUsedSpotRateQuotes = value;
            this.LoadData();
        }
    }

    private isShowingExpiredQuotes: boolean = false;
    get IsShowingExpiredQuotes() { return this.isShowingExpiredQuotes; }
    set IsShowingExpiredQuotes(value: boolean) {
        if (this.isShowingExpiredQuotes != value) {
            this.isShowingExpiredQuotes = value;
            this.LoadData();
        }
    }

    private isGeneratePayables: boolean = true;
    get IsGeneratePayables() { return this.isGeneratePayables; }
    set IsGeneratePayables(value: boolean) {
        if (this.isGeneratePayables != value) {
            this.isGeneratePayables = value;
        }
    }

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.IsNoData = false;
        this.ItemsSource = [];        
        this.BaseQuote = null;
        this.SelectedItem = null;

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "OpenDate";
        filters.SortDirection = "Descending";        

        var myToDate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        myToDate.setUTCHours(23);
        myToDate.setUTCMinutes(59);
        myToDate.setUTCSeconds(59);

        filters.addAdditionalFilter("DirectionId", this.EntityPM.DirectionId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", this.EntityPM.TransportModeId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("ShipmentTypeId", this.EntityPM.ShipmentTypeId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("CustomerId", this.EntityPM.CustomerId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("FromPortId", this.EntityPM.MainCarriageFromPortId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("ToPortId", this.EntityPM.MainCarriageFinalDestinationPortId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("IsShowingUsedSpotRateQuotes", this.IsShowingUsedSpotRateQuotes, null, null, "Equals", true, false, false, "Boolean");
        filters.addAdditionalFilter("StartDate", myToDate, null, null, "LessThanOrEqual", false, false, false, "Date");
        filters.addAdditionalFilter("IsShowingExpiredQuotes", this.IsShowingExpiredQuotes, null, null, "Equals", true, false, false, "Boolean");

        if (!this.IsShowingExpiredQuotes) {
            filters.addAdditionalFilter("ExpirationDate", this.ExpirationDateComparisonDate, null, null, "GreaterThanOrEqual", false, false, false, "Date");
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
            filters.addAdditionalFilter("RoutingRatesAgentId", this.EntityPM.AgentId, null, null, "StartsWith", true, false, false, "string");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.IsNoData = true;
                this.ItemsSource = [];
            }

            else {
                if (myResponse.HasError) {

                }

                else {
                    var list: QuoteList[] = myResponse.Result;

                    if (list.length == 0) {
                        this.IsNoData = true;
                    }

                    list.forEach(item => {
                        this.ItemsSource.push(new QuoteItem(item, this.EntityPM));
                    });
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    ViewQuoteClicked(QuoteId: string) {
        if (!AppTool.IsNullOrEmpty(QuoteId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Quote";
            logWindow.IsFillScreen = true;

            var isSaveEditSuccess = false;
            logWindow.ComponentLoaded.subscribe(comp => {
                comp.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    isSaveEditSuccess = isSaveSuccess;
                });
            });

            logWindow.WindowClosed.subscribe(s => {
                if (isSaveEditSuccess) {
                    this.LoadData();
                }
            });

            logWindow.ShowEditComponent(QuoteId, "Quote");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    GenerateButtonClicked() {
        if (this.SelectedItem == null) {
            return;
        }

        if (this.SelectedItem.StageName != "Used" && this.SelectedItem.StageName != "Accepted") {
            var messageWindow = new MessageWindow();
            messageWindow.Height = 150;
            messageWindow.Show(TextCodeTranslator.Translate("Shipment.M.GenerateIsAvailableAfterQuoteApproval"));
            return;
        }

        var isPayablesConnectedToInvoice = this.EntityPM.ShipmentPayables.filter(f => (f.ShipmentPayableLineStatusCode == 'PACC' || f.ShipmentPayableLineStatusCode == 'ACCT') && f.QuoteChargeId != null).length > 0;
        var isReceivablesConnectedToInvoice = this.EntityPM.ShipmentReceivables.filter(f => (f.ShipmentReceivableLineStatusCode == 'ACCT' || f.ShipmentReceivableLineStatusCode == 'DRFT') && f.QuoteChargeId != null).length > 0;
         if (this.EntityPM.QuoteId != null) {
            if (isPayablesConnectedToInvoice|| isReceivablesConnectedToInvoice) {
                var messageWindow = new MessageWindow();
                messageWindow.Width = 450;
                messageWindow.Show("You cannot connect a quote to this shipment while some Receivables/Payables generated from a different quote are connected to an invoice.");
            }
            else if (this.EntityPM.QuoteId != null) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("There is already a quote connected to this shipment. Connecting this new one will cause the previous quote to be disconnected. Please confirm.");
                confirmWindow.Width = 450;
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.DisconnectQuote();
                    }
                });
            }
        }
        else {
            this.GenerateReceivablesPayables();
        }
    }

    GenerateReceivablesPayables() {
        this.LoadQuotePM(this.SelectedItem.Id);
    }

    DisconnectQuote() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.shipmentDomainService.DisconnectQuote(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ReloadShipment();
                    this.CurrentSession.FireEvent("LoadConnectedShipments");
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
    
    private ReloadShipment() {
        var myService: ShipmentPMService = new ShipmentPMService();
        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                this.EntityPM = myResponse.Result;
                this.EntityArgs.EditComponent.EntityPM = this.EntityPM;
                this.GenerateReceivablesPayables();
            }
        });
    }

    LoadQuotePM(QuoteId: string) {
        if (!AppTool.IsNullOrEmpty(QuoteId)) {

            var myService = new QuotePMService();
            myService.get(QuoteId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.BaseQuote = myResponse.Result;

                    if (this.BaseQuote) {
                        this.Generate();
                    }

                    else {
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    }

    Generate() {
        if (this.BaseQuote) {

            this.EntityPM.QuoteId = this.BaseQuote.Id;
            this.EntityPM.QuoteNumber = this.BaseQuote.QuoteNumber;

            var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);

            Generator.GenerateReceivablesFromQuote(this.BaseQuote);           

            if (this.IsGeneratePayables) {
                Generator.GeneratePayablesFromQuote(this.BaseQuote);                
            }
            this.EntityArgs.EditComponent.SaveChanges();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}
class QuoteItem {
    public Entity: QuoteList = null;
    private shipmentGrossWeight: number = 0;
    private shipmentPM: ShipmentPM;
    constructor(entity: QuoteList, shipment: ShipmentPM) {
        this.Entity = entity;
        this.shipmentPM = shipment;
        this.shipmentGrossWeight = shipment.GrossWeight;
        this.SetDiffernece();
        this.SetStageBackground();
        this.SetValidByDate();
    }

    get Id() { return this.Entity.Id; }
    get QuoteNumber() { return this.Entity.QuoteNumber; }
    get OpenDate() { return this.Entity.OpenDate; }
    get ExpirationDate() { return this.Entity.ExpirationDate; }
    get StageName() { return this.Entity.StageName; }
    get StartDate() { return this.Entity.StartDate; }
    get QuoteTypeName() { return this.Entity.QuoteTypeName; }
    get CarrierName() { return this.Entity.CarrierName; }
    get ChargeableWeight() { return this.Entity.ChargeableWeight; }
    get GrossWeight() { return this.Entity.GrossWeight; }
    get UsageCount() { return this.Entity.UsageCount == 0 ? null : this.Entity.UsageCount; }
    get Notes() { return this.Entity.Notes; }
    get ValidBy() { return !AppTool.IsNullOrEmpty(this.Entity.ValidByTypeName) ? this.Entity.ValidByTypeName : "Today"; }

    public ValidByDate: Date;
    private SetValidByDate() {
        if (this.shipmentPM.MainCarriageATD != null) {
            this.ValidByDate = this.shipmentPM.MainCarriageATD;
        }

        else if (this.shipmentPM.MainCarriageETD != null) {
            this.ValidByDate = this.shipmentPM.MainCarriageETD;
        }

        else {
            this.ValidByDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        }
    }    

    public WeightDiffernece: number = 0;
    SetDiffernece() {
        var shipmentWeight = AppTool.IsNullOrEmpty(this.shipmentGrossWeight) ? 0 : this.shipmentGrossWeight;
        var quoteWeight = AppTool.IsNullOrEmpty(this.GrossWeight) ? 0 : this.GrossWeight;
        this.WeightDiffernece = shipmentWeight - quoteWeight;
    }

    public StageBackground: string = "transparent";
    SetStageBackground() {
        if (this.StageName == "Used" || this.StageName == "Accepted") {
            this.StageBackground = "transparent";
        }

        else {
            this.StageBackground = "rgba(255, 171, 3, 0.6)";
        }
    }
}
