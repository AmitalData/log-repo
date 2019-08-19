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
var AddressPMService_1 = require("../../../Services/StandardPMs/AddressPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Args");
var AddEditBranchAddressComponent = /** @class */ (function (_super) {
    __extends(AddEditBranchAddressComponent, _super);
    function AddEditBranchAddressComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Address";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.country = null;
        _this.state = null;
        return _this;
    }
    AddEditBranchAddressComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.AddressPM = windowArgs;
        this.SetUIProperties();
        this.Clone();
    };
    AddEditBranchAddressComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    AddEditBranchAddressComponent.prototype.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    AddEditBranchAddressComponent.prototype.SetUIProperties_StateRequired = function () {
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
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "Name", {
        get: function () { return this.AddressPM.Name; },
        set: function (newValue) {
            if (this.AddressPM.Name != newValue) {
                this.AddressPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "Address1", {
        get: function () { return this.AddressPM.Address1; },
        set: function (newValue) {
            if (this.AddressPM.Address1 != newValue) {
                this.AddressPM.Address1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "Address2", {
        get: function () { return this.AddressPM.Address2; },
        set: function (newValue) {
            if (this.AddressPM.Address2 != newValue) {
                this.AddressPM.Address2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "City", {
        get: function () { return this.AddressPM.City; },
        set: function (newValue) {
            if (this.AddressPM.City != newValue) {
                this.AddressPM.City = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "ZipCode", {
        get: function () { return this.AddressPM.ZipCode; },
        set: function (newValue) {
            if (this.AddressPM.ZipCode != newValue) {
                this.AddressPM.ZipCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "PhoneNumber", {
        get: function () { return this.AddressPM.PhoneNumber; },
        set: function (newValue) {
            if (this.AddressPM.PhoneNumber != newValue) {
                this.AddressPM.PhoneNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "FaxNumber", {
        get: function () { return this.AddressPM.FaxNumber; },
        set: function (newValue) {
            if (this.AddressPM.FaxNumber != newValue) {
                this.AddressPM.FaxNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "ATTN", {
        get: function () { return this.AddressPM.ATTN; },
        set: function (newValue) {
            if (this.AddressPM.ATTN != newValue) {
                this.AddressPM.ATTN = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "CountryId", {
        get: function () { return this.AddressPM.CountryId; },
        set: function (newValue) {
            if (this.AddressPM.CountryId != newValue) {
                this.AddressPM.CountryId = newValue;
                this.StateId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "StateId", {
        get: function () { return this.AddressPM.StateId; },
        set: function (newValue) {
            if (this.AddressPM.StateId != newValue) {
                this.AddressPM.StateId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "Country", {
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
    Object.defineProperty(AddEditBranchAddressComponent.prototype, "State", {
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
    AddEditBranchAddressComponent.prototype.OnCountryChanged = function (list) {
        this.SetUIProperties();
    };
    AddEditBranchAddressComponent.prototype.OnStateChanged = function (list) {
        this.SetUIProperties_StateRequired();
    };
    // Commands
    AddEditBranchAddressComponent.prototype.SelectCityCommand = function () {
        var _this = this;
        var args = new Args_1.CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.AddressPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
                _this.StateId = args.StateId;
            }
        });
    };
    AddEditBranchAddressComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditBranchAddressComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (!this.AddressPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.Save();
            }
        }
    };
    AddEditBranchAddressComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.AddressPM != null) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            Validator_1.Validator.TryValidateObject(this.AddressPM, this.ObjectTableName, errors);
            if (this.Country != null) {
                if (this.State == null) {
                    if (this.Country.IsStateRequired) {
                        errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }
        }
        isValid = errors.length == 0 ? true : false;
        this.ValidationErrorsList = errors;
        return isValid;
    };
    AddEditBranchAddressComponent.prototype.Save = function () {
        var _this = this;
        var myService = new AddressPMService_1.AddressPMService();
        if (Tools_1.AppTool.IsNullOrEmpty(this.AddressPM.Id)) {
            myService.insert(this.AddressPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.AddressPM.Id);
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
        else {
            myService.update(this.AddressPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.AddressPM.Id);
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AddEditBranchAddressComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Address1');
        this.myCloner.AddField('Address2');
        this.myCloner.AddField('City');
        this.myCloner.AddField('StateId');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddField('ZipCode');
        this.myCloner.AddField('PhoneNumber');
        this.myCloner.AddField('FaxNumber');
        this.myCloner.AddField('ATTN');
        this.myCloner.AddEntity(this.AddressPM);
    };
    AddEditBranchAddressComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditBranchAddressComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditBranchAddressComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditBranchAddressComponent);
    return AddEditBranchAddressComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditBranchAddressComponent = AddEditBranchAddressComponent;
//# sourceMappingURL=AddEditBranchAddressComponent.js.map