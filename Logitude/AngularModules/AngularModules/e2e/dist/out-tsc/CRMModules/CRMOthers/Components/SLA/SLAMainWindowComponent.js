"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SLAHeaderPM_1 = require("../../../../CRM/EntityPMs/SLAHeaderPM");
var SLAHeaderListService_1 = require("../../../../CRM/Services/StandardLists/SLAHeaderListService");
var CRMDomainService_1 = require("../../../../CRM/Services/CRMDomainService");
var SLAMainWindowComponent = /** @class */ (function (_super) {
    __extends(SLAMainWindowComponent, _super);
    function SLAMainWindowComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.SLAList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.includeInactive = false;
        _this.SLAHeaderListService = new SLAHeaderListService_1.SLAHeaderListService();
        _this.LoadSLAList(false);
        return _this;
    }
    SLAMainWindowComponent.prototype.LoadSLAList = function (arg) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.SLAList = [];
        var service = new CRMDomainService_1.CRMDomainService();
        service.GetActiveSLAbyTenant().subscribe(function (myResponse) {
            var pmResponse = myResponse;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (arg == false) {
                    myResult = pmResponse.Result.filter(function (d) { return d.Inactive == false; });
                }
                var order = 1;
                myResult.forEach(function (item) {
                    var slsItem = new SLAItem(item, order);
                    order = order + 1;
                    _this.SLAList.push(slsItem);
                });
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    Object.defineProperty(SLAMainWindowComponent.prototype, "IncludeInactive", {
        get: function () {
            return this.includeInactive;
        },
        set: function (value) {
            if (this.includeInactive != value) {
                this.includeInactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SLAMainWindowComponent.prototype.AddSLA = function () {
        var item = new SLAItem(null, null);
        item.EntityPM = new SLAHeaderPM_1.SLAHeaderPM();
        this.RunSLAWindow(item, "New SLA", true);
    };
    SLAMainWindowComponent.prototype.EditSLA = function (item) {
        this.RunSLAWindow(item, "Edit SLA", false);
    };
    SLAMainWindowComponent.prototype.RunSLAWindow = function (itemComponent, windowTitle, isNew) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 815;
        var args = {};
        args.IsNewEntity = isNew;
        args.EntityPM = itemComponent.EntityPM;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Show('./CRMModules/CRMOthers/Components/SLA/NewSLAComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "ok") {
                _this.LoadSLAList(_this.IncludeInactive);
            }
        });
    };
    SLAMainWindowComponent.prototype.IncludeInactiveChecked = function (arg) {
        this.LoadSLAList(arg);
    };
    SLAMainWindowComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SLAMainWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SLAMainWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SLAMainWindowComponent);
    return SLAMainWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.SLAMainWindowComponent = SLAMainWindowComponent;
var SLAItem = /** @class */ (function () {
    function SLAItem(entityPM, order) {
        this.DataContext = this;
        this.EntityPM = entityPM;
        this.Order = order;
    }
    Object.defineProperty(SLAItem.prototype, "CreateDate", {
        get: function () {
            return this.EntityPM.CreateDate;
        },
        set: function (newValue) {
            if (this.EntityPM.CreateDate != newValue) {
                this.EntityPM.CreateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "UpdateDate", {
        get: function () {
            return this.EntityPM.UpdateDate;
        },
        set: function (newValue) {
            if (this.EntityPM.UpdateDate != newValue) {
                this.EntityPM.UpdateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "UpdatedByUserName", {
        get: function () {
            return this.EntityPM.UpdatedByUserName;
        },
        set: function (newValue) {
            if (this.EntityPM.UpdatedByUserName != newValue) {
                this.EntityPM.UpdatedByUserName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "CreatedByUserName", {
        get: function () {
            return this.EntityPM.CreatedByUserName;
        },
        set: function (newValue) {
            if (this.EntityPM.CreatedByUserName != newValue) {
                this.EntityPM.CreatedByUserName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "Description", {
        get: function () {
            return this.EntityPM.Description;
        },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "Name", {
        get: function () {
            return this.EntityPM.Name;
        },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLAItem.prototype, "Inactive", {
        get: function () {
            return this.EntityPM.Inactive;
        },
        set: function (newValue) {
            if (this.EntityPM.Inactive != newValue) {
                this.EntityPM.Inactive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return SLAItem;
}());
exports.SLAItem = SLAItem;
//# sourceMappingURL=SLAMainWindowComponent.js.map