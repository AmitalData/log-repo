
declare var System: any;
declare var window: any;
import { SessionLocator } from '../Utilities/SessionLocator';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';

export class DetectUserInActivity {
    WarningTimeInMiliseconds: any;
    LifeTimeInMiliseconds: any;
    timeoutId: any;
    IsSignout: boolean = false;
    IsTokenExpiration: boolean = false;
    constructor(isTokenExpiration: boolean = false) {
        this.IsTokenExpiration = isTokenExpiration;
    }

    //timeUnit // H:Hour , M:Minutes 

    Start(lifeTime: any, warningTime: any = 1, timeUnit: string = "H") {
        if (lifeTime) {

        
            if (timeUnit && timeUnit.toUpperCase() == "H") {
                lifeTime = lifeTime * 60;
                warningTime = warningTime * 60;
            }


            if (lifeTime == 60 && warningTime == 60 && !this.IsTokenExpiration) warningTime = 30;
        
                this.LifeTimeInMiliseconds = ((lifeTime - warningTime) * 60000);
                this.WarningTimeInMiliseconds = (warningTime * 60000);


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

        if (viewModeil.IsSignout) {
            SessionLocator.HomeComponent.SignoutClicked();
            viewModeil.IsSignout = false;
        }
        else if (SessionLocator.CurrentSession != null && SessionLocator.CurrentSession.SessionLocation != null) {
            viewModeil.ShowMessage(viewModeil);
            window.clearTimeout(viewModeil.timeoutId)
            viewModeil.StartTimer(viewModeil, viewModeil.WarningTimeInMiliseconds);
            viewModeil.IsSignout = true;
        }


    }


    ShowMessage(viewModeil:any) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Title = "Logitude Message";

        var minutes = viewModeil.WarningTimeInMiliseconds / 60000;

       //var hours = Math.floor(minutes / 60);
       // var minutes = minutes % 60;

        var displayWarningTime = "";
        if ((minutes % 60)  == 0) {
            displayWarningTime = (minutes / 60) + " hour";
        } else {
            displayWarningTime = minutes + " minute";
        }

        var warningMessage: string = !viewModeil.IsTokenExpiration ? "You will be logged out in " + displayWarningTime + " due to inactivity, unless you continue using the system" : "You will be logged out in " + displayWarningTime + " due to session expiration";
        messageWindow.Show(warningMessage);
    }



    ResetTimer(viewModeil: any) {
        if (!viewModeil.IsTokenExpiration) {
            viewModeil.IsSignout = false;
            window.clearTimeout(viewModeil.timeoutId)
            viewModeil.StartTimer(viewModeil);
        }
    }


}
