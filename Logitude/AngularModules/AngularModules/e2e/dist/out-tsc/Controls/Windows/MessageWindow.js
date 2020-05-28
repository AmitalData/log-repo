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
var Settings_1 = require("../../Infrastructure/Settings");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var MessageWindow = /** @class */ (function () {
    function MessageWindow() {
        this.Width = 320;
        this.Height = 170;
        //public Message: string = null;
        this.Title = "Message";
        this.IsOverAll = false;
        this.LayoutDirection = 'ltr';
        this.OkButtonText = "Ok";
        this.ZIndex = 0;
        this.WindowClosed = new core_1.EventEmitter();
        this.RTL = false;
        this.ShowSuccessIcon = false;
        this.ShowErrorIcon = false;
        this.ShowWarningIcon = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.message = null;
        this.ComponentRef = null;
        this.InstanceComponent = null;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Message");
        this.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Ok");
    }
    Object.defineProperty(MessageWindow.prototype, "Message", {
        get: function () { return this.message; },
        set: function (newValue) {
            this.message = newValue;
            if (this.InstanceComponent) {
                this.InstanceComponent.Message = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    MessageWindow.prototype.Show = function (message) {
        var _this = this;
        this.Message = message;
        if (!this.CurrentSession) {
            this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        }
        if (this.CurrentSession.SessionLocation) {
            var viewContainerRefLocation = this.CurrentSession.SessionLocation.viewContainerRef;
        }
        else {
            viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
        }
        if (this.IsOverAll) {
            viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
        }
        else if (this.CurrentSession.CurrentWindow) {
            if (this.CurrentSession.CurrentWindow.IsOverAll) {
                this.IsOverAll = true;
                viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Controls/Windows/MessageWindowTemplateComponent", viewContainerRefLocation)
            .then(function (cmpRef) {
            _this.ComponentRef = cmpRef;
            _this.InstanceComponent = cmpRef.instance;
            _this.InstanceComponent.InjectWindowComponent(_this);
        });
    };
    MessageWindow.prototype.Close = function () {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
            this.WindowClosed.emit("event");
        }
        this.InstanceComponent = null;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], MessageWindow.prototype, "WindowClosed", void 0);
    return MessageWindow;
}());
exports.MessageWindow = MessageWindow;
var MessageWindowTemplateComponent = /** @class */ (function () {
    function MessageWindowTemplateComponent() {
        this.Top = "50%";
        this.Left = "50%";
        this.Width = "320px";
        this.Height = "170px";
        this.Title = null;
        this.Message = null;
        this.ShowModal = true;
        this.WindowId = null;
        this.OkButtonId = null;
        this.IsOverAll = false;
        this.OkButtonText = "Ok";
        this.ZIndex = 0;
        this.LayoutDirection = 'ltr';
        this.ShowSuccessIcon = false;
        this.ShowErrorIcon = false;
        this.ShowWarningIcon = false;
        this.RTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.last = null;
        this.IsMouseCapture = false;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Message");
        this.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Ok");
    }
    MessageWindowTemplateComponent.prototype.ngAfterViewInit = function () {
        this.isLoaderReady = true;
        this.Focus();
    };
    MessageWindowTemplateComponent.prototype.InjectWindowComponent = function (myWindow) {
        this.CreateDynamicIds();
        this.MessageWindow = myWindow;
        this.Title = myWindow.Title;
        this.Message = myWindow.Message;
        this.IsOverAll = myWindow.IsOverAll;
        this.ZIndex = myWindow.ZIndex;
        this.RTL = myWindow.RTL;
        this.ShowSuccessIcon = myWindow.ShowSuccessIcon;
        this.ShowErrorIcon = myWindow.ShowErrorIcon;
        this.ShowWarningIcon = myWindow.ShowWarningIcon;
        if (myWindow.Width != null) {
            this.Width = myWindow.Width + "px";
        }
        if (myWindow.Height != null) {
            this.Height = myWindow.Height + "px";
        }
    };
    MessageWindowTemplateComponent.prototype.CreateDynamicIds = function () {
        this.WindowId = "MessageWindow_" + this.CurrentSession.SessionIndex;
        this.OkButtonId = "MessageWindow_Ok_" + this.CurrentSession.SessionIndex;
        this.Focus();
    };
    MessageWindowTemplateComponent.prototype.Focus = function () {
        if (this.isLoaderReady) {
            if (this.OkButtonId != null) {
                document.getElementById(this.OkButtonId).focus();
            }
        }
    };
    MessageWindowTemplateComponent.prototype.OnMouseDown = function () {
        this.IsMouseCapture = true;
    };
    MessageWindowTemplateComponent.prototype.onMouseup = function (event) {
        this.IsMouseCapture = false;
    };
    MessageWindowTemplateComponent.prototype.onMousemove = function (event) {
        if (this.IsMouseCapture) {
            var windowObject = document.getElementById(this.WindowId);
            if (windowObject != null) {
                if (event != null) {
                    if (this.last != null) {
                        var rect = windowObject.getBoundingClientRect();
                        var yPosition = (windowObject.offsetTop + event.clientY - this.last.clientY);
                        var xPosition = (windowObject.offsetLeft + event.clientX - this.last.clientX);
                        var isDraggingLeft = false;
                        if ((event.clientX - this.last.clientX) < 0) {
                            isDraggingLeft = true;
                        }
                        if (isDraggingLeft) {
                            if (rect.left >= 10) {
                                windowObject.style.left = xPosition + 'px';
                            }
                        }
                        else {
                            if (rect.right <= (window.innerWidth - 10)) {
                                windowObject.style.left = xPosition + 'px';
                            }
                        }
                        var isDraggingTop = false;
                        if ((event.clientY - this.last.clientY) < 0) {
                            isDraggingTop = true;
                        }
                        if (isDraggingTop) {
                            if (rect.top >= 10) {
                                windowObject.style.top = yPosition + 'px';
                            }
                        }
                        else {
                            if (rect.bottom <= (window.innerHeight - 10)) {
                                windowObject.style.top = yPosition + 'px';
                            }
                        }
                    }
                    this.last = event;
                }
            }
        }
    };
    MessageWindowTemplateComponent.prototype.OkButtonClicked = function () {
        this.MessageWindow.Close();
    };
    MessageWindowTemplateComponent.prototype.GetIconPath = function () {
        var path = "./Images/InfoIcon.png";
        if (this.ShowSuccessIcon)
            path = "./Images/SuccessIcon.png";
        if (this.ShowErrorIcon)
            path = "./Images/ErrorIcon.png";
        if (this.ShowWarningIcon)
            path = "./Images/SimplogIcons/Warning.png";
        return path;
    };
    __decorate([
        core_1.HostListener('document:mouseup', ['$event']),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [MouseEvent]),
        __metadata("design:returntype", void 0)
    ], MessageWindowTemplateComponent.prototype, "onMouseup", null);
    __decorate([
        core_1.HostListener('document:mousemove', ['$event']),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [MouseEvent]),
        __metadata("design:returntype", void 0)
    ], MessageWindowTemplateComponent.prototype, "onMousemove", null);
    MessageWindowTemplateComponent = __decorate([
        core_1.Component({
            selector: 'MessageWindow',
            moduleId: module.id,
            templateUrl: "./MessageWindow.html",
        }),
        __metadata("design:paramtypes", [])
    ], MessageWindowTemplateComponent);
    return MessageWindowTemplateComponent;
}());
exports.MessageWindowTemplateComponent = MessageWindowTemplateComponent;
//# sourceMappingURL=MessageWindow.js.map