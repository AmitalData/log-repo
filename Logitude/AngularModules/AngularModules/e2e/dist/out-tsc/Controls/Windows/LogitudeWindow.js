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
var LogitudeWindow = /** @class */ (function () {
    function LogitudeWindow() {
        this.Width = 750;
        this.Height = 500;
        //public Title: string = null;
        this.TitleIcon = null;
        this.CustomTitleIcon = null;
        this.WindowIndex = null;
        this.IsOverWindow = false;
        this.IsSameWindowSize = false;
        this.IsShowCloseButton = false;
        this.IsFillScreen = false;
        this.IsFillScreenHeight = false;
        this.WindowId = null;
        this.WindowContainerId = null;
        this.NotifyOnClose = false;
        this.ShowCloseButton = false;
        this.ShowHeaderButtons = false;
        this.IsHideHeader = false;
        this.IsEditComponent = false;
        this.IsFullScreen = false;
        this.IsOverAll = false;
        this.IsShowAutomationDelayTitle = false;
        this.ShowHelpIcon = false;
        this.HelpText = null;
        this.RTL = false;
        this.BottomBorderForTitle = "none";
        this.IsFillScreen_115 = false;
        this.LayoutDirection = 'ltr';
        this.ZIndex = 0;
        this.IsFillScreen_90 = false;
        this.SuppressBusyIndicator = false;
        this.IsHideWindowMargin = false;
        this.WindowClosed = new core_1.EventEmitter();
        this.ComponentLoaded = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ComponentRef = null;
        this.InstanceComponent = null;
        this.title = null;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        if (this.LayoutDirection == 'rtl') {
            this.RTL = true;
        }
    }
    LogitudeWindow.prototype.Show = function (myContent) {
        var _this = this;
        if (myContent != null) {
            var viewContainerRefLocation = this.CurrentSession.SessionLocation.viewContainerRef;
            if (this.CurrentSession.CurrentEditComponent) { // itzik : due crush !!!- abdulllah add this lines ...
                if (this.CurrentSession.CurrentEditComponent.IsSplitBtnVisible) {
                    viewContainerRefLocation = this.CurrentSession.CurrentEditComponent.WindowLocationViewContainerRef;
                }
            }
            if (this.CurrentSession.CurrentWindow) {
                this.IsOverWindow = true;
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
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Controls/Windows/LogitudeWindowTemplateComponent", viewContainerRefLocation)
                .then(function (cmpRef) {
                _this.ComponentRef = cmpRef;
                _this.InstanceComponent = cmpRef.instance;
                _this.WindowIndex = _this.CurrentSession.GetNewWindowIndex();
                if (_this.CurrentSession.CurrentWindow != null) {
                    if (_this.CurrentSession.CurrentWindow.IsEditComponent == false) {
                        _this.IsOverWindow = true;
                        if (_this.CurrentSession.CurrentWindow.Width == _this.Width && _this.CurrentSession.CurrentWindow.Height == _this.Height) {
                            _this.IsSameWindowSize = true;
                        }
                    }
                }
                _this.CurrentSession.AddWindow(_this);
                cmpRef.instance.InjectWindowComponent(myContent, _this);
            });
        }
    };
    LogitudeWindow.prototype.ShowEditComponent = function (entityId, objectTableName, selectedTabCode, isFillScreen) {
        var _this = this;
        if (selectedTabCode === void 0) { selectedTabCode = null; }
        if (isFillScreen === void 0) { isFillScreen = true; }
        this.IsFillScreen = isFillScreen;
        this.IsEditComponent = true;
        var viewContainerRefLocation = this.CurrentSession.SessionLocation.viewContainerRef;
        if (this.IsOverAll) {
            viewContainerRefLocation = SessionLocator_1.SessionLocator.ApplicationLocation;
        }
        else if (this.CurrentSession.CurrentWindow) {
            if (this.CurrentSession.CurrentWindow.IsOverAll) {
                this.IsOverAll = true;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Controls/Windows/LogitudeWindowTemplateComponent", viewContainerRefLocation)
            .then(function (cmpRef) {
            _this.ComponentRef = cmpRef;
            _this.InstanceComponent = cmpRef.instance;
            _this.WindowIndex = _this.CurrentSession.GetNewWindowIndex();
            if (_this.CurrentSession.CurrentWindow != null) {
                _this.IsOverWindow = true;
                if (_this.CurrentSession.CurrentWindow.Width == _this.Width && _this.CurrentSession.CurrentWindow.Height == _this.Height) {
                    _this.IsSameWindowSize = true;
                }
            }
            _this.CurrentSession.AddWindow(_this);
            cmpRef.instance.InjectEditComponent(entityId, objectTableName, _this, selectedTabCode);
        });
    };
    LogitudeWindow.prototype.Close = function (emit) {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
        if (this.InstanceComponent != null) {
            this.InstanceComponent.Destroy();
        }
        this.CurrentSession.RemoveWindow(this);
        this.WindowClosed.emit(emit);
    };
    LogitudeWindow.prototype.DestroyWindow = function () {
        //if (this.ComponentRef != null) {
        //    this.ComponentRef.destroy();
        //    this.ComponentRef = null;
        //}
        //if (this.InstanceComponent != null) {
        //    this.InstanceComponent.Dispose();
        //    this.InstanceComponent = null;
        //}
        //this.WindowArgs = null;
        //this.DataContext = null;
    };
    LogitudeWindow.prototype.StartBusyIndicator = function (myText) {
        if (this.InstanceComponent != null) {
            if (this.SuppressBusyIndicator) {
                ///
            }
            else {
                this.InstanceComponent.BusyIndicatorText = myText;
                this.InstanceComponent.ShowBusyIndicator = true;
            }
        }
    };
    LogitudeWindow.prototype.StopBusyIndicator = function () {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.BusyIndicatorText = null;
            this.InstanceComponent.ShowBusyIndicator = false;
        }
    };
    LogitudeWindow.prototype.ShowCancelControl = function (isVisible) {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.IsCancelControlVisible = isVisible;
        }
    };
    LogitudeWindow.prototype.ToShowCloseButton = function (isVisible) {
        if (this.InstanceComponent != null) {
            this.InstanceComponent.ShowCloseButton = isVisible;
        }
    };
    Object.defineProperty(LogitudeWindow.prototype, "Title", {
        get: function () { return this.title; },
        set: function (newValue) {
            this.title = newValue;
            if (this.InstanceComponent) {
                this.InstanceComponent.Title = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogitudeWindow.prototype, "WindowClosed", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogitudeWindow.prototype, "ComponentLoaded", void 0);
    return LogitudeWindow;
}());
exports.LogitudeWindow = LogitudeWindow;
var LogitudeWindowTemplateComponent = /** @class */ (function () {
    function LogitudeWindowTemplateComponent() {
        this.Top = "0";
        this.Left = "0";
        this.Width = "750px";
        this.Height = "500px";
        this.Title = null;
        this.TitleIcon = null;
        this.CustomTitleIcon = null;
        this.ShowModal = true;
        this.WindowId = null;
        this.WindowContainerId = null;
        this.FocusElementId = null;
        this.IsShowCloseButton = false;
        this.IsHideHeader = false;
        this.IsCancelControlVisible = false;
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
        this.ShowCloseButton = false;
        this.ShowHeaderButtons = false;
        this.IsWindowMaximize = false;
        this.NotifyOnClose = false;
        this.IsFullScreen = false;
        this.IsShowAutomationDelayTitle = false;
        this.ShowHelpIcon = false;
        this.HelpText = null;
        this.RTL = false;
        this.BottomBorderForTitle = "none";
        this.IsHideWindowMargin = false;
        this.LayoutDirection = 'ltr';
        this.ZIndex = 0;
        this.leftPadding = 0;
        this.IsOverAll = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ChildComponentPath = null;
        this.IsEditComponent = false;
        this.Retries = 0;
        this.ComponentRef = null;
        this.ComponentInstance = null;
        this.LayoutDirection = Settings_1.Settings.LayoutDirection;
        if (this.LayoutDirection == 'rtl') {
            this.RTL = true;
        }
    }
    LogitudeWindowTemplateComponent.prototype.ngAfterViewInit = function () {
        //this.isLoaderReady = true;
        //this.LoadComponent();
        this.Focus();
    };
    LogitudeWindowTemplateComponent.prototype.InjectWindowComponent = function (myComponentPath, logWindow) {
        this.logWindow = logWindow;
        this.IsEditComponent = false;
        this.CreateDynamicIds();
        this.Title = logWindow.Title;
        this.TitleIcon = logWindow.TitleIcon;
        this.WindowArgs = logWindow.WindowArgs;
        this.NewWizardArgs = logWindow.NewWizardArgs;
        this.DataContext = logWindow.DataContext;
        this.IsShowCloseButton = logWindow.IsShowCloseButton;
        this.ShowCloseButton = logWindow.ShowCloseButton;
        this.ShowHeaderButtons = logWindow.ShowHeaderButtons;
        this.NotifyOnClose = logWindow.NotifyOnClose;
        this.IsHideHeader = logWindow.IsHideHeader;
        this.IsFullScreen = logWindow.IsFullScreen;
        this.IsOverAll = logWindow.IsOverAll;
        this.IsShowAutomationDelayTitle = logWindow.IsShowAutomationDelayTitle;
        this.ShowHelpIcon = logWindow.ShowHelpIcon;
        this.ZIndex = logWindow.ZIndex;
        this.ChildComponentPath = myComponentPath;
        this.HelpText = logWindow.HelpText;
        this.RTL = logWindow.RTL;
        this.CustomTitleIcon = logWindow.CustomTitleIcon;
        this.BottomBorderForTitle = logWindow.BottomBorderForTitle;
        this.IsHideWindowMargin = logWindow.IsHideWindowMargin;
        this.SetWindowSize();
        this.RunComponent();
    };
    LogitudeWindowTemplateComponent.prototype.InjectEditComponent = function (entityId, objectTableName, logWindow, selectedTabCode) {
        if (selectedTabCode === void 0) { selectedTabCode = null; }
        this.logWindow = logWindow;
        this.IsEditComponent = true;
        this.EditComponentEntityId = entityId;
        this.EditComponentTableName = objectTableName;
        this.EditComponentTabCode = selectedTabCode;
        this.ShowHeaderButtons = true;
        this.CreateDynamicIds();
        this.Title = logWindow.Title;
        this.TitleIcon = logWindow.TitleIcon;
        this.WindowArgs = logWindow.WindowArgs;
        this.NewWizardArgs = logWindow.NewWizardArgs;
        this.DataContext = logWindow.DataContext;
        this.IsShowCloseButton = logWindow.IsShowCloseButton;
        this.IsHideHeader = logWindow.IsHideHeader;
        this.IsFullScreen = logWindow.IsFullScreen;
        this.IsShowAutomationDelayTitle = logWindow.IsShowAutomationDelayTitle;
        this.ShowHelpIcon = logWindow.ShowHelpIcon;
        this.ZIndex = logWindow.ZIndex;
        this.NotifyOnClose = logWindow.NotifyOnClose;
        this.BottomBorderForTitle = logWindow.BottomBorderForTitle;
        this.IsHideWindowMargin = logWindow.IsHideWindowMargin;
        this.ChildComponentPath = "./Infrastructure/Components/EditComponent/EditComponent";
        this.HelpText = logWindow.HelpText;
        this.RTL = logWindow.RTL;
        this.CustomTitleIcon = logWindow.CustomTitleIcon;
        this.SetWindowSize();
        this.RunComponent();
    };
    LogitudeWindowTemplateComponent.prototype.SetWindowSize = function () {
        var ApplicationSession = document.getElementById("ApplicationSession");
        if (ApplicationSession) {
            var SetOverProperty = false;
            var windowWidth = 100;
            var windowHeight = 100;
            var appWidth = ApplicationSession.clientWidth;
            var appHeight = ApplicationSession.clientHeight;
            if (this.logWindow.IsFullScreen) {
                windowWidth = appWidth;
                windowHeight = appHeight;
            }
            else if (this.logWindow.IsFillScreen) {
                windowWidth = appWidth - 50;
                windowHeight = appHeight - 50;
            }
            else if (this.logWindow.IsFillScreen_90) {
                windowWidth = appWidth - 90;
                windowHeight = appHeight - 90;
            }
            else if (this.logWindow.IsFillScreen_115) {
                windowWidth = appWidth - 115;
                windowHeight = appHeight - 115;
            }
            else if (this.logWindow.IsFillScreenHeight) {
                windowHeight = appHeight - 50;
                windowWidth = this.logWindow.Width;
            }
            else {
                SetOverProperty = true;
                if (this.logWindow.Width) {
                    windowWidth = this.logWindow.Width;
                }
                if (this.logWindow.Height) {
                    windowHeight = this.logWindow.Height;
                }
            }
            if (windowWidth >= appWidth) {
                windowWidth = appWidth - 20;
            }
            if (windowHeight >= appHeight) {
                windowHeight = appHeight - 20;
            }
            this.Width = windowWidth + "px";
            this.Height = windowHeight + "px";
            var topProperty = (appHeight - windowHeight) / 2;
            var leftProperty = (appWidth - windowWidth) / 2;
            //#region Abdullah: this code to paint the window over editcomponent section while split component is opened (customs)
            var windowPlaceholderWidth = null;
            var windowPlaceholderHeight = null;
            var isOverEditComponent = false;
            isOverEditComponent = this.CurrentSession.CurrentEditComponent != null && this.CurrentSession.CurrentEditComponent != undefined;
            // if (this.CurrentSession.CurrentWindow.IsOverWindow)
            //     isOverEditComponent = false;
            //change window position according to editcomponent location
            if (isOverEditComponent || (isOverEditComponent && this.logWindow.IsOverWindow)) {
                //get window location from edit component
                var editComponentCelId = this.CurrentSession.CurrentEditComponent.EditComponentCellId;
                var windowPlaceholderDiv = document.getElementById(editComponentCelId);
                if (windowPlaceholderDiv) {
                    windowPlaceholderWidth = windowPlaceholderDiv.clientWidth;
                    windowPlaceholderHeight = windowPlaceholderDiv.clientHeight;
                }
                //update top,left poisition
                topProperty = (windowPlaceholderHeight - windowHeight) / 2;
                leftProperty = (windowPlaceholderWidth - windowWidth) / 2;
            }
            //#endregion
            if (SetOverProperty) {
                if (this.logWindow.IsOverWindow) {
                    if (this.logWindow.IsSameWindowSize) {
                        if (!this.IsEditComponent) {
                            topProperty -= 10;
                            leftProperty += 10;
                        }
                    }
                }
            }
            this.Top = topProperty + "px";
            this.Left = leftProperty + "px";
        }
    };
    LogitudeWindowTemplateComponent.prototype.CreateDynamicIds = function () {
        this.WindowId = "LogitudeWindow_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.FocusElementId = "LogitudeWindowFocusElement_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.WindowContainerId = "LogitudeWindowContainerElement_" + this.CurrentSession.SessionIndex + "_" + this.CurrentSession.SessionWindowIndex;
        this.logWindow.WindowId = this.WindowId;
        this.logWindow.WindowContainerId = this.WindowContainerId;
    };
    LogitudeWindowTemplateComponent.prototype.Focus = function () {
        if (this.isLoaderReady) {
            if (this.FocusElementId != null) {
                var element = document.getElementById(this.FocusElementId);
                if (element != null) {
                    element.focus();
                }
            }
        }
    };
    LogitudeWindowTemplateComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.isLoaderReady = true;
            this.LoadComponent();
            this.Focus();
        }
        else {
            this.RunComponentTimer();
        }
    };
    LogitudeWindowTemplateComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    LogitudeWindowTemplateComponent.prototype.LoadComponent = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.ChildComponentPath != null) {
                if (this.IsEditComponent) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load(this.ChildComponentPath, this.viewContainerRef)
                        .then(function (cmpRef) {
                        _this.ComponentRef = cmpRef;
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.IsInsideWindow = true;
                        cmpRef.instance.ComponentBackground = "transparent";
                        cmpRef.instance.Run({ EntityId: _this.EditComponentEntityId, ObjectTableName: _this.EditComponentTableName, SelectedTabCode: _this.EditComponentTabCode });
                        _this.logWindow.ComponentLoaded.emit(_this.ComponentRef.instance);
                    });
                }
                else {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load(this.ChildComponentPath, this.viewContainerRef)
                        .then(function (cmpRef) {
                        _this.ComponentRef = cmpRef;
                        _this.ComponentInstance = _this.ComponentRef.instance;
                        if (_this.WindowArgs != null) {
                            if (_this.ComponentRef.instance['SetWindowArgs']) {
                                _this.ComponentRef.instance.SetWindowArgs(_this.WindowArgs);
                            }
                        }
                        if (_this.NewWizardArgs != null) {
                            if (_this.ComponentRef.instance['SetNewWizardArgs']) {
                                _this.ComponentRef.instance.SetNewWizardArgs(_this.NewWizardArgs);
                            }
                        }
                        if (_this.DataContext != null) {
                            if (_this.ComponentRef.instance['SetDataContext']) {
                                _this.ComponentRef.instance.SetDataContext(_this.DataContext);
                            }
                        }
                        _this.logWindow.ComponentLoaded.emit(_this.ComponentRef.instance);
                    });
                }
            }
        }
    };
    LogitudeWindowTemplateComponent.prototype.OnMouseDown = function (event) {
        dragger.startMoving(this.WindowId, this.WindowContainerId, event);
    };
    LogitudeWindowTemplateComponent.prototype.OnMouseUp = function () {
        dragger.stopMoving(this.WindowContainerId);
    };
    LogitudeWindowTemplateComponent.prototype.Destroy = function () {
        //if (this.ComponentRef != null) {
        //    this.ComponentRef.destroy();
        //    this.ComponentRef = null;
        //}
        //this.ChildComponent = null;
        //this.WindowArgs = null;
        //this.DataContext = null;
    };
    LogitudeWindowTemplateComponent.prototype.MaximizeClicked = function () {
        this.defaultWidth = this.Width;
        this.defaultHeight = this.Height;
        this.Width = "calc(100% - 10px)";
        this.Height = "calc(100% - 10px)";
        this.IsWindowMaximize = true;
    };
    LogitudeWindowTemplateComponent.prototype.RestoreClicked = function () {
        this.Width = this.defaultWidth;
        this.Height = this.defaultHeight;
        this.IsWindowMaximize = false;
    };
    LogitudeWindowTemplateComponent.prototype.CloseClicked = function () {
        if (this.IsEditComponent) {
            this.CurrentSession.CurrentEditComponent.BackButtonClicked();
        }
        else {
            if (this.NotifyOnClose) {
                if (this.ComponentInstance) {
                    this.ComponentInstance.OnWindowClosed();
                }
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    __decorate([
        core_1.ViewChild("WindowContent", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], LogitudeWindowTemplateComponent.prototype, "viewContainerRef", void 0);
    LogitudeWindowTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./LogitudeWindow.html",
        }),
        __metadata("design:paramtypes", [])
    ], LogitudeWindowTemplateComponent);
    return LogitudeWindowTemplateComponent;
}());
exports.LogitudeWindowTemplateComponent = LogitudeWindowTemplateComponent;
//# sourceMappingURL=LogitudeWindow.js.map