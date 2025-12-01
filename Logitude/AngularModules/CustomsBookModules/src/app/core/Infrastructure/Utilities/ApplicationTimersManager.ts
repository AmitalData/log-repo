declare var System: any;
declare var window: any;
import {Injectable, Injector, Output, EventEmitter} from '@angular/core';
import {ServiceArgs} from '../DataContracts/ServiceArgs';
import {ErrorLogPM} from '../../Infrastructure/EntityPMs/ErrorLogPM';
import {ErrorsLogPMService} from '../../Infrastructure/Services/ExtendedPMs/ErrorsLogPMService';
import {LogitudeApplicationService} from '../../Infrastructure/Services/WebServices/LogitudeApplicationService';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {SessionLocator} from '../Utilities/SessionLocator';
import {CachedDataManager} from '../Utilities/CachedDataManager';
import {SessionInfo} from '../Utilities/SessionInfo';
import {AppTool} from '../Tools';
import {UserLastLoginPMService}  from '../../Common/Services/StandardPMs/UserLastLoginPMService';
import {UserLastLoginPM}  from '../../Common/EntityPMs/UserLastLoginPM';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
import { PerformanceLogService } from '../../Infrastructure/Services/ExtendedPMs/PerformanceLogService';
import { PerformanceLog } from '../Others/PerformanceLog';
import { LogitudeHubChannelEvent } from '../Services/SignalRServices/SignalRChannelService';
import { SignalRChannelService } from '../Services/SignalRServices/SignalRChannelService';
import {ObjectsLocator} from '../Locators/ObjectsLocator';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';

@Injectable()

export class ApplicationTimersManager {

     
    logService: ErrorsLogPMService;
    performanceLogService: PerformanceLogService;

    logitudeApplicationService: LogitudeApplicationService;
    userLastLoginPMService: UserLastLoginPMService;
    //signalRGeneralService: SignalRGeneralService;
    signalRChannelService: SignalRChannelService;

    @Output() SignoutCompleted = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


        this.logService = new ErrorsLogPMService();
        this.logitudeApplicationService = new LogitudeApplicationService();
        this.userLastLoginPMService = new UserLastLoginPMService();
        this.performanceLogService = new PerformanceLogService();
        //this.signalRGeneralService = new SignalRGeneralService();
       
    }

    public StartApplicationTimers() {

        //this.signalRChannelService = new SignalRChannelService();
        //SessionLocator.SignalRChannelService = this.signalRChannelService;
        SessionLocator.TimersSubscribtions.push(
            this.getTimer(30000).subscribe((res:any) => {
                this.AddErrorLogs();
                //console.log('The response is received.');
            })
        );
        if (ObjectsLocator != null && ObjectsLocator.GlobalSetting != null && ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
            console.log("WorkEnvironment is customs! Suppress this.AddPeformanceLogs();");
        } else {
            SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe((res:any) => {
                this.AddPeformanceLogs();
            }));
        }


        SessionLocator.TimersSubscribtions.push(this.getTimer(60000).subscribe((res:any) => {
            this.CheckIsupgradingSystem();
        }));

        SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe((res:any) => {
            this.CheckApplicationLocalStorage();
        }));

        SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe((res:any) => {
            this.CheckUserLastLogin();
        }));


        SessionLocator.TimersSubscribtions.push(this.getTimer(120000).subscribe((res:any) => {
            this.CheckUserValidity();
        }));

        SessionLocator.TimersSubscribtions.push(this.getTimer(60000).subscribe((res:any) => {
            CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(reponse => {
                console.log("------------- SystemMetadataLastUpdate has been checked by timer! ---------------");
            });

        }));

        if (SessionLocator.UseCachedData) {
            SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe((res:any) => {
                CachedDataManager.CheckCachedTableLastUpdateDate().subscribe(reponse => {

                    console.log("cached tables checked by timer!");
                });

            }));





            //Observable.Interval(TimeSpan.FromSeconds(1.0));
            //var timerId = setTimeout(this.AddErrorLogs, 2000)  
            // setInterval(SaveErrorLogs, delay);//60000

            //this.signalRGeneralService.messageReceived.subscribe((ms: ChannelEvent) => {
            //    


            //});
            //var channelName: string = "Tenant" + SessionInfo.LoggedUserTenant + "Channel";
            //this.signalRChannelService.subscribeChannel(channelName).subscribe(
            //    (ev: any) => {
            //        console.log('Application Manager :signalR event received:' + channelName);

            //        if (ev.EventName === "CachedTableUpdate") {
            //                CachedDataManager.RefreshTableData(ev.EventParameter);
            //        }
            //    },
            //    (error: any) => {
            //        console.warn("Attempt to join channel failed!", error);
            //    }

            //)
        }
    }

    getTimer(period?: number) {
        return interval(period).pipe(timeInterval());
    }

    



    private CheckApplicationLocalStorage() {
        try {
            const isCypress = window.sessionStorage.getItem('ControlledByCypress');
            var isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;//MAC//WIN32
            if (window.localStorage.length === 0 && !isMac && !isCypress) {
                var messageWindow: MessageWindow = new MessageWindow();

                messageWindow.Width = 450;
                messageWindow.Title = "Application Storage Deleted";
                messageWindow.Height = 190;
                messageWindow.Show("Your application storage has been deleted. Please login again.");
                messageWindow.WindowClosed.subscribe(($event: any) => {
                    this.OnSignoutClicked();
                });


            }


        }
        catch (e) { console.error(e); }
    }

    IsUserUnlock: boolean = false;

    private CheckUserLastLogin() {
        this.userLastLoginPMService.GetUserLastLogin(SessionInfo.LoggedUserPM.Id, SessionInfo.LoggedUserTenant).subscribe((response:any) => {
            if (!response.HasError && response.Result) {

                var lastloginPM: UserLastLoginPM = response.Result;

                var computerId: string = SessionLocator.GetComputerIdFromStorage();
                if (!AppTool.IsNullOrEmpty(computerId) && lastloginPM.ComputerId != computerId && !ObjectsLocator.GlobalSetting.SameUserLoginEnabled) {

                    if (!this.IsUserUnlock) {//
                        this.IsUserUnlock = true;

                        if (this.CurrentSession) {
                            this.CurrentSession.StopBusyIndicator();
                            if (this.CurrentSession.CurrentWindow) {
                                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            }
                        }
                        var args = "";

                        var logWindow = new LogitudeWindow();
                        logWindow.IsOverAll = true;
                        logWindow.Title = "";
                        logWindow.WindowArgs = args;
                        logWindow.Width = 1000;
                        logWindow.Height = 250;
                        logWindow.IsHideWindowMargin = true;
                        logWindow.IsHideHeader = true;
                        SessionLocator.HomeComponent.ShowLockIndicator = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/UserUnlockComponent/UserUnlockComponent');
                        logWindow.WindowClosed.subscribe(($event: any) => {
                            SessionLocator.HomeComponent.ShowLockIndicator = false;
                            this.IsUserUnlock = false;

                    });

                }


            }


        }

        });
    }


    IsUpgradingEnd: boolean = false;
    private CheckIsupgradingSystem() {
        try {

            if (!this.IsUpgradingEnd) {

                this.logitudeApplicationService.GetCheckIsupgradingSystem().subscribe((res: ServiceResponse) => {

                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            if (myResult == true && !this.IsUpgradingEnd) {
                                this.IsUpgradingEnd = true;
                                var messageWindow: MessageWindow = new MessageWindow();
                                messageWindow.IsOverAll = true;
                                messageWindow.Width = 450;
                                messageWindow.Title = "Logitude Message";
                                messageWindow.Height = 190;
                                messageWindow.Show("The site is upgrading right now and you will be logged out , sorry for disturbing you!");
                                messageWindow.WindowClosed.subscribe(($event: any) => {
                                    this.OnSignoutClicked();
                                });


                            }
                        }
                    }

                }, error=> {
                    console.error("Check Isupgrading System: ", error);
                });

            }
        }
        
        catch (e) { console.error(e); }
    }


    private CheckUserValidity() {
        try {


            this.logitudeApplicationService.GetCurrenctUserValidity().subscribe((res: ServiceResponse) => {

                var response: ServiceResponse = res;
                 

                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        if (myResult.IsValid == false) {
                            var isShowMessage: boolean = true;
                            if (myResult.ErrorCode == "AUTH") {
                                SessionLocator.HomeComponent.SignoutClicked();
                            }
                            else {

                                var errorMessage = "This user is not authenticated!";
                                if (myResult.ErrorCode == "SUSR") {
                                    errorMessage = "Another user has logged!";
                                }

                                if (myResult.ErrorCode == "SUPG") {
                                    errorMessage = "The site is upgrading right now and you will be logged out , sorry for disturbing you!"; 

                                    if (this.IsUpgradingEnd) {
                                        isShowMessage = false;
                                    } else {
                                        this.IsUpgradingEnd = true;
                                    }
                                }
                                
                                if (isShowMessage) {
                                    var messageWindow: MessageWindow = new MessageWindow();
                                    messageWindow.Width = 450;
                                    messageWindow.Title = "Logitude Message";
                                    messageWindow.Height = 190;
                                    messageWindow.Show(errorMessage);
                                    messageWindow.WindowClosed.subscribe(($event: any) => {
                                        this.OnSignoutClicked();
                                    });
                                }
                            }
                        }
                        SessionInfo.DocumentDownloadToken = myResult.DocumentDownloadToken;

                    }
                }

            }, error=> {
                console.error("Check User Validity: ", error);
            });

        }

        catch (e) { console.error(e); }
    }


    OnSignoutClicked() {
        SessionLocator.HomeComponent.SignoutClicked();
    }
    
    private AddErrorLogs() {
        try {
            for (var key in sessionStorage) {
                if (key.indexOf("ErrorLogs") != -1) {

                    var logJson = window.sessionStorage.getItem(key)
                    var errorLog: ErrorLogPM = JSON.parse(logJson);

                    this.logService.insert(errorLog).subscribe((response: ServiceResponse) => {
                      
                        window.sessionStorage.removeItem(["ErrorLogs", response.Result.Id]);

                    }, error=> {
                        console.error("Adding ErrorLog Timer: ", error);
                    });


                }
            }
        }
        catch (e) { console.error(e); }
    }

    private AddPeformanceLogs() {
        try {
            var AllLogsList: PerformanceLog[] = [];
            for (var key in sessionStorage) {
                if (key.indexOf("PerformanceLogs") != -1) {

                    var logJson = window.sessionStorage.getItem(key)
                    var performanceLog: PerformanceLog = JSON.parse(logJson);
                    performanceLog.LogDateTimeLocal = new Date();
                    AllLogsList.push(performanceLog);
                    //PerformanceLogs,5d816163-030d-4d76-a5ef-c19a43951e0b


                    //this.performanceLogService.insert(performanceLog).subscribe((response:any) => {

                    //    window.sessionStorage.removeItem(["PerformanceLogs", response.Result.Id]);

                    //}, error => {
                    //    console.error("Adding Performance Log Timer: ", error);
                    //});
                }

            }

            //if (logsList.length <= 20) {
            var tobeAddedLogsList: PerformanceLog[] = [];
            let count = 0;
            for (var lk in AllLogsList) {
                tobeAddedLogsList.push(AllLogsList[lk]);
                window.sessionStorage.removeItem(["PerformanceLogs", AllLogsList[lk].Id]);
                count++;
                if (count == 20)
                    break;
            }
            if (tobeAddedLogsList.length > 0) {
                this.performanceLogService.insertLogsList(tobeAddedLogsList).subscribe((response: ServiceResponse) => {
                }, error => {
                    console.error("Adding Performance Log Timer: ", error);
                });
            }
        }
        catch (e) { console.error(e); }
    }

    


//    private AtomicLong lastTick = new AtomicLong(0L);
//    private Subscription subscription;

//     resume() {
//    System.out.println("resumed");
//    subscription = Observable.interval(5, TimeUnit.SECONDS, Schedulers.io())
//        .map(tick -> lastTick.getAndIncrement())
//        .subscribe(tick -> System.out.println("tick = " + tick));
//}

// StopAllTimers() {
//     forEach(var timerObs in SessionLocator.TimersSubscribtions.values) {
//         timerObs.un
//     }

         
//    //if (subscription != null && !subscription.isUnsubscribed()) {
//    //    System.out.println("stopped");
//    //    subscription.unsubscribe();
//    //}
//}



}

