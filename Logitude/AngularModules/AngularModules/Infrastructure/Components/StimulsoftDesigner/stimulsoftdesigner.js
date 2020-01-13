import { Component, NgZone } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
export var StimulsoftDesigner = (function () {
    //signalRChannelService: SignalRChannelService;
    function StimulsoftDesigner(_ngZone) {
        var _this = this;
        this._ngZone = _ngZone;
        this.URI = "";
        //this.signalRChannelService = new SignalRChannelService();
        window.stimulsoftDesignerComponentRef = {
            zone: this._ngZone,
            componentFn: function (value) { return _this.stimuldesignerFinished(value); },
            component: this
        };
        //this.URI = SessionLocator.GetLogitudeURL() + "/Stimulsoft/Designer.aspx"; 
    }
    StimulsoftDesigner.prototype.stimuldesignerFinished = function (value) {
        // this.zone.run(() => {
        SessionLocator.SelectedSession.CurrentWindow.Close(this.TemplateId);
        // });
    };
    StimulsoftDesigner.prototype.ngOnDestroy = function () {
        window.stimulsoftDesignerComponentRef = null;
    };
    StimulsoftDesigner.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var sessionId = Guid.newGuid();
        this.windowArgs = args;
        this.TemplateId = this.windowArgs.TemplateId;
        this.URI = SessionLocator.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token + "&tenant=" + SessionInfo.LoggedUserTenant + "&templateId=" + this.windowArgs.TemplateId + "&sessionId=" + sessionId;
        var observable = SessionLocator.SignalRChannelService.subscribeChannel("User" + SessionInfo.LoggedUserId + SessionInfo.LoggedUserTenant + sessionId).subscribe(function (ev) {
            if (ev.EventName === "StimulSaved") {
                observable.unsubscribe();
                SessionLocator.SelectedSession.CurrentWindow.Close(_this.TemplateId);
            }
        }, function (error) {
            console.warn("Attempt to join channel failed!", error);
        });
        // this.URI = SessionLocator.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token;
        //var WindowHeight = window.innerHeight - 100;
        //var WindowWidth = window.innerWidth - 100;
        //window.open(this.URI, 'Stimulsoft Designer', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowHeight + ',height=' + WindowWidth);
        //this.windowArgs.TemplateId = item.Id;
        //this.windowArgs.Tenant = item.Entity.Tenant;
        //this.URI = SessionLocator.GetLogitudeURL() + "/Stimulsoft/Designer.html"//;?token=" + SessionInfo.Token + "&tenant=" + this.windowArgs.Tenant + "&templateId=" + this.windowArgs.TemplateId;
    };
    StimulsoftDesigner.prototype.onClose = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Report Designer";
        confirmWindow.Show("Do you want to close Report Designer? Changes you made may not be saved.");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                SessionLocator.SelectedSession.CurrentWindow.Close(_this.TemplateId);
            }
            else {
            }
        });
    };
    StimulsoftDesigner.decorators = [
        { type: Component, args: [{
                    selector: 'DropBoxLogin',
                    moduleId: module.id,
                    templateUrl: './StimulsoftDesigner.html',
                },] },
    ];
    /** @nocollapse */
    StimulsoftDesigner.ctorParameters = [
        { type: NgZone, },
    ];
    return StimulsoftDesigner;
}());
//# sourceMappingURL=StimulsoftDesigner.js.map