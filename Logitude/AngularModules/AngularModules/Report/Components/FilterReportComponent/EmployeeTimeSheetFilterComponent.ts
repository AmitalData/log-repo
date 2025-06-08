import {Component}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { UserListService } from '../../../Common/Services/StandardLists/UserListService';
import { UserList } from '../../../Common/EntityLists/UserList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({    
    selector: 'EmployeeTimeSheetFilterComponent',
    templateUrl: './EmployeeTimeSheetFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class EmployeeTimeSheetFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    public DataContext: EmployeeTimeSheetFilterComponent = this;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    EmployeeUserId: string;
    TimeRequired: number;
    queryFilterItems: QueryFilterItem[];
    public AgentId = null;
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "TMEmployeeTime";
    private CurrentSession = SessionLocator.SelectedSession;
    public EmployeesSelected: string = null;
    constructor() {
        super();
        this.DateOfWorkMinutes = 525;

        //this.DropButtonId += this.CurrentSession.GetNewId("DropButtonId_1");
        //this.SearchTextBoxId += this.CurrentSession.GetNewId("SearchTextBoxId_1");

        this.GetUsers();
    }

    private dateOfWorkMinutes: number;;
    get DateOfWorkMinutes() {
        return this.dateOfWorkMinutes;
    }
    set DateOfWorkMinutes(value: number) {
        if (this.dateOfWorkMinutes != value) {
            this.dateOfWorkMinutes = value;
            this.DateOfWorkDateFormat = this.ApplyTimeFormat(value);
        }
    }
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

    private dateOfWorkDateFormat = "";
    get DateOfWorkDateFormat() {
        return this.dateOfWorkDateFormat;
    }
    set DateOfWorkDateFormat(value: string) {
        if (this.dateOfWorkDateFormat != value) {
            this.dateOfWorkDateFormat = value;
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.EmployeeUserId = SessionLocator.LoggedUserId;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);

        this.ReportsPreview = myReportsPreview;        
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }

    public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {

        if (this.IsSchedulerReport) {
            this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
        }
        else {
            this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
        }

    }
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>, isSchedulerReport: boolean = true) {
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {

        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "Employees":
                    this.EmployeesSelected = queryFilterItem.FieldValue;
                    break;
                case "TimeRequired":
                    this.DateOfWorkMinutes = queryFilterItem.FieldValue;
                    break;
               

            }

        }
    }
    GetQueryFilterItems(){
        var myEmployees: string = "";

        if (this.UsersComboList.filter(i => i.Checked)[0] == null) {
            this.UsersComboList.forEach((i) => {
                myEmployees += i.Id + ",";
            });
        }
        else {
            this.UsersComboList.forEach((i) => {
                if (i.Checked) {
                    myEmployees += i.Id + ",";
                }
            });
        }
    

        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "FromDate";
        this.queryFilterItem.FieldValue = this.FromDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ToDate";
        this.queryFilterItem.FieldValue = this.ToDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "Employees";
        this.queryFilterItem.FieldValue = myEmployees;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "TimeRequired";
        this.TimeRequired = this.DateOfWorkMinutes;
        this.queryFilterItem.FieldValue = this.TimeRequired;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return this.queryFilterItems;
    }
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");
        }

        if (this.FromDate > this.ToDate) {
            this.ValidationErrorsList.push("From Date cannot be greater than To Date");
        }

        return this.ValidationErrorsList.length == 0;
    }
    RunReport() {
       
        if (this.ValidateSelectedFilters()) {
          
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    }

    public SelectedUsers: string;
    EditedItemSource(newSource: any) {
        this.UsersComboList = newSource;
    }

    public UsersComboList: UserItemClass[] = [];
    GetUsers() {
        var AddtionalService: UserListService = new UserListService();
        AddtionalService.getAllFromCache().subscribe((result: any) => {
            var usesrList: UserList[] = result.Result.filter(s => !s.InActive);
            usesrList.sort((a, b) => { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1 });

            usesrList.forEach((item) => {
                var isLoggedUser = false;
                if (item.Id == SessionLocator.LoggedUserId || this.EmployeesSelected?.indexOf(item.Id) > -1) {
                    isLoggedUser = true;
                }
                this.UsersComboList.push(new UserItemClass(item, isLoggedUser));
            });
        });
    }
}

export class UserItemClass {
    public entityList: UserList;
    constructor(entityList: UserList, isLoggedUser: boolean) {
        this.entityList = entityList;
        if (isLoggedUser) {
            this.Checked = true;
        }
    }

    get Id() { return this.entityList.Id; }
    get Name() { return this.entityList.EnglishName; }

    private checked: boolean;
    public get Checked() { return this.checked; }
    public set Checked(value: boolean) { this.checked = value; }
}
