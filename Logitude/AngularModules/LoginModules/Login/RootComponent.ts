import {Component, OnInit, ViewChild, ViewContainerRef, Compiler, ComponentFactoryResolver} from '@angular/core';
import {Http} from '@angular/http';
import {DynamicLoaderTSC} from './Utilities/DynamicLoaderTSC';
import {SessionInfo} from './SessionInfo';
import {Tools} from './Utilities/Tools';
import {ExternalParams, ExternalParamsArg} from './Utilities/ExternalParams';
//import {ServiceHelper} from './Utilities/ServiceHelper';
//import {SessionLocator} from './Utilities/SessionLocator';
//import {ExternalParams, ExternalParamsArg} from './Utilities/ExternalParams';
//import {TermsofUseService} from './Services/WebServices/TermsofUseService';
//import {SessionInfo} from './Utilities/SessionInfo'
//import {ServiceResponse} from './DataContracts/ServiceResponse';
//import {TermsofUseArgs} from './DataContracts/TermsofUseArgs';
declare var changeFavicon: any;
declare var changeTitle: any;
declare var IsMobileDetected;


@Component({
    selector: 'RootComponent',
    template:
    `
        <div class="MediaFillRelative">
            <div #Child></div>
        </div>
    `,
})

export class RootComponent implements OnInit {
    @ViewChild("Child", { read: ViewContainerRef }) location: ViewContainerRef;
    isDSV: boolean = true;
    ResetPWD: string;
    //DynamicLoader: DynamicLoaderTSC;
    constructor(compiler: Compiler, resolver: ComponentFactoryResolver, private http: Http) {
        //ServiceHelper.Http = http;
        DynamicLoaderTSC.Compiler = compiler;
        DynamicLoaderTSC.Resolver = resolver;
        Tools.DynamicLoader = DynamicLoaderTSC;
        this.ResetPWD = window.sessionStorage.getItem("ResetPWD");
        //this.DynamicLoader = DynamicLoaderTSC;
        //SessionLocator.IsProduction = false;

        var data = window.sessionStorage.getItem('userdata');

        if (data != "SignOut") {
            this.BuildExternalParams();
        }
    }

    BuildExternalParams() {
        //var url = 'login.aspx?Menu=Tickets&Action=Query&Parmters=[{"CompanyId":"1-720","ShipmentId":"1-26027", "ShipmentNumber" : "bdf22789-46e3-4"}]';
        var url = window.location.href;
        if (url) {
            var keys = url.split('?');
            if (keys[1]) {
                var vars = keys[1].split('&');
                if (vars) {
                    SessionInfo.myExternalParams = new ExternalParams();
                    SessionInfo.IsExternalParams = true;
                    for (var i = 0; i < vars.length; i++) {
                        var pair = vars[i].split('=');
                        SessionInfo.myExternalParams[decodeURIComponent(pair[0])] = pair[1];
                        if (decodeURIComponent(pair[0]) == "Parmters") {
                            var str1 = pair[1];
                            var str = decodeURIComponent(str1);
                            var jsonPMKeys = JSON.parse(str);
                            for (var key in jsonPMKeys) {
                                var arg = new ExternalParamsArg();
                                arg.FieldName = key;
                                arg.FieldValue = jsonPMKeys[key];
                                SessionInfo.myExternalParams.Args.push(arg);
                            }
                        }
                    }
                }
            }
        }
    }

    ngOnInit() {

        var url = window.location.href;
        var data = window.sessionStorage.getItem('userdata');

        this.isDSV = url.toLowerCase().indexOf(".dsv.") > -1 ? true : false;
        if (this.isDSV) {
            SessionInfo.PlShortName = "DSV";
            //Temp Code, must changed to dynamic 
            changeFavicon('data:image/JPEG;base64,/9j/4AAQSkZJRgABAAAAAQABAAD//gAgSnBlZyBDb2RlYyB8IGZsdXhjYXBhY2l0eS5uZXQg/9sAhAADAgIDAgIDAwMDBAMDBAUIBQUEBAUKBwcGCAwKDAwLCgsLDQ4SEA0OEQ4LCxAWEBETFBUVFQwPFxgWFBgSFBUUAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wAARCAAZABkDAREAAhEBAxEB/8QBogAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoLEAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+foBAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKCxEAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD4pyK/SzwQyKADNAz6i8H/APIpaJ/14wf+i1r8kx3+9Vf8UvzYHs3wJ+FKT+CPg/cy+EvAfirwrrdvO3iO61KyjtrvTIVYBXMzT5kdgXxhP4MEDOR9tiK1p1FzSUlt2f4HbCOkdEz0YeCfhn4lguNR8G+FPCuj+FNNspZIr3XdCtri3vTCzBiLgXolAbbgExdATk5FcvtK0NKkm5N9G9L+VrfiaWi9YrQ8X+I/w8+FPww8C+Mfi5pkGkanpnjPS7a38KeH3xN9gu513XB2H7vl7GIJ+7yvBxXfSq16044d3Ti3zPult95jKMIpzXXY4TwfHHD4L0WTht2n24A9/LXNfG46MYYirLvJ/myeWMIc3c+WBPKI/LEr7MY27jjH0r9ROYQTSCPyxI/l/wB3ccflQAhdiioXYqvRSeBQI+n/AAf/AMilon/XjB/6LWvyTG/71V/xS/NjD/hD9B/6Amnf+Akf+FH13Ff8/Zf+BP8AzEH/AAh+g/8AQE07/wABI/8ACj67iv8An7L/AMCf+YB/wh+g/wDQE07/AMBI/wDCj67iv+fsv/An/mBqwwx28SRRIsUSKFREGFUDoAOwrklJyblJ3bAA/9k=');
            changeTitle("DSV");
        }
        //if ((data && data == "SignOut") || (!data && !SessionLocator.IsExternalParams && url.indexOf('localhost') == -1)) {
        //    document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
        //}

        //else {
        SessionInfo.MainLocation = this.location;
            this.LoadLoginPage();

        //    if (url && url.indexOf('localhost') == -1) {
        //        window.onbeforeunload = function () {
        //            if (SessionLocator.ExternalParams && SessionLocator.ExternalParams.OneTimePasswordId) {
        //                return "when you leave this site can't not be used the key agin";
        //            }
        //            else if (!SessionLocator.IsSiguOut) {
        //                return "Are you sure you want to leave this page ?";
        //            }
        //        };
        //    }
        //}
    }  

    private ClearLocation() {
        if (this.location) {
            this.location.clear();
        }
    }
  
    LoadLoginPage() {
        this.ClearLocation();

       if (!this.isDSV) {
           if (this.ResetPWD == "true") {
               DynamicLoaderTSC.Load("./Login/Components/ChangePasswordComponent", this.location)
                    .then(cmpRef => {
                        window.sessionStorage.setItem("ResetPWD", "false");
                    });
            }
            else {
               DynamicLoaderTSC.Load("./Login/Components/DSVLoginComponent", this.location)
                    .then(cmpRef => {

                        //cmpRef.instance.Blocking.subscribe(s => {
                        //    SessionLocator.BlockType = s;
                        //    this.LoadBlockingScreen();
                        //});

                        //cmpRef.instance.LoginCompleted.subscribe(s => {
                        //    this.OnLoginCompleted();
                        //});
                    });
            } 
        }
        else {
            if (this.ResetPWD == "true") {
                DynamicLoaderTSC.Load("./Login/Components/DSVChangePasswordComponent", this.location)
                    .then(cmpRef => {
                        window.sessionStorage.setItem("ResetPWD", "false");
                    });
            }
            else {
                if (SessionInfo.IsExternalParams) {
                    if (SessionInfo.myExternalParams) {
                        if (SessionInfo.myExternalParams.Menu && SessionInfo.myExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
                            this.LoadDSVMobileLoginPage();
                        }
                        else {
                            this.LoadDSVLoginPage();
                        }
                    }
                    else {
                        this.LoadDSVLoginPage();
                    }
                }
                else {
                    this.LoadDSVLoginPage();
                }

            }
        }
    }

    LoadDSVLoginPage() {
        DynamicLoaderTSC.Load("./Login/Components/DSVLoginComponent", this.location)
            .then(cmpRef => { });
    }

    LoadDSVMobileLoginPage() {
        DynamicLoaderTSC.Load("./Login/Components/DSVMobileLoginComponent", this.location)
            .then(cmpRef => { });
    }
   
}