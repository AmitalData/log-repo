
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {Component, OnInit}  from '@angular/core';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';



@Component({
    moduleId: module.id,
    selector: 'RevenueExpenseFilterComponent',
    templateUrl: './RevenueExpenseFilterComponent.html',
    inputs: ['ReportsPreview']
})



export class RevenueExpenseFilterComponent extends BaseComponent{
    public ReportsPreview: ReportsPreviewComponent;     
    GLAccountHtmlinputId: string;
    ChartofaccountHtmlinputId: string;
    chartofaccounttypeHtmlinputId: string;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    public ValidationErrorsList: string[] = [];
    queryFilterItem: QueryFilterItem;
    Level: string = "GLAccount";
  ToDate: Date = new Date();
    DataContext: any = this;
    showlocal: boolean;
    constructor() {

        super();
        this.chartofaccounttypeHtmlinputId = Guid.newGuid();
        this.GLAccountHtmlinputId = Guid.newGuid();
        this.ChartofaccountHtmlinputId = Guid.newGuid();
        this.showlocal = !SessionLocator.LoggedUserPM.DontShowLocal;

    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;    
        this.BuildFilterList();
    }
    public BalanceOptionsFilterList: CodeNameClass[];
    private BuildFilterList() {
        this.BalanceOptionsFilterList = [];
        this.BalanceOptionsFilterList.push(new CodeNameClass("WITHOUT", "Without Transactions","בלי תנועות"));
        this.BalanceOptionsFilterList.push(new CodeNameClass("WITH", "With Transactions", "עם תנועות"));
      
        this.SelectedBalanceOptionFilter = this.BalanceOptionsFilterList.filter(d => d.Code == "WITHOUT")[0];
    }

    private selectedBalanceOptionFilter: CodeNameClass;
    get SelectedBalanceOptionFilter() { return this.selectedBalanceOptionFilter; }
    set SelectedBalanceOptionFilter(value: CodeNameClass) {
        if (this.selectedBalanceOptionFilter != value) {
            this.selectedBalanceOptionFilter = value;
        }
    }
    
    private useBalanceFilter: boolean;
    get UseBalanceFilter() { return this.useBalanceFilter; }
    set UseBalanceFilter(value: boolean) {
        if (this.useBalanceFilter != value) {
            this.useBalanceFilter = value;
        }
    }

  public FilterSelectedValue: string = 'GLAccount';
  FilterItemClicked(itemValue: string) {
    if (this.FilterSelectedValue != itemValue) {
      this.FilterSelectedValue = itemValue;
      this.Level = itemValue;
 
    }
  }

    RunReport() {
        this.ValidationErrorsList = [];
        if (this.ToDate > new Date()) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));

        }
        if (this.ToDate == null)
        {
            var FIELD_IS_REQUIERD: string = null;
            FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
         
          
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.Date"));
            this.ValidationErrorsList.push(s);
        }

        

        

        if (this.ValidationErrorsList.length == 0) {

            this.queryFilterItems = new Array<QueryFilterItem>();

           
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
          if (!this.Level) this.Level = "GLAccount";
          this.queryFilterItems.push(new QueryFilterItem("Level", this.Level));
                if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
                    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "0"));
                 
                }

                else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT"){
                    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "2"));
                }
                else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITH") {
                    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "1"));
                }


               
              

                this.reportFliter = new ReportFliter();
               // this.reportFliter.Level = this.Level;
                this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
                this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
                this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
               
                this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
                this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
                this.reportFliter.NumberOfPage = 1;
                this.reportFliter.ProcessType = "GenerateReport";

                this.ReportsPreview.CleanPartnersObslist();

               


                this.ReportsPreview.GenerateReport(this.reportFliter, true);
            
           
        }

    }
}
