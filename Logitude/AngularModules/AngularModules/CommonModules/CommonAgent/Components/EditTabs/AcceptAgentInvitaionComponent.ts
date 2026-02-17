import { Component } from '@angular/core';
import { AgentPM } from '../../../../Common/EntityPMs/AgentPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AgentSharedLogisticsKey } from '../../../../Common/EntityPMs/AgentSharedLogisticsKey';
import { AgentSharedLogisticsKeyPMService } from '../../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './AcceptAgentInvitaionComponent.html',
    providers: [AgentSharedLogisticsKeyPMService],
})

export class AcceptAgentInvitaionComponent extends BaseComponent {
    agentSharedLogisticsKey: AgentSharedLogisticsKey;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _agentSharedLogisticsKeyPMService: AgentSharedLogisticsKeyPMService) {
        super();


    }

    public SharedKey: string;
    SetWindowArgs(args: any) {

        this.EntityPM = args.CurrentEntity;
    }

    public ValidationErrorsList: string[] = [];

    OnAcceptInvitaion() {
        this.ValidationErrorsList = [];
        if (!AppTool.IsNullOrEmpty(this.SharedKey)) {
            this.CurrentSession.StartBusyIndicator("Sending...");

            this._agentSharedLogisticsKeyPMService.GetSingle(this.SharedKey).subscribe(response => {

                this.CurrentSession.StopBusyIndicator();
                if (!response.HasError) {
                    if (response.Result) {

                        this.agentSharedLogisticsKey = response.Result;

                        if (this.agentSharedLogisticsKey.StatusCode === "I") {
                            this.ValidationErrorsList.push("Invalid invitation key!");
                            return;
                        }
                        //this.EntityPM.AgentSharedLogisticsKey = this.agentSharedLogisticsKey.SharedKey;
                        //this.CurrentSession.CurrentEditComponent.SaveChanges();
                        this.agentSharedLogisticsKey.Agent2Tenant = SessionLocator.Tenant;
                        this.agentSharedLogisticsKey.ApproveDate = DateTool.GetCurrentDateTimeAsUtc();
                        this.agentSharedLogisticsKey.ApprovedByUserEmail = SessionLocator.LoggedUserPM.Email;
                        this.agentSharedLogisticsKey.StatusCode = "A";

                        this.CurrentSession.StartBusyIndicator("Saving...");
                        this._agentSharedLogisticsKeyPMService.update(this.agentSharedLogisticsKey,this.EntityPM.Id,true).subscribe(res => {
                            this.CurrentSession.StopBusyIndicator();
                            if (!res.HasError) {
                                this.agentSharedLogisticsKey = res.Result;
                                this.CurrentSession.CurrentWindow.Close("true");
                            }
                            else {
                                this.ValidationErrorsList = res.ErrorsArray;
                            }

                        });
                    }
                    else {
                        this.ValidationErrorsList.push("Invalid invitation key!");
                    }
                   
                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
               

            });
        }
        else {

             
            this.ValidationErrorsList.push("Please enter the shared key");

        }
    }

    OnCancel() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
