import { Component ,ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../Filters/ReportFliter';
import { QueryFilterItem } from '../../Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { CodeNameClass } from 'Infrastructure/DataContracts/CodeNameClass';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { AppTool } from 'Infrastructure/Tools';
import { ChartOfAccountsTypeListService } from 'Accounting/Services/StandardLists/ChartOfAccountsTypeListService';
import { ChartOfAccountListService } from 'Accounting/Services/StandardLists/ChartOfAccountListService';

@Component({

    templateUrl: './MonthlyBalancesReportFilterComponent.html',
})

export class MonthlyBalancesReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[] = [];

    public DataContext = this;
    public ObjectTableName: string = "GLAccount";
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public TaxReportLists: CodeNameClass[] = [];
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isReady: boolean = false;
    public ChartOfAccountsComboBoxValue: string ="All";
    private filterAll = "All";
    private filterNotAll = "NotAll";
    selectedChartOfAccountsTypes: any[] = [];
    chartOfAccountsTypes: any[] = [];
    selectedChartOfAccounts: any[] = [];
    chartOfAccounts: any[] = [];

    chartOfAccountsTypeListService: ChartOfAccountsTypeListService = new ChartOfAccountsTypeListService();
    chartOfAccountListService: ChartOfAccountListService = new ChartOfAccountListService();
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;

    constructor(public entityListService: EntityListService, private CD: ChangeDetectorRef) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("TaxReport").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("ARInvoiceLine").subscribe((response: any) => {
                        this.entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((response: any) => {
                            this.entityResourceService.getEntityResourceByTableName("General").subscribe((response: any) => {
                            this.isReady = true;

                    });
                    });
                    });
                });
            });
        });
        this.DataContext.UIProperties.SetRequired("ChartOfAccountsComboBoxValue", this.ObjectTableName, true)
        this.DataContext.UIProperties.SetRequired("NumberOfYear", this.ObjectTableName, true)

    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.DetailedForJobs=false
        this.getChartOfAccounts();

    }

  

    ValidateDate() {
        
    }


    private detailedForJobs: boolean;
    get DetailedForJobs() { return this.detailedForJobs; }
    set DetailedForJobs(value: boolean)
    {
        if (this.detailedForJobs != value) {
            this.detailedForJobs = value;
        }
    }

    private numberOfYear: number;
    get NumberOfYear() { return this.numberOfYear; }
    set NumberOfYear(value: number)
    {
        if (this.numberOfYear != value) {
            this.numberOfYear = value;
        }
        if(this.numberOfYear!=null){
            this.DataContext.UIProperties.SetRequired("NumberOfYear", this.ObjectTableName, false)

        }
        else{
            this.DataContext.UIProperties.SetRequired("NumberOfYear", this.ObjectTableName, true)

        }
    }
    SetChartOfAccountsFilterProperties(){
              this.selectedChartOfAccounts = this.chartOfAccounts.filter(item=>item.Checked == true);
    }
    
    AdditionalServiceSelectedValue:string

    private getChartOfAccounts()
    {
        let apiQueryFilters = new ApiQueryFilters(true);

        this.chartOfAccountListService.getByFilters(apiQueryFilters)
            .subscribe((arg: any) =>
            {
                this.chartOfAccounts = arg.Result;                
                this.chartOfAccounts = this.chartOfAccounts.map(item=> {return {...item,
                    Name: `(${ item.Code }) ${ item.LocalName || item.EnglishName }`,
                    Checked: this.ChartOfAccountsComboBoxValue === this.filterNotAll && this.AdditionalServiceSelectedValue?.split(',').some(selectedItem =>  selectedItem === item.Code||selectedItem === item.Id)

                }}).sort((a, b) => a.Code - b.Code); });

    }
    OnChartOfAccountsItemClicked(items){

        this.selectedChartOfAccounts = this.chartOfAccounts.filter(item=>item.Checked == true);

    }
    SelectedItemChanged(item) {
        this.ChartOfAccountsComboBoxValue = item;
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
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) { 
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
                case "DetailedForJobs":
                    this.DetailedForJobs= queryFilterItem.FieldValue;
                    break;
                case "NumberOfYear":
                    this.NumberOfYear = queryFilterItem.FieldValue;
                    break;
                case "ChartOfAccountsIdList":
                    {  this.ChartOfAccountsComboBoxValue =  queryFilterItem.FieldValue === this.filterAll ? this.filterAll : this.filterNotAll; 
                       this.AdditionalServiceSelectedValue = queryFilterItem.FieldValue;
                       
                       break;
    
                    }
                                 
            }
                  
        }
    }
   
    RunReport(isInteractive: boolean) {
        if (this.ValidateSelectedFilters()) {
            this.BuildReport(isInteractive);
        }
    }
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];

        var FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
       
            if (this.NumberOfYear == null) {
                var FromDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", "שנה");
                this.ValidationErrorsList.push(FromDateValidation);
            }

            if (this.selectedChartOfAccounts == null) {
                var ToDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", "קבוצת מאזן");
                this.ValidationErrorsList.push(ToDateValidation);
            }
        return this.ValidationErrorsList.length == 0;
          
    }
    GetQueryFilterItems() {
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItems.push(new QueryFilterItem("DetailedForJobs", this.DetailedForJobs, "boolean"));
        if(this.selectedChartOfAccounts)
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList", this.selectedChartOfAccounts.map(item=>item.Id).join(','), "String"));
        this.queryFilterItems.push(new QueryFilterItem("NumberOfYear", this.NumberOfYear, "number"));
        return this.queryFilterItems;
             

    }
    BuildFilterByOperator(FieldName, FieldValue, FieldValue2, Type, Operator: { Code: string, EnglishName: string, LocalName: string }) {
        if (!AppTool.IsNullOrEmpty(FieldValue) && !AppTool.IsNullOrEmpty(Operator)) {

            var FilterOperator = Operator.EnglishName.replace(/ /g, ''); // remove white spaces
            var num1 = FieldValue;
            var num2 = FieldValue2;
            this.queryFilterItems.push(this.GetNewQueryFilterItem(FieldName, num1, num2, Type, FilterOperator));


        }
    }
    BuildReport(isInteractive: boolean) {
        
        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(this.reportFliter, isInteractive);
    }

    GetNewQueryFilterItem(FieldName: string, FieldValue: any, FieldValue2: any = null, FieldDataType: string = null, Operator: string = "Equals") {
        var queryFilterItem = new QueryFilterItem();
        queryFilterItem.DisplayInList = false;
        queryFilterItem.FieldName = FieldName;
        queryFilterItem.FieldValue = FieldValue;
        queryFilterItem.FieldValue2 = FieldValue2;
        queryFilterItem.Operator = Operator;
        queryFilterItem.FieldDataType = FieldDataType;

        return queryFilterItem;
    }
    
}




