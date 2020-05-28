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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var ARInvoicePMService_1 = require("../../Services/StandardPMs/ARInvoicePMService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var FieldTemplateComponent = /** @class */ (function (_super) {
    __extends(FieldTemplateComponent, _super);
    function FieldTemplateComponent() {
        var _this = _super.call(this) || this;
        _this.FieldValue = null;
        _this.ObjectTableName = null;
        _this.IsHeaderScreenTemplate = false;
        _this.SpotlightDataTemplate = null;
        _this.IsSpotLightTemplate = false;
        _this.DataContext = _this;
        _this.DisplaySATFields = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsUnpaidInvoice = false;
        _this.ShowBusyIndicator = false;
        //AR Invoice
        _this.IsEntityLoaded = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATFields = true;
        }
        return _this;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
            }
            if (this.IsSpotLightTemplate) {
                if (this.ObjectTableName == "ARInvoice") {
                    this.myService = new ARInvoicePMService_1.ARInvoicePMService();
                    if (this.Entity.StatusCode != "DR" && this.Entity.StatusCode != "VD" && !this.Entity.IsClosed) {
                        this.IsUnpaidInvoice = true;
                    }
                    this.ShowBusyIndicator = true;
                    this.BusyIndicatorMessage = "Loading...";
                    this.LoadARInvoicePM();
                }
            }
        }
    };
    FieldTemplateComponent.prototype.LoadARInvoicePM = function () {
        var _this = this;
        this.myService.get(this.Entity.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ShowBusyIndicator = false;
                _this.EntityPM = myResponse.Result;
                if (_this.EntityPM != null) {
                    _this.IsEntityLoaded = true;
                    _this.MainEntityStatus = _this.EntityPM.MainEntityStatus;
                    _this.ExpectedPaymentDate = Tools_1.DateTool.GetDateParts(_this.EntityPM.ExpectedPaymentDate).DateObject;
                }
            }
            else {
                _this.ShowBusyIndicator = false;
            }
        }, function (error) {
            _this.ShowBusyIndicator = false;
        });
    };
    Object.defineProperty(FieldTemplateComponent.prototype, "InternalNotes", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.InternalNotes; },
        set: function (newValue) {
            if (this.EntityPM.InternalNotes != newValue) {
                this.EntityPM.InternalNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldTemplateComponent.prototype, "ExpectedPaymentDate", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ExpectedPaymentDate; },
        set: function (newValue) {
            if (this.EntityPM.ExpectedPaymentDate != newValue) {
                this.EntityPM.ExpectedPaymentDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    FieldTemplateComponent.prototype.ViewEntityClicked = function (entityType) {
        if (this.Entity != null) {
            var tableName;
            var entityId;
            if (entityType == "SHI") {
                tableName = "Shipment";
                entityId = this.Entity.MainEntityId;
            }
            else if (entityType == "INV") {
                tableName = "ARInvoice";
                entityId = this.Entity.Id;
            }
            else if (entityType == "PAY") {
                tableName = "ARPayment";
                entityId = this.arPaymentId;
            }
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
        }
    };
    FieldTemplateComponent.prototype.SaveInvoiceChanges = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            this.ShowBusyIndicator = true;
            this.BusyIndicatorMessage = "Saving...";
            this.myService.update(this.EntityPM).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                }
                _this.ShowBusyIndicator = false;
            });
        }
    };
    FieldTemplateComponent.prototype.NewPaymentClicked = function () {
        var _this = this;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "NEW")) {
            if (this.EntityPM.StatusCode == "DR") {
                var messageText = "Cant add payment for Draft invoice";
                var window = new MessageWindow_1.MessageWindow();
                window.Width = 300;
                window.Height = 150;
                window.Show(messageText);
            }
            else if (this.EntityPM.AmountDue <= 0) {
                var messageText = "Amount paid equals or bigger than invoice amount";
                var window = new MessageWindow_1.MessageWindow();
                window.Width = 400;
                window.Height = 150;
                window.Show(messageText);
            }
            else {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.WindowArgs = { ARInvoice: this.EntityPM };
                logWindow.Title = "Create new Payment";
                logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
                logWindow.ComponentLoaded.subscribe(function (comp) {
                    logWindow.WindowClosed.subscribe(function (s) {
                        if (s) {
                            _this.arPaymentId = comp.newARPaymentPM.Id;
                            _this.ViewEntityClicked("PAY");
                        }
                    });
                });
            }
        }
    };
    FieldTemplateComponent.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map