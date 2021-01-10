import {Component} from '@angular/core';
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
import {ShipmentTool, ShipmentGenerator} from '../../../../../Shipment/Tools';

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
    constructor() {
        this.myService = new QuoteListService();        
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.AllRates = args['AllRates'];
        this.LoadData();
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
                        this.ItemsSource.push(new QuoteItem(item, this.EntityPM.GrossWeight));
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
        if (this.SelectedItem) {
            if (this.SelectedItem.StageName == "Used" || this.SelectedItem.StageName == "Accepted") {
                this.LoadQuotePM(this.SelectedItem.Id);
            }

            else {
                var messageWindow = new MessageWindow();
                messageWindow.Height = 150;
                messageWindow.Show(TextCodeTranslator.Translate("Shipment.M.GenerateIsAvailableAfterQuoteApproval"));
            }
        }
    }

    LoadQuotePM(QuoteId: string) {
        if (!AppTool.IsNullOrEmpty(QuoteId)) {

            this.CurrentSession.StartBusyIndicatorLoading();

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

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}
class QuoteItem {
    public Entity: QuoteList = null;
    private shipmentGrossWeight: number = 0;
    constructor(entity: QuoteList, shipmentGrossWeight: number) {
        this.Entity = entity;
        this.shipmentGrossWeight = shipmentGrossWeight;
        this.SetDiffernece();
        this.SetStageBackground();
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
