import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceTotalVATPM} from '../../../../Invoice/EntityPMs/APInvoiceTotalVATPM';
import {AppTool, DateTool, FontTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {SummaryItem, InvoiceTotalsClass} from '../../../../Invoice/Args';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {VATTypesGroupPM} from '../../../../Common/EntityPMs/VATTypesGroupPM';
import {NumbersPipe} from '../../../../Infrastructure/Pipes/NumbersPipe';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {UpdateCurrencyRateComponent} from '../../../../CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {GLAccountPMService} from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {GLAccountPM} from '../../../../Accounting/EntityPMs/GLAccountPM';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountList } from 'Accounting/EntityLists/GLAccountList';
import { GLAccountListService } from 'Accounting/Services/StandardLists/GLAccountListService';
declare var window: any;

@Component({

    templateUrl: './APInvoiceDetailsTabGeneral.html',
})

export class APInvoiceDetailsTabGeneral extends BaseComponent implements OnDestroy {
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public LocalCurrencyCode: string = SessionLocator.LocalCurrencyCode;
    private isBaseDataLoaded = false;
    public todayDate: Date;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    public IsFullAccounting: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    InvoiceLineHeader: string;
    accountingActivated: boolean= false;
    DisplayFieldsFromList:string;
    DisplayLocalFieldsFromList:string;
    VendorLovSizeForFullAccounting: number;
    forceShowLocalAndEnglishColumns = false;
    public AllowVatTypes: boolean = true;
    ColumnsWidths: any[] = [];
    public IsUsingVirtuallization: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.SetIsUsingVirtuallization();
        this.IsFullAccounting = SessionLocator.TenantPM.AccountingActivated;
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);
        this.todayDate = DateTool.GetCurrentDateAsUtc();
        this.isBaseDataLoaded = false;

        this.CheckFeatures();
        this.InitializeServices();
        this.SetUIProperties();
        this.BuildScreenData();
        this.Listen();
        if (SessionLocator.TenantPM.AccountingActivated) {
            this.InvoiceLineHeader = TextCodeTranslator.Translate("APInvoiceLine.F.Description");
            this.accountingActivated = true;
        }
        else {
            this.InvoiceLineHeader = TextCodeTranslator.Translate("APInvoiceLine.O.Name");
         }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        this.InitializeVendorLov();
        
        if (this.EntityPM.StatusCode == null)
        {
            this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
        }
  
    }

    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
        }
    }

    private InitializeVendorLov() {
        if (this.accountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
            this.VendorLovSizeForFullAccounting = 550;
            this.forceShowLocalAndEnglishColumns = true;
            this.FillLOVColumnsWidths();

        }
    }


    FillLOVColumnsWidths()
    {
        this.ColumnsWidths = [
            { ColumnName: 'Code', Width: 100 },
            { ColumnName: 'CalculatedEnglishName', Width: 120 },
            { ColumnName: 'CalculatedLocalName', Width: 120 },
            { ColumnName: 'LocalName', Width: 120 },
            { ColumnName: 'GLAccountDisplayNumber', Width: 120 },
            // { ColumnName: 'CityName', Width: 85 },
            { ColumnName: 'CountryCode', Width: 60 },
            { ColumnName: 'PartnerTypeName', Width: 60 }
        ];
    }

    CheckFeatures() {

        var table = window.ObjectTables.filter(d => d.Name === 'APInvoice')[0];
        var hideVatTypesFeature = FeatureLocator.Features.filter(f => (f.Code == "HideVatTypes") && f.ObjectTableId == table.Id)[0];
        if (hideVatTypesFeature) {
            this.AllowVatTypes = false;
        }
    }


    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildInvoiceLines();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildInvoiceLines();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public AllVatTypes: VatTypeList[] = [];
    private myCommonDomainService: CommonDomainService;
    private myCardListService: CardListService;
    private myPaymentTermListService: PaymentTermListService;
    private myCurrencyListService: CurrencyListService;
    public myVatTypeListService: VatTypeListService;
    private myChargesTypeListService: ChargesTypeListService;
    public myGLAccountPMService: GLAccountPMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCommonDomainService = new CommonDomainService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myGLAccountPMService = new GLAccountPMService();
        this.GetAllVatTypes();
    }
    GetAllVatTypes() {
        this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });
    }

    // SetUIProperties
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AccountingDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, false);

        }

        else {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AccountingDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, true);
            if (this.EntityPM.InvoicePayments.length > 0) {
                this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);

                this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            }

            if (this.EntityPM.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
            }
        }

        this.SetUIProperties_DueDate();
        this.SetUIProperties_VATNumber();
        this.SetUIProperties_ExchangeRate();
    }
    SetUIProperties_DueDate() {
        var AllowManuallyDueDate: boolean = false;

        if (SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }

        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }

        if (!this.IsScreenEnabled) {
            AllowManuallyDueDate = false;
        }

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    }
    SetUIProperties_VATNumber() {
        var isRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(this.VATNumber)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetRequired("VATNumber", this.ObjectTableName, isRequired);
    }

    public RateIsEnabled: boolean = false;
    SetUIProperties_ExchangeRate() {
        var isEnabled: boolean = false;

        if (this.IsScreenEnabled) {
            if (FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                        if (this.EntityPM.InvoicePayments.length == 0) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }

        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isEnabled);
    }


    // Refresh Screen
    private RefreshScreen() {
        this.SetUIProperties();
        this.BuildInvoiceLines();
    }

    get IsScreenEnabled() {
        var result = true;
        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
                result = true;
            }

            else {
                result = false;
            }
        }
        return result;
    }

    get VatTypeFilterButtonIsEnabled() {
        var result = true;
        if (this.EntityPM != null) {
            if (!this.IsScreenEnabled) {
                result = false;
            }

            else if (AppTool.IsNullOrEmpty(this.VatTypeId)) {
                result = false;
            }
        }

        return result;
    }

    // LoadVatsPercentages
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

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {

                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.BuildInvoiceLines();

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    UpdateData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.SetCurrencyRateData();

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.ItemsSource.Collection.forEach(item => {
                        item.SetVatPercentage(this.GetVatTypePercentage(item.VatTypeId));
                    });

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

    BuildScreenData() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA") {
            if (!this.isBaseDataLoaded) {
                this.isBaseDataLoaded = true;
                this.LoadData();
            }
        }
        else {
            this.BuildInvoiceLines();
        }
    }
    BuildInvoiceLines() {

        this.ItemsSource.Clear();
        this.ComputeRelativeRateDate();

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            var defaultConnectedLines: APInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(d => d.VendorId == this.EntityPM.VendorId);
            var otherLines: APInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(d => d.VendorId != this.VendorId);

            defaultConnectedLines.forEach(line => {
                this.ItemsSource.Insert(new APInvoiceLineItem(line, this, false));
            });

            otherLines.forEach(line => {
                this.EntityPM.RemoveAPInvoiceLinePM(line);
                this.ItemsSource.Insert(new APInvoiceLineItem(line, this, false));
            });

            this.ComputeTotals();
        }

        else {
            this.EntityPM.InvoiceLines.forEach(line => {
                this.ItemsSource.Insert(new APInvoiceLineItem(line, this, false));
            });

            this.BuildSummary();
        }
    }

    // Totals
    get SummaryCurrencyButtonsVisibility() {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.InvoiceCurrencyId)) {
            if (SessionLocator.TenantPM.CurrencyId != this.EntityPM.InvoiceCurrencyId) {
                myResult = true;
            }
        }

        return myResult;
    }
    private isTotalInLocalCurrency: boolean = false;

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    get IsTotalInLocalCurrency() { return this.isTotalInLocalCurrency; }
    set IsTotalInLocalCurrency(value: boolean) {
        if (this.isTotalInLocalCurrency != value) {
            this.isTotalInLocalCurrency = value;
            this.BuildSummary();
        }
    }

    public SummaryItems: SummaryItem[] = [];
    ComputeTotals() {
        this.BuildTotalVATs();

        this.SubTotalInLocalCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);

        if (this.EntityPM.TotalVATs.length > 0) {
            this.EntityPM.AmountInLocalCurrency_Summary = AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
            this.EntityPM.AmountInInvoiceCurrency_Summary = AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);
        }

        else {
            this.EntityPM.AmountInLocalCurrency_Summary = 0;
            this.EntityPM.AmountInInvoiceCurrency_Summary = 0;
        }

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


        this.BuildSummary();
    }
    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];
        var pipe = new NumbersPipe();

        var myDataLines: APInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null);
        if (myDataLines.length > 0) {

            this.GetAllVatTypes();

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
                       // myQroupItem.VatRecognizedPercentage = lineVatType.RecognizedPercentage;
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
                              //  myQroupItem.VatRecognizedPercentage = vatType.RecognizedPercentage;
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
                itemTotalVAT.VatPercent = AppTool.Round(item.VatTypePercentage, 3);
                itemTotalVAT.LocalVatableAmount = AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + pipe.transform(itemTotalVAT.VatPercent, "N3") + "%)";
                itemTotalVAT.VatRecognizedPercentage = itemVatType.RecognizedPercentage/100;
                this.EntityPM.AddAPInvoiceTotalVATPM(itemTotalVAT);
            });
        }
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
                //mySummaryItem.Label = item.VatTypeCell;
                mySummaryItem.Label = item.VatTypeName + " (" + pipe.transform(item.VatPercent, "N3") + "%)";
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

    private OnInvoiceDateChangedLoad() {
        this.UpdateData();
    }

    get SubTotalInLocalCurrency() { return this.EntityPM.SubTotalInLocalCurrency; }
    set SubTotalInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.SubTotalInLocalCurrency != setValue) {
            this.EntityPM.SubTotalInLocalCurrency = setValue;
        }
    }

    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.SubTotalInInvoiceCurrency != setValue) {
            this.EntityPM.SubTotalInInvoiceCurrency = setValue;
        }
    }

    get AmountInInvoiceCurrency() { return this.EntityPM.AmountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
            this.EntityPM.AmountInInvoiceCurrency = setValue;
            this.EntityPM.InvoiceExpectedAmount = setValue;

            //// Local
            //if (SessionLocator.LocalCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            //    this.EntityPM.AmountInLocalCurrency = setValue;
            //}
            //else {
            //    this.EntityPM.AmountInLocalCurrency = AppTool.Round(setValue * this.InvoiceCurrencyExchangeRate, 2);
            //}

            //// Profit
            //if (this.EntityPM.ProfitCurrencyId == this.InvoiceCurrencyId) {
            //    this.EntityPM.AmountInProfitCurrency = setValue;
            //}
            //else if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
            //    this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
            //}
            //else {
            //    this.EntityPM.AmountInProfitCurrency = AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
            //}

            //this.EntityPM.AmountDue = this.EntityPM.AmountInInvoiceCurrency == null ? 0 : this.EntityPM.AmountInInvoiceCurrency;
            //this.EntityPM.AmountDueInLocalCurrency = this.EntityPM.AmountInLocalCurrency == null ? 0 : this.EntityPM.AmountInLocalCurrency;
            //this.EntityPM.AmountDueInProfitCurrency = this.EntityPM.AmountInProfitCurrency == null ? 0 : this.EntityPM.AmountInProfitCurrency;
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInLocalCurrency != setValue) {
            this.EntityPM.AmountInLocalCurrency = setValue;
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInProfitCurrency != setValue) {
            this.EntityPM.AmountInProfitCurrency = setValue;
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

    // Properties
    public VendorDependencyProperty1: string = InvoiceTool.GetGeneralAPInvoiceVendorPartnerTypes();

    public glaccount: GLAccountPM = null;
    get VendorId() {
        if (this.EntityPM == null) {
            return null;
        }

        return this.EntityPM.VendorId;
    }
    set VendorId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
                this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                if (!AppTool.IsNullOrEmpty(value)) {
                    this.ItemsSource.Collection.forEach(item => {
                        if (AppTool.IsNullOrEmpty(item.VendorId)) {
                            item.Exists = true;
                        }

                        else {
                            item.Exists = (item.VendorId == this.EntityPM.VendorId);
                        }
                    });

                }

                this.GetCardProperties();
            }
        }
    }
    public GLAccountId: string;
    public BillToId: string;
    GetCardProperties() {
        this.myCardListService.getSingle(this.EntityPM.VendorId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CardList = myResponse.Result;
                if (list == null) {
                    this.GLAccountId = null;
                    this.BillToId =null;
                    this.VATNumber = null;
                    this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                    this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
                    this.glaccount = null;
                    this.EntityPM.VendorGLAccountId = null;
                    this.EntityPM.VendorName = null;
                    this.EntityPM.VendorLocalName = null;
                }

                else {
                    this.GLAccountId = list.GLAccountId;
                    this.BillToId = list.BillToId;
                    this.VATNumber = list.VatNumber;
                    this.EntityPM.VendorName = list.EnglishName;
                    this.EntityPM.VendorLocalName = list.LocalName || list.EnglishName;

                    if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                        this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                    }
                    if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                        this.PaymentTermId = list.PaymentTermId;
                    }

                    if (!AppTool.IsNullOrEmpty(list.VatTypeId)) {

                    }

                    if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                        this.myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.glaccount = myResponse.Result;
                                this.EntityPM.VendorGLAccountId = this.glaccount.Id;
                                this.EntityPM.IsEquipment = this.glaccount.IsEquipmentVendor;
                            }
                        });
                    }
                }
                if(this.IsFullAccounting)
                this.GetConnectedGLAccount();
                else 
                this.GetConnectedBillTo();
                this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);

            }
        });
    }

    GetConnectedBillTo() {
        if (this.BillToId)
        {

            this.CurrentSession.StartBusyIndicatorLoading();
            this.myCardListService.getSingle(this.BillToId).subscribe((myResult:any) => {
                var myResponse: ServiceResponse = myResult;
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var cardList: CardList = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(cardList.InvoiceCurrencyId)) {
                        this.InvoiceCurrencyId = cardList.InvoiceCurrencyId;
                        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
                    }

                }
            });
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

    get VATNumber() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.VATNumber;
    }
    set VATNumber(newValue: string) {
        if (this.EntityPM.VATNumber != newValue) {
            this.EntityPM.VATNumber = newValue;
            this.SetUIProperties_VATNumber();
        }
    }

    get InvoiceNumber() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.InvoiceNumber;
    }
    set InvoiceNumber(newValue: string) {
        if (this.EntityPM.InvoiceNumber != newValue) {
            this.EntityPM.InvoiceNumber = newValue;

        }
    }

    get PaymentTermId() {
        if (this.EntityPM == null) {
            return null;
        }

        return this.EntityPM.PaymentTermId;
    }
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

            InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
        }
    }

    get PaymentTermName() { return this.EntityPM.PaymentTermName; }
    set PaymentTermName(newValue: string) {
        if (this.EntityPM.PaymentTermName != newValue) {
            this.EntityPM.PaymentTermName = newValue;
        }
    }

    get IsEquipment() { return this.EntityPM.IsEquipment; }
    set IsEquipment(newValue: boolean) {
        if (this.EntityPM.IsEquipment != newValue) {
            this.EntityPM.IsEquipment = newValue;
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(newValue: Date) {
        if (this.EntityPM.DueDate != newValue) {
            this.EntityPM.DueDate = newValue;
            InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
        }
    }

    get InvoiceDate() { return this.EntityPM.InvoiceDate; }
    set InvoiceDate(newValue: Date) {
        if (this.EntityPM.InvoiceDate != newValue) {
            this.EntityPM.InvoiceDate = newValue;
            InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            this.OnInvoiceDateChangedLoad();
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
        }
    }

    // Currency
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(value: string) {
        if (this.EntityPM.InvoiceCurrencyId != value) {
            this.EntityPM.InvoiceCurrencyId = value;
            this.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(value);
            this.ExchangeRateDate = this.GetCurrencyRateDate(value);

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

                    this.ItemsSource.Collection.forEach(item => {
                        if (!AppTool.IsNullOrEmpty(value)) {
                            if (AppTool.IsNullOrEmpty(item.VendorId)) {
                                item.Exists = true;
                            }

                            else {
                                item.Exists = (item.VendorId == this.EntityPM.VendorId);
                            }
                        }

                        item.OnInvoiceCurrencyChanged();
                    });

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

            this.ItemsSource.Collection.forEach(item => {
                item.OnInvoiceExchangeRateChanged();
            });
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
    set ProfitCurrencyId(newValue: string) {
        if (this.EntityPM.ProfitCurrencyId != newValue) {
            this.EntityPM.ProfitCurrencyId = newValue;
        }
    }

    get ProfitCurrencyExchangeRate() { return this.EntityPM.ProfitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(newValue: number) {
        if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
            this.EntityPM.ProfitCurrencyExchangeRate = AppTool.Round(newValue, 5);
        }
    }

    // VAT Type Filter
    private vatTypeId = null;
    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(value: string) {
        if (this.vatTypeId != value) {
            this.vatTypeId = value;
        }
    }

    public RunVatTypeFilterMethod() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var vatType = this.VatTypeId;
            this.VatTypeId = null;

            if (this.ItemsSource != null) {
                this.ItemsSource.Collection.forEach(item => {
                    item.VatTypeId = vatType;
                });
            }

            this.ComputeTotals();
        }
    }

    public RefreshLinesVat(vatId: string, vatPercentage: number) {
        if (this.ItemsSource != null) {
            this.ItemsSource.Collection.forEach(item => {
                if (item.VatTypeId == vatId) {
                    item.VatPercentage = vatPercentage;
                }
            });
        }
    }

    AddButtonClicked() {
        var line: APInvoiceLinePM = new APInvoiceLinePM(null);
        line.Tenant = SessionLocator.TenantPM.Id;
        line.APInvoiceId = this.EntityPM.Id;
        line.VendorId = this.VendorId;
        line.EntityId = this.EntityPM.MainEntityId;
        line.EntityReference = this.EntityPM.MainEntityReference;
        line.AmountTypeCode = "NEXP";
        line.ForiegnCurrencyId = this.InvoiceCurrencyId;
        line.ForiegnCurrencyCode = this.InvoiceCurrencyCode;
        line.ForiegnExchangeRate = this.InvoiceCurrencyExchangeRate;

        var myService: CardListService = new CardListService();
        myService.getSingle(this.VendorId).subscribe((myResult:any) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var card: CardList = myResponse.Result;
                if (card != null) {
                    line.VendorName = card.EnglishName;
                }

                var addEditViewModel: APInvoiceLineItem = new APInvoiceLineItem(line, this, true);
                var logWindow = new LogitudeWindow();
                logWindow.DataContext = addEditViewModel;
                var title = TextCodeTranslator.Translate("APInvoiceLine.O.AddInvoiceLine");
                logWindow.Title = title;
                logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditAPGeneralInvoiceLineComponent');
            }
        });
    }
    EditLineClicked(item: APInvoiceLineItem) {
        if (item != null && item.EditControlIsEnabled) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("APInvoiceLine.O.EditInvoiceLine");
            logWindow.DataContext = item;
            logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditAPGeneralInvoiceLineComponent');
        }
    }
}
export class APInvoiceLineItem extends BaseComponent {
    public invoiceLinePM: APInvoiceLinePM = null;
    public ObjectTableName = "APInvoiceLine";
    public DataContext: APInvoiceLineItem = this;
    invoicePM: APInvoicePM;
    public IsScreenEnabled: boolean;
    public CorrectionByUserName = "";
    public LocalCurrencyId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(line: APInvoiceLinePM, public fatherComponent: APInvoiceDetailsTabGeneral, public AddNewLineMode) {
        super();
        this.invoiceLinePM = line;
        this.invoicePM = fatherComponent.EntityPM;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.IsScreenEnabled = fatherComponent.IsScreenEnabled;
        this.SetUIProperties();
        this.GetUserName();
        this.setColors();
        this.ReadVatTypeData();     
    }

    private GetUserName() {
        if (!AppTool.IsNullOrEmpty(this.invoiceLinePM.CorrectionByUserId)) {
            var myService: UserListService = new UserListService();
            myService.getSingleFromCache(this.invoiceLinePM.CorrectionByUserId).subscribe((resp: ServiceResponse) => {
                if (!resp.HasError) {
                    var result: ServiceResponse = resp;
                    var list: UserList = result.Result;
                    if (list != null) {
                        this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    }
    public OnInvoiceCurrencyChanged() {
        // this.GetInvoiceCurrencyCode();
    }

    private ReCalculateTotals() {
        if (this.Exists) {
            this.fatherComponent.ComputeTotals();
        }
    }

    // Currencies
    get ProfitCurrencyId() {
        return this.invoicePM.ProfitCurrencyId;
    }
    get InvoiceCurrencyId() {
        return this.invoicePM.InvoiceCurrencyId;
    }

    get ForiegnCurrencyId() { return this.invoiceLinePM.ForiegnCurrencyId; }
    set ForiegnCurrencyId(value: string) {
        if (this.invoiceLinePM.ForiegnCurrencyId != value) {
            this.invoiceLinePM.ForiegnCurrencyId = value;
        }
    }

    get ForiegnExchangeRate() { return this.invoiceLinePM.ForiegnExchangeRate; }
    set ForiegnExchangeRate(value: number) {
        if (this.invoiceLinePM != null) {
            if (this.invoiceLinePM.ForiegnExchangeRate != value) {
                this.invoiceLinePM.ForiegnExchangeRate = AppTool.Round(value, 5);

                this.ComputeOtherAmounts();
            }
        }
    }

    get ForiegnCurrencyCode() {
        return this.invoiceLinePM.ForiegnCurrencyCode;
    }

    UpdateCurrencyRateClicked() {

        var loadingDate = this.fatherComponent.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.ForiegnCurrencyId, CurrencyCode: this.ForiegnCurrencyCode, Rate: this.ForiegnExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.ForiegnExchangeRate = comp.Rate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    // SetUIProperties
    public IsRateEnabled: boolean = false;
    public IsEditingEnabled: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    private SetUIProperties() {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;

        this.IsEditingEnabled = this.EditControlIsEnabled;

        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, this.OpenAmountIsEnabled);

        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, this.EditControlIsEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, this.EditControlIsEnabled);

        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, true);

        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        }

        else if (!AppTool.IsNullOrEmpty(this.invoiceLinePM.EntityPayableId)) {
            this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        }

        this.SetUIProperties_Rate();
        this.SetUIProperties_OpenAmount();
        this.SetUIProperties_EditControls();
        this.SetUIProperties_VAT();
        this.SetUIProperties_Description();
    }
    SetUIProperties_Rate() {
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId) {
                    if (this.ForiegnCurrencyId != this.LocalCurrencyId) {
                        isFieldEnabled = true;
                    }
                }
            }
        }

        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    }
    private SetUIProperties_EditControls() {
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, true);

        var result = true;

        if (!this.IsScreenEnabled) {
            result = false;
        }

        else if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            result = false;
        }

        if (!result) {
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_OpenAmount() {
        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, true);

        if (this.invoicePM.StatusCode == "VD") {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }

        else if (this.invoiceLinePM.AmountTypeCode == "NEXP") {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }

        else if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, false);
        }
    }
    private SetUIProperties_VAT() {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsScreenEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsScreenEnabled);

        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }

        var isVatPercentageRequired = false;

        if (AppTool.IsNullOrEmpty(this.VatPercentage)) {
            isVatPercentageRequired = true;

            if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (this.VatIsMultiPercentage) {
                    isVatPercentageRequired = false;
                }
            }
        }

        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    }
    SetUIProperties_Description() {
        this.UIProperties.SetRequired("LocalDescription", this.ObjectTableName, AppTool.IsNullOrEmpty(this.LocalDescription));
    }

    // Line Properties
    public RefreshLine() {
        //FirePropertyChanged("Exists");
        //FirePropertyChanged("CheckBoxVisibility");
        //FirePropertyChanged("NotMatchedVisibility");
        //FirePropertyChanged("CellBackground");
        //FirePropertyChanged("AmountCellBackground");
        //FirePropertyChanged("CurrencyCellBackground");
        //FirePropertyChanged("IsScreenEnabled");
        //FirePropertyChanged("VendorCellBackground");

        //FirePropertyChanged("EditControlIsEnabled");
        //FirePropertyChanged("OpenAmountIsEnabled");
        this.setColors();
        this.SetUIProperties();
    }

    get EditControlIsEnabled() {
        var result = true;

        if (!this.IsScreenEnabled) {
            result = false;
        }

        else if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            result = false;
        }

        return result;
    }

    get OpenAmountIsEnabled() {
        var result = true;
        if (this.invoicePM.StatusCode == "VD") {
            result = false;
        }

        else if (this.ExpectedAmount == null) {
            result = false;
        }

        else if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
            result = false;
        }

        return result;
    }

    get Exists() {
        var myResult = false;
        if (this.invoicePM.InvoiceLines.indexOf(this.invoiceLinePM) > -1) {
            myResult = true;
        }
        return myResult;
    }

    set Exists(newValue: boolean) {
        if (newValue == true) {
            this.invoicePM.AddAPInvoiceLinePM(this.invoiceLinePM);
        }

        else {
            this.InvoiceCurrencyAmount = null;
            this.invoicePM.RemoveAPInvoiceLinePM(this.invoiceLinePM);
        }

        this.RefreshLine();
        this.setColors();
        this.fatherComponent.ComputeTotals();
    }

    private setColors() {
        this.ReadCellBackground();
        this.ReadAmountCellBackground();
        this.ReadCurrencyCellBackground();
        this.ReadVendorCellBackground();
    }

    public CellBackgroundColor: string;
    public AmountCellBackground: string;
    public CurrencyCellBackground: string;
    public VendorCellBackground: string;

    ReadCellBackground() {
        var myResult = this.CellReadOnlyBackground;

        if (this.Exists) {
            myResult = "rgba(208, 224, 234, 0.4)";
        }

        this.CellBackgroundColor = myResult;
    }

    public CellReadOnlyBackground = "#E6E7E8";
    public CellReadOnlyForeground = "#6E7172";
    ReadAmountCellBackground() {
        var myResult = "transparent";
        if (!this.IsScreenEnabled) {
            myResult = this.CellReadOnlyBackground;
        }

        else {
            if (this.Exists) {
                myResult = "rgba(208, 224, 234, 0.4)";
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                    myResult = this.CellReadOnlyBackground;
                }

                myResult = "transparent";
            }
        }

        this.AmountCellBackground = myResult;
    }
    ReadCurrencyCellBackground() {
        this.CurrencyCellBackground = "transparent";
    }
    ReadVendorCellBackground() {
        var myResult = "transparent";
        if (this.Exists) {
            myResult = "transparent";
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                myResult = "rgba(255, 171, 3, 0.6)";
            }
        }
        this.VendorCellBackground = myResult;
    }

    get NotMatchedVisibility() {
        var result = false;

        if (!this.Exists) {
            if (!AppTool.IsNullOrEmpty(this.VendorId) && this.VendorId != this.invoicePM.VendorId) {
                result = true;
            }
        }

        return result;
    }

    get CheckBoxVisibility() {
        var result = false;
        if (this.Exists) {
            result = true;
        }

        else {
            if (AppTool.IsNullOrEmpty(this.VendorId)) {
                result = true;
            }

            else if (this.VendorId == this.invoicePM.VendorId) {
                result = true;
            }
        }

        return result;

    }

    // ChargeType
    public chargesTypeList: ChargesTypeList = null;
    public Glaccount: GLAccountPM = null;
    get ChargesTypeId() {
        return this.invoiceLinePM == null ? null : this.invoiceLinePM.ChargesTypeId;
    }
    set ChargesTypeId(value: string) {
        if (this.invoiceLinePM != null) {
            if (this.invoiceLinePM.ChargesTypeId != value) {
                this.invoiceLinePM.ChargesTypeId = value;
            }
            this.OnChargeTypeChanged();
        }
    }

    private OnChargeTypeChanged() {
        if (AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.VatTypeId = null;
            this.Description = null;
            this.LocalDescription = null;
            this.Glaccount = null;
        }

        else {
            var chargesTypeService = new ChargesTypeListService();
            chargesTypeService.getSingle(this.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.chargesTypeList = myResponse.Result;
                    if (this.chargesTypeList  != null) {
                        this.ChargesTypeCode = this.chargesTypeList.Code;
                        this.ChargesTypeName = this.chargesTypeList.EnglishName;
                        this.VatTypeId = this.chargesTypeList.VatTypeId;
                        this.Description = this.chargesTypeList.EnglishName;
                        if (SessionLocator.TenantPM.AccountingActivated) {
                            this.Description = this.chargesTypeList.Description != null ? this.chargesTypeList.Description : this.chargesTypeList.EnglishName;
                        }

                        this.LocalDescription = this.chargesTypeList.LocalName;

                        if (!AppTool.IsNullOrEmpty(this.chargesTypeList.PayableDebitGLAcountId)) {
                            this.fatherComponent.myGLAccountPMService.get(this.chargesTypeList.PayableDebitGLAcountId).subscribe((myResponse: ServiceResponse) => {
                                this.invoiceLinePM.ChargeTypeGLAccountId = this.chargesTypeList.PayableDebitGLAcountId;
                                if (!myResponse.HasError) {
                                    this.Glaccount = myResponse.Result;
                                }
                            });
                        }
                    }
                }
            });
        }
    }

    get ChargesTypeCode() { return this.invoiceLinePM.ChargesTypeCode; }
    set ChargesTypeCode(value: string) {
        if (this.invoiceLinePM.ChargesTypeCode != value) {
            this.invoiceLinePM.ChargesTypeCode = value;
        }
    }

    get ChargesTypeName() { return this.invoiceLinePM.ChargesTypeName; }
    set ChargesTypeName(value: string) {
        if (this.invoiceLinePM.ChargesTypeName != value) {
            this.invoiceLinePM.ChargesTypeName = value;
        }
    }

    get Description() { return this.invoiceLinePM.Description; }
    set Description(value: string) {
        if (this.invoiceLinePM.Description != value) {
            this.invoiceLinePM.Description = value;
        }
    }

    get LocalDescription() { return this.invoiceLinePM.LocalDescription; }
    set LocalDescription(value: string) {
        if (this.invoiceLinePM.LocalDescription != value) {
            this.invoiceLinePM.LocalDescription = value;
            this.SetUIProperties_Description();
        }
    }

    // VAT Type
    get VatTypeId() {
        return this.invoiceLinePM == null ? null : this.invoiceLinePM.VatTypeId;
    }
    set VatTypeId(value: string) {
        if (this.invoiceLinePM != null) {
            if (this.invoiceLinePM.VatTypeId != value) {
                this.invoiceLinePM.VatTypeId = value;
                this.GetVatTypeData();
            }
        }
    }

    GetVatTypeData() {
        if (AppTool.IsNullOrEmpty(this.VatTypeId)) {
            this.VatTypeName = null;
            this.VatPercentage = null;
            this.VatIsMultiPercentage = false;
            //this.invoiceLinePM.ExternalVATCard = null;
            this.invoiceLinePM.ExternalTAXItemId = null;
            this.ReadVatTypeData();
            this.SetUIProperties_VAT();
        }

        else {
            this.fatherComponent.myVatTypeListService.getSingle(this.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: VatTypeList = myResponse.Result;
                    if (list != null) {
                        this.VatTypeName = list.EnglishName;
                        this.VatIsMultiPercentage = list.IsMultiPercentage;
                        //this.invoiceLinePM.ExternalVATCard = list.ExternalVATCard;
                        this.invoiceLinePM.ExternalTAXItemId = list.ExternalTAXItemId;
                        if (list.RecognizedPercentage != null) {
                            this.invoiceLinePM.VatRecognizedPercentage = list.RecognizedPercentage / 100;
                        } else { this.invoiceLinePM.VatRecognizedPercentage =  null;}


                        if (list.IsMultiPercentage) {
                            this.VatPercentage = null;
                        }

                        else {
                            this.VatPercentage = this.fatherComponent.GetVatTypePercentage(this.VatTypeId);
                        }

                        this.ReadVatTypeData();
                        this.SetUIProperties_VAT();
                    }
                }
            });
        }
    }

    SetVatPercentage(myPercentage: number) {
        this.VatPercentage = myPercentage;
    }
    get VatTypeName() { return this.invoiceLinePM.VatTypeName; }
    set VatTypeName(newValue: string) {
        if (this.invoiceLinePM.VatTypeName != newValue) {
            this.invoiceLinePM.VatTypeName = newValue;
        }
    }

    get VatPercentage() { return this.invoiceLinePM.VatPercentage; }
    set VatPercentage(newValue: number) {
        if (this.invoiceLinePM.VatPercentage != newValue) {
            this.invoiceLinePM.VatPercentage = AppTool.Round(newValue, 3);
            this.ReadVatTypeData();
            this.ReCalculateTotals();
            this.SetUIProperties_VAT();
        }
    }

    get VatIsMultiPercentage() { return this.invoiceLinePM.VatIsMultiPercentage; }
    set VatIsMultiPercentage(value: boolean) {
        if (this.invoiceLinePM.VatIsMultiPercentage != value) {
            this.invoiceLinePM.VatIsMultiPercentage = value;
            this.SetUIProperties_VAT();
        }
    }

    public VatTypeCell: string;
    public VatTypeCellColor: string;
    public VatTypeUpdateIsVisible: boolean = false;
    public VatTypeMultiIconVisible: boolean = false;
    public VatTypesGroups: VATTypesGroupPM[] = [];
    ReadVatTypeData() {
        var myValue: string = null;
        var myColor: string = FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];

        var pipe: NumbersPipe = new NumbersPipe();
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == this.VatTypeId);
            }

            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + pipe.transform(this.VatPercentage, "N3") + "%)";
                myColor = FontTool.Black;
            }

            else {
                myValue = TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = FontTool.Red;
                isUpdateVisible = true;
            }
        }

        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    }

    //Amounts
    get ExpectedAmount() {
        return this.invoiceLinePM.ExpectedAmount;
    }

    get OtherInvoicesAmounts() {
        return this.invoiceLinePM.OtherInvoicesAmounts == null ? 0 : this.invoiceLinePM.OtherInvoicesAmounts;
    }

    get CorrectionIconVisibility() {
        var result = false;
        if (this.CorrectionAmount < 0 || this.CorrectionAmount > 0) {
            result = true;
        }
        return result;
    }

    get CorrectionForeground() {
        var myResult = "#282E30";

        if (this.CorrectionAmount != null) {
            if (this.CorrectionAmount > 0) {
                myResult = "Orange";
            }
        }

        return myResult;
    }

    get CorrectionAmount() {
        return this.invoiceLinePM.CorrectionAmount;
    }

    set CorrectionAmount(value: number) {
        if (this.invoiceLinePM.CorrectionAmount != value) {
            this.invoiceLinePM.CorrectionAmount = AppTool.Round(value, 2);
            this.CorrectionByUserId = SessionLocator.LoggedUserId;
            this.CorrectionDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    get CorrectionByUserId() {
        return this.invoiceLinePM.CorrectionByUserId;
    }
    set CorrectionByUserId(value: string) {
        if (this.invoiceLinePM.CorrectionByUserId != value) {
            this.invoiceLinePM.CorrectionByUserId = value;
            this.GetUserName();
        }
    }

    //get CorrectionByUserName() {
    //    return this.connectionUserName;
    //}

    get CorrectionDate() {
        return this.invoiceLinePM.CorrectionDate;
    }

    set CorrectionDate(value: Date) {
        if (this.invoiceLinePM.CorrectionDate != value) {
            this.invoiceLinePM.CorrectionDate = value;
        }
    }

    get CorrectionNote() {
        return this.invoiceLinePM.CorrectionNote;
    }

    set CorrectionNote(value: string) {
        if (this.invoiceLinePM.CorrectionNote != value) {
            this.invoiceLinePM.CorrectionNote = value;
        }
    }

    get InvoiceCurrencyCode() {
        return this.fatherComponent.InvoiceCurrencyCode;
    }
    //set InvoiceCurrencyCode(newValue: string) {
    //    if (this.invoiceLinePM.InvoiceCurrencyCode != newValue) {
    //        this.invoiceLinePM.InvoiceCurrencyCode = newValue;
    //    }
    //}

    get OpenAmount() { return this.invoiceLinePM.OpenAmount; }
    set OpenAmount(value: number) {
        var xValue = value == null ? 0 : value;
        if (this.invoiceLinePM.OpenAmount != value) {
            this.invoiceLinePM.OpenAmount = AppTool.Round(value, 2);

            var expect = this.ExpectedAmount == null ? 0 : this.ExpectedAmount;
            var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre = expect - others - amount - xValue;
            this.CorrectionAmount = corre;
        }
    }

    get InvoiceCurrencyAmount() { return this.invoiceLinePM.InvoiceCurrencyAmount; }
    set InvoiceCurrencyAmount(value: number) {
        if (this.invoiceLinePM.InvoiceCurrencyAmount != value) {
            this.invoiceLinePM.InvoiceCurrencyAmount = AppTool.Round(value, 2);
            this.OnInvoiceCurrencyAmountChanged(value);
        }
    }

    OnInvoiceCurrencyAmountChanged(value: number) {
        var valueInLocal = value * this.invoicePM.InvoiceCurrencyExchangeRate;
        var foriegnAmount = valueInLocal / this.invoiceLinePM.ForiegnExchangeRate;

        this.invoiceLinePM.ForiegnCurrencyAmount = AppTool.Round(foriegnAmount, 2);

        if (!this.AddNewLineMode) {
            this.Exists = foriegnAmount < 0 || foriegnAmount > 0;
        }

        this.ComputeOpenAmount();

        this.LocalCurrencyAmount = valueInLocal;

        if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
            this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
        }

        else {
            this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.invoicePM.ProfitCurrencyExchangeRate;
        }

        this.ReCalculateTotals();
    }

    get ForiegnCurrencyAmount() { return this.invoiceLinePM.ForiegnCurrencyAmount; }
    set ForiegnCurrencyAmount(value: number) {
        if (this.invoiceLinePM.ForiegnCurrencyAmount != value) {
            this.invoiceLinePM.ForiegnCurrencyAmount = AppTool.Round(value, 2);

            this.ComputeOpenAmount();
            this.ComputeOtherAmounts();

            if (!this.AddNewLineMode) {
                this.Exists = value < 0 || value > 0;
            }
        }
    }



    OnInvoiceExchangeRateChanged() {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.ForiegnExchangeRate = this.fatherComponent.InvoiceCurrencyExchangeRate;
        }

        else {
            this.ForiegnExchangeRate = this.fatherComponent.GetCurrencyRate(this.ForiegnCurrencyId);
        }

        this.ComputeOtherAmounts();
    }

    ComputeOtherAmounts() {
        var invoiceAmount = 0;
        this.LocalCurrencyAmount = this.ForiegnCurrencyAmount * this.ForiegnExchangeRate;

        if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
            this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
        }

        else {
            this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.invoicePM.ProfitCurrencyExchangeRate;
        }

        if (this.ForiegnCurrencyId == this.InvoiceCurrencyId) {
            invoiceAmount = this.ForiegnCurrencyAmount;
        }

        else {
            invoiceAmount = this.LocalCurrencyAmount / this.invoicePM.InvoiceCurrencyExchangeRate;
        }

        this.invoiceLinePM.InvoiceCurrencyAmount = AppTool.Round(invoiceAmount, 2);
        this.ReCalculateTotals();
    }

    private ComputeOpenAmount() {
        if (this.invoiceLinePM.AmountTypeCode == "NEXP") {
            this.invoiceLinePM.OpenAmount = null;
        }

        else {
            var expect = this.ExpectedAmount;
            var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre = this.CorrectionAmount == null ? 0 : this.CorrectionAmount;
            var open = expect - others - amount - corre;

            this.invoiceLinePM.OpenAmount = AppTool.Round(open, 2);
        }
    }

    get LocalCurrencyAmount() { return this.invoiceLinePM.LocalCurrencyAmount; }
    set LocalCurrencyAmount(value: number) {
        if (this.invoiceLinePM.LocalCurrencyAmount != value) {
            this.invoiceLinePM.LocalCurrencyAmount = AppTool.Round(value, 2);
        }
    }

    get ProfitCurrencyAmount() { return this.invoiceLinePM.ProfitCurrencyAmount; }
    set ProfitCurrencyAmount(value: number) {
        if (this.invoiceLinePM.ProfitCurrencyAmount != value) {
            this.invoiceLinePM.ProfitCurrencyAmount = AppTool.Round(value, 2);
        }
    }

    // Properties
    get VendorId() {
        return this.invoiceLinePM.VendorId;
    }

    set VendorId(value: string) {
        if (this.invoiceLinePM.VendorId != value) {
            this.invoiceLinePM.VendorId = value;
            this.getVendorCardData();
            this.ReadVendorCellBackground();
        }
    }

    getVendorCardData() {
        if (AppTool.IsNullOrEmpty(this.VendorId)) {
            this.VendorName = null;
        }
        else {
            var myCardListService = new CardListService();
            myCardListService.getSingle(this.VendorId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: CardList = myResponse.Result;
                    if (list == null) {
                        this.VendorName = list.EnglishName;
                    }
                }
            });
        }
    }

    get VendorName() {
        return this.invoiceLinePM.VendorName;
    }

    set VendorName(value: string) {
        if (this.invoiceLinePM.VendorName != value) {
            this.invoiceLinePM.VendorName = value;
        }
    }

    get Notes() {
        return this.invoiceLinePM.Notes;
    }

    set Notes(value: string) {
        if (this.invoiceLinePM.Notes != value) {
            this.invoiceLinePM.Notes = value;
        }
    }

    get NotesIconVisibility() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.invoiceLinePM.Notes)) {
            result = true;
        }
        return result;
    }

    UpdateVatPercentageClicked() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.fatherComponent.ItemsSource.Collection.filter(f => f.VatTypeId == this.VatTypeId).forEach(item => {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    }

}

