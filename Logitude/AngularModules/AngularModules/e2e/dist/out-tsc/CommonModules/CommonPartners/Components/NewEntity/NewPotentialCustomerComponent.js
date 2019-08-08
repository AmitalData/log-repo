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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var CustomerPM_1 = require("../../../../Common/EntityPMs/CustomerPM");
var ContactPM_1 = require("../../../../Common/EntityPMs/ContactPM");
var CustomerSalesNotePM_1 = require("../../../../Common/EntityPMs/CustomerSalesNotePM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var VatNumberValidator_1 = require("../../../../Infrastructure/Validators/VatNumberValidator");
var Args_1 = require("../../../../Common/Args");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var NewPotentialCustomerComponent = /** @class */ (function (_super) {
    __extends(NewPotentialCustomerComponent, _super);
    function NewPotentialCustomerComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.CardId = null;
        _this.PartnerTypeId = "PO";
        _this.ObjectTableName = "Customer";
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.IsRadioButtonsVisible = false;
        _this.ShowContactPart = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.searchText = null;
        _this.country = null;
        _this.state = null;
        //Contact
        _this.isAddContactChecked = false;
        _this.EntityPM = new CustomerPM_1.CustomerPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.PartnerTypeId = _this.PartnerTypeId;
        _this.EntityPM.CustomerStatusCode = "POT";
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.IsCustomer = true;
        _this.EntityPM.Code = "new";
        _this.Contact = new ContactPM_1.ContactPM();
        _this.Contact.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.Contact.CardId = "newCard";
        _this.Contact.IsCreatedWithPartner = true;
        _this.Contact.SetAsPrimaryForCard = true;
        _this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.ContactDataContext = new ContactItem(_this.Contact, _this.EntityPM, _this);
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHIPPERSANDCONSIGNEES")) {
            _this.IsRadioButtonsVisible = true;
        }
        _this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Contact").subscribe(function (response2) {
                _this.entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (response3) {
                    _this.IsResourcesReady = true;
                    if (_this.ShowContactPart) {
                        _this.IsAddContactChecked = false;
                    }
                    else {
                        _this.IsAddContactChecked = true;
                    }
                    _this.SetUIProperties();
                    _this.RunComponent();
                });
            });
        });
        return _this;
    }
    NewPotentialCustomerComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.args = args;
            this.ShowContactPart = args.ShowContactPart;
            if (this.ShowContactPart) {
                this.IsAddContactChecked = false;
            }
            else {
                this.IsAddContactChecked = true;
            }
            if (args.Perspective == "ShippersAndConsignees") {
                this.IsCustomer = false;
            }
        }
    };
    NewPotentialCustomerComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewPotentialCustomerComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewPotentialCustomerComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, "Customer.AdditionalFields");
        });
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties = function () {
        var isCountryRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            isCountryRequired = true;
        }
        this.UIProperties.SetRequired("CountryId_Potential", this.ObjectTableName, isCountryRequired);
        this.SetUIProperties_LocalName();
        this.SetUIProperties_PhonFax();
        this.SetUIProperties_State();
        this.SetUIProperties_VAT();
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_LocalName = function () {
        var isLocalNameRequired = false;
        if (this.IsLocalLanguage) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.LocalName)) {
                isLocalNameRequired = true;
            }
        }
        this.UIProperties.SetRequired("LocalName", this.ObjectTableName, isLocalNameRequired);
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_PhonFax = function () {
        var isTelRequired = false;
        var isFaxRequired = false;
        if (this.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    isTelRequired = true;
                }
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    isFaxRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("PhoneNumber_Potential", this.ObjectTableName, isTelRequired);
        this.UIProperties.SetRequired("FaxNumber_Potential", this.ObjectTableName, isFaxRequired);
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_VAT = function () {
        if (this.IsCustomer) {
            var args = new VatNumberValidator_1.VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.IsCustomer;
            args.PartnerTypeId = this.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.EntityPM.CountryName;
            args.SetReady = false;
            VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
        }
        else {
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, false);
        }
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_State = function () {
        //var isEnabled = false;
        //if (this.Country != null) {
        //    if (this.Country.HasStates) {
        //        isEnabled = true;
        //    }
        //}
        //this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, isEnabled);
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId_Potential", this.ObjectTableName, isEnabled);
    };
    NewPotentialCustomerComponent.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("StateId_Potential", this.ObjectTableName, isRequired);
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPotentialCustomerComponent.prototype.SetIsCustomer = function (isCustomer) {
        this.IsCustomer = isCustomer;
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "IsCustomer", {
        get: function () { return this.EntityPM.IsCustomer; },
        set: function (newValue) {
            if (this.EntityPM.IsCustomer != newValue) {
                this.EntityPM.IsCustomer = newValue;
                this.SetUIProperties_VAT();
                this.SetUIProperties_PhonFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "IsLocalLanguage", {
        get: function () { return this.EntityPM.IsLocalLanguage; },
        set: function (newValue) {
            if (this.EntityPM.IsLocalLanguage != newValue) {
                this.EntityPM.IsLocalLanguage = newValue;
                this.EnableLocalLanguage();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPotentialCustomerComponent.prototype.EnableLocalLanguage = function () {
        if (!this.IsLocalLanguage) {
            var pattern = /^a-zA-Z0-9\s/;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EnglishName)) {
                this.EnglishName = this.EnglishName.replace(pattern, "");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Address1_Potential)) {
                this.Address1_Potential = this.Address1_Potential.replace(pattern, "");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Address2_Potential)) {
                this.Address2_Potential = this.Address2_Potential.replace(pattern, "");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.City_Potential)) {
                this.City_Potential = this.City_Potential.replace(pattern, "");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ATTN_Potential)) {
                this.EntityPM.ATTN_Potential = this.EntityPM.ATTN_Potential.replace(pattern, "");
            }
        }
        this.SetUIProperties_LocalName();
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
                this.SearchText = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM.VatNumber != newValue) {
                this.EntityPM.VatNumber = newValue;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "SalesmanUserId", {
        get: function () { return this.EntityPM.SalesmanUserId; },
        set: function (newValue) {
            if (this.EntityPM.SalesmanUserId != newValue) {
                this.EntityPM.SalesmanUserId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
                this.SetUIProperties_LocalName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "Address1_Potential", {
        get: function () { return this.EntityPM.Address1_Potential; },
        set: function (newValue) {
            if (this.EntityPM.Address1_Potential != newValue) {
                this.EntityPM.Address1_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "Address2_Potential", {
        get: function () { return this.EntityPM.Address2_Potential; },
        set: function (newValue) {
            if (this.EntityPM.Address2_Potential != newValue) {
                this.EntityPM.Address2_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "ZipCode_Potential", {
        get: function () { return this.EntityPM.ZipCode_Potential; },
        set: function (newValue) {
            if (this.EntityPM.ZipCode_Potential != newValue) {
                this.EntityPM.ZipCode_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "City_Potential", {
        get: function () { return this.EntityPM.City_Potential; },
        set: function (newValue) {
            if (this.EntityPM.City_Potential != newValue) {
                this.EntityPM.City_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "CountryId_Potential", {
        get: function () { return this.EntityPM.CountryId_Potential; },
        set: function (newValue) {
            if (this.EntityPM.CountryId_Potential != newValue) {
                this.EntityPM.CountryId_Potential = newValue;
                this.StateId_Potential = null;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "Country", {
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
    NewPotentialCustomerComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.EntityPM.CountryCode = null;
            this.EntityPM.CountryName = null;
        }
        else {
            this.EntityPM.CountryCode = list.Code;
            this.EntityPM.CountryName = list.EnglishName;
        }
        this.SetUIProperties();
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "StateId_Potential", {
        get: function () { return this.EntityPM.StateId_Potential; },
        set: function (newValue) {
            if (this.EntityPM.StateId_Potential != newValue) {
                this.EntityPM.StateId_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "State", {
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
    NewPotentialCustomerComponent.prototype.OnStateChanged = function (list) {
        if (list == null) {
            //this.StateCode = null;
            //this.StateEnglishName = null;
        }
        else {
            //this.StateCode = list.Code;
            //this.StateEnglishName = list.EnglishName;
        }
        this.SetUIProperties_StateRequired();
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "PhoneNumber_Potential", {
        get: function () { return this.EntityPM.PhoneNumber_Potential; },
        set: function (newValue) {
            if (this.EntityPM.PhoneNumber_Potential != newValue) {
                this.EntityPM.PhoneNumber_Potential = newValue;
                this.SetUIProperties_PhonFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "FaxNumber_Potential", {
        get: function () { return this.EntityPM.FaxNumber_Potential; },
        set: function (newValue) {
            if (this.EntityPM.FaxNumber_Potential != newValue) {
                this.EntityPM.FaxNumber_Potential = newValue;
                this.SetUIProperties_PhonFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPotentialCustomerComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId_Potential);
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
                _this.City_Potential = mySelectedCity;
                _this.CountryId_Potential = args.CountryId;
                _this.StateId_Potential = args.StateId;
            }
        });
    };
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "IsAddContactChecked", {
        get: function () { return this.isAddContactChecked; },
        set: function (value) {
            if (this.isAddContactChecked != value) {
                this.isAddContactChecked = value;
                if (value) {
                    if (this.EntityPM.Contacts.length == 0) {
                        this.EntityPM.Contacts.push(this.Contact);
                    }
                }
                else {
                    this.EntityPM.Contacts = [];
                    this.ContactDataContext.Email = null;
                    this.ContactDataContext.EnglishName = null;
                    this.ContactDataContext.LocalName = null;
                    this.ContactDataContext.BusinessPhone = null;
                    this.ContactDataContext.Mobile = null;
                    this.ContactDataContext.Fax = null;
                    this.ContactDataContext.Position = null;
                    this.ContactDataContext.LocalName = null;
                    this.ContactDataContext.Info2Text = null;
                }
                this.ContactDataContext.CloseContactFields(!value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPotentialCustomerComponent.prototype, "EntityNotes", {
        get: function () { return this.entityNotes; },
        set: function (newValue) {
            if (this.entityNotes != newValue) {
                this.entityNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPotentialCustomerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewPotentialCustomerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.IsLocalLanguage) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalName)) {
                errors.push("Local Name is Required");
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNotes)) {
            if (this.EntityNotes.length > 250) {
                errors.push("Sales Notes must be less than 250 char");
            }
        }
        var isLanguageValid = this.ValidateLocalLanguage(this.EntityPM);
        if (!isLanguageValid) {
            errors.push("Main address does not allow non-english characters");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId_Potential)) {
            errors.push("Country is required");
        }
        if (this.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber_Potential)) {
                    errors.push("Phone Number is required");
                }
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber_Potential)) {
                    errors.push("Fax Number is required");
                }
            }
            var args = new VatNumberValidator_1.VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.IsCustomer;
            args.PartnerTypeId = this.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.EntityPM.CountryName;
            args.SetReady = false;
            VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
            args.Errors.forEach(function (item) {
                errors.push(item);
            });
        }
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    errors.push("State is required");
                }
            }
        }
        if (this.IsAddContactChecked) {
            var index = this.EntityPM.Contacts.indexOf(this.Contact);
            if (index > -1) {
                if (!this.ShowContactPart) {
                    Validator_1.Validator.TryValidateObject(this.Contact, this.ContactDataContext.ObjectTableName, errors);
                }
                if (this.ContactDataContext.ContactAlreadyExist) {
                    errors.push("This Contact is Already added for you");
                }
            }
            if (!Tools_1.FormatTool.IsEmail(this.ContactDataContext.Contact.Email)) {
                errors.push("Invalid email format!");
            }
        }
        else {
            this.EntityPM.Contacts = [];
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityNotes)) {
                var note = new CustomerSalesNotePM_1.CustomerSalesNotePM(this.EntityPM);
                note.Tenant = SessionLocator_1.SessionLocator.Tenant;
                note.CustomerId = this.EntityPM.Id;
                note.Notes = this.EntityNotes;
                this.EntityPM.AddCustomerSalesNotePM(note);
            }
            var partnerArgs = new PartnersDomainService_1.PartnerServicePM();
            partnerArgs.Tenant = this.EntityPM.Tenant;
            partnerArgs.PartnerTypeId = this.PartnerTypeId;
            partnerArgs.Customer = this.EntityPM;
            //partnerArgs.Address = this.PartnerTamplate.Address;
            if (this.IsAddContactChecked) {
                this.ContactDataContext.Contact.SetAsPrimaryForCard = true;
                partnerArgs.Contact = this.ContactDataContext.Contact;
            }
            this.DomainService.PostPartnerAddress(partnerArgs).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result.Customer;
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    NewPotentialCustomerComponent.prototype.ValidateLocalLanguage = function (entityPM) {
        var isValid = true;
        if (!entityPM.IsLocalLanguage) {
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.EnglishName) && !Tools_1.FormatTool.IsEnglishText(entityPM.EnglishName)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address1_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address1_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address2_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address2_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.City_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.City_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ATTN_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.ATTN_Potential)) {
                isValid = false;
            }
        }
        return isValid;
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewPotentialCustomerComponent.prototype, "viewContainerRef", void 0);
    NewPotentialCustomerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewPotentialCustomerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewPotentialCustomerComponent);
    return NewPotentialCustomerComponent;
}(BaseComponent_1.BaseComponent));
exports.NewPotentialCustomerComponent = NewPotentialCustomerComponent;
var ContactItem = /** @class */ (function (_super) {
    __extends(ContactItem, _super);
    function ContactItem(contact, customer, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "Contact";
        _this.IsChecked = false;
        _this.Info1Text = null;
        _this.Info2Text = null;
        _this.loadedContact = null;
        _this.ContactAlreadyExist = false;
        _this.allCardContacts = [];
        _this.Contact = contact;
        _this.Customer = customer;
        _this.CloseContactFields(true);
        _this.SetUIProperties();
        return _this;
    }
    ContactItem.prototype.SetUIProperties = function () {
        var isRequired = false;
        if (this.IsChecked) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EnglishName)) {
                isRequired = true;
            }
        }
        if (this.father.ShowContactPart) {
            this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("EnglishName", this.ObjectTableName, isRequired);
        }
    };
    ContactItem.prototype.CloseContactFields = function (close) {
        this.IsChecked = !close;
        this.SetUIProperties();
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Position", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("BusinessPhone", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Mobile", this.ObjectTableName, this.IsChecked);
        this.UIProperties.SetEnabled("Fax", this.ObjectTableName, this.IsChecked);
    };
    Object.defineProperty(ContactItem.prototype, "Email", {
        get: function () { return this.Contact.Email; },
        set: function (newValue) {
            if (this.Contact.Email != newValue) {
                this.Contact.Email = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "EnglishName", {
        get: function () { return this.Contact.EnglishName; },
        set: function (newValue) {
            if (this.Contact.EnglishName != newValue) {
                this.Contact.EnglishName = newValue;
                this.LocalName = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "LocalName", {
        get: function () { return this.Contact.LocalName; },
        set: function (newValue) {
            if (this.Contact.LocalName != newValue) {
                this.Contact.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "Position", {
        get: function () { return this.Contact.Position; },
        set: function (newValue) {
            if (this.Contact.Position != newValue) {
                this.Contact.Position = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "Mobile", {
        get: function () { return this.Contact.Mobile; },
        set: function (newValue) {
            if (this.Contact.Mobile != newValue) {
                this.Contact.Mobile = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "Fax", {
        get: function () { return this.Contact.Fax; },
        set: function (newValue) {
            if (this.Contact.Fax != newValue) {
                this.Contact.Fax = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "BusinessPhone", {
        get: function () { return this.Contact.BusinessPhone; },
        set: function (newValue) {
            if (this.Contact.BusinessPhone != newValue) {
                this.Contact.BusinessPhone = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactItem.prototype, "Notes", {
        get: function () { return this.Contact.Notes; },
        set: function (newValue) {
            if (this.Contact.Notes != newValue) {
                this.Contact.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ContactItem.prototype.EmailLostFocus = function (email) {
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
            this.Customer.ExistedContactId = null;
            this.CloseContactFields(false);
            this.ValidateContactExist();
        }
        else {
            var domainService = new PartnersDomainService_1.PartnersDomainService();
            domainService.GetContactsByEmail(email).subscribe(function (myResult) {
                if (myResult != null) {
                    _this.loadedContact = myResult[0];
                    if (_this.loadedContact == null) {
                        _this.CloseContactFields(false);
                        _this.ContactAlreadyExist = false;
                        var index = _this.Customer.Contacts.indexOf(_this.Contact);
                        if (index == -1) {
                            _this.Customer.ExistedContactId = null;
                        }
                    }
                    else {
                        _this.Customer.ExistedContactId = _this.loadedContact.Id;
                        _this.Email = _this.loadedContact.Email;
                        _this.EnglishName = _this.loadedContact.EnglishName;
                        _this.LocalName = _this.loadedContact.LocalName;
                        _this.BusinessPhone = _this.loadedContact.BusinessPhone;
                        _this.Mobile = _this.loadedContact.Mobile;
                        _this.Fax = _this.loadedContact.Fax;
                        _this.Contact.Anniversary = _this.loadedContact.Anniversary;
                        _this.Contact.Birthday = _this.loadedContact.Birthday;
                        _this.Contact.InActive = _this.loadedContact.InActive;
                        _this.Position = _this.loadedContact.Position;
                        _this.CloseContactFields(true);
                        domainService.GetCardContactsByContact(_this.loadedContact.Id).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.allCardContacts = myResponse.Result;
                                _this.ValidateContactExist();
                                var otherCardContacts = _this.allCardContacts.filter(function (d) { return d.ContactId == _this.loadedContact.Id && d.CardId != _this.Customer.Id; });
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
            });
        }
        this.SetUIProperties();
    };
    ContactItem.prototype.ValidateContactExist = function () {
        var _this = this;
        this.ContactAlreadyExist = false;
        if (this.Customer.Id != null) {
            if (this.loadedContact != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(function (d) { return d.ContactId == _this.loadedContact.Id && d.CardId == _this.Customer.Id; }).length > 0) {
                        this.ContactAlreadyExist = true;
                    }
                }
            }
        }
        this.Info1Text = this.ContactAlreadyExist ? TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
        return this.ContactAlreadyExist;
    };
    return ContactItem;
}(BaseComponent_1.BaseComponent));
exports.ContactItem = ContactItem;
//# sourceMappingURL=NewPotentialCustomerComponent.js.map