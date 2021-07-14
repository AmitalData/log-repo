import { Component, ViewChild, ViewContainerRef, Compiler, ComponentFactoryResolver } from '@angular/core';
import { Http } from '@angular/http';
import { DynamicLoaderAOT } from './Utilities/DynamicLoaderAOT';
import { Tools } from './Utilities/Tools';
import { ExternalParams, ExternalParamsArg } from './Utilities/ExternalParams';
import { SessionInfo } from './SessionInfo';
import { PrivateLabelsService } from './PrivateLabels/Services/PrivateLabelsService';
export var RootComponentAOT = (function () {
    function RootComponentAOT(compiler, resolver, http, privateLabelsService) {
        this.resolver = resolver;
        this.http = http;
        this.privateLabelsService = privateLabelsService;
        this.isPrivateLable = false;
        this.isDSV = false;
        //ServiceHelper.Http = http;
        DynamicLoaderAOT.Compiler = compiler;
        DynamicLoaderAOT.Resolver = resolver;
        Tools.DynamicLoader = DynamicLoaderAOT;
        this.ResetPWD = window.sessionStorage.getItem("ResetPWD");
        var data = window.sessionStorage.getItem('userdata');
        if (data != "SignOut") {
            this.BuildExternalParams();
        }
    }
    RootComponentAOT.prototype.BuildExternalParams = function () {
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
    };
    RootComponentAOT.prototype.ngOnInit = function () {
        window.sessionStorage.setItem("ssHHHH", "WssESEW");
        this.isPrivateLable = window.sessionStorage.getItem("IsPrivateLabel") == "true";
        this.isDSV = window.sessionStorage.getItem("IsDSV") == "true";
        if (this.isPrivateLable) {
            var privateLableShortName = window.sessionStorage.getItem("PrivateLabelShortName");
            SessionInfo.PlShortName = privateLableShortName;
            changeFavicon(window.sessionStorage.getItem("SmallLogoURL"));
            changeTitle(privateLableShortName);
        }
        else {
            //document.location.href = Tools.GetSystemURL() + "Login.aspx";
            this.GetPrivateLabelsData();
        }
        SessionInfo.MainLocation = this.location;
        this.LoadPrivateLableLoginPages();
    };
    RootComponentAOT.prototype.ClearLocation = function () {
        if (this.location) {
            this.location.clear();
        }
    };
    RootComponentAOT.prototype.LoadPrivateLableLoginPages = function () {
        this.ClearLocation();
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
    };
    RootComponentAOT.prototype.LoadPrivateLableChangePasswordPage = function () {
        DynamicLoaderAOT.Load("./Login/Components/DSVChangePasswordComponent", this.location)
            .then(function (cmpRef) {
            window.sessionStorage.setItem("ResetPWD", "false");
        });
    };
    RootComponentAOT.prototype.LoadPrivateLableLoginPage = function () {
        DynamicLoaderAOT.Load("./Login/PrivateLabels/LoginComponents/PrivateLoginComponent", this.location)
            .then(function (cmpRef) { });
    };
    RootComponentAOT.prototype.LoadDSVMobileLoginPage = function () {
        DynamicLoaderAOT.Load("./Login/Components/DSVMobileLoginComponent", this.location)
            .then(function (cmpRef) { });
    };
    RootComponentAOT.prototype.GetPrivateLabelsData = function () {
        this.privateLabelsService.GetIsPrivateLableUrl(Tools.GetSystemURL()).subscribe(function (response) {
            if (response.EnablePrivateLable) {
                window.sessionStorage.setItem("ContactEmail", response.ContactUsEmail);
                window.sessionStorage.setItem("IsPrivateLabel", "true");
                window.sessionStorage.setItem("SmallLogoURL", response.SmallLogoURL);
                window.sessionStorage.setItem("LogoURL", response.LogoURL);
                window.sessionStorage.setItem("PrivateLabelUrl", response.PrivateLabelUrl);
                window.sessionStorage.setItem("PrivateLabelShortName", response.PrivateLabelShortName);
                window.sessionStorage.setItem("IsDSV", (response.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1).toString());
            }
        });
    };
    RootComponentAOT.decorators = [
        { type: Component, args: [{
                    selector: 'RootComponentAOT',
                    template: "\n        <div class=\"MediaFillRelative\">\n            <div #Child></div>\n        </div>\n    ",
                },] },
    ];
    /** @nocollapse */
    RootComponentAOT.ctorParameters = [
        { type: Compiler, },
        { type: ComponentFactoryResolver, },
        { type: Http, },
        { type: PrivateLabelsService, },
    ];
    RootComponentAOT.propDecorators = {
        'location': [{ type: ViewChild, args: ["Child", { read: ViewContainerRef },] },],
    };
    return RootComponentAOT;
}());
//# sourceMappingURL=RootComponentAOT.js.map