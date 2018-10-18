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
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';




// Send Request
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';


import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';

@Component({
    moduleId: module.id,
    templateUrl: './VehiclesSelectionComponent.html',
})

export class VehiclesSelectionComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: VehiclePM;
    public ObjectTableName: string = "Customs.Vehicle";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValdationErrorList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;

    //RequestParams: VehicleInsertUpdateDeleteMessageRequestParams;
    ResponseData: INF_MSG_GenericResponseData;
    //VehicleMessagesService: VehicleMessagesService = new VehicleMessagesService();


    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.Listen();
            });
        });

    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.DisplayOnlyCheck();
                    }
                })
            );

            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            //this.DisplayOnlyCheck();
                        }
                    }
                })
            );
        }
    }

    SetTabArgs(args: any, valdationErrorList: any[]) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);


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
        SessionLocator.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    }
    //#endregion
}

