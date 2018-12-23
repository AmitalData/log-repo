import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component}  from '@angular/core';
import {CodeNameClass} from './CodeNameClass';

@Component({
    moduleId: module.id,
    selector: 'ExpectedIncomeFilterComponent',
    templateUrl: './ExpectedIncomeFilterComponent.html',
})

export class ExpectedIncomeFilterComponent extends BaseComponent  {
    public ReportsPreview: ReportsPreviewComponent;
    public queryFilterItems: QueryFilterItem[];
    public queryFilterItem: QueryFilterItem;    
    public reportFliter: ReportFliter;
    public DataContext: ExpectedIncomeFilterComponent = this;
    private mySelectedDirectionFilter: string = "All";
    public IsCRMTenant: boolean = false;
    ToDate: Date;
    FromDate: Date;
    public RecurringComboList: CodeNameClass[] = [];
    constructor() {
        super();

        if (SessionLocator.Tenant == 341) {
            this.IsCRMTenant = true;
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        //this.FromDate = this.SetDate(Year, month - 1, 1);
        //this.ToDate = this.SetDate(Year, month - 1, daysofmonth);

        this.BuildRecurringComboList();
    }

    BuildRecurringComboList() {
        this.RecurringComboList = [];
        this.RecurringComboList.push(new CodeNameClass("A", "All"));
        this.RecurringComboList.push(new CodeNameClass("Y", "Yes"));
        this.RecurringComboList.push(new CodeNameClass("N", "No"));

        this.selectedItemComboBox = this.RecurringComboList.filter(d => d.Code == "A")[0];
    }

    private selectedItemComboBox: CodeNameClass;
    get SelectedItemComboBox() { return this.selectedItemComboBox; }
    set SelectedItemComboBox(value: CodeNameClass) {
        if (this.selectedItemComboBox != value) {
            this.selectedItemComboBox = value;            
        }
    }

    public SalesmanId: string;
    public PaymentChannelCode: string = null;
    public CountryId: string = null;
    public ResellerId: string = null;
    public ValidationErrorsList: string[];

    RunReport() {
        this.queryFilterItems = new Array<QueryFilterItem>();


        this.ValidationErrorsList = [];


        if (!this.SelectedItemComboBox) {
            this.ValidationErrorsList.push("Recurring field is required");
        }

        if (this.ValidationErrorsList.length == 0) {

            if (this.FromDate != null) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "FromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.ToDate != null) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ToDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "Recurring";
            this.queryFilterItem.FieldValue = this.SelectedItemComboBox.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            if (this.SalesmanId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalesmanId";
                this.queryFilterItem.FieldValue = this.SalesmanId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.PaymentChannelCode) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "PaymentChannelCode";
                this.queryFilterItem.FieldValue = this.PaymentChannelCode;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);

            }

            if (this.CountryId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CountryId";
                this.queryFilterItem.FieldValue = this.CountryId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.IsCRMTenant) {
                if (this.ResellerId) {
                    this.queryFilterItem = new QueryFilterItem();
                    this.queryFilterItem.DisplayInList = false;
                    this.queryFilterItem.FieldName = "ResellerId";
                    this.queryFilterItem.FieldValue = this.ResellerId;
                    this.queryFilterItem.Operator = "Equals";
                    this.queryFilterItems.push(this.queryFilterItem);
                }
            }

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

    private DaysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    }
}
