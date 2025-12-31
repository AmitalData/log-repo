import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { CustomSendOptionsArgs, HsmStationContext, RequestParamsBase, SendRequestVIA, TestCase } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { GenericRequestParams } from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { CustomsCollateralPM } from '../../../../../Customs/EntityPMs/CustomsCollateralPM';

declare var window: any;
 
@Component({
    selector: 'DeclarationCancellationComponent',

    templateUrl: './DeclarationCancellationComponent.html',
    providers: [DeclarationPMService, DeclarationWebService]
})

export class DeclarationCancellationComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: DeclarationCancellationComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ForcePersonalSign: boolean;
    RequestVIA: SendRequestVIA;

    ValidationErrorsList: string[];
    readonly: boolean = false;
    get CancelRequestNumber() { return this.EntityPM.CancelRequestNumber; }
    set CancelRequestNumber(value: number) {
        if (this.EntityPM.CancelRequestNumber != value) {
            this.EntityPM.CancelRequestNumber = value;
        }
    }

    get IsClaimable() { return this.EntityPM.IsClaimable; }
    set IsClaimable(value: boolean) {
        if (this.EntityPM.IsClaimable != value) {
            this.EntityPM.IsClaimable = value;
        }
    }


    get CancelRequestReasonCode() { return this.EntityPM.CancelRequestReasonCode; }
    set CancelRequestReasonCode(value: string) {
        if (this.EntityPM.CancelRequestReasonCode != value) {
            this.EntityPM.CancelRequestReasonCode = value;
        }

        if (value) {
            this.UIProperties.SetRequired("CancelRequestReasonCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CancelRequestReasonCode", this.ObjectTableName, true);
        }
    }

    get CancelRequestReasonExplanation() { return this.EntityPM.CancelRequestReasonExplanation; }
    set CancelRequestReasonExplanation(value: string) {
        if (this.EntityPM.CancelRequestReasonExplanation != value) {
            this.EntityPM.CancelRequestReasonExplanation = value;
        }
    }


    get CancelRequestStatusName() { return this.EntityPM.CancelRequestStatusName; }
    set CancelRequestStatusName(value: string) {
        if (this.EntityPM.CancelRequestStatusName != value) {
            this.EntityPM.CancelRequestStatusName = value;
        }
    }


    get CancelRequestStatusCode() { return this.EntityPM.CancelRequestStatusCode; }
    set CancelRequestStatusCode(value: string) {
        if (this.EntityPM.CancelRequestStatusCode != value) {
            this.EntityPM.CancelRequestStatusCode = value;
        }
    }

    get CustomCancelRequestRemarks() { return this.EntityPM.CustomCancelRequestRemarks; }
    set CustomCancelRequestRemarks(value: string) {
        if (this.EntityPM.CustomCancelRequestRemarks != value) {
            this.EntityPM.CustomCancelRequestRemarks = value;
        }
    }

    get CancelRequestApproveDate() {
        if (this.EntityPM != null) {
            if (this.EntityPM.CancelRequestApproveDate != null) {
                var myFormats = DateTool.GetDateFormats(this.EntityPM.CancelRequestApproveDate);
                return myFormats.DateString + " " + myFormats.ShortTimeString;
            }
        }
        return null;
    }
    set CancelRequestApproveDate(value: string) {
        if (this.EntityPM.CancelRequestApproveDate != value) {
            this.EntityPM.CancelRequestApproveDate = value;
        }
    }




    get CancelRequestRejectionReason() { return this.EntityPM.CancelRequestRejectionReason; }
    set CancelRequestRejectionReason(value: string) {
        if (this.EntityPM.CancelRequestRejectionReason != value) {
            this.EntityPM.CancelRequestRejectionReason = value;
        }
    }

    get CancelRejectionReasonName() { return this.EntityPM.CancelRejectionReasonName; }
    set CancelRejectionReasonName(value: string) {
        if (this.EntityPM.CancelRejectionReasonName != value) {
            this.EntityPM.CancelRejectionReasonName = value;
        }
    }
    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService, private _DeclarationWebService: DeclarationWebService) {
        super();


    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {



        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        this.RequestVIA = customSendOptionsArgs.RequestVIA;


        this.ValidationErrorsList = null;
        SessionLocator.SelectedSession.StartBusyIndicator("");

        var currRequestParams: GenericRequestParams = new GenericRequestParams();
        currRequestParams.InterfaceTypeCode = "5002";
        currRequestParams.ForcePersonalSign = false;
        currRequestParams.IsAngularClient = true;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.LoggingEntityId = this.EntityPM.Id;
        currRequestParams.AppicationId = this.EntityPM.Id;
        currRequestParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0].Id;
        currRequestParams.LoggingEnabled = true;
        currRequestParams.RequestName = "Declaration Cancellation Request";
        currRequestParams.ResponseName = "Declaration Cancellation Response";
        currRequestParams.RequestVIA = this.RequestVIA;
        
        if (this.EntityPM?.IsCourierDeclaration === true ) {
            currRequestParams.HsmStationContext = HsmStationContext.Courier;
        }
        else if (this.EntityPM?.Direction === "E") {
            currRequestParams.HsmStationContext = HsmStationContext.Export;
        }
        else {
            currRequestParams.HsmStationContext = HsmStationContext.Import;
        }


        this._DeclarationWebService.GetIsDeclarationCancellationAttachmentNumberIsMoreThenAllow(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            this.ValidationErrorsList = [];

            if (!AppTool.IsNullOrEmpty(response.Result) && response.Result == false) {
                //  SessionLocator.SelectedSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrorsList.push("חובה לצרף מכתב בקשה לביטול הצהרה. ");
            }
            this.FillErrors();
            if (this.ValidationErrorsList.length > 0) {
                SessionLocator.SelectedSession.StopBusyIndicator();

                return;
            }
            this._declarationPMService.update(this.EntityPM).subscribe(x => {


                if (customSendOptionsArgs.TestCase) {

                    let windowArgs = { "SincroScreen": "SincroSendDeclarationCancellation" };

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = "תרחשי הצהרה";
                    logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;

                    logWindow.ComponentLoaded.subscribe(comp => {
                        logWindow.WindowClosed.subscribe(res => {
                            if (!AppTool.IsNullOrEmpty(res) && res == "Ok") {
                                currRequestParams.TestCase = new TestCase();
                                currRequestParams.TestCase.Code = comp._ScenarioCode;
                                currRequestParams.TestCase.Param1 = comp.Param1;
                                currRequestParams.TestCase.Param2 = comp.Param2;

                                CustomMessageProgressComponent
                                    .ShowProgressBar(this.CurrentSession, currRequestParams.PBId, "שליחת מסר ביטול הצהרה", true)
                                    .then((res) => {
                                        //this.ResponseData = res;
                                        //this.IsResponseMessageVisibility = true;
                                        //this.OnMassageDisplayMethod();
                                        //this.InitScreen(this.CargoSealIdentifierId);

                                    }
                                    ).catch((err) => {
                                        //this.IsResponseMessageVisibility = true;
                                        //this.ResponseMessage = err;
                                        this.ValidationErrorsList.push(err);
                                    });
                                this._DeclarationWebService.PostSendDeclarationCancellation(currRequestParams)
                                    .subscribe((myServiceResponse: ServiceResponse) => {
                                        this.InitScreen();

                                    });

                            }
                        });
                    });

                    logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');
                    ///this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    SessionLocator.SelectedSession.StopBusyIndicator();

                    return;
                }
                else {



                    CustomMessageProgressComponent
                        .ShowProgressBar(this.CurrentSession, currRequestParams.PBId, "שליחת מסר ביטול הצהרה", true)
                        .then((res) => {
                            //this.ResponseData = res;
                            //this.IsResponseMessageVisibility = true;
                            //this.OnMassageDisplayMethod();
                            //this.InitScreen(this.CargoSealIdentifierId);

                        }
                        ).catch((err) => {
                            //this.IsResponseMessageVisibility = true;
                            //this.ResponseMessage = err;
                            this.ValidationErrorsList.push(err);
                        });
                    this._DeclarationWebService.PostSendDeclarationCancellation(currRequestParams)
                        .subscribe((myServiceResponse: ServiceResponse) => {
                            if (!myServiceResponse.HasError && myServiceResponse.Result != null && myServiceResponse.Result.HasException != true) {
                                var messageWindow = new MessageWindow();
                                messageWindow.Width = 400;
                                messageWindow.Height = 200;
                                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                messageWindow.Show("בקשת ביטול נשלחה בהצלחה.");

                            }
                            SessionLocator.SelectedSession.CloseCurrentWindow();


                        });
                }

                SessionLocator.SelectedSession.StopBusyIndicator();
            });


        });


    }

    FillErrors() {
        var errors: string[] = [];
        // this.ValidationErrorsList = errors;



        if (AppTool.IsNullOrEmpty(this.CancelRequestReasonCode)) {
            var msg = "קוד סיבת ביטול שדה חובה.";//TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypeCode");
            this.ValidationErrorsList.push(msg);
        }
        if (this.EntityPM.Direction == "E") {
            if (!this.EntityPM.isSubmitDeclaration) {
                var msg = "לא ניתן לבטל ביטול הצהרה להצהרה שלא נמצאת בסטטוס הגשה.";//TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypeCode");
                this.ValidationErrorsList.push(msg);
            }
        } else {
            if (AppTool.IsNullOrEmpty(this.EntityPM.PaymentDate)) {
                var msg = "לא ניתן לבטל ביטול הצהרה להצהרה שלא נמצאת בסטטוס הגשה.";//TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypeCode");
                this.ValidationErrorsList.push(msg);
            }
        }


    }
    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.EntityPM = args.Declaration as DeclarationPM;

            this.InitScreen();

        });

    }

    InitScreen() {
        this.UIProperties.SetEnabled("CancelRequestStatusName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomCancelRequestRemarks", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CancelRequestApproveDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CancelRequestRejectionReason", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CancelRejectionReasonName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CancelRequestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IsClaimable", this.ObjectTableName, false);

        if (this.CancelRequestStatusCode == "5" || this.CancelRequestStatusCode == "2") {
            this.UIProperties.SetEnabled("CancelRequestReasonExplanation", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CancelRequestReasonCode", this.ObjectTableName, false);
            this.readonly = true;
        }
    }
    SkipCtor: boolean = false;
    ViewDocumentsComponent() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        //windowArgs.ObjectTableName = "Customs.DeclarationCancellation";
        windowArgs.ObjectTableName = "Customs.Declaration";// this.ObjectTableName;
        windowArgs.EntityParentPM = "DeclarationCancellation";
        //    windowArgs.SkipCtor = this.SkipCtor;
        windowArgs.IsFromStandAloneScreen = true;
        var windowTitle = "Customs.Declaration.TH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SkipCtor = true);
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //ViewDocumentsComponent() {


    //     var windowArgs: any = {};
    //    windowArgs.EntityPM = this.EntityPM;
    //    windowArgs.ObjectTableName = "Customs.DeclarationCancellation";// this.ObjectTableName;
    //    windowArgs.EntityParentPM = "DeclarationCancellation";
    //    var windowTitle = "Customs.Declaration.TH.Documents";
    ////    windowArgs.ParentEntityCode = "DeclarationCancellation";
    //    var logWindow = new LogitudeWindow();
    //    logWindow.IsHideHeader = true;
    //    logWindow.Width = 1000;
    //    logWindow.Height = 700;
    //    logWindow.Title = windowTitle;
    //    logWindow.ShowCloseButton = false;
    //    logWindow.WindowArgs = windowArgs;
    //   // logWindow.WindowClosed.subscribe(($event: any) => this.OnDocumentsWindowClosed($event));
    //    //this.entityArgs.SkipCtor = true;
    //    logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    //}

    ngOnInit() {

    }


}
