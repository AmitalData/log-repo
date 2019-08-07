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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var PaymentChequeLinePM_1 = require("../../../EntityPMs/PaymentChequeLinePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var BankAccountPMService_1 = require("../../../Services/StandardPMs/BankAccountPMService");
var PaymentChequeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(PaymentChequeGeneralTabComponent, _super);
    function PaymentChequeGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "PaymentCheque";
        _this.DataContext = _this;
        _this.Lines = new ObservableCollection_1.ObservableCollection([]);
        _this.DisableFieldsEvent = null;
        _this.AddLineEnabled = true;
        _this.visible = false;
        _this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.BankAccountPMService = new BankAccountPMService_1.BankAccountPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPM = entityArgs.EntityPM;
        _this.SetFilters();
        _this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        _this.EntityResourceService.getEntityResourceByTableName("PaymentCheque").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("PaymentChequeLine").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("ChartOfAccount").subscribe(function (response) {
                    _this.visible = true;
                    _this.AmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentChequeLine.F.Amount") + " (" + _this.entityPM.CurrencyCode + ")";
                });
            });
        });
        if (_this.entityPM.BankLocalName) {
            _this.BankName = "LocalName";
        }
        else {
            _this.BankName = "EnglishName";
        }
        _this.BuildPaymentChequeLinesList();
        if (_this.entityPM.PaymentChequeStatusCode == "2" || _this.entityPM.IsCancelled) {
            _this.DisableFieldsMethod();
        }
        if (_this.DisableFieldsEvent == null) {
            _this.DisableFieldsEvent = _this.CurrentSession.DisableFieldsEvent.subscribe(function (res) {
                _this.DisableFieldsMethod();
            });
        }
        return _this;
    }
    PaymentChequeGeneralTabComponent.prototype.SetFilters = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string");
    };
    PaymentChequeGeneralTabComponent.prototype.DisableFieldsMethod = function () {
        this.UIProperties.SetEnabled("PayToGLAccountId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("PayToName", "PaymentCheque", false);
        this.UIProperties.SetEnabled("CurrencyId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("LocalAmount", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ValueDate", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ExchangeRate", "PaymentCheque", false);
        this.UIProperties.SetEnabled("BankAccountId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ForeignAmount", "PaymentCheque", false);
        this.AddLineEnabled = false;
    };
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "PayToGLAccountId", {
        get: function () { return this.entityPM.PayToGLAccountId; },
        set: function (value) {
            if (this.entityPM.PayToGLAccountId != value) {
                this.entityPM.PayToGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "PayToName", {
        get: function () { return this.entityPM.PayToName; },
        set: function (value) {
            if (this.entityPM.PayToName != value) {
                this.entityPM.PayToName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "CurrencyId", {
        get: function () { return this.entityPM.CurrencyId; },
        set: function (value) {
            if (this.entityPM.CurrencyId != value) {
                this.entityPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "ForeignAmount", {
        get: function () { return this.entityPM.ForeignAmount; },
        set: function (value) {
            if (this.entityPM.ForeignAmount != value) {
                this.entityPM.ForeignAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "LocalAmount", {
        get: function () { return this.entityPM.LocalAmount; },
        set: function (value) {
            if (this.entityPM.LocalAmount != value) {
                this.entityPM.LocalAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "ValueDate", {
        get: function () { return this.entityPM.ValueDate; },
        set: function (value) {
            if (this.entityPM.ValueDate != value) {
                this.entityPM.ValueDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "ChequeNumber", {
        get: function () { return this.entityPM.ChequeNumber; },
        set: function (value) {
            if (this.entityPM.ChequeNumber != value) {
                this.entityPM.ChequeNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "ExchangeRate", {
        get: function () { return this.entityPM.ExchangeRate; },
        set: function (value) {
            if (this.entityPM.ExchangeRate != value) {
                this.entityPM.ExchangeRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "BankAccountId", {
        get: function () { return this.entityPM.BankAccountId; },
        set: function (value) {
            if (this.entityPM.BankAccountId != value) {
                this.entityPM.BankAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "BankName", {
        get: function () { return this.bankName; },
        set: function (value) {
            if (this.bankName != value) {
                this.bankName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "BankAccount", {
        get: function () { return this.bankAccount; },
        set: function (value) {
            if (this.bankAccount != value) {
                this.bankAccount = value;
                if (value) {
                    if (value.LocalName) {
                        this.BankName = "LocalName";
                    }
                    else {
                        this.BankName = "EnglishName";
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeGeneralTabComponent.prototype, "Account", {
        get: function () { return this.account; },
        set: function (value) {
            if (this.account != value) {
                this.account = value;
                if (value != null) {
                    if (value.AccountTypeCode == "3" && this.entityPM.APPaymentId == null) {
                        this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount"));
                    }
                    else {
                        this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, true, null);
                        this.PayToName = value.LocalName;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    PaymentChequeGeneralTabComponent.prototype.Add = function () {
        if (this.AddLineEnabled) {
            var line = 0;
            var sequence = 0;
            //if (this.entityPM.PaymentChequeLines.length > 0) {
            //    if (isNaN(this.entityPM.PaymentChequeLineLastLine)) this.entityPM.PaymentChequeLineLastLine = 0;
            //    line = this.entityPM.PaymentChequeLineLastLine;
            //    this.entityPM.PaymentChequeLineLastLine = this.entityPM.PaymentChequeLineLastLine + 1;
            //}
            //else {
            //    this.entityPM.PaymentChequeLineLastLine = 1;
            //    line = 0;
            //}
            var items = this.entityPM.PaymentChequeLines.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
            if (items.length == 0)
                sequence = 0;
            else {
                sequence = items[this.entityPM.PaymentChequeLines.length - 1].SequenceNumeric;
            }
            line += 1;
            sequence += 1;
            var item = new PaymentChequeLinePM_1.PaymentChequeLinePM(this.entityPM);
            item.PaymentChequeId = this.entityPM.Id;
            item.Tenant = this.entityPM.Tenant;
            //item.Line = line;
            item.SequenceNumeric = sequence;
            item.ChangeSetOp = "Insert";
            this.entityPM.AddPaymentChequeLine(item);
            this.BuildPaymentChequeLinesList();
        }
    };
    PaymentChequeGeneralTabComponent.prototype.BuildPaymentChequeLinesList = function () {
        this.Lines.Clear();
        for (var _i = 0, _a = this.entityPM.PaymentChequeLines; _i < _a.length; _i++) {
            var item = _a[_i];
            this.Lines.Insert(new PaymentChequeLine(item, this));
        }
    };
    PaymentChequeGeneralTabComponent.prototype.ngOnDestroy = function () {
        if (this.DisableFieldsEvent) {
            this.DisableFieldsEvent.unsubscribe();
            this.DisableFieldsEvent = null;
        }
    };
    PaymentChequeGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'PaymentChequeGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './PaymentChequeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PaymentChequeGeneralTabComponent);
    return PaymentChequeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PaymentChequeGeneralTabComponent = PaymentChequeGeneralTabComponent;
var PaymentChequeLine = /** @class */ (function (_super) {
    __extends(PaymentChequeLine, _super);
    function PaymentChequeLine(Entity, Parent) {
        var _this = _super.call(this) || this;
        _this.entity = Entity;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(PaymentChequeLine.prototype, "SequenceNumeric", {
        get: function () { return this.entity.SequenceNumeric; },
        set: function (value) {
            if (this.entity.SequenceNumeric != value) {
                this.entity.SequenceNumeric = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeLine.prototype, "Line", {
        get: function () { return this.entity.Line; },
        set: function (value) {
            if (this.entity.Line != value) {
                this.entity.Line = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeLine.prototype, "Amount", {
        get: function () { return this.entity.Amount; },
        set: function (value) {
            if (this.entity.Amount != value) {
                this.entity.Amount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentChequeLine.prototype, "Note", {
        get: function () { return this.entity.Notes; },
        set: function (value) {
            if (this.entity.Notes != value) {
                this.entity.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    PaymentChequeLine.prototype.DeleteButtonClicked = function () {
        var sequence = 1;
        this.parent.Lines.Remove(this);
        this.parent.entityPM.RemovePaymentChequeLine(this.entity);
        this.parent.Lines.Collection.forEach(function (item) {
            item.SequenceNumeric = sequence;
            sequence++;
        });
    };
    return PaymentChequeLine;
}(BaseComponent_1.BaseComponent));
exports.PaymentChequeLine = PaymentChequeLine;
//# sourceMappingURL=PaymentChequeGeneralTabComponent.js.map