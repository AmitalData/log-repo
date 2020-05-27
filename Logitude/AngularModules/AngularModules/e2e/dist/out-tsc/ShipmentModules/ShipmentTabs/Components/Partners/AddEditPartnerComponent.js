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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditPartnerComponent = /** @class */ (function () {
    function AddEditPartnerComponent() {
        this.ObjectTableName = "Shipment";
        this.isMyCustomer = false;
        this.oldCustomerPartnerId = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditPartnerComponent.prototype.ngOnInit = function () {
        this.DataContext.SetUIProperties();
    };
    AddEditPartnerComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        this.oldCustomerPartnerId = this.EntityPM.CustomerId;
        this.Clone();
    };
    AddEditPartnerComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPartnerComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Name")));
        }
        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Reference1") + " max length is 50");
            }
        }
        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Reference2") + " max length is 50");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.isMyCustomer) {
                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.CustomerId != null) {
                        this.EntityPM.CustomerId = null;
                    }
                    if (this.EntityPM.CustomerName != null) {
                        this.EntityPM.CustomerName = null;
                    }
                    if (this.EntityPM.CustomerReference1 != null) {
                        this.EntityPM.CustomerReference1 = null;
                    }
                    if (this.EntityPM.CustomerReference2 != null) {
                        this.EntityPM.CustomerReference2 = null;
                    }
                    if (this.EntityPM.CustomerAddressId != null) {
                        this.EntityPM.CustomerAddressId = null;
                    }
                    if (this.EntityPM.CustomerContactId != null) {
                        this.EntityPM.CustomerContactId = null;
                    }
                    if (this.EntityPM.ShipmentCustomerTypeCode != null) {
                        this.EntityPM.ShipmentCustomerTypeCode = null;
                    }
                }
                else {
                    if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                        this.EntityPM.CustomerId = this.DataContext.PartnerId;
                    }
                    if (this.EntityPM.CustomerName != this.DataContext.Name) {
                        this.EntityPM.CustomerName = this.DataContext.Name;
                    }
                    if (this.EntityPM.CustomerAddressId != this.DataContext.AddressId) {
                        this.EntityPM.CustomerAddressId = this.DataContext.AddressId;
                    }
                    if (this.EntityPM.CustomerContactId != this.DataContext.ContactId) {
                        this.EntityPM.CustomerContactId = this.DataContext.ContactId;
                    }
                    if (this.EntityPM.CustomerReference1 != this.DataContext.Reference1) {
                        this.EntityPM.CustomerReference1 = this.DataContext.Reference1;
                    }
                    if (this.EntityPM.CustomerReference2 != this.DataContext.Reference2) {
                        this.EntityPM.CustomerReference2 = this.DataContext.Reference2;
                    }
                }
                if (this.oldCustomerPartnerId != this.EntityPM.CustomerId) {
                    this.DataContext.fatherComponent.OnCustomerChanged();
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
            this.CurrentSession.FireEvent("ShipmentPartnersChanged");
        }
    };
    AddEditPartnerComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PartnerId');
        this.myCloner.AddField('AddressId');
        this.myCloner.AddField('ContactId');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Note');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditPartnerComponent.prototype.RejectChanges = function () {
        this.DataContext.IsReseting = true;
        this.myCloner.RejectChanges();
        this.DataContext.IsReseting = false;
    };
    AddEditPartnerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddEditPartnerComponent',
            templateUrl: './AddEditPartnerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPartnerComponent);
    return AddEditPartnerComponent;
}());
exports.AddEditPartnerComponent = AddEditPartnerComponent;
//# sourceMappingURL=AddEditPartnerComponent.js.map