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
var AgentShareInvitaionComponent = /** @class */ (function (_super) {
    __extends(AgentShareInvitaionComponent, _super);
    function AgentShareInvitaionComponent(_agentSharedLogisticsKeyPMService) {
        var _this = _super.call(this) || this;
        _this._agentSharedLogisticsKeyPMService = _agentSharedLogisticsKeyPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InvitationSent = false;
        _this.CancelButtonLabel = "Cancel";
        _this.ValidationErrorsList = [];
        return _this;
    }
    AgentShareInvitaionComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.CurrentEntity;
    };
    AgentShareInvitaionComponent.prototype.OnSendInvitaion = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!Tools_1.FormatTool.IsEmail(this.Email)) {
            this.ValidationErrorsList.push("Invalid email format!");
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Email)) {
            this.CurrentSession.StartBusyIndicator("Sending...");
            this._agentSharedLogisticsKeyPMService.SendAgentInvitaion(this.EntityPM.Id, this.Email, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                if (!response.HasError) {
                    _this.CancelButtonLabel = "Close";
                    _this.InvitationSent = true;
                    //this.CurrentSession.CurrentWindow.Close(response.Result);
                }
                else {
                    if (response.ErrorsArray.length) {
                        for (var k in response.ErrorsArray) {
                            _this.ValidationErrorsList.push(response.ErrorsArray[k]);
                        }
                    }
                }
            });
        }
        else {
            this.ValidationErrorsList.push("Email field is required");
        }
    };
    AgentShareInvitaionComponent.prototype.OnCancel = function () {
        if (this.InvitationSent) {
            this.CurrentSession.CurrentWindow.Close("InvitationSent");
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AgentShareInvitaionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgentShareInvitaionComponent.html',
            providers: [AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService],
        }),
        __metadata("design:paramtypes", [AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService])
    ], AgentShareInvitaionComponent);
    return AgentShareInvitaionComponent;
}(BaseComponent_1.BaseComponent));
exports.AgentShareInvitaionComponent = AgentShareInvitaionComponent;
//# sourceMappingURL=AgentShareInvitaionComponent.js.map