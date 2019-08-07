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
var DocumentTypePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var DocumentPermissiosViewModel_1 = require("./ViewModel/DocumentPermissiosViewModel");
var SharedLogisticsDocumentPermissiosComponent = /** @class */ (function () {
    function SharedLogisticsDocumentPermissiosComponent(_documentTypePMExtendedService) {
        this._documentTypePMExtendedService = _documentTypePMExtendedService;
        this.OnCloseWindowEvent = new core_1.EventEmitter();
        this.FullComponentsVisibility = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    SharedLogisticsDocumentPermissiosComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.OnCloseWindowEvent.subscribe(function ($event) {
            _this.SaveButtonClicked();
        });
        this.Run();
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.Run = function () {
        this.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0].Id;
        this.LoadTenantZeroDate();
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.LoadTenantZeroDate = function () {
        var _this = this;
        this.myTenantZeroList = [];
        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, 0).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.myTenantZeroList = pmResponse.Result;
                _this.LoadTenantData();
            }
            else
                _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.LoadTenantData = function () {
        var _this = this;
        this.myTenantList = [];
        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.myTenantList = pmResponse.Result;
                _this.BuildData();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.BuildData = function () {
        var _this = this;
        var myList = [];
        this.DocumentPermissiosLists = [];
        if (!this.mySearchText) {
            myList = this.myTenantList;
        }
        else {
            myList = this.myTenantList.filter(function (d) { return (d.Code && d.Code.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1) || (d.Name && d.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1); });
        }
        myList = this.SortItemSource(myList);
        myList.forEach(function (item) {
            var tenantZeroItem = _this.myTenantZeroList.filter(function (t) { return t.Code == item.Code; })[0];
            if (tenantZeroItem != null) {
                _this.DocumentPermissiosLists.push(new DocumentPermissiosViewModel_1.DocumentPermissiosViewModel(tenantZeroItem, item));
            }
        });
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.SortItemSource = function (items) {
        items.sort(function (a, b) {
            if (a.Name && a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name && a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
        return items;
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myTenantList = [];
        this.DocumentPermissiosLists.forEach(function (item) {
            if (item.entityPM.IsDirty) {
                _this.myTenantList.push(item.entityPM);
            }
        });
        if (this.myTenantList.length > 0) {
            this._documentTypePMExtendedService.update(this.myTenantList).subscribe(function (res) {
                _this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.onSearchTextChangeEvent = function (searchText) {
        if (!searchText)
            searchText = "";
        this.mySearchText = searchText;
        this.BuildData();
    };
    SharedLogisticsDocumentPermissiosComponent.prototype.SetWindowArgs = function (args) {
        this.FullComponentsVisibility = true;
    };
    SharedLogisticsDocumentPermissiosComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticsDocumentPermissios',
            templateUrl: './SharedLogisticsDocumentPermissiosComponent.html',
            inputs: ['OnCloseWindowEvent'],
            providers: [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService],
        }),
        __metadata("design:paramtypes", [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService])
    ], SharedLogisticsDocumentPermissiosComponent);
    return SharedLogisticsDocumentPermissiosComponent;
}());
exports.SharedLogisticsDocumentPermissiosComponent = SharedLogisticsDocumentPermissiosComponent;
//# sourceMappingURL=SharedLogisticsDocumentPermissiosComponent.js.map