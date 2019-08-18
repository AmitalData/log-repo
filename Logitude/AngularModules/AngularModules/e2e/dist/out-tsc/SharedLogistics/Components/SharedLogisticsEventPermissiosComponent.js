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
var EventTypeExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService");
var EventPermissiosViewModel_1 = require("./ViewModel/EventPermissiosViewModel");
var SharedLogisticsEventPermissiosComponent = /** @class */ (function () {
    function SharedLogisticsEventPermissiosComponent(_eventTypeExtendedPMService) {
        this._eventTypeExtendedPMService = _eventTypeExtendedPMService;
        this.OnCloseWindowEvent = new core_1.EventEmitter();
        this.FullComponentsVisibility = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    SharedLogisticsEventPermissiosComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.OnCloseWindowEvent.subscribe(function ($event) {
            _this.SaveButtonClicked();
        });
        this.Run();
    };
    SharedLogisticsEventPermissiosComponent.prototype.Run = function () {
        this.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0].Id;
        this.LoadTenantZeroDate();
    };
    SharedLogisticsEventPermissiosComponent.prototype.LoadTenantZeroDate = function () {
        var _this = this;
        this.myTenantZeroList = [];
        this._eventTypeExtendedPMService.GetEventTypesByObjectTable(this.ObjectTableId, 0).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.myTenantZeroList = pmResponse.Result;
                _this.LoadTenantData();
            }
            else
                _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedLogisticsEventPermissiosComponent.prototype.LoadTenantData = function () {
        var _this = this;
        this.myTenantList = [];
        this._eventTypeExtendedPMService.GetEventTypesByObjectTable(this.ObjectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.myTenantList = pmResponse.Result;
                _this.BuildData();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedLogisticsEventPermissiosComponent.prototype.BuildData = function () {
        var _this = this;
        var myList = [];
        this.EventPermissiosLists = [];
        if (!this.mySearchText) {
            myList = this.myTenantList;
        }
        else {
            myList = this.myTenantList.filter(function (d) { return (d.Code && d.Code.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1) || (d.LocalName && d.LocalName.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1); });
        }
        myList = this.SortItemSource(myList);
        myList.forEach(function (item) {
            var tenantZeroItem = _this.myTenantZeroList.filter(function (t) { return t.Code == item.Code && t.IsSharedLogisticsEnabled; })[0];
            if (tenantZeroItem != null) {
                _this.EventPermissiosLists.push(new EventPermissiosViewModel_1.EventPermissiosViewModel(tenantZeroItem, item));
            }
            else {
                if (item.AddedManually) {
                    _this.EventPermissiosLists.push(new EventPermissiosViewModel_1.EventPermissiosViewModel(null, item));
                }
            }
        });
    };
    SharedLogisticsEventPermissiosComponent.prototype.SortItemSource = function (items) {
        items.sort(function (a, b) {
            if (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) {
                return -1;
            }
            else if (a.EnglishName.toLowerCase() > b.EnglishName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
        return items;
    };
    SharedLogisticsEventPermissiosComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedLogisticsEventPermissiosComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myTenantList = [];
        this.EventPermissiosLists.forEach(function (item) {
            if (item.entityPM.IsDirty) {
                _this.myTenantList.push(item.entityPM);
            }
        });
        if (this.myTenantList.length > 0) {
            this._eventTypeExtendedPMService.update(this.myTenantList).subscribe(function (res) {
                _this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }
    };
    SharedLogisticsEventPermissiosComponent.prototype.onSearchTextChangeEvent = function (searchText) {
        if (!searchText)
            searchText = "";
        this.mySearchText = searchText;
        this.BuildData();
    };
    SharedLogisticsEventPermissiosComponent.prototype.SetWindowArgs = function (args) {
        this.FullComponentsVisibility = true;
    };
    SharedLogisticsEventPermissiosComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticsEventPermissios',
            templateUrl: './SharedLogisticsEventPermissiosComponent.html',
            inputs: ['OnCloseWindowEvent'],
            providers: [EventTypeExtendedPMService_1.EventTypeExtendedPMService],
        }),
        __metadata("design:paramtypes", [EventTypeExtendedPMService_1.EventTypeExtendedPMService])
    ], SharedLogisticsEventPermissiosComponent);
    return SharedLogisticsEventPermissiosComponent;
}());
exports.SharedLogisticsEventPermissiosComponent = SharedLogisticsEventPermissiosComponent;
//# sourceMappingURL=SharedLogisticsEventPermissiosComponent.js.map