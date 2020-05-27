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
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var OpportunityShortTitleComponent = /** @class */ (function () {
    function OpportunityShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityPM = this.entityArgs.EntityPM;
    }
    OpportunityShortTitleComponent.prototype.ViewCustomerMethod = function () {
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
                    cmpRef.instance.Run({ EntityId: _this.EntityPM.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Opportunity" });
                });
            }
        }
    };
    Object.defineProperty(OpportunityShortTitleComponent.prototype, "EnglishName", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.CustomerName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityShortTitleComponent.prototype, "RankName", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.CustomerRankName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityShortTitleComponent.prototype, "RankSource1", {
        get: function () {
            var myResult = null;
            // silver to lower
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.CustomerRankCode;
                switch (RankCode) {
                    case "1":
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityShortTitleComponent.prototype, "RankSource2", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.CustomerRankCode;
                switch (RankCode) {
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityShortTitleComponent.prototype, "RankSource3", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.CustomerRankCode;
                switch (RankCode) {
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    OpportunityShortTitleComponent.prototype.EditBlockedCustomer = function (customerList) {
        var windowTitle = "View Customer";
        var windowArgs = {};
        windowArgs.CustomerList = customerList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
    };
    OpportunityShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "OpportunityShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OpportunityShortTitleComponent);
    return OpportunityShortTitleComponent;
}());
exports.OpportunityShortTitleComponent = OpportunityShortTitleComponent;
//# sourceMappingURL=OpportunityShortTitleComponent.js.map