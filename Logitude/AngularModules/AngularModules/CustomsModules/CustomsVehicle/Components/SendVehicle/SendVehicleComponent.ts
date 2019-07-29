declare var window: any;
declare var System: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator'
import { AppTool, DateTool } from '../../../../Infrastructure/Tools'
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { UpdateDeleteVehicleRequestParams } from    '../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from     '../../../../Infrastructure/Services/EntityPMService';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { VehicleExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService';

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
    _VehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();ehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();
    //------------------------------------------------------//

    ObjectTableName = "Customs.Vehicle";
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
        this.ValidationErrors = [];

        this.PostSendVehicleAndPrecalculations(customSendOptionsArgs);
    }

    public static SaveEntityChanges(customSendOptionsArgs, EntityPM: VehiclePM, isDelete: boolean) {
        EntityPM.Tenant = SessionLocator.Tenant;
        SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(EntityPM.Id)) {
            //this.CancelButtonClicked();
            return;
        }

        SessionLocator.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
        //SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges()
        let entityPMService = new EntityPMService();
        entityPMService.update("Customs.Vehicle", EntityPM).then((res: any) => {
            res.subscribe((myResponse: ServiceResponse) => {

                SessionLocator.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }

                else {
                    EntityPM = myResponse.Result;
                    if (AppTool.IsNullOrEmpty(EntityPM.Id)) {

                        var myErrors: string[] = [];
                        myErrors.push("this.EntityPM.Id is null");
                        SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList  = myErrors;
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
                                    SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(err);
                                    //this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                            myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                .subscribe((myServiceResponse: ServiceResponse) => {
                                    //SessionLocator.CurrentSession.StopBusyIndicator();

                                    //this.ResponseData = myServiceResponse.Result;
                                    //this.OnMassageDisplayMethod();
                                });
                        }

                    }
                }

            }, error => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                var myErrors: string[] = [];
                myErrors.push(error.message);
                SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;
                
            });
        });
        //}

    }

    FillValidationErrors() {
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        var windowTitle = TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
    }

    OnAddEditWindowClosed(event) {
        this.ValidationErrors = [];
    }

    PostSendVehicleAndPrecalculations(customSendOptionsArgs) {
        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._VehicleExtendedPMService.GetIsVehicleAttachmentNumberIsMoreThenAllow(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!AppTool.IsNullOrEmpty(response.Result) && response.Result == true) {
                SessionLocator.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors.push("לא ניתן לשלוח ריכבית עם מעל 5 מסמכים ");
                this.FillValidationErrors();
                return;
            }
            SendVehicleComponent.SaveEntityChanges(customSendOptionsArgs, this.EntityPM, false);
        });
    }
 

}
