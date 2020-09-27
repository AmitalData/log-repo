var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AddressPMService } from '../../../Services/StandardPMs/AddressPMService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { CitySelectionArgs } from '../../../Args';
export var AddEditBranchAddressComponent = (function (_super) {
    __extends(AddEditBranchAddressComponent, _super);
    function AddEditBranchAddressComponent() {
        _super.call(this);
        this.DataContext = this;
        this.ObjectTableName = "Address";
        this.ValidationErrorsList = [];
        this.country = null;
        this.state = null;
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
        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./Common/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                var mySelectedCity = args.CityName;
                if (_this.AddressPM.IsLocalLanguage && args.CityLocalName != null) {
                    mySelectedCity = args.CityLocalName;
                }
                _this.City = mySelectedCity;
                _this.CountryId = args.CountryId;
            }
        });
    };
    AddEditBranchAddressComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    AddEditBranchAddressComponent.prototype.OkButtonClicked = function () {
        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        var isValid = this.Validate();
        if (!isValid) {
            SessionLocator.SelectedSession.StopBusyIndicator();
        }
        else {
            if (!this.AddressPM.IsDirty) {
                SessionLocator.SelectedSession.CloseCurrentWindow();
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
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            Validator.TryValidateObject(this.AddressPM, this.ObjectTableName, errors);
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
        var myService = new AddressPMService();
        if (AppTool.IsNullOrEmpty(this.AddressPM.Id)) {
            myService.insert(this.AddressPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    SessionLocator.SelectedSession.CloseCurrentWindowEmit(_this.AddressPM.Id);
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.SelectedSession.StopBusyIndicator();
                }
            });
        }
        else {
            myService.update(this.AddressPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    SessionLocator.SelectedSession.CloseCurrentWindowEmit(_this.AddressPM.Id);
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.SelectedSession.StopBusyIndicator();
                }
            });
        }
    };
    AddEditBranchAddressComponent.prototype.Clone = function () {
        this.myCloner = new Cloner(this.DataContext);
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
    AddEditBranchAddressComponent.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    templateUrl: './AddEditBranchAddressComponent.html',
                },] },
    ];
    /** @nocollapse */
    AddEditBranchAddressComponent.ctorParameters = [];
    return AddEditBranchAddressComponent;
}(BaseComponent));
//# sourceMappingURL=AddEditBranchAddressComponent.js.map