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
var SharedLogisticsService_1 = require("../Services/Others/SharedLogisticsService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var ActivityItemDetailsViewModel_1 = require("./ViewModel/ActivityItemDetailsViewModel");
var ActivityZoomItemViewModel_1 = require("./ViewModel/ActivityZoomItemViewModel");
var ActivityZoomComponent = /** @class */ (function () {
    function ActivityZoomComponent(_sharedLogisticsService) {
        this._sharedLogisticsService = _sharedLogisticsService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
    }
    ActivityZoomComponent.prototype.ngOnInit = function () {
        this.Run();
    };
    ActivityZoomComponent.prototype.Run = function () {
        this.BuildFilters();
    };
    ActivityZoomComponent.prototype.BuildFilters = function () {
        var _this = this;
        this.ComboList = [];
        this.ComboList.push(new CodeNameClass("T", "Today"));
        this.ComboList.push(new CodeNameClass("W", "Last 7 Days"));
        this.ComboList.push(new CodeNameClass("M", "Last Month"));
        this.ComboListSelectedItem = this.ComboList.filter(function (d) { return d.Code == _this.DateParameter; })[0];
        this.GetZoomDetails();
    };
    ActivityZoomComponent.prototype.ComboListSelectedValueChanged = function (item) {
        if (this.ComboListSelectedItem != item) {
            this.ComboListSelectedItem = item;
            this.DateParameter = this.ComboListSelectedItem.Code;
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
            this.ActivityZoomSelectedItemViewModel = null;
            this.GetZoomDetails();
        }
    };
    //GetZoomDetails
    ActivityZoomComponent.prototype.GetZoomDetails = function () {
        var _this = this;
        this.ActivityList = [];
        this.ActivityDetailsList = [];
        this._sharedLogisticsService.getCardLogDetails(this.PartnerTypeId, this.DateParameter, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        _this.ActivityList.push(new ActivityZoomItemViewModel_1.ActivityZoomItemViewModel(item));
                    });
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (_this.ActivityList && _this.ActivityList.length > 0) {
                        _this.GetActivityDetailsList(_this.ActivityList[0]);
                    }
                }
            }
            else
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    //GetActivityDetailsList
    ActivityZoomComponent.prototype.GetActivityDetailsList = function (item) {
        var _this = this;
        if (this.ActivityZoomSelectedItemViewModel != item) {
            this.ActivityZoomSelectedItemViewModel = item;
            if (this.ActivityZoomSelectedItemViewModel) {
                this.ActivityDetailsList = [];
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                this._sharedLogisticsService.getCardLogActivityDetailsList(item.LogDetails.CardId, item.LogDetails.ContactId, this.PartnerTypeId, this.DateParameter, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            myResult.forEach(function (item) {
                                _this.ActivityDetailsList.push(new ActivityItemDetailsViewModel_1.ActivityItemDetailsViewModel(item, _this.ActivityZoomSelectedItemViewModel));
                            });
                        }
                    }
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
            }
        }
    };
    ActivityZoomComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ActivityZoomComponent.prototype.SetWindowArgs = function (args) {
        this.PartnerTypeId = args.PartnerTypeId;
        this.DateParameter = args.DateParameter;
        this.DataContext = args.DataContext;
    };
    ActivityZoomComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ActivityZoomControl',
            templateUrl: './ActivityZoomComponent.html',
            inputs: ['PartnerTypeId', 'DateParameter', 'DataContext', 'OnCloseWindowEvent'],
            providers: [SharedLogisticsService_1.SharedLogisticsService],
        }),
        __metadata("design:paramtypes", [SharedLogisticsService_1.SharedLogisticsService])
    ], ActivityZoomComponent);
    return ActivityZoomComponent;
}());
exports.ActivityZoomComponent = ActivityZoomComponent;
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass(code, name) {
        this.Code = code;
        this.Name = name;
    }
    return CodeNameClass;
}());
//# sourceMappingURL=ActivityZoomComponent.js.map