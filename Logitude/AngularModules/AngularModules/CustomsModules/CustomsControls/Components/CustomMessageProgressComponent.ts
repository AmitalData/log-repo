import { Component, EventEmitter, Output, Input, OnInit, OnDestroy, ViewChild, AfterViewInit, AfterContentInit } from '@angular/core';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ResponseDataBase, CustomsStepEnum } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { IIGGeneralMessagesService, ResultClientProgressBar } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SessionComponent } from '../../../Infrastructure/Components/Session/SessionComponent';

//////////////////
/// CustomMessageProgressComponent  is below 



@Component({
    selector: 'custom-message-progress',
    
    templateUrl: './CustomMessageProgressComponent.html',
})

export class CustomMessageProgressComponent {
    //private CurrentSession = SessionLocator.SelectedSession;
    //private static StaticCurrentSession = SessionLocator.SelectedSession;

    public _Message: string;
    public static CurrCustomMessageProgressHelper: CustomMessageProgressHelper = null;
    public static ShowProgressBar
        (currentSession: SessionComponent ,PBId: string, Title: string, OnSuccessCloseWin: boolean
        //, OnSuccessCloseWinMethod?: (response: any) => boolean
            , myShowProgressBarParams?: ShowProgressBarParams
            
        )
        : Promise<any> {

        let alreadyDone = false;

        return new Promise<any>((resolve, reject) => {
            currentSession
            //SessionLocator.SelectedSession
            /*this.StaticCurrentSession*/.StartBusyIndicator("");
            var currCustomMessageProgressHelper = new CustomMessageProgressHelper();
            CustomMessageProgressComponent.CurrCustomMessageProgressHelper = currCustomMessageProgressHelper;
            currCustomMessageProgressHelper.StartProgress(PBId, 3, OnSuccessCloseWin);
            currCustomMessageProgressHelper.OnMessageArrived.subscribe(
                () => {
                    //currCustomMessageProgressHelper.OnMessageArrived.unsubscribe();
                    if (alreadyDone) return;
                    alreadyDone = true;

                    try {
                        currentSession
                        //SessionLocator.SelectedSession
                        /*this.StaticCurrentSession*/.StopBusyIndicator();
                        var response = currCustomMessageProgressHelper.ResponseData;
                        resolve(response);

                        let success: boolean = false;
                        let continueProcessInBackground: boolean = false;
                        if (currCustomMessageProgressHelper.ResponseData) {
                            if (currCustomMessageProgressHelper.ResponseData.Succeeded && !currCustomMessageProgressHelper.ResponseData.HasException) {
                                ////xxxxxxxxxxxxx
                                success = true;
                            }
                            if (currCustomMessageProgressHelper.ResponseData.ContinueProcessInBackground) {
                                continueProcessInBackground = true;
                            }
                        }
                        if (OnSuccessCloseWin) {
                            if (!continueProcessInBackground && success) {
                                //this.CurrentSession.StopBusyIndicator();
                                //currCustomMessageProgressComponent.ngOnDestroy();
                                return;
                            }
                        }
                        currentSession
                        //SessionLocator.SelectedSession
                        /*this.StaticCurrentSession*/.StopBusyIndicator();

                        if (myShowProgressBarParams) {
                            if (myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod) {
                                let successAnalyze: boolean = myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod(currCustomMessageProgressHelper.ResponseData);
                                if (successAnalyze) {
                                    return;
                                }
                            }
                        }
                        let myOnCloseCustomMessageProgressComponentMethod: (response: any) => void;
                        if (myShowProgressBarParams) {
                            myOnCloseCustomMessageProgressComponentMethod = myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod;
                        }
                        //CustomMessageProgressComponent.ShowCustomMessageProgressComponent(Title, currCustomMessageProgressHelper._Message,
                        //    currCustomMessageProgressHelper.ResponseData, myOnCloseCustomMessageProgressComponentMethod
                        //);

                        CustomMessageProgressComponent.ShowCustomMessageProgressComponent(Title, currCustomMessageProgressHelper._Message,
                            () => {
                                if (myOnCloseCustomMessageProgressComponentMethod) {
                                    myOnCloseCustomMessageProgressComponentMethod(currCustomMessageProgressHelper.ResponseData);
                                }
                            }
                            //currCustomMessageProgressHelper.ResponseData, myOnCloseCustomMessageProgressComponentMethod
                        );


                    } finally {
                        currentSession
                        //SessionLocator.SelectedSession
                        /*this.StaticCurrentSession*/.StopBusyIndicator();

                        currCustomMessageProgressHelper.ngOnDestroy();

                    }

                }
            );

        });
    }
    public static ShowCustomMessageProgressComponent(title: string, mess: string,
        //    response: any, OnCloseCustomMessageProgressComponentMethod?: (response: any) => void
        OnCloseCustomMessageProgressComponentMethod: () => void
    ) {
        var customMassagingProgressWindow = new LogitudeWindow();
        customMassagingProgressWindow.Height = 400;
        customMassagingProgressWindow.Width = 600;
        customMassagingProgressWindow.ShowCloseButton = true; //.CloseButton.IsEnabled = false;
        customMassagingProgressWindow.Title = title;


        customMassagingProgressWindow.Show('./CustomsModules/CustomsControls/Components/CustomMessageProgressComponent');

        customMassagingProgressWindow.ComponentLoaded
            .subscribe((customMessageProgressComponent: CustomMessageProgressComponent) => {
                customMessageProgressComponent._Message = mess; //currCustomMessageProgressHelper._Message;

            });
        customMassagingProgressWindow.WindowClosed.subscribe((anyString) => {
            ///
            if (OnCloseCustomMessageProgressComponentMethod) {
                OnCloseCustomMessageProgressComponentMethod();
            }
        });
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession
        /*this.CurrentSession*/.CloseCurrentWindow();
    }
}



export class ShowProgressBarParams {
    public OnSuccessAnalyzeCloseWinMethod: (response: any) => boolean;

    public OnCloseCustomMessageProgressComponentMethod: (response: any) => void;
}


export class CustomMessageProgressHelper
    implements OnDestroy {
    @Output()
    public OnMessageArrived: EventEmitter<void> = new EventEmitter<void>();

    _DispatcherTimer: any; //DispatcherTimer
    private _PBId: string;


    private CurrentSession = SessionLocator.SelectedSession;

    _LastUpdateCurrentStageLine: Date = DateTool.GetCurrentDateTimeAsUtc();
    _CurrentStageLine: string;


    _Disposed: boolean = false;
    public _Message: string;
    private _MessageArrived: boolean;

    private _WebServiceClientHasError: boolean;
    private _WaitSignFinish: boolean;
    private _ServerCalc: boolean = true;
    private _ContinueInBackgroundMess: string = "המסר נבנה בהצלחה וישלח בתהליך רקע";

    private _DispatcherTimerIntervalStart: number = 99;// 300;
    private _DispatcherTimerInterval: number = 99;// 400;



    private _TimeOutInMinutes: number;
    private _OnSuccessCloseWin: boolean;

    public StartProgress(PBId: string, timeOutInMinutes: number, OnSuccessCloseWin: boolean) {

        this._OnSuccessCloseWin = OnSuccessCloseWin;
        this._PBId = PBId;
        this._TimeOutInMinutes = 3;
        this._TimeOutInMinutes = timeOutInMinutes;
        this.DoWork();
    }



    public StopTimer() {
        if (this._DispatcherTimer) {
            clearTimeout(this._DispatcherTimer);
        }
    }

    GetformatedLine(val: string): string {
        if (!AppTool.IsNullOrEmpty(val) && val.startsWith("<?xml")) {
            let toStop = false;;
            var res = this.AnalyzeResponseMessage(val);
            if (res.toStop) {
                return null;
            }
            return res.messageFromServer;
        }
        else {
            return val;
        }
    }

    private AnalyzeResponseMessage(messageFromServer: string) {
        let toStop = true;
        try {
            this.ResponseData = JSON.parse(messageFromServer);
        } catch (err) {
            ///Maybe Onlt mesaage like : No Sign For Id ...
        }
        if (this.ResponseData != null) {

            if (this.ResponseData.HasException) {

                messageFromServer = this.ResponseData.UserMessage;

            }

            else if (this.ResponseData.ContinueProcessInBackground) {

                messageFromServer = this._ContinueInBackgroundMess;// ("המסר נבנה בהצלחה וישלח בתהליך רקע");

            }
            else if (this.ResponseData.Succeeded) {
                messageFromServer = "בוצע בהצלחה";
                if (!AppTool.IsNullOrEmpty(this.ResponseData.UserMessage)) {
                    messageFromServer = this.ResponseData.UserMessage;
                }
            }

        }
        toStop = true;
        this.StopAndShowMessage(messageFromServer);


        return { 'messageFromServer': messageFromServer, 'toStop': toStop };

    }

    DoWork() {
        clearTimeout(this._DispatcherTimer);
        //if (DateTime.Now.Subtract(_LastUpdateCurrentStageLine) > TimeSpan.FromMinutes(TimeOutInMinutes))
        if (DateTool.AddMinute(this._LastUpdateCurrentStageLine, this._TimeOutInMinutes) < DateTool.GetCurrentDateTimeAsUtc()) {

            this.CurrentSession.StopBusyIndicator();

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("הבקשה נתקלה בחוסר מענה , האם להמשיך להמתין לתשובה ?");
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) {
                    this._LastUpdateCurrentStageLine = DateTool.GetCurrentDateTimeAsUtc();
                    this._TimeOutInMinutes = 1;
                    this.DoWork();

                }
                else {
                    this.StopAndShowMessage("הבקשה נתקלה בחוסר מענה");
                }
            });
            return;
        }

        this.GetClientProgressBarIndicatorCurrentStage();


    }
    BasicResponse: boolean = false;
    GetClientProgressBarIndicatorCurrentStage() {
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
        var sub = myIIGGeneralMessagesService
            .GetClientProgressBarIndicatorCurrentStage(
            SessionLocator.Tenant, this._PBId, this.BasicResponse
            ).subscribe((serviceResponse: ServiceResponse) => {
                sub.unsubscribe();
                this.ClientProgressBarIndicatorCurrentStageCompleted(serviceResponse);


            });
    }


    private ClientProgressBarIndicatorCurrentStageCompleted(serviceResponse: ServiceResponse) {


        if (this.MessageArrived) return;


        try {

            this._WebServiceClientHasError = (serviceResponse.HasError);
            var e: ResultClientProgressBar = serviceResponse.Result;

            var continueInBackground = e.continueInBackground;
            var Error1 = serviceResponse.ErrorsArray.map(err => err).join(', ');;
            //e.stopMeNow
            var responseDataXml = e.responseDataXml;
            if (this.BasicResponse) {
                this.CurrentStageLine = responseDataXml;
                return;
            }
            var customsRequestStep: CustomsStepEnum = e.ProgressStage;

            var mess = "";

            mess = ResponseDataBase.GetCustomsRequestStepText(customsRequestStep);
            if (continueInBackground) {
                console.log("ContinueInBackground !!")
                this.CurrentStageLine = this._ContinueInBackgroundMess;// "המסר נבנה בהצלחה וישלח בתהליך רקע" + mess;
                if (!AppTool.IsNullOrEmpty(responseDataXml)) {
                    this.CurrentStageLine = responseDataXml;
                }
                this.ResponseData = {
                    //ProgressStage: "????",
                    ///ContinueProcessInBackgroundMessage: "Dummy From CustomMessageProgressComponent",
                    HasException: false,
                    UserMessage: "",
                    ContinueProcessInBackground: true,
                    Succeeded: true,
                    CustomsRequestsSheetId: "ContinueProcessInBackground DummyResponsdata!! - From CustomMessageProgressComponent",
                    CorrelationId: "",

                };
                this.StopAndShowMessage(this.CurrentStageLine);
                //var res = this.AnalyzeResponseMessage(responseDataXml);//
                
                //var res = this.AnalyzeResponseMessage(responseDataXml);
                this.MessageArrived = true;
                return;
            }
            if (e.stopMeNow) {

                if (AppTool.IsNullOrEmpty(responseDataXml)) {
                    this.CurrentStageLine = this._ContinueInBackgroundMess;//"המסר נבנה בהצלחה וישלח בתהליך רקע" + mess;
                    this.StopAndShowMessage(this.CurrentStageLine);
                }
                else {
                    var o = false;
                    var res = this.AnalyzeResponseMessage(responseDataXml);

                }
                this.MessageArrived = true;
                return;
            }
            this.CurrentStageLine = mess;



        }
        catch (err) {


        }
        finally {
            if (!this.MessageArrived) {
                if (this._DispatcherTimerInterval < 2000) {
                    this._DispatcherTimerInterval = this._DispatcherTimerInterval + 50;
                }
                this._DispatcherTimerIntervalStart
                this._DispatcherTimer = setTimeout(() => this.DoWork(), this._DispatcherTimerInterval);

            }
        }
    }

    StopAndShowMessage(mess: string) {

        this.CurrentSession.StopBusyIndicator();
        this.StopTimer();// _DispatcherTimer.Tick -= _DispatcherTimer_Tick;
        this._Message = mess;
        this.CurrentStageLine = null;
        this.MessageArrived = true;
    }

    public ResponseData: ResponseDataBase;
    public get CurrentStageLine() { return this._CurrentStageLine; }
    public set CurrentStageLine(value: string) {
        if (this._CurrentStageLine == value) return;

        this._CurrentStageLine = value;
        this.CurrentSession.StartBusyIndicator(this._CurrentStageLine);

        this._LastUpdateCurrentStageLine = DateTool.GetCurrentDateTimeAsUtc();
        
        this._DispatcherTimerInterval = this._DispatcherTimerIntervalStart;
    }
    public get MessageArrived() { return this._MessageArrived; }
    public set MessageArrived(value: boolean) {
        if (value) {
            this.OnMessageArrived.emit();
        }
        if (this._MessageArrived == value) return;
        this._MessageArrived = value;

    }


    ngOnDestroy() {
        this.StopTimer();
    }



}



//////////////////




/////////////////////////////////////
