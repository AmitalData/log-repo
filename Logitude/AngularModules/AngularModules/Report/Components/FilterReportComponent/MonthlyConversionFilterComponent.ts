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
    public SelectedProdustsItem: any;   
    private reportDoaminService: ReportsDomainService;
    public ResellerId: string = null;
    public IncludeCancelled: boolean = false;
    AdditionalServiceSelectedValue:string

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
        this.FilterdAdditionalService.forEach((item: any) => {
            item.Checked = this.SelectedItem == "NotAll" && this.AdditionalServiceSelectedValue.split(',').some(selectedItem =>  selectedItem === item.Code||selectedItem === item.Id);
        });
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
    public IsSchedulerReport: boolean = false;  
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) {
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
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
               
                case "ToDate":
                    this.ToDate =new Date(queryFilterItem.FieldValue);
                    break;                          
                case "BusinessUnitId":
                    this.BusinessUnitId =   queryFilterItem.FieldValue;
                        break; 
                case "DataType":
                    this.OpportunityTypeId = queryFilterItem.FieldValue;
                    break;
                case "OwnerId":
                    this.OwnerId = queryFilterItem.FieldValue;
                    break;
                case "CountryId":
                    this.CountryId = queryFilterItem.FieldValue;
                    break;
                case "LeadSources":{
                    this.SelectedItem =  queryFilterItem.FieldValue=="All"?"All":"NotAll"; 
                    this.AdditionalServiceSelectedValue = queryFilterItem.FieldValue;
                    break;
                }
                case "IncludeCancelled":
                    this.IncludeCancelled = queryFilterItem.FieldValue;
                    break;
                case "StageCount":
                    this.StageCountRadio = queryFilterItem.FieldValue;
                    break;
                   
                        
                        
            }
    
        }
    }
    ValidateSelectedFilters() {
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

        return this.ValidationErrorsList.length == 0;
    }
    RunReport(isloading: boolean) {

      

        if (this.ValidateSelectedFilters()) {
            
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";            
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
    GetQueryFilterItems(){
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

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "IncludeCancelled";
        this.queryFilterItem.FieldValue = this.IncludeCancelled;
        this.queryFilterItem.Operator = "Equals";
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

      if (this.StageCountRadio != null) {
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "StageCount";
        this.queryFilterItem.FieldValue = this.StageCountRadio;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
      }
        return this.queryFilterItems;
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

  public StageCountRadio: string = "Actual";
  SetStageCountRadio(value: string) {
    if (this.StageCountRadio != value) {
      this.StageCountRadio = value;
    }
  }
}
