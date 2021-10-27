declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { AutomationSetValue } from 'Infrastructure/DataContracts/AutomationSetValue';
import { AutomationSetValueViewModel } from '../Maintenance/Automation/ViewModel/AutomationSetValueViewModel';
import { ObjectFieldPM } from 'Infrastructure/EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from 'Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from 'Infrastructure/Tools';

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

    ValidationErrorsList: any[];

    @Output() MenuHeaderchangeevent = new EventEmitter();


    SelectedItemsCountText: string = null;
    AllRecordsCount: number = 0;
    MethodName: string = null;
    public columns: any[] = null;
    SelectedRecords: any[] = [];
    SelectedRecordsCount: number = 0;
    public items: any[] = [];

    private isAllRecordSelected: boolean;
    Title: string;
    AllRecords: any[] = [];

    public DataContext: MultiUpdateComponent = this;
    AutomationSetValueLists: AutomationSetValueViewModel[] = [];
    AutomationSetValuebjectFieldLists: ObjectFieldPM[] = [];
    ObjectFieldsLists: ObjectFieldPM[] = [];
    public ObjectTable: ObjectTablePM;
    private isInit = true;


    public get IsAllRecordSelected() { return this.isAllRecordSelected };
    public set IsAllRecordSelected(value: boolean) {
        this.isAllRecordSelected = value;
        if (value == true) {
            this.SelectedItemsCountText = this.AllRecordsCount + " of " + this.AllRecordsCount + " " + this.Title + " selected";
            this.SelectedRecordsCount = this.AllRecords.length;
            this.SelectedRecords = this.AllRecords;
        }
        else {
            this.SelectedItemsCountText = "0 of " + this.AllRecordsCount + " " + this.Title + " selected";
            this.SelectedRecords = [];
            this.SelectedRecordsCount = 0;
        }

    }


    constructor(private _entityListService: EntityListService) {
        super();
    }
    ngOnInit() {
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
        return this._entityListService.getByFilters(this.ObjectTableName, this.Filters, null);
    }

    SelectedRow: any;
    onRowSelected(CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
    }

    GridAfterViewInitCompleted($event) {
        this.LoadList();
    }

    OnDataLoaded(result) {
        this.AllRecords = result.map(({ rowData }) => rowData);
        if (this.isInit) {
            this.IsAllRecordSelected = true;
            this.isInit = false;
            this.SelectedRecords = result.map(({ rowData }) => rowData).filter(a => a.IsChecked === undefined);
            return;
        }
        this.SelectedRecords = result.map(({ rowData }) => rowData).filter(a => a.IsChecked === true);
    }

    SetWindowArgs(args: any) {
        this.QueryCode = args.QueryCode;
        this.Filters = this.Clone(args.Filters);
        this.columns = this.Clone(args.Columns);
        this.ObjectTableName = args.ObjectTable.Name;
        this.ObjectTable = args.ObjectTable;
        this.ObjectTableId = args.ObjectTable.Id;
        this.Title = args.Title;

        this.initGrid();
        this.FillObjectField();
    }


    private initGrid() {
        this.Filters.GetCount = true;
        var checkBoxColumn = {
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: 'Checked',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            IsCheckBox: true,
        };
        this.columns.splice(0, 0, checkBoxColumn);
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    onCountReady(count) {
        this.SelectedRecordsCount = count;
        this.AllRecordsCount = count;
    }

    LoadList() {
        this.Filters.SortBy = this.Filters.SortBy;
        this.Filters.SortDirection = this.Filters.SortDirection;
        this.MenuHeaderchangeevent.emit({ Filters: this.Filters, IgnoreFilter: false });
    }

    onCheckBoxChecked(event) {
        var temp = this.SelectedRecords.filter(a => a.Id == event.rowData.Id);
        if (event.IsChecked && temp.length == 0) {
            this.SelectedRecords.push(event.rowData);
            this.SelectedRecordsCount++;
            this.SelectedItemsCountText = this.SelectedRecordsCount + " of " + this.AllRecordsCount + " " + this.Title + " selected";
            return;
        }
        if (temp.length > 0) {
            this.SelectedRecords = this.SelectedRecords.filter(a => a.Id != event.rowData.Id);
            this.SelectedRecordsCount--;
            this.SelectedItemsCountText = this.SelectedRecordsCount + " of " + this.AllRecordsCount + " " + this.Title + " selected";
        }
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


    UpdateClick() {
        if (!this.IsUpdateValid()) {
            return;
        }

        var automationSetValuelist: AutomationSetValue[] = [];
        this.AutomationSetValueLists.forEach((item) => {
            automationSetValuelist.push(item.CurrentEntityPM);
        });


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
            if (AppTool.IsNullOrEmpty(item.CurrentEntityPM.Value)) {
                this.ValidationErrorsList.push(((item.SelectedCustomField?.FullNameTextCodeDefaultText) == undefined ? "" : item.SelectedCustomField.FullNameTextCodeDefaultText) + " field is required");
                return;
            }

            if (this.isNotValidAutomationTextValue(item)) {
                this.ValidationErrorsList.push(item.SelectedCustomField.FullNameTextCodeDefaultText + " must butween " + item.SelectedCustomField.MinLength + " and " + item.SelectedCustomField.MaxLength + " characters");
                return;
            }
        });

        if (this.ValidationErrorsList.length != 0) {
            return false;
        }

        return true;
    }

    private isNotValidAutomationTextValue(item: AutomationSetValueViewModel) {
        if (item.SelectedCustomField.DataTypeCode != "Text" && item.SelectedCustomField.DataTypeCode != "nText")
            return false;

        if (item.CurrentEntityPM.Value.length > item.SelectedCustomField.MaxLength || item.CurrentEntityPM.Value.length < item.SelectedCustomField.MinLength)
            return true;

        return false;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
