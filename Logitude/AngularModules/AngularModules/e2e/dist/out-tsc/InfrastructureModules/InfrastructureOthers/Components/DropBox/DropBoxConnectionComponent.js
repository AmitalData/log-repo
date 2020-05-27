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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DropBoxConnectionComponent = /** @class */ (function () {
    function DropBoxConnectionComponent(CD) {
        var _this = this;
        this.CD = CD;
        this.messageWindow = new MessageWindow_1.MessageWindow();
        this.IsConnected = false;
        this.ShowTestButton = false;
        //Status: string = "Not Connected";
        //timer: any;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.DropBoxEmail = "";
        this.DropBoxWindowCLosed();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "DROPBOXTESTFILE")) {
            this.ShowTestButton = true;
        }
        this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res.Name == "DropBoxWindowCLosed") {
                _this.DropBoxWindowCLosed(res.Timer);
            }
        });
    }
    DropBoxConnectionComponent.prototype.ngOnInit = function () {
    };
    DropBoxConnectionComponent.prototype.ngAfterViewInit = function () {
    };
    DropBoxConnectionComponent.prototype.ConnectToDropBox = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetDropBoxAuthURI(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
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
            _this.PopupCenter(temp, 'Authenticate with Dropbox', 1000, 650);
        });
    };
    DropBoxConnectionComponent.prototype.PopupCenter = function (url, title, w, h) {
        // Fixes dual-screen position                         Most browsers      Firefox
        var dualScreenLeft = window.screenLeft; //window.screenLeft != undefined ? window.screenLeft : screen.left;
        var dualScreenTop = window.screenTop; //window.screenTop != undefined ? window.screenTop : screen.top;
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
    };
    DropBoxConnectionComponent.prototype.DropBoxWindowCLosed = function (timer) {
        var _this = this;
        if (timer === void 0) { timer = null; }
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetDropBoxAccessTocken(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            if (myResult.HasError == false && !Tools_1.AppTool.IsNullOrEmpty(myResult.Result.DropBoxAccessToken)) {
                _this.DropBoxEmail = myResult.Result.DropBoxUEmail;
                var temp = myResult.Result;
                if (timer != null) {
                    clearInterval(timer);
                }
                _this.IsConnected = true;
            }
            //else {
            //}
            //this.messageWindow.Width = 300;
            //this.messageWindow.Height = 200;
            //this.messageWindow.Title = "DropBox Communicaiton Log";
            //this.messageWindow.Message = "Communicaiton Log Created For DropBox Successfully";
            //this.messageWindow.Show(this.messageWindow.Message);
        });
    };
    DropBoxConnectionComponent.prototype.CreateCommLogForDropBox = function () {
        var windowTitle = "Send DropBox Test File";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 550;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxTestFileComponent');
    };
    DropBoxConnectionComponent.prototype.Disconnect = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetRedOfDropBoxAccessTocken(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            if (myResult.HasError == false) {
                var temp = myResult.Result;
                _this.IsConnected = false;
            }
        });
    };
    DropBoxConnectionComponent.prototype.CheckConnection = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Testing ...");
        myService.GetDropBoxConnectionTest(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.messageWindow.Width = 300;
            _this.messageWindow.Height = 200;
            _this.messageWindow.Title = "DropBox Connection Status";
            if (myResult.HasError == false) {
                _this.messageWindow.Message = "Dropbox Connection is Valid";
            }
            else {
                _this.messageWindow.Message = "Dropbox Connection is not Valid , Please disconnect and re-connect";
            }
            _this.messageWindow.Show(_this.messageWindow.Message);
        });
    };
    DropBoxConnectionComponent.prototype.SendTestFile = function () {
        this.CreateCommLogForDropBox();
    };
    DropBoxConnectionComponent.prototype.CloseBtnClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DropBoxConnectionComponent = __decorate([
        core_1.Component({
            selector: 'DropBoxConnection',
            moduleId: module.id,
            templateUrl: './DropBoxConnectionComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DropBoxConnectionComponent);
    return DropBoxConnectionComponent;
}());
exports.DropBoxConnectionComponent = DropBoxConnectionComponent;
//# sourceMappingURL=DropBoxConnectionComponent.js.map