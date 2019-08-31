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
var ConfirmWindow = /** @class */ (function () {
    function ConfirmWindow() {
        this.Width = 320;
        this.Height = 170;
        this.Message = null;
        this.ShowCheckBox = false;
        this.Title = "Confirm";
        this.No = false;
        this.Yes = false;
        this.Cancel = false;
        this.ShowCancelButton = false;
        this.ShowNoButton = true;
        this.IsOverAll = false;
        this.ShowWarningImage = false;
        this.LayoutDirection = 'ltr';
        this.WindowClosed = new core_1.EventEmitter();
        this.IsChecked = false;
        this.IsYesEnabled = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ComponentRef = null;
        this.InstanceComponent = null;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        this.CancelButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Cancel");
        this.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
        this.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.No");
    }
    ConfirmWindow.prototype.Show = function (message) {
        var _this = this;
        this.Message = message;
        var viewContainerRefLocation = this.CurrentSession.SessionLocation.viewContainerRef;
        if (this.IsOverAll) {
            viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
        }
        else if (this.CurrentSession.CurrentWindow) {
            if (this.CurrentSession.CurrentWindow.IsOverAll) {
                this.IsOverAll = true;
                viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Controls/Windows/ConfirmWindowTemplateComponent", viewContainerRefLocation)
            .then(function (cmpRef) {
            _this.ComponentRef = cmpRef;
            _this.InstanceComponent = cmpRef.instance;
            _this.InstanceComponent.InjectWindowComponent(_this);
        });
    };
    ConfirmWindow.prototype.WindowClosedPromise = function () {
        var _this = this;
        return new Promise(function (resolve) {
            _this.WindowClosed.subscribe(function (event) {
                resolve(_this);
            });
        });
    };
    ConfirmWindow.prototype.Close = function () {
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
    ], ConfirmWindow.prototype, "WindowClosed", void 0);
    return ConfirmWindow;
}());
exports.ConfirmWindow = ConfirmWindow;
var ConfirmWindowTemplateComponent = /** @class */ (function () {
    function ConfirmWindowTemplateComponent() {
        this.Top = "50%";
        this.Left = "50%";
        this.Width = "320px";
        this.Height = "170px";
        this.Title = null;
        this.Message = null;
        this.ShowModal = true;
        this.WindowId = null;
        this.ShowCancelButton = false;
        this.ShowNoButton = true;
        this.NoButtonId = null;
        this.YesButtonId = null;
        this.CancelButtonId = null;
        this.NoButtonText = "No";
        this.YesButtonText = "Yes";
        this.CancelButtonText = "Cancel";
        this.IsOverAll = false;
        this.ShowWarningImage = false;
        this.LayoutDirection = 'ltr';
        this.ShowCheckBox = false;
        this.IsYesEnabled = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.last = null;
        this.IsMouseCapture = false;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        this.CancelButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Cancel");
        this.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
        this.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.No");
    }
    ConfirmWindowTemplateComponent.prototype.ngAfterViewInit = function () {
        this.isLoaderReady = true;
        this.Focus();
    };
    ConfirmWindowTemplateComponent.prototype.InjectWindowComponent = function (myWindow) {
        this.CreateDynamicIds();
        this.ConfirmWindow = myWindow;
        this.Title = myWindow.Title;
        this.Message = myWindow.Message;
        this.ShowCancelButton = myWindow.ShowCancelButton;
        this.ShowNoButton = myWindow.ShowNoButton;
        this.NoButtonText = myWindow.NoButtonText;
        this.YesButtonText = myWindow.YesButtonText;
        this.IsOverAll = myWindow.IsOverAll;
        this.ShowWarningImage = myWindow.ShowWarningImage;
        this.ShowCheckBox = myWindow.ShowCheckBox;
        this.IsYesEnabled = myWindow.IsYesEnabled;
        if (myWindow.Width != null) {
            this.Width = myWindow.Width + "px";
        }
        if (myWindow.Height != null) {
            this.Height = myWindow.Height + "px";
        }
    };
    Object.defineProperty(ConfirmWindowTemplateComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            if (this.isChecked != newValue) {
                this.isChecked = newValue;
                this.IsYesEnabled = newValue;
                this.ConfirmWindow.IsChecked = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ConfirmWindowTemplateComponent.prototype.CreateDynamicIds = function () {
        this.WindowId = "ConfirmWindow_" + this.CurrentSession.SessionIndex;
        this.NoButtonId = "ConfirmWindow_No_" + this.CurrentSession.SessionIndex;
        this.YesButtonId = "ConfirmWindow_Yes_" + this.CurrentSession.SessionIndex;
        this.CancelButtonId = "ConfirmWindow_Cancel_" + this.CurrentSession.SessionIndex;
        this.Focus();
    };
    ConfirmWindowTemplateComponent.prototype.Focus = function () {
        if (this.isLoaderReady) {
            if (this.ShowCancelButton) {
                if (this.CancelButtonId != null) {
                    document.getElementById(this.CancelButtonId).focus();
                }
            }
            else {
                if (this.NoButtonId != null && this.ShowNoButton) {
                    document.getElementById(this.NoButtonId).focus();
                }
            }
        }
    };
    ConfirmWindowTemplateComponent.prototype.OnMouseDown = function () {
        this.IsMouseCapture = true;
    };
    ConfirmWindowTemplateComponent.prototype.onMouseup = function (event) {
        this.IsMouseCapture = false;
    };
    ConfirmWindowTemplateComponent.prototype.onMousemove = function (event) {
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
    ConfirmWindowTemplateComponent.prototype.CancelButtonClicked = function () {
        this.ConfirmWindow.Cancel = true;
        this.ConfirmWindow.Close();
    };
    ConfirmWindowTemplateComponent.prototype.NoButtonClicked = function () {
        this.ConfirmWindow.No = true;
        this.ConfirmWindow.Close();
    };
    ConfirmWindowTemplateComponent.prototype.YesButtonClicked = function () {
        this.ConfirmWindow.Yes = true;
        this.ConfirmWindow.Close();
    };
    __decorate([
        core_1.HostListener('document:mouseup', ['$event']),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [MouseEvent]),
        __metadata("design:returntype", void 0)
    ], ConfirmWindowTemplateComponent.prototype, "onMouseup", null);
    __decorate([
        core_1.HostListener('document:mousemove', ['$event']),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [MouseEvent]),
        __metadata("design:returntype", void 0)
    ], ConfirmWindowTemplateComponent.prototype, "onMousemove", null);
    ConfirmWindowTemplateComponent = __decorate([
        core_1.Component({
            selector: 'ConfirmWindow',
            moduleId: module.id,
            templateUrl: "./ConfirmWindow.html",
        }),
        __metadata("design:paramtypes", [])
    ], ConfirmWindowTemplateComponent);
    return ConfirmWindowTemplateComponent;
}());
exports.ConfirmWindowTemplateComponent = ConfirmWindowTemplateComponent;
//# sourceMappingURL=ConfirmWindow.js.map