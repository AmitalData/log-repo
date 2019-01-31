import { Component, OnInit } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import { CardList } from '../../../Common/EntityLists/CardList';
import { AgentSharedLogisticsKeyPMService } from '../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService';
import { AgentSharedLogisticsKey } from '../../../Common/EntityPMs/AgentSharedLogisticsKey';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
@Component({
    moduleId: module.id,
    selector: 'SharedManifestStarted',
    templateUrl: './SharedManifestStarted.html',
    providers: [SharedAgentManifestService, AgentSharedLogisticsKeyPMService],
})
export class SharedManifestStarted {
    //private myCardListService: CardListService;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService, public _agentSharedLogisticsKeyPMService: AgentSharedLogisticsKeyPMService) {
        this.Listen();
    }
    public ValidationErrorsList: string[] = [];
    EntityPM: ShipmentPM;
    HasError: boolean = false;
    IsSuccessfullySharedManifest: boolean = false;

    IsEnableButtonSharedManifest: boolean = false;
    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
            this.ValidationErrorsList.push("Please define shipment agent in the Partners tab");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
            this.ValidationErrorsList.push("The consignee partner is missing");
        }

        if (this.EntityPM.TransportModeId == "A" && AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            this.ValidationErrorsList.push("The master number is missing");
        }

      
        if (this.ValidationErrorsList.length == 0) {

            SessionLocator.CurrentSession.StartBusyIndicator("Loading...");

            this._agentSharedLogisticsKeyPMService.GetSingleByAgentId(this.EntityPM.AgentId).subscribe((myResponse: ServiceResponse) => {

                SessionLocator.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {

                    var agentSharedKey: AgentSharedLogisticsKey = myResponse.Result;
                    if (agentSharedKey) {
                        switch (agentSharedKey.StatusCode) {
                            case "A":

                                if (this.EntityPM.ShipmentLevelCode == "C") {
                                    SessionLocator.CurrentSession.StartBusyIndicator("Loading...");
                                    this._sharedAgentManifestService.GetCheckIfMasterShipmentHaveHouseWithOtherAgent(this.EntityPM.Id, this.EntityPM.AgentId, this.EntityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
                                        SessionLocator.CurrentSession.StopBusyIndicator();
                                        if (!myResponse.HasError) {
                                            if (myResponse.Result == true) {
                                                this.ValidationErrorsList.push("One of the houses has agent different from the master shipment");
                                        
                                            } else {
                                                this.IsEnableButtonSharedManifest = true;
                                            }
                                        }
                                    });
                                }
                                else this.IsEnableButtonSharedManifest = true;
                                break;

                            case "W":

                                this.ValidationErrorsList.push("Waiting for the Agent’s approval to enable sharing");
                                this.HasError = true;
                                break;

                            case "I":
                                this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                                this.HasError = true;
                                break;


                        }
                    }
                    else {
                        this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                        this.HasError = true;
                    }
                }
                else {
                    this.ValidationErrorsList.push(myResponse.ErrorsArray[0]);
                    this.HasError = true;
                }

            });

        }
        else this.HasError = true;


    }


    isSharingManifesRequested: boolean = false;
     SharingManifesButtonClick() {

        if (this.EntityPM.IsDirty) {
            this.isSharingManifesRequested = true;
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartSharingManifest();
        }



    };


    Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    if (this.isSharingManifesRequested) {
                        this.isSharingManifesRequested = false;
                        this.StartSharingManifest();
                    }

                } 


            });
        }
    }


    StartSharingManifest() {

        SessionLocator.CurrentSession.StartBusyIndicator("Sharing Manifest...");
        this._sharedAgentManifestService.ShareAgentManifest(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            this.IsEnableButtonSharedManifest = false;
            if (!myResponse.HasError) {
                this.IsSuccessfullySharedManifest = true;
                this.EntityPM.IsManifestSentToAgent = true;
                ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Share Manifests");


            } else {

                if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    myResponse.ErrorsArray.forEach((item) => {
                        this.ValidationErrorsList.push(item);
                    });

                }
                this.HasError = true;
            }


        });





    }


    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}

