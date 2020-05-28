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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TenantPM_1 = require("../../../../Common/EntityPMs/TenantPM");
var AgentPM_1 = require("../../../../Common/EntityPMs/AgentPM");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var Args_1 = require("../../../../Common/Args");
var AddressPMService_1 = require("../../../../Common/Services/StandardPMs/AddressPMService");
var AgentPMService_1 = require("../../../../Common/Services/StandardPMs/AgentPMService");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var CompanyAddressSettingsComponent = /** @class */ (function (_super) {
    __extends(CompanyAddressSettingsComponent, _super);
    function CompanyAddressSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.TenantPm = new TenantPM_1.TenantPM();
        _this.TenantAgent = new AgentPM_1.AgentPM();
        _this.TenantAddress = new AddressPM_1.AddressPM();
        _this.LocalTenantAddress = new AddressPM_1.AddressPM();
        _this.DataContext = _this;
        _this.AddressObjectTableName = "Address";
        _this.AgentObjectTableName = "Agent";
        _this.TenantObjectTableName = "Tenant";
        _this.IsVisibile = false;
        _this.IsLocalAddressTabVisible = false;
        _this.SelectedTabCode = "0";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //Tenant 65
        _this.DemoMessageVisibility = false;
        _this.state = null;
        _this.country = null;
        return _this;
    }
    CompanyAddressSettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(function (response2) {
                _this.LoadTenantPMMethod();
            });
        });
    };
    CompanyAddressSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            var pmResponse = response;
            if (!pmResponse.HasError) {
                _this.TenantPm = response.Result;
                _this.LoadAddressPM();
                _this.GetDemoMessageVisibility();
                if (_this.TenantPm.Id == 65 && SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
                    _this.SetUIPropertiesHitVisible();
                }
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLocalAddress")) {
                    _this.LoadLocalAddress();
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    CompanyAddressSettingsComponent.prototype.LoadAddressPM = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.TenantPm.AddressId)) {
            this.TenantAddress = new AddressPM_1.AddressPM();
            this.TenantAddress.Tenant = this.TenantPm.Id;
            this.TenantAddress.AddressTypeId = "M";
            this.InitializeData();
            this.IsVisibile = true;
        }
        else {
            var myService = new AddressPMService_1.AddressPMService();
            myService.get(this.TenantPm.AddressId).subscribe(function (myResult) {
                _this.TenantAddress = myResult.Result;
                _this.InitializeData();
                _this.IsVisibile = true;
            });
        }
    };
    CompanyAddressSettingsComponent.prototype.LoadLocalAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.TenantPm.LocalAddressId)) {
            this.LocalTenantAddress = new AddressPM_1.AddressPM();
            this.LocalTenantAddress.Tenant = this.TenantPm.Id;
            this.LocalTenantAddress.AddressTypeId = "L";
            this.LocalAddressDataContext = new AddressItem(this.LocalTenantAddress, this.TenantPm, this);
            this.IsLocalAddressTabVisible = true;
        }
        else {
            var myService = new AddressPMService_1.AddressPMService();
            myService.get(this.TenantPm.LocalAddressId).subscribe(function (myResult) {
                _this.LocalTenantAddress = myResult.Result;
                _this.LocalAddressDataContext = new AddressItem(_this.LocalTenantAddress, _this.TenantPm, _this);
                _this.IsLocalAddressTabVisible = true;
            });
        }
    };
    CompanyAddressSettingsComponent.prototype.InitializeData = function () {
        var _this = this;
        if (this.TenantAddress != null) {
            var countryListService = new CountryListService_1.CountryListService();
            countryListService.getAllFromCache().subscribe(function (result) {
                _this.SetUIProperties_State();
                if (Tools_1.AppTool.IsNullOrEmpty(_this.TenantAddress.Id)) {
                    _this.TenantAddress.Name = _this.TenantPm.Company;
                    _this.TenantAddress.Description = _this.TenantPm.Company;
                    _this.TenantAddress.PhoneNumber = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessPhone;
                    _this.TenantAgent = new AgentPM_1.AgentPM();
                    _this.TenantAgent.Code = "new";
                    _this.TenantAgent.PartnerTypeId = "AG";
                    _this.TenantAgent.Tenant = 1;
                    _this.TenantAgent.EnglishName = "new";
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.TenantPm.AgentId)) {
                        _this.TenantAgent = new AgentPM_1.AgentPM();
                        _this.TenantAgent = new AgentPM_1.AgentPM();
                        _this.TenantAgent.Code = "new";
                        _this.TenantAgent.PartnerTypeId = "AG";
                        _this.TenantAgent.Tenant = 1;
                        _this.TenantAgent.EnglishName = "new";
                        _this.TenantAgent.TenantAddressId = _this.TenantAddress.Id;
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.TenantPm.AgentId)) {
                    _this.LoadTenantAgentMethod();
                }
            });
        }
    };
    CompanyAddressSettingsComponent.prototype.SetUIPropertiesHitVisible = function () {
        this.UIProperties.SetEnabled("Company", this.TenantObjectTableName, false);
        this.UIProperties.SetEnabled("Address1", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("Address2", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("City", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("CountryId", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("ZipCode", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("Signature", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("PhoneNumber", this.AddressObjectTableName, false);
        this.UIProperties.SetEnabled("FaxNumber", this.AddressObjectTableName, false);
    };
    // Cach Lists 
    CompanyAddressSettingsComponent.prototype.LoadTenantAgentMethod = function () {
        var _this = this;
        var myService = new AgentPMService_1.AgentPMService();
        myService.get(this.TenantPm.AgentId).subscribe(function (myResult) {
            if (myResult) {
                _this.TenantAgent = myResult.Result;
            }
        });
    };
    CompanyAddressSettingsComponent.prototype.GetDemoMessageVisibility = function () {
        var result = false;
        if (this.TenantPm.Id == 65) {
            result = true;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                result = false;
            }
        }
        this.DemoMessageVisibility = result;
    };
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Company", {
        // Properties 
        get: function () { return this.TenantPm.Company; },
        set: function (value) {
            if (this.TenantPm.Company != value) {
                this.TenantPm.Company = value;
                this.TenantAddress.Name = value;
                this.TenantAgent.EnglishName = value;
                this.TenantAddress.Description = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("Name", "Address", true);
                }
                else {
                    this.UIProperties.SetRequired("Name", "Address", false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Name", {
        get: function () {
            return this.TenantAddress.Name;
        },
        set: function (value) {
            if (this.TenantAddress.Name != value) {
                this.TenantAddress.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Address1", {
        get: function () {
            return this.TenantAddress.Address1;
        },
        set: function (value) {
            if (this.TenantAddress.Address1 != value) {
                this.TenantAddress.Address1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Address2", {
        get: function () {
            return this.TenantAddress.Address2;
        },
        set: function (value) {
            if (this.TenantAddress.Address2 != value) {
                this.TenantAddress.Address2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Signature", {
        get: function () {
            return this.TenantPm.Signature;
        },
        set: function (value) {
            if (this.TenantPm.Signature != value) {
                this.TenantPm.Signature = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "City", {
        get: function () {
            return this.TenantAddress.City;
        },
        set: function (value) {
            if (this.TenantAddress.City != value) {
                this.TenantAddress.City = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("City", "Address", true);
                }
                else {
                    this.UIProperties.SetRequired("City", "Address", false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "FaxNumber", {
        get: function () {
            return this.TenantAddress.FaxNumber;
        },
        set: function (value) {
            if (this.TenantAddress.FaxNumber != value) {
                this.TenantAddress.FaxNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "ZipCode", {
        get: function () {
            return this.TenantAddress.ZipCode;
        },
        set: function (value) {
            if (this.TenantAddress.ZipCode != value) {
                this.TenantAddress.ZipCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "PhoneNumber", {
        get: function () {
            return this.TenantAddress.PhoneNumber;
        },
        set: function (value) {
            if (this.TenantAddress.PhoneNumber != value) {
                this.TenantAddress.PhoneNumber = value;
                SessionLocator_1.SessionLocator.LoggedUserPM.BusinessPhone = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "State", {
        get: function () { return this.state; },
        set: function (value) {
            if (this.state != value) {
                this.state = value;
                this.OnStateChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "StateId", {
        get: function () { return this.TenantAddress.StateId; },
        set: function (value) {
            if (this.TenantAddress.StateId != value) {
                this.TenantAddress.StateId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "StateCode", {
        get: function () { return this.TenantAddress.StateCode; },
        set: function (newValue) {
            if (this.TenantAddress.StateCode != newValue) {
                this.TenantAddress.StateCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "StateEnglishName", {
        get: function () { return this.TenantAddress.StateEnglishName; },
        set: function (newValue) {
            if (this.TenantAddress.StateEnglishName != newValue) {
                this.TenantAddress.StateEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "Country", {
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
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "CountryId", {
        get: function () { return this.TenantAddress.CountryId; },
        set: function (value) {
            if (this.TenantAddress.CountryId != value) {
                this.TenantAddress.CountryId = value;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "CountryCode", {
        get: function () { return this.TenantAddress.CountryCode; },
        set: function (value) {
            if (this.TenantAddress.CountryCode != value) {
                this.TenantAddress.CountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "CountryName", {
        get: function () { return this.TenantAddress.CountryName; },
        set: function (value) {
            if (this.TenantAddress.CountryName != value) {
                this.TenantAddress.CountryName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyAddressSettingsComponent.prototype, "CountryEnglishName", {
        get: function () { return this.TenantAddress.CountryEnglishName; },
        set: function (value) {
            if (this.TenantAddress.CountryEnglishName != value) {
                this.TenantAddress.CountryEnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CompanyAddressSettingsComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryName = this.TenantAddress.IsLocalLanguage ? list.LocalName : list.EnglishName;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_State();
    };
    CompanyAddressSettingsComponent.prototype.OnStateChanged = function (list) {
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
    CompanyAddressSettingsComponent.prototype.SetUIProperties_State = function () {
        if (this.TenantAddress != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    };
    CompanyAddressSettingsComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", "Address", isEnabled);
    };
    CompanyAddressSettingsComponent.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("StateId", "Address", isRequired);
    };
    // Commands 
    CompanyAddressSettingsComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.TenantAddress.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    CompanyAddressSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CompanyAddressSettingsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.TenantAddress, this.DataContext.AddressObjectTableName, errors);
        Validator_1.Validator.TryValidateObject(this.TenantAgent, this.DataContext.AgentObjectTableName, errors);
        Validator_1.Validator.TryValidateObject(this.TenantPm, this.DataContext.TenantObjectTableName, errors);
        if (this.LocalAddressDataContext != null) {
            Validator_1.Validator.TryValidateObject(this.LocalAddressDataContext.Address, this.LocalAddressDataContext.ObjectTableName, errors);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (Tools_1.AppTool.IsNullOrEmpty(this.TenantAddress.Id)) {
                this.SubmitCreatingAgent();
            }
            else {
                this.SubmitUpdatingAgent();
            }
        }
    };
    CompanyAddressSettingsComponent.prototype.SubmitCreatingAgent = function () {
        var _this = this;
        var myService = new AgentPMService_1.AgentPMService();
        myService.insert(this.TenantAgent).subscribe(function (myRespone) {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    _this.OnSaveAgentCompletedSuccessfully();
                }
                else {
                    _this.ValidationErrorsList = myRespone.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    CompanyAddressSettingsComponent.prototype.SubmitUpdatingAgent = function () {
        var _this = this;
        var myService = new AgentPMService_1.AgentPMService();
        myService.update(this.TenantAgent).subscribe(function (myRespone) {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    _this.OnSaveAgentCompletedSuccessfully();
                }
                else {
                    _this.ValidationErrorsList = myRespone.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    CompanyAddressSettingsComponent.prototype.SubmitUpdatingAddress = function (type) {
        var _this = this;
        var address = null;
        if (type == "M") {
            address = this.TenantAddress;
        }
        else if (type == "L") {
            address = this.LocalTenantAddress;
        }
        if (address != null && address.IsDirty) {
            var myService = new AddressPMService_1.AddressPMService();
            myService.update(address).subscribe(function (myRespone) {
                if (myRespone != null) {
                    if (!myRespone.HasError) {
                        _this.SubmitTenantChanges();
                    }
                    else {
                        _this.ValidationErrorsList = myRespone.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
        else if (this.TenantPm.IsDirty) {
            this.SubmitTenantChanges();
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    CompanyAddressSettingsComponent.prototype.SubmitCreatingAddress = function (type) {
        var _this = this;
        var address = null;
        if (type == "M") {
            address = this.TenantAddress;
        }
        else if (type == "L") {
            address = this.LocalTenantAddress;
        }
        if (address != null && address.IsDirty) {
            var myService = new AddressPMService_1.AddressPMService();
            myService.insert(address).subscribe(function (myRespone) {
                if (myRespone != null) {
                    if (!myRespone.HasError) {
                        if (type == "L" && Tools_1.AppTool.IsNullOrEmpty(_this.TenantPm.LocalAddressId)) {
                            _this.TenantPm.LocalAddressId = myRespone.Result.Id;
                        }
                        _this.SubmitTenantChanges();
                    }
                    else {
                        _this.ValidationErrorsList = myRespone.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    CompanyAddressSettingsComponent.prototype.OnSaveAgentCompletedSuccessfully = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.TenantAddress.Id)) {
            this.SubmitCreatingAddress("M");
        }
        else {
            this.SubmitUpdatingAddress("M");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.LocalTenantAddress.Id)) {
            this.SubmitCreatingAddress("L");
        }
        else {
            this.SubmitUpdatingAddress("L");
        }
    };
    CompanyAddressSettingsComponent.prototype.SubmitTenantChanges = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.TenantPm).subscribe(function (myRespone) {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.TenantPm;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myRespone.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    CompanyAddressSettingsComponent = __decorate([
        core_1.Component({
            selector: 'CompanyAddressSettingsComponent',
            moduleId: module.id,
            templateUrl: './CompanyAddressSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CompanyAddressSettingsComponent);
    return CompanyAddressSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.CompanyAddressSettingsComponent = CompanyAddressSettingsComponent;
var AddressItem = /** @class */ (function (_super) {
    __extends(AddressItem, _super);
    function AddressItem(address, tenant, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "Address";
        _this.DemoMessageVisibility = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.country = null;
        _this.state = null;
        _this.Address = address;
        _this.Tenant = tenant;
        _this.DemoMessageVisibility = _this.father.DemoMessageVisibility;
        return _this;
    }
    Object.defineProperty(AddressItem.prototype, "Name", {
        get: function () { return this.Address.Name; },
        set: function (newValue) {
            if (this.Address.Name != newValue) {
                this.Address.Name = newValue;
                this.Address.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "Address1", {
        get: function () { return this.Address.Address1; },
        set: function (newValue) {
            if (this.Address.Address1 != newValue) {
                this.Address.Address1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "Address2", {
        get: function () { return this.Address.Address2; },
        set: function (newValue) {
            if (this.Address.Address2 != newValue) {
                this.Address.Address2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "City", {
        get: function () { return this.Address.City; },
        set: function (newValue) {
            if (this.Address.City != newValue) {
                this.Address.City = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "CountryId", {
        get: function () { return this.Address.CountryId; },
        set: function (newValue) {
            if (this.Address.CountryId != newValue) {
                this.Address.CountryId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "StateId", {
        get: function () { return this.Address.StateId; },
        set: function (newValue) {
            if (this.Address.StateId != newValue) {
                this.Address.StateId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "ZipCode", {
        get: function () { return this.Address.ZipCode; },
        set: function (newValue) {
            if (this.Address.ZipCode != newValue) {
                this.Address.ZipCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "Signature", {
        get: function () { return this.Address.Signature; },
        set: function (newValue) {
            if (this.Address.Signature != newValue) {
                this.Address.Signature = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "PhoneNumber", {
        get: function () { return this.Address.PhoneNumber; },
        set: function (newValue) {
            if (this.Address.PhoneNumber != newValue) {
                this.Address.PhoneNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "FaxNumber", {
        get: function () { return this.Address.FaxNumber; },
        set: function (newValue) {
            if (this.Address.FaxNumber != newValue) {
                this.Address.FaxNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItem.prototype, "Country", {
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
    Object.defineProperty(AddressItem.prototype, "State", {
        get: function () { return this.state; },
        set: function (value) {
            if (this.state != value) {
                this.state = value;
                this.OnStateChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    AddressItem.prototype.OnCountryChanged = function (list) {
        this.SetUIProperties_State();
    };
    AddressItem.prototype.OnStateChanged = function (list) {
        this.SetUIProperties_StateRequired();
    };
    AddressItem.prototype.SetUIProperties_State = function () {
        if (this.Address != null) {
            this.SetUIProperties_StateEnabled();
            this.SetUIProperties_StateRequired();
        }
    };
    AddressItem.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", "Address", isEnabled);
    };
    AddressItem.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("StateId", "Address", isRequired);
    };
    AddressItem.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.Address.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    return AddressItem;
}(BaseComponent_1.BaseComponent));
exports.AddressItem = AddressItem;
//# sourceMappingURL=CompanyAddressSettingsComponent.js.map