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
var BIReportPM_1 = require("../../../../Infrastructure/EntityPMs/BIReportPM");
var BIReportPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/BIReportPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NewBIReport = /** @class */ (function (_super) {
    __extends(NewBIReport, _super);
    function NewBIReport() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.ObjectTableName = "BIReport";
        _this.IsNewQuery = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BIReportPM_1.BIReportPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM.CreateDate = todayDate;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.UpdateDate = todayDate;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.TypeCode = "EXL";
        _this.myService = new BIReportPMService_1.BIReportPMService();
        _this.SetUIProperties();
        return _this;
    }
    NewBIReport.prototype.SetWindowArgs = function (args) {
        this.DWQueryId = args.DWQueryId;
        this.EntityPM.BIReportFolderId = args.FolderId;
        this.SetUIProperties();
    };
    NewBIReport.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DWQueryId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DWQueryId));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DWQueryId)) {
            this.IsNewQuery = false;
        }
        else {
            this.IsNewQuery = true;
        }
    };
    Object.defineProperty(NewBIReport.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReport.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReport.prototype, "BIReportFolderId", {
        get: function () { return this.EntityPM.BIReportFolderId; },
        set: function (newValue) {
            if (this.EntityPM.BIReportFolderId != newValue) {
                this.EntityPM.BIReportFolderId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReport.prototype, "DWQueryId", {
        get: function () { return this.EntityPM.DWQueryId; },
        set: function (newValue) {
            if (this.EntityPM.DWQueryId != newValue) {
                this.EntityPM.DWQueryId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReport.prototype, "TypeCode", {
        get: function () { return this.EntityPM.TypeCode; },
        set: function (newValue) {
            if (this.EntityPM.TypeCode != newValue) {
                this.EntityPM.TypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewBIReport.prototype.ShowQueryBuilderClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.DWQueryId = this.DWQueryId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (s != null) {
                    _this.EntityPM.DWQueryId = s.QID;
                    _this.SetUIProperties();
                }
            });
        });
    };
    NewBIReport.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    };
    NewBIReport.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name Field is Required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BIReportFolderId)) {
            this.ValidationErrorsList.push("Folder Field is Required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var windowArgs = {};
            windowArgs.DWQueryId = this.DWQueryId;
            windowArgs.IsBIReportWorkspace = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 1200;
            logWindow.Height = 820;
            logWindow.Title = "Query Builder";
            logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    if (s != null) {
                        _this.EntityPM.DWQueryId = s.QID;
                        if (_this.ValidationErrorsList.length == 0) {
                            _this.CurrentSession.StartBusyIndicatorSaving();
                            _this.myService.insert(_this.EntityPM).subscribe(function (myResponse) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (myResponse.HasError) {
                                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                                }
                                else {
                                    _this.CurrentSession.CloseCurrentWindow();
                                    if (d != "cancel") {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", _this.CurrentSession.SessionLocation.viewContainerRef)
                                            .then(function (cmpRef) {
                                            cmpRef.instance.ComponentRef = cmpRef;
                                            cmpRef.instance.Run({
                                                DWQueryId: s.QID,
                                                ObjectTableName: 'BIReport',
                                                EntityId: _this.EntityPM.Id,
                                            });
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            });
        }
    };
    NewBIReport = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewBIReport.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewBIReport);
    return NewBIReport;
}(BaseComponent_1.BaseComponent));
exports.NewBIReport = NewBIReport;
//# sourceMappingURL=NewBIReport.js.map