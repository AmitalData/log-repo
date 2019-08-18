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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../../Common/Args");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var AgentPM_1 = require("../../../../Common/EntityPMs/AgentPM");
var CustomerPM_1 = require("../../../../Common/EntityPMs/CustomerPM");
var AddressPMService_1 = require("../../../../Common/Services/StandardPMs/AddressPMService");
var AgentPMService_1 = require("../../../../Common/Services/StandardPMs/AgentPMService");
var CustomerPMService_1 = require("../../../../Common/Services/StandardPMs/CustomerPMService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var AddressValidator_1 = require("../../../../Infrastructure/Validators/AddressValidator");
var VatNumberValidator_1 = require("../../../../Infrastructure/Validators/VatNumberValidator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var AddEditPartnerComponent = /** @class */ (function (_super) {
    __extends(AddEditPartnerComponent, _super);
    function AddEditPartnerComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.IsNewEntity = false;
        _this.IsCancelled = false;
        _this.DataContext = _this;
        _this.ObjectTableName = "Address";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isPartnerLoaded = false;
        _this.isAddressLoaded = false;
        _this.country = null;
        _this.state = null;
        _this.isPartnerDirty = false;
        return _this;
    }
    AddEditPartnerComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.bookingPM = windowArgs.EntityPM;
        this.IsNewEntity = windowArgs.IsNewEntity;
        this.PartnerTypeCode = windowArgs.PartnerTypeCode;
        this.FatherComponent = windowArgs.FatherComponent;
        this.InitializeComponent();
    };
    AddEditPartnerComponent.prototype.ngAfterViewInit = function () {
        this.SetUIProperties();
    };
    AddEditPartnerComponent.prototype.InitializeComponent = function () {
        var _this = this;
        if (this.IsNewEntity) {
            if (this.PartnerTypeCode == "AGT" || this.bookingPM.BookingLevelCode == "C") {
                this.PartnerTypeId = "AG";
                this.myAgentPM = new AgentPM_1.AgentPM();
                this.myAgentPM.Tenant = this.bookingPM.Tenant;
                this.myAgentPM.PartnerTypeId = this.PartnerTypeId;
                this.myAgentPM.Code = "new";
                this.EntityPM = new AddressPM_1.AddressPM();
                this.EntityPM.Tenant = this.bookingPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";
                this.myAgentPM.Addresses.push(this.EntityPM);
            }
            else {
                this.PartnerTypeId = "CS";
                this.myCustomerPM = new CustomerPM_1.CustomerPM();
                this.myCustomerPM.Tenant = this.bookingPM.Tenant;
                this.myCustomerPM.PartnerTypeId = this.PartnerTypeId;
                this.myCustomerPM.CustomerStatusCode = "ACT";
                this.myCustomerPM.IsCustomer = this.PartnerTypeCode == "SHI" ? true : false;
                this.myCustomerPM.Code = "new";
                this.EntityPM = new AddressPM_1.AddressPM();
                this.EntityPM.Tenant = this.bookingPM.Tenant;
                this.EntityPM.AddressTypeId = "M";
                this.EntityPM.Description = "Main Address";
                this.myCustomerPM.Addresses.push(this.EntityPM);
            }
            this.SetUIProperties();
        }
        else {
            switch (this.PartnerTypeCode) {
                case "SHI":
                    {
                        this.CurrentPartnerId = this.bookingPM.ShipperId;
                        this.CurrentAddressId = this.bookingPM.ShipperAddressId;
                        break;
                    }
                case "CON":
                    {
                        this.CurrentPartnerId = this.bookingPM.ConsigneeId;
                        this.CurrentAddressId = this.bookingPM.ConsigneeAddressId;
                        break;
                    }
                case "AGT":
                    {
                        this.CurrentPartnerId = this.bookingPM.IssuingCarrierAgentId;
                        this.CurrentAddressId = this.bookingPM.IssuingCarrierAddressId;
                        break;
                    }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentPartnerId)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                var myService = new CardListService_1.CardListService();
                myService.getSingle(this.CurrentPartnerId).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        var card = myResponse.Result;
                        if (card != null) {
                            _this.PartnerTypeId = card.PartnerTypeId;
                            _this.LoadPartner();
                            _this.LoadAddress();
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }, function (error) {
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
    };
    AddEditPartnerComponent.prototype.LoadPartner = function () {
        var _this = this;
        this.isPartnerLoaded = false;
        var myService = null;
        switch (this.PartnerTypeId) {
            case "AG": {
                myService = new AgentPMService_1.AgentPMService();
                myService.get(this.CurrentPartnerId).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        _this.myAgentPM = myResponse.Result;
                        _this.isPartnerLoaded = true;
                        _this.OnLoadCompleted();
                    }
                });
                break;
            }
            case "CS": {
                myService = new CustomerPMService_1.CustomerPMService();
                myService.get(this.CurrentPartnerId).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        _this.myCustomerPM = myResponse.Result;
                        _this.isPartnerLoaded = true;
                        _this.OnLoadCompleted();
                    }
                });
                break;
            }
        }
    };
    AddEditPartnerComponent.prototype.LoadAddress = function () {
        var _this = this;
        this.isAddressLoaded = false;
        var myService = new AddressPMService_1.AddressPMService();
        myService.get(this.CurrentAddressId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                _this.isAddressLoaded = true;
                _this.OnLoadCompleted();
            }
        });
    };
    AddEditPartnerComponent.prototype.OnLoadCompleted = function () {
        if (this.isPartnerLoaded && this.isAddressLoaded) {
            this.SetUIProperties();
            this.CurrentSession.StopBusyIndicator();
        }
    };
    // SetUIProperties
    AddEditPartnerComponent.prototype.SetUIProperties = function () {
        if (this.EntityPM != null) {
            this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CardEnglishName) ? true : false);
            if (this.EntityPM.AddressTypeId == "O") {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetVisibility("Description", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("InActive", this.ObjectTableName, false);
            }
            this.SetUIProperties_VAT();
            this.SetUIProperties_State();
            this.SetUIProperties_TelFax();
        }
    };
    AddEditPartnerComponent.prototype.SetUIProperties_VAT = function () {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
                args.PartnerTypeId = this.PartnerTypeId;
                args.CountryId = this.CountryId;
                args.CountryName = this.CountryName;
                args.CountryEnglishName = this.CountryEnglishName;
                args.SetReady = false;
                VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
                VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
                this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
            }
        }
    };
    AddEditPartnerComponent.prototype.SetUIProperties_State = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    };
    AddEditPartnerComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    AddEditPartnerComponent.prototype.SetUIProperties_StateRequired = function () {
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
    AddEditPartnerComponent.prototype.SetUIProperties_TelFax = function () {
        if (this.myCustomerPM != null) {
            var isTelRequired = false;
            var isFaxRequired = false;
            if (this.myCustomerPM != null) {
                if (this.myCustomerPM.IsCustomer) {
                    if (this.myCustomerPM.PartnerTypeId == "CS") {
                        if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerTelRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                                isTelRequired = true;
                            }
                        }
                        if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerFaxRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                                isFaxRequired = true;
                            }
                        }
                    }
                    else if (this.myCustomerPM.PartnerTypeId == "PO") {
                        if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialTelRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                                isTelRequired = true;
                            }
                        }
                        if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialFaxRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                                isFaxRequired = true;
                            }
                        }
                    }
                }
            }
            this.UIProperties.SetRequired("PhoneNumber", this.ObjectTableName, isTelRequired);
            this.UIProperties.SetRequired("FaxNumber", this.ObjectTableName, isFaxRequired);
        }
    };
    Object.defineProperty(AddEditPartnerComponent.prototype, "Description", {
        // Properties
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Description != newValue) {
                    this.EntityPM.Description = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "CardEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CardEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CardEnglishName != newValue) {
                    this.EntityPM.Name = newValue;
                    this.EntityPM.CardEnglishName = newValue;
                    this.UIProperties.SetRequired("CardEnglishName", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(newValue) ? true : false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "Address1", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Address1; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Address1 != newValue) {
                    this.EntityPM.Address1 = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "Address2", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.Address2; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Address2 != newValue) {
                    this.EntityPM.Address2 = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "City", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.City; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.City != newValue) {
                    this.EntityPM.City = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "ATTN", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ATTN; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ATTN != newValue) {
                    this.EntityPM.ATTN = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "ZipCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ZipCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ZipCode != newValue) {
                    this.EntityPM.ZipCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.PhoneNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PhoneNumber != newValue) {
                    this.EntityPM.PhoneNumber = newValue;
                    this.SetUIProperties_TelFax();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "FaxNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.FaxNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.FaxNumber != newValue) {
                    this.EntityPM.FaxNumber = newValue;
                    this.SetUIProperties_TelFax();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VatNumber != newValue) {
                    this.EntityPM.VatNumber = newValue;
                    this.SetUIProperties_VAT();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "Country", {
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
    Object.defineProperty(AddEditPartnerComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryId != newValue) {
                    this.EntityPM.CountryId = newValue;
                    this.StateId = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryCode != newValue) {
                    this.EntityPM.CountryCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "CountryName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryName != newValue) {
                    this.EntityPM.CountryName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CountryEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CountryEnglishName != newValue) {
                    this.EntityPM.CountryEnglishName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "State", {
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
    Object.defineProperty(AddEditPartnerComponent.prototype, "StateId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateId; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateId != newValue) {
                    this.EntityPM.StateId = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "StateCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateCode; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateCode != newValue) {
                    this.EntityPM.StateCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPartnerComponent.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.StateEnglishName; },
        set: function (newValue) {
            if (this.EntityPM != null) {
                if (this.EntityPM.StateEnglishName != newValue) {
                    this.EntityPM.StateEnglishName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditPartnerComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = this.EntityPM.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_VAT();
        this.SetUIProperties_State();
    };
    AddEditPartnerComponent.prototype.OnStateChanged = function (list) {
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
    AddEditPartnerComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
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
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    AddEditPartnerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPartnerComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                switch (this.PartnerTypeId) {
                    case "AG":
                        {
                            if (this.myAgentPM != null) {
                                if (this.myAgentPM.EnglishName != this.CardEnglishName) {
                                    this.myAgentPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myAgentPM.VatNumber != this.VatNumber) {
                                    this.myAgentPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myAgentPM.IsDirty;
                            }
                            break;
                        }
                    case "CS":
                        {
                            if (this.myCustomerPM != null) {
                                if (this.myCustomerPM.EnglishName != this.CardEnglishName) {
                                    this.myCustomerPM.EnglishName = this.CardEnglishName;
                                }
                                if (this.myCustomerPM.VatNumber != this.VatNumber) {
                                    this.myCustomerPM.VatNumber = this.VatNumber;
                                }
                                this.isPartnerDirty = this.myCustomerPM.IsDirty;
                            }
                            break;
                        }
                }
                this.Save();
            }
        }
    };
    AddEditPartnerComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.EntityPM != null) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            var isLanguageValid = AddressValidator_1.AddressValidator.IsMainAddressEnglishCharacters(this.EntityPM);
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
            this.ValidateCustomerFields(errors);
            if (errors.length == 0) {
                var myCity = this.City;
                var myName = this.CardEnglishName;
                if (!Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    myCity = myCity.trim();
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(myName)) {
                    myName = myName.trim();
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    errors.push(msg.replace("%FieldName", "City"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myName)) {
                    errors.push(msg.replace("%FieldName", "Name"));
                }
            }
        }
        if (errors.length > 0) {
            isValid = false;
        }
        this.ValidationErrorsList = errors;
        return isValid;
    };
    AddEditPartnerComponent.prototype.ValidateCustomerFields = function (errors) {
        if (this.myCustomerPM != null) {
            if (this.myCustomerPM.IsCustomer) {
                var args = new VatNumberValidator_1.VATValidatorArgs();
                args.VATNumber = this.VatNumber;
                args.IsCustomer = this.myCustomerPM.IsCustomer;
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
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsCustomerFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
                else if (this.PartnerTypeId == "PO") {
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialTelRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber)) {
                            errors.push("Phone Number is required");
                        }
                    }
                    if (InfraSettings_1.InfraSettings.TenantPM.IsPotentialFaxRequired) {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber)) {
                            errors.push("Fax Number is required");
                        }
                    }
                }
            }
        }
    };
    AddEditPartnerComponent.prototype.Save = function () {
        var _this = this;
        if (this.myPartnersDomainService == null) {
            this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        }
        var args = new PartnersDomainService_1.PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.AddressId = this.CurrentAddressId;
        args.PartnerId = this.CurrentPartnerId;
        args.PartnerTypeId = this.PartnerTypeId;
        args.IsAddressDirty = this.EntityPM.IsDirty;
        args.IsPartnerDirty = this.isPartnerDirty;
        args.Address = this.EntityPM;
        args.Agent = this.myAgentPM;
        args.Customer = this.myCustomerPM;
        this.myPartnersDomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentAddressId = myResponse.Result.AddressId;
                _this.CurrentPartnerId = myResponse.Result.PartnerId;
                _this.FatherComponent.UpdatePartner(_this.PartnerTypeCode, _this.CurrentPartnerId, _this.CurrentAddressId);
                _this.CurrentSession.CloseCurrentWindow();
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        }, function (error) {
            _this.ValidationErrorsList.push(error);
            _this.CurrentSession.CloseCurrentWindow();
        });
    };
    AddEditPartnerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPartnerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPartnerComponent);
    return AddEditPartnerComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPartnerComponent = AddEditPartnerComponent;
//# sourceMappingURL=AddEditPartnerComponent.js.map