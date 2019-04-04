import { Component, NgZone} from '@angular/core'
import {AppTool} from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { SignalRChannelService } from '../../Services/SignalRServices/SignalRChannelService';
declare var window: any;
declare var startLinking;
@Component({
    selector: 'DropBoxLogin',
    moduleId: module.id,
    templateUrl: './StimulsoftDesigner.html',
//    template: `
 
//<iframe id="stimuldesignerframeId" name="stimuldesignerframe" [src]="URI | SafePipe" style="width:100%;height:100%"></iframe>


// `    
})

export class StimulsoftDesigner {

   signalRChannelService: SignalRChannelService;
    public URI: string = "";
    private windowArgs: any;
    public TemplateId: string;
    public ReportTemplateId: string = "";
    ProcessType: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _ngZone: NgZone) {
        window.stimulsoftDesignerComponentRef = {
            zone: this._ngZone,
            componentFn: (value) => this.stimuldesignerFinished(value),
            component: this
        };
        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx";
       
      this.signalRChannelService = new SignalRChannelService();

         
    }

    stimuldesignerFinished(value) {
        // this.zone.run(() => {
        this.CurrentSession.CurrentWindow.Close(this.TemplateId);
        // });
    }

    ngOnDestroy() {
        window.stimulsoftDesignerComponentRef = null;
    }

    SetWindowArgs(args: any) {
        var sessionId: string = Guid.newGuid();
        this.windowArgs = args;
        this.TemplateId = !AppTool.IsNullOrEmpty(this.windowArgs.TemplateId) ? this.windowArgs.TemplateId:"";
        this.ReportTemplateId = !AppTool.IsNullOrEmpty(this.windowArgs.ReportTemplateId) ? this.windowArgs.ReportTemplateId : "";
        this.ProcessType = !AppTool.IsNullOrEmpty(this.windowArgs.ProcessType) ? this.windowArgs.ProcessType : "";

        this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token + "&tenant=" + SessionInfo.LoggedUserTenant + "&templateId=" + this.TemplateId + "&sessionId=" + sessionId + "&reportTemplateId=" + this.ReportTemplateId + "&processType=" + this.ProcessType;


      var observable = this.signalRChannelService.subscribeChannel("User" + SessionInfo.LoggedUserId + SessionInfo.LoggedUserTenant + sessionId).subscribe(
            (ev: any) => {

                if (ev.EventName === "StimulSaved") {
                    observable.unsubscribe();
                    this.CurrentSession.CurrentWindow.Close(this.TemplateId);
                } else if (ev.EventName === "StimulReportSaved") {
                    observable.unsubscribe();
                  this.CurrentSession.CurrentWindow.Close(this.ReportTemplateId);
                  //this.signalRChannelService.unSubscribeChannel
                }

            },
            (error: any) => {
                console.warn("Attempt to join channel failed!", error);
            }
        )
       // this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token;

        //var WindowHeight = window.innerHeight - 100;
        //var WindowWidth = window.innerWidth - 100;

        //window.open(this.URI, 'Stimulsoft Designer', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowHeight + ',height=' + WindowWidth);

        //this.windowArgs.TemplateId = item.Id;
        //this.windowArgs.Tenant = item.Entity.Tenant;

        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.html"//;?token=" + SessionInfo.Token + "&tenant=" + this.windowArgs.Tenant + "&templateId=" + this.windowArgs.TemplateId;
        
    }
    onClose() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Report Designer";
            confirmWindow.Show("Do you want to close Report Designer? Changes you made may not be saved.");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {

                    if (this.TemplateId) {
                        this.CurrentSession.CurrentWindow.Close(this.TemplateId);
                    }
                    else {
                        this.CurrentSession.CurrentWindow.Close(this.ReportTemplateId);
                    }

                }
                else {

                }


            });
    }



     
    
     

}
