declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { AutomationSetValue } from 'Infrastructure/DataContracts/AutomationSetValue';
import { AutomationSetValueViewModel } from '../Maintenance/Automation/ViewModel/AutomationSetValueViewModel';
import { ObjectFieldPM } from 'Infrastructure/EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from 'Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from 'Infrastructure/Tools';
import { MultiEntityUpdateLogPMService } from 'Infrastructure/Services/StandardPMs/MultiEntityUpdateLogPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { MultiEntityUpdateLogPM } from 'Infrastructure/EntityPMs/MultiEntityUpdateLogPM';
import { MultiEntityUpdateData } from 'Infrastructure/DataContracts/MultiEntityUpdateData';
import { MultiEntityUpdateDataEntity } from 'Infrastructure/DataContracts/MultiEntityUpdateDataEntity';

@Component({
    selector: 'MultiUpdateComponent',
    templateUrl: 'MultiUpdateComponent.html',
})

export class MultiUpdateComponent extends BaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    Filters: ApiQueryFilters;
    QueryCode: string;
    ObjectTableName: string;
    ObjectTableId: string;
    IsMultiEntityUpdatedSuccessfully: boolean = false;
    IsMultiUpdateComponent: boolean = true;
    ParentComponent: any;
    ValidationErrorsList: any[];

    @Output() MenuHeaderchangeevent = new EventEmitter();


    SelectedItemsCountText: string = null;
    AllRecordsCount: number = 0;
    MethodName: string = null;
    public columns: any[] = null;
    SelectedRecords: any[] = [];
    SelectedRecordsCount: number = 0;
    public items: any[] = [];

    Title: string;
    AllRecords: any[] = [];

    public DataContext: MultiUpdateComponent = this;
    AutomationSetValueLists: AutomationSetValueViewModel[] = [];
    AutomationSetValuebjectFieldLists: ObjectFieldPM[] = [];
    ObjectFieldsLists: ObjectFieldPM[] = [];
    public ObjectTable: ObjectTablePM;


    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    multiEntityUpdateLogPMService: MultiEntityUpdateLogPMService;

    @Output() SearchFieldchangeevent = new EventEmitter();

    constructor(private _entityListService: EntityListService) {
        super();
        this.multiEntityUpdateLogPMService = new MultiEntityUpdateLogPMService();
        window.AllRecords = [];
        this.Listen();
    }


    ngOnInit() {
    }

    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    public IsAllRecordSelected: boolean;


    public IsAllRecordSelectedChange(isChecked, applyForAll, refreshList = true) {
        this.IsAllRecordSelected = isChecked;
        if (applyForAll) {
            this.ChangeAllRecordsSelection(isChecked, refreshList);
        }
    }

    private ChangeAllRecordsSelection(isChecked: boolean, refreshList: boolean) {
        isChecked == true ? this.CheckAllRecords() : this.UnCheckAllRecords();
        if (refreshList) {
            this.RefreshList();
        }
    }
    RefreshList() {
        window.AllRecords = this.AllRecords;
        this.SearchFieldchangeevent.emit("");
    }

    private CheckAllRecords() {
        this.AllRecords.forEach(element => {
            element.IsChecked = true;
        });

        this.ChangeSelectedItemsCountText(this.AllRecordsCount);
        this.SelectedRecordsCount = this.AllRecords.length;
        this.SelectedRecords = this.AllRecords;
    }

    private UnCheckAllRecords() {
        this.AllRecords.forEach(element => {
            element.IsChecked = false;
        });

        this.ChangeSelectedItemsCountText(0);
        this.SelectedRecords = [];
        this.SelectedRecordsCount = 0;
    }

    private ChangeSelectedItemsCountText(selectedCount) {
        this.SelectedItemsCountText = selectedCount + " of " + this.AllRecordsCount + " " + this.ObjectTable.DBTableName + " selected";
    }

    dataSource = {
        pageSize: 100,
        rowCount: null,
        sortingCol: "",
        sortingDir: "",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };


    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        filters = this.Filters;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.PageIndex = 0;
        filters.PageSize = 100;
        return this._entityListService.getByFilters(this.ObjectTableName, filters, null);
    }

    SelectedRow: any;
    onRowSelected(CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
    }

    GridAfterViewInitCompleted($event) {
        this.LoadList();
    }

    FirstTime: boolean = true;
    AllRecordsReady(result) {
        if(!this.FirstTime){
            return;
        }
        this.FirstTime = false;
        this.AllRecords = result;
        this.SelectedRecords = result;
        window.AllRecords = result;
        this.IsAllRecordSelectedChange(true, true, false);
    }

    SetWindowArgs(windowArgs: any) {
        let args: any = windowArgs.args;
        this.ParentComponent = windowArgs.parentComponent;
        this.QueryCode = args.QueryCode;
        this.Filters = this.Clone(args.Filters);
        this.columns = this.Clone(args.Columns);
        this.ObjectTableName = args.ObjectTable.Name;
        this.ObjectTable = args.ObjectTable;
        this.ObjectTableId = args.ObjectTable.Id;
        this.Title = args.Title;

        this.InitGrid();
        this.FillObjectField();
    }


    private InitGrid() {
        this.Filters.GetCount = true;
        var checkBoxColumn = {
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'MultiUpdateCheckBoxComponent',
            HtmlListComponentUrl: './Infrastructure/Components/MultiUpdateComponent/MultiUpdateCheckBoxComponent',
        };

        var updateSuccess = {
            FieldName: 'UpdateSuccess',
            DataTypeCode: 'Boolean',
            Display: 'Update Status',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'MultiUpdateCheckTemplate',
            HtmlListComponentUrl: './Infrastructure/Components/MultiUpdateComponent/MultiUpdateCheckTemplate',
        };
        this.columns.splice(0, 0, checkBoxColumn);
        this.columns.splice(1, 0, updateSuccess);
    }

    OnUpdateFinish(entities) {
        this.AllRecords.forEach(function (record) {
            var entity = entities.find(item => item.EntityId == record.Id);
            record.UpdateSuccess = entity ? !entity.HasException : undefined;
        });

        this.RefreshList();
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    onCountReady(count) {
        if(!this.FirstTime){
            return;
        }
        this.SelectedRecordsCount = count;
        this.AllRecordsCount = count;
    }

    LoadList() {
        this.Filters.SortBy = this.Filters.SortBy;
        this.Filters.SortDirection = this.Filters.SortDirection;
        this.MenuHeaderchangeevent.emit({ Filters: this.Filters, IgnoreFilter: false });
    }

    private ListenEvent: any = null;
    Listen() {
        this.ListenEvent = this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
            var temp = this.SelectedRecords.filter(a => a.Id == res.Id);
            this.ChangeItemCheck(res);
            if (res.IsChecked && temp.length == 0) {
                this.AddToSelectedRecords(res);
                return;
            }
            if (temp.length > 0) {
                this.RemoveFromSelectedRecords(res);
            }
        });
    }

    private ChangeItemCheck(res: any) {
        this.AllRecords.forEach(function (record) {
            if (record.Id == res.Id)
                record.IsChecked = res.IsChecked;
        });
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null
    }


    private RemoveFromSelectedRecords(event: any) {
        this.SelectedRecords = this.SelectedRecords.filter(a => a.Id != event.Id);
        this.SelectedRecordsCount--;
        this.IsAllRecordSelectedChange(false, false);
        this.ChangeSelectedItemsCountText(this.SelectedRecordsCount);
    }

    private AddToSelectedRecords(event: any) {
        this.SelectedRecords.push(event);
        this.SelectedRecordsCount++;
        if (this.SelectedRecordsCount == this.AllRecords.length) {
            this.IsAllRecordSelectedChange(true, false);
        }
        this.ChangeSelectedItemsCountText(this.SelectedRecordsCount);
    }

    FillObjectField() {
        this.AutomationSetValuebjectFieldLists = [];
        this.ObjectFieldsLists = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTable.Id);
        this.ObjectFieldsLists.forEach((objectField) => {
            if (objectField.CanAutomateSetValue || objectField.IsCustom) this.AutomationSetValuebjectFieldLists.push(objectField);
        });
    }


    AddAutomationSetValueButtonClick() {
        var automationSetValue: AutomationSetValue = new AutomationSetValue();
        automationSetValue.ObjectFieldCode = "";
        automationSetValue.OperatorCode = "SV";
        automationSetValue.Value = "";
        automationSetValue.FieldName = "";
        automationSetValue.IsCustomField = false;
        this.AutomationSetValueLists.push(new AutomationSetValueViewModel(automationSetValue, this));
    }

    NextClicked() {
        this.ParentComponent.NextButtonClicked();
    }

    UpdateClick() {
        if (!this.IsUpdateValid()) {
            return;
        }


        var multiEntityUpdateLog = this.BuildMultiEntityUpdateLog();

        this.CurrentSession.StartBusyIndicatorLoading();
        this.multiEntityUpdateLogPMService.insert(multiEntityUpdateLog).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ParentComponent.UpdateButtonClicked(myResponse.Result, this.multiEntityUpdateLogPMService);
            }
        });
    }

    BuildMultiEntityUpdateLog(): MultiEntityUpdateLogPM {
        var multiEntityUpdateLog = new MultiEntityUpdateLogPM();

        multiEntityUpdateLog.Tenant = SessionLocator.Tenant;
        multiEntityUpdateLog.ObjectTableId = this.ObjectTableId;
        multiEntityUpdateLog.MultiEntityUpdateData = this.BuildMultiEntityUpdateData();
        return multiEntityUpdateLog;
    }

    BuildMultiEntityUpdateData(): MultiEntityUpdateData {
        var multiEntityUpdateData = new MultiEntityUpdateData();

        var setValueList: AutomationSetValue[] = [];
        this.AutomationSetValueLists.forEach((item) => {
            setValueList.push(item.CurrentEntityPM);
        });

        var entities: MultiEntityUpdateDataEntity[] = [];

        this.SelectedRecords.forEach((item) => {
            entities.push(this.DataEntityMap(item));
        });

        multiEntityUpdateData.UserId = SessionLocator.LoggedUserId;
        multiEntityUpdateData.SetValueLists = setValueList;
        multiEntityUpdateData.Entities = entities;
        multiEntityUpdateData.ObjectTableId = this.ObjectTableId;
        multiEntityUpdateData.ObjectTableName = this.ObjectTableName;

        return multiEntityUpdateData;

    }

    DataEntityMap(item: any): MultiEntityUpdateDataEntity {
        var entity = new MultiEntityUpdateDataEntity();
        entity.EntityId = item.Id;
        entity.EntityNumber = this.ObjectTableName == "Shipment" ? item.ShipmentNumber : item.ContainerNumber
        entity.Tenant = SessionLocator.Tenant;
        entity.StatusCode = "W";
        return entity;
    }

    IsUpdateValid(): boolean {
        this.ValidationErrorsList = [];

        if ((!this.AutomationSetValueLists || this.AutomationSetValueLists.length == 0)) {
            this.ValidationErrorsList.push("Please add at least one set Value");
        }

        if (!this.SelectedRecordsCount || this.SelectedRecordsCount === 0) {
            this.ValidationErrorsList.push("Please select at least one item to update");
        }

        this.AutomationSetValueLists.forEach((item) => {
            this.AutomationSetValueValidation(item);
        });

        if (this.ValidationErrorsList.length != 0) {
            return false;
        }

        return true;
    }

    private AutomationSetValueValidation(item: AutomationSetValueViewModel) {
        if (AppTool.IsNullOrEmpty(item.CurrentEntityPM.ObjectFieldCode)) {
            this.ValidationErrorsList.push("Please select at least one field to be updated");
            return;
        }
        if (AppTool.IsNullOrEmpty(item.CurrentEntityPM.Value)) {
            this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " field is required");
            return;
        }
        if (item.SelectedCustomField.DataTypeCode != "Text" && item.SelectedCustomField.DataTypeCode != "nText")
            return;
        if (this.IsNotValidAutomationSetValue(item)) {
            this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " must between " + item.SelectedCustomField.MinLength + " and " + item.SelectedCustomField.MaxLength + " characters");
            return;
        }
        if (this.IsNotValidAutomationSetField(item)) {
            this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " must between " + item.SelectedCustomField.MinLength + " and " + item.SelectedCustomField.MaxLength + " characters");
            return;
        }
    }


    private IsNotValidAutomationSetField(item: AutomationSetValueViewModel) {
        if (item.SelectedOperator.Code != "SF")
            return false;

        if (item.AutomationHelper.ConditionMaxLength > item.SelectedCustomField.MaxLength || item.AutomationHelper.ConditionMinLength > item.SelectedCustomField.MinLength)
            return true;


        return false;
    }

    private IsNotValidAutomationSetValue(item: AutomationSetValueViewModel) {
        if (item.SelectedOperator.Code != "SV")
            return false;

        if (item.CurrentEntityPM.Value.length > item.SelectedCustomField.MaxLength || item.CurrentEntityPM.Value.length < item.SelectedCustomField.MinLength)
            return true;

        return false;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
