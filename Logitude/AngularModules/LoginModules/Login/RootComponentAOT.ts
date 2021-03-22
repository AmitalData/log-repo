import { Component, OnInit, ViewChild, ViewContainerRef, Compiler, ComponentFactoryResolver } from '@angular/core';
import {Http} from '@angular/http';
import {DynamicLoaderAOT} from './Utilities/DynamicLoaderAOT';
import {Tools} from './Utilities/Tools';
import {ExternalParams, ExternalParamsArg} from './Utilities/ExternalParams';
import {SessionInfo} from './SessionInfo'
declare var changeFavicon: any;
declare var changeTitle: any;
declare var IsMobileDetected;

@Component({
    selector: 'RootComponentAOT',
    template:
    `
        <div class="MediaFillRelative">
            <div #Child></div>
        </div>
    `,
})

export class RootComponentAOT implements OnInit {
    @ViewChild("Child", { read: ViewContainerRef }) location: ViewContainerRef;

    isPrivateLable: boolean = false;
    isDSV: boolean = false;
    ResetPWD: string;
    constructor(compiler: Compiler, private resolver: ComponentFactoryResolver, private http: Http) {
        //ServiceHelper.Http = http;
        DynamicLoaderAOT.Compiler = compiler;
        DynamicLoaderAOT.Resolver = resolver;
        Tools.DynamicLoader = DynamicLoaderAOT;
        this.ResetPWD = window.sessionStorage.getItem("ResetPWD");
        //SessionLocator.DynamicLoader = DynamicLoaderAOT;
        //SessionLocator.IsProduction = true;

        const data = window.sessionStorage.getItem('userdata');
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
        this.isPrivateLable = window.sessionStorage.getItem("IsPrivateLabel") == "true";
        this.isDSV = window.sessionStorage.getItem("IsDSV") == "true";
        if (this.isPrivateLable) {
            const privateLableShortName = window.sessionStorage.getItem("PrivateLabelShortName");
            SessionInfo.PlShortName = privateLableShortName;
            changeFavicon(window.sessionStorage.getItem("SmallLogoURL"));
            changeTitle(privateLableShortName);
        }
        SessionInfo.MainLocation = this.location;
        this.LoadLoginPage();
    }    

    private ClearLocation() {
        if (this.location) {
            this.location.clear();
        }
    }
  
    LoadLoginPage() {
        this.ClearLocation();
        if (!this.isPrivateLable) {
            if (this.ResetPWD == "true") {
                DynamicLoaderAOT.Load("./Login/Components/ChangePasswordComponent", this.location)
                    .then(cmpRef => {
                        window.sessionStorage.setItem("ResetPWD", "false");
                    });
            }
            else {
                DynamicLoaderAOT.Load("./Login/Components/LoginComponent", this.location)
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
            this.LoadPrivateLableLoginPages();
        }
    }

    LoadPrivateLableLoginPages() {
        if (this.ResetPWD == "true") {
            this.LoadPrivateLableChangePasswordPage();
        }
        else {
            if (SessionInfo.IsExternalParams) {
                if (SessionInfo.myExternalParams) {
                    if (SessionInfo.myExternalParams.Menu && SessionInfo.myExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
                        this.LoadDSVMobileLoginPage();
                    }
                    else {
                        this.LoadPrivateLableLoginPage();
                    }
                }
                else {
                    this.LoadPrivateLableLoginPage();
                }
            }
            else {
                this.LoadPrivateLableLoginPage();
            }
        }
    }

    LoadPrivateLableChangePasswordPage() {
        DynamicLoaderAOT.Load("./Login/Components/DSVChangePasswordComponent", this.location)
            .then(cmpRef => {
                window.sessionStorage.setItem("ResetPWD", "false");
            });
    }

    LoadPrivateLableLoginPage() {
        DynamicLoaderAOT.Load("./Login/PrivateLabels/LoginComponents/PrivateLoginComponent", this.location)
            .then(cmpRef => { });
    }

    LoadDSVMobileLoginPage() {
        DynamicLoaderAOT.Load("./Login/Components/DSVMobileLoginComponent", this.location)
            .then(cmpRef => { });
    }
 
}