

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'IATAStatisticsFilterComponent',
    templateUrl: './IATAStatisticsFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class IATAStatisticsFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;



 
    MainCarriageCarrierId: string;
    reportFliter: ReportFliter;
    ToDate: Date;
    public HeightControl: string;
    FromDate: Date;

    public myForm: FormGroup;
    queryFilterItems: QueryFilterItem[];

    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: IATAStatisticsFilterComponent = this;
    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    }

    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(70);
        //}
        //else {
        
        //    var month = new Date().getMonth();
        //    var Year = new Date().getFullYear();
        //    var daysofmonth = this.daysInMonth(new Date());
        //    this.FromDate = this.SetDate(Year, month - 1, 1);
        //    this.ToDate = this.SetDate(Year, month, daysofmonth);
        //    this.RunReport(false);
        //}
    }



    daysInMonth(aDate: Date) {
   
        return (new Date(aDate.getFullYear(), aDate.getMonth()+1, 0)).getDate();
    }



    RunReport(isloading: boolean) {


        this.queryFilterItems = new Array<QueryFilterItem>();
        
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);


            if (this.MainCarriageCarrierId) {

                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "MainCarriageCarrierId";
                this.queryFilterItem.FieldValue = this.MainCarriageCarrierId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
           }
        

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;


            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";


            this.reportFliter.IncludeOperationalyClosed = false;


            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                this.ReportsPreview.AddPartner("Airline", this.MainCarriageCarrierId);
            }


            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        
        
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
    


}