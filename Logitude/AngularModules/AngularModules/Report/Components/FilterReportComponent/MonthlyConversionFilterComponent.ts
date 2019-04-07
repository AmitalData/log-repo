import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReportsDomainService} from '../../Services/ReportsDomainService';
import {CodeNameClass} from './CodeNameClass';
import {DateTool} from '../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'MonthlyConversionFilterComponent',
    templateUrl: './MonthlyConversionFilterComponent.html',
})

export class MonthlyConversionFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[] = [];
    reportFliter: ReportFliter;
    public FilterdAdditionalService: any;
    public SelectedItem: string = "All";
    public CountryId: string = null;
    public FromDate: Date;
    public ToDate: Date;
    public OpportunityTypeId: string;
    public BusinessUnitId: string;
    public OwnerId: string;
    public IsByCreateDate: boolean = true;
    public IsStageDate: boolean = false;
    public IsCreateDate: boolean = true;
    public SelectedProdustsItem: any;
    public IsCreateDateId: string = "IsCreateDateId_";
    public IsStageDateId: string = "IsStageDateId";
    public ShipmentTypeRadio: string = "ShipmentTypeRadio_";
    private reportDoaminService: ReportsDomainService;
    public ResellerId: string = null;
    IsStageDateClicked() {
        this.IsByCreateDate = false;
    }

    fillcombo(arr: any) {
        this.FilterdAdditionalService = [];
        arr.forEach((i) => {
            if (!i.InActive) {
                var item = new CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                item.Checked = false;
                this.FilterdAdditionalService.push(i);
            }          
        });
        this.FilterdAdditionalService.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });        
    }

    IsCreateDateClicked() {
        this.IsByCreateDate = true;        
    }
    public TenantPM: TenantPM;
    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";

    public SelectedViewItem: any;



    public DataContext: MonthlyConversionFilterComponent = this;
    public IsCRMTenant: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.IsStageDateId = this.IsStageDateId+this.CurrentSession.GetNewId(this.IsStageDateId);
        this.IsCreateDateId = this.IsCreateDateId + this.CurrentSession.GetNewId(this.IsCreateDateId);
        this.ShipmentTypeRadio = this.ShipmentTypeRadio + this.CurrentSession.GetNewId(this.ShipmentTypeRadio);
        this.reportDoaminService = new ReportsDomainService();

        if (SessionLocator.Tenant == 341) {
            this.IsCRMTenant = true;
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator.TenantPM;
        var fromDate: Date = DateTool.GetCurrentDateAsUtc();
        fromDate.setDate(1);
        var toDate: Date = DateTool.GetCurrentDateAsUtc();
        var daysofmonth = this.daysInMonth(DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject);
        toDate.setDate(daysofmonth+1);
        this.FromDate = fromDate;
        this.ToDate = toDate
        this.reportDoaminService.GetLeadSourceLists(this.TenantPM.Id).subscribe((myResult: any) => {
                this.fillcombo(myResult);
        });
    }

    SelectedViewByComboList(item) {
        this.SelectedViewItem = item;
    }
    
    EditedItemSource(newSource: any) {
        this.FilterdAdditionalService = newSource;
    }

    SelectedItemChanged(item) {
        this.SelectedItem = item;
    }
    
    daysInMonth(aDate: Date) {
        aDate.setMonth(aDate.getMonth() + 1);
        aDate.setDate(0);
        return (DateTool.GetDateParts(aDate).DateObject.getDate());
    }

    RunReport(isloading: boolean) {

        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");
        }

        if (this.FromDate > this.ToDate) {
            this.ValidationErrorsList.push("From Date cannot be greater than To Date");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();
            var myAdditionalServices: string = "";

            if (this.SelectedItem == "All")
                myAdditionalServices = "All";

            else {
                this.FilterdAdditionalService.forEach((i) => {
                    if (i.Checked) {
                        myAdditionalServices += i.Id + ",";
                    }
                });
            }

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LeadSources";
            this.queryFilterItem.FieldValue = myAdditionalServices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BusinessUnitId";
            this.queryFilterItem.FieldValue = this.BusinessUnitId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DataType";
            this.queryFilterItem.FieldValue = this.OpportunityTypeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "OwnerId";
            this.queryFilterItem.FieldValue = this.OwnerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CountryId";
            this.queryFilterItem.FieldValue = this.CountryId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsByCreateDate";
            this.queryFilterItem.FieldValue = this.IsByCreateDate;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);   

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
