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
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var ARInvoiceStockInputTemplate = /** @class */ (function (_super) {
    __extends(ARInvoiceStockInputTemplate, _super);
    function ARInvoiceStockInputTemplate(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARInvoiceStock";
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.IsEditMode = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = true;
        _this.SelectedItem = null;
        _this.Listen();
        return _this;
    }
    ARInvoiceStockInputTemplate.prototype.Listen = function () {
        var _this = this;
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "RefreshARInvoiceStockScreen") {
                _this.SetUIProperties();
            }
        });
        if (this.entityArgs != null && this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.FillStockLines();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.FillStockLines();
                }
            });
        }
    };
    ARInvoiceStockInputTemplate.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceStockInputTemplate.prototype.InitTemplate = function (args) {
        if (args != null) {
            this.EntityPM = args.Stock;
            this.IsEditMode = args.IsEditMode;
        }
        this.SetUIProperties();
        this.FillStockLines();
    };
    ARInvoiceStockInputTemplate.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.EntityPM = args.Stock;
            this.IsEditMode = args.IsEditMode;
        }
        this.FillStockLines();
    };
    ARInvoiceStockInputTemplate.prototype.SetUIProperties = function () {
        var isEditingEnabled = true;
        if (this.EntityPM.StatusCode == "C") {
            isEditingEnabled = false;
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("StartDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("EndDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isEditingEnabled);
    };
    ARInvoiceStockInputTemplate.prototype.FillStockLines = function () {
        var _this = this;
        this.ItemsSource = [];
        this.SelectedItem = null;
        this.EntityPM.ARInvoiceStockLines.forEach(function (item) {
            _this.ItemsSource.push(item);
        });
        this.ItemsCount = this.EntityPM.ARInvoiceStockLines.filter(function (d) { return !d.IsUsed; }).length;
    };
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDate; },
        set: function (newValue) {
            if (this.EntityPM.StartDate != newValue) {
                this.EntityPM.StartDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "EndDate", {
        get: function () { return this.EntityPM.EndDate; },
        set: function (newValue) {
            if (this.EntityPM.EndDate != newValue) {
                this.EntityPM.EndDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "IsAddEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsEditingEnabled) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceStockInputTemplate.prototype, "IsRemoveEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsEditingEnabled) {
                if (this.SelectedItem != null && !this.SelectedItem.IsUsed) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceStockInputTemplate.prototype.AddStockLineClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Stock Numbers";
        logWindow.WindowArgs = { Stock: this.EntityPM };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewARInvoiceStockLinesComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.IsNew = false;
                _this.FillStockLines();
            }
        });
    };
    ARInvoiceStockInputTemplate.prototype.RemoveStockLineClicked = function () {
        this.Remove(false);
    };
    ARInvoiceStockInputTemplate.prototype.RemoveSeriesClicked = function () {
        this.Remove(true);
    };
    ARInvoiceStockInputTemplate.prototype.Remove = function (isDeletingSeries) {
        var _this = this;
        if (this.SelectedItem != null) {
            var win = new MessageWindow_1.MessageWindow();
            var usedSeries = this.EntityPM.ARInvoiceStockLines.filter(function (d) { return d.CreateDate == _this.SelectedItem.CreateDate && d.IsUsed; });
            if (isDeletingSeries && this.SelectedItem.CreateDate == null) {
                win.Show("This stock does not have a create date");
            }
            else if (isDeletingSeries && usedSeries.length > 0) {
                win.Show("You can't remove this series, since it contains at least one used number");
            }
            else {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = "Removing Stock";
                confirmWindow.YesButtonText = "Delete";
                confirmWindow.NoButtonText = "Cancel";
                var message = "Are you sure you want to delete the stock number";
                var removeNotes = "";
                if (isDeletingSeries) {
                    var myGetDateFormats = Tools_1.DateTool.GetDateFormats(this.SelectedItem.CreateDate);
                    message = "Are you sure you want to delete the series inserted on";
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";
                    removeNotes = "Invoice numbers inserted on [" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] removed";
                }
                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                    removeNotes = "Invoice  number [" + this.SelectedItem.Number + "] removed";
                }
                confirmWindow.Show(message);
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        if (isDeletingSeries) {
                            var deletedSeries = _this.EntityPM.ARInvoiceStockLines.filter(function (d) { return d.CreateDate == _this.SelectedItem.CreateDate; });
                            if (deletedSeries.length > 0) {
                                deletedSeries.forEach(function (item) {
                                    _this.EntityPM.RemoveARInvoiceStockLinePM(item);
                                });
                                _this.EntityPM.SeriesRemoved = true;
                                _this.EntityPM.EventNotes = removeNotes;
                            }
                        }
                        else {
                            _this.EntityPM.RemoveARInvoiceStockLinePM(_this.SelectedItem);
                            _this.EntityPM.NumberRemoved = true;
                            _this.EntityPM.EventNotes = removeNotes;
                        }
                        _this.FillStockLines();
                    }
                });
            }
        }
    };
    ARInvoiceStockInputTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceStockInputTemplate.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceStockInputTemplate);
    return ARInvoiceStockInputTemplate;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceStockInputTemplate = ARInvoiceStockInputTemplate;
//# sourceMappingURL=ARInvoiceStockInputTemplate.js.map