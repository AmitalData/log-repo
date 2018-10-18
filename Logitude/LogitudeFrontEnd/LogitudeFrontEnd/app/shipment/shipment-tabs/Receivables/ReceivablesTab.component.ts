import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {NgStyle} from 'angular2/common';
import {StringToColorPipe} from '../../../infrastructure/pipes/StringToColorPipe';
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton';
import {DecimalFormatter} from '../../../infrastructure/utilities/DecimalFormatter';

class ShipmentReceivableItem {
    public EntityPM: any;
    ChargesTypeName: string;
    PrepaidCollectId: string;
    MeasurementCode: string;
    CurrencyCode: string;
    UnitPrice: number;
    Quantity: number;
    TotalAmount: number;
    TotalAmountLocal: number;
    InvoiceNumber: string;
    InvoiceStatus: string;

    constructor(entity: any, shipmentPM: any) {

        this.EntityPM = entity;
        this.ChargesTypeName = entity.ChargesTypeName;
        this.PrepaidCollectId = entity.PrepaidCollectId;
        this.CurrencyCode = entity.CurrencyCode;
        this.MeasurementCode = entity.MeasurementCode;
        this.Quantity = entity.Quantity;
        this.UnitPrice = entity.UnitPrice;
        this.TotalAmount = entity.TotalAmount;
        this.TotalAmountLocal = entity.TotalAmountLocal;

        var invoicePM = shipmentPM.ShipmentARInvoices.filter(d => d.Id === this.EntityPM.ARInvoiceId)[0];
        if (invoicePM != null) {
            this.InvoiceNumber = invoicePM.InvoiceNumber;
            this.InvoiceStatus = invoicePM.StatusName;
        }
    }
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/ReceivablesTab.html',
    directives: [NgStyle, IconButton],
    pipes: [TextcodeTranslationPipe, StringToColorPipe],
})

export class ReceivablesComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public ItemsSource: ShipmentReceivableItem[];
    public IsGenerateButtonsVisible: boolean = false;

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    BuildItemsSource() {
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<ShipmentReceivableItem>();
        }

        else {
            this.ItemsSource = [];
        }

        this.EntityPM.ShipmentReceivables.forEach((item) => {
            this.ItemsSource.push(new ShipmentReceivableItem(item, this.EntityPM));
        })

        if (this.ItemsSource.length == 0) {
            this.IsGenerateButtonsVisible = true;
        }

        else {
            this.IsGenerateButtonsVisible = false;
        }
    }

    public OpenReceivablesCount: number = 0;
    public InvoicesCount: number = 0;
    public CreditNotesCount: number = 0;
    BuildSummaryData(){

        var myOpenReceivablesCount: number = 0;
        var myInvoicesCount: number = 0;
        var myCreditNotesCount: number = 0;

        this.EntityPM.ShipmentReceivables.forEach((item) => {
            if (item.ShipmentReceivableLineStatusCode == "OAMT") {
                myOpenReceivablesCount += 1;
            }
        })

        this.EntityPM.ShipmentARInvoices.forEach((item) => {
            if (item.InvoiceTypeCode == "IN" || item.InvoiceTypeCode == "MN") {
                myInvoicesCount += 1;
            }

            else if (item.InvoiceTypeCode == "CD") {
                myCreditNotesCount += 1;
            }
        })

        this.OpenReceivablesCount = myOpenReceivablesCount;
        this.InvoicesCount = myInvoicesCount;
        this.CreditNotesCount = myCreditNotesCount;
    }

    public IsByLocalCurrency: boolean = false;
    public SelectedCurrencyCode: string = "USD";
    public ProfitInSelectedCurrencyText: string = "N/A";
    public PayablesInSelectedCurrencyText: string = "N/A";
    public ReceivablesInSelectedCurrencyText: string = "N/A";
    public ProfitRate: string = "N/A";

    public ProfitInSelectedCurrency: number;
    public EstimateProfitInSelectedCurrency: number;
    public DefferenceInSelectedCurrencyText: string = "";
    public DefferenceColor: string = "#282E30";
    BuildProfitData() {

        var myProfitInSelectedCurrencyText = "N/A";
        var myPayablesInSelectedCurrencyText = "N/A";
        var myReceivablesInSelectedCurrencyText = "N/A";
        var myProfitRate = "N/A";
        
        var myDefferenceColor = "#282E30";
        var myDefferenceInSelectedCurrency = 0;
        var myDefferenceInSelectedCurrencyText = "";

        // Payables
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            if (this.EntityPM.ConnectedShipmentsPayablesCount > 0) {
                myPayablesInSelectedCurrencyText = DecimalFormatter.format(this.GetPayablesInSelectedCurrency(), 2);
            }
        }

        else if (this.EntityPM.ShipmentPayables.length > 0) {
            myPayablesInSelectedCurrencyText = DecimalFormatter.format(this.GetPayablesInSelectedCurrency(), 2);
        }

        // Receivables && Profit
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (this.EntityPM.ShipmentReceivables.length + this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                myProfitInSelectedCurrencyText = DecimalFormatter.format(this.GetProfitInSelectedCurrency(), 2);
                myReceivablesInSelectedCurrencyText = DecimalFormatter.format(this.GetReceivablesInSelectedCurrency(), 2);
            }
        }

        else if (this.EntityPM.ShipmentReceivables.length > 0) {
            myProfitInSelectedCurrencyText = DecimalFormatter.format(this.GetProfitInSelectedCurrency(), 2);
            myReceivablesInSelectedCurrencyText = DecimalFormatter.format(this.GetReceivablesInSelectedCurrency(), 2);
        }

        if (this.EntityPM.ProfitExchangeRate != null) {
            myProfitRate = DecimalFormatter.format(this.EntityPM.ProfitExchangeRate, 5);
        }

        this.ProfitInSelectedCurrencyText = myProfitInSelectedCurrencyText;
        this.PayablesInSelectedCurrencyText = myPayablesInSelectedCurrencyText;
        this.ReceivablesInSelectedCurrencyText = myReceivablesInSelectedCurrencyText;
        this.ProfitRate = myProfitRate;        
        this.SetProfitColor();

        this.ProfitInSelectedCurrency = this.GetProfitInSelectedCurrency();
        this.EstimateProfitInSelectedCurrency = this.GetEstimateProfitInSelectedCurrency();

        if (this.ProfitInSelectedCurrency != null && this.EstimateProfitInSelectedCurrency != null) {
            myDefferenceInSelectedCurrency = this.ProfitInSelectedCurrency - this.EstimateProfitInSelectedCurrency;
            myDefferenceInSelectedCurrencyText = DecimalFormatter.format(myDefferenceInSelectedCurrency, 2);

            if (myDefferenceInSelectedCurrency > 0) {
                myDefferenceColor = "#009161";
            }

            else if (myDefferenceInSelectedCurrency < 0) {
                myDefferenceColor = "#E53030";        
            }
        }

        this.DefferenceColor = myDefferenceColor;
        this.DefferenceInSelectedCurrencyText = myDefferenceInSelectedCurrencyText;
    }

    public ProfitColor: string = "#282E30";
    SetProfitColor() {

        var myProfitColor = "#282E30";
        var allReceivablesCount: number = 0;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            allReceivablesCount = this.EntityPM.ShipmentReceivables.length + this.EntityPM.ConnectedShipmentsReceivablesCount;
        }

        else {
            allReceivablesCount = this.EntityPM.ShipmentReceivables.length;
        }

        if (allReceivablesCount > 0) {

            var myPayables = this.GetPayablesInSelectedCurrency();
            var myReceivables = this.GetReceivablesInSelectedCurrency();

            if (myReceivables > myPayables) {
                myProfitColor = "#009161";
            }

            else if (myReceivables < myPayables) {
                myProfitColor = "#E53030";                
            }
        }

        this.ProfitColor = myProfitColor;
    }

    GetPayablesInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInLocalCurrency;
            }

            if (this.EntityPM.AccountedPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInLocalCurrency;
            }
        }

        else {
            if (this.EntityPM.OpenPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInProfitCurrency;
            }

            if (this.EntityPM.AccountedPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInProfitCurrency;
            }           
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }

    GetReceivablesInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInLocalCurrency;
            }

            if (this.EntityPM.AccountedReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInLocalCurrency;
            }           
        }

        else {
            if (this.EntityPM.OpenReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInProfitCurrency;
            }

            if (this.EntityPM.AccountedReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInProfitCurrency;
            }
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }

    GetProfitInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.ProfitInLocalCurrency;
        }

        else {
            myResult = this.EntityPM.ProfitInProfitCurrency;
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }

    GetEstimateProfitInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.EstimateProfitInLocalCurrency;
        }

        else {
            myResult = this.EntityPM.EstimateProfitInProfitCurrency;
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null && this.EntityPM !== undefined) {
            this.IsDataLoaded = true;
            this.BuildItemsSource();
            this.BuildSummaryData();
            this.BuildProfitData();
        }
    }

    onSelectCurrency(myArgs: string) {
        this.SelectedCurrencyCode = myArgs;

        if (myArgs == "NIS") {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;

        }

        this.BuildProfitData();
    }
}
