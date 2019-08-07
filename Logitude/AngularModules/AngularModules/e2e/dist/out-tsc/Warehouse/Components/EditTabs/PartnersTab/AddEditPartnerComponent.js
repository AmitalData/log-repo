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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AddEditPartnerComponent = /** @class */ (function (_super) {
    __extends(AddEditPartnerComponent, _super);
    function AddEditPartnerComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "WarehouseEntry";
        _this.isMyCustomer = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = true;
        _this.IsEditPartnerEnabled = false;
        return _this;
    }
    AddEditPartnerComponent.prototype.ngOnInit = function () {
        this.DataContext.SetUIProperties();
    };
    AddEditPartnerComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        if (this.IsEditingEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DataContext.PartnerId)) {
                this.IsEditPartnerEnabled = true;
            }
        }
    };
    AddEditPartnerComponent.prototype.CancelButtonClicked = function () {
        this.DataContext.ResetOriginData();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPartnerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Name")));
        }
        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Reference1") + " max length is 50");
            }
        }
        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("WarehouseEntry.S.Partners.Reference2") + " max length is 50");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.isMyCustomer) {
                if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                    this.EntityPM.CustomerId = this.DataContext.PartnerId;
                }
            }
            if (this.DataContext.IsNewAdded) {
                if (this.DataContext.fatherComponent.ItemsCollection.indexOf(this.DataContext) == -1) {
                    var BreakException = {};
                    try {
                        this.DataContext.fatherComponent.ItemsCollection.sort(function (a, b) { return a.PartnerIndex - b.PartnerIndex; }).forEach(function (item) {
                            if (_this.DataContext.PartnerIndex < item.PartnerIndex) {
                                _this.DataContext.fatherComponent.ItemsCollection.splice(_this.DataContext.fatherComponent.ItemsCollection.indexOf(item), 0, _this.DataContext);
                                throw BreakException;
                            }
                        });
                    }
                    catch (e) {
                        if (e !== BreakException)
                            throw e;
                    }
                    finally {
                        if (this.DataContext.fatherComponent.ItemsCollection.indexOf(this.DataContext) == -1) {
                            this.DataContext.fatherComponent.ItemsCollection.push(this.DataContext);
                        }
                    }
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
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
}(BaseComponent_1.BaseComponent));
exports.AddEditPartnerComponent = AddEditPartnerComponent;
//# sourceMappingURL=AddEditPartnerComponent.js.map