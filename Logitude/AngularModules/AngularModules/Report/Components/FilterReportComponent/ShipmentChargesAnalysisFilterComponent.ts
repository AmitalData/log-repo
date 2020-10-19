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
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {AppTool, DateTool, DateParts} from '../../../Infrastructure/Tools';

@Component({
    
    selector: 'ShipmentChargesAnalysisFilterComponent',
    templateUrl: './ShipmentChargesAnalysisFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class ShipmentChargesAnalysisFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: ShipmentChargesAnalysisFilterComponent = this;

    public CustomerId: string;    
    public FromDate: Date;
    public ToDate: Date;    
    public ProfitCurrencyCode: string;
    public LocalCurrencyCode: string;
    constructor() {
        super();        
    }

    ngOnInit() {
        
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);

        this.ProfitCurrencyCode = SessionLocator.TenantPM.ProfitCurrencyCode;
        this.LocalCurrencyCode = SessionLocator.TenantPM.AccountingCurrencyCode;
        this.SelectedCurrencyCode = this.LocalCurrencyCode;

        this.SelectedOperationalCode = "All";
        this.SelectedAccountingCode = "All";

        this.BuildFunnelFilters();
        this.FillFiltersList();
        this.SetFiltersEnabled();
    }

    private DaysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    }

    private SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }

    public DateFilterList: CodeNameClass[];
    private BuildFunnelFilters() {
        this.DateFilterList = [];
        this.DateFilterList.push(new CodeNameClass("CRT", "Create Date"));
        this.DateFilterList.push(new CodeNameClass("ARR", "Actual Arrival Date"));
        this.DateFilterList.push(new CodeNameClass("DEP", "Actual Departure Date"));
        this.DateFilterList.push(new CodeNameClass("OPE", "Operational Date"));
        this.DateFilterList.push(new CodeNameClass("FOPC", "First Operational Close Date"));
        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == "CRT")[0];
    }

    private selectedDateFilter: CodeNameClass;
    get SelectedDateFilter() { return this.selectedDateFilter; }
    set SelectedDateFilter(value: CodeNameClass) {
        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;
        }
    }

    public FiltersComboList: CodeNameClass[];
    private FillFiltersList() {
        this.FiltersComboList = [];
        this.FiltersComboList.push(new CodeNameClass("NOFI", "No Filter"));
        this.FiltersComboList.push(new CodeNameClass("OPEN", "Open Amount"));
        this.FiltersComboList.push(new CodeNameClass("ACCT", "Accounted Amount"));
        this.FiltersComboList.push(new CodeNameClass("OPAT", "Open or Accounted"));
        this.FiltersComboList.push(new CodeNameClass("MISS", "Missing"));

        this.receivablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
        this.payablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
    }

    public ReceivablesFilterEnabled: boolean = true;
    public PayablesFilterEnabled: boolean = true;
    private SetFiltersEnabled() {
        var receivablesIsEnabled: boolean = true;
        var payablesIsEnabled: boolean = true;

        if (AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            receivablesIsEnabled = false;
            payablesIsEnabled = false;
        }

        else {
            if (this.PayablesFilterSelectedItem != null) {
                if (this.PayablesFilterSelectedItem.Code == "MISS") {
                    receivablesIsEnabled = false;
                }
            }

            if (this.ReceivablesFilterSelectedItem != null) {
                if (this.ReceivablesFilterSelectedItem.Code == "MISS") {
                    payablesIsEnabled = false;
                }
            }
        }
        
        this.ReceivablesFilterEnabled = receivablesIsEnabled;
        this.PayablesFilterEnabled = payablesIsEnabled;
    }

    private RefereshFiltersValues() {
        if (AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ReceivablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
            this.PayablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
        }

        else {
            if (this.ReceivablesFilterSelectedItem != null) {
                if (this.ReceivablesFilterSelectedItem.Code == "MISS") {
                    this.PayablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
                }
            }

            else if (this.PayablesFilterSelectedItem != null) {
                if (this.PayablesFilterSelectedItem.Code == "MISS") {
                    this.ReceivablesFilterSelectedItem = this.FiltersComboList.filter(d => d.Code == "NOFI")[0];
                }
            }
        }
    }

    private receivablesFilterSelectedItem: CodeNameClass;
    get ReceivablesFilterSelectedItem() { return this.receivablesFilterSelectedItem; }
    set ReceivablesFilterSelectedItem(value: CodeNameClass) {
        if (this.receivablesFilterSelectedItem != value) {
            this.receivablesFilterSelectedItem = value;

            this.SetFiltersEnabled();
            this.RefereshFiltersValues();
        }
    }

    private payablesFilterSelectedItem: CodeNameClass;
    get PayablesFilterSelectedItem() { return this.payablesFilterSelectedItem; }
    set PayablesFilterSelectedItem(value: CodeNameClass) {
        if (this.payablesFilterSelectedItem != value) {
            this.payablesFilterSelectedItem = value;

            this.SetFiltersEnabled();
            this.RefereshFiltersValues();
        }
    }

    private chargesTypeId: string;
    public get ChargesTypeId() { return this.chargesTypeId; }
    public set ChargesTypeId(value: string) {
        if (this.chargesTypeId != value) {
            this.chargesTypeId = value;

            this.SetFiltersEnabled();
            this.RefereshFiltersValues();
        }
    }

    public SelectedCurrencyCode: string = null;
    OnSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;
    }
    
    private selectedOperationalCode: string = "All";
    public get SelectedOperationalCode() { return this.selectedOperationalCode; }
    public set SelectedOperationalCode(value: string) {
        if (this.selectedOperationalCode != value) {
            this.selectedOperationalCode = value;
            this.ApplySelectedStyle("OPE");
        }
    }

    OnOperationalClicked(myCode: string) {
        this.SelectedOperationalCode = myCode;
    }
    
    private selectedAccountingCode: string = "All";
    public get SelectedAccountingCode() { return this.selectedAccountingCode; }
    public set SelectedAccountingCode(value: string) {
        if (this.selectedAccountingCode != value) {
            this.selectedAccountingCode = value;
            this.ApplySelectedStyle("ACC");
        }
    }

    OnAccountingClicked(myCode: string) {
        this.SelectedAccountingCode = myCode;
    }
    
    queryFilterItems: QueryFilterItem[];    
    queryFilterItem: QueryFilterItem;    
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];

        if (!this.SelectedDateFilter) {
            this.ValidationErrorsList.push("Date field is required");
        }

        if (!this.ReceivablesFilterSelectedItem) {
            this.ValidationErrorsList.push("Receivables field is required");
        }

        if (!this.PayablesFilterSelectedItem) {
            this.ValidationErrorsList.push("Payables field is required");
        }


        if (this.FromDate != null && this.ToDate != null) {
            if (this.ToDate < this.FromDate) {
                this.ValidationErrorsList.push("From date must be less than to date");
            }

            else {
                var total = 0;

                if (((this.ToDate.valueOf() - this.FromDate.valueOf()) / (1000 * 60 * 60 * 24) )>365)
                    this.ValidationErrorsList.push("Dates should be within one year");
                }
            }        

        else {
            if (this.FromDate == null) {
                this.ValidationErrorsList.push("From Date is required");                
            }

            if (this.ToDate == null) {
                this.ValidationErrorsList.push("To Date is required");
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ChargesTypeId";
            this.queryFilterItem.FieldValue = this.ChargesTypeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DateType";
            this.queryFilterItem.FieldValue = this.SelectedDateFilter.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            var IsProfitCurrency = false;
            if (this.SelectedCurrencyCode == this.ProfitCurrencyCode) {
                IsProfitCurrency = true;
            }

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsProfitCurrecny";
            this.queryFilterItem.FieldValue = IsProfitCurrency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CurrencyCode";
            this.queryFilterItem.FieldValue = this.SelectedCurrencyCode;
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
            this.queryFilterItem.FieldName = "OperationalType";
            this.queryFilterItem.FieldValue = this.SelectedOperationalCode;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AccountingType";
            this.queryFilterItem.FieldValue = this.SelectedAccountingCode;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ReceivablesType";
            this.queryFilterItem.FieldValue = this.ReceivablesFilterSelectedItem.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "PayablesType";
            this.queryFilterItem.FieldValue = this.PayablesFilterSelectedItem.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            
            var reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            
            this.ReportsPreview.CleanPartnersObslist();
            //if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            this.ReportsPreview.GenerateReport(reportFliter, isloading);
        }
    }

    public FilterId_OO: string = "operational_open";
    public FilterId_OC: string = "operational_close";
    OperationalMouseOver(itemValue: string) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);

            switch (itemValue) {
                case "All": {
                    
                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    }
    OperationalMouseLeave(itemValue: string) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);

            switch (itemValue) {
                case "All": {
                    
                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    }

    public FilterId_AO: string = "accounting_open";
    public FilterId_AC: string = "accounting_close";
    AccountingMouseOver(itemValue: string) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    }
    AccountingMouseLeave(itemValue: string) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    }
    
    ApplySelectedStyle(mode: string) {
        var img_O;
        var img_C;

        switch (mode) {
            case "OPE": {
                img_O = document.getElementById(this.FilterId_OO);
                img_C = document.getElementById(this.FilterId_OC);

                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");

                switch (this.SelectedOperationalCode) {
                    case "All": {
                        
                        break;
                    }

                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }

                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }

                break;
            }

            case "ACC": {
                img_O = document.getElementById(this.FilterId_AO);
                img_C = document.getElementById(this.FilterId_AC);

                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");

                switch (this.SelectedAccountingCode) {
                    case "All": {

                        break;
                    }

                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }

                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }
                break;
            }
        }
    }
}
