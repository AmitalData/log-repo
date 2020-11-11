import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { FullAccountingSettingListService } from 'Accounting/Services/StandardLists/FullAccountingSettingListService';
import { FullAccountingSettingList } from 'Accounting/EntityLists/FullAccountingSettingList';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { CardExtendedPMService } from '../../../../Common/Services/ExtendedPMs/CardExtendedPMService';
import { ReportsPreviewComponent } from '../../ReportsPreviewComponent';

@Component({
    templateUrl: './CustomerStatusReportFilterComponent.html',
})

export class CustomerStatusReportFilterComponent extends BaseComponent implements OnInit {
  public IsCreditLimitSet: boolean = false;

    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    public SalesmanFilterItems: ApiQueryFilters;
    public ReportsPreview: ReportsPreviewComponent;
    public _FullAccountingSettingListService: FullAccountingSettingListService = new FullAccountingSettingListService();
    public IsSchedulerReport: boolean;
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public showLocals: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    public RunReportTitle: string;
    CardExtendedPMService: CardExtendedPMService = new CardExtendedPMService();
    constructor() {
        super();

        // get requierd resources
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(response => { this.isReady = true; this.SetRunReportTitle();  });
        this.entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(response => { });

        // salesman lov field filtera
        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);
        // set default value for no of months
        // var newDate = new Date();
        // var currentMonth = newDate.getMonth()+1;
        //this.NumberOfMonths = currentMonth - 6; // 6 backward
        // this.NumberOfMonths = 6; // 6 backward

    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }
    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        // this.UIProperties.SetRequired("AgingForDate", "GLAccount", true);
        //this.UIProperties.SetRequired("Customer", "GLAccount", true);
        //this.UIProperties.SetRequired("NumberOfMonths", "GLAccount", true);

        // if(this.filterSelectedValue == "filter_vendor"){
        //     this.UIProperties.SetEnabled("Salesman","GLAccount",false);
        //     this.UIProperties.SetEnabled("Collector","GLAccount",false);
        // }else{
        //     this.UIProperties.SetEnabled("Salesman","GLAccount",true);
        //     this.UIProperties.SetEnabled("Collector","GLAccount",true);
        // }

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

    //#region Filters

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) { //For Scheduler Report
        this.IsSchedulerReport = true;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    //row 1
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

            this.DimAndResetCategoryFields(value);

        }
    }

    private chartOfAccount: string;
    private DimAndResetCategoryFields(value: string)
    {
        if (value) {
            this.IsCategoryDisabled = true;
            this.Category1 = null;
            this.Category2 = null;
            this.Category3 = null;
            this.Category4 = null;
            this.Category5 = null;
        }
        else
            this.IsCategoryDisabled = false;
    }
    private DimAndResetCustomerFields(value: string)
    {
        if (value) {
            this.UIProperties.SetEnabled("Customer", "GLAccount", false);
            this.UIProperties.SetEnabled("ChartOfAccount", "GLAccount", false);
            this.Customer = null;
            this.ChartOfAccount = null;
        }
        else {
            this.UIProperties.SetEnabled("Customer", "GLAccount", false);
            this.UIProperties.SetEnabled("ChartOfAccount", "GLAccount", false);
            this.IsCategoryDisabled = false;
        }
    }

    public get ChartOfAccount() { return this.chartOfAccount; }
    public set ChartOfAccount(value: string) {
        if (this.chartOfAccount != value) {
            this.chartOfAccount = value;

            this.DimAndResetCategoryFields(value);

        }
    }
    ValidateDate() {
        if (this.AgingForDate) {
            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()+1, 0, 0, 0); // +1 is to include today date to allowed values

            if (this.AgingForDate > currentDate) {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", false, TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
                return false;
            } else {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", true, "valid");
                this.UIProperties.SetRequired("AgingForDate", "GLAccount", false);
                return true;
            }
        }
        return true;
    }
    IsPartnersChanged(SelectedTab) {
        if (SelectedTab == '2')
            this.GLAccountChanged = false;
        return this.GLAccountChanged;
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
    public set NumberOfMonths(value: number) {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }


    //row 2
    GLAccountChanged: boolean;
    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string) {
        if (this.collector != value) {
            this.collector = value;
            this.GLAccountChanged = true;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string) {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }

    //row 3

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string) {
        if (this.category1 != value) {
            this.category1 = value;
            this.DimAndResetCustomerFields(value);
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string) {
        if (this.category2 != value) {
            this.category2 = value;
            this.DimAndResetCustomerFields(value);
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string) {
        if (this.category3 != value) {
            this.category3 = value;
            this.DimAndResetCustomerFields(value);
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string) {
        if (this.category4 != value) {
            this.category4 = value;
            this.DimAndResetCustomerFields(value);
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string) {
        if (this.category5 != value) {
            this.category5 = value;
            this.DimAndResetCustomerFields(value);
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

    public  accSettings: FullAccountingSettingList;
            LoadAccSettings() {
              return new Promise(resolve => {
                // Full Accounting Settings
                this.CurrentSession.StartBusyIndicatorLoading();
                this._FullAccountingSettingListService.getAll().subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        var res = myResponse.Result;
                      if (res != null && res.length > 0) {
                        var list: FullAccountingSettingList[];
                        list = res;
                        this.accSettings = list[0]; // because there is only one record for each tenant
                        resolve(this.accSettings);
                      }
                      else {
                      //  reject();
                      }
                    }
                });
              });
            }
    //#endregion
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            this.SetAgingForDateFilter(queryFilterItem);
            this.SetCustomerIdFilter(queryFilterItem);            
            this.SetCollectorIdFilter(queryFilterItem);          
            this.SetSalesmanIdFilter(queryFilterItem);        
            this.SetDetailedFilter(queryFilterItem);          
            this.SetIsCreditLimitFilter(queryFilterItem);          
            this.SetBalanceFilter(queryFilterItem);
            this.SetBalanceFilterValue(queryFilterItem);
            this.SetSortFieldFilter(queryFilterItem);          
            this.SetSortDirectionFilter(queryFilterItem);          
        }
    }

    SetAgingForDateFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "AgingForDate") {
            this.AgingForDate = queryFilterItem.FieldValue;
        }
    }
    SetCustomerIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "CustomerId") {
            this.Customer = queryFilterItem.FieldValue;
        }
    }
    SetNumberOfMonthsFilter(queryFilterItem: QueryFilterItem) {
        if(queryFilterItem.FieldName == "NumberOfMonths") {
            this.NumberOfMonths = 5;
        }
    }
    SetCollectorIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "CollectorId") {
            this.Collector = queryFilterItem.FieldValue;
        }
    }
    SetSalesmanIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "SalesmanId") {
            this.Salesman = queryFilterItem.FieldValue;
        }
    }
    SetDetailedFilter(queryFilterItem: QueryFilterItem) {
    if (queryFilterItem.FieldName == "Detailed") {
        this.CurrenciesDetailed = queryFilterItem.FieldValue;
    }
    }

    SetIsCreditLimitFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "IsCreditLimitSet") {
            this.IsCreditLimitSet = queryFilterItem.FieldValue;
        }
    }


    SetSortDirectionFilter(queryFilterItem: QueryFilterItem) {
          if (queryFilterItem.FieldName == "SortDirection") {
            this.SelectedSortDirectionCode = queryFilterItem.FieldValue;
        }
    }
    SetSortFieldFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "SortField") {
            this.SelectedSortTypeItem.Code = queryFilterItem.FieldValue;
        }
    }
    SetBalanceFilterValue(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "BalanceFilterValue") {
            this.balance = queryFilterItem.FieldValue;
        }
    }
    SetBalanceFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "BalanceFilter") {
            this.SelectedBalanceTypeItem.Code = queryFilterItem.FieldValue;
        }
    }


    GetLookUpFieldValue(field) {
        if (field) {
            if (field[0]["@nil"] != "true")
                return field;
        }
        return null
    }

    PrepareContactList() {
        //var cardExtendedPMService = new CardExtendedPMService();
        //var glAccountId = this.GetLookUpFieldValue(this.Collector);
        //if (glAccountId != null) {
        //    cardExtendedPMService.GetAllConnectedPartnersByGLAccountId(glAccountId).subscribe((response: ServiceResponse) => {
        //        if (!response.HasError) {
        //            var allContacts = response.Result;
        //            if (allContacts != null && allContacts.length > 0) {
        //                allContacts.forEach(contact => {
        //                    if (!AppTool.IsNullOrEmpty(contact)) this.ReportsPreview.AddPartner(contact.PartnerName, contact.PartnerId);
        //                });
        //                this.ReportsPreview.PartnersObslist.reverse();
        //            }
        //        }
        //    });
        //}
    }

    RunButtonClicked() {
        this.SetUIProperties();
      this.LoadAccSettings().then(res => { 

        var errors: string[] = [];
     
        this.ValidationErrorsList = [];

        //#region requierd fields
        // if (!this.AgingForDate) { errors.push("Aging for date field is requierd"); }
        //if (!this.Customer) { errors.push("Customer field is requierd"); }
        // if (!this.NumberOfMonths) { errors.push("Number of months field is requierd"); }
        //#endregion

        //#region Date validation
          if (this.ValidateSelectedFilters()) {
              var myReportFliter: ReportFliter = new ReportFliter();
              myReportFliter.NumberOfPage = 1;
              myReportFliter.ProcessType = "GenerateReport";
              myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();

              this.RunReportEvent.emit(myReportFliter);
          }
        else {
          
            this.ValidationErrorsList = errors;
        }
      }); 
    }
   
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        var isValid: boolean = true;
        this.ValidateFutureDate(isValid);
        this.ValidateAgingMonth(isValid);
        this.ValidateDebitBalance(isValid);      
        return isValid;
    }

    ValidateFutureDate(isValid:boolean){
        var isDateValid = this.ValidateDate();
        if (!isDateValid) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
            isValid = false;
        }
        return isValid;
    }

    ValidateAgingMonth(isValid: boolean) {

        if (this.accSettings && !this.accSettings.NumberOfAgingMonths) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("LedgerTransaction.O.AgingMonthNotSet"));
            isValid = false;
        }
        return isValid;
    }
    ValidateDebitBalance(isValid: boolean) {
        if (this.SelectedBalanceTypeCode == "debt" && !this.Balance) {
            this.ValidationErrorsList.push(this.showLocals ? "נא לבחור סכום לשדה ''מעל חוב" : "Please enter an amount for the 'Debt Above' field");
            isValid = false;
        }
        return isValid;
    }
    GetQueryFilterItems() {
        var queryFilterItems = new Array<QueryFilterItem>();
        var queryFilterItem: QueryFilterItem;              
        queryFilterItems.push(new QueryFilterItem("AgingForDate", new Date(), "Date"));
        queryFilterItems.push(new QueryFilterItem("GLAccountType", "2"));
        queryFilterItems.push(new QueryFilterItem("CustomerId", this.Customer ? this.Customer : null));
        queryFilterItems.push(new QueryFilterItem("NumberOfMonths", 5, "Number"));
        queryFilterItems.push(new QueryFilterItem("CollectorId", this.Collector));
        queryFilterItems.push(new QueryFilterItem("SalesmanId", this.Salesman));
        queryFilterItems.push(new QueryFilterItem("Detailed", this.CurrenciesDetailed));
        queryFilterItems.push(new QueryFilterItem("IsCreditLimitSet", this.IsCreditLimitSet));       
        queryFilterItems.push(new QueryFilterItem("GroupByDate", ""));
        queryFilterItems.push(new QueryFilterItem("BalanceFilter", this.SelectedBalanceTypeItem.Code));
        queryFilterItems.push(new QueryFilterItem("BalanceFilterValue", this.balance || 0, "decimal"));
        queryFilterItems.push(new QueryFilterItem("SortField", this.SelectedSortTypeItem.Code));
        queryFilterItems.push(new QueryFilterItem("SortDirection", this.SelectedSortDirectionCode));
        this.SetCategoryIndexAndValueFilters(queryFilterItems);   
        queryFilterItems.push(new QueryFilterItem("CategoryIndex", this.categoryIndex));
        queryFilterItems.push(new QueryFilterItem("CategoryValue", this.categoryValue));
        return queryFilterItems;
    }
    categoryIndex: any = null;
    categoryValue: any = null;
    SetCategoryIndexAndValueFilters(queryFilterItems: Array<QueryFilterItem>) {
       this.categoryIndex = this.SelectedCategory? this.SelectedCategory.replace(' ', ''): null;      
        if (this.categoryIndex)       
        this.SetCategoryValueFilter( );      
    }
   
    SetCategoryValueFilter() {
        this.categoryValue = this.DataContext[this.categoryIndex];      
    }

    IsBalanceTypeDisabled = false;

    SelectedBalanceTypeItem = { Code: "all", EnglishName: "All", LocalName: "הכל" };
    BalanceTypeChanged(type){
        this.SelectedBalanceTypeItem = type;
        this.SelectedBalanceTypeCode = type.Code;
        this.Balance = null;
    }
    SelectedBalanceTypeCode: string;
    BalanceTypes = [
        { Code: "all", EnglishName: "All", LocalName: "הכל" },
        { Code: "debtors", EnglishName: "Debtors only", LocalName: "רק בעלי חוב" },
        { Code: "debt", EnglishName: "Debt Above", LocalName: "חוב מעל" },
    ];


    SelectedSortDirectionCode: string = "Ascending";
    // SortDirections = [
    //     { Code: "asc", EnglishName: "Ascending", LocalName: "Ascending" },
    //     { Code: "desc", EnglishName: "Descending", LocalName: "Descending" },
    // ];
    // SortDirectionChanged(dir){
    //     this.SelectedSortDirectionCode = dir.Code;
    // }

    SelectedSortTypeCode: string = "balance";
    SelectedSortTypeItem = { Code: "balance", EnglishName: "Balance", LocalName: "Balance" };
    SortTypes = [
        { Code: "balance", EnglishName: "Balance", LocalName: "Balance" },
        { Code: "customer", EnglishName: "Customer", LocalName: "Customer" },
    ];
    SortTypeChanged(dir){
        this.SelectedSortTypeCode = dir.Code;
        this.SelectedSortTypeItem = dir;
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
    //#endregion



    //#region Filter Methods
    public filterSelectedValue: string = 'filter_customer';
    // public AccountTypeCode: string = '2';
    FilterItemClicked(itemValue: string)
    {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterChanged();
        }
    }
    FilterChanged()
    {

        // this.Customer = null;
        // this.Salesman = null;
        // this.Collector = null;
        switch (this.filterSelectedValue) {
            case 'filter_customer':
                this.ChartOfAccount = null;
                break;
            case 'filter_chartOfAccount':
                this.Customer = null;
                break;
            default:
                break;
        }

        this.SetUIProperties();
        // this.ValidateDate();

    }
      //#endregion

      public balanceFilterSelectedValue: string = 'filter_All';
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
              case 'filter_DebtAbove':
                  this.balance = 0;
                  break;
              default:
                  break;
          }

          this.SetUIProperties();

      }

    // Filter Methods
    // public DateFilterSelectedValue: string = 'filter_Due';
    // DateFilterItemClicked(itemValue: string) {
    //     if (this.DateFilterSelectedValue != itemValue) {
    //         this.DateFilterSelectedValue = itemValue;
    //     }
    // }

}
