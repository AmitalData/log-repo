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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var StimulsoftDesigner = /** @class */ (function () {
    function StimulsoftDesigner(_ngZone) {
        var _this = this;
        this._ngZone = _ngZone;
        //signalRChannelService: SignalRChannelService;
        this.URI = "";
        this.ReportTemplateId = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        window.stimulsoftDesignerComponentRef = {
            zone: this._ngZone,
            componentFn: function (value) { return _this.stimuldesignerFinished(value); },
            component: this
        };
        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx";
        //this.signalRChannelService = new SignalRChannelService();
    }
    StimulsoftDesigner.prototype.stimuldesignerFinished = function (value) {
        // this.zone.run(() => {
        this.CurrentSession.CurrentWindow.Close(this.TemplateId);
        // });
    };
    StimulsoftDesigner.prototype.ngOnDestroy = function () {
        window.stimulsoftDesignerComponentRef = null;
    };
    StimulsoftDesigner.prototype.SetWindowArgs = function (args) {
        var sessionId = Guid_1.Guid.newGuid();
        this.windowArgs = args;
        this.TemplateId = !Tools_1.AppTool.IsNullOrEmpty(this.windowArgs.TemplateId) ? this.windowArgs.TemplateId : "";
        this.ReportTemplateId = !Tools_1.AppTool.IsNullOrEmpty(this.windowArgs.ReportTemplateId) ? this.windowArgs.ReportTemplateId : "";
        this.ProcessType = !Tools_1.AppTool.IsNullOrEmpty(this.windowArgs.ProcessType) ? this.windowArgs.ProcessType : "";
        this.URI = Tools_1.AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo_1.SessionInfo.Token + "&tenant=" + SessionInfo_1.SessionInfo.LoggedUserTenant + "&templateId=" + this.TemplateId + "&sessionId=" + sessionId + "&reportTemplateId=" + this.ReportTemplateId + "&processType=" + this.ProcessType;
        //var observable = this.signalRChannelService.subscribeChannel("User" + SessionInfo.LoggedUserId + SessionInfo.LoggedUserTenant + sessionId).subscribe(
        //      (ev: any) => {
        //          if (ev.EventName === "StimulSaved") {
        //              observable.unsubscribe();
        //              this.CurrentSession.CurrentWindow.Close(this.TemplateId);
        //          } else if (ev.EventName === "StimulReportSaved") {
        //              observable.unsubscribe();
        //            this.CurrentSession.CurrentWindow.Close(this.ReportTemplateId);
        //            //this.signalRChannelService.unSubscribeChannel
        //          }
        //      },
        //      (error: any) => {
        //          console.warn("Attempt to join channel failed!", error);
        //      }
        //)
        // this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token;
        //var WindowHeight = window.innerHeight - 100;
        //var WindowWidth = window.innerWidth - 100;
        //window.open(this.URI, 'Stimulsoft Designer', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowHeight + ',height=' + WindowWidth);
        //this.windowArgs.TemplateId = item.Id;
        //this.windowArgs.Tenant = item.Entity.Tenant;
        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.html"//;?token=" + SessionInfo.Token + "&tenant=" + this.windowArgs.Tenant + "&templateId=" + this.windowArgs.TemplateId;
    };
    StimulsoftDesigner.prototype.onClose = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Report Designer";
        confirmWindow.Show("Do you want to close Report Designer? Changes you made may not be saved.");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.TemplateId) {
                    _this.CurrentSession.CurrentWindow.Close(_this.TemplateId);
                }
                else {
                    _this.CurrentSession.CurrentWindow.Close(_this.ReportTemplateId);
                }
            }
            else {
            }
        });
    };
    StimulsoftDesigner = __decorate([
        core_1.Component({
            selector: 'DropBoxLogin',
            moduleId: module.id,
            templateUrl: './StimulsoftDesigner.html',
        }),
        __metadata("design:paramtypes", [core_1.NgZone])
    ], StimulsoftDesigner);
    return StimulsoftDesigner;
}());
exports.StimulsoftDesigner = StimulsoftDesigner;
//# sourceMappingURL=StimulsoftDesigner.js.map