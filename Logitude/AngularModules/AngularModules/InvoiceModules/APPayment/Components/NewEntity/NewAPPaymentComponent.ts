import { ServiceResponse } from './../../../../Infrastructure/DataContracts/ServiceResponse';
import { TenantPM } from './../../../../Common/EntityPMs/TenantPM';
import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { AccountingPaymentMethodList } from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import { AccountingPaymentMethodListService } from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { APPaymentInvoicePM } from '../../../../Invoice/EntityPMs/APPaymentInvoicePM';

@Component({
    templateUrl: './NewAPPaymentComponent.html',
})

export class NewAPPaymentComponent extends BaseComponent implements OnInit {
    DefaultSelectedPaymentMethodCode = "BT";
    public DataContext: NewAPPaymentComponent = this;
    public ObjectTableName: string = "APPayment";
    public newAPPaymentPM: APPaymentPM = new APPaymentPM();
    public TenantPM: TenantPM;
    public invoicePm: APInvoicePM = new APInvoicePM();
    public LastRatesList: LastRate[] = [];
    public AllMethods: AccountingPaymentMethodList[] = [];
    public EnableNegativeOffsetAPPayments: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public IsCreatedFromInvoiceSide: boolean = false;
    get IsNegativeAmountEnabled() { return this.EnableNegativeOffsetAPPayments == true && this.PaymentMethodCode == "FS" ? true : false; }
    public isRTL: boolean = false;
    public accountingActivated: boolean;
    _AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
    private CurrentSession = SessionLocator.SelectedSession;
    DisplayFieldsFromList: string;
    DisplayLocalFieldsFromList: string;
    VendorLovSizeForFullAccounting: number;
    vendorAddressId: string;
    public GLAccountsFilterItems: ApiQueryFilters;

    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.accountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this._entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("APInvoice", 0).subscribe((response: any) => { });
        this.InitLOVFilters();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.TodayDate = DateTool.GetCurrentDateAsUtc();
        this.TenantPM = SessionLocator.TenantPM;

        this.EnableNegativeOffsetAPPayments = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetAPPayments;

        this.InitializeInvoice();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
    InitializeInvoice() {
        if (this.invoicePm == null) {
            this.invoicePm = new APInvoicePM();
        }
    }

    ngOnInit() {
        this.Initialize();
    }


    public IsVisible: boolean = false;
    Initialize() {
        this.InitializeInvoice();
        this.CreateAPPayment();

        if (this.IsCreatedFromInvoiceSide) {
            this.newAPPaymentPM.VendorId = this.invoicePm.VendorId;
            this.newAPPaymentPM.VendorAddressId = this.vendorAddressId;
            this.newAPPaymentPM.VendorName = this.invoicePm.VendorName;
            this.newAPPaymentPM.VendorPartnerTypeId = this.invoicePm.VendorPartnerTypeId;
            this.newAPPaymentPM.PaymentCurrencyId = this.invoicePm.InvoiceCurrencyId;
            this.newAPPaymentPM.PaymentCurrencyCode = this.invoicePm.InvoiceCurrencyCode;
            this.newAPPaymentPM.PaymentCurrencyExchangeRateDate = this.invoicePm.ExchangeRateDate;
            this.newAPPaymentPM.PaymentCurrencyExchangeRate = this.invoicePm.InvoiceCurrencyExchangeRate;
        }

        else {
            this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.newAPPaymentPM.PaymentCurrencyExchangeRate = 1;
        }

        if (SessionLocator.TenantPM.AccountingActivated)
            this.SetDefalutPaymentMethod();

        this.LoadData();
        this.SetUIProperties();
        this.IsVisible = true;
    }

    windowArgs;
    preselectedPaymentMethodCode;
    SetWindowArgs(args: any) {
        if (args) {
            this.windowArgs = args;
            this.invoicePm = args['APInvoice'];
            this.vendorAddressId = args['VendorAddressId'];

            if (args.AccountingPaymentMethodCode) {
                this.preselectedPaymentMethodCode = args.AccountingPaymentMethodCode;

                this.RegisterDate = new Date();
            }
        }

        if (this.invoicePm != null) {
            this.IsCreatedFromInvoiceSide = true;
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            if (!this.accountingActivated) {
                this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, false);
            }
        }
    }

    public RateIsEnabled = false;
    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentEditExchangeRate")) {
            if (this.PaymentCurrencyId) {
                if (this.PaymentCurrencyId != this.TenantPM.CurrencyId) {
                    if (this.IsCreatedFromInvoiceSide == false) {
                        this.RateIsEnabled = true;
                    }
                }
            }
        }
        else if (SessionLocator.TenantPM.AccountingActivated) {
            if (this.PaymentCurrencyId) {
                if (this.PaymentCurrencyId != this.TenantPM.CurrencyId) {

                    this.RateIsEnabled = true;
                }
                else {
                    this.RateIsEnabled = false;

                }
            }
        }

        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, this.RateIsEnabled);
        this.SetUIProperties_Payment();
    }

    LoadData() {
        if (this.invoicePm != null && AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
            var loadingDate = this.newAPPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            var myService: CurrencyRatesService = new CurrencyRatesService();
            myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((resp: ServiceResponse) => {
                if (resp != null) {
                    if (!resp.HasError) {
                        this.LastRatesList = resp.Result;
                        this.SetCurrencyRateData();
                    }
                }
            });
        }

        var myService1: AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
        myService1.getAll().subscribe((response: ServiceResponse) => {
            if (response != null) {
                this.AllMethods = response.Result;
                if (this.preselectedPaymentMethodCode) {
                    var preselectedMethod = this.AllMethods.find(m => m.Code == this.preselectedPaymentMethodCode);
                    this.AccountingPaymentMethodId = preselectedMethod.Id;
                    this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);

                }
            }
        });
    }

    SetDefalutPaymentMethod() {
        var filter = new ApiQueryFilters(true);
        filter.addAdditionalFilter("Code", this.DefaultSelectedPaymentMethodCode, null, null, "Equal", false, false, false, "string");
        this._AccountingPaymentMethodListService.getByFilters(filter).subscribe(e => {
            if (e && !e.HasError && e.Result.length > 0) {
                this.AccountingPaymentMethodId = e.Result[0].Id;
                this.PaymentMethodCode = this.DefaultSelectedPaymentMethodCode;
            }
        });
    }

    SetCurrencyRateData() {
        if (this.invoicePm != null && AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
            var rate = null;
            var rateDate = null;

            if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
                if (this.PaymentCurrencyId == this.TenantPM.CurrencyId) {
                    rate = 1;
                }

                else {
                    if (this.LastRatesList != null) {
                        var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.PaymentCurrencyId)[0];
                        if (lastRate != null) {
                            rate = lastRate.Rate;
                            rateDate = lastRate.ValueDate;
                        }
                    }
                }
            }

            this.PaymentCurrencyExchangeRate = rate;
            this.PaymentCurrencyExchangeRateDate = rateDate;
        }
    }

    CreateAPPayment() {
        this.newAPPaymentPM = new APPaymentPM();
        this.newAPPaymentPM.Tenant = this.TenantPM.Id;
        this.newAPPaymentPM.StatusCode = "DR";
        this.newAPPaymentPM.StatusName = "Draft";
        this.newAPPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.newAPPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.newAPPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.newAPPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.newAPPaymentPM.LocalCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.newAPPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();

        if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.newAPPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        }

        this.EntityPM = this.newAPPaymentPM;
    }

    get RegisterDate() { return this.newAPPaymentPM.RegisterDate; }
    set RegisterDate(newValue: Date) {
        if (this.newAPPaymentPM.RegisterDate != newValue) {
            this.newAPPaymentPM.RegisterDate = newValue;
            this.LoadData();
        }
    }
    get PrintNotes() { return this.newAPPaymentPM.PrintNotes; }
    set PrintNotes(newValue: string) {
        if (this.newAPPaymentPM.PrintNotes != newValue) {
            this.newAPPaymentPM.PrintNotes = newValue;
        }
    }
    public TodayDate: Date = new Date();

    get SelectableDateStart() { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() - 100); }
    get SelectableDateEnd() { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() + 100); }
    get SelectablePaymentDateEnd() { return this.TodayDate; }

    get AccountingPaymentMethodId() { return this.newAPPaymentPM.AccountingPaymentMethodId; }
    set AccountingPaymentMethodId(newValue: string) {
        if (this.newAPPaymentPM.AccountingPaymentMethodId != newValue) {
            this.newAPPaymentPM.AccountingPaymentMethodId = newValue;
            this.RefreshPaymentMethodFields();
        }
    }

    get PaymentMethodCode() { return this.EntityPM.PaymentMethodCode; }
    set PaymentMethodCode(newValue: string) {
        if (this.EntityPM.PaymentMethodCode != newValue) {
            this.EntityPM.PaymentMethodCode = newValue;
        }
    }


    // Vendor Properties
    get VendorId() { return this.newAPPaymentPM.VendorId; }
    set VendorId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newAPPaymentPM.VendorId != newValue) {
                this.newAPPaymentPM.VendorId = newValue;
                this.GetCardProperties();
            }
        }
    }

    get VendorName() { return this.newAPPaymentPM.VendorName; }
    set VendorName(newValue: string) {
        if (this.newAPPaymentPM.VendorName != newValue) {
            this.newAPPaymentPM.VendorName = newValue;
        }
    }

    VendorCard: CardList;
    GetCardProperties() {
        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.VendorId)) {
            this.FillDataFromCardList(new CardList());
        }

        else {
            var myService: CardListService = new CardListService();
            myService.getSingle(this.newAPPaymentPM.VendorId).subscribe((resp: ServiceResponse) => {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            this.VendorCard = cardList;
                            this.FillDataFromCardList(cardList);
                        }
                    }
                }
            });
        }
    }

    FillDataFromCardList(list: CardList) {
        if (list == null) {
            this.VendorAddressId = null;
            this.VendorName = null;
            this.AccountingPaymentMethodId = null;
            this.PaymentMethodCode = null;
            this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.newAPPaymentPM.VendorPartnerTypeId = null;
        }

        else {
            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }

            this.newAPPaymentPM.VendorPartnerTypeId = list.PartnerTypeId;
            this.VendorName = list.EnglishName;
            this.LoadAddress();
        }
    }

    LoadAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetAddressByCardAndType(this.newAPPaymentPM.VendorId, "B").subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                var item = billingAddress;
                this.VendorAddressId = item.Id;
            }

            else {
                this.GetBillingAddress();
            }
        });
    }

    GetBillingAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.newAPPaymentPM.VendorId).subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                this.VendorAddressId = billingAddress.Id;
            }

            else {
                var myService: PartnersDomainService = new PartnersDomainService();
                myService.GetAddressByCardAndType(this.newAPPaymentPM.VendorId, "M").subscribe((resp: any) => {
                    if (resp != null) {
                        var mainAddress = resp;
                        if (mainAddress != null) {
                            var item = mainAddress;
                            this.VendorAddressId = item.Id;
                        }
                        else {
                            this.GetMainAddressListByCardId();
                        }
                    }
                });
            }
        });
    }

    GetMainAddressListByCardId() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetMainAddressListByCardId(this.newAPPaymentPM.VendorId).subscribe((resp: any) => {
            if (resp != null) {
                var mainAddress = resp;
                if (mainAddress != null) {
                    this.VendorAddressId = mainAddress.Id;
                }

                else {
                    this.VendorAddressId = null;
                }
            }
        });
    }

    get VendorAddressId() { return this.newAPPaymentPM.VendorAddressId; }
    set VendorAddressId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newAPPaymentPM.VendorAddressId != newValue) {
                this.newAPPaymentPM.VendorAddressId = newValue;
            }
        }
    }

    // Currency Properties
    get PaymentCurrencyIsEnabled() {
        if (this.invoicePm != null) {
            return AppTool.IsNullOrEmpty(this.invoicePm.Id);
        }
        else
            return false;
    }

    get PaymentCurrencyId() { return this.newAPPaymentPM.PaymentCurrencyId; }
    set PaymentCurrencyId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newAPPaymentPM.PaymentCurrencyId != newValue) {
                this.newAPPaymentPM.PaymentCurrencyId = newValue;
                this.SetUIProperties();
                this.SetCurrencyCode();
                this.ComputeTotals();
            }
            this.SetCurrencyRateData();
        }
    }

    get PaymentCurrencyExchangeRate() { return this.newAPPaymentPM.PaymentCurrencyExchangeRate; }
    set PaymentCurrencyExchangeRate(newValue: number) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newAPPaymentPM.PaymentCurrencyExchangeRate != newValue) {
                this.newAPPaymentPM.PaymentCurrencyExchangeRate = AppTool.Round(newValue, 5);
                this.ComputeTotals();
            }
        }
    }

    get PaymentCurrencyExchangeRateDate() { return this.newAPPaymentPM.PaymentCurrencyExchangeRateDate; }
    set PaymentCurrencyExchangeRateDate(newValue: Date) {
        if (this.newAPPaymentPM.PaymentCurrencyExchangeRateDate != newValue) {
            this.newAPPaymentPM.PaymentCurrencyExchangeRateDate = newValue;
        }
    }

    get RelativeRateDate() { return DateTool.GetRelativeRateDate(this.newAPPaymentPM.RegisterDate, this.PaymentCurrencyExchangeRateDate, "old"); }

    SetCurrencyCode() {
        if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            var myService: CurrencyListService = new CurrencyListService();
            myService.getSingleFromCache(this.PaymentCurrencyId).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if (response != null) {
                        var currency = response.Result;
                        if (currency != null) {
                            this.newAPPaymentPM.PaymentCurrencyCode = currency.Code;
                        }
                    }
                }
            });
        }
        else {
            var myService: CurrencyListService = new CurrencyListService();
            myService.getSingle(this.PaymentCurrencyId).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if (response != null) {
                        var list = response.Result;
                        if (list != null && list.length > 0) {
                            this.newAPPaymentPM.PaymentCurrencyCode = list.Code;
                        }
                    }
                }
            });
        }
    }

    // Payment Line Properties
    RefreshPaymentMethodFields() {
        this.newAPPaymentPM.Bank = null;
        this.newAPPaymentPM.BankBranch = null;
        this.newAPPaymentPM.Account = null;

        var lists: AccountingPaymentMethodList[] = this.AllMethods.filter(d => d.Id == this.AccountingPaymentMethodId);
        if (lists) {
            var list = lists[0];
            if (list) {
                this.EntityPM.PaymentMethodCode = list.Code;
            }
        }

        if (this.EntityPM.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
            this.newAPPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    get AmountInPaymentCurrency() { return this.newAPPaymentPM.AmountInPaymentCurrency; }
    set AmountInPaymentCurrency(newValue: number) {
        if (this.newAPPaymentPM.AmountInPaymentCurrency != newValue) {
            this.newAPPaymentPM.AmountInPaymentCurrency = AppTool.Round(newValue, 2);
            this.ComputeTotals();
        }
    }

    get AmountInLocalCurrency() { return this.newAPPaymentPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(newValue: number) {
        if (this.newAPPaymentPM.AmountInLocalCurrency != newValue) {
            this.newAPPaymentPM.AmountInLocalCurrency = AppTool.Round(newValue, 2);
        }
    }

    get OpenAmount() { return this.newAPPaymentPM.OpenAmount; }
    set OpenAmount(newValue: number) {
        if (this.newAPPaymentPM.OpenAmount != newValue) {
            this.newAPPaymentPM.OpenAmount = AppTool.Round(newValue, 2);

        }
    }

    get OpenAmountInLocalCurrency() { return this.EntityPM.OpenAmountInLocalCurrency == null ? 0 : this.EntityPM.OpenAmountInLocalCurrency; }
    set OpenAmountInLocalCurrency(value: number) {
        if (this.EntityPM.OpenAmountInLocalCurrency != value) {
            this.EntityPM.OpenAmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get BranchId() { return this.newAPPaymentPM.BranchId; }
    set BranchId(value: string) {
        if (this.newAPPaymentPM.BranchId != value) {
            this.newAPPaymentPM.BranchId = value;
        }
    }

    ComputeTotals() {
        this.OpenAmount = this.AmountInPaymentCurrency;
        this.OpenAmountInLocalCurrency = this.PaymentCurrencyExchangeRate == null ? 0 : this.OpenAmount * this.PaymentCurrencyExchangeRate;
        this.AmountInLocalCurrency = this.PaymentCurrencyExchangeRate == null ? 0 : this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    }

    //Commands
    UpdateCurrencyRateClicked() {
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe((response: any) => {

            var loadingDate = this.newAPPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: this.newAPPaymentPM.PaymentCurrencyId, CurrencyCode: this.newAPPaymentPM.PaymentCurrencyCode, Rate: this.newAPPaymentPM.PaymentCurrencyExchangeRate, Date: loadingDate };
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.PaymentCurrencyExchangeRate = comp.Rate;
                        this.PaymentCurrencyExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    }

    SetUIProperties_Payment() {
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.VendorId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.VendorId")));
        }

        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.VendorAddressId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.VendorAddressId")));
        }

        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.PaymentCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.PaymentCurrencyCode")));
        }

        if (this.RegisterDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.RegisterDate")));
        }

        else {
            if (DateTool.GetDateParts(this.RegisterDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
                errors.push(TextCodeTranslator.Translate("APPayment.M.CantSetFutureDatePayment"));
            }
            if (DateTool.GetDateFromDate(this.RegisterDate) > DateTool.GetDateFromDate(this.newAPPaymentPM.ValueDate) && this.PaymentMethodCode == "BT") {
                errors.push(TextCodeTranslator.Translate("APPayment.M.ValueDateBiggerOrEqualRegisterDate"));
            }
        }

        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.AccountingPaymentMethodId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.AccountingPaymentMethodId")));
        }

        if (this.newAPPaymentPM.AmountInPaymentCurrency == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.AmountInPaymentCurrency")));
        }

        if (this.newAPPaymentPM.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (this.PaymentMethodCode != null) {
                if (this.PaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                }
            }

            if (!isAllowed) {
                errors.push(TextCodeTranslator.Translate("APPayment.M.CantSetZeroAmount"));
            }
        }

        if (this.newAPPaymentPM.AmountInPaymentCurrency < 0) {
            if (!this.IsNegativeAmountEnabled) {
                errors.push(TextCodeTranslator.Translate("APPayment.M.CantSetMinusAmount"));
            }
        }

        if (AppTool.IsNullOrEmpty(this.newAPPaymentPM.BranchId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.BranchId")));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.IsCreatedFromInvoiceSide) {
                if (this.VendorId != this.invoicePm.VendorId) {
                    errors.push("Bill to doesn't match the invoice bill to");
                }

                if (this.PaymentCurrencyId != this.invoicePm.InvoiceCurrencyId) {
                    errors.push("Payment currency doesn't match the invoice currency");
                }
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            // Check Full Accounting
            if (SessionLocator.TenantPM.AccountingActivated == true) {
                //var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
                //invoiceDomainService.ValidateAPPaymentFullAccounting(this.AccountingPaymentMethodCode, this.newAPPaymentPM.PaymentCurrencyId, this.newAPPaymentPM.VendorId, this.newAPPaymentPM.AccountingPaymentMethodCode, this.newAPPaymentPM.RegisterDate, this.newAPPaymentPM.BankAccountId).subscribe((response: ServiceResponse) => {
                //    if (response != null) {
                //        if (!response.HasError) {
                //            this.CompleteSubmission(errors);
                //        }
                //        else {
                //            this.ValidationErrorsList = response.ErrorsArray;
                //        }
                //    }
                //});

                this.CompleteSubmission(errors);
            }
            else {
                this.CompleteSubmission(errors);
            }
        }
    }

    CompleteSubmission(errors) {
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.invoicePm != null && !AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
                this.ConnectARInvoiceToPayment(this.newAPPaymentPM, this.invoicePm);
            }

            this.RunEditWindow();
        }
    }
    ConnectARInvoiceToPayment(entityPM: APPaymentPM, invoicePM: APInvoicePM) {
        var connectAmount = this.GetSmallestAmount(entityPM, invoicePM);
        var connectAmountLocal = connectAmount * invoicePM.InvoiceCurrencyExchangeRate;

        var record: APPaymentInvoicePM = new APPaymentInvoicePM(null);

        record.Tenant = SessionLocator.TenantPM.Id;
        record.APInvoiceId = invoicePM.Id;
        record.APPaymentId = entityPM.Id;
        record.ForeignCurrencyId = invoicePM.InvoiceCurrencyId;
        record.ForeignAmount = connectAmount == null ? 0 : connectAmount;
        record.LocalAmount = connectAmountLocal == null ? 0 : connectAmountLocal;
        record.PaymentAmount = record.ForeignAmount;
        record.ExchangeRate = invoicePM.InvoiceCurrencyExchangeRate;

        entityPM.PaymentInvoices = [];
        entityPM.PaymentInvoices.push(record);

        if (entityPM.OpenAmount > connectAmount) {
            entityPM.OpenAmount = entityPM.OpenAmount - connectAmount;
        }

        else {
            entityPM.OpenAmount = 0;
        }
    }
    GetSmallestAmount(paymentPM: APPaymentPM, invoicePM: APInvoicePM): number {
        var invoiceAmount = invoicePM.AmountDue;
        var paymentAmount = paymentPM.OpenAmount == null ? 0 : paymentPM.OpenAmount;
        var smallestAmount = null;

        if (invoiceAmount <= paymentAmount) {
            smallestAmount = invoiceAmount;
        }

        else {
            smallestAmount = paymentAmount;
        }

        return smallestAmount;
    }
    RunEditWindow() {

        this.CurrentSession.CurrentWindow.WindowClosed.subscribe(s => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.newAPPaymentPM.Id, EntityPM: this.newAPPaymentPM, ObjectTableName: 'APPayment' });

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.CurrentSession.FireEvent("NewAPPaymentInvoiceTabCreated");
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
        });

        this.CurrentSession.CloseCurrentWindow();
    }
}
