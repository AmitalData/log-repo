declare var window: any;
declare var System: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator'
import { AppTool, DateTool } from '../../../../Infrastructure/Tools'
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { UpdateDeleteVehicleRequestParams } from '../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { VehicleExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService';

@Component({
    
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
    _VehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();
    //------------------------------------------------------//

    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    private _CurrentSession = SessionLocator.SelectedSession;

    ObjectTableName = "Customs.Vehicle";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityPMService: EntityPMService) {

    }

    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;

        this.Listen();
    }

    Listen() {
        if (this._CurrentSession.CurrentEditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this._CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this._CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this._CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this._CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        this.ValidationErrors = [];

        this.PostSendVehicleAndPrecalculations(customSendOptionsArgs);
    }

    private reloadEvent: any;
    public SaveEntityChanges(customSendOptionsArgs, entityPM: VehiclePM, isDelete: boolean) {
        this.EntityPM = entityPM;
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            //this.CancelButtonClicked();
            return;
        }

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        let entityPMService = new EntityPMService();
        entityPMService.update("Customs.Vehicle", this.EntityPM).then((res: any) => {
            res.subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }
                else {
                    this.EntityPM = myResponse.Result;

                    if (this.CurrentSession.CurrentEditComponent) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.reloadEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                            this.reloadEvent.unsubscribe();
                            if (isLoadSuccess) {
                                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                this.CurrentSession.StartBusyIndicator("");

                                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                                    var myErrors: string[] = [];
                                    myErrors.push("this.EntityPM.Id is null");
                                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;
                                }
                                else {
                                    if (customSendOptionsArgs == null) {
                                        //this.CancelButtonClicked();
                                    } else {
                                        var currRequestParams = new UpdateDeleteVehicleRequestParams();///Force new GUID On Each Send !!
                                        currRequestParams.LoggingEnabled = true;
                                        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                                        currRequestParams.Tenant = SessionLocator.Tenant;

                                        currRequestParams.VehicleId = this.EntityPM.Id;
                                        currRequestParams.IsDelete = isDelete;

                                        CustomMessageProgressComponent
                                            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                                                "שליחת מסר עדכון פרטי רכב", false)
                                            .then((res) => {
                                                console.log(res);
                                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                            }
                                            ).catch((err) => {
                                                this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(err);
                                            });

                                        var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                                        myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                            .subscribe((myServiceResponse: ServiceResponse) => {
                                            });
                                    }
                                }
                            }
                        });
                    }

                }
            }, error => {
                this.CurrentSession.StopBusyIndicator();
                var myErrors: string[] = [];
                myErrors.push(error.message);
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;

            });
        });
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
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }

    OnAddEditWindowClosed(event) {
        this.ValidationErrors = [];
    }

    PostSendVehicleAndPrecalculations(customSendOptionsArgs) {
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._VehicleExtendedPMService.GetIsVehicleAttachmentNumberIsMoreThenAllow(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!AppTool.IsNullOrEmpty(response.Result) && response.Result == true) {
                SessionLocator.SelectedSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors.push("לא ניתן לשלוח מסר הקמת רכבית כאשר מקושרים יותר מ-5 מסמכים ");
                this.FillValidationErrors();
                return;
            }
            this.SaveEntityChanges(customSendOptionsArgs, this.EntityPM, false);
        });
    }


}
