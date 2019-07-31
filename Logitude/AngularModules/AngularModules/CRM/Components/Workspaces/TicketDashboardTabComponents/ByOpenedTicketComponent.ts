import {Component, ViewChildren, QueryList,ViewEncapsulation} from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DashboardWorkspaceComponent} from '../DashboardWorkspaceComponent';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LastFilterClass} from '../../../../Infrastructure/Utilities/LastFilterClass';
import {AppTool, FormatTool, DateTool} from '../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {CRMUtilities} from '../../../CRMUtilities';
import {CRMDomainService} from '../../../Services/CRMDomainService';
import {ChartingDataClass} from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {FunctionsCRM} from '../../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {List} from '../../../../Infrastructure/DataContracts/Dashboard/List';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {EmployeeGroupPM} from '../../../EntityPMs/EmployeeGroupPM';

declare var makeAmBarChart, BarClick, PieClick, makePieChart, ResetItemPie, ResetItem, makeAMLineChart, Lineclick, ResetLineclick: any;

@Component({
    moduleId: module.id,
    templateUrl: './ByOpenedTicketComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class ByOpenedTicketComponent extends BaseComponent {

    private filterName_Owner: string = "Owner";
    private filterName_EmployeeGroup: string = "EmployeeGroup";
    private filterControlNameSpace: string = "Logitude.CRM.Views.TicketMainMenu.TicketDashboardTabs.ByOpenedTicketControl";
    private filterName_CreateDate: string = "CreateDate";
    public EmployeeGroupFilterList: CodeNameClass[] = [];
    public EmployeeGroupFilterListPM: EmployeeGroupPM[] = [];
    private crmDomainService: CRMDomainService;
    private myUserListService: UserListService;
    public DateFilterList: Array<CodeNameClass> = [];
    public DataContext: ByOpenedTicketComponent = this;
    private fieldCode: string = "S";
    private NewOpenedTicketsFiltered: Array<any> = [];
    private NewSLATicketsFiltered: Array<any> = [];
    public TicketsByTicketsOwnerLegendId: string;
    public TicketsBySeverityLegendId: string;
    public TicketsByClassificationLegendId: string;
    private CurrentTicketByClassificationChart: any;
    private CurrentTicketBySeverityChart: any;
    private CurrentTicketByTicketOwnerChart: any;

    private InitializeServices() {
        this.myUserListService = new UserListService();
        this.crmDomainService = new CRMDomainService();
    }

    public TicketsByClassificationId: string;
    public TicketsBySeverityId: string;
    public TicketsByTicketOwnerId: string;
    public SLAViolationId: string;
    public OpenTicketsId: string;
    public TicketsByClassificationIdExistance: boolean = false;
    public TicketsBySeverityIdExistance: boolean = false;
    public TicketsByTicketOwnerIdExistance: boolean = false;
    public SLAViolationIdExistance: boolean = false;
    public OpenTicketsIdExistance: boolean = false;

    RefreshButtonClicked() {
        this.LoadFilteredQueries();
    }

    private BuildDateFilters() {
        this.DateFilterList = CRMUtilities.GetClosingDateFilterList();

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreateDate);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-7";
        }

        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == defaultFilterCode)[0];
    }
    private InitializeIds() {
        this.TicketsByClassificationId = "TicketsByClassificationId_" + this.CurrentSession.GetNewId("TicketsByClassificationId");
        this.TicketsBySeverityId = "TicketsBySeverityId_" + this.CurrentSession.GetNewId("TicketsBySeverityId");
        this.TicketsByTicketOwnerId = "TicketsByTicketOwnerId_" + this.CurrentSession.GetNewId("TicketsByTicketOwnerId");
        this.SLAViolationId = "SLAViolationId_" + this.CurrentSession.GetNewId("SLAViolationId");
        this.OpenTicketsId = "OpenTicketsId_" + this.CurrentSession.GetNewId("OpenTicketsId");
        this.TicketsByTicketsOwnerLegendId = "TicketsByTicketsOwnerLegendId_" + this.CurrentSession.GetNewId("TicketsByTicketsOwnerLegendId");
        this.TicketsBySeverityLegendId = "TicketsBySeverityLegendId_" + this.CurrentSession.GetNewId("TicketsBySeverityLegendId");
        this.TicketsByClassificationLegendId = "TicketsByClassificationLegendId_" + this.CurrentSession.GetNewId("TicketsByClassificationLegendId");

        
    }

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitializeIds();
        this.InitializeServices();
        this.BuildEmployeeGroupFilterList();
        this.BuildDateFilters();
    }
    private Wizard: DashboardWorkspaceComponent;
    InitTab(wizard: DashboardWorkspaceComponent) {
        this.Wizard = wizard;
    }

    RefreshTab() {
        this.RefreshButtonClicked();
    }

    public UsersFilterList: CodeNameClass[] = [];
    private BuildEmployeeGroupFilterList() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetEmployeeGroupsPMList().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: EmployeeGroupPM[] = myResponse.Result;
                this.EmployeeGroupFilterListPM = list;
                this.EmployeeGroupFilterList = [];
                this.EmployeeGroupFilterList.push(new CodeNameClass("M", "My Records"));

                if (list) {
                    list.filter(d => d.Id != SessionLocator.Tenant.toString()).forEach((item) => {
                        this.EmployeeGroupFilterList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }

                this.EmployeeGroupFilterList.push(new CodeNameClass("A", "All Records"));

                var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_EmployeeGroup);
                if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                    defaultFilterCode = "M";
                }

                this.selectedEmployeeGroupFilter = this.EmployeeGroupFilterList.filter(d => d.Code == defaultFilterCode)[0];
                this.GetSelectedEmployeeGroup();
                this.BuildUsersFilters(false);
            }
        });
    }

    private BuildUsersFilters(isUpdatingFilter: boolean) {
        this.UsersFilterList = [];

        if (this.SelectedEmployeeGroupFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();

            if (isUpdatingFilter) {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            }

            this.LoadFilteredQueries();
        }

        else {
            switch (this.SelectedEmployeeGroupFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass(SessionLocator.LoggedUserId, SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();

                        if (isUpdatingFilter) {
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                case "A":
                    {
                        var item = new CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);

                        if (isUpdatingFilter) {
                            //this.OwnerId = null;
                            //this.listOfValuesUserId = null;
                            //this.selectedUserFilter = null;
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        else {
                            this.selectedUserFilter = item;
                            this.GetSelectedOwnerId();
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                default:
                    {
                        var item = new CodeNameClass("A", "All " + this.SelectedEmployeeGroupFilter.Name + " Owners");
                        this.UsersFilterList.push(item);

                        var selectedEmployee: EmployeeGroupPM = this.EmployeeGroupFilterListPM.filter(a => a.Id == this.SelectedEmployeeGroupFilter.Code)[0];
                        var employeeGroupLinesdIds: string[] = [];
                        selectedEmployee.EmployeeGroupLines.forEach(item => {
                            employeeGroupLinesdIds.push(item.UserId);
                        });

                        var myIds = this.GetIdsString(employeeGroupLinesdIds);
                        var loadedUsersList: UserList[] = [];

                        var service: CRMDomainService = new CRMDomainService();
                        if (employeeGroupLinesdIds != null && employeeGroupLinesdIds.length > 0) {
                            service.GetUsersByEmployeeGroupIds(myIds).subscribe(myResult => {
                                var myResponse: ServiceResponse = myResult;
                                if (!myResponse.HasError) {
                                    loadedUsersList = myResponse.Result.sort((a, b) => { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1 });
                                    if (loadedUsersList != null) {
                                        loadedUsersList.forEach((item) => {
                                            var record: CodeNameClass = new CodeNameClass();
                                            record.Code = item.Id;
                                            record.Name = item.EnglishName;
                                            this.UsersFilterList.push(record);
                                        });
                                    }
                                }

                                if (isUpdatingFilter) {
                                    this.OwnerId = null;
                                    this.listOfValuesUserId = null;
                                    this.selectedUserFilter = item;
                                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                                }

                                else {
                                    var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                                    if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                                        defaultFilterCode = null;
                                    }

                                    this.OwnerId = defaultFilterCode;
                                    this.listOfValuesUserId = this.OwnerId;

                                    if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                                        this.selectedUserFilter = this.UsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                                    }

                                    if (this.selectedUserFilter == null) {
                                        this.selectedUserFilter = this.UsersFilterList[0];
                                    }
                                }

                                this.LoadFilteredQueries();

                            });
                        }

                        break;
                    }
            }
        }
    }
    GetSelectedEmployeeGroup() {
        var myResult: string = null;

        if (this.SelectedEmployeeGroupFilter) {
            switch (this.SelectedEmployeeGroupFilter.Code) {
                case "M": {
                    myResult = null;
                    break;
                }

                case "A": {
                    myResult = null;
                    break;
                }

                default: {
                    myResult = this.SelectedEmployeeGroupFilter.Code;
                    break;
                }
            }
        }

        this.EmployeeGroupId = myResult;
    }
    GetSelectedOwnerId() {
        var myResult = null;
        this.listOfValuesUserId = null;

        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    var defaultFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                        defaultFilterCode = null;
                    }

                    myResult = defaultFilterCode;
                    this.listOfValuesUserId = myResult;
                    break;
                }

                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }

        this.OwnerId = myResult;
    }

    private selectedEmployeeGroupFilter: CodeNameClass;
    get SelectedEmployeeGroupFilter() {
        return this.selectedEmployeeGroupFilter;
    }
    set SelectedEmployeeGroupFilter(newValue: CodeNameClass) {
        if (this.selectedEmployeeGroupFilter != newValue) {
            this.selectedEmployeeGroupFilter = newValue;
            this.GetSelectedEmployeeGroup();
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_EmployeeGroup, (newValue == null ? null : newValue.Code));
            this.BuildUsersFilters(false);
        }
    }

    private OwnerId: string = "";
    private EmployeeGroupId: string = "";
    private selectedUserId: string = "";
    private selectedEmployeeGroupId: string = "";

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() {
        return this.selectedUserFilter;
    }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;
            this.GetSelectedOwnerId();
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    private listOfValuesUserId: string;
    get ListOfValuesUserId() {
        return this.listOfValuesUserId;
    }
    set ListOfValuesUserId(value: string) {
        if (this.listOfValuesUserId != value) {
            this.OwnerId = value;
            this.listOfValuesUserId = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.BuildUsersFilters(true);
        }
    }


    get IsUsersFilterEnabled() {
        var myResult = false;

        if (this.SelectedEmployeeGroupFilter != null) {
            if (this.SelectedEmployeeGroupFilter.Code != "M" && this.SelectedEmployeeGroupFilter.Code != "A") {
                myResult = true;
            }
        }

        return myResult;
    }
    get IsListOfValuesVisible() {
        var myResult = false;

        if (this.SelectedEmployeeGroupFilter != null) {
            if (this.SelectedEmployeeGroupFilter.Code == "A") {
                myResult = true;
            }
        }

        return myResult;
    }
    private GetIdsString(ids: string[]) {
        var myResult = "";

        ids.forEach(Id => {
            if (AppTool.IsNullOrEmpty(myResult)) {
                myResult = Id;
            }

            else {
                myResult += ":" + Id;
            }
        });

        return myResult;
    }
  

    private selectedDateFilter: CodeNameClass;
    public get SelectedDateFilter() { return this.selectedDateFilter; }
    public set SelectedDateFilter(value: CodeNameClass) {

        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreateDate, (value == null ? null : value.Code));
            this.LoadFilteredQueries();
        }


    }
    
    LoadFilteredQueries() {
        if (this.SelectedDateFilter != null) {
            this.LoadOpenedTicketsClassificationData();
            this.LoadOpenedTicketsSeverityData();
            this.LoadOpenedTicketsOwnerData();
            this.LoadOpenedSLAViolationData();
            this.LoadOpenedTicketsByOwner();
        }

    }

    SLAClicking() {
        if (BarClick() != null) {
            this.OnSLAClick(BarClick());
            ResetItemPie();
        }
    }

    OpenTicketsClicking() {
        if (Lineclick() != null) {
            this.OnOpenTicketsClick(Lineclick());
            ResetLineclick();
        }
    }
    
    OnTicketClick(e, code) {

        var item = null;

        var myQueryCode: string = "All Tickets";
        var myTableName: string = "Ticket";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();


        if (code == "M") {
            item = this.TicketsGroupByClassification[e.index];
            filterAgrs.addAdditionalFilter("MainClassificationId", item.ClassificationId, null, null, "Equals", false, false, false, "String");
        }
        else if (code == "S") {
            item = this.TicketsGroupBySeverity[e.index];
            filterAgrs.addAdditionalFilter("SeverityId", item.SeverityId, null, null, "Equals", false, false, false, "String");
        }
        else {
            item = this.TicketsGroupByOwner[e.index];
        }
       


        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateTicketFilter", item.Code, null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("EmployeeGroupId", item.EmployeeGroupId, null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Ticket";
        listArgs.BackButtonTitle = "Ticket";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });



    }

    OnOpenTicketsClick(e) {

        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;

        if (item.values.value != 0) {
            var myQueryCode: string = "All Tickets";
            var myTableName: string = "Ticket";
            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var typeName = "Ticket";

            var myDateFilter: string = this.NewOpenedTicketsFiltered[item.index].DateTimeProperty + "?" + this.NewOpenedTicketsFiltered[item.index].GroupByCode + "?" + this.NewOpenedTicketsFiltered[item.index].Code;
            filterAgrs.addAdditionalFilter("OwnerId", this.NewOpenedTicketsFiltered[item.index].OwnerId, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("OpenedTicketsCreateDateFilter", myDateFilter, this.NewOpenedTicketsFiltered[item.index].DateTimeProperty, null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewOpenedTicketsFiltered[item.index].EmployeeGroupId, null, null, "Equals", false, false, false, "String");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = typeName;
            listArgs.BackButtonTitle = "Ticket";

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }

    }


    OnSLAClick(e) {

        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;

        if (item.values.value != 0) {
            var myQueryCode: string = "All Tickets";
            var myTableName: string = "Ticket";
            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var typeName = "Ticket";
            

            var myDateFilter: string = this.NewSLATicketsFiltered[e.target.index].DateTimeProperty[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].GroupByCode[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].DataTypeCode[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].Code[item.index] ;
            filterAgrs.addAdditionalFilter("OwnerId", this.NewSLATicketsFiltered[e.target.index].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("SLAEscalationsOpenedTicketsFilter", myDateFilter, this.NewSLATicketsFiltered[e.target.index].DateTimeProperty[item.index], null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewSLATicketsFiltered[e.target.index].EmployeeGroupId[item.index], null, null, "Equals", false, false, false, "String");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = typeName;
            listArgs.BackButtonTitle = "Ticket";

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }


    }

    LoadOpenedTicketsByOwner() {
        this.crmDomainService.GetOpenedTicketsByOpenedStage(this.SelectedDateIndex, this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(result => {
            try {
                var elm = document.getElementById(this.OpenTicketsId);
                elm.innerHTML = "";
            }
            catch (er) { }
            if (result.Result.length == 0) {
                this.OpenTicketsIdExistance = false;
            }
            else {
                this.NewOpenedTicketsFiltered = result.Result;
                this.FillTicketsByTicketOwner(result.Result);
                this.OpenTicketsIdExistance = true;
            }


        });
    }
    public ActivityList: Array<any> = [];
    public QuoteList: Array<any> = [];

    FillTicketsByTicketOwner(List: Array<ChartingDataClass>) {
        var lineData= this.FillLine(List);

       lineData.forEach(p => {
           if (p.visits != "0")
                this.OpenTicketsIdExistance = false;


        });
       try {
           var els = document.getElementById(this.OpenTicketsId);
           if (!this.OpenTicketsIdExistance) {
               makeAMLineChart(this.OpenTicketsId, lineData);
                els.hidden = false;
            }
            else {
                els.hidden = true;
            }

        }
        catch (Ex) { }


    }

    FillLine(data) {

        var index = 0;
            var lineChartData: any[] = [{ data: [], label: '' }];
        var AmLineChartTest = [];
        lineChartData = [{
            scales: {
                xAxes: [{
                    gridThickness: 0,
                }]
            },        
            xAxes: {
                gridThickness: 0,
            },
            offsetGridLines: false
            ,
            scaleShowVerticalLines: false,

            data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
        }];
        var lineChartLabels = [];
        data.forEach(element => {
            lineChartData[0].data[index] = element.IntegerProperty + "";
            lineChartLabels.push(element.LabelProperty != null ? element.LabelProperty : "");
            index++;
            
            AmLineChartTest.push({
                date: element.LabelProperty != null ? element.LabelProperty : "",
                visits: element.IntegerProperty + ""
            });

        }); 
        return AmLineChartTest;   
    }
    private selectedDateIndex: number = 0;
    get SelectedDateIndex() {
        return this.selectedDateIndex;
    }
    set SelectedDateIndex(value: number) {
        if (value != this.selectedDateIndex)
            this.SelectedDateIndex = value;
    }
    
    LoadOpenedSLAViolationData() {
        this.crmDomainService.GetOpenedTicketsBySLAViolation(this.SelectedDateIndex,this.SelectedDateFilter.Code, this.OwnerId, this.EmployeeGroupId).subscribe(result => {
            if (result.Result.length == 0) {
                this.SLAViolationIdExistance = false;
                try {
                    var elm = document.getElementById(this.SLAViolationId);
                }
                catch (er) { }
                elm.innerHTML = "";

            }
            else {
                this.SLAViolationIdExistance = true;
                this.FillSLAViolationList(result.Result);
            }

        });
    }

    FillSLAViolationList(List: Array<ChartingDataClass>) {


        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        List = List.filter(element => element.DataTypeCode == "FR" || element.DataTypeCode == "RW");
        List.forEach(element => {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], EmployeeGroupId: [], DateTimeProperty: [], GroupByCode: [], DataTypeCode: [], Code: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        List.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.LabelProperty == StringArr[i]) {
                    var k = 0;              
                     if (element.DataTypeCode == "RW")
                        k = 1;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(2);

                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].EmployeeGroupId[k] = element.EmployeeGroupId;
                    NewCustomerYAxis[i].DateTimeProperty[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].GroupByCode[k] = element.GroupByCode;
                    NewCustomerYAxis[i].DataTypeCode[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].Code[k] = element.Code;                    
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.LabelProperty);

                    }
                }
            }
        });

        var barChartColors: any[] = [
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',         
                borderWidth: 0
            },

            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',     
                borderWidth: 0,
            },
        ]
        this.NewSLATicketsFiltered = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.NewSLATicketsFiltered[i] == null) {
                    this.NewSLATicketsFiltered[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], EmployeeGroupId: [], DateTimeProperty: [], GroupByCode: [], DataTypeCode:[],Code:[] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.NewSLATicketsFiltered[i].data.push(element.data[i]);
                this.NewSLATicketsFiltered[i].BindingElement.push(element.BindingElement[i]);
                this.NewSLATicketsFiltered[i].OwnerIds.push(element.OwnerIds[i]);
                this.NewSLATicketsFiltered[i].EmployeeGroupId.push(element.EmployeeGroupId[i]);
                this.NewSLATicketsFiltered[i].DateTimeProperty.push(element.DateTimeProperty[i]);
                this.NewSLATicketsFiltered[i].GroupByCode.push(element.GroupByCode[i]);
                this.NewSLATicketsFiltered[i].DataTypeCode.push(element.DataTypeCode[i]);
                this.NewSLATicketsFiltered[i].Code.push(element.Code[i]);
                this.NewSLATicketsFiltered[i].OwnerIds.push(element.OwnerIds[i]);
                this.NewSLATicketsFiltered[i].label = element.label;
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [barChartColors[i].backgroundColor1 + "", barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1]};
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (NewCustomerXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.SLAViolationId, Graphs, DataProvider, maximum, null, null, 0,true);
            }

        }
        catch (e) {

        }






    }

    LoadOpenedTicketsOwnerData() {
        this.crmDomainService.GetOpenedTicketsGroupByOwner(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(result => {
            if (this.CurrentTicketByTicketOwnerChart != null) {
                this.CurrentTicketByTicketOwnerChart.clear();
                this.CurrentTicketByTicketOwnerChart = null;
            }
            if (result.Result.length == 0) {
                this.TicketsByTicketOwnerIdExistance = false;
                try {
                    var elm = document.getElementById(this.TicketsByTicketOwnerId);
                }
                catch (er) { }
                elm.innerHTML = "";

            }
            else {
                this.TicketsByTicketOwnerIdExistance = true;
                this.FillTicketsGroupByOwner(result.Result);
            }

        });
    }

    LoadOpenedTicketsSeverityData() {
        this.crmDomainService.GetOpenedTicketsGroupBySeverity(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(result => {
            try {
                if (this.CurrentTicketBySeverityChart != null) {
                    this.CurrentTicketBySeverityChart.clear();
                    this.CurrentTicketBySeverityChart = null;
                }
            }
            catch (er) { }

            if (result.Result.length == 0) {

                this.TicketsBySeverityIdExistance = false;
            }
            else {

                this.FillTicketsGroupBySeverity(result.Result);
                this.TicketsBySeverityIdExistance = true;

            }
        });
    }
    LoadOpenedTicketsClassificationData() {
        this.crmDomainService.GetOpenedTicketsGroupByClassification(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(result => {
            try {
                if (this.CurrentTicketByClassificationChart != null) {
                    this.CurrentTicketByClassificationChart.clear();
                    this.CurrentTicketByClassificationChart = null;
                }              
            }
            catch (er) { }

            if (result.Result.length == 0) {

                this.TicketsByClassificationIdExistance = false;
            }
            else {

                this.FillTicketsGroupByClassifications(result.Result);
                this.TicketsByClassificationIdExistance = true;

            }
        });
    }

    public TicketsGroupByClassification: Array<any> = [];
    public TicketsGroupBySeverity: Array<any> = [];
    public TicketsGroupByOwner: Array<any> = [];


    FillTicketsGroupByClassifications(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupByClassification = List;
        List.forEach(element => {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty })
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);

        });

        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
           
            this.CurrentTicketByClassificationChart= makePieChart(this.TicketsByClassificationId, fullData, false, true, this.TicketsByClassificationLegendId);
        }

    }

    TicketClicking(code: string) {
        if (PieClick() != null) {
            this.OnTicketClick(PieClick(), code);
            ResetItemPie();
        }

    }

    FillTicketsGroupBySeverity(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupBySeverity = List;
        List.forEach(element => {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty })
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);

        });

        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {          
            this.CurrentTicketBySeverityChart=  makePieChart(this.TicketsBySeverityId, fullData, false, true, this.TicketsBySeverityLegendId);
        }

    }

    FillTicketsGroupByOwner(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupByOwner = List;
        List.forEach(element => {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty })
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);

        });

        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });

        if (!flagEmpty) {
           
            this.CurrentTicketByTicketOwnerChart=makePieChart(this.TicketsByTicketOwnerId, fullData, false, true, this.TicketsByTicketsOwnerLegendId);
        }

    }


}
