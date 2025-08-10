declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({

    selector: 'ExternalReconciliationLinesReportFilterControl',
    templateUrl: './ExternalReconciliationLinesReportFilterControl.html',
    inputs: ['ReportsPreview']
})

export class ExternalReconciliationLinesReportFilterControl extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Report";
    public DataContext: ExternalReconciliationLinesReportFilterControl = this;
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public ObjectTableId:string;
    public REFFromDate: Date;
    public REFToDate: Date;
    public BankAccountId:string;
    public IsScreenLoaded:boolean =false;
    constructor() {
        super();
         this._entityResourceService.getEntityResourceByTableName("LedgerTransaction", 0).subscribe((response: any) => { this.IsScreenLoaded=true;});

         this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation", 0).subscribe((response: any) => { });

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var yesterdayDate = new Date().setDate(new Date().getDate() - 1);
        this.REFFromDate = this.SetDate(Year, month, 1);
        this.REFToDate = new Date(yesterdayDate);
    }

    ngOnInit() {

    }



    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }



    public IsExternalReconciledFilter: string = 'open';
    IsExternalReconciledFilterItemClicked(itemValue: string)
    {
        
        if (this.IsExternalReconciledFilter != itemValue && !this.ExternalReconciliationNumber) {
            this.IsExternalReconciledFilter = itemValue;
        }
    }
    public IsShowCrossYear: string = 'all';
    IsShowCrossYearFilterItemClicked(itemValue: string)
    {
        
        if (this.IsShowCrossYear != itemValue ) {
            this.IsShowCrossYear = itemValue;
        }
    }

    public TypeFilter: string = 'all';
    TypeFilterItemClicked(itemValue: string)
    {
        if (this.TypeFilter != itemValue && !this.ExternalReconciliationNumber) {
            this.TypeFilter = itemValue;
            this.SetObjectTableId(itemValue);
        }
    }


    public SortBy: string = 'ReferenceDate';
    SortByFilterItemClicked(itemValue: string)
    {
        if (this.SortBy != itemValue) {
            this.SortBy = itemValue;
        }
    }

    public IncludesTransferGlaccount:boolean =true;

    private externalReconciliationNumber: number;
    public get ExternalReconciliationNumber() { return this.externalReconciliationNumber; }
    public set ExternalReconciliationNumber(value: number)
    {
        if (this.externalReconciliationNumber != value) {
            this.externalReconciliationNumber = value;

        }
    }

    SetObjectTableId(Val:string){
        switch(Val){
            case"bank":{
                this.ObjectTableId = window.ObjectTables.filter(d => d.Name === "BankAccount")[0].Id;
                break;
            }
            case"gLAccount":{
                this.ObjectTableId = window.ObjectTables.filter(d => d.Name === "GLAccount")[0].Id;
                break;
            }
            default:{
                this.ObjectTableId = null;
            }

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
                case "CrossYearReconcile":
                    this.IsShowCrossYear = queryFilterItem.FieldValue;
                    break;
                case "IncludesTransferGlaccount":
                    this.IncludesTransferGlaccount = queryFilterItem.FieldValue;
                    break;
                case "ExternalReconciliationNumber":
                    this.ExternalReconciliationNumber = queryFilterItem.FieldValue;
                    break;
                
                case "ObjectTableId":
                    this.ObjectTableId = queryFilterItem.FieldValue;
                    break;
                case "BankAccountId":
                    this.BankAccountId = queryFilterItem.FieldValue;
                     break;
                case "REFFromDate":
                    this.REFFromDate = new Date(queryFilterItem.FieldValue);
                         break;
                 case "REFToDate":
                    this.REFToDate = new Date(queryFilterItem.FieldValue);
                         break;
                case "SortBy":
                    this.SortBy = queryFilterItem.FieldValue;
                    break;
                case "Type":
                    this.TypeFilter = queryFilterItem.FieldValue;
                    break;
                case "IsExternalReconciled":
                    this.IsExternalReconciledFilter = queryFilterItem.FieldValue;
                    break;
              
            }
    
           
    
        }
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        // this.REFFromDate.setHours(0,0,0,);
        // this.REFToDate.setHours(0,0,0,);

        var FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.REFFromDate == null) {
            var REFFromDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("LedgerTransaction.O.REFFrom"));
            this.ValidationErrorsList.push(REFFromDateValidation);
        }

        if (this.REFToDate == null) {
            var REFToDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("LedgerTransaction.O.REFTo"));
            this.ValidationErrorsList.push(REFToDateValidation);
        }

        if(this.REFFromDate != null && this.REFToDate != null){
            var FromDate = new Date(this.REFFromDate.getUTCFullYear(),this.REFFromDate.getUTCMonth(),this.REFFromDate.getUTCDate(),0,0,0,0);
            var ToDate = new Date(this.REFToDate.getUTCFullYear(),this.REFToDate.getUTCMonth(),this.REFToDate.getUTCDate(),0,0,0,0);
            if (FromDate > ToDate) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
            }
        }

        return this.ValidationErrorsList.length == 0;
    }

    RunReport(isInteractive: boolean) {
        if (this.ValidateSelectedFilters()) {
           this.BuildReport(isInteractive);
        }
    }

    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItems.push(this.GetNewQueryFilterItem("Type",this.TypeFilter));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("ObjectTableId",this.ObjectTableId));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("BankAccountId",this.BankAccountId));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("SortBy",this.SortBy));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("REFFromDate",this.REFFromDate,"Date"));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("REFToDate",this.REFToDate,"Date"));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("IsExternalReconciled",this.IsExternalReconciledFilter));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("ExternalReconciliationNumber",this.ExternalReconciliationNumber,"int"));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("IncludesTransferGlaccount",this.IncludesTransferGlaccount));
        this.queryFilterItems.push(this.GetNewQueryFilterItem("CrossYearReconcile",this.IsShowCrossYear));
        return this.queryFilterItems;
    }

    BuildReport(isInteractive: boolean){
        this.GetQueryFilterItems();

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(this.reportFliter, isInteractive);
    }

    GetNewQueryFilterItem(FieldName:string,FieldValue:any,FieldDataType:string=null,Operator:string="Equals"){
             var queryFilterItem = new QueryFilterItem();
                 queryFilterItem.DisplayInList = false;
                 queryFilterItem.FieldName = FieldName;
                 queryFilterItem.FieldValue = FieldValue;
                 queryFilterItem.Operator = Operator;
                 queryFilterItem.FieldDataType = FieldDataType;

                 return queryFilterItem;
    }
}
