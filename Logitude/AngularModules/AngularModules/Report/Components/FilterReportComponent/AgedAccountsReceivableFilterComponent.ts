import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'AgedAccountsReceivableFilterComponent',
    templateUrl: './AgedAccountsReceivableFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class AgedAccountsReceivableFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;

    public IsAr: boolean = false;
    public IsAp: boolean = false;
    public IsAll: boolean = true;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;

    settingShipmentTypeCode(code) {
        this.ShipmentTypeRadio = code;
    }

    public shipmentTypeRadio: string = "All";
    public set ShipmentTypeRadio(code) {
        this.shipmentTypeRadio = code;
    }

    public get ShipmentTypeRadio() {
        return this.shipmentTypeRadio;
    }
    
    public get InvoiceType() {
        return this.ShipmentTypeRadio;
    }

    allClicked() {
        this.IsAll = true;
        this.IsAp = false;
        this.IsAr = false;
    }

    IsApClicked() {
        this.IsAll = false;
        this.IsAp = true;
        this.IsAr = false;
    }

    IsArClicked() {
        this.IsAll = false;
        this.IsAp = false;
        this.IsAr = true;
    }

public selectedCurrency: string = this.LocalCurrencyCode;
    public get SelectedCurrency() {
        return this.selectedCurrency;
    }
    public set SelectedCurrency(value: string) {
        this.selectedCurrency = value;
    }

    queryFilterItems: QueryFilterItem[];    
    queryFilterItem: QueryFilterItem;

    public ObjectTableName: string = "Report";
    public DataContext: AgedAccountsReceivableFilterComponent = this;
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        //this.RunReport(false);
    }
        
    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(65);
        //}
        //else {
        //}

        //this.RunReport(false);
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
                case "InvoiceType":
                    this.ShipmentTypeRadio = queryFilterItem.FieldValue;
                    break;
                case "CurrencyType":
                    this.SelectedCurrency =queryFilterItem.FieldValue;
                    break;
                
               
            }
   
           
    
        }
    }

    ValidateSelectedFilters() {
         return true;
    }
    
    RunReport(isloading: boolean) {
        
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
    GetQueryFilterItems() {
        this.queryFilterItems = new Array<QueryFilterItem>();

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "InvoiceType";
        this.queryFilterItem.FieldValue = this.InvoiceType;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        var currencyType: string;
        if (this.SelectedCurrency == this.LocalCurrencyCode)
            currencyType = "local";
        else
            currencyType = "profit";

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CurrencyType";
        this.queryFilterItem.FieldValue = currencyType;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
              
        return this.queryFilterItems;
    }
}
