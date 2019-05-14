declare var System: any, window: any;
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {Http, Response} from '@angular/http';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {HybridPartnerExtendedListService} from '../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService';
import {HybridPartnerList} from '../../../../Common/EntityLists/HybridPartnerList';
import {WebFreightDomainService} from '../../../../Infrastructure/Services/WebFreightDomainService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CustomerPMService} from '../../../../Common/Services/StandardPMs/CustomerPMService';
import {CustomerTenantAccessRequestPM} from '../../../../Common/EntityPMs/CustomerTenantAccessRequestPM';
import {CustomerTenantAccessRequestExtendedPMService} from '../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService'
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';


@Component({
    selector: 'DropBoxConnection',
    moduleId: module.id,
    templateUrl: './DropBoxConnectionComponent.html',
})

export class DropBoxConnectionComponent implements OnInit, AfterViewInit {

    private messageWindow: MessageWindow = new MessageWindow();
    IsConnected: boolean = false;
    ShowTestButton: boolean = false;
    //Status: string = "Not Connected";
    //timer: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public CD: ChangeDetectorRef) {
        this.DropBoxWindowCLosed();
        if (FeatureLocator.HasFeaturePermession("General", "DROPBOXTESTFILE")) {
            this.ShowTestButton = true;
        }
        this.CurrentSession.SessionEvent.subscribe(res => {
            if (res.Name == "DropBoxWindowCLosed") {
                this.DropBoxWindowCLosed(res.Timer);
            }
        });
    }
    ngOnInit() {

    }
    ngAfterViewInit() {

    }

    ConnectToDropBox() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetDropBoxAuthURI(SessionLocator.Tenant).subscribe((myResult) => {
            var temp = myResult.Result;
            //this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
            //var new_window = window.open(temp, 'Authenticate with Dropbox', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=1300,height=650');
            //var timer = setInterval(function () {
            //    if (new_window) {
            //        if (new_window.closed) {
            //            this.CurrentSession.SessionEvent.emit({ Name: "DropBoxWindowCLosed", Timer: timer });
            //            console.log("Child window closed");
            //        }
            //    }
            //}, 500);
            this.PopupCenter(temp, 'Authenticate with Dropbox', 1000, 650);

        });
    }

    PopupCenter(url, title, w, h) {
        // Fixes dual-screen position                         Most browsers      Firefox
        var dualScreenLeft = window.screenLeft;//window.screenLeft != undefined ? window.screenLeft : screen.left;
        var dualScreenTop = window.screenTop;//window.screenTop != undefined ? window.screenTop : screen.top;

        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        var left = ((width / 2) - (w / 2)) + dualScreenLeft;
        var top = ((height / 2) - (h / 2)) + dualScreenTop;
        var new_window = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no');

        // Puts focus on the newWindow
        if (window.focus) {
            new_window.focus();
        }
        var timer = setInterval(function () {
            if (new_window) {
                if (new_window.closed) {
                    this.CurrentSession.SessionEvent.emit({ Name: "DropBoxWindowCLosed", Timer: timer });
                    console.log("Child window closed");
                }
            }
        }, 500);
    }
    DropBoxEmail: string = "";
    DropBoxWindowCLosed(timer: any = null) {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetDropBoxAccessTocken(SessionLocator.Tenant).subscribe((myResult) => {
            if (myResult.HasError == false && !AppTool.IsNullOrEmpty(myResult.Result.DropBoxAccessToken)) {
                this.DropBoxEmail = myResult.Result.DropBoxUEmail;
                var temp = myResult.Result;
                if (timer != null) {
                    clearInterval(timer);
                }
                this.IsConnected = true;
            }
            //else {
            //}

            //this.messageWindow.Width = 300;
            //this.messageWindow.Height = 200;
            //this.messageWindow.Title = "DropBox Communicaiton Log";
            //this.messageWindow.Message = "Communicaiton Log Created For DropBox Successfully";
            //this.messageWindow.Show(this.messageWindow.Message);

        });


    }

    CreateCommLogForDropBox() {
        var windowTitle = "Send DropBox Test File";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 550;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxTestFileComponent');
    }

    Disconnect() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetRedOfDropBoxAccessTocken(SessionLocator.Tenant).subscribe((myResult) => {
            if (myResult.HasError == false) {
                var temp = myResult.Result;
                this.IsConnected = false;
            }
        });
    }

    CheckConnection() {
        var myService: CommonDomainService = new CommonDomainService();
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Testing ...");
        myService.GetDropBoxConnectionTest(SessionLocator.Tenant).subscribe((myResult) => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 200;
            this.messageWindow.Title = "DropBox Connection Status";

            if (myResult.HasError == false) {
                this.messageWindow.Message = "Dropbox Connection is Valid";
            }
            else {
                this.messageWindow.Message = "Dropbox Connection is not Valid , Please disconnect and re-connect";
            }
            this.messageWindow.Show(this.messageWindow.Message);
        });
    }

    SendTestFile() {
        this.CreateCommLogForDropBox();
    }

    CloseBtnClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}

