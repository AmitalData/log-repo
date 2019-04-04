/// <reference path="sharedmanifeststarted.ts" />


import {Component, OnInit} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnerItem} from './SharedManifestStarted';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SharedManifestStarted } from './SharedManifestStarted';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';

@Component({
    moduleId: module.id,
    selector: 'SharedManifestEditAgentComponent',
    templateUrl: './SharedManifestEditAgentComponent.html',
    providers: [SharedAgentManifestService],
})

export class SharedManifestEditAgentComponent implements OnInit {
    public EntityPM: ShipmentPM;
    public DataContext: PartnerItem;
    public ObjectTableName: string = "Shipment";
    private isMyCustomer: boolean = false;
    public ValidationErrorsList: string[];
    IsSaveShipment: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService) {
        this.Listen();
    }

    ngOnInit() {
        this.DataContext.SetUIProperties();
    }



    Listen() {

        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    if (this.IsSaveShipment) {
                        this.Close();
                    }
                    this.IsSaveShipment = false;
                }


            });
        }
    }




    SetDataContext(dataContext: PartnerItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        this.Clone();
        this.IsAgentSharedManifests(dataContext);
    }



    IsAgentSharedManifests(partnerItem: PartnerItem) {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");

        this._sharedAgentManifestService.GetIsAgentSharedManifests(partnerItem.AgentId, partnerItem.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                partnerItem.IsDisableNextButton = myResponse.Result ? true : false;;
            }

        });
    }



    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        this.ValidationErrorsList = [];

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.S.Partners.Name")));
        }

        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.S.Partners.Reference1") + " max length is 50");
            }
        }

        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.S.Partners.Reference2") + " max length is 50");
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Saving...");
            this.DataContext.fatherComponent._agentSharedLogisticsKeyPMService.GetSingleByAgentId(this.EntityPM.AgentId).subscribe((myResponse: any) => {

                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {

                    var agentSharedKey: any = myResponse.Result;
                    if (agentSharedKey) {
                        switch (agentSharedKey.StatusCode) {
                            case "A":

                                if (this.EntityPM.ShipmentLevelCode == "C") {

                                    this._sharedAgentManifestService.GetCheckIfMasterShipmentHaveHouseWithOtherAgent(this.EntityPM.Id, this.EntityPM.AgentId, this.EntityPM.Tenant).subscribe((myResponse: any) => {
                                        this.CurrentSession.StopBusyIndicator();
                                        if (!myResponse.HasError) {
                                            if (myResponse.Result == true) {
                                                this.ValidationErrorsList.push("One of the houses has agent different from the master shipment");
                                            } else {
                                                this.CompleteSave();
                                            }
                                        }
                                    });
                                } else {
                                    this.CompleteSave();
                                }

                                break;

                            case "W":

                                this.ValidationErrorsList.push("Waiting for the Agent’s approval to enable sharing");

                                break;

                            case "I":
                                this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                                break;


                        }
                    }
                    else {
                        this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                    }
                }
                else {
                    this.ValidationErrorsList.push(myResponse.ErrorsArray[0]);
                }

            });
        }
    }

    CompleteSave() {
        if (this.ValidationErrorsList.length == 0) {
            if (this.isMyCustomer) {
                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.CustomerId != null) {
                        this.EntityPM.CustomerId = null;
                    }

                    if (this.EntityPM.CustomerName != null) {
                        this.EntityPM.CustomerName = null;
                    }

                    if (this.EntityPM.CustomerReference1 != null) {
                        this.EntityPM.CustomerReference1 = null;
                    }

                    if (this.EntityPM.CustomerReference2 != null) {
                        this.EntityPM.CustomerReference2 = null;
                    }

                    if (this.EntityPM.CustomerAddressId != null) {
                        this.EntityPM.CustomerAddressId = null;
                    }

                    if (this.EntityPM.CustomerContactId != null) {
                        this.EntityPM.CustomerContactId = null;
                    }

                    if (this.EntityPM.ShipmentCustomerTypeCode != null) {
                        this.EntityPM.ShipmentCustomerTypeCode = null;
                    }
                }

                else {
                    if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                        this.EntityPM.CustomerId = this.DataContext.PartnerId;
                    }

                    if (this.EntityPM.CustomerName != this.DataContext.Name) {
                        this.EntityPM.CustomerName = this.DataContext.Name;
                    }

                    if (this.EntityPM.CustomerAddressId != this.DataContext.AddressId) {
                        this.EntityPM.CustomerAddressId = this.DataContext.AddressId;
                    }

                    if (this.EntityPM.CustomerContactId != this.DataContext.ContactId) {
                        this.EntityPM.CustomerContactId = this.DataContext.ContactId;
                    }

                    if (this.EntityPM.CustomerReference1 != this.DataContext.Reference1) {
                        this.EntityPM.CustomerReference1 = this.DataContext.Reference1;
                    }

                    if (this.EntityPM.CustomerReference2 != this.DataContext.Reference2) {
                        this.EntityPM.CustomerReference2 = this.DataContext.Reference2;
                    }
                }


            }



            if (this.EntityPM.IsDirty) {
                this.IsSaveShipment = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else this.Close();



  
        }
    }


    Close() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
        this.CurrentSession.FireEvent("ShipmentPartnersChanged");
    }
    
    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PartnerId');
        this.myCloner.AddField('AddressId');
        this.myCloner.AddField('ContactId');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Note');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.DataContext.IsReseting = true;
        this.myCloner.RejectChanges();
        this.DataContext.IsReseting = false;
    }
}
