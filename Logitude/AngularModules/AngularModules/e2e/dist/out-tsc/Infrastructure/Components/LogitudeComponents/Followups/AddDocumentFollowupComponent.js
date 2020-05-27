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
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var QuoteFollowUpPM_1 = require("../../../../Quote/EntityPMs/QuoteFollowUpPM");
var FollowUpPM_1 = require("../../../../Infrastructure/EntityPMs/FollowUpPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddDocumentFollowupComponent = /** @class */ (function (_super) {
    __extends(AddDocumentFollowupComponent, _super);
    function AddDocumentFollowupComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentFollowUp = new FollowUpPM_1.FollowUpPM();
        _this.IsStardLoadPage = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddDocumentFollowupComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.CurrentFollowUp = args.CurrentFollowUp;
        this.IsStardLoadPage = true;
    };
    AddDocumentFollowupComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddDocumentFollowupComponent.prototype.NumericButtonClicked = function (isIncreas) {
        if (this.CurrentFollowUp.Date == null) {
            this.CurrentFollowUp.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            var day = isIncreas ? 1 : -1;
            var newDate = Tools_1.DateTool.GetDateParts(this.CurrentFollowUp.Date).DateObject;
            newDate.setUTCMilliseconds(0);
            newDate.setUTCSeconds(0);
            newDate.setUTCMinutes(0);
            newDate.setUTCHours(0);
            newDate.setUTCDate(newDate.getDate() + day);
            newDate.setUTCMonth(newDate.getMonth());
            newDate.setUTCFullYear(newDate.getFullYear());
            this.CurrentFollowUp.Date = newDate;
        }
    };
    AddDocumentFollowupComponent.prototype.SaveButtonClicked = function () {
        if (this.EntityPM) {
            this.ValidationErrorsList = [];
            Validator_1.Validator.TryValidateObject(this.CurrentFollowUp, "FollowUp", this.ValidationErrorsList);
            if (this.ValidationErrorsList.length == 0) {
                if (this.ObjectTableName == "Quote") {
                    var myQuoteFollowUpPM = new QuoteFollowUpPM_1.QuoteFollowUpPM(null);
                    myQuoteFollowUpPM.Tenant = this.EntityPM.Tenant;
                    myQuoteFollowUpPM.QuoteId = this.EntityPM.Id;
                    myQuoteFollowUpPM.IsNew = true;
                    myQuoteFollowUpPM.EventTypeId = this.CurrentFollowUp.EventTypeId;
                    myQuoteFollowUpPM.EventTypeFollowUpName = this.CurrentFollowUp.EventTypeFollowUpName;
                    myQuoteFollowUpPM.ManualActivatedFollowUp = this.CurrentFollowUp.ManualActivatedFollowUp;
                    myQuoteFollowUpPM.Date = this.CurrentFollowUp.Date;
                    myQuoteFollowUpPM.OwnerUserId = this.CurrentFollowUp.OwnerUserId;
                    myQuoteFollowUpPM.Note = this.CurrentFollowUp.Notes;
                    myQuoteFollowUpPM.Done = this.CurrentFollowUp.Done;
                    myQuoteFollowUpPM.DocumentTypeId = this.CurrentFollowUp.DocumentTypeId;
                    myQuoteFollowUpPM.Area = this.CurrentFollowUp.Area;
                    myQuoteFollowUpPM.ExternalDocumentId = this.CurrentFollowUp.ExternalDocumentId;
                    this.EntityPM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
                }
                else if (this.ObjectTableName == "Shipment") {
                    var myShipmentFollowUpPM = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
                    myShipmentFollowUpPM.Tenant = this.EntityPM.Tenant;
                    myShipmentFollowUpPM.ShipmentId = this.EntityPM.Id;
                    myShipmentFollowUpPM.IsNew = true;
                    myShipmentFollowUpPM.EventTypeId = this.CurrentFollowUp.EventTypeId;
                    myShipmentFollowUpPM.EventTypeFollowUpName = this.CurrentFollowUp.EventTypeFollowUpName;
                    myShipmentFollowUpPM.ManualActivatedFollowUp = this.CurrentFollowUp.ManualActivatedFollowUp;
                    myShipmentFollowUpPM.Date = this.CurrentFollowUp.Date;
                    myShipmentFollowUpPM.OwnerUserId = this.CurrentFollowUp.OwnerUserId;
                    myShipmentFollowUpPM.Note = this.CurrentFollowUp.Notes;
                    myShipmentFollowUpPM.Done = this.CurrentFollowUp.Done;
                    myShipmentFollowUpPM.DocumentTypeId = this.CurrentFollowUp.DocumentTypeId;
                    myShipmentFollowUpPM.Area = this.CurrentFollowUp.Area;
                    myShipmentFollowUpPM.ExternalDocumentId = this.CurrentFollowUp.ExternalDocumentId;
                    this.EntityPM.AddShipmentFollowUp(myShipmentFollowUpPM);
                }
                this.CurrentSession.FireEvent("FollowupsChanged");
                this.CurrentSession.CurrentWindow.Close("AddFollowUpSucceeded");
            }
        }
    };
    AddDocumentFollowupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentFollowUp',
            templateUrl: './AddDocumentFollowupComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddDocumentFollowupComponent);
    return AddDocumentFollowupComponent;
}(BaseComponent_1.BaseComponent));
exports.AddDocumentFollowupComponent = AddDocumentFollowupComponent;
//# sourceMappingURL=AddDocumentFollowupComponent.js.map