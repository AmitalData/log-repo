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
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CustomizationMainComponent = /** @class */ (function () {
    function CustomizationMainComponent() {
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.IsButtonEnabled = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new GeneralDomainService_1.GeneralDomainService();
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.LoadTableTranslations();
    }
    Object.defineProperty(CustomizationMainComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomizationMainComponent.prototype.SearchTextChanged = function (text) {
        this.SearchText = text;
        this.BuildItemsSources();
    };
    CustomizationMainComponent.prototype.LoadTableTranslations = function () {
        var _this = this;
        this.myService.GetTranslationsByParam("T", "", SessionLocator_1.SessionLocator.TenantPM.Language).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.allTablesItems = myResponse.Result;
                if (_this.allTablesItems != null) {
                    _this.BuildItemsSources();
                }
            }
        });
    };
    CustomizationMainComponent.prototype.BuildItemsSources = function () {
        var _this = this;
        var myTablesItems;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            myTablesItems = this.allTablesItems.filter(function (f) { return f.DefaultText != null && f.DefaultText.toLowerCase().startsWith(_this.SearchText.toLowerCase())
                || f.TranslatedText != null && f.TranslatedText.toLowerCase().startsWith(_this.SearchText.toLowerCase())
                || f.TranslatedTextPlural != null && f.TranslatedTextPlural.toLowerCase().startsWith(_this.SearchText.toLowerCase()); });
        }
        else {
            myTablesItems = this.allTablesItems;
        }
        var tablesList = window.ObjectTables.filter(function (d) { return (d.IsMain && d.EnableSecurity && !d.IsClosed && !d.IsComposition) || d.Name == "Address"; });
        var myData = [];
        myTablesItems.forEach(function (field) {
            var table = tablesList.filter(function (d) { return d.Id == field.ObjectTableID; })[0];
            if (table != null) {
                myData.push(field);
            }
        });
        this.ItemsSource1 = myData.filter(function (f) { return f.ObjectTableTypeCode != "MD"; });
        this.ItemsSource2 = myData.filter(function (f) { return f.ObjectTableTypeCode == "MD"; });
    };
    CustomizationMainComponent.prototype.Selecting = function (item) {
        this.selectedRow = item;
        if (item == null) {
            this.IsButtonEnabled = false;
        }
        else {
            this.IsButtonEnabled = true;
        }
    };
    CustomizationMainComponent.prototype.StandardFieldsClicked = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.selectedRow.ObjectTableID; })[0];
        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Standard Fields: " + _this.selectedRow.DefaultText;
                logWindow.IsFillScreen_115 = true;
                logWindow.WindowArgs = { ObjectTableId: _this.selectedRow.ObjectTableID };
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/StandardFieldsComponent');
            });
        }
    };
    CustomizationMainComponent.prototype.LabelsClicked = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.selectedRow.ObjectTableID; })[0];
        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Object Labels: " + _this.selectedRow.DefaultText;
                logWindow.IsFillScreen = true;
                logWindow.WindowArgs = _this.selectedRow;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/ObjectLabelsComponent');
            });
        }
    };
    CustomizationMainComponent.prototype.ScreensLayoutClicked = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.selectedRow.ObjectTableID; })[0];
        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Screens Layout: " + _this.selectedRow.DefaultText;
                logWindow.IsFillScreen = true;
                logWindow.WindowArgs = { ObjectTableID: _this.selectedRow.ObjectTableID };
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/ScreenLayoutComponent');
            });
        }
    };
    CustomizationMainComponent.prototype.CustomFieldsClicked = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.selectedRow.ObjectTableID; })[0];
        if (table != null) {
            this.entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Custom Fields: " + _this.selectedRow.DefaultText;
                logWindow.IsFillScreen_115 = true;
                logWindow.WindowArgs = { ObjectTableId: table.Id, ObjectTableName: table.Name }; //this.selectedRow.ObjectTableID;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomFieldsComponent');
            });
        }
    };
    CustomizationMainComponent.prototype.RulesClicked = function () {
        var _this = this;
        //RulesMainComponent
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.selectedRow.ObjectTableID; })[0];
        if (table != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Object Rules: " + this.selectedRow.DefaultText;
            logWindow.IsFillScreen = true;
            logWindow.WindowArgs = this.selectedRow;
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/RulesMainComponent');
        }
    };
    CustomizationMainComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomizationMainComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomizationMainComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomizationMainComponent);
    return CustomizationMainComponent;
}());
exports.CustomizationMainComponent = CustomizationMainComponent;
//# sourceMappingURL=CustomizationMainComponent.js.map