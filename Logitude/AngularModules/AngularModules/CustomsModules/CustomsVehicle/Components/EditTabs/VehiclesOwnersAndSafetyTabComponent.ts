declare var window: any;
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { VehicleSafetyAccessoryPM } from '../../../../Customs/EntityPMs/VehicleSafetyAccessoryPM';
import { VehicleOwnerPM } from '../../../../Customs/EntityPMs/VehicleOwnerPM';
import { ClientList } from '../../../../Customs/EntityLists/ClientList';



// Send Request
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';


import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './VehiclesOwnersAndSafetyTabComponent.html',
})

export class VehiclesOwnersAndSafetyTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: VehiclePM;
    public ObjectTableName: string = "Customs.Vehicle";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValdationErrorList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;
    Loaded: boolean = false;

    //RequestParams: VehicleInsertUpdateDeleteMessageRequestParams;
    ResponseData: INF_MSG_GenericResponseData;
    //VehicleMessagesService: VehicleMessagesService = new VehicleMessagesService();
    public SafetiesList: ObservableCollection;
    public OwnersList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.SafetiesList = new ObservableCollection([]);
        this.OwnersList = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.VehicleOwner").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.VehicleSafetyAccessory").subscribe(response => {
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.EntityPM = this.entityArgs.EntityPM;

                this.OnEditTabSelected();
                

                this.Loaded = true;

                this.Listen();
            });
        });

    }

    OnEditTabSelected() {
        this.SafetiesList.Clear();
        this.OwnersList.Clear();
        this.EntityPM.VehicleSafetyAccessories.forEach((item) => this.SafetiesList.Insert(item));
        this.EntityPM.VehicleOwners.forEach((item) => this.OwnersList.Insert(item));
        this.CheckErrorState();
    }

    DeleteOwnersList(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            
            this.EntityPM.RemoveVehicleOwner(item);
            this.OwnersList.Remove(item);

        }
        this.CheckErrorState();
    } 
    DeleteSafetiesList(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            
            this.EntityPM.RemoveVehicleSafetyAccessory(item);
            this.SafetiesList.Remove(item);
        }
        this.CheckErrorState();
    } 
    CheckErrorState() {
        this.SetOwnersErrorMessage();
        this.SetSafetiesErrorMessage();
    }
    VehicleOwnerChanged(item, $event) {
        let myClientList: ClientList = $event;
        let myVehicleOwnerPM: VehicleOwnerPM = item;
        if (!AppTool.IsNullOrEmpty(myClientList)) {
            myVehicleOwnerPM.ClientName = myClientList.FullName;
            myVehicleOwnerPM.FirstName = myClientList.LocalFirstName;
            if (AppTool.IsNullOrEmpty(myClientList.LocalFirstName) && AppTool.IsNullOrEmpty(myClientList.LocalLastName)) {
                myVehicleOwnerPM.LastNameOrCorporationName = myClientList.LocalCorporationName;
            } else {
                myVehicleOwnerPM.LastNameOrCorporationName = myClientList.LocalLastName;
            }

        } else {
            myVehicleOwnerPM.LastNameOrCorporationName = null;
            myVehicleOwnerPM.ClientName = null;
            myVehicleOwnerPM.FirstName = null;
        }
        

    }
    SetVehicleSafeAccessoryInstlType(item, lookupEntity) {
        if (!AppTool.IsNullOrEmpty(lookupEntity)) {
            item.VehicleSafAccessoryInstlTypName = lookupEntity.LocalName;
        }
        else
        {
            item.VehicleSafAccessoryInstlTypName = null;
        }
    }
    SetVehicleSafetyAccessoryTypeLocalName(item, lookupEntity) {
        //alert("SetLocalName");
        ///console.log(lookupEntity);
        if (!AppTool.IsNullOrEmpty(lookupEntity)) {
            item.VehicleSafetyAccessoryName = lookupEntity.LocalName;
        }
        else {
            item.VehicleSafetyAccessoryName = null;
        }
    }


    _OwnersErrorMessage: String;
    SetOwnersErrorMessage() {
        this._OwnersErrorMessage = null;
        if (this.EntityPM.VehicleOwners.length >= 10) {
            this._OwnersErrorMessage = TextCodeTranslator.Translate("Customs.Vehicle.O.CantEnterMore");
            
        }
    }

    AddOwnersList() {
        this.CheckErrorState(); if (this._OwnersErrorMessage) return;

        var newVehicleOwnerPM = new VehicleOwnerPM(this.EntityPM);
        newVehicleOwnerPM.Tenant = this.EntityPM.Tenant;
        newVehicleOwnerPM.LineNumber = (ArrayTool.Max(this.EntityPM.VehicleSafetyAccessories, "LineNumber") + 1);
        //this.SafetiesList.Clear();
        this.OwnersList.Insert(newVehicleOwnerPM)
        this.EntityPM.AddVehicleOwner(newVehicleOwnerPM);
    }
    _SafetiesErrorMessage: String;
    SetSafetiesErrorMessage() {
        this._SafetiesErrorMessage = null;
        if (this.EntityPM.VehicleSafetyAccessories.length >= 10) {
            this._SafetiesErrorMessage = TextCodeTranslator.Translate("Customs.Vehicle.O.CantEnterMore");

        }
    }
    AddRowSafetiesList() {
        //alert("AddRowSafetiesList");
        this.CheckErrorState(); if (this._SafetiesErrorMessage) return;
        var newVehicleSafetyAccessoryPM = new VehicleSafetyAccessoryPM(this.EntityPM);
        newVehicleSafetyAccessoryPM.Tenant = this.EntityPM.Tenant;
        newVehicleSafetyAccessoryPM.LineNumber = (ArrayTool.Max(this.EntityPM.VehicleSafetyAccessories, "LineNumber")+1);
        //this.SafetiesList.Clear();
        this.SafetiesList.Insert(newVehicleSafetyAccessoryPM)
        this.EntityPM.AddVehicleSafetyAccessory(newVehicleSafetyAccessoryPM);
    }
    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.OnEditTabSelected();
                }
                })
                );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.DisplayOnlyCheck();
                        this.OnEditTabSelected();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            //this.DisplayOnlyCheck();
                            this.OnEditTabSelected();
                        }
                    }
                })
            );
        }
    }

    public SetTabArgs(args: any, valdationErrorList: any[]=null) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        ///console.log("EntityPM", this.EntityPM);

        this.OnEditTabSelected();

        this.SetFieldsEditability();
    }


    SetFieldsEditability() {
        //this.UIProperties.SetEnabled("VehicleTypeCode", this.ObjectTableName, this.IsNewEntity);
        //this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    }



    //#endregion




    private checkForChassisNumberOp_Completed(exists: boolean) {

        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            //this.InvalidChassisNumber = true;
        }
        else {
            //this.InvalidChassisNumber = false;
        }
    }

    ///#region Properties



    public SendButtonsVisibility: boolean = false;




    //#endregion

    line = 0;

    //#region Send + Delete
    SendButtonClicked() {
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs

        // validate Vehicle
        Validator.TryValidateObject(this.EntityPM, "Customs.Vehicle", errors);

        //if (this.EntityPM.VehicleCommunications.length == 0) {
        //    errors.push(TextCodeTranslator.Translate("Customs.Vehicle.O.RequierdCommunication"));
        //} else {
        //    // validate Vehicle communication items
        //    this.EntityPM.VehicleCommunications.forEach((item) => {
        //        Validator.TryValidateObject(item, "Customs.VehicleCommunication", errors);
        //    });
        //}

        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
            // send request
            //this.SendRequest(false);
        }

    }




    OnSendCompleted() {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    }

    ApplyDeleteVehicle() {
        this.IsDelete = false;

        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    }

    SetPassportTypeLocalName(item, lookupEntity) {
        if (!AppTool.IsNullOrEmpty(lookupEntity)) {
            item.ImporterPassportTypeName = lookupEntity.LocalName;
        }
        else
        {
            item.ImporterPassportTypeName = null;
        }
    }

    SetPassportCountryLocalName(item, lookupEntity) {
        if (!AppTool.IsNullOrEmpty(lookupEntity)) {
            item.PassCountryName = lookupEntity.LocalName;
        }
        else {
            item.PassCountryName = null;
        }
    }
    //#endregion
}

