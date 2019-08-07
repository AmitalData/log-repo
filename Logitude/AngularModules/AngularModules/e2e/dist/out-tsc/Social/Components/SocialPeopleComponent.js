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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../Infrastructure/Services/EntityListService");
var PostsArgs_1 = require("../../Infrastructure/DataContracts/PostsArgs");
var Tools_1 = require("../../Infrastructure/Tools");
var SocialPeopleComponent = /** @class */ (function () {
    function SocialPeopleComponent(_entityListService) {
        var _this = this;
        this._entityListService = _entityListService;
        this.ObjectTableName = "User";
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this.SocialContactLinkEvent = null;
        this.items = [];
        this.SearchFieldchangeevent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchFields = "";
        this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
    }
    SocialPeopleComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
        this.Listen();
    };
    SocialPeopleComponent.prototype.SetWindowArgs = function (args) {
    };
    SocialPeopleComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SocialContactLinkEvent) {
            this.SocialContactLinkEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s) {
                    if (s[0] == "SocialContactLinkEvent") {
                        _this.ViewPostUserFeedsButtonClick(s[1]);
                    }
                }
            });
        }
    };
    SocialPeopleComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'EnglishName',
            Styles: { width: '400px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Social/Components/QueryColumnsComponents/SocialContactNameLink',
        });
        this.columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '400px' },
        });
        this.columns.push({
            //FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: '',
            Styles: { width: '100px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Social/Components/QueryColumnsComponents/SocialPeopleFollowComponent',
        });
    };
    SocialPeopleComponent.prototype.ViewPostUserFeedsButtonClick = function (user) {
        var postsArgs = new PostsArgs_1.PostsArgs();
        postsArgs.QueryName = "UserPosts";
        postsArgs.SubQueryName = "All";
        postsArgs.UserId = user.Id;
        postsArgs.ScreenCode = "UserPostsControl";
        postsArgs.IsUserMode = true;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Social/Components/SocialPostsComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.InitializePostComponent(postsArgs);
        });
    };
    SocialPeopleComponent.prototype.ngOnDestroy = function () {
        if (this.SocialContactLinkEvent) {
            this.SocialContactLinkEvent.unsubscribe();
            this.SocialContactLinkEvent = null;
        }
    };
    SocialPeopleComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;
        if (!Tools_1.AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("HasEmail", "", null, null, "NotEqual", true, false, false, "String");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("Id", SessionLocator_1.SessionLocator.LoggedUserId, null, null, "NotEqual", false, false, false, "String");
        return this._entityListService.getByFilters(rowsObjectTable, filters);
    };
    SocialPeopleComponent.prototype.onSearchTextChangeEvent = function (searchtext) {
        if (searchtext != null && searchtext != undefined) {
            this.searchFields = searchtext;
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.SearchFieldchangeevent.emit(this.searchFields);
        }
        else {
            this.SearchFieldchangeevent.emit("");
        }
    };
    SocialPeopleComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SocialPeopleComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SocialPeopleComponent.prototype, "SearchFieldchangeevent", void 0);
    SocialPeopleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialPeopleComponent',
            templateUrl: './SocialPeopleComponent.html',
            providers: [EntityListService_1.EntityListService],
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], SocialPeopleComponent);
    return SocialPeopleComponent;
}());
exports.SocialPeopleComponent = SocialPeopleComponent;
//# sourceMappingURL=SocialPeopleComponent.js.map