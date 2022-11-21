import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { APInvoicePM } from '../../../Invoice/EntityPMs/APInvoicePM';
import { InvoiceTool } from '../../../Invoice/Tools';
import { AppTool, ArrayTool, DateTool } from '../../../Infrastructure/Tools';
import { PaymentTermListService } from '../../../Common/Services/StandardLists/PaymentTermListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { PaymentTermList } from '../../../Common/EntityLists/PaymentTermList';
import { CardList } from '../../../Common/EntityLists/CardList';
import { InvoiceDomainService } from '../../../Invoice/Services/InvoiceDomainService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';
import { CurrencyRatesService, LastRate } from '../../../Common/Services/CurrencyRatesService';
import { VatTypePercentagePM } from '../../../Common/EntityPMs/VatTypePercentagePM';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { CardPMService } from '../../../Common/Services/StandardPMs/CardPMService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { APInvoiceLinePM } from '../../../Invoice/EntityPMs/APInvoiceLinePM';
import { ChargesTypePMService } from '../../../Common/Services/StandardPMs/ChargesTypePMService';
import { VatTypePMService } from '../../../Common/Services/StandardPMs/VatTypePMService';


@Component({
    selector: 'CopyInvoiceComponent',

    templateUrl: './CopyInvoiceComponent.html',
})

export class CopyInvoiceComponent extends BaseComponent implements OnInit  {
    public EntityPM: APInvoicePM;
    public DataContext: any = this;
    public ObjectTableName: string = "APInvoice";
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public IsAccountingActivated = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    public errors: string[] = [];
    public LocalCurrencyId: string;
    public WaitingForApprovalStatusCode: string = "WA";
    private VatTypePercentagesList: VatTypePercentagePM[] = [];

    // services
    private paymentTermListService: PaymentTermListService = new PaymentTermListService();
    private invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
    private cardPMService: CardPMService = new CardPMService();
    private chargesTypePMService: ChargesTypePMService = new ChargesTypePMService();
    private vatTypePMService: VatTypePMService = new VatTypePMService();
    private cardListService: CardListService = new CardListService();
    private gLAccountPMService: GLAccountPMService = new GLAccountPMService();
    private currencyListService: CurrencyListService = new CurrencyListService();

    DisplayFieldsFromList: string;
    DisplayLocalFieldsFromList: string;
    VendorLovSizeForFullAccounting: number;

    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        this.GetVatTypePercentegeListByDates();
        this.InitializeVendorLov();

    }

    private InitializeVendorLov() {
        if (this.IsAccountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.VendorLovSizeForFullAccounting = 550;
        }
    }

    GetVatTypePercentegeListByDates() {
        return new Promise(resolve => {
            var loadingDate = this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }
            var commonDomainService: CommonDomainService = new CommonDomainService();
            commonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                if (!myResponse2.HasError) {
                    this.VatTypePercentagesList = myResponse2.Result;
                    resolve(myResponse2.Result);
                }
            });
        });
    }

    public forceFocus: boolean = false;
    ngOnInit() {
        var t = setTimeout(() => { this.forceFocus = true; }, 1);
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.APInvoicePM;
            this.SetDefaultValues();
            this.SetUIProperties();
            this.LoadData();
            this.IsResourcesReady = true;
        }
    }

    SetDefaultValues() {
        this.SetVendorIdForActiveCard(this.EntityPM.VendorId);
        this.AmountInInvoiceCurrency = this.EntityPM.AmountInInvoiceCurrency;
        this.BranchId = this.EntityPM.BranchId;
        this.InternalNotes = this.EntityPM.InternalNotes;
        var todayDate = DateTool.GetCurrentDateAsUtc();
        this.AccountingDate = todayDate;
    }

    SetUIProperties() {
        var isVatNumberRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.VATNumber)) {
                isVatNumberRequired = true;
            }
        }

        this.UIProperties.SetRequired("VATNumber", "APInvoice", isVatNumberRequired);
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, false);
    }

    // Load Data
    private LastRatesList: LastRate[] = [];
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var currencyRatesService = new CurrencyRatesService();
        var commonDomainService = new CommonDomainService();

        var loadingDate = this.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.GetCurrencyExchangeRate(currencyRatesService, loadingDate, commonDomainService);
    }

    private GetCurrencyExchangeRate(currencyRatesService: CurrencyRatesService, loadingDate: Date, commonDomainService: CommonDomainService) {
        currencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, loadingDate).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.LastRatesList = response.Result;
                this.SetCurrencyRateData();
                this.SetVatTypePercentages(commonDomainService, loadingDate);
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private SetVatTypePercentages(commonDomainService: CommonDomainService, loadingDate: Date) {
        commonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.VatTypePercentagesList = serviceResponse.Result;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private SetVendorIdForActiveCard(id: string) {
        this.cardPMService.get(id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var card = response.Result;
                if (!card.InActive)
                    this.VendorId = this.EntityPM.VendorId;
            }
        });
    }

    SetCurrencyRateData() {
        var rate: number = null;
        var rateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                rate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    rate = lastRate.Rate;
                    rateDate = lastRate.ValueDate;
                }
            }
        }

        this.InvoiceCurrencyExchangeRate = rate;
        this.ExchangeRateDate = rateDate;
    }
 
    // Vendor Properties
    private vendorName: string;
    get VendorName() { return this.vendorName; }
    set VendorName(value: string) {
        if (this.vendorName != value) {
            this.vendorName = value;
        }
    }

    private vendorLocalName: string;
    get VendorLocalName() { return this.vendorLocalName; }
    set VendorLocalName(value: string) {
        if (this.vendorLocalName != value) {
            this.vendorLocalName = value;
        }
    }

    get VendorDependencyProperty1() { return InvoiceTool.GetGeneralAPInvoiceVendorPartnerTypes(); }
    private vendorPartnerTypeId: string;
    get VendorPartnerTypeId() { return this.vendorPartnerTypeId; }
    set VendorPartnerTypeId(value: string) {
        if (this.vendorPartnerTypeId != value) {
            this.vendorPartnerTypeId = value;
        }
    }

    private vendorGLAccountId: string;
    get VendorGLAccountId() { return this.vendorGLAccountId; }
    set VendorGLAccountId(value: string) {
        if (this.vendorGLAccountId != value) {
            this.vendorGLAccountId = value;
        }
    }


    private vendorId: string;
    get VendorId() { return this.vendorId; }
    set VendorId(value: string) {
        if (this.vendorId != value) {
            this.vendorId = value;

            this.CheckDuplication();

            if (AppTool.IsNullOrEmpty(value)) {
                this.SetVendorData(null);
            }

            else {
                this.SetVendorDataFromConnectedCard(value);
            }
        }
    }

    private SetVendorDataFromConnectedCard(value: string) {
        this.cardListService.getSingle(value).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: CardList = response.Result;
                if (list != null) {
                    this.SetVendorData(list);
                    this.SetInvoiceCurrency(list);
                    this.SetVendorGLAccount(list);
                }
            }
        });
    }

    private SetVendorData(card: CardList) {
        this.VATNumber = card == null ? null : card.VatNumber;
        this.VendorName = card == null ? null : card.EnglishName;
        this.VendorLocalName = card == null ? null : card.LocalName || card.EnglishName;
        this.VendorPartnerTypeId = card == null ? null : card.PartnerTypeId;
        this.InvoiceCurrencyId = card == null ? SessionLocator.AccountingCurrencyId : card.InvoiceCurrencyId;
        this.PaymentTermId = card == null ? SessionLocator.TenantPM.PaymentTermId : card.PaymentTermId == null ? SessionLocator.TenantPM.PaymentTermId : card.PaymentTermId;
        this.VendorGLAccountId = null;
    }

    SetInvoiceCurrency(card: CardList) {
        if (!card.GLAccountCurrency) {
            this.InvoiceCurrencyId = card.InvoiceCurrencyId;
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, true);
        }
        else {
            this.InvoiceCurrencyId = card.GLAccountCurrency;
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
        }
    }

    private SetVendorGLAccount(list: CardList) {
        if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
            this.gLAccountPMService.get(list.GLAccountId).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var glaccount = response.Result;
                    if (glaccount != null) {
                        this.VendorGLAccountId = glaccount.Id;
                    }
                }
            });
        }
    }

    CheckDuplication() {
        var warnings: string[] = [];
        this.FillWarnings(warnings);

        if (!AppTool.IsNullOrEmpty(this.VendorId) && !AppTool.IsNullOrEmpty(this.InvoiceNumber)) {

            this.CheckIfInvoiceNumberDuplicated(warnings);
        }
    }

    private CheckIfInvoiceNumberDuplicated(warnings: string[]) {
        this.invoiceDomainService.CheckVendor_NumberDuplication(this.VendorId, this.InvoiceNumber, this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {

                var isDuplicated: boolean = response.Result;

                if (isDuplicated) {
                    warnings.push(TextCodeTranslator.Translate("APInvoice.M.SameInvoiceNumber"));
                    this.FillWarnings(warnings);
                }
            }
        });
    }
 
    // Commands
    FillWarnings(warnings: string[]) {
        this.ValidationWarningsList = [];
        if (warnings != null && warnings.length > 0) {
            warnings.forEach(item => {
                this.ValidationWarningsList.push(item);
            });
        }
    }

    private internalNotes: string;
    get InternalNotes() { return this.internalNotes; }
    set InternalNotes(value: string) {
        if (this.internalNotes != value) {
            this.internalNotes = value;

        }
    }

    private invoiceNumber: string
    get InvoiceNumber() { return this.invoiceNumber; }
    set InvoiceNumber(value: string) {
        if (this.invoiceNumber != value) {
            this.invoiceNumber = value;
            this.CheckDuplication();
        }
    }


    // Currency Properties
    private amountInInvoiceCurrency: number;
    get AmountInInvoiceCurrency() { return this.amountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.amountInInvoiceCurrency != setValue) {
            this.amountInInvoiceCurrency = setValue;
            this.InvoiceExpectedAmount = setValue;
        }
    }

    private invoiceCurrencyId: string;
    get InvoiceCurrencyId() { return this.invoiceCurrencyId; }
    set InvoiceCurrencyId(value: string) {
        if (this.invoiceCurrencyId != value) {
            this.invoiceCurrencyId = value;
            this.SetCurrencyRateData();
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.InvoiceCurrencyCode = null;
            }

            else {
                this.SetInvoiceCurrencyCode(value);
            }
        }
    }

    private SetInvoiceCurrencyCode(value: string) {
        this.currencyListService.getSingleFromCache(value).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: CurrencyList = response.Result;
                if (list != null) {
                    this.InvoiceCurrencyCode = list.Code;
                }
            }
        });
    }

    private invoiceCurrencyCode: string;
    get InvoiceCurrencyCode() { return this.invoiceCurrencyCode; }
    set InvoiceCurrencyCode(value: string) {
        if (this.invoiceCurrencyCode != value) {
            this.invoiceCurrencyCode = value;
        }
    }

    private invoiceCurrencyExchangeRate: number;
    get InvoiceCurrencyExchangeRate() { return this.invoiceCurrencyExchangeRate; }
    set InvoiceCurrencyExchangeRate(value: number) {
        var setValue: number = AppTool.Round(value, 5);
        if (this.invoiceCurrencyExchangeRate != setValue) {
            this.invoiceCurrencyExchangeRate = setValue;
        }
    }

    private exchangeRateDate: Date;
    get ExchangeRateDate() { return this.exchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.exchangeRateDate != value) {
            this.exchangeRateDate = value;
            this.ComputeRelativeRateDate();
        }
    }

    ComputeRelativeRateDate() {
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    }

    private relativeRateDate: string = null;
    get RelativeRateDate() { return this.relativeRateDate; }
    set RelativeRateDate(value: string) {
        if (this.relativeRateDate != value) {
            this.relativeRateDate = value;
        }
    }

    private vatNumber: string;
    get VATNumber() { return this.vatNumber; }
    set VATNumber(value: string) {
        if (this.vatNumber != value) {
            this.vatNumber = value;
            this.SetUIProperties();
        }
    }

    private paymentTermId: string;
    get PaymentTermId() { return this.paymentTermId; }
    set PaymentTermId(value: string) {
        if (this.paymentTermId != value) {
            this.paymentTermId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.paymentTermName = null;
            }

            else {
                this.SetPaymentTermName(value);
            }
            this.ComputeAPInvoiceDueDate();
        }
    }

    private SetPaymentTermName(value: string) {
        this.paymentTermListService.getSingleFromCache(value).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: PaymentTermList = response.Result;
                if (list != null) {
                    this.paymentTermName = list.EnglishName;
                }
            }
        });
    }

    private paymentTermName: string;
    get PaymentTermName() { return this.paymentTermName; }
    set PaymentTermName(value: string) {
        if (this.paymentTermName != value) {
            this.paymentTermName = value;
        }
    }

    private invoiceDate: Date;
    get InvoiceDate() { return this.invoiceDate; }
    set InvoiceDate(value: Date) {
        if (this.invoiceDate != value) {
            this.invoiceDate = value;
            this.EntityPM.InvoiceDate = value;
            this.ComputeAPInvoiceDueDate();
        }
    }

    private dueDate: Date;
    get DueDate() { return this.dueDate; }
    set DueDate(value: Date) {
        if (this.dueDate != value) {
            this.dueDate = value;
           // InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
        }
    }

    private accountingDate: Date;
    get AccountingDate() { return this.accountingDate; }
    set AccountingDate(value: Date) {
        if (this.accountingDate != value) {
            this.accountingDate = value;
            if (value == null) {
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, false);
            }
        }
    }

    private branchId: string;
    get BranchId() { return this.branchId; }
    set BranchId(value: string) {
        if (this.branchId != value) {
            this.branchId = value;
        }
    }

    disableCopyAmounts: boolean = false;
    private isCopyLinesChecked: boolean = true;
    get IsCopyLinesChecked() { return this.isCopyLinesChecked; }
    set IsCopyLinesChecked(value: boolean) {
        if (this.isCopyLinesChecked != value) {
            this.isCopyLinesChecked = value;
            this.disableCopyAmounts = !value;
        }

        if (!this.isCopyLinesChecked) {
            this.IsCopyAmountsChecked = false;
        }
    }

    private isCopyAmountsChecked: boolean = true;
    get IsCopyAmountsChecked() { return this.isCopyAmountsChecked; }
    set IsCopyAmountsChecked(value: boolean) {
        if (this.isCopyAmountsChecked != value) {
            this.isCopyAmountsChecked = value;
        }
    }

    private profitCurrencyId: string;
    get ProfitCurrencyId() { return this.profitCurrencyId; }
    set ProfitCurrencyId(value: string) {
        if (this.profitCurrencyId != value) {
            this.profitCurrencyId = value;
            this.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(value);
        }
    }

    private profitCurrencyExchangeRate: number;
    get ProfitCurrencyExchangeRate() { return this.profitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(value: number) {
        if (this.profitCurrencyExchangeRate != value) {
            this.profitCurrencyExchangeRate = AppTool.Round(value, 5);
        }
    }

    private amountInLocalCurrency: number;
    get AmountInLocalCurrency() { return this.amountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.amountInLocalCurrency != setValue) {
            this.amountInLocalCurrency = setValue;
            this.AmountDueInLocalCurrency = setValue;
        }
    }

    private amountDueInLocalCurrency: number;
    get AmountDueInLocalCurrency() { return this.amountDueInLocalCurrency; }
    set AmountDueInLocalCurrency(value: number) {
        if (this.amountDueInLocalCurrency != value) {
            this.amountDueInLocalCurrency = AppTool.Round(value, 5);
        }
    }

    private amountDueInProfitCurrency: number;
    get AmountDueInProfitCurrency() { return this.amountDueInProfitCurrency; }
    set AmountDueInProfitCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.amountDueInProfitCurrency != setValue) {
            this.amountDueInProfitCurrency = setValue;
        }
    }


    private amountInProfitCurrency: number;
    get AmountInProfitCurrency() { return this.amountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.amountInProfitCurrency != setValue) {
            this.amountInProfitCurrency = setValue;
            this.AmountDueInProfitCurrency = setValue;
        }
    }


    private invoiceExpectedAmount: number;
    get InvoiceExpectedAmount() { return this.invoiceExpectedAmount; }
    set InvoiceExpectedAmount(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.invoiceExpectedAmount != setValue) {
            this.invoiceExpectedAmount = setValue;
        }
    }

    private subTotalInLocalCurrency: number;
    get SubTotalInLocalCurrency() {
        return this.subTotalInLocalCurrency;
    }
    set SubTotalInLocalCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);

        if (this.subTotalInLocalCurrency != setValue) {
            this.subTotalInLocalCurrency = setValue;
        }
    }

    private subTotalInInvoiceCurrency: number;
    get SubTotalInInvoiceCurrency() { return this.subTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);

        if (this.subTotalInInvoiceCurrency != setValue) {
            this.subTotalInInvoiceCurrency = setValue;
        }
    }

    public ComputeAPInvoiceDueDate() {
        if (AppTool.IsNullOrEmpty(this.PaymentTermId)) {
            this.ComputeDueDateWithoutPaymentTerm();
        }

        else {
            this.ComputeDueDateWithPaymentTerm();
        }
    }

    private ComputeDueDateWithoutPaymentTerm() {
        this.DueDate = DateTool.GetDateParts(this.InvoiceDate).DateObject;
    }

    private ComputeDueDateWithPaymentTerm() {
        var service = new PaymentTermListService();
        service.getSingleFromCache(this.PaymentTermId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: PaymentTermList = response.Result;
                if (list != null) {
                    if (list.IsManuallySet) {
                        this.DueDate = null;
                    }

                    else {
                        this.SetDueDateAccordingToPaymentTerm(list);
                    }
                }
            }
        });
    }

    private SetDueDateAccordingToPaymentTerm(list: PaymentTermList) {
        var invoiceDateParts: Date = DateTool.GetDateParts(this.InvoiceDate).DateObject;

        if (invoiceDateParts != null) {
       

            invoiceDateParts = this.SetDateProperties(invoiceDateParts, list);

            if (!AppTool.IsNullOrZero(list.Days)) {
                invoiceDateParts.setUTCDate(invoiceDateParts.getUTCDate() + list.Days);
            }

            if (this.DueDate != invoiceDateParts) {
                this.DueDate = invoiceDateParts;
            }
        }
    }

    private SetDateProperties(invoiceDateParts: Date, list: PaymentTermList) {
        var dateYear = invoiceDateParts.getUTCFullYear();
        var dateMonth = list.EndOfMonth ? invoiceDateParts.getUTCMonth() + 2 : invoiceDateParts.getUTCMonth() + 1;
        var dateDay = list.EndOfMonth ? 1 : invoiceDateParts.getUTCDate();
     
        var date = new Date();
        date.setUTCFullYear(dateYear);
        date.setUTCMonth(dateMonth - 1);
        date.setUTCDate(dateDay);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        date.setUTCMilliseconds(0);

        return date;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = this.ValidateInvoiceFields();
        if (this.ValidationErrorsList.length == 0) {
            this.CreateNewAPInvoicePM();
        }
    }

    private ValidateInvoiceFields() {
        this.errors = [];

        this.CheckSpecialCharacters() != null ? this.errors.push(this.CheckSpecialCharacters()) : null;
        this.ValidateRequiedFields();
        if (this.InvoiceDate > this.AccountingDate) {
            this.errors.push(TextCodeTranslator.Translate("APInvoice.O.CheckInvoiceDate"));
        }

        else if (DateTool.GetDateParts(this.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
            this.errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }

        if (DateTool.GetDateParts(this.AccountingDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
            this.errors.push("Cant issue Invoice with Future Accounting Date");
        }
  
        return this.errors;
    }

    CheckSpecialCharacters() {
        if (FeatureLocator.HasFeaturePermession("APInvoice", "INSC")) {
            var invoiceNumber_Check = /^[A-Za-z0-9]+$/i;
            if (!invoiceNumber_Check.test(this.InvoiceNumber)) {
                return TextCodeTranslator.Translate("APInvoice.O.ValidateInvoiceNumber");
            }
        }
    }

    ValidateRequiedFields() {

        if (AppTool.IsNullOrEmpty(this.VendorId)) {
            this.AddReuiredErrorMessage("APInvoice.F.VendorId");
        }

        if (AppTool.IsNullOrEmpty(this.InvoiceNumber)) {
            this.AddReuiredErrorMessage("APInvoice.F.InvoiceNumber");
        }

        if (AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            this.AddReuiredErrorMessage("APInvoice.F.InvoiceCurrencyId");
        }

        if (this.InvoiceDate == null) {
            this.AddReuiredErrorMessage("APInvoice.F.InvoiceDate");
        }

        if (AppTool.IsNullOrEmpty(this.PaymentTermId)) {
            this.AddReuiredErrorMessage("Payment Term");
        }

        if (this.DueDate == null) {
            this.AddReuiredErrorMessage("APInvoice.F.DueDate");
        }

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.VATNumber)) {
                this.AddReuiredErrorMessage("APInvoice.F.VATNumber");
            }
        }

        if (this.IsAccountingActivated && this.AccountingDate == null) {
            this.AddReuiredErrorMessage("APInvoice.F.AccountingDate");
        }

        if (AppTool.IsNullOrEmpty(this.BranchId)) {
            this.AddReuiredErrorMessage("APInvoice.F.BranchId");
        }
    }

    AddReuiredErrorMessage(TextCode) {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate(TextCode)));
    }

    private CreateNewAPInvoicePM() {
        var entityPM: APInvoicePM = new APInvoicePM();
        this.MapNewAPInvoice(entityPM);
        if (this.IsCopyLinesChecked) {
            this.CopyInvoiceLines(entityPM);
        }

        entityPM.SubTotalInLocalCurrency = AppTool.Round(ArrayTool.Sum(entityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        entityPM.SubTotalInInvoiceCurrency = AppTool.Round(ArrayTool.Sum(entityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.InitializeProfitCurrency(entityPM);
        this.OpenInvoiceEditScreen(entityPM);
    }

    MapNewAPInvoice(entityPM) {
        entityPM.Tenant = this.EntityPM.Tenant;
        entityPM.VendorId = this.VendorId;
        entityPM.VendorName = this.VendorName;
        entityPM.VendorGLAccountId = this.VendorGLAccountId;
        entityPM.VendorLocalName = this.VendorLocalName;
        entityPM.InvoiceNumber = this.invoiceNumber;
        entityPM.InvoiceCurrencyId = this.InvoiceCurrencyId;
        entityPM.InvoiceDate = this.InvoiceDate;
        entityPM.PaymentTermId = this.PaymentTermId;
        entityPM.DueDate = this.DueDate;
        entityPM.VATNumber = this.vatNumber;
        entityPM.AccountingDate = this.AccountingDate;
        entityPM.BranchId = this.BranchId;
        entityPM.AmountInInvoiceCurrency = this.AmountInInvoiceCurrency;
        entityPM.InvoiceCurrencyExchangeRate = this.InvoiceCurrencyExchangeRate;
        entityPM.InvoiceCurrencyCode = this.InvoiceCurrencyCode;
        entityPM.InternalNotes = this.InternalNotes;
        entityPM.InvoiceExpectedAmount = this.InvoiceExpectedAmount;
        entityPM.ProfitCurrencyExchangeRate = this.ProfitCurrencyExchangeRate;
        entityPM.AmountInProfitCurrency = this.EntityPM.AmountInProfitCurrency;
        entityPM.localCurrencyId = this.LocalCurrencyId;
        entityPM.IsNew = true;
        entityPM.IsCopied = true;
        entityPM.CopiedFrom = this.EntityPM.InvoiceNumber;
        entityPM.IsGeneralInvoice = true;
        entityPM.StatusCode = this.WaitingForApprovalStatusCode;
        entityPM.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        entityPM.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        if (SessionLocator.LocalCurrencyId == this.InvoiceCurrencyId) {
            entityPM.AmountInLocalCurrency = this.AmountInInvoiceCurrency;
        }
        else {
            entityPM.AmountInLocalCurrency = AppTool.Round(this.AmountInInvoiceCurrency * this.InvoiceCurrencyExchangeRate, 2);
        }
    
    }

    CopyInvoiceLines(entityPM) {
        for (var i = 0; i < this.EntityPM.InvoiceLines.length; i++) {
            entityPM.InvoiceLines.push(this.MapNewInvoiceLine(entityPM, this.EntityPM.InvoiceLines[i]));
        }

    }

    MapNewInvoiceLine(apInvoicePM: APInvoicePM, originalAPInvoiceLine: APInvoiceLinePM) {

        var apInvoiceLinePM = new APInvoiceLinePM(apInvoicePM);
        apInvoiceLinePM = originalAPInvoiceLine;
        this.SetChargesTypeIdIfActive(originalAPInvoiceLine, apInvoiceLinePM);
        apInvoiceLinePM.InvoiceCurrencyAmount = this.IsCopyAmountsChecked ? originalAPInvoiceLine.InvoiceCurrencyAmount : 0;
        apInvoiceLinePM.ForiegnCurrencyAmount = this.IsCopyAmountsChecked ? originalAPInvoiceLine.ForiegnCurrencyAmount : 0;
        apInvoiceLinePM.LocalCurrencyAmount = this.IsCopyAmountsChecked ? originalAPInvoiceLine.LocalCurrencyAmount : 0;
        apInvoiceLinePM.ForiegnCurrencyId = apInvoicePM.InvoiceCurrencyId;
        apInvoiceLinePM.ForiegnCurrencyCode = apInvoicePM.InvoiceCurrencyCode;
        apInvoiceLinePM.OpenAmount = this.IsCopyAmountsChecked ? originalAPInvoiceLine.OpenAmount : 0;
        apInvoiceLinePM.Description = originalAPInvoiceLine.Description;
        apInvoiceLinePM.Notes = null;
        apInvoiceLinePM.ForiegnExchangeRate = apInvoicePM.InvoiceCurrencyExchangeRate;
        apInvoiceLinePM.VendorId = this.VendorId;
        apInvoiceLinePM.VendorName = this.VendorName;
        apInvoiceLinePM.AmountTypeCode = "NEXP";
        return apInvoiceLinePM;
    }


    private SetChargesTypeIdIfActive(originalAPInvoiceLine: APInvoiceLinePM, copiedAPInvoiceLinePM: APInvoiceLinePM) {
        this.chargesTypePMService.get(originalAPInvoiceLine.ChargesTypeId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var chargetype = response.Result;
                if (!chargetype.InActive) {
                    copiedAPInvoiceLinePM.ChargesTypeId = this.IsCopyLinesChecked ? originalAPInvoiceLine.ChargesTypeId : null;
                    this.SetVatTypeIdIfActive(chargetype.VatTypeId, copiedAPInvoiceLinePM);
                    copiedAPInvoiceLinePM.ChargeTypeGLAccountId = chargetype.PayableDebitGLAcountId;
                    copiedAPInvoiceLinePM.ChargesTypeCode = chargetype.Code;
                }

                else {
                    this.ResetChargeTypeValues(copiedAPInvoiceLinePM);
                }
            }
        });
    }

    private ResetChargeTypeValues(copiedAPInvoiceLinePM: APInvoiceLinePM) {
        copiedAPInvoiceLinePM.ChargesTypeId = null;
        copiedAPInvoiceLinePM.LocalDescription = null;
        copiedAPInvoiceLinePM.Description = null;
        copiedAPInvoiceLinePM.ChargesTypeCode = null;
    }

    private SetVatTypeIdIfActive(vatTypeId: string, copiedAPInvoiceLinePM: APInvoiceLinePM) {
        this.vatTypePMService.get(vatTypeId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var vattype = response.Result;
                if (!vattype.InActive) {
                    copiedAPInvoiceLinePM.VatTypeId = this.IsCopyLinesChecked ? vatTypeId : null;
                    copiedAPInvoiceLinePM.VatTypeName = vattype.Code;
                    copiedAPInvoiceLinePM.VatPercentage = this.GetVatTypePercentage(vatTypeId);
                }
                else 
                    this.ResetVatTypeValues(copiedAPInvoiceLinePM);
            }
        });
    }

    private ResetVatTypeValues(copiedAPInvoiceLinePM: APInvoiceLinePM) {
        copiedAPInvoiceLinePM.VatTypeId = null;
        copiedAPInvoiceLinePM.VatTypeName = null;
        copiedAPInvoiceLinePM.VatPercentage = null;
    }

    private InitializeProfitCurrency(entityPM) {
        this.SetProfitCurrencyId(entityPM);
        this.SetProfitCurrencyCode(entityPM);

        entityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(entityPM.ProfitCurrencyId);
    }

    private SetProfitCurrencyId(entityPM: any) {
        if (this.EntityPM.IsMultipleEntities) {
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        else if (AppTool.IsNullOrEmpty(this.EntityPM.ProfitCurrencyId)) {
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }
    }

    private SetProfitCurrencyCode(entityPM: any) {
        this.currencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var list: CurrencyList = response.Result;
                if (list != null) {
                    entityPM.ProfitCurrencyCode = list.Code;
                    entityPM.ProfitCurrencyId = list.Id;
                }
            }
        });
    }

    GetCurrencyRate(currencyId: string) {
        var result: number = null;

        if (AppTool.IsNullOrEmpty(currencyId)) { return; }
        if (currencyId == SessionLocator.TenantPM.CurrencyId) {
            result = 1;
        }

        else {
            var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
            if (lastRate != null) {
                result = lastRate.Rate;
            }
        }
        
        return result;
    }

    OpenInvoiceEditScreen(entityPM) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'APInvoice',
                });
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.CancelButtonClicked();
                });
            });
    }

    GetVatTypePercentage(vatTypeId: string) {
        var vatTypePercentage: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            vatTypePercentage = vatTypePercentagePM.Percentage;
        }

        return vatTypePercentage;
    }
}
