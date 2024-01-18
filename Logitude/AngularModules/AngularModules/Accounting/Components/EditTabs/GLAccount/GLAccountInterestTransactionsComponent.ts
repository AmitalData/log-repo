import { Component, OnInit, Output, EventEmitter, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { GLAccountPM } from '../../../EntityPMs/GLAccountPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { FullAccountingSettingList } from '../../../../Accounting/EntityLists/FullAccountingSettingList';
import { InterestReportList } from 'Accounting/EntityLists/InterestReportList';
import { InterestReportListService } from 'Accounting/Services/StandardLists/InterestReportListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

const InterestTransactionCode = "GLIT";
const maxInterestReportsFilterCount = 1000;
const SearchBoxDelayTime = 700;
@Component({

    templateUrl: './GLAccountInterestTransactionsComponent.html',
    styleUrls: ['./GLAccountInterestTransactionsComponent.css']
})

export class GLAccountInterestTransactionsComponent extends BaseComponent implements OnInit, AfterViewInit
{

    @Output() QueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeEvent = new EventEmitter();
    public glaccountPM: GLAccountPM = null;
    public ObjectTableName = "GLAccount";
    public DataContext = this;
    public filterAgrs: ApiQueryFilters;
    public dateFilter: FilterItem;
    public searchFieldFilter: FilterItem;
    public isControlAccount: boolean = false;
    public isRTL: boolean = false;
    InterestReports: any[] = [];

    private CurrentSession = SessionLocator.SelectedSession;

    // Services
    private entityListService: EntityListService = new EntityListService();
    public excelService: LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();
    private interestReportService: InterestReportListService = new InterestReportListService();

    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef)
    {
        super();

        this.SetTabArguments();

        this.LoadDefaultValues();

        this.LoadGridData();

        this.UIProperties.SetEnabled("CurrencyId", "GLAccount", this.glaccountPM.IsMultiCurrency);

        this.ListenEvents();

    }

    ngOnInit()
    {
        this.BuildColumns();
    }

    ngAfterViewInit() { }

    SetTabArguments()
    {
        this.SetRTL();
        this.SetTabGLAccountData();
        this.SetDefaultDatesFilter();

        // this.CD.detectChanges();
    }
    LoadDefaultValues()
    {
        this.GetInterestReports();
    }
    ListenEvents()
    {
        if (this.CurrentSession.CurrentEditComponent != null) {

            var _CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) =>
                {
                    if (_CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == InterestTransactionCode) {
                            this.LoadGridData();
                        }
                    }
                })
            );
        }
    }

    GetInterestReports(){

        var filters = new ApiQueryFilters();
		filters.PageIndex = 0;
		filters.PageSize = maxInterestReportsFilterCount;
		filters.SortBy = "CreateDateTime";
		filters.SortDirection = "Descending";

        const statusFilter = [InterestReportStatus.Invoiced , InterestReportStatus.ClosedwithoutInvoice].join(',');

		filters.addAdditionalFilter("InterestReportStatusCode", statusFilter, null, null, "InList", false, false, false, "string");
		filters.addAdditionalFilter("GLAccountId", this.glaccountPM.Id, null, null, "Equals", false, false, false, "string");


        this.interestReportService.getByFilters(filters)
            .subscribe((myResponse: ServiceResponse) =>
            {
                if (!myResponse.HasError) {
                    const result = myResponse.Result;
                    this.InterestReports = [{ ReportNumber : 'None' }];
                    this.InterestReports = this.InterestReports.concat(result);
                }
            });
    }

    private SetTabGLAccountData()
    {
        this.glaccountPM = this.entityArgs.EntityPM;
        this.isControlAccount = this.glaccountPM.IsControlAccount;
    }
    private SetRTL()
    {
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    private SetDefaultDatesFilter()
    {
        const today = new Date();
        this.ToDate = new Date();
        this.oldToDate = new Date();
        const lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        this.oldFromDate = new Date(lastmonth);
    }

    //#region Properties
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date)
    {
        if (this.fromDate != value) {
            this.oldFromDate = this.fromDate;
            this.fromDate = value;

            if (!this.isValidate)
                this.ValidateDates();
            else {
                this.isValidate = false;
            }

        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date)
    {
        if (this.toDate != value) {
            this.oldToDate = this.toDate;
            this.toDate = value;

            if (!this.isValidate)
                this.ValidateDates();
            else {
                this.isValidate = false;
            }
        }
    }
    //#endregion

    //#region Data Source
    public columns: any[] = [];
    public QueryColumns: QueryColumnPM[] = [];

    BuildColumns()
    {
        this.columns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.InterestEntityTypeCode"),
            Styles: { width: '95px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("Source", 'Text', TextCodeTranslator.Translate("InterestTransaction.F.InterestEntityTypeCode")));

        this.columns.push({
            FieldName: 'InterestValueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.InterestValueDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("InterestValueDate", 'DateTime', TextCodeTranslator.Translate("InterestTransaction.F.InterestValueDate")));

        this.columns.push({
            FieldName: 'CreateDateTime',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.CreateDateTime"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("CreateDateTime", 'DateTime', TextCodeTranslator.Translate("InterestTransaction.F.CreateDateTime")));



        this.columns.push({
            FieldName: 'LocalAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.LocalAmount"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("LocalAmount", 'Number', TextCodeTranslator.Translate("InterestTransaction.F.LocalAmount")));

        this.columns.push({
           FieldName: 'CurrencyCode',
           DataTypeCode: 'String',
           Display: TextCodeTranslator.Translate("InterestTransaction.F.CurrencyCode"),
           Styles: { width: '100px' },
           HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
           HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
           IsCustomTemplate: true,
           ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("CurrencyCode", 'Text', TextCodeTranslator.Translate("InterestTransaction.F.CurrencyCode")));

        this.columns.push({
            FieldName: 'ForeignAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.ForeignAmount"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("ForeignAmount", 'Number', TextCodeTranslator.Translate("InterestTransaction.F.ForeignAmount")));


        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.JournalNumber"), // 'Journal No.',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("JournalNumber", 'Text', TextCodeTranslator.Translate("InterestTransaction.F.JournalNumber")));

        this.columns.push({
            FieldName: 'InterestReportNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.InterestReportNumber"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("InterestReportNumber", 'Text', TextCodeTranslator.Translate("InterestTransaction.F.InterestReportNumber")));

        this.columns.push({
            FieldName: 'IsClosed',
            DataTypeCode: 'Boolean',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.IsClosed"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("IsClosed", 'Boolean', TextCodeTranslator.Translate("InterestTransaction.F.IsClosed")));


        this.columns.push({
            FieldName: 'IsCancelled',
            DataTypeCode: 'Boolean',
            Display: TextCodeTranslator.Translate("InterestTransaction.F.IsCancelled"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("IsCancelled", 'Boolean', TextCodeTranslator.Translate("InterestTransaction.F.IsCancelled")));

        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'string',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InternalNotes"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'GlAccountInterestTransactionsNotesTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountInterestTransactionsNotesTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            ObjectTableName:'InterestTransaction',
            EntityPM:this.InterestReports[0],
        });
        this.QueryColumns.push(this.excelService.GetQueryColumn("Notes", 'string', TextCodeTranslator.Translate("InterestTransaction.F.Notes")));

        


    }

    DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) =>
        {
            var tempo = this.GetInterestTransactions(skip, take, sortingCol, sortingDir);
            return tempo;
        },
    };


    GetInterestTransactions(skip, take, sortingCol, sortingDir)
    {
        this.filterAgrs = new ApiQueryFilters();

        if (this.dateFilter) {
            this.filterAgrs.AdditionalFilters.push(this.dateFilter);
        } else {
            return new Promise(() => { });
        }

        if (this.searchFieldFilter) {
            this.filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
        }

        if (this.dateTypeCode) {
            // const dateTypeFilter = new FilterItem("DateTypeCode", this.dateTypeCode, null, null, "Equals", false, false, false, "string", false);
            // this.filterAgrs.AdditionalFilters.push(dateTypeFilter);
        } else {
            return;
        }

        this.filterAgrs.PageSize = take;
        this.filterAgrs.PageIndex = skip;
        this.filterAgrs.GetAll = false;
        this.filterAgrs.GetCount = true;

        if (sortingCol) {
            this.filterAgrs.SortBy = sortingCol;
        }
        if (sortingDir) {
            this.filterAgrs.SortDirection = sortingDir;
        }

        if(this.SelectedInterestReport && this.SelectedInterestReport.ReportNumber == 'None')
            this.filterAgrs.addAdditionalFilter("InterestReportId", "Please Don't Erase Me", null, null, "IsNull", true, false, false, "string");
            else if(this.SelectedInterestReport && this.SelectedInterestReport.ReportNumber != 'None')
            this.filterAgrs.addAdditionalFilter("InterestReportId", this.SelectedInterestReport.Id, null, null, "Equals", false, false, false, "string");

        this.filterAgrs.addAdditionalFilter("GLAccountId", this.glaccountPM.Id, null, null, "Equals", false, false, false, "string");

        return this.entityListService.getByFilters("InterestTransaction", this.filterAgrs);
    }

    OnDataLoaded(result) { }

    //#endregion

    //#region Date Filters Validation
    oldToDate: Date;
    oldFromDate: Date;
    isValidate: boolean = false;
    ValidateDates()
    {
        if (this.FromDate > this.ToDate) {
            this.timerToken = setTimeout(() =>
            {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateGreater"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);
        } else {
            this.timerToken = setTimeout(() =>
            {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
                this.CD.detectChanges();
            }, 200);

            this.LoadDataByDateFilter();
        }
    }

    LoadDataByDateFilter()
    {
        if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate))
        {
            let fieldName = "";

            switch (this.dateFilterSelectedValue) {
                case 'filter_Accounting': { fieldName = 'AccountingDate'; break; }
                case 'filter_Create': { fieldName = 'CreateDateTime'; break; }
                default:
                case 'filter_Interest': { fieldName = 'InterestValueDate'; break; }
            }

            this.dateFilter = new FilterItem(fieldName, this.FromDate, this.toDate, null, "Between", true, true, false, "Date", false);
            this.RefreshButtonClicked();
        }
    }
    //#endregion

    //#region Buttons Handlers

    RefreshButtonClicked()
    {
        this.LoadGridData();
    }

    LoadGridData()
    {
        this.QueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    ExportToExcelClick()
    {
        this.excelService.ExportToExcelExcute("GLAccountInterestTransactions", this.filterAgrs, this.QueryColumns);
    }

    //#endregion

    //#region Search
    private timerToken: any;
    TextChanged(searchtext)
    {
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(() =>
            {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.LoadGridData();
            }, SearchBoxDelayTime);

        } else {
            this.searchFieldFilter = null;
            this.LoadGridData();
        }
    }
    //#endregion

    //#region Filter Methods
    public dateFilterSelectedValue: string = 'filter_Interest';
    public dateTypeCode: string = '1';
    FilterItemClicked(itemValue: string)
    {
        if (this.dateFilterSelectedValue != itemValue) {
            this.dateFilterSelectedValue = itemValue;
            this.LoadDataByDateFilter();
        }
    }



    private _SelectedInterestReport : InterestReportList;
    public get SelectedInterestReport() : InterestReportList {
        return this._SelectedInterestReport;
    }
    public set SelectedInterestReport(v : InterestReportList) {
        this._SelectedInterestReport = v;

        this.LoadGridData();
    }

    //#endregion

}


export class InterestReportStatus{
    public static Draft = "1";
    public static Cancelled = "3";
    public static ClosedwithoutInvoice = "4";
    public static Invoiced = "2";
    public static Failed = "6";
    public static InProgress = "5";
    public static InvoicingInProgress = "8";
    public static InvoicingFailed = "9";
}
