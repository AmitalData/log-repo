
import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
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
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { reject } from 'q';
import { CodeNameClass } from 'Infrastructure/DataContracts/CodeNameClass';
import { TaxReportExtendedPMService } from 'Accounting/Services/ExtendedPMs/TaxReportExtendedPMService';
import { TaxReportPM } from 'Accounting/EntityPMs/TaxReportPM';
import { Operators } from 'Accounting/DataContracts/Operators';
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

    //new
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

        //new 
        this.getChartOfAccounts();
    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.DetailedForJobs=false
    }

  

    ValidateDate() {
        // var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        // if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {

        //     setTimeout(() => {
        //         if (!this.IsOldDate("ToDate"))
        //             this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
        //         this.errors = [];
        //         if (!this.IsOldDate("FromDate"))
        //             this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
        //         this.CD.detectChanges();
        //     }, 200);

        // } else {
        //     setTimeout(() => {
        //         this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
        //         this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
        //         this.CD.detectChanges();
        //     }, 200);

        // }
    }

      
   
   //new
    private chartOfAccountsComboBoxValue: string;
    public get ChartOfAccountsComboBoxValue(): string
    {
        return this.chartOfAccountsComboBoxValue;
    }
    public set ChartOfAccountsComboBoxValue(v: string)
    {
        this.chartOfAccountsComboBoxValue = v;
        this.SetChartOfAccountsFilterProperties();
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
    

    private getChartOfAccounts()
    {
        let apiQueryFilters = new ApiQueryFilters(true);

        this.chartOfAccountListService.getByFilters(apiQueryFilters)
            .subscribe((arg: any) =>
            {
                this.chartOfAccounts = arg.Result;                
                this.chartOfAccounts = this.chartOfAccounts.map(item=> {return {...item,Name: `(${ item.Code }) ${ item.LocalName || item.EnglishName }`}}).sort((a, b) => a.Code - b.Code);
            });
    }
    OnChartOfAccountsItemClicked(items){

        this.selectedChartOfAccounts = this.chartOfAccounts.filter(item=>item.Checked == true);

    }


   
   


   

    RunReport() {
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

          
        
        if (this.ValidationErrorsList.length == 0) {

            this.BuildReport();

        }
    }

    InitilaizeFilter() {
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItems.push(new QueryFilterItem("DetailedForJobs", this.DetailedForJobs, "boolean"));
        if(this.selectedChartOfAccounts)
            this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountsIdList", this.selectedChartOfAccounts.map(item=>item.Id).join(','), "String"));
        this.queryFilterItems.push(new QueryFilterItem("NumberOfYear", this.NumberOfYear, "number"));
             

    }
    BuildFilterByOperator(FieldName, FieldValue, FieldValue2, Type, Operator: { Code: string, EnglishName: string, LocalName: string }) {
        if (!AppTool.IsNullOrEmpty(FieldValue) && !AppTool.IsNullOrEmpty(Operator)) {

            var FilterOperator = Operator.EnglishName.replace(/ /g, ''); // remove white spaces
            var num1 = FieldValue;
            var num2 = FieldValue2;
            this.queryFilterItems.push(this.GetNewQueryFilterItem(FieldName, num1, num2, Type, FilterOperator));


        }
    }
    BuildReport() {
        this.InitilaizeFilter();

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(this.reportFliter, true);
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
    ClearFields(){
       

    }
}




