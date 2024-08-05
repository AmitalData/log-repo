import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoiceMultipleShipmentPM} from '../../../../Invoice/EntityPMs/APInvoiceMultipleShipmentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {NumbersPipe} from '../../../../Infrastructure/Pipes/NumbersPipe';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceTotalsClass, SummaryItem} from '../../../../Invoice/Args';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Output, EventEmitter } from '@angular/core';
import { InvoiceDomainService } from '../../../../Invoice/Services/InvoiceDomainService';
import { ShipmentPayablePM } from '../../../../Shipment/EntityPMs/ShipmentPayablePM';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { VatTypePercentageList } from '../../../../Common/EntityLists/VatTypePercentageList';
import { APInvoiceLinePM } from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { VatTypeListService } from '../../../../Common/Services/StandardLists/VatTypeListService';
import { VatTypeList } from '../../../../Common/EntityLists/VatTypeList';
import { APInvoiceMultipleShortPM } from '../../../../Invoice/EntityPMs/APInvoiceMultipleShortPM';

@Component({    
    templateUrl: './APInvoiceMultipleDetailsTabComponent.html',
})

export class APInvoiceMultipleDetailsTabComponent extends BaseComponent implements OnDestroy {
    @Output() SelectedValueChanged = new EventEmitter();
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    public ItemsSource: MultipleShipmentLine[] = [];
    public LocalCurrencyId: string;
    public LocalCurrencyCode: string;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    public apiQueryFilters: ApiQueryFilters = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private invoiceDomainService: InvoiceDomainService;
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       
        this.EntityPM = entityArgs.EntityPM;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.InitLOVFilters();
        this.InitializeServices();
        this.SetUIProperties();
        this.BuildScreenData();
        this.Listen();
        if (FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
            this.IsEditExchangeRateVisible = true;
        }
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildItemsSource();

                    if (this.editingShipmentRequested) {
                        this.RunEditShipment();
                    }

                    else if (this.loadShipmentsRequested) {
                        this.StartLoadingShipments();
                    }

                    else if (this.addPayablesRequested) {
                        this.StartAddingPayables();
                    }
                }

                this.editingShipmentRequested = false;
                this.loadShipmentsRequested = false;
                this.addPayablesRequested = false;
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildItemsSource();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private myPaymentTermListService: PaymentTermListService;
    InitializeServices() {
        this.myPaymentTermListService = new PaymentTermListService();
        this.invoiceDomainService = new InvoiceDomainService();
    }

    SetShipmentSearchApiQueryFilter(shipmentsLevelCods: string) {
        this.apiQueryFilters = new ApiQueryFilters();
        this.apiQueryFilters.PageIndex = 0;
        this.apiQueryFilters.PageSize = 10;
        this.apiQueryFilters.addAdditionalFilter("ShipmentLevelCode", shipmentsLevelCods, null, null, "InList", true, true, false, "string");
    }

    public IsEditingEnabled: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = InvoiceTool.IsEditingAPInvoiceEnabled(this.EntityPM);

        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, false);

        //this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isEditingEnabled);

        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, isEditingEnabled);
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_DueDate();
    }
    SetUIProperties_VatNumber() {
        var isFieldRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.VATNumber)) {
                isFieldRequired = true;
                isFieldRequired = true;
            }

            this.UIProperties.SetRequired("VATNumber", this.ObjectTableName, isFieldRequired);
        }
    }
    SetUIProperties_DueDate() {
        var AllowManuallyDueDate: boolean = false;

        if (SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }

        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }

        if (!this.IsEditingEnabled) {
            AllowManuallyDueDate = false;
        }

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    }

    BuildScreenData() {
        this.BuildItemsSource();
    }

    public SelectedItem: MultipleShipmentLine;
    BuildItemsSource() {
        this.ItemsSource = [];

        var list = this.EntityPM.InvoiceMultipleShipments;
        list = list.sort(function (a, b) { return a.IndexOrder == b.IndexOrder ? 0 : a.IndexOrder < b.IndexOrder ? -1 : 1; });

        list.forEach(item => {
            this.ItemsSource.push(new MultipleShipmentLine(item));
        });

        this.BuildTotalsCollection();
    }

    // Vendor
    get VendorDependencyProperty1() { return InvoiceTool.GetVendorPartnerTypes(); }

    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;
        }
    }

    get VendorName() { return this.EntityPM.VendorName; }
    set VendorName(value: string) {
        if (this.EntityPM.VendorName != value) {
            this.EntityPM.VendorName = value;
        }
    }

    get InvoiceNumber() { return this.EntityPM.InvoiceNumber; }
    set InvoiceNumber(value: string) {
        if (this.EntityPM.InvoiceNumber != value) {
            this.EntityPM.InvoiceNumber = value;
        }
    }

    get VATNumber() { return this.EntityPM.VATNumber; }
    set VATNumber(value: string) {
        if (this.EntityPM.VATNumber != value) {
            this.EntityPM.VATNumber = value;
            this.SetUIProperties_VatNumber();
        }
    }

    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(value: string) {
        if (this.EntityPM.InvoiceCurrencyId != value) {
            this.EntityPM.InvoiceCurrencyId = value;
        }
    }

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    set InvoiceCurrencyCode(value: string) {
        if (this.EntityPM.InvoiceCurrencyCode != value) {
            this.EntityPM.InvoiceCurrencyCode = value;
        }
    }

    get InvoiceCurrencyExchangeRate() { return this.EntityPM.InvoiceCurrencyExchangeRate; }
    set InvoiceCurrencyExchangeRate(value: number) {
        if (this.EntityPM.InvoiceCurrencyExchangeRate != value) {
            this.EntityPM.InvoiceCurrencyExchangeRate = AppTool.Round(value, 5);
            this.ComputeAllAmounts();
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM.ExchangeRateDate != value) {
            this.EntityPM.ExchangeRateDate = value;
        }
    }

    get RelativeRateDate() { return DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old") }

    get ProfitCurrencyId() { return this.EntityPM.ProfitCurrencyId; }
    set ProfitCurrencyId(value: string) {
        if (this.EntityPM.ProfitCurrencyId != value) {
            this.EntityPM.ProfitCurrencyId = value;
        }
    }

    get ProfitCurrencyExchangeRate() { return this.EntityPM.ProfitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(value: number) {
        if (this.EntityPM.ProfitCurrencyExchangeRate != value) {
            this.EntityPM.ProfitCurrencyExchangeRate = AppTool.Round(value, 5);
            this.ComputeAllAmounts();
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(value: string) {
        if (this.EntityPM.PaymentTermId != value) {
            this.EntityPM.PaymentTermId = value;
            InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);

            if (AppTool.IsNullOrEmpty(value)) {
                this.PaymentTermName = null;
            }

            else {
                this.myPaymentTermListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;

                        if (list != null) {
                            this.PaymentTermName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get PaymentTermName() { return this.EntityPM.PaymentTermName; }
    set PaymentTermName(value: string) {
        if (this.EntityPM.PaymentTermName != value) {
            this.EntityPM.PaymentTermName = value;
        }
    }

    get InvoiceDate() { return this.EntityPM.InvoiceDate; }
    set InvoiceDate(value: Date) {
        if (this.EntityPM.InvoiceDate != value) {
            this.EntityPM.InvoiceDate = value;
            InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(value: Date) {
        if (this.EntityPM.DueDate != value) {
            this.EntityPM.DueDate = value;
            InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
        }
    }

    get AmountInInvoiceCurrency() { return this.EntityPM.AmountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        if (this.EntityPM.AmountInInvoiceCurrency != value) {
            this.EntityPM.AmountInInvoiceCurrency = AppTool.Round(value, 2);
            this.EntityPM.InvoiceExpectedAmount = AppTool.Round(value, 2);
            this.ComputeAllAmounts();
        }
    }

    // Totals
    get IsCurrencyFilterVisible() {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.LocalCurrencyId != this.InvoiceCurrencyId) {
                myResult = true;
            }
        }

        return myResult;
    }

    private isTotalInLocalCurrency: boolean = false;
    get IsTotalInLocalCurrency() { return this.isTotalInLocalCurrency; }
    set IsTotalInLocalCurrency(value: boolean) {
        if (this.isTotalInLocalCurrency != value) {
            this.isTotalInLocalCurrency = value;
            this.BuildTotalsControl();
        }
    }

    public TotalsList: InvoiceTotalsClass[] = [];
    public SummaryItems: SummaryItem[] = [];
    ComputeAllAmounts() {
        this.AmountInLocalCurrency = AppTool.Round(this.AmountInInvoiceCurrency * this.InvoiceCurrencyExchangeRate, 2);

        if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
            this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
        }

        else {
            if (AppTool.IsNullOrZero(this.ProfitCurrencyExchangeRate)) {
                this.AmountInProfitCurrency = 0;
            }

            else {
                this.AmountInProfitCurrency = AppTool.Round(this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate,2);
            }
        }

        this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
        this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
        this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
    }
    ComputeTotals() {
        this.BuildTotalsCollection(true);
    }

    BuildTotalsCollection(isComputingTotals: boolean = false) {
        var totalsList: InvoiceTotalsClass[] = [];
        var myDataList = this.EntityPM.InvoiceMultipleShipments;

        var allLinesList: InvoiceTotalsClass[] = [];
        myDataList.forEach(item => {
            var vatsList: string[] = item.TotalVatsList;
            if (vatsList.length > 0) {
                vatsList.forEach(myString => {
                    if (!AppTool.IsNullOrEmpty(myString)) {
                        var lineArray: string[] = myString.split(':');

                        var myRecord: InvoiceTotalsClass = new InvoiceTotalsClass();
                        myRecord.VatTypeId = lineArray[0];
                        myRecord.VatTypePercentage = this.ConvertToDouble(lineArray[1]);
                        myRecord.LocalCurrencyAmount = this.ConvertToDouble(lineArray[2]);
                        myRecord.InvoiceCurrencyAmount = this.ConvertToDouble(lineArray[3]);
                        myRecord.ProfitCurrencyAmount = this.ConvertToDouble(lineArray[4]);
                        myRecord.VatTypeCell = lineArray[5];
                        allLinesList.push(myRecord);
                    }
                });
            }
        });

        var dataGroupList: InvoiceTotalsClass[] = [];
        allLinesList.filter(f => f.VatTypeId != null).forEach(item => {
            var dataGroupItem = dataGroupList.filter(d => d.VatTypeCell == item.VatTypeCell)[0];
            if (dataGroupItem == null) {
                dataGroupItem = new InvoiceTotalsClass();
                dataGroupItem.RowLabel = item.VatTypeCell;
                dataGroupItem.VatTypeCell = item.VatTypeCell;
                dataGroupItem.LocalCurrencyAmount = 0;
                dataGroupItem.InvoiceCurrencyAmount = 0;
                dataGroupList.push(dataGroupItem);
            }

            if (item.VatTypePercentage != null) {
                if (item.LocalCurrencyAmount != null) {
                    dataGroupItem.LocalCurrencyAmount = dataGroupItem.LocalCurrencyAmount + AppTool.Round((item.VatTypePercentage * item.LocalCurrencyAmount / 100), 2);
                }

                if (item.InvoiceCurrencyAmount != null) {
                    dataGroupItem.InvoiceCurrencyAmount = dataGroupItem.InvoiceCurrencyAmount + AppTool.Round((item.VatTypePercentage * item.InvoiceCurrencyAmount / 100), 2);
                }
            }
        });

        var subTotalItem = new InvoiceTotalsClass();
        var allTotalItem = new InvoiceTotalsClass();
        subTotalItem.RowLabel = TextCodeTranslator.Translate("APInvoice.S.Details.Subtotal");
        allTotalItem.RowLabel = TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency");
       
        if (dataGroupList.length > 0) {
            subTotalItem.LocalCurrencyAmount = ArrayTool.Sum(myDataList, "SubTotalInLocalCurrency");
            subTotalItem.InvoiceCurrencyAmount = ArrayTool.Sum(myDataList, "SubTotalInInvoiceCurrency");
            totalsList.push(subTotalItem);

            dataGroupList.forEach(item => {
                totalsList.push(item);                
            });

            allTotalItem.LocalCurrencyAmount = subTotalItem.LocalCurrencyAmount + AppTool.Round(ArrayTool.Sum(dataGroupList, "LocalCurrencyAmount"), 2);
            allTotalItem.InvoiceCurrencyAmount = subTotalItem.InvoiceCurrencyAmount + AppTool.Round(ArrayTool.Sum(dataGroupList, "InvoiceCurrencyAmount"), 2);
            totalsList.push(allTotalItem);
        }

        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = AppTool.Round(subTotalItem.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = AppTool.Round(subTotalItem.InvoiceCurrencyAmount, 2);
        }

        this.TotalsList = totalsList;
        this.BuildTotalsControl();
    }
    BuildTotalsControl() {

        this.BuildSummary();

        //this.SummaryItems = [];
        //var pipe = new NumbersPipe();
        //var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";

        //if (this.TotalsList.length == 0) {
        //    var myTotalItem = new SummaryItem();
        //    myTotalItem.Label = TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency");
        //    myTotalItem.Label += " " + selectedCurrencyCode;
        //    myTotalItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(0, "N2") : pipe.transform(0, "N2");
        //    this.SummaryItems.push(myTotalItem);
        //}

        //else {
        //    for (var i = 0; i < this.TotalsList.length; i++) {
        //        var item: InvoiceTotalsClass = this.TotalsList[i];

        //        var mySummaryItem = new SummaryItem();
        //        mySummaryItem.Label = item.RowLabel;
        //        mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
        //        this.SummaryItems.push(mySummaryItem);

        //        if (i + 2 < this.TotalsList.length) {
        //            var myOperatorItem = new SummaryItem();
        //            myOperatorItem.Value = "+";
        //            this.SummaryItems.push(myOperatorItem);
        //        }

        //        else if (i + 1 < this.TotalsList.length) {
        //            var myOperatorItem = new SummaryItem();
        //            myOperatorItem.Value = "=";
        //            this.SummaryItems.push(myOperatorItem);
        //        }

        //        else if (i + 1 == this.TotalsList.length) {
        //            mySummaryItem.Label += " " + selectedCurrencyCode;
        //        }
        //    }
        //}
    }
    BuildSummary() {
        this.SummaryItems = [];
        var pipe = new NumbersPipe();
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";

        if (this.EntityPM.TotalVATs.length > 0) {
            var mySummaryItem_Sub = new SummaryItem();
            mySummaryItem_Sub.Label = TextCodeTranslator.Translate("APInvoice.S.Details.Subtotal");
            mySummaryItem_Sub.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.SubTotalInLocalCurrency, "N2") : pipe.transform(this.EntityPM.SubTotalInInvoiceCurrency, "N2");
            this.SummaryItems.push(mySummaryItem_Sub);

            this.EntityPM.TotalVATs.forEach(item => {

                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);

                var mySummaryItem = new SummaryItem();
                mySummaryItem.Label = item.VatTypeCell;
                mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalVATAmount, "N2") : pipe.transform(item.InvoiceCurrencyVATAmount, "N2");
                this.SummaryItems.push(mySummaryItem);
            });

            var myOperatorItem = new SummaryItem();
            myOperatorItem.Value = "=";
            this.SummaryItems.push(myOperatorItem);
        }

        var mySummaryItem_All = new SummaryItem();
        mySummaryItem_All.Label = TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency") + " " + selectedCurrencyCode;
        mySummaryItem_All.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency_Summary, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency_Summary, "N2");
        this.SummaryItems.push(mySummaryItem_All);
    }

    ConvertToDouble(myString: string) {
        var myResult: number = 0;

        if (!AppTool.IsNullOrEmpty(myString)) {
            myResult = +myString;
        }

        return myResult;
    }


    get SubTotalInLocalCurrency() { return this.EntityPM.SubTotalInLocalCurrency; }
    set SubTotalInLocalCurrency(value: number) {
        if (this.EntityPM.SubTotalInLocalCurrency != value) {

            if (AppTool.IsNullOrEmpty(value)) {
                value = 0;
            }

            this.EntityPM.SubTotalInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        if (this.EntityPM.SubTotalInInvoiceCurrency != value) {
            this.EntityPM.SubTotalInInvoiceCurrency = AppTool.Round(value, 2);

            if (AppTool.IsNullOrEmpty(value)) {
                value = 0;
            }
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        if (this.EntityPM.AmountInLocalCurrency != value) {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        if (this.EntityPM.AmountInProfitCurrency != value) {
            this.EntityPM.AmountInProfitCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountDue() { return this.EntityPM.AmountDue; }
    set AmountDue(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDue != setValue) {
            this.EntityPM.AmountDue = setValue;
        }
    }

    get AmountDueInLocalCurrency() { return this.EntityPM.AmountDueInLocalCurrency; }
    set AmountDueInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDueInLocalCurrency != setValue) {
            this.EntityPM.AmountDueInLocalCurrency = setValue;
        }
    }

    get AmountDueInProfitCurrency() { return this.EntityPM.AmountDueInProfitCurrency; }
    set AmountDueInProfitCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDueInProfitCurrency != setValue) {
            this.EntityPM.AmountDueInProfitCurrency = setValue;
        }
    }

    private editingShipmentId: string = null;
    private editingShipmentRequested: boolean = false;
    AddShipment(item: ShipmentList) {
        if (item != null) {
            if (this.EntityPM.InvoiceMultipleShipments.filter(f => f.ShipmentId == item.Id).length == 0) {

                if (item.IsAccountingClosed) {
                    var window = new MessageWindow();
                    window.Show("Please open for accounting to enable adding");
                }

                else {
                    var itemPM = new APInvoiceMultipleShipmentPM(null);
                    itemPM.ShipmentId = item.Id;
                    itemPM.APInvoiceId = this.EntityPM.Id;
                    itemPM.Tenant = this.EntityPM.Tenant;
                    itemPM.House = item.House;
                    itemPM.Master = item.Master;
                    itemPM.LongMaster = item.LongMaster;
                    itemPM.ShipmentNumber = item.ShipmentNumber;
                    itemPM.ShipmentLevelCode = item.ShipmentLevelCode;
                    itemPM.PartnerType = item.ShipmentLevelCode == "C" ? "Agent:" : "Customer:";
                    itemPM.PartnerName = item.ShipmentLevelCode == "C" ? item.AgentName : item.CustomerName;
                    itemPM.MainCarriageCarrierName = item.MainCarriageCarrierName;
                    itemPM.ExpectedAmount = 0;
                    itemPM.OpenAmount = 0;
                    itemPM.TotalAmount = 0;
                    itemPM.TotalVATAmount = 0;
                    itemPM.AccountedAmount = 0;
                    itemPM.SubTotalInLocalCurrency = 0;
                    itemPM.SubTotalInInvoiceCurrency = 0;

                    var myIndexOrder = 0;
                    if (this.EntityPM.InvoiceMultipleShipments.length > 0) {
                        myIndexOrder = ArrayTool.Max(this.EntityPM.InvoiceMultipleShipments, "IndexOrder");
                        myIndexOrder += 1;
                    }

                    itemPM.IndexOrder = myIndexOrder;
                    itemPM.TotalVatsList = new Array<string>();

                    if (this.EntityPM.InvoiceCurrencyId == this.EntityPM.ProfitCurrencyId) {
                        itemPM.OpenAmount = item.OpenPayablesInProfitCurrency;
                        itemPM.ExpectedAmount = AppTool.Round(item.OpenPayablesInProfitCurrency + item.AccountedPayablesInProfitCurrency, 2);
                    }

                    else {
                        if (AppTool.IsNullOrZero(this.InvoiceCurrencyExchangeRate)) {
                            itemPM.OpenAmount = 0;
                            itemPM.ExpectedAmount = 0;
                        }

                        else {
                            itemPM.OpenAmount = AppTool.Round(item.OpenPayablesInLocalCurrency / this.InvoiceCurrencyExchangeRate, 2);
                            itemPM.ExpectedAmount = AppTool.Round((item.OpenPayablesInLocalCurrency + item.AccountedPayablesInLocalCurrency) / this.InvoiceCurrencyExchangeRate, 2);
                        }
                    }

                    this.EntityPM.AddAPInvoiceMultipleShipmentPM(itemPM);
                    this.BuildItemsSource();
                    this.ComputeTotals();
                    this.ComputeShipmentsNumbers();
                }
            }
        }
    }
    EditShipmentLine(item: MultipleShipmentLine) {
        if (!this.editingShipmentRequested) {
            this.editingShipmentId = item.ShipmentId;
            this.editingShipmentRequested = true;
            this.SaveChanges();
        }
    }
    RunEditShipment() {
        if (this.editingShipmentId) {
            var entityPM = this.EntityPM.InvoiceMultipleShipments.filter(f => f.ShipmentId == this.editingShipmentId)[0];
            if (entityPM) {
                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = "Edit Shipment Lines";
                logWindow.WindowArgs = { APInvoicePM: this.EntityPM, EntityShipmentPM: entityPM, IsEditingEnabled: this.IsEditingEnabled };

                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                    }
                });

                logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/EditMultipleShipmentComponent');
            }
        }
    }
    DeleteShipmentLine(item: MultipleShipmentLine) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this shipment line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.EntityPM.RemoveAPInvoiceMultipleShipmentPM(item.EntityPM);

                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.BuildItemsSource();
                    this.ComputeShipmentsNumbers();
                }

                else {
                    this.SaveChanges();
                }
            }
        });
    }
    ViewShipmentClicked(item: MultipleShipmentLine) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.ShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "A/P Invoice: " + this.EntityPM.InvoiceNumber });
            });
    }
    SaveChanges() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }

    public LevelCodeSelectedValue: string = "All";
    LevelCodeitemClicked(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {
            this.LevelCodeSelectedValue = itemValue;
            var shipmentsLevelCods = "";
            if (itemValue == "All") {
                this.apiQueryFilters = null;
            } else if (itemValue == "MasterAndDirect") {
                shipmentsLevelCods = "D,C";
                this.SetShipmentSearchApiQueryFilter(shipmentsLevelCods);
            } else if (itemValue == "HouseAndDirect") {
                shipmentsLevelCods = "D,H";
                this.SetShipmentSearchApiQueryFilter(shipmentsLevelCods);
            }           
        }
    }

    LevelCodeMouseOver(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {          
        }
    }

    LevelCodeMouseLeave(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {      
        }
    }

    ComputeShipmentsNumbers() {
        var shipmentsNumbers: string = "";

        this.ItemsSource.forEach(item => {
            if (AppTool.IsNullOrEmpty(shipmentsNumbers)) {
                shipmentsNumbers = item.ShipmentNumber;
            }

            else {
                shipmentsNumbers = shipmentsNumbers + ", " + item.ShipmentNumber;
            }
        });

        if (shipmentsNumbers.length > 1000) {
            shipmentsNumbers = shipmentsNumbers.substring(0, 1000);
        }

        this.EntityPM.ShipmentsNumbers = shipmentsNumbers;
    }

    private loadShipmentsRequested: boolean = false;
    AddShipmentsButtonClicked() {
        if (this.EntityPM.IsDirty) {
            this.loadShipmentsRequested = true;
            this.SaveChanges();
        }

        else {
            this.StartLoadingShipments();
        }        
    }
    private StartLoadingShipments() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.invoiceDomainService.GetShipmentsForMultipleAPInvoice(this.EntityPM.Id, this.VendorId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var shipments: ShipmentList[] = myResponse.Result;
                if (shipments.length > 0) {
                    shipments.forEach(item => {
                        this.AddShipment(item);
                    });
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private addPayablesRequested: boolean = false;
    AddPayablesButtonClicked(item: MultipleShipmentLine) {
        this.editingShipmentId = item.ShipmentId;

        if (this.EntityPM.IsDirty) {            
            this.addPayablesRequested = true;
            this.SaveChanges();
        }

        else {
            this.StartAddingPayables();
        } 
    }

    private StartAddingPayables() {
        if (this.editingShipmentId) {
            this.LoadEntity();
        }
    }

    public APInvoiceMultipleShortEntity: APInvoiceMultipleShortPM = null;
    private LoadEntity() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.invoiceDomainService.GetSingleAPInvoiceShortPM(this.EntityPM.Id, this.editingShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.APInvoiceMultipleShortEntity = myResponse.Result;

                if (this.APInvoiceMultipleShortEntity) {
                    this.LoadOpenPayables();
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private invoiceLinesAdded: boolean = false;
    private myOpenPayables: ShipmentPayablePM[] = [];
    private missingVATPayables: APInvoiceLinePM[] = [];
    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentageList[] = [];
    private LoadOpenPayables() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.missingVATPayables = [];
        this.invoiceDomainService.GetInvoiceOpenAmountPayables(this.editingShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.myOpenPayables = myResponse.Result;
            }

            var loadingDate = this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            var currencyRatesService: CurrencyRatesService = new CurrencyRatesService();    
            currencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse1: ServiceResponse) => {
                this.LastRatesList = myResponse1.Result;

                var commonDomainService: CommonDomainService = new CommonDomainService();
                commonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    this.VatTypePercentagesList = myResponse2.Result;
                    this.AddInvoiceLines();
                    this.CurrentSession.StopBusyIndicator();

                    if (this.invoiceLinesAdded) {
                        this.invoiceLinesAdded = false;
                        this.SaveAPInvoiceMultipleShortEntity();
                    }

                    else if (this.missingVATPayables.length > 0) {
                        var window: MessageWindow = new MessageWindow();
                        window.Show("Some of the payables in this shipment don't have a VAT type and will not be added.");
                    }
                });
            });
        });
    }
    private AddInvoiceLines() {
        var chargesTypeListService: ChargesTypeListService = new ChargesTypeListService();
        var vatTypeListService: VatTypeListService = new VatTypeListService();;

        this.myOpenPayables = this.myOpenPayables.filter(f => f.ShipmentPayableParentId == null && f.VendorId == this.VendorId);
        this.myOpenPayables.forEach(item => {
            if (this.APInvoiceMultipleShortEntity.InvoiceLines.filter(f => f.EntityPayableId == item.Id).length == 0) {
                var line = new APInvoiceLinePM(null);
                line.APInvoiceId = this.EntityPM.Id;
                line.Tenant = item.Tenant;
                line.ChargesTypeId = item.ChargesTypeId;
                line.ChargesTypeCode = item.ChargesTypeCode;
                line.ChargesTypeName = item.ChargesTypeName;
                line.EntityPayableId = item.Id;
                line.EntityId = item.ShipmentId;
                line.EntityReference = item.ShipmentNumber;
                line.VendorId = item.VendorId;
                line.VendorName = item.VendorName;
                line.ExpectedAmount = item.ExpectedAmount;
                line.OtherInvoicesAmounts = item.AccountedAmount;
                line.CorrectionAmount = item.CorrectionAmount;
                line.CorrectionNote = item.CorrectionNote;
                line.CorrectionByUserId = item.CorrectionByUserId;
                line.CorrectionDate = item.CorrectionDate;
                line.AmountTypeCode = item.ShipmentPayableAmountTypeCode;
                line.ForiegnCurrencyId = item.CurrencyId;
                line.ForiegnCurrencyCode = item.CurrencyCode;
                line.VatPercentage = this.GetVatTypePercentage(item.VatTypeId);
                line.ForiegnExchangeRate = this.GetCurrencyRate(item.CurrencyId);
                line.ForiegnCurrencyAmount = item.OpenAmount;
                line.LocalCurrencyAmount = line.ForiegnCurrencyAmount * line.ForiegnExchangeRate;
                line.OpenAmount = this.ComputeOpenAmount(item);
                
                if (line.ForiegnCurrencyId == this.ProfitCurrencyId) {
                    line.ProfitCurrencyAmount = line.ForiegnCurrencyAmount;
                }

                else {
                    line.ProfitCurrencyAmount = line.LocalCurrencyAmount / this.EntityPM.ProfitCurrencyExchangeRate;
                }

                var invoiceAmount: number = 0;
                if (line.ForiegnCurrencyId == this.EntityPM.InvoiceCurrencyId) {
                    invoiceAmount = line.ForiegnCurrencyAmount;
                }

                else {
                    invoiceAmount = line.LocalCurrencyAmount / this.EntityPM.InvoiceCurrencyExchangeRate;
                }

                line.InvoiceCurrencyAmount = AppTool.Round(invoiceAmount, 2);

                chargesTypeListService.getSingleFromCache(line.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;
                        if (list) {
                            line.Description = list.EnglishName;
                            line.LocalDescription = list.LocalName;

                            if (AppTool.IsNullOrEmpty(line.VatTypeId)) {
                                line.VatTypeId = list.VatTypeId;
                            }
                        }
                    }
                });

                if (!AppTool.IsNullOrEmpty(line.VatTypeId)) {
                    vatTypeListService.getSingleFromCache(line.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list_VAT: VatTypeList = myResponse.Result;
                            if (list_VAT) {
                                line.VatTypeName = list_VAT.EnglishName;
                                line.VatIsMultiPercentage = list_VAT.IsMultiPercentage;

                                if (!list_VAT.IsMultiPercentage) {
                                    line.VatPercentage = this.GetVatTypePercentage(line.VatTypeId);
                                }
                            }
                        }
                    });
                }

                if (AppTool.IsNullOrEmpty(line.VatTypeId)) {
                    this.missingVATPayables.push(line);
                }

                else {
                    this.APInvoiceMultipleShortEntity.AddInvoiceLinePM(line);
                    this.invoiceLinesAdded = true;
                }
            }
        });
    }
    private GetVatTypePercentage(myVatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentage = this.VatTypePercentagesList.filter(d => d.VatTypeId == myVatTypeId)[0];
        if (vatTypePercentage != null) {
            myResult = vatTypePercentage.Percentage;
        }

        return myResult;
    }
    private GetCurrencyRate(myCurrencyId: string) {
        var myResult: number = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    private SaveAPInvoiceMultipleShortEntity() {
        this.invoiceDomainService.PutSingleAPInvoiceShortPM(this.APInvoiceMultipleShortEntity).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (this.missingVATPayables.length > 0) {
                    var window: MessageWindow = new MessageWindow();
                    window.Show("Some of the payables in this shipment don't have a VAT type and will not be added.");
                    window.WindowClosed.subscribe(s => {
                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                    });
                }

                else {
                    if (this.CurrentSession.CurrentEditComponent) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                } 
            }

            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;                
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    ComputeOpenAmount(payable: ShipmentPayablePM): number {
        var expect: number = payable.ExpectedAmount;
        var amount: number = payable.OpenAmount == null ? 0 : payable.OpenAmount;
        var others: number = payable.AccountedAmount == null ? 0 : payable.AccountedAmount;
        var corre: number = payable.CorrectionAmount == null ? 0 : payable.CorrectionAmount;
        var open: number = expect - others - amount - corre;

        return AppTool.Round(open, 2);
    }
}

export class MultipleShipmentLine {
    public EntityPM: APInvoiceMultipleShipmentPM;
    constructor(item: APInvoiceMultipleShipmentPM) {
        this.EntityPM = item;
    }

    get IndexOrder() { return this.EntityPM.IndexOrder; }
    get APInvoiceId() { return this.EntityPM.APInvoiceId; }
    get ShipmentId() { return this.EntityPM.ShipmentId; }
    get House() { return this.EntityPM.House; }
    get Master() { return this.EntityPM.Master; }
    get LongMaster() { return this.EntityPM.LongMaster; }
    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }
    get PartnerType() { return this.EntityPM.PartnerType; }
    get PartnerName() { return this.EntityPM.PartnerName; }
    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    get ExpectedAmount() { return this.EntityPM.ExpectedAmount; }
    get AccountedAmount() { return this.EntityPM.AccountedAmount; }
    get OpenAmount() { return this.EntityPM.OpenAmount; }
    get TotalAmount() { return this.EntityPM.TotalAmount; }
    get TotalVATAmount() { return this.EntityPM.TotalVATAmount; }
    get SubTotalInLocalCurrency() { return this.EntityPM.SubTotalInLocalCurrency; }
    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
}
