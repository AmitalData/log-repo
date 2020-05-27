"use strict";
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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_1 = require("../../../Infrastructure/Args");
var ContactListService_1 = require("../../../Common/Services/StandardLists/ContactListService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var ContactWorkspaceComponent = /** @class */ (function () {
    function ContactWorkspaceComponent() {
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.QuickSearchItems = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //View by Filter
        this.filterName_ViewUpcomingBirthdays = "ViewUpcomingBirthdays";
        this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.ContactsPageControl";
        this.ViewByFilterList = [];
        // Load  Data
        this.UpcomingBirthdaysListCount = 0;
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService;
        this.ContactListService = new ContactListService_1.ContactListService();
    }
    ContactWorkspaceComponent.prototype.InitComponent = function (father) {
        this.FatherComp = father;
        this.BuildFilters();
        this.LoadAllScreenData();
    };
    ContactWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    ContactWorkspaceComponent.prototype.BuildFilters = function () {
        this.ViewByFilterList = [];
        this.ViewByFilterList.push(new CodeNameClass_1.CodeNameClass("W05", "Within 5 Days"));
        this.ViewByFilterList.push(new CodeNameClass_1.CodeNameClass("W10", "Within 10 Days"));
        this.ViewByFilterList.push(new CodeNameClass_1.CodeNameClass("W30", "Within a Month"));
        this.ViewByFilterList.push(new CodeNameClass_1.CodeNameClass("TOD", "Today"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewUpcomingBirthdays);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "W05";
        }
        this.SelectedViewByItem = this.ViewByFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    Object.defineProperty(ContactWorkspaceComponent.prototype, "SelectedViewByItem", {
        get: function () { return this.selectedViewByItem; },
        set: function (newValue) {
            if (this.selectedViewByItem != newValue) {
                this.selectedViewByItem = newValue;
                this.RunUpcomingBirthdaysFilter();
            }
        },
        enumerable: true,
        configurable: true
    });
    ContactWorkspaceComponent.prototype.RunUpcomingBirthdaysFilter = function () {
        if (this.SelectedViewByItem == null) {
            this.LoadUpcomingBirthdays(0, 5);
        }
        else {
            switch (this.SelectedViewByItem.Code) {
                case "W05":
                    {
                        this.LoadUpcomingBirthdays(0, 5);
                        break;
                    }
                case "W10":
                    {
                        this.LoadUpcomingBirthdays(0, 10);
                        break;
                    }
                case "W30":
                    {
                        this.LoadUpcomingBirthdays(0, 30);
                        break;
                    }
                case "TOD":
                    {
                        this.LoadUpcomingBirthdays(0, 0);
                        break;
                    }
            }
        }
    };
    ContactWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.RunUpcomingBirthdaysFilter();
        this.ReloadUsersQuery();
    };
    ContactWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    ContactWorkspaceComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    ContactWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myCommonDomainService.GetContactsCounts().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.WithoutRemindersCount = myData.WithoutRemindersCount;
                }
            }
        });
    };
    ContactWorkspaceComponent.prototype.LoadUpcomingBirthdays = function (start, end) {
        var _this = this;
        if (this.ContactListService == null) {
            this.ContactListService = new ContactListService_1.ContactListService();
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("UpcomingBirthdaysFilter", start, end, null, "Equals", true, true, false, "number");
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";
        filters.SortBy = "EnglishName";
        filters.GetCount = true;
        filters.GetAll = true;
        this.ContactListService.getByFilters(filters).subscribe(function (myResult) {
            if (myResult == null) {
                _this.UpcomingBirthdaysList = [];
                _this.UpcomingBirthdaysListCount = 0;
                _this.FatherComp.UpcomingCountVisibility = false;
                _this.FatherComp.UpcomingCount = 0;
            }
            else {
                _this.UpcomingBirthdaysList = myResult.Result;
                _this.UpcomingBirthdaysListCount = myResult.Count;
                _this.FatherComp.UpcomingCount = myResult.Count;
                if (myResult.Count != 0) {
                    _this.FatherComp.UpcomingCountVisibility = true;
                }
                else {
                    _this.FatherComp.UpcomingCountVisibility = false;
                }
            }
        });
    };
    ContactWorkspaceComponent.prototype.ViewContactQuery = function (code) {
        var _this = this;
        if (code != null) {
            var objectTableName = "Contact";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";
            switch (code) {
                case "all":
                    {
                        queryCode = "Contacts";
                        displayTitle = "All Contacts";
                        break;
                    }
                case "reminders":
                    {
                        queryCode = "No Reminders";
                        displayTitle = "Contacts Without Reminders";
                        break;
                    }
                //case "upcoming":
                //    {
                //        queryCode = "Upcoming Birthdays";
                //        displayTitle = "Upcoming Birthdays";
                //        break;
                //    }              
                default: {
                    break;
                }
            }
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var listArgs = new Args_1.ListComponentArgs();
            //listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
            ;
        }
    };
    ContactWorkspaceComponent.prototype.EditContact = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Contact', BackButtonLabel: "Contacts" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
            });
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ContactWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    ContactWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContactWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContactWorkspaceComponent);
    return ContactWorkspaceComponent;
}());
exports.ContactWorkspaceComponent = ContactWorkspaceComponent;
//# sourceMappingURL=ContactWorkspaceComponent.js.map