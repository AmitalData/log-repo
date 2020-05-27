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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AutomationItemViewModel_1 = require("./ViewModel/AutomationItemViewModel");
var AutomationExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService");
var AutomationPMService_1 = require("../../../../Common/Services/StandardPMs/AutomationPMService");
var AutomationPMExtended_1 = require("../../../../Common/EntityPMs/AutomationPMExtended");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AutomationArgs_1 = require("../../../../Infrastructure/DataContracts/AutomationArgs");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AutomationsSettingsComponent = /** @class */ (function () {
    function AutomationsSettingsComponent(_automationExtendedPMService, _automationPMService) {
        this._automationExtendedPMService = _automationExtendedPMService;
        this._automationPMService = _automationPMService;
        this.IsShowTabUpdate = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.SelectedTabCode = "";
        this.AutomationList = [];
        this.ScheduleAutomationList = [];
        this.OnUpdateAutomationList = [];
        this.OnCreateAutomationList = [];
        this.IsAddAtomationEnable = false;
        this.OnUpdateTabVisibility = false;
        this.ScheduleTabVisibility = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AutomationsSettingsComponent.prototype.ngOnInit = function () {
    };
    AutomationsSettingsComponent.prototype.SetDataContext = function (tableName) {
        var _this = this;
        this.AutomationList = [];
        if (tableName) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == tableName; })[0];
            if (table) {
                this.ObjectTableName = table.Name;
                this.ObjectTableId = table.Id;
                this.LoadAutomationsList();
            }
            this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
                if (tableName == "Master") {
                    _this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe(function (response) {
                        _this.Start();
                    });
                }
                else
                    _this.Start();
            });
        }
    };
    AutomationsSettingsComponent.prototype.Start = function () {
        this.ShowIncludeInactiveOnCreateCheckBoxKey = Guid_1.Guid.newGuid();
        this.ShowIncludeInactiveOnUpDateCheckBoxKey = Guid_1.Guid.newGuid();
        this.SelectedTabCode = "CRA";
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Automation", "NEW")) {
            this.IsAddAtomationEnable = false;
        }
        else {
            this.IsAddAtomationEnable = true;
        }
        if (this.ObjectTableName != "LogitudeMessagesTransmissionLog") {
            this.ScheduleTabVisibility = true;
            this.OnUpdateTabVisibility = true;
            this.IsShowTabUpdate = true;
        }
    };
    AutomationsSettingsComponent.prototype.LoadAutomationsList = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.AutomationList = [];
        this._automationExtendedPMService.getAutomationesByObjectTableId(this.ObjectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach(function (automationes) {
                    var automationItemViewModel = new AutomationItemViewModel_1.AutomationItemViewModel(automationes);
                    _this.AutomationList.push(automationItemViewModel);
                });
                _this.RefreshAutomationList("OnCreate");
                _this.RefreshAutomationList("OnUpdate");
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    AutomationsSettingsComponent.prototype.AutomationUpdateListChangeSelected = function (item) {
        this.OnUpdateAutomationListSelected = item;
        this.OnUpdateAutomationList.forEach(function (automation) {
            automation.IsShowArrowUpDown = false;
        });
        this.OnUpdateAutomationListSelected.IsShowArrowUpDown = true;
    };
    AutomationsSettingsComponent.prototype.AutomationOnCreateListChangeSelected = function (item) {
        this.OnCreateAutomationListSelected = item;
        this.OnCreateAutomationList.forEach(function (automation) {
            automation.IsShowArrowUpDown = false;
        });
        this.OnCreateAutomationListSelected.IsShowArrowUpDown = true;
    };
    AutomationsSettingsComponent.prototype.CheckboxInCludeInActiveClick = function (type) {
        if (type == 'OnCreate') {
            this.IsInCludeInActiveOnCreateCheckBox = !this.IsInCludeInActiveOnCreateCheckBox;
            this.RefreshAutomationList("OnCreate");
        }
        else if (type == 'OnUpdate') {
            this.IsInCludeInActiveOnUpdateCheckBox = !this.IsInCludeInActiveOnUpdateCheckBox;
            this.RefreshAutomationList("OnUpdate");
        }
    };
    AutomationsSettingsComponent.prototype.SetObjectTableInWindoWArgs = function (windowArgs) {
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ObjectTableName = this.ObjectTableName;
        if (this.ObjectTableName == "Master") {
            windowArgs.IsMasterShipment = true;
            var table = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
            if (table) {
                windowArgs.ObjectTableId = table.Id;
                windowArgs.ObjectTableName = table.Name;
            }
        }
    };
    AutomationsSettingsComponent.prototype.AddAutomation = function (type) {
        var newEntity = new AutomationPMExtended_1.AutomationPM();
        newEntity.Type = type;
        newEntity.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        newEntity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newEntity.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newEntity.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        newEntity.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        newEntity.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newEntity.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newEntity.Description = "";
        newEntity.Version = 1,
            newEntity.Inactive = false;
        newEntity.ResultCode = "EMAIL";
        newEntity.DocumentTypeId = "";
        newEntity.TemplateId = "";
        newEntity.Id = "";
        newEntity.From = "";
        newEntity.FromEmail = "";
        newEntity.AutomationXML = "";
        newEntity.ObjectTableId = this.ObjectTableId;
        newEntity.Order = this.AutomationList.filter(function (d) { return d.EntityPM.Type == type; }) ? this.AutomationList.filter(function (d) { return d.EntityPM.Type == type; }).length : 0;
        newEntity.Name = "";
        var windowArgs = {};
        this.SetObjectTableInWindoWArgs(windowArgs);
        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM = newEntity;
        windowArgs.Mode = "Add";
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 815;
        logWindow.Title = "Add Automation";
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent");
    };
    AutomationsSettingsComponent.prototype.EditAutomation = function (type, item) {
        var isDirty = item.EntityPM.IsDirty;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM = item.EntityPM;
        windowArgs.Mode = "Edit";
        this.SetObjectTableInWindoWArgs(windowArgs);
        windowArgs.IsNewEntity = false;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 815;
        logWindow.Title = "Edit Automation";
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "Cancel") {
                item.EntityPM.IsDirty = isDirty;
            }
        });
    };
    AutomationsSettingsComponent.prototype.RefreshAutomationList = function (automationListType) {
        if (automationListType == "OnCreate") {
            if (this.IsInCludeInActiveOnCreateCheckBox)
                this.OnCreateAutomationList = this.AutomationList.filter(function (d) { return d.EntityPM.Type == "OnCreate"; });
            else
                this.OnCreateAutomationList = this.AutomationList.filter(function (d) { return d.EntityPM.Type == "OnCreate" && d.EntityPM.Inactive == false; });
            this.OnCreateAutomationList = this.OnCreateAutomationList.sort(function (a, b) { return a.Order - b.Order; });
        }
        else if (automationListType == "OnUpdate") {
            if (this.IsInCludeInActiveOnUpdateCheckBox)
                this.OnUpdateAutomationList = this.AutomationList.filter(function (d) { return d.EntityPM.Type == "OnUpdate"; });
            else
                this.OnUpdateAutomationList = this.AutomationList.filter(function (d) { return d.EntityPM.Type == "OnUpdate" && d.EntityPM.Inactive == false; });
            this.OnUpdateAutomationList = this.OnUpdateAutomationList.sort(function (a, b) { return a.Order - b.Order; });
        }
        //else if (automationListType == "Schedule") {
        //    if (Inacive) this.ScheduleAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "Schedule");
        //    else this.ScheduleAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "Schedule" && d.Inactive == false);
        //    this.ScheduleAutomationList = this.ScheduleAutomationList.sort((a, b) => { return a.Order - b.Order });
        //}
        this.RefreshAutomationTitles();
    };
    AutomationsSettingsComponent.prototype.RefreshAutomation = function (item, pross) {
        if (pross == "Edit")
            this.AutomationList = this.AutomationList.filter(function (d) { return d.Id != item.Id; });
        this.AutomationList.push(new AutomationItemViewModel_1.AutomationItemViewModel(item));
        this.RefreshAutomationList(item.Type);
    };
    AutomationsSettingsComponent.prototype.RefreshAutomationTitles = function () {
        this.OnCreateAutomationTabTitle = "On Create (" + this.OnCreateAutomationList.length.toString() + ")";
        this.OnUpdateAutomationTabTitle = "On Update (" + this.OnUpdateAutomationList.length.toString() + ")";
        this.ScheduleAutomationTabTitle = "Schedule (" + this.ScheduleAutomationList.length.toString() + ")";
    };
    AutomationsSettingsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AutomationsSettingsComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var automations = this.AutomationList.filter(function (d) { return d.EntityPM.IsDirty; });
        var automationArgsLists = [];
        if (automations && automations.length > 0) {
            automations.forEach(function (item) {
                var automationArgs = new AutomationArgs_1.AutomationArgs();
                automationArgs.Id = item.Id;
                automationArgs.Tenant = item.Tenant;
                automationArgs.Order = item.Order;
                automationArgsLists.push(automationArgs);
            });
            //automations.forEach((item) => {
            //    item.EntityPM.AutomatedDataBackup = null;
            //    item.EntityPM.AutomationXML = "";
            //    if (item.EntityPM) {
            //        automationsPMList.push(item.EntityPM);
            //    }
            //});
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._automationExtendedPMService.putAuomationList(automationArgsLists).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
        else
            this.CurrentSession.CloseCurrentWindow();
    };
    AutomationsSettingsComponent.prototype.ArrowUpAutomationButtonClicked = function (item, type) {
        if (item != null) {
            if (type == "OnCreate") {
                var i = this.OnCreateAutomationList.indexOf(item);
                var upColumn = this.OnCreateAutomationList[i - 1];
                if (i > 0) {
                    this.OnCreateAutomationList = this.OnCreateAutomationList.filter(function (d) { return d.Id != upColumn.Id; });
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = upColumn.Order;
                    upColumn.Order = upColumn.EntityPM.Order = tempOrder;
                    this.OnCreateAutomationList.splice(i, 0, upColumn);
                }
            }
            else if (type == "OnUpdate") {
                var i = this.OnUpdateAutomationList.indexOf(item);
                var upColumn = this.OnUpdateAutomationList[i - 1];
                if (i > 0) {
                    this.OnUpdateAutomationList = this.OnUpdateAutomationList.filter(function (d) { return d.Id != upColumn.Id; });
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = upColumn.Order;
                    upColumn.Order = upColumn.EntityPM.Order = tempOrder;
                    this.OnUpdateAutomationList.splice(i, 0, upColumn);
                }
            }
        }
    };
    AutomationsSettingsComponent.prototype.ArrowDownAutomationButtonClicked = function (item, type) {
        if (item != null) {
            if (type == "OnCreate") {
                var i = this.OnCreateAutomationList.indexOf(item);
                var downColumn = this.OnCreateAutomationList[i + 1];
                if (i < this.OnCreateAutomationList.length - 1) {
                    this.OnCreateAutomationList = this.OnCreateAutomationList.filter(function (d) { return d.Id != downColumn.Id; });
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = downColumn.Order;
                    downColumn.Order = downColumn.EntityPM.Order = tempOrder;
                    this.OnCreateAutomationList.splice(i, 0, downColumn);
                }
            }
            else if (type == "OnUpdate") {
                var i = this.OnUpdateAutomationList.indexOf(item);
                var downColumn = this.OnUpdateAutomationList[i + 1];
                if (i < this.OnUpdateAutomationList.length - 1) {
                    this.OnUpdateAutomationList = this.OnUpdateAutomationList.filter(function (d) { return d.Id != downColumn.Id; });
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = downColumn.Order;
                    downColumn.Order = downColumn.EntityPM.Order = tempOrder;
                    this.OnUpdateAutomationList.splice(i, 0, downColumn);
                }
            }
        }
    };
    AutomationsSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AutomationsSettingsComponent',
            templateUrl: './AutomationsSettingsComponent.html',
            providers: [AutomationExtendedPMService_1.AutomationExtendedPMService, AutomationPMService_1.AutomationPMService],
        }),
        __metadata("design:paramtypes", [AutomationExtendedPMService_1.AutomationExtendedPMService, AutomationPMService_1.AutomationPMService])
    ], AutomationsSettingsComponent);
    return AutomationsSettingsComponent;
}());
exports.AutomationsSettingsComponent = AutomationsSettingsComponent;
//# sourceMappingURL=AutomationsSettingsComponent.js.map