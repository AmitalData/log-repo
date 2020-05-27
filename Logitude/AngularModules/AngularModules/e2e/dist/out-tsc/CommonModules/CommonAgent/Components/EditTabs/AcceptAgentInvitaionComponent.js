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
var AgentSharedLogisticsKeyPMService_1 = require("../../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AcceptAgentInvitaionComponent = /** @class */ (function (_super) {
    __extends(AcceptAgentInvitaionComponent, _super);
    function AcceptAgentInvitaionComponent(_agentSharedLogisticsKeyPMService) {
        var _this = _super.call(this) || this;
        _this._agentSharedLogisticsKeyPMService = _agentSharedLogisticsKeyPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        return _this;
    }
    AcceptAgentInvitaionComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.CurrentEntity;
    };
    AcceptAgentInvitaionComponent.prototype.OnAcceptInvitaion = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SharedKey)) {
            this.CurrentSession.StartBusyIndicator("Sending...");
            this._agentSharedLogisticsKeyPMService.GetSingle(this.SharedKey).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                if (!response.HasError) {
                    if (response.Result) {
                        _this.agentSharedLogisticsKey = response.Result;
                        if (_this.agentSharedLogisticsKey.StatusCode === "I") {
                            _this.ValidationErrorsList.push("Invalid invitation key!");
                            return;
                        }
                        //this.EntityPM.AgentSharedLogisticsKey = this.agentSharedLogisticsKey.SharedKey;
                        //this.CurrentSession.CurrentEditComponent.SaveChanges();
                        _this.agentSharedLogisticsKey.Agent2Tenant = SessionLocator_1.SessionLocator.Tenant;
                        _this.agentSharedLogisticsKey.ApproveDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                        _this.agentSharedLogisticsKey.ApprovedByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
                        _this.agentSharedLogisticsKey.StatusCode = "A";
                        _this.CurrentSession.StartBusyIndicator("Saving...");
                        _this._agentSharedLogisticsKeyPMService.update(_this.agentSharedLogisticsKey, _this.EntityPM.Id, true).subscribe(function (res) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (!res.HasError) {
                                _this.agentSharedLogisticsKey = res.Result;
                                _this.CurrentSession.CurrentWindow.Close("true");
                            }
                            else {
                                _this.ValidationErrorsList = res.ErrorsArray;
                            }
                        });
                    }
                    else {
                        _this.ValidationErrorsList.push("Invalid invitation key!");
                    }
                }
                else {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
            });
        }
        else {
            this.ValidationErrorsList.push("Please enter the shared key");
        }
    };
    AcceptAgentInvitaionComponent.prototype.OnCancel = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AcceptAgentInvitaionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AcceptAgentInvitaionComponent.html',
            providers: [AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService],
        }),
        __metadata("design:paramtypes", [AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService])
    ], AcceptAgentInvitaionComponent);
    return AcceptAgentInvitaionComponent;
}(BaseComponent_1.BaseComponent));
exports.AcceptAgentInvitaionComponent = AcceptAgentInvitaionComponent;
//# sourceMappingURL=AcceptAgentInvitaionComponent.js.map