import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TaskSchedulerHistoryList} from '../../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import {TasksSchedulerPM} from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import {SchedulerDetails, FTPSchedulerDetails} from '../../../../Infrastructure/DataContracts/SchedulerDetails';
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

       
    }


    SchedulerType: string = "";
    public LoadData(schedulerType: string) {
        this.SchedulerType = schedulerType;
        this.GetTasksSchedular();
    }     


    public GetTasksSchedular() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        this.IsHistoryGridVsisible = false;

        this.infraDomainService.GetAllTasksSchedulerPMs(this.SchedulerType).subscribe(myResult => {
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

  
    public RefreshTasksSchedular(entityPM: TasksSchedulerPM) {
        var index = this.loadedDataList.indexOf(entityPM);
        if (index > -1) {
            this.loadedDataList[index] = entityPM;
        } else this.loadedDataList.push(entityPM);

       
        this.BuildItemsSource();

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
        newItem.Type = this.SchedulerType;
        var logWindow = new LogitudeWindow();
        logWindow.Height = this.SchedulerType == "FTP" ? 645 : 570;
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
        logWindow.Height = this.SchedulerType == "FTP" ? 645 : 570;
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

 


    FTPDetails: FTPSchedulerDetails;
    SchedulerDetailsData: SchedulerDetails = new SchedulerDetails();
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


    get Extension() {
        return this.FTPDetails ? this.FTPDetails.Extension : "";
    }
    set Extension(newValue: string) {
        if (this.FTPDetails && this.FTPDetails.Extension != newValue) {
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