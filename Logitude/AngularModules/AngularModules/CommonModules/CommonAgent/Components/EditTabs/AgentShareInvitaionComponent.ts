import { Component } from '@angular/core';
import { AgentPM } from '../../../../Common/EntityPMs/AgentPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AgentSharedLogisticsKey } from '../../../../Common/EntityPMs/AgentSharedLogisticsKey';
import { AgentSharedLogisticsKeyPMService } from '../../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool, FormatTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './AgentShareInvitaionComponent.html',
    providers: [AgentSharedLogisticsKeyPMService],
})

export class AgentShareInvitaionComponent extends BaseComponent {
    agentSharedLogisticsKey: AgentSharedLogisticsKey;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _agentSharedLogisticsKeyPMService: AgentSharedLogisticsKeyPMService) {
        super();
        

    }

    public InvitationSent: boolean = false;
    public Email: string;
    public CancelButtonLabel: string = "Cancel";
     SetWindowArgs(args: any) {

        this.EntityPM = args.CurrentEntity;
    }

     public ValidationErrorsList: string[] = [];

     OnSendInvitaion() {
         this.ValidationErrorsList = [];
         if (!FormatTool.IsEmail(this.Email)) {
             this.ValidationErrorsList.push("Invalid email format!");
             return;
         }

         if (!AppTool.IsNullOrEmpty(this.Email)) {
             this.CurrentSession.StartBusyIndicator("Sending...");

             this._agentSharedLogisticsKeyPMService.SendAgentInvitaion(this.EntityPM.Id, this.Email, SessionLocator.Tenant).subscribe(response => {

                 this.CurrentSession.StopBusyIndicator();
                 if (!response.HasError) {
                     this.CancelButtonLabel = "Close";
                     this.InvitationSent = true;

                     //this.CurrentSession.CurrentWindow.Close(response.Result);
                 }
                 else
                 {
                     if (response.ErrorsArray.length) {
                         for (var k in response.ErrorsArray) {
                             this.ValidationErrorsList.push(response.ErrorsArray[k]);
                         }
                     }
                 }

             });
         }
         else {

             this.ValidationErrorsList.push("Email field is required");

         }
     }

     OnCancel() {
         if (this.InvitationSent) {
             this.CurrentSession.CurrentWindow.Close("InvitationSent");
         } else {
             this.CurrentSession.CloseCurrentWindow();
         }
     }
}
