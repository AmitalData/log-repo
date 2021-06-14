import {Component, OnInit, Output, EventEmitter,AfterViewInit,ChangeDetectorRef}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {LedgerTransactionExtendedListService} from '../../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';


@Component({
    templateUrl: './GLAccountChequeListComponent.html',
    providers: [LedgerTransactionListService, LedgerTransactionExtendedListService, GLAccountExtendedListService]
})

export class GLAccountChequeListComponent extends BaseComponent implements OnInit, AfterViewInit {
    @Output() onQueryChangeEvent = new EventEmitter();
    public EntityPM: GLAccountPM = null;
    public ObjectTableName = "GLAccount";
    public DataContext = this;
    public filterAgrs: ApiQueryFilters;
 
    // Services
    _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    private _entityListService: EntityListService;
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();

    // Filters
    searchFieldFilter: FilterItem;

    public ItemsSource: LedgerTransactionList[];

    public UsingLogGridV2:boolean= false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( private CD: ChangeDetectorRef){
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.UsingLogGridV2 = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LV2")[0]? true : false;
        this._entityListService = new EntityListService();
    }
    
    SetWindowArgs(args: any) {

        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;

            this.LoadData();
        }
    }

    LoadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    ngOnInit() {
       this.BuildColumns();
    }


    ngAfterViewInit() {
        this.CD.detectChanges();
    }



    //#region Data Source
    public columns: any[] = null;
  
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'PaymentValueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ARPayment.F.ValueDate"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Source"), // 'Source',
            Styles: { width: '80px' }, // TASK 47563
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'LocalAmountCredit',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.LocalAmountCredit"), // 'Local Amount',
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'ForeignAmountCredit',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmountCredit"), // 'Foreign Amount',
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"), // 'Ref. 1',
            Styles: { width: '85px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"), // 'Ref. 2',
            Styles: { width: '85px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"), // 'Ref. 3',
            Styles: { width: '85px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"), // 'Journal No.',
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'PaymentChequeStatus',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARPaymentCheque.F.StatusCode"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Notes"), // 'Notes',
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
    }

    DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingCol: "PaymentValueDate",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    @Output() MenuHeaderchangeevent = new EventEmitter();

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string,filters:ApiQueryFilters=null) {
        this.filterAgrs = new ApiQueryFilters();
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
 

        return this._entityListService.getARPyamentChequesListAsLedgerTransactions("LedgerTransaction", this.filterAgrs);//this.ledgerTransactionListExtendedService.getByFilters(filters);
    }


    GetLTB() {

        // Filters
        var filters = new ApiQueryFilters;
    
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
    
        filters.PageSize = 30;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = "PaymentValueDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("GLAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

        this.MenuHeaderchangeevent.emit({ Filters: filters, IgnoreFilter: false });

    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    LoadAllScreenData() {
        this.GetLTB();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }


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
                }

            }, 100);
    }

    OnMouseLeave() {
        this.isMouseIn = false;
            this.timerToken = setTimeout(() => {
                document.getElementById("tooltip-body-1").style.visibility = "hidden";

            }, 400);
    }
    //#endregion

    //#region Search + screen dimention
    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.RefreshButtonClicked();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

   
}
