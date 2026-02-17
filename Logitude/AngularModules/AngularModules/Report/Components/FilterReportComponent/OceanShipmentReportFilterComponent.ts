import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,
    selector: 'OceanShipmentReportFilterComponent',
    templateUrl: './OceanShipmentReportFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class OceanShipmentReportFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: OceanShipmentReportFilterComponent = this;
    constructor() {
        super();
        this.ShipmentTypeRadioId = Guid.newGuid();
    }
    
    public ShipmentTypeRadioId: string = "";
    public IsDomestic: boolean = false;
    public IsExport: boolean = false;
    public IsImport: boolean = false;
    public IncludeClosed: boolean = false;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    public currentDirectionId: string;
    public SelectedCurrency: string = "USD,profit";  
    public InActive: boolean = false;

    public ShipmentTypeRadio: string = "All";
    SetShipmentTypeRadio(value: string) {
        if (this.ShipmentTypeRadio != value) {
            this.ShipmentTypeRadio = value;
        }
    }
    
    public get CurrentDirectionId() {
        var s: string = "";
        if (this.IsImport) {
            s = s + "I";
        }

        if (this.IsExport) {
            s = s + ",E";
        }

        if (this.IsDomestic) {
            s = s + ",D"
        }

        return s;
    }
    
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    }

    ChangeCurrency(code: string) {

        if (code != this.SelectedCurrency) {
            this.SelectedCurrency = code;
        }

    }
    
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    RunReport(isloading: boolean) {
        this.queryFilterItems = new Array<QueryFilterItem>();

        if (this.FromDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.ToDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.ShipmentTypeRadio != null) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipmentTypeId";
            this.queryFilterItem.FieldValue = this.ShipmentTypeRadio == "All" ? "" : this.ShipmentTypeRadio;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.CurrentDirectionId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.CurrentDirectionId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }       

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.CurrentCurrencyCodeType = this.SelectedCurrency;

        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.reportFliter.CustomerId = this.CustomerId;

        this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;

        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);

        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);
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
