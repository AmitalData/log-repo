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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TimeManagementDomainService_1 = require("../../../Services/TimeManagementDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ExcelExportService_1 = require("../../../../Common/Services/Others/ExcelExportService");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var ProjectsWorkspaceComponent = /** @class */ (function () {
    function ProjectsWorkspaceComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.MyProjectsCount = 0;
        this.AllProjectsCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AllProjectsQueriesVisibility = false;
        this.MyProjectsQueriesVisibility = false;
        this.ClockTimeHtmlId = Guid_1.Guid.NewRandomString();
    }
    ProjectsWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.SetQueriesVisibility();
        this.LoadQueriesCounts();
    };
    ProjectsWorkspaceComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
    };
    ProjectsWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        var myService = new TimeManagementDomainService_1.TimeManagementDomainService();
        myService.GetProjectsCounts(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (myResult) {
            if (myResult != null) {
                _this.MyProjectsCount = myResult.MyProjectsCount > 1000 ? "1000+" : myResult.MyProjectsCount.toString();
                _this.AllProjectsCount = myResult.AllProjectsCount > 1000 ? "1000+" : myResult.AllProjectsCount.toString();
            }
        });
    };
    ProjectsWorkspaceComponent.prototype.SetQueriesVisibility = function () {
        this.AllProjectsQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("TMProject", "TMProject.Q.AllProjects") ? true : false;
        this.MyProjectsQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("TMProject", "TMProject.Q.MyProjects") ? true : false;
    };
    ProjectsWorkspaceComponent.prototype.UploadClockTimeFileClick = function () {
        document.getElementById(this.ClockTimeHtmlId).click();
    };
    ProjectsWorkspaceComponent.prototype.UploadClockTimeFile = function (event) {
        var file = UploadLogoFile(this.ClockTimeHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    };
    ProjectsWorkspaceComponent.prototype.ArrayBufferToBase64 = function (file, viewModel) {
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
    ProjectsWorkspaceComponent.prototype.ImportFeatures = function (data) {
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
    // Commands 
    ProjectsWorkspaceComponent.prototype.RefreshButtonClicked = function () {
    };
    ProjectsWorkspaceComponent.prototype.EditTMProject = function (args) {
    };
    ProjectsWorkspaceComponent.prototype.NewProject = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "New Project";
            logWindow.Show('./TimeManagement/Components/NewEntity/NewProjectComponent');
            logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        });
    };
    ProjectsWorkspaceComponent.prototype.ViewProjectsQuery = function (code) {
        var _this = this;
        var displayTitle = "";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        switch (code) {
            case "All Projects":
                {
                    displayTitle = "All Projects";
                    break;
                }
            case "My Projects":
                {
                    displayTitle = "My Projects";
                    break;
                }
            default: {
                break;
            }
        }
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = code;
        listArgs.ObjectTableName = "TMProject";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Projects";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    ProjectsWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ProjectsWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    ProjectsWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ProjectsWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ProjectsWorkspaceComponent);
    return ProjectsWorkspaceComponent;
}());
exports.ProjectsWorkspaceComponent = ProjectsWorkspaceComponent;
//# sourceMappingURL=ProjectsWorkspaceComponent.js.map