
import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../Filters/ReportFliter';
import { QueryFilterItem } from '../../Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { reject } from 'q';

@Component({

    templateUrl: './ARinvoiceSequencesReportFilterComponent.html',
})

export class ARinvoiceSequencesReportFilterComponent extends BaseComponent {
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;

    public CardFilterItems: ApiQueryFilters;
    public ObjectTableName: string = "Card";
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;

    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    constructor(public entityListService: EntityListService, private CD: ChangeDetectorRef) {
        super();
        this.TenantPM = SessionLocator.TenantPM;

        this.InitComponent();

    }

    private InitComponent() {
        this.GetResources();
        this.InitLOVFilters();
        // this.SetGLaccountFilterEnability();
        this.SetMonthFilterDefaults();
        //this.FillAgingMethodList();
    }

    private SetMonthFilterDefaults() {
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var day = new Date().getDate();


        this.FromDate = this.SetDate(Year, month - 1, day);
        this.ToDate = new Date();
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
    InitLOVFilters() {
        // initialize query filters for Accounts
        this.CardFilterItems = new ApiQueryFilters();
        // this.CardFilterItems.addAdditionalFilter("PartnerTypeId", "VD", null, null, "Equals", false, false, true, "string");
    }

    private GetResources() {
        this.entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => { this.isReady = true; });
    }

    ValidateDate() {
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {

            setTimeout(() => {
                if (!this.IsOldDate("ToDate"))
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                    this.errors = [];
                if (!this.IsOldDate("FromDate"))
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);

        } else {
            setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
                this.CD.detectChanges();
            }, 200);

        }
    }

    private IsOldDate(fieldName) {
        let isOldDate: boolean = false;
        const uiProperty = this.UIProperties.UIPropertyList.filter(uiProp => uiProp.FieldName == fieldName)[0];
        if (uiProperty)
            isOldDate = uiProperty.ValidationError == "Date time is too way in the past!" || uiProperty.ValidationError == "Invalid Date";

        return isOldDate;
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
    private SetFilterItem(queryFilterItem: QueryFilterItem) {

        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FromDate":
                     this.FromDate = new Date(queryFilterItem.FieldValue) ;
                     break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue) ;
                    break;        
                  
            }

           
    
        }
    }
    public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {
        if (this.isReady) {
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
        }
    }
    ValidateSelectedFilters() {
        this.errors = [];
        this.ValidationErrorsList = [];
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {
            this.errors.push(TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
        }
        return this.errors.length == 0;
    }
    //#endregion
    private errors: string[] = [];
    RunButtonClicked(isInteractive: boolean) {

       
        if (this.ValidateSelectedFilters()) {

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            myReportFliter.IsInteractive = isInteractive;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = this.errors;
        }
    }
    GetQueryFilterItems(){
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));
         myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate, "Date"));
        return myFilterItems;
    }
     
    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
        }
    }

    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();

        }
    }

    FilterChanged() {

        //this.Customer = null;
        //this.Salesman = null;
        //this.Collector = null;
        //this.ChartOfAccountsId_Dummy = null;
        //this.UIProperties.SetValidity("ChartOfAccountsId", "GLAccount", true, "");
        //this.UIProperties.SetRequired("ChartOfAccountsId", "GLAccount", false);


        //switch (this.filterSelectedValue) {
        //    case 'filter_customer':
        //        this.AccountTypeCode = '2';
        //        this.ChartOfAccountsTypeCode = '3';
        //        break;
        //    case 'filter_vendor':
        //        this.AccountTypeCode = '3';
        //        this.ChartOfAccountsTypeCode = '4';
        //        break;
        //    default:
        //        break;


        //this.SetUIProperties();
        //this.ValidateDate();

    }

}




