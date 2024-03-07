import { Component, OnInit, Output, EventEmitter, AfterViewInit, ChangeDetectorRef, Input, ViewChild } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { GLAccountPM } from '../../../EntityPMs/GLAccountPM';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogGridComponent } from 'Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent';
import { Operators } from 'Accounting/DataContracts/Operators';
import { ReconcileEventManager } from 'Accounting/Utilities/ReconcileEventManager';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ReconciliationExtendedPMService } from 'Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';

@Component({

    templateUrl: './ManageReconciliationsTabComponent.html',
})

export class ManageReconciliationsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: GLAccountPM = null;
    public ObjectTableName = "GLAccount";
    public DataContext = this;

    @ViewChild('DataGrid') DataGrid: LogGridComponent;

    // Events
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public SelectedLines: ObservableCollection = new ObservableCollection([]);

    // Filters
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;
    amountFieldFilter: FilterItem;
    CancelSelectedRecoFeature: boolean = false;
    // Services
    private _entityListService: EntityListService = new EntityListService();

    public isRTL: boolean = false;
    public JournalNumber: string = ""
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {

        super();
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //#region Default date filter value
        if (AppTool.IsNullOrEmpty(entityArgs.EditComponent.JournalNumber)) {
            var today = new Date();
            this.ToDate = new Date();
            var lastmonth = today.setMonth(today.getMonth() - 1); // month backward 

            this.FromDate = new Date(lastmonth);
        }
        else {

            this.ToDate = new Date()
            this.FromDate = new Date(new Date('01/01/2010').setHours(2));
            this.JournalNumber = entityArgs.EditComponent?.JournalNumber;
        }
        this.CancelSelectedRecoFeature = FeatureLocator.HasFeaturePermession("GLAccount", "CancelSelectedReco");
        //#endregion

    }



    ngOnInit() {

        this.BuildColumns();
        this.ReloadData();

    }

    ngOnDestroy() {
        var selectedItems=ReconcileEventManager.GetSelectedItems();
        if(!AppTool.IsNullOrUndefined(selectedItems)){
            selectedItems= [];
        }
        ReconcileEventManager.SetIsAllSelected(false);
    }

    //#region Filters Properties
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.ReloadData();
            }
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.ReloadData();
            }
        }
    }
    operatorsList =
        [{ Code: Operators.Equals, EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
        { Code: Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
        { Code: Operators.LargerThan, EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
        { Code: Operators.LessThan, EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
        { Code: Operators.LessThanOrEqual, EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
        { Code: Operators.GreaterThanOrEqual, EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
        { Code: Operators.Between, EnglishName: 'Between', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Between") },
        ];
    selectedAmountOperator: { Code: string, EnglishName: string, LocalName: string };
    amount: number;
    amountFrom: number;
    amountTo: number;
    get Amount() { return this.amount; }
    set Amount(value: number) {
        if (this.amount != value) {
            this.amount = value;
        }
    }
    //#endregion

    //#region Search 
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
    AmountOperatorChanged($event) {
        this.selectedAmountOperator = $event;
        this.Amount = null;
        this.AmountTextChanged(this.Amount);
    }
    AmountTextChanged(num) {
        if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.amount) && this.selectedAmountOperator) {
            var amountFieldName = 'TransactionAmount';
            this.timerToken = setTimeout(() => {
                if (!AppTool.IsNullOrEmpty(num) && !AppTool.IsNullOrEmpty(this.amount)) {
                    this.amountFieldFilter = new FilterItem(amountFieldName, num, null, null, this.selectedAmountOperator.Code, true, false, false, "number", false);
                    this.RefreshButtonClicked();
                } else {
                    this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
                }
            }, 700);

        } else {
            this.timerToken = setTimeout(() => {
                this.amountFieldFilter = null;
                this.RefreshButtonClicked();
            }, 700);
        }
    }
    AmountTextFromChanged(num) {
        this.amountFrom = num;
        this.filterByAmountFromAndTo();
    }
    AmountTextToChanged(num) {
        this.amountTo = num;
        this.filterByAmountFromAndTo();
    }

    private filterByAmountFromAndTo() {
        if (!AppTool.IsNullOrEmpty(this.amountFrom) && !AppTool.IsNullOrEmpty(this.amountTo) && this.selectedAmountOperator) {
            var amountFieldName = 'TransactionAmount';
            this.timerToken = setTimeout(() => {
                if (!AppTool.IsNullOrEmpty(this.amountFrom) && !AppTool.IsNullOrEmpty(this.amountTo)) {
                    this.amountFieldFilter = new FilterItem(amountFieldName, this.amountFrom, this.amountTo, null, this.selectedAmountOperator.Code, true, false, false, "number", false);
                    this.RefreshButtonClicked();
                } else {
                    this.amountFieldFilter = null;
                    this.RefreshButtonClicked();
                }
            }, 700);

        } else {
            this.timerToken = setTimeout(() => {
                this.amountFieldFilter = null;
                this.RefreshButtonClicked();
            }, 700);
        }
    }
    //#endregion

    //#region Data
    public columns: any[] = null;

    ReloadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    CancelRecoDisabled = true;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Number',
            DataTypeCode: 'String',
            //Display: 'Reconciliation No.',
            Display: TextCodeTranslator.Translate("Reconciliation.F.Number"),
            Styles: { width: '140px' },
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            //Display: 'Create Date',
            Display: TextCodeTranslator.Translate("Reconciliation.F.CreateDate"),
            Styles: { width: '130px' },
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreatedByUserName',
            DataTypeCode: 'String',
            //Display: 'Created By',
            Display: TextCodeTranslator.Translate("Reconciliation.F.CreatedByUserName"),
            Styles: { width: '200px' },
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'IsCancelled',
            DataTypeCode: 'boolean',
            //Display: 'Created By',
            Display: TextCodeTranslator.Translate("Reconciliation.F.IsCancelled"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'ManageReconciliationListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ManageReconciliationListTemplate',
        });

        if (this.CancelSelectedRecoFeature) {
            ReconcileEventManager.ManageReconciliationCheckBoxChecked.subscribe(($event) => {
                debugger;
                if (!AppTool.IsNullOrEmpty($event)) {
                    
                    if ($event.SendSessionIndex != this.CurrentSession.SessionIndex)
                        return;
                    var params = $event.Params;
                    if (params.isChecked) {
                        ReconcileEventManager.InsertIntoSelectedItems(params.line);
                    } else {
                            ReconcileEventManager.RemoveFromSelectedItems(params.line);
                        if (ReconcileEventManager.GetUnAllSelected()) {
                            var selectedItems=ReconcileEventManager.GetSelectedItems();
                            if(!AppTool.IsNullOrUndefined(selectedItems)){
                                selectedItems = [];
                            }
                        }
                    }
                }

                if (ReconcileEventManager.GetIsAllSelected() == false) {
                    this.CancelRecoDisabled = false;
                }
                var selectedItems=ReconcileEventManager.GetSelectedItems();
                if(AppTool.IsNullOrUndefined(selectedItems)|| selectedItems.length === 0){
                    this.CancelRecoDisabled = true;
                } else {
                    this.CancelRecoDisabled = false;
                }
            });
        }
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            this.DataGrid.DetectChangesTimer();
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        //#region Filters
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        } else {
            return;
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.amountFieldFilter) {
            filters.AdditionalFilters.push(this.amountFieldFilter);
        }
        if (this.amountFieldFilter) {
            filters.AdditionalFilters.push(this.amountFieldFilter);
        }


        filters.PageSize = 50;
        filters.PageIndex = 0;
        filters.GetCount = true;

        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("AccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        //#endregion

        return this._entityListService.getByFilters("Reconciliation", filters);

    }

    //#endregion

    //#region Buttons
    RefreshButtonClicked() {
        this.ReloadData();
    }

    //#endregion

    // returned value{ colDef, colIndex, rowData, rowIndex }
    onRowSelected(item) {
        if (ReconcileEventManager.GetSupperssOnRowSelectedAction()) {
            ReconcileEventManager.SetSupperssOnRowSelectedAction(false);
            return;
        }
        if (!AppTool.IsNullOrEmpty(item)) {
            var lineData = item.rowData;
            var entityId = lineData.Id;
            this.OpenReco(entityId);
        }
    }

    CancelSelectedRecoButtonClicked() {
        var selectedItems=ReconcileEventManager.GetSelectedItems();
        if (AppTool.IsNullOrUndefined(selectedItems)|| selectedItems.length === 0) {
            const confirmWindow = new ConfirmWindow();
            const msg = TextCodeTranslator.Translate("GLAccount.O.NoSelectedItems");
            confirmWindow.Show(msg);
            return;
        }
        var selectedItems=ReconcileEventManager.GetSelectedItems();
        const selectedIds: string[] = selectedItems?.reduce(
            (acc, item) => acc.concat(item.Id),
            [] as string[]
        ) || [];
        const confirmWindow = new ConfirmWindow();
        const msg = TextCodeTranslator.Translate("GLAccount.O.CancelSelectedRecoConfirm");
        confirmWindow.Show(msg);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                const service = new ReconciliationExtendedPMService();
                service.CancelSelectedReco(selectedIds).subscribe(
                    (response: ServiceResponse) => {
                        this.RefreshButtonClicked();
                        // Handle success if needed
                        console.log('Reconciliation canceled successfully', response);
                    },
                    (error) => {
                        // Handle error
                        console.error('Error canceling reconciliation', error);
                    }
                );
            }
        });
    }

    AllSelectedClicked(event) {
        this.IsAllSelected = event;
    }

    isAllSelected: boolean = false;
    get IsAllSelected() { return this.isAllSelected; }
    set IsAllSelected(value: boolean) {
        if (value) {
            this.isAllSelected = value;
            ReconcileEventManager.SetIsAllSelected(value);
            ReconcileEventManager.SetUnAllSelected(!value);
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid

        } else {
            this.isAllSelected = value;
            ReconcileEventManager.SetUnAllSelected(!value);
            ReconcileEventManager.SetIsAllSelected(value);
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        }
    }

    OpenReco(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Reconciliation' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.ReloadData();
                        this.CurrentSession.CloseCurrentWindow();
                    });
                    this.CurrentSession.CloseCurrentWindow();
                });

        }
    }


}

