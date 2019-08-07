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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var AddressPMService_1 = require("../../../../Common/Services/StandardPMs/AddressPMService");
var Args_1 = require("../../../../Common/Args");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var RoutingsAddEditAddressComponent = /** @class */ (function (_super) {
    __extends(RoutingsAddEditAddressComponent, _super);
    function RoutingsAddEditAddressComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Address";
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.IsNewEntity = false;
        _this.PartnerTypeId = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.country = null;
        _this.state = null;
        _this.EntityPM = new AddressPM_1.AddressPM();
        _this.myCardListService = new CardListService_1.CardListService();
        _this.myEntityPMService = new AddressPMService_1.AddressPMService();
        return _this;
    }
    RoutingsAddEditAddressComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var entityId = args['EntityId'];
        var entityPM = args['EntityPM'];
        var myCardId = args['CardId'];
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.CurrentSession.StartBusyIndicatorLoading();
            _this.myCardListService.getSingle(myCardId).subscribe(function (myResponse1) {
                var list = myResponse1.Result;
                if (list) {
                    _this.PartnerTypeId = list.PartnerTypeId;
                    _this.IsCustomer = list.IsCustomer;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(entityId)) {
                    _this.IsNewEntity = true;
                    _this.EntityPM = entityPM;
                    _this.SetUIProperties();
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.myEntityPMService.get(entityId).subscribe(function (myResponse2) {
                        if (myResponse2.HasError) {
                            _this.ValidationErrorsList = myResponse2.ErrorsArray;
                        }
                        else {
                            _this.EntityPM = myResponse2.Result;
                        }
                        _this.SetUIProperties();
                        _this.CurrentSession.StopBusyIndicator();
                    });
                }
            });
        });
    };
    RoutingsAddEditAddressComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_State();
        this.SetUIProperties_TelFax();
    };
    RoutingsAddEditAddressComponent.prototype.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    RoutingsAddEditAddressComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    RoutingsAddEditAddressComponent.prototype.SetUIProperties_StateRequired = function () {
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
    RoutingsAddEditAddressComponent.prototype.SetUIProperties_TelFax = function () {
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
    };
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "Address1", {
        get: function () { return this.EntityPM.Address1; },
        set: function (newValue) {
            if (this.EntityPM.Address1 != newValue) {
                this.EntityPM.Address1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "Address2", {
        get: function () { return this.EntityPM.Address2; },
        set: function (newValue) {
            if (this.EntityPM.Address2 != newValue) {
                this.EntityPM.Address2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "ZipCode", {
        get: function () { return this.EntityPM.ZipCode; },
        set: function (newValue) {
            if (this.EntityPM.ZipCode != newValue) {
                this.EntityPM.ZipCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "City", {
        get: function () { return this.EntityPM.City; },
        set: function (newValue) {
            if (this.EntityPM.City != newValue) {
                this.EntityPM.City = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "Country", {
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
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (newValue) {
            if (this.EntityPM.CountryCode != newValue) {
                this.EntityPM.CountryCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "CountryName", {
        get: function () { return this.EntityPM.CountryName; },
        set: function (newValue) {
            if (this.EntityPM.CountryName != newValue) {
                this.EntityPM.CountryName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM.CountryEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.CountryEnglishName != newValue) {
                this.EntityPM.CountryEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "State", {
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
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "StateId", {
        get: function () { return this.EntityPM.StateId; },
        set: function (newValue) {
            if (this.EntityPM.StateId != newValue) {
                this.EntityPM.StateId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "StateCode", {
        get: function () { return this.EntityPM.StateCode; },
        set: function (newValue) {
            if (this.EntityPM.StateCode != newValue) {
                this.EntityPM.StateCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM.StateEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.StateEnglishName != newValue) {
                this.EntityPM.StateEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM.PhoneNumber; },
        set: function (newValue) {
            if (this.EntityPM.PhoneNumber != newValue) {
                this.EntityPM.PhoneNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "FaxNumber", {
        get: function () { return this.EntityPM.FaxNumber; },
        set: function (newValue) {
            if (this.EntityPM.FaxNumber != newValue) {
                this.EntityPM.FaxNumber = newValue;
                this.SetUIProperties_TelFax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "ATTN", {
        get: function () { return this.EntityPM.ATTN; },
        set: function (newValue) {
            if (this.EntityPM.ATTN != newValue) {
                this.EntityPM.ATTN = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "IsLocalLanguage", {
        get: function () { return this.EntityPM.IsLocalLanguage; },
        set: function (newValue) {
            if (this.EntityPM.IsLocalLanguage != newValue) {
                this.EntityPM.IsLocalLanguage = newValue;
                if (this.Country != null) {
                    this.CountryName = this.EntityPM.IsLocalLanguage ? this.Country.LocalName : this.Country.EnglishName;
                }
                if (!newValue) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Description)) {
                        this.Description = this.Description.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
                        this.Name = this.Name.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Address1)) {
                        this.Address1 = this.Address1.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.Address2)) {
                        this.Address2 = this.Address2.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.City)) {
                        this.City = this.City.replace(/[^\x20-\x7F]/g, "");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ATTN)) {
                        this.ATTN = this.ATTN.replace(/[^\x20-\x7F]/g, "");
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RoutingsAddEditAddressComponent.prototype, "AddressTypeId", {
        get: function () { return this.EntityPM.AddressTypeId; },
        enumerable: true,
        configurable: true
    });
    RoutingsAddEditAddressComponent.prototype.OnCountryChanged = function (list) {
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
        this.SetUIProperties_State();
    };
    RoutingsAddEditAddressComponent.prototype.OnStateChanged = function (list) {
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
    RoutingsAddEditAddressComponent.prototype.SelectCityCommand = function () {
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
    RoutingsAddEditAddressComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RoutingsAddEditAddressComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.IsCustomer) {
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
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.IsNewEntity) {
                this.myEntityPMService.insert(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
            else {
                this.myEntityPMService.update(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    };
    RoutingsAddEditAddressComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './RoutingsAddEditAddressComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], RoutingsAddEditAddressComponent);
    return RoutingsAddEditAddressComponent;
}(BaseComponent_1.BaseComponent));
exports.RoutingsAddEditAddressComponent = RoutingsAddEditAddressComponent;
//# sourceMappingURL=RoutingsAddEditAddressComponent.js.map