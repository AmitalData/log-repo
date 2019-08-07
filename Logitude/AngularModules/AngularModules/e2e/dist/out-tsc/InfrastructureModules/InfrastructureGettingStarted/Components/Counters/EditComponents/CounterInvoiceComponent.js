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
var CounterInvoiceComponent = /** @class */ (function (_super) {
    __extends(CounterInvoiceComponent, _super);
    function CounterInvoiceComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.IsCounterUsed = false;
        _this.IsResourcesReady = false;
        _this.HasConsolidationFeature = false;
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.sameForAllTypes = true;
        _this.HasConsolidationFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent");
        if (_this.HasConsolidationFeature) {
            _this.SameRadioButtonLabel = "Same for Invoice, Credit, Manifest and Consolidation.";
            _this.DiffRadioButtonLabel = "Different for Invoice, Credit, Manifest and Consolidation.";
        }
        else {
            _this.SameRadioButtonLabel = "Same for Invoice, Credit and Manifest.";
            _this.DiffRadioButtonLabel = "Different for Invoice, Credit and Manifest.";
        }
        return _this;
    }
    CounterInvoiceComponent.prototype.SetWindowArgs = function (args) {
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
                        _this.APIHelper.CounterDefinitions = _this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 != "TX"; });
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
    CounterInvoiceComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("Suffix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, !this.IsCounterUsed);
    };
    CounterInvoiceComponent.prototype.InitializeDefinitions = function () {
        var _this = this;
        // Dummy:Init
        this.EntityPM = new CounterDefinitionPM_1.CounterDefinitionPM();
        this.EntityPM.CounterId = this.CounterPM.Id;
        this.EntityPM.Tenant = this.CounterPM.Tenant;
        this.EntityPM.UniquePerPrefix = false;
        this.EntityPM.StartNumber = 1000;
        this.EntityPM.StartNumber_Old = 0;
        this.EntityPM.Parameter1 = "IN";
        this.EntityPM.Parameter2 = null;
        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }
        else {
            if (this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1; })[0] != null) {
                this.EntityPM = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == _this.EntityPM.Parameter1; })[0];
            }
            else {
                this.APIHelper.CounterDefinitions.push(this.EntityPM);
            }
            var myPipe = new GroupByPipe_1.GroupByPipe();
            var myGroupbyCount = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
            if (myGroupbyCount == 1) {
                this.sameForAllTypes = true;
            }
            else {
                this.sameForAllTypes = false;
            }
        }
        this.BuildItemsSource();
    };
    CounterInvoiceComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var itemsParams = [];
        itemsParams.push({ Code: 'IN', Name: "Invoice" });
        itemsParams.push({ Code: 'CD', Name: "Credit" });
        itemsParams.push({ Code: 'MN', Name: "Manifest" });
        itemsParams.push({ Code: 'CI', Name: "Customs Invoice" });
        itemsParams.push({ Code: 'CC', Name: "Customs Credit" });
        if (this.HasConsolidationFeature) {
            itemsParams.push({ Code: 'CON', Name: "Consolidation" });
        }
        itemsParams.forEach(function (item) {
            var itemPM = _this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == item['Code']; })[0];
            if (itemPM == null) {
                itemPM = new CounterDefinitionPM_1.CounterDefinitionPM();
                itemPM.CounterId = _this.EntityPM.CounterId;
                itemPM.Tenant = _this.EntityPM.Tenant;
                itemPM.UniquePerPrefix = _this.EntityPM.UniquePerPrefix;
                itemPM.StartNumber = _this.EntityPM.StartNumber;
                itemPM.StartNumber_Old = _this.EntityPM.StartNumber_Old;
                itemPM.Parameter1 = item['Code'];
                itemPM.Parameter1 = null;
                _this.APIHelper.CounterDefinitions.push(itemPM);
            }
            _this.ItemsSource.push(new CounterInvoiceDefinitionItem(itemPM, item['Name'], _this));
        });
    };
    Object.defineProperty(CounterInvoiceComponent.prototype, "SameForAllTypes", {
        get: function () { return this.sameForAllTypes; },
        set: function (value) {
            var _this = this;
            if (this.sameForAllTypes != value) {
                this.sameForAllTypes = value;
                this.EntityPM.UniquePerPrefix = false;
                this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "IN"; })[0].Prefix;
                this.EntityPM.StartNumber = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "IN"; })[0].StartNumber;
                this.ItemsSource.forEach(function (item) {
                    item.UniquePerPrefix = _this.EntityPM.UniquePerPrefix;
                    item.Prefix = _this.EntityPM.Prefix;
                    item.StartNumber = _this.EntityPM.StartNumber;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceComponent.prototype, "UniquePerPrefix", {
        get: function () { return this.EntityPM.UniquePerPrefix; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.UniquePerPrefix != value) {
                this.EntityPM.UniquePerPrefix = value;
                this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "IN"; })[0].Prefix;
                this.ItemsSource.forEach(function (item) {
                    item.UniquePerPrefix = value;
                    if (!value) {
                        item.Prefix = _this.EntityPM.Prefix;
                    }
                    item.SetUIProperties();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceComponent.prototype, "Prefix", {
        get: function () { return this.EntityPM.Prefix; },
        set: function (value) {
            if (this.EntityPM.Prefix != value) {
                this.EntityPM.Prefix = value;
                this.ItemsSource.forEach(function (item) {
                    item.Prefix = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceComponent.prototype, "Suffix", {
        get: function () { return this.EntityPM.Suffix; },
        set: function (value) {
            if (this.EntityPM.Suffix != value) {
                this.EntityPM.Suffix = value;
                this.ItemsSource.forEach(function (item) {
                    item.Suffix = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceComponent.prototype, "CounterSize", {
        get: function () { return this.EntityPM.CounterSize; },
        set: function (value) {
            if (this.EntityPM.CounterSize != value) {
                this.EntityPM.CounterSize = value;
                this.ItemsSource.forEach(function (item) {
                    item.CounterSize = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceComponent.prototype, "StartNumber", {
        get: function () { return this.EntityPM.StartNumber; },
        set: function (value) {
            if (this.EntityPM.StartNumber != value) {
                this.EntityPM.StartNumber = value;
                this.ItemsSource.forEach(function (item) {
                    item.StartNumber = value;
                });
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    CounterInvoiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CounterInvoiceComponent.prototype.OkButtonClicked = function () {
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
                var myPipe = new GroupByPipe_1.GroupByPipe();
                var myGroupbyCount = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
                if (myGroupbyCount != this.APIHelper.CounterDefinitions.length) {
                    isValidUniquePrefix = false;
                }
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
                            if ((item.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(item.Prefix) + Tools_1.AppTool.GetCounterPrefixLength(item.Suffix) > 20) {
                                errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
                            }
                        }
                    });
                }
                else {
                    if ((this.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(this.Prefix) + Tools_1.AppTool.GetCounterPrefixLength(this.Suffix) > 20) {
                        errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
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
    CounterInvoiceComponent.prototype.CalculateSampleValue = function () {
        this.SampleValue = Tools_1.AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, this.Suffix, this.CounterSize);
    };
    CounterInvoiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CounterInvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CounterInvoiceComponent);
    return CounterInvoiceComponent;
}(BaseComponent_1.BaseComponent));
exports.CounterInvoiceComponent = CounterInvoiceComponent;
var CounterInvoiceDefinitionItem = /** @class */ (function (_super) {
    __extends(CounterInvoiceDefinitionItem, _super);
    function CounterInvoiceDefinitionItem(itemPM, name, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.Name = name;
        _this.EntityPM = itemPM;
        _this.SetUIProperties();
        return _this;
    }
    CounterInvoiceDefinitionItem.prototype.SetUIProperties = function () {
        var isEnabled = true;
        var isEnabled_StartNumber = true;
        if (this.father.IsCounterUsed) {
            isEnabled = false;
            isEnabled_StartNumber = false;
        }
        else if (!this.UniquePerPrefix) {
            isEnabled_StartNumber = false;
        }
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Suffix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, isEnabled_StartNumber);
    };
    Object.defineProperty(CounterInvoiceDefinitionItem.prototype, "UniquePerPrefix", {
        get: function () { return this.EntityPM.UniquePerPrefix; },
        set: function (value) {
            if (this.EntityPM.UniquePerPrefix != value) {
                this.EntityPM.UniquePerPrefix = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceDefinitionItem.prototype, "Prefix", {
        get: function () { return this.EntityPM.Prefix; },
        set: function (value) {
            if (this.EntityPM.Prefix != value) {
                this.EntityPM.Prefix = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceDefinitionItem.prototype, "Suffix", {
        get: function () { return this.EntityPM.Suffix; },
        set: function (value) {
            if (this.EntityPM.Suffix != value) {
                this.EntityPM.Suffix = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceDefinitionItem.prototype, "CounterSize", {
        get: function () { return this.EntityPM.CounterSize; },
        set: function (value) {
            if (this.EntityPM.CounterSize != value) {
                this.EntityPM.CounterSize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterInvoiceDefinitionItem.prototype, "StartNumber", {
        get: function () { return this.EntityPM.StartNumber; },
        set: function (value) {
            if (this.EntityPM.StartNumber != value) {
                this.EntityPM.StartNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return CounterInvoiceDefinitionItem;
}(BaseComponent_1.BaseComponent));
exports.CounterInvoiceDefinitionItem = CounterInvoiceDefinitionItem;
//# sourceMappingURL=CounterInvoiceComponent.js.map