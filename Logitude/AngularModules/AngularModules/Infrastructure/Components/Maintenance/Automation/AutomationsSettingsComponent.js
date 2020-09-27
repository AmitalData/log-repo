import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import { Component } from '@angular/core';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { DateTool } from '../../../../Infrastructure/Tools';
import { AutomationItemViewModel } from './ViewModel/AutomationItemViewModel';
import { AutomationExtendedPMService } from '../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService';
import { AutomationPMService } from '../../../../Common/Services/StandardPMs/AutomationPMService';
import { AutomationPM } from '../../../../Common/EntityPMs/AutomationPMExtended';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
export var AutomationsSettingsComponent = (function () {
    function AutomationsSettingsComponent(_automationExtendedPMService, _automationPMService) {
        this._automationExtendedPMService = _automationExtendedPMService;
        this._automationPMService = _automationPMService;
        this.IsShowTabUpdate = false;
        this._entityResourceService = new EntityResourceService();
        this.SelectedTabCode = "";
        this.AutomationList = [];
        this.ScheduleAutomationList = [];
        this.OnUpdateAutomationList = [];
        this.OnCreateAutomationList = [];
        this.IsAddAtomationEnable = false;
        this.OnUpdateTabVisibility = false;
        this.ScheduleTabVisibility = false;
    }
    AutomationsSettingsComponent.prototype.ngOnInit = function () {
    };
    AutomationsSettingsComponent.prototype.SetDataContext = function (tableName) {
        var _this = this;
        this.AutomationList = [];
        this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
            _this.ShowIncludeInactiveOnCreateCheckBoxKey = Guid.newGuid();
            _this.ShowIncludeInactiveOnUpDateCheckBoxKey = Guid.newGuid();
            _this.SelectedTabCode = "CRA";
            if (tableName) {
                var table = window.ObjectTables.filter(function (d) { return d.Name == tableName; })[0];
                if (table) {
                    _this.ObjectTableName = table.Name;
                    _this.ObjectTableId = table.Id;
                    _this.LoadAutomationsList();
                }
            }
            if (!FeatureLocator.HasFeaturePermession("Automation", "NEW")) {
                _this.IsAddAtomationEnable = false;
            }
            else {
                _this.IsAddAtomationEnable = true;
            }
            if (_this.ObjectTableName != "LogitudeMessagesTransmissionLog") {
                _this.ScheduleTabVisibility = true;
                _this.OnUpdateTabVisibility = true;
                _this.IsShowTabUpdate = true;
            }
        });
    };
    AutomationsSettingsComponent.prototype.LoadAutomationsList = function () {
        var _this = this;
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this.AutomationList = [];
        this._automationExtendedPMService.getAutomationesByObjectTableId(this.ObjectTableId, SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach(function (automationes) {
                    var automationItemViewModel = new AutomationItemViewModel(automationes);
                    _this.AutomationList.push(automationItemViewModel);
                });
                _this.RefreshAutomationList("OnCreate");
                _this.RefreshAutomationList("OnUpdate");
                SessionLocator.SelectedSession.StopBusyIndicator();
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
    AutomationsSettingsComponent.prototype.AddAutomation = function (type) {
        var newEntity = new AutomationPM();
        newEntity.Type = type;
        newEntity.Tenant = SessionLocator.TenantPM.Id;
        newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
        newEntity.UpdatedByUserId = SessionLocator.LoggedUserId;
        newEntity.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        newEntity.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        newEntity.CreateDate = DateTool.GetCurrentDateAsUtc();
        newEntity.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newEntity.Description = "";
        newEntity.Version = 0,
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
        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM = newEntity;
        windowArgs.Mode = "Add";
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ObjectTableName = this.ObjectTableName;
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 815;
        logWindow.Title = "Add Automation";
        logWindow.IsShowCloseButton = true;
        //logWindow.IsFullScreen = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
        });
    };
    AutomationsSettingsComponent.prototype.EditAutomation = function (type, item) {
        var isDirty = item.EntityPM.IsDirty;
        var windowArgs = {};
        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM = item.EntityPM;
        windowArgs.Mode = "Edit";
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ObjectTableName = this.ObjectTableName;
        windowArgs.IsNewEntity = false;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 815;
        logWindow.Title = "Edit Automation";
        logWindow.IsShowCloseButton = true;
        //logWindow.IsFullScreen = true;
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
        this.AutomationList.push(new AutomationItemViewModel(item));
        this.RefreshAutomationList(item.Type);
    };
    AutomationsSettingsComponent.prototype.RefreshAutomationTitles = function () {
        this.OnCreateAutomationTabTitle = "On Create (" + this.OnCreateAutomationList.length.toString() + ")";
        this.OnUpdateAutomationTabTitle = "On Update (" + this.OnUpdateAutomationList.length.toString() + ")";
        this.ScheduleAutomationTabTitle = "Schedule (" + this.ScheduleAutomationList.length.toString() + ")";
    };
    AutomationsSettingsComponent.prototype.CloseButtonClicked = function () {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    AutomationsSettingsComponent.prototype.SaveButtonClicked = function () {
        var automations = this.AutomationList.filter(function (d) { return d.EntityPM.IsDirty; });
        var automationsPMList = [];
        if (automations && automations.length > 0) {
            automations.forEach(function (item) {
                item.EntityPM.AutomatedDataBackup = null;
                item.EntityPM.AutomationXML = "";
                if (item.EntityPM) {
                    automationsPMList.push(item.EntityPM);
                }
            });
            SessionLocator.SelectedSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._automationExtendedPMService.putAuomationList(automationsPMList).subscribe(function (res) {
                SessionLocator.SelectedSession.CurrentWindow.StopBusyIndicator();
                SessionLocator.SelectedSession.CloseCurrentWindow();
            });
        }
        else
            SessionLocator.SelectedSession.CloseCurrentWindow();
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
    AutomationsSettingsComponent.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    selector: 'AutomationsSettingsComponent',
                    templateUrl: './AutomationsSettingsComponent.html',
                    providers: [AutomationExtendedPMService, AutomationPMService],
                },] },
    ];
    /** @nocollapse */
    AutomationsSettingsComponent.ctorParameters = [
        { type: AutomationExtendedPMService, },
        { type: AutomationPMService, },
    ];
    return AutomationsSettingsComponent;
}());
//# sourceMappingURL=AutomationsSettingsComponent.js.map