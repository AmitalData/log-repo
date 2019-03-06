import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TimeOfficeHourDomainService } from '../../../Services/TimeOfficeHourDomainService';
import { DateTool, AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TMOfficeHourPM} from '../../../EntityPMs/TMOfficeHourPM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';

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
    private myDomainService: TimeOfficeHourDomainService
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.ItemSourceCollection = new ObservableCollection([]);
        this.myDomainService = new TimeOfficeHourDomainService();

        this._entityResourceService.getEntityResourceByTableName("TMOfficeHour", 0).subscribe(response => {

        });
    }

    private employeeUserId: string = null;
    get EmployeeUserId() {  return this.employeeUserId; }
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

    //DeleteLineClicked(item: ItemSourceItem) {
    //    var confirmWindow = new ConfirmWindow();
    //    confirmWindow.Show("Are you sure you want to delete this line ?");

    //    confirmWindow.WindowClosed.subscribe((event: any) => {
    //        if (confirmWindow.Yes) {
    //            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
    //            if (this.myDomainService == null) {
    //                this.myDomainService = new TimeManagementDomainService();
    //            }
    //            SessionLocator.CurrentSession.StopBusyIndicator();

    //        }
    //    });


    //}


    get HasChanged() {
        if (this.ItemSourceCollection.Collection.filter(p => p.IsDirty)[0])
            return true;
        return false;
    }

    LoadClockTimeSheet() {
        this.ValidationErrorsList = [];
        if (this.FromDate == null)
            this.ValidationErrorsList.push("From date field is required");

        if (this.ToDate == null)
            this.ValidationErrorsList.push("To date field is required");

        if (this.ToDate < this.FromDate && this.ValidationErrorsList.length == 0)
            this.ValidationErrorsList.push("From date field must be less than To date field");

        if (this.ValidationErrorsList.length == 0) {

            if (this.myDomainService == null) {
                this.myDomainService = new TimeOfficeHourDomainService();
            }

            SessionLocator.CurrentSession.StartBusyIndicatorLoading();

            this.myDomainService.GetTimeOfficeClock(this.EmployeeUserId, this.FromDate, this.ToDate).subscribe((myResponse: ServiceResponse) => {

                SessionLocator.CurrentSession.StopBusyIndicator();

                this.ItemSource = [];
                this.ItemSourceCollection.Clear();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myCollection: ItemSourceItem[] = [];
                    var index = 0;
                    myResponse.Result.sort((a, b) => { return ((a.WorkDate === b.WorkDate) ? ((a.EntryTime === b.EntryTime) ? 0 : (a.EntryTime < b.EntryTime) ? -1 : 1) : (a.WorkDate < b.WorkDate ? -1 : 1)) });
                    myResponse.Result.forEach(item => {
                        index += 1;
                        this.ItemSource.push(new ItemSourceItem(item, this, index));
                    });
                    this.ItemSourceCollection.InsertCollection(this.ItemSource);
                }

                this.ComputeTotals();
            });
        }

    }

    SaveSingleTimeOfficeHourRecord(item: TMOfficeHourPM) {
        var items: TMOfficeHourPM[] = [];

        if (item.IsDirty) {
            items.push(item);
        }

        if (items.length != 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();

            this.myDomainService.UpdateOfficeHourList(items).subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    item.IsDirty = false;
                }

                this.ComputeTotals();
            });
        }
    }

    SaveTimeOfficeHour() {
        var items: TMOfficeHourPM[] = [];

        this.ItemSourceCollection.Collection.filter(p => p.IsDirty).forEach(p => {
            items.push(p.entity);
        });

        if (items.length != 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            
            this.myDomainService.UpdateOfficeHourList(items).subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.ItemSourceCollection.Collection.forEach((p: ItemSourceItem) => {
                        p.EntityPM.IsDirty = false;
                    });
                }

                this.ComputeTotals();
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
        this.myDomainService = new TimeOfficeHourDomainService();
        this.LoadClockTimeSheet();
    }

    SearchButtonClicked() {
        if (this.ItemSourceCollection.Collection.filter(p => p.IsDirty).length > 0) {
            this.SaveTimeOfficeHour();
        }

        else {
            this.LoadClockTimeSheet();
        }
    }

    RefreshTab() {

    }

    PrintPreviewClicked() {

    }

    public TotalMinutes: number;
    public ComputeTotals() {
        this.TotalMinutes = ArrayTool.Sum(this.ItemSourceCollection.Collection.filter(f => f.Inactive == false), "Minutes");
    }
}

export class ItemSourceItem extends BaseComponent {
    public EntityPM: TMOfficeHourPM
    public DataContext = this;
    public Index: number;   
    public DivId: string;
    public IsCopy: boolean = false;
    constructor(public entity: TMOfficeHourPM, private father: ClockTimeComponent, index: number) {
        super();
        this.EntityPM = entity;
        this.Index = index;        
        var idIndex = SessionLocator.CurrentSession.GetNewId("DIV");
        this.DivId = "DIV_" + idIndex;
    }  

    get Id() { return this.EntityPM.Id; }
    get IsDirty() { return this.EntityPM.IsDirty; }
    
    get WorkDate() { return this.EntityPM.WorkDate; }
    set WorkDate(value: Date) {
        if (this.EntityPM.WorkDate != value) {
            this.EntityPM.WorkDate = value;
        }
    }

    get EntryTime() { return this.EntityPM.EntryTime; }
    set EntryTime(value: Date) {
        if (this.EntityPM.EntryTime != value) {

            var iResult: Date = null;

            if (value) {
                var iDateParts = DateTool.GetDateParts(value);

                iResult = DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;

                iResult.setUTCHours(iDateParts.Hours);
                iResult.setUTCMinutes(iDateParts.Minutes);

                if (this.ExitTime) {
                    if (DateTool.GetDateParts(iResult).DateTicks > DateTool.GetDateParts(this.ExitTime).DateTicks) {
                        iResult = this.EntryTime;
                    }
                }
            }

            this.EntityPM.EntryTime = iResult;
            this.ComputeMinutes();
        }
    }

    get ExitTime() { return this.EntityPM.ExitTime; }
    set ExitTime(value: Date) {
        if (this.EntityPM.ExitTime != value) {

            var iResult: Date = null;

            if (value) {
                var iDateParts = DateTool.GetDateParts(value);

                iResult = DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;

                iResult.setUTCHours(iDateParts.Hours);
                iResult.setUTCMinutes(iDateParts.Minutes);

                if (this.EntryTime) {
                    if (DateTool.GetDateParts(iResult).DateTicks < DateTool.GetDateParts(this.EntryTime).DateTicks) {
                        iResult = this.ExitTime;
                    }
                }
            }

            this.EntityPM.ExitTime = iResult;
            this.ComputeMinutes();
        }
    }

    get Minutes() { return this.EntityPM.Minutes; }
    set Minutes(value: number) {
        if (this.EntityPM.Minutes != value) {
            this.EntityPM.Minutes = value;
            this.father.ComputeTotals();
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(value: string) {
        if (this.EntityPM.UpdatedByUserId != value) {
            this.EntityPM.UpdatedByUserId = value;
        }
    }

    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    set UpdatedByUserName(value: string) {
        if (this.EntityPM.UpdatedByUserName != value) {
            this.EntityPM.UpdatedByUserName = value;
        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
            this.father.ComputeTotals();
            this.SaveSingleLine();
        }
    }
    
    get RecordedEntryAddedManually() {
        if (this.EntryTime != this.EntityPM.RecordedEntryTime || this.EntityPM.RecordedEntryTime == null)
            return true;
        return false;
    }
    get RecordedExitAddedManually() {
        if (this.ExitTime != this.EntityPM.RecordedExitTime || this.EntityPM.RecordedExitTime == null)
            return true;
        return false;
    }
    get EditedManualyEntryTime() {
        if (this.EntryTime != this.EntityPM.RecordedEntryTime && this.EntityPM.RecordedEntryTime != null) {
            var timeRecorded: string = "";
            var RecordedDate: Date = DateTool.GetDateParts(this.EntityPM.RecordedEntryTime).DateObject;
            if (RecordedDate != null) {
                timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
            }
            var timeEntry: string = "";
            var RecordedEntry: Date = DateTool.GetDateParts(this.EntryTime).DateObject;
            if (RecordedEntry != null) {
                timeEntry = RecordedEntry.getUTCHours() + ":" + (RecordedEntry.getUTCMinutes() >= 10 ? RecordedEntry.getUTCMinutes() : "0" + RecordedEntry.getUTCMinutes());
            }

            return "The value edited by " + this.UpdatedByUserName + " from " + timeRecorded + " to " + timeEntry;
        }
        else if (this.EntityPM.RecordedEntryTime == null) {
            return "The value added manually by " + this.UpdatedByUserName;
        }
    }
    get EditedManualyExitTime() {
        if (this.ExitTime != this.EntityPM.RecordedExitTime && this.EntityPM.RecordedExitTime != null) {
            var timeRecorded: string = "";
            var RecordedDate: Date = DateTool.GetDateParts(this.EntityPM.RecordedExitTime).DateObject;
            if (RecordedDate != null) {
                timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
            }
            var timeExit: string = "";
            var RecordedExit: Date = DateTool.GetDateParts(this.ExitTime).DateObject;
            if (RecordedExit != null) {
                timeExit = RecordedExit.getUTCHours() + ":" + (RecordedExit.getUTCMinutes() >= 10 ? RecordedExit.getUTCMinutes() : "0" + RecordedExit.getUTCMinutes());
            }

            return "The value edited by " + this.UpdatedByUserName + " from " + timeRecorded + " to " + timeExit;
        }
        else if (this.EntityPM.RecordedExitTime == null) {
            return "The value added manually by " + this.UpdatedByUserName;
        }
    }

    ComputeMinutes() {
        var iResult: number = 0;

        if (this.EntryTime && this.ExitTime) {
            var ExitTimeTotalMinutes = DateTool.GetDateParts(this.ExitTime).TotalMinutes;
            var EntryTimeTotalMinutes = DateTool.GetDateParts(this.EntryTime).TotalMinutes;
            iResult = ExitTimeTotalMinutes - EntryTimeTotalMinutes;
        }

        this.Minutes = iResult;

        this.SaveSingleLine();
    }

    private SaveSingleLine() {
         this.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;

        this.father.SaveSingleTimeOfficeHourRecord(this.EntityPM);
    }
}
