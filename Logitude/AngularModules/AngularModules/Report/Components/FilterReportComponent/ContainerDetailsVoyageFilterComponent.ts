import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {CodeNameClass} from './CodeNameClass';

@Component({
    moduleId: module.id,
    selector: 'ContainerDetailsVoyageFilterComponent',
    templateUrl: './ContainerDetailsVoyageFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class ContainerDetailsVoyageFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;

    private mySelectedDirectionFilter: string = "All";
    public get MySelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    public ValidationErrorsList: string[];

    public set MySelectedDirectionFilter(newValue: string) {
        if (newValue == "All") {
            newValue = null;
        }
        if (this.mySelectedDirectionFilter != newValue) {

            this.mySelectedDirectionFilter = newValue;
        }
    }
    public CustomerId: string;
    public VoyageNumber: string;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    FromDateActual: Date;
    ToDateActual: Date;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";

    public DataContext: ContainerDetailsVoyageFilterComponent = this;
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
    }

    ngOnInit() {
        
    }
    
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() , 0)).getDate();
    }
    
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];
        if (this.MySelectedDirectionFilter == "All")
            this.MySelectedDirectionFilter = null;


        if (this.FromDate == null && this.ToDate == null && this.FromDateActual == null && this.ToDateActual == null) {
            this.ValidationErrorsList.push("At lease one date type range is mandatory");
        }

        else {
            if (this.FromDate != null && this.ToDate != null) {
                if (this.ToDate < this.FromDate) {
                    this.ValidationErrorsList.push("From date must be less than to date");
                }
            }


            if (this.FromDateActual != null && this.ToDateActual != null) {
                if (this.ToDateActual < this.FromDateActual) {
                    this.ValidationErrorsList.push("From date must be less than to date");
                }
            }
        }
        
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            if (this.CustomerId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CustomerId";
                this.queryFilterItem.FieldValue = this.CustomerId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.mySelectedDirectionFilter) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "DirectionId";
                this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.VoyageNumber) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "VoyageNumber";
                this.queryFilterItem.FieldValue = this.VoyageNumber;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.FromDate) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateFromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.ToDate) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateToDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.FromDateActual) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalingFromDate";
                this.queryFilterItem.FieldValue = this.FromDateActual;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.ToDateActual) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalingToDate";
                this.queryFilterItem.FieldValue = this.ToDateActual;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
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
            
            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            }

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
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