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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var Args_1 = require("../../Args");
var ApiQueryFilters_1 = require("../../DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var UsersQueryList = /** @class */ (function () {
    function UsersQueryList() {
        this.BackCompletedEvent = new core_1.EventEmitter();
        this.ShowNoViews = false;
        // public SearchTextValue: Control;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    UsersQueryList.prototype.ngOnInit = function () {
        var _this = this;
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.UserQueries = window.Queries.filter(function (x) { return x.ObjectTableId === ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant; });
        if (this.ReloadUserQueries) {
            this.ReloadUserQueries.subscribe(function (res) {
                _this.UserQueries = window.Queries.filter(function (x) { return x.ObjectTableId === ObjectTable.Id && x.UserId != null && x.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant; });
            });
        }
    };
    UsersQueryList.prototype.ViewQuery = function (myQueryCode, NameTextCodeCode) {
        var _this = this;
        if (myQueryCode != null) {
            var objectTableName = "";
            var queryCode = "";
            var MethodName = null;
            var displayTitle = "";
            var backButtonTitle = this.BackButtonTitle;
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = this.ObjectTableName;
            listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(NameTextCodeCode);
            listArgs.BackButtonTitle = this.BackButtonTitle != "" && this.BackButtonTitle != null ? this.BackButtonTitle : "Back";
            listArgs.MethodName = MethodName;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.BackCompletedEvent.emit("Completed"); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], UsersQueryList.prototype, "BackCompletedEvent", void 0);
    UsersQueryList = __decorate([
        core_1.Component({
            selector: 'UsersQueryList',
            template: "<div *ngIf=\"UserQueries.length > 0\">\n                \n                <div>\n                    <div class=\"ScrollContent\">\n                        <p class=\"QueryLink\" *ngFor=\"let Query of UserQueries\" (click)=\"ViewQuery(Query.Code,Query.NameTextCodeCode)\" >{{Query.NameTextCodeCode | TextCodeTranslationPipe}}</p>\n                    </div>\n                </div>                \n               </div>\n               <label class=\"QueryLink\" *ngIf=\"ShowNoViews && UserQueries.length == 0\" style=\"vertical-align:top;font-family:Arial; font-size:11px;color:gray;cursor: default;\"> No views </label>\n              ",
            inputs: ['ObjectTableName', 'BackButtonTitle', 'ReloadUserQueries', 'ShowNoViews'],
        }),
        __metadata("design:paramtypes", [])
    ], UsersQueryList);
    return UsersQueryList;
}());
exports.UsersQueryList = UsersQueryList;
//# sourceMappingURL=UsersQueryList.js.map