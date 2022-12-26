import {Component, Output, EventEmitter} from '@angular/core';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LastFilterClass } from '../../../Infrastructure/Utilities/LastFilterClass';
import { BusinessUnitListService } from '../../../Common/Services/StandardLists/BusinessUnitListService';
import { BusinessUnitList } from '../../../Common/EntityLists/BusinessUnitList';
import { UserListService } from '../../../Common/Services/StandardLists/UserListService';
import { UserList } from '../../../Common/EntityLists/UserList';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    
    templateUrl: './QuoteFiltersMenuComponent.html',
})

export class QuoteFiltersMenuComponent extends BaseComponent {
    public DataContext: QuoteFiltersMenuComponent = this;
    @Output() SelectedValueChanged = new EventEmitter();
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();

    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;

    DirectionFilter_E: string;
    DirectionFilter_R: string;
    DirectionFilter_D: string;
    DirectionFilter_I: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        if (this.CurrentSession == null) {
            this.TransportFilter_A = "TransportFilter_A_-1_-1";
            this.TransportFilter_O = "TransportFilter_O_-1_-1";
            this.TransportFilter_I = "TransportFilter_I_-1_-1";
            this.DirectionFilter_E = "DirectionFilter_E_-1_-1";
            this.DirectionFilter_I = "DirectionFilter_I_-1_-1";
            this.DirectionFilter_R = "DirectionFilter_R_-1_-1";
            this.DirectionFilter_D = "DirectionFilter_D_-1_-1";
        }

        else {
            var index_T = this.CurrentSession.GetNewId("QuoteTransportFilterMenu");
            var index_D = this.CurrentSession.GetNewId("QuoteDirectionFilterMenu");
            this.TransportFilter_A = "TransportFilter_A" + index_T;
            this.TransportFilter_O = "TransportFilter_O" + index_T;
            this.TransportFilter_I = "TransportFilter_I" + index_T;
            this.DirectionFilter_E = "DirectionFilter_E" + index_D;
            this.DirectionFilter_R = "DirectionFilter_R" + index_D;
            this.DirectionFilter_D = "DirectionFilter_D" + index_D;
            this.DirectionFilter_I = "DirectionFilter_I" + index_D;
        }

        this.InitializeServices();
        this.InitializeFilters();
    }

    /////////////////////////////////////

    public SelectedTransportMode: string = "All";
    
    TransportModeItemClicked(itemValue: string) {
        if (this.SelectedTransportMode != itemValue) {
            this.SelectedTransportMode = itemValue;
            var RemoveFilter = false;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId");
            }

            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            if (itemValue == "All") {
                RemoveFilter = true;
            }

            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });

            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_G.png");

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }
    }
    TransportModeItemMouseOver(itemValue: string) {
        if (this.SelectedTransportMode != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    break;
                }
            }
        }
    }
    TransportModeItemMouseLeave(itemValue: string) {
        if (this.SelectedTransportMode != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }

    /////////////////////////////////////

    public SelectedDirection: string = "All";

    DirectionItemClicked(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            this.SelectedDirection = itemValue;
            var RemoveFilter = false;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DirectionId");
            }

            this.apiQueryFilters.addAdditionalFilter("DirectionId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });

            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);
            img_E.setAttribute("src", "./Images/Directions/E_g.png");
            img_I.setAttribute("src", "./Images/Directions/I_g.png");
            img_R.setAttribute("src", "./Images/Directions/R_G.png");
            img_D.setAttribute("src", "./Images/Directions/D_G.png");

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_w.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_w.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_w.png");
                    break;
                }
            }
        }
    }
    DirectionItemMouseOver(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D.png");
                    break;
                }
            }
        }
    }
    DirectionItemMouseLeave(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_g.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_g.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_g.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_g.png");
                    break;
                }
            }
        }
    } 

    /////////////Created/Business units filters////////////////

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Simplog.QuoteLib.Views.QuotesMainMenu.QuotesMainControl";
    private myUserListService: UserListService;
    private myBusinessUnitListService: BusinessUnitListService;
    public OwnerId: string = null;
    public BusinessUnitId: string = null;
    public RecordsTypeFilterCode: string = null;
    public CreatedByTypeFilterCode: string = null;
    public BusinessUnitFilterCode: string = null;
    public RecordsTypesFilterList: CodeNameClass[] = [];
    public CreatedByTypesFilterList: CodeNameClass[] = [];
    public BusinessUnitFilterList: CodeNameClass[] = [];
    public BusinessUnitUsersFilterList: CodeNameClass[] = [];
    public IsBusinessUnitUsers: boolean = false;

    private InitializeServices() {
        this.myUserListService = new UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService();
    }

    InitializeFilters() {
        // Records Types
        this.RecordsTypesFilterList = [];
        this.RecordsTypesFilterList.push(new CodeNameClass("S", "Salesman Records"));
        this.RecordsTypesFilterList.push(new CodeNameClass("C", "Created By Records"));
        this.RecordsTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_RecordsType);
        if (AppTool.IsNullOrEmpty(this.RecordsTypeFilterCode)) {
            this.RecordsTypeFilterCode = "S";
        }
        this.selectedRecordsTypeFilter = this.RecordsTypesFilterList.filter(d => d.Code == this.RecordsTypeFilterCode)[0];

        // CreatedBy Types
        this.CreatedByTypesFilterList = [];
        this.CreatedByTypesFilterList.push(new CodeNameClass("M", "Created By Me"));
        this.CreatedByTypesFilterList.push(new CodeNameClass("All", "Created By"));

        // Business Units
        this.myBusinessUnitListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BusinessUnitList[] = myResponse.Result;

                this.BusinessUnitFilterList = [];
                this.BusinessUnitFilterList.push(new CodeNameClass("M", "My Records"));

                if (list) {
                    list.filter(d => d.Id != SessionLocator.Tenant.toString()).forEach((item) => {
                        this.BusinessUnitFilterList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }

                this.BusinessUnitFilterList.push(new CodeNameClass("A", "All Records"));
            }

            this.OnFiltersInitialized();
        });
    }

    OnFiltersInitialized() {
        if (this.RecordsTypeFilterCode == "C") {
            this.CreatedByTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreatedByType);
            if (AppTool.IsNullOrEmpty(this.CreatedByTypeFilterCode)) {
                this.CreatedByTypeFilterCode = "M";
            }
            this.selectedCreatedByTypeFilter = this.CreatedByTypesFilterList.filter(d => d.Code == this.CreatedByTypeFilterCode)[0];

            if (this.CreatedByTypeFilterCode == "M") {
                this.OwnerId = SessionLocator.LoggedUserId;
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
            }

            else {
                this.OwnerId = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
            }

            this.LoadFilteredQueries();
        }

        else {
            this.BusinessUnitFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_BusinessUnit);
            if (AppTool.IsNullOrEmpty(this.BusinessUnitFilterCode)) {
                this.BusinessUnitFilterCode = "M";
            }
            this.selectedBusinessUnitFilter = this.BusinessUnitFilterList.filter(d => d.Code == this.BusinessUnitFilterCode)[0];

            switch (this.BusinessUnitFilterCode) {
                case "M": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
                    this.OwnerId = SessionLocator.LoggedUserId;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                    this.LoadFilteredQueries();
                    break;
                }

                case "A": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = null;
                    this.OwnerId = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                    this.LoadFilteredQueries();
                    break;
                }

                default: {
                    this.IsBusinessUnitUsers = true;
                    this.BusinessUnitId = this.BusinessUnitFilterCode;

                    var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                    this.BusinessUnitUsersFilterList.push(item);

                    var filters = new ApiQueryFilters();
                    filters.PageIndex = 0;
                    filters.PageSize = 100;
                    filters.Filter1Name = "BusinessUnitId";
                    filters.Filter1Value = this.BusinessUnitId;
                    filters.Filter1Operator = "Equals";

                    this.myUserListService.getAllFromCache(filters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var loadedUsers: UserList[] = myResponse.Result;

                            if (loadedUsers != null) {
                                loadedUsers.forEach((list) => {
                                    this.BusinessUnitUsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                });
                            }
                        }

                        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                            defaultFilterCode = null;
                        }

                        this.OwnerId = defaultFilterCode;
                        this.listOfValuesUserId = this.OwnerId;

                        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                            this.selectedUserFilter = this.BusinessUnitUsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                        }

                        if (this.selectedUserFilter == null) {
                            this.selectedUserFilter = this.BusinessUnitUsersFilterList[0];
                        }

                        this.LoadFilteredQueries();
                    });

                    break;
                }
            }
        }
    }

    private selectedRecordsTypeFilter: CodeNameClass;
    get SelectedRecordsTypeFilter() { return this.selectedRecordsTypeFilter; }
    set SelectedRecordsTypeFilter(value: CodeNameClass) {
        if (this.selectedRecordsTypeFilter != value) {
            this.selectedRecordsTypeFilter = value;
            this.RecordsTypeFilterCode = value == null ? "S" : value.Code;
            this.BusinessUnitFilterCode = "M";
            this.CreatedByTypeFilterCode = "M";
            this.OwnerId = SessionLocator.LoggedUserId;
            this.BusinessUnitId = this.RecordsTypeFilterCode == "S" ? SessionLocator.LoggedUserPM.BusinessUnitId : null;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_RecordsType, this.RecordsTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.OnFiltersInitialized();
        }
    }

    private selectedCreatedByTypeFilter: CodeNameClass;
    get SelectedCreatedByTypeFilter() { return this.selectedCreatedByTypeFilter; }
    set SelectedCreatedByTypeFilter(value: CodeNameClass) {
        if (this.selectedCreatedByTypeFilter != value) {
            this.selectedCreatedByTypeFilter = value;
            this.CreatedByTypeFilterCode = value == null ? "M" : value.Code;
            this.OwnerId = this.CreatedByTypeFilterCode == "M" ? SessionLocator.LoggedUserId : null;
            this.listOfValuesUserId = this.OwnerId;
            this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', this.CreatedByTypeFilterCode == "M" ? false : true);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    private selectedBusinessUnitFilter: CodeNameClass;
    get SelectedBusinessUnitFilter() { return this.selectedBusinessUnitFilter; }
    set SelectedBusinessUnitFilter(value: CodeNameClass) {
        if (this.selectedBusinessUnitFilter != value) {
            this.selectedBusinessUnitFilter = value;

            this.BusinessUnitFilterCode = value == null ? "M" : value.Code;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);

            switch (this.BusinessUnitFilterCode) {
                case "M": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
                    this.OwnerId = SessionLocator.LoggedUserId;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                    this.LoadFilteredQueries();
                    break;
                }

                case "A": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = null;
                    this.OwnerId = null;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                    this.LoadFilteredQueries();
                    break;
                }

                default: {
                    this.IsBusinessUnitUsers = true;
                    this.BusinessUnitId = this.BusinessUnitFilterCode;
                    this.OwnerId = null;
                    this.listOfValuesUserId = this.OwnerId;
                    this.BusinessUnitUsersFilterList = [];
                    var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                    this.BusinessUnitUsersFilterList.push(item);
                    var filters = new ApiQueryFilters();
                    filters.PageIndex = 0;
                    filters.PageSize = 100;
                    filters.Filter1Name = "BusinessUnitId";
                    filters.Filter1Value = this.BusinessUnitId;
                    filters.Filter1Operator = "Equals";

                    this.myUserListService.getAllFromCache(filters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var loadedUsers: UserList[] = myResponse.Result;

                            if (loadedUsers != null) {
                                loadedUsers.forEach((list) => {
                                    this.BusinessUnitUsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                });
                            }
                        }

                        this.selectedUserFilter = this.BusinessUnitUsersFilterList[0];
                        this.LoadFilteredQueries();
                    });

                    break;
                }
            }
        }
    }

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() { return this.selectedUserFilter; }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;
            var myCode: string = value == null ? "A" : value.Code;
            this.OwnerId = myCode == "A" ? null : myCode;
            this.listOfValuesUserId = this.OwnerId;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    private listOfValuesUserId: string;
    get ListOfValuesUserId() { return this.listOfValuesUserId; }
    set ListOfValuesUserId(value: string) {
        if (this.listOfValuesUserId != value) {
            this.OwnerId = value;
            this.listOfValuesUserId = value;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    LoadFilteredQueries() {
         this.ClearFilters();

        var myOwnerId: string = null;
        var myBusinessUnitId: string = null;
        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;

            if (myOwnerId == "all" || myOwnerId == "null") {
                myOwnerId = null;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;

            if (myBusinessUnitId == "all" || myBusinessUnitId == "null") {
                myBusinessUnitId = null;
            }
        }

        if (this.RecordsTypeFilterCode == "C") {

            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "CreatedByUserId");
                this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId" && a.FieldName != "DirectionId").forEach(item => {
                    item.IgnoreFilter = true;
                });
            }

            this.apiQueryFilters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }

        else {
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "SalesmanUserId" && a.FieldName != "BusinessUnitId");
                this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId" && a.FieldName != "DirectionId").forEach(item => {
                    item.IgnoreFilter = true;
                });
            }

            this.apiQueryFilters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
            this.apiQueryFilters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
        }

        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: false });
    }

    ClearFilters() {
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId" && a.FieldName != "DirectionId");
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: true });
        }
        //this.RemoveFilters("CreatedByUserId");
        //this.RemoveFilters("SalesmanUserId");
        //this.RemoveFilters("BusinessUnitId");
    }

    RemoveFilters(fieldName) {
        var item = this.apiQueryFilters.AdditionalFilters.filter(d => d.FieldName == fieldName)[0];
        if (item) {
            var index = this.apiQueryFilters.AdditionalFilters.indexOf(item);
            this.apiQueryFilters.AdditionalFilters.splice(index, 1);
        }
    }

}
