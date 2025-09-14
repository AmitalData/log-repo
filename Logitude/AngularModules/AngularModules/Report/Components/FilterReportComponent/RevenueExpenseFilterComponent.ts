
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { Component, ChangeDetectorRef, ViewChild } from '@angular/core';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ChartOfAccountsTypeListService } from 'Accounting/Services/StandardLists/ChartOfAccountsTypeListService';
import { ChartOfAccountListService } from 'Accounting/Services/StandardLists/ChartOfAccountListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ComboBoxWithInCheckBox } from 'Controls/ComboBoxWithInCheckBox';



@Component({

    selector: 'RevenueExpenseFilterComponent',
    templateUrl: './RevenueExpenseFilterComponent.html',
    inputs: ['ReportsPreview']
})



export class RevenueExpenseFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    chartOfAccountsTypeListService: ChartOfAccountsTypeListService = new ChartOfAccountsTypeListService();
    chartOfAccountListService: ChartOfAccountListService = new ChartOfAccountListService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    @ViewChild("comboBoxWithCheckBoxChartOfAccountsComboBoxValue") comboBoxWithCheckBox: ComboBoxWithInCheckBox;

    private static readonly ALL_OPTION     = 'ALL';
    private static readonly WITH_OPTION    = 'WITH';
    private static readonly CARD_FILTER_ALL_NO_BALANCE      = "0";
    private static readonly CARD_FILTER_WITH_BALANCE        = "1";
    private static readonly CARD_FILTER_ALL_WITH_BALANCE    = "2";
    private static readonly CARD_FILTER_WITH_NO_BALANCE     = "3";


    GLAccountHtmlinputId: string;
    ChartofaccountHtmlinputId: string;
    chartofaccounttypeHtmlinputId: string;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    public ValidationErrorsList: string[] = [];
    queryFilterItem: QueryFilterItem;
    Level: string = "GLAccount";
    DataContext: any = this;
    showLocal: boolean;
    ChartOfAccountsSelectedValue:string
    ChartOfAccountsTypeSelectedValue: string;

    constructor(private CD: ChangeDetectorRef) {

        super();
        this.chartofaccounttypeHtmlinputId = Guid.newGuid();
        this.GLAccountHtmlinputId = Guid.newGuid();
        this.ChartofaccountHtmlinputId = Guid.newGuid();
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        var date = new Date();
        this.ToDate = new Date();
        date.setDate(1);
        date.setMonth(0);
        this.fromDate = date;


        this.GetDropDownItemsData();

        this.LoadResources();

    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.BuildFilterList();
    }
    public BalanceOptionsFilterList: CodeNameClass[];
    private BuildFilterList() {
        this.BalanceOptionsFilterList = [];
        this.BalanceOptionsFilterList.push(new CodeNameClass(RevenueExpenseFilterComponent.ALL_OPTION,  
            TextCodeTranslator.Translate("Accounting.General.O.AllAccountsOption")));

        this.BalanceOptionsFilterList.push(new CodeNameClass(RevenueExpenseFilterComponent.WITH_OPTION,  
            TextCodeTranslator.Translate("Accounting.General.O.WithTransactionsOption")
));
        this.SelectedBalanceOptionFilter = this.BalanceOptionsFilterList.filter(d => d.Code === RevenueExpenseFilterComponent.ALL_OPTION)[0];
    }

    private selectedBalanceOptionFilter: CodeNameClass;
    get SelectedBalanceOptionFilter() { return this.selectedBalanceOptionFilter; }
    set SelectedBalanceOptionFilter(value: CodeNameClass) {
        if (this.selectedBalanceOptionFilter !== value) {
            this.selectedBalanceOptionFilter = value;
        }
    }

    private useBalanceFilter: boolean;
    get UseBalanceFilter() { return this.useBalanceFilter; }
    set UseBalanceFilter(value: boolean) {
        if (this.useBalanceFilter !== value) {
            this.useBalanceFilter = value;
        }
    }

    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate !== value) {
            this.fromDate = value;
            this.Validate("FromDate");
        }
    }

    private toDate: Date = new Date()
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate !== value) {
            this.toDate = value;
            this.Validate("ToDate");

        }
    }

    Validate(dateFieldName) {
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {

            setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", null, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
                this.UIProperties.SetValidity("FromDate", null, false, TextCodeTranslator.Translate("Accounting.General.FromDateMustBeLTT"));
                this.CD.detectChanges();
            }, 200);

        } else {
            setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", null, true, "");
                this.UIProperties.SetValidity("FromDate", null, true, "");
                this.CD.detectChanges();
            }, 200);

        }


    }
    public FilterSelectedValue: string = 'GLAccount';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue !== itemValue) {
            this.FilterSelectedValue = itemValue;
            this.Level = itemValue;

        }
    }
    chartOfAccounts: any[] = [];
    chartOfAccountsTypes: any[] = [];
    selectedChartOfAccounts: any[] = [];
    selectedChartOfAccountsTypes: any[] = [];
    isChartOfAccountsTypesDisabled: boolean = false;
    isChartOfAccountsDisabled: boolean = false;
    isOpened: boolean = false;

    private chartOfAccountsTypeComboboxValue: string;
    public get ChartOfAccountsTypeComboboxValue(): string {
        return this.chartOfAccountsTypeComboboxValue;
    }
    public set ChartOfAccountsTypeComboboxValue(v: string) {
        this.chartOfAccountsTypeComboboxValue = v;

        if (this.chartOfAccountsTypeComboboxValue === "All") {
            this.idFilter = null;
            this.getChartOfAccounts();
        }
        this.resetCheckBoxTitle();

    }

    private chartOfAccountsComboBoxValue: string;
    public get ChartOfAccountsComboBoxValue(): string {
        return this.chartOfAccountsComboBoxValue;
    }
    public set ChartOfAccountsComboBoxValue(v: string) {
        this.chartOfAccountsComboBoxValue = v;
        this.SetChartOfAccountsFilterProperties();
    }


    isReady = false;
    private LoadResources() {
        this.entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) => { });
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {
            this.isReady = true;
        });
    }

    resetCheckBoxTitle() {
        if(this.selectedChartOfAccounts.length > 0){
            this.comboBoxWithCheckBox.TotalPickedItems = " ";
        }
    }
    GetDropDownItemsData() {

        this.getChartOfAccounts();
        this.getChartOfAccountsTypes();
    }


    private isRevenueExpenseFilter: boolean = false;
    private getChartOfAccounts() {

        this.isRevenueExpenseFilter = true;
        let apiQueryFilters = new ApiQueryFilters(true);
        
        this.selectedChartOfAccountsTypes = this.chartOfAccountsTypes.filter(item => item.Checked === true);
        if (this.selectedChartOfAccountsTypes.length === 0 || this.selectedChartOfAccountsTypes.length === 2) {
            apiQueryFilters.addAdditionalFilter("isRevenueExpenseFilter", "1", "2", null, "Equals", true, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.idFilter)) {
            apiQueryFilters.addAdditionalFilter("isRevenueExpenseFilter", this.idFilter, null, null, "Equals", true, false, false, "boolean");
        }
       
        this.chartOfAccountListService.getByFilters(apiQueryFilters)
            .subscribe((arg: any) => {
                this.chartOfAccounts = arg.Result;
                this.chartOfAccounts = this.chartOfAccounts.map(item => { return { ...item,
                     Name: item.LocalName || item.EnglishName,
                     Checked: this.ChartOfAccountsSelectedValue?.split(',').some(selectedItem => this.ChartOfAccountsComboBoxValue=="NotAll" && (selectedItem === item.Id || selectedItem === item.Code))
                    } });
            });
    }

    private getChartOfAccountsTypes() {

        let apiQueryFilters = new ApiQueryFilters(true);
        apiQueryFilters.addAdditionalFilter("isRevenueExpenseFilter", false, null, null, "Equals", true, false, false, "boolean");
        this.chartOfAccountsTypeListService.getByFilters(apiQueryFilters)
            .subscribe((arg: any) => {

                this.chartOfAccountsTypes = arg.Result;
                this.chartOfAccountsTypes = this.chartOfAccountsTypes.map(item => { return { ...item, Name: item.LocalName || item.EnglishName } });
                this.chartOfAccountsTypes = this.chartOfAccountsTypes.map(item => { 
                    return { 
                        ...item, 
                        Name: item.LocalName || item.EnglishName,
                        Checked: this.ChartOfAccountsTypeSelectedValue?.split(',').some(selectedItem =>this.ChartOfAccountsTypeComboboxValue=="NotAll" && (selectedItem === item.Id || selectedItem === item.Code))
                    } 
                });

                
            });
    }

    OnChartOfAccountsTypeItemClicked(items) {
        this.selectedChartOfAccountsTypes = this.chartOfAccountsTypes.filter(item => item.Checked === true);
        this.idFilter = this.selectedChartOfAccountsTypes.length === 1 ? this.selectedChartOfAccountsTypes[0].Code : null;
        this.getChartOfAccounts();
        
        this.resetCheckBoxTitle();
    }

    private idFilter: string = null;

    private DisableChartOfAccountField(haveSelectedItems: boolean) {
        this.isChartOfAccountsDisabled = haveSelectedItems;
        this.chartOfAccountsComboBoxValue = null;
    }

    OnChartOfAccountsItemClicked(items) {
        this.selectedChartOfAccounts = this.chartOfAccounts.filter(item => item.Checked === true);

        const haveSelectedItems = this.selectedChartOfAccounts.length > 0;
        this.DisableChartOfAccountsTypesField(haveSelectedItems);
        this.DisableCategoryFields(haveSelectedItems);

    }

    SetChartOfAccountsFilterProperties() {
        this.selectedChartOfAccountsTypes = this.chartOfAccountsTypes.filter(item => item.Checked === true);
        let haveSelectedItems = this.selectedChartOfAccountsTypes.length > 0;
        this.DisableChartOfAccountField(haveSelectedItems);
        this.DisableCategoryFields(haveSelectedItems);
        
        this.selectedChartOfAccounts = this.chartOfAccounts.filter(item => item.Checked === true);
        haveSelectedItems = this.selectedChartOfAccounts.length > 0;
        this.DisableChartOfAccountsTypesField(haveSelectedItems);
        this.DisableCategoryFields(haveSelectedItems);
    }
    private DisableChartOfAccountsTypesField(haveSelectedItems: boolean) {
        this.isChartOfAccountsTypesDisabled = haveSelectedItems;
    }
    private DisableCategoryFields(haveSelectedItems: boolean) {
        this.IsCategoryDisabled = haveSelectedItems;
        if (haveSelectedItems) {
            this.SelectedCategory = null;
            this.Category1 = null;
            this.Category2 = null;
            this.Category3 = null;
            this.Category4 = null;
            this.Category5 = null;
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


    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string) {
        if (this.category1 !== value) {
            this.category1 = value;
        }
    }

    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string) {
        if (this.category2 !== value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string) {
        if (this.category3 !== value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string) {
        if (this.category4 !== value) {
            this.category4 = value;
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string) {
        if (this.category5 !== value) {
            this.category5 = value;
        }
    }
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) { //For Scheduler Report
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
     public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {
         
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
       
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "CreateDate":{
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
                }
                case "FromDate":{
                    this.FromDate = new Date(queryFilterItem.FieldValue)
                    break;
                }
                case "ChartOfAccountsTypeCodeList":{
                    this.ChartOfAccountsTypeSelectedValue=queryFilterItem.FieldValue
                    break;
                }
                case"ChartOfAccountsTypeCodeList_param":{
                     this.chartOfAccountsTypeComboboxValue=queryFilterItem.FieldValue
                    break;
                }
                case "ChartOfAccountsIdList_param":{
                    this.chartOfAccountsComboBoxValue=queryFilterItem.FieldValue
                    break;
                }
                case "ChartOfAccountsIdList":{
                   this.ChartOfAccountsSelectedValue= queryFilterItem.FieldValue
                    
                    break;
                }
                case "Level":{
                    this.FilterSelectedValue = queryFilterItem.FieldValue;
                     break;
                }
                    

                 case "CardFilter":
                 {  this.BuildFilterList();

                    this.SelectedBalanceOptionFilter =
                        (queryFilterItem.FieldValue == RevenueExpenseFilterComponent.CARD_FILTER_ALL_NO_BALANCE ||
                        queryFilterItem.FieldValue == RevenueExpenseFilterComponent.CARD_FILTER_ALL_WITH_BALANCE)
                            ? this.BalanceOptionsFilterList.filter(d => d.Code == RevenueExpenseFilterComponent.ALL_OPTION)[0]
                            : this.BalanceOptionsFilterList.filter(d => d.Code == RevenueExpenseFilterComponent.WITH_OPTION)[0];

                    this.UseBalanceFilter =
                        queryFilterItem.FieldValue == RevenueExpenseFilterComponent.CARD_FILTER_WITH_BALANCE ||
                        queryFilterItem.FieldValue == RevenueExpenseFilterComponent.CARD_FILTER_ALL_WITH_BALANCE;

                    break;
                 }
              
            }
    
           
    
        }
    }
    
    RunReport(isInteractive: boolean) {
        
        if (this.ValidateSelectedFilters()) {
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();

            this.ReportsPreview.GenerateReport(this.reportFliter, isInteractive);
        }

    }
    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();


        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CreateDate";
        this.queryFilterItem.FieldValue = this.ToDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "LessThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);
        this.queryFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));


        if (this.selectedChartOfAccountsTypes && this.selectedChartOfAccountsTypes.length > 0){
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCodeList", this.selectedChartOfAccountsTypes.map(item => item.Code).join(','), "String"));
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCodeList_param", "NotAll", "String"));

        }
        else {
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCodeList", this.chartOfAccountsTypes.map(item => item.Code).join(','), "String"));
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsTypeCodeList_param", "All", "String"));

        }

        if (this.selectedChartOfAccounts && this.selectedChartOfAccounts.length > 0){
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList", this.selectedChartOfAccounts.map(item => item.Id).join(','), "String"));
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList_param", "NotAll", "String"));

        }
        else {
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList_param", "All", "String"));
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList", this.chartOfAccounts.map(item => item.Id).join(','), "String"));
        }
        
        if (!this.Level) this.Level = "GLAccount";
        this.queryFilterItems.push(new QueryFilterItem("Level", this.Level));

        this.queryFilterItems.push(new QueryFilterItem("IncludeZeroBalance", this.UseBalanceFilter));
        this.queryFilterItems.push(new QueryFilterItem("SelectedBalance", this.SelectedBalanceOptionFilter.Code));

        if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code === RevenueExpenseFilterComponent.ALL_OPTION) {
            this.queryFilterItems.push(new QueryFilterItem("CardFilter", RevenueExpenseFilterComponent.CARD_FILTER_ALL_NO_BALANCE));
        }
        else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code === RevenueExpenseFilterComponent.ALL_OPTION) {
            this.queryFilterItems.push(new QueryFilterItem("CardFilter", RevenueExpenseFilterComponent.CARD_FILTER_ALL_WITH_BALANCE));
        }
        else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code === RevenueExpenseFilterComponent.WITH_OPTION) {
            this.queryFilterItems.push(new QueryFilterItem("CardFilter", RevenueExpenseFilterComponent.CARD_FILTER_WITH_BALANCE));
        }
        else if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code === RevenueExpenseFilterComponent.WITH_OPTION) {
            this.queryFilterItems.push(new QueryFilterItem("CardFilter", RevenueExpenseFilterComponent.CARD_FILTER_WITH_NO_BALANCE));
        }



        return this.queryFilterItems
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        var FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.ToDate > new Date()) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
        }
        if (this.ToDate === null) {
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
            this.ValidationErrorsList.push(s);
        }
        if (this.FromDate === null) {
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.FromDate"));
            this.ValidationErrorsList.push(s);
        }
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
        }

        return this.ValidationErrorsList.length === 0;
    }


}

