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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CompetitorPM_1 = require("../../EntityPMs/CompetitorPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var CountryListService_1 = require("../../Services/StandardLists/CountryListService");
var StateListService_1 = require("../../Services/StandardLists/StateListService");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var CompetitorPMService_1 = require("../../Services/StandardPMs/CompetitorPMService");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CompetitorComponent = /** @class */ (function (_super) {
    __extends(CompetitorComponent, _super);
    function CompetitorComponent(entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Competitor";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (entityArgs.EntityPM != null)
            _this.EntityPM = entityArgs.EntityPM;
        else {
            _this.EntityPM = new CompetitorPM_1.CompetitorPM();
            _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        }
        _this.UIProperties.SetRequired("City", _this.ObjectTableName, true);
        _this.UIProperties.SetRequired("CountryId", _this.ObjectTableName, true);
        _this.SetUIProperties(null);
        return _this;
    }
    CompetitorComponent.prototype.SetWindowArgs = function (args) {
    };
    Object.defineProperty(CompetitorComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Address1", {
        get: function () { return this.EntityPM.Address1; },
        set: function (value) {
            if (this.EntityPM.Address1 != value)
                this.EntityPM.Address1 = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Address2", {
        get: function () { return this.EntityPM.Address2; },
        set: function (value) {
            if (this.EntityPM.Address2 != value)
                this.EntityPM.Address2 = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "ZipCode", {
        get: function () { return this.EntityPM.ZipCode; },
        set: function (value) {
            if (this.EntityPM.ZipCode != value)
                this.EntityPM.ZipCode = value;
        },
        enumerable: true,
        configurable: true
    });
    CompetitorComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    Object.defineProperty(CompetitorComponent.prototype, "City", {
        get: function () { return this.EntityPM.City; },
        set: function (value) {
            if (this.EntityPM.City != value) {
                this.EntityPM.City = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("City", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("City", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.CountryId != value) {
                this.EntityPM.CountryId = value;
                var StateId = null;
                var CountryCode = null;
                var CountryName = null;
                var countryListService = new CountryListService_1.CountryListService();
                countryListService.getAllFromCache().subscribe(function (result) {
                    var list = result.Result.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant && d.Id == value; })[0];
                    if (list != null) {
                        CountryCode = list.Code;
                        CountryName = list.EnglishName;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        _this.UIProperties.SetRequired("CountryId", _this.ObjectTableName, true);
                    }
                    else {
                        _this.UIProperties.SetRequired("CountryId", _this.ObjectTableName, false);
                    }
                    _this.SetUIProperties(list);
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    CompetitorComponent.prototype.SetUIProperties = function (list) {
        if (list != null) {
            this.UIProperties.SetEnabled("StateId", this.ObjectTableName, list.HasStates);
            this.UIProperties.SetRequired("StateId", this.ObjectTableName, list.IsStateRequired);
            if (!list.HasStates) {
                this.StateId = null;
                this.StateName = null;
            }
        }
        else {
            this.UIProperties.SetEnabled("StateId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("StateId", this.ObjectTableName, false);
        }
    };
    Object.defineProperty(CompetitorComponent.prototype, "StateId", {
        get: function () { return this.EntityPM.StateId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.StateId != value) {
                this.EntityPM.StateId = value;
                var stateListService = new StateListService_1.StateListService();
                stateListService.getAllFromCache().subscribe(function (result) {
                    var list = result.Result.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant && d.Id == value; })[0];
                    if (list != null) {
                        _this.StateName = list.EnglishName;
                    }
                });
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("StateId", this.ObjectTableName, false);
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(value))
                    this.UIProperties.SetRequired("StateId", this.ObjectTableName, true);
                if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId))
                    this.SetUIProperties(null);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "PhoneNumber", {
        get: function () { return this.EntityPM.PhoneNumber; },
        set: function (value) {
            if (this.EntityPM.PhoneNumber != value)
                this.EntityPM.PhoneNumber = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "FaxNumber", {
        get: function () { return this.EntityPM.FaxNumber; },
        set: function (value) {
            if (this.EntityPM.FaxNumber != value)
                this.EntityPM.FaxNumber = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Website", {
        get: function () { return this.EntityPM.Website; },
        set: function (value) {
            if (this.EntityPM.Website != value)
                this.EntityPM.Website = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Strengths", {
        get: function () { return this.EntityPM.Strengths; },
        set: function (value) {
            if (this.EntityPM.Strengths != value)
                this.EntityPM.Strengths = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Weaknesses", {
        get: function () { return this.EntityPM.Weaknesses; },
        set: function (value) {
            if (this.EntityPM.Weaknesses != value)
                this.EntityPM.Weaknesses = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Opportunity", {
        get: function () { return this.EntityPM.Opportunity; },
        set: function (value) {
            if (this.EntityPM.Opportunity != value)
                this.EntityPM.Opportunity = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "Threat", {
        get: function () { return this.EntityPM.Threat; },
        set: function (value) {
            if (this.EntityPM.Threat != value)
                this.EntityPM.Threat = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "StateName", {
        get: function () { return this.EntityPM.StateName; },
        set: function (value) {
            if (this.EntityPM.StateName != value)
                this.EntityPM.StateName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (value) {
            if (this.EntityPM.CountryCode != value)
                this.EntityPM.CountryCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorComponent.prototype, "CountryName", {
        get: function () { return this.EntityPM.CountryName; },
        set: function (value) {
            if (this.EntityPM.CountryName != value)
                this.EntityPM.CountryName = value;
        },
        enumerable: true,
        configurable: true
    });
    CompetitorComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CompetitorComponent.prototype.Validate = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.Name))
            this.ValidationErrorsList.push("Name field is required");
        if (Tools_1.AppTool.IsNullOrEmpty(this.City))
            this.ValidationErrorsList.push("City field is required");
        if (Tools_1.AppTool.IsNullOrEmpty(this.CountryId))
            this.ValidationErrorsList.push("Country field is required");
        if (this.UIProperties.GetUIProperty("StateId", this.ObjectTableName, this.DataContext, true).IsRequired && Tools_1.AppTool.IsNullOrEmpty(this.StateId)) {
            this.ValidationErrorsList.push("State field is required");
        }
    };
    CompetitorComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorCreating();
            var myService = new CompetitorPMService_1.CompetitorPMService();
            myService.insert(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    CompetitorComponent = __decorate([
        core_1.Component({
            selector: 'CompetitorComponent',
            moduleId: module.id,
            templateUrl: './CompetitorComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], CompetitorComponent);
    return CompetitorComponent;
}(BaseComponent_1.BaseComponent));
exports.CompetitorComponent = CompetitorComponent;
//# sourceMappingURL=CompetitorComponent.js.map