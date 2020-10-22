


declare var window: any;
declare var System: any;
import {Component, Output, EventEmitter, Input,OnDestroy} from '@angular/core';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM'
import {MenuButtonPM} from '../../../../../Infrastructure/EntityPMs/MenuButtonPM'
import {MenuButtonGroupPM} from '../../../../../Infrastructure/EntityPMs/MenuButtonGroupPM'
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator'
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper'
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools'
import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ClientPM } from '../../../../../Customs/EntityPMs/ClientPM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {CustomsExchangeRateExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService';
import {CustomsExchangeRatePM} from '../../../../../Customs/EntityPMs/CustomsExchangeRatePM';
import {DeclarationValidator} from '../../../../../Customs/Validators/DeclarationValidator';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {GenericRequestParams} from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import {SendRequestVIA, CustomSendOptionsArgs, TestCase} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {CustomMessageProgressComponent, ShowProgressBarParams} from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import {SupplierInvoicePMService} from  '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../../Customs/Controller/UnifreightController';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
////// ../edittabs/supplierinvoices/addeditsupplierinvoicecomponent.ts" />
import {CustomsDocumentPM} from '../../../../../Customs/EntityPMs/CustomsDocumentPM';
import { AnalyzeUnifreightInsuranceService } from '../../../DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import { EntityPMService } from '../../../../../Infrastructure/Services/EntityPMService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';

@Component({
    
    selector: 'SendDeclarationComponent',
    templateUrl: "SendDeclarationComponent.html",
})

export class SendDeclarationComponent implements OnDestroy {

    //-----------------Properties----------------------------//
    EntityPM: DeclarationPM;
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    presendValidationsTitle: string;
    DeclarationService: DeclarationWebService;
    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();
    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    _SupplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    _SendDeclarationService: SendDeclarationService = new SendDeclarationService();
    _WorkWithService: boolean = true;
     //------------------------------------------------------//
    private CurrentSession = SessionLocator.SelectedSession;
    
    constructor() {

    }
    Run(args: any) {
        this.EntityPM = args.EntityPM;
         if (!this.EntityPM.IsCourierDeclaration) {
            this.ButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Send");
        }
        else {
            this.ButtonText = "שלח הצהרה"; // TextCodeTranslator.Translate("Customs.Declaration.O.SendDeclaration");
        }

        if (this.EntityPM.IsAmendment == true) {
            this.ButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.SendAmendmentDeclaration");
        }
        if (this._WorkWithService) {
            this._SendDeclarationService.Run(args);
            return;
        }
       
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
        this._SendDeclarationService._TestCase = null;
        if (event.TestCase) {
            
            let windowArgs = { "SincroScreen": "SincroSendDeclaration"};
            
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "תרחשי הצהרה";
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(res => {
                    if (!AppTool.IsNullOrEmpty(res) && res=="Ok") {
                         this._SendDeclarationService._TestCase = new TestCase();
                        this._SendDeclarationService._TestCase.Code = comp._ScenarioCode;
                        this._SendDeclarationService._TestCase.Param1 = comp.Param1;
                        this._SendDeclarationService._TestCase.Param2 = comp.Param2;
                        this._SendDeclarationService.OnCustomSendOptionsButtonClick(event)
                    }
                });
            });

            logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');
            ///this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();

            return;
        }

        if (this._WorkWithService) {
            this._SendDeclarationService.OnCustomSendOptionsButtonClick(event);
            return;
        }


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
            this._SendDeclarationService.ngOnDestroy();
            return;
        }
    }
}

export class SendDeclarationService implements OnDestroy {

    //-----------------Properties----------------------------//
    EntityPM: DeclarationPM;
    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    presendValidationsTitle: string;
    DeclarationService: DeclarationWebService;
    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();
    supplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    _SupplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    //------------------------------------------------------//
    private CurrentSession = SessionLocator.SelectedSession;
    _TestCase: TestCase;
    constructor() {

    }

    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.CourierWorksheetmode = args.CourierWorksheetmode;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.DeclarationService = new DeclarationWebService();
        if (!this.EntityPM.IsCourierDeclaration) {
            this.ButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Send");
        }
        else {
            this.ButtonText = "שלח הצהרה"; // TextCodeTranslator.Translate("Customs.Declaration.O.SendDeclaration");
        }
 
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
    ButtonText: string;
    public CourierWorksheetmode: boolean = false;
    OnCustomSendOptionsButtonClick(event) {
        this.CurrentSession.StartBusyIndicator("");
        this.RequestVIA = event.RequestVIA;
        this.Option = event.Option;
        this.ForcePersonalSign = event.ForcePersonalSign;
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            if (this.CourierWorksheetmode) {
                this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                return;
            }
            // this.CurrentSession.CurrentEditComponent.SaveChanges("");

            //var firstInvoice: SupplierInvoicePM = this.EntityPM.SupplierInvoices.filter(d => d.SequenceNumeric == 1)[0];



            //if (firstInvoice && firstInvoice.InsruancePercentage && this.EntityPM.TaxationDateTime) {
            //    this.CalculateInsuranceAmount(firstInvoice);

            //}

            //else {
            {


                this.declarationPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    if (myResponse.HasError) {

                        this.ValidationErrors = myResponse.ErrorsArray;
                        this.FillValidationErrors(this.presendValidationsTitle);
                        this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    }

                    else {
                         this.EntityPM = myResponse.Result;
                        if (this.CurrentSession.CurrentEditComponent) {
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
                                                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                                this.ValidationErrors = response.Result.ErrorMessages;
                                                this.FillValidationErrors(TextCodeTranslator.Translate(response.Result.ErrorsType));
                                            }
                                        });
                                    }
                                }
                            });

                        }
                    }
                });




            }
        }
        else {
            this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
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
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors = response.Result.ErrorMessages;
                this.FillValidationErrors(TextCodeTranslator.Translate(response.Result.ErrorsType));
            }
        });

    }

    CheckRequiredFields() {
       
        this.DeclarationService.GetRequiredFieldsForDeclaration(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            //List < CustomsRequiredFieldsErrorItem > errorsList = requiredFieldsErrors.RequiredFields;
            var errorsList = response.Result.RequiredFields;
            if (errorsList.length == 0 || this.EntityPM.IsAmendment) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) || true) { //|| !ScriptableGatewayUtil.AmitalBrowserInUse) { i put true temporarly--MM
                    this.InstructionSendToMehes();//this.ConfirmB4TaxationDateTimeCheck();
                    return;
                }
            }
            else {
                this.ValidationErrors = this.GetRequiredErrorsList(errorsList);
                this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
            }
        });
    }

    private InstructionSendToMehes() {
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {//if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");

            var myUnifreightPrintStimulController = new UnifreightController(this.EntityPM,
                "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");
            myUnifreightPrintStimulController.SendRequestInstructionToUnifreightAsync("SENDTOMEHES");
            myUnifreightPrintStimulController.GetPromise().
                then((e) => {
                    var UnifreightResponseStatus = e.UnifreightResponseStatus;
                    var UnifreightMessage = e.UnifreightMessage;
                    if (UnifreightResponseStatus) {
                        //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = true, Message = TextCodeTranslator.Translate("Customs.General.O.Sending") });
                        //var IFritz_feature = FeatureLocator.Features.filter(d => d.Code == "IFRITZ")[0];
                        if (FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) {//    o        לאחר שמירה ובדיקת שדות לשליחה, יש לבדוק Feature כפי שבודקים במסך חשבון ספק
                            console.log("FritzFeatureIsON .. ");
                            this.UnifreightRequestExpenseFreight();



                        } else {

                            this.ConfirmB4TaxationDateTimeCheck();
                        }

                    }
                    else {
                        this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    }
                });
        } else {
            this.ConfirmB4TaxationDateTimeCheck();
        }
    }
    UnifreightRequestExpenseFreight() {
        console.log("UnifreightRequestExpenseFreight .. ");
        this.CurrentSession.StartBusyIndicator("Check Insurance ...");
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
            (unifreightMessageM: UnifreightMessageM) => {
                var IsMatchUnifreightCallbackCommand = (
                    unifreightMessageM.UnifreightEntityNumber == this.EntityPM.CustomFileNo &&
                    unifreightMessageM.LogitudeViewModel == "SendDeclarationService");
                if (IsMatchUnifreightCallbackCommand) {
                    sub.unsubscribe();
                    console.log("UnifreightRequestExpenseFreight .. UnifaceRequestArrived ");
                    let supplierInvoice: SupplierInvoicePM = null;
                    supplierInvoice = this.EntityPM.SupplierInvoices[0];

                    this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
                    this._SupplierInvoiceExtendedPMService
                        .GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, supplierInvoice.InvoiceCounterKey, 0, 0, "").subscribe((response:any) => {
                            supplierInvoice = response.Result;

                            var service: AnalyzeUnifreightInsuranceService = new AnalyzeUnifreightInsuranceService();
                            service.Open(unifreightMessageM, this.EntityPM, supplierInvoice);
                            if (this.EntityPM.IsChanged) {
                                this.UpdateReloadAndConfirmB4TaxationDateTimeCheck(supplierInvoice);
                            } else {
                                console.log("UnifaceRequestArrived But noting change ");
                                this.ConfirmB4TaxationDateTimeCheck();
                            }

                        });
                }
            });

        AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCheckInsuranseReturnIsNeededAmount(
            this.EntityPM.CustomFileNo, this.EntityPM.Id
            , "SendDeclarationService", "OPEN"
        );
    }
    public ObjectTableName: string = "Customs.Declaration";
    UpdateReloadAndConfirmB4TaxationDateTimeCheck(supplierInvoice: SupplierInvoicePM) {
        console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 1.update");
        //o	יש לבצע שמירה מחדש של ההצהרה (וחשבון ספק)
        this.supplierInvoicePMService.update(supplierInvoice)
            //this.declarationPMService.update(this.EntityPM)
            .subscribe((myResponse: ServiceResponse) => {

                if (myResponse.HasError) {


                    console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.1 update failed StopBusyIndicator");
                    this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }

                else {
                    //itzik:result is supplierInvoice that set in typeof(EntityPM)== declaration
                    //this.EntityPM = myResponse.Result;//reload fix this problem 
                    console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.1 update Success");
                    if (this.CurrentSession.CurrentEditComponent) {
                        //o	יש לבצע רענון לנתוני client
                        console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.2 Reload");
                        this.reloadEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                            this.reloadEvent.unsubscribe();
                            console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.3 Continue to this.ConfirmB4TaxationDateTimeCheck();");
                            //o	לאחר מכן להמשיך בתהליך השליחה למכס
                            this.ConfirmB4TaxationDateTimeCheck();
                        });
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                    } else {// courier worksheet  >>>send dec

                        let entityPMService: EntityPMService = new EntityPMService();
                        entityPMService.getSingle(this.ObjectTableName, this.EntityPM.Id).then((res: any) => {
                            res.subscribe((myResponse: ServiceResponse) => {
                                if (myResponse.HasError) {
                                }
                                else {
                                    this.EntityPM = myResponse.Result;
                                    console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.3 Continue to this.ConfirmB4TaxationDateTimeCheck();");
                                    //o	לאחר מכן להמשיך בתהליך השליחה למכס
                                    this.ConfirmB4TaxationDateTimeCheck();
                                }
                            });
                        });


                    }
                }
            });

    }

    ConfirmB4TaxationDateTimeCheck() {
        var declarationValidator = new DeclarationValidator();
        declarationValidator.SetEntityPM(this.EntityPM);
        var taxationDateTimeMessage = declarationValidator.TaxationDateTimeCheck();
        if (AppTool.IsNullOrEmpty(taxationDateTimeMessage)) {
            this.CheckCertificateStatus();

        }
        else {

            this.ValidationErrors.push(TextCodeTranslator.Translate(taxationDateTimeMessage));
            var windowArgs: any = {};
            windowArgs.Errors = this.ValidationErrors;
            windowArgs.NoButtonVisibility = true;
            windowArgs.CancelButtonVisibility = true;
            windowArgs.NoButtonText = "לא";
            windowArgs.SaveButtonText = "עדכן";
            windowArgs.CancelButtonText = "בטל";
            windowArgs.ComponentHeight = '328px';
            var windowTitle = TextCodeTranslator.Translate("Customs.General.O.TaxationDateTimeCheck");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.TaxationWindowClosed($event));

            logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
            this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            // this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.TaxationDateTimeCheck"));
        }


    }

    TaxationWindowClosed(event) {
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
                this.EntityPM.TaxationDateTime = DateTool.GetCurrentDateAsUtc();
                var declarationPMService: DeclarationPMService = new DeclarationPMService();
                declarationPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrors = myResponse.ErrorsArray;
                        this.FillValidationErrors(this.presendValidationsTitle);
                    }
                    else {
                        this.EntityPM = myResponse.Result;
                        this.CheckCertificateStatus();
                    }

                });
                break;
            }
            case "no": {
                this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
                this.CheckCertificateStatus();
                break;
            }
            case "cancel": {
                break;
            }
        }
    }

    CheckCertificateStatus() {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");// avoid resend
        this.DeclarationService.CheckCertificateStatus(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            //List < CustomsRequiredFieldsErrorItem > items = requiredFieldsErrors.RequiredFields;
            var items: any[] = myResponse.Result.RequiredFields;
            var warningsList: string[] = [];
            items.forEach((item) => {
                warningsList.push("חשבון ספק " + item.EntityReference + " - שורה  " + item.EntityReference2 + " מסך אישורים - לא הוזנו שדות החובה שנדרשים לאישור הנ”ל ");
            });

            if (warningsList.length > 0) {

                this.ValidationErrors = warningsList;
                var windowArgs: any = {};
                windowArgs.Errors = this.ValidationErrors;
                windowArgs.NoButtonVisibility = false;
                windowArgs.CancelButtonVisibility = true;

                windowArgs.SaveButtonText = "שלח נוכחי";
                windowArgs.CancelButtonText = "בטל שליחה";
                windowArgs.ComponentHeight = '328px';
                var windowTitle = TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");

                var logWindow = new LogitudeWindow();
                logWindow.Width = 600;
                logWindow.Height = 400;
                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => this.CheckCertificateStatusClosed($event));

                logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            }
            else {
                this.DeclarationSendChecks();
            }
        });
    }

    DeclarationSendChecks() {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var declarationValidator = new DeclarationValidator();
        declarationValidator.SetEntityPM(this.EntityPM);
        declarationValidator.PreDeclarationSendChecks();
        if (declarationValidator.ValidationErrorMessageCodes.length == 0) {
            this.DeclarationService.GetDeclarationDocumentList(this.EntityPM.Id, "Declaration").subscribe((myResponse: ServiceResponse) => {
                var myCustomsDocumentPMList: CustomsDocumentPM[] = myResponse.Result;
                this.DeclarationService.GetDeclarationDocumentWithConnectNotValid(this.EntityPM.Id, "Declaration").subscribe((myResponse2: ServiceResponse) => {
                    var myCustomsDocumentPMList2: CustomsDocumentPM[] = myResponse2.Result;
                    if (myCustomsDocumentPMList2 && myCustomsDocumentPMList2.length > 0) {

                        myCustomsDocumentPMList2.forEach(x =>  {

                            this.ValidationErrors.push("לא קושר חשבון/חשבון פרט מכס לצרופה : " + x.ExternalAttachmentId);

                        })
                    }
                    if (myCustomsDocumentPMList && myCustomsDocumentPMList.filter(x => x.DocumentStatusCode == '7').length > 0) {
                        this.ValidationErrors.push("קיימים מסמכים בתהליך שליחה.");
                 
                    }

                    if (this.ValidationErrors && this.ValidationErrors.length > 0) {
                        var windowArgs: any = {};
                        windowArgs.Errors = this.ValidationErrors;
                        windowArgs.ComponentHeight = '328px'; // بدك تقيم 72 
                        var windowTitle = "בדיקת מסמכים לפני שליחת הצהרת יבוא";

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 600;
                        logWindow.Height = 400;
                        logWindow.Title = windowTitle;
                        logWindow.ShowCloseButton = false;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));

                        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
                        this.StopMyBusyIndicator();

                        return;
                    }
                    if (!myCustomsDocumentPMList) {
                        this.CheckMandatoryTickets();
                    }
                    else if (myCustomsDocumentPMList.length == 0) {
                        this.CheckMandatoryTickets();
                    }
                    else {
                        var notSendList = myCustomsDocumentPMList.filter(r => AppTool.IsNullOrEmpty(r.CustomsDocId));
                        if (notSendList.length == 0) {
                            this.CheckMandatoryTickets();
                            return;
                        }

                        var errorMessage = "";
                        // this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                        myCustomsDocumentPMList.forEach((customsDocumentPM) => {
                            if (AppTool.IsNullOrEmpty(customsDocumentPM.CustomsDocId)) {
                                errorMessage = TextCodeTranslator.Translate("Customs.General.O.DocumetsUploaded");
                            }
                        });

                        if (AppTool.IsNullOrEmpty(errorMessage)) {
                            //this.SendDeclaration();
                            this.CheckMandatoryTickets();
                        }
                        else {

                            this.ValidationErrors.push(errorMessage);
                            var windowArgs: any = {};
                            windowArgs.Errors = this.ValidationErrors;
                            windowArgs.NoButtonVisibility = false;
                            windowArgs.CancelButtonVisibility = true;
                            windowArgs.ComponentHeight = '328px';
                            var windowTitle = TextCodeTranslator.Translate("Customs.General.O.DocumetsUploadedCheck");

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 600;
                            logWindow.Height = 400;
                            logWindow.Title = windowTitle;
                            logWindow.ShowCloseButton = false;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.WindowClosed.subscribe(($event: any) => this.DocumetsUploadedCheckClosed($event));

                            logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
                            this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                        }
                    }
                });
              
            });
        }
        else {
            this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            this.ValidationErrors = declarationValidator.ValidationErrorMessageCodes;
            this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.PreSendValidations"));
        }
    }

    CheckMandatoryTickets() {
        //this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend

        this.DeclarationService.GetDeclarationMandatoryTicketList(this.EntityPM.Id, "Declaration").subscribe((myResponse: ServiceResponse) => {
            var myCustomsDocumentPMList: any[] = myResponse.Result;
            if (!myCustomsDocumentPMList) {
                this.CheckFreightByIncoterm();
            }
            else if (myCustomsDocumentPMList.length == 0) {
                this.CheckFreightByIncoterm();
            }
            else {
                var errorMessage = TextCodeTranslator.Translate("Customs.General.O.DocumetsMandatoryTicket");
                if (AppTool.IsNullOrEmpty(errorMessage)) {
                    this.CheckFreightByIncoterm();
                }
                else {

                    this.ValidationErrors.push(errorMessage);
                    var windowArgs: any = {};
                    windowArgs.Errors = this.ValidationErrors;
                    windowArgs.NoButtonVisibility = false;
                    windowArgs.CancelButtonVisibility = true;
                    windowArgs.ComponentHeight = '328px';
                    var windowTitle = TextCodeTranslator.Translate("Customs.General.O.DocumetsMandatoryTicket");

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(($event: any) => this.DocumetsTicketUploadedCheckClosed($event));

                    logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
                    this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }
            }
        });
    }

    CheckFreightByIncoterm() {
        //this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend

        //this.DeclarationService.CheckFreightAmountsByIncoterm(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
        this.DeclarationService.CheckFreightAmountsByIncotermWithDefault(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            var isFreightAmount: boolean = myResponse.Result;
            if (!isFreightAmount) {
                this.SendDeclaration();
            }
            else {
                //var errorMessage = TextCodeTranslator.Translate("Customs.General.O.FreightIncotermMandatory");
                var errorMessage = "קיימים נתוני ערך הובלה אך תנאי המכר בתיק אינם דורשים זאת , להמשיך ? ";
                if (AppTool.IsNullOrEmpty(errorMessage)) {
                    this.SendDeclaration();
                }
                else {

                    this.ValidationErrors.push(errorMessage);
                    var windowArgs: any = {};
                    windowArgs.Errors = this.ValidationErrors;
                    windowArgs.NoButtonVisibility = false;
                    windowArgs.CancelButtonVisibility = true;
                    windowArgs.ComponentHeight = '328px';
                    var windowTitle = TextCodeTranslator.Translate("Customs.General.O.FreightIncotermMandatory");

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(($event: any) => this.FreightByIncotermUploadedCheckClosed($event));

                    logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
                    this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }
            }
        });
    }

    DocumetsUploadedCheckClosed(event) {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.CheckMandatoryTickets();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    }

    DocumetsTicketUploadedCheckClosed(event) {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.CheckFreightByIncoterm();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    }

    FreightByIncotermUploadedCheckClosed(event) {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.SendDeclaration();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    }

    CheckCertificateStatusClosed(event) {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.DeclarationSendChecks();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    }
    public OnSuccessSendMethod: (response: any) => void;

    SendAmendmentDeclaration() {
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.ObjectTable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Amendment Declaration Request";
        searchParams.ResponseName = "Amendment Declaration Response";
        searchParams.RequestVIA = this.RequestVIA;
        searchParams.ForcePersonalSign = this.ForcePersonalSign;
        searchParams.TestCase = this._TestCase;
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
                            this.OnSuccessSendMethod(this.ResponseData);
                        }
                    }
                };
        }
        CustomMessageProgressComponent
            .ShowProgressBar(searchParams.PBId,
                "שליחת תיקון הצהרת יבוא", false
                , myShowProgressBarParams)
            .then((res) => {
                this.ResponseData = res;
                if (this.CourierWorksheetmode) {

                } else {
                    if (this.ResponseData && this.ResponseData.ContinueProcessInBackground) {
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    else if (this.Option == 'WB' || this.Option == 'D') { // work around itzik shall fix the undefined problem.
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
                    myDeclarationEditComponentController.CustomsAnswersShowManifest = false;

                    this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DCCA";
                    this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
            ).catch((err) => {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        this.DeclarationService.PostSendDeclarationAmendment(searchParams).subscribe((response: ServiceResponse) => {
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });

    }
    SendDeclaration() {

        if (this.EntityPM.IsAmendment) {
            this.SendAmendmentDeclaration();
            return;
        }
        this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.ObjectTable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = this.RequestVIA;
        searchParams.ForcePersonalSign = this.ForcePersonalSign;
        searchParams.TestCase = this._TestCase;

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
                            this.OnSuccessSendMethod(this.ResponseData);
                        }
                    }
                };
        }
        CustomMessageProgressComponent
            .ShowProgressBar(searchParams.PBId,
            "שליחת הצהרת יבוא", false
            , myShowProgressBarParams)
            .then((res) => {
                this.ResponseData = res;
                if (this.CourierWorksheetmode) {

                } else {
                    if (this.ResponseData && this.ResponseData.ContinueProcessInBackground) {
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    else if (this.Option == 'WB' || this.Option == 'D') { // work around itzik shall fix the undefined problem.
                        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
                    myDeclarationEditComponentController.CustomsAnswersShowManifest = false;

                    this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DCCA";
                    this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
            ).catch((err) => {
                this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });
        if (this.EntityPM.Direction == "E") {
            this.DeclarationService.PostSendExportDeclaration(searchParams).subscribe((response: ServiceResponse) => {
                //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
        }
        else {
            this.DeclarationService.PostSendDeclaration(searchParams).subscribe((response: ServiceResponse) => {
                //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            });
        
}
      
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
    StopMyBusyIndicator() {
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StopBusyIndicator();
        } else {
            this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        }
    }
    StartMyBusyIndicator(mess) {
        ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StartBusyIndicator(mess);
        } else {
            this.CurrentSession.CurrentEditComponent.StartBusyIndicator(mess);
        }
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
