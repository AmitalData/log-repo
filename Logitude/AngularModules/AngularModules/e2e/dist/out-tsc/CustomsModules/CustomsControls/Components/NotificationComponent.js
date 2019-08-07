"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
//import {NotificationListService} from  '../../../Services/StandardLists/NotificationListService';
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var DueDate_1 = require("../../../Customs/DataContract/DueDate");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var NotificationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/NotificationExtendedListService");
var NotificationWebService_1 = require("../../../Customs/Services/WebServices/NotificationWebService");
var CustomsCollateralPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsCollateralPMService");
var SelectedNotifications_1 = require("../../../Customs/DataContract/SelectedNotifications");
var NotificationComponent = /** @class */ (function (_super) {
    __extends(NotificationComponent, _super);
    function NotificationComponent(entityArgs, EntityResourceService, cd) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.cd = cd;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.CustomBackFromEditevent = new core_1.EventEmitter();
        _this.ShowHLineOverRow = new core_1.EventEmitter();
        _this.declarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.notificationExtendedListService = new NotificationExtendedListService_1.NotificationExtendedListService();
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.notificationWebService = new NotificationWebService_1.NotificationWebService();
        _this.customsCollateralPMService = new CustomsCollateralPMService_1.CustomsCollateralPMService();
        _this.selectedNotifications = new SelectedNotifications_1.SelectedNotifications();
        _this.DataContext = _this;
        _this.IsVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.columns = null;
        _this.Right = 0;
        _this.AssigneeFilterSelectedValue = 'Assignee';
        _this.NotificationTypeFilterSelectedValue = 'All';
        _this.OpenClosedFilterSelectedValue = null;
        _this.DataSource = {
            pageSize: 17,
            rowCount: null,
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.CheckBoxFilterChanged = new core_1.EventEmitter();
        _this.status = null;
        _this.IsSelectedTextVisible = false;
        _this.SelectedItemsCountText = null;
        _this.departmentId = null;
        _this.declarationOfficeCode = null;
        _this.EnableFilters = true;
        _this.selectedIds = [];
        //var t = setInterval(() => {this.timerValue++;}, 1000); // TESTING!! timer for testing detect changes
        _this.CurrentSession.SubscriptionAdd(_this.CurrentSession.SessionEvent.subscribe(function ($event) {
            if ($event.Name == "ClosedByAssigneeClicked") {
                _this.ShowHLineOverRow.emit($event.rowIndex);
                _this.preventSelect = true;
            }
        }));
        _this.ExcludedItems = new ObservableCollection_1.ObservableCollection([]);
        _this.selectedItems = new ObservableCollection_1.ObservableCollection([]);
        _this.connectedItems = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.SubscriptionAdd(_this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res == "select") {
                _this.preventSelect = true;
            }
        }));
        _this.ObjectTableName = entityArgs.ObjectTableName;
        _this.DeclarationPM = entityArgs.EntityPM;
        if (_this.ObjectTableName == null) {
            _this.ObjectTableName = "Customs.Notification";
            _this.OpenClosedFilterSelectedValue = "open";
            _this.AssigneToId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
        if (_this.ObjectTableName == "Customs.Declaration") {
            _this.IsDeclarationTab = true;
            _this.OpenClosedFilterSelectedValue = "all";
        }
        return _this;
    }
    NotificationComponent.prototype.LoadNotifications = function () {
        this.selectedItems.Collection = [];
        this.ExcludedItems.Collection = [];
        if ((this.status == "none" || this.status == null) && !this.IsSelected) {
            this.SelectedItemsCountText = null;
            this.SelectedItemsCount = 0;
            this.IsSelectedTextVisible = false;
        }
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    NotificationComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe(function (response) {
            _this.IsVisible = true;
            _this.BuildColumns();
            _this.DueDatesList = [];
            _this.DueDatesList.push(new DueDate_1.DueDate("0", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.All")));
            _this.DueDatesList.push(new DueDate_1.DueDate("1", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.UntilToday")));
            _this.DueDatesList.push(new DueDate_1.DueDate("2", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.Next3Days")));
            _this.DueDatesList.push(new DueDate_1.DueDate("3", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.NextWeek")));
            _this.DueDatesList.push(new DueDate_1.DueDate("4", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.NextMonth")));
            _this.selectedDate = _this.DueDatesList[0];
            var screenWidth = _this.getScreenWidth();
            var screenHeight = _this.getScreenHeight();
            if (screenWidth == 1024) {
                _this.Right = 190;
            }
            else {
                _this.Right = 520;
            }
        });
    };
    NotificationComponent.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    NotificationComponent.prototype.getScreenWidth = function () {
        if (self.innerWidth) {
            return self.innerWidth;
        }
        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }
        if (document.body) {
            return document.body.clientWidth;
        }
    };
    NotificationComponent.prototype.AssigneeFilterItemClicked = function (value) {
        if (this.EnableFilters) {
            if (this.AssigneeFilterSelectedValue != value) {
                this.AssigneeFilterSelectedValue = value;
                this.LoadNotifications();
            }
        }
    };
    NotificationComponent.prototype.NotificationTypeilterItemClicked = function (value) {
        if (this.EnableFilters) {
            if (this.NotificationTypeFilterSelectedValue != value) {
                this.NotificationTypeFilterSelectedValue = value;
                this.LoadNotifications();
            }
        }
    };
    NotificationComponent.prototype.OpenClosedFilterItemClicked = function (value) {
        if (this.OpenClosedFilterSelectedValue != value) {
            this.IsSelected = false;
            this.OpenClosedFilterSelectedValue = value;
            this.status = null;
            this.LoadNotifications();
        }
    };
    Object.defineProperty(NotificationComponent.prototype, "Assignee", {
        get: function () { return this.assignee; },
        set: function (value) {
            if (this.assignee != value) {
                this.assignee = value;
                this.LoadNotifications();
            }
        },
        enumerable: true,
        configurable: true
    });
    NotificationComponent.prototype.DueDateSelectionChanged = function (date) {
        if (date) {
            this.selectedDateId = date.Id;
        }
        this.LoadNotifications();
    };
    NotificationComponent.prototype.BuildColumns = function () {
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
            Display: 'מספר חשבון',
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.NotificationDefinitionName"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.DueDate"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.Reference1Number"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.CustomerName"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.CreateDate"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.AssigneToName"),
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
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.F.IsClosedByAssignee"),
            HtmlListComponentName: 'NotificationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/NotificationListTemplate',
            IsCustomTemplate: true,
            Styles: { width: '100px' },
            EnableHoverVisibility: true,
            ServerSideSortable: true,
            SortByName: 'IsClosedByAssignee'
        });
    };
    NotificationComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        this.EnableFilters = false;
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        //   this.CurrentSession.StartBusyIndicatorLoading();
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
                    filters.addAdditionalFilter("DueDate", Tools_1.DateTool.GetCurrentDateAsUtc(), Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 3), null, "Between", false, false, false, "DateTime");
                    break;
                }
            case '3':
                {
                    filters.addAdditionalFilter("DueDate", Tools_1.DateTool.GetCurrentDateAsUtc(), Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 7), null, "Between", false, false, false, "DateTime");
                    break;
                }
            case '4':
                {
                    filters.addAdditionalFilter("DueDate", Tools_1.DateTool.GetCurrentDateAsUtc(), Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 30), null, "Between", false, false, false, "DateTime");
                    break;
                }
            default: {
                break;
            }
        }
        switch (this.AssigneeFilterSelectedValue) {
            case 'Assignee':
                {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.AssigneToId)) {
                        filters.addAdditionalFilter("AssigneToId", this.AssigneToId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case 'Department':
                {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.DepartmentId)) {
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
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
            filters.addAdditionalFilter("DeclarationOfficeCode", this.DeclarationOfficeCode, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchValue)) {
            filters.addAdditionalFilter("SearchFields", this.searchValue, null, null, "Contains", false, false, false, "string");
        }
        if (this.ObjectTableName == "Customs.Declaration") {
            filters.addAdditionalFilter("Declaration", this.DeclarationPM.Id, this.DeclarationPM.CustomFileNo, null, "Equals", true, false, true, "string");
        }
        this.notificationExtendedListService.getCountByFilters(filters).subscribe(function (res) {
            _this.counts = res.Result.Result;
            _this.allreadCount = _this.counts.AllReadCount;
            _this.allUnreadCount = _this.counts.AllUnreadCount;
            if (_this.allreadCount == 0) {
                _this.IsUnReadButtonVisible = false;
            }
            if (_this.allUnreadCount == 0) {
                _this.IsReadButtonVisible = false;
            }
            if (_this.OpenClosedFilterSelectedValue != "closed") {
                _this.openCount = "(" + _this.counts.OpenCount.toString() + ")";
                _this.allCount = "(" + _this.counts.AllCount.toString() + ")";
                _this.infoCount = "(" + _this.counts.InfoCount.toString() + ")";
                _this.actionCount = "(" + _this.counts.ActionCount.toString() + ")";
            }
            else {
                _this.openCount = "(" + _this.counts.OpenCount.toString() + ")";
                _this.allCount = null;
                _this.infoCount = null;
                _this.actionCount = null;
            }
            if (_this.status == "read" || _this.status == "unread" || _this.status == "all") {
                if (_this.OpenClosedFilterSelectedValue != "closed") {
                    _this.dataCount = _this.counts.OpenCount;
                }
                else {
                    _this.dataCount = _this.counts.ClosedCount;
                }
                if (_this.dataCount) {
                    if (_this.IsSelected) {
                        _this.IsSelectedTextVisible = true;
                        _this.SelectedItemsCount = _this.dataCount;
                        _this.SelectedItemsCountText = "נבחרו " + _this.dataCount.toString() + " פריטים מתוך " + _this.dataCount.toString();
                    }
                }
                else {
                    _this.IsReadButtonVisible = false;
                    _this.IsUnReadButtonVisible = false;
                    _this.IsReopenButtonVisible = false;
                    _this.IsCloseButtonVisible = false;
                    _this.IsSelectedTextVisible = false;
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
            //    this.CurrentSession.StopBusyIndicator();
        });
        return this.entityListService.getExtendedByFilters("Customs.Notification", filters);
    };
    Object.defineProperty(NotificationComponent.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
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
                    else if (this.status == "read")
                        this.IsUnReadButtonVisible = true;
                    else if (this.status == "unread")
                        this.IsReadButtonVisible = true;
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
        },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(NotificationComponent.prototype, "SelectedItemsCount", {
        get: function () { return this.selectedItemsCount; },
        set: function (value) {
            this.selectedItemsCount = value;
            if (value == 0) {
                this.IsSelectedTextVisible = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    NotificationComponent.prototype.onCheckBoxChecked = function ($event) {
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
            if (removedIndex != null) {
                this.selectedItems.RemoveFromIndex(removedIndex);
            }
            if (this.selectedItems.Length > 0) {
                var read = this.selectedItems.Collection.filter(function (d) { return d.rowData.IsSeenByAssignee; })[0];
                var unread = this.selectedItems.Collection.filter(function (d) { return !d.rowData.IsSeenByAssignee; })[0];
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
            if (this.SelectedItemsCount == 0) {
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
    };
    NotificationComponent.prototype.OnAllBtnClicked = function () {
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
    };
    NotificationComponent.prototype.OnReadBtnClicked = function () {
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
    };
    NotificationComponent.prototype.OnUnReadBtnClicked = function () {
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
    };
    NotificationComponent.prototype.OnNoneBtnClicked = function () {
        this.IsReadButtonVisible = false;
        this.IsUnReadButtonVisible = false;
        this.IsCloseButtonVisible = false;
        this.IsReopenButtonVisible = false;
        this.status = "none";
        this.LoadNotifications();
    };
    Object.defineProperty(NotificationComponent.prototype, "DepartmentId", {
        get: function () { return this.departmentId; },
        set: function (newValue) {
            this.departmentId = newValue;
            this.LoadNotifications();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationComponent.prototype, "AssigneToId", {
        get: function () { return this.assigneToId; },
        set: function (newValue) {
            this.assigneToId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationComponent.prototype, "DeclarationOfficeCode", {
        get: function () { return this.declarationOfficeCode; },
        set: function (newValue) {
            this.declarationOfficeCode = newValue;
            this.LoadNotifications();
        },
        enumerable: true,
        configurable: true
    });
    NotificationComponent.prototype.RefreshEntity = function () {
        this.selectedItems = new ObservableCollection_1.ObservableCollection([]);
        this.LoadNotifications();
    };
    NotificationComponent.prototype.ViewInitCompleted = function ($event) {
        this.LoadNotifications();
    };
    NotificationComponent.prototype.Search = function (value) {
        var _this = this;
        var tkn = setTimeout(function () {
            _this.searchValue = value;
            _this.LoadNotifications();
        }, 200);
    };
    NotificationComponent.prototype.OnRowSelected = function (event) {
        var _this = this;
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
                var customEditIdentityKey = Guid_1.Guid.newGuid();
                var control = null;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
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
                                case "5101D":
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
                                    {
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                case "5101N":
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
                                    {
                                        currentScreenCode = "DCTP";
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
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
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
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
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
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
                                    {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                            _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                                                _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(function (response) {
                                                    _this.customsCollateralPMService.get(selected.EntityId).subscribe(function (response) {
                                                        var result = response.Result;
                                                        console.log("[response] customsCollateralPMService.get", result);
                                                        if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                                                            control = './CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
                                                            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                            logitudeWindow.WindowArgs = { CurrentEntity: result };
                                                            logitudeWindow.Height = 730;
                                                            logitudeWindow.Width = 660;
                                                            //logitudeWindow.ZIndex = 5;
                                                            _this.cd.detach();
                                                            logitudeWindow.Show(control);
                                                            logitudeWindow.WindowClosed.subscribe(function () {
                                                                _this.cd.reattach();
                                                                _this.RefreshEntity();
                                                            });
                                                            var Ids = [];
                                                            Ids.push(selected.Id);
                                                            Ids.push(selected.Id);
                                                            _this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                                                                _this.SetStatusCompleted(selected.Id, event);
                                                            });
                                                        }
                                                        else {
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
                                        var msg = new MessageWindow_1.MessageWindow();
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
                    default:
                        {
                            if (!Tools_1.AppTool.IsNullOrEmpty(selected.Reference1Number)) {
                                this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(selected.Reference1Number).subscribe(function (res) {
                                    var declaration = res.Result;
                                    console.log("[reponse] GetSingleDeclarationByCustomFileNo: ", declaration);
                                    if (!Tools_1.AppTool.IsNullOrEmpty(declaration)) {
                                        selected.ObjectTableName = "Customs.Declaration";
                                        currentScreenCode = "DEGC";
                                        _this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);
                                        //selectedIds.Clear();
                                        //selectedIds.Add(selected.Id);
                                        //status = "Read";
                                        //statusOp = Context.SetNotificationsStatus(selectedIds, "Read", TenantContext.Current.Id);
                                        //statusOp.Completed += statusOp_Completed;
                                        var Ids = [];
                                        Ids.push(selected.Id);
                                        Ids.push(selected.Id);
                                        _this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                                            _this.SetStatusCompleted(selected.Id, event);
                                        });
                                    }
                                });
                            }
                            else {
                                var msg = new MessageWindow_1.MessageWindow();
                                //msg.ZIndex = 5;
                                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            }
                            break;
                        }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(control)) {
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
                    //this.CurrentSession.StartBusyIndicatorLoading();
                    //EntityIdForCustomEditControlEvent entityIdEvent = eventAggregator.GetEvent<EntityIdForCustomEditControlEvent>();
                    //entityIdEvent.Publish(new EntityIdForCustomEditControlEventArgs() { EntityId = selected.EntityId, ObjectTableName = selected.ObjectTableName, IdentityKey = customEditIdentityKey });
                    //selectedIds.Clear();
                    //selectedIds.Add(selected.Id);
                    //status = "Read";
                    //statusOp = Context.SetNotificationsStatus(selectedIds, "Read", TenantContext.Current.Id);
                    //statusOp.Completed += statusOp_Completed;
                    var Ids = [];
                    Ids.push(selected.Id);
                    Ids.push(selected.Id);
                    this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                        _this.SetStatusCompleted(selected.Id, event);
                    });
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(currentScreenCode)) {
                        if (selected.ObjectTableName == "Customs.Declaration") {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    SelectedTabCode: currentScreenCode,
                                    EntityId: selected.EntityId,
                                    ObjectTableName: selected.ObjectTableName
                                });
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
                        var Ids = [];
                        Ids.push(selected.Id);
                        Ids.push(selected.Id);
                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                            _this.SetStatusCompleted(selected.Id, event);
                        });
                    }
                }
            }
        }
        this.preventSelect = false;
    };
    NotificationComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        //this.currentEditObjectTableName = objectTableName;
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        //editWindow.ZIndex = 5;
        this.cd.detach();
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
            _this.cd.reattach();
            _this.RefreshEntity();
            //editControl.BackButton.Click += new RoutedEventHandler(BackButton_Click);
            //editWindow.CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
            //editControl.OkButton.Click += new RoutedEventHandler(OkButton_Click);
            //editControl.OkAndCloseButton.Click += new RoutedEventHandler(OkAndCloseButton_Click);
        });
    };
    NotificationComponent.prototype.OnDataLoaded = function (result) {
        this.EnableFilters = true;
    };
    NotificationComponent.prototype.SetStatusCompleted = function (id, $event) {
        this.entityListService.getSingle(id, "Customs.Notification").then(function (res) {
            //var re = res;
            res.subscribe(function (aa) {
                $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
            });
        });
    };
    NotificationComponent.prototype.MarkAsReadMethod = function () {
        var _this = this;
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
            this.CurrentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                _this.LoadNotifications();
                _this.IsSelected = false;
                _this.CurrentSession.StopBusyIndicator();
                if (_this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("1000 התראות שנבחרות סומנו כנקראו");
                }
            });
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;
            this.selectedNotifications.selectedIds = [];
            this.selectedItems.Collection.forEach(function (item) {
                _this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });
            var Notifications = [];
            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                Notifications = response.Result;
                response.Result.forEach(function (value, key) {
                    var temp = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                    _this.SelectedItemsCount = 0;
                    _this.IsReadButtonVisible = false;
                    _this.IsUnReadButtonVisible = false;
                    _this.IsCloseButtonVisible = false;
                    _this.IsSelectedTextVisible = false;
                    _this.IsReopenButtonVisible = false;
                    var selected = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    if (selected) {
                        _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0].rowData = value;
                    }
                });
                //for (let item of this.selectedItems.Collection) {
                //    var temp = Notifications.filter(d => d.Id == item.rowData.Id)[0];
                //    if (item.rowData.Id == temp.Id) {
                //        //item.rowData.IsSeenByAssignee = temp.IsSeenByAssignee;
                //    }
                //}
                _this.CustomBackFromEditevent.emit(_this.selectedItems.Collection);
                _this.selectedItems.Clear();
            });
            this.CurrentSession.StopBusyIndicator();
        }
    };
    NotificationComponent.prototype.MarkAsUnreadMethod = function () {
        var _this = this;
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
            this.selectedNotifications.dataCount = this.DataSource.rowCount;
            this.selectedNotifications.IsAllSelected = true;
            if (this.ExcludedItems.Length > 0) {
                this.selectedNotifications.ExcludedIds = this.ExcludedItems.Collection;
            }
            this.CurrentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                _this.LoadNotifications();
                _this.IsSelected = false;
                _this.CurrentSession.StopBusyIndicator();
                if (_this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("1000 התראות שנבחרו סומנו כ-לא נקראו");
                }
            });
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;
            this.selectedNotifications.selectedIds = [];
            this.selectedItems.Collection.forEach(function (item) {
                _this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });
            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                response.Result.forEach(function (value, key) {
                    var temp = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                    _this.SelectedItemsCount = 0;
                    _this.IsReadButtonVisible = false;
                    _this.IsUnReadButtonVisible = false;
                    _this.IsCloseButtonVisible = false;
                    _this.IsSelectedTextVisible = false;
                    _this.IsReopenButtonVisible = false;
                    var selected = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    if (selected) {
                        _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0].rowData = value;
                    }
                });
                _this.CustomBackFromEditevent.emit(_this.selectedItems.Collection);
                _this.selectedItems.Clear();
            });
            this.CurrentSession.StopBusyIndicator();
        }
    };
    NotificationComponent.prototype.MarkAsClosedMethod = function () {
        var _this = this;
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
            this.CurrentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                _this.LoadNotifications();
                _this.IsSelected = false;
                _this.CurrentSession.StopBusyIndicator();
                if (_this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("1000 התראות שנבחרו סומנו כסגורות");
                }
            });
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.selectedNotifications.IsAllSelected = false;
            this.selectedNotifications.selectedIds = [];
            this.selectedItems.Collection.forEach(function (item) {
                _this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });
            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                response.Result.forEach(function (value, key) {
                    var temp = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    if (temp) {
                        _this.ShowHLineOverRow.emit(temp.rowIndex);
                    }
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                    _this.SelectedItemsCount = 0;
                    _this.IsReadButtonVisible = false;
                    _this.IsUnReadButtonVisible = false;
                    _this.IsCloseButtonVisible = false;
                    _this.IsSelectedTextVisible = false;
                    _this.IsReopenButtonVisible = false;
                    _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0].rowData = value;
                });
                _this.selectedItems.Clear();
                _this.CurrentSession.StopBusyIndicator();
                //this.CustomBackFromEditevent.emit(this.selectedItems.Collection);
            });
        }
    };
    NotificationComponent.prototype.ReopenMethod = function () {
        var _this = this;
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
            this.CurrentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                _this.LoadNotifications();
                _this.IsSelected = false;
                _this.CurrentSession.StopBusyIndicator();
                if (_this.DataSource.rowCount > 1000) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("1000 התראות שנבחרו סומנו כפתוחות");
                }
            });
        }
        else {
            this.selectedNotifications.selectedIds = [];
            this.selectedNotifications.IsAllSelected = false;
            this.selectedItems.Collection.forEach(function (item) {
                _this.selectedNotifications.selectedIds.push(item.rowData.Id);
            });
            this.selectedNotifications.dataCount = this.selectedItems.Length;
            this.CurrentSession.StartBusyIndicatorLoading();
            this.notificationExtendedListService.PutNotificationStatus(this.selectedNotifications).subscribe(function (response) {
                response.Result.forEach(function (value, key) {
                    var temp = _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0];
                    //var IsChecked = temp.IsChecked;
                    //value.IsChecked = IsChecked;
                    _this.SelectedItemsCount = 0;
                    _this.selectedItems.Collection.filter(function (a) { return a.rowData.Id == value.Id; })[0].rowData = value;
                });
                _this.CustomBackFromEditevent.emit(_this.selectedItems.Collection);
                _this.IsReopenButtonVisible = false;
                _this.selectedItems.Clear();
                _this.LoadNotifications();
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NotificationComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NotificationComponent.prototype, "CustomBackFromEditevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NotificationComponent.prototype, "ShowHLineOverRow", void 0);
    NotificationComponent = __decorate([
        core_1.Component({
            selector: 'NotificationComponent',
            moduleId: module.id,
            templateUrl: './NotificationComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService, core_1.ChangeDetectorRef])
    ], NotificationComponent);
    return NotificationComponent;
}(BaseComponent_1.BaseComponent));
exports.NotificationComponent = NotificationComponent;
//# sourceMappingURL=NotificationComponent.js.map