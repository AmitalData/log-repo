declare var window: any;
declare var System: any;
import {Component, Output, EventEmitter, Input} from '@angular/core';
import {ObjectTablePM} from '../../../../../Infrastructure/EntityPMs/ObjectTablePM'
import {MenuButtonPM} from '../../../../../Infrastructure/EntityPMs/MenuButtonPM'
import {MenuButtonGroupPM} from '../../../../../Infrastructure/EntityPMs/MenuButtonGroupPM'
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator'
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper'
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools'
import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ClientPM} from '../../../../../Customs/EntityPMs/ClientPM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {CustomsExchangeRateExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService';
import {CustomsExchangeRatePM} from '../../../../../Customs/EntityPMs/CustomsExchangeRatePM';
import {DeclarationValidator} from '../../../../../Customs/Validators/DeclarationValidator';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {GenericRequestParams} from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import {SendRequestVIA} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {CustomMessageProgressComponent, ShowProgressBarParams} from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import {SupplierInvoicePMService} from  '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { AmitalGatewayUtil } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../../Customs/Controller/UnifreightController';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {INF_MSG_GenericResponseData}  from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData'; 
import {MANIFESTRequestRequestParams} from '../../../../../Customs/DataContract/RequestParams/MANIFESTRequestRequestParams'; 
import {MANIFESTRequestResponseData} from  '../../../../../Customs/DataContract/ResponseData/MANIFESTRequestResponseData'; 
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';

@Component({
    moduleId: module.id,
    selector: 'SendManifestComponent',
    templateUrl: "SendManifestComponent.html",
})

export class SendManifestComponent {
    _SendManifestService: SendManifestService
    
    constructor() {
        this._SendManifestService = new SendManifestService()
        this.ButtonText = "שלח מצהר";
    }
   
 
    ButtonText: string;
    Run(args: any) {
        
        this._SendManifestService.Run(args);
    }

  
    OnCustomSendOptionsButtonClick(event) {
        this._SendManifestService.OnCustomSendOptionsButtonClick(event);
    }
                                                                      
}





export class SendManifestService {

    EntityPM: DeclarationPM;
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    presendValidationsTitle: string;
    DeclarationService: DeclarationWebService;
    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    public entityResourceService: EntityResourceService = new EntityResourceService();

    declarationPMService: DeclarationPMService = new DeclarationPMService();
    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    responseData: MANIFESTRequestResponseData;

    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.entityResourceService.getEntityResourceByTableName("Customs.CourierDeclaration").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(response => {
                });

            });
        });
    }

    CourierWorksheetmode: boolean = false;
    ButtonText: string;
    Run(args: any) {
        this.CourierWorksheetmode = args.CourierWorksheetmode;
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.DeclarationService = new DeclarationWebService();
        this.ButtonText = "שלח מצהר";
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

    reloadEvent: any;

    OnCustomSendOptionsButtonClick(event) {
        this.CurrentSession.StartBusyIndicator("");
        this.RequestVIA = event.RequestVIA;
        this.Option = event.Option;
        this.ForcePersonalSign = event.ForcePersonalSign;
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            // this.CurrentSession.CurrentEditComponent.SaveChanges("");

            var firstInvoice: SupplierInvoicePM = this.EntityPM.SupplierInvoices.filter(d => d.SequenceNumeric == 1)[0];



            //if (firstInvoice && firstInvoice.InsruancePercentage) {
            //    this.CalculateInsuranceAmount(firstInvoice);

            //}

            //else {

            if (this.CourierWorksheetmode) {
                this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                return;
            }

            this.declarationPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                if (myResponse.HasError) {
                    this.StopMyBusyIndicator();
                    this.ValidationErrors = myResponse.ErrorsArray;
                    this.FillValidationErrors(this.presendValidationsTitle);
                }

                else {
                    this.EntityPM = myResponse.Result;
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.reloadEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                        this.reloadEvent.unsubscribe();
                        if (isLoadSuccess) {
                            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                            let asREUSEService = true;
                            if (asREUSEService) {
                                this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                            } else {
                                this.CurrentSession.StartBusyIndicator("");
                                this.DeclarationService.PostSendDeclarationChecksAndPrecalculations(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
                                    if (!response.Result.HasError) {
                                        this.CheckRequiredFields();
                                    }
                                    else {
                                        this.StopMyBusyIndicator();
                                        this.ValidationErrors = response.Result.ErrorMessages;
                                        this.FillValidationErrors(TextCodeTranslator.Translate(response.Result.ErrorsType));
                                    }
                                });
                            }
                        }
                    });


                }
            });



        }
        else {
            this.StopMyBusyIndicator();
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    }
    PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields() {
        this.CurrentSession.StartBusyIndicator("");
        this.DeclarationService.PostSendDeclarationChecksAndPrecalculations(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.Result.HasError) {
                this.CheckRequiredFields();
            }
            else {
                this.StopMyBusyIndicator();
                this.ValidationErrors = response.Result.ErrorMessages;
                this.FillValidationErrors(TextCodeTranslator.Translate(response.Result.ErrorsType));
            }
        });
    }
    CheckRequiredFields() {
        this.DeclarationService.GetRequiredFieldsForCourierDeclaration(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            var errorsList = null;
            if (response.Result) {
                errorsList = response.Result.RequiredFields;
            }
            if (errorsList) {
                if (errorsList.length == 0) {
                    //this.SendDeclaration();
                    this.CheckCourierMaster();
                }

                else {
                    this.ValidationErrors = this.GetRequiredErrorsList(errorsList);
                    this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
                }
            }
            else {
                //this.SendDeclaration();
                this.CheckCourierMaster();
            }
        });
    }

    CheckCourierMaster() {
        this.StartMyBusyIndicator("");// avoid resend
        this.DeclarationService.GetMAWBCourierMasterByDeclaration(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            var result = myResponse.Result;

            if (AppTool.IsNullOrEmpty(result)) {
                this.ValidationErrors.push("ההצרה אינה מקושרת לבלדר ראשי");
                this.FillValidationErrors(this.presendValidationsTitle);
            }
            else {
                this.SendManifest();
            }
        });
    }
    public OnSuccessSendMethod: (response: any) => void;
    //SendDeclaration() {
    SendManifest() {
        var sendParams: MANIFESTRequestRequestParams = new MANIFESTRequestRequestParams();
        sendParams.Tenant = SessionLocator.Tenant;
        sendParams.DeclarationId = this.EntityPM.Id;
        sendParams.LoggingEnabled = true;
        sendParams.LoggingEntityId = this.EntityPM.Id;
        sendParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        sendParams.LoggingObjectTableId = this.ObjectTable.Id;
        sendParams.LoggingUserId = SessionLocator.LoggedUserId;
        sendParams.RequestName = "Declaration Request";
        sendParams.ResponseName = "Declaration Response";
        sendParams.RequestVIA = this.RequestVIA;
        sendParams.ForcePersonalSign = this.ForcePersonalSign;

        //let myShowProgressBarParams = new ShowProgressBarParams();
        //myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
        //    (response: any) => {
        //        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //            .then(cmpRef => {
        //                cmpRef.instance.ComponentRef = cmpRef;
        //                cmpRef.instance.Run({
        //                    SelectedTabCode: "DCCA",
        //                    EntityId: this.EntityPM.Id,
        //                    ObjectTableName: "Customs.Declaration",
        //                });
        //                cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
        //                    .subscribe(myResult => {
        //                        var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
        //                        myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
        //                        console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
        //                    });
        //            });

        //    };
        let myShowProgressBarParams: ShowProgressBarParams = null;
        if (this.CourierWorksheetmode) {
            myShowProgressBarParams = new ShowProgressBarParams();
            myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
                (response: any) => {

                    let myResponseData = response;
                    if (myResponseData) {
                        if (myResponseData.HasException || !myResponseData.Succeeded) {
                            //do not close Win !!

                        } else {

                            //if OK then  close Win !!
                            this.OnSuccessSendMethod(myResponseData);
                        }
                    }
                };
        }
        CustomMessageProgressComponent
            .ShowProgressBar(sendParams.PBId,
            "שליחת מצהר", false, myShowProgressBarParams)
            .then((res) => {
                this.responseData = res;
                if (this.CourierWorksheetmode) {
                } else {
                    if (this.responseData && this.responseData.ContinueProcessInBackground) {
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    else if (this.Option == 'WB' || this.Option == 'D') { // work around itzik shall fix the undefined problem.
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
                    myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                    this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DCCA";
                    this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                    var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
                    myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
            ).catch((err) => {
                this.StopMyBusyIndicator();
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        this.DeclarationService.PostSendManifest(sendParams).subscribe((response: ServiceResponse) => {
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            this.StopMyBusyIndicator();

        });

        //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //    .then(cmpRef => {
        //        cmpRef.instance.ComponentRef = cmpRef;
        //        cmpRef.instance.Run({
        //            SelectedTabCode: "DCCA",
        //            EntityId: this.EntityPM.Id,
        //            ObjectTableName: "Customs.Declaration",
        //        });
        //        cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
        //            .subscribe(myResult => {
        //                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
        //                myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
        //                console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
        //            });
        //    });
    }

    GetRequiredErrorsList(errorsList: any[]) {
        var errorsMessages: string[] = [];
        errorsList.forEach((error) => {
            if (!AppTool.IsNullOrEmpty(error.CustomMessageError)) {

                errorsMessages.push(error.CustomMessageError);

            }
            else {
                var table = window.ObjectTables.filter(d => d.Name === error.TableName)[0];
                var field = window.ObjectFields.filter(d => d.FieldName == error.FieldName && d.ObjectTableId == table.Id)[0];

                if (error.TableName == "Customs.SupplierInvoiceItem" && !AppTool.IsNullOrEmpty(error.EntityReference2)) {
                    error.EntityReference = error.EntityReference + " (חשבון " + error.EntityReference2 + " )";
                }
                var message = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");

              var fieldName = error.FieldName;
              if (field) {
                fieldName = TextCodeTranslator.Translate(field.FullNameTextCodeCode);
              }


                var tableName = TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;

    }

    OnAddEditWindowClosed(event) {
        this.ValidationErrors = [];
    }

    FillValidationErrors(title: string) {


        this.StopMyBusyIndicator();
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

        logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
    }


    StopMyBusyIndicator() {
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StopBusyIndicator();
        } else {
            this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        }
    }
    StartMyBusyIndicator(mess) {
        ///this.StartMyBusyIndicator("");
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StartBusyIndicator(mess);
        } else {
            this.CurrentSession.CurrentEditComponent.StartBusyIndicator(mess);
        }
    }

}
