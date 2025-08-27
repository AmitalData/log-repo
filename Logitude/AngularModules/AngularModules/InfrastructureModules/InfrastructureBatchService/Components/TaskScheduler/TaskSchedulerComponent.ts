import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { InfrastructureDomainService } from '../../../../Infrastructure/Services/InfrastructureDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TaskSchedulerHistoryList } from '../../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import { TasksSchedulerPM } from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { SchedulerDetails, FTPSchedulerDetails } from '../../../../Infrastructure/DataContracts/SchedulerDetails';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    
    templateUrl: './TaskSchedulerComponent.html',
})

export class TaskSchedulerComponent implements OnInit {
    public ItemsSource: TaskSchedulerItemClass[] = [];
    public FixedItemsSource: TaskSchedulerItemClass[] = [];
    public HistoryItemsSource: TaskSchedulerHistoryList[] = [];
    private loadedDataList: TasksSchedulerPM[] = [];
    private infraDomainService: InfrastructureDomainService;
    IsEnableAddButton: boolean = false;
    public columns: any[] = null;
    public Taskscolumns: any[] = null;
    @Output() TasksCustomColumnsReady = new EventEmitter();
    @Output() SelectedRowChanged = new EventEmitter();
    @Output() ShowUTCTimes = new EventEmitter();
    @Output() MenuHeaderchangeeventTasks = new EventEmitter();

    filterAgrs: ApiQueryFilters;
    ShowUTCTimesLabel: string = "Show UTC Time";
    ShowUTCTimeEnabled: boolean = false;
    HasUTCFeature: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    SearchFilter: string = "";
    constructor(private _entityListService: EntityListService) {
        this.infraDomainService = new InfrastructureDomainService();
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "SHOWUTCBUTTON")) {
            this.HasUTCFeature = true;
        }
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "NEW")) this.IsEnableAddButton = true;
    }

    ngOnInit() {
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadTasks") {
                this.RefreshButtonClicked(); 
            } 
        });
    }

    SchedulerType: string = "";
    public LoadData(schedulerType: string) {
        this.SchedulerType = schedulerType;
        this.LoadTaskSchedulers();
        //this.GetTasksSchedular();
    }

    public GetTasksSchedular() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.infraDomainService.GetAllTasksSchedulerPMs(this.SchedulerType).subscribe((myResult: ServiceResponse) => {
            if (myResult == null) {
                this.ItemsSource = [];
                this.FixedItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.loadedDataList = myResponse.Result;

                    this.BuildItemsSource();
                }
            }
        });
    }

    public RefreshTasksSchedular(entityPM: TasksSchedulerPM) {
        var index = this.loadedDataList.indexOf(entityPM);
        if (index > -1) {
            this.loadedDataList[index] = entityPM;
        } else this.loadedDataList.push(entityPM);


        this.BuildItemsSource();

    }

    BuildItemsSource() {
        this.ItemsSource = [];
        this.FixedItemsSource = [];
        this.loadedDataList.forEach(item => {
            this.ItemsSource.push(new TaskSchedulerItemClass(item, this));
            this.FixedItemsSource.push(new TaskSchedulerItemClass(item, this));
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

    public ShowArrow = false;
    public SelectedRow: any;
    onRowSelected(item: any) {
        this.SelectedRow = item.rowData;//new TaskSchedulerItemClass(item.rowData, this, true);//item;
        this.SelectedRowChanged.emit(this.SelectedRow);
    }
    onFirstRowSelected(item: any) {
        this.SelectedRow = item.SelectedRow;
        this.SelectedRowChanged.emit(this.SelectedRow);
    }

    NewTaskClicked() {
        var newItem: TasksSchedulerPM = new TasksSchedulerPM();
        newItem.CreatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.UpdatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.TriggerType = "O";
        newItem.Tenant = SessionLocator.Tenant;

        newItem.Type = this.SchedulerType;
        var logWindow = new LogitudeWindow();
        logWindow.Height = (this.SchedulerType == "FTP" || this.SchedulerType == "SFTP") ? 820 : 750;
        logWindow.Width = 900;
        logWindow.Title = this.SchedulerType + " Scheduler Details";
        logWindow.DataContext = new TaskSchedulerItemClass(newItem, this, true);
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.RefreshButtonClicked();
            }
        });
    }

    EditClicked(item: TaskSchedulerItemClass) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = this.SchedulerType + " Scheduler Details";
        logWindow.DataContext = item;
        logWindow.Height = (this.SchedulerType == "FTP" || this.SchedulerType == "SFTP") ? 820 : 750;
        logWindow.Width = 900;
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.RefreshButtonClicked();
            }
        });
    }

    RefreshButtonClicked() {
        //this.GetTasksSchedular();
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
            Display: 'Task Name',
            Styles: { width: '200px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "Name"
        });
        if (this.ShowUTCTimeEnabled == false) {
            this.Taskscolumns.push({
                FieldName: "NextRunTime",
                DataTypeCode: 'String',
                Display: 'Next Run Time',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "NextRunTime"
            });

            this.Taskscolumns.push({
                FieldName: "LastRunStartTime",
                DataTypeCode: 'String',
                Display: 'Last Run Start Time',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "LastRunStartTime"
            });
            this.Taskscolumns.push({
                FieldName: "LastRunEndTime",
                DataTypeCode: 'String',
                Display: 'Last Run End Time',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "LastRunEndTime"
            });
        }
        else {

            this.Taskscolumns.push({
                FieldName: "NextRunTimeUTC",
                DataTypeCode: 'String',
                Display: 'Next Run Time UTC',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "NextRunTimeUTC"
            });

            this.Taskscolumns.push({
                FieldName: "LastRunStartTimeUTC",
                DataTypeCode: 'String',
                Display: 'Last Run Start Time UTC',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "LastRunStartTimeUTC"
            });
            this.Taskscolumns.push({
                FieldName: "LastRunEndTimeUTC",
                DataTypeCode: 'String',
                Display: 'Last Run End Time UTC',
                Styles: { width: '160px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "LastRunEndTimeUTC"
            });
        }
        this.Taskscolumns.push({
            FieldName: "MyDuration",
            DataTypeCode: 'String',
            Display: 'Avg. Run Time',
            Styles: { width: '90px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.Taskscolumns.push({
            FieldName: "Status",
            DataTypeCode: 'String',
            Display: 'Status',
            Styles: { width: '90px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.Taskscolumns.push({
            FieldName: "InActive",
            DataTypeCode: 'Boolean',
            Display: 'In Active',
            Styles: { width: '90px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });

        this.Taskscolumns.push({
            FieldName: "EditTaskButton;" + this.SchedulerType,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });


        this.TasksCustomColumnsReady.emit(this.Taskscolumns);
    }

    TasksDataSource = {
        pageSize: 20,
        rowCount: null,

        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.geTaskstRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    geTaskstRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        filters = new ApiQueryFilters();
        //filters.SortBy = "StatusDate";
        // filters.SortDirection = "Desc";
        if (!sortingCol) {
            sortingCol = "NextRunTime";
            sortingDir = "descending";
        }
        //if (!this.SelectedRow) {
        //    //if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
        //    //    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
        //    //}
        //    filters.addAdditionalFilter("TaskId", "0-0", null, null, "Equals", false, false, false, "String");

        //}
        //else {
        //    if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
        //        if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
        //            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
        //        }
        //        filters.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Equals", false, false, false, "String");
        //    }

        //return
        //}
        if (this.filterTypeCode == "AL") {
            filters.AdditionalFilters = [];//addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Contains", true, false, false, "String");
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

        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            filters.addAdditionalFilter("Name", this.SearchFilter, null, null, "Contains", true, false, false, "String");
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

        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == "Type").length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != "Type");
        }
        this.filterAgrs.addAdditionalFilter("Type", this.SchedulerType, null, null, "Equals", true, false, false, "String");

        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            this.filterAgrs.addAdditionalFilter("Name", this.SearchFilter, null, null, "Contains", true, false, false, "String");
        }

        this.MenuHeaderchangeeventTasks.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    ShowUTCTimesClicked() {
        if (this.ShowUTCTimeEnabled == true) {
            this.ShowUTCTimeEnabled = false;
            this.ShowUTCTimesLabel = "Show UTC Time";
        }
        else {
            this.ShowUTCTimeEnabled = true;
            this.ShowUTCTimesLabel = "Hide UTC Time";
        }
        this.LoadTaskSchedulers();
        this.ShowUTCTimes.emit(this.ShowUTCTimeEnabled);
    }

    private filterTypeCode: string = "AC";
    public get FilterTypeCode() { return this.filterTypeCode; }
    public set FilterTypeCode(value: string) {
        if (this.filterTypeCode != value) {
            this.filterTypeCode = value;
            this.LoadTaskSchedulers();
            //if (value == "AC") {
            //    this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == false);
            //}
            //else if (value == "IN") {
            //    this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == true);
            //}
            //else {
            //    this.ItemsSource = this.FixedItemsSource;
            //}
        }
    }

    onSearchTextChangeEvent(searchText: string) {
        if (!searchText) searchText = "";
        this.SearchFilter = searchText.replace(/\s+$/, '');
        this.LoadTaskSchedulers();
    }
}

export class TaskSchedulerItemClass extends BaseComponent {
    public EntityPM: TasksSchedulerPM;
    public ObjectTableName: string = "TasksScheduler";
    public IsNew: boolean = false;
    private newValueinDateFormat: Date;




    FTPDetails: FTPSchedulerDetails;
    SchedulerDetailsData: SchedulerDetails = new SchedulerDetails();
    constructor(item: TasksSchedulerPM, public fatherComponent: TaskSchedulerComponent, isNew: boolean = false) {
        super();
        this.EntityPM = item;
        this.IsNew = isNew;
    }

    get Id() { return this.EntityPM.Id; }
    get CreateDate() { return this.EntityPM.CreateDateTime; }
    get UpdateDate() { return this.EntityPM.UpdateDateTime; }
    get NextRunTime() { return this.EntityPM.NextRunTime; }
    get LastRunTime() { return this.EntityPM.LastRunStartTime; }
    get NextRunTimeUTC() { return this.EntityPM.NextRunTimeUTC; }
    get LastRunTimeUTC() { return this.EntityPM.LastRunStartTimeUTC; }
    get LastRunResult() { return this.EntityPM.LastRunResult; }
    get StartDate() { return this.EntityPM.StartDateTime; }
    get StartDateUTC() { return this.EntityPM.StartDateTimeUTC; }
    get Status() { return this.EntityPM.Status; }
    get UpdatedBy() { return this.EntityPM.UpdatedBy; }
    get CreatedBy() { return this.EntityPM.CreatedBy; }
    get Duration() { return Math.abs(this.EntityPM.Duration); }
    get LastRunEndTime() { return this.EntityPM.LastRunEndTime; }
    get LastRunEndTimeUTC() { return this.EntityPM.LastRunEndTimeUTC; }



    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get ProcedureCode() { return this.EntityPM.ProcedureCode; }
    set ProcedureCode(newValue: string) {
        if (this.EntityPM.ProcedureCode != newValue) {
            this.EntityPM.ProcedureCode = newValue;
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

    get TriggerType() { return this.EntityPM.TriggerType; }
    set TriggerType(newValue: string) {
        if (this.EntityPM.TriggerType != newValue) {
            this.EntityPM.TriggerType = newValue;
        }
    }

    get StartDateTime() { return this.EntityPM.StartDateTime; }
    set StartDateTime(newValue: Date) {

        if (this.EntityPM.StartDateTime != newValue) {
            this.EntityPM.StartDateTime = newValue;
            this.EntityPM.StartDateTimeUTC = this.GetUtcTenantDateValueFromDate(newValue);

        }
    }

    private GetUtcTenantDateValueFromDate(newValue: Date) {
        let utcDateValue = new Date(newValue);
        if (SessionLocator.TenantPM.TimeZoneOffset && SessionLocator.TenantPM.TimeZoneOffset != 0) {
            utcDateValue.setHours(utcDateValue.getHours() - SessionLocator.TenantPM.TimeZoneOffset);
        }
        //if (SessionLocator.TenantPM.DayLightOffset && SessionLocator.TenantPM.DayLightOffset != 0) {
        //    utcDateValue.setHours(utcDateValue.getHours() + SessionLocator.TenantPM.DayLightOffset);
        //}
        return utcDateValue;
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


    get Type() {
        return this.EntityPM.Type ? this.EntityPM.Type : "";
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



    get From() {
        return this.FTPDetails ? this.FTPDetails.From : "";
    }
    set From(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.From != newValue) {
            this.FTPDetails.From = newValue;
            this.EntityPM.IsDirty = true;
        }
    }



    get Subject() {
        return this.FTPDetails ? this.FTPDetails.Subject : "";
    }
    set Subject(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Subject != newValue) {
            this.FTPDetails.Subject = newValue;
            this.EntityPM.IsDirty = true;
        }
    }


    get Prefix() {
        return this.FTPDetails ? this.FTPDetails.Prefix : "";
    }
    set Prefix(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Prefix != newValue) {
            this.FTPDetails.Prefix = newValue;
            this.EntityPM.IsDirty = true;
        }
    }

    get Suffix() {
        return this.FTPDetails ? this.FTPDetails.Suffix : "";
    }
    set Suffix(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Suffix != newValue) {
            this.FTPDetails.Suffix = newValue;
            this.EntityPM.IsDirty = true;
        }
    }

    get Extension() {
        return this.FTPDetails ? this.FTPDetails.Extension : "";
    }
    set Extension(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Extension != newValue) {
            //if (!AppTool.IsNullOrEmpty(newValue) && newValue.startsWith("."))
            //    newValue = newValue.substring(1, newValue.length);

            this.FTPDetails.Extension = newValue;
            this.EntityPM.IsDirty = true;
        }
    }

    get IsSFTP() {
        return this.FTPDetails.IsSFTP;
    }
    set IsSFTP(newValue: boolean) {
        if (this.FTPDetails && this.FTPDetails.IsSFTP != newValue) {


            this.FTPDetails.IsSFTP = newValue;
            this.EntityPM.IsDirty = true;
        }
    }


    SetSchedulerDetailsData(schedulerDetailsData: SchedulerDetails) {
        this.SchedulerDetailsData = schedulerDetailsData;
        if (schedulerDetailsData) {
            this.RemoveReportDetails(schedulerDetailsData);
            if (this.EntityPM.Type == "FTP" || this.EntityPM.Type == "SFTP") {
                if (!schedulerDetailsData.FTPDetails) {
                    schedulerDetailsData.FTPDetails = new FTPSchedulerDetails();
                    schedulerDetailsData.FTPDetails.IsSFTP = (this.EntityPM.Type == "SFTP" ? true : false);


                }
                this.FTPDetails = schedulerDetailsData.FTPDetails;

                this.EntityPM.SchedulerDetailsData = schedulerDetailsData;

            }
        }
    }

    private RemoveReportDetails(schedulerDetailsData: SchedulerDetails) {
        if (this.EntityPM.Type != "Report") {
            schedulerDetailsData.ReportDetails = null;
        }
    }
}
