import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { AgentSharedManifestPMService } from '../../../Common/Services/StandardPMs/AgentSharedManifestPMService';
import { AgentSharedManifestList } from '../../../Common/EntityLists/AgentSharedManifestList';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';
import { HouseSL } from '../../../Common/DataContracts/HouseSL';
import { AgentSharedManifesRefShipment } from '../../../Common/DataContracts/AgentSharedManifesRefShipment';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    moduleId: module.id,
    selector: 'SharedManifestComponent',
    templateUrl: './SharedManifestComponent.html',
    providers: [SharedAgentManifestService, AgentSharedManifestPMService, EntityResourceService],
})
export class SharedManifestComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService, private _entityResourceService: EntityResourceService, private _aentSharedManifestPMService: AgentSharedManifestPMService) {

        this.CurrentSession.StartBusyIndicatorLoading();
    }
    IsShowMarkASCompleted: boolean = false;
    public ValidationWarningsList: string[] = [];
    EntityList: AgentSharedManifestList;
    CurrentEntity: AgentSharedManifestPM;
    ManifestSL: ManifestSL;
    HousesList: HouseSL[] = [];
    MessageNoHouseFound: string = "No Houses Found";


    IsDisableEdit: boolean = false;


    IsNoHouses: boolean;
    LableMasterCerate: string = "Create";
    LableHouseArea: string = "";
    WidthButtonStatusChange: string = "110px";
    ButtonChangeStatusLable: string;

    IsEnableCreateHouse: boolean;
    IsEnableCreateMasterButton: boolean;
    IsShowEditButon: boolean = false;
    IsLoadComponent: boolean = false;

    ShowAreaButton: boolean = false;


    SetWindowArgs(args: any) {

        this.EntityList = args.CurrentEntity;
        this.LoadData();

    }
    agentManifestSharedRefListIds: string[] = [];

    LoadData() {

      
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe(res => {
            this._entityResourceService.getEntityResourceByTableName("Master").subscribe(res1 => {
                this.IsLoadComponent = true;
                this._sharedAgentManifestService.get(this.EntityList.Id).subscribe(response => {
  
                    if (!response.HasError) {
                    
                        this.CurrentEntity = response.Result;
                        this.RunSharedManifestHeaderComponent();
                        this.ManifestSL = this.CurrentEntity.ManifestSL;
                        if (this.ManifestSL.ShipmentLevelCode == 'D') {
                            this.MessageNoHouseFound = "This is a direct shipment";
                        }

                        if (this.CurrentEntity.CancelledBySenderAgent) {
                            this.ValidationWarningsList = [];
                            this.ValidationWarningsList.push("The manifest was cancelled by the sender.You are not allowed to reactivate it.");
                            this.IsDisableEdit = true;
                        }
                        else if (!AppTool.IsNullOrEmpty(this.ManifestSL.MasterNumber) && this.ManifestSL.TransportModeId == "A") {
                            this.CheckIfAnyShipmentHaveMasterNumber(this.ManifestSL.MasterNumber, this.ManifestSL.LongMaster);
                        }
                    
                  
                        if (this.ManifestSL) {
                            this.HousesList = this.ManifestSL.Houses;
                            this.CheckifShipmentCreateOrNotAndEnableEdit();

                        } else this.CurrentSession.StopBusyIndicator();


                    } else this.CurrentSession.StopBusyIndicator();
                });
            });
        });
    }

    NumbeofCreatedHouse: number = 0;
    CheckifShipmentCreateOrNotAndEnableEdit() {
    
        this.ManifestSL.SharedManifestRef = this.CurrentEntity.Id; 

        this.agentManifestSharedRefListIds = [];
        this.agentManifestSharedRefListIds.push(this.ManifestSL.SharedManifestRef);

        if (!this.HousesList || (this.HousesList && this.HousesList.length == 0)) this.IsNoHouses = true;
        else {
            this.IsNoHouses = false;
            var index: number = 0;
            this.HousesList.forEach((house) => {
                index += 1;
                house.SharedManifestRef = this.ManifestSL.SharedManifestRef + "/" + index.toString();
                this.agentManifestSharedRefListIds.push(house.SharedManifestRef);
            });
        }


        // Check if Shipment Create Or Not and Enable Edit
            this._sharedAgentManifestService.getAgentSharedManifesRefShipmentListsByIds(this.agentManifestSharedRefListIds).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var result: AgentSharedManifesRefShipment[] = pmResponse.Result;
                    var item = result.filter(d => d.AgentSharedManifestRef == this.ManifestSL.SharedManifestRef)[0];
                    if (item) {
                        this.ManifestSL.EntityId = item.ShipmentId;
                    } else this.IsShowMarkASCompleted = true;
                    
                   
                    if (!this.IsNoHouses) {

                        this.HousesList.forEach((house) => {

                            var item = result.filter(d => d.AgentSharedManifestRef == house.SharedManifestRef)[0];
                            if (item) {
                                this.NumbeofCreatedHouse += 1;
                                house.EntityId = item.ShipmentId;
                            }
                            else {
                                this.IsShowMarkASCompleted = true;
                            }
                        });

                        if (AppTool.IsNullOrEmpty(this.ManifestSL.EntityId)) {
                            this.LableHouseArea = "Create Master to enable houses area";
                        }
                        else {
                            this.LableHouseArea = this.NumbeofCreatedHouse + " of " + this.HousesList.length + " houses created";
                        }
                      

                    }

                    else {
                        this.LableHouseArea = "";
                       
                    }




                }

                this.RefreshSharedManifiestoStatus();
            });


    }



    RefreshSharedManifiestoStatus() {

        this.ButtonChangeStatusLable = "Cancel Manifest";
        this.WidthButtonStatusChange = "110px";
        this.IsEnableCreateMasterButton = true;

        if (!AppTool.IsNullOrEmpty(this.ManifestSL.EntityId)) {
            this.IsShowEditButon = true;
            this.IsEnableCreateHouse = true;
        }

        else if (this.CurrentEntity.StatusCode == "CANC") {
            this.ButtonChangeStatusLable = "reactivate";
            this.IsEnableCreateMasterButton = false;
            this.WidthButtonStatusChange = "70px";
        }
      


        this.ShowAreaButton = true;

        this.CurrentSession.StopBusyIndicator();
    }

    OpenSharedManifestAdditionalComponent(houseEntity: HouseSL) {

        var windowArgs: any = {};

        if (!this.IsNoHouses) {
            if ((this.NumbeofCreatedHouse + 1) == this.HousesList.length) {
                windowArgs.SharedManifestStatus = "COMP";
            }
        } else {
            windowArgs.SharedManifestStatus = "COMP";
        }

            windowArgs.CurrentEntity = this.CurrentEntity;
            windowArgs.ManifestSL = this.ManifestSL;
            windowArgs.HouseEntity = houseEntity;
            windowArgs.AgentSharedManifestList = this.EntityList;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 830;
            logWindow.Height = 750;
            logWindow.Title = "Shared Manifest";
            logWindow.WindowArgs = windowArgs;
            logWindow.IsShowCloseButton = true;
            logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestAdditionalComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event) {
                    if (!houseEntity) {
                        this.LableMasterCerate = "Edit";
                        this.IsShowEditButon = true;
                      
                        if (!this.IsNoHouses) {
                            this.LableHouseArea = this.NumbeofCreatedHouse + " of " +        this.HousesList.length + " houses created";

                        } else this.IsShowMarkASCompleted = false;

                        this.RefreshSharedManifiestoStatus();
                    }
                    else {
                        this.NumbeofCreatedHouse += 1;
                        this.LableHouseArea = this.NumbeofCreatedHouse + " of " + this.HousesList.length + " houses created";

                        if (this.NumbeofCreatedHouse == this.HousesList.length) {
                            this.IsShowMarkASCompleted = false;
                        }

                    }
                }
            });

    
    }


    EditShipment(houseEntity: HouseSL) {
        var entityId: string = !houseEntity ? this.ManifestSL.EntityId : houseEntity.EntityId;
        if (!AppTool.IsNullOrEmpty(entityId)) {
            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Shipment' });

                });
        }
      
    }

    ChangeStatusAgentSharedManifest() {
            var status = this.CurrentEntity.StatusCode == "WAIT" ? "CANC" : "WAIT";

            if (status == "CANC") {
                ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Decline Shared Manifests");
            }
            this.UpDateAgentSharedManifest(status);
        
    }

    UpDateAgentSharedManifest(status:string) {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving....");

        this.CurrentEntity.StatusCode = status;
        this.CurrentEntity.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this._aentSharedManifestPMService.update(this.CurrentEntity).subscribe(res => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.RefreshSharedManifiestoStatus();
        });
    }

    CheckIfAnyShipmentHaveMasterNumber(master: string, longMaster: string) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._sharedAgentManifestService.GetCheckIfAnyShipmentHaveMasterNumber(master, longMaster, SessionInfo.LoggedUserTenant).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                if (!AppTool.IsNullOrEmpty(myResponse.Result)) {
                    this.ValidationWarningsList = [];
                    this.ValidationWarningsList.push(" Master field already used in another shipment (" + myResponse.Result +")");
                }

            }
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });


    }

    MarkASCompleted() {
        if (this.CurrentEntity.StatusCode != "COMP") {
            this.CurrentEntity.StatusCode = "COMP";
            this.UpDateAgentSharedManifest("COMP");

        }

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private timerToken: any;
    private Retries: number = 0;

    RunSharedManifestHeaderComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.CurrentEntity, this.EntityList);

          
            });
    }

    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunSharedManifestHeaderComponent(), 1);
        }
    }

}



