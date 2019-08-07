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
var CustomerProductPM_1 = require("../../../../Common/EntityPMs/CustomerProductPM");
var CustomerAdditionalServicePM_1 = require("../../../../Common/EntityPMs/CustomerAdditionalServicePM");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var AdditionalServiceListService_1 = require("../../../../Common/Services/StandardLists/AdditionalServiceListService");
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
var QuestionnairePMService_1 = require("../../../../CRM/Services/StandardPMs/QuestionnairePMService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var ReadyForActivationComponent = /** @class */ (function (_super) {
    __extends(ReadyForActivationComponent, _super);
    function ReadyForActivationComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.ProductsList = [];
        _this.ServicesList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.country = null;
        _this.oldProducts = [];
        _this.partnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.customerService = new CustomerPMService_1.CustomerPMService;
        _this._QuestionnairePMService = new QuestionnairePMService_1.QuestionnairePMService();
        return _this;
    }
    ReadyForActivationComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.Message = args.Message;
        this.SetUIProperties();
        this.LoadPickupDeliveryAddress();
        this.CheckIfVatUnique();
        this.RunAdditionalFieldsComponent();
        this.BuildProductsList();
        this.BuildServicesList();
        this.Clone();
    };
    ReadyForActivationComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Others();
        this.SetUIProperties_VAT();
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    };
    ReadyForActivationComponent.prototype.SetUIProperties_Others = function () {
        this.UIProperties.SetRequired("City_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.City_Potential));
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerAddress1Required) {
                this.UIProperties.SetRequired("Address1_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Address1_Potential));
            }
        }
    };
    ReadyForActivationComponent.prototype.SetUIProperties_VAT = function () {
        if (this.EntityPM.IsCustomer) {
            var args = new VatNumberValidator_1.VATValidatorArgs();
            args.VATNumber = this.VatNumber;
            args.IsCustomer = this.EntityPM.IsCustomer;
            args.PartnerTypeId = this.EntityPM.PartnerTypeId;
            args.CountryId = this.CountryId_Potential;
            args.CountryName = this.CountryName;
            args.CountryEnglishName = this.CountryEnglishName;
            args.SetReady = true;
            VatNumberValidator_1.VatNumberValidator.ValidateVatFormat(args);
            VatNumberValidator_1.VatNumberValidator.ValidateVatMandatory(args);
            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, args.Errors.length > 0 ? true : false);
        }
    };
    ReadyForActivationComponent.prototype.SetUIProperties_State = function () {
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
    ReadyForActivationComponent.prototype.SetUIProperties_TelFax = function () {
        if (this.EntityPM.IsCustomer) {
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                this.UIProperties.SetRequired("PhoneNumber_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PhoneNumber_Potential));
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                this.UIProperties.SetRequired("FaxNumber_Potential", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FaxNumber_Potential));
            }
        }
    };
    ReadyForActivationComponent.prototype.BuildProductsList = function () {
        var _this = this;
        this.ProductsList = [];
        var _productTypeListService = new ProductTypeListService_1.ProductTypeListService();
        _productTypeListService.getAllFromCache().subscribe(function (result) {
            var fullProductsList = result.Result;
            fullProductsList.filter(function (i) { return i.InActive == false; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                _this.ProductsList.push(new ProductTypeItemClass(item, _this.EntityPM));
            });
        });
    };
    ReadyForActivationComponent.prototype.BuildServicesList = function () {
        var _this = this;
        this.ServicesList = [];
        var service = new AdditionalServiceListService_1.AdditionalServiceListService();
        service.getAllFromCache().subscribe(function (result) {
            var fullServicesList = result.Result;
            fullServicesList.filter(function (i) { return i.InActive == false; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                _this.ServicesList.push(new ServiceItemClass(item, _this.EntityPM));
            });
        });
    };
    ReadyForActivationComponent.prototype.LoadPickupDeliveryAddress = function () {
        var _this = this;
        this.partnersDomainService.GetAddressByCardAndType(this.EntityPM.Id, "P").subscribe(function (myResponse) {
            _this.PickupAddress = myResponse;
        });
    };
    ReadyForActivationComponent.prototype.CheckIfVatUnique = function () {
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
    ReadyForActivationComponent.prototype.RunAdditionalFieldsComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ReadyForActivationComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunAdditionalFieldsComponent(); }, 1);
        }
    };
    ReadyForActivationComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, "Customer.AdditionalFields");
        });
    };
    Object.defineProperty(ReadyForActivationComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "Address1_Potential", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "Address2_Potential", {
        get: function () { return this.EntityPM.Address2_Potential; },
        set: function (newValue) {
            if (this.EntityPM.Address2_Potential != newValue) {
                this.EntityPM.Address2_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "ZipCode_Potential", {
        get: function () { return this.EntityPM.ZipCode_Potential; },
        set: function (newValue) {
            if (this.EntityPM.ZipCode_Potential != newValue) {
                this.EntityPM.ZipCode_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "City_Potential", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "CountryId_Potential", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "Country", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "StateId_Potential", {
        get: function () { return this.EntityPM.StateId_Potential; },
        set: function (newValue) {
            if (this.EntityPM.StateId_Potential != newValue) {
                this.EntityPM.StateId_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "VatNumber", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "PhoneNumber_Potential", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "FaxNumber_Potential", {
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
    Object.defineProperty(ReadyForActivationComponent.prototype, "ATTN_Potential", {
        get: function () { return this.EntityPM.ATTN_Potential; },
        set: function (newValue) {
            if (this.EntityPM.ATTN_Potential != newValue) {
                this.EntityPM.ATTN_Potential = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "CustomerSizeId", {
        get: function () { return this.EntityPM.CustomerSizeId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerSizeId != newValue) {
                this.EntityPM.CustomerSizeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReadyForActivationComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ReadyForActivationComponent.prototype.OnCountryChanged = function (list) {
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
    ReadyForActivationComponent.prototype.SelectCityCommand = function () {
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
    ReadyForActivationComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ReadyForActivationComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.CustomerProducts.forEach(function (item) {
            var productItem = new CustomerProductPM_1.CustomerProductPM(null);
            productItem.CommitmentChargeableWeight = item.CommitmentChargeableWeight;
            productItem.CommitmentNumberOfShipments = item.CommitmentNumberOfShipments;
            productItem.CommitmentRevenue = item.CommitmentRevenue;
            productItem.CommitmentTEU = item.CommitmentTEU;
            productItem.CustomerId = item.CustomerId;
            productItem.LastShipmentDate = item.LastShipmentDate;
            productItem.Notes = item.Notes;
            productItem.PotentialChargeableWeight = item.PotentialChargeableWeight;
            productItem.PotentialNumberOfShipments = item.PotentialNumberOfShipments;
            productItem.PotentialRevenue = item.PotentialRevenue;
            productItem.PotentialTEU = item.PotentialTEU;
            productItem.PrepaidCollectId = item.PrepaidCollectId;
            productItem.ProductTypeCode = item.ProductTypeCode;
            _this.oldProducts.push(productItem);
        });
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
    ReadyForActivationComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldProducts.forEach(function (item) {
            var existingItem = _this.EntityPM.CustomerProducts.filter(function (f) { return f == item; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.CustomerProducts.forEach(function (item) {
            var oldItem = _this.oldProducts.filter(function (f) { return f == item; })[0];
            if (oldItem) {
            }
            else {
                addedItems.push(item);
            }
        });
        addedItems.forEach(function (item) {
            _this.EntityPM.RemoveCustomerProductPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.AddCustomerProductPM(item);
        });
        this.myCloner.RejectChanges();
    };
    ReadyForActivationComponent.prototype.OkButtonClicked = function () {
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
            args.SetReady = true;
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
    ReadyForActivationComponent.prototype.DoActivation = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.DefaultQuestionnaireId) && this.EntityPM.IsCustomer) {
            this.ShowQuestionnaire();
        }
        else {
            this.EntityPM.SetActivated = false;
            this.EntityPM.SetReady = true;
            this.Save("Activated");
        }
    };
    ReadyForActivationComponent.prototype.ShowQuestionnaire = function () {
        var _this = this;
        this._QuestionnairePMService.get(SessionLocator_1.SessionLocator.TenantPM.DefaultQuestionnaireId).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var result = response.Result;
            if (result) {
                logWindow.Title = result.Name;
                logWindow.Width = 1000;
                logWindow.Height = 800;
                var windowArgs = {};
                windowArgs.ObjectTableId = window.ObjectTables.filter(function (f) { return f.Name === _this.ObjectTableName; })[0].Id;
                windowArgs.EntityId = _this.EntityPM.Id;
                windowArgs.EntityPM = result;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./CRMModules/CRMOthers/Components/Questionnaire/QuestionnaireAnswersComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event === "cancelled") {
                        _this.ValidationErrorsList.push("The customer questionnaire must be answerd before saving.");
                    }
                    else {
                        _this.EntityPM.SetActivated = false;
                        _this.EntityPM.SetReady = true;
                        _this.Save("Activated");
                    }
                });
            }
        });
    };
    ReadyForActivationComponent.prototype.Save = function (msg) {
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
    ], ReadyForActivationComponent.prototype, "viewContainerRef", void 0);
    ReadyForActivationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReadyForActivationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ReadyForActivationComponent);
    return ReadyForActivationComponent;
}(BaseComponent_1.BaseComponent));
exports.ReadyForActivationComponent = ReadyForActivationComponent;
var ProductTypeItemClass = /** @class */ (function () {
    function ProductTypeItemClass(itemList, itemPM) {
        var _this = this;
        this.entityPM = itemPM;
        this.entityList = itemList;
        var isChecked = null;
        var productPM = this.entityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == _this.entityList.Code; })[0];
        this.isChecked = false;
        if (productPM != null) {
            this.isChecked = true;
        }
    }
    Object.defineProperty(ProductTypeItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Code", {
        get: function () { return this.entityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#6E7172" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "DirectionId", {
        get: function () { return this.entityList.Code.substr(1, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "TransportModeId", {
        get: function () { return this.entityList.Code.substr(0, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var notesRightToLeft = false;
                    if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                        notesRightToLeft = true;
                    }
                    var type = null;
                    var productTypeListService = new ProductTypeListService_1.ProductTypeListService();
                    productTypeListService.getSingleFromCache(this.entityList.Id).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                type = list.Name;
                            }
                        }
                    });
                    var newItem = new CustomerProductPM_1.CustomerProductPM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.CustomerId = this.entityPM.Id;
                    newItem.ProductTypeCode = this.Code;
                    newItem.CommitmentChargeableWeight = 0;
                    newItem.PotentialChargeableWeight = 0;
                    newItem.CommitmentTEU = 0;
                    newItem.PotentialTEU = 0;
                    newItem.CommitmentNumberOfShipments = 0;
                    newItem.PotentialNumberOfShipments = 0;
                    newItem.CommitmentRevenue = 0;
                    newItem.PotentialRevenue = 0;
                    newItem.NotesRightToLeft = notesRightToLeft;
                    newItem.ProductTypeName = type;
                    var itemIndex = this.entityPM.CustomerProducts.indexOf(newItem);
                    if (itemIndex == -1) {
                        this.entityPM.AddCustomerProductPM(newItem);
                    }
                }
                else {
                    var item = this.entityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == _this.Code; })[0];
                    if (item != null) {
                        var itemIndex = this.entityPM.CustomerProducts.indexOf(item);
                        if (itemIndex > -1) {
                            this.entityPM.RemoveCustomerProductPM(item);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return ProductTypeItemClass;
}());
exports.ProductTypeItemClass = ProductTypeItemClass;
var ServiceItemClass = /** @class */ (function () {
    function ServiceItemClass(itemList, itemPM) {
        var _this = this;
        this.entityPM = itemPM;
        this.entityList = itemList;
        var isChecked = null;
        var servicePM = this.entityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.entityList.Id; })[0];
        this.isChecked = false;
        if (servicePM != null) {
            this.isChecked = true;
        }
    }
    Object.defineProperty(ServiceItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#6E7172" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var notesRightToLeft = false;
                    if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                        notesRightToLeft = true;
                    }
                    var name = null;
                    var service = new AdditionalServiceListService_1.AdditionalServiceListService();
                    service.getSingleFromCache(this.entityList.Id).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                name = list.Name;
                            }
                        }
                    });
                    var newItem = new CustomerAdditionalServicePM_1.CustomerAdditionalServicePM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.CustomerId = this.entityPM.Id;
                    newItem.AdditionalServiceId = this.entityList.Id;
                    newItem.AdditionalServiceName = name;
                    var itemIndex = this.entityPM.CustomerAdditionalServices.indexOf(newItem);
                    if (itemIndex == -1) {
                        this.entityPM.AddCustomerAdditionalServicePM(newItem);
                    }
                }
                else {
                    var item = this.entityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.entityList.Id; })[0];
                    if (item != null) {
                        var itemIndex = this.entityPM.CustomerAdditionalServices.indexOf(item);
                        if (itemIndex > -1) {
                            this.entityPM.RemoveCustomerAdditionalServicePM(item);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return ServiceItemClass;
}());
exports.ServiceItemClass = ServiceItemClass;
//# sourceMappingURL=ReadyForActivationComponent.js.map