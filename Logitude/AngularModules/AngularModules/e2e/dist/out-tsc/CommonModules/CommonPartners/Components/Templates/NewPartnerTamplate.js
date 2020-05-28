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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Args_1 = require("../../../../Common/Args");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var ContactPM_1 = require("../../../../Common/EntityPMs/ContactPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AddressValidator_1 = require("../../../../Infrastructure/Validators/AddressValidator");
var VatNumberValidator_1 = require("../../../../Infrastructure/Validators/VatNumberValidator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewPartnerTamplate = /** @class */ (function (_super) {
    __extends(NewPartnerTamplate, _super);
    function NewPartnerTamplate() {
        var _this = _super.call(this) || this;
        _this.CardId = null;
        _this.EntityPM = null;
        _this.CardTableName = null;
        _this.ObjectTableName = "Address";
        _this.DataContext = _this;
        _this.PartnerTypeId = null;
        _this.IsCustomer = false;
        _this.IsCustomerPartner = false;
        _this.IsCardCodeVisible = false;
        _this.IsAdditionalFieldsVisible = false;
        _this.SimilaryCardsHeader = "";
        _this.IsWarehouseTypeCodeVisible = false;
        _this.IsWarehouseFirmCodeVisible = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.Retries = 0;
        _this.IsSelectCityEnabled = true;
        _this.IsAddContactEnabled = true;
        _this.IsAdditionalEnabled = true;
        _this.CodeMessage = null;
        _this.IsCodeAlreadyExists = false;
        _this.searchText = null;
        _this.country = null;
        _this.state = null;
        // Contact
        _this.isAddContactChecked = false;
        _this.HasCardContact = false;
        _this.Info1Text = null;
        _this.Info2Text = null;
        _this.ExistedContactId = null;
        _this.loadedContact = null;
        _this.allCardContacts = [];
        _this.Address = new AddressPM_1.AddressPM();
        _this.Address.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.Address.AddressTypeId = "M";
        _this.Address.Description = "Main Address";
        _this.Address.IsCreatedWithPartner = true;
        _this.Contact = new ContactPM_1.ContactPM();
        _this.Contact.CardId = "newCard";
        _this.Contact.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.Contact.IsCreatedWithPartner = true;
        return _this;
    }
    NewPartnerTamplate.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    NewPartnerTamplate.prototype.InitTemplate = function () {
        if (this.PartnerTypeId == "CS" || this.PartnerTypeId == "PO") {
            this.IsCustomerPartner = true;
            this.IsAdditionalFieldsVisible = true;
        }
        switch (this.PartnerTypeId) {
            case "TR": {
                this.CardCode = null;
                this.IsCardCodeVisible = true;
                break;
            }
            case "WH": {
                this.Warehouse = this.EntityPM;
                this.CardCode = null;
                this.IsCardCodeVisible = true;
                this.IsWarehouseTypeCodeVisible = true;
                if (SessionLocator_1.SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
                    this.IsWarehouseFirmCodeVisible = true;
                }
                break;
            }
            default: {
                this.CardCode = "new";
                this.IsCardCodeVisible = false;
                break;
            }
        }
        //DefaultValues Abed Code
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DefaultValues)) {
            var DefaultValueData = this.DefaultValues.split("^");
            if (DefaultValueData[0] == "Trucker") {
                //Trucker
                this.CardCode = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[1]) ? DefaultValueData[1] : "";
                this.Name = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[2]) ? DefaultValueData[2] : "";
            }
            else {
                //Partners
                this.Name = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[0]) ? DefaultValueData[0] : "";
                this.Address1 = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[1]) ? DefaultValueData[1] : "";
                this.Address2 = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[2]) ? DefaultValueData[2] : "";
                this.City = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[3]) ? DefaultValueData[3] : "";
                this.CountryId = !Tools_1.AppTool.IsNullOrEmpty(DefaultValueData[4]) ? DefaultValueData[4] : "";
            }
        }
        this.SetUIProperties();
        this.RunComponent();
    };
    NewPartnerTamplate.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewPartnerTamplate.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewPartnerTamplate.prototype.LoadChildComponent = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response2) {
                _this.BuildAdditionalFields();
                _this.SimilaryCardsHeader = "Similar " + _this.CardTableName + " in the system";
            });
        });
    };
    NewPartnerTamplate.prototype.BuildAdditionalFields = function () {
        var _this = this;
        if (this.IsAdditionalFieldsVisible) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
                .then(function (cmpRef) {
                var screenCode = _this.CardTableName + ".AdditionalFields";
                cmpRef.instance.LabelWidth = 110;
                cmpRef.instance.Run(_this.EntityPM, _this.CardTableName, screenCode);
            });
        }
    };
    NewPartnerTamplate.prototype.SetUIProperties = function () {
        if (this.PartnerTypeId == "CS") {
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CREATEACTIVECUSTOMER")) {
                var isFieldsEnabled = true;
                if (this.IsCustomer) {
                    isFieldsEnabled = false;
                }
                this.UIProperties.SetEnabled("Name", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("Address1", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("Address2", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ZipCode", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("City", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("FaxNumber", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactEmail", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactName", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactPosition", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactBusinessPhone", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactMobile", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("ContactFax", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("FirmCode", this.ObjectTableName, isFieldsEnabled);
                this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, isFieldsEnabled);
                this.IsSelectCityEnabled = isFieldsEnabled;
                this.IsAddContactEnabled = isFieldsEnabled;
                this.IsAdditionalEnabled = isFieldsEnabled;
            }
        }
        this.UIProperties.SetVisibility("VatNumber", this.ObjectTableName, this.IsCustomerPartner);
        this.UIProperties.SetVisibility("SalesmanUserId", this.ObjectTableName, this.IsCustomerPartner);
        this.SetUIProperties_Code();
        this.SetUIProperties_VAT();
        this.SetUIProperties_TelFax();
        this.SetUIProperties_State();
        this.SetUIProperties_Contact();
    };
    NewPartnerTamplate.prototype.SetUIProperties_Code = function () {
        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {
            var isRequired = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.CardCode)) {
                isRequired = true;
            }
            this.UIProperties.SetRequired("CardCode", this.ObjectTableName, isRequired);
            if (!isRequired) {
                if (this.CardCode != null) {
                    if (this.CardTableName == "Trucker") {
                        if (this.CardCode.length >= 7) {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, false, "Code field must be less than 7 and more than 0");
                        }
                        else {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, true, "");
                        }
                    }
                    else {
                        if (this.CardCode.length >= 5) {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, false, "Code field must be less than 5 and more than 0");
                        }
                        else {
                            this.UIProperties.SetValidity("CardCode", this.ObjectTableName, true, "");
                        }
                    }
                }
            }
        }
    };
    NewPartnerTamplate.prototype.SetUIProperties_VAT = function () {
        if (this.IsCustomerPartner) {
            if (this.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.EntityPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;
                VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
            }
            else {
                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, false);
            }
        }
    };
    NewPartnerTamplate.prototype.SetUIProperties_TelFax = function () {
        if (this.IsCustomerPartner) {
            var isTelRequired = false;
            var isFaxRequired = false;
            if (this.IsCustomer) {
                if (this.PartnerTypeId == "CS") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            isFaxRequired = true;
                        }
                    }
                }
                else if (this.PartnerTypeId == "PO") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            isTelRequired = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            isFaxRequired = true;
                        }
                    }
                }
            }
            this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
            this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
        }
    };
    NewPartnerTamplate.prototype.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    NewPartnerTamplate.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    NewPartnerTamplate.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    };
    NewPartnerTamplate.prototype.SetUIProperties_Contact = function () {
        var isFieldEnabled = false;
        if (this.IsAddContactChecked) {
            if (this.loadedContact == null) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("ContactEmail", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactName", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactPosition", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactBusinessPhone", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactMobile", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ContactFax", this.ObjectTableName, isFieldEnabled);
        var isRequired = false;
        if (this.IsAddContactChecked) {
            if (this.ContactName == null) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("ContactName", this.ObjectTableName, isRequired);
    };
    Object.defineProperty(NewPartnerTamplate.prototype, "CardCode", {
        // Address
        get: function () { return this.Address.CardCode; },
        set: function (newValue) {
            if (this.Address.CardCode != newValue) {
                this.Address.CardCode = newValue;
                this.SetUIProperties_Code();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPartnerTamplate.prototype.CodeLostFocus = function (code) {
        var _this = this;
        this.CodeMessage = null;
        this.IsCodeAlreadyExists = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(code)) {
            if (code.length <= 4) {
                if (this.PartnerTypeId == "TR") {
                    this.DomainService.GetTruckerByCode(code, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
                        if (myResult != null) {
                            _this.CodeMessage = "This trucker already exists";
                            _this.IsCodeAlreadyExists = true;
                        }
                        else {
                            _this.DomainService.GetTruckerByCode(code, 0).subscribe(function (myResult) {
                                if (myResult != null) {
                                    _this.Name = myResult.EnglishName;
                                    _this.CodeMessage = "This trucker already exists in our database and on save it will be copied to your truckers list";
                                }
                            });
                        }
                    });
                }
                else if (this.PartnerTypeId == "WH") {
                    this.DomainService.GetWarehouseByCode(code, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
                        if (myResult != null) {
                            _this.CodeMessage = "This Warehouse already exists";
                            _this.IsCodeAlreadyExists = true;
                        }
                        else {
                            _this.DomainService.GetWarehouseByCode(code, 0).subscribe(function (myResult) {
                                if (myResult != null) {
                                    _this.Name = myResult.EnglishName;
                                    _this.CodeMessage = "This Warehouse already exists in our database and on save it will be copied to your Warehouses list";
                                }
                            });
                        }
                    });
                }
            }
        }
    };
    Object.defineProperty(NewPartnerTamplate.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "Name", {
        get: function () { return this.Address.Name; },
        set: function (value) {
            if (this.Address.Name != value) {
                this.Address.Name = value;
                this.LocalName = value;
                if (!this.DefaultValues) {
                    this.SearchText = value;
                }
                else {
                    this.DefaultValues = "";
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "LocalName", {
        get: function () { return this.localName; },
        set: function (newValue) {
            if (this.localName != newValue) {
                this.localName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "Description", {
        get: function () { return this.Address.Description; },
        set: function (newValue) {
            if (this.Address.Description != newValue) {
                this.Address.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "Address1", {
        get: function () { return this.Address.Address1; },
        set: function (newValue) {
            if (this.Address.Address1 != newValue) {
                this.Address.Address1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "Address2", {
        get: function () { return this.Address.Address2; },
        set: function (newValue) {
            if (this.Address.Address2 != newValue) {
                this.Address.Address2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ZipCode", {
        get: function () { return this.Address.ZipCode; },
        set: function (newValue) {
            if (this.Address.ZipCode != newValue) {
                this.Address.ZipCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "PhoneNumber", {
        get: function () { return this.Address.PhoneNumber; },
        set: function (newValue) {
            if (this.Address.PhoneNumber != newValue) {
                this.Address.PhoneNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "FaxNumber", {
        get: function () { return this.Address.FaxNumber; },
        set: function (newValue) {
            if (this.Address.FaxNumber != newValue) {
                this.Address.FaxNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "FirmCode", {
        get: function () { return this.Warehouse.FirmCode; },
        set: function (newValue) {
            if (this.Warehouse.FirmCode != newValue) {
                this.Warehouse.FirmCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "TypeCode", {
        get: function () { return this.Warehouse.TypeCode; },
        set: function (newValue) {
            if (this.Warehouse.TypeCode != newValue) {
                this.Warehouse.TypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ATTN", {
        get: function () { return this.Address.ATTN; },
        set: function (newValue) {
            if (this.Address.ATTN != newValue) {
                this.Address.ATTN = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "City", {
        get: function () { return this.Address.City; },
        set: function (newValue) {
            if (this.Address.City != newValue) {
                this.Address.City = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "Country", {
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
    Object.defineProperty(NewPartnerTamplate.prototype, "CountryId", {
        get: function () { return this.Address.CountryId; },
        set: function (newValue) {
            if (this.Address.CountryId != newValue) {
                this.Address.CountryId = newValue;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "CountryCode", {
        get: function () { return this.Address.CountryCode; },
        set: function (newValue) {
            if (this.Address.CountryCode != newValue) {
                this.Address.CountryCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "CountryName", {
        get: function () { return this.Address.CountryName; },
        set: function (newValue) {
            if (this.Address.CountryName != newValue) {
                this.Address.CountryName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "CountryEnglishName", {
        get: function () { return this.Address.CountryEnglishName; },
        set: function (newValue) {
            if (this.Address.CountryEnglishName != newValue) {
                this.Address.CountryEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "State", {
        get: function () { return this.state; },
        set: function (newValue) {
            if (this.state != newValue) {
                this.state = newValue;
                this.OnStateChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "StateId", {
        get: function () { return this.Address.StateId; },
        set: function (newValue) {
            if (this.Address.StateId != newValue) {
                this.Address.StateId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "StateCode", {
        get: function () { return this.Address.StateCode; },
        set: function (newValue) {
            if (this.Address.StateCode != newValue) {
                this.Address.StateCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "StateEnglishName", {
        get: function () { return this.Address.StateEnglishName; },
        set: function (newValue) {
            if (this.Address.StateEnglishName != newValue) {
                this.Address.StateEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "VatNumber", {
        get: function () { return this.Address.VatNumber; },
        set: function (newValue) {
            if (this.Address.VatNumber != newValue) {
                this.Address.VatNumber = newValue;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "SalesmanUserId", {
        get: function () { return this.Address.SalesmanUserId; },
        set: function (newValue) {
            if (this.Address.SalesmanUserId != newValue) {
                this.Address.SalesmanUserId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "IsLocalLanguage", {
        get: function () { return this.Address.IsLocalLanguage; },
        set: function (newValue) {
            if (this.Address.IsLocalLanguage != newValue) {
                this.Address.IsLocalLanguage = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPartnerTamplate.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = this.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_VAT();
        this.SetUIProperties_State();
    };
    NewPartnerTamplate.prototype.OnStateChanged = function (list) {
        if (list == null) {
            this.StateCode = null;
            this.StateEnglishName = null;
        }
        else {
            this.StateCode = list.Code;
            this.StateEnglishName = list.EnglishName;
        }
        this.SetUIProperties_StateRequired();
    };
    // Select City
    NewPartnerTamplate.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    Object.defineProperty(NewPartnerTamplate.prototype, "IsAddContactChecked", {
        get: function () { return this.isAddContactChecked; },
        set: function (newValue) {
            if (this.isAddContactChecked != newValue) {
                this.isAddContactChecked = newValue;
                if (!newValue) {
                    this.ContactEmail = null;
                    this.ContactName = null;
                    this.ContactPosition = null;
                    this.ContactBusinessPhone = null;
                    this.ContactMobile = null;
                    this.ContactFax = null;
                    this.Info1Text = null;
                    this.Info2Text = null;
                    this.loadedContact = null;
                    this.allCardContacts = [];
                    this.ExistedContactId = null;
                    this.ValidateContactExist();
                }
                this.SetUIProperties_Contact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactEmail", {
        get: function () { return this.Address.ContactEmail; },
        set: function (newValue) {
            if (this.Address.ContactEmail != newValue) {
                this.Address.ContactEmail = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactName", {
        get: function () { return this.Address.ContactName; },
        set: function (newValue) {
            if (this.Address.ContactName != newValue) {
                this.Address.ContactName = newValue;
                var isRequired = false;
                if (this.IsAddContactChecked) {
                    if (this.ContactName == null) {
                        isRequired = true;
                    }
                }
                this.UIProperties.SetRequired("ContactName", this.ObjectTableName, isRequired);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactPosition", {
        get: function () { return this.Address.ContactPosition; },
        set: function (newValue) {
            if (this.Address.ContactPosition != newValue) {
                this.Address.ContactPosition = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactBusinessPhone", {
        get: function () { return this.Address.ContactBusinessPhone; },
        set: function (newValue) {
            if (this.Address.ContactBusinessPhone != newValue) {
                this.Address.ContactBusinessPhone = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactMobile", {
        get: function () { return this.Address.ContactMobile; },
        set: function (newValue) {
            if (this.Address.ContactMobile != newValue) {
                this.Address.ContactMobile = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPartnerTamplate.prototype, "ContactFax", {
        get: function () { return this.Address.ContactFax; },
        set: function (newValue) {
            if (this.Address.ContactFax != newValue) {
                this.Address.ContactFax = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPartnerTamplate.prototype.EmailLostFocus = function (email) {
        var _this = this;
        var isLoading = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
            if (email.indexOf('@') > -1 && email.indexOf('.') > -1) {
                isLoading = true;
            }
        }
        if (!isLoading) {
            this.Info1Text = null;
            this.Info2Text = null;
            this.loadedContact = null;
            this.allCardContacts = [];
            this.ExistedContactId = null;
            this.SetUIProperties_Contact();
            this.ValidateContactExist();
        }
        else {
            this.DomainService.GetContactsByEmail(email).subscribe(function (myResult) {
                if (myResult != null) {
                    _this.loadedContact = myResult[0];
                    _this.SetUIProperties_Contact();
                    if (_this.loadedContact != null) {
                        _this.ExistedContactId = _this.loadedContact.Id;
                        if (_this.loadedContact.InActive) {
                            _this.Info1Text = "This Contact is InActive";
                        }
                        else {
                            _this.ContactName = _this.loadedContact.EnglishName;
                            _this.ContactPosition = _this.loadedContact.Position;
                            _this.ContactBusinessPhone = _this.loadedContact.BusinessPhone;
                            _this.ContactMobile = _this.loadedContact.Mobile;
                            _this.ContactFax = _this.loadedContact.Fax;
                            _this.HasCardContact = _this.loadedContact.HasCardContact;
                            _this.DomainService.GetCardContactsByContact(_this.loadedContact.Id).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    _this.allCardContacts = myResponse.Result;
                                    _this.ValidateContactExist();
                                    var otherCardContacts = _this.allCardContacts.filter(function (d) { return d.ContactId == _this.loadedContact.Id && d.CardId != _this.CardId; });
                                    if (otherCardContacts.length == 0) {
                                        _this.Info2Text = null;
                                    }
                                    else {
                                        _this.Info2Text = "This Contact is Already Added for " + otherCardContacts.length + " other Partners!";
                                    }
                                }
                            });
                        }
                    }
                }
            });
        }
    };
    NewPartnerTamplate.prototype.Validate = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidateCardCode(errors, msg);
        Validator_1.Validator.TryValidateObject(this.Address, this.ObjectTableName, errors);
        var isLanguageValid = AddressValidator_1.AddressValidator.IsMainAddressEnglishCharacters(this.Address);
        if (!isLanguageValid) {
            errors.push("Main address does not allow non-english characters");
        }
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    errors.push(msg.replace("%FieldName", "State"));
                }
            }
        }
        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CardCode)) {
                errors.push(msg.replace("%FieldName", "Code"));
            }
        }
        if (this.IsCustomerPartner) {
            this.ValidateCustomerFields(errors);
        }
        if (this.IsAddContactChecked) {
            this.Contact.EnglishName = this.ContactName;
            this.Contact.LocalName = this.ContactName;
            this.Contact.Email = this.ContactEmail;
            this.Contact.Position = this.ContactPosition;
            this.Contact.BusinessPhone = this.ContactBusinessPhone;
            this.Contact.Mobile = this.ContactMobile;
            this.Contact.Fax = this.ContactFax;
            if (this.loadedContact != null) {
                this.Contact.Anniversary = this.loadedContact.Anniversary;
                this.Contact.Birthday = this.loadedContact.Birthday;
                this.Contact.InActive = this.loadedContact.InActive;
            }
            Validator_1.Validator.TryValidateObject(this.Contact, "Contact", errors);
            if (Tools_1.AppTool.IsNullOrEmpty(this.Contact.EnglishName)) {
                errors.push(msg.replace("%FieldName", "English Name"));
            }
            if (!Tools_1.FormatTool.IsEmail(this.ContactEmail)) {
                errors.push("Invalid email format!");
            }
            if (this.ValidateContactExist()) {
                errors.push("This Contact is Already added for you");
            }
            if (this.Contact.InActive) {
                errors.push("This Contact is InActive");
            }
        }
        return errors;
    };
    NewPartnerTamplate.prototype.ValidateCardCode = function (errors, msg) {
        if (this.PartnerTypeId == "TR" || this.PartnerTypeId == "WH") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CardCode)) {
                errors.push(msg.replace("%FieldName", "Code"));
            }
            else {
                if (this.CardTableName == "Trucker") {
                    if (this.CardCode.length >= 7) {
                        errors.push("Code must be less than 7");
                    }
                }
                else {
                    if (this.CardCode.length >= 5) {
                        errors.push("Code must be less than 5");
                    }
                }
            }
        }
    };
    NewPartnerTamplate.prototype.ValidateCustomerFields = function (errors) {
        if (this.IsCustomerPartner) {
            if (this.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.EntityPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;
                VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
                args.Errors.forEach(function (item) {
                    errors.push(item);
                });
                if (this.PartnerTypeId == "CS") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
                else if (this.PartnerTypeId == "PO") {
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
            }
        }
    };
    NewPartnerTamplate.prototype.ValidateContactExist = function () {
        var _this = this;
        var isContactAlreadyExist = false;
        if (this.CardId != null) {
            if (this.loadedContact != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(function (d) { return d.ContactId == _this.loadedContact.Id && d.CardId == _this.CardId; }).length > 0) {
                        isContactAlreadyExist = true;
                    }
                }
            }
        }
        this.Info1Text = isContactAlreadyExist ? TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
        return isContactAlreadyExist;
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewPartnerTamplate.prototype, "viewContainerRef", void 0);
    NewPartnerTamplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewPartnerTamplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewPartnerTamplate);
    return NewPartnerTamplate;
}(BaseComponent_1.BaseComponent));
exports.NewPartnerTamplate = NewPartnerTamplate;
//# sourceMappingURL=NewPartnerTamplate.js.map