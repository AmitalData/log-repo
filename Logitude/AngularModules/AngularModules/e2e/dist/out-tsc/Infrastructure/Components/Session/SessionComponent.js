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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var LocationDirective_1 = require("../../Utilities/LocationDirective");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var LogitudeGridHelper_1 = require("../../Utilities/LogitudeGridHelper");
var ApiFiltersChangeEvent_1 = require("../../Utilities/events/ApiFiltersChangeEvent");
var AmitalGatewayUtil_1 = require("../../Utilities/AmitalGatewayUtil");
var Subscription_1 = require("rxjs/Subscription"); //itzik
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var SessionComponent = /** @class */ (function () {
    function SessionComponent(temp, ChangeDetectorRef) {
        this.temp = temp;
        this.ChangeDetectorRef = ChangeDetectorRef;
        this.SessionEvent = new core_1.EventEmitter();
        this.SessionInitialize = new core_1.EventEmitter();
        this.SessionSeleced = new core_1.EventEmitter();
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.isLoaderReady = false;
        this.Retries = 0;
        this.IdCounters = [];
        this.EndOfRowReachedEvent = new core_1.EventEmitter();
        this.PseventRowSelectEvent = new core_1.EventEmitter();
        this.WindowResizeEvent = new core_1.EventEmitter();
        this.MouseUpEvent = new core_1.EventEmitter();
        this.ObsNewElementInsertedEvent = new core_1.EventEmitter();
        this.QuantityTypeCodeLoadedEvent = new core_1.EventEmitter();
        this.CollateralAnswerRefreshEvent = new core_1.EventEmitter();
        this.ConnectedItemSelectedEvent = new core_1.EventEmitter();
        this.SelectItemEvent = new core_1.EventEmitter();
        this.CloseNotificationBellEvent = new core_1.EventEmitter();
        this.AccumulatedFilterChangedEvent = new core_1.EventEmitter();
        this.MouseDownEvent = new core_1.EventEmitter();
        this.SearchFilterChangedEvent = new core_1.EventEmitter();
        this.DisableFieldsEvent = new core_1.EventEmitter();
        this.CopyCellIntoMemory = new core_1.EventEmitter();
        this.ChartId = null;
        this.IsOpenDatabaseBackupWindowFromSetting = false;
        this.IsOpenChangePasswordWindowFromSetting = false;
        this.IsOpenSignatureWindowFromSetting = false;
        this.IsOpenDocumentBackupWindowFromSetting = false;
        this.isShiftClicked = false;
        this.isTabWithShiftClicked = false;
        this.AllowShiftTab = false;
        this.LostFocusEvent = new core_1.EventEmitter();
        this.IsShowErrorWindow = false;
        this._Subscription = new Subscription_1.Subscription(); //itzik///https://stackoverflow.com/a/42274637
        this.BusyIndicatorText = null;
        this.showBusyIndicator = false;
        this.CurrentWindow = null;
        this.SessionWindowIndex = null;
        this.CurrentEditComponent = null;
        this.SessionEditComponentIndex = null;
        this.CurrentLogGrid = "";
        this.CurrentListComponent = null;
        this.SessionListComponentIndex = null;
        //public DestroyS
        this.isDestroingSession = false;
        this.PubSubFiltersChangeEventService = temp;
        this.SessionWindowIndex = null;
        this.CurrentWindow = null;
        this.Windows = new Array();
        this.EditControls = new Array();
        this.ListControls = new Array();
        this.IdCounters = new Array();
        this.MenuReferences = new Array();
        window.onresize = this.onWindowResized.bind(this);
        //window.onmouseup = this.onMouseUp.bind(this);
        window.onmousedown = this.onMouseDown.bind(this);
    }
    Object.defineProperty(SessionComponent.prototype, "SessionLocation", {
        get: function () { return this.iSessionLocation; },
        set: function (value) {
            if (this.iSessionLocation != value) {
                if (value) {
                    this.iSessionLocation = value;
                }
                else if (this.isDestroingSession) {
                    this.iSessionLocation = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionComponent.prototype, "SessionMenuLocation", {
        get: function () { return this.iSessionMenuLocation; },
        set: function (value) {
            if (this.iSessionMenuLocation != value) {
                if (value) {
                    this.iSessionMenuLocation = value;
                }
                else if (this.isDestroingSession) {
                    this.iSessionMenuLocation = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SessionComponent.prototype.OnSessionMouseUp = function ($event) {
        this.MouseUpEvent.emit(event);
    };
    SessionComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'SessionContainer'; });
                var myLocation = locs[0];
                this.SessionLocation = myLocation;
                if (this.LogitudeGridHelper == null) {
                    this.LogitudeGridHelper = new LogitudeGridHelper_1.LogitudeGridHelper(this.SessionIndex);
                }
                this.SessionInitialize.emit(true);
                if (!SessionLocator_1.SessionLocator.IsNewSignupTenant) {
                    this.entityResourceService.getEntityResourceByTableName("General", 0).subscribe(function (response) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/MainMenu/MainMenuComponent", _this.SessionLocation.viewContainerRef).then(function (cmpRef) {
                            _this.MainMenuComponent = cmpRef.instance;
                            cmpRef.instance.RunComponent();
                        });
                    });
                }
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    SessionComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    SessionComponent.prototype.GetNewId = function (Name) {
        if (this.IdCounters == null) {
            this.IdCounters = new Array();
        }
        var idCounter = this.IdCounters.filter(function (f) { return f.Name == Name; })[0];
        if (idCounter) {
            idCounter.Counter = idCounter.Counter + 1;
        }
        else {
            idCounter = new SessionIdCounter(Name);
            this.IdCounters.push(idCounter);
        }
        return this.SessionIndex + "_" + idCounter.Counter;
    };
    SessionComponent.prototype.GetNewCounter = function (Name) {
        if (this.IdCounters == null) {
            this.IdCounters = new Array();
        }
        var idCounter = this.IdCounters.filter(function (f) { return f.Name == Name; })[0];
        if (idCounter) {
            idCounter.Counter = idCounter.Counter + 1;
        }
        else {
            idCounter = new SessionIdCounter(Name);
            this.IdCounters.push(idCounter);
        }
        return idCounter.Counter;
    };
    SessionComponent.prototype.SubscriptionAdd = function (teardown) {
        //    this.someService.change.subscribe(() => {
        //[...]
        //    })
        this._Subscription.add(teardown);
    };
    SessionComponent.prototype.UnsubscribeStaticEvent = function () {
        this._Subscription.unsubscribe(); //itzik
    };
    SessionComponent.prototype.onWindowResized = function (event) {
        this.WindowResizeEvent.emit(event);
    };
    SessionComponent.prototype.onMouseDown = function (event) {
        this.MouseDownEvent.emit(event);
    };
    SessionComponent.prototype.onMouseUp = function (event) {
        this.MouseUpEvent.emit(event);
    };
    SessionComponent.prototype.GetChartId = function () {
        if (this.ChartId == null) {
            this.ChartId = 0;
        }
        else {
            this.ChartId += 1;
        }
        return this.SessionIndex + "_" + this.ChartId;
    };
    Object.defineProperty(SessionComponent.prototype, "ShowBusyIndicator", {
        get: function () { return this.showBusyIndicator; },
        set: function (newValue) {
            if (this.showBusyIndicator != newValue) {
                this.showBusyIndicator = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    //public ShowBusyIndicator: boolean = false;
    SessionComponent.prototype.StartBusyIndicator = function (myText) {
        var _this = this;
        if (this.CurrentWindow != null && !this.CurrentWindow.SuppressBusyIndicator) {
            this.CurrentWindow.StartBusyIndicator(myText);
        }
        else if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.StartBusyIndicator(myText);
        }
        else {
            this.BusyIndicatorText = myText;
            this.ShowBusyIndicator = true;
        }
        if (this.BusyIndicatorTimer) {
            clearTimeout(this.BusyIndicatorTimer);
        }
        this.BusyIndicatorTimer = setTimeout(function () { return _this.CheckBusyIndicator(); }, 5000);
    };
    SessionComponent.prototype.CheckBusyIndicator = function () {
        if (this.SessionIndex != this.SessionIndex) {
            if (this.ShowBusyIndicator) {
                this.StopBusyIndicator();
            }
        }
    };
    SessionComponent.prototype.StartBusyIndicatorSaving = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
    };
    SessionComponent.prototype.StartBusyIndicatorLoading = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
    };
    SessionComponent.prototype.StartBusyIndicatorCreating = function () {
        this.StartBusyIndicator("Creating...");
    };
    SessionComponent.prototype.StopBusyIndicator = function () {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.StopBusyIndicator();
        }
        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.StopBusyIndicator();
        }
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    SessionComponent.prototype.AddMenuReference = function (element) {
        if (this.MenuReferences == null) {
            this.MenuReferences = new Array();
        }
        this.MenuReferences.push(element);
    };
    SessionComponent.prototype.DestroyMenuReferences = function () {
        if (this.MenuReferences == null) {
            this.MenuReferences = new Array();
        }
        this.MenuReferences.forEach(function (item) {
            item.destroy();
        });
        this.MenuReferences = [];
    };
    SessionComponent.prototype.GetNewWindowIndex = function () {
        if (this.SessionWindowIndex == null) {
            this.SessionWindowIndex = 0;
        }
        else {
            this.SessionWindowIndex += 1;
        }
        return this.SessionWindowIndex;
    };
    SessionComponent.prototype.AddWindow = function (element) {
        if (this.Windows == null) {
            this.Windows = new Array();
        }
        this.Windows.push(element);
        this.CurrentWindow = element;
    };
    SessionComponent.prototype.RemoveWindow = function (element) {
        var newCurrentWindow = null;
        if (this.Windows != null) {
            var index = this.Windows.indexOf(element);
            if (index > -1) {
                this.Windows.splice(index, 1);
            }
            var biggestIndex = -1;
            this.Windows.forEach(function (item) {
                if (item.WindowIndex > biggestIndex) {
                    biggestIndex = item.WindowIndex;
                }
            });
            if (biggestIndex > -1) {
                newCurrentWindow = this.Windows.filter(function (f) { return f.WindowIndex == biggestIndex; })[0];
            }
        }
        this.CurrentWindow = newCurrentWindow;
    };
    SessionComponent.prototype.CloseCurrentWindow = function () {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.Close(null);
        }
    };
    SessionComponent.prototype.CloseCurrentWindowEmit = function (emit) {
        if (this.CurrentWindow != null) {
            this.CurrentWindow.Close(emit);
        }
    };
    SessionComponent.prototype.DestroyWindows = function () {
        if (this.Windows == null) {
            this.Windows = new Array();
        }
        this.Windows.forEach(function (item) {
            item.DestroyWindow();
        });
        this.Windows = [];
    };
    SessionComponent.prototype.GetNewEditComponentIndex = function () {
        if (this.SessionEditComponentIndex == null) {
            this.SessionEditComponentIndex = 0;
        }
        else {
            this.SessionEditComponentIndex += 1;
        }
        return this.SessionEditComponentIndex;
    };
    SessionComponent.prototype.AddEditComponent = function (element) {
        if (this.EditControls == null) {
            this.EditControls = new Array();
        }
        this.EditControls.push(element);
        this.CurrentEditComponent = element;
    };
    SessionComponent.prototype.RemoveEditComponent = function (element) {
        var newCurrentEditComponent = null;
        if (this.EditControls != null) {
            var index = this.EditControls.indexOf(element);
            if (index > -1) {
                this.EditControls.splice(index, 1);
            }
            var biggestIndex = -1;
            this.EditControls.forEach(function (item) {
                if (item.ComponentIndex > biggestIndex) {
                    biggestIndex = item.ComponentIndex;
                }
            });
            if (biggestIndex > -1) {
                newCurrentEditComponent = this.EditControls.filter(function (f) { return f.ComponentIndex == biggestIndex; })[0];
            }
        }
        this.CurrentEditComponent = newCurrentEditComponent;
    };
    SessionComponent.prototype.RealCloseCurrentEditComponent = function () {
        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.Close();
        }
    };
    SessionComponent.prototype.CloseCurrentEditComponent = function () {
        if (this.CurrentEditComponent != null) {
            this.CurrentEditComponent.DestroyEditControl();
        }
    };
    SessionComponent.prototype.DestroyEditControls = function () {
        if (this.EditControls == null) {
            this.EditControls = new Array();
        }
        this.EditControls.forEach(function (item) {
            item.DestroyEditControl();
        });
        this.EditControls = [];
    };
    SessionComponent.prototype.GetNewListComponentIndex = function () {
        if (this.SessionListComponentIndex == null) {
            this.SessionListComponentIndex = 0;
        }
        else {
            this.SessionListComponentIndex += 1;
        }
        return this.SessionListComponentIndex;
    };
    SessionComponent.prototype.AddListComponent = function (element) {
        if (this.ListControls == null) {
            this.ListControls = new Array();
        }
        this.ListControls.push(element);
        this.CurrentListComponent = element;
    };
    SessionComponent.prototype.DestroyListComponentReferences = function () {
        this.UnsubscribeStaticEvent();
        this.ListControls = null;
        this.ListControls = new Array();
        this.CurrentListComponent = null;
        ;
    };
    SessionComponent.prototype.RemoveListComponent = function (element) {
        var newCurrentListComponent = null;
        if (this.ListControls != null) {
            var index = this.ListControls.indexOf(element);
            if (index > -1) {
                this.ListControls.splice(index, 1);
            }
            var biggestIndex = -1;
            this.ListControls.forEach(function (item) {
                if (item.ComponentIndex > biggestIndex) {
                    biggestIndex = item.ComponentIndex;
                }
            });
            if (biggestIndex > -1) {
                newCurrentListComponent = this.ListControls.filter(function (f) { return f.ComponentIndex == biggestIndex; })[0];
            }
        }
        this.CurrentListComponent = newCurrentListComponent;
    };
    SessionComponent.prototype.CloseCurrentListComponent = function () {
        if (this.CurrentListComponent != null) {
            this.CurrentListComponent.DestroyListControl();
        }
    };
    SessionComponent.prototype.DestroyListControls = function () {
        if (this.ListControls == null) {
            this.ListControls = new Array();
        }
        this.ListControls.forEach(function (item) {
            item.DestroyListControl();
        });
        this.ListControls = [];
    };
    SessionComponent.prototype.DestroySession = function () {
        this.isDestroingSession = true;
        this.DestroyWindows();
        this.DestroyEditControls();
        this.DestroyListControls();
        this.DestroyMenuReferences();
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
        }
        this.SessionLocation = null;
        this.SessionMenuLocation = null;
        if (this.BusyIndicatorTimer) {
            clearTimeout(this.BusyIndicatorTimer);
        }
        //window.removeEventListener("keydown", (evt) => this.keydownevt(evt, this.SessionIndex));
    };
    SessionComponent.prototype.FireEvent = function (eventArgs) {
        this.SessionEvent.emit(eventArgs);
    };
    SessionComponent.prototype.ChangeSessionHeader = function (args) {
        this.SessionTabItem.ChangeSessionHeader(args);
    };
    SessionComponent.prototype.StopChangeDetection = function () {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse && this.CurrentListComponent) {
            this.CurrentListComponent.DestroyMe = true;
            //setTimeout(() => { this.ChangeDetectorRef.detach(); }, 100);// cause malfunction !!!!!!!!!!!!!
        }
        else {
            this.ChangeDetectorRef.detach();
        }
    };
    SessionComponent.prototype.StartChangeDetection = function () {
        this.ChangeDetectorRef.reattach();
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse && this.CurrentListComponent) {
            this.CurrentListComponent.DestroyMe = false;
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], SessionComponent.prototype, "AllLocations", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "SessionEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "SessionInitialize", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "SessionSeleced", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "EndOfRowReachedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "PseventRowSelectEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "WindowResizeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "MouseUpEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "ObsNewElementInsertedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "QuantityTypeCodeLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "CollateralAnswerRefreshEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "ConnectedItemSelectedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "SelectItemEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "CloseNotificationBellEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "AccumulatedFilterChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "MouseDownEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "SearchFilterChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "DisableFieldsEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "CopyCellIntoMemory", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SessionComponent.prototype, "LostFocusEvent", void 0);
    SessionComponent = __decorate([
        core_1.Component({
            selector: 'SessionComponent',
            moduleId: module.id,
            templateUrl: "./SessionComponent.html",
            providers: [ApiFiltersChangeEvent_1.PubSubFiltersChangeEventService],
        }),
        __metadata("design:paramtypes", [ApiFiltersChangeEvent_1.PubSubFiltersChangeEventService, core_1.ChangeDetectorRef])
    ], SessionComponent);
    return SessionComponent;
}());
exports.SessionComponent = SessionComponent;
var SessionIdCounter = /** @class */ (function () {
    function SessionIdCounter(name) {
        this.Name = name;
        this.Counter = 0;
    }
    return SessionIdCounter;
}());
exports.SessionIdCounter = SessionIdCounter;
//# sourceMappingURL=SessionComponent.js.map