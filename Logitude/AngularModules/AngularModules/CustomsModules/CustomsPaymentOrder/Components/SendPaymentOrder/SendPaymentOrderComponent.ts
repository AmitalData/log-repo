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
import { GenericRequestParams } from '../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { AmitalGatewayUtil } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../Customs/Controller/UnifreightController';
import { PaymentOrderWebService } from '../../../../Customs/Services/WebServices/PaymentOrderWebService';
import { PaymentOrderPM } from '../../../../Customs/EntityPMs/PaymentOrderPM';
import { PaymentOrderPMService } from '../../../../Customs/Services/StandardPMs/PaymentOrderPMService';


@Component({
    
    selector: 'SendPaymentOrderComponent',
    templateUrl: "SendPaymentOrderComponent.html",
})

export class SendPaymentOrderComponent {
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[] = [];
    EntityPM: PaymentOrderPM;

    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    ObjectTableName = "Customs.PaymentOrder";
    presendValidationsTitle: string;

    PaymentOrderPMService: PaymentOrderPMService = new PaymentOrderPMService();

    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityPMService: EntityPMService) {
    }

    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
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

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        Validator.TryValidateObject(this.EntityPM, "Customs.PaymentOrder", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            this.SaveEntityChanges(customSendOptionsArgs, false);
        }
        else {
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    }

    reloadEvent: any;
    public SaveEntityChanges(customSendOptionsArgs, isDelete: boolean) {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            return;
        }

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.PaymentOrderPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                this.ValidationErrors = myResponse.ErrorsArray;
                this.FillValidationErrors(this.presendValidationsTitle);
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

                            if (this.PostSendPaymentOrderChecksAndPrecalculations() == true) {
                                this.SendPaymentOrder();
                            }
                            else {
                                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                this.FillValidationErrors(this.presendValidationsTitle);
                            }
                        }
                    });
                }
            }
        });
    }

    public PostSendPaymentOrderChecksAndPrecalculations() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.AccountingCustomFile)) {
            this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
            return false;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.AccountingCustomFile) && this.EntityPM.AccountingCustomFile.startsWith("R")) {
            this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.NoPayWithRAccountingCard"));
            return false;
        }

        var sumOfPaymentMethods = 0;
        if (this.EntityPM.PaymentOrderMethods) {
            this.EntityPM.PaymentOrderMethods.forEach((itemLine) => {
                sumOfPaymentMethods = sumOfPaymentMethods + itemLine.Amount;
            });
        }

        var deffirence = this.EntityPM.PaymentOrderLeftAmount - sumOfPaymentMethods;
        if (deffirence == 0) {
            return true;
        }
        else {
            this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.PaymentOrderLeftAmountDifference"));
            return false;
        }
    }

    FillValidationErrors(title: string) {
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
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

    public SendPaymentOrder() {
        var currRequestParams = new GenericRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.AppicationId = this.EntityPM.Id;
        currRequestParams.RequestName = "send payment order request";
        currRequestParams.ResponseName = "send  payment order  response";
        currRequestParams.ForcePersonalSign = this.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת בקשת תשלום הוראה", false)
            .then((res) => {
                console.log(res);
                this.ResponseData = res;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            ).catch((err) => {
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        var myPaymentOrderWebService = new PaymentOrderWebService();

        myPaymentOrderWebService.PostSendPaymentOrderRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                //this.CurrentSession.StopBusyIndicator();
                //OnSendCompleted(); //to check refresh !!!! ????
                //this.BuildProtestsList();
                //RefreshProperties();
            });

    }
}
