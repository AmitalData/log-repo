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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CustomerListService_1 = require("../../../Common/Services/StandardLists/CustomerListService");
var Tools_2 = require("../../Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ActivityShortTitleComponent = /** @class */ (function () {
    function ActivityShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }
    ActivityShortTitleComponent.prototype.BuildComponent = function () {
        this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(this.EntityPM.ActivityTypePathCode);
    };
    Object.defineProperty(ActivityShortTitleComponent.prototype, "CustomerDataVisibility", {
        //Properties
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "CancelledVisibility", {
        get: function () {
            var result = false;
            if (this.EntityPM != null) {
                if (this.EntityPM.ActivityStatusCode == "X") {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "ControlBackground", {
        get: function () {
            var result = null;
            if (this.EntityPM != null) {
                result = "red"; //new SolidColorBrush(Colors.Red) { Opacity = 0.1 };
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "ActivityTypePathCode", {
        get: function () {
            var result = "";
            if (this.EntityPM != null) {
                result = this.EntityPM.ActivityTypePathCode;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "ActivityTypeName", {
        get: function () {
            var result = "";
            if (this.EntityPM != null) {
                result = this.EntityPM.ActivityTypeName;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "Subject", {
        get: function () {
            var result = "";
            if (this.EntityPM != null) {
                result = this.EntityPM.Subject;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "CustomerName", {
        get: function () {
            var myResult = "";
            if (this.EntityPM != null) {
                myResult = this.EntityPM.CustomerName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityShortTitleComponent.prototype, "IsCustomerBlockedBusinessUnit", {
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.IsCustomerBlockedBusinessUnit;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ActivityShortTitleComponent.prototype.ViewCustomerMethod = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            if (this.EntityPM.IsCustomerBlockedBusinessUnit) {
                var service = new CustomerListService_1.CustomerListService();
                service.getSingleFromCache(this.EntityPM.CustomerId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var customerList = myResponse.Result;
                        if (customerList != null) {
                            _this.EditBlockedCustomer(customerList);
                        }
                    }
                });
            }
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: _this.EntityPM.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Activity" });
                });
            }
        }
    };
    ActivityShortTitleComponent.prototype.EditBlockedCustomer = function (customerList) {
        var windowTitle = "View Customer";
        var windowArgs = {};
        windowArgs.CustomerList = customerList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
    };
    ActivityShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "ActivityShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ActivityShortTitleComponent);
    return ActivityShortTitleComponent;
}());
exports.ActivityShortTitleComponent = ActivityShortTitleComponent;
//# sourceMappingURL=ActivityShortTitleComponent.js.map