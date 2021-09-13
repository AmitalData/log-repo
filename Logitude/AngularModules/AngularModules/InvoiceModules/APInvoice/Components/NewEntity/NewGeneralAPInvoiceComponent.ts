import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceTotalVATPM} from '../../../../Invoice/EntityPMs/APInvoiceTotalVATPM';
import {APInvoicePMService} from '../../../../Invoice/Services/StandardPMs/APInvoicePMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {DateTool, AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {UpdateCurrencyRateComponent} from '../../../../CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {InvoiceTotalsClass} from '../../../../Invoice/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {GLAccountPMService} from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    
    templateUrl: './NewGeneralAPInvoiceComponent.html',
})

export class NewGeneralAPInvoiceComponent extends BaseComponent {
    public EntityPM: APInvoicePM;
    public ObjectTableName: string = "APInvoice";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsAccountingActivated = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    DisplayFieldsFromList:string;
    DisplayLocalFieldsFromList:string;
    VendorLovSizeForFullAccounting:number;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.InitializeServices();
        this.InitializeVendorLov();
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }

    private InitializeVendorLov() {
        if (this.IsAccountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.VendorLovSizeForFullAccounting = 550;
        }
    }

    public AllVatTypes: VatTypeList[] = [];
    private myCardListService: CardListService;
    private myPaymentTermListService: PaymentTermListService;
    private myCurrencyListService: CurrencyListService;
    private myChargesTypeListService: ChargesTypeListService;
    private myVatTypeListService: VatTypeListService;
    private myInvoiceDomainService: InvoiceDomainService;
    private myEntityPMService: APInvoicePMService;
    public myGLAccountPMService: GLAccountPMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myEntityPMService = new APInvoicePMService();
        this.myGLAccountPMService = new GLAccountPMService();

        this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });
    }

    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            var todayDate = DateTool.GetCurrentDateAsUtc();
            this.EntityPM = this.myEntityPMService.GetNewEntityPM();
            this.EntityPM.IsGeneralInvoice = true;
            this.AccountingDate = todayDate;
            this.EntityPM.MainEntityId = null;
            this.EntityPM.MainEntityReference = null;
            this.EntityPM.HouseNumber = null;
            this.EntityPM.MasterNumber = null
            this.EntityPM.MasterShipmentNumbers = null;
            this.EntityPM.MasterNumbers = null;
            this.EntityPM.HouseNumbers = null;
            InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            this.IsResourcesReady = true;
            this.SetUIProperties();
            this.LoadData();
        });
    }

    // SetUIProperties
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        var isVatNumberRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.VATNumber)) {
                isVatNumberRequired = true;
            }
        }

        this.UIProperties.SetRequired("VATNumber", "APInvoice", isVatNumberRequired);
        this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingDate));

        this.SetUIProperties_DueDate();
        this.SetUIProperties_ExchangeRate();
    }
    SetUIProperties_DueDate() {
        var AllowManuallyDueDate: boolean = false;

        if (SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);

        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    }
    SetUIProperties_ExchangeRate() {
        var isEnabled = false;

        if (FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
            if (this.InvoiceCurrencyId) {
                if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                    isEnabled = true;
                }
            }
        }

        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", "APInvoice", isEnabled);
    }

    // Load Data
    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var myCurrencyRatesService = new CurrencyRatesService();
        var myCommonDomainService = new CommonDomainService();

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

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
    GetCurrencyRate(currencyId: string) {
        var myResult: number = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetCurrencyRateDate(currencyId: string) {
        var myResult: Date = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = null;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }

        return myResult;
    }
    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }

    // Vendor Properties
    get VendorDependencyProperty1() { return InvoiceTool.GetGeneralAPInvoiceVendorPartnerTypes(); }
    get InternalNotes() { return this.EntityPM.InternalNotes; }
    set InternalNotes(value: string) {
        if (this.EntityPM.InternalNotes != value) {
            this.EntityPM.InternalNotes = value;

        }
    }
    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;

            this.CheckDuplication();

            if (AppTool.IsNullOrEmpty(value)) {
                this.VATNumber = null;
                this.VendorName = null;
                this.EntityPM.VendorPartnerTypeId = null;
                this.InvoiceCurrencyId = SessionLocator.AccountingCurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
                this.EntityPM.VendorGLAccountId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list != null) {
                            this.VATNumber = list.VatNumber;
                            this.VendorName = list.EnglishName;
                            this.VendorLocalName = list.LocalName || list.EnglishName;
                            this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
                            this.SetInvoiceCurrency(list);
                            //if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                            //    this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                            //}

                            if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                this.PaymentTermId = list.PaymentTermId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.VatTypeId)) {
                                this.VatTypeId = list.VatTypeId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                this.myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        var glaccount = myResponse.Result;
                                        if (glaccount != null) {
                                            this.EntityPM.VendorGLAccountId = glaccount.Id;
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
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
    get VendorName() { return this.EntityPM.VendorName; }
    set VendorName(value: string) {
        if (this.EntityPM.VendorName != value) {
            this.EntityPM.VendorName = value;
        }
    }

    get VendorLocalName() { return this.EntityPM.VendorLocalName; }
    set VendorLocalName(value: string) {
        if (this.EntityPM.VendorLocalName != value) {
            this.EntityPM.VendorLocalName = value;
        }
    }

    get InvoiceNumber() { return this.EntityPM.InvoiceNumber; }
    set InvoiceNumber(value: string) {
        if (this.EntityPM.InvoiceNumber != value) {
            this.EntityPM.InvoiceNumber = value;
            this.CheckDuplication();          
        }
    }

    CheckSpecialCharacters() {
        if (FeatureLocator.HasFeaturePermession("APInvoice", "INSC")) {
            var invoiceNumber_Check = /^[A-Za-z0-9]+$/i;
            if (!invoiceNumber_Check.test(this.InvoiceNumber)) {
                return TextCodeTranslator.Translate("APInvoice.O.ValidateInvoiceNumber");
            }
        }
    }

    CheckDuplication() {
        var warnings: string[] = [];
        this.FillWarnings(warnings);

        if (!AppTool.IsNullOrEmpty(this.VendorId) && !AppTool.IsNullOrEmpty(this.InvoiceNumber)) {

            this.myInvoiceDomainService.CheckVendor_NumberDuplication(this.EntityPM.VendorId, this.EntityPM.InvoiceNumber, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    var isDuplicated: boolean = myResponse.Result;

                    if (isDuplicated) {
                        warnings.push(TextCodeTranslator.Translate("APInvoice.M.SameInvoiceNumber"));
                        this.FillWarnings(warnings);
                    }
                }
            });
        }
    }

    // Currency Properties
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(value: string) {
        if (this.EntityPM.InvoiceCurrencyId != value) {
            this.EntityPM.InvoiceCurrencyId = value;
            this.SetCurrencyRateData();
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.InvoiceCurrencyCode = null;
            }

            else {
                this.myCurrencyListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list != null) {
                            this.InvoiceCurrencyCode = list.Code;
                        }
                    }
                });
            }
        }
    }

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    set InvoiceCurrencyCode(value: string) {
        if (this.EntityPM.InvoiceCurrencyCode != value) {
            this.EntityPM.InvoiceCurrencyCode = value;
        }
    }

    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.InvoiceCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    }

    get InvoiceCurrencyExchangeRate() { return this.EntityPM.InvoiceCurrencyExchangeRate; }
    set InvoiceCurrencyExchangeRate(value: number) {
        var setValue: number = AppTool.Round(value, 5);
        if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
            this.EntityPM.InvoiceCurrencyExchangeRate = setValue;
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM.ExchangeRateDate != value) {
            this.EntityPM.ExchangeRateDate = value;
            this.ComputeRelativeRateDate();
        }
    }

    private myRelativeRateDate: string = null;
    get RelativeRateDate() { return this.myRelativeRateDate; }
    set RelativeRateDate(value: string) {
        if (this.myRelativeRateDate != value) {
            this.myRelativeRateDate = value;
        }
    }
    ComputeRelativeRateDate() {
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    }

    get ProfitCurrencyId() { return this.EntityPM.ProfitCurrencyId; }
    set ProfitCurrencyId(value: string) {
        if (this.EntityPM.ProfitCurrencyId != value) {
            this.EntityPM.ProfitCurrencyId = value;
            this.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(value);
        }
    }

    get ProfitCurrencyExchangeRate() { return this.EntityPM.ProfitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(value: number) {
        if (this.EntityPM.ProfitCurrencyExchangeRate != value) {
            this.EntityPM.ProfitCurrencyExchangeRate = AppTool.Round(value, 5);
        }
    }

    // UpdateCurrencyRate
    public RateIsEnabled: boolean = false;
    UpdateCurrencyRateClicked() {
        this.entityResourceService.getEntityResourceByTableName("RatesTable").subscribe((res: any) => {
            var loadingDate = this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: this.InvoiceCurrencyId, CurrencyCode: this.InvoiceCurrencyCode, Rate: this.InvoiceCurrencyExchangeRate, Date: loadingDate };
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.LastRatesList = comp.RatesList;
                        this.InvoiceCurrencyExchangeRate = comp.Rate;
                        this.ExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    }

    // Properties
    get VATNumberRedDotVisibility() {
        var myResult = false;
        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            myResult = true;
        }

        return myResult;
    }

    private vatTypeId = null;

    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(value: string) {
        if (this.vatTypeId != value) {
            this.vatTypeId = value;

            if (this.EntityPM.InvoiceLines != null) {
                this.EntityPM.InvoiceLines.forEach(item => {
                    item.VatTypeId = value;
                    if (AppTool.IsNullOrEmpty(value)) {
                        item.VatTypeName = null;
                        item.VatPercentage = null;
                    }

                    else {
                        this.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: VatTypeList = myResponse.Result;
                                if (list != null) {
                                    item.VatTypeName = list.EnglishName;
                                    item.VatPercentage = this.GetVatTypePercentage(this.VatTypeId);
                                }
                            }
                        });
                    }
                });

            }
        }
    }

    get VATNumber() { return this.EntityPM.VATNumber; }
    set VATNumber(value: string) {
        if (this.EntityPM.VATNumber != value) {
            this.EntityPM.VATNumber = value;
            this.SetUIProperties();
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(value: string) {
        if (this.EntityPM.PaymentTermId != value) {
            this.EntityPM.PaymentTermId = value;

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

            // if (!this.IsAccountingActivated) {
                InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            // }
            // else {
            //     InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
            // }
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
            // if (!this.IsAccountingActivated) {
                InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            // }
            this.ComputeRelativeRateDate();
            this.LoadData();
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(value: Date) {
        if (this.EntityPM.DueDate != value) {
            this.EntityPM.DueDate = value;
            InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
        }
    }

    get AccountingDate() { return this.EntityPM.AccountingDate; }
    set AccountingDate(value: Date) {
        if (this.EntityPM.AccountingDate != value) {
            this.EntityPM.AccountingDate = value;
            if (value == null) {
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, false);
            }

            // if (this.IsAccountingActivated) {
            //   //InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
            //     InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            //     this.LoadData();
            // }
        }
    }

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
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

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
     invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
    OkButtonClicked() {
        var errors: string[] = [];
       
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");      
        this.CheckSpecialCharacters() != null ? errors.push(this.CheckSpecialCharacters()) : null;
        if (AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.VendorId")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceNumber")));
        }

        if (this.EntityPM.InvoiceExpectedAmount == null) {
            errors.push(msg.replace("%FieldName", "Invoice Amount"));
        }
        if (this.EntityPM.InvoiceDate > this.EntityPM.AccountingDate) {
            errors.push(TextCodeTranslator.Translate("APInvoice.O.CheckInvoiceDate"));
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceCurrencyId")));
        }

        if (this.EntityPM.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceDate")));
        }


        else if (DateTool.GetDateParts(this.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
            errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }

        if (DateTool.GetDateParts(this.AccountingDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
            errors.push("Cant issue Invoice with Future Accounting Date");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.PaymentTermId)) {
            errors.push(msg.replace("%FieldName", "Payment Term"));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.PaymentTermId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.PaymentTermId")));
        }

        if (this.EntityPM.DueDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.DueDate")));
        }

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.VATNumber)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.VATNumber")));
            }
        }

        if (this.IsAccountingActivated && this.AccountingDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.AccountingDate")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.BranchId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.BranchId")));
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CompleteSubmission();
        }
    
    }

    ValidateInvoiceNumber(errors:string[]) {
        this.invoiceDomainService.ValidateInvoiceNumber(this.EntityPM.InvoiceNumber).subscribe((response: ServiceResponse) => {
            if (response != null) {
                if (!response.HasError) {
                    if (this.IsAccountingActivated == true) {
                        this.ValidateAPInvoiceFullAccounting(errors);
                    }
                    else {
                        this.CompleteSubmission();
                    }
                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
            }
        });
    }
    ValidateAPInvoiceFullAccounting(errors: string[]) {
       // var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
        this.invoiceDomainService.ValidateAPInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.VendorId, this.EntityPM.AccountingDate).subscribe((response: ServiceResponse) => {
            if (response != null) {
                if (!response.HasError) {
                    this.ValidateInvoiceDate(errors);
                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
            }
        });
    }
    
    CompleteSubmission() {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

        this.InitializeProfitCurrency();
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    private ValidateInvoiceDate(errors) {
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
           // var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
            this.invoiceDomainService.ValidateInvoiceDate(this.EntityPM.InvoiceDate).subscribe((response: ServiceResponse) => {
                if (response != null) {
                  this.CurrentSession.StopBusyIndicator();
                    if (!response.HasError ) {
                       if(response.Result != null){
                        this.ShowConfirmWindow(response.Result);
                        }
                      else{
                       this.CompleteSubmission();
                           }
                    }
                    else {
                     this.ValidationErrorsList = response.ErrorsArray;
                    }
                }

            });
        }
    }

    private ShowConfirmWindow(warningMessage: string) {

        let confirmWindow = new ConfirmWindow();
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");

        confirmWindow.ShowWarningImage = true;
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CompleteSubmission();

            }
        });
        confirmWindow.Show(warningMessage);

    }

    private InitializeProfitCurrency() {
        if (this.EntityPM.IsMultipleEntities) {
            this.EntityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        else if (AppTool.IsNullOrEmpty(this.EntityPM.ProfitCurrencyId)) {
            this.EntityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        this.myCurrencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
                if (list != null) {
                    this.EntityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });

        this.EntityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
    }

    get AmountInInvoiceCurrency() { return this.EntityPM.AmountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
            this.EntityPM.AmountInInvoiceCurrency = setValue;
            this.EntityPM.InvoiceExpectedAmount = setValue;
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInLocalCurrency != setValue) {
            this.EntityPM.AmountInLocalCurrency = setValue;
            this.EntityPM.AmountDueInLocalCurrency = setValue;
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInProfitCurrency != setValue) {
            this.EntityPM.AmountInProfitCurrency = setValue;
            this.EntityPM.AmountDueInProfitCurrency = setValue;
        }
    }

    get SubTotalInLocalCurrency() {
        return this.EntityPM.SubTotalInLocalCurrency;
    }
    set SubTotalInLocalCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);

        if (this.EntityPM.SubTotalInLocalCurrency != setValue) {
            this.EntityPM.SubTotalInLocalCurrency = setValue;
        }
    }

    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        var setValue = AppTool.Round(value, 2);

        if (this.EntityPM.SubTotalInInvoiceCurrency != setValue) {
            this.EntityPM.SubTotalInInvoiceCurrency = setValue;
        }
    }
}
