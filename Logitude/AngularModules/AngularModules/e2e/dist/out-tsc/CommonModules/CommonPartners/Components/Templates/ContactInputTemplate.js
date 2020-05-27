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
var ContactPM_1 = require("../../../../Common/EntityPMs/ContactPM");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ContactInputTemplate = /** @class */ (function (_super) {
    __extends(ContactInputTemplate, _super);
    function ContactInputTemplate() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Contact";
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.CardId = null;
        _this.IsNewEntity = false;
        _this.IsCustomerVisible = false;
        _this.CardDependencyProperty1 = "CS";
        _this.CustomerLable = "Customer";
        _this.DependencyFilter1IsList = false;
        _this.ShowSearchContacts = false;
        _this.ShowSecondPartOfWindow = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.LoadEntityCompletedEvent = null;
        _this.IsEditingEnabled = false;
        _this.IsEditingEmailEnabled = false;
        _this.IsBlockingUnifreightCustomer = false;
        // Products
        _this.IsProductsVisible = false;
        _this.Info1Text = null;
        _this.Info2Text = null;
        _this.loadedContact = null;
        _this.allCardContacts = [];
        _this.EntityPM = new ContactPM_1.ContactPM();
        _this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.Listen();
        return _this;
    }
    ContactInputTemplate.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null && this.CurrentSession.CurrentEditComponent.ObjectTableName == "Contact") {
            if (this.LoadEntityCompletedEvent == null) {
                this.LoadEntityCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    ContactInputTemplate.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.LoadEntityCompletedEvent);
    };
    ContactInputTemplate.prototype.CopyDomain = function () {
        var selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = this.Email;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
    };
    ContactInputTemplate.prototype.InitTemplate = function (args) {
        this.args = args;
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        this.IsCustomerVisible = args.IsCustomerVisible;
        this.CardId = args.CardId;
        this.ShowSearchContacts = args.ShowSearchContacts;
        if (!Tools_1.AppTool.IsNullOrEmpty(args.CustomerId)) {
            this.CustomerId = args.CustomerId;
        }
        if (args.ComponentName == "Ticket") {
            this.CardId = args.CustomerId;
            this.EntityPM.CustomerId = args.CustomerId;
            this.CardDependencyProperty1 = args.CustomerId;
            this.CustomerLable = args.CustomerLable;
            this.DependencyFilter1IsList = args.CardDependencyProperty1IsList;
            this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        }
        if (args.ShowSecondPartOfWindow == "yes") {
            this.ShowSecondPartOfWindow = false;
        }
        else {
            this.ShowSecondPartOfWindow = true;
        }
        this.SetUIProperties();
    };
    ContactInputTemplate.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = true;
        this.IsEditingEmailEnabled = true;
        if (!this.IsNewEntity) {
            if (this.args.BlockEditingEmail) {
                this.IsEditingEmailEnabled = false;
            }
        }
        //this.IsBlockingUnifreightCustomer = this.fatherComponent.IsBlockingUnifreightCustomer;
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, this.IsEditingEmailEnabled);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Position", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("BusinessPhone", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Mobile", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Fax", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Birthday", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Anniversary", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNewEntity);
        //this.UIProperties.SetEnabled("IsAll", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsAirExport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsAirImport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsOceanExport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsOceanImport", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsInlandDomestic", this.ObjectTableName, this.IsEditingEnabled);
        //this.UIProperties.SetEnabled("IsCustomsImport", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_Dates();
    };
    ContactInputTemplate.prototype.SetUIProperties_Dates = function () {
        var isBirthdayReminderEnabled = this.IsEditingEnabled;
        var isAnniversaryReminderEnabled = this.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            if (this.Birthday == null) {
                isBirthdayReminderEnabled = false;
            }
            if (this.Anniversary == null) {
                isAnniversaryReminderEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("BirthdayReminder", this.ObjectTableName, isBirthdayReminderEnabled);
        this.UIProperties.SetEnabled("AnniversaryReminder", this.ObjectTableName, isAnniversaryReminderEnabled);
    };
    ContactInputTemplate.prototype.SelectEmailClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = this;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Contact") + " Search";
        ;
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/SearchContactsComponent');
    };
    Object.defineProperty(ContactInputTemplate.prototype, "Email", {
        // Properties
        get: function () { return this.EntityPM.Email; },
        set: function (newValue) {
            if (this.EntityPM.Email != newValue) {
                this.EntityPM.Email = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
                this.LocalName = this.EntityPM.EnglishName;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Position", {
        get: function () { return this.EntityPM.Position; },
        set: function (newValue) {
            if (this.EntityPM.Position != newValue) {
                this.EntityPM.Position = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "BusinessPhone", {
        get: function () { return this.EntityPM.BusinessPhone; },
        set: function (newValue) {
            if (this.EntityPM.BusinessPhone != newValue) {
                this.EntityPM.BusinessPhone = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Mobile", {
        get: function () { return this.EntityPM.Mobile; },
        set: function (newValue) {
            if (this.EntityPM.Mobile != newValue) {
                this.EntityPM.Mobile = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Fax", {
        get: function () { return this.EntityPM.Fax; },
        set: function (newValue) {
            if (this.EntityPM.Fax != newValue) {
                this.EntityPM.Fax = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Birthday", {
        get: function () { return this.EntityPM.Birthday; },
        set: function (newValue) {
            if (this.EntityPM.Birthday != newValue) {
                this.EntityPM.Birthday = newValue;
                this.SetUIProperties_Dates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Anniversary", {
        get: function () { return this.EntityPM.Anniversary; },
        set: function (newValue) {
            if (this.EntityPM.Anniversary != newValue) {
                this.EntityPM.Anniversary = newValue;
                this.SetUIProperties_Dates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "BirthdayReminder", {
        get: function () { return this.EntityPM.BirthdayReminder; },
        set: function (newValue) {
            if (this.EntityPM.BirthdayReminder != newValue) {
                this.EntityPM.BirthdayReminder = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "AnniversaryReminder", {
        get: function () { return this.EntityPM.AnniversaryReminder; },
        set: function (newValue) {
            if (this.EntityPM.AnniversaryReminder != newValue) {
                this.EntityPM.AnniversaryReminder = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerId != newValue) {
                this.EntityPM.CustomerId = newValue;
                this.CardId = newValue;
                this.ValidateContactExist();
            }
        },
        enumerable: true,
        configurable: true
    });
    ContactInputTemplate.prototype.SetIsProductsVisible = function () {
        var isProductsVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Contact", "ACTIVEPRODUCTTYPES")) {
            if (this.HasCardContact) {
                isProductsVisible = true;
            }
        }
        this.IsProductsVisible = isProductsVisible;
    };
    Object.defineProperty(ContactInputTemplate.prototype, "IsAll", {
        get: function () { return this.EntityPM.IsAll; },
        set: function (newValue) {
            if (this.EntityPM.IsAll != newValue) {
                this.EntityPM.IsAll = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsAirExport", {
        get: function () { return this.EntityPM.IsAirExport; },
        set: function (newValue) {
            if (this.EntityPM.IsAirExport != newValue) {
                this.EntityPM.IsAirExport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsAirImport", {
        get: function () { return this.EntityPM.IsAirImport; },
        set: function (newValue) {
            if (this.EntityPM.IsAirImport != newValue) {
                this.EntityPM.IsAirImport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsInlandDomestic", {
        get: function () { return this.EntityPM.IsInlandDomestic; },
        set: function (newValue) {
            if (this.EntityPM.IsInlandDomestic != newValue) {
                this.EntityPM.IsInlandDomestic = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsCustomsImport", {
        get: function () { return this.EntityPM.IsCustomsImport; },
        set: function (newValue) {
            if (this.EntityPM.IsCustomsImport != newValue) {
                this.EntityPM.IsCustomsImport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsOceanExport", {
        get: function () { return this.EntityPM.IsOceanExport; },
        set: function (newValue) {
            if (this.EntityPM.IsOceanExport != newValue) {
                this.EntityPM.IsOceanExport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "IsOceanImport", {
        get: function () { return this.EntityPM.IsOceanImport; },
        set: function (newValue) {
            if (this.EntityPM.IsOceanImport != newValue) {
                this.EntityPM.IsOceanImport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactInputTemplate.prototype, "HasCardContact", {
        get: function () { return this.EntityPM.HasCardContact; },
        set: function (newValue) {
            if (this.EntityPM.HasCardContact != newValue) {
                this.EntityPM.HasCardContact = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ContactInputTemplate.prototype.EmailLostFocus = function (email) {
        var _this = this;
        this.Info1Text = null;
        this.Info2Text = null;
        this.loadedContact = null;
        this.allCardContacts = [];
        this.ValidateContactExist();
        if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
            if (email.indexOf('@') > -1 && email.indexOf('.') > -1) {
                this.DomainService.GetContactsByEmail(email).subscribe(function (myResult) {
                    if (myResult != null) {
                        _this.loadedContact = myResult[0];
                        if (_this.loadedContact != null) {
                            _this.EnglishName = _this.loadedContact.EnglishName;
                            _this.LocalName = _this.loadedContact.LocalName;
                            _this.Email = _this.loadedContact.Email;
                            _this.BusinessPhone = _this.loadedContact.BusinessPhone;
                            _this.Mobile = _this.loadedContact.Mobile;
                            _this.Fax = _this.loadedContact.Fax;
                            _this.Position = _this.loadedContact.Position;
                            _this.Birthday = _this.loadedContact.Birthday;
                            _this.Anniversary = _this.loadedContact.Anniversary;
                            _this.BirthdayReminder = _this.loadedContact.BirthdayReminder;
                            _this.AnniversaryReminder = _this.loadedContact.AnniversaryReminder;
                            _this.InActive = _this.loadedContact.InActive;
                            _this.IsAirImport = _this.loadedContact.IsAirImport;
                            _this.IsAirExport = _this.loadedContact.IsAirExport;
                            _this.IsAll = _this.loadedContact.IsAll;
                            _this.IsInlandDomestic = _this.loadedContact.IsInlandDomestic;
                            _this.IsCustomsImport = _this.loadedContact.IsCustomsImport;
                            _this.IsOceanExport = _this.loadedContact.IsOceanExport;
                            _this.IsOceanImport = _this.loadedContact.IsOceanImport;
                            _this.HasCardContact = _this.loadedContact.HasCardContact;
                            _this.SetIsProductsVisible();
                            if (!Tools_1.AppTool.IsNullOrEmpty(_this.CardId)) {
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
                            else {
                                _this.ValidateContactExist();
                            }
                        }
                    }
                });
            }
        }
    };
    ContactInputTemplate.prototype.Validate = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (!Tools_1.FormatTool.IsEmail(this.Email)) {
            errors.push("Invalid email format!");
        }
        if (this.ValidateContactExist()) {
            if (this.CardId != null) {
                errors.push("This Contact is Already added for you");
            }
            else {
                errors.push("Sorry this contact already exists!");
            }
        }
        return errors;
    };
    ContactInputTemplate.prototype.ValidateContactExist = function () {
        var _this = this;
        var isContactAlreadyExist = false;
        if (this.loadedContact != null) {
            if (this.CardId != null) {
                if (this.allCardContacts != null) {
                    if (this.allCardContacts.filter(function (d) { return d.ContactId == _this.loadedContact.Id && d.CardId == _this.CardId; }).length > 0) {
                        isContactAlreadyExist = true;
                    }
                }
                this.Info1Text = isContactAlreadyExist ? TextCodeTranslator_1.TextCodeTranslator.Translate("Contact.M.ContactAddedForYou") : null;
            }
            else {
                isContactAlreadyExist = true;
            }
        }
        return isContactAlreadyExist;
    };
    ContactInputTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContactInputTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContactInputTemplate);
    return ContactInputTemplate;
}(BaseComponent_1.BaseComponent));
exports.ContactInputTemplate = ContactInputTemplate;
var ContactInputTemplateArgs = /** @class */ (function () {
    function ContactInputTemplateArgs() {
        this.IsNewEntity = false;
        this.EntityPM = null;
        this.CardId = null;
        this.IsCustomerVisible = false;
        this.CardDependencyProperty1 = null;
        this.CustomerId = null;
        this.CustomerLable = null;
        this.CardDependencyProperty1IsList = false;
        this.ComponentName = null;
        this.BlockEditingEmail = false;
        this.ShowSearchContacts = false;
        this.ShowSecondPartOfWindow = null;
    }
    return ContactInputTemplateArgs;
}());
exports.ContactInputTemplateArgs = ContactInputTemplateArgs;
//# sourceMappingURL=ContactInputTemplate.js.map