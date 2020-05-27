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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var CustomerPM_1 = require("../../../../Common/EntityPMs/CustomerPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var NewCustomerComponent = /** @class */ (function () {
    function NewCustomerComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.PartnerTypeId = "CS";
        this.ObjectTableName = "Customer";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.IsCustomerRadioEnabled = true;
        this.IsOkButtonEnabled = true;
        this.IsMessageVisible = false;
        this.EntityPM = new CustomerPM_1.CustomerPM();
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.EntityPM.PartnerTypeId = this.PartnerTypeId;
        this.EntityPM.CustomerStatusCode = "ACT";
        this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.IsCustomer = false;
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsSalesman) {
            this.EntityPM.SalesmanUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
        this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        this.RunComponent();
    }
    NewCustomerComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.args = args;
            if (args.Perspective == "ShippersAndConsignees") {
                this.IsCustomer = false;
            }
            else {
                this.IsCustomer = true;
            }
        }
    };
    NewCustomerComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewCustomerComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewCustomerComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (response2) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/NewPartnerTamplate", _this.viewContainerRef)
                    .then(function (cmpRef) {
                    _this.PartnerTamplate = cmpRef.instance;
                    _this.PartnerTamplate.EntityPM = _this.EntityPM;
                    _this.PartnerTamplate.CardTableName = _this.ObjectTableName;
                    _this.PartnerTamplate.PartnerTypeId = _this.PartnerTypeId;
                    _this.PartnerTamplate.DomainService = _this.DomainService;
                    _this.PartnerTamplate.IsCustomer = _this.IsCustomer;
                    if (_this.args) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.args.DefaultValues)) {
                            _this.PartnerTamplate.DefaultValues = _this.args.DefaultValues;
                        }
                    }
                    _this.PartnerTamplate.InitTemplate();
                    _this.SetEnabled();
                });
            });
        });
    };
    NewCustomerComponent.prototype.SetEnabled = function () {
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {
            if (this.IsCustomer) {
                //this.IsCustomerRadioEnabled = false;
                this.IsOkButtonEnabled = false;
                this.IsMessageVisible = true;
            }
            else {
                //this.IsCustomerRadioEnabled = true;
                this.IsOkButtonEnabled = true;
                this.IsMessageVisible = false;
            }
        }
    };
    NewCustomerComponent.prototype.SetIsCustomer = function (isCustomer) {
        this.IsCustomer = isCustomer;
    };
    Object.defineProperty(NewCustomerComponent.prototype, "IsCustomer", {
        get: function () { return this.EntityPM.IsCustomer; },
        set: function (newValue) {
            if (this.EntityPM.IsCustomer != newValue) {
                this.EntityPM.IsCustomer = newValue;
                this.SetEnabled();
                if (this.PartnerTamplate) {
                    this.PartnerTamplate.IsCustomer = this.IsCustomer;
                    this.PartnerTamplate.SetUIProperties();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewCustomerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewCustomerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = this.PartnerTamplate.Validate();
        if (errors.length == 0) {
            this.EntityPM.Code = this.PartnerTamplate.CardCode;
            this.EntityPM.EnglishName = this.PartnerTamplate.Name;
            this.EntityPM.LocalName = !Tools_1.AppTool.IsNullOrEmpty(this.PartnerTamplate.LocalName) ? this.PartnerTamplate.LocalName : this.PartnerTamplate.Name;
            this.EntityPM.VatNumber = this.PartnerTamplate.VatNumber;
            this.EntityPM.SalesmanUserId = this.PartnerTamplate.SalesmanUserId;
            this.EntityPM.ExistedContactId = this.PartnerTamplate.ExistedContactId;
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        }
        this.ValidationErrorsList = errors;
        if (this.IsCustomer) {
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {
                errors.push("You are not allowed to add a new active customer");
            }
        }
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            //this.EntityPM.UIProperties = null;
            var args = new PartnersDomainService_1.PartnerServicePM();
            args.Tenant = this.EntityPM.Tenant;
            args.PartnerTypeId = this.PartnerTypeId;
            args.Customer = this.EntityPM;
            args.Address = this.PartnerTamplate.Address;
            if (this.PartnerTamplate.IsAddContactChecked) {
                this.PartnerTamplate.Contact.SetAsPrimaryForCard = true;
                args.Contact = this.PartnerTamplate.Contact;
            }
            this.DomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result.Customer;
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewCustomerComponent.prototype, "viewContainerRef", void 0);
    NewCustomerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewCustomerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewCustomerComponent);
    return NewCustomerComponent;
}());
exports.NewCustomerComponent = NewCustomerComponent;
//# sourceMappingURL=NewCustomerComponent.js.map