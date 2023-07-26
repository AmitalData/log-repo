import { OnInit } from '@angular/core';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { EntityListService } from './../../../../Infrastructure/Services/EntityListService';
import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReconciliationPM} from '../../../EntityPMs/ReconciliationPM';
import {ReconciliationLinePM} from '../../../EntityPMs/ReconciliationLinePM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';


// export class ReconciliationLineModel {

//     constructor(line, color) {
//         this.Line = line;
//         this.OddEven = color;

//         // Calculate Transaction Amount
//         if (AppTool.IsNullOrZero(this.Line.ForeignAmountCredit)) {
//             this.TransactionAmount = -1 * this.Line.ForeignAmountDebit;
//         } else {
//             this.TransactionAmount = this.Line.ForeignAmountCredit;
//         }
//     }

//     Line: ReconciliationLinePM;
//     OddEven: boolean;
//     TransactionAmount: number;


// }

@Component({
    
    templateUrl: './ReconciliationDetailsTabComponent.html',
})

export class ReconciliationDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: ReconciliationPM = null;
    public ObjectTableName = "Reconciliation";
    public DataContext = this;
    public TotalSum = 0;
    public NoRows: boolean = false;
    searchText: string = "";
    public filterAgrs: ApiQueryFilters;
    public CurrencyCode;
    public AmountText: string;
    private CurrentSession = SessionLocator.SelectedSession;
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    public isRTL: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();
    // Events
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    // Services
    private _entityListService: EntityListService = new EntityListService();
    reconciliationInit = false

    constructor(private entityArgs: EntityArgs) {
        super();


        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.LoadData();
        var currencyCode= this.EntityPM.AccountReconcileMethodCode =="0"?  SessionLocator.TenantPM.AccountingCurrencyCode: this.EntityPM.CurrencyCode;
        this.LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent(); 

        this.AmountText = TextCodeTranslator.Translate("Accounting.General.O.Amount") + " ("+  currencyCode + ")";
    }

    ngOnInit() {
             this.BuildColumns();
            this.ReloadData();
       
    }

    //#region Properties

    //get Number() { return this.EntityPM.Number; }

    //#endregion

    private timerToken: any;
    TextChanged(searchtext) {
        if (!AppTool.IsNullOrEmpty(searchtext)) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.RefreshButtonClicked();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    LoadData() {
        // this.ItemSource = [];
        // this.originalItemSource = [];

        // this.EntityPM.ReconciliationLines.forEach((line) => {
        //     var item = new ReconciliationLineModel(line, this.ColorMe(line));
        //     this.ItemSource.push(item);
        // });
        // this.originalItemSource = this.ItemSource;


        // this.CalculateTotals();
    }

    OpenJournal(id) {
        // open Journal screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                });
        }
    }

    CalculateTotals() {
        // if (!AppTool.IsNullOrEmpty(this.ItemSource)) {
        //     for (let line of this.ItemSource) {
        //         this.TotalSum += line.Line.ReconciliationAmount;
        //     }
        // }
    }

    originalItemSource;
    FilterLines() {
        // var lines = this.originalItemSource;

        // // Filtering
        // if (!AppTool.IsNullOrEmpty(this.searchText)) {
        //    lines = lines.filter((el) => {
        //        var line = el.Line;
        //        if (line.SearchFields != null)
        //            if (line.SearchFields.toLowerCase().includes(this.searchText.toLowerCase())) return true;
        //        return false;
        //    });
        // }
        // this.ItemSource = lines;
        // this.NoRows = lines.length == 0;
    }

    //#region Row Coloring
    lastGroupNumber: number;
    lastColorOperation: boolean = true;
    ColorMe(line: ReconciliationLinePM) { // "Line says"
        if (!AppTool.IsNullOrEmpty(line)) {

            if (AppTool.IsNullOrEmpty(this.lastGroupNumber)) this.lastGroupNumber = line.GroupNumber;

            if (this.lastGroupNumber == line.GroupNumber) {
                return this.lastColorOperation == true;
            } else {
                this.lastGroupNumber = line.GroupNumber;
                this.lastColorOperation = !this.lastColorOperation;
                return this.lastColorOperation == true;
            }

        }
        return false;
    }
    //#endregion

    RefreshButtonClicked() {
        // this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        // this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        // this.LoadData();

        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

    }


    //#region Data
    searchFieldFilter: FilterItem;
    public columns: any[] = null;

    ReloadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }
    public QueryColumns: QueryColumnPM[] = [];

    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),//'Acc. Date',
            Styles: { width: '105px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'AccountingDate'
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccountingDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate")));

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("JournalNumber",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber")));

        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DueDate",'DateTime',TextCodeTranslator.Translate("LedgerTransaction.F.DueDate")));

        this.columns.push({
            FieldName: 'TransactionAmount',
            DataTypeCode: 'DateTime',
            Display: this.AmountText,
            Styles: { width: '110px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ExcelTransactionAmount",'Decimal',this.AmountText));

        this.columns.push({
            FieldName: 'ReconciliationAmount',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("Accounting.General.O.ReconciliationAmount"),
            Styles: { width: '110px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ReconciliationAmount",'Decimal',TextCodeTranslator.Translate("Accounting.General.O.ReconciliationAmount")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("CurrencySign", 'Text', TextCodeTranslator.Translate("LedgerTransaction.F.CurrencyId")));

        this.columns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"),
            Styles: { width: '75px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference1",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference1")));

        this.columns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"),
            Styles: { width: '75px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference2",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference2")));

        this.columns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"),
            Styles: { width: '75px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference3",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Reference3")));

        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("LedgerTransaction.F.Notes"),
            Styles: { width: '200px' },
            HtmlListComponentName: 'ReconciliationLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconciliationLineListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Notes",'Text',TextCodeTranslator.Translate("LedgerTransaction.F.Notes")));

        //this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        //#region Filters
        this.filterAgrs = new ApiQueryFilters;
        // if (this.dateFilter) {
        //     filters.AdditionalFilters.push(this.dateFilter);
        // } else {
        //     return;
        // }
        if (this.searchFieldFilter) {
            this.filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
        }

        this.filterAgrs.PageIndex = skip;
        this.filterAgrs.PageSize = take;
        this.filterAgrs.GetCount = getCount;

        this.filterAgrs.SortBy = "Line";
        this.filterAgrs.SortDirection = "Ascending";
        if (sortingCol) {
            this.filterAgrs.SortBy = sortingCol;
        }
        if (sortingDir) {
            this.filterAgrs.SortDirection = sortingDir;
        }
        this.filterAgrs.addAdditionalFilter("ReconciliationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

        //#endregion

        return this._entityListService.getByFilters("ReconciliationLine", this.filterAgrs);

    }

    //#endregion
    public ExportToExcelClick(){
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute("ReconciliationLine",this.filterAgrs,this.QueryColumns);
    }
    

}
