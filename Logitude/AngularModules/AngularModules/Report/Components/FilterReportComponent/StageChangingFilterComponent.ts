import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReportsDomainService} from '../../Services/ReportsDomainService';
import {CodeNameClass} from './CodeNameClass';


@Component({
    moduleId: module.id,
    selector: 'StageChangingFilterComponent',
    templateUrl: './StageChangingFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StageChangingFilterComponent extends BaseComponent  {
    public ReportsPreview: ReportsPreviewComponent;
    private reportDomainService: ReportsDomainService;
    reportFliter: ReportFliter;
    public FilterdAdditionalService: any;
    public CountryId: string = null;
    public FromDate: Date;
    public ToDate: Date;
    public OpportunityTypeId: string;
    public BusinessUnitId: string;
    public ShipmentTypeRadio: string = "ShipmentTypeRadio_";
    public OwnerId: string;
    public IsOpportunitesDate: boolean = true;
    public IsStageDate: boolean = false;
    public SelectedProdustsItem: any;
    public ValidationErrorsList: string[] = [];
    public SelectedItemChanged(item) {
        this.SelectedProdustsItem = item;
    }
    IsOpportunitesDateClicked() {
        this.IsStageDate = false;
        this.IsOpportunitesDate = true;
    }
    IsStageDateClicked() {
        this.IsStageDate = true;
        this.IsOpportunitesDate = false;
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
            else {
            }
        });
        this.FilterdAdditionalService.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });

    }
    public TenantPM: TenantPM;
    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public IsOpportunitesDateId: string = "IsOpportunitesDate_";
    public SelectedViewItem: any;    
    public IsStageDateId: string = "IsStageDateId_";
    public DataContext: StageChangingFilterComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ShipmentTypeRadio += this.ShipmentTypeRadio + this.CurrentSession.GetNewId(this.ShipmentTypeRadio);
        this.IsOpportunitesDateId += this.IsOpportunitesDateId + this.CurrentSession.GetNewId(this.IsOpportunitesDateId);
        this.IsStageDateId += this.IsStageDateId + this.CurrentSession.GetNewId(this.IsStageDateId);
        this.reportDomainService = new ReportsDomainService();
        
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        this.TenantPM = SessionLocator.TenantPM;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = new Date(Year, month - 1, 2);
        this.ToDate = new Date(Year, month, daysofmonth + 1);


        this.reportDomainService.GetLeadSourceLists(this.TenantPM.Id).subscribe((myResult: any) => {
            this.fillcombo(myResult);
        });
    }

    SelectedViewByComboList(item) {

        this.SelectedViewItem = item;

    }
    
    EditedItemSource(newSource: any) {

        this.FilterdAdditionalService = newSource;

    }
    
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
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
            this.FilterdAdditionalService.forEach((i) => {
                if (i.Checked) {
                    myAdditionalServices += i.Id + ",";
                }
            });
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LeadSources";
            this.queryFilterItem.FieldValue = myAdditionalServices;
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
            this.queryFilterItem.FieldName = "IsByStageDate";
            this.queryFilterItem.FieldValue = this.IsStageDate;
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
