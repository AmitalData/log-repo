import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../Filters/ReportFliter';
import { QueryFilterItem } from '../../Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { GLAccountListService } from '../../../../Accounting/Services/StandardLists/GLAccountListService';
import { ChartOfAccountListService } from '../../../../Accounting/Services/StandardLists/ChartOfAccountListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Operators } from 'Accounting/DataContracts/Operators';

@Component({

    templateUrl: './NewAgingFilterComponent.html',
})

export class NewAgingFilterComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    IsSalesmanRestricted: boolean = false;
    public SalesmanFilterItems: ApiQueryFilters;
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    gLAccountListService: GLAccountListService = new GLAccountListService();
    chartOfAccountListService: ChartOfAccountListService = new ChartOfAccountListService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public Name: string;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private loggedUser: any;
    public RunReportTitle: string = 'Run Report';
    public IsSchedulerReport: boolean = false;
    IsGroupMultiAccounts: boolean = false; 

    constructor(public entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;

        this.GetResources();

        this.GetSalesmanFeature();

        this.InitComponent();

    }

    private InitComponent() {
        this.InitSalesmanFilters();

    }

    private InitSalesmanFilters() {
        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);

    }

    private GetResources() {
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { this.isReady = true; });
    }

    GetSalesmanFeature() {
        var salesmanAging = FeatureLocator.HasFeaturePermession("GLAccount", "SalesmanAging");
        var isSalesmanRestrictionsEnabled = !!salesmanAging;
        this.loggedUser = SessionLocator.LoggedUserPM;
        if (this.loggedUser.IsSalesman && isSalesmanRestrictionsEnabled) {
            this.IsSalesmanRestricted = true;
            this.Salesman = this.loggedUser.Id;
        }
    }




    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("AgingForDate", "GLAccount", AppTool.IsNullOrEmpty(this.AgingForDate));

        if (this.IsSalesmanRestricted) {
            this.UIProperties.SetRequired("Salesman", "GLAccount", true);
            this.UIProperties.SetEnabled("Salesman", "GLAccount", false);
        }
        else {
            if (this.customerOrVendorFilterSelected == "filter_vendor") {
                this.UIProperties.SetEnabled("Salesman", "GLAccount", false);
                this.UIProperties.SetEnabled("Collector", "GLAccount", false);
            } else {
                this.UIProperties.SetEnabled("Salesman", "GLAccount", true);
                this.UIProperties.SetEnabled("Collector", "GLAccount", true);
            }
        }

        this.UIProperties.SetEnabled("ChartOfAccountsId_Dummy", "GLAccount", !this.Customer);


    }


    private securityLevel: any;
    SetGLAccountChartOfAccountSecurityLevel() {
        this.gLAccountListService.getSingle(this.Customer).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.chartOfAccountListService.getSingle(response.Result.ChartOfAccountsId).subscribe((response: ServiceResponse) => {
                    if (!response.HasError) {
                        var chartOfAccount = response.Result;
                        this.securityLevel = chartOfAccount.ChartOfAccountSecurityLevel;
                    }
                });
            }
        });
    }

    private _ChartOfAccountsTypeCode: string = "3";
    public get ChartOfAccountsTypeCode(): string {
        return this._ChartOfAccountsTypeCode;
    }
    public set ChartOfAccountsTypeCode(v: string) {
        this._ChartOfAccountsTypeCode = v;
        this.ChartOfAccountsId_Dummy = null;
    }
    private ChartOfAccountSecurityLevel: any;
    private chartOfAccount: any;
    public get ChartOfAccount() { return this.chartOfAccount; }
    public set ChartOfAccount(value: any) {
        if (this.chartOfAccount != value) {
            this.chartOfAccount = value;
            this.ChartOfAccountSecurityLevel = value ? value.ChartOfAccountSecurityLevel : null;
        }
    }

    private _ChartOfAccountsId: string = null;
    public get ChartOfAccountsId_Dummy(): string {
        return this._ChartOfAccountsId;
    }
    public set ChartOfAccountsId_Dummy(v: string) {
        this._ChartOfAccountsId = v;
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

    private balance: number;
    public get Balance() { return this.balance; }
    public set Balance(value: number) {
        if (this.balance != value) {
            this.balance = value;
        }
    }

    private fromBalance: number = null;
    public get FromBalance() { return this.fromBalance; }
    public set FromBalance(value: number) {
        if (this.fromBalance != value) {
            this.fromBalance = value;
        }
    }

    private toBalance: number = null;
    public get ToBalance() { return this.toBalance; }
    public set ToBalance(value: number) {
        if (this.toBalance != value) {
            this.toBalance = value;
        }
    }

    private currencyId: string = null;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;


        }
    }

    private agingForDate: Date = new Date();
    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date) {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
        this.UIProperties.SetRequired("AgingForDate", "GLAccount", AppTool.IsNullOrEmpty(this.AgingForDate));

    }
    private obligo: number;
    public get Obligo() { return this.obligo; }
    public set Obligo(newValue: number) { if (this.obligo != newValue) { this.obligo = newValue; } }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string) {
        if (this.customer != value) {
            this.customer = value;

            if (value) {
                this.ChartOfAccountsId_Dummy = null;
                this.IsCategoryDisabled = true;
            }
            else
                this.IsCategoryDisabled = false;

            this.SetUIProperties();
            this.SetGLAccountChartOfAccountSecurityLevel();

        }
    }
    SetRunReportTitle() {
        if (this.isReady) {
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
        }
    }
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>, isSchedulerReport: boolean = true) {
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {



        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "ChartOfAccountsId":
                    this.ChartOfAccountsId_Dummy = queryFilterItem.FieldValue;
                    break;
                case "ChartOfAccountsTypeCode":
                    this.ChartOfAccountsTypeCode = queryFilterItem.FieldValue;
                    break;
                case "ToBalanceFilterValue": {

                    this.balanceFilterSelectedValue == "filter_DebtBetween"
                    this.toBalance = queryFilterItem.FieldValue;
                    break;
                }
                case "FromBalanceFilterValue":
                    {
                        this.balanceFilterSelectedValue == "filter_DebtBetween"
                        this.FromBalance = queryFilterItem.FieldValue;
                        break;
                    }
                case "ToBalance":
                    this.ToBalance = queryFilterItem.FieldValue;
                    break;
                case "BalanceFilterValue":
                    this.balance = queryFilterItem.FieldValue;
                    break;
                case "BalanceFilter":
                    this.balanceFilterSelectedValue = "filter_" + queryFilterItem.FieldValue;
                    break;
                case "CurrencyOriginalLocalValue":
                    this.currencyFilterSelectedValue = queryFilterItem.FieldValue;
                    break;

                case "GroupByDate":
                    this.DateFilterSelectedValue = queryFilterItem.FieldValue;
                    break;
                case "AgingForDate":
                    this.AgingForDate = queryFilterItem.FieldValue;
                    break;
                case "Detailed":
                    this.CurrenciesDetailed = queryFilterItem.FieldValue;
                    break;
                case "SalesmanId":
                    this.Salesman = queryFilterItem.FieldValue;
                    break;
                case "CollectorId":
                    this.Collector = queryFilterItem.FieldValue;
                    break;
                case "GLAccountType": {
                    this.customerOrVendorFilterSelected = queryFilterItem.FieldValue == "3" ? "filter_vendor" : "filter_customer";
                    this.AccountTypeCode = queryFilterItem.FieldValue;
                    break;
                }

                case "CustomerId":
                    this.Customer = queryFilterItem.FieldValue;
                    break;

                case "CategoryIndex": {
                    if (queryFilterItem.FieldValue) {
                        this.SelectedCategory = `Category ${queryFilterItem.FieldValue.slice(-1)}`;
                    }
                    break;
                }
                case "CategoryValue": {
                    this.DataContext[this.SelectedCategory?.replace(' ', '')] = queryFilterItem.FieldValue;
                    break;
                }
                case "Obligo":{
                    this.SelectedObligo =this.operatorsList.find(a=>a.Code === queryFilterItem.FieldValue); ;
                    break;
                }
                case "IsGroupMultiAccounts":
                    this.IsGroupMultiAccounts = queryFilterItem.FieldValue;
                    break;
                


            }



        }
    }

    private errors: string[];
    RunButtonClicked(isInteractive: boolean) {
        this.SetUIProperties();


       
        this.ValidationErrorsList = [];


        this.CheckIfChartOfAccountAndUserSecurityLevelAreMatched();

        if (this.ValidateSelectedFilters()) {

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            myReportFliter.IsInteractive = isInteractive;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = this.errors;
        }
    }
    GetQueryFilterItems() {
        var categoryValue = null;
        var categoryIndex = null;
        if (this.SelectedCategory) {
            categoryIndex = this.SelectedCategory.replace(' ', '');

            if (categoryIndex)
                categoryValue = this.DataContext[categoryIndex];
        }

        var myFilterItems: QueryFilterItem[] = [];
        var queryFilterItem: QueryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "AgingForDate";
        queryFilterItem.FieldDataType = 'Date';
        queryFilterItem.FieldValue = this.AgingForDate ? this.AgingForDate : null;
        queryFilterItem.Operator = "Equals";
        myFilterItems.push(queryFilterItem);

        myFilterItems.push(new QueryFilterItem("GLAccountType", this.safeValue(this.AccountTypeCode)));
        myFilterItems.push(new QueryFilterItem("CustomerId", this.safeValue(this.Customer)));
        myFilterItems.push(new QueryFilterItem("CollectorId", this.safeValue(this.Collector)));
        myFilterItems.push(new QueryFilterItem("CurrencyId", this.safeValue(this.CurrencyId), "string"));
        myFilterItems.push(new QueryFilterItem("Obligo", this.safeValue(this.SelectedObligo && this.SelectedObligo.Code), "string"));
        myFilterItems.push(new QueryFilterItem("SalesmanId", this.safeValue(this.Salesman)));
        myFilterItems.push(new QueryFilterItem("Detailed", this.safeValue(this.CurrenciesDetailed)));
        myFilterItems.push(new QueryFilterItem("CategoryIndex", this.safeValue(categoryIndex)));
        myFilterItems.push(new QueryFilterItem("CategoryValue", this.safeValue(categoryValue)));
        myFilterItems.push(new QueryFilterItem("GroupByDate", this.safeValue(this.DateFilterSelectedValue)));
        myFilterItems.push(new QueryFilterItem("CurrencyOriginalLocalValue", this.safeValue(this.currencyFilterSelectedValue)));
        myFilterItems.push(new QueryFilterItem("BalanceFilter", this.safeValue(this.balanceFilterSelectedValue ? this.balanceFilterSelectedValue.replace("filter_", "") : null)));
        myFilterItems.push(new QueryFilterItem("BalanceFilterValue", this.safeValue(this.balance != null ? this.balance : 0), "decimal"));
        myFilterItems.push(new QueryFilterItem("IsGroupMultiAccounts", this.safeValue(this.IsGroupMultiAccounts)));

        if (this.balanceFilterSelectedValue === "filter_DebtBetween") {
            myFilterItems.push(new QueryFilterItem("FromBalanceFilterValue", this.safeValue(this.fromBalance), "decimal"));
            myFilterItems.push(new QueryFilterItem("ToBalanceFilterValue", this.safeValue(this.toBalance), "decimal"));
        }

        myFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCode", this.safeValue(this.ChartOfAccountsTypeCode)));
        myFilterItems.push(new QueryFilterItem("ChartOfAccountsId", this.safeValue(this.ChartOfAccountsId_Dummy)));
        myFilterItems.push(new QueryFilterItem("SortField", this.safeValue(this.SelectedSortTypeItem && this.SelectedSortTypeItem.Code)));
        myFilterItems.push(new QueryFilterItem("SortDirection", this.safeValue(this.SelectedSortDirectionCode)));

        return myFilterItems

    }
    ValidateSelectedFilters() {
        this.errors = [];
        if (!this.AgingForDate) { this.errors.push("Aging for date field is requierd"); }

        this.ValidationErrorsList = this.errors;

        return this.errors.length == 0;

    }
    ValidateDate() {
        if (this.AgingForDate) {

            this.UIProperties.SetValidity("AgingForDate", "GLAccount", true, "valid");
            this.UIProperties.SetRequired("AgingForDate", "GLAccount", false);
            return true;

        }
        return true;
    }

    private CheckGLAccountChartOfAccountSecurityLevel() {
        if (this.Customer) {
            return this.CheckSecurityLevel(this.securityLevel);
        } else return true;

    }
    private CheckChartOfAccountSecurityLevel() {
        if (this.ChartOfAccount) {
            return this.CheckSecurityLevel(this.ChartOfAccountSecurityLevel);
        }
        else return true;
    }
    private CheckIfChartOfAccountAndUserSecurityLevelAreMatched() {

        if (this.CheckGLAccountChartOfAccountSecurityLevel()) {

            return this.CheckChartOfAccountSecurityLevel();
        }
        else return false;
    }
    private CheckSecurityLevel(securityLevel: any) {
        if (!this.loggedUser.IsCustomerCare) {
            if (securityLevel == undefined) {
                securityLevel = 0;
            }
            if (!this.loggedUser.IsCustomerCare && securityLevel > this.loggedUser.SecurityLevel) {
                this.errors.push(TextCodeTranslator.Translate("ChartOfAccounts.O.SecurityLevelErrorMessage"));
                return false;
            }
            else return true;

        }
        else return true;
    }


    safeValue(value: any): string | number | boolean | null {
        if (value === undefined || value === null) {
            return null;
        }
        if (typeof value === "object") {
            if ("Id" in value) return value.Id;
            if ("Code" in value) return value.Code;
            return null;
        }
        return value;
    }


    SortTypeChanged(dir) {
        this.SelectedSortTypeCode = dir.Code;
        this.SelectedSortTypeItem = dir;
    }

    SelectedCategory: string;
    SelectedCategoryChanged(item) {
        this.SelectedCategory = item;
    }
    SelectedObligo: { Code: string, EnglishName: string, LocalName: string };
    SelectedObligoChanged(item){
        this.SelectedObligo = item;
    }
    public customerOrVendorFilterSelected: string = 'filter_customer';
    public AccountTypeCode: string = '2';
    FilterCustomerOrVendorClicked(itemValue: string) {
        if (this.customerOrVendorFilterSelected != itemValue) {
            this.customerOrVendorFilterSelected = itemValue;
            this.FilterCustomerOrVendorChanged();
        }
    }
    FilterCustomerOrVendorChanged() {

        this.Customer = null;
        this.Salesman = null;
        this.Collector = null;
        this.ChartOfAccountsId_Dummy = null;
        this.UIProperties.SetValidity("ChartOfAccountsId", "GLAccount", true, "");
        this.UIProperties.SetRequired("ChartOfAccountsId", "GLAccount", false);


        switch (this.customerOrVendorFilterSelected) {
            case 'filter_customer':
                this.AccountTypeCode = '2';
                this.ChartOfAccountsTypeCode = '3';
                break;
            case 'filter_vendor':
                this.AccountTypeCode = '3';
                this.ChartOfAccountsTypeCode = '4';
                break;
            default:
                break;
        }

        this.SetUIProperties();
        this.ValidateDate();

    }
    CurrencyFilterClicked(itemValue: string) {
        if (this.currencyFilterSelectedValue != itemValue) {
            this.currencyFilterSelectedValue = itemValue;
        }
    }


    public DateFilterSelectedValue: string = 'filter_Due';
    DateFilterItemClicked(itemValue: string) {
        if (this.DateFilterSelectedValue != itemValue) {
            this.DateFilterSelectedValue = itemValue;
        }
    }
    public balanceFilterSelectedValue: string = 'filter_Debtors';
    public currencyFilterSelectedValue: string = 'filter_OriginalCurr';
    BalanceFilterItemClicked(itemValue: string) {
        if (this.balanceFilterSelectedValue != itemValue) {
            this.balanceFilterSelectedValue = itemValue;
            this.BalanceFilterChanged();
        }
    }
    BalanceFilterChanged() {

        switch (this.balanceFilterSelectedValue) {
            case 'filter_All':
                //   this.AccountTypeCode = '2';
                break;
            case 'filter_Debtors':
                //   this.AccountTypeCode = '2';
                break;
            case 'filter_DebtBetween':
                this.balance = 0;
                break;
            default:
                break;
        }

        this.SetUIProperties();

    }

    IsCategoryDisabled: boolean = false;
    CategoriesList: string[] = [
        'Category 1',
        'Category 2',
        'Category 3',
        'Category 4',
        'Category 5'
    ];
    public operatorsList =
        [
            { Code: Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") + " 0" },
            { Code: Operators.LargerThan, EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") + " 0" },
            { Code: Operators.LessThan, EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") + " 0" }
        ];
    SelectedSortTypeItem = { Code: "balance", EnglishName: "Balance", LocalName: "יתרה בעו''ש" };
    SelectedSortTypeCode: string = "balance";
    SelectedSortDirectionCode: string = "Ascending";

    SortTypes = [
        { Code: "balance", EnglishName: "Balance", LocalName: "יתרה בעו''ש" },
        { Code: "customer", EnglishName: "Customer", LocalName: "לקוח" },
        { Code: "TotalToCollect", EnglishName: "Total To Collect", LocalName: "סה''כ לגביה" },
        { Code: "Obligo", EnglishName: "Obligo", LocalName: "אובליגו" },
        { Code: "CreditUsed", EnglishName: "Credit Used", LocalName: "ע/ח מהמסגרת" },
    ];



}