import {Component} from '@angular/core';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AgentSharedLogisticsKey} from '../../../../Common/EntityPMs/AgentSharedLogisticsKey';
import {AgentSharedLogisticsKeyPMService} from '../../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AgentSharedLogisticsTabComponent.html',
    providers: [AgentSharedLogisticsKeyPMService],
})

export class AgentSharedLogisticsTabComponent extends BaseComponent {
    public EntityPM: AgentPM;
    public ObjectTableName: string = "Agent";
    IsShowDefultButton: boolean = true;
    agentSharedLogisticsKey: AgentSharedLogisticsKey;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public _agentSharedLogisticsKeyPMService: AgentSharedLogisticsKeyPMService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.LoadData();

    }

    LoadData() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.AgentSharedLogisticsKey)) {

            this.LoadAgentSharedLogisticsKey();
        }
        else {
            this.IsShowDefultButton = true;
        }
    }

    StatusName: string = "";
    LoadAgentSharedLogisticsKey() {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._agentSharedLogisticsKeyPMService.GetSingle(this.EntityPM.AgentSharedLogisticsKey).subscribe(res => {

            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.agentSharedLogisticsKey = myResult;
                    this.SetButtonsVisibility();

                }
            }
        });
    }


    SetButtonsVisibility()
    {
        this.IsShowDefultButton = true;
        if (this.agentSharedLogisticsKey.StatusCode == "A") {
            this.IsShowDefultButton = false;
            this.StatusName = "Active";
        }
        else if (this.agentSharedLogisticsKey.StatusCode == "W") {
            if (this.agentSharedLogisticsKey.Agent1Tenant == SessionLocator.Tenant) {
                this.IsShowDefultButton = false;
            }

            this.StatusName = "Waiting";
        }
        else if (this.agentSharedLogisticsKey.StatusCode == "I") this.StatusName = "In Active";

    }

    SendInvitation() {

        var windowArgs: any = {};
        windowArgs.CurrentEntity = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = "Send Invitation";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./CommonModules/CommonAgent/Components/EditTabs/AgentShareInvitaionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {

                //this.StatusName = "Waiting";
                //this.SetButtonsVisibility();

                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadData();
                    }
                });

               
            }
        });
    }

    OnStopSharing() {
        this.agentSharedLogisticsKey.StatusCode = "I";
        this.agentSharedLogisticsKey.InactiveByUserEmail = SessionLocator.LoggedUserPM.Email;
        this.agentSharedLogisticsKey.InactiveDate = DateTool.GetCurrentDateTimeAsUtc();
        this.CurrentSession.StartBusyIndicator("Saving...");
        this._agentSharedLogisticsKeyPMService.update(this.agentSharedLogisticsKey, this.EntityPM.Id, false).subscribe(res => {
            this.CurrentSession.StopBusyIndicator();
            if (!res.HasError) {
                this.agentSharedLogisticsKey = res.Result;
                this.SetButtonsVisibility();
            }
            else {
                // this.
            }

        });
    }


    Acceptinvitation() {

        var windowArgs: any = {};
        windowArgs.CurrentEntity = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.Title = "Accept Invitation";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./CommonModules/CommonAgent/Components/EditTabs/AcceptAgentInvitaionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadData();
                    }
                });

                 


            }
        });
    }

}
