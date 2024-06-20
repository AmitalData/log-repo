import { CashBookExtendedPMService } from './../../../Services/ExtendedPMs/CashBookExtendedPMService';
import { Output,OnInit } from '@angular/core';
import { EventEmitter } from '@angular/core';
import {Component} from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CashBookPM } from '../../../EntityPMs/CashBookPM';
import { GLAccountList } from '../../../EntityLists/GLAccountList';
import { CashBookLinePM } from '../../../EntityPMs/CashBookLinePM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountListService } from '../../../Services/StandardLists/GLAccountListService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CashbookChequesCounter } from '../../../DataContracts/CashbookChequesCounter';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

const CashbookTotalUpdateWindow = "Adjust Cashbook Total";
const CashbookUpdateTotalWindowWidth = 400;
const CashbookUpdateTotalWindowHeight = 180;
const CashCashbookTypeCode = '1';
@Component({

    templateUrl: './CashBookDetailsTabComponent.html',
})

export class CashBookDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    public DataContext = this;
    public filterAgrs: ApiQueryFilters;
    public TotalSum = 0;
    public NoRows: boolean = false;
    tenantCurrency: string = SessionLocator.TenantPM.CurrencyCode;
    searchText: string = "";
    ItemSource: CashBookLinePM[];
    FilteredLines: CashBookLinePM[]; // only non deposited chequeus
    public isRTL: boolean = false;
    private _entityListService: EntityListService = new EntityListService();
    _GLAccountListService: GLAccountListService = new GLAccountListService();
    _CashBookExtendedPMService: CashBookExtendedPMService = new CashBookExtendedPMService();

    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    ChequesList: ObservableCollection;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.Listen();
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


        this.SetUIProperties();
    }

    get ShowAdjustTotalButton() {
        return SessionLocator?.LoggedUserPM?.IsCustomerCare;
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

        this.LoadScreen();
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
            ServerSideSortable: true,
            SortByName: 'DueDate',
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
            ServerSideSortable: true,
            SortByName: 'ARPaymentNumber',
            Display: TextCodeTranslator.Translate("CashBookLine.F.ARPaymentNumber"),
            Styles: { width: '110px' },
            HtmlListComponentName: 'CashBookLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CardLocalName',
            DataTypeCode: 'String',
            Display:  TextCodeTranslator.Translate("CashbookLine.O.LocalName"),
            Styles: { width: '110px' },
            HtmlListComponentName: 'CashBookLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CashBookLineListTemplate',
            IsCustomTemplate: true
        });
        //this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingCol: "DueDate",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CreateApiQueryFilters(take, skip, sortingCol, sortingDir);
        return this._entityListService.getByFilters("CashBookLine", this.filterAgrs);

    }
    private CreateApiQueryFilters(take: any, skip: any, sortingCol: any, sortingDir: any) {
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
        if (this.searchFieldFilter) {
            this.filterAgrs.AdditionalFilters.push(this.searchFieldFilter);
        }
        var today = new Date();

        if (this.FilterSelectedValue == 'cash')
        this.filterAgrs.addAdditionalFilter("DueDate", today, null, null, "LessThanOrEqual", false, false, false, "DateTime");
        else if (this.FilterSelectedValue == 'postdated')
        this.filterAgrs.addAdditionalFilter("DueDate", today, null, null, "LargerThan", false, false, false, "DateTime");

        this.filterAgrs.addAdditionalFilter("ARPChequeStatusCode", "1,4", null, null, "InListExact", false, false, false, "string");
        this.filterAgrs.addAdditionalFilter("IsDeposited", false, null, null, "Equals", false, false, false, "Boolean");
        this.filterAgrs.addAdditionalFilter("CashBookId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

    }
    //#endregion


    LoadScreen() {
        this.GetChequesCounter();
        this.CalculateTotalAmount();
        this.ToggleGLAccountEditablity();
    }


    private ToggleGLAccountEditablity() {
        if (this.EntityPM.AccountId)
            this._GLAccountListService.getSingle(this.EntityPM.AccountId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var glaccount: GLAccountList = myResponse.Result;
                        var glaBalance = glaccount.BalanceInLocalCurrency;
                        if (this.TotalSum == 0 && (!glaBalance || glaBalance == 0)) {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, true);
                        }
                        else {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
                        }
                        this.SetUIProperties();
                    }
                }
            });
    }

    SetUIProperties() {
        //if (this.TotalSum > 0) {
        //    this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
        //}

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
                        this.ReloadData();
                        this.SetUIProperties();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        this.ReloadData();
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

    CalculateTotalAmount() {

        var chequeFilterType = "All";
        if (this.FilterSelectedValue == 'cash')
            chequeFilterType = "CashCheque"
        else if (this.FilterSelectedValue == 'postdated')
            chequeFilterType = "PostdatedCheque"




        this._CashBookExtendedPMService.GetCashbookTotalAmount(this.EntityPM.Id, chequeFilterType)
            .subscribe((response: ServiceResponse) =>
            {
                console.log("[GetCashbookTotalAmount]", response);

                if (!response.HasError) {
                    this.TotalSum = response.Result;
                }
                else {
                    console.error(response.ErrorsArray);
                }
            });

    }

    //#region Filter Methods
    public FilterSelectedValue: string = 'all';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.ReloadData();
        }
    }

    //#endregion


    public ChequesCounter: CashbookChequesCounter = new CashbookChequesCounter();
    GetChequesCounter() {
        this.ChequesCounter = new CashbookChequesCounter();
        this._CashBookExtendedPMService.GetCashbookChequesCounter(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) => {
                console.log("[GetCashbookChequesCounter]", response);

                if (!response.HasError) {
                    this.ChequesCounter = response.Result;
                }
                else {
                    console.error(response.ErrorsArray);
                }
            });

    }

    RecalculateCashbookTotals() {
        if (this.EntityPM.CashBookTypeCode == CashCashbookTypeCode)
            this.ShowCashbookTotalUpdateWindow();
    }

    private ShowCashbookTotalUpdateWindow() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = CashbookUpdateTotalWindowWidth;
        logitudeWindow.Height = CashbookUpdateTotalWindowHeight;
        logitudeWindow.Title = CashbookTotalUpdateWindow;
        logitudeWindow.WindowArgs = { CashbookPM: this.EntityPM };
        logitudeWindow.Show('./Accounting/Components/EditTabs/CashBook/CashbookTotalAdjustWindow');
        logitudeWindow.WindowClosed.subscribe(() => this.ReloadData());
    }

}
