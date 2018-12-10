"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var DynamicLoaderAOT_1 = require("./Utilities/DynamicLoaderAOT");
var Tools_1 = require("./Utilities/Tools");
var ExternalParams_1 = require("./Utilities/ExternalParams");
var SessionInfo_1 = require("./SessionInfo");
var RootComponentAOT = /** @class */ (function () {
    function RootComponentAOT(compiler, resolver, http) {
        this.resolver = resolver;
        this.http = http;
        this.isDSV = true;
        //ServiceHelper.Http = http;
        DynamicLoaderAOT_1.DynamicLoaderAOT.Compiler = compiler;
        DynamicLoaderAOT_1.DynamicLoaderAOT.Resolver = resolver;
        Tools_1.Tools.DynamicLoader = DynamicLoaderAOT_1.DynamicLoaderAOT;
        this.ResetPWD = window.sessionStorage.getItem("ResetPWD");
        //SessionLocator.DynamicLoader = DynamicLoaderAOT;
        //SessionLocator.IsProduction = true;
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
                    SessionInfo_1.SessionInfo.myExternalParams = new ExternalParams_1.ExternalParams();
                    SessionInfo_1.SessionInfo.IsExternalParams = true;
                    for (var i = 0; i < vars.length; i++) {
                        var pair = vars[i].split('=');
                        SessionInfo_1.SessionInfo.myExternalParams[decodeURIComponent(pair[0])] = pair[1];
                        if (decodeURIComponent(pair[0]) == "Parmters") {
                            var str1 = pair[1];
                            var str = decodeURIComponent(str1);
                            var jsonPMKeys = JSON.parse(str);
                            for (var key in jsonPMKeys) {
                                var arg = new ExternalParams_1.ExternalParamsArg();
                                arg.FieldName = key;
                                arg.FieldValue = jsonPMKeys[key];
                                SessionInfo_1.SessionInfo.myExternalParams.Args.push(arg);
                            }
                        }
                    }
                }
            }
        }
    };
    RootComponentAOT.prototype.ngOnInit = function () {
        var url = window.location.href;
        this.isDSV = url.toLowerCase().indexOf(".dsv.") > -1 ? true : false;
        if (this.isDSV) {
            SessionInfo_1.SessionInfo.PlShortName = "DSV";
            //Temp Code, must changed to dynamic 
            changeFavicon('data:image/JPEG;base64,/9j/4AAQSkZJRgABAAAAAQABAAD//gAgSnBlZyBDb2RlYyB8IGZsdXhjYXBhY2l0eS5uZXQg/9sAhAADAgIDAgIDAwMDBAMDBAUIBQUEBAUKBwcGCAwKDAwLCgsLDQ4SEA0OEQ4LCxAWEBETFBUVFQwPFxgWFBgSFBUUAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wAARCAAZABkDAREAAhEBAxEB/8QBogAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoLEAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+foBAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKCxEAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD4pyK/SzwQyKADNAz6i8H/APIpaJ/14wf+i1r8kx3+9Vf8UvzYHs3wJ+FKT+CPg/cy+EvAfirwrrdvO3iO61KyjtrvTIVYBXMzT5kdgXxhP4MEDOR9tiK1p1FzSUlt2f4HbCOkdEz0YeCfhn4lguNR8G+FPCuj+FNNspZIr3XdCtri3vTCzBiLgXolAbbgExdATk5FcvtK0NKkm5N9G9L+VrfiaWi9YrQ8X+I/w8+FPww8C+Mfi5pkGkanpnjPS7a38KeH3xN9gu513XB2H7vl7GIJ+7yvBxXfSq16044d3Ti3zPult95jKMIpzXXY4TwfHHD4L0WTht2n24A9/LXNfG46MYYirLvJ/myeWMIc3c+WBPKI/LEr7MY27jjH0r9ROYQTSCPyxI/l/wB3ccflQAhdiioXYqvRSeBQI+n/AAf/AMilon/XjB/6LWvyTG/71V/xS/NjD/hD9B/6Amnf+Akf+FH13Ff8/Zf+BP8AzEH/AAh+g/8AQE07/wABI/8ACj67iv8An7L/AMCf+YB/wh+g/wDQE07/AMBI/wDCj67iv+fsv/An/mBqwwx28SRRIsUSKFREGFUDoAOwrklJyblJ3bAA/9k=');
            changeTitle("DSV");
        }
        SessionInfo_1.SessionInfo.MainLocation = this.location;
        this.LoadLoginPage();
    };
    RootComponentAOT.prototype.ClearLocation = function () {
        if (this.location) {
            this.location.clear();
        }
    };
    RootComponentAOT.prototype.LoadLoginPage = function () {
        this.ClearLocation();
        if (!this.isDSV) {
            if (this.ResetPWD == "true") {
                DynamicLoaderAOT_1.DynamicLoaderAOT.Load("./Login/Components/ChangePasswordComponent", this.location)
                    .then(function (cmpRef) {
                    window.sessionStorage.setItem("ResetPWD", "false");
                });
            }
            else {
                DynamicLoaderAOT_1.DynamicLoaderAOT.Load("./Login/Components/LoginComponent", this.location)
                    .then(function (cmpRef) {
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
                DynamicLoaderAOT_1.DynamicLoaderAOT.Load("./Login/Components/DSVChangePasswordComponent", this.location)
                    .then(function (cmpRef) {
                    window.sessionStorage.setItem("ResetPWD", "false");
                });
            }
            else {
                if (SessionInfo_1.SessionInfo.IsExternalParams) {
                    if (SessionInfo_1.SessionInfo.myExternalParams) {
                        if (SessionInfo_1.SessionInfo.myExternalParams.Menu && SessionInfo_1.SessionInfo.myExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
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
    };
    RootComponentAOT.prototype.LoadDSVLoginPage = function () {
        DynamicLoaderAOT_1.DynamicLoaderAOT.Load("./Login/Components/DSVLoginComponent", this.location)
            .then(function (cmpRef) { });
    };
    RootComponentAOT.prototype.LoadDSVMobileLoginPage = function () {
        DynamicLoaderAOT_1.DynamicLoaderAOT.Load("./Login/Components/DSVMobileLoginComponent", this.location)
            .then(function (cmpRef) { });
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], RootComponentAOT.prototype, "location", void 0);
    RootComponentAOT = __decorate([
        core_1.Component({
            selector: 'RootComponentAOT',
            template: "\n        <div class=\"MediaFillRelative\">\n            <div #Child></div>\n        </div>\n    ",
        }),
        __metadata("design:paramtypes", [core_1.Compiler, core_1.ComponentFactoryResolver, http_1.Http])
    ], RootComponentAOT);
    return RootComponentAOT;
}());
exports.RootComponentAOT = RootComponentAOT;
//# sourceMappingURL=RootComponentAOT.js.map