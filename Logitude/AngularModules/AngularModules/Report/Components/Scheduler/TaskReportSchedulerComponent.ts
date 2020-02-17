import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { InfrastructureDomainService } from '../../../Infrastructure/Services/InfrastructureDomainService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TaskSchedulerHistoryList } from '../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import { TasksSchedulerPM } from '../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ReportSchedulerDetails, SchedulerDetails } from '../../DataContracts/SchedulerDetails';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';

@Component({
    moduleId: module.id,
    templateUrl: './TaskReportSchedulerComponent.html',
})

export class TaskReportSchedulerComponent implements OnInit {
    public ItemsSource: TaskReportSchedulerItemClass[] = [];
    public FixedItemsSource: TaskReportSchedulerItemClass[] = [];
    public HistoryItemsSource: TaskSchedulerHistoryList[] = [];
    public TasksHistoryColumns: any[] = null;
    public Taskscolumns: any[] = null;
    private loadedDataList: TasksSchedulerPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ReportGroupList: ReportGroupList;
    public ReportList: ReportList;
    filterAgrs: ApiQueryFilters;
    SchedulerType: string = "Report";

    @Output() TasksHistoryCustomColumnsReady = new EventEmitter();
    @Output() TasksCustomColumnsReady = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() MenuHeaderchangeeventTasks = new EventEmitter();

    constructor(private _entityListService: EntityListService) {
    }

    ngOnInit() {
        this.LoadTaskSchedulers();
        this.LoadTaskHistories();
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadTasks") {
                this.RefreshButtonClicked();
            } 
        });
    }

    SetWindowArgs(windowArgs) {
        this.ReportGroupList = windowArgs.ReportGroupList;
        this.ReportList = windowArgs.ReportList;
    }

    public IsHistoryGridVsisible = false;
    public SelectedRow: any;
    onRowSelected(item: any) {
        this.SelectedRow = item.rowData;

        if (item == null) {
            this.IsHistoryGridVsisible = false;
        }

        else {
            this.LoadTaskHistories();
        }
    }

    NewTaskClicked() {
        var newItem: TasksSchedulerPM = new TasksSchedulerPM();
        newItem.CreatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.UpdatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.TriggerType = "O";
        newItem.Tenant = SessionLocator.Tenant;
        newItem.Type = this.SchedulerType;

        var windowArgs: any = {};
        windowArgs.ReportGroupList = this.ReportGroupList;
        windowArgs.ReportList = this.ReportList;

        var logWindow = new LogitudeWindow();
        logWindow.Height = 820;
        logWindow.Width = 900;
        logWindow.Title = "Report Scheduler Details";
        logWindow.DataContext = new TaskReportSchedulerItemClass(newItem, this, true);
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Report/Components/Scheduler/AddEditReportSchedulerComponent');
        logWindow.WindowClosed.subscribe(closed => {
            if (closed) {
                this.RefreshButtonClicked();
            }
        });
    }

    public RefreshTasksSchedular(entityPM: TasksSchedulerPM) {
        var index = this.loadedDataList.indexOf(entityPM);
        if (index > -1) {
            this.loadedDataList[index] = entityPM;
        }
        else {
            this.loadedDataList.push(entityPM);
        }

        this.BuildItemsSource();
    }

    BuildItemsSource() {
        this.ItemsSource = [];
        this.FixedItemsSource = [];
        this.loadedDataList.forEach(item => {
            this.ItemsSource.push(new TaskReportSchedulerItemClass(item, this));
            this.FixedItemsSource.push(new TaskReportSchedulerItemClass(item, this));
        });
        if (this.filterTypeCode) {

            if (this.filterTypeCode == "AL") {
                this.ItemsSource = this.FixedItemsSource;
            }
            else if (this.filterTypeCode == "IN") {
                this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == true);
            }
            else {
                this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == false);;
            }
        }
        else {
            this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == false);
        }


        this.CurrentSession.StopBusyIndicator();
    }

    RefreshButtonClicked() {
        this.LoadTaskSchedulers();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    BuildTasksColumns() {
        this.Taskscolumns = [];
        this.Taskscolumns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            Display: 'Name',
            Styles: { width: '200px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "Name"
        });

        this.Taskscolumns.push({
            FieldName: "Description",
            DataTypeCode: 'String',
            Display: 'Description',
            Styles: { width: '200px' },
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.Taskscolumns.push({
            FieldName: "LastRunEndTime",
            DataTypeCode: 'String',
            Display: 'Last Run Date',
            Styles: { width: '160px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "LastRunEndTime"
        });

        this.Taskscolumns.push({
            FieldName: "InActive",
            DataTypeCode: 'Boolean',
            Display: 'In Active',
            Styles: { width: '90px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "InActive"
        });

        var test: any = [];
        test.ReportGroupList = this.ReportGroupList;
        test.ReportList = this.ReportList;
        this.Taskscolumns.push({
            FieldName: "EditTaskButton;" + this.SchedulerType,
            AdditionalData: test,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.TasksCustomColumnsReady.emit(this.Taskscolumns);
    }

    BuildTasksHistoryColumns() {
        this.TasksHistoryColumns = [];
        this.TasksHistoryColumns.push({
                FieldName: "StartDateTime",
                DataTypeCode: 'String',
                Display: 'Start Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "StartDateTime"
            });
        this.TasksHistoryColumns.push({
                FieldName: "EndDateTime",
                DataTypeCode: 'String',
                Display: 'End Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "EndDateTime"
            });
        
        this.TasksHistoryColumns.push({
            FieldName: "Duration",
            DataTypeCode: 'String',
            Display: 'Duration',
            Styles: { width: '100px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.TasksHistoryColumns.push({
            FieldName: "Status",
            DataTypeCode: 'String',
            Display: 'Status',
            Styles: { width: '90px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        //this.columns.push({
        //    FieldName: "ViewLog",
        //    DataTypeCode: 'String',
        //    Display: '',
        //    Styles: { width: '100px' },
        //    HtmlListComponentName: 'SchedulerDateListTemplate',
        //    HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
        //    IsCustomTemplate: true,
        //    ServerSideSortable: false
        //});
        
        //this.columns.push({
        //    FieldName: "RunResult",
        //    DataTypeCode: 'String',
        //    Display: 'Run Result',
        //    Styles: { width: '200px' },
        //    IsCustomTemplate: true,
        //    ServerSideSortable: false
        //});
        this.TasksHistoryCustomColumnsReady.emit(this.TasksHistoryColumns);
    }

    TasksHistoryDataSource = {
        pageSize: 20,
        rowCount: null,

        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getTasksHistoryRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    TasksDataSource = {
        pageSize: 20,
        rowCount: null,

        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getTasksRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    getTasksHistoryRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        filters = new ApiQueryFilters();
        if (!sortingCol) {
            sortingCol = "StartDateTimeUTC";
            sortingDir = "descending";
        }
        if (this.SelectedRow) {
            if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
                if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
                    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
                }
                filters.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Equals", false, false, false, "String");
            }
        }
        else {
            filters.addAdditionalFilter("TaskId", "0-0", null, null, "Equals", false, false, false, "String");
        }

        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator.Tenant;

        return this._entityListService.getByFilters("TaskSchedulerHistory", filters);
    }

    getTasksRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        filters = new ApiQueryFilters();
        if (!sortingCol) {
            sortingCol = "NextRunTime";
            sortingDir = "descending";
        }

        if (this.filterTypeCode == "AL") {
            filters.AdditionalFilters = [];
        }
        else if (this.filterTypeCode == "IN") {
            if (filters.AdditionalFilters.filter(a => a.FieldName == "InActive").length > 0) {
                filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "InActive");
            }
            filters.addAdditionalFilter("InActive", true, null, null, "Equals", true, false, false, "Boolean");
        }
        else {
            if (filters.AdditionalFilters.filter(a => a.FieldName == "InActive").length > 0) {
                filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "InActive");
            }
            filters.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "Boolean");
        }
        if (filters.AdditionalFilters.filter(a => a.FieldName == "Type").length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "Type");
        }
        filters.addAdditionalFilter("Type", this.SchedulerType, null, null, "Equals", true, false, false, "String");
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator.Tenant;

        return this._entityListService.getByFilters("TasksScheduler", filters);
    }

    private LoadTaskSchedulers() {

        this.BuildTasksColumns();

        this.filterAgrs = new ApiQueryFilters();
        

        if (this.filterTypeCode == "AL") {
            this.filterAgrs.AdditionalFilters = []; 
        }
        else if (this.filterTypeCode == "IN") {
            this.filterAgrs.addAdditionalFilter("InActive", true, null, null, "Equals", true, false, false, "Boolean");
        }
        else {
            this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "Boolean");
        }

        this.filterAgrs.addAdditionalFilter("Type", this.SchedulerType, null, null, "Equals", true, false, false, "String");

        this.MenuHeaderchangeeventTasks.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    private LoadTaskHistories() {
        this.IsHistoryGridVsisible = true;
        this.BuildTasksHistoryColumns();

        this.filterAgrs = new ApiQueryFilters();
        if (!this.SelectedRow) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
            this.filterAgrs.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Contains", true, false, false, "String");
        }

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    private filterTypeCode: string = "AC";
    public get FilterTypeCode() { return this.filterTypeCode; }
    public set FilterTypeCode(value: string) {
        if (this.filterTypeCode != value) {
            this.filterTypeCode = value;
            this.LoadTaskSchedulers();
        }
    }
}

export class TaskReportSchedulerItemClass extends BaseComponent {
    public EntityPM: TasksSchedulerPM;
    public ObjectTableName: string = "TasksScheduler";
    public IsNew: boolean = false;
    private newValueinDateFormat: Date;

    ReportDetails: ReportSchedulerDetails;
    SchedulerDetailsData: SchedulerDetails = new SchedulerDetails();

    constructor(item: TasksSchedulerPM, public fatherComponent: TaskReportSchedulerComponent, isNew: boolean = false) {
        super();
        this.EntityPM = item;
        this.IsNew = isNew;
    }

    get Id() { return this.EntityPM.Id; }
    get CreateDate() { return this.EntityPM.CreateDateTime; }
    get UpdateDate() { return this.EntityPM.UpdateDateTime; }
    get LastRunTime() { return this.EntityPM.LastRunStartTime; }
    get NextRunTimeUTC() { return this.EntityPM.NextRunTimeUTC; }
    get StartDate() { return this.EntityPM.StartDateTime; }
    get Status() { return this.EntityPM.Status; }
    get LastRunEndTime() { return this.EntityPM.LastRunEndTime; }
    get Duration() { return Math.abs(this.EntityPM.Duration); }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get StartDateTime() { return this.EntityPM.StartDateTime; }
    set StartDateTime(newValue: Date) {

        if (this.EntityPM.StartDateTime != newValue) {
            this.EntityPM.StartDateTime = newValue;
            this.newValueinDateFormat = new Date(newValue);
            this.EntityPM.StartDateTimeUTC = new Date(this.newValueinDateFormat.getUTCFullYear(), this.newValueinDateFormat.getUTCMonth(), this.newValueinDateFormat.getUTCDate(), this.newValueinDateFormat.getUTCHours(), this.newValueinDateFormat.getUTCMinutes(), this.newValueinDateFormat.getUTCSeconds(), this.newValueinDateFormat.getUTCMilliseconds());

        }
    }

    get TriggerType() { return this.EntityPM.TriggerType; }
    set TriggerType(newValue: string) {
        if (this.EntityPM.TriggerType != newValue) {
            this.EntityPM.TriggerType = newValue;
        }
    }
    get MonthlyDay() { return this.EntityPM.MonthlyDay; }
    set MonthlyDay(newValue: number) {
        if (this.EntityPM.MonthlyDay != newValue) {
            this.EntityPM.MonthlyDay = newValue;
        }
    }

    get Satarday() { return this.EntityPM.Satarday; }
    set Satarday(newValue: boolean) {
        if (this.EntityPM.Satarday != newValue) {
            this.EntityPM.Satarday = newValue;
        }
    }

    get Sunday() { return this.EntityPM.Sunday; }
    set Sunday(newValue: boolean) {
        if (this.EntityPM.Sunday != newValue) {
            this.EntityPM.Sunday = newValue;
        }
    }

    get Monday() { return this.EntityPM.Monday; }
    set Monday(newValue: boolean) {
        if (this.EntityPM.Monday != newValue) {
            this.EntityPM.Monday = newValue;
        }
    }

    get Tuesday() { return this.EntityPM.Tuesday; }
    set Tuesday(newValue: boolean) {
        if (this.EntityPM.Tuesday != newValue) {
            this.EntityPM.Tuesday = newValue;
        }
    }

    get Wednesday() { return this.EntityPM.Wednesday; }
    set Wednesday(newValue: boolean) {
        if (this.EntityPM.Wednesday != newValue) {
            this.EntityPM.Wednesday = newValue;
        }
    }

    get Thursday() { return this.EntityPM.Thursday; }
    set Thursday(newValue: boolean) {
        if (this.EntityPM.Thursday != newValue) {
            this.EntityPM.Thursday = newValue;
        }
    }

    get Friday() { return this.EntityPM.Friday; }
    set Friday(newValue: boolean) {
        if (this.EntityPM.Friday != newValue) {
            this.EntityPM.Friday = newValue;
        }
    }

    SetReportSchedulerDetailsData(schedulerDetailsData: SchedulerDetails) {
        this.SchedulerDetailsData = schedulerDetailsData;
        if (schedulerDetailsData) {
            this.EntityPM.SchedulerDetailsData = schedulerDetailsData;
        }
    }
}
