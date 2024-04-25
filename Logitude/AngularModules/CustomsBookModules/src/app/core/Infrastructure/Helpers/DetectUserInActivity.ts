declare var window: any;
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { SessionLocator } from '../Utilities/SessionLocator';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';

export class DetectUserInActivity {
    WarningTimeInMiliseconds: any;
    LifeTimeInMiliseconds: any;
    timeoutId: any;
    IsSignout: boolean = false;
    IsTokenExpiration: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(isTokenExpiration: boolean = false) {
        this.IsTokenExpiration = isTokenExpiration;
    }

    //timeUnit // H:Hour , M:Minutes 

    Start(lifeTime: any, warningTime: any = 1, timeUnit: string = "H") {
        if (lifeTime && lifeTime >0 ) {
            if (timeUnit && timeUnit.toUpperCase() == "H") {
                lifeTime = lifeTime * 60;
            }

            if (!this.IsTokenExpiration) {
                warningTime = lifeTime / 4;
                warningTime = Math.round(warningTime);
            }


            this.LifeTimeInMiliseconds = ((lifeTime - warningTime) * 60000);
            this.WarningTimeInMiliseconds = (warningTime * 60000);

            if (this.LifeTimeInMiliseconds > 2147483647) this.LifeTimeInMiliseconds = 2147483647; // 24.8 Days
            if (this.WarningTimeInMiliseconds > 2147483647) this.WarningTimeInMiliseconds = 2147483647;

                if (this.WarningTimeInMiliseconds == 0) {
                    this.ShowMessage(this);
                    this.IsSignout = true;
                }
                
                this.SetupTimers(this);
        }
    }


    
    SetupTimers(viewModeil: any) {

        document.addEventListener("click", function () { viewModeil.ResetTimer(viewModeil); }, false);
        document.addEventListener("mousedown", function () { viewModeil.ResetTimer(viewModeil); }, false);
        document.addEventListener("keypress", function () { viewModeil.ResetTimer(viewModeil); }, false);
        this.StartTimer(viewModeil);
    }

    StartTimer(viewModeil: any, lifeTimeInMiliseconds: any = null) {

        if (!lifeTimeInMiliseconds) lifeTimeInMiliseconds = viewModeil.LifeTimeInMiliseconds;
        viewModeil.timeoutId = window.setTimeout(function () { viewModeil.DoInactive(viewModeil); }, lifeTimeInMiliseconds)


    }

    DoInactive(viewModeil: any) {

        if (this.CurrentSession == null) this.CurrentSession = SessionLocator.SelectedSession;

        if (viewModeil.IsSignout) {
            if (!SessionLocator.IsSiguOut) {
                SessionLocator.IsSiguOut = true;
                var messageWindow: MessageWindow = new MessageWindow();
               // messageWindow.Title = "Logitude Message";
                messageWindow.ShowWarningIcon = true;
                messageWindow.IsOverAll = true;
                var message: string = this.IsTokenExpiration ? "Your session has expired, Please login again" :"Logged out due to inactivity, you can login again to enter the system";
                messageWindow.Show(message);
                SessionLocator.StopApplicationTimers();
                messageWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        SessionLocator.HomeComponent.SignoutClicked();
                        viewModeil.IsSignout = false;
                    }
                });
            }
        }
        else if (this.CurrentSession != null && this.CurrentSession.SessionLocation != null) {
            viewModeil.ShowMessage(viewModeil);
            window.clearTimeout(viewModeil.timeoutId)
            viewModeil.StartTimer(viewModeil, viewModeil.WarningTimeInMiliseconds);
            viewModeil.IsSignout = true;
        }


    }

    IsShowMessageWindow: boolean = false;
    messageWindow: MessageWindow;
    ShowMessage(viewModeil:any) {
        viewModeil.messageWindow  = new MessageWindow();
       // viewModeil.messageWindow.Title = "Logitude Message";
        viewModeil.messageWindow.ShowWarningIcon = true;
        var minutes = viewModeil.WarningTimeInMiliseconds / 60000;

        viewModeil.WarningTimeInMinute = minutes;
      
        var displayWarningTime = "";
        if ((minutes % 60) == 0) {
            displayWarningTime = (minutes / 60) + " hour";
        } else {
            displayWarningTime = minutes + " minute";
        }


        //var displayWarningTime = "";
        //if ((minutes % 60) == 0) {
        //    displayWarningTime = (minutes / 60) + " hour";
        //} else {

        //    var hour: any = minutes / 60;
        //    var minute: any = minutes % 60;
        //    //4 Hour 40 Minute
        //    displayWarningTime = minutes + " minute";
        //}

        var warningMessage: string = !viewModeil.IsTokenExpiration ? "You will be logged out in " + displayWarningTime + " due to inactivity, unless you continue using the system" : "You will be logged out in " + displayWarningTime + " due to session expiration";
        viewModeil.messageWindow.Show(warningMessage);

        viewModeil.messageWindow.WindowClosed.subscribe(s => {
            if (s) {
                viewModeil.messageWindow = null;
                viewModeil.IsStopTimer = true;
                viewModeil.IsShowMessageWindow = false;

                if (!viewModeil.IsTokenExpiration) {
                    viewModeil.IsSignout = false;
                    window.clearTimeout(viewModeil.timeoutId)
                    viewModeil.StartTimer(viewModeil);
                }
            }
        });

        viewModeil.IsShowMessageWindow = true;
        viewModeil.IsStopTimer = false;
        viewModeil.StartWarningTimeTimer(viewModeil);
        

    }



    ResetTimer(viewModeil: any) {
        if (!viewModeil.IsTokenExpiration && !viewModeil.IsShowMessageWindow) {
            viewModeil.IsSignout = false;
            window.clearTimeout(viewModeil.timeoutId)
            viewModeil.StartTimer(viewModeil);
        }
    }


    WarningTimeTimer() {
      return interval(60000).pipe(timeInterval());       
    }

    WarningTimeInMinute: number;
    private WarningTimesub: any = null;
    IsStopTimer: boolean = false;
    StartWarningTimeTimer(viewModeil:any) {

        this.WarningTimesub = this.WarningTimeTimer().subscribe((res:any) => {

            if (!this.IsStopTimer) {
                if (viewModeil.messageWindow) {
                    viewModeil.WarningTimeInMinute = viewModeil.WarningTimeInMinute - 1;
                    
                    if (viewModeil.WarningTimeInMinute > 0) {
                        var warningMessage: string = !viewModeil.IsTokenExpiration ? "You will be logged out in " + (viewModeil.WarningTimeInMinute).toString() + " minute due to inactivity, unless you continue using the system" : "You will be logged out in " + (viewModeil.WarningTimeInMinute).toString() + " minute due to session expiration";

                        viewModeil.messageWindow.Message = warningMessage;
                    } else viewModeil.IsStopTimer = true;
                }
            
            }


        });
    }
}
