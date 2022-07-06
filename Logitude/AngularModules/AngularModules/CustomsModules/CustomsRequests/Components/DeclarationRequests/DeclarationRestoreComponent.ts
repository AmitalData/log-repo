import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { DeclarationRestoreRequestParams } from '../../../../Customs/DataContract/RequestParams/DeclarationRestoreRequestParams';
import { DeclarationRestoreResponseData } from '../../../../Customs/DataContract/ResponseData/DeclarationRestoreResponseData';
import { Validator } from                   '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from          '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from           '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, TestCase } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'DeclarationRestoreComponent',    
    
    templateUrl: './DeclarationRestoreComponent.html',
})


export class DeclarationRestoreComponent
    extends BaseRequestsSheetMassaging
    implements OnInit, AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: DeclarationRestoreComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    _MyResponseObjectToShow: any = null;
    _UserMessagehidden: boolean = true;

    _LastFetchDeclarationList: DeclarationList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
    }

    
    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }



    SetMenuArg(MenuArg) {
        this.RequestParams = MenuArg;
        this.OnMassageDisplayMethod();
        //this.CustomFileNo = MenuArg.CustomFileNo;
        //this.DeclarationNumber= MenuArg.DeclarationNumber;
    }
    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }

        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchDeclarationList = null;
        this.ValidationErrorsList = [];
    }
    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }


        if (this._LastFetchDeclarationList != null) {
            if (this.CustomFileNo == this._LastFetchDeclarationList.CustomFileNo) {
                return;
            }
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);

            });

    }



    DeclarationNumberTextChanged(DeclarationNumberText: string): void {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }

        if (this._LastFetchDeclarationList != null) {
            if (this.DeclarationNumber == this._LastFetchDeclarationList.DeclarationNumber) {
                return;
            }
        }
        this.DueChangeClearChildField(false);

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, false);
            });
    }


    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchDeclarationList = myResponse.Result
        if (this._LastFetchDeclarationList != null) {
            this.DeclarationId = this._LastFetchDeclarationList.Id;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        } else {

            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
            //else {
            //    this.SetValidityDeclarationNumber();
            //}
        }
    }

    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }

    

    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomsFile : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomsFile != value) {
            this.RequestParams.CustomsFile = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
            if (value) {
                this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            }
        } else {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
        }
    }
    get DeclarationId() { return this.RequestParams ? this.RequestParams.DeclarationId : null; }
    set DeclarationId(value: string) {
        if (this.RequestParams.DeclarationId != value) {
            this.RequestParams.DeclarationId = value;
        }
    }


    RefreshScreen() {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;


        if (!AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
    }

    OnMassageDisplayMethod() {
        
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationRestoreRequestParams();
        }

        this.RefreshScreen();
    }


    CheckDeclarationPayment() {

        if (this._LastFetchDeclarationList.PaymentDate && AppTool.IsNullOrEmpty(this._LastFetchDeclarationList.CorrectionsXml)) { // if declaration was already paid
            var confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show("הצהרה זו כבר שולמה, האם למחוק נתוני הגשה ולשחזר אותם מחדש");
            confirm.WindowClosed.subscribe((event: any) => {
                confirm.Close();
                if (confirm.Yes) {
                    //this.DeleteDeclarationPayment();
                }
            });
        }
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        if (customSendOptionsArgs.TestCase) {

            let windowArgs = { "SincroScreen": "SincroSendRetrieveDeclaration" };

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "תרחשי הצהרה";
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(res => {
                    if (!AppTool.IsNullOrEmpty(res) && res == "Ok") {
                        this.RequestParams.TestCase = new TestCase();
                        this.RequestParams.TestCase.Code = comp._ScenarioCode;
                        this.RequestParams.TestCase.Param1 = comp.Param1;
                        this.RequestParams.TestCase.Param2 = comp.Param2;
                        this.SendDeclarationRestoreRequest(customSendOptionsArgs);
                    }
                });
            });

            logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');
            ///this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();

            return;
        }

        if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
            declarationDisplayOnlyChecks.CheckIfRequestInProgress("2715", this.CustomFileNo, SessionLocator.Tenant)
                .subscribe((response: ServiceResponse) => {
                    if (!response.HasError) {
                        var requestSheets = response.Result;
                        var haveRS2715: boolean = false;
                        if ((requestSheets == null || requestSheets.length == 0)
                            || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                            haveRS2715 = false;
                        } else {
                            haveRS2715 = true;
                        }
                        if (haveRS2715) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Width = 400;
                            messageWindow.Height = 150;
                            messageWindow.Title = "שיחזור מספר הצהרה";
                            messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                            return;
                        }

                        declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", this.CustomFileNo, SessionLocator.Tenant)
                            .subscribe((response: ServiceResponse) => {
                                if (!response.HasError) {
                                    var requestSheets = response.Result;
                                    var haveRS2755: boolean = false;
                                    if ((requestSheets == null || requestSheets.length == 0)
                                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                                        haveRS2755 = false;
                                    } else {
                                        haveRS2755 = true;
                                    }
                                    if (haveRS2755) {
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Width = 400;
                                        messageWindow.Height = 150;
                                        messageWindow.Title = "שיחזור מספר הצהרה";
                                        messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                                        return;
                                    }
                                    this.SendDeclarationRestoreRequest(customSendOptionsArgs);
                                }
                            });
                    }
                });

        }
        else {
            this.SendDeclarationRestoreRequest(customSendOptionsArgs);
        }

    }


    SendDeclarationRestoreRequest(customSendOptionsArgs: CustomSendOptionsArgs) {
        var currRequestParams = new DeclarationRestoreRequestParams();///Force new GUID On Each Send !!

         currRequestParams.TestCase = this.RequestParams.TestCase;
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.LoggingEntityReference = this.RequestParams.LoggingEntityReference || this._LastFetchDeclarationList?.Direction;
        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;


        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לשיחזור נתוני הצהרה", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._DeclarationMessagesService.PostDeclarationRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }
    OnEscHotKeyPressed() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
}
