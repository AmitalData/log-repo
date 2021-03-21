import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TasksSchedulerPM } from '../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';
import { SchedulerDetails, ReportSchedulerDetails, FTPSchedulerDetails } from '../../../Infrastructure/DataContracts/SchedulerDetails';

@Component({
    
    templateUrl: './TaskReportSchedulerComponent.html',
})

export class TaskReportSchedulerComponent implements OnInit {
    public ItemsSource: TaskReportSchedulerItemClass[] = [];
    public FixedItemsSource: TaskReportSchedulerItemClass[] = [];
    public TasksHistoryColumns: any[] = null;
    public Taskscolumns: any[] = null;
    private loadedDataList: TasksSchedulerPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ReportGroupList: ReportGroupList;
    public ReportList: ReportList;
    filterAgrs: ApiQueryFilters;
    SchedulerType: string = "Report";
    IsEditReportSchedulerEventAlreadyExist: boolean = false;
    @Output() TasksCustomColumnsReady = new EventEmitter();
    @Output() MenuHeaderchangeeventTasks = new EventEmitter();
    @Output() SelectedRowChanged = new EventEmitter();
    constructor(private _entityListService: EntityListService) {

        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event && $event.Name == "IsEditReportScheduler") {
                if (!this.IsEditReportSchedulerEventAlreadyExist) {
                    this.IsEditReportSchedulerEventAlreadyExist = true;
                    this.EditTaskClicked($event.DataContext);
                }
            }
        });
    }

    ngOnInit() {
        this.BuildTasksColumns();
        this.LoadTaskSchedulers();
    }

    SetWindowArgs(windowArgs) {
        this.ReportGroupList = windowArgs.ReportGroupList;
        this.ReportList = windowArgs.ReportList;
    }

    public SelectedRow: any;
    onRowSelected(item: any) {
        this.SelectedRow = item.rowData;
        this.SelectedRowChanged.emit(this.SelectedRow);
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
        logWindow.Width = 1250;
        logWindow.Title = this.ReportList.Name + " Scheduler Details";
        logWindow.DataContext = new TaskReportSchedulerItemClass(newItem, this, true);
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Report/Components/Scheduler/AddEditReportSchedulerComponent');
        logWindow.WindowClosed.subscribe(closed => {
            this.IsEditReportSchedulerEventAlreadyExist = false;
        });
    }

    EditTaskClicked(DataContext) {
        var windowArgs: any = {};
        windowArgs.ReportGroupList = this.ReportGroupList;
        windowArgs.ReportList = this.ReportList;

        var logWindow = new LogitudeWindow();
        DataContext.fatherComponent = this;
        logWindow.DataContext = DataContext;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = this.ReportList.Name + " Scheduler Details";
        logWindow.Height = 820;
        logWindow.Width = 1250;
        logWindow.Show('./Report/Components/Scheduler/AddEditReportSchedulerComponent');
        logWindow.WindowClosed.subscribe(closed => {
            this.IsEditReportSchedulerEventAlreadyExist = false;
        });
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
            FieldName: "TriggerType",
            DataTypeCode: 'String',
            Display: 'Frequency',
            Styles: { width: '160px' },
            HtmlListComponentName: 'ReportSchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.Taskscolumns.push({
            FieldName: "Recepients",
            DataTypeCode: 'String',
            Display: 'Recipients',
            Styles: { width: '160px' },
            HtmlListComponentName: 'ReportSchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false,
            
        });

        this.Taskscolumns.push({
            FieldName: "LastRunEndTime",
            DataTypeCode: 'String',
            Display: 'Last Run Date',
            Styles: { width: '160px' },
            HtmlListComponentName: 'ReportSchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "LastRunEndTime"
        });

        this.Taskscolumns.push({
            FieldName: "InActive",
            DataTypeCode: 'Boolean',
            Display: 'InActive',
            Styles: { width: '90px' },
            HtmlListComponentName: 'ReportSchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "InActive"
        });

        this.Taskscolumns.push({
            FieldName: "EditTaskButton;" + this.SchedulerType,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'ReportSchedulerDateListTemplate',
            HtmlListComponentUrl: '../Report/Components/Scheduler/ListTemplates/ReportSchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.TasksCustomColumnsReady.emit(this.Taskscolumns);
    }

    private firstLoad = true;
    LogGridDataCountReady(event) {
        if (this.firstLoad) {
            console.log('reloaded...');
            this.RefreshButtonClicked();
            this.firstLoad = false;
        }
    }

    TasksDataSource = {
        pageSize: 100,
        rowCount: null,
        sortingCol: "CreateDateTime",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getTasksRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    getTasksRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        filters = new ApiQueryFilters();

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
        filters.addAdditionalFilter("EntityId", this.ReportList.Id, null, null, "Equals", true, false, false, "String");
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = 100;
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

        //this.BuildTasksColumns();

        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.PageSize = 100;
        this.filterAgrs.SortBy = "CreateDateTime";
        this.filterAgrs.SortDirection = "Descending";

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

    SchedulerDetails: SchedulerDetails;
    ReportSchedulerDetails: ReportSchedulerDetails;
    FTPDetails: FTPSchedulerDetails;

    constructor(item: TasksSchedulerPM, public fatherComponent: TaskReportSchedulerComponent, isNew: boolean = false) {
        super();
        this.EntityPM = item;
        this.IsNew = isNew;
    }

    get Id() { return this.EntityPM.Id; }
    get CreateDate() { return this.EntityPM.CreateDateTime; }
    get UpdateDate() { return this.EntityPM.UpdateDateTime; }
    get LastRunTime() { return this.EntityPM.LastRunStartTime; }
    get StartDate() { return this.EntityPM.StartDateTime; }
    get Status() { return this.EntityPM.Status; }
    get LastRunEndTime() { return this.EntityPM.LastRunEndTime; }
    get Duration() { return Math.abs(this.EntityPM.Duration); }
    get UpdatedBy() { return this.EntityPM.UpdatedBy; }
    get CreatedBy() { return this.EntityPM.CreatedBy; }

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

    get Recepients() { return this.EntityPM.Recepients; }
    set Recepients(newValue: string) {
        if (this.EntityPM.Recepients != newValue) {
            this.EntityPM.Recepients = newValue;
        }
    }

    get RepeatInMinutes() { return this.EntityPM.RepeatInMinutes; }
    set RepeatInMinutes(newValue: number) {
        if (this.EntityPM.RepeatInMinutes != newValue) {


            this.EntityPM.RepeatInMinutes = newValue;

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
    get Host() {
        return this.FTPDetails ? this.FTPDetails.Host : "";
    }
    set Host(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Host != newValue) {
            this.FTPDetails.Host = newValue;
            this.EntityPM.IsDirty = true;
        }
    }


    get Folder() {
        return this.FTPDetails ? this.FTPDetails.Folder : "";
    }
    set Folder(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Folder != newValue) {
            this.FTPDetails.Folder = newValue;
            this.EntityPM.IsDirty = true;
        }
    }


    get UserName() {
        return this.FTPDetails ? this.FTPDetails.UserName : "";
    }
    set UserName(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.UserName != newValue) {
            this.FTPDetails.UserName = newValue;
            this.EntityPM.IsDirty = true;
        }
    }



    get Password() {
        return this.FTPDetails ? this.FTPDetails.Password : "";
    }
    set Password(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Password != newValue) {
            this.FTPDetails.Password = newValue;
            this.EntityPM.IsDirty = true;
        }
    }



  

    private isFTP: boolean;
    get IsFTP() {
        return this.isFTP;
    }
    set IsFTP(newValue: boolean) {
        if (this.isFTP != newValue) {
            this.isFTP = newValue;
        }
    }

    get SendIfEmpty() {
        return this.SchedulerDetails.SendIfEmpty;
    }
    set SendIfEmpty(newValue: boolean) {
        if (this.SchedulerDetails.SendIfEmpty != newValue) {
            this.SchedulerDetails.SendIfEmpty = newValue;
        }
    }

    SetReportSchedulerDetailsData(schedulerDetails: SchedulerDetails) {
        this.SchedulerDetails = schedulerDetails;
        if (schedulerDetails) {
            if (!schedulerDetails.ReportDetails) {
                schedulerDetails.ReportDetails = new ReportSchedulerDetails;
             }
            if (!schedulerDetails.FTPDetails) {
                schedulerDetails.FTPDetails = new FTPSchedulerDetails();
            }

            this.FTPDetails = schedulerDetails.FTPDetails;
            this.EntityPM.SchedulerDetailsData = schedulerDetails;
        }
    }
}
