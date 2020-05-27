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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ComponentArgs");
var ParameterComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ParameterComponentArgs");
var UserExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var UserSearchComponent = /** @class */ (function () {
    function UserSearchComponent(_entityListService, cd) {
        var _this = this;
        this._entityListService = _entityListService;
        this.cd = cd;
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this.ComponentName = "Users";
        //OnCloseSendToContactsEvent = new EventEmitter();
        this.searchFields = "";
        this.ObjectTableName = "User";
        this.items = [];
        this.PartnersObslist = [];
        this.IsSearchIconVisible = true;
        this.SearchFieldchangeevent = new core_1.EventEmitter();
        this.Componentkey = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Descending",
            sortingCol: "IsTwoFactorAuthenticationEnabled",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.userExtendedPMService = new UserExtendedPMService_1.UserExtendedPMService();
        this.ToEmailLists = [];
        window.ToEmailLists = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid_1.Guid.newGuid();
        }
        this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res && res.IsCheck)
                _this.RefreshEmailList(res);
        });
        this.BuildColumns();
        this.Run();
    }
    UserSearchComponent.prototype.Run = function () {
        var _this = this;
        this.userExtendedPMService.GetUsersTwoFactorAuthenticationEnabled(SessionLocator_1.SessionLocator.Tenant).subscribe(function (resp) {
            //this.PartnersObslist.push(new EntityPartner("All", "", false));
            _this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            _this.filterAgrs.SortBy = "IsTwoFactorAuthenticationEnabled";
            _this.filterAgrs.SortDirection = "Descending";
            _this.onQueryChangeEvent.emit({ QueryId: "", Filters: _this.filterAgrs });
            ComponentArgs_1.ComponentArgs.AddComponent(new ParameterComponentArgs_1.ParameterComponentArgs(_this.CurrentSession.Sessionkey + "SendTo", _this));
            // To  Email
            if (resp.Result) {
                resp.Result.forEach(function (item) {
                    if (item) {
                        _this.ToEmailLists.push(item.toLowerCase());
                    }
                });
                window.ToEmailLists = _this.ToEmailLists;
            }
            //if (this.ToEmail) {
            //    this.ToEmail.split(';').forEach((item) => {
            //        if (item) {
            //            this.ToEmailLists.push(item.toLowerCase());
            //        }
            //    });
            //    window.ToEmailLists = this.ToEmailLists;
            //}
        });
    };
    UserSearchComponent.prototype.ngOnInit = function () {
    };
    UserSearchComponent.prototype.SelectionChanged = function (item) {
        this.SelectedPartnerItem = item;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
        console.log(this.myPartnerId);
        this.onQueryChangeEvent.emit({ QueryId: "", Filters: this.filterAgrs });
    };
    UserSearchComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "SelectedUser",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });
        this.columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '180px' },
        });
        this.columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '180px' },
        });
        //this.columns.push({
        //    FieldName: "Company",
        //    DataTypeCode: 'String',
        //    IsCustomTemplate: true,
        //    Display: 'Company',
        //    Styles: { width: '150px' },
        //});
        this.columns.push({
            FieldName: "Notes",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Notes',
            Styles: { width: '140px' },
        });
    };
    UserSearchComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        //if (filters == null) {
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        //}
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = "IsTwoFactorAuthenticationEnabled";
        filters.SortDirection = "Descending";
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;
        if (!Tools_1.AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        return this._entityListService.getByFilters(rowsObjectTable, filters);
    };
    UserSearchComponent.prototype.RefreshEmailList = function (res) {
        var item = null;
        var index = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            if (ComponentArgs_1.ComponentArgs && ComponentArgs_1.ComponentArgs.ComponentLists) {
                var sessionkey = this.CurrentSession.Sessionkey + "SendTo";
                var Component = ComponentArgs_1.ComponentArgs.ComponentLists.filter(function (d) { return d.key == sessionkey; })[0];
                if (Component) {
                    var myComponent = Component.Component;
                    if (myComponent) {
                        if (res.FieldName == "SelectedUser") {
                            item = myComponent.ToEmailLists.filter(function (d) { return d.toLowerCase() == res.UserId.toLowerCase(); })[0];
                            if (item) {
                                index = myComponent.ToEmailLists.indexOf(res.UserId.toLowerCase());
                                if (index != -1)
                                    myComponent.ToEmailLists.splice(index, 1);
                            }
                            else
                                myComponent.ToEmailLists.push(res.UserId.toLowerCase());
                            if (myComponent.ToEmailLists.length > 10) {
                                myComponent.IsShowMessageCountTo = true;
                            }
                            else
                                myComponent.IsShowMessageCountTo = false;
                            window.ToEmailLists = myComponent.ToEmailLists;
                        }
                        if (myComponent.IsShowMessageCountTo) {
                            myComponent.IsShowMessageCount = true;
                        }
                        else
                            myComponent.IsShowMessageCount = false;
                    }
                }
            }
        }
        res.IsCheck = false;
    };
    UserSearchComponent.prototype.onSearchTextChangeEvent = function (searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    UserSearchComponent.prototype.DeleteEmail = function (fieldName, email) {
        if (fieldName == "SelectedUser")
            window.ToEmailLists = this.ToEmailLists = this.ToEmailLists.filter(function (d) { return d.toLowerCase() != email.toLowerCase(); });
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    UserSearchComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UserSearchComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        //this.OnCloseSendToContactsEvent.emit(this);
        if (!Tools_1.AppTool.IsNullOrEmpty(window.ToEmailLists) && window.ToEmailLists.length > 0) {
            this.userExtendedPMService.PostUpdateTwoFactorAuthenticationEnabled(SessionLocator_1.SessionLocator.Tenant, window.ToEmailLists).subscribe(function (res) {
                _this.CloseButtonClicked();
            });
        }
        else {
            this.ValidationErrorsList.push("Please define at least one user to be enabled for two factor authentication");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], UserSearchComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], UserSearchComponent.prototype, "SearchFieldchangeevent", void 0);
    UserSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserSearchComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, core_1.ChangeDetectorRef])
    ], UserSearchComponent);
    return UserSearchComponent;
}());
exports.UserSearchComponent = UserSearchComponent;
//# sourceMappingURL=UserSearchComponent.js.map