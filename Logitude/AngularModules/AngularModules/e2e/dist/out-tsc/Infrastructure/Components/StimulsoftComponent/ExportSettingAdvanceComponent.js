"use strict";
/// <reference path="stimulsoftviewercomponent.ts" />
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ExportSettingAdvanceComponent = /** @class */ (function () {
    function ExportSettingAdvanceComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.exportDataOnly = false;
        this.exportObjectFormatting = false;
        this.useOnePageHeaderandFooter = false;
    }
    ExportSettingAdvanceComponent.prototype.ngOnInit = function () {
    };
    ExportSettingAdvanceComponent.prototype.SetWindowArgs = function (args) {
        this.StimulsoftViewerComponent = args.StimulsoftViewerComponent;
        if (this.StimulsoftViewerComponent != null) {
            this.ExportDataOnly = this.StimulsoftViewerComponent.ExportDataOnly;
            this.ExportObjectFormatting = this.StimulsoftViewerComponent.ExportObjectFormatting;
            //this.UseOnePageHeaderandFooter = this.StimulsoftViewerComponent.UseOnePageHeaderandFooter;
        }
        this.UseOnePageHeaderandFooter = !this.ExportDataOnly;
    };
    Object.defineProperty(ExportSettingAdvanceComponent.prototype, "ExportDataOnly", {
        get: function () {
            return this.exportDataOnly;
        },
        set: function (newValue) {
            if (this.exportDataOnly != newValue) {
                this.exportDataOnly = newValue;
                if (this.exportDataOnly) {
                    this.ExportObjectFormatting = false;
                    this.UseOnePageHeaderandFooter = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportSettingAdvanceComponent.prototype, "ExportObjectFormatting", {
        get: function () {
            return this.exportObjectFormatting;
        },
        set: function (newValue) {
            if (this.exportObjectFormatting != newValue) {
                this.exportObjectFormatting = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportSettingAdvanceComponent.prototype, "UseOnePageHeaderandFooter", {
        get: function () {
            return this.useOnePageHeaderandFooter;
        },
        set: function (newValue) {
            if (this.useOnePageHeaderandFooter != newValue) {
                this.useOnePageHeaderandFooter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ExportSettingAdvanceComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportSettingAdvanceComponent.prototype.SaveButtonClicked = function () {
        if (this.StimulsoftViewerComponent != null) {
            this.StimulsoftViewerComponent.SaveToExcelFileAdvanced(this.ExportDataOnly, this.ExportObjectFormatting, this.UseOnePageHeaderandFooter);
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportSettingAdvanceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ExportSettingAdvanceComponent',
            templateUrl: './ExportSettingAdvanceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExportSettingAdvanceComponent);
    return ExportSettingAdvanceComponent;
}());
exports.ExportSettingAdvanceComponent = ExportSettingAdvanceComponent;
//# sourceMappingURL=ExportSettingAdvanceComponent.js.map