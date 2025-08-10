import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../Filters/ReportFliter';
import { QueryFilterItem } from '../../Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ReportsPreviewComponent } from '../../ReportsPreviewComponent';
import { CardExtendedPMService } from '../../../../Common/Services/ExtendedPMs/CardExtendedPMService';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { ChartOfAccountPMService } from '../../../../Accounting/Services/StandardPMs/ChartOfAccountPMService';
import { ChartOfAccountPM } from '../../../../Accounting/EntityPMs/ChartOfAccountPM';
import { ChartOfAccountList } from '../../../../Accounting/EntityLists/ChartOfAccountList';

import { UserPM } from '../../../../Common/EntityPMs/UserPM';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { FullAccountingSettingList } from '../../../../Accounting/EntityLists/FullAccountingSettingList';
import { Operators } from 'Accounting/DataContracts/Operators';
import { GLAccountListService } from 'Accounting/Services/StandardLists/GLAccountListService';
import { GLAccountList } from 'Accounting/EntityLists/GLAccountList';


@Component({

    templateUrl: './NewLedgerTransactionsFilterControl.html',
})

export class NewLedgerTransactionsFilterControl extends BaseComponent implements OnInit {
    public CurrencyFilters: any;
    public List: string;
    public Range: string;
    public GLAccountFilterRadio: string;


    public IsListGLAccounts: boolean = true;
    ObjectTableName: string = "LedgerTransaction";
    public ReportsPreview: ReportsPreviewComponent;
    public RunReportTitle: string = 'Run Report';
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    operatorsList =
        [{ Code: Operators.Equals, EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
        { Code: Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
        { Code: Operators.LargerThan, EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
        { Code: Operators.LessThan, EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
        { Code: Operators.LessThanOrEqual, EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
        { Code: Operators.GreaterThanOrEqual, EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
        ];
    selectedAmountOperator: { Code: string, EnglishName: string, LocalName: string };
    selectedAmountOperatorLocalBalanceInDue: { Code: string, EnglishName: string, LocalName: string };

    public IsSchedulerReport: boolean = false;
    public GLAccountChanged: boolean = false;
    isReady: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = false;
    public filterControlHight: string = "100";
    private CurrentSession = SessionLocator.SelectedSession;
    private entityListService: EntityListService = new EntityListService();
    public IsSalesmanRestricted: boolean = false;
    public SalesmanFilterItems: ApiQueryFilters;
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    public GLAccountFilterItems: ApiQueryFilters;
    private chartOfAccountPMService: ChartOfAccountPMService = new ChartOfAccountPMService();
    private fullAccountingSetting: FullAccountingSettingList = new FullAccountingSettingList();
    private ChartOfAccountSecurityLevel: any;
    private securityLevel: any;
    private glaccountPM: any;
    private loggedUser: UserPM;

    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();

    constructor(private changeDetector: ChangeDetectorRef) {
        super();
        this.initializeComponent();

    }
    //#region Lifecycle

    ngOnInit() {
        this.SetUIProperties();
        this.SetDefaultDates();
    }

    private initializeComponent(): void {
        this.InitFilters();
        this.SetLayoutDirection();
        this.LoadResources();
        this.LoadAccountingSettings();
        this.InitializeSalesmanFeature();
    }

    private SetLayoutDirection(): void {
        const layout = ObjectsLocator.GlobalSetting?.LayoutDirection;
        this.isRTL = layout === 'rtl';
    }
    //#endregion

    private LoadAccountingSettings(): void {
        const tenantId = SessionLocator.Tenant.toString();
        this.entityListService.getSingle(tenantId, 'FullAccountingSetting')
            .then((res: any) => {
                res.subscribe(response => {
                    this.CurrentSession.StopBusyIndicator();
                    if (response?.Result) {
                        this.fullAccountingSetting = response.Result;
                    }
                });
            });
    }
    private LoadResources(): void {
        this.entityResourceService.getEntityResourceByTableName('GLAccount').subscribe(() => {
            this.entityResourceService.getEntityResourceByTableName('LedgerTransaction').subscribe(() => {
                this.isReady = true;
                this.SetRunReportTitle();
            });
        });
    }

    SetRunReportTitle(): void {
        const key = this.IsSchedulerReport ? "AgingReport.O.PreviewReport" : "AgingReport.O.RunReport";
        this.RunReportTitle = TextCodeTranslator.Translate(key);
    }

    InitFilters(): void {
        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters();
        this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "3,4", null, null, "Exclude", false, false, false, "string", false, true);

        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);

        this.GLAccountFilterItems = new ApiQueryFilters();
        this.GLAccountFilterItems.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string", false, true);

        this.List = "List_" + this.CurrentSession.SessionIndex+ this.CurrentSession.GetNewId("List");
        this.Range = "Range_" + this.CurrentSession.SessionIndex +this.CurrentSession.GetNewId("Range");
        this.GLAccountFilterRadio = "GLAccountFilterRadio_" + this.CurrentSession.SessionIndex + this.CurrentSession.GetNewId("GLAccountFilterRadio");

    }

    private InitializeSalesmanFeature(): void {
        const hasSalesmanPermission = FeatureLocator.HasFeaturePermession("LedgerTransaction", "SalesmanLTRP");
        this.loggedUser = SessionLocator.LoggedUserPM;

        if (this.loggedUser?.IsSalesman && hasSalesmanPermission) {
            this.IsSalesmanRestricted = true;
            this.Salesman = this.loggedUser.Id;
            this.GLAccountFilterItems.addAdditionalFilter("ConnectedToSalesmanId", this.loggedUser.Id, null, null, "Equals", true, true, false, "string", false, false);
        }
    }

    private SetDefaultDates(): void {
        if (this.IsSchedulerReport) return;

        const today = new Date();
        const lastMonth = new Date(today);
        lastMonth.setMonth(today.getMonth() - 1);

        this.ToDate = this.ToDate ?? today;
        this.FromDate = this.FromDate ?? lastMonth;
    }

    private SetUIProperties(): void {
        this.UIProperties.SetRequired('FromDate', this.ObjectTableName, !this.FromDate);
        this.UIProperties.SetRequired('ToDate', this.ObjectTableName, !this.ToDate);

        if (this.IsSalesmanRestricted) {
            this.UIProperties.SetRequired('Salesman', 'GLAccount', true);
            this.UIProperties.SetEnabled('Salesman', 'GLAccount', false);
        }

        const isEnabled = !!this.glaccountPM;
        this.UIProperties.SetEnabled('BalanceInLocalCurrency', 'GLAccount', isEnabled);
        this.UIProperties.SetEnabled('LocalBalanceInDue', 'GLAccount', isEnabled);
    }

    ValidateDate(): void {
        const isValidRange = new AdvancedDatePickerResolverComponent()
            .SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate);

        setTimeout(() => {
            if (isValidRange) {
                this.UIProperties.SetValidity('ToDate', this.ObjectTableName, true, '');
                this.UIProperties.SetValidity('FromDate', this.ObjectTableName, true, '');
            } else {
                if (!this.IsOldDate('ToDate')) {
                    this.UIProperties.SetValidity('ToDate', this.ObjectTableName, false,
                        TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                }
                if (!this.IsOldDate('FromDate')) {
                    this.UIProperties.SetValidity('FromDate', this.ObjectTableName, false,
                        TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                }
            }
            this.changeDetector.detectChanges();
        }, 200);
    }
    IsOldDate(fieldName: string): boolean {
        const prop = this.UIProperties.UIPropertyList.find(ui => ui.FieldName === fieldName);
        return prop?.ValidationError === "Date time is too way in the past!" ||
            prop?.ValidationError === "Invalid Date" || false;
    }

    InitializeComponent(reportPreview: any): void {
        this.ReportsPreview = reportPreview;
    }

    public filterDateSelectedValue: string = 'filter_accounting';
    public dateTypeCode: string = '1';
    FilterDateClicked(itemValue: string) {
        if (this.filterDateSelectedValue != itemValue) {
            this.filterDateSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    FilterLines() {
        switch (this.filterDateSelectedValue) {
            case 'filter_accounting':
                this.dateTypeCode = '1';
                break;
            case 'filter_due':
                this.dateTypeCode = '2';
                break;
            case 'filter_reference':
                this.dateTypeCode = '3';
                break;
            default:
                break;
        }
    }

    SetFilterSelectedValue() {
        switch (this.dateTypeCode) {
            case '1':
                this.filterDateSelectedValue = 'filter_accounting';
                break;
            case '2':
                this.filterDateSelectedValue = 'filter_due';
                break;
            case '3':
                this.filterDateSelectedValue = 'filter_reference';
                break;
            default:
                break;
        }
    }

    GetMainCustomerFieldName() {
        return  null;
    }

    IsPartnersChanged(SelectedTab) {
        if (SelectedTab == '2')
            this.GLAccountChanged = false;
        return this.GLAccountChanged;
    }

    GetQueryFilterItems() {
        var queryFilterItems = new Array<QueryFilterItem>();
        var queryFilterItem: QueryFilterItem;

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "FromDate";
        queryFilterItem.FieldDataType = 'Date';
        queryFilterItem.FieldValue = this.FromDate ? this.FromDate : null;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ToDate";
        queryFilterItem.FieldDataType = 'Date';
        queryFilterItem.FieldValue = this.ToDate ? this.ToDate : null;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "SalesmanUserId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.salesman);
        queryFilterItem.Operator = "Equals";
        queryFilterItem.DisplayInList = true; 
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "CollectorId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.collector);
        queryFilterItem.Operator = "Equals";
        queryFilterItem.DisplayInList = true; 
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ChartOfAccountId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.ChartOfAccountId);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ChartOfAccountsTypeCode";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.ChartOfAccountsTypeCode);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "CurrencyId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.CurrencyId);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "IncludeChildAccounts";
        queryFilterItem.FieldValue = this.IncludeChildAccounts ? true : false;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "DateTypeCode";
        queryFilterItem.FieldValue = this.dateTypeCode;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "IncludeRelatedCurrenciesAccount";
        queryFilterItem.FieldValue = this.IncludeRelatedCurrenciesAccount ? true : false;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem)

        queryFilterItem = new QueryFilterItem();
        this.IsReconciled = this.AttachedGLAccountCheckBox ? null : false;
        queryFilterItem.FieldName = "IsReconciled";
        queryFilterItem.Operator = "Equals";
        queryFilterItem.FieldValue = this.IsReconciled;
        queryFilterItems.push(queryFilterItem)

        // category
        var categoryIndex = null;
        var categoryValue = null;
        if (this.SelectedCategory) {
            categoryIndex = this.SelectedCategory.replace(' ', ''); 
            if (categoryIndex)
                categoryValue = this.DataContext[categoryIndex]; 
        }
        queryFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); 
        queryFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));

        queryFilterItems.push(new QueryFilterItem("UseSecurityLevel", this.fullAccountingSetting.IsSecurityLevelActivated));


        if (this.BalanceInLocalCurrency != null) {
            queryFilterItem = new QueryFilterItem();
            queryFilterItem.FieldName = "BalanceInLocalCurrency";
            queryFilterItem.FieldValue = this.BalanceInLocalCurrency;
            if (this.selectedAmountOperator) {
                queryFilterItem.Operator = this.selectedAmountOperator.Code; 
                this.selectedAmountOperator
            }
            queryFilterItems.push(queryFilterItem);
        }
        if (this.LocalBalanceInDue != null) {
            queryFilterItem = new QueryFilterItem();
            queryFilterItem.FieldName = "LocalBalanceInDue";
            queryFilterItem.FieldValue = this.LocalBalanceInDue;
            if (this.selectedAmountOperatorLocalBalanceInDue) {
                queryFilterItem.Operator = this.selectedAmountOperatorLocalBalanceInDue.Code; //ayed
                this.selectedAmountOperatorLocalBalanceInDue
            }
            queryFilterItems.push(queryFilterItem);
        }
        if(this.ListGLAccounts.length > 0) {
            queryFilterItem = new QueryFilterItem();
            queryFilterItem.FieldName = "ListGLAccounts";
            queryFilterItem.FieldValue = this.ListGLAccounts.map(x => x.Id).join(',');
            queryFilterItem.Operator = "InList";
            queryFilterItems.push(queryFilterItem);
        }
        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "FromGLAccountDisplayNumber";
        queryFilterItem.FieldValue = this.FromGLAccount?.DisplayNumber;
        queryFilterItem.FieldValue2 = this.FromGLAccountId ? this.FromGLAccountId : null; 
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ToGLAccountDisplayNumber";
        queryFilterItem.FieldValue = this.ToGLAccount?.DisplayNumber;
        queryFilterItem.FieldValue2 = this.ToGLAccountId ? this.ToGLAccountId : null;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        return queryFilterItems;
    }

    GetLookUpFieldValue(field) {
        if (field) {
            if (field[0]["@nil"] != "true")
                return field;
        }
        return null
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>, isSchedulerReport: boolean = true) { //For Scheduler Report
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(async queryFilterItem => {
               await this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private async  SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FromDate":
                    this.FromDate = queryFilterItem.FieldValue;
                    break;
                case "ToDate":
                    this.ToDate = queryFilterItem.FieldValue;
                    break;
                
                case "ChartOfAccountId":
                    this.ChartOfAccountId = queryFilterItem.FieldValue;
                    break;
                case "CurrencyId":
                    this.CurrencyId = queryFilterItem.FieldValue;
                    break;
                case "IncludeChildAccounts":
                    this.IncludeChildAccounts = queryFilterItem.FieldValue;
                    break;
                case "SearchFields":
                    this.SearchFields = queryFilterItem.FieldValue;
                    break;
                case "DateTypeCode":
                    this.dateTypeCode = queryFilterItem.FieldValue;
                    this.SetFilterSelectedValue();
                    break;
                case "IncludeRelatedCurrenciesAccount":
                    this.IncludeRelatedCurrenciesAccount = queryFilterItem.FieldValue;
                    break;
                case "IsReconciled": {
                    this.IsReconciled = queryFilterItem.FieldValue;
                    this.AttachedGLAccountCheckBox = queryFilterItem.FieldValue == null;
                    break;
                }
                case "SalesmanUserId":
                    this.Salesman = queryFilterItem.FieldValue;
                    break;
                case "CollectorId":
                    this.Collector = queryFilterItem.FieldValue;
                    break;
                case "CollectorId":
                    this.Collector = queryFilterItem.FieldValue;
                    break;
                case "CategoryIndex":
                    this.SelectedItemChanged(this.GetLookUpFieldValue(queryFilterItem.FieldValue));
                    break;
                case "CategoryValue":
                    //CategoryValue
                    break;
                case "ChartOfAccountsTypeCode":
                    this.ChartOfAccountsTypeCode = queryFilterItem.FieldValue;
                    break;
                case "BalanceInLocalCurrency":
                    this.BalanceInLocalCurrency = queryFilterItem.FieldValue;//ayed
                    if (this.operatorsList.filter(x => x.Code == queryFilterItem.Operator).length > 0) {
                        this.selectedAmountOperator = this.operatorsList.filter(x => x.Code == queryFilterItem.Operator)[0];
                    }
                    break;
                case "LocalBalanceInDue":
                    this.LocalBalanceInDue = queryFilterItem.FieldValue;//ayed
                    if (this.operatorsList.filter(x => x.Code == queryFilterItem.Operator).length > 0) {
                        this.selectedAmountOperatorLocalBalanceInDue = this.operatorsList.filter(x => x.Code == queryFilterItem.Operator)[0];
                    }
                    break;
                case "ListGLAccounts":{
                    if (queryFilterItem.FieldValue) {
                        this.IsListGLAccounts = true;
                        this.ListGLAccounts = await this.FetchGLAccountDataFromServer(queryFilterItem.FieldValue);
                    } else {
                        this.ListGLAccounts = [];
                    }
                    break;
                }
                case "FromGLAccountDisplayNumber":
                    this.FromGLAccountId = queryFilterItem.FieldValue2 ? queryFilterItem.FieldValue2 : null;
                    break;
                case "ToGLAccountDisplayNumber":
                    this.ToGLAccountId = queryFilterItem.FieldValue2 ? queryFilterItem.FieldValue2 : null;
                    break;
            
            }
        }
    }
   async FetchGLAccountDataFromServer(ids: string): Promise<any> {
        if (ids) {
            let apiQueryFilters = new ApiQueryFilters(true);
            apiQueryFilters.addAdditionalFilter("Id", ids, null, null, "InListExact", false, false, false, "string", false, true);

            const gLAccountListService = new GLAccountListService();
            return new Promise((resolve, reject) => {
                gLAccountListService.getByFilters(apiQueryFilters).subscribe(
                    (response: ServiceResponse) => {
                        if (!response.HasError) {
                            resolve(response.Result);
                        } else {
                            reject(new Error('Error fetching GL Account data'));
                        }
                    },
                    (error) => reject(error)
                );
            });
        }
        return Promise.resolve(null);
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];

        var isValid: boolean = true;
        isValid = this.CheckIfChartOfAccountAndUserSecurityLevelAreMatched();
        if ( !this.ChartOfAccountId && !this.ChartOfAccountsTypeCode && !this.SelectedCategoryValue && !this.Salesman && this.ListGLAccounts.length < 1 && (!this.FromGLAccountId || !this.ToGLAccountId)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLTransactionReport.O.RequiredFieldsForNew"));
            isValid = false;
        }

        if (!this.FromDate) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLAccounts.O.fromfieldrequired"));
            isValid = false;
        }
        if (!this.ToDate) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLAccounts.O.tofieldrequired"));
            isValid = false;
        }

        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
            isValid = false;
        }

        if (this.ValidationErrorsList.length == 0)
            this.filterControlHight = "100";
        else
            this.filterControlHight = "80";

        return isValid;
    }
    private CheckIfChartOfAccountAndUserSecurityLevelAreMatched() {

        if (this.CheckGLAccountChartOfAccountSecurityLevel()) {

            return this.CheckChartOfAccountSecurityLevel();
        }
        else return false;
    }

    public get SelectedCategoryValue(): string {
        if (this.SelectedCategory) {
            var categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category
            if (categoryIndex)
                return this.DataContext[categoryIndex]; // select the value from the context
        }
        return null;
    }


    PrepareContactList() {
      
    }

    RunButtonClicked() {
        this.SetUIProperties();


        if (this.ValidateSelectedFilters()) {


           

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();

            this.RunReportEvent.emit(myReportFliter);

        }
    }

    //#region Category fields
    IsCategoryDisabled: boolean = false;
    CategoriesList: string[] = [
        'Category 1',
        'Category 2',
        'Category 3',
        'Category 4',
        'Category 5'
    ];
    SelectedCategory: string;
    SelectedItemChanged(item) {
        this.SelectedCategory = item;
    }

    private SetGLAccountChanged(value: string) {
        if (value != undefined)
            this.GLAccountChanged = true;
    }


    private SetChartOfAccountSecurityLevel(ChartOfAccountId: string) {
        this.chartOfAccountPMService.get(ChartOfAccountId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var chartOfAccount: ChartOfAccountPM = response.Result;
                this.securityLevel = chartOfAccount.ChartOfAccountSecurityLevel == undefined ? 0 : chartOfAccount.ChartOfAccountSecurityLevel;
            }
        });
    }
    private CheckGLAccountChartOfAccountSecurityLevel() {
        if (this.GLAccount) {
            return this.CheckSecurityLevel(this.securityLevel);
        } else return true;

    }
    private CheckChartOfAccountSecurityLevel() {
        if (this.ChartOfAccount) {
            return this.CheckSecurityLevel(this.ChartOfAccountSecurityLevel);
        }
        else return true;
    }
    private CheckSecurityLevel(securityLevel: any) {
        if (!this.loggedUser.IsCustomerCare && (securityLevel > this.loggedUser.SecurityLevel)) {

            this.ValidationErrorsList.push(TextCodeTranslator.Translate("ChartOfAccounts.O.SecurityLevelErrorMessage"));
            return false;
        }
        else return true;
    }
    SettingListOrRangGlaccount() {
        this.ListGLAccounts = [];
        this.FromGLAccountId = null;
        this.ToGLAccountId = null;
        this.IsListGLAccounts = !this.IsListGLAccounts;
    }


    get GLAccount() { return this.glaccountPM; }
    set GLAccount(value: any) {
        if (this.glaccountPM != value) {
            this.glaccountPM = value;
            if (this.glaccountPM) {
                if (this.glaccountPM.IsMultiCurrency) {

                    this.CurrencyId = !AppTool.IsNullOrEmpty(this.CurrencyId) && this.IsSchedulerReport ? this.CurrencyId : null;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                } else {
                    this.CurrencyId = !AppTool.IsNullOrEmpty(this.CurrencyId) && this.IsSchedulerReport ? this.CurrencyId : this.glaccountPM.CurrencyId;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                }

                this.SetChartOfAccountSecurityLevel(value.ChartOfAccountsId);
            }

        }

        if (this.glaccountPM) {
            this.UIProperties.SetEnabled("BalanceInLocalCurrency", "GLAccount", true);
            this.UIProperties.SetEnabled("LocalBalanceInDue", "GLAccount", true);

        } else {
            this.UIProperties.SetEnabled("BalanceInLocalCurrency", "GLAccount", false);
            this.UIProperties.SetEnabled("LocalBalanceInDue", "GLAccount", false);

            this.BalanceInLocalCurrency = '';
            this.LocalBalanceInDue = '';

        }
    }
    private fromGLAccount: GLAccountList;
    get FromGLAccount() { return this.fromGLAccount; }
    set FromGLAccount(value: GLAccountList) {
        if (this.fromGLAccount != value) {
            this.fromGLAccount = value;
        }
    } 
    private toGLAccount: GLAccountList;
    get ToGLAccount() { return this.toGLAccount; }
    set ToGLAccount(value: GLAccountList) {
        if (this.toGLAccount != value) {
            this.toGLAccount = value;
        }
    }
    private chartOfAccountId: string;

    get ChartOfAccountId() { return this.chartOfAccountId; }
    set ChartOfAccountId(value: string) {
        if (this.chartOfAccountId != value) {
            this.chartOfAccountId = value;


        }
    }

    private chartOfAccountsTypeCode: string;
    public get ChartOfAccountsTypeCode(): string {
        return this.chartOfAccountsTypeCode;
    }
    public set ChartOfAccountsTypeCode(v: string) {
        this.chartOfAccountsTypeCode = v;
    }

    private chartOfAccount: ChartOfAccountList;
    public get ChartOfAccount() { return this.chartOfAccount; }
    public set ChartOfAccount(value: ChartOfAccountList) {
        if (this.chartOfAccount != value) {
            this.chartOfAccount = value;
            if (value)
                this.ChartOfAccountSecurityLevel = value.ChartOfAccountSecurityLevel == undefined ? 0 : value.ChartOfAccountSecurityLevel;
        }
    }

    private numberOfMonths: number;
    public get NumberOfMonths() { return this.numberOfMonths; }
    public set NumberOfMonths(value: number) {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }



    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string) {
        if (this.collector != value) {
            this.collector = value;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string) {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }


    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string) {
        if (this.category1 != value) {
            this.category1 = value;
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string) {
        if (this.category2 != value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string) {
        if (this.category3 != value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string) {
        if (this.category4 != value) {
            this.category4 = value;
        }
    }

    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string) {
        if (this.category5 != value) {
            this.category5 = value;
        }
    }

    private currenciesDetailed: boolean;
    public get CurrenciesDetailed() { return this.currenciesDetailed; }
    public set CurrenciesDetailed(value: boolean) {
        if (this.currenciesDetailed != value) {
            this.currenciesDetailed = value;
        }
    }
    private agingForDate: Date = null;
    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date) {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string) {
        if (this.customer != value) {
            this.customer = value;

            if (value)
                this.IsCategoryDisabled = true;
            else
                this.IsCategoryDisabled = false;
        }
    }
    private balanceInLocalCurrency: string;
    public get BalanceInLocalCurrency(): string {
        return this.balanceInLocalCurrency;
    }
    public set BalanceInLocalCurrency(v: string) {
        this.balanceInLocalCurrency = v;
    }

    private localBalanceInDue: string;
    public get LocalBalanceInDue(): string {
        return this.localBalanceInDue;
    }
    public set LocalBalanceInDue(v: string) {
        this.localBalanceInDue = v;
    }
    private isReconciled: boolean;
    public get IsReconciled(): boolean {
        return this.isReconciled;
    }
    public set IsReconciled(v: boolean) {
        this.isReconciled = v;
    }


    private includeChildAccounts: boolean;
    public get IncludeChildAccounts(): boolean {
        return this.includeChildAccounts;
    }
    public set IncludeChildAccounts(v: boolean) {
        this.includeChildAccounts = v;
    }

    private includeRelatedCurrenciesAccount: boolean;
    public get IncludeRelatedCurrenciesAccount(): boolean {
        return this.includeRelatedCurrenciesAccount;
    }
    public set IncludeRelatedCurrenciesAccount(v: boolean) {
        this.includeRelatedCurrenciesAccount = v;
    }

    private searchFields: string;
    public get SearchFields(): string {
        return this.searchFields;
    }
    public set SearchFields(v: string) {
        this.searchFields = v;
    }
    amount: number;
    get Amount() { return this.amount; }
    set Amount(value: number) {
        if (this.amount != value) {
            this.amount = value;
        }
    }
    listGLAccounts: any[] = [];
    get ListGLAccounts() { return this.listGLAccounts; }
    set ListGLAccounts(value) {
        if (this.listGLAccounts != value) {
            this.listGLAccounts = value;
        }
    }
    glaccountIdentifier: string;
    get GLAccountIdentifier() { return this.glaccountIdentifier; }
    set GLAccountIdentifier(value: string) {
        if (this.glaccountIdentifier != value) {
            this.glaccountIdentifier = value;
        }
    }
    fromGLAccountId: string;
    get FromGLAccountId() { return this.fromGLAccountId; }
    set FromGLAccountId(value: string) {
        if (this.fromGLAccountId != value) {
            this.fromGLAccountId = value;
        }
    }
    toGLAccountId: string;
    get ToGLAccountId() { return this.toGLAccountId; }
    set ToGLAccountId(value: string) {
        if (this.toGLAccountId != value) {
            this.toGLAccountId = value;
        }
    }
   
    private openAmountHint: string;
    get OpenAmountHint() { return this.openAmountHint; }
    set OpenAmountHint(value: string) {
        if (this.openAmountHint != value) {
            this.openAmountHint = value;
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
            if (!this.IsOldDate("FromDate"))
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !value);
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();
            if (!this.IsOldDate("ToDate"))
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !value);

        }
    }

    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;
        }
    }


    private attachedGLAccountCheckBox: boolean = true;
    get AttachedGLAccountCheckBox() { return this.attachedGLAccountCheckBox; }
    set AttachedGLAccountCheckBox(value: boolean) {
        if (this.attachedGLAccountCheckBox != value) {
            const formday = new Date();
            if (!value)
                this.FromDate = new Date(formday.setFullYear(2000, 0, 1));
            else
                this.FromDate = new Date(formday.setMonth(formday.getMonth() - 1));
            this.attachedGLAccountCheckBox = value;
        }
    }

    private splittedByCurrencyCheckBox: boolean = false;
    get SplittedByCurrencyCheckBox() { return this.splittedByCurrencyCheckBox; }
    set SplittedByCurrencyCheckBox(value: boolean) {
        if (this.splittedByCurrencyCheckBox != value) {
            this.splittedByCurrencyCheckBox = value;

        }
    }


    //#endregion


}
