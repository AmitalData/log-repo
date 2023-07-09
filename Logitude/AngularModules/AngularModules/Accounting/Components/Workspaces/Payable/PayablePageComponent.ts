declare var window: any;
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {GLAccountSummary} from '../../../DataContracts/AccountingSummery';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {APPaymentPM} from '../../../../Invoice/EntityPMs/APPaymentPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {ModulesService} from '../../../Services/ModulesService';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';

@Component({

    templateUrl: './PayablePageComponent.html',
})

export class PayablePageComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    public TenantPM: TenantPM;

    glAccountSummary: GLAccountSummary = new GLAccountSummary();

    // Queries
    collectorsGLAVisibility: boolean = false;
    debetorsGLAVisibility: boolean = false;
    activeVendorsGLAVisibility: boolean = false;
    inactiveVendorsGlaVisibility: boolean = false;
    CLIENTGLACCOUNTSGlaVisibility: boolean = false;

    //Counts
    public APPaymentsDraftsCount: string;
    public APPaymentsDraftPermission: boolean;
    public APPaymentsOpenedCount: string;
    public APInvoicesDraftsCount: string;
    public APInvoicesUnpaidCount: string;

    RecentGLAccountsCount: number = 0;

    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private LastRatesList: LastRate[] = [];
    private myCurrencyListService: CurrencyListService;

    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.myCurrencyListService = new CurrencyListService();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("APPayment").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("APInvoice").subscribe((response: any) => { });

        this.TenantPM = SessionLocator.TenantPM;

        this.LoadAllScreenData();

    }
    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    LoadAllScreenData() {
        this.LoadRecentGLAccounts();
        this.LoadQueriesCounts();
    }

    SetQueriesVisibility() {
        //this.collectorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "collectorsGLA") ? true : false;
        //this.debetorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "debetorsGLA") ? true : false;
        this.activeVendorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "activeVendorsGLA") ? true : false;
        this.inactiveVendorsGlaVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "inactiveVendorsGla") ? true : false;
        this.CLIENTGLACCOUNTSGlaVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "VENDORGLACCOUNTS") ? true : false;
    }

    // Vendors
    RunNewVendorWizard() {
        //var windowTitle = "New Vendor";

        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 700;
        //logWindow.Height = 500;
        //logWindow.Title = windowTitle;
        ////logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        //logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    }
    ViewVendorQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "Vendor Accounts";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                // MyVendorsAsCollectors
                // DebetorsVendors
                // ActiveVendorsGLAccounts
                // InactiveVendorsGLAccount
                //case "MyVendorsAsCollectors":
                //    {
                //        displayTitle = "My Vendors (As Collectors)";

                //        break;
                //    }

                //case "DebetorsVendors":
                //    {
                //        displayTitle = "Debtors Vendors";

                //        break;
                //    }
                case "ActiveVendorsGLAccounts":
                    {
                        displayTitle = "Active Vendors";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.ActiveVendors");

                        break;
                    }
                case "InactiveVendorsGLAccount":
                    {
                        displayTitle = "Inactive Vendors";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.InactiveVendors");

                        break;
                    }
                case "All Vendors":
                    {
                        displayTitle = "All Vendors";
                        myQueryCode = "Vendor Accounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllVendors");

                        break;
                    }



                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Payables");
            listArgs.Perspective = "GLAccountPayables";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    // APPayments
    NewAPPaymentMethod() {
        var newApPaymentPM: APPaymentPM = new APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;

        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, ObjectTableName: 'APPayment' });
                cmpRef.instance.BackCompleted.subscribe(($event1: any) => {
                    this.LoadAllScreenData();
                });

            });

    }
    filterAgrs: ApiQueryFilters;
    ViewInvoiceQuery(args: string) {
        if (args != null) {

            var displayTitle = "";
            var queryCode = args;
            queryCode = "APPayment";
            var filters = new ApiQueryFilters();
            switch (args) {

                case "Draft Payments":
                    {

                        displayTitle = TextCodeTranslator.Translate("APPayment.Q.DraftAPPayments");

                        break;
                    }
                case "Open Payments":
                    {

                        displayTitle = TextCodeTranslator.Translate("APPayment.Q.OpenAPPayments");

                        break;
                    }
                case "All Payments":
                    {


                        displayTitle = TextCodeTranslator.Translate("APPayment.Q.AllAPPayments");

                        break;
                    }



                default: { break; }
            }

            var backButtonTitle = "Accounting";
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];

            this.filterAgrs = new ApiQueryFilters();

            var ObjectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
            var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];

            if (window.PreDefinedFilters.filter(d => d.queryCode == query.Code) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.queryCode == query.Code);

                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
                });
            }

            this.filterAgrs.ObjectTableName = query.ObjectTableName;

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Payables");
            listArgs.DisplayTitle = displayTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }


    public RecentGLAccountsList: GLAccountList[];
    LoadRecentGLAccounts() {
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;

        this._GLAccountExtendedListService.GetRecentGLAccounts("3").subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentGLAccountsList = myResult;
                    this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    }

    LoadQueriesCounts() {
        var myService = new ModulesService();
        myService.GetAccountPayablesSummary().subscribe((myResult:any) => {
            if (myResult != null) {
                this.APInvoicesDraftsCount = myResult.APInvoicesDraftsCount > 1000 ? "1000+" : myResult.APInvoicesDraftsCount.toString();
                this.APPaymentsDraftsCount = myResult.APPaymentsDraftsCount;
                this.APPaymentsDraftPermission = FeatureLocator.HasFeaturePermession("APPayment", "DRAFTPAYMENTS");
            }
        });
    }

    EditGLAccount(entity: any) {
        if (entity != null) {

            GLAccountSecurityLevelService.CheckLevel(entity.Id).then(hasAccess =>
            {
                if (hasAccess)
                    this.OpenGLAccountEditWindow(entity);
                else
                GLAccountSecurityLevelService.ShowSecurityBockingMessage();
            });



        }
    }

    private OpenGLAccountEditWindow(entity: any)
    {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Payables") });
                cmpRef.instance.BackCompleted.subscribe(($event: any) =>
                {
                    this.RefreshButtonClicked();
                });
            });
    }

    // General Invoice
    NewGeneralAPInvoice() {

        if (SessionLocator.TenantPM.AccountingActivated) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                this.CurrentSession.SessionLocation.viewContainerRef)

                .then(cmpRef => {
                    const entity = new APInvoicePM();
                    this.GetCurrenciesExchangeRateByValueDate(entity);

                    entity.Tenant = SessionLocator.TenantPM.Id;
                    entity.LocalCurrencyId = SessionLocator.LocalCurrencyId;
                    entity.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
                    const additionalFieldsScreenCode = "APInvoice.AdditionalFields";

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: "", EntityPM: entity, ObjectTableName: 'APInvoice' });
                });
            return;
        }

        var str = TextCodeTranslator.Translate("Accounting.General.O.NewGeneralInvoice");
        var windowTitle = str;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = "";
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice' });
                        });
                }
            });
        });
        logWindow.Show("./InvoiceModules/APInvoice/Components/NewEntity/NewGeneralAPInvoiceComponent");
    }

    private GetCurrenciesExchangeRateByValueDate(entity: APInvoicePM){
        var myCurrencyRatesService = new CurrencyRatesService();
        var myCommonDomainService = new CommonDomainService();

        var loadingDate = DateTool.GetCurrentDateAsUtc();

        myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
                this.InitializeProfitCurrency(entity);
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private InitializeProfitCurrency(entityPM: APInvoicePM) {
        debugger;
        if (entityPM.IsMultipleEntities) {
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        else if (AppTool.IsNullOrEmpty(entityPM.ProfitCurrencyId)) {
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        this.myCurrencyListService.getSingleFromCache(entityPM.ProfitCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
                if (list != null) {
                    entityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });

        entityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(entityPM.ProfitCurrencyId);
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
}
