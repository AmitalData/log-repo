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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var AgGridColumnsOperations = /** @class */ (function (_super) {
    __extends(AgGridColumnsOperations, _super);
    function AgGridColumnsOperations() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsAll = false;
        _this.itemSource_Unsaved = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FieldSelectedItem = null;
        return _this;
    }
    AgGridColumnsOperations.prototype.BuildList = function () {
        var _this = this;
        this.ItemsSource = [];
        this.IsAll = this.father.BIReportXMLData.BITabularViewSettings.Columns.filter(function (a) { return !a.IsChecked; })[0] != null ? false : true;
        this.father.BIReportXMLData.BITabularViewSettings.Columns.sort(function (a, b) { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1; }).forEach(function (item) {
            _this.ItemsSource.push(item);
        });
    };
    AgGridColumnsOperations.prototype.SetWindowArgs = function (args) {
        this.father = args.father;
        this.itemSource_Unsaved = JSON.parse(JSON.stringify(this.father.BIReportXMLData.BITabularViewSettings.Columns));
        this.BuildList();
    };
    AgGridColumnsOperations.prototype.SetFieldSelectedItem = function (item) {
        this.FieldSelectedItem = item;
    };
    AgGridColumnsOperations.prototype.CancelButtonClicked = function () {
        this.father.BIReportXMLData.BITabularViewSettings.Columns = this.itemSource_Unsaved;
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    };
    AgGridColumnsOperations.prototype.OkButtonClicked = function () {
        var _this = this;
        var _InfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        var result = new InfrastructureDomainService_1.BIReportXMLData();
        result.BITabularViewSettings = this.father.BIReportXMLData.BITabularViewSettings;
        result.BIReportId = this.father.EntityId;
        result.BIReportPM = this.father.EntityPM;
        _InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.father.BIReportXMLData = myResult.Result;
                _this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
        });
    };
    AgGridColumnsOperations.prototype.btnUp_Click = function () {
        var item = this.FieldSelectedItem;
        if (item != null && item.Code != "All") {
            var currIndex = this.father.BIReportXMLData.BITabularViewSettings.Columns.indexOf(item);
            var nextIndex = currIndex - 1;
            var currItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[currIndex];
            var nextItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[nextIndex];
            if (nextItem) {
                currItem.Index = nextIndex;
                nextItem.Index = currIndex;
                this.BuildList();
            }
        }
    };
    AgGridColumnsOperations.prototype.btnDown_Click = function () {
        var item = this.FieldSelectedItem;
        if (item != null && item.Code != "All") {
            var currIndex = this.father.BIReportXMLData.BITabularViewSettings.Columns.indexOf(item);
            var nextIndex = currIndex + 1;
            var currItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[currIndex];
            var nextItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[nextIndex];
            if (nextItem) {
                currItem.Index = nextIndex;
                nextItem.Index = currIndex;
                this.BuildList();
            }
        }
    };
    AgGridColumnsOperations.prototype.onValueChanged = function (item, index, event) {
        item.IsChecked = event;
        item.Index = index;
        this.IsAll = this.father.BIReportXMLData.BITabularViewSettings.Columns.filter(function (a) { return !a.IsChecked; })[0] != null ? false : true;
    };
    AgGridColumnsOperations.prototype.IsAllClicked = function (event) {
        this.father.BIReportXMLData.BITabularViewSettings.Columns.sort(function (a, b) { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1; }).forEach(function (item) {
            item.IsChecked = event;
        });
    };
    AgGridColumnsOperations = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgGridColumnsOperations.html',
        }),
        __metadata("design:paramtypes", [])
    ], AgGridColumnsOperations);
    return AgGridColumnsOperations;
}(BaseComponent_1.BaseComponent));
exports.AgGridColumnsOperations = AgGridColumnsOperations;
//# sourceMappingURL=AgGridColumnsOperations.js.map