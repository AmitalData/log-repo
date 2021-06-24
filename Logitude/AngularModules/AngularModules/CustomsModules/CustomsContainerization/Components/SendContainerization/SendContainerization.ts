import { Component, OnDestroy } from "@angular/core";
import { LogitudeWindow } from "../../../../Controls/Windows/LogitudeWindow";
import { GenericRequestParams } from "../../../../Customs/DataContract/RequestParams/GenericRequestParams";
import { CustomSendOptionsArgs, SendRequestVIA } from "../../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { ContainerizationResponseData } from "../../../../Customs/DataContract/ResponseData/ContainerizationResponseData";
import { ContainerizationPM } from "../../../../Customs/EntityPMs/ContainerizationPM";
import { ContainerizationPMService } from "../../../../Customs/Services/StandardPMs/ContainerizationPMService";
import { ContainerizationMessagesService } from "../../../../Customs/Services/WebServices/ContainerizationMessagesService";
import { ServiceResponse } from "../../../../Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "../../../../Infrastructure/EntityPMs/ObjectTablePM";
import { SessionLocator } from "../../../../Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "../../../../Infrastructure/Utilities/TextCodeTranslator";
import { CustomMessageProgressComponent, ShowProgressBarParams } from "../../../CustomsControls/Components/CustomMessageProgressComponent";


@Component({

    selector: 'SendContainerization',
    templateUrl: "SendContainerization.html",
})

export class SendContainerization implements OnDestroy {

    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    presendValidationsTitle: string;
    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    EntityPM: ContainerizationPM;
    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    _WorkWithService: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    SendContainerizationService: SendContainerizationService = new SendContainerizationService();
    constructor() {
    }

    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Send");
        this.SendContainerizationService.Run(args);
        return;
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }


    }
    reloadEvent: any;
    ButtonText: string;
    OnCustomSendOptionsButtonClick(event: CustomSendOptionsArgs) {
        this.SendContainerizationService.OnCustomSendOptionsButtonClick(event);
        return;
    }

    ngOnDestroy() {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }

        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
        if (this._WorkWithService) {
            this.SendContainerizationService.ngOnDestroy();
            return;
        }
    }
}

export class SendContainerizationService implements OnDestroy {

    ObjectTable: ObjectTablePM;
    EntityPM: ContainerizationPM;
    ValidationErrors: string[];
    presendValidationsTitle: string;
    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    private CurrentSession = SessionLocator.SelectedSession;
    containerizationMessagesService: ContainerizationMessagesService = new ContainerizationMessagesService();
    ResponseData: ContainerizationResponseData;
    containerizationPMService: ContainerizationPMService = new ContainerizationPMService();

    constructor() {

    }

    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.Listen();
    }
    Listen() {
        if (this.CurrentSession.CurrentEditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }


    }



    OnCustomSendOptionsButtonClick(event) {
        this.EntityPM.IsChange = false;
        this.containerizationPMService.update(this.EntityPM).subscribe((response: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            var params: GenericRequestParams = new GenericRequestParams();
            params.Tenant = SessionLocator.Tenant;
            params.RequestVIA = event.RequestVIA;
            params.ForcePersonalSign = event.ForcePersonalSign;
            params.LoggingEnabled = true;
            params.LoggingEntityId = this.EntityPM.Id;
            params.LoggingUserId = SessionLocator.LoggedUserId;
            params.RequestName = "המכלה";
            params.ResponseName = "המכלה תשובה"
            CustomMessageProgressComponent
                .ShowProgressBar(params.PBId,
                    "שליחת המכלה", false)
                .then((res) => {
                    this.ResponseData = res;
                }
                ).catch((err) => {
                    this.ValidationErrors.push(err);
                    this.FillValidationErrors("Errors");
                });
            this.containerizationMessagesService.SendContainerization(params)
                .subscribe((myServiceResponse: ServiceResponse) => {
                });

        });
    }

    


    public OnSuccessSendMethod: (response: any) => void;

    SendDeclaration() {
    }

    FillValidationErrors(title: string) {


        this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px'; // بدك تقيم 72 
        var windowTitle = title;

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

    StopMyBusyIndicator() {
        this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
    }
    StartMyBusyIndicator(mess) {
        this.CurrentSession.StartBusyIndicator(mess);

    }
    ngOnDestroy() {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }

        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
    }
}
