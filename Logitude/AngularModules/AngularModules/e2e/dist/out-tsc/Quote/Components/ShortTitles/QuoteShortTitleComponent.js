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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var QuoteShortTitleComponent = /** @class */ (function () {
    function QuoteShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsRankVisible = false;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }
    Object.defineProperty(QuoteShortTitleComponent.prototype, "IsCancelled", {
        get: function () { return this.EntityPM.IsCancelled; },
        enumerable: true,
        configurable: true
    });
    QuoteShortTitleComponent.prototype.BuildComponent = function () {
        this.RankName = this.EntityPM.CustomerRankName;
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
        this.DirectionImageSRC = "./Images/Directions/" + this.EntityPM.DirectionId + ".png";
        this.TransportModeImageSRC = "./Images/Icons/" + this.EntityPM.TransportModeId + ".png";
    };
    QuoteShortTitleComponent.prototype.ViewCustomer = function () {
        var _this = this;
        if (this.EntityPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                var myService = new CardListService_1.CardListService();
                myService.getSingle(this.EntityPM.CustomerId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null) {
                            var objectTableName = null;
                            switch (list.PartnerTypeId) {
                                case "CS":
                                case "PO":
                                    {
                                        objectTableName = "Customer";
                                        break;
                                    }
                                case "AG": {
                                    objectTableName = "Agent";
                                    break;
                                }
                                case "AL": {
                                    objectTableName = "Airline";
                                    break;
                                }
                                case "TR": {
                                    objectTableName = "Trucker";
                                    break;
                                }
                                case "CG": {
                                    objectTableName = "CustomAgent";
                                    break;
                                }
                                case "SG": {
                                    objectTableName = "ShippingAgent";
                                    break;
                                }
                                case "SL": {
                                    objectTableName = "ShippingLine";
                                    break;
                                }
                                case "VD": {
                                    objectTableName = "Vendor";
                                    break;
                                }
                                case "WH": {
                                    objectTableName = "Warehouse";
                                    break;
                                }
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(objectTableName)) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run({ EntityId: _this.EntityPM.CustomerId, ObjectTableName: objectTableName, BackButtonLabel: 'Quote' });
                                    var isEditComponentSaved = false;
                                    cmpRef.instance.BackCompleted.subscribe(function (bk) {
                                        if (isEditComponentSaved) {
                                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        }
                                    });
                                    cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                                        if (isSaveSuccess) {
                                            isEditComponentSaved = true;
                                        }
                                    });
                                });
                            }
                        }
                    }
                });
            }
        }
    };
    QuoteShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./QuoteShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], QuoteShortTitleComponent);
    return QuoteShortTitleComponent;
}());
exports.QuoteShortTitleComponent = QuoteShortTitleComponent;
//# sourceMappingURL=QuoteShortTitleComponent.js.map