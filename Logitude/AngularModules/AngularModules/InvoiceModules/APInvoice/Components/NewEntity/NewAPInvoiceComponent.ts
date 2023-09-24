import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceTotalVATPM} from '../../../../Invoice/EntityPMs/APInvoiceTotalVATPM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from '../../../../Shipment/EntityPMs/ShipmentPayablePM';
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
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { GLAccountList } from 'Accounting/EntityLists/GLAccountList';
import { GLAccountListService } from 'Accounting/Services/StandardLists/GLAccountListService';
declare var window: any;

@Component({
    
    templateUrl: './NewAPInvoiceComponent.html',
})

export class NewAPInvoiceComponent extends BaseComponent {
    public EntityPM: APInvoicePM;
    public ObjectTableName: string = "APInvoice";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsAccountingActivated = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    public IsFullAccounting: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.IsFullAccounting = SessionLocator.TenantPM.AccountingActivated;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.InitializeServices();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }

    ngOnInit() {
        this.BuildAdditionalFields();
    }

    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;	
    private additionalFieldsScreenCode = "APInvoice.AdditionalFields";
    public ShowAdditionalFieldsScreen: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    BuildAdditionalFields() {

        var objectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ObjectTableName)[0].Id;
        var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === objectTableId && x.Code.toLowerCase() == this.additionalFieldsScreenCode.toLowerCase())[0];

        if (myScreen != null) {

            var myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

            if (myScreenFields.length == 0) {
                myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id);
            }

            if (myScreenFields.length != 0) {
                this.ShowAdditionalFieldsScreen = true;
                this.RunComponent();
            }
        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {

                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = this.additionalFieldsScreenCode;
                cmpRef.instance.LabelWidth = 160;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });
    }

    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(true);
        }
    }

    public AllVatTypes: VatTypeList[] = [];
    public AllCurrencies: CurrencyList[] = [];
    public AllPaymentTerms: PaymentTermList[] = [];
    private myCardListService: CardListService;
    private myPaymentTermListService: PaymentTermListService;
    private myCurrencyListService: CurrencyListService;
    private myChargesTypeListService: ChargesTypeListService;
    private myVatTypeListService: VatTypeListService;
    private myInvoiceDomainService: InvoiceDomainService;
    private myEntityPMService: APInvoicePMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myEntityPMService = new APInvoicePMService();

        this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });

        this.myCurrencyListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllCurrencies = myResponse.Result;
            }
        });

        this.myPaymentTermListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPaymentTerms = myResponse.Result;
            }
        });
    }

    private shipmentPM: ShipmentPM = null;
    private isMultipleEntities: boolean = false;
    SetWindowArgs(args: any) {
        this.shipmentPM = args['ShipmentPM'];
        this.isMultipleEntities = args['IsMultipleEntities'];

        if (this.shipmentPM) {
            this.isMultipleEntities = false;
        }

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = this.myEntityPMService.GetNewEntityPM();
            this.EntityPM.IsMultipleEntities = this.isMultipleEntities;

            this.IsResourcesReady = true;

            this.GetShipmentData(this.shipmentPM);
            this.SetUIProperties();
            this.LoadData();
        });
    }

    // GetShipmentData
    private myMainEntityObjectTableName;
    private GetShipmentData(shipmentPM: ShipmentPM) {
        if (!this.EntityPM.IsMultipleEntities) {
            this.shipmentPM = shipmentPM;

            if (shipmentPM.ShipmentLevelCode == "C") {
                this.myMainEntityObjectTableName = "Master";
            }

            else {
                this.myMainEntityObjectTableName = "Shipment";
            }

            this.EntityPM.MainEntityId = shipmentPM.Id;
            this.EntityPM.MainEntityReference = shipmentPM.ShipmentNumber;
            this.EntityPM.HouseNumber = shipmentPM.House;
            this.EntityPM.MasterNumber = shipmentPM.LongMaster;
            this.EntityPM.ProfitCurrencyId = shipmentPM.ProfitCurrencyId;
            this.EntityPM.BranchId = shipmentPM.BranchId;

            var myDescription = null;
            switch (shipmentPM.DirectionId) {
                case "E": { myDescription = "Export to " + shipmentPM.MainCarriageFinalDestinationPortCode; break; }
                case "I": { myDescription = "Import from " + shipmentPM.MainCarriageFromPortCode; break; }
                case "D": { myDescription = "Ship to " + shipmentPM.ToPartnerCity; break; }
            }

            this.EntityPM.Description = myDescription;
            this.EntityPM.OperationalDate = InvoiceTool.GetOperationalDate(shipmentPM);
            this.EntityPM.ShipmentsNumbers = this.shipmentPM.ShipmentNumber;
            this.EntityPM.ShipmentConcurrencyGUID = this.shipmentPM.ConcurrencyGUID;
            this.EntityPM.ShipmentNewConcurrencyGUID = this.shipmentPM.NewConcurrencyGUID;
        }
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
        var isEnabled: boolean = false;

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
    get VendorDependencyProperty1() { return InvoiceTool.GetVendorPartnerTypes(); }
    public GLAccountId: string;
    public BillToId: string;
    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;
            this.InvoiceCurrencyId = null;
            this.CheckDuplication();

            if (AppTool.IsNullOrEmpty(value)) {
                this.VATNumber = null;
                this.VendorName = null;
                this.GLAccountId = null;
                this.BillToId =null;
                this.EntityPM.VendorPartnerTypeId = null;
                this.SetInvoiceCurrencyFromTenant();
                this.SetPaymentTermFromTenant();
                this.VatTypeId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list != null) {
                            this.VATNumber = list.VatNumber;
                            this.GLAccountId = list.GLAccountId;
                            this.BillToId = list.BillToId;
                            this.VendorName = list.LocalName || list.EnglishName;
                            this.EntityPM.VendorName = list.LocalName || list.EnglishName;
                            this.EntityPM.VendorLocalName = list.LocalName;
                            this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
                            this.VatTypeId = list.VatTypeId;
                            
                            if(this.IsFullAccounting){
                                
                                this.GetConnectedGLAccount();
                                if(AppTool.IsNullOrEmpty(this.InvoiceCurrencyId))
                                this.SetInvoiceCurrencyFromTenant();
                            }
                        
                            else {
                                this.GetConnectedBillTo();
                                if(AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)){
                                    this.SetInvoiceCurrencyFromPartner(list.InvoiceCurrencyId);
                                }
                            }
                            
                            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);

                            this.SetPaymentTermFromPartner(list.PaymentTermId);
                        }
                    }
                });
            }
        }
    }

    BillTo: CardList;

    GetConnectedBillTo() {
        if (this.BillToId)
        {

            this.CurrentSession.StartBusyIndicatorLoading();
            this.myCardListService.getSingle(this.BillToId).subscribe((myResult:any) => {
                var myResponse: ServiceResponse = myResult;
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var cardList: CardList = myResponse.Result;
                    this.BillTo = cardList;
                    if (!AppTool.IsNullOrEmpty(cardList.InvoiceCurrencyId)) {
                        this.InvoiceCurrencyId = cardList.InvoiceCurrencyId;
                        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
                    }

                }
            });
        }else {
            
            this.BillTo = null;
            
        }
    }
    vendorGLAccount: GLAccountList;
    _GLAccountListService: GLAccountListService = new GLAccountListService();
    GetConnectedGLAccount() {
        if (this.GLAccountId)
        {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._GLAccountListService.getSingle(this.GLAccountId).subscribe((myResult:any) => {
                console.log("[_GLAccountListService.getSingle]", myResult);
                this.CurrentSession.StopBusyIndicator();

                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var gla: GLAccountList = myResponse.Result;
                    this.vendorGLAccount = gla;
                    this.EntityPM.VendorGLAccountId = gla.Id;
                    if (!gla.IsMultiCurrency) {
                        this.InvoiceCurrencyId = gla.CurrencyId;
                        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
                    }
                }
            });
        }else{
            this.vendorGLAccount = null;
              this.EntityPM.VendorGLAccountId = null;
        }
    }

    private SetInvoiceCurrencyFromPartner(currencyId: string) {
        if (!AppTool.IsNullOrEmpty(currencyId)) {
            var currency: CurrencyList = this.AllCurrencies.filter(f => f.Id == currencyId)[0];
            if (currency != null && !currency.InActive)
                this.InvoiceCurrencyId = currencyId;
            else
                this.SetInvoiceCurrencyFromTenant();
        }
        else {
            this.SetInvoiceCurrencyFromTenant();
        }
    }
    private SetPaymentTermFromPartner(paymentTermId: string) {
        if (!AppTool.IsNullOrEmpty(paymentTermId)) {
            var paymentTerm: PaymentTermList = this.AllPaymentTerms.filter(f => f.Id == paymentTermId)[0];
            if (paymentTerm != null && !paymentTerm.InActive)
                this.PaymentTermId = paymentTermId;
            else
                this.SetPaymentTermFromTenant();
        }
        else {
            this.SetPaymentTermFromTenant();
        }
    }

    private SetInvoiceCurrencyFromTenant() {
        var currency: CurrencyList = this.AllCurrencies.filter(f => f.Id == SessionLocator.TenantPM.CurrencyId)[0];
        if (currency != null && !currency.InActive)
            this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
    }
    private SetPaymentTermFromTenant() {
        var paymentTerm: PaymentTermList = this.AllPaymentTerms.filter(f => f.Id == SessionLocator.TenantPM.PaymentTermId)[0];
        if (paymentTerm != null && !paymentTerm.InActive)
            this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
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
            //this.CheckDuplication();
        }
    }

    OnInvoiceNumberLostFocus(input: string) {
        if (!AppTool.IsNullOrEmpty(input)) {
            this.CheckDuplication();
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

            if (!this.IsFullAccounting) {
                InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            }
            else {
                InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
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
            if (!this.IsFullAccounting) {
                InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            }
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
            if (this.IsFullAccounting) {
                InvoiceTool.ComputeFullAccountingAPInvoiceDueDate(this.EntityPM);
                this.LoadData();
            }
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

    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate( "APInvoice.F.VendorId")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceNumber")));
        }

        if (this.EntityPM.InvoiceExpectedAmount == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceCurrencyId")));
        }

        if (this.EntityPM.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoice.F.InvoiceDate")));
        }

        else if (DateTool.GetDateParts(this.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
            errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
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
            if (this.IsAccountingActivated == true) {
                var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
                invoiceDomainService.ValidateAPInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.VendorId, this.EntityPM.AccountingDate).subscribe((response: ServiceResponse) => {
                    if (response != null) {
                        if (!response.HasError) {
                            this.CompleteSubmission(errors);
                        }
                        else {
                            this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
            else {
                this.CompleteSubmission(errors);
            }
        }
    }
    CompleteSubmission(errors) {
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

            this.InitializeProfitCurrency();

            if (this.EntityPM.IsMultipleEntities) {
                this.ComputeTotals();
                this.CurrentSession.CloseCurrentWindowEmit("Ok");
            }

            else {
                this.BuildOpenAmounts();
            }
        }
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

    // BuildInvoiceLines

    private BuildOpenAmounts() {
        var filteredPayables: ShipmentPayablePM[] = this.shipmentPM.ShipmentPayables;

        filteredPayables = filteredPayables.filter(d => d.ShipmentPayableParentId == null && (d.ShipmentPayableLineStatusCode == "OAMT" || d.ShipmentPayableLineStatusCode == "PACC"));
        this.BuildInvoiceLines(filteredPayables);
    }
    private BuildInvoiceLines(filteredPayables: ShipmentPayablePM[]) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            filteredPayables.forEach(payable => {

                var invoiceLine: APInvoiceLinePM = new APInvoiceLinePM(this.EntityPM);
                invoiceLine.APInvoiceId = this.EntityPM.Id;
                invoiceLine.Tenant = payable.Tenant;
                invoiceLine.ChargesTypeId = payable.ChargesTypeId;
                invoiceLine.ChargesTypeCode = payable.ChargesTypeCode;
                invoiceLine.ChargesTypeName = payable.ChargesTypeName;
                invoiceLine.EntityPayableId = payable.Id;
                invoiceLine.EntityId = payable.ShipmentId;
                invoiceLine.EntityReference = payable.ShipmentNumber;
                invoiceLine.VendorId = payable.VendorId;
                invoiceLine.VendorName = payable.VendorName;
                invoiceLine.ExpectedAmount = payable.ExpectedAmount;
                invoiceLine.OtherInvoicesAmounts = payable.AccountedAmount;
                invoiceLine.OpenAmount = payable.OpenAmount;
                invoiceLine.CorrectionAmount = payable.CorrectionAmount;
                invoiceLine.CorrectionNote = payable.CorrectionNote;
                invoiceLine.CorrectionByUserId = payable.CorrectionByUserId;
                invoiceLine.CorrectionDate = payable.CorrectionDate;
                invoiceLine.AmountTypeCode = payable.ShipmentPayableAmountTypeCode;
                invoiceLine.ForiegnCurrencyId = payable.CurrencyId;
                invoiceLine.ForiegnCurrencyCode = payable.CurrencyCode;
                invoiceLine.Notes = payable.Notes;
                invoiceLine.PrepaidCollectId = payable.PrepaidCollectId;

                if (invoiceLine.ForiegnCurrencyId == this.EntityPM.InvoiceCurrencyId) {
                    invoiceLine.ForiegnExchangeRate = this.EntityPM.InvoiceCurrencyExchangeRate;
                }

                else {
                    invoiceLine.ForiegnExchangeRate = this.GetCurrencyRate(payable.CurrencyId);
                }

                this.myChargesTypeListService.getSingleFromCache(invoiceLine.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {

                        var list: ChargesTypeList = myResponse.Result;

                        this.SetInvoiceLineVatType(list, payable, invoiceLine);

                        if (list != null) {
                            invoiceLine.Description = list.EnglishName;
                            invoiceLine.LocalDescription = list.LocalName;
                        }
                    }
                });

                this.EntityPM.InvoiceLines.push(invoiceLine);
            });
        }

        this.ComputeTotals();
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    SetInvoiceLineVatType(chargesType: ChargesTypeList, payable: ShipmentPayablePM, invoiceLine: APInvoiceLinePM) {
        var vatTypeId: string = null;
        var inactiveVatTypeId: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var vendor_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == this.VatTypeId)[0];
            if (vendor_VAT != null && !vendor_VAT.InActive)
                vatTypeId = this.VatTypeId;
            else
                inactiveVatTypeId = true;
        }

        if ((AppTool.IsNullOrEmpty(vatTypeId) || inactiveVatTypeId) && payable.IsFromQuote && !AppTool.IsNullOrEmpty(payable.VatTypeId)) {
            var payable_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == payable.VatTypeId)[0];
            if (payable_VAT != null && !payable_VAT.InActive) {
                vatTypeId = payable.VatTypeId;
                inactiveVatTypeId = false
            }
            else
                inactiveVatTypeId = true;
        }

        if ((AppTool.IsNullOrEmpty(vatTypeId) || inactiveVatTypeId) && chargesType != null) {
            var chargesType_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == chargesType.VatTypeId)[0];
            if (chargesType_VAT != null && !chargesType_VAT.InActive) {
                vatTypeId = chargesType.VatTypeId;
                inactiveVatTypeId = false
            }
            else
                inactiveVatTypeId = true;
        }

        invoiceLine.VatTypeId = vatTypeId;

        if (!AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
            var list_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == invoiceLine.VatTypeId)[0];
            if (list_VAT) {
                invoiceLine.VatTypeName = list_VAT.EnglishName;
                invoiceLine.VatIsMultiPercentage = list_VAT.IsMultiPercentage;

                if (!list_VAT.IsMultiPercentage) {
                    invoiceLine.VatPercentage = this.GetVatTypePercentage(invoiceLine.VatTypeId);
                }
            }
        }
    }

    //  ComputeTotals
    ComputeTotals() {
        this.ComputeAmounts();
        this.BuildTotalVATs();

        this.SubTotalInLocalCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.EntityPM.AmountInLocalCurrency_Summary = AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.EntityPM.AmountInInvoiceCurrency_Summary = AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
    }
    ComputeAmounts() {
        // Local
        if (SessionLocator.LocalCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.EntityPM.AmountInLocalCurrency = this.AmountInInvoiceCurrency;
        }
        else {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(this.AmountInInvoiceCurrency * this.InvoiceCurrencyExchangeRate, 2);
        }

        // Profit
        if (this.EntityPM.ProfitCurrencyId == this.InvoiceCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
        }
        else if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }
        else {
            this.EntityPM.AmountInProfitCurrency = AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
        }

        this.EntityPM.AmountDue = this.EntityPM.AmountInInvoiceCurrency == null ? 0 : this.EntityPM.AmountInInvoiceCurrency;
        this.EntityPM.AmountDueInLocalCurrency = this.EntityPM.AmountInLocalCurrency == null ? 0 : this.EntityPM.AmountInLocalCurrency;
        this.EntityPM.AmountDueInProfitCurrency = this.EntityPM.AmountInProfitCurrency == null ? 0 : this.EntityPM.AmountInProfitCurrency;
    }
    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];

        var myDataLines: APInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null);
        if (myDataLines.length > 0) {

            // Build Totals Class
            var group_Source: InvoiceTotalsClass[] = [];
            myDataLines.forEach(item => {
                var lineVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                if (lineVatType) {

                    if (AppTool.IsNullOrEmpty(item.LocalCurrencyAmount)) {
                        item.LocalCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.InvoiceCurrencyAmount)) {
                        item.InvoiceCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.ProfitCurrencyAmount)) {
                        item.ProfitCurrencyAmount = 0;
                    }

                    if (!lineVatType.IsMultiPercentage) {
                        var myQroupItem = new InvoiceTotalsClass();
                        myQroupItem.Id = lineVatType.Id;
                        myQroupItem.VatTypeId = lineVatType.Id;
                        myQroupItem.VatTypePercentage = item.VatPercentage;
                        myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                        myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                        myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                        myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.PayableVATCard;
                        myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                        group_Source.push(myQroupItem);
                    }

                    else {
                        var myVatGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == item.VatTypeId);

                        myVatGroups.forEach(itemGroup => {
                            var myQroupItem = new InvoiceTotalsClass();
                            myQroupItem.Id = itemGroup.SingleVATTypeId;
                            myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.PayableVATCard;

                            var vatType = this.AllVatTypes.filter(f => f.Id == itemGroup.SingleVATTypeId)[0];
                            if (vatType) {
                                myQroupItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                myQroupItem.VatTypePercentage = this.GetVatTypePercentage(vatType.Id);
                            }

                            group_Source.push(myQroupItem);
                        });
                    }
                }
            });

            // Group Totals Class
            var group_data: InvoiceTotalsClass[] = [];
            group_Source.forEach(item => {
                var record: InvoiceTotalsClass = group_data.filter(f => f.VatTypeId == item.VatTypeId && f.VatTypePercentage == item.VatTypePercentage && f.ExternalVatCard == item.ExternalVatCard && f.ExternalTAXItemId == item.ExternalTAXItemId)[0];
                if (record) {
                    record.LocalCurrencyAmount += item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount += item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount += item.ProfitCurrencyAmount;
                }

                else {
                    record = new InvoiceTotalsClass();
                    record.Id = item.Id;
                    record.VatTypeId = item.VatTypeId;
                    record.VatTypePercentage = item.VatTypePercentage;
                    record.ExternalVatCard = item.ExternalVatCard;
                    record.ExternalTAXItemId = item.ExternalTAXItemId;
                    record.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    group_data.push(record);
                }
            });

            // Build Invoice Total VATs
            group_data.forEach(item => {

                var itemVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                var itemTotalVAT = new APInvoiceTotalVATPM(null);
                itemTotalVAT.Tenant = SessionLocator.Tenant;
                itemTotalVAT.APInvoiceId = this.EntityPM.Id;
                itemTotalVAT.VatTypeId = item.VatTypeId;
                itemTotalVAT.VatTypeName = itemVatType.EnglishName;
                itemTotalVAT.ExternalVATCard = item.ExternalVatCard;
                itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                itemTotalVAT.VatPercent = AppTool.Round(item.VatTypePercentage, 2);
                itemTotalVAT.LocalVatableAmount = AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VatPercent + "%)";
                this.EntityPM.AddAPInvoiceTotalVATPM(itemTotalVAT);
            });
        }
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
    set AmountInLocalCurrency(value: number)
    {
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
