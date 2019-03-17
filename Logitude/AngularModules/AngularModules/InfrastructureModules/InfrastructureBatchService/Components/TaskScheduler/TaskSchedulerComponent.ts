import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TaskSchedulerHistoryList} from '../../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import {TasksSchedulerPM} from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';

@Component({
    moduleId: module.id,
    templateUrl: './TaskSchedulerComponent.html',
})

export class TaskSchedulerComponent  {
    public ItemsSource: TaskSchedulerItemClass[] = [];
    public HistoryItemsSource: TaskSchedulerHistoryList[] = []; 
    private loadedDataList: TasksSchedulerPM[] = [];
    private infraDomainService: InfrastructureDomainService;
    constructor() {
        this.infraDomainService = new InfrastructureDomainService();

        this.GetTasksSchedular();
    }
    
    public GetTasksSchedular() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        this.IsHistoryGridVsisible = false;

        this.infraDomainService.GetAllTasksSchedulerPMs().subscribe(myResult => {
            if (myResult == null) {
                this.ItemsSource = [];
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

    BuildItemsSource() {
        this.ItemsSource = [];

        this.loadedDataList.forEach(item => {
            this.ItemsSource.push(new TaskSchedulerItemClass(item, this));
        });

        SessionLocator.CurrentSession.StopBusyIndicator();
    }

    public IsHistoryGridVsisible = false;
    public SelectedRow: TaskSchedulerItemClass;
    Selecting(item: TaskSchedulerItemClass) {
        this.SelectedRow = item;

        if (item == null) {
            this.IsHistoryGridVsisible = false;
        }

        else {
            this.LoadHistoryList();
        }
    }
    
    private LoadHistoryList() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        this.infraDomainService.GetTaskSchedulerHistory(this.SelectedRow.Id).subscribe(myResult => {
            if (myResult == null) {
                this.HistoryItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.HistoryItemsSource = myResponse.Result;
                    this.IsHistoryGridVsisible = true;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    NewTaskClicked() {
        var newItem: TasksSchedulerPM = new TasksSchedulerPM();
        newItem.CreatedBy = SessionLocator.LoggedUserId;
        newItem.UpdatedBy = SessionLocator.LoggedUserId;
        newItem.TriggerType = "O";

        var logWindow = new LogitudeWindow();
        logWindow.Height = 570;
        logWindow.Width = 800;
        logWindow.Title = "Task Scheduler Details";
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
        logWindow.Title = "Task Scheduler Details";
        logWindow.DataContext = item;
        logWindow.Height = 570;
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
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}

export class TaskSchedulerItemClass extends BaseComponent {
    public EntityPM: TasksSchedulerPM;
    public ObjectTableName: string = "TasksScheduler";
    public IsNew: boolean = false;
    constructor(item: TasksSchedulerPM, public fatherComponent: TaskSchedulerComponent, isNew: boolean = false) {
        super();
        this.EntityPM = item;
        this.IsNew = isNew;
    }

    get Id() { return this.EntityPM.Id; }
    get CreateDate() { return this.EntityPM.CreateDateTime; }
    get NextRunTime() { return this.EntityPM.NextRunTime; }
    get LastRunTime() { return this.EntityPM.LastRunTime; }
    get LastRunResult() { return this.EntityPM.LastRunResult; }
    get StartDate() { return this.EntityPM.StartDateTime; }

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
}