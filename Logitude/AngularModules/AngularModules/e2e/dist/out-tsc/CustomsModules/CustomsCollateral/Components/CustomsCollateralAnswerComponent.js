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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CollateralsRequestFileCondPM_1 = require("../../../Customs/EntityPMs/CollateralsRequestFileCondPM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsCollateralAnswerComponent = /** @class */ (function (_super) {
    __extends(CustomsCollateralAnswerComponent, _super);
    function CustomsCollateralAnswerComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.CustomsCollateralsAnswer";
        _this.DataContext = _this;
        _this.IsClosed = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    CustomsCollateralAnswerComponent.prototype.SetTabArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.collateralPM = args.Parent;
        this.IsClosed = this.collateralPM.IsClosed;
        this.CurrentSession.SubscriptionAdd(this.CurrentSession.CollateralAnswerRefreshEvent.subscribe(function (res) {
            _this.SetClosedCollateralScreesn(res.IsClosed);
        }));
        this.firstTime = true;
        this.SetClosedCollateralScreesn(this.collateralPM.IsClosed);
        if (this.EntityPM.IsClosed) {
            this.IsClosed = true;
            this.DisplayOnlyMessageVisibility = true;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            this.AnswerSentTextVisibility = true;
            this.DisplayOnlyMessageVisibility = true;
            this.RequestNumberLabelVisibility = false;
            this.TapagFileLabelVisibility = false;
            if (this.EntityPM.AnswerForCollateralStatusCode) {
                this.AnswerForCollateralStatusVisibility = true;
            }
        }
        else {
            this.DisplayOnlyMessageVisibility = false;
        }
        if (this.EntityPM.CustomsTapgFile != null) {
            this.RequestNumberLabelVisibility = true;
            this.TapagFileLabelVisibility = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile + "/" + this.EntityPM.RequestedTapagNumeral;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagNumeral;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile + "/" + this.CustomsNumeral;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsTapgFile) && Tools_1.AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsNumeral;
            }
        }
        if (this.EntityPM.PaymentOrderId != null) {
            this.PaymentOrderVsibility = true;
            this.PaymentOrder = this.EntityPM.PaymentOrderNumber;
            this.PaymentOrderStatus = this.EntityPM.PaymentOrderStatus;
        }
    };
    CustomsCollateralAnswerComponent.prototype.SetClosedCollateralScreesn = function (IsClosed) {
        if (IsClosed) {
            this.IsClosed = true;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;
            }
            else {
                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }
            }
        }
        else {
            this.IsClosed = false;
            if (!this.EntityPM.NewFileRequest) {
                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }
                else if (this.EntityPM.AnswerEntityTypeCode == null && this.EntityPM.AllocatedAmount == null) {
                    return;
                }
                this.IsTapag = true;
                if (this.EntityPM.AnswerEntityTypeCode == null) {
                    this.AnswerEntityRedIconVisibility = true;
                }
                else {
                    this.AnswerEntityRedIconVisibility = false;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                    this.CustomsTapgFileRedIconVisibility = true;
                }
                else {
                    this.CustomsTapgFileRedIconVisibility = false;
                }
                if (this.EntityPM.AllocatedAmount == null) {
                    this.AllocatedAmountRedIconVisibility = true;
                }
                else {
                    this.AllocatedAmountRedIconVisibility = false;
                }
            }
            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;
                if (this.EntityPM.RequestFileTypeCode != null) {
                    this.RequestFileCodeRedIconVisibility = false;
                }
                else {
                    this.RequestFileCodeRedIconVisibility = true;
                }
                if (this.EntityPM.RequestFileAmount != null) {
                    this.RequestFileRedIconVisibility = false;
                }
                else {
                    this.RequestFileRedIconVisibility = true;
                }
            }
        }
    };
    CustomsCollateralAnswerComponent.prototype.UseRequestNewFile = function (newValue) {
        var _this = this;
        if (!this.collateralPM.IsClosed) {
            this.IsNewFile = null;
            this.IsTapag = null;
            if (this.EntityPM.AnswerEntityTypeCode != null || !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile) || this.EntityPM.AllocatedAmount != null || !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomsNumeral) || !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Remarks)) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateral"));
                this.timerToken = setTimeout(function () {
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.IsNewFile = true;
                            _this.IsTapag = false;
                            if (_this.EntityPM.NewFileRequest) {
                                _this.CustomsTapgFile = null;
                                _this.AllocatedAmount = null;
                                _this.AnswerEntityTypeCode = null;
                                _this.CustomsNumeral = null;
                                _this.Remarks = null;
                                _this.AnswerEntityRedIconVisibility = false;
                                _this.AllocatedAmountRedIconVisibility = false;
                                _this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                _this.RequestFileAmount = null;
                                _this.RequestFileTypeCode = null;
                                _this.RequestFileCodeRedIconVisibility = false;
                                _this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            _this.IsNewFile = false;
                            _this.IsTapag = true;
                        }
                    });
                }, 200);
            }
            else {
                this.IsNewFile = true;
                this.IsTapag = false;
                this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);
            }
            this.AnswerEntityRedIconVisibility = false;
            this.AllocatedAmountRedIconVisibility = false;
            this.CustomsTapgFileRedIconVisibility = false;
            //else {
            //    this.AnswerEntityRedIconVisibility = false;
            //    this.AllocatedAmountRedIconVisibility = false;
            //    this.CustomsTapgFileRedIconVisibility = false;
            //}
        }
    };
    CustomsCollateralAnswerComponent.prototype.UseExistingTapagFile = function (newValue) {
        var _this = this;
        if (!this.collateralPM.IsClosed) {
            this.IsTapag = null;
            this.IsNewFile = null;
            if (this.EntityPM.RequestFileAmount != null || this.EntityPM.RequestFileTypeCode != null) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateral"));
                this.timerToken = setTimeout(function () {
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.IsTapag = true;
                            _this.IsNewFile = false;
                            if (_this.EntityPM.NewFileRequest) {
                                _this.CustomsTapgFile = null;
                                _this.AllocatedAmount = null;
                                _this.AnswerEntityTypeCode = null;
                                _this.CustomsNumeral = null;
                                _this.Remarks = null;
                                _this.AnswerEntityRedIconVisibility = false;
                                _this.AllocatedAmountRedIconVisibility = false;
                                _this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                _this.RequestFileAmount = null;
                                _this.RequestFileTypeCode = null;
                                _this.RequestFileCodeRedIconVisibility = false;
                                _this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            _this.IsTapag = false;
                            _this.IsNewFile = true;
                        }
                    });
                }, 1000);
            }
            else {
                this.IsTapag = true;
                this.IsNewFile = false;
                this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);
            }
            this.RequestFileCodeRedIconVisibility = false;
            this.RequestFileRedIconVisibility = false;
            //this.RequestFileCodeRedIconVisibility = false
            //this.RequestFileRedIconVisibility = false;
        }
    };
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "PaymentOrderStatus", {
        get: function () { return this.paymentOrderStatus; },
        set: function (newValue) {
            this.paymentOrderStatus = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "PaymentOrder", {
        get: function () { return this.paymentOrder; },
        set: function (newValue) {
            this.paymentOrder = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "IsNewFile", {
        get: function () { return this.isNewFile; },
        set: function (newValue) {
            this.isNewFile = newValue;
            if (newValue && !this.collateralPM.IsClosed) {
                this.IsTapag = false;
                this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
                this.EntityPM.NewFileRequest = true;
                if (this.EntityPM.RequestFileTypeCode != null) {
                    this.RequestFileCodeRedIconVisibility = false;
                }
                else {
                    this.RequestFileCodeRedIconVisibility = true;
                }
                if (this.EntityPM.RequestFileAmount != null) {
                    this.RequestFileRedIconVisibility = false;
                }
                else {
                    this.RequestFileRedIconVisibility = true;
                }
                //    this.firstTime = false;
                if ((this.EntityPM.CollateralsRequestFileConds == null || this.EntityPM.CollateralsRequestFileConds.length == 0) && (this.collateralPM != null && this.collateralPM.CustomsCollateralsConditions != null)) {
                    for (var _i = 0, _a = this.collateralPM.CustomsCollateralsConditions; _i < _a.length; _i++) {
                        var item = _a[_i];
                        var collateralsRequestFileCond = new CollateralsRequestFileCondPM_1.CollateralsRequestFileCondPM(this.collateralPM);
                        collateralsRequestFileCond.CustomsCollateralId = this.EntityPM.CustomsCollateralId;
                        collateralsRequestFileCond.LineNumber = this.EntityPM.LineNumber;
                        collateralsRequestFileCond.Tenant = this.EntityPM.Tenant;
                        collateralsRequestFileCond.ConditionCode = item.ConditionCode;
                        collateralsRequestFileCond.ConditionName = item.ConditionName;
                        collateralsRequestFileCond.RequestedAmount = item.RequestedAmount;
                        this.EntityPM.AddCollateralsRequestFileCond(collateralsRequestFileCond);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "IsTapag", {
        get: function () { return this.isTapag; },
        set: function (newValue) {
            this.isTapag = newValue;
            if (newValue && !this.collateralPM.IsClosed) {
                this.IsNewFile = false;
                this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);
                this.EntityPM.NewFileRequest = false;
                if (this.EntityPM.AnswerEntityTypeCode == null) {
                    this.AnswerEntityRedIconVisibility = true;
                }
                else {
                    this.AnswerEntityRedIconVisibility = false;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                    this.CustomsTapgFileRedIconVisibility = true;
                }
                else {
                    this.CustomsTapgFileRedIconVisibility = false;
                }
                if (this.EntityPM.AllocatedAmount == null) {
                    this.AllocatedAmountRedIconVisibility = true;
                }
                else {
                    this.AllocatedAmountRedIconVisibility = false;
                }
                //  this.firstTime = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "DisplayOnlyMessageVisibility", {
        get: function () { return this.displayOnlyMessageVisibility; },
        set: function (newValue) { this.displayOnlyMessageVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "TapagFile", {
        get: function () { return this.tapagFile; },
        set: function (newValue) { this.tapagFile = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestNumber", {
        get: function () { return this.requestNumber; },
        set: function (newValue) { this.requestNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AnswerEntityTypeCode", {
        get: function () { return this.EntityPM ? this.EntityPM.AnswerEntityTypeCode : null; },
        set: function (newValue) {
            this.EntityPM.AnswerEntityTypeCode = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.AnswerEntityRedIconVisibility = true;
            }
            else {
                this.AnswerEntityRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AnswerForCollateralStatusName", {
        get: function () { return this.EntityPM ? this.EntityPM.AnswerForCollateralStatusName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AllocatedAmount", {
        get: function () { return this.EntityPM ? this.EntityPM.AllocatedAmount : null; },
        set: function (newValue) {
            this.EntityPM.AllocatedAmount = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.AllocatedAmountRedIconVisibility = true;
            }
            else {
                this.AllocatedAmountRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "CustomsTapgFile", {
        get: function () { return this.EntityPM ? this.EntityPM.CustomsTapgFile : null; },
        set: function (newValue) {
            this.EntityPM.CustomsTapgFile = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.CustomsTapgFileRedIconVisibility = true;
            }
            else {
                this.CustomsTapgFileRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "CustomsNumeral", {
        get: function () { return this.EntityPM ? this.EntityPM.CustomsNumeral : null; },
        set: function (newValue) { this.EntityPM.CustomsNumeral = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "Remarks", {
        get: function () { return this.EntityPM ? this.EntityPM.Remarks : null; },
        set: function (newValue) { this.EntityPM.Remarks = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestFileTypeName", {
        get: function () { return this.EntityPM ? this.EntityPM.RequestFileTypeName : null; },
        set: function (newValue) { this.EntityPM.RequestFileTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestFileTypeCode", {
        get: function () { return this.EntityPM ? this.EntityPM.RequestFileTypeCode : null; },
        set: function (newValue) {
            this.EntityPM.RequestFileTypeCode = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.RequestFileCodeRedIconVisibility = true;
            }
            else {
                this.RequestFileCodeRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestFileAmount", {
        get: function () { return this.EntityPM ? this.EntityPM.RequestFileAmount : null; },
        set: function (newValue) {
            this.EntityPM.RequestFileAmount = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.RequestFileRedIconVisibility = true;
            }
            else {
                this.RequestFileRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "NewFileRequest", {
        get: function () { return this.EntityPM ? this.EntityPM.NewFileRequest : null; },
        set: function (newValue) {
            this.EntityPM.NewFileRequest = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "CustomsTapgFileRedIconVisibility", {
        get: function () { return this.customsTapgFileRedIconVisibility; },
        set: function (newValue) { this.customsTapgFileRedIconVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AllocatedAmountRedIconVisibility", {
        get: function () { return this.allocatedAmountRedIconVisibility; },
        set: function (newValue) { this.allocatedAmountRedIconVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AnswerEntityRedIconVisibility", {
        get: function () { return this.answerEntityRedIconVisibility; },
        set: function (newValue) { this.answerEntityRedIconVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestFileRedIconVisibility", {
        get: function () { return this.requestFileRedIconVisibility; },
        set: function (newValue) { this.requestFileRedIconVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestFileCodeRedIconVisibility", {
        get: function () { return this.requestFileCodeRedIconVisibility; },
        set: function (newValue) { this.requestFileCodeRedIconVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "PaymentOrderVsibility", {
        get: function () { return this.paymentOrderVsibility; },
        set: function (newValue) { this.paymentOrderVsibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "TapagFileLabelVisibility", {
        get: function () { return this.tapagFileLabelVisibility; },
        set: function (newValue) { this.tapagFileLabelVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "RequestNumberLabelVisibility", {
        get: function () { return this.requestNumberLabelVisibility; },
        set: function (newValue) { this.requestNumberLabelVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralAnswerComponent.prototype, "AnswerSentTextVisibility", {
        get: function () { return this.answerSentTextVisibility; },
        set: function (newValue) { this.answerSentTextVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    CustomsCollateralAnswerComponent.prototype.OpenPaymentOrder = function () {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(function (response) {
                            _this.EditEntity("Customs.PaymentOrder", _this.EntityPM.PaymentOrderId, null, "POGN");
                        });
                    });
                });
            });
        });
    };
    CustomsCollateralAnswerComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    CustomsCollateralAnswerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsCollateralAnswerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], CustomsCollateralAnswerComponent);
    return CustomsCollateralAnswerComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsCollateralAnswerComponent = CustomsCollateralAnswerComponent;
//# sourceMappingURL=CustomsCollateralAnswerComponent.js.map