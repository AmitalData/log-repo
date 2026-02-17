declare var window: any;
declare var System: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { MenuButtonPM } from '../../../../Infrastructure/EntityPMs/MenuButtonPM'
import { MenuButtonGroupPM } from '../../../../Infrastructure/EntityPMs/MenuButtonGroupPM'
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator'
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper'
import { AppTool, DateTool } from '../../../../Infrastructure/Tools'


import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { GenericRequestParams } from                '../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { UpdateDeleteVehicleRequestParams } from    '../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from     '../../../../Infrastructure/Services/EntityPMService';

import { AmitalGatewayUtil } from   '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../Customs/Controller/UnifreightController';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
@Component({
    moduleId: module.id,
    selector: 'SendVehicleComponent', 
    templateUrl: "SendVehicleComponent.html",
})

export class SendVehicleComponent {

    //-----------------Properties----------------------------//
    
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    EntityPM: VehiclePM;

    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    //------------------------------------------------------//
    ObjectTableName = "Customs.Vehicle";
    private static CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityPMService: EntityPMService) {

    }
    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        
        
        
    }
    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        SendVehicleComponent.SaveEntityChanges(customSendOptionsArgs, this.EntityPM,false);
    }

    public static SaveEntityChanges(customSendOptionsArgs, EntityPM: VehiclePM, isDelete: boolean) {
        EntityPM.Tenant = SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(EntityPM.Id)) {
            //this.CancelButtonClicked();
            return;
        }

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
        //this.CurrentSession.CurrentEditComponent.SaveChanges()
        let entityPMService = new EntityPMService();
        entityPMService.update("Customs.Vehicle", EntityPM).then((res: any) => {
            res.subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }

                else {
                    EntityPM = myResponse.Result;
                    if (AppTool.IsNullOrEmpty(EntityPM.Id)) {

                        var myErrors: string[] = [];
                        myErrors.push("this.EntityPM.Id is null");
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList  = myErrors;
                    } else {
                        if (customSendOptionsArgs == null) {
                            //this.CancelButtonClicked();
                        } else {
                            var currRequestParams = new UpdateDeleteVehicleRequestParams();///Force new GUID On Each Send !!
                            currRequestParams.LoggingEnabled = true;
                            currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                            currRequestParams.Tenant = SessionLocator.Tenant;

                            currRequestParams.VehicleId = EntityPM.Id;
                            currRequestParams.IsDelete = isDelete;



                            CustomMessageProgressComponent
                                .ShowProgressBar(currRequestParams.PBId,
                                "שליחת מסר עדכון פרטי רכב", false)
                                .then((res) => {
                                    console.log(res);
                                    //this.CancelButtonClicked();
                                }
                                ).catch((err) => {
                                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(err);
                                    //this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                            myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                .subscribe((myServiceResponse: ServiceResponse) => {
                                    //this.CurrentSession.StopBusyIndicator();

                                    //this.ResponseData = myServiceResponse.Result;
                                    //this.OnMassageDisplayMethod();
                                });
                        }

                    }
                }

            }, error => {
                this.CurrentSession.StopBusyIndicator();
                var myErrors: string[] = [];
                myErrors.push(error.message);
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;
                
            });
        });
        //}

    }

   
 

}
