import { Output,OnInit } from '@angular/core';
import { EventEmitter } from '@angular/core';
import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {CashBookLinePM} from '../../../EntityPMs/CashBookLinePM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {GLAccountListService} from '../../../Services/StandardLists/GLAccountListService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './CashBookDetailsTabComponent.html',
})

export class CashBookDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    public DataContext = this;
    public TotalSum = 0;
    public NoRows: boolean = false;
    tenantCurrency: string = SessionLocator.TenantPM.CurrencyCode;
    searchText: string = "";
    ItemSource: CashBookLinePM[];
    FilteredLines: CashBookLinePM[]; // only non deposited chequeus
    public isRTL: boolean = false;
    private _entityListService: EntityListService = new EntityListService();
    _GLAccountListService: GLAccountListService = new GLAccountListService();
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    ChequesList: ObservableCollection;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.Listen();
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //this.ItemSource = this.EntityPM.CashBookLines;
        this.LoadScreen();


        //if (this.TotalSum > 0) {
        //    this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
        //}

        this.SetUIProperties();
    }

    ngOnInit() {
        this.BuildColumns();
        this.ReloadData();
    }

     //#region Data
     searchFieldFilter: FilterItem;
     public columns: any[] = null;

     ReloadData() {
         this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
     }

     BuildColumns() {
         this.columns = [];

         this.columns.push({
             FieldName: 'ChequeNumber',
             DataTypeCode: 'String',
             Display: TextCodeTranslator.Translate("CashBookLine.F.ChequeNumber"),
             Styles: { width: '90px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });

         this.columns.push({
            FieldName: 'ARPChequeStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("CashBookLine.F.ARPChequeStatusName"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'CashBookLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("CashBookLine.F.DueDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'CashBookLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
            IsCustomTemplate: true
        });
         this.columns.push({
             FieldName: 'LocalAmount',
             DataTypeCode: 'Decimal',
             Display: TextCodeTranslator.Translate("CashBookLine.F.LocalAmount"),
             Styles: { width: '110px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
         this.columns.push({
             FieldName: 'Currency',
             DataTypeCode: 'String',
             Display: TextCodeTranslator.Translate("CashBookLine.F.Currency"),
             Styles: { width: '90px' },
            //  HtmlListComponentName: 'CashBookLineListTemplate',
            //  HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
         this.columns.push({
             FieldName: 'ForeignAmount',
             DataTypeCode: 'Decimal',
             Display: TextCodeTranslator.Translate("CashBookLine.F.ForeignAmount"),
             Styles: { width: '110px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
         this.columns.push({
             FieldName: 'AccountNumber',
             DataTypeCode: 'String',
             Display: TextCodeTranslator.Translate("CashBookLine.F.AccountNumber"),
             Styles: { width: '110px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
         this.columns.push({
             FieldName: 'Bank',
             DataTypeCode: 'String',
             Display: TextCodeTranslator.Translate("CashBookLine.F.Bank"),
             Styles: { width: '75px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
         this.columns.push({
            FieldName: 'Branch',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("CashBookLine.F.Branch"),
            Styles: { width: '75px' },
            HtmlListComponentName: 'CashBookLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
            IsCustomTemplate: true
        });
         this.columns.push({
             FieldName: 'ARPaymentNumber',
             DataTypeCode: 'String',
             Display: TextCodeTranslator.Translate("CashBookLine.F.ARPaymentNumber"),
             Styles: { width: '110px' },
             HtmlListComponentName: 'CashBookLineListTemplate',
             HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
             IsCustomTemplate: true
         });
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
         var filters = new ApiQueryFilters;
         // if (this.dateFilter) {
         //     filters.AdditionalFilters.push(this.dateFilter);
         // } else {
         //     return;
         // }
         if (this.searchFieldFilter) {
             filters.AdditionalFilters.push(this.searchFieldFilter);
         }

         filters.PageSize = 50;
         filters.PageIndex = 0;
         filters.GetCount = true;

        //  filters.SortBy = "Line";
        //  filters.SortDirection = "Ascending";
         var today = new Date();

        if (this.FilterSelectedValue == 'cash')
            filters.addAdditionalFilter("DueDate", today, null, null, "LessThanOrEqual", false, false, false, "DateTime");
        else if (this.FilterSelectedValue == 'postdated')
            filters.addAdditionalFilter("DueDate", today, null, null, "Larger", false, false, false, "DateTime");

         filters.addAdditionalFilter("ARPChequeStatusCode", "5", null, null, "NotEqual", false, false, false, "string");
         filters.addAdditionalFilter("IsDeposited", false, null, null, "Equals", false, false, false, "Boolean");
         filters.addAdditionalFilter("CashBookId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

         //#endregion

         return this._entityListService.getByFilters("CashBookLine", filters);

     }

     //#endregion


    LoadScreen() {
        // this.RemoveDepositedLines();
        this.CalculateTotals();
        this.ComputeFilterTotals();

        // toggle GLAccount editability
        if (this.EntityPM.AccountId)
            this._GLAccountListService.getSingle(this.EntityPM.AccountId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var glaccount: GLAccountList = myResponse.Result;
                        var glaBalance = glaccount.BalanceInLocalCurrency

                        if (this.TotalSum == 0 && (!glaBalance || glaBalance == 0)) {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, true);
                        } else {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
                        }
                        this.SetUIProperties();

                    }
                }
            });
    }


    SetUIProperties() {
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false); // always dim, WI 41740

        if (this.EntityPM.Inactive) {
            this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
            //this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookTypeCode", this.ObjectTableName, false);
        }
    }



    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadScreen();
                        this.SetUIProperties();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        this.LoadScreen();
                        this.SetUIProperties();
                    }
                });
            }
        }
    }

    //#region Properties
    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;

        }
    }

    get AccountId() { return this.EntityPM.AccountId; }
    set AccountId(value: string) {
        if (this.EntityPM.AccountId != value) {
            this.EntityPM.AccountId = value;

        }
    }

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;

        }
    }

    get CashBookTypeCode() { return this.EntityPM.CashBookTypeCode; }
    set CashBookTypeCode(value: string) {
        if (this.EntityPM.CashBookTypeCode != value) {
            this.EntityPM.CashBookTypeCode = value;
        }
    }

    //get AccountId() { return this.EntityPM.AccountId; }
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

    TextChangedOld(searchtext) {
        // this.timerToken = setTimeout(() => {
        //     this.searchText = searchtext;
        //     this.FilterLines();
        // }, 500);
    }

    RefreshButtonClicked() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    OpenARPayment(id) {
        // open ARPayment screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARPayment' });
                });
        }
    }

    CalculateTotals() {
        // if (!AppTool.IsNullOrEmpty(this.ItemSource)) {

        //     this.TotalSum = 0;

        //     if (this.CashBookTypeCode == "1") { //1-cash
        //         this.TotalSum = this.EntityPM.TotalAmount;
        //     } else {
        //         for (let line of this.ItemSource) {
        //             this.TotalSum += line.ForeignAmount;
        //         }
        //     }


        // }
    }

    FilterLines() {
        this.FilterCheques();
        // var lines = this.ItemSource;

        // // Filtering
        // if (!AppTool.IsNullOrEmpty(this.searchText)) {
        //     lines = lines.filter((el) => {
        //         if (el.ChequeNumber != null)
        //             if (el.ChequeNumber.toLowerCase().includes(this.searchText.toLowerCase())) return true;
        //         if (el.AccountNumber != null)
        //             if (el.AccountNumber.toLowerCase().includes(this.searchText.toLowerCase())) return true;
        //         return false;
        //     });
        // }
        // this.ItemSource = lines;

        // this.ChequesList = new ObservableCollection([]);
        // this.ChequesList.InsertCollection(lines, true);

        // this.NoRows = lines.length == 0;
    }

    RemoveDepositedLines() {
        // var lines = this.EntityPM.CashBookLines;

        // // Filtering
        // lines = lines.filter((el) => {
        //     if (el.ARPChequeStatusCode == "5") return false; // 5- Returned to Customer
        //     else if (el.IsDeposited == true) return false;
        //     else return true;
        // });

        // this.FilteredLines = lines;
        // this.ItemSource = this.FilteredLines;

        // this.ChequesList = new ObservableCollection([]);
        // this.ChequesList.InsertCollection(this.FilteredLines, true);

        // this.NoRows = this.FilteredLines.length == 0;
    }

    //#region Filter Methods
    public FilterSelectedValue: string = 'all';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            //this.FilterCheques();
            this.FilterLines(); // set search then filter
        }
    }
    FilterCheques() {
        this.ReloadData();
        // var originalCheques = this.FilteredLines;
        // var filteredQuery = originalCheques;
        // var today = new Date();
        // this.NoRows = false;
        // if (this.FilterSelectedValue == 'cash') {
        //     filteredQuery = originalCheques.filter((el) => {

        //         if (el.DueDate != null) {
        //             var date = new Date(el.DueDate.toString());
        //             if (date <= today) {
        //                 return true;
        //             }
        //             return false;

        //         }
        //         return false;
        //     }); // cash cheques

        // } else if (this.FilterSelectedValue == 'postdated') {
        //     filteredQuery = originalCheques.filter((el) => {

        //         if (el.DueDate != null) {
        //             var date = new Date(el.DueDate.toString());
        //             if (date > today) {
        //                 return true;
        //             }
        //             return false;

        //         }
        //         return false;
        //     }); // postdated cheques
        // }

        // this.ItemSource = filteredQuery;

        // this.ChequesList = new ObservableCollection([]);
        // this.ChequesList.InsertCollection(filteredQuery, true);

        this.CalculateTotals();
        this.ComputeFilterTotals();

    }

    //#endregion
    CashCount: number = 0;
    PostdatesCount: number = 0;
    ComputeFilterTotals() {

        // this.CashCount = 0;
        // this.PostdatesCount = 0;

        // var todayDate = new Date();
        // this.CashCount = this.FilteredLines.filter((el) => {

        //     if (el.DueDate != null) {
        //         var date = new Date(el.DueDate.toString());
        //         if (date <= todayDate) {
        //             return true;
        //         }
        //         return false;

        //     }
        //     return false;
        // }).length;
        // this.PostdatesCount = this.FilteredLines.filter((el) => {

        //     if (el.DueDate != null) {
        //         var date = new Date(el.DueDate.toString());
        //         if (date > todayDate) {
        //             return true;
        //         }
        //         return false;

        //     }
        //     return false;
        // }).length;
    }

}
