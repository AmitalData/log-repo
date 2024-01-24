import { FeatureLocator } from './../../../../Infrastructure/Utilities/FeatureLocator';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { FullAccountingSettingPM } from '../../../../Accounting/EntityPMs/FullAccountingSettingPM';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { GLAccountList } from '../../../../Accounting/EntityLists/GLAccountList';
import { GLAccountListService } from '../../../../Accounting/Services/StandardLists/GLAccountListService';
import { ChartOfAccountListService } from '../../../../Accounting/Services/StandardLists/ChartOfAccountListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({

    templateUrl: './AgingFilterComponent.html',
})

export class AgingFilterComponent extends BaseComponent implements OnInit
{
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    IsSalesmanRestricted: boolean = true ;
    public SalesmanFilterItems: ApiQueryFilters;
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    gLAccountListService: GLAccountListService = new GLAccountListService();
    chartOfAccountListService: ChartOfAccountListService = new ChartOfAccountListService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    constructor(public entityListService: EntityListService)
    {
        super();
        this.TenantPM = SessionLocator.TenantPM;

        this.GetResources();

        this.GetSalesmanFeature();

        this.InitComponent();

    }

    private InitComponent()
    {
        this.InitFilters();

        this.SetMonthFilterDefaults();

        this.FillAgingMethodList();
    }

    private SetMonthFilterDefaults()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityListService.getSingle(this.TenantPM.Id.toString(), "FullAccountingSetting").then((res: any) => {
            this.CurrentSession.StopBusyIndicator();
            res.subscribe(myResponse => {
                if (myResponse != null) {
                    var res = myResponse.Result;
                    this.FullAccountingSetting = res;
                    this.NumberOfMonths = this.FullAccountingSetting.NumberOfAgingMonths;
                }
            })
        });
    }

    private InitFilters()
    {
        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);

        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters();
        // this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
    }

    private GetResources()
    {
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { this.isReady = true; });
    }
    private loggedUser: any;
    GetSalesmanFeature()
    {
        var salesmanAging = FeatureLocator.HasFeaturePermession("GLAccount", "SalesmanAging");
        var isSalesmanRestrictionsEnabled = !!salesmanAging;
        console.log("[Salesman Aging]", salesmanAging);

        this.loggedUser = SessionLocator.LoggedUserPM;
        if(this.loggedUser.IsSalesman && isSalesmanRestrictionsEnabled){
            this.IsSalesmanRestricted = true;
            this.Salesman = this.loggedUser.Id;
        }
    }


    public AgingMethodsList: CodeNameClass[];
    public Name: string;
    FillAgingMethodList()
    {
        this.Name = this.showLocal ? "LocalName" : "Name";

        this.AgingMethodsList = [];
        this.AgingMethodsList.push(new CodeNameClass("1", "Open Transaction", "תנועות פתוחות"));
        this.AgingMethodsList.push(new CodeNameClass("2", "Total By Month FIFO", "סכומים חודשים לפי FIFO"));
        this.SelectedAgingMethod = this.AgingMethodsList[0];
    }
    ngOnInit()
    {
        this.SetUIProperties();
    }

    SetUIProperties()
    {
        //this.UIProperties.SetRequired("Customer", "GLAccount", true);
        //this.UIProperties.SetRequired("NumberOfMonths", "GLAccount", true);

        if (this.IsSalesmanRestricted)
        {
            this.UIProperties.SetRequired("Salesman", "GLAccount", true);
            this.UIProperties.SetEnabled("Salesman", "GLAccount", false);
        }
        else
        {
            if (this.filterSelectedValue == "filter_vendor") {
                this.UIProperties.SetEnabled("Salesman", "GLAccount", false);
                this.UIProperties.SetEnabled("Collector", "GLAccount", false);
            } else {
                this.UIProperties.SetEnabled("Salesman", "GLAccount", true);
                this.UIProperties.SetEnabled("Collector", "GLAccount", true);
            }
        }

        this.UIProperties.SetEnabled("ChartOfAccountsId_Dummy", "GLAccount", !this.Customer);


    }

    //#region Filters

    //row 1
    currentDate: Date = new Date();
    agingForDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), this.currentDate.getDate() + 1, 0, 0, 0); // +1 is to include today date to allowed values

    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date)
    {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string)
    {
        if (this.customer != value) {
            this.customer = value;

            if (value){
                this.ChartOfAccountsId_Dummy = null;
                this.IsCategoryDisabled = true;
            }
            else
                this.IsCategoryDisabled = false;

            this.SetUIProperties();
            this.SetGLAccountChartOfAccountSecurityLevel();

        }
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
    private _ChartOfAccountsTypeCode : string = "3";
    public get ChartOfAccountsTypeCode() : string {
        return this._ChartOfAccountsTypeCode;
    }
    public set ChartOfAccountsTypeCode(v : string) {
        this._ChartOfAccountsTypeCode = v;
        this.ChartOfAccountsId_Dummy = null;
    }
    private ChartOfAccountSecurityLevel: any;
    private chartOfAccount: any;
    public get ChartOfAccount() { return this.chartOfAccount; }
    public set ChartOfAccount(value: any)
    {
        if (this.chartOfAccount != value) {
            this.chartOfAccount = value;
            this.ChartOfAccountSecurityLevel = value? value.ChartOfAccountSecurityLevel: null;
        }
    }


    private _ChartOfAccountsId : string = null;
    public get ChartOfAccountsId_Dummy() : string {
        return this._ChartOfAccountsId;
    }
    public set ChartOfAccountsId_Dummy(v : string) {
        this._ChartOfAccountsId = v;
    }



    ValidateDate()
    {
        if (this.AgingForDate) {
            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate() + 1, 0, 0, 0); // +1 is to include today date to allowed values

            if (this.AgingForDate > currentDate) {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", false, TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
                return false;
            } else {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", true, "valid");
                return true;
            }
        }
        return true;
    }

    //private chartOfAccount: string;
    //public get ChartOfAccount() { return this.chartOfAccount; }
    //public set ChartOfAccount(value: string) {
    //    if (this.chartOfAccount != value) {
    //        this.chartOfAccount = value;
    //    }
    //}

    private numberOfMonths: number;
    public get NumberOfMonths() { return this.numberOfMonths; }
    public set NumberOfMonths(value: number)
    {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }


    //row 2

    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string)
    {
        if (this.collector != value) {
            this.collector = value;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string)
    {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }

    //row 3

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string)
    {
        if (this.category1 != value) {
            this.category1 = value;
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string)
    {
        if (this.category2 != value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string)
    {
        if (this.category3 != value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string)
    {
        if (this.category4 != value) {
            this.category4 = value;
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string)
    {
        if (this.category5 != value) {
            this.category5 = value;
        }
    }

    private currenciesDetailed: boolean;
    public get CurrenciesDetailed() { return this.currenciesDetailed; }
    public set CurrenciesDetailed(value: boolean)
    {
        if (this.currenciesDetailed != value) {
            this.currenciesDetailed = value;
        }
    }

    private balance: number;
    public get Balance() { return this.balance; }
    public set Balance(value: number)
    {
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

    //#endregion
     private errors: string[];
    RunButtonClicked()
    {
        this.SetUIProperties();

         this.errors = [];
        var categoryValue = null;
        var categoryIndex = null;
        this.ValidationErrorsList = [];

        //#region requierd fields
        if (!this.AgingForDate) { this.errors.push("Aging for date field is requierd"); }
        //if (!this.Customer) { errors.push("Customer field is requierd"); }
        if (!this.NumberOfMonths) { this.errors.push("Number of months field is requierd"); }
        //#endregion

        //#region Date validation
        var isDateValid = this.ValidateDate();
        if (!isDateValid)
            this.errors.push(TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
        //#endregion
        this.CheckIfChartOfAccountAndUserSecurityLevelAreMatched();


        if (this.errors.length == 0) {


            // Selecting category
            if (this.SelectedCategory) {
                categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category

                if (categoryIndex)
                    categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            }

            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("AgingForDate", this.AgingForDate, "Date"));
            myFilterItems.push(new QueryFilterItem("GLAccountType", this.AccountTypeCode));
            myFilterItems.push(new QueryFilterItem("CustomerId", this.Customer ? this.Customer : null));
            myFilterItems.push(new QueryFilterItem("NumberOfMonths", this.NumberOfMonths, "Number"));
            myFilterItems.push(new QueryFilterItem("CollectorId", this.Collector));

            myFilterItems.push(new QueryFilterItem("SalesmanId", this.Salesman));

            myFilterItems.push(new QueryFilterItem("Detailed", this.CurrenciesDetailed));
            myFilterItems.push(new QueryFilterItem("AgingMethod", this.SelectedAgingMethod.Name));

            myFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            myFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));
            myFilterItems.push(new QueryFilterItem("GroupByDate", this.DateFilterSelectedValue));
            myFilterItems.push(new QueryFilterItem("CurrencyOriginalLocalValue", this.currencyFilterSelectedValue));

            myFilterItems.push(new QueryFilterItem("BalanceFilter", this.balanceFilterSelectedValue.replace("filter_", "")));
            myFilterItems.push(new QueryFilterItem("BalanceFilterValue", this.balance || 0, "decimal"));

            if (this.balanceFilterSelectedValue == "filter_DebtBetween") {
                myFilterItems.push(new QueryFilterItem("FromBalanceFilterValue", this.fromBalance, "decimal"));
                myFilterItems.push(new QueryFilterItem("ToBalanceFilterValue", this.toBalance, "decimal"));
            }

            myFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCode", this.ChartOfAccountsTypeCode ? this.ChartOfAccountsTypeCode : null));
            myFilterItems.push(new QueryFilterItem("ChartOfAccountsId", this.ChartOfAccountsId_Dummy));



            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = this.errors;
        }
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
    private selectedAgingMethod: CodeNameClass;
    get SelectedAgingMethod() { return this.selectedAgingMethod; }
    set SelectedAgingMethod(value: CodeNameClass)
    {
        if (this.selectedAgingMethod != value) {
            this.selectedAgingMethod = value;
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
    SelectedItemChanged(item)
    {
        this.SelectedCategory = item;
    }
    //#endregion

    //#region Filter Methods
    public filterSelectedValue: string = 'filter_customer';
    public AccountTypeCode: string = '2';
    FilterItemClicked(itemValue: string)
    {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterChanged();
        }
    }
    FilterChanged()
    {

        this.Customer = null;
        this.Salesman = null;
        this.Collector = null;
        this.ChartOfAccountsId_Dummy = null;
        this.UIProperties.SetValidity("ChartOfAccountsId", "GLAccount", true,"");
        this.UIProperties.SetRequired("ChartOfAccountsId", "GLAccount", false);


        switch (this.filterSelectedValue) {
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
    //#endregion

    public balanceFilterSelectedValue: string = 'filter_Debtors';
    public currencyFilterSelectedValue: string = 'filter_OriginalCurr';
    BalanceFilterItemClicked(itemValue: string)
    {
        if (this.balanceFilterSelectedValue != itemValue) {
            this.balanceFilterSelectedValue = itemValue;
            this.BalanceFilterChanged();
        }
    }
    BalanceFilterChanged()
    {
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

    CurrencyFilterClicked(itemValue: string)
    {
        if (this.currencyFilterSelectedValue != itemValue) {
            this.currencyFilterSelectedValue = itemValue;
        }
    }

    // Filter Methods
    public DateFilterSelectedValue: string = 'filter_Due';
    DateFilterItemClicked(itemValue: string)
    {
        if (this.DateFilterSelectedValue != itemValue) {
            this.DateFilterSelectedValue = itemValue;
        }
    }

}
