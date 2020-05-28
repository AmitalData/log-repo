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
var BaseComponent_1 = require("../../LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Tools");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var StateListService_1 = require("../../../../Common/Services/StandardLists/StateListService");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var DateTimeZone_1 = require("../../../Utilities/DateTimeZone");
var Validator_1 = require("../../../Validators/Validator");
var TextCodeTranslator_1 = require("../../../Utilities/TextCodeTranslator");
var WizardAddressCompnent = /** @class */ (function (_super) {
    __extends(WizardAddressCompnent, _super);
    function WizardAddressCompnent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "Address";
        _this.AgentPM = null;
        _this.TenantPM = null;
        _this.TimeZonesList = [];
        _this.IsEditingEnabled = true;
        _this.TimeZonesList = DateTimeZone_1.DateTimeZone.GetTimeZonesList();
        _this.InitializeServices();
        return _this;
    }
    WizardAddressCompnent.prototype.InitializeServices = function () {
        this.myStateListService = new StateListService_1.StateListService();
        this.myCountryListService = new CountryListService_1.CountryListService();
    };
    WizardAddressCompnent.prototype.InitializeComponent = function (tenantPM, addressPM, agentPM) {
        var _this = this;
        this.TenantPM = tenantPM;
        this.EntityPM = addressPM;
        this.AgentPM = agentPM;
        this.selectedTimeZone = this.TimeZonesList.filter(function (f) { return f.BaseUtcOffset == _this.TenantPM.TimeZoneOffset; })[0];
        if (Tools_1.AppTool.IsNullOrEmpty(addressPM.Description)) {
            this.EntityPM.Description = addressPM.Description = "Main Address";
        }
        if (tenantPM.DayLightOffset != 0) {
            this.ShowDayLightSettings = false;
        }
        this.SetUIProperties();
    };
    WizardAddressCompnent.prototype.Validate = function (errors) {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Address", errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.City)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Address.F.City")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Address.F.CountryId")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.StateId)) {
            if (this.IsStateRequired) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Address.F.State")));
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.TimeZoneOffset)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Tenant.F.TimeZoneOffset")));
        }
        return errors;
    };
    WizardAddressCompnent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = true;
        this.UIProperties.SetRequired("City", "Address", Tools_1.AppTool.IsNullOrEmpty(this.City) ? true : false);
        this.UIProperties.SetRequired("CountryId", "Address", Tools_1.AppTool.IsNullOrEmpty(this.CountryId) ? true : false);
        this.SetUIProperties_State();
        this.SetUIProperties_TimeZone();
    };
    WizardAddressCompnent.prototype.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    WizardAddressCompnent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.IsEditingEnabled) {
            isEnabled = this.HasStates;
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    WizardAddressCompnent.prototype.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.IsStateRequired) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.StateId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    };
    WizardAddressCompnent.prototype.SetUIProperties_TimeZone = function () {
        var isRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.TimeZoneOffset)) {
            isRequired = true;
        }
        this.UIProperties.SetRequired("TimeZoneOffset", "Tenant", isRequired);
    };
    Object.defineProperty(WizardAddressCompnent.prototype, "Name", {
        // Address 
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
                this.TenantPM.Company = value;
                this.AgentPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "Address1", {
        get: function () { return this.EntityPM.Address1; },
        set: function (value) {
            if (this.EntityPM.Address1 != value) {
                this.EntityPM.Address1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "Address2", {
        get: function () { return this.EntityPM.Address2; },
        set: function (value) {
            if (this.EntityPM.Address2 != value) {
                this.EntityPM.Address2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "Signature", {
        get: function () { return this.EntityPM.Signature; },
        set: function (value) {
            if (this.EntityPM.Signature != value) {
                this.EntityPM.Signature = value;
                this.TenantPM.Signature = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "City", {
        get: function () { return this.EntityPM.City; },
        set: function (value) {
            if (this.EntityPM.City != value) {
                this.EntityPM.City = value;
                this.UIProperties.SetRequired("City", "Address", Tools_1.AppTool.IsNullOrEmpty(value) ? true : false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "FaxNumber", {
        get: function () { return this.EntityPM.FaxNumber; },
        set: function (value) {
            if (this.EntityPM.FaxNumber != value) {
                this.EntityPM.FaxNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "ZipCode", {
        get: function () { return this.EntityPM.ZipCode; },
        set: function (value) {
            if (this.EntityPM.ZipCode != value) {
                this.EntityPM.ZipCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM.PhoneNumber; },
        set: function (value) {
            if (this.EntityPM.PhoneNumber != value) {
                this.EntityPM.PhoneNumber = value;
                SessionLocator_1.SessionLocator.LoggedUserPM.BusinessPhone = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "StateId", {
        get: function () { return this.EntityPM.StateId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.StateId != value) {
                this.EntityPM.StateId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.StateCode = null;
                    this.StateEnglishName = null;
                    this.SetUIProperties_StateRequired();
                }
                else {
                    this.myStateListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.StateCode = list.Code;
                                _this.StateEnglishName = list.EnglishName;
                                _this.SetUIProperties_StateRequired();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "StateCode", {
        get: function () { return this.EntityPM.StateCode; },
        set: function (value) {
            if (this.EntityPM.StateCode != value) {
                this.EntityPM.StateCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM.StateEnglishName; },
        set: function (value) {
            if (this.EntityPM.StateEnglishName != value) {
                this.EntityPM.StateEnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.CountryId != value) {
                this.EntityPM.CountryId = value;
                this.StateId = null;
                this.UIProperties.SetRequired("CountryId", "Address", Tools_1.AppTool.IsNullOrEmpty(value) ? true : false);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.TenantPM.CountryCode = null;
                    this.TenantPM.CountryName = null;
                    this.CountryCode = null;
                    this.CountryName = null;
                    this.CountryEnglishName = null;
                    this.HasStates = false;
                    this.IsStateRequired = false;
                    this.SetUIProperties_State();
                }
                else {
                    this.myCountryListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.TenantPM.CountryCode = list.Code;
                                _this.TenantPM.CountryName = list.EnglishName;
                                _this.CountryCode = list.Code;
                                _this.CountryName = list.EnglishName;
                                _this.CountryEnglishName = list.EnglishName;
                                _this.HasStates = list.HasStates;
                                _this.IsStateRequired = list.IsStateRequired;
                                _this.SetUIProperties_State();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (value) {
            if (this.EntityPM.CountryCode != value) {
                this.EntityPM.CountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "CountryName", {
        get: function () { return this.EntityPM.CountryName; },
        set: function (value) {
            if (this.EntityPM.CountryName != value) {
                this.EntityPM.CountryName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM.CountryEnglishName; },
        set: function (value) {
            if (this.EntityPM.CountryEnglishName != value) {
                this.EntityPM.CountryEnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "HasStates", {
        get: function () { return this.EntityPM.HasStates; },
        set: function (value) {
            if (this.EntityPM.HasStates != value) {
                this.EntityPM.HasStates = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "IsStateRequired", {
        get: function () { return this.EntityPM.IsStateRequired; },
        set: function (value) {
            if (this.EntityPM.IsStateRequired != value) {
                this.EntityPM.IsStateRequired = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "TimeZoneOffset", {
        // Tenant
        get: function () { return this.TenantPM.TimeZoneOffset; },
        set: function (value) {
            if (this.TenantPM.TimeZoneOffset != value) {
                this.TenantPM.TimeZoneOffset = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "DayLightStartDate", {
        get: function () { return this.TenantPM.DayLightStartDate; },
        set: function (value) {
            if (this.TenantPM.DayLightStartDate != value) {
                this.TenantPM.DayLightStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "DayLightEndDate", {
        get: function () { return this.TenantPM.DayLightEndDate; },
        set: function (value) {
            if (this.TenantPM.DayLightEndDate != value) {
                this.TenantPM.DayLightEndDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "DayLightOffset", {
        get: function () { return this.TenantPM.DayLightOffset; },
        set: function (value) {
            if (this.TenantPM.DayLightOffset != value) {
                this.TenantPM.DayLightOffset = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "ShowDayLightSettings", {
        get: function () { return this.showDayLightSettings; },
        set: function (value) {
            if (this.showDayLightSettings != value) {
                this.showDayLightSettings = value;
                if (!value) {
                    this.DayLightOffset = 0;
                    this.DayLightStartDate = null;
                    this.DayLightEndDate = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardAddressCompnent.prototype, "SelectedTimeZone", {
        get: function () { return this.selectedTimeZone; },
        set: function (value) {
            if (this.selectedTimeZone != value) {
                this.selectedTimeZone = value;
                if (value) {
                    this.TenantPM.TimeZoneOffset = value.BaseUtcOffset;
                }
                else {
                    this.TenantPM.TimeZoneOffset = 0;
                }
                this.SetUIProperties_TimeZone();
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardAddressCompnent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardAddressCompnent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WizardAddressCompnent);
    return WizardAddressCompnent;
}(BaseComponent_1.BaseComponent));
exports.WizardAddressCompnent = WizardAddressCompnent;
//# sourceMappingURL=WizardAddressCompnent.js.map