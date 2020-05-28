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
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var TicketShortTitleComponent = /** @class */ (function () {
    function TicketShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsRankVisible = false;
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }
    TicketShortTitleComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildComponent();
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    TicketShortTitleComponent.prototype.BuildComponent = function () {
        this.RankName = this.EntityPM.RankName;
        if (this.RankName != null) {
            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "gold": {
                    this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "platinum": {
                    this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }
            this.IsRankVisible = true;
        }
    };
    Object.defineProperty(TicketShortTitleComponent.prototype, "TicketNumber", {
        get: function () {
            return this.EntityPM.TicketNumber;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketShortTitleComponent.prototype, "CompanyName", {
        get: function () {
            return this.EntityPM.CompanyName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketShortTitleComponent.prototype, "IsCancelled", {
        get: function () { return this.EntityPM.IsCancelled; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketShortTitleComponent.prototype, "IsClosed", {
        get: function () { return this.EntityPM.IsClosed; },
        enumerable: true,
        configurable: true
    });
    TicketShortTitleComponent.prototype.ViewCustomerMethod = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CompanyId)) {
            var objectTable = this.EntityPM.CompanyTableName;
            if (objectTable.toLowerCase() == "shipping line") {
                objectTable = "ShippingLine";
            }
            else if (objectTable.toLowerCase() == "shipping agent") {
                objectTable = "ShippingAgent";
            }
            else if (objectTable.toLowerCase() == "custom agent") {
                objectTable = "CustomAgent";
            }
            else if (objectTable.toLocaleLowerCase() == "potential customer") {
                objectTable = "Customer";
            }
            //this._entityResourceService.getEntityResourceByTableName(objectTable, 0).subscribe(response => {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.EntityPM.CompanyId, ObjectTableName: objectTable, BackButtonLabel: "Ticket" });
            });
            //});
        }
    };
    TicketShortTitleComponent.prototype.EditBlockedCustomer = function (customerList) {
        var windowTitle = "View Customer";
        var windowArgs = {};
        windowArgs.CustomerList = customerList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
    };
    TicketShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "TicketShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TicketShortTitleComponent);
    return TicketShortTitleComponent;
}());
exports.TicketShortTitleComponent = TicketShortTitleComponent;
//# sourceMappingURL=TicketShortTitleComponent.js.map