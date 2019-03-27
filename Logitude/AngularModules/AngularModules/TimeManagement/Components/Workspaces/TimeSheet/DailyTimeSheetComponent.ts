import {Component, ViewChildren, QueryList} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TMProjectListService} from '../../../Services/StandardLists/TMProjectListService';
import {TimeManagementDomainService, TimeManagementAPIHelper, TimeSheetItem, TimeSheetItemDay} from '../../../Services/TimeManagementDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {DateTimePipe} from '../../../../Controls/Pipes/DateTimePipe';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {TMEmployeeTimePM} from '../../../EntityPMs/TMEmployeeTimePM';

@Component({
    selector: 'DailyTimeSheetComponent',
    moduleId: module.id,
    templateUrl: './DailyTimeSheetComponent.html',
})

export class DailyTimeSheetComponent extends BaseComponent {

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    //public ItemSource: ItemSourceItem[];
    public ItemSource: ObservableCollection;
    public DataContext = this;
    public HasChanges: boolean = false;
    public TotalFromClock = "";
    private myDomainService: TimeManagementDomainService = new TimeManagementDomainService();
    constructor() {
        super();
    }

    public LoggedUserName: string = "";
    ComputeLoggedUserName() {
        var pipe = new DateTimePipe();
        this.LoggedUserName = SessionLocator.LoggedUserPM.EnglishName + " " + pipe.transform(DateTool.GetCurrentDateAsUtc(), "SD");
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.Initialize();
            }
        }
        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitTab(arg) {
        this.ItemSource = new ObservableCollection([]);
        this.employeeUserId = SessionLocator.LoggedUserId;        
        this.locationCodeFilter = this.SelectedLocationFilter;
        this.startDate = DateTool.GetCurrentDateTimeAsUtc();
        this.endDate = this.startDate;
        this.myDomainService = new TimeManagementDomainService();
        this.ComputeLoggedUserName();
        this.Initialize();
    }
    Initialize() {
        //if (this.isLoaderReady) {
        this.LoadDailyTimeSheetList();
        //}
    }
    RefreshTab() {
        this.ComputeLoggedUserName();
        this.LoadDailyTimeSheetList();
    }
    LoadDailyTimeSheetList() {

        var itemSource: ItemSourceItem[] = [];

        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService();
        }

        this.myDomainService.GetPeriodTimeSheetList(this.EmployeeUserId, this.LocationCodeFilter, this.StartDate, this.EndDate).subscribe((myResponse: ServiceResponse) => {
            // this.ItemSource = [];
            this.ItemSource.Clear();
            if (myResponse.HasError) {
                this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection: ItemSourceItem[] = [];
                myResponse.Result.ItemsPM.forEach(item => {
                    index += 1;
                    itemSource.push(new ItemSourceItem(item, this, index));
                });

                this.ItemSource.InsertCollection(itemSource);
                setTimeout(() => this.SetTotalDatesOfList(), 2);
                this.TotalFromClock = myResponse.Result.TotalFromClock;
            }
        });
    }
    ShowMessage(msg) {
        var myMessageWindow = new MessageWindow();
        myMessageWindow.Show(msg);
    }

    public TotalDayHours: number = 0;
    public TotalDayClockHours: string = null;
    public SetTotalDatesOfList() {
        this.TotalDayHours = 0;
        this.ItemSource.Collection.forEach(item => {
            this.TotalDayHours += item.TimeInMinutes;
        });
    }

    private employeeUserId: string = null;
    get EmployeeUserId() { return this.employeeUserId; }
    set EmployeeUserId(value: string) {
        if (this.employeeUserId != value) {
            this.employeeUserId = value;            
        }
    }

    private locationCodeFilter: string;
    get LocationCodeFilter() {
        return this.locationCodeFilter;
    }
    set LocationCodeFilter(value: string) {
        if (this.locationCodeFilter != value) {
            this.locationCodeFilter = value;
            this.SaveChanges();
        }
    }

    private startDate: Date;
    get StartDate() {
        return this.startDate;
    }
    set StartDate(value: Date) {
        if (this.startDate != value) {
            this.startDate = value;
        }
    }

    private endDate: Date = null;
    get EndDate() {
        return this.endDate;
    }
    set EndDate(value: Date) {
        if (this.endDate != value) {
            this.endDate = value;
        }
    }

    // Filters
    private mySelectedLocationFilter: string = "O";
    get SelectedLocationFilter() { return this.mySelectedLocationFilter; }
    set SelectedLocationFilter(value: string) {
        if (this.mySelectedLocationFilter != value) {
            this.mySelectedLocationFilter = value;
            this.LocationCodeFilter = value;
        }
    }

    private mySelectedDateFilter: string = "T";
    get SelectedDateFilter() { return this.mySelectedDateFilter; }
    set SelectedDateFilter(value: string) {
        if (this.mySelectedDateFilter != value) {
            this.mySelectedDateFilter = value;
            this.SetPeriodDates();
        }
    }

    SetPeriodDates() {
        if (this.SelectedDateFilter == "T") {
            this.StartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EndDate = this.StartDate;
        }
        else if (this.SelectedDateFilter == "Y") {
            this.StartDate = DateTool.NextDay(DateTool.GetCurrentDateTimeAsUtc(), -1);
            this.EndDate = this.StartDate;
        }
        else {
            this.StartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EndDate = DateTool.NextDay(DateTool.GetCurrentDateTimeAsUtc(), 7);
        }

        this.SaveChanges();
    }

    // Commands
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }
    PrintPreviewClicked() {

    }
    AddLineClicked() {
        var args: any = {};
        args.IsNew = true;
        args.LocationCode = this.LocationCodeFilter != "A" ? this.LocationCodeFilter : "O";
        args.EmployeeUserId = this.EmployeeUserId;
        args.Father = this;

        if (this.SelectedDateFilter != "P") {
            var date = DateTool.GetCurrentDateTimeAsUtc();

            if (this.SelectedDateFilter == "Y") {
                date = DateTool.NextDay(DateTool.GetCurrentDateTimeAsUtc(), -1);
            }

            args.DateOfWork = date;
        }

        args.WINumber = null;
        args.ProjectId = null;
        args.Description = null;


        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Line";
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
    }
    CopyLineClicked(item: ItemSourceItem) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Copy Line";
        var args: any = {};
        args.LocationCode = item.LocationCode;
        args.EmployeeUserId = this.EmployeeUserId;
        var date = DateTool.GetCurrentDateTimeAsUtc();
        if (this.SelectedDateFilter == "Y") {
            date = DateTool.NextDay(DateTool.GetCurrentDateTimeAsUtc(), -1);
        }
        args.DateOfWork = date;
        args.Father = this;
        args.IsNew = true;
        args.WINumber = item.WINumber;
        args.ProjectId = item.ProjectId;
        args.SprintId = item.SprintId;
        args.Description = item.Description;
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
    }
    OnWindowClosed(arg: any) {
        if (arg == 'OK') {
            this.LoadDailyTimeSheetList();
        }
    }
    SearchButtonClicked() {
        if (this.StartDate == null || this.EndDate == null) {
            this.ShowMessage("Please enter both Start and End dates");
        }
        else {
            this.SaveChanges();
        }
    }

    private IsValid = true;
    SaveClicked() {
        this.IsValid = true;
        var items: ItemSourceItem[] = this.ItemSource.Collection;
        var itemsChanges: ItemSourceItem[] = this.ItemSource.Collection.filter(f => f.HasChanges == true);
        if (itemsChanges.length > 0) {
            //if (items.filter(f => f.TimeInMinutes == 0 || AppTool.IsNullOrEmpty(f.Description)).length > 0) {
            //    this.IsValid = false;
            //    this.ShowMessage("Time and Description fields are required for each line");
            //}
            if (this.IsValid) {
                SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                this.HasChanges = false;
                var myServiceHelper = new TimeManagementAPIHelper();
                myServiceHelper.Id = SessionLocator.Tenant;
                myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                myServiceHelper.LocationCode = this.LocationCodeFilter;
                myServiceHelper.StartDate = this.StartDate;
                myServiceHelper.EndDate = this.EndDate;
                itemsChanges.forEach(item => {
                    myServiceHelper.ItemsPM.push(item.entity);
                });

                if (this.myDomainService == null) {
                    this.myDomainService = new TimeManagementDomainService();
                }

                this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        this.OnDataLoaded(myResponse.Result);
                    }
                });
            }
        }
    }
    SaveChanges() {
        var items: ItemSourceItem[] = this.ItemSource.Collection;
        var itemsChanges: ItemSourceItem[] = this.ItemSource.Collection.filter(f => f.HasChanges == true);
        if (itemsChanges.length > 0) {

            //if (items.filter(f => f.TimeInMinutes == 0 || AppTool.IsNullOrEmpty(f.Description)).length > 0) {
            //    this.IsValid = false;
            //    this.ShowMessage("Time and Description fields are required for each line");
            //}
            if (this.IsValid) {
                SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                this.HasChanges = false;
                var myServiceHelper = new TimeManagementAPIHelper();
                myServiceHelper.Id = SessionLocator.Tenant;
                myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                myServiceHelper.LocationCode = this.LocationCodeFilter;
                myServiceHelper.StartDate = this.StartDate;
                myServiceHelper.EndDate = this.EndDate;
                itemsChanges.forEach(item => {
                    myServiceHelper.ItemsPM.push(item.entity);
                });

                if (this.myDomainService == null) {
                    this.myDomainService = new TimeManagementDomainService();
                }

                this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        this.LoadDailyTimeSheetList();
                    }
                });
            }
        }
        else {
            this.LoadDailyTimeSheetList();
        }
    }
    OnDataLoaded(myResultHelper: TimeManagementAPIHelper) {
        //this.ItemSource = [];
        this.ItemSource.Clear();

        if (myResultHelper) {
            var myCollection: ItemSourceItem[] = [];
            var index = 0;
            myResultHelper.ItemsPM.forEach(item => {
                index += 1;
                myCollection.push(new ItemSourceItem(item, this, index));
            });

            this.ItemSource.InsertCollection(myCollection);
            setTimeout(() => this.SetTotalDatesOfList(), 2);
            this.TotalFromClock = myResultHelper.TotalFromClock;
        }
    }
    DeleteLineClicked(item: ItemSourceItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (item != null) {
                    // var isValid = true;
                    //if (this.ItemSource.Collection.filter(f => AppTool.IsNullOrEmpty(f.ProjectId) || AppTool.IsNullOrEmpty(f.Description)).length > 0) {
                    //    isValid = false;
                    //    this.ShowMessage("Project and Description fields are required for each line");
                    //}
                    // if (isValid) {
                    SessionLocator.CurrentSession.StartBusyIndicatorSaving();
                    this.HasChanges = false;
                    if (this.myDomainService == null) {
                        this.myDomainService = new TimeManagementDomainService();
                    }
                    this.myDomainService.DeleteTimeSheetItem(item.Id, item.EmployeeUserId, item.LocationCode, this.StartDate, this.EndDate).subscribe((myResponse: ServiceResponse) => {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            this.OnDataLoaded(myResponse.Result);
                        }
                    });
                    //}
                }
            }
        });
    }
    EditLineClicked(item: ItemSourceItem) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Line";
        var args: any = {};
        args.EntityPM = item.entityPM;
        args.Father = this;
        args.IsNew = false;
        args.LocationCode = item.LocationCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
    }
    RefreshButtonClicked(){
        this.RefreshTab();
    }
}

export class ItemSourceItem extends BaseComponent {
    public DataContext = this;
    public Index: number;
    public entityPM: TMEmployeeTimePM;
    public IsCopy: boolean = false;
    private TMProjectListService: TMProjectListService;

    constructor(public entity: TMEmployeeTimePM, private father: DailyTimeSheetComponent, index: number) {
        super();
        this.entityPM = entity;
        this.TMProjectListService = new TMProjectListService();
        this.Index = index;
        this.DayDateFormat = this.ApplyTimeFormat(this.TimeInMinutes);
    }
    //public DayDateFormat = "";
    ApplyTimeFormat(minutes) {
        var formattedMinutes = "";
        var val = minutes;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            formattedMinutes = "";
        }
        else {
            formattedMinutes = result;
        }
        return formattedMinutes;
    }

    private hasChanges: boolean = false;
    get HasChanges() { return this.hasChanges; }
    set HasChanges(value: boolean) {
        if (this.hasChanges != value) {
            this.hasChanges = value;
            this.father.HasChanges = true;
        }
    }

    get Id() { return this.entity.Id; }
    set Id(value: string) {
        if (this.entity.Id != value) {
            this.entity.Id = value;
        }
    }

    get SprintId() { return this.entity.SprintId; }
    set SprintId(value: string) {
        if (this.entity.SprintId != value) {
            this.entity.SprintId = value;
        }
    }

    get ProjectId() { return this.entity.ProjectId; }
    set ProjectId(value: string) {
        if (this.entity.ProjectId != value) {
            this.entity.ProjectId = value;
            this.HasChanges = true;
            //this.entity.IsHeaderUpdated = true;
            this.getProjectName(value);
        }
    }

    private getProjectName(value: string) {
        if (this.TMProjectListService == null) {
            this.TMProjectListService = new TMProjectListService();
        }
        this.TMProjectListService.getSingle(value).subscribe((myResult: ServiceResponse) => {
            var project = myResult.Result;
            if (project != null) {
                this.ProjectName = project.Name;
            } else {
                this.ProjectName = null;
            }
        });
    }

    get ProjectName() { return this.entity.ProjectName; }
    set ProjectName(value: string) {
        if (this.entity.ProjectName != value) {
            this.entity.ProjectName = value;
            this.HasChanges = true;
        }
    }

    get LocationName() { return this.entity.LocationName; }

    get Description() { return this.entity.Description; }
    set Description(value: string) {
        if (this.entity.Description != value) {
            this.entity.Description = value;
            this.HasChanges = true;
            //this.entity.IsHeaderUpdated = true;
        }
    }

    get WINumber() { return this.entity.WINumber; }
    set WINumber(value: string) {
        if (this.entity.WINumber != value) {
            this.entity.WINumber = value;
            this.HasChanges = true;
        }
    }

    get EmployeeUserId() { return this.entity.EmployeeUserId; }
    get LocationCode() { return this.entity.LocationCode; }

    private showText = true;
    get ShowText() {
        return this.showText;
    }
    set ShowText(value: boolean) {
        if (this.showText != value) {
            this.showText = value;
        }
    }

    Ondblclicked() {
        this.ShowText = false;
    }
    OnDivBlur() {
        this.ShowText = true;
    }

    get TimeInMinutes() { return this.entity.TimeInMinutes; }
    set TimeInMinutes(value: number) {
        if (this.entity.TimeInMinutes != value) {
            this.entity.TimeInMinutes = value;
            this.HasChanges = true;
            this.DayDateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }


    private dayDateFormat: string;
    get DayDateFormat() { return this.dayDateFormat; }
    set DayDateFormat(value: string) {
        if (this.dayDateFormat != value) {
            this.dayDateFormat = value;
        }
    }

    get DateOfWork() {
        return this.entity.DateOfWork;
    }
    set DateOfWork(value: Date) {
        if (this.entity.DateOfWork != value) {
            this.entity.DateOfWork = value;
        }
    }

    //public get DayMinutes() {
    //    if (this.DayDate != null) {
    //        return this.entity.TimeInMinutes;
    //    }
    //}
    //public set DayMinutes(value: number) {
    //    if (this.DayDate.Minuts != value) {
    //        this.DayDate.Minuts = value;
    //        this.HasChanges = true;
    //        this.DayDateFormat = this.ApplyTimeFormat(value);
    //        this.father.SetTotalDatesOfList();
    //    }
    //}

    public ViewWI(wiNumber: string) {
        var url = "https://logitudeteam.visualstudio.com/DefaultCollection/LogitudeWorld/_workitems/edit/" + wiNumber;
        window.open(url);
    }
}
