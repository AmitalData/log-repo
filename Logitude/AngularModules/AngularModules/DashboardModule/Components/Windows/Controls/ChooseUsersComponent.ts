import { Component, Output, EventEmitter, OnDestroy } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardSharedUserPM } from '../../../../DashboardModule/EntityPMs/DashboardSharedUserPM';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../Infrastructure/Tools';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
declare var window: any;

@Component({
    templateUrl: './ChooseUsersComponent.html',
})

export class ChooseUsersComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "DashboardSharedUser";
    public Columns: any[] = [];
    public Items: any[] = [];
    @Output() SearchFieldChangeEvent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    public _entityListService: EntityListService;
    public EntityPM: DashboardPM;
    SelectedRecords: any[] = [];
    AllRecords: any[] = [];
    constructor() {
        window.DashboardUsers = [];
        this._entityListService = new EntityListService();
        this.Listen();
    }

    SetWindowArgs(entityPM: DashboardPM) {
        this.EntityPM = entityPM;
        window.DashboardUsers = this.EntityPM.DashboardSharedUsers;
        this.BuildColumns();
    }

    private ListenEvent: any = null;
    Listen() {
        this.ListenEvent = this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res.Name == "ChooseUserCheckBoxComponent") {
                this.RefreshUsersList(res.select);
            }
        });
    }    

    RefreshUsersList(res: any) {
        if (res.IsCheck) {
            var item: DashboardSharedUserPM = this.EntityPM.DashboardSharedUsers.filter(d => d.UserId == res.UserId)[0];
            if (item == null) {
                var newUser = new DashboardSharedUserPM(null);
                newUser.Tenant = SessionLocator.Tenant;
                newUser.UserId = res.UserId;
                newUser.UserName = res.UserName;
                newUser.DashboardId = this.EntityPM.Id;
                this.EntityPM.AddDashboardSharedUser(newUser);
            }
        }
        else {
            var item: DashboardSharedUserPM = this.EntityPM.DashboardSharedUsers.filter(d => d.UserId == res.UserId)[0];
            if (item) {
                this.EntityPM.RemoveDashboardSharedUser(item);
            }
        }

        window.DashboardUsers = this.EntityPM.DashboardSharedUsers;
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null
    }

    BuildColumns() {
        this.Columns = [];

        this.Columns.push({
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            HtmlListComponentName: 'ChooseUserCheckBoxComponent',
            HtmlListComponentUrl: './DashboardModule/Components/Windows/Controls/ChooseUserCheckBoxComponent',
        });

        this.Columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            Display: 'Email',
            Styles: { width: '200px' },
        });

        this.Columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            Display: 'Name',
            Styles: { width: '200px' },
        });
    }

    DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    private filters: ApiQueryFilters;
    private GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.GetCount = true;
        filters.PageIndex = skip;
        filters.PageSize = 100;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator.Tenant;
        filters.GetCount = getCount;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.Filter1Name = "SearchFields";
            filters.Filter1Value = this.SearchText;
            filters.Filter1Operator = "Contains";
        }

        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        this.filters = filters;
        return new Promise((resolve, reject) => { resolve(this._entityListService.getByFilters("User", filters)) });
    }

    private searchText: string;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    onSearchTextChangeEvent(searchtext) {
        this.SearchText = searchtext;
        this.SearchFieldChangeEvent.emit(this.SearchText);
    }

    OnSearchTextChangeEvent(text: string) {
        this.SearchText = text;
        //this.BrowseClicked();
    }
    BrowseClicked() {
        this.DataSource = {
            pageSize: 50,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };

        this.onQueryChangeEvent.emit({ Filters: this.filters, Reload: true });
    }

    private removedItems: any[] = [];
    get RemovedItems() { return this.removedItems; }
    set RemovedItems(value: any[]) {
        if (this.removedItems != value) {
            this.removedItems = value;
        }
    }

    OnSetRemoved(event) {
        if (!AppTool.IsNullOrEmpty(event)) {
            this.RemovedItems = event;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        this.SelectedRecords.forEach(item => {
            var existContact: DashboardSharedUserPM = this.EntityPM.DashboardSharedUsers.filter(d => d.UserId == item.rowData.Id)[0];

            if (existContact == null) {
                var newUser = new DashboardSharedUserPM(this.EntityPM);
                newUser.Tenant = SessionLocator.Tenant;
                newUser.UserId = item.rowData.Id;
                newUser.UserName = item.rowData.EnglishName;
                newUser.DashboardId = this.EntityPM.Id;
                this.EntityPM.AddDashboardSharedUser(newUser);
            }
        });

        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }
}
