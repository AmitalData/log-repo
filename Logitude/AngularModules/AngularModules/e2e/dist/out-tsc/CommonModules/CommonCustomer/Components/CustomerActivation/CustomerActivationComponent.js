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
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var CustomerPMService_1 = require("../../../../Common/Services/StandardPMs/CustomerPMService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var Args_1 = require("../../../../Common/Args");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Tools_1 = require("../../../../Infrastructure/Tools");
var VatNumberValidator_1 = require("../../../../Infrastructure/Validators/VatNumberValidator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var AddressValidator_1 = require("../../../../Infrastructure/Validators/AddressValidator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var CustomerActivationComponent = /** @class */ (function (_super) {
    __extends(CustomerActivationComponent, _super);
    function CustomerActivationComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.country = null;
        _this.partnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.customerService = new CustomerPMService_1.CustomerPMService;
        return _this;
    }
    CustomerActivationComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        if (args.ActivatedFromQuoteSide) {
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "ACTIVATE")) {
                this.Message = "You have no permession to activate this customer";
                this.ActivateFeatureON = false;
            }
            else {
                if (args.ActivatedPartnerType == "SH") {
                    this.Message = "The Shipper is a potential one and has to be activated in order to build a shipment";
                }
                else {
                    this.Message = "The Consignee is a potential one and has to be activated in order to build a shipment";
                }
                this.ActivateFeatureON = true;
            }
        }
        else {
            this.ActivateFeatureON = true;
        }
        this.SetUIProperties();
        this.LoadPickupDeliveryAddress();
        this.CheckIfVatUnique();
        this.RunAdditionalFieldsComponent();
        this.Clone();
    };
    CustomerActivationComponent.prototype.SetUIProperties = function () {
        if (!this.ActivateFeatureON) {
            this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Address1_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Address2_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ZipCode_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("City_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CountryId_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PhoneNumber_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FaxNumber_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomerSizeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ATTN_Potential", this.ObjectTableName, false);
            this.SetUIProperties_GeneratedComponent();
        }
        else {
            this.SetUIProperties_Others();
            this.SetUIProperties_VAT();
            this.SetUIProperties_State();
            this.SetUIProperties_TelFax();
            this.SetUIProperties_GeneratedComponent();
        }
    };
    CustomerActivationComponent.prototype.SetUIProperties_Others = function () {
        this.UIProperties.SetRequired("City_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.City_Potential));
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerAddress1Required) {
                this.UIProperties.SetRequired("Address1_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Address1_Potential));
            }
        }
    };
    CustomerActivationComponent.prototype.SetUIProperties_VAT = function () {
        if (this.EntityPM.IsCustomer) {
            var args = new VatNumberValidator_1.VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.EntityPM.IsCustomer;
            args.PartnerTypeId = this.EntityPM.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.CountryName;
            args.CountryEnglishName = this.CountryEnglishName;
            args.SetReady = false;
            VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
        }
    };
    CustomerActivationComponent.prototype.SetUIProperties_State = function () {
        var _this = this;
        var stateIsEnabled = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            stateIsEnabled = false;
        }
        else {
            var countryListService = new CountryListService_1.CountryListService();
            countryListService.getSingleFromCache(this.CountryId_Potential).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        if (!list.HasStates) {
                            stateIsEnabled = false;
                        }
                    }
                    _this.UIProperties.SetEnabled("StateId_Potential", _this.ObjectTableName, stateIsEnabled);
                }
            });
        }
    };
    CustomerActivationComponent.prototype.SetUIProperties_TelFax = function () {
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                this.UIProperties.SetRequired("PhoneNumber_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber_Potential));
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                this.UIProperties.SetRequired("FaxNumber_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber_Potential));
            }
        }
    };
    CustomerActivationComponent.prototype.LoadPickupDeliveryAddress = function () {
        var _this = this;
        this.partnersDomainService.GetAddressByCardAndType(this.EntityPM.Id, "P").subscribe(function (myResponse) {
            _this.PickupAddress = myResponse;
        });
    };
    CustomerActivationComponent.prototype.CheckIfVatUnique = function () {
        var _this = this;
        if (this.EntityPM.IsCustomer) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.VatNumber)) {
                this.partnersDomainService.GetIsVATUniqueForCustomer(this.VatNumber, this.EntityPM.Id, this.CountryId_Potential).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.vatTypeNotUnique = myResponse.Result;
                    }
                });
            }
        }
    };
    CustomerActivationComponent.prototype.RunAdditionalFieldsComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    CustomerActivationComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunAdditionalFieldsComponent(); }, 1);
        }
    };
    CustomerActivationComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            //cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Customer.AdditionalFields");
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            var screenCode = "Customer.AdditionalFields";
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    CustomerActivationComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.ActivateFeatureON);
        }
    };
    Object.defineProperty(CustomerActivationComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "Address1_Potential", {
        get: function () { return this.EntityPM.Address1_Potential; },
        set: function (newValue) {
            if (this.EntityPM.Address1_Potential != newValue) {
                this.EntityPM.Address1_Potential = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "Address2_Potential", {
        get: function () { return this.EntityPM.Address2_Potential; },
        set: function (newValue) {
            if (this.EntityPM.Address2_Potential != newValue) {
                this.EntityPM.Address2_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "ZipCode_Potential", {
        get: function () { return this.EntityPM.ZipCode_Potential; },
        set: function (newValue) {
            if (this.EntityPM.ZipCode_Potential != newValue) {
                this.EntityPM.ZipCode_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "City_Potential", {
        get: function () { return this.EntityPM.City_Potential; },
        set: function (newValue) {
            if (this.EntityPM.City_Potential != newValue) {
                this.EntityPM.City_Potential = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "CountryId_Potential", {
        get: function () { return this.EntityPM.CountryId_Potential; },
        set: function (newValue) {
            if (this.EntityPM.CountryId_Potential != newValue) {
                this.EntityPM.CountryId_Potential = newValue;
                this.StateId_Potential = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "Country", {
        get: function () { return this.country; },
        set: function (newValue) {
            if (this.country != newValue) {
                this.country = newValue;
                this.OnCountryChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "StateId_Potential", {
        get: function () { return this.EntityPM.StateId_Potential; },
        set: function (newValue) {
            if (this.EntityPM.StateId_Potential != newValue) {
                this.EntityPM.StateId_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM.VatNumber != newValue) {
                this.EntityPM.VatNumber = newValue;
                this.SetUIProperties_VAT();
                this.CheckIfVatUnique();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "PhoneNumber_Potential", {
        get: function () { return this.EntityPM.PhoneNumber_Potential; },
        set: function (newValue) {
            if (this.EntityPM.PhoneNumber_Potential != newValue) {
                this.EntityPM.PhoneNumber_Potential = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "FaxNumber_Potential", {
        get: function () { return this.EntityPM.FaxNumber_Potential; },
        set: function (newValue) {
            if (this.EntityPM.FaxNumber_Potential != newValue) {
                this.EntityPM.FaxNumber_Potential = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "ATTN_Potential", {
        get: function () { return this.EntityPM.ATTN_Potential; },
        set: function (newValue) {
            if (this.EntityPM.ATTN_Potential != newValue) {
                this.EntityPM.ATTN_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerActivationComponent.prototype, "CustomerSizeId", {
        get: function () { return this.EntityPM.CustomerSizeId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerSizeId != newValue) {
                this.EntityPM.CustomerSizeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerActivationComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryName = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryName = this.EntityPM.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_State();
        this.SetUIProperties_VAT();
    };
    CustomerActivationComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId_Potential);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.EntityPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City_Potential = mySelectedCity;
                _this.CountryId_Potential = args.CountryId;
                _this.StateId_Potential = args.StateId;
            }
        });
    };
    CustomerActivationComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
        var test = this.CurrentSession.CurrentEditComponent;
    };
    CustomerActivationComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('EnglishName');
        this.myCloner.AddField('LocalName');
        this.myCloner.AddField('Address1_Potential');
        this.myCloner.AddField('Address2_Potential');
        this.myCloner.AddField('ZipCode_Potential');
        this.myCloner.AddField('City_Potential');
        this.myCloner.AddField('CountryId_Potential');
        this.myCloner.AddField('StateId_Potential');
        this.myCloner.AddField('VatNumber');
        this.myCloner.AddField('PhoneNumber_Potential');
        this.myCloner.AddField('FaxNumber_Potential');
        this.myCloner.AddField('ATTN_Potential');
        this.myCloner.AddField('CustomerSizeId');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomerActivationComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CustomerActivationComponent.prototype.OkButtonClicked = function () {
        var screenErrors = [];
        var allErrors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, screenErrors);
        if (this.EntityPM.IsCustomer) {
            var args = new VatNumberValidator_1.VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.EntityPM.IsCustomer;
            args.PartnerTypeId = this.EntityPM.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.CountryName;
            args.CountryEnglishName = this.CountryEnglishName;
            args.SetReady = false;
            VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
            VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
            if (args.Errors.length > 0) {
                screenErrors = args.Errors;
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    screenErrors.push("Phone Number is required");
                }
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    screenErrors.push("Fax Number is required");
                }
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerAddress1Required) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.Address1_Potential)) {
                    screenErrors.push("Address1 is required");
                }
            }
        }
        if (this.vatTypeNotUnique) {
            if (SessionLocator_1.SessionLocator.TenantPM.VatUniqueTypeCode == "UFA") {
                screenErrors.push("VAT Number already exists");
            }
            else if (SessionLocator_1.SessionLocator.TenantPM.VatUniqueTypeCode == "USC") {
                screenErrors.push("VAT Number already exists for " + this.CountryEnglishName);
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.City_Potential)) {
            screenErrors.push("City is required");
        }
        var isLanguageValid = AddressValidator_1.AddressValidator.IsMainAddressEnglishCharacters_Potential(this.EntityPM);
        if (!isLanguageValid) {
            screenErrors.push("Main address does not allow non-english characters");
        }
        screenErrors.forEach(function (error) {
            allErrors.push(error);
        });
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsPickDelAdrsRequired) {
                if (this.PickupAddress == null) {
                    allErrors.push("PickUp Delivery Address is required please fill it");
                }
            }
            if (SessionLocator_1.SessionLocator.TenantPM.HasPrimaryContact) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PrimaryContactId)) {
                    allErrors.push("There is no Primary Contact please add one");
                }
            }
        }
        this.ValidationErrorsList = allErrors;
        if (allErrors.length == 0) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.DoActivation();
        }
        else if (screenErrors.length == 0) {
            this.EntityPM.SavedForActivation = true;
            this.Save("Just Saved");
        }
    };
    CustomerActivationComponent.prototype.DoActivation = function () {
        this.EntityPM.SetActivated = true;
        this.EntityPM.SetReady = false;
        this.Save("Activated");
    };
    CustomerActivationComponent.prototype.Save = function (msg) {
        var _this = this;
        if (msg == "Activated") {
            this.CurrentSession.StartBusyIndicatorSaving();
        }
        this.customerService.update(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                if (msg == "Activated") {
                    _this.CurrentSession.CloseCurrentWindowEmit(msg);
                    _this.CurrentSession.FireEvent("EntityActivated");
                }
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CustomerActivationComponent.prototype, "viewContainerRef", void 0);
    CustomerActivationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerActivationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerActivationComponent);
    return CustomerActivationComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerActivationComponent = CustomerActivationComponent;
//# sourceMappingURL=CustomerActivationComponent.js.map