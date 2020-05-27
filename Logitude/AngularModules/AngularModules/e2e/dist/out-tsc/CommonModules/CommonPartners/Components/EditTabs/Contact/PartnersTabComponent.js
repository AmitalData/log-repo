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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var CardPMService_1 = require("../../../../../Common/Services/StandardPMs/CardPMService");
var PartnersDomainService_1 = require("../../../../../Common/Services/PartnersDomainService");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var PartnersTabComponent = /** @class */ (function () {
    function PartnersTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "Contact";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsNoDataVisible = false;
        this.EntityPM = entityArgs.EntityPM;
        this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        this.ItemsSource = [];
        this.LoadData();
    }
    PartnersTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.IsNoDataVisible = false;
        this.DomainService.GetCardsForContact(this.EntityPM.Id).subscribe(function (myResult) {
            _this.ItemsSource = myResult;
            _this.IsNoDataVisible = _this.ItemsSource.length == 0 ? true : false;
        });
    };
    PartnersTabComponent.prototype.EditEntityClicked = function (item) {
        var _this = this;
        if (item != null) {
            var itemTableName = item.PartnerTypeId == "PO" ? "Customer" : item.PartnerTypeName;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: itemTableName });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.LoadData();
                });
            });
        }
    };
    PartnersTabComponent.prototype.DisconnectClicked = function (item) {
        var _this = this;
        if (item != null) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Title = "Disconnect";
            confirmWindow.Show("Are you sure you want to disconnect?");
            confirmWindow.WindowClosed.subscribe(function (s) {
                if (confirmWindow.Yes) {
                    item.DisconectFromContact = true;
                    item.ContactId = _this.EntityPM.Id;
                    var indexOfItem = _this.ItemsSource.indexOf(item);
                    if (indexOfItem > -1) {
                        _this.ItemsSource.splice(indexOfItem, 1);
                    }
                    if (_this.CardService == null) {
                        _this.CardService = new CardPMService_1.CardPMService();
                    }
                    _this.CardService.update(item).subscribe(function (myResult) {
                    });
                }
            });
        }
    };
    PartnersTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PartnersTabComponent);
    return PartnersTabComponent;
}());
exports.PartnersTabComponent = PartnersTabComponent;
//# sourceMappingURL=PartnersTabComponent.js.map