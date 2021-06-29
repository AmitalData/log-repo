import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { APInvoicePM } from '../../../Invoice/EntityPMs/APInvoicePM';
import { InvoiceTool } from '../../../Invoice/Tools';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
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


    // services
    private paymentTermListService: PaymentTermListService = new PaymentTermListService();
    private invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
    private cardPMService: CardPMService = new CardPMService();
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
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        this.InitializeVendorLov();

    }

    private InitializeVendorLov() {
        if (this.IsAccountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.VendorLovSizeForFullAccounting = 550;
        }
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
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, false);
        this.UIProperties.SetRequired("VATNumber",this.ObjectTableName,true);
    }

    // Load Data
    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var myCurrencyRatesService = new CurrencyRatesService();
        var myCommonDomainService = new CommonDomainService();

        var loadingDate = this.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.GetCurrencyExchangeRate(myCurrencyRatesService, loadingDate, myCommonDomainService);
    }

    private GetCurrencyExchangeRate(myCurrencyRatesService: CurrencyRatesService, loadingDate: Date, myCommonDomainService: CommonDomainService) {
        myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
                this.SetCurrencyRateData();

                myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private SetVendorIdForActiveCard(id: string) {
        this.cardPMService.get(id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var card = myResponse.Result;
                if (!card.InActive)
                    this.VendorId = this.EntityPM.VendorId;
            }
        });
    }

    SetCurrencyRateData() {
        var rate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                rate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    rate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.InvoiceCurrencyExchangeRate = rate;
        this.ExchangeRateDate = myRateDate;
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
        this.cardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CardList = myResponse.Result;
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
            this.gLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var glaccount = myResponse.Result;
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
        this.invoiceDomainService.CheckVendor_NumberDuplication(this.VendorId, this.InvoiceNumber, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                var isDuplicated: boolean = myResponse.Result;

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
            this.amountInInvoiceCurrency = setValue;
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
        this.currencyListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
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
        this.paymentTermListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: PaymentTermList = myResponse.Result;
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

    public ComputeAPInvoiceDueDate() {
        if (AppTool.IsNullOrEmpty(this.PaymentTermId)) {
            this.DueDate = DateTool.GetDateParts(this.InvoiceDate).DateObject;
        }

        else {
            var myService = new PaymentTermListService();
            myService.getSingleFromCache(this.PaymentTermId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PaymentTermList = myResponse.Result;
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
        var dateMonth = list.CurrentMonth ? invoiceDateParts.getUTCMonth() + 2 : invoiceDateParts.getUTCMonth() + 1;
        var dateDay = list.CurrentMonth ? 1 : invoiceDateParts.getUTCDate();
     
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
    }

    private ValidateInvoiceFields() {
        this.errors = [];

        this.CheckSpecialCharacters() != null ? this.errors.push(this.CheckSpecialCharacters()) : null;
        this.ValidateRequiedFields();
        if (this.InvoiceDate > this.AccountingDate) {
            this.errors.push(TextCodeTranslator.Translate("APInvoice.O.CheckInvoiceDate"));
        }
 
        else if (DateTool.GetDateParts(this.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
            this.errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }

        if (DateTool.GetDateParts(this.AccountingDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
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
}
