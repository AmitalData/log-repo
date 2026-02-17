import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TMEmployeeTimePM} from '../../../EntityPMs/TMEmployeeTimePM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TMProjectListService} from '../../../Services/StandardLists/TMProjectListService'; 
import {TimeManagementDomainService, TimeManagementAPIHelper, TimeSheetItem, TimeSheetItemDay} from '../../../Services/TimeManagementDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow'; 

@Component({
    selector: 'WeeklyTimeSheetComponent',
    moduleId: module.id,
    templateUrl: './WeeklyTimeSheetComponent.html',
    providers: [EntityResourceService],
})

export class WeeklyTimeSheetComponent extends BaseComponent {
    //public ItemSource: ObservableCollection;
    public ItemSource: ItemSourceItem[];
    public DataContext = this;
    public HasChanges: boolean = false;
    private myDomainService: TimeManagementDomainService = new TimeManagementDomainService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
    }
    InitTab(arg) {
        this.employeeUserId = SessionLocator.LoggedUserId;
        this.locationCode = this.SelectedLocationFilter;
        this.periodStartDate = DateTool.GetStartOfTheWeek(DateTool.GetCurrentDateTimeAsUtc());
        this.SetDates();
        //this.ItemSource = new ObservableCollection([]);
        this.myDomainService = new TimeManagementDomainService();
        this.LoadWeeklyTimeSheetList();
    }
    RefreshTab() {
        this.LoadWeeklyTimeSheetList();
    }
    LoadWeeklyTimeSheetList() {
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService();
        }
        this.myDomainService.GetWeeklyTimeSheetList(this.EmployeeUserId, this.LocationCode, this.PeriodStartDate).subscribe((myResponse: ServiceResponse) => {
            // this.ItemSource.Clear();
            this.ItemSource = [];
            if (myResponse.HasError) {
                this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection: ItemSourceItem[] = [];

                myResponse.Result.Items.forEach(item => {
                    index += 1;
                    this.ItemSource.push(new ItemSourceItem(item, this, index));
                });

                setTimeout(() => this.SetTotalDatesOfList(), 2);
                setTimeout(() => this.SetTotalDatesFromClockOfList(myResponse.Result.OfficeClockDays), 2);
            }
        });
    }
    ShowMessage(msg) {
        var myMessageWindow = new MessageWindow();
        myMessageWindow.Show(msg);
    }

    public MonDate: Date;
    public TueDate: Date;
    public WedDate: Date;
    public ThuDate: Date;
    public SatDate: Date;
    public FriDate: Date;
    SetDates() {
        if (this.periodStartDate != null) {
            this.MonDate = DateTool.NextDay(this.periodStartDate, 1);
            this.TueDate = DateTool.NextDay(this.periodStartDate, 2);
            this.WedDate = DateTool.NextDay(this.periodStartDate, 3);
            this.ThuDate = DateTool.NextDay(this.periodStartDate, 4);
            this.FriDate = DateTool.NextDay(this.periodStartDate, 5);
            this.SatDate = DateTool.NextDay(this.periodStartDate, 6);

        }
    }

    public TotalSunDayHours: number = 0;
    public TotalMonDayHours: number = 0;
    public TotalTueDayHours: number = 0;
    public TotalWedDayHours: number = 0;
    public TotalThuDayHours: number = 0;
    public TotalSatDayHours: number = 0;
    public TotalFriDayHours: number = 0;
    public TotalWeekHours: number = 0;

    public TotalSunDayClockHours: string = null;
    public TotalMonDayClockHours: string = null;
    public TotalTueDayClockHours: string = null;
    public TotalWedDayClockHours: string = null;
    public TotalThuDayClockHours: string = null;
    public TotalSatDayClockHours: string = null;
    public TotalFriDayClockHours: string = null;
    public TotalWeekClockHours: number = 0;
    public SetTotalDatesOfList() {
        this.TotalSunDayHours = 0;
        this.TotalMonDayHours = 0;
        this.TotalTueDayHours = 0;
        this.TotalWedDayHours = 0;
        this.TotalThuDayHours = 0;
        this.TotalSatDayHours = 0;
        this.TotalFriDayHours = 0;
        this.TotalWeekHours = 0;
        this.ItemSource.forEach(item => {
            this.TotalSunDayHours += item.Day1Minutes;
            this.TotalMonDayHours += item.Day2Minutes;
            this.TotalTueDayHours += item.Day3Minutes;
            this.TotalWedDayHours += item.Day4Minutes;
            this.TotalThuDayHours += item.Day5Minutes;
            this.TotalFriDayHours += item.Day6Minutes;
            this.TotalSatDayHours += item.Day7Minutes;
            this.TotalWeekHours += item.TotalMinutes;
        });
    }

    public SetTotalDatesFromClockOfList(items: TimeSheetItemDay[]) {
        var totalSunDayClockHoursString = items[0].TotalFromClockString;
        var totalMonDayClockHoursString = items[1].TotalFromClockString;
        var totalTueDayClockHoursString = items[2].TotalFromClockString;
        var totalWedDayClockHoursString = items[3].TotalFromClockString;
        var totalThuDayClockHoursString = items[4].TotalFromClockString;
        var totalSatDayClockHoursString = items[5].TotalFromClockString;
        var totalFriDayClockHoursString = items[6].TotalFromClockString;

        var totalSunDayClockHours = items[0].TotalFromClock;
        var totalMonDayClockHours = items[1].TotalFromClock;
        var totalTueDayClockHours = items[2].TotalFromClock;
        var totalWedDayClockHours = items[3].TotalFromClock;
        var totalThuDayClockHours = items[4].TotalFromClock;
        var totalSatDayClockHours = items[5].TotalFromClock;
        var totalFriDayClockHours = items[6].TotalFromClock;

        this.TotalSunDayClockHours = totalSunDayClockHoursString;
        this.TotalMonDayClockHours = totalMonDayClockHoursString;
        this.TotalTueDayClockHours = totalTueDayClockHoursString;
        this.TotalWedDayClockHours = totalWedDayClockHoursString;
        this.TotalThuDayClockHours = totalThuDayClockHoursString;
        this.TotalSatDayClockHours = totalSatDayClockHoursString;
        this.TotalFriDayClockHours = totalFriDayClockHoursString;
        this.TotalWeekClockHours = (totalSunDayClockHours + totalMonDayClockHours + totalTueDayClockHours +
            totalWedDayClockHours + totalThuDayClockHours + totalSatDayClockHours + totalFriDayClockHours) * 60;
    }

    private employeeUserId: string = null;
    get EmployeeUserId() {
        return this.employeeUserId;
    }
    set EmployeeUserId(value: string) {
        if (this.employeeUserId != value) {
            this.employeeUserId = value;
            this.LoadWeeklyTimeSheetList();
        }
    }

    private locationCode: string;
    get LocationCode() {
        return this.locationCode;
    }
    set LocationCode(value: string) {
        if (this.locationCode != value) {
            this.locationCode = value;
            this.LoadWeeklyTimeSheetList();
        }
    }

    private periodStartDate: Date;
    get PeriodStartDate() {
        return this.periodStartDate;
    }
    set PeriodStartDate(value: Date) {
        if (this.periodStartDate != value) {
            this.periodStartDate = value;
            this.SetDates();
            this.LoadWeeklyTimeSheetList();
        }
    }

    // Filters 
    private mySelectedLocationFilter: string = "O";
    get SelectedLocationFilter() { return this.mySelectedLocationFilter; }
    set SelectedLocationFilter(value: string) {
        if (this.mySelectedLocationFilter != value) {
            this.mySelectedLocationFilter = value;
            this.LocationCode = value;
        }
    }

    // Commands
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }
    PrintPreviewClicked() {


    }
    AddLineClicked() {
        //this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "New Line";
            var args: any = {};
            args.LocationCode = this.LocationCode;
            args.EmployeeUserId = this.EmployeeUserId;
            args.SunDate = this.PeriodStartDate;
            args.MonDate = this.MonDate;
            args.TueDate = this.TueDate;
            args.WedDate = this.WedDate;
            args.ThuDate = this.ThuDate;
            args.SatDate = this.SatDate;
            args.FriDate = this.FriDate;
            args.Father = this;
            args.WINumber = "";
            args.ProjectId ="";
            args.Description = "";
            logWindow.WindowArgs = args;
            logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
        //});
    }
    CopyLineClicked(item: ItemSourceItem) {
        //this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Copy Line";
            var args: any = {};
            args.LocationCode = this.LocationCode;
            args.EmployeeUserId = this.EmployeeUserId;
            args.SunDate = this.PeriodStartDate;
            args.MonDate = this.MonDate;
            args.TueDate = this.TueDate;
            args.WedDate = this.WedDate;
            args.ThuDate = this.ThuDate;
            args.SatDate = this.SatDate;
            args.FriDate = this.FriDate;
            args.Father = this;
            args.WINumber = item.WINumber;
            args.ProjectId = item.ProjectId;
            args.Description = item.Description;
            logWindow.WindowArgs = args;
            logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
        //});
    }
    OnWindowClosed(arg: any) {
        if (arg == 'OK') {
            this.LoadWeeklyTimeSheetList();
        }
    }
    SaveWeeklyClicked() {
        var items: ItemSourceItem[] = this.ItemSource;
        var itemsChanges: ItemSourceItem[] = this.ItemSource.filter(f => f.HasChanges == true);

        if (itemsChanges.length > 0) {
            var isValid: boolean = true;

            if (items.filter(f => AppTool.IsNullOrEmpty(f.ProjectId) || AppTool.IsNullOrEmpty(f.Description)).length > 0) {
                isValid = false;
                this.ShowMessage("Project and Description fields are required for each line");
            }

            //else if (items.filter(f => AppTool.IsNullOrEmpty(f.Description)).length > 0) {
            //    isValid = false;
            //    this.ShowMessage("Description field is required for each line");
            //}

            if (isValid) {
                var groupeditems: ItemSourceItem[] = [];

                items.forEach(item => {
                    if (groupeditems.filter(f => f.ProjectId == item.ProjectId && f.Description == item.Description && f.WINumber == item.WINumber).length > 0) {
                        isValid = false;
                    }

                    else {
                        groupeditems.push(item);
                    }
                });


                if (!isValid) {
                    this.ShowMessage("Can't add more than one line with the same Project, Description, WINumber");
                }

                else {
                  
                    this.CurrentSession.StartBusyIndicatorSaving();
                    this.HasChanges = false;
                    var myServiceHelper = new TimeManagementAPIHelper();
                    myServiceHelper.Id = SessionLocator.Tenant;
                    myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                    myServiceHelper.LocationCode = this.LocationCode;
                    myServiceHelper.StartDate = this.PeriodStartDate;

                    itemsChanges.forEach(item => {
                        myServiceHelper.Items.push(item.entity);
                    });

                    if (this.myDomainService == null) {
                        this.myDomainService = new TimeManagementDomainService();
                    }

                    this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            this.OnDataLoaded(myResponse.Result);
                        }
                    });
                }
            }
        }
    }
    OnDataLoaded(myResultHelper: TimeManagementAPIHelper) {
        this.ItemSource = [];
        if (myResultHelper) {
            var myCollection: ItemSourceItem[] = [];
            var index = 0;
            myResultHelper.Items.forEach(item => {
                //myCollection.push(new ItemSourceItem(item, this));
                index += 1;
                this.ItemSource.push(new ItemSourceItem(item, this, index));
            });
            //this.ItemSource.InsertCollection(myCollection);
            setTimeout(() => this.SetTotalDatesOfList(), 2);
            setTimeout(() => this.SetTotalDatesFromClockOfList(myResultHelper.OfficeClockDays), 2);

        }
    }

    GetProjectsClicked() {
        var employee = this.EmployeeUserId;
        var location = this.LocationCode;
        var periodDate = this.PeriodStartDate;

        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService();
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetTMProjects(this.EmployeeUserId, this.LocationCode, this.PeriodStartDate).subscribe((myResponse: ServiceResponse) => {
            this.ItemSource = [];
            if (myResponse.HasError) {
                this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection: ItemSourceItem[] = [];

                myResponse.Result.Items.forEach(item => {
                    index += 1;
                    this.ItemSource.push(new ItemSourceItem(item, this, index));
                });

                setTimeout(() => this.SetTotalDatesOfList(), 2);
                setTimeout(() => this.SetTotalDatesFromClockOfList(myResponse.Result.OfficeClockDays), 2);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
}
export class ItemSourceItem extends BaseComponent {
    public DataContext = this;
    public Index: number;
    public Day1Date: TimeSheetItemDay;
    public Day2Date: TimeSheetItemDay;
    public Day3Date: TimeSheetItemDay;
    public Day4Date: TimeSheetItemDay;
    public Day5Date: TimeSheetItemDay;
    public Day6Date: TimeSheetItemDay;
    public Day7Date: TimeSheetItemDay;
    public IsCopy: boolean = false; 
    private TMProjectListService: TMProjectListService;

    constructor(public entity: TimeSheetItem, private father: WeeklyTimeSheetComponent, index:number) {
        super();
        this.TMProjectListService = new TMProjectListService();
        this.Index = index;
        this.Day1Date = entity.Days.filter(d => d.Index == 0)[0];
        this.Day2Date = entity.Days.filter(d => d.Index == 1)[0];
        this.Day3Date = entity.Days.filter(d => d.Index == 2)[0];
        this.Day4Date = entity.Days.filter(d => d.Index == 3)[0];
        this.Day5Date = entity.Days.filter(d => d.Index == 4)[0];
        this.Day6Date = entity.Days.filter(d => d.Index == 5)[0];
        this.Day7Date = entity.Days.filter(d => d.Index == 6)[0];

        this.Day1DateFormat = this.ApplyTimeFormat(this.Day1Minutes);
        this.Day2DateFormat = this.ApplyTimeFormat(this.Day2Minutes);
        this.Day3DateFormat = this.ApplyTimeFormat(this.Day3Minutes);
        this.Day4DateFormat = this.ApplyTimeFormat(this.Day4Minutes);
        this.Day5DateFormat = this.ApplyTimeFormat(this.Day5Minutes);
        this.Day6DateFormat = this.ApplyTimeFormat(this.Day6Minutes);
        this.Day7DateFormat = this.ApplyTimeFormat(this.Day7Minutes);
    }
    public Day1DateFormat = ""; 
    public Day2DateFormat = ""; 
    public Day3DateFormat = ""; 
    public Day4DateFormat = ""; 
    public Day5DateFormat = ""; 
    public Day6DateFormat = ""; 
    public Day7DateFormat = ""; 
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

    get ProjectId() { return this.entity.ProjectId; }
    set ProjectId(value: string) {
        if (this.entity.ProjectId != value) {
            this.entity.ProjectId = value;   
            this.HasChanges = true;     
            this.entity.IsHeaderUpdated = true;
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

    get Description() { return this.entity.Description; }
    set Description(value: string) {
        if (this.entity.Description != value) {
            this.entity.Description = value;
            this.HasChanges = true;
            this.entity.IsHeaderUpdated = true;
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

    public get Day1Minutes() {
        if (this.Day1Date != null) {
            return this.Day1Date.Minuts;
        }
    }
    public set Day1Minutes(value: number) {
        if (this.Day1Date.Minuts != value) {
            this.Day1Date.Minuts  = value;
            this.HasChanges = true;
            this.Day1DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day2Minutes() {
        if (this.Day2Date != null) {
            return this.Day2Date.Minuts;
        }
    }
    set Day2Minutes(value: number) {
        if (this.Day2Date.Minuts  != value) {
            this.Day2Date.Minuts  = value;
            this.HasChanges = true;
            this.Day2DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day3Minutes() {
        if (this.Day3Date != null) {
            return this.Day3Date.Minuts;
        }
    }
    set Day3Minutes(value: number) {
        if (this.Day3Date.Minuts  != value) {
            this.Day3Date.Minuts  = value;
            this.HasChanges = true;
            this.Day3DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day4Minutes() {
        if (this.Day4Date != null) {
            return this.Day4Date.Minuts;
        }
    }
    set Day4Minutes(value: number) {
        if (this.Day4Date.Minuts  != value) {
            this.Day4Date.Minuts  = value;
            this.HasChanges = true;
            this.Day4DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day5Minutes() {
        if (this.Day5Date != null) {
            return this.Day5Date.Minuts;
        }
    }
    set Day5Minutes(value: number) {
        if (this.Day5Date.Minuts  != value) {
            this.Day5Date.Minuts  = value;
            this.HasChanges = true;
            this.Day5DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day6Minutes() {
        if (this.Day6Date != null) {
            return this.Day6Date.Minuts;
        }
    }
    set Day6Minutes(value: number) {
        if (this.Day6Date.Minuts  != value) {
            this.Day6Date.Minuts  = value;
            this.HasChanges = true;
            this.Day6DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get Day7Minutes() {
        if (this.Day7Date != null) {
            return this.Day7Date.Minuts;
        }
    }
    set Day7Minutes(value: number) {
        if (this.Day7Date.Minuts  != value) {
            this.Day7Date.Minuts = value;
            this.HasChanges = true;
            this.Day7DateFormat = this.ApplyTimeFormat(value);
            this.father.SetTotalDatesOfList();
        }
    }

    get TotalMinutes() { return this.entity.TotalMinutes; }
    set TotalMinutes(value: number) {
        if (this.entity.TotalMinutes != value) {
            this.entity.TotalMinutes = value;
            this.HasChanges = true;
        }
    }

    public ViewWI(wiNumber: string) {
        var url = "https://logitudeteam.visualstudio.com/DefaultCollection/LogitudeWorld/_workitems/edit/" + wiNumber;
        window.open(url);
    }
}
