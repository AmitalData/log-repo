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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var MessagingStockPM_1 = require("../../../../Shipment/EntityPMs/MessagingStockPM");
var MessagingStockPMService_1 = require("../../../../Shipment/Services/StandardPMs/MessagingStockPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AddEditAWBStockComponent = /** @class */ (function (_super) {
    __extends(AddEditAWBStockComponent, _super);
    function AddEditAWBStockComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "MessagingStock";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditAWBStockComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.IsNew = windowArgs.IsNewEntity;
        this.FatherComponent = windowArgs.FatherComponent;
        if (windowArgs.IsNewEntity) {
            this.EntityPM = new MessagingStockPM_1.MessagingStockPM();
            this.EntityPM.TenantNumber = windowArgs.FatherComponent.EntityPM.Id;
            this.EntityPM.DummyTenant = SessionLocator_1.SessionLocator.Tenant;
            this.EntityPM.StockType = "Champ";
        }
        else {
            this.LoadEntityPM(windowArgs.EntityId);
        }
        this.SetUIProperties();
        this.Clone();
    };
    AddEditAWBStockComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("TenantNumber", this.ObjectTableName, false);
        if (this.StartDate == null) {
            this.UIProperties.SetRequired("StartDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("StartDate", this.ObjectTableName, false);
        }
        if (this.EndDate == null) {
            this.UIProperties.SetRequired("EndDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("EndDate", this.ObjectTableName, false);
        }
        if (this.Amount == null) {
            this.UIProperties.SetRequired("Amount", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetRequired("Amount", this.ObjectTableName, false);
        }
    };
    AddEditAWBStockComponent.prototype.LoadEntityPM = function (id) {
        var _this = this;
        var myService = new MessagingStockPMService_1.MessagingStockPMService();
        myService.get(id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    if (_this.EntityPM != null) {
                        _this.SetUIProperties();
                    }
                }
            }
        });
    };
    Object.defineProperty(AddEditAWBStockComponent.prototype, "TenantNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.TenantNumber : 0; },
        set: function (newValue) {
            if (this.EntityPM.TenantNumber != newValue) {
                this.EntityPM.TenantNumber = newValue;
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAWBStockComponent.prototype, "StartDate", {
        get: function () { return this.EntityPM != null ? this.EntityPM.StartDate : null; },
        set: function (newValue) {
            if (this.EntityPM.StartDate != newValue) {
                this.EntityPM.StartDate = newValue;
                this.SetUIProperties();
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAWBStockComponent.prototype, "EndDate", {
        get: function () { return this.EntityPM != null ? this.EntityPM.EndDate : null; },
        set: function (newValue) {
            if (this.EntityPM.EndDate != newValue) {
                this.EntityPM.EndDate = newValue;
                this.SetUIProperties();
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAWBStockComponent.prototype, "Amount", {
        get: function () { return this.EntityPM != null ? this.EntityPM.Amount : 0; },
        set: function (newValue) {
            if (this.EntityPM.Amount != newValue) {
                this.EntityPM.Amount = newValue;
                this.SetUIProperties();
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAWBStockComponent.prototype, "TotalPrice", {
        get: function () { return this.EntityPM != null ? this.EntityPM.TotalPrice : 0; },
        set: function (newValue) {
            if (this.EntityPM.TotalPrice != newValue) {
                this.EntityPM.TotalPrice = newValue;
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAWBStockComponent.prototype, "Notes", {
        get: function () { return this.EntityPM != null ? this.EntityPM.Notes : null; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
                this.EntityPM.IsOtherFieldsChanged = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAWBStockComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAWBStockComponent.prototype.OkButtonClicked = function () {
        var isValidating = true;
        if (!this.DataContext.IsNew) {
            if (this.DataContext.EntityPM.IsTotalPriceChanged) {
                if (!this.DataContext.EntityPM.IsOtherFieldsChanged) {
                    isValidating = false;
                }
            }
        }
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        if (isValidating) {
            Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);
            if (this.DataContext.EntityPM.StartDate != null && this.DataContext.EntityPM.EndDate != null) {
                var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                if (this.DataContext.EntityPM.EndDate.valueOf() < todayDate.valueOf()) {
                    errors.push("End date cant be past date");
                }
                else if (this.DataContext.EntityPM.EndDate.valueOf() <= this.DataContext.EntityPM.StartDate.valueOf()) {
                    errors.push("End date must be bigger than start date");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNew) {
                this.CurrentSession.StartBusyIndicatorCreating();
            }
            else {
                this.CurrentSession.StartBusyIndicator("Updating...");
            }
            this.Submit();
        }
    };
    AddEditAWBStockComponent.prototype.Submit = function () {
        var _this = this;
        if (this.DataContext.EntityPM.IsDirty) {
            var myService = new MessagingStockPMService_1.MessagingStockPMService();
            if (this.DataContext.EntityPM.Id == null) {
                myService.insert(this.DataContext.EntityPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.FatherComponent.BuilItemsSource();
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }, function (error) {
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                myService.update(this.DataContext.EntityPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.FatherComponent.BuilItemsSource();
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }, function (error) {
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    AddEditAWBStockComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('TenantNumber');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('EndDate');
        this.myCloner.AddField('Amount');
        this.myCloner.AddField('TotalPrice');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.DataContext.EntityPM);
    };
    AddEditAWBStockComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAWBStockComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAWBStockComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAWBStockComponent);
    return AddEditAWBStockComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAWBStockComponent = AddEditAWBStockComponent;
//# sourceMappingURL=AddEditAWBStockComponent.js.map