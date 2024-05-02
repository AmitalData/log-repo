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
import { CLAIM_2340_ClaimRequestRequestParams } from '../../../../Customs/DataContract/RequestParams/CLAIM_2340_ClaimRequestRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { AmitalGatewayUtil } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../Customs/Controller/UnifreightController';
import { ClaimWebService } from '../../../../Customs/Services/WebServices/ClaimWebService';
import { ClaimPMService } from '../../../../Customs/Services/StandardPMs/ClaimPMService';
import { ClaimPM } from '../../../../Customs/EntityPMs/ClaimPM';

@Component({

    selector: 'SendClaimComponent',
    templateUrl: "SendClaimComponent.html",
})

export class SendClaimComponent {
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[] = [];
    EntityPM: ClaimPM;

    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    ObjectTableName = "Customs.Claim";
    presendValidationsTitle: string;
    sendClaimsRelatedEntitiesList: string[] = [];

    ClaimWebService: ClaimWebService = new ClaimWebService();
    ClaimPMService: ClaimPMService = new ClaimPMService();

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
                        this.UpdateEntityWithSendClaimsRelatedEntity();

                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.UpdateEntityWithSendClaimsRelatedEntity();
                    }
                });
            }
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        Validator.TryValidateObject(this.EntityPM, "Customs.Claim", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            this.SaveEntityChanges(customSendOptionsArgs, false);
        }
        else {
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    }

    sendList: string[] = [];
    RetrieveIsSendClaimsRelatedEntity() {
        this.sendList = [];
        if (this.EntityPM.ClaimsRelatedEntities != null && this.EntityPM.ClaimsRelatedEntities.length > 0) {
            for (let claimsRelatedEntityItem of this.EntityPM.ClaimsRelatedEntities) {
                if (claimsRelatedEntityItem.IsSendClaimsRelatedEntity == true) {
                    this.sendList.push(claimsRelatedEntityItem.EntityCounterKey.toString());
                }
            }
        }
    }
    UpdateEntityWithSendClaimsRelatedEntity() {
        var isDirty = false;
        if (this.sendList != null) {
            this.EntityPM.ClaimsRelatedEntities.forEach((item) => {
                if (this.sendList.includes(item.EntityCounterKey.toString())) {
                    item.IsSendClaimsRelatedEntity = true;
                }
                else {
                    item.IsSendClaimsRelatedEntity = false;
                    isDirty = true;
                }
            });
            if (isDirty) {
                this.EntityPM.IsDirty = false
            }
        }
    }

    reloadEvent: any;
    public SaveEntityChanges(customSendOptionsArgs, isDelete: boolean) {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            return;
        }
        this.RetrieveIsSendClaimsRelatedEntity();
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.ClaimPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            if (myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors = myResponse.ErrorsArray;
                this.FillValidationErrors(this.presendValidationsTitle);
            }

            else {
                this.EntityPM = myResponse.Result;
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.UpdateEntityWithSendClaimsRelatedEntity();
                    this.reloadEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                        this.reloadEvent.unsubscribe();
                        if (isLoadSuccess) {
                            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                            this.UpdateEntityWithSendClaimsRelatedEntity();
                            this.CurrentSession.StartBusyIndicator("");

                            if (this.PostSendPaymentOrderChecksAndPrecalculations() == true) {
                                this.CheckRequiredFields();
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
        return true;
        //if (this.EntityPM.PaymentOrderSelectedLabel == "" && AppTool.IsNullOrEmpty(this.EntityPM.AccountingCustomFile)) {
        //    this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
        //    return false;
        //}
        //var sumOfPaymentMethods = 0;
        //if (this.EntityPM.PaymentOrderMethods) {
        //    this.EntityPM.PaymentOrderMethods.forEach((itemLine) => {
        //        sumOfPaymentMethods = sumOfPaymentMethods + itemLine.Amount;
        //    });
        //}

        //var deffirence = this.EntityPM.PaymentOrderLeftAmount - sumOfPaymentMethods;
        //if (deffirence == 0) {
        //    return true;
        //}
        //else {
        //    this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.PaymentOrderLeftAmountDifference"));
        //    return false;
        //}
    }

    CheckRequiredFields() {
        this.ClaimWebService.GetRequiredFieldsForClaim(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            var errorsList = response.Result.RequiredFields;
            if (errorsList.length == 0) {
                this.InstructionSendToMehes();
                return;
            }
            else {
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors = this.GetRequiredErrorsList(errorsList);
                this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
            }
        });
    }

    GetRequiredErrorsList(errorsList: any[]) {
        var errorsMessages: string[] = [];
        errorsList.forEach((error) => {
            if (!AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                if (error.CustomMessageError.indexOf("specialerror") > -1) {
                    var ErrorMessage = "";
                    var errorArr = error.CustomMessageError.split(',');
                    ErrorMessage = errorArr[1] + TextCodeTranslator.Translate(errorArr[2]);
                    errorsMessages.push(ErrorMessage);
                }
                else {
                    errorsMessages.push(TextCodeTranslator.Translate(error.CustomMessageError));
                }
            }
            else {
                var table = window.ObjectTables.filter(d => d.Name === error.TableName)[0];
                var field = window.ObjectFields.filter(d => d.FieldName == error.FieldName && d.ObjectTableId == table.Id)[0];
                var message = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
                var fieldName = TextCodeTranslator.Translate(field.FullNameTextCodeCode);
                var tableName = TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;

    }

    InstructionSendToMehes() {
        var counter: number = 0;
        this.sendClaimsRelatedEntitiesList = [];
        this.UpdateEntityWithSendClaimsRelatedEntity();
        if (this.EntityPM.ClaimsRelatedEntities != null && this.EntityPM.ClaimsRelatedEntities.length > 0) {
            for (let claimsRelatedEntityItem of this.EntityPM.ClaimsRelatedEntities) {
                if (claimsRelatedEntityItem.IsSendClaimsRelatedEntity == true) {
                    this.sendClaimsRelatedEntitiesList.push(claimsRelatedEntityItem.EntityCounterKey.toString());
                }
                if (!AppTool.IsNullOrEmpty(claimsRelatedEntityItem.TapagNumber)) {
                    counter++;
                }
            }
            if (this.sendClaimsRelatedEntitiesList == null || (this.sendClaimsRelatedEntitiesList != null && this.sendClaimsRelatedEntitiesList.length == 0)) {
                var errorMessage: string = TextCodeTranslator.Translate("Customs.General.O.SelectOneEntityAtLeast");
                if (this.EntityPM.ClaimsRelatedEntities.length == counter) {
                    errorMessage = TextCodeTranslator.Translate("Customs.General.O.CanotSendReceivedClaim");
                }
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                var winConfirmation: ConfirmWindow = new ConfirmWindow();
                winConfirmation.Title = TextCodeTranslator.Translate("Customs.Claim.G.RelatedEntitiesCheck");
                winConfirmation.Width = 250;
                winConfirmation.Height = 150;
                winConfirmation.ShowNoButton = false;
                winConfirmation.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                //winConfirmation.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
                winConfirmation.ShowCancelButton = false;
                winConfirmation.Show(errorMessage);
                return;
            }
        }
        else {
            var winConfirmation: ConfirmWindow = new ConfirmWindow();
            winConfirmation.Title = TextCodeTranslator.Translate("Customs.Claim.G.RelatedEntitiesCheck");
            winConfirmation.Width = 250;
            winConfirmation.Height = 150;
            winConfirmation.ShowNoButton = false;
            winConfirmation.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            winConfirmation.ShowCancelButton = false;
            winConfirmation.Show(TextCodeTranslator.Translate("Customs.General.O.SelectOneEntityAtLeast"));
            return;
        }
        this.PreClaimSendChecks();
    }

    PreClaimSendChecks() {

        //var ClaimValidator = new ClaimValidator(entityPM);
        //ClaimValidator.PreClaimSendChecks();
        //if (ClaimValidator.ErrorCode.Count == 0) {
        //    //בעתיד לבדוק צרופות
        //    //LoadOperation customsDocument = this.context.Load(this.context.GetClaimDocumentListQuery(entityPM.Id, "Claim", TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
        //    //customsDocument.Completed += customsDocument_Completed;
        //    SendClaim();
        //    return;
        //}

        this.SendClaim();
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

    public SendClaim() {
        var currRequestParams = new CLAIM_2340_ClaimRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.AppicationId = this.EntityPM.Id;
        currRequestParams.ClaimsRelatedEntitiesList = this.sendClaimsRelatedEntitiesList;
        currRequestParams.RequestName = "Claim Request";
        currRequestParams.ResponseName = "Claim Response";
        currRequestParams.ForcePersonalSign = this.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId,
                "שליחת תביעה", false)
            .then((res) => {
                console.log(res);
                this.ResponseData = res;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            ).catch((err) => {
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        var myClaimWebService = new ClaimWebService();

        myClaimWebService.PostSendClaimRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }
}
