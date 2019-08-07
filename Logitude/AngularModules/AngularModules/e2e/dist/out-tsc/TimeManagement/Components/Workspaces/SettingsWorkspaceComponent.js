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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Args_1 = require("../../../Infrastructure/Args");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ExcelExportService_1 = require("../../../Common/Services/Others/ExcelExportService");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SettingsWorkspaceComponent = /** @class */ (function () {
    function SettingsWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ClockTimeHtmlId = Guid_1.Guid.NewRandomString();
    }
    SettingsWorkspaceComponent.prototype.ClockClicked = function () {
        document.getElementById(this.ClockTimeHtmlId).click();
    };
    SettingsWorkspaceComponent.prototype.UploadClockTimeFile = function (event) {
        var file = UploadLogoFile(this.ClockTimeHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    };
    SettingsWorkspaceComponent.prototype.ArrayBufferToBase64 = function (file, viewModel) {
        if (file) {
            var reader = new FileReader();
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                viewModel.ImportFeatures(window.btoa(binary));
            };
            reader.onerror = function (e) {
                SessionLocator_1.SessionLocator.SelectedSession.StopBusyIndicator();
                var wind = new MessageWindow_1.MessageWindow();
                wind.Show("Error Importing file");
            };
            reader.readAsArrayBuffer(file);
        }
    };
    SettingsWorkspaceComponent.prototype.ImportFeatures = function (data) {
        var _this = this;
        var service = new ExcelExportService_1.ExcelExportService();
        var file = new ImageParameter_1.ImageParameter();
        file.Base64String = data;
        service.ImportClockTimeData(file).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var wind = new MessageWindow_1.MessageWindow();
            wind.Show("Import completed successfully");
        });
    };
    SettingsWorkspaceComponent.prototype.CategoriesClicked = function () {
        var _this = this;
        var displayTitle = "All Categories";
        var code = "All Categories";
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProjectCategory";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    SettingsWorkspaceComponent.prototype.ProjectsClicked = function () {
        var _this = this;
        var displayTitle = "Active Projects";
        var code = "Active Projects";
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProject";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    SettingsWorkspaceComponent.prototype.SprintsClicked = function () {
        var _this = this;
        var displayTitle = "All Sprints";
        var code = "All Sprints";
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "Sprint";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    SettingsWorkspaceComponent.prototype.BudgetClicked = function () {
        var _this = this;
        var displayTitle = "All Budgets";
        var code = "All Budgets";
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMBudget";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Settings";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    SettingsWorkspaceComponent.prototype.GetProjectsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Get Projects";
        logWindow.Show('./TimeManagement/Components/NewEntity/NewGetProjectComponent');
    };
    SettingsWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'SettingsWorkspaceComponent',
            moduleId: module.id,
            templateUrl: './SettingsWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SettingsWorkspaceComponent);
    return SettingsWorkspaceComponent;
}());
exports.SettingsWorkspaceComponent = SettingsWorkspaceComponent;
//# sourceMappingURL=SettingsWorkspaceComponent.js.map