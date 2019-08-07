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
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var TraceEventPM_1 = require("../../../Infrastructure/EntityPMs/TraceEventPM");
var WebFreightDomainService_1 = require("../../../Infrastructure/Services/WebFreightDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var EventsTabComponent = /** @class */ (function () {
    function EventsTabComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.EventsCount = 0;
        this.IsCustomerCare = false;
        this.IsHybrid = false;
        this.myDomainService = null;
        this.IsVisibile = false;
        this.LayoutDirection = 'ltr';
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsAddButtonEnabled = false;
        this.SessionEvent = null;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.IsPagesMenuVisible = false;
        this.includeDeleted = false;
        this.SearchText = null;
        this.TabHeaderTextCode = entityArgs.ObjectTableName + ".TH.Events";
        this._entityResourceService.getEntityResourceByTableName("TraceEvent", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            _this.myDomainService = new WebFreightDomainService_1.WebFreightDomainService();
            _this.ItemsSource = [];
            _this.AllTraceEvents = [];
            _this.InitTab();
        });
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    EventsTabComponent.prototype.InitTab = function () {
        var _this = this;
        this.EntityId = this.entityArgs.EntityPM.Id;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0].Id;
        this.IsCustomerCare = SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare;
        this.IsHybrid = SessionLocator_1.SessionLocator.TenantPM.IsHybrid;
        this.BuildPagesMenu();
        this.SetUIProperties();
        this.Listen();
        this.LoadData();
    };
    EventsTabComponent.prototype.SetUIProperties = function () {
        var isEnabled = false;
        if (this.EntityId) {
            if (!this.IsHybrid) {
                isEnabled = true;
            }
        }
        this.IsAddButtonEnabled = isEnabled;
    };
    EventsTabComponent.prototype.Listen = function () {
        var _this = this;
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "LoadEventTabData") {
                _this.LoadData();
            }
        });
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                var textcode = _this.CurrentSession.CurrentEditComponent.SelectedTab.TextCode;
                if (textcode && textcode.includes("TH.Event")) {
                    _this.LoadData();
                }
            });
            if (!this.EntityId) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityId = _this.entityArgs.EditComponent.EntityPM.Id;
                        _this.SetUIProperties();
                    }
                });
            }
        }
    };
    EventsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    EventsTabComponent.prototype.BuildPagesMenu = function () {
        this.PagesMenu = [];
        this.PagesMenu.push(new PageMenu("ALL"));
        if (this.ObjectTableName == "Shipment") {
            this.PagesMenu.push(new PageMenu("LEG"));
            this.PagesMenu.push(new PageMenu("OPE"));
            this.PagesMenu.push(new PageMenu("LOG"));
            this.PagesMenu.push(new PageMenu("CUS"));
            //this.PagesMenu.push(new PageMenu("DOC"));
            this.IsPagesMenuVisible = true;
        }
        this.OnPagesMenuSelecting(this.PagesMenu[0]);
    };
    EventsTabComponent.prototype.OnPagesMenuSelecting = function (item) {
        this.SelectedMenu = item;
        this.BuildItemsSource();
    };
    Object.defineProperty(EventsTabComponent.prototype, "IncludeDeleted", {
        get: function () { return this.includeDeleted; },
        set: function (newValue) {
            if (this.includeDeleted != newValue) {
                this.includeDeleted = newValue;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    EventsTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadData();
    };
    EventsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemsSource = [];
        this.myDomainService.GetTraceEventsForEntity(this.ObjectTableId, this.EntityId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.AllTraceEvents = myResponse.Result;
                    _this.BuildItemsSource();
                }
            }
        });
    };
    EventsTabComponent.prototype.SearchMethod = function (text) {
        if (Tools_1.AppTool.IsNullOrEmpty(text)) {
            this.SearchText = null;
        }
        else {
            this.SearchText = text;
        }
        this.BuildItemsSource();
    };
    EventsTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.SelectedMenu != null) {
            if (this.AllTraceEvents != null) {
                var items = [];
                this.AllTraceEvents.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.SearchText)) {
                        var searchText = _this.SearchText.toLowerCase();
                        if (item.EventTypeCode != null && item.EventTypeCode.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }
                        else if (item.EventTypeEnglishName != null && item.EventTypeEnglishName.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }
                        else if (item.ContactEnglishFirstName != null && item.ContactEnglishFirstName.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }
                        else if (item.Notes != null && item.Notes.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }
                    }
                    else {
                        items.push(item);
                    }
                });
                if (!this.IncludeDeleted) {
                    items = items.filter(function (f) { return f.Deleted == false; });
                }
                switch (this.SelectedMenu.Code) {
                    case "ALL": {
                        break;
                    }
                    case "CUS": {
                        items = items.filter(function (f) { return f.IsCustomerView; });
                        break;
                    }
                    default: {
                        items = items.filter(function (f) { return f.EventTypeCategoryCode != null; });
                        items = items.filter(function (f) { return f.EventTypeCategoryCode.toUpperCase() == _this.SelectedMenu.Code.toUpperCase(); });
                        break;
                    }
                }
                this.ItemsSource = [];
                this.EventsCount = items.length;
                items.forEach(function (item) {
                    _this.ItemsSource.push(new EventItemClass(item, _this));
                });
            }
        }
    };
    EventsTabComponent.prototype.AddButtonClicked = function () {
        var traceEventPM = new TraceEventPM_1.TraceEventPM();
        traceEventPM.IsManualEntry = true;
        traceEventPM.LogDateTime = Tools_2.DateTool.GetCurrentDateTimeAsUtc();
        traceEventPM.EventDateTime = Tools_2.DateTool.GetCurrentDateAsUtc();
        traceEventPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        traceEventPM.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
        traceEventPM.ObjectTableId = this.ObjectTableId;
        traceEventPM.EntityId = this.EntityId;
        traceEventPM.IsAddedManually = true;
        var itemClass = new EventItemClass(traceEventPM, this);
        itemClass.IsNewEntity = true;
        this.RunAddEditWindow(itemClass, TextCodeTranslator_1.TextCodeTranslator.Translate("TraceEvent.O.AddEvent"));
    };
    EventsTabComponent.prototype.EditItemClicked = function (itemClass) {
        this.RunAddEditWindow(itemClass, "Edit Event");
    };
    EventsTabComponent.prototype.DeleteItemClicked = function (itemClass) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Remove this event?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.myDomainService.DeleteTraceEvent(_this.EntityId, _this.ObjectTableId, itemClass.EntityPM.Id, true).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.LoadData();
                        }
                    }
                });
            }
        });
    };
    EventsTabComponent.prototype.RunAddEditWindow = function (item, title) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.WindowArgs = item;
        logWindow.Title = title;
        logWindow.Show('./Common/Components/Events/AddEditEventComponent');
    };
    EventsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EventsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], EventsTabComponent);
    return EventsTabComponent;
}());
exports.EventsTabComponent = EventsTabComponent;
var EventItemClass = /** @class */ (function (_super) {
    __extends(EventItemClass, _super);
    function EventItemClass(item, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.IsNewEntity = false;
        _this.ObjectTableName = "TraceEvent";
        _this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.Background = "rgba(235, 235, 235, 0.5)";
        _this.IsEditingDisabled = true;
        _this.IsDeletingDisabled = true;
        _this.EntityPM = item;
        _this.SetBackground();
        _this.SetUIProperties();
        return _this;
    }
    EventItemClass.prototype.SetBackground = function () {
        var myResult = "rgba(235, 235, 235, 0.5)";
        if (this.EntityPM.Deleted && this.EntityPM.IsAddedManually) {
            myResult = "rgba(247, 227, 227, 1)";
        }
        else if (this.EntityPM.Deleted) {
            myResult = "rgba(247, 227, 227, 1)";
        }
        else if (this.EntityPM.IsAddedManually) {
            myResult = "rgba(0, 125, 0, 0.098)";
        }
        this.Background = myResult;
    };
    EventItemClass.prototype.SetUIProperties = function () {
        var isEditingDisabled = true;
        var isDeletingDisabled = true;
        if (!this.Deleted) {
            if (this.IsAddedManually) {
                isEditingDisabled = false;
                isDeletingDisabled = false;
            }
            if (this.IsCustomerView) {
                isEditingDisabled = false;
            }
        }
        this.IsEditingDisabled = isEditingDisabled;
        this.IsDeletingDisabled = isDeletingDisabled;
        this.UIProperties.SetEnabled("EventDateTime", this.ObjectTableName, this.IsManualEntry);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsManualEntry);
    };
    Object.defineProperty(EventItemClass.prototype, "Name", {
        get: function () {
            return this.showLocal ? this.EntityPM.EventTypeLocalName : this.EntityPM.EventTypeEnglishName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "Location", {
        get: function () { return this.EntityPM.Location; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "LogDateTime", {
        get: function () { return this.EntityPM.LogDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "User", {
        get: function () {
            var userName = this.EntityPM.ContactEnglishFirstName;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PartnerName)) {
                userName = this.EntityPM.PartnerName;
            }
            return userName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "LableUser", {
        get: function () {
            var lableUser = "User";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PartnerName)) {
                lableUser = "Partner";
            }
            return lableUser;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "CustomerCare", {
        get: function () { return this.EntityPM.CustomerCareUserEmail; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "Deleted", {
        get: function () { return this.EntityPM.Deleted; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "IsManualEntry", {
        get: function () { return this.EntityPM.IsManualEntry; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "IsAddedManually", {
        get: function () { return this.EntityPM.IsAddedManually; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "IsCustomerView", {
        get: function () { return this.EntityPM.IsCustomerView; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "ObjectTableId", {
        get: function () { return this.EntityPM.ObjectTableId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "EventTypeId", {
        get: function () { return this.EntityPM.EventTypeId; },
        set: function (newValue) {
            if (this.EntityPM.EventTypeId != newValue) {
                this.EntityPM.EventTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "EventDateTime", {
        get: function () { return this.EntityPM.EventDateTime; },
        set: function (newValue) {
            if (this.EntityPM.EventDateTime != newValue) {
                this.EntityPM.EventDateTime = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventItemClass.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return EventItemClass;
}(BaseComponent_1.BaseComponent));
exports.EventItemClass = EventItemClass;
var PageMenu = /** @class */ (function () {
    function PageMenu(myCode) {
        this.Code = myCode;
        switch (myCode) {
            case "ALL": {
                this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TraceEvent.O.All");
                break;
            }
            case "LEG": {
                this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TraceEvent.O.Routings");
                break;
            }
            case "OPE": {
                this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TraceEvent.O.Operations");
                break;
            }
            case "LOG": {
                this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TraceEvent.O.Logs");
                break;
            }
            case "CUS": {
                this.Name = "Shared with Customer";
                break;
            }
            case "DOC": {
                this.Name = "Documents";
                break;
            }
        }
    }
    return PageMenu;
}());
//# sourceMappingURL=EventsTabComponent.js.map