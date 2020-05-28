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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AgentSharedLogisticsKeyPMService_1 = require("../../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AgentSharedLogisticsTabComponent = /** @class */ (function (_super) {
    __extends(AgentSharedLogisticsTabComponent, _super);
    function AgentSharedLogisticsTabComponent(entityArgs, _agentSharedLogisticsKeyPMService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._agentSharedLogisticsKeyPMService = _agentSharedLogisticsKeyPMService;
        _this.ObjectTableName = "Agent";
        _this.IsShowDefultButton = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.StatusName = "";
        _this.EntityPM = entityArgs.EntityPM;
        _this.LoadData();
        return _this;
    }
    AgentSharedLogisticsTabComponent.prototype.LoadData = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentSharedLogisticsKey)) {
            this.LoadAgentSharedLogisticsKey();
        }
        else {
            this.IsShowDefultButton = true;
        }
    };
    AgentSharedLogisticsTabComponent.prototype.LoadAgentSharedLogisticsKey = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._agentSharedLogisticsKeyPMService.GetSingle(this.EntityPM.AgentSharedLogisticsKey).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.agentSharedLogisticsKey = myResult;
                    _this.SetButtonsVisibility();
                }
            }
        });
    };
    AgentSharedLogisticsTabComponent.prototype.SetButtonsVisibility = function () {
        this.IsShowDefultButton = true;
        if (this.agentSharedLogisticsKey.StatusCode == "A") {
            this.IsShowDefultButton = false;
            this.StatusName = "Active";
        }
        else if (this.agentSharedLogisticsKey.StatusCode == "W") {
            if (this.agentSharedLogisticsKey.Agent1Tenant == SessionLocator_1.SessionLocator.Tenant) {
                this.IsShowDefultButton = false;
            }
            this.StatusName = "Waiting";
        }
        else if (this.agentSharedLogisticsKey.StatusCode == "I")
            this.StatusName = "In Active";
    };
    AgentSharedLogisticsTabComponent.prototype.SendInvitation = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.CurrentEntity = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = "Send Invitation";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./CommonModules/CommonAgent/Components/EditTabs/AgentShareInvitaionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                //this.StatusName = "Waiting";
                //this.SetButtonsVisibility();
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadData();
                    }
                });
            }
        });
    };
    AgentSharedLogisticsTabComponent.prototype.OnStopSharing = function () {
        var _this = this;
        this.agentSharedLogisticsKey.StatusCode = "I";
        this.agentSharedLogisticsKey.InactiveByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
        this.agentSharedLogisticsKey.InactiveDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.CurrentSession.StartBusyIndicator("Saving...");
        this._agentSharedLogisticsKeyPMService.update(this.agentSharedLogisticsKey, this.EntityPM.Id, false).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            if (!res.HasError) {
                _this.agentSharedLogisticsKey = res.Result;
                _this.SetButtonsVisibility();
            }
            else {
                // this.
            }
        });
    };
    AgentSharedLogisticsTabComponent.prototype.Acceptinvitation = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.CurrentEntity = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = "Accept Invitation";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./CommonModules/CommonAgent/Components/EditTabs/AcceptAgentInvitaionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadData();
                    }
                });
            }
        });
    };
    AgentSharedLogisticsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgentSharedLogisticsTabComponent.html',
            providers: [AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService])
    ], AgentSharedLogisticsTabComponent);
    return AgentSharedLogisticsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AgentSharedLogisticsTabComponent = AgentSharedLogisticsTabComponent;
//# sourceMappingURL=AgentSharedLogisticsTabComponent.js.map