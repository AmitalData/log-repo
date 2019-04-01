import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceTool, InvoicePartnerType} from '../../../../Invoice/Tools';
import {InvoiceTotalsClass} from '../../../../Invoice/Args';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {UpdateCurrencyRateComponent} from '../../../../CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {GLAccountPMService} from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {GLAccountPM} from '../../../../Accounting/EntityPMs/GLAccountPM';
import {AccountingPeriodExtendedListService} from '../../../../Accounting/Services/ExtendedLists/AccountingPeriodExtendedListService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'NewGeneralARInvoiceComponent',
    moduleId: module.id,
    templateUrl: './NewGeneralARInvoiceComponent.html',
})

export class NewGeneralARInvoiceComponent extends BaseComponent {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
    public DataContext = this;
    public InvoicePartners: InvoicePartnerType[] = [];
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public DisplaySATPaymentMethod: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.InitializeServices();
           
        });

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            this.DisplaySATPaymentMethod = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }

    private TypeCode = "";
    SetWindowArgs(args: any) {
        this.TypeCode = args["InvoiceTypeCode"];
        this.CreateNewEntity();
    }

    private myCardListService: CardListService;
    private myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    private myVatTypeListService: VatTypeListService;
    private myChargesTypeListService: ChargesTypeListService;
    private myCommonDomainService: CommonDomainService;
    private myGLAccountPMService: GLAccountPMService;
    private myAccountingPeriodListService: AccountingPeriodExtendedListService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService();
        this.myGLAccountPMService = new GLAccountPMService();
        this.myAccountingPeriodListService = new AccountingPeriodExtendedListService();
    }

    private accountingPeriod:any;
    private GetClosedMonth() {
        var periodTypeCode = "1" // 1-Regular
        this.myAccountingPeriodListService.getByYear(this.InvoiceDate.getFullYear(), periodTypeCode).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.accountingPeriod = myResponse.Result;
                }
            }
        });
    }
     
    CreateNewEntity() {
        var todayDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new ARInvoicePM();
        this.EntityPM.BillToPartnerTypeId = "CS";
        this.EntityPM.StatusCode = "DR";
        this.EntityPM.StatusName = "Draft";
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.IssuedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.InvoiceDate = todayDate;
        this.EntityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        this.EntityPM.LocalCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.EntityPM.ARInvoiceTypeCode = this.TypeCode;
        //this.EntityPM.PrepaidCollectId = this.args.InvoiceTypeCode == "MN" ? "C" : "B";
        this.EntityPM.MainEntityId = null;
        this.EntityPM.MainEntityReference = null;
        this.EntityPM.HouseNumber =null;
        this.EntityPM.MasterNumber = null;
        //this.EntityPM.ProfitCurrencyId = this.shipmentPM.ProfitCurrencyId;
        //this.EntityPM.OperationalDate = InvoiceTool.GetOperationalDate(this.shipmentPM);

        var myDescription: string = null;
        //switch (this.shipmentPM.DirectionId) {
        //    case "E": { myDescription = "Export to " + this.shipmentPM.MainCarriageToPortCode; break; }
        //    case "I": { myDescription = "Import from " + this.shipmentPM.MainCarriageFromPortCode; break; }
        //    case "D": { myDescription = "Ship to " + this.shipmentPM.ToPartnerCity; break; }
        //}
        this.EntityPM.Description = myDescription;
        this.EntityPM.IsGeneralInvoice = true;
        this.EntityPM.IsFullAccounting = true;
        this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
        this.EntityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;

        this.SetUIProperties();
        this.BuildPartnersTypes();
        this.LoadData();
        this.IsResourcesReady = true;
    }

    // SetUIProperties
    public RateIsEnabled: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    private SetUIProperties() {
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        //this.SetUIProperties_General(false);
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    }
    SetUIProperties_BillToAddress() {
        var isFieldtEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.BillToId)) {
            isFieldtEnabled = true;
        }

        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isFieldtEnabled);
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
    SetUIProperties_General(isEnabled: boolean) {
        this.UIProperties.SetEnabled("IsConstituentInvoice", this.ObjectTableName, isEnabled);
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

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
            }
        }
    }

    // BillTo
    public BillToDependencyProperty1: string = null;
    public BillToDependencyProperty1IsList: boolean = true;
    BuildPartnersTypes() {

        this.BillToDependencyProperty1 = "CS";

        if (this.EntityPM.Id == null) {
            //if (this.args.EntityTableName == "Shipment") {
            //    var customer: InvoicePartnerType = this.InvoicePartners.filter(d => d.Code == "CUS")[0];
            //    this.SelectedPartnerType = customer;
            //    this.PartnersTypeSelectionMethod(customer);
            //}

            //else if (this.args.EntityTableName == "Master") {
            //    var agent: InvoicePartnerType = this.InvoicePartners.filter(d => d.Code == "AGE")[0];
            //    this.SelectedPartnerType = agent;
            //    this.PartnersTypeSelectionMethod(agent);
            //}
        }
    }

    public SelectedPartnerType: InvoicePartnerType = null;
 
    private billToPartnerTypeId: string;
    get BillToPartnerTypeId() { return this.billToPartnerTypeId; }
    set BillToPartnerTypeId(newValue: string) {
        if (this.billToPartnerTypeId != newValue) {
            this.billToPartnerTypeId = newValue;
        }
    }

    private cardList: CardList = null;
    private glaccount: GLAccountPM = null;
    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.SetUIProperties_BillToAddress();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToAddressId = null;
                this.SATPaymentMethodCode = null;
                this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        this.cardList = list;
                        if (list != null) {
                            this.VatNumber = list.VatNumber;
                            this.BillToName = list.EnglishName;
                            if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                            }

                            if (this.isOkClicked == false) {
                                if (this.ValidationErrorsList != null && this.ValidationErrorsList.length == 0) {
                                    this.ValidationErrorsList = [];
                                }
                            }
                            else {
                                this.ValidateEntity();
                            }

                            if (AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                if (this.isOkClicked == false) {
                                    this.ValidationErrorsList = [];
                                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg1"));
                                }
                            }

                            else {
                                this.ValidationErrorsList = null;
                                this.myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        this.glaccount = myResponse.Result;
                                        if (this.glaccount != null) {
                                            if (this.glaccount.IsMultiCurrency == true) {
                                                this.InvoiceCurrencyId = null;
                                            }
                                            else {
                                                this.InvoiceCurrencyId = this.glaccount.CurrencyId;
                                            }
                                        }
                                    }
                                });
                            }

                            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                this.PaymentTermId = list.PaymentTermId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.VatTypeId)) {
                                this.VatTypeId = list.VatTypeId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.BillingAddressId)) {
                                this.BillToAddressId = list.BillingAddressId;
                            }

                            else if (!AppTool.IsNullOrEmpty(list.MainAddressId)) {
                                this.BillToAddressId = list.MainAddressId;
                            }

                            if (this.isOkClicked == true) {
                                this.ValidateEntity();
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

            if (this.isOkClicked == false) {
                if (this.ValidationErrorsList == null) {
                    this.ValidationErrorsList = [];
                }
                // Check if the same as glaccount currency
                if (this.glaccount != null && this.glaccount.IsMultiCurrency == false && newValue != this.glaccount.CurrencyId) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg2"));
                }
            }
            else {
                this.ValidateEntity();
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
    private vatTypeId: string;
    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.vatTypeId != newValue) {
            this.vatTypeId = newValue;
        }
    }

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
            this.GetClosedMonth();
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

    get IsConstituentInvoice() { return this.EntityPM.IsConstituentInvoice; }
    set IsConstituentInvoice(newValue: boolean) {
        if (this.EntityPM.IsConstituentInvoice != newValue) {
            this.EntityPM.IsConstituentInvoice = newValue;
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
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
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

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
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
    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
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
    private isOkClicked = false;
    private errors: string[] = [];
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.isOkClicked = true;
        this.ValidateEntity();
      
        if (this.ValidationErrorsList.length == 0) {
            this.OnEntityValid();
        }
    }
    ValidateEntity() {
        this.errors = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid) this.errors.push(TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth")); // closed month

        if (this.glaccount != null && this.glaccount.IsMultiCurrency == false && this.InvoiceCurrencyId != this.glaccount.CurrencyId) {
            this.errors.push(TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg2"));
        }

        if (this.cardList != null && AppTool.IsNullOrEmpty(this.cardList.GLAccountId)) {
            this.errors.push(TextCodeTranslator.Translate("ARInvoice.M.NewGeneralInvoiceErrorMsg3"));
        }

        if (AppTool.IsNullOrEmpty(this.BillToId)) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.BillToId")));
        }

        //if (AppTool.IsNullOrEmpty(this.BillToAddressId)) {
        //    this.errors.push(msg.replace("%FieldName", "Address"));
        //}

        if (AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyId")));
        }

        if (this.InvoiceDate == null) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));
        }

        else {
            var date1 = new Date(this.InvoiceDate.toString());
            var date2 = DateTool.GetCurrentDateAsUtc();

            if (date1.valueOf() > date2.valueOf()) {
                this.errors.push(TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
            }
        }

        if (this.DueDate == null) {
            this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.DueDate")));
        }

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.VatNumber")));
            }
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.SATPaymentMethodCode")));
            }
        }

        if (this.DueDate.valueOf() < this.InvoiceDate.valueOf()) {
            this.errors.push(TextCodeTranslator.Translate("ARInvoice.M.DueDateLowerThanInvoiceDate"));
        }

        this.ValidationErrorsList = this.errors;
    }
    IsMonthOpenForAccountingDate() {
        var valid = true;
        if (this.accountingPeriod == null) {
            valid = false;
            //errorsList.Add(transText);
        }
        else {
            var accountingDateMonth = this.EntityPM.InvoiceDate.getMonth() + 1;

            if (accountingDateMonth > this.accountingPeriod.ClosedMonth) {
                //Valid ... AccountingDateMonth must be greater than close Mounth
            }
            else {
                //Not Valid ... AccountingDateMonth must be greater than close Mounth
                //not valid  8>=8 
                //not valid  0>=1 - Must Open mounth before work on year !!
                valid = false;
                //errorsList.Add(transText); //ClosedMonth Must B
            }
            if (accountingDateMonth == this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  equal to OpenMonth
            }
            else if (accountingDateMonth < this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  less than OpenMonth
            }
            else {
                valid = false;
                //errorsList.Add(transText);
            }
        }
        return valid;
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
        this.EntityPM.StatusCode = "DR";
        this.EntityPM.StatusName = "Draft";
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }
}
