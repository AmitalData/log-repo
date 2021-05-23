declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {ReportFliter} from '../Filters/ReportFliter';
import {QueryFilterItem} from '../Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    selector: 'UserDefinedReportFilterControl',
    templateUrl: './UserDefinedReportFilterControl.html',
    inputs: ['ReportsPreview']
})

export class UserDefinedReportFilterControl extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Report";
    public DataContext: UserDefinedReportFilterControl = this;
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public ObjectTableId:string;
    public SecoundPeriodDateFrom: Date;
    public SecoundPeriodDateTo: Date;
    public FirstPeriodDateFrom: Date;
    public FirstPeriodDateTo: Date;
    public UserDefinedReportId:string;
    public IncludeAnOpeningBalance:boolean =true;
    public ExpandChartOfAccountToGLAccounts:boolean =false;
    public IsScreenLoaded: boolean = false;
    public UserDefinedReportFilterItems: ApiQueryFilters = new ApiQueryFilters();
    constructor() {
        super();
      
         this._entityResourceService.getEntityResourceByTableName("UserDefinedReport", 0).subscribe((response: any) => { this.IsScreenLoaded=true; });
        this.ExcludeCancelledReports();

    }
    ExcludeCancelledReports() {
        this.UserDefinedReportFilterItems.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

    }
    ngOnInit(): void {
        throw new Error('Method not implemented.');
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }
 
  
 
   
    RunReport() {      
        this.CheckIsReportFiltersValid();
        if (this.ValidationErrorsList.length == 0) {
           this.BuildReport();
        }
    }


    CheckIsReportFiltersValid(){
        this.ValidationErrorsList = [];
        var FieldIsRequiredText = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidatePeriodsDateIsNotNull();
        this.ValidateFromForFirstPeriodsDate(FieldIsRequiredText);
        this.ValidateFromForSecoundPeriodsDate(FieldIsRequiredText);
        this.ValidateToForFirstPeriodsDate(FieldIsRequiredText);
        this.ValidateToForSecoundPeriodsDate(FieldIsRequiredText);
        this.ValidateReportIsNotNull(FieldIsRequiredText);
        this.ValidateFirstPeriodDateFromLessorEqualDateTo();
        this.ValidateSecounddPeriodDateFromLessorEqualDateTo();     
       }

   ValidateFromForFirstPeriodsDate(fieldIsRequiredText:string){
        if (!this.FirstPeriodDateFrom  && this.FirstPeriodDateTo) {
                var FirstPeriodDateFromValidation: string = TextCodeTranslator.Translate("UserDefinedReport.O.Period1") +" "+
                fieldIsRequiredText.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.FromDate"));
                this.ValidationErrorsList.push(FirstPeriodDateFromValidation);
            }
 
    }

    ValidateFromForSecoundPeriodsDate(fieldIsRequiredText:string){
        if (!this.SecoundPeriodDateFrom  && this.SecoundPeriodDateTo) {
                var SecoundPeriodDateFromValidation: string = TextCodeTranslator.Translate("UserDefinedReport.O.Period2") +" "+
                fieldIsRequiredText.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.FromDate"));
                this.ValidationErrorsList.push(SecoundPeriodDateFromValidation);
            }
    }
    
    ValidateToForSecoundPeriodsDate(fieldIsRequiredText:string){
       if (!this.SecoundPeriodDateTo  && this.SecoundPeriodDateFrom) {
            var REFFromDateValidation: string = TextCodeTranslator.Translate("UserDefinedReport.O.Period2") +" "+
            fieldIsRequiredText.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
            this.ValidationErrorsList.push(REFFromDateValidation);
        }
            
    }

    ValidateToForFirstPeriodsDate(fieldIsRequiredText:string){
        if (!this.FirstPeriodDateTo  && this.FirstPeriodDateFrom) {
             var FirstPeriodDateToValidation: string =  TextCodeTranslator.Translate("UserDefinedReport.O.Period1") +" "+
             fieldIsRequiredText.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
             this.ValidationErrorsList.push(FirstPeriodDateToValidation);
         }
             
     }
     ValidatePeriodsDateIsNotNull(){
        if ((!this.SecoundPeriodDateFrom && !this.SecoundPeriodDateTo) &&  
             (!this.FirstPeriodDateTo  && !this.FirstPeriodDateFrom)) {
             var PeriodsDateValidation: string =  TextCodeTranslator.Translate("UserDefinedReport.O.AtLeastOnePeriodIsRequired");
             this.ValidationErrorsList.push(PeriodsDateValidation);
         }
             
     }

    ValidateFirstPeriodDateFromLessorEqualDateTo(){
        if(this.FirstPeriodDateFrom != null && this.FirstPeriodDateTo != null){
            var FromDate = new Date(this.FirstPeriodDateFrom.getUTCFullYear(),this.FirstPeriodDateFrom.getUTCMonth(),this.FirstPeriodDateFrom.getUTCDate(),0,0,0,0);
            var ToDate = new Date(this.FirstPeriodDateTo.getUTCFullYear(),this.FirstPeriodDateTo.getUTCMonth(),this.FirstPeriodDateTo.getUTCDate(),0,0,0,0);
            if (FromDate > ToDate) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("UserDefinedReport.O.Period1") +" "+TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
            }
        }
    }

    ValidateReportIsNotNull(fieldIsRequiredText:string){
        if(!this.UserDefinedReportId){
            var FirstPeriodDateToValidation: string = fieldIsRequiredText.replace("%FieldName", TextCodeTranslator.Translate("UserDefinedReport.O.Report"));
             this.ValidationErrorsList.push(FirstPeriodDateToValidation);
        }
    }

    ValidateSecounddPeriodDateFromLessorEqualDateTo(){
        if(this.SecoundPeriodDateFrom != null && this.SecoundPeriodDateTo != null){
            var FromDate = new Date(this.SecoundPeriodDateFrom.getUTCFullYear(),this.SecoundPeriodDateFrom.getUTCMonth(),this.SecoundPeriodDateFrom.getUTCDate(),0,0,0,0);
            var ToDate = new Date(this.SecoundPeriodDateTo.getUTCFullYear(),this.SecoundPeriodDateTo.getUTCMonth(),this.SecoundPeriodDateTo.getUTCDate(),0,0,0,0);
            if (FromDate > ToDate) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("UserDefinedReport.O.Period2") +" "+TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
            }
        }
    }

    InitilaizeFilter(){
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItems.push(this.GetNewQueryFilterItem("UserDefinedReportId",this.UserDefinedReportId));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("SecoundPeriodDateFrom",this.SecoundPeriodDateFrom));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("SecoundPeriodDateTo",this.SecoundPeriodDateTo));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("FirstPeriodDateFrom",this.FirstPeriodDateFrom,"Date"));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("FirstPeriodDateTo",this.FirstPeriodDateTo,"Date"));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("IncludeAnOpeningBalance",this.IncludeAnOpeningBalance));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("ExpandChartOfAccountToGLAccounts",this.ExpandChartOfAccountToGLAccounts));
    }

    BuildReport(){
        this.InitilaizeFilter();
        this.SetReportFilters();
        this.ReportsPreview.GenerateReport(this.reportFliter, true);
    }

    SetReportFilters(){
        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
    }

    GetNewQueryFilterItem(FieldName:string,FieldValue:any,FieldDataType:string=null){
             var queryFilterItem = new QueryFilterItem();
                 queryFilterItem.DisplayInList = false;
                 queryFilterItem.FieldName = FieldName;
                 queryFilterItem.FieldValue = FieldValue;
                 queryFilterItem.Operator = "Equals";
                 queryFilterItem.FieldDataType = FieldDataType;
                 return queryFilterItem;
    }
}
