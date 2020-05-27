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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CountryCityGeneralTabComponent = /** @class */ (function (_super) {
    __extends(CountryCityGeneralTabComponent, _super);
    function CountryCityGeneralTabComponent(args) {
        var _this = _super.call(this) || this;
        _this.args = args;
        _this.DataContext = _this;
        _this.IsNewEntity = true;
        _this.ObjectTableName = "CountryCity";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.country = null;
        _this.state = null;
        _this.EntityPM = args.EntityPM;
        _this.Listen();
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
            _this.IsNewEntity = true;
        }
        else {
            _this.IsNewEntity = false;
        }
        return _this;
    }
    CountryCityGeneralTabComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    CountryCityGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    CountryCityGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    CountryCityGeneralTabComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_State();
    };
    CountryCityGeneralTabComponent.prototype.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    CountryCityGeneralTabComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    CountryCityGeneralTabComponent.prototype.SetUIProperties_StateRequired = function () {
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
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "Country", {
        get: function () { return this.country; },
        set: function (value) {
            if (this.country != value) {
                this.country = value;
                this.OnCountryChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (value) {
            if (this.EntityPM.CountryId != value) {
                this.EntityPM.CountryId = value;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (value) {
            if (this.EntityPM.CountryCode != value) {
                this.EntityPM.CountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "CountryEnglishName", {
        get: function () { return this.EntityPM.CountryEnglishName; },
        set: function (value) {
            if (this.EntityPM.CountryEnglishName != value) {
                this.EntityPM.CountryEnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "State", {
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
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "StateId", {
        get: function () { return this.EntityPM.StateId; },
        set: function (value) {
            if (this.EntityPM.StateId != value) {
                this.EntityPM.StateId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "StateCode", {
        get: function () { return this.EntityPM.StateCode; },
        set: function (newValue) {
            if (this.EntityPM.StateCode != newValue) {
                this.EntityPM.StateCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryCityGeneralTabComponent.prototype, "StateEnglishName", {
        get: function () { return this.EntityPM.StateEnglishName; },
        set: function (newValue) {
            if (this.EntityPM.StateEnglishName != newValue) {
                this.EntityPM.StateEnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CountryCityGeneralTabComponent.prototype.OnCountryChanged = function (list) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryEnglishName = null;
        }
        else {
            this.CountryCode = list.Code;
            this.CountryEnglishName = list.EnglishName;
        }
        this.SetUIProperties_State();
    };
    CountryCityGeneralTabComponent.prototype.OnStateChanged = function (list) {
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
    CountryCityGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CountryCityGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CountryCityGeneralTabComponent);
    return CountryCityGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CountryCityGeneralTabComponent = CountryCityGeneralTabComponent;
//# sourceMappingURL=CountryCityGeneralTabComponent.js.map