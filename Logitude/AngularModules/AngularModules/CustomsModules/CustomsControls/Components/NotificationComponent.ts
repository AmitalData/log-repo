declare var window: any;
import { Component, Output, EventEmitter, OnInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
//import {NotificationListService} from  '../../../Services/StandardLists/NotificationListService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';

import {DueDate} from '../../../Customs/DataContract/DueDate';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { NotificationExtendedListService } from '../../../Customs/Services/ExtendedLists/NotificationExtendedListService';
import {NotificationFiltersDataCount} from '../../../Customs/DataContract/NotificationFiltersDataCount';
import {DeclarationPM} from '../../../Customs/EntityPMs/DeclarationPM';
import { NotificationWebService } from '../../../Customs/Services/WebServices/NotificationWebService';
import {UserPM} from '../../../Common/EntityPMs/UserPM';
import { CustomsCollateralPMService } from '../../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import {SelectedNotifications} from '../../../Customs/DataContract/SelectedNotifications';
import { NotificationPM } from '../../../Customs/EntityPMs/NotificationPM';
import { DeclarationEditComponentController } from '../../../Customs/Controller/DeclarationEditComponentController';
import { EntityPMService } from '../../../Infrastructure/Services/EntityPMService';

@Component({
    selector: 'NotificationComponent',    
    templateUrl: './NotificationComponent.html',
})

export class NotificationComponent extends BaseComponent implements OnInit {
  public LayoutDirection: any;
  public IsDisplayOnly: any;
  public SelectedRow: any;

    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() CustomBackFromEditevent = new EventEmitter();
    @Output() ShowHLineOverRow = new EventEmitter();
    declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    notificationExtendedListService: NotificationExtendedListService = new NotificationExtendedListService();
    entityListService: EntityListService = new EntityListService();
    notificationWebService: NotificationWebService = new NotificationWebService();
    customsCollateralPMService: CustomsCollateralPMService = new CustomsCollateralPMService();
    _EntityPMService: EntityPMService = new EntityPMService();
    public selectedItems: ObservableCollection;
    public connectedItems: ObservableCollection;
    ToolTipHeight: number;
    selectedNotifications: SelectedNotifications = new SelectedNotifications();
    ObjectTableName: string;
    DataContext: any = this;
    filterAgrs: ApiQueryFilters;
    IsVisible: boolean = false;
    DueDatesList: DueDate[];
    selectedDate: DueDate;
    preventSelect: boolean;
    public CurrentEditComponentId: string;
    public DeclarationPM: DeclarationPM;
    IsDeclarationTab: boolean;
    public ExcludedItems: ObservableCollection;
    private currentSession=SessionLocator.SelectedSession;
    constructor(entityArgs: EntityArgs, private EntityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        super();

        //var t = setInterval(() => {this.timerValue++;}, 1000); // TESTING!! timer for testing detect changes
        SessionLocator.SelectedSession.SubscriptionAdd(
            SessionLocator.SelectedSession.SessionEvent.subscribe(($event: any) => {
                if ($event.Name == "ClosedByAssigneeClicked") {
                    this.ShowHLineOverRow.emit($event.rowIndex);
                    this.preventSelect = true;

                }
            })
        );
        this.ExcludedItems = new ObservableCollection([]);
        this.selectedItems = new ObservableCollection([]);
        this.connectedItems = new ObservableCollection([]);
        SessionLocator.SelectedSession.SubscriptionAdd(
            SessionLocator.SelectedSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == "select") {
                    this.preventSelect = true;
                }
            })
        );
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.DeclarationPM = entityArgs.EntityPM;
        if (this.ObjectTableName == null) {
            this.ObjectTableName = "Customs.Notification";
            this.OpenClosedFilterSelectedValue = "open";
            this.AssigneToId = SessionLocator.LoggedUserId;
        }

        if (this.ObjectTableName == "Customs.Declaration") {
            this.IsDeclarationTab = true;
            this.OpenClosedFilterSelectedValue = "all"; 
        }



    }



    public columns: any[] = null;

    LoadNotifications() {
        this.selectedItems.Collection = [];
        this.ExcludedItems.Collection = [];

        if ((this.status == "none" || this.status == null) && !this.IsSelected) {
            this.SelectedItemsCountText = null;
            this.SelectedItemsCount = 0;
            this.IsSelectedTextVisible = false;
        }

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }
    ngOnInit() {
        this.EntityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe((response:any) => {

            this.IsVisible = true;
            this.BuildColumns();
            this.DueDatesList = [];
            this.DueDatesList.push(new DueDate("0", TextCodeTranslator.Translate("Customs.Notification.O.All")));
            this.DueDatesList.push(new DueDate("1", TextCodeTranslator.Translate("Customs.Notification.O.UntilToday")));
            this.DueDatesList.push(new DueDate("2", TextCodeTranslator.Translate("Customs.Notification.O.Next3Days")));
            this.DueDatesList.push(new DueDate("3", TextCodeTranslator.Translate("Customs.Notification.O.NextWeek")));
            this.DueDatesList.push(new DueDate("4", TextCodeTranslator.Translate("Customs.Notification.O.NextMonth")));
            this.selectedDate = this.DueDatesList[0];

            var screenWidth = this.getScreenWidth();
            var screenHeight = this.getScreenHeight();

            if (screenWidth == 1024 ) {
                this.Right = 190;
            }
            else {
                this.Right = 520;
            }
        });
    }
    Right: number = 0;
    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }

    AssigneeFilterSelectedValue: string = 'Assignee';
    AssigneeFilterItemClicked(value: string) {
        if (this.EnableFilters) {
            if (this.AssigneeFilterSelectedValue != value) {
                this.AssigneeFilterSelectedValue = value;

                this.LoadNotifications();
            }
        }
    }

    NotificationTypeFilterSelectedValue: string = 'All';
    NotificationTypeilterItemClicked(value: string) {
        if (this.EnableFilters) {
            if (this.NotificationTypeFilterSelectedValue != value) {
                this.NotificationTypeFilterSelectedValue = value;
                this.LoadNotifications();

            }
        }
    }

    OpenClosedFilterSelectedValue = null;

    OpenClosedFilterItemClicked(value: string) {
        if (this.OpenClosedFilterSelectedValue != value) {
            this.IsSelected = false;
            this.OpenClosedFilterSelectedValue = value;
            this.status = null;
            this.LoadNotifications();

        }

    }

    assignee: UserPM;
    get Assignee() { return this.assignee; }
    set Assignee(value: UserPM) {
        if (this.assignee != value) {
            this.assignee = value;
            this.LoadNotifications();
        }
    }
    //OnRowSelected($event) {
    //    //this.ShowHLineOverRow.emit($event.rowIndex);
    //}
    selectedDateId: any;
    DueDateSelectionChanged(date: DueDate) {
        if (date) {
            this.selectedDateId = date.Id;
        }
            this.LoadNotifications();
       
    }
    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: "",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            IsCheckBox: true

        });


        this.columns.push({
            FieldName: 'AssigneToNotificationTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Notification.F.AssigneToNotificationTypeCode'),
            Styles: { width: '70px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'AssigneToNotificationTypeCode'
        });

        this.columns.push({
            FieldName: 'NotificationDefinitionName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.NotificationDefinitionName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',

            ServerSideSortable: true,
            SortByName: 'NotificationDefinitionName'
        });

        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.DueDate"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'DueDate'
        });

        this.columns.push({
            FieldName: 'Reference1Number',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.Reference1Number"),
            Styles: { width: '120px' },

            IsCustomTemplate: true,
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'Reference1Number'
        });


        this.columns.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.CustomerName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'CustomerName'
        });



        this.columns.push({
            FieldName: "CreateDate",
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.CreateDate"),


            IsCustomTemplate: true,
            Styles: { width: '120px' },
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'CreateDate'

        });
        this.columns.push({
            FieldName: "AssigneToName",
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.AssigneToName"),


            IsCustomTemplate: true,
            Styles: { width: '100px' },
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            ServerSideSortable: true,
            SortByName: 'AssigneToName'

        });

        this.columns.push({
            FieldName: 'IsClosedByAssignee',
            DataTypeCode: 'boolean',
            Display: TextCodeTranslator.Translate("Customs.Notification.F.IsClosedByAssignee"),
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            IsCustomTemplate: true,
            Styles: { width: '100px' },
            EnableHoverVisibility: true,
            ServerSideSortable: true,
            SortByName: 'IsClosedByAssignee'

        });

    }

    DataSource = {

        pageSize: 17,
        rowCount: null,


        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;


        },

    };
    IsClosed: boolean;
    AssigneToNotificationTypeCode: string;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {


        this.EnableFilters = false;
        if (filters == null) {
            filters = new ApiQueryFilters();
        }
      //   SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        if (this.ObjectTableName == "Customs.Declaration") {

            filters.SortBy = "CreateDate";
            filters.SortDirection = "Descending";
        }

        if (this.status) {
            
            switch (this.status) {
                case 'all':
                    {
                        break;
                    }
                case 'read':
                    {
                      
                        filters.addAdditionalFilter("IsSeenByAssignee", true, null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case 'unread':
                    {
                       
                        filters.addAdditionalFilter("IsSeenByAssignee", false, null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case 'none':
                    {
                        this.IsSelected = false;
                        this.status = null;
                        break;
                    }

            }
        }

        switch (this.NotificationTypeFilterSelectedValue) {
            case 'All':
                {

                    break;
                }
            case 'Action':
                {
                    this.AssigneToNotificationTypeCode = "A";
                    filters.addAdditionalFilter("AssigneToNotificationTypeCode", "A", null, null, "Equals", false, false, false, "string");

                    break;
                }
            case 'Info':
                {
                    this.AssigneToNotificationTypeCode = "I";
                    filters.addAdditionalFilter("AssigneToNotificationTypeCode", "I", null, null, "Equals", false, false, false, "string");

                    break;
                }

        }

        switch (this.OpenClosedFilterSelectedValue) {
            case 'all':
                {

                    break;
                }
            case 'open':
                {
                    this.IsClosed = false;
                    filters.addAdditionalFilter("IsClosedByAssignee", false, null, null, "Equals", false, false, false, "boolean");

                    break;
                }
            case 'closed':
                {
                    this.IsClosed = true;
                    filters.addAdditionalFilter("IsClosedByAssignee", true, null, null, "Equals", false, false, false, "boolean");

                    break;
                }

        }

      
        switch (this.selectedDateId) {
            case '0':
                {
                    break;
                }
            case '1':
                {
                    var TodayDate = new Date();
                    TodayDate.setHours(0, 0, 0, 0);


                    filters.addAdditionalFilter("DueDate", TodayDate, null, null, "LessThan", false, false, false, "DateTime");
                    break;
                }
            case '2':
                {
                    filters.addAdditionalFilter("DueDate", DateTool.GetCurrentDateAsUtc(), DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 3), null, "Between", false, false, false, "DateTime");
                    break;
                }
            case '3':
                {
                    filters.addAdditionalFilter("DueDate", DateTool.GetCurrentDateAsUtc(), DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 7), null, "Between", false, false, false, "DateTime");
                    break;
                }
            case '4':
                {
                    filters.addAdditionalFilter("DueDate", DateTool.GetCurrentDateAsUtc(), DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 30), null, "Between", false, false, false, "DateTime");
                    break;
                }
            default: {
                break;
            }


        }

        switch (this.AssigneeFilterSelectedValue) {
            case 'Assignee':
                {
                    if (!AppTool.IsNullOrEmpty(this.AssigneToId)) {
                        filters.addAdditionalFilter("AssigneToId", this.AssigneToId, null, null, "Equals", false, false, false, "string");


                    }

                    break;
                }
            case 'Department':
                {
                    if (!AppTool.IsNullOrEmpty(this.DepartmentId)) {
                        filters.addAdditionalFilter("DepartmentId", this.DepartmentId, null, null, "Equals", false, false, false, "string");


                    }

                    break;
                }

            case 'Office':
                {
                    this.IsHandledByCustomOffice = true;
                    filters.addAdditionalFilter("IsHandledByCustomOffice", true, null, null, "Equals", false, false, false, "string");




                    break;
                }


        }

        if (!AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
            filters.addAdditionalFilter("DeclarationOfficeCode", this.DeclarationOfficeCode, null, null, "Equals", false, false, false, "string");

        }

        if (!AppTool.IsNullOrEmpty(this.searchValue)) {

            filters.addAdditionalFilter("SearchFields", this.searchValue, null, null, "Contains", false, false, false, "string");

        }

        if (this.ObjectTableName == "Customs.Declaration") {
            filters.addAdditionalFilter("Declaration", this.DeclarationPM.Id, this.DeclarationPM.CustomFileNo, null, "Equals", true, false, true, "string");


        }

       
            this.notificationExtendedListService.getCountByFilters(filters).subscribe((res: any) => {
                this.counts = res.Result.Result;
                this.allreadCount = this.counts.AllReadCount;
                this.allUnreadCount = this.counts.AllUnreadCount;
                if (this.allreadCount==0) {
                    this.IsUnReadButtonVisible = false;
                }
                if (this.allUnreadCount == 0) {
                    this.IsReadButtonVisible = false;
                }
                if (this.OpenClosedFilterSelectedValue != "closed") {
               
                this.openCount = "(" + this.counts.OpenCount.toString() + ")";
                this.allCount = "(" + this.counts.AllCount.toString() + ")";
                this.infoCount = "(" + this.counts.InfoCount.toString() + ")";
                this.actionCount = "(" + this.counts.ActionCount.toString() + ")";
                }
                else {
                    this.openCount = "(" + this.counts.OpenCount.toString() + ")";
                    this.allCount = null;
                    this.infoCount = null;
                    this.actionCount = null;
                }
                if (this.status == "read" || this.status == "unread" || this.status == "all") {
                    if (this.OpenClosedFilterSelectedValue != "closed") {
                        this.dataCount = this.counts.OpenCount;
                    }
                    else {
                        this.dataCount = this.counts.ClosedCount;
                    }
                        if (this.dataCount) {

                            if (this.IsSelected) {
                                this.IsSelectedTextVisible = true;
                                this.SelectedItemsCount = this.dataCount;
                                this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
                            }
                        }
                        else {
                            this.IsReadButtonVisible = false;
                            this.IsUnReadButtonVisible = false;
                            this.IsReopenButtonVisible = false;
                            this.IsCloseButtonVisible = false;
                            this.IsSelectedTextVisible = false;
                        }
                    }
                 //  this.IsSelected = true;

                //}
                //else {
                //    this.IsReadButtonVisible = false;
                //    this.IsUnReadButtonVisible = false;
                //    this.IsReopenButtonVisible = false;
                //    this.IsCloseButtonVisible = false;
                //    this.IsSelectedTextVisible = false;

                //}
               // this.status = null;

            //    SessionLocator.SelectedSession.StopBusyIndicator();
            });

        if (AppTool.IsNullOrEmpty(this.dataCount))
        {
            this.EnableFilters = true;
        }
        return this.entityListService.getExtendedByFilters("Customs.Notification", filters);


    }
    IsHandledByCustomOffice: boolean;
    openCount: string;
    allCount: string;
    infoCount: string;
    actionCount: string;
    counts: NotificationFiltersDataCount;
    filters: ApiQueryFilters;
    allreadCount: number;
    allUnreadCount: number;
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;

        if (this.isSelected) {

            if (this.status == null || this.OpenClosedFilterSelectedValue == "closed") {
                this.dataCount = this.DataSource.rowCount;
            }
            if (this.dataCount > 0) {
                this.IsSelectedTextVisible = true;
            }
            else {
                this.IsSelectedTextVisible = false;
            }
            this.SelectedItemsCount = this.dataCount;
            if (this.dataCount) {
                this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
            }
            if (this.dataCount > 0) {
                if (this.status == "all" || this.status == null) {
                    if (this.allreadCount == 0) {
                        this.IsUnReadButtonVisible = false;
                    }
                    else {
                        this.IsUnReadButtonVisible = true;
                    }
                    if (this.allUnreadCount == 0) {
                        this.IsReadButtonVisible = false;
                    }
                    else {
                        this.IsReadButtonVisible = true;
                    }
                    
                }
                else if (this.status == "read") this.IsUnReadButtonVisible = true;
                else if (this.status == "unread") this.IsReadButtonVisible = true;

                if (this.OpenClosedFilterSelectedValue == "open") {
                    this.IsCloseButtonVisible = true;
                    this.IsReopenButtonVisible = false;
                }
                else if (this.OpenClosedFilterSelectedValue === "closed") {
                    this.IsCloseButtonVisible = false;
                    this.IsReopenButtonVisible = true;
                }
                else {
                    this.IsCloseButtonVisible = true;
                    this.IsReopenButtonVisible = true;
                }
            }
        }
        else {
            this.SelectedItemsCount = 0;
            this.selectedItems.Clear();
            this.selectedIds = [];
            this.IsSelectedTextVisible = false;
            this.IsReadButtonVisible = false;
            this.IsUnReadButtonVisible = false;
            this.IsCloseButtonVisible = false;
            this.IsReopenButtonVisible = false;
        }


    }
    selectedItemsCount: number;
    get SelectedItemsCount() { return this.selectedItemsCount; }
    set SelectedItemsCount(value: any) {
        this.selectedItemsCount = value;
        if (value == 0) {
            this.IsSelectedTextVisible = false;
        }
    }
    SelectedItemsFullObject: any[];
    dataCount: number;

    onCheckBoxChecked($event) {
        if ($event.IsChecked) {

            if (!this.selectedItems.Collection.includes($event)) {
                this.selectedItems.Insert($event);
                this.IsSelectedTextVisible = true;
                this.SelectedItemsCount += 1;
                this.dataCount = this.DataSource.rowCount;
                if (this.dataCount != null) {
                    this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

                }
               
                if ($event.rowData.IsSeenByAssignee) {
                    //this.IsReadButtonVisible = false;
                    this.IsUnReadButtonVisible = true;
                }
                else {
                    this.IsReadButtonVisible = true;
                   // this.IsUnReadButtonVisible = false;
                }
                if (this.OpenClosedFilterSelectedValue == "open") {
                    this.IsCloseButtonVisible = true;
                    this.IsReopenButtonVisible = false;
                }
                else if (this.OpenClosedFilterSelectedValue === "closed") {
                    this.IsCloseButtonVisible = false;
                    this.IsReopenButtonVisible = true;
                }

                else {
                    this.IsCloseButtonVisible = true;
                    this.IsReopenButtonVisible = true;
                }
                }
               

                
            

            if (this.IsSelected) {
                if (this.ExcludedItems.Collection.includes($event.rowData.Id)) {
                    this.ExcludedItems.Remove($event.rowData.Id);
                }
            }
        }
        else {
            var removedIndex = null;
            for (var i = 0; i < this.selectedItems.Collection.length; i++) {
                if ($event.rowIndex == this.selectedItems.Collection[i].rowIndex) {
                    removedIndex = i;
                    break;
                }
            }

            //var item: any = this.selectedItems.Collection.filter(d => d.rowIndex == $event.rowIndex);
            //if (this.selectedItems.Collection.includes($event)) {
            if (removedIndex!=null) {
                
                this.selectedItems.RemoveFromIndex(removedIndex);
            }

            if (this.selectedItems.Length > 0) {
                var read: any = this.selectedItems.Collection.filter(d => d.rowData.IsSeenByAssignee)[0];
                var unread: any = this.selectedItems.Collection.filter(d => !d.rowData.IsSeenByAssignee)[0];
                if (!read) {
                    this.IsUnReadButtonVisible = false;
                }
                if (!unread) {
                    this.IsReadButtonVisible = false;
                }
            }
            this.SelectedItemsCount -= 1;
           
            if (this.dataCount != null) {
                this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

            }

            if ( this.SelectedItemsCount == 0) {
                this.IsReadButtonVisible = false;
                this.IsUnReadButtonVisible = false;
                this.IsCloseButtonVisible = false;
                this.IsReopenButtonVisible = false;
                this.IsSelectedTextVisible = false;
                this.IsSelected = false;
            }



            if (this.IsSelected) {
                if (!this.ExcludedItems.Collection.includes($event.rowData.Id)) {
                    this.ExcludedItems.Insert($event.rowData.Id);
                }
            }
        }
    }

    CheckBoxFilterChanged: EventEmitter<any> = new EventEmitter(); 
    status: string = null;
    OnAllBtnClicked() {
        this.status = "all";
        this.IsSelected = true;

        this.IsReadButtonVisible = true;
        this.IsUnReadButtonVisible = true;
        if (this.OpenClosedFilterSelectedValue == "open") {
            this.IsCloseButtonVisible = true;
            this.IsReopenButtonVisible = false;
        }
        else if (this.OpenClosedFilterSelectedValue === "closed") {
            this.IsCloseButtonVisible = false;
            this.IsReopenButtonVisible = true;
        }
      
      
        this.LoadNotifications();
    }
    OnReadBtnClicked() {
        this.status = "read";
        this.IsSelected = true;
        this.IsReadButtonVisible = false;
        this.IsUnReadButtonVisible = true;
        if (this.OpenClosedFilterSelectedValue == "open") {
            this.IsCloseButtonVisible = true;
            this.IsReopenButtonVisible = false;
        }
        else if (this.OpenClosedFilterSelectedValue === "closed") {
            this.IsCloseButtonVisible = false;
            this.IsReopenButtonVisible = true;
        }
     
        this.LoadNotifications();
    }

    OnUnReadBtnClicked() {
        this.status = "unread";
        this.IsSelected = true;

        this.IsReadButtonVisible = true;
        this.IsUnReadButtonVisible = false;
        if (this.OpenClosedFilterSelectedValue == "open") {
            this.IsCloseButtonVisible = true;
            this.IsReopenButtonVisible = false;
        }
        else if (this.OpenClosedFilterSelectedValue === "closed") {
            this.IsCloseButtonVisible = false;
            this.IsReopenButtonVisible = true;
        }
     
        this.LoadNotifications();
    }

    OnNoneBtnClicked() {

        this.IsReadButtonVisible = false;
        this.IsUnReadButtonVisible = false;
        this.IsCloseButtonVisible = false;
        this.IsReopenButtonVisible = false;
        this.status = "none";
        this.LoadNotifications();
       
    }


    IsReadButtonVisible: boolean;
    IsUnReadButtonVisible: boolean;
    IsCloseButtonVisible: boolean;
    IsReopenButtonVisible: boolean;
    IsSelectedTextVisible: boolean = false;
    SelectedItemsCountText: string = null;
    private departmentId: string = null;
    public get DepartmentId() { return this.departmentId; }
    public set DepartmentId(newValue: string) {
        this.departmentId = newValue;
        this.LoadNotifications();
    }


    private assigneToId: string;//= SessionLocator.LoggedUserId;
    public get AssigneToId() { return this.assigneToId; }
    public set AssigneToId(newValue: string) {
        this.assigneToId = newValue;

    }

    private declarationOfficeCode: string = null;
    public get DeclarationOfficeCode() { return this.declarationOfficeCode; }
    public set DeclarationOfficeCode(newValue: string) {
        this.declarationOfficeCode = newValue;
        this.LoadNotifications();
    }


    RefreshEntity() {
        this.selectedItems = new ObservableCollection([]);
        this.LoadNotifications();
    }
    ViewInitCompleted($event) {

        this.LoadNotifications();
    }

    searchValue: string;
    Search(value: string) {
        var tkn = setTimeout(() => {
            this.searchValue = value;
            this.LoadNotifications();
        }, 200);

    }

    OnRowSelected(event) {
        if (!this.preventSelect) {
            //this.ShowHLineOverRow.emit($event.rowIndex);
            var selected = event.rowData;


            ////**test

            //var Ids: string[] = [];
            //Ids.push(selected.Id);
            //Ids.push(selected.Id);
            //this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {

            //});


            //return; 
            ////**tset



            //this.selected = selected;
            if (selected) {

                var customEditIdentityKey = Guid.newGuid();
                var control = null;

                var logitudeWindow = new LogitudeWindow();
                //logitudeWindow.ZIndex = 5;
                var currentScreenCode = "";
 
                switch (selected.ObjectTableName) {
                    case "Customs.Declaration":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {
                                        currentScreenCode = "DCPO";
                                        break;

                                    }

                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        currentScreenCode = "DCPC";
                                        break;

                                    }

                                case "2470N":
                                case "2470C":
                                case "2470P":
                                case "5018N":
                                case "8400C":
                                case "5117N":
                                case "8400A":
                                case "5101C":
                                case "5101G":
                                //case "5101I":
                                case "5101S":
                                case "5101T":
                                case "5101U":
                                case "5101B":
                                case "5101P":
                                case "5107N":
                                case "2754N":
                                case "70N":
                                case "70C":
                                case "60A":
                                case "5117C":
                                case "5117W":
                                case "5117D":
                                case "5117A":
                                case "5117P":
                                    {
                                       // var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);
                                        var tab;
                                        var tabs = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId);
                                        if (tabs.lenght > 0) {
                                            tabs = tabs.sort((n1, n2) => {
                                                if (n1.IndexOrder > n2.IndexOrder) {
                                                    return 1;
                                                }

                                                if (n1.IndexOrder < n2.IndexOrder) {
                                                    return -1;
                                                }

                                                return 0;
                                            });

                                            tab = tabs[0];
                                        }
                                        else {
                                            tab = tabs;
                                        }

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }

                                case "5101N":
                                case "5101R":
                                case "5101A":
                                case "5101E":
                                case "5101D":
                                    {
                                        currentScreenCode = "DCNT";
                                        break;
                                    }

                                case "8215A":
                                case "8215D":
                                case "8215C":
                                    {
                                        currentScreenCode = "DCCA";
                                        break;
                                    }

                                case "8227N":
                                case "8227D":
                                case "8227A":
                                case "8228D":
                                case "8228A":
                                    {
                                        currentScreenCode = "DCCD";
                                        break;
                                    }
                                case "1812N":
                                case "1812U":
                                case "2020N":
                                case "2000N":
                                case "2753A":
                                case "5110N":
                                case "5108N":
                                    {
                                        currentScreenCode = "DCTP";
                                        break;

                                    }
                                case "8374A":
                                case "8374J":
                                case "8374C":
                                case "8374D":
                                    {
                                        //currentScreenCode = "DCCS";
                                        this.ShowCustomsDeclarationCargoSplit(selected.Reference2Number);
                                        break;

                                    }
                                case "5101F":
                                    {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                                            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(response => {
                                                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(response => {
                                                    this.customsCollateralPMService.get(selected.Reference2Number).subscribe((response: any) => {

                                                        var result = response.Result;
                                                        console.log("[response] customsCollateralPMService.get", result);
                                                        if (!AppTool.IsNullOrEmpty(result)) {
                                                            control = './CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
                                                            logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                            logitudeWindow.WindowArgs = { CurrentEntity: result };
                                                            logitudeWindow.Height = 730;
                                                            logitudeWindow.Width = 660;
                                                            //logitudeWindow.ZIndex = 5;
                                                            this.cd.detach()
                                                            logitudeWindow.Show(control);
                                                            logitudeWindow.WindowClosed.subscribe(() => {
                                                                this.cd.reattach();
                                                                this.RefreshEntity();
                                                            });

                                                            var Ids: string[] = [];
                                                            Ids.push(selected.Id);
                                                            Ids.push(selected.Id);
                                                            this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                                                                this.SetStatusCompleted(selected.Id, event);

                                                            });
                                                        } else {
                                                            console.log("No collateral found!!!!!!");
                                                            return;
                                                        }
                                                    });
                                                });
                                            });
                                        });
                                        break;
                                    }

                                default:
                                    {

                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }


                            }
                            break;
                        }

                    case "Customs.PaymentOrder":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {
                                        var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;

                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }


                            }
                            break;

                        }

                    //case "Customs.ProceduralFault":
                    //    {
                    //        switch (selected.NotificationDefinitionCode) {
                    //            case "8218N":
                    //            case "8218U":
                    //            case "8219C":

                    //                {
                    //                    control = container.Resolve(typeof (UserControl),
                    //                        "Logitude.Customs.Views.ProceduralFaultsControl", new ParameterOverride("", 1)) as UserControl;
                    //                    window.Title = TextCodeTranslator.Translate("Customs.ProceduralFault.Q.ProceduralFaults");

                    //                    window.FlowDirection = FlowDirection.RightToLeft;
                    //                    break;

                    //                }

                    //            default:
                    //                {
                    //                    var msg = new MessageWindow();
                    //                    msg.Show("לא נמצאה ישות להצגה");
                    //                    break;
                    //                }


                    //        }
                    //        break;

                    //    }

                    case "Customs.PhysicalCheck":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }

                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;

                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }

                            }
                            break;

                        }

                    case "Customs.CustomsCollateral":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "8213N":
                                case "8211N":
                                case "8211U":
                                case "5101N":
                                case "5101R":
                                case "5101D":
                                case "5101A":
                                case "5101E":
                                case "5101F":
                                    {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
                                            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response:any) => {
                                                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe((response:any) => {
                                                    this.customsCollateralPMService.get(selected.EntityId).subscribe((response: any) => {

                                                        var result = response.Result;
                                                        console.log("[response] customsCollateralPMService.get", result);
                                                        if (!AppTool.IsNullOrEmpty(result)) {
                                                          control = './CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
                                                            logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                            logitudeWindow.WindowArgs = { CurrentEntity: result };
                                                            logitudeWindow.Height = 730;
                                                            logitudeWindow.Width = 660;
                                                            //logitudeWindow.ZIndex = 5;
                                                            this.cd.detach()
                                                            logitudeWindow.Show(control);
                                                            logitudeWindow.WindowClosed.subscribe(() => {
                                                                this.cd.reattach();
                                                                this.RefreshEntity();
                                                            });

                                                            var Ids: string[] = [];
                                                            Ids.push(selected.Id);
                                                            Ids.push(selected.Id);
                                                            this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                                                                this.SetStatusCompleted(selected.Id, event);

                                                            });
                                                        } else {
                                                            console.log("No collateral found!!!!!!");
                                                            return;
                                                        }



                                                    });
                                                });
                                            });
                                        });
                                        break;
                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }

                            }
                            break;

                        }

                    //case "Customs.Claim":
                    //    {
                    //        switch (selected.NotificationDefinitionCode) {
                    //            case "2300N":
                    //            case "5115N":
                    //                {
                    //                    ObjectTableTabPM tab = TenantContext.Current.ObjectTableTabs.Where(d => d.ObjectTableName == selected.ObjectTableName && d.IndexOrder == 0).FirstOrDefault();
                    //                    if (tab != null) {
                    //                        currentScreenCode = tab.Code;
                    //                    }
                    //                    else {
                    //                        SimplogMessageWindow msgWindow = new SimplogMessageWindow();
                    //                        msgWindow.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                    //                    }
                    //                    break;
                    //                }

                    //            default:
                    //                {
                    //                    SimplogMessageWindow msgWindow = new SimplogMessageWindow();
                    //                    msgWindow.Show("לא נמצאה ישות להצגה");
                    //                    break;
                    //                }
                    //        }
                    //        break;

                    //    }

                    case "Customs.DeclarationCargoSplit":
                        {
                            this.ShowCustomsDeclarationCargoSplit(selected.EntityId);
                        }

                    default:
                        {
                            if (!AppTool.IsNullOrEmpty(selected.Reference1Number)) {
                                this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(selected.Reference1Number).subscribe((res: any) => {
                                    var declaration = res.Result;
                                    console.log("[reponse] GetSingleDeclarationByCustomFileNo: ", declaration);
                                    if (!AppTool.IsNullOrEmpty(declaration)) {
                                        selected.ObjectTableName = "Customs.Declaration";
                                        currentScreenCode = "DEGC";
                                        this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);
                                        //selectedIds.Clear();
                                        //selectedIds.Add(selected.Id);
                                        //status = "Read";
                                        //statusOp = Context.SetNotificationsStatus(selectedIds, "Read", TenantContext.Current.Id);
                                        //statusOp.Completed += statusOp_Completed;

                                        var Ids: string[] = [];
                                        Ids.push(selected.Id);
                                        Ids.push(selected.Id);
                                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                                            this.SetStatusCompleted(selected.Id, event);
                                        });

                                    }
                                });
                            }
                            else {

                                var msg = new MessageWindow();
                                //msg.ZIndex = 5;
                                msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            }
                            break;
                        }

                }

                if (!AppTool.IsNullOrEmpty(control)) {
                    if (selected.ObjectTableName == "Customs.ProceduralFault") {
                        logitudeWindow.Height = 400;
                        logitudeWindow.Width = 820;
                        logitudeWindow.ShowCloseButton = true;
                    }
                    else {
                        logitudeWindow.Height = 730;
                        logitudeWindow.Width = 660;
                    }
                    //logitudeWindow.ZIndex = 5;
                    //logitudeWindow.Add(control);
                    //logitudeWindow.ShowSaveAsButton = true;
                    logitudeWindow.Show(control);

                    //SessionLocator.SelectedSession.StartBusyIndicatorLoading();

                    //EntityIdForCustomEditControlEvent entityIdEvent = eventAggregator.GetEvent<EntityIdForCustomEditControlEvent>();
                    //entityIdEvent.Publish(new EntityIdForCustomEditControlEventArgs() { EntityId = selected.EntityId, ObjectTableName = selected.ObjectTableName, IdentityKey = customEditIdentityKey });
                    //selectedIds.Clear();
                    //selectedIds.Add(selected.Id);
                    //status = "Read";
                    //statusOp = Context.SetNotificationsStatus(selectedIds, "Read", TenantContext.Current.Id);
                    //statusOp.Completed += statusOp_Completed;
                    var Ids: string[] = [];
                    Ids.push(selected.Id);
                    Ids.push(selected.Id);
                    this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                        this.SetStatusCompleted(selected.Id, event);

                    });

                }
                else {
                    if (!AppTool.IsNullOrEmpty(currentScreenCode)) {

                        
                        if (selected.ObjectTableName == "Customs.Declaration") {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run({
                                        SelectedTabCode: currentScreenCode,
                                        EntityId: selected.EntityId,
                                        ObjectTableName: selected.ObjectTableName
                                    });
                                    if (selected.NotificationDefinitionCode == "2753A") {
                                        cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                            .subscribe(myResult => {
                                                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
                                                myDeclarationEditComponentController.TapagId = selected.Reference2Number;
                                                console.log("myDeclarationEditComponentController.TapagId = " + selected.Reference2Number);
                                            });
                                    }
                                    if (selected.NotificationDefinitionCode.substring(0,4) == "8374") {
                                        cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                            .subscribe(myResult => {
                                                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
                                                myDeclarationEditComponentController.CargoSplitId = selected.Reference2Number;
                                                console.log("myDeclarationEditComponentController.CargoSplitId = " + selected.Reference2Number);
                                            });
                                    }
                                });


                            this.preventSelect = false;
                            return;
                        }

                        this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);
                        //selectedIds.Clear();
                        //selectedIds.Add(selected.Id);
                        //status = "Read";
                        //statusOp = Context.SetNotificationsStatus(selectedIds, "Read", TenantContext.Current.Id);
                        //statusOp.Completed += statusOp_Completed;
                        var Ids: string[] = [];
                        Ids.push(selected.Id);
                        Ids.push(selected.Id);
                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                            this.SetStatusCompleted(selected.Id, event);

                        });


                    }
                }


            }



        }
        this.preventSelect = false;



    }


    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {
        //this.currentEditObjectTableName = objectTableName;

        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        //editWindow.ZIndex = 5;
        this.cd.detach();
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {
            this.cd.reattach();
            this.RefreshEntity();

            //editControl.BackButton.Click += new RoutedEventHandler(BackButton_Click);
            //editWindow.CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
            //editControl.OkButton.Click += new RoutedEventHandler(OkButton_Click);
            //editControl.OkAndCloseButton.Click += new RoutedEventHandler(OkAndCloseButton_Click);

        });

    }
    EnableFilters: boolean = true;
    OnDataLoaded(result) {
        this.EnableFilters = true;
    }

    SetStatusCompleted(id: string, $event: any) {


        this.entityListService.getSingle(id, "Customs.Notification").then((res: any) => {
            //var re = res;
            res.subscribe((aa: any) => {
                $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
            })
        });
    }


    ShowCustomsDeclarationCargoSplit(EntityId: string) {
        var windowArgs: any = {};
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this._EntityPMService.getSingle("Customs.DeclarationCargoSplit", EntityId).then((res: any) => {
                    res.subscribe((myResponse: any) => {

                        if (myResponse.HasError) {
                            console.log("Error while getting EntityPM", myResponse);
                        }
                        else {
                            windowArgs.CurrentEntity = myResponse.Result;
                            var logWindow = new LogitudeWindow();

                            logWindow.Width = 770;
                            logWindow.Height = 750;
                            logWindow.Title = "בקשת פיצול מטען ";
                            if (myResponse.Result != null) {
                                if (!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber)) {
                                    logWindow.Title = logWindow.Title + myResponse.Result.RequestNumber;
                                }
                                if (!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName)) {
                                    logWindow.Title = logWindow.Title + " - " + myResponse.Result.ResponseStatusName;
                                }
                            }

                            logWindow.WindowArgs = windowArgs;
                            logWindow.ShowCloseButton = true;
                            logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
                            logWindow.WindowClosed.subscribe(($event1: any) => {
                            });
                        }
                    });

                });
            });

        });
    }

    selectedIds: string[] = [];
  
    MarkAsReadMethod() {
        this.selectedIds = [];
        this.connectedItems = this.selectedItems;
        this.selectedNotifications.status = "Read";
        if (this.AssigneeFilterSelectedValue != 'Assignee') {
            this.selectedNotifications.AssigneToId = null;
        }
        else {
            this.selectedNotifications.AssigneToId = this.AssigneToId;
        }
        this.selectedNotifications.AssigneToNotificationTypeCode = this.AssigneToNotificationTypeCode;
        if (this.ObjectTableName == "Customs.Declaration") {
            this.selectedNotifications.CustomFileNo = this.DeclarationPM.CustomFileNo;
            this.selectedNotifications.DeclarationId = this.DeclarationPM.Id;
        }
        this.selectedNotifications.DeclarationOfficeCode = this.DeclarationOfficeCode;
        this.selectedNotifications.DepartmentId = this.DepartmentId;
        this.selectedNotifications.DueDate = this.selectedDateId;
        this.selectedNotifications.IsClosedByAssignee = this.OpenClosedFilterSelectedValue;
        this.selectedNotifications.IsHandledByCustomOffice = this.IsHandledByCustomOffice;
        this.selectedNotifications.SearchFields = this.searchValue;
        this.selectedNotifications.SeenByAssigneeStatus = this.status;
        this.selectedNotifications.ObjectTableName = this.ObjectTableName;

        if (this.IsSelected) {
            this.selectedNotifications.dataCount = this.DataSource.rowCount;
            this.selectedNotifications.IsAllSelected = true;
            if (this.ExcludedItems.Length > 0) {
                this.selectedNotifications.ExcludedIds = this.ExcludedItems.Collection;
            }

            SessionLocator.SelectedSession.StartBusyIndicatorLoading();

            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
              
                this.LoadNotifications();
                this.IsSelected = false;
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow();

                    msg.Show("1000 התראות שנבחרות סומנו כנקראו");
                }
            });
        }
        else {

            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;

            this.selectedNotifications.selectedIds = [];

            this.selectedItems.Collection.forEach((item) => {
                this.selectedNotifications.selectedIds.push(item.rowData.Id); 
            });
            var Notifications: NotificationPM[] = [];
            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
                Notifications = response.Result;
               
                response.Result.forEach((value, key) => {
                    var temp = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                  
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                   
                    this.SelectedItemsCount = 0;
                    this.IsReadButtonVisible = false;
                    this.IsUnReadButtonVisible = false;
                    this.IsCloseButtonVisible = false;
                    this.IsSelectedTextVisible = false;
                    this.IsReopenButtonVisible = false;
                    var selected = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                    if (selected) {
                        this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0].rowData = value;
                    }
                });

                //for (let item of this.selectedItems.Collection) {
                //    var temp = Notifications.filter(d => d.Id == item.rowData.Id)[0];
                //    if (item.rowData.Id == temp.Id) {
                //        //item.rowData.IsSeenByAssignee = temp.IsSeenByAssignee;
                //    }
                //}

                this.CustomBackFromEditevent.emit(this.selectedItems.Collection);
                this.selectedItems.Clear();

            });
            SessionLocator.SelectedSession.StopBusyIndicator();
        }

    }

    MarkAsUnreadMethod() {
        this.selectedIds = [];
        this.selectedNotifications.status = "Unread";
        if (this.AssigneeFilterSelectedValue != 'Assignee') {
            this.selectedNotifications.AssigneToId = null;
        }
        else {
            this.selectedNotifications.AssigneToId = this.AssigneToId;
        }
        this.selectedNotifications.AssigneToNotificationTypeCode = this.AssigneToNotificationTypeCode;
        if (this.ObjectTableName == "Customs.Declaration") {
            this.selectedNotifications.CustomFileNo = this.DeclarationPM.CustomFileNo;
            this.selectedNotifications.DeclarationId = this.DeclarationPM.Id;
        }
        this.selectedNotifications.DeclarationOfficeCode = this.DeclarationOfficeCode;
        this.selectedNotifications.DepartmentId = this.DepartmentId;
        this.selectedNotifications.DueDate = this.selectedDateId;
        this.selectedNotifications.IsClosedByAssignee = this.OpenClosedFilterSelectedValue;
        this.selectedNotifications.IsHandledByCustomOffice = this.IsHandledByCustomOffice;
        this.selectedNotifications.SearchFields = this.searchValue;
        this.selectedNotifications.SeenByAssigneeStatus = this.status;
        this.selectedNotifications.ObjectTableName = this.ObjectTableName;

        if (this.IsSelected) {
            this.selectedNotifications.dataCount  = this.DataSource.rowCount;
            this.selectedNotifications.IsAllSelected = true;
            if (this.ExcludedItems.Length > 0) {
                this.selectedNotifications.ExcludedIds = this.ExcludedItems.Collection;
            }
            this.currentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
                
                this.LoadNotifications();
                this.IsSelected = false;
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow();

                    msg.Show("1000 התראות שנבחרו סומנו כ-לא נקראו");
                }
            });
        }
        else {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;

            this.selectedNotifications.selectedIds = [];

            this.selectedItems.Collection.forEach((item) => {
                this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });

            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
                response.Result.forEach((value, key) => {
                    var temp = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                 
                    this.SelectedItemsCount = 0;
                    this.IsReadButtonVisible = false;
                    this.IsUnReadButtonVisible = false;
                    this.IsCloseButtonVisible = false;
                    this.IsSelectedTextVisible = false;
                    this.IsReopenButtonVisible = false;
                    var selected = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                    if (selected) {
                        this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0].rowData = value;

                    }
                });

                this.CustomBackFromEditevent.emit(this.selectedItems.Collection);
                this.selectedItems.Clear();

            });
            SessionLocator.SelectedSession.StopBusyIndicator();
        }
    }

    MarkAsClosedMethod() {
       
        this.selectedIds = [];
        this.selectedNotifications.status = "Close";
        if (this.AssigneeFilterSelectedValue != 'Assignee') {
            this.selectedNotifications.AssigneToId = null;
        }
        else {
            this.selectedNotifications.AssigneToId = this.AssigneToId;
        }
        this.selectedNotifications.AssigneToNotificationTypeCode = this.AssigneToNotificationTypeCode;
        if (this.ObjectTableName == "Customs.Declaration") {
            this.selectedNotifications.CustomFileNo = this.DeclarationPM.CustomFileNo;
            this.selectedNotifications.DeclarationId = this.DeclarationPM.Id;
        }
        this.selectedNotifications.DeclarationOfficeCode = this.DeclarationOfficeCode;
        this.selectedNotifications.DepartmentId = this.DepartmentId;
        this.selectedNotifications.DueDate = this.selectedDateId;
        this.selectedNotifications.IsClosedByAssignee = this.OpenClosedFilterSelectedValue;
        this.selectedNotifications.IsHandledByCustomOffice = this.IsHandledByCustomOffice;
        this.selectedNotifications.SearchFields = this.searchValue;
        this.selectedNotifications.SeenByAssigneeStatus = this.status;
        this.selectedNotifications.ObjectTableName = this.ObjectTableName;

        if (this.IsSelected) {
            this.selectedNotifications.dataCount = this.DataSource.rowCount;
            this.selectedNotifications.IsAllSelected = true;
            if (this.ExcludedItems.Length > 0) {
                this.selectedNotifications.ExcludedIds = this.ExcludedItems.Collection;
            }
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();

            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
              
                this.LoadNotifications();
                this.IsSelected = false;
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow();

                    msg.Show("1000 התראות שנבחרו סומנו כסגורות");
                }
            });
        }
        else {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;

            this.selectedNotifications.selectedIds = [];

            this.selectedItems.Collection.forEach((item) => {
                this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });

            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
                response.Result.forEach((value, key) => {
                
                    var temp = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                    if (temp) {
                        this.ShowHLineOverRow.emit(temp.rowIndex);
                    }
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
               
                    this.SelectedItemsCount = 0;
                    this.IsReadButtonVisible = false;
                    this.IsUnReadButtonVisible = false;
                    this.IsCloseButtonVisible = false;
                    this.IsSelectedTextVisible = false;
                    this.IsReopenButtonVisible = false;
                    this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0].rowData = value;
                   
                  
                });
                this.selectedItems.Clear();

                SessionLocator.SelectedSession.StopBusyIndicator();
                //this.CustomBackFromEditevent.emit(this.selectedItems.Collection);


            });
        }
    }

    ReopenMethod() {
        this.selectedIds = [];
        this.selectedNotifications.status = "Open";
        if (this.AssigneeFilterSelectedValue != 'Assignee') {
            this.selectedNotifications.AssigneToId = null;
        }
        else {
            this.selectedNotifications.AssigneToId = this.AssigneToId;
        }
        this.selectedNotifications.AssigneToNotificationTypeCode = this.AssigneToNotificationTypeCode;
        if (this.ObjectTableName == "Customs.Declaration") {
            this.selectedNotifications.CustomFileNo = this.DeclarationPM.CustomFileNo;
            this.selectedNotifications.DeclarationId = this.DeclarationPM.Id;
        }
        this.selectedNotifications.DeclarationOfficeCode = this.DeclarationOfficeCode;
        this.selectedNotifications.DepartmentId = this.DepartmentId;
        this.selectedNotifications.DueDate = this.selectedDateId;
        this.selectedNotifications.IsClosedByAssignee = this.OpenClosedFilterSelectedValue;
        this.selectedNotifications.IsHandledByCustomOffice = this.IsHandledByCustomOffice;
        this.selectedNotifications.SearchFields = this.searchValue;
        this.selectedNotifications.SeenByAssigneeStatus = this.status;
        this.selectedNotifications.ObjectTableName = this.ObjectTableName;

        if (this.IsSelected) {
            this.selectedNotifications.dataCount = this.DataSource.rowCount;
            this.selectedNotifications.IsAllSelected = true;

            if (this.ExcludedItems.Length > 0) {
                this.selectedNotifications.ExcludedIds = this.ExcludedItems.Collection;
            }

            SessionLocator.SelectedSession.StartBusyIndicatorLoading();

            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
              
                this.LoadNotifications();
                this.IsSelected = false;
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow();

                    msg.Show("1000 התראות שנבחרו סומנו כפתוחות");
                }

            });
        }
        else {
          
            this.selectedNotifications.selectedIds = [];

            this.selectedNotifications.IsAllSelected = false;

            this.selectedItems.Collection.forEach((item) => {
                this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });

            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.currentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe((response:any) => {
                response.Result.forEach((value, key) => {
                    var temp = this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0];
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                  
                    this.SelectedItemsCount = 0;
                    this.selectedItems.Collection.filter(a => a.rowData.Id == value.Id)[0].rowData = value;
                });

                this.CustomBackFromEditevent.emit(this.selectedItems.Collection);
                this.IsReopenButtonVisible = false;
                this.selectedItems.Clear();
                this.LoadNotifications();
                SessionLocator.SelectedSession.StopBusyIndicator();
            });
        }
    }

}
