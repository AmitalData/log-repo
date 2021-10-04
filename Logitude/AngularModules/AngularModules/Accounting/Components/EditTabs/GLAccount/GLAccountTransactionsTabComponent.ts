import { DateTool } from './../../../../Infrastructure/Tools';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';

import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {RatesTableList} from '../../../../Infrastructure/EntityLists/RatesTableList';
import {LTBResponse} from '../../../DataContracts/LTBResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

import {ReconcileEventManager} from '../../../Utilities/ReconcileEventManager';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {LedgerTransactionExtendedListService} from '../../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {RatesTableListService} from '../../../../Infrastructure/Services/StandardLists/RatesTableListService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { TaxReportExtendedPMService } from '../../../Services/ExtendedPMs/TaxReportExtendedPMService'
import { TaxReportPM } from '../../../EntityPMs/TaxReportPM';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { FullAccountingSettingList } from '../../../../Accounting/EntityLists/FullAccountingSettingList';

@Component({

    templateUrl: './GLAccountTransactionsTabComponent.html',
    providers: [LedgerTransactionListService, LedgerTransactionExtendedListService, GLAccountExtendedListService]
})

export class GLAccountTransactionsTabComponent extends BaseComponent implements OnInit, AfterViewInit{
    @Output() onQueryChangeEvent = new EventEmitter();
    public EntityPM: GLAccountPM = null;
    public ObjectTableName = "GLAccount";
    public DataContext = this;
    public filterAgrs: ApiQueryFilters;
    private fullAccountingSetting: FullAccountingSettingList;
    // Services
    private _entityListService: EntityListService;
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    private _CurrencyListService: CurrencyListService = new CurrencyListService();
    private _RatesTableListService: RatesTableListService = new RatesTableListService();
    private ledgerTransactionListService: LedgerTransactionListService = new LedgerTransactionListService();
    private ledgerTransactionListExtendedService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    private glAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private taxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    // Filters
    dateFilter: FilterItem;
    currencyFilter: FilterItem;
    searchFieldFilter: FilterItem;
    CurrencyFilters: ApiQueryFilters = new ApiQueryFilters();
    private TenatTaxReports: TaxReportPM[];
    public ItemsSource: LedgerTransactionList[];
    ratesTable: RatesTableList[];
    public LTBSummery: LTBResponse = new LTBResponse();
    public OpenReconciliationMessage: string = "There are no Open Transactions";
    public TaxReportLists: CodeNameClass[] =[];
    LocalSums: number[];
    ForeignSums: number[];
    isSingleCurrency: boolean = false;
    isControlAccount: boolean = false;
    public UsingLogGridV2:boolean= false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public UseTaxreportFilter: boolean = false;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef){
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.UsingLogGridV2 = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LV2")[0]? true : false;
        this._entityListService = new EntityListService();
        this.EntityPM = entityArgs.EntityPM;
        this.CurrencyId = this.EntityPM.CurrencyId;
        this.isControlAccount = this.EntityPM.IsControlAccount;
        this.LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();
        this.GetCurrencies();

        this.LoadDefaultValues();

        this.LoadAllScreenData();

        //this.RefreshButtonClicked();
        //this.GetNonReconciledTransactionsCount();

        // Set GLAccountReconcileMethodCode to use it in reconcile window
        ReconcileEventManager.GLAccountReconcileMethodCode = this.EntityPM.ReconcileMethodCode;

        //Set Currency LOV editability
        if (this.EntityPM.IsMultiCurrency) {
            this.UIProperties.SetEnabled("CurrencyId", "GLAccount", true);
        } else {
            this.UIProperties.SetEnabled("CurrencyId", "GLAccount", false);
        }

        //event listening
        if (this.CurrentSession.CurrentEditComponent != null) {

            var _CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (_CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "GATR") {
                            this.LoadAllScreenData();
                        }
                    }
                })
            );
        }

        this.GetTransactionsCurrencies();
        this.GetFullAccountingSettings();
        
    }
    DisplayTaxReportFilter: boolean;
    GetFullAccountingSettings() {
        this._entityListService.getSingle(SessionLocator.Tenant.toString(), "FullAccountingSetting").then((res: any) => {
            this.CurrentSession.StopBusyIndicator();
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var res = myResponse.Result;
                    this.fullAccountingSetting = res;
                    if (this.fullAccountingSetting.VATInputsGLAccountId == this.EntityPM.Id || this.fullAccountingSetting.VATOutputGLAccountId == this.EntityPM.Id) {
                        this.UseTaxreportFilter = true;
                        this.DisplayTaxReportFilter = true;
                        this.GetTransmittedTaxReports();
                    }
                    else {
                        this.UseTaxreportFilter = false;
                        this.DisplayTaxReportFilter = false;
                    }
                }
            })
        });
    }
    GetTransmittedTaxReports() {
        this.taxReportExtendedPMService.GetTenantTransmittedTaxReports().subscribe((response: any) => {
            this.TenatTaxReports = response.Result;
            this.CurrentSession.StopBusyIndicator();
            if (this.TenatTaxReports != null) {
                this.TenatTaxReports.forEach(p => {                   
                    this.TaxReportLists.push(new CodeNameClass(p.Id,this.FormatTaxReportDate(p.TaxReportMonth)));                   
                });
            }


        });
    }
    FormatTaxReportDate(date: Date) {
      var newDate=  new Date(date);
        var month: number = newDate.getMonth()+1;
        var year: number = newDate.getFullYear();
        return month + "." + year;
    }
    ngOnInit() {
        this.BuildColumns();
    }
    ngAfterViewInit() {
        //this.RefreshButtonClicked();
        //this.LoadAllScreenData();

        //#region Fill Date Default Values
        var today = new Date();
        this.ToDate = new Date();
        this.oldToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        this.oldFromDate = new Date(lastmonth);
        //#endregion


        this.CD.detectChanges();

    }

    LoadDefaultValues() {
        // Load Tenant rates
        var filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter("BaseCurrencyId", SessionLocator.TenantPM.CurrencyId, null, null, "Equals", false, false, false, "string");

        this._RatesTableListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var rates = myResponse.Result;

                    if (!AppTool.IsNullOrEmpty(rates)) {
                        this.ratesTable = rates;
                    }
                }
            }
        });
    }

    //#region Properties
    //OpenAmountHint: string = "";
    private openAmountHint: string;
    get OpenAmountHint() { return this.openAmountHint; }
    set OpenAmountHint(value: string) {
        if (this.openAmountHint != value) {
            this.openAmountHint = value;
        }
    }

    private selectedTaxReport: CodeNameClass;
    get SelectedTaxReport() { return this.selectedTaxReport; }
    set SelectedTaxReport(value: CodeNameClass) {
        if (this.selectedTaxReport != value) {
            this.selectedTaxReport = value;
            this.LoadAllScreenData();
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.oldFromDate = this.fromDate;
            this.fromDate = value;
            //if (this.fromDate > this.ToDate) {

            //    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("To date must be Greater or equal than from date"));
            //}
            //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
            //    this.dateFilter = new FilterItem("CreateDate", this.FromDate, this.ToDate, null, "Between", false, false, false, "Date", false);
            //    //this.GetTransactions();
            //    //this.GetLTB();
            //}

            if (!this.isValidate)
               this.validateDates();
            else {
                this.isValidate = false;
            }

        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.oldToDate = this.toDate;
            this.toDate = value;
            //if (this.toDate < this.FromDate) {

            //    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("To date must be Greater or equal than from date"));
            //}
            //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
            //    this.dateFilter = new FilterItem("CreateDate", this.FromDate, this.ToDate, null, "Between", false, false, false, "Date", false);
            //    //this.GetTransactions();
            //    //this.GetLTB();
            //}

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }
        }
    }

    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.currencyFilter = new FilterItem("CurrencyId", value, null, null, "Equals", false, false, false, "string", false);
                //this.GetTransactions();
            } else {
                this.currencyFilter = null
            }
            this.GetLTB();



        }
    }

    private totalSum: number;
    get TotalSum() {
        if (this.LocalSums)
            return this.LocalSums[this.LocalSums.length - 1];
        return 0;
    }
    set TotalSum(value: number) {
    }

    private attachedGLAccountCheckBox: boolean = false;
    get AttachedGLAccountCheckBox() { return this.attachedGLAccountCheckBox; }
    set AttachedGLAccountCheckBox(value: boolean) {
        if (this.attachedGLAccountCheckBox != value) {
            this.attachedGLAccountCheckBox = value;
            this.RefreshButtonClicked();
        }
    }

    private splittedByCurrencyCheckBox: boolean = false;
    get SplittedByCurrencyCheckBox() { return this.splittedByCurrencyCheckBox; }
    set SplittedByCurrencyCheckBox(value: boolean) {
        if (this.splittedByCurrencyCheckBox != value) {
            this.splittedByCurrencyCheckBox = value;
            this.RefreshButtonClicked();
        }
    }

    private notIncludedInAnyTaxReport: boolean = false;
    get NotIncludedInAnyTaxReport() { return this.notIncludedInAnyTaxReport; }
    set NotIncludedInAnyTaxReport(value: boolean) {
        if (this.notIncludedInAnyTaxReport != value) {
            this.notIncludedInAnyTaxReport = value;
            this.SelectedTaxReport = null;
            this.RefreshButtonClicked();
        }
    }

    //#endregion

    //#region Data Source
    public columns: any[] = null;
    public QueryColumns: QueryColumnPM[] = [];
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'GLAccountIndicator',
            DataTypeCode: 'text',
            Display: '',
            Styles: { width: '20px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        
        this.columns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),//'Acc. Date',
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccountingDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate")));

        this.columns.push({
            FieldName: 'DocumentDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"), //'Ref. Date',
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DocumentDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate")));

        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"), // 'Due Date',
            Styles: { width: '75px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DueDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.DueDate")));

        this.columns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Source"), // 'Source',
            Styles: { width: '100px' }, // TASK 47563
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Source",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Source")));

        this.columns.push({
            FieldName: 'LocalAmountCredit',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.LocalAmountCredit"), // 'Local Amount',
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("CalculatedLocalAmount",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.LocalAmountCredit")));

        this.columns.push({
            FieldName: 'CumulativeLocalAmount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeLocalAmount"), // 'Cu. Amount',
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("CumulativeLocalAmount",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeLocalAmount")));

        //this.columns.push({
        //    FieldName: 'CurrencyCode',
        //    DataTypeCode: 'String',
        //    Display: 'Currency',
        //    Styles: { width: '70px' },
        //    IsCustomTemplate: true
        //});
        if (this.EntityPM.CurrencyId != SessionLocator.TenantPM.CurrencyId) {
            this.columns.push({
                FieldName: 'ForeignAmountCredit',
                DataTypeCode: 'String',
                Display: TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmountCredit"), // 'Foreign Amount',
                Styles: { width: '120px' },
                HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                IsCustomTemplate: true
            });
            this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ForeignAmountCreditWithSign",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmountCredit")));


            if (this.EntityPM.IsMultiCurrency != true) {
                this.columns.push({
                    FieldName: 'CumulativeForeignAmount',
                    DataTypeCode: 'String',
                    Display: TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeForeignAmount"), // 'Cu. F. Amount',
                    Styles: { width: '120px' },
                    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                    IsCustomTemplate: true
                });
                this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("CumulativeForeignAmountSign",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeForeignAmount")));

            }
        }
        this.columns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"), // 'Ref. 1',
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference1",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference1")));

        this.columns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"), // 'Ref. 2',
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference2",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference2")));

        this.columns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"), // 'Ref. 3',
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference3",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference3")));

        this.columns.push({
            FieldName: 'OppositeAccountLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.OppositeAccountLocalName"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("OppositeAccountLocalName",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.OppositeAccountLocalName")));

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"), // 'Journal No.',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("JournalNumber",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber")));

        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Notes"), // 'Notes',
            Styles: { width: '200px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Notes",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Notes")));

        //this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 50,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    @Output() MenuHeaderchangeevent = new EventEmitter();

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string,filters:ApiQueryFilters=null) {
        this.filterAgrs = new ApiQueryFilters();
 
        if (this.dateFilter) {
            this.filterAgrs.AdditionalFilters.push(this.dateFilter);
         } else {
            return new Promise((resolve, reject) => { });
        }
        if (this.currencyFilter) {
            this.filterAgrs.AdditionalFilters.push(this.currencyFilter);
 
        }
        if (this.searchFieldFilter) {
            this.filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
 
        }
        if (this._dateTypeCode) {
            var dummyFilter =  new FilterItem("DateTypeCode", this._dateTypeCode, null, null, "Equals", false, false, false, "string", false);
            this.filterAgrs.AdditionalFilters.push(dummyFilter);
        }else{
            var msg = new MessageWindow();
            msg.Show("No filter selected!!!!");
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
        this.filterAgrs.addAdditionalFilter("GLAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        if (this.SelectedTaxReport) {
            
            this.filterAgrs.addAdditionalFilter("TaxReportId", this.SelectedTaxReport.Code, null, null, "Equals", true, false, false, "string");

        }
        this.filterAgrs.addAdditionalFilter("IncludeRelatedCurrenciesAccount", this.splittedByCurrencyCheckBox == null ? false : this.splittedByCurrencyCheckBox, null, null, "Equals", false, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("IncludeChildAccounts", this.attachedGLAccountCheckBox == null ? false : this.attachedGLAccountCheckBox, null, null, "Equals", false, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("NotIncludedInAnyTaxReport", this.notIncludedInAnyTaxReport == null ? false : this.notIncludedInAnyTaxReport, null, null, "Equals", false, false, false, "boolean");
        
        this.filterAgrs.addAdditionalFilter("UseTaxreportFilter", this.UseTaxreportFilter , null, null, "Equals", false, false, false, "boolean");

        return this._entityListService.getExtendedByFilters("LedgerTransaction", this.filterAgrs);//this.ledgerTransactionListExtendedService.getByFilters(filters);
    }

 
     
    public ExportToExcelClick(){
       // this._entityResourceService.getEntityResourceByTableName("LedgerTransaction", 0).subscribe((response: any) => {
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute("GLAccountLedgerTransaction",this.filterAgrs,this.QueryColumns);
      //  });
    }

    GetTransactions() {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        filters.PageSize = 10000;
        filters.addAdditionalFilter("AccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.ledgerTransactionListService.getByFilters(filters).subscribe((myResult:any) => {
            console.log("Response: ", myResult);
            if (myResult == null) {
                this.ItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                    this.LocalSums = [];
                    this.ForeignSums = [];

                    // check if transactions is multi currency
                    this.isSingleCurrency = true;
                    for (var i = 0; i < this.ItemsSource.length -1 ; i++) {
                        if (this.ItemsSource[i].CurrencyId != this.ItemsSource[i + 1].CurrencyId)
                            this.isSingleCurrency = false;
                    }

                    // Calculate commulative sums
                    for (var i = 0; i < this.ItemsSource.length; i++) {

                        if (i == 0) {
                            this.LocalSums[i] = this.ItemsSource[i].LocalAmountCredit - this.ItemsSource[i].LocalAmountDebit;
                            this.ForeignSums[i] = this.ItemsSource[i].ForeignAmountCredit - this.ItemsSource[i].ForeignAmountDebit;
                        } else {
                            this.LocalSums[i] = this.ItemsSource[i].LocalAmountCredit - this.ItemsSource[i].LocalAmountDebit + this.LocalSums[i - 1];
                            if (this.isSingleCurrency)
                                this.ForeignSums[i] = this.ItemsSource[i].ForeignAmountCredit - this.ItemsSource[i].ForeignAmountDebit + this.ForeignSums[i - 1];
                        }
                        //console.log("LocalSums[" + i + "]=" + this.LocalSums[i]);
                    }
                }
            }
        });
    }

    GetLTB() {

        // Filters
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        } else {
            return;
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this._dateTypeCode) {
            var dummyFilter =  new FilterItem("DateTypeCode", this._dateTypeCode, null, null, "Equals", false, false, false, "string", false);
            filters.AdditionalFilters.push(dummyFilter);
        }
        filters.PageSize = 30;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = "AccountingDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("GLAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("IncludeRelatedCurrenciesAccount", this.splittedByCurrencyCheckBox == null ? false : this.splittedByCurrencyCheckBox, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IncludeChildAccounts", this.attachedGLAccountCheckBox == null ? false : this.attachedGLAccountCheckBox, null, null, "Equals", false, false, false, "boolean");

        this.MenuHeaderchangeevent.emit({ Filters: filters, IgnoreFilter: false });

        this.ledgerTransactionListExtendedService.getBalanceByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                        //console.log("Response: ", myResult);
                        //if (myResult == null) {
                        //}
                        //else {
                             if (!myResponse.HasError) {
                                 this.LTBSummery = myResponse.Result;
                                // if (this.EntityPM.IsMultiCurrency) {
                                    var text = " &nbsp;";

                                    for (let item of this.LTBSummery.EndBalanceForeignList) {
                                        item.CurrencyCode = this.GetCurrencyCode(item.CurrencyId);
                                        item.CurrencySign = this.GetCurrencySign(item.CurrencyId);
                                    }

                                    for (let item of this.LTBSummery.StartBalanceForeignList) {
                                        item.CurrencyCode = this.GetCurrencyCode(item.CurrencyId);
                                        item.CurrencySign = this.GetCurrencySign(item.CurrencyId);

                                        //text += item.CurrencyCode + ' ' + item.BalanceForeign + ' ' + item.CurrencyCode + '<br>';
                                    }

                                    this.OpenAmountHint = text;
                                // }

                                console.log("Result: ", myResponse.Result);
                            }
                        //}
                    });

    }

    currencyFilterValues;
    OnDataLoaded(result) {
        if (result && this.EntityPM.IsMultiCurrency) {
            var transactions = result;

            if (!this.currencyFilterValues) {
                // Create filter string that maintain values of current curreincies in the list
                // transactions.forEach((item) => {
                //     if (item.rowData) {
                //         this.currencyFilterValues += (item.rowData.CurrencyId + ",");
                //     }
                // });


            }

            console.log(this.currencyFilterValues);
        } else {
            this.CurrencyFilters = new ApiQueryFilters(true);
        }

    }

    GetTransactionsCurrencies(){
        this.CurrentSession.StartBusyIndicatorLoading();
        this._LedgerTransactionExtendedListService.GetTransactionsCurrencies(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) =>
        {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                console.log("[GetTransactionsCurrencies]", result);
                var currenciesIds: string[] = result;

                this.CurrencyFilters = new ApiQueryFilters();
                this.CurrencyFilters.addAdditionalFilter("Id", currenciesIds.join(','), null, null, "InListExact", false, false, false, "string", false, true);

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    reconciliationCount: number = 0;
    GetNonReconciledTransactionsCount() {
        this.glAccountExtendedListService.GetAccountReconcilesCount(this.EntityPM.Id).subscribe((myResult:number) => {
            this.reconciliationCount = 0;

            if (!AppTool.IsNullOrEmpty(myResult)) {
                this.reconciliationCount = myResult;

                if (myResult == 1) {
                    this.OpenReconciliationMessage = "There is " + this.reconciliationCount + " Open Transactions";
                } else if (myResult > 1) {
                    this.OpenReconciliationMessage = "There are " + this.reconciliationCount + " Open Transactions";
                }

            }

        });
    }

    GetOpenBalanceCurrencySign() {
        var result = "";
        if (this.EntityPM) {
            if (this.EntityPM.IsMultiCurrency) {
                result = this.TenantCurrencySign;
            }
            else {
                if(SessionLocator.TenantPM.CurrencyId == this.EntityPM.CurrencyId)
                    result = this.TenantCurrencySign;
                else if(this.LTBSummery.StartBalanceForeignList.length > 0)
                    result = this.EntityPM.CurrencySign;
            }
        }
        return result;
    }
    GetOpenBalanceAmount() {
        var result = 0;
        if (this.EntityPM && this.LTBSummery) 
        {
            if (this.EntityPM.IsMultiCurrency) {
                if(this.LTBSummery.StartBalanceLocal)
                    result = Number(this.LTBSummery.StartBalanceLocal);
            }
            else {
                if(SessionLocator.TenantPM.CurrencyId == this.EntityPM.CurrencyId){
                    result = Number(this.LTBSummery.StartBalanceLocal);
                }
                else if(this.LTBSummery.StartBalanceForeignList.length > 0)
                    result = Number(this.LTBSummery.StartBalanceForeignList[0].BalanceForeign);
            }

        }
        return result;
    }

    //#endregion

    //#region Date Filters Validation
    oldToDate: Date;
    oldFromDate: Date;
    isValidate: boolean = false;
    validateDates() {
        if (this.FromDate > this.ToDate) {

            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateGreater"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);



            //// Change dates
            // this.timerToken = setTimeout(() => {
            //     this.isValidate = true;
            //     this.FromDate = this.oldFromDate;
            //     this.isValidate = true;
            //     this.ToDate = this.oldToDate;
            //     this.LoadData();

            // }, 200);

        } else {
            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
                this.CD.detectChanges();
            }, 200);

            this.LoadData();

        }
    }

    //load data after validate date
    LoadData() {
        if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {

            // var _fromDate = new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0);
            var _fromDate =  [this.fromDate.getFullYear().toString(), this.FromDate.getMonth(), this.FromDate.getDate()].join(";");
            // var _toDate = Date.UTC(this.toDate.getFullYear(), this.toDate.getMonth(), this.toDate.getDate(), 23, 59, 59);
            var _toDate =  [this.toDate.getFullYear().toString(), this.toDate.getMonth(), this.toDate.getDate()].join(";");
            this.dateFilter = new FilterItem("AccountingDate", _fromDate, _toDate, null, "Between", false, false, false, "Date", false);
            console.log(">> Date Filter: ", _fromDate, _toDate);

            // this.dateFilter = new FilterItem("AccountingDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            this.RefreshButtonClicked();

        }
    }
    //#endregion

    //#region Buttons + CheckBox Handlers
    ReconcileButtonClicked() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result.Result; // get the data
                var openAmountCurrency = transaction.OpenAmountCurrencySign;

                // original amount currency
                var originalAmountCurrency;
                if (ReconcileEventManager.GLAccountReconcileMethodCode == "0") originalAmountCurrency = SessionLocator.TenantPM.CurrencySign;
                else if (ReconcileEventManager.GLAccountReconcileMethodCode == "1") originalAmountCurrency = transaction.CurrencySign;


                var windowArgs: any = {};
                windowArgs.GLAccountPM = this.EntityPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;

                logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";

                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event == 'ok') {
                        // show alert
                    }
                    this.RefreshButtonClicked();
                });
                this.CurrentSession.StopBusyIndicator();


            }
        });
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();

        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    LoadAllScreenData() {
        if (this._dateTypeCode !="4")
        this.GetLTB();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        this.GetNonReconciledTransactionsCount();
    }

    Export2ExcelClicked() {
        var windowArgs: any = {};
        //windowArgs.query = this.SelectedQuery;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.tenant = SessionInfo.LoggedUserTenant;
        windowArgs.userid = SessionInfo.LoggedUserId;
        //windowArgs.Filters = this.CurrentQueryFilters

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = "Exporting View Data List To Excel File";
        logitudeWindow.WindowArgs = windowArgs;
        //logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
        //logitudeWindow.WindowClosed.subscribe(($event: any) => {
        //    this.QueryValueChanged({ QueryId: this.SelectedQueryId })
        //});
        //});
    }

    AttachedGLAccountChanged(event) {
        if (event == true) {

        } else {

        }
    }

    SplittedByCurrencyChanged(event) {
        if (event == true) {

        } else {

        }
    }

    //#endregion

    //#region ToolTip
    private isMouseIn: boolean = false;
    OnMouseOver() {
        this.isMouseIn = true;
        //if (this.currencyRate) {
            this.timerToken = setTimeout(() => {
                var item = document.getElementById("tooltip-1");
                if (AppTool.IsNullOrEmpty(item))
                    return;
                var itemRect = item.getBoundingClientRect();

                if (this.isMouseIn) {
                    var i = document.getElementById("tooltip-body-1");
                    if (AppTool.IsNullOrEmpty(i))
                        return;
                    document.getElementById("tooltip-body-1").style.position = "fixed";
                    document.getElementById("tooltip-body-1").style.top = (itemRect.top - 70) + 'px';
                    document.getElementById("tooltip-body-1").style.left = (itemRect.left + 22) + 'px';
                    document.getElementById("tooltip-body-1").style.visibility = "visible";

                    //this.timerToken = setTimeout(() => {
                    //    document.getElementById("tooltip-body-1").style.visibility = "hidden";

                    //}, 15000);
                }

            }, 100);
        //}
    }
    OnMouseLeave() {
        this.isMouseIn = false;
        //if (this.currencyRate) {

            this.timerToken = setTimeout(() => {
                document.getElementById("tooltip-body-1").style.visibility = "hidden";

            }, 400);

        //}

    }
    //#endregion

    //#region Search + screen dimention
    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                //this.GetTransactions();
                this.RefreshButtonClicked();
                //this.GetLTB();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    }
    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }
    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }
    //#endregion

    //#region Currencies + Amounts
    Currencies: CurrencyList[];
    TenantCurrency: string;
    TenantCurrencySign: string;

    GetCurrencies() {
        this.TenantCurrency = SessionLocator.TenantPM.CurrencyCode;
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;

        this._CurrencyListService.getAll().subscribe((myResult:any) => {
            console.log("Currencies: ", myResult);
            if (myResult == null) {
                this.Currencies = [];
            }
            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.Currencies = myResponse.Result;
                }
            }
        });

    }
    GetCurrencyCode(currencyId: string) {
        var c;
        if (!AppTool.IsNullOrEmpty(this.Currencies)) {
            c = this.Currencies.find(d => d.Id == currencyId);
        }
        return AppTool.IsNullOrEmpty(c) ? null : c.Code;
    }
    GetCurrencySign(currencyId: string) {
        var c;
        if (!AppTool.IsNullOrEmpty(this.Currencies)) {
            c = this.Currencies.find(d => d.Id == currencyId);
        }
        return AppTool.IsNullOrEmpty(c) ? null : c.Sign;
    }

    CalculateLocalAmount(foreignCurrencyId: string,foreignAmount: number) {
        if (!AppTool.IsNullOrEmpty(foreignAmount) || !AppTool.IsNullOrEmpty(this.ratesTable)) {

            if (foreignCurrencyId)
            {
                var rate = this.ratesTable.find(d => d.ForeignCurrencyId == foreignCurrencyId);
                if (rate)
                    return rate.Rate * foreignAmount;
                else {
                    console.error("no rate for provided foreignCurrencyId ! ", foreignCurrencyId, this.ratesTable);
                    return 0;
                }
            }
            else
            {
                console.error("Cannot convert foreign amount to local amount, foreign Currency Id does not provided! ", foreignCurrencyId);
                return 0;
            }

        } else {
            console.error("Cannot convert foreign amount to local amount, Amount or Rates Table is empty! ", foreignAmount, this.ratesTable);
            return 0;
        }
    }

    //#endregion

    //#region Filter Methods
    public filterSelectedValue: string = 'filter_accounting';
    public _dateTypeCode: string = '1';
    FilterItemClicked(itemValue: string) {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    FilterLines() {

        //Task 46666: Transaction Tab - date filter new design
        // <DateTypeCode>2</DateTypeCode> 1/2/3
        // Accounting- - code 1- חשבונאי
        // Due - code 2 - לגביה
        // Reference -code-3-  אסמכתא

        switch (this.filterSelectedValue) {
            case 'filter_accounting':
                this._dateTypeCode = '1';
                break;
            case 'filter_due':
                this._dateTypeCode = '2';
                break;
            case 'filter_reference':
                this._dateTypeCode = '3';
                break;
            case 'filter_Tax':
                this._dateTypeCode = '4';
                this.ResetLTBFields();
                break;
            default:
                break;
        }
        if (this._dateTypeCode != '4') {
           this.UseTaxreportFilter = false;
            this.RefreshButtonClicked();
        }


    }
    ResetLTBFields() {
        if (this.LTBSummery) {
            this.LTBSummery.StartBalanceLocal = 0;
            this.LTBSummery.StartBalanceForeignList = null;
            this.LTBSummery.EndBalanceLocal = 0;
            this.LTBSummery.EndBalanceForeignList = null;
            this.LTBSummery.StartBalanceForeign = 0;
            this.LTBSummery.EndBalanceForeign = 0;
        }
    }
    //#endregion

}
