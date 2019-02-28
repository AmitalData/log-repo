import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TimeManagementDomainService } from '../../../Services/TimeManagementDomainService';
import {TMOfficeHourListService} from '../../../Services/StandardLists/TMOfficeHourListService'; 
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow'; 
import {TMOfficeHourPM} from '../../../EntityPMs/TMOfficeHourPM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
@Component({
    selector: 'ClockTimeComponent',
    moduleId: module.id,
    templateUrl: './ClockTimeComponent.html',
})

export class ClockTimeComponent extends BaseComponent {
    public DataContext = this;
    public ItemSource: ItemSourceItem[];
    public ItemSourceCollection: ObservableCollection;
    public ValidationErrorsList: Array<string> = [];
    public ObjectTableName: string = "TMOfficeHour";
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this._entityResourceService.getEntityResourceByTableName("TMOfficeHour", 0).subscribe(response => {
        });
        this.ItemSourceCollection = new ObservableCollection([]);
    }
    private myDomainService: TimeManagementDomainService = new TimeManagementDomainService();

    private employeeUserId: string = null;
    get EmployeeUserId() {
        return this.employeeUserId;
    }
    set EmployeeUserId(value: string) {
        if (this.employeeUserId != value) {
            this.employeeUserId = value;
        }
    }

    private fromDate: Date;
    get FromDate() {
        return this.fromDate;
    }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
        }
    }


    private toDate: Date;
    get ToDate() {
        return this.toDate;
    }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
        }
    }



    AddNewClockHour() {
        this._entityResourceService.getEntityResourceByTableName("TMOfficeHour", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "New Office Hour";
            var args: any = {};
            args.EmployeeUserId = this.EmployeeUserId;
            logWindow.WindowArgs = args;
            logWindow.Show('./TimeManagement/Components/NewEntity/NewOfficeHourComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
        });

    }

    OnWindowClosed(event) {
        if (event == "OK") {
            this.LoadClockTimeSheet();
        }
    }

    DeleteLineClicked(item: ItemSourceItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this line ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                if (this.myDomainService == null) {
                    this.myDomainService = new TimeManagementDomainService();
                }
                SessionLocator.CurrentSession.StopBusyIndicator();

            }
        });


    }


    get HasChanged() {
        if (this.ItemSourceCollection.Collection.filter(p => p.IsDirty)[0])
            return true;
        return false;
    }

    SaveTimeOfficeHour() {
        var items: TMOfficeHourPM[] = [];
        this.ItemSourceCollection.Collection.filter(p => p.IsDirty).forEach(p => {
            items.push(p.entity);
        });
        if (items.length != 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            if (this.myDomainService == null) {
                this.myDomainService = new TimeManagementDomainService();
            }
            this.myDomainService.UpdateOfficeHourList(items).subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.ItemSourceCollection.Collection.forEach(p => {
                        p.IsDirty = false;
                    });
                }
            });
        }
    }


    InitTab(arg) {
        this.employeeUserId = SessionLocator.LoggedUserId;
        var fromDate: Date = DateTool.GetCurrentDateAsUtc();
        fromDate.setDate(1);
        this.fromDate = fromDate;
        var toDate: Date = DateTool.GetCurrentDateAsUtc();
        toDate.setMonth(toDate.getUTCMonth() + 1);
        toDate.setDate(1);
        this.toDate = toDate;
        this.myDomainService = new TimeManagementDomainService();
        this.LoadClockTimeSheet();
    }

    SearchButtonClicked() {
        if(this.ItemSourceCollection.Collection.filter(p => p.IsDirty).length > 0)
        {
            this.SaveTimeOfficeHour();
        }
        else {
            this.LoadClockTimeSheet();
        }   
    }

    LoadClockTimeSheet() {
        this.ValidationErrorsList = [];
        if (this.FromDate == null)
            this.ValidationErrorsList.push("From date field is required");

        if (this.ToDate == null)
            this.ValidationErrorsList.push("To date field is required");

        if (this.ToDate < this.FromDate && this.ValidationErrorsList.length==0)
            this.ValidationErrorsList.push("From date field must be less than To date field");



        if (this.ValidationErrorsList.length == 0) {
            if (this.myDomainService == null) {
                this.myDomainService = new TimeManagementDomainService();
            }
            SessionLocator.CurrentSession.StartBusyIndicatorLoading();
            this.myDomainService.GetTimeOfficeClock(this.EmployeeUserId, this.FromDate, this.ToDate).subscribe((myResponse: ServiceResponse) => {
                this.ItemSource = [];
                this.ItemSourceCollection.Clear();
                if (!myResponse.HasError) {
                    var myCollection: ItemSourceItem[] = [];
                    var index = 0;
                    myResponse.Result.sort((a, b) => { return ((a.WorkDate === b.WorkDate) ? ((a.EntryTime === b.EntryTime) ? 0 : (a.EntryTime < b.EntryTime) ? -1 : 1) : (a.WorkDate < b.WorkDate ? -1 : 1)) });
                    myResponse.Result.forEach(item => {
                        index += 1;
                        this.ItemSource.push(new ItemSourceItem(item, this, index));
                    });
                    this.ItemSourceCollection.InsertCollection(this.ItemSource);
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            });
        }

    }

    RefreshTab() {

    }

    PrintPreviewClicked() {

    }
}


export class ItemSourceItem extends BaseComponent {
    public DataContext = this;
    public HasChanges: boolean = false;
    public Index: number;   
    public DivId: string;
    public IsCopy: boolean = false;

    private TMOfficeHoursListService: TMOfficeHourListService;

    constructor(public entity: TMOfficeHourPM, private father: ClockTimeComponent, index: number) {
        super();
        this.TMOfficeHoursListService = new TMOfficeHourListService();
        this.Index = index;    
    
        var idIndex = SessionLocator.CurrentSession.GetNewId("DIV");
        this.DivId = "DIV_" + idIndex;
        this.GetEmployeeUserName();
    }  

    get Description() { return this.entity.Description; }
    set Description(value: string) {
        if (this.entity.Description != value) {
            this.entity.Description = value;
            this.HasChanges = true;
        }
    }

    private minusItem: boolean;
    get EmployeeUserId() { return this.entity.UserId; }
    get UpdatedByUserId() { return this.entity.UpdatedByUserId; }
    set UpdatedByUserId(value: string) {
        if (this.entity.UpdatedByUserId != value)
            this.entity.UpdatedByUserId = value;
    }
    private GetEmployeeUserName() {
        var service: ContactListService = new ContactListService();
        service.getSingle(this.UpdatedByUserId).subscribe(p => {
            this.employeeUserName=p.Result.EnglishName;
        });
    }
    get RecordedEntryAddedManually() {
        if (this.EntryTime != this.RecordedEntryTime || this.RecordedEntryTime == null)
            return true;
        return false;
    }

    get RecordedExitAddedManually() {
        if (this.ExitTime != this.RecordedExitTime || this.RecordedExitTime == null)
            return true;
        return false;
    }
    get RecordedEntryTime() { return this.entity.RecordedEntryTime; }
    get RecordedExitTime() { return this.entity.RecordedExitTime; }    
    get WorkDate() { return this.entity.WorkDate; }
    get EntryTime() { return this.entity.EntryTime; }
    get ExitTime() { return this.entity.ExitTime; }
    get Inactive() { return this.entity.Inactive; }
    get MinusItem() { return this.minusItem; }
    set MinusItem(value: boolean) { if (this.minusItem != value) this.minusItem = value; }
    get Duration() {
        return this.CalculateDifferentBetweenTwoDates(this.entity.ExitTime, this.entity.EntryTime);          
    }

    private employeeUserName: string;
    get EmployeeUserName() { return this.employeeUserName; }
    set EmployeeUserName(value: string) { if (this.employeeUserName != value) this.employeeUserName = value; }

    get EditedManualyEntryTime() {
        if (this.EntryTime != this.RecordedEntryTime && this.RecordedEntryTime != null) {
            var timeRecorded: string = "";
            var RecordedDate: Date = DateTool.GetDateParts(this.RecordedEntryTime).DateObject;
            if (RecordedDate != null) {
                timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
            }
            var timeEntry: string = "";
            var RecordedEntry: Date = DateTool.GetDateParts(this.EntryTime).DateObject;
            if (RecordedEntry != null) {
                timeEntry = RecordedEntry.getUTCHours() + ":" + (RecordedEntry.getUTCMinutes() >= 10 ? RecordedEntry.getUTCMinutes() : "0" + RecordedEntry.getUTCMinutes());
            }

            return "The value edited by " + this.EmployeeUserName + " from " + timeRecorded + " to " + timeEntry;
        }
        else if (this.RecordedEntryTime == null) {
            return "The value added manually by " + this.EmployeeUserName;
        }
    }

    get EditedManualyExitTime() {
        if (this.ExitTime != this.RecordedExitTime && this.RecordedExitTime != null) {
            var timeRecorded: string = "";
            var RecordedDate: Date = DateTool.GetDateParts(this.RecordedExitTime).DateObject;
            if (RecordedDate != null) {
                timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
            }
            var timeExit: string = "";
            var RecordedExit: Date = DateTool.GetDateParts(this.ExitTime).DateObject;
            if (RecordedExit != null) {
                timeExit = RecordedExit.getUTCHours() + ":" + (RecordedExit.getUTCMinutes() >= 10 ? RecordedExit.getUTCMinutes() : "0" + RecordedExit.getUTCMinutes());
            }

            return "The value edited by " + this.EmployeeUserName + " from " + timeRecorded + " to " + timeExit;
        }
        else if (this.RecordedExitTime == null) {
            return "The value added manually by " + this.EmployeeUserName;
        }
    }


    CalculateDifferentBetweenTwoDates(date1: Date, date2: Date) {
        var diffMs = 0;        
        if (this.entity.EntryTime != null && this.entity.ExitTime != null) {
             diffMs = (DateTool.GetDateParts(date1).DateObject.getTime() - DateTool.GetDateParts(date2).DateObject.getTime());
        }
            var diffDays = Math.floor(diffMs / 86400000); // days
        var diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
        var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
        if (diffMins < 0)
            diffHrs += 1;
        //if (diffMins > 0)
        //    diffHrs -= 1;   
        if (diffHrs < 0 || diffMins < 0)
            this.MinusItem = true;
        else
            this.MinusItem = false;

        if (diffMins < 0 && diffHrs == 0) {
                diffMins *= -1;
            return "-" + diffHrs + ":" + diffMins;
        }

        if (diffMins < 0)
            diffMins *= -1;
        if (diffMins < 10 && diffMins >= 0)
            return diffHrs + ":0" + diffMins;

        return diffHrs + ":" + diffMins;
    }
   

    get Id() { return this.entity.Id; }
    set Id(value: string) { if (this.entity.Id != value) this.entity.Id = value; }
    set WorkDate(value: Date) { if (this.entity.WorkDate != value) this.entity.WorkDate = value; }
    set EntryTime(value: Date) {        
        if (this.entity.EntryTime != value) {
            if (value == null)
                this.entity.EntryTime = value;
            else {
                if (this.entity.ExitTime != null) {
                    var a = DateTool.GetDateParts(this.entity.ExitTime).DateObject.valueOf();
                    var b = DateTool.GetDateParts(value).DateObject.valueOf();
                    if (DateTool.GetDateParts(this.entity.ExitTime).DateObject.valueOf() < DateTool.GetDateParts(value).DateObject.valueOf())
                        this.entity.EntryTime = this.entity.EntryTime;
                    else
                        this.entity.EntryTime = value;
                }
                else {
                    this.entity.EntryTime = value;
                }
            }
            this.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.GetEmployeeUserName();
        }
    }
    set ExitTime(value: Date) {
        if (this.entity.ExitTime != value) {
            if (value == null)
                this.entity.ExitTime = value;
            else {

            if (this.entity.EntryTime != null) {
                var a = DateTool.GetDateParts(value).DateObject.valueOf();
                var b = DateTool.GetDateParts(this.entity.EntryTime).DateObject.valueOf();
                    if (DateTool.GetDateParts(value).DateObject.valueOf() < DateTool.GetDateParts(this.entity.EntryTime).DateObject.valueOf()) {
                        this.entity.ExitTime = this.entity.ExitTime;
                    }
                    else
                        this.entity.ExitTime = value;
                }
            else
                this.entity.ExitTime = value;
            }        
            this.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.GetEmployeeUserName();
  
        }
    }
    get IsDirty() { return this.entity.IsDirty; }
    set IsDirty(value: boolean) { if (value != this.entity.IsDirty) this.entity.IsDirty = value; }
    set Inactive(value: boolean) { if (this.entity.Inactive != value) { this.entity.Inactive = value; this.MinusItem = value; }}
}
