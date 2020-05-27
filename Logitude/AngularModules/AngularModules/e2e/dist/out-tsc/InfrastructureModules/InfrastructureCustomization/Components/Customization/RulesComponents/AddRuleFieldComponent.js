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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AddRuleFieldComponent = /** @class */ (function (_super) {
    __extends(AddRuleFieldComponent, _super);
    function AddRuleFieldComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectFields = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.HideLov = false;
        return _this;
    }
    AddRuleFieldComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.RuleFieldTXTAreaId = Guid_1.Guid.newGuid();
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && !Tools_1.AppTool.IsNullOrEmpty(d.PMPropertyPath) && !d.DisplayOnly && !d.IsMulti && !d.IsCustomFilter; });
        this.ObjectFields = this.ObjectFields.sort(function (a, b) { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1; });
        this.FieldsLovQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.FieldsLovQueryFilters.Tenant = 0;
        this.FieldsLovQueryFilters.addAdditionalFilter("ObjectTableId", this.ObjectTableId, null, null, "Equals", false, false, false, "string");
    };
    AddRuleFieldComponent.prototype.onAddFieldTagClicked = function () {
        this.ViewDataField(this.ObjectTable.Id);
    };
    AddRuleFieldComponent.prototype.ViewDataField = function (tableId) {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTableId = tableId;
        windowArgs.ObjectTypeField = "";
        windowArgs.HideSystemDataTab = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.Expression = insertAtSubject(_this.RuleFieldTXTAreaId, $event);
            }
        });
    };
    AddRuleFieldComponent.prototype.onSelectFieldTagClicked = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.ObjectTableId = this.ObjectTable.Id;
        windowArgs.ObjectTypeField = "";
        windowArgs.HideSystemDataTab = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.SelectedObjectFieldId = $event;
            }
        });
    };
    AddRuleFieldComponent.prototype.onSelectedItemChanged = function (item) {
        var _this = this;
        this.Expression = null;
        this.HideLov = true;
        if (item) {
            this.SelectedObjectField = this.ObjectFields.filter(function (f) { return f.Id == item.Id; })[0];
            if (item && this.SelectedObjectFieldId != item.Id) {
                this.SelectedObjectFieldId = item.Id;
                item;
            }
        }
        setTimeout(function () {
            _this.HideLov = false;
        }, 300);
    };
    AddRuleFieldComponent.prototype.SelectedLOVTextChange = function ($event) {
        this.Expression = $event;
    };
    AddRuleFieldComponent.prototype.OkButtonClicked = function () {
        if (this.SelectedObjectFieldId) {
            if (this.Expression) {
                this.CurrentSession.CurrentWindow.Close(this.SelectedObjectFieldId + ',' + this.Expression);
            }
            else {
                this.CurrentSession.CurrentWindow.Close(this.SelectedObjectFieldId);
            }
        }
    };
    AddRuleFieldComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddRuleFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddRuleFieldComponent',
            templateUrl: './AddRuleFieldComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddRuleFieldComponent);
    return AddRuleFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.AddRuleFieldComponent = AddRuleFieldComponent;
//# sourceMappingURL=AddRuleFieldComponent.js.map