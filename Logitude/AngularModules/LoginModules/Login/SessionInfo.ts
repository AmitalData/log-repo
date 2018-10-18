import {ViewContainerRef} from '@angular/core';
import {ExternalParams, ExternalParamsArg} from './Utilities/ExternalParams';


export class SessionInfo {

    private static plShortName: string = "";
    public static get PlShortName(): string { return this.plShortName; }
    public static set PlShortName(newValue: string) { this.plShortName = newValue; }

    private static mainLocation: ViewContainerRef;
    public static get MainLocation(): ViewContainerRef { return this.mainLocation; }
    public static set MainLocation(newValue: ViewContainerRef) { this.mainLocation = newValue; }

    private static loggedUserId: string;
    public static get LoggedUserId(): string { return this.loggedUserId; }
    public static set LoggedUserId(newValue: string) { this.loggedUserId = newValue; }

    private static loggedUserTenant: number;
    public static get LoggedUserTenant(): number { return this.loggedUserTenant; }
    public static set LoggedUserTenant(newValue: number) { this.loggedUserTenant = newValue; }

    private static token: string;
    public static get Token(): string { return this.token; }
    public static set Token(newValue: string) { this.token = newValue; }



    private static loggedSessionToken: string;
    public static get LoggedSessionToken(): string { return this.loggedSessionToken; }
    public static set LoggedSessionToken(newValue: string) { this.loggedSessionToken = newValue; }

    private static loggedUserEmail: string;
    public static get LoggedUserEmail(): string { return this.loggedUserEmail; }
    public static set LoggedUserEmail(newValue: string) { this.loggedUserEmail = newValue; }


    public static myExternalParams: ExternalParams = null;
    public static IsExternalParams: boolean = false;

    public static ClearExternalParams() {
        this.IsExternalParams = false;
    }


    public static GetLogitudeURL() {

        var logitude_url = location.href.replace('index.html', '');

        if (location.href.indexOf('localhost') > -1) {
            logitude_url = 'http://localhost:9996/';
        }

        else {
            var urlArr = location.href.split("/index.html");
            var url = urlArr[0];
            url = url.replace(url.substring(url.lastIndexOf('/'), url.length), "");
            logitude_url = url + "/";
        }

        return logitude_url;
    }
    //private static loggedUserPM: UserPM;
    //public static get LoggedUserPM(): UserPM { return this.loggedUserPM; }
    //public static set LoggedUserPM(newValue: UserPM)
    //{
    //    if (this.loggedUserPM != newValue) {
    //        this.loggedUserPM = newValue;
    //        SessionLocator.LoggedUserPM = newValue;

    //        if (newValue) {
    //            SessionLocator.LoggedUserId = newValue.Id;
    //        }
    //    }
    //}

  

}