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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddEditShipmentAssemblyComponent = /** @class */ (function (_super) {
    __extends(AddEditShipmentAssemblyComponent, _super);
    function AddEditShipmentAssemblyComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myCardListService = new CardListService_1.CardListService();
        return _this;
    }
    AddEditShipmentAssemblyComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.ShipmentPM = windowArgs['ShipmentPM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.ObjectTableName = "ShipmentAssembly";
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNew = true;
        }
        else {
            this.IsNew = false;
        }
        this.SetLOVDependency();
        this.SetUIProperties();
        this.Clone();
    };
    AddEditShipmentAssemblyComponent.prototype.SetLOVDependency = function () {
        var myDependency = "CS";
        var myDependencyIsList = false;
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            myDependency = "CS,AG";
            myDependencyIsList = true;
        }
        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    };
    AddEditShipmentAssemblyComponent.prototype.SetUIProperties = function () {
    };
    Object.defineProperty(AddEditShipmentAssemblyComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (value) {
            if (this.EntityPM.ShipperId != value) {
                this.EntityPM.ShipperId = value;
                this.GetShipperCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditShipmentAssemblyComponent.prototype, "House", {
        get: function () { return this.EntityPM.House; },
        set: function (value) {
            if (this.EntityPM.House != value) {
                this.EntityPM.House = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditShipmentAssemblyComponent.prototype.GetShipperCard = function () {
        var _this = this;
        if (this.ShipperId == null) {
            this.EntityPM.ShipperName = null;
        }
        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.ShipperName = myCard.EnglishName;
                    }
                    else {
                        _this.LoadShipperCard();
                    }
                }
            });
        }
    };
    AddEditShipmentAssemblyComponent.prototype.LoadShipperCard = function () {
        var _this = this;
        this.myCardListService.getSingle(this.ShipperId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myCard = myResponse.Result;
                if (myCard != null) {
                    _this.EntityPM.ShipperName = myCard.EnglishName;
                }
            }
        });
    };
    AddEditShipmentAssemblyComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditShipmentAssemblyComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && Tools_1.AppTool.IsNullOrEmpty(this.House)) {
            errors.push("Please fill Shipper or House");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                if (this.ShipmentPM.ShipmentAssemblies.indexOf(this.EntityPM) == -1) {
                    this.ShipmentPM.AddAssembly(this.EntityPM);
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    AddEditShipmentAssemblyComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('House');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.ShipmentPM);
    };
    AddEditShipmentAssemblyComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditShipmentAssemblyComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditShipmentAssemblyComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditShipmentAssemblyComponent);
    return AddEditShipmentAssemblyComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditShipmentAssemblyComponent = AddEditShipmentAssemblyComponent;
//# sourceMappingURL=AddEditShipmentAssemblyComponent.js.map