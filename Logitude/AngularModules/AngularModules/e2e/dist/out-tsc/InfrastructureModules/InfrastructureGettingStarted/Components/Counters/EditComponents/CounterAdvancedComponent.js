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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var CounterDefinitionPM_1 = require("../../../../../Common/EntityPMs/CounterDefinitionPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var CountersDomainService_1 = require("../../../../../Common/Services/CountersDomainService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var GroupByPipe_1 = require("../../../../../Infrastructure/Pipes/GroupByPipe");
var CounterAdvancedComponent = /** @class */ (function (_super) {
    __extends(CounterAdvancedComponent, _super);
    function CounterAdvancedComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.IsCounterUsed = false;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.HasAllTransportsFeature = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.sameForAllDirectios = true;
        _this.sameForAllTransports = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ALLTRANSPORTMODES")) {
            _this.HasAllTransportsFeature = true;
        }
        return _this;
    }
    CounterAdvancedComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.CounterId = args["CounterId"];
        if (this.CounterId) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var myService = new CountersDomainService_1.CountersDomainService();
            myService.GetCounterAPIHelper(this.CounterId).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.APIHelper = myResponse.Result;
                    if (_this.APIHelper) {
                        _this.CounterPM = _this.APIHelper.CounterPM;
                        _this.IsCounterUsed = _this.APIHelper.IsCounterUsed;
                        _this.InitializeDefinitions();
                        _this.SetUIProperties();
                    }
                    _this.CalculateSampleValue();
                }
                _this.IsResourcesReady = true;
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    CounterAdvancedComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, !this.IsCounterUsed);
    };
    CounterAdvancedComponent.prototype.InitializeDefinitions = function () {
        var _this = this;
        // Dummy:Init
        this.EntityPM = new CounterDefinitionPM_1.CounterDefinitionPM();
        this.EntityPM.CounterId = this.CounterPM.Id;
        this.EntityPM.Tenant = this.CounterPM.Tenant;
        this.EntityPM.UniquePerPrefix = false;
        this.EntityPM.StartNumber = 1000;
        this.EntityPM.StartNumber_Old = 0;
        this.EntityPM.Parameter1 = "E";
        this.EntityPM.Parameter2 = "A";
        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }
        else {
            if (this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1 && f.Parameter2 == _this.EntityPM.Parameter2; })[0] != null) {
                this.EntityPM = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1 && f.Parameter2 == _this.EntityPM.Parameter2; })[0];
            }
            else {
                this.APIHelper.CounterDefinitions.push(this.EntityPM);
            }
            var myPipe = new GroupByPipe_1.GroupByPipe();
            var myGroupbyCount = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
            if (myGroupbyCount > 4) {
                this.sameForAllDirectios = false;
                this.sameForAllTransports = false;
            }
            else {
                switch (myGroupbyCount) {
                    case 1: {
                        this.sameForAllDirectios = true;
                        this.sameForAllTransports = true;
                        break;
                    }
                    case 2:
                    case 3: {
                        this.sameForAllDirectios = true;
                        this.sameForAllTransports = false;
                        break;
                    }
                    case 4: {
                        this.sameForAllDirectios = false;
                        this.sameForAllTransports = true;
                        break;
                    }
                }
            }
        }
        this.BuildItemsSource();
    };
    CounterAdvancedComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var itemsParams = [];
        itemsParams.push({ Parameter1: 'E', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'I', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'D', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'R', Parameter2: "A" });
        itemsParams.forEach(function (item) {
            var itemPM = _this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == item['Parameter1'] && f.Parameter2 == item['Parameter2']; })[0];
            if (itemPM == null) {
                itemPM = new CounterDefinitionPM_1.CounterDefinitionPM();
                itemPM.CounterId = _this.CounterPM.Id;
                itemPM.Tenant = _this.CounterPM.Tenant;
                itemPM.UniquePerPrefix = _this.EntityPM.UniquePerPrefix;
                itemPM.StartNumber = _this.EntityPM.StartNumber;
                itemPM.StartNumber_Old = _this.EntityPM.StartNumber_Old;
                itemPM.Parameter1 = item['Parameter1'];
                itemPM.Parameter2 = item['Parameter2'];
                _this.APIHelper.CounterDefinitions.push(itemPM);
            }
            if (itemPM.Parameter1 == _this.EntityPM.Parameter1) {
                _this.ItemsSource.push(new CounterAdvancedColumnItem(itemPM, _this));
            }
            else {
                if (!_this.SameForAllDirectios) {
                    _this.ItemsSource.push(new CounterAdvancedColumnItem(itemPM, _this));
                }
            }
        });
    };
    Object.defineProperty(CounterAdvancedComponent.prototype, "SameForAllDirectios", {
        get: function () { return this.sameForAllDirectios; },
        set: function (value) {
            if (this.sameForAllDirectios != value) {
                this.sameForAllDirectios = value;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedComponent.prototype, "SameForAllTransports", {
        get: function () { return this.sameForAllTransports; },
        set: function (value) {
            if (this.sameForAllTransports != value) {
                this.sameForAllTransports = value;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedComponent.prototype, "UniquePerPrefix", {
        get: function () { return this.EntityPM.UniquePerPrefix; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.UniquePerPrefix != value) {
                this.EntityPM.UniquePerPrefix = value;
                this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1 && f.Parameter2 == _this.EntityPM.Parameter2; })[0].Prefix;
                this.APIHelper.CounterDefinitions.forEach(function (item) {
                    item.UniquePerPrefix = value;
                    if (!value) {
                        item.Prefix = _this.EntityPM.Prefix;
                    }
                });
                this.ItemsSource.forEach(function (item) {
                    item.SetUIProperties();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedComponent.prototype, "Prefix", {
        get: function () { return this.EntityPM.Prefix; },
        set: function (value) {
            if (this.EntityPM.Prefix != value) {
                this.EntityPM.Prefix = value;
                this.APIHelper.CounterDefinitions.forEach(function (item) {
                    item.Prefix = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedComponent.prototype, "CounterSize", {
        get: function () { return this.EntityPM.CounterSize; },
        set: function (value) {
            if (this.EntityPM.CounterSize != value) {
                this.EntityPM.CounterSize = value;
                this.APIHelper.CounterDefinitions.forEach(function (item) {
                    item.CounterSize = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedComponent.prototype, "StartNumber", {
        get: function () { return this.EntityPM.StartNumber; },
        set: function (value) {
            if (this.EntityPM.StartNumber != value) {
                this.EntityPM.StartNumber = value;
                this.APIHelper.CounterDefinitions.forEach(function (item) {
                    item.StartNumber = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    CounterAdvancedComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CounterAdvancedComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var isValidGreaterStartNumber = true;
        this.APIHelper.CounterDefinitions.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.StartNumber)) {
                if (item.StartNumber < item.StartNumber_Old) {
                    isValidGreaterStartNumber = false;
                }
            }
        });
        if (!isValidGreaterStartNumber) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("The new start number must be greater than current start number!");
        }
        else {
            var isValidUniquePrefix = true;
            if (this.UniquePerPrefix) {
                isValidUniquePrefix = false;
                var myPipe = new GroupByPipe_1.GroupByPipe();
                var myGroupbyCount = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
                if (this.SameForAllDirectios == true && this.SameForAllTransports == true) {
                    if (myGroupbyCount == 1) {
                        isValidUniquePrefix = true;
                    }
                }
                if (this.SameForAllDirectios == false && this.SameForAllTransports == true) {
                    if (myGroupbyCount == 4) {
                        isValidUniquePrefix = true;
                    }
                }
                if (this.SameForAllDirectios == true && this.SameForAllTransports == false) {
                    if (myGroupbyCount == 3) {
                        isValidUniquePrefix = true;
                    }
                }
                if (this.SameForAllDirectios == false && this.SameForAllTransports == false) {
                    if (myGroupbyCount == 12) {
                        isValidUniquePrefix = true;
                    }
                }
            }
            else {
            }
            if (!isValidUniquePrefix) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Some Prefix values are invalid (Prefix should be unique)");
            }
            else {
                var errors = [];
                if (this.CounterSize > 20) {
                    errors.push("Maximum size allowed for counter is 20");
                }
                if (this.UniquePerPrefix == true) {
                    this.APIHelper.CounterDefinitions.forEach(function (item) {
                        Validator_1.Validator.TryValidateObject(item, _this.ObjectTableName, errors);
                        if (item.UniquePerPrefix && !Tools_1.AppTool.IsNullOrEmpty(item.Prefix) && !Tools_1.AppTool.IsNullOrEmpty(item.StartNumber)) {
                            if ((item.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(item.Prefix) > 20) {
                                errors.push("Maximum length allowed for [Prefix + StartNumber] is 20");
                            }
                        }
                    });
                }
                else {
                    //var m = AppTool.GetCounterPrefixLength(this.Prefix);
                    if ((this.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(this.Prefix) > 20) {
                        errors.push("Maximum length allowed for [Prefix + StartNumber] is 20");
                    }
                    this.APIHelper.CounterDefinitions.forEach(function (item) {
                        Validator_1.Validator.TryValidateObject(item, _this.ObjectTableName, errors);
                    });
                }
                this.ValidationErrorsList = errors;
                if (errors.length == 0) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var myService = new CountersDomainService_1.CountersDomainService();
                    myService.Post(this.APIHelper).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        }
                    });
                }
            }
        }
    };
    CounterAdvancedComponent.prototype.CalculateSampleValue = function () {
        this.SampleValue = Tools_1.AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, "", this.CounterSize);
    };
    CounterAdvancedComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CounterAdvancedComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CounterAdvancedComponent);
    return CounterAdvancedComponent;
}(BaseComponent_1.BaseComponent));
exports.CounterAdvancedComponent = CounterAdvancedComponent;
var CounterAdvancedColumnItem = /** @class */ (function () {
    function CounterAdvancedColumnItem(itemPM, father) {
        this.father = father;
        this.DataContext = this;
        this.ObjectTableName = "CounterDefinition";
        this.ItemsSource = [];
        this.EntityPM = itemPM;
        this.BuildItemsSource();
        switch (itemPM.Parameter1) {
            case "E": {
                this.Name = "Export";
                break;
            }
            case "I": {
                this.Name = "Import";
                break;
            }
            case "D": {
                this.Name = "Domestic";
                break;
            }
            case "R": {
                this.Name = "Drop";
                break;
            }
        }
    }
    CounterAdvancedColumnItem.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.father.SameForAllTransports) {
            this.ItemsSource.push(new CounterAdvancedDefinitionItem(this.EntityPM, this));
        }
        else {
            var itemsParams = [];
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "A" });
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "O" });
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "I" });
            itemsParams.forEach(function (item) {
                var itemPM = _this.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == item['Parameter1'] && f.Parameter2 == item['Parameter2']; })[0];
                if (itemPM == null) {
                    itemPM = new CounterDefinitionPM_1.CounterDefinitionPM();
                    itemPM.CounterId = _this.father.CounterPM.Id;
                    itemPM.Tenant = _this.father.CounterPM.Tenant;
                    itemPM.UniquePerPrefix = _this.father.UniquePerPrefix;
                    itemPM.StartNumber = _this.father.StartNumber;
                    itemPM.Parameter1 = item['Parameter1'];
                    itemPM.Parameter2 = item['Parameter2'];
                }
                _this.ItemsSource.push(new CounterAdvancedDefinitionItem(itemPM, _this));
            });
        }
    };
    CounterAdvancedColumnItem.prototype.SetUIProperties = function () {
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    return CounterAdvancedColumnItem;
}());
exports.CounterAdvancedColumnItem = CounterAdvancedColumnItem;
var CounterAdvancedDefinitionItem = /** @class */ (function (_super) {
    __extends(CounterAdvancedDefinitionItem, _super);
    function CounterAdvancedDefinitionItem(itemPM, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.EntityPM = itemPM;
        _this.Parameter = itemPM.Parameter2;
        _this.SetUIProperties();
        return _this;
    }
    CounterAdvancedDefinitionItem.prototype.SetUIProperties = function () {
        var isEnabled = true;
        var isEnabled_StartNumber = true;
        if (this.father.father.IsCounterUsed) {
            isEnabled = false;
            isEnabled_StartNumber = false;
        }
        else if (!this.father.father.UniquePerPrefix) {
            isEnabled_StartNumber = false;
        }
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, isEnabled_StartNumber);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, isEnabled);
    };
    Object.defineProperty(CounterAdvancedDefinitionItem.prototype, "Prefix", {
        get: function () { return this.EntityPM.Prefix; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Prefix != value) {
                this.EntityPM.Prefix = value;
                if (this.father.father.SameForAllDirectios == false && this.father.father.SameForAllTransports == false) {
                    // No need to apply for others
                }
                else {
                    if (this.father.father.SameForAllTransports) {
                        // Get All Definitions with same Parameter1: E (Export)
                        this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1; }).forEach(function (item) {
                            item.Prefix = value;
                        });
                    }
                    if (this.father.father.SameForAllDirectios) {
                        // Get All Definitions with same Parameter2: A (Airline)
                        this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter2 == _this.EntityPM.Parameter2; }).forEach(function (item) {
                            item.Prefix = value;
                        });
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedDefinitionItem.prototype, "CounterSize", {
        get: function () { return this.EntityPM.CounterSize; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.CounterSize != value) {
                this.EntityPM.CounterSize = value;
                if (this.father.father.SameForAllDirectios == false && this.father.father.SameForAllTransports == false) {
                    // No need to apply for others
                }
                else {
                    if (this.father.father.SameForAllTransports) {
                        // Get All Definitions with same Parameter1: E (Export)
                        this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1; }).forEach(function (item) {
                            item.CounterSize = value;
                        });
                    }
                    if (this.father.father.SameForAllDirectios) {
                        // Get All Definitions with same Parameter2: A (Airline)
                        this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter2 == _this.EntityPM.Parameter2; }).forEach(function (item) {
                            item.CounterSize = value;
                        });
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterAdvancedDefinitionItem.prototype, "StartNumber", {
        get: function () { return this.EntityPM.StartNumber; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.StartNumber != value) {
                this.EntityPM.StartNumber = value;
                if (this.father.father.SameForAllTransports) {
                    this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1; }).forEach(function (item) {
                        item.StartNumber = value;
                    });
                }
                if (this.father.father.SameForAllDirectios) {
                    this.father.father.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter2 == _this.EntityPM.Parameter2; }).forEach(function (item) {
                        item.StartNumber = value;
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return CounterAdvancedDefinitionItem;
}(BaseComponent_1.BaseComponent));
exports.CounterAdvancedDefinitionItem = CounterAdvancedDefinitionItem;
//# sourceMappingURL=CounterAdvancedComponent.js.map