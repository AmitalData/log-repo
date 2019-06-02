import { Component, Output, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TaskSchedulerHistoryList} from '../../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import {TasksSchedulerPM} from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { SchedulerDetails, FTPSchedulerDetails } from '../../../../Infrastructure/DataContracts/SchedulerDetails';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    moduleId: module.id,
    templateUrl: './TaskSchedulerComponent.html',
})

export class TaskSchedulerComponent implements OnInit  {
    public ItemsSource: TaskSchedulerItemClass[] = [];
    public FixedItemsSource: TaskSchedulerItemClass[] = [];
    public HistoryItemsSource: TaskSchedulerHistoryList[] = []; 
    private loadedDataList: TasksSchedulerPM[] = [];
    private infraDomainService: InfrastructureDomainService;
    IsEnableAddButton: boolean = false;

   
    public columns: any[] = null;
    @Output() CustomColumnsReady = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    ShowUTCTimesLabel: string = "Show UTC Time";
    ShowUTCTimeEnabled: boolean = false;
    HasUTCFeature: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        this.infraDomainService = new InfrastructureDomainService();
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "SHOWUTCBUTTON")) {
            this.HasUTCFeature = true;
		}
		if (FeatureLocator.HasFeaturePermession("TasksScheduler", "NEW")) this.IsEnableAddButton = true;
       
    }

    ngOnInit() {
        this.LoadTaskHistories();
    }

    SchedulerType: string = "";
    public LoadData(schedulerType: string) {
        this.SchedulerType = schedulerType;
        this.GetTasksSchedular();
    }     


    public GetTasksSchedular() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.IsHistoryGridVsisible = false;

        this.infraDomainService.GetAllTasksSchedulerPMs(this.SchedulerType).subscribe(myResult => {
            if (myResult == null) {
                this.ItemsSource = [];
                this.FixedItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.loadedDataList = myResponse.Result;
                   
                    this.BuildItemsSource();
                    this.LoadTaskHistories();
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

        this.CurrentSession.StopBusyIndicator();
    }

    public IsHistoryGridVsisible = false;
    public SelectedRow: TaskSchedulerItemClass;
    Selecting(item: TaskSchedulerItemClass) {
        this.SelectedRow = item;

        if (item == null) {
            this.IsHistoryGridVsisible = false;
        }

        else {
            //this.LoadHistoryList();
            this.LoadTaskHistories();
        }
    }
    
    private LoadHistoryList() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.infraDomainService.GetTaskSchedulerHistory(this.SelectedRow.Id).subscribe(myResult => {
            if (myResult == null) {
                this.HistoryItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.HistoryItemsSource = myResponse.Result;
                    this.IsHistoryGridVsisible = true;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    NewTaskClicked() {
        var newItem: TasksSchedulerPM = new TasksSchedulerPM();
        newItem.CreatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.UpdatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newItem.TriggerType = "O";
        newItem.Tenant = SessionLocator.Tenant;
        
        newItem.Type = this.SchedulerType;
        var logWindow = new LogitudeWindow();
        logWindow.Height = this.SchedulerType == "FTP" ? 730 : 620;
        logWindow.Width = 800;
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
        logWindow.Title = this.SchedulerType  + " Scheduler Details";
        logWindow.DataContext = item;
        logWindow.Height = this.SchedulerType == "FTP" ? 730 : 620;
        logWindow.Width = 800;
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.RefreshButtonClicked();
            }
        });
    }

    RefreshButtonClicked() {
        this.GetTasksSchedular();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    BuildColumns() { 
        this.columns = [];
        this.columns.push({
            FieldName: "Log",
            DataTypeCode: 'String',
            Display: 'Log',
            Styles: { width: '280px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        if (this.ShowUTCTimeEnabled == false) {
            this.columns.push({
                FieldName: "StartDateTime",
                DataTypeCode: 'String',
                Display: 'Start Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "StartDateTime"
            });
            this.columns.push({
                FieldName: "EndDateTime",
                DataTypeCode: 'String',
                Display: 'End Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "EndDateTime"
            });
        }
        else {
            this.columns.push({
                FieldName: "StartDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'Start Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "StartDateTime"
            });
            this.columns.push({
                FieldName: "EndDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'End Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "EndDateTime"
            });
        }
        this.columns.push({
            FieldName: "Duration",
            DataTypeCode: 'String',
            Display: 'Duration',
            Styles: { width: '200px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        this.columns.push({
            FieldName: "ViewLog",
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '100px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        /////
        //this.columns.push({
        //    FieldName: "RunResult",
        //    DataTypeCode: 'String',
        //    Display: 'Run Result',
        //    Styles: { width: '200px' },
        //    IsCustomTemplate: true,
        //    ServerSideSortable: false
        //});
         
        this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 20,
        rowCount: null,
       
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
          
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
       
        filters = new ApiQueryFilters();
        //filters.SortBy = "StatusDate";
        //filters.SortDirection = "Descending";
        if (!this.SelectedRow) {
            if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
                filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
            }
            filters.addAdditionalFilter("TaskId", "0-0", null, null, "Equals", false, false, false, "String");
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
                if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
                    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
                }
                filters.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Equals", false, false, false, "String");
            }
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
    private LoadTaskHistories() {
        this.IsHistoryGridVsisible = true;
        this.BuildColumns();
       
        this.filterAgrs = new ApiQueryFilters();
        if (!this.SelectedRow) {
            return;
        }
         
        if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
            this.filterAgrs.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Contains", true, false, false, "String");
        }

        //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsCancelled').length > 0) {
        //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsCancelled');
        //}
        //this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");

        //}

        //this.filterAgrs.addAdditionalFilter("IsMissingDocument", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", false, null, null, "Equals", false, false, false, "Boolean");
        //if (this.SelectedTransportFilter != "All") {
        //    this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        //}
        //else {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
        //    }
        //}
        //if (this.SelectedArchiveFilter != "All") {
        //    this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedArchiveFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedArchiveFilter == "All" ? true : false);
        //}
        //else {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
        //    }
        //}

        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedTransportFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", true);

        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", "null", null, null, "Equals", false, true, false, "string", true);

        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", true, null, null, "Equals", false, true, false, "string", true);
 
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
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
        this.LoadTaskHistories();
    }

    private filterTypeCode: string = "AL";
    public get FilterTypeCode() { return this.filterTypeCode; }
    public set FilterTypeCode(value: string) {
        if (this.filterTypeCode != value) {
            this.filterTypeCode = value;
            if (value == "AC") {
                this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == false);
            }
            else if (value == "IN") {
                this.ItemsSource = this.FixedItemsSource.filter(a => a.InActive == true);
            }
            else {
                this.ItemsSource = this.FixedItemsSource;
            }
        }
    }
}

export class TaskSchedulerItemClass extends BaseComponent {
    public EntityPM: TasksSchedulerPM;
    public ObjectTableName: string = "TasksScheduler";
    public IsNew: boolean = false;

 


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
    get Duration() { return this.EntityPM.Duration; }
    get LastRunEndTime() { return this.EntityPM.LastRunEndTime; }
    get LastRunEndTimeUTC() { return this.EntityPM.LastRunEndTimeUTC; }

   

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get ServiceClassName() { return this.EntityPM.ServiceClassName; }
    set ServiceClassName(newValue: string) {
        if (this.EntityPM.ServiceClassName != newValue) {
            this.EntityPM.ServiceClassName = newValue;
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
            this.EntityPM.StartDateTimeUTC = new Date(newValue.getUTCFullYear(), newValue.getUTCMonth(), newValue.getUTCDate(), newValue.getUTCHours(), newValue.getUTCMinutes(), newValue.getUTCSeconds(), newValue.getUTCMilliseconds());
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


    SetSchedulerDetailsData(schedulerDetailsData: SchedulerDetails) {
        this.SchedulerDetailsData = schedulerDetailsData;
        if (schedulerDetailsData) {
            if (this.EntityPM.Type == "FTP") {
                if (!schedulerDetailsData.FTPDetails) {
                    schedulerDetailsData.FTPDetails = new FTPSchedulerDetails();
                }
                this.FTPDetails = schedulerDetailsData.FTPDetails;


            }
        }
    }
}
