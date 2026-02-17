import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoicePMService} from '../../../../Invoice/Services/StandardPMs/ARInvoicePMService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool, AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceTool, InvoicePartnerType} from '../../../../Invoice/Tools';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {UpdateCurrencyRateComponent} from '../../../../CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './NewConsolidationComponent.html',
})

export class NewConsolidationComponent extends BaseComponent {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
    public DataContext = this;
    public InvoicePartners: InvoicePartnerType[] = [];
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
  public DisplaySATSettings: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");          
        this.InitializeServices();
        
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }        
    }

    private myCardListService: CardListService;
    private myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    private myInvoiceDomainService: InvoiceDomainService;
    private myEntityPMService: ARInvoicePMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myEntityPMService = new ARInvoicePMService();
    }
   
    SetWindowArgs(typeCode: string) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = this.myEntityPMService.GetNewEntityPM();           
            this.EntityPM.ARInvoiceTypeCode = typeCode;
            this.EntityPM.IsConsolidationInvoice = true;
            this.IsResourcesReady = true;

            this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;

            this.SetUIProperties();
            this.BuildPartnersTypes();
          this.LoadData();

          if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATSettings = true;
            this.MetodoPagoCode = SessionLocator.SATInterfaceSettings.MetodoPagoCode;

            if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
              this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
            }
          }
        });
    }

    // SetUIProperties
    public RateIsEnabled: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        this.SetUIProperties_BillTo();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    }
    SetUIProperties_BillTo() {
        var isBillToEnabled = false;
        var isBillToAddressEnabled = false;

        if (this.SelectedPartnerType) {
            if (!AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
                isBillToEnabled = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.BillToId)) {
            isBillToAddressEnabled = true;
        }

        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isBillToEnabled);
        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isBillToAddressEnabled);
    }
    SetUIProperties_VatNumber() {
        var isFieldRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }

            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    }
    SetUIProperties_ExchangeRate() {
        var isFieldtEnabled = false;

        if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
            if (this.InvoiceCurrencyId) {
                if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                    isFieldtEnabled = true;
                }
            }
        }
        
        this.RateIsEnabled = isFieldtEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldtEnabled);
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
    SetUIProperties_Payment() {
        this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, false);
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
          }

          if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
            this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
          }
        }
    }

  get MetodoPagoCode() { return this.EntityPM.MetodoPagoCode; }
  set MetodoPagoCode(newValue: string) {
    if (this.EntityPM) {
      if (this.EntityPM.MetodoPagoCode != newValue) {
        this.EntityPM.MetodoPagoCode = newValue;
        this.SetUIProperties_Payment();
      }
    }
  }
    // BillTo
    public BillToDependencyValue1: string = "CS";
    public BillToDependencyValue2: boolean = false;
    public BillToDependencyValue1IsList: boolean = false;
    BuildPartnersTypes() {
        this.InvoicePartners = InvoiceTool.GetARInvoicePartners(null);
        this.PartnersTypeSelectionMethod(this.InvoicePartners[0]);
    }

    public SelectedPartnerType: InvoicePartnerType = null;
    PartnersTypeSelectionMethod(selected: InvoicePartnerType) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected;

            this.BillToId = null;
            this.BillToAddressId = null;
            this.BillToPartnerTypeId = null;

            if (selected) {
                this.BillToPartnerTypeId = selected.PartnerTypeId;
                this.BillToDependencyValue1 = selected.PartnerTypeId;
                this.BillToDependencyValue2 = selected.IsCustomer;
            }

            this.SetUIProperties();
        }
    }

    get BillToPartnerTypeId() { return this.EntityPM.BillToPartnerTypeId; }
    set BillToPartnerTypeId(newValue: string) {
        if (this.EntityPM.BillToPartnerTypeId != newValue) {
            this.EntityPM.BillToPartnerTypeId = newValue;
        }
    }

    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.SetUIProperties_BillTo();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToAddressId = null;
                this.SATPaymentMethodCode = null;
                this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
                this.EntityPM.IsBillToAllowConsolidation = false;

                this.EntityPM.BillToIsCreditLimitEnabled = false;
                this.EntityPM.BillToCreditLimitAmount = null;
                this.EntityPM.BillToCreditLimitOpenBalance = null;
                this.EntityPM.BillToCreditLimitWarningPercentage = null;
                this.EntityPM.BillToBlockNewInvoiceCreation = false;
                this.EntityPM.BillToBlockNewInvoiceCreation = false;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        if (list != null) {
                            this.VatNumber = list.VatNumber;
                            this.BillToName = list.EnglishName;
                            this.EntityPM.IsBillToAllowConsolidation = list.EnableConsolidationInvoices;

                            this.EntityPM.BillToIsCreditLimitEnabled = list.IsCreditLimitEnabled;
                            this.EntityPM.BillToCreditLimitAmount = list.CreditLimitAmount;
                            this.EntityPM.BillToCreditLimitOpenBalance = list.CreditLimitOpenBalance;
                            this.EntityPM.BillToCreditLimitWarningPercentage = list.CreditLimitWarningPercentage;
                            this.EntityPM.BillToBlockNewInvoiceCreation = list.BlockNewInvoiceCreation;

                            if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                          }
                          if (!AppTool.IsNullOrEmpty(list.MetodoPagoCode)) {
                            this.MetodoPagoCode = list.MetodoPagoCode;
                          }
                          else if (SessionLocator.SATInterfaceSettings != null && !AppTool.IsNullOrEmpty(SessionLocator.SATInterfaceSettings.MetodoPagoCode)) {
                            this.MetodoPagoCode = SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                          }

                            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                this.PaymentTermId = list.PaymentTermId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.BillingAddressId)) {
                                this.BillToAddressId = list.BillingAddressId;
                            }

                            else if (!AppTool.IsNullOrEmpty(list.MainAddressId)) {
                                this.BillToAddressId = list.MainAddressId;
                            }

                            else {
                                this.BillToAddressId = null;
                            }
                        }
                    }
                });
            }
        }
    }

    get BillToName() { return this.EntityPM.BillToName; }
    set BillToName(newValue: string) {
        if (this.EntityPM.BillToName != newValue) {
            this.EntityPM.BillToName = newValue;
        }
    }

    get BillToAddressId() { return this.EntityPM.BillToAddressId; }
    set BillToAddressId(newValue: string) {
        if (this.EntityPM.BillToAddressId != newValue) {
            this.EntityPM.BillToAddressId = newValue;
        }
    }

    // Currency
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyId != newValue) {
            this.EntityPM.InvoiceCurrencyId = newValue;
            this.SetCurrencyRateData();
            this.SetUIProperties_ExchangeRate();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.InvoiceCurrencyCode = null;
            }

            else {
                this.myCurrencyListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
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

    // Properties
    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;
            this.SetUIProperties_VatNumber();
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(newValue: string) {
        if (this.EntityPM.PaymentTermId != newValue) {
            this.EntityPM.PaymentTermId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PaymentTermName = null;
            }

            else {
                this.myPaymentTermListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            this.PaymentTermName = list.EnglishName;
                        }
                    }
                });
            }

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
        }
    }

    get PaymentTermName() { return this.EntityPM.PaymentTermName; }
    set PaymentTermName(newValue: string) {
        if (this.EntityPM.PaymentTermName != newValue) {
            this.EntityPM.PaymentTermName = newValue;
        }
    }

    get InvoiceDate() { return this.EntityPM.InvoiceDate; }
    set InvoiceDate(newValue: Date) {
        if (this.EntityPM.InvoiceDate != newValue) {
            this.EntityPM.InvoiceDate = newValue;

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
            this.ComputeRelativeRateDate();
            this.LoadData();
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(newValue: Date) {
        if (this.EntityPM.DueDate != newValue) {
            this.EntityPM.DueDate = newValue;
            InvoiceTool.ComputeARInvoicePaymentTerm(this.EntityPM);
        }
    }

    get CustomerRef() { return this.EntityPM.CustomerRef; }
    set CustomerRef(newValue: string) {
        if (this.EntityPM.CustomerRef != newValue) {
            this.EntityPM.CustomerRef = newValue;
        }
    }

    get SATPaymentMethodCode() { return this.EntityPM.SATPaymentMethodCode; }
    set SATPaymentMethodCode(newValue: string) {
        if (this.EntityPM.SATPaymentMethodCode != newValue) {
            this.EntityPM.SATPaymentMethodCode = newValue;
            this.SetUIProperties_Payment();
        }
    }

    // Load Date 
    private LastRatesList: LastRate[] = [];
    private myCurrencyRatesService: CurrencyRatesService;
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
                this.SetCurrencyRateData();                
            }

            this.CurrentSession.StopBusyIndicator();
        });
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
    UpdateCurrencyRateClicked() {

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
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.PartnerType")));
        }

        if (AppTool.IsNullOrEmpty(this.BillToId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.BillToId")));
        }

        //if (AppTool.IsNullOrEmpty(this.BillToAddressId)) {
        //    errors.push(msg.replace("%FieldName", "Address"));
        //}

        if (AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyId")));
        }

        if (this.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));
        }

        else if (DateTool.GetDateParts(this.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push(TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
        }

        if (this.DueDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.DueDate")));
        }

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.VatNumber")));
            }
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.SATPaymentMethodCode")));
            }
            if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.MetodoPagoCode")));
            }

            if (this.MetodoPagoCode == "PUE" && this.SATPaymentMethodCode == "99") {
                errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
            }
        }

        //if (errors.length == 0) {
        //    if (!this.EntityPM.IsBillToAllowConsolidation) {
        //        errors.push(TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg1"));
        //    }
        //}

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.UpdateCreditLimitFlags();

            if (SessionLocator.TenantPM.AccountingActivated) {
                this.ValidateFullAccounting();
            }

            else if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction && this.EntityPM.BillToIsCreditLimitEnabled) {
                this.ValidateCreditLimit();
            }

            else {
                this.OnEntityValid();
            }
        }
    }

    ValidateFullAccounting() {
        this.CurrentSession.StartBusyIndicator("Checking ...");

        this.myInvoiceDomainService.ValidateARInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.BillToId, this.EntityPM.InvoiceDate).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {

                if (!myResponse.HasError) {
                    if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction) {
                        this.ValidateCreditLimit();
                    }

                    else {
                        this.OnEntityValid();
                    }
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            }
        });
    }
    ValidateCreditLimit() {
        if (this.EntityPM.BillToBlockNewInvoiceCreation) {

            var errorText_Blocking = TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg6") + ": " + this.EntityPM.BillToName;

            var errors: string[] = [];
            var warnings: string[] = [];

            if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                errors.push(errorText_Blocking);
            }

            else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                warnings.push(errorText_Blocking);
            }

            if (errors.length > 0 || warnings.length > 0) {

                var logWindow = new LogitudeWindow();
                logWindow.Width = 450;
                logWindow.Height = 200;
                logWindow.Title = "Credit limit";
                logWindow.WindowArgs = { Errors: errors, Warnings: warnings };

                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.OnEntityValid();
                    }
                });

                logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
            }

            else {
                this.OnEntityValid();
            }
        }

        else {
            this.CurrentSession.StartBusyIndicatorLoading();

            this.myInvoiceDomainService.GetCustomerCreditLimitActualAmount(this.BillToId).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    var errors: string[] = [];
                    var warnings: string[] = [];

                    this.EntityPM.BillToCreditLimitActualAmount = myResponse.Result;
                    this.EntityPM.BillToCreditLimitActualBalance = AppTool.AddAmounts(this.EntityPM.BillToCreditLimitOpenBalance, this.EntityPM.BillToCreditLimitActualAmount);

                    var LimitAmount = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? 0 : this.EntityPM.BillToCreditLimitAmount;
                    var WarningPercentage = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? 0 : this.EntityPM.BillToCreditLimitWarningPercentage;
                    var ActualBalance = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitActualBalance) ? 0 : this.EntityPM.BillToCreditLimitActualBalance;

                    var isBillToHasLimitAmount = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? false : true;
                    var isBillToHasWarningPercentage = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? false : true;

                    if (isBillToHasLimitAmount && ActualBalance > LimitAmount) {
                        var LimitError = "";
                        var LimitWarning = TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg2");
                        LimitError += TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg3") + " " + FormatTool.FormatNumber(LimitAmount) + " (" + this.EntityPM.LocalCurrencyCode + ")."
                        LimitError += " ";
                        LimitError += TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg4") + " " + FormatTool.FormatNumber(ActualBalance) + " (" + this.EntityPM.LocalCurrencyCode + ").";

                        if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                            errors.push(LimitError);
                        }

                        else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                            warnings.push(LimitWarning);
                        }
                    }

                    else if (isBillToHasWarningPercentage && ActualBalance > (WarningPercentage * LimitAmount / 100)) {
                        if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {

                            var RemainingLimit = FormatTool.FormatNumber(LimitAmount - ActualBalance);
                            var PercentageWarning: string = TextCodeTranslator.Translate("ARInvoice.M.NewConsolidationInvoiceErrorMsg5") + " (" + RemainingLimit + ")";
                            warnings.push(PercentageWarning);
                        }
                    }

                    if (errors.length > 0 || warnings.length > 0) {

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = "Credit limit";
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings };

                        logWindow.WindowClosed.subscribe(s => {
                            if (s) {
                                this.OnEntityValid();
                            }
                        });

                        logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                    }

                    else {
                        this.OnEntityValid();
                    }
                }
            });
        }
    }
    OnEntityValid() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.InitializeComponent();
    }

    InitializeComponent() {

        this.myCurrencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
                if (list != null) {
                    this.EntityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });

        this.EntityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);

        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    // CreditLimit
    public HasCreditLimitFeature: boolean = false;
    public HasCreditOverrideFeature: boolean = false;
    public IsCreditLimitActivated: boolean = false;
    public IsCreditLimitHasAction: boolean = false;
    UpdateCreditLimitFlags() {
        this.HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;

        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }
    }
}
