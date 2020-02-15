
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
import { UIProperties, UIProperty } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { DateTool } from '../../../Infrastructure/Tools';



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
 // ToDate: Date = new Date();
    DataContext: any = this;
    showlocal: boolean;
    constructor() {

        super();
        this.chartofaccounttypeHtmlinputId = Guid.newGuid();
        this.GLAccountHtmlinputId = Guid.newGuid();
        this.ChartofaccountHtmlinputId = Guid.newGuid();
        this.showlocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        var date = new Date();
        this.ToDate = new Date();
        date.setDate(1);
        date.setMonth(0);
        this.fromDate = date;
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
  
    private fromDate: Date;
  public get FromDate() { return this.fromDate; }
  public set FromDate(value: Date) {
    if (this.fromDate != value) {
        this.fromDate = value;
        if (value > this.ToDate) {
            this.UIProperties.SetValidity("FromDate", null, false, TextCodeTranslator.Translate("Accounting.General.FromDateMustBeLTT"));

        }
    }
    }

    private toDate: Date = new Date()
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            if (value < this.FromDate) {
                this.UIProperties.SetValidity("ToDate", null, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));

            }
            if (value > new Date()) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
            }
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
       
        this.ValidationErrorsList = this.ValidateFilters();
        if (this.ValidationErrorsList.length == 0) {

            this.queryFilterItems = new Array<QueryFilterItem>();

           
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));

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

    ValidateFilters() {
        this.ValidationErrorsList = [];    
        var FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.ToDate > new Date()) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
        }
        if (this.ToDate == null) {
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
            this.ValidationErrorsList.push(s);
        }
        if (this.FromDate == null) {
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.FromDate"));
            this.ValidationErrorsList.push(s);
        }
        if (this.ToDate < this.FromDate) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
        }
       
        return this.ValidationErrorsList;
    }


    }

