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
var APInvoiceLinePM_1 = require("../../../../Invoice/EntityPMs/APInvoiceLinePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var EditMultipleShipmentComponent = /** @class */ (function (_super) {
    __extends(EditMultipleShipmentComponent, _super);
    function EditMultipleShipmentComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.EntityShipmentPM = null;
        _this.ObjectTableName = "APInvoice";
        _this.DataContext = _this;
        _this.IsEditingEnabled = false;
        _this.InvoiceCurrencyCode = null;
        _this.LocalCurrencyId = null;
        _this.LocalCurrencyCode = null;
        _this.ValidationErrorsList = [];
        _this.ShipmentId = null;
        _this.APInvoiceId = null;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.VatTypeFilterIsEnabled = false;
        _this.myOpenPayables = [];
        _this.LastRatesList = [];
        _this.VatTypePercentagesList = [];
        _this.SummeryAmount = 0;
        _this.SummeryOpenAmount = 0;
        _this.SummeryCurrencyCode = _this.InvoiceCurrencyCode;
        _this.isTotalInLocalCurrency = false;
        _this.isAllChecked = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.InitializeServices();
        return _this;
    }
    EditMultipleShipmentComponent.prototype.InitializeServices = function () {
        this.myInvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.myVatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
    };
    EditMultipleShipmentComponent.prototype.SetWindowArgs = function (args) {
        this.APInvoicePM = args['APInvoicePM'];
        this.EntityShipmentPM = args['EntityShipmentPM'];
        this.IsEditingEnabled = args['IsEditingEnabled'];
        this.ShipmentId = this.EntityShipmentPM.ShipmentId;
        this.APInvoiceId = this.APInvoicePM.Id;
        this.InvoiceCurrencyCode = this.APInvoicePM.InvoiceCurrencyCode;
        this.SetUIProperties();
        this.LoadEntity();
    };
    EditMultipleShipmentComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_VatTypeFilter();
    };
    EditMultipleShipmentComponent.prototype.SetUIProperties_VatTypeFilter = function () {
        var isFieldtEnabled = this.IsEditingEnabled;
        if (isFieldtEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                isFieldtEnabled = false;
            }
        }
        this.VatTypeFilterIsEnabled = isFieldtEnabled;
    };
    EditMultipleShipmentComponent.prototype.LoadEntity = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myInvoiceDomainService.GetSingleAPInvoiceShortPM(this.APInvoiceId, this.ShipmentId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                if (_this.IsEditingEnabled) {
                    _this.LoadOpenPayables();
                }
                else {
                    _this.BuildItemsSource();
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    EditMultipleShipmentComponent.prototype.LoadOpenPayables = function () {
        var _this = this;
        this.myShipmentDomainService.GetInvoiceOpenAmountPayables(this.ShipmentId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.myOpenPayables = myResponse.Result;
            }
            var loadingDate = _this.APInvoicePM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            _this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.AccountingCurrencyId, loadingDate).subscribe(function (myResponse1) {
                _this.LastRatesList = myResponse1.Result;
                _this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe(function (myResponse2) {
                    _this.VatTypePercentagesList = myResponse2.Result;
                    _this.BuildItemsSource();
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
        });
    };
    EditMultipleShipmentComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource.Clear();
        this.RefreshSummery();
        this.EntityPM.InvoiceLines.forEach(function (line) {
            _this.ItemsSource.Insert(new APInvoiceLineShortItem(line, _this, false));
        });
        this.myOpenPayables = this.myOpenPayables.filter(function (f) { return f.ShipmentPayableParentId == null; });
        this.myOpenPayables.forEach(function (item) {
            if (_this.EntityPM.InvoiceLines.filter(function (f) { return f.EntityPayableId == item.Id; }).length == 0) {
                var line = new APInvoiceLinePM_1.APInvoiceLinePM(null);
                line.APInvoiceId = _this.APInvoiceId;
                line.Tenant = item.Tenant;
                line.ChargesTypeId = item.ChargesTypeId;
                line.ChargesTypeCode = item.ChargesTypeCode;
                line.ChargesTypeName = item.ChargesTypeName;
                line.EntityPayableId = item.Id;
                line.EntityId = item.ShipmentId;
                line.EntityReference = item.ShipmentNumber;
                line.VendorId = item.VendorId;
                line.VendorName = item.VendorName;
                line.ExpectedAmount = item.ExpectedAmount;
                line.OtherInvoicesAmounts = item.AccountedAmount;
                line.OpenAmount = item.OpenAmount;
                line.CorrectionAmount = item.CorrectionAmount;
                line.CorrectionNote = item.CorrectionNote;
                line.CorrectionByUserId = item.CorrectionByUserId;
                line.CorrectionDate = item.CorrectionDate;
                line.AmountTypeCode = item.ShipmentPayableAmountTypeCode;
                line.ForiegnCurrencyId = item.CurrencyId;
                line.ForiegnCurrencyCode = item.CurrencyCode;
                line.VatPercentage = _this.GetVatTypePercentage(item.VatTypeId);
                line.ForiegnExchangeRate = _this.GetCurrencyRate(item.CurrencyId);
                _this.myChargesTypeListService.getSingleFromCache(line.ChargesTypeId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            line.Description = list.EnglishName;
                            line.LocalDescription = list.LocalName;
                            if (Tools_1.AppTool.IsNullOrEmpty(line.VatTypeId)) {
                                line.VatTypeId = list.VatTypeId;
                            }
                        }
                    }
                });
                if (!Tools_1.AppTool.IsNullOrEmpty(line.VatTypeId)) {
                    _this.myVatTypeListService.getSingleFromCache(line.VatTypeId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list_VAT = myResponse.Result;
                            if (list_VAT) {
                                line.VatTypeName = list_VAT.EnglishName;
                                line.VatIsMultiPercentage = list_VAT.IsMultiPercentage;
                                if (!list_VAT.IsMultiPercentage) {
                                    line.VatPercentage = _this.GetVatTypePercentage(line.VatTypeId);
                                }
                            }
                        }
                    });
                }
                _this.ItemsSource.Insert(new APInvoiceLineShortItem(line, _this, false));
            }
        });
        this.RefreshSummery();
        this.CheckIsAllChecked();
    };
    EditMultipleShipmentComponent.prototype.GetCurrencyRate = function (myCurrencyId) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }
            else {
                var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == myCurrencyId; })[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }
        return myResult;
    };
    EditMultipleShipmentComponent.prototype.GetVatTypePercentage = function (myVatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.VatTypePercentagesList.filter(function (d) { return d.VatTypeId == myVatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "House", {
        get: function () { return this.EntityShipmentPM.House; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "Master", {
        get: function () { return this.EntityShipmentPM.Master; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "LongMaster", {
        get: function () { return this.EntityShipmentPM.LongMaster; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "ShipmentNumber", {
        get: function () { return this.EntityShipmentPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "PartnerName", {
        get: function () { return this.EntityShipmentPM.PartnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "PartnerType", {
        get: function () { return this.EntityShipmentPM.PartnerType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "MainCarriageCarrierName", {
        get: function () { return this.EntityShipmentPM.MainCarriageCarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "OpenAmount", {
        get: function () { return this.EntityShipmentPM.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "VatTypeId", {
        get: function () { return this.vatTypeId; },
        set: function (newValue) {
            if (this.vatTypeId != newValue) {
                this.vatTypeId = newValue;
                this.SetUIProperties_VatTypeFilter();
            }
        },
        enumerable: true,
        configurable: true
    });
    EditMultipleShipmentComponent.prototype.VatTypeFilterClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            this.ItemsSource.Collection.forEach(function (item) {
                item.VatTypeId = _this.VatTypeId;
            });
            this.VatTypeId = null;
            this.ComputeTotals();
        }
    };
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "IsCurrencyFilterVisible", {
        // Totals
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.APInvoicePM.InvoiceCurrencyId)) {
                if (this.LocalCurrencyId != this.APInvoicePM.InvoiceCurrencyId) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "IsTotalInLocalCurrency", {
        get: function () { return this.isTotalInLocalCurrency; },
        set: function (value) {
            if (this.isTotalInLocalCurrency != value) {
                this.isTotalInLocalCurrency = value;
                this.RefreshSummery();
            }
        },
        enumerable: true,
        configurable: true
    });
    EditMultipleShipmentComponent.prototype.RefreshSummery = function () {
        var _this = this;
        var mySummeryAmount = 0;
        var mySummeryOpenAmount = 0;
        var mySummeryCurrencyCode = this.InvoiceCurrencyCode;
        if (this.EntityPM) {
            if (this.IsTotalInLocalCurrency) {
                mySummeryAmount = Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount");
                mySummeryCurrencyCode = this.LocalCurrencyCode;
                this.EntityPM.InvoiceLines.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.OpenAmount) && !Tools_1.AppTool.IsNullOrEmpty(item.ForiegnExchangeRate)) {
                        mySummeryOpenAmount = mySummeryOpenAmount + (item.OpenAmount * item.ForiegnExchangeRate);
                    }
                });
            }
            else {
                mySummeryAmount = Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount");
                mySummeryCurrencyCode = this.InvoiceCurrencyCode;
                this.EntityPM.InvoiceLines.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.OpenAmount) && !Tools_1.AppTool.IsNullOrEmpty(item.ForiegnExchangeRate)) {
                        mySummeryOpenAmount = mySummeryOpenAmount + (item.OpenAmount * item.ForiegnExchangeRate / _this.EntityPM.InvoiceCurrencyExchangeRate);
                    }
                });
            }
        }
        this.SummeryAmount = mySummeryAmount;
        this.SummeryOpenAmount = mySummeryOpenAmount;
        this.SummeryCurrencyCode = mySummeryCurrencyCode;
    };
    EditMultipleShipmentComponent.prototype.ComputeTotals = function () {
        this.RefreshSummery();
        var subInLocal = Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount");
        var subInInvoice = Tools_1.ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount");
        this.EntityPM.SubTotalInLocalCurrency = Tools_1.AppTool.Round(subInLocal, 2);
        this.EntityPM.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(subInInvoice, 2);
    };
    EditMultipleShipmentComponent.prototype.AddLineClicked = function () {
        var line = new APInvoiceLinePM_1.APInvoiceLinePM(null);
        line.Tenant = this.APInvoicePM.Tenant;
        line.APInvoiceId = this.APInvoicePM.Id;
        line.VendorId = this.APInvoicePM.VendorId;
        line.VendorName = this.APInvoicePM.VendorName;
        line.EntityId = this.EntityShipmentPM.ShipmentId;
        line.EntityReference = this.EntityShipmentPM.ShipmentNumber;
        line.AmountTypeCode = "NEXP";
        line.ForiegnCurrencyId = this.APInvoicePM.InvoiceCurrencyId;
        line.ForiegnCurrencyCode = this.APInvoicePM.InvoiceCurrencyCode;
        line.ForiegnExchangeRate = this.APInvoicePM.InvoiceCurrencyExchangeRate;
        var itemComponent = new APInvoiceLineShortItem(line, this, true);
        this.RunWindow(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.O.AddInvoiceLine"));
    };
    EditMultipleShipmentComponent.prototype.EditLineClicked = function (itemComponent) {
        this.RunWindow(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.O.EditInvoiceLine"));
    };
    EditMultipleShipmentComponent.prototype.RunWindow = function (itemComponent, titile) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = titile;
        logWindow.DataContext = itemComponent;
        logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditMultipleAPInvoiceLineComponent');
    };
    EditMultipleShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditMultipleShipmentComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.EntityPM.InvoiceLines.filter(function (f) { return f.EntityId == _this.ShipmentId; }).length == 0) {
            errors.push("You sould add invoice lines");
        }
        else {
            this.EntityPM.InvoiceLines.forEach(function (item) {
                Validator_1.Validator.TryValidateObject(item, "APInvoiceLine", errors);
            });
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myInvoiceDomainService.PutSingleAPInvoiceShortPM(this.EntityPM).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    // IsAllChecked
    EditMultipleShipmentComponent.prototype.CheckIsAllChecked = function () {
        var myResult = false;
        if (this.ItemsSource) {
            if (this.ItemsSource.Collection.length > 0) {
                var allItems_NotChecked = this.ItemsSource.Collection.filter(function (f) { return f.IsChecked == false; });
                if (allItems_NotChecked.length == 0) {
                    myResult = true;
                }
                else if (allItems_NotChecked.filter(function (f) { return f.IsMatch == true; }).length == 0) {
                    myResult = true;
                }
            }
        }
        this.isAllChecked = myResult;
    };
    Object.defineProperty(EditMultipleShipmentComponent.prototype, "IsAllChecked", {
        get: function () { return this.isAllChecked; },
        set: function (value) {
            var _this = this;
            if (this.isAllChecked != value) {
                this.isAllChecked = value;
                var items = this.ItemsSource.Collection.filter(function (f) { return f.IsMatch == true; });
                if (items.length > 0) {
                    if (value) {
                        items.filter(function (f) { return f.IsChecked == false; }).forEach(function (item) {
                            item.ForiegnCurrencyAmount = item.OpenAmount;
                            _this.EntityPM.AddInvoiceLinePM(item.EntityPM);
                            item.isChecked = value;
                            item.SetCellColor();
                        });
                    }
                    else {
                        items.filter(function (f) { return f.IsChecked == true; }).forEach(function (item) {
                            item.ForiegnCurrencyAmount = null;
                            _this.EntityPM.RemoveInvoiceLinePM(item.EntityPM);
                            item.isChecked = value;
                            item.SetCellColor();
                        });
                    }
                    this.ComputeTotals();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EditMultipleShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditMultipleShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditMultipleShipmentComponent);
    return EditMultipleShipmentComponent;
}(BaseComponent_1.BaseComponent));
exports.EditMultipleShipmentComponent = EditMultipleShipmentComponent;
var APInvoiceLineShortItem = /** @class */ (function (_super) {
    __extends(APInvoiceLineShortItem, _super);
    function APInvoiceLineShortItem(entityPM, fatherComponent, isAddNewLineMode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.EntityPM = null;
        _this.ObjectTableName = "APInvoiceLine";
        _this.DataContext = _this;
        _this.ProfitCurrencyId = null;
        _this.InvoiceCurrencyId = null;
        _this.InvoiceCurrencyCode = null;
        _this.IsMatch = true;
        _this.IsEditingEnabled = false;
        _this.AddNewLineMode = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CellBackgroundColor = Tools_1.FontTool.CellDisabledBackground;
        _this.CellBackgroundColor_Editable = "transparent";
        _this.isChecked = false;
        _this.VatTypeUpdateIsVisible = false;
        _this.VatTypeMultiIconVisible = false;
        _this.VatTypesGroups = [];
        _this.correctionByUserName = null;
        _this.EntityPM = entityPM;
        _this.APInvoicePM = fatherComponent.APInvoicePM;
        _this.ProfitCurrencyId = _this.APInvoicePM.ProfitCurrencyId;
        _this.InvoiceCurrencyId = _this.APInvoicePM.InvoiceCurrencyId;
        _this.InvoiceCurrencyCode = _this.APInvoicePM.InvoiceCurrencyCode;
        _this.AddNewLineMode = isAddNewLineMode;
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.VendorId)) {
            if (_this.VendorId != _this.APInvoicePM.VendorId) {
                _this.IsMatch = false;
            }
        }
        if (_this.fatherComponent.EntityPM.InvoiceLines.indexOf(_this.EntityPM) > -1) {
            _this.isChecked = true;
        }
        if (_this.fatherComponent.IsEditingEnabled && _this.IsMatch) {
            _this.IsEditingEnabled = true;
        }
        _this.SetCellColor();
        _this.ReadVatTypeData();
        _this.SetUIProperties();
        _this.GetCorrectionUserName();
        return _this;
    }
    APInvoiceLineShortItem.prototype.GetCorrectionUserName = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CorrectionByUserId)) {
            this.fatherComponent.myUserListService.getSingleFromCache(this.CorrectionByUserId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    };
    APInvoiceLineShortItem.prototype.SetCellColor = function () {
        if (this.IsChecked) {
            this.CellBackgroundColor = Tools_1.FontTool.CellIsCheckedBackground;
            this.CellBackgroundColor_Editable = Tools_1.FontTool.CellIsCheckedBackground;
        }
        else {
            this.CellBackgroundColor = Tools_1.FontTool.CellDisabledBackground;
            if (!this.IsEditingEnabled) {
                this.CellBackgroundColor_Editable = Tools_1.FontTool.CellDisabledBackground;
            }
            else {
                this.CellBackgroundColor_Editable = "transparent";
            }
        }
    };
    APInvoiceLineShortItem.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        var isOpenAmountEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.AmountTypeCode != "NEXP") {
                isOpenAmountEnabled = true;
            }
        }
        var isChargeEnabled = false;
        if (this.IsEditingEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EntityPayableId)) {
                isChargeEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, isOpenAmountEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled);
        this.SetUIProperties_VAT();
    };
    APInvoiceLineShortItem.prototype.SetUIProperties_VAT = function () {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);
        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }
        var isVatPercentageRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.VatPercentage)) {
            isVatPercentageRequired = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (this.VatIsMultiPercentage) {
                    isVatPercentageRequired = false;
                }
            }
        }
        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    };
    Object.defineProperty(APInvoiceLineShortItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    this.fatherComponent.EntityPM.AddInvoiceLinePM(this.EntityPM);
                }
                else {
                    this.fatherComponent.EntityPM.RemoveInvoiceLinePM(this.EntityPM);
                }
                this.SetCellColor();
                this.fatherComponent.ComputeTotals();
                this.fatherComponent.CheckIsAllChecked();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ChargesTypeId", {
        // ChargesTypeId
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ChargesTypeCode = null;
                    this.ChargesTypeName = null;
                    this.Description = null;
                    this.LocalDescription = null;
                    this.VatTypeId = null;
                }
                else {
                    this.fatherComponent.myChargesTypeListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.ChargesTypeCode = list.Code;
                                _this.ChargesTypeName = list.EnglishName;
                                _this.Description = list.EnglishName;
                                _this.LocalDescription = list.LocalName;
                                _this.VatTypeId = list.VatTypeId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeCode != value) {
                this.EntityPM.ChargesTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeName != value) {
                this.EntityPM.ChargesTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "LocalDescription", {
        get: function () { return this.EntityPM.LocalDescription; },
        set: function (value) {
            if (this.EntityPM.LocalDescription != value) {
                this.EntityPM.LocalDescription = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VatTypeId", {
        // VatTypeId
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.VatTypeId != value) {
                this.EntityPM.VatTypeId = value;
                this.VatPercentage = this.fatherComponent.GetVatTypePercentage(value);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.VatTypeName = null;
                    this.VatPercentage = null;
                    this.VatIsMultiPercentage = false;
                    this.EntityPM.ExternalTAXItemId = null;
                    this.ReadVatTypeData();
                    this.SetUIProperties_VAT();
                }
                else {
                    this.fatherComponent.myVatTypeListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VatTypeName = list.EnglishName;
                                _this.VatIsMultiPercentage = list.IsMultiPercentage;
                                _this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;
                                if (list.IsMultiPercentage) {
                                    _this.VatPercentage = null;
                                }
                                else {
                                    _this.VatPercentage = _this.fatherComponent.GetVatTypePercentage(_this.VatTypeId);
                                }
                                _this.ReadVatTypeData();
                                _this.SetUIProperties_VAT();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VatTypeName", {
        get: function () { return this.EntityPM.VatTypeName; },
        set: function (value) {
            if (this.EntityPM.VatTypeName != value) {
                this.EntityPM.VatTypeName = value;
                this.ReadVatTypeData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VatPercentage", {
        get: function () { return this.EntityPM.VatPercentage; },
        set: function (value) {
            if (this.EntityPM.VatPercentage != value) {
                this.EntityPM.VatPercentage = value;
                this.ReadVatTypeData();
                this.fatherComponent.ComputeTotals();
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VatIsMultiPercentage", {
        get: function () { return this.EntityPM.VatIsMultiPercentage; },
        set: function (value) {
            if (this.EntityPM.VatIsMultiPercentage != value) {
                this.EntityPM.VatIsMultiPercentage = value;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineShortItem.prototype.ReadVatTypeData = function () {
        var _this = this;
        var myValue = null;
        var myColor = Tools_1.FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = Tools_1.FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator_1.SessionLocator.AllVatTypesGroups.filter(function (f) { return f.GroupVATTypeId == _this.VatTypeId; });
            }
            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + this.VatPercentage + "%)";
                myColor = Tools_1.FontTool.Black;
            }
            else {
                myValue = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = Tools_1.FontTool.Red;
                isUpdateVisible = true;
            }
        }
        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    };
    APInvoiceLineShortItem.prototype.UpdateVatPercentageClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.VatTypeId == _this.VatTypeId; }).forEach(function (item) {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
        }
    };
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ForiegnCurrencyId", {
        // Properties
        get: function () { return this.EntityPM.ForiegnCurrencyId; },
        set: function (value) {
            if (this.EntityPM.ForiegnCurrencyId != value) {
                this.EntityPM.ForiegnCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ForiegnCurrencyCode", {
        get: function () { return this.EntityPM.ForiegnCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.ForiegnCurrencyCode != value) {
                this.EntityPM.ForiegnCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (value) {
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (value) {
            if (this.EntityPM.VendorName != value) {
                this.EntityPM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ExpectedAmount", {
        // Amounts
        get: function () {
            var myResult = 0;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExpectedAmount)) {
                myResult = this.EntityPM.ExpectedAmount;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "OtherInvoicesAmounts", {
        get: function () {
            var myResult = 0;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherInvoicesAmounts)) {
                myResult = this.EntityPM.OtherInvoicesAmounts;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ForiegnCurrencyAmount", {
        get: function () { return this.EntityPM.ForiegnCurrencyAmount; },
        set: function (value) {
            if (this.EntityPM.ForiegnCurrencyAmount != value) {
                this.EntityPM.ForiegnCurrencyAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeOpenAmount();
                this.ComputeOtherAmounts();
                if (!this.AddNewLineMode) {
                    if (Tools_1.AppTool.IsNullOrZero(value)) {
                        this.IsChecked = false;
                    }
                    else {
                        this.IsChecked = true;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "InvoiceCurrencyAmount", {
        get: function () { return this.EntityPM.InvoiceCurrencyAmount; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyAmount != value) {
                this.EntityPM.InvoiceCurrencyAmount = Tools_1.AppTool.Round(value, 2);
                var valueInLocal = null;
                var foriegnAmount = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    valueInLocal = value * this.APInvoicePM.InvoiceCurrencyExchangeRate;
                    foriegnAmount = valueInLocal / this.EntityPM.ForiegnExchangeRate;
                }
                this.EntityPM.ForiegnCurrencyAmount = Tools_1.AppTool.Round(foriegnAmount, 2);
                if (!this.AddNewLineMode) {
                    if (Tools_1.AppTool.IsNullOrZero(foriegnAmount)) {
                        this.IsChecked = false;
                    }
                    else {
                        this.IsChecked = true;
                    }
                }
                this.ComputeOpenAmount();
                this.LocalCurrencyAmount = valueInLocal;
                if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
                    this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
                }
                else {
                    this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.APInvoicePM.ProfitCurrencyExchangeRate;
                }
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "LocalCurrencyAmount", {
        get: function () { return this.EntityPM.LocalCurrencyAmount; },
        set: function (value) {
            if (this.EntityPM.LocalCurrencyAmount != value) {
                this.EntityPM.LocalCurrencyAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "ProfitCurrencyAmount", {
        get: function () { return this.EntityPM.ProfitCurrencyAmount; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyAmount != value) {
                this.EntityPM.ProfitCurrencyAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount; },
        set: function (value) {
            var xValue = (value == null) ? 0 : value;
            if (this.EntityPM.OpenAmount != xValue) {
                this.EntityPM.OpenAmount = Tools_1.AppTool.Round(xValue, 2);
                var expect = this.ExpectedAmount == null ? 0 : this.ExpectedAmount;
                var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
                var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
                var corre = expect - others - amount - xValue;
                this.CorrectionAmount = corre;
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "CorrectionAmount", {
        get: function () { return this.EntityPM.CorrectionAmount; },
        set: function (value) {
            if (this.EntityPM.CorrectionAmount != value) {
                this.EntityPM.CorrectionAmount = Tools_1.AppTool.Round(value, 2);
                this.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.CorrectionDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "CorrectionByUserId", {
        get: function () { return this.EntityPM.CorrectionByUserId; },
        set: function (value) {
            if (this.EntityPM.CorrectionByUserId != value) {
                this.EntityPM.CorrectionByUserId = value;
                this.GetCorrectionUserName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "CorrectionByUserName", {
        get: function () { return this.correctionByUserName; },
        set: function (value) {
            if (this.correctionByUserName != value) {
                this.correctionByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "CorrectionDate", {
        get: function () { return this.EntityPM.CorrectionDate; },
        set: function (value) {
            if (this.EntityPM.CorrectionDate != value) {
                this.EntityPM.CorrectionDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceLineShortItem.prototype, "CorrectionNote", {
        get: function () { return this.EntityPM.CorrectionNote; },
        set: function (value) {
            if (this.EntityPM.CorrectionNote != value) {
                this.EntityPM.CorrectionNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceLineShortItem.prototype.ComputeOpenAmount = function () {
        if (this.EntityPM.AmountTypeCode == "NEXP") {
            this.EntityPM.OpenAmount = null;
        }
        else {
            var expect = this.ExpectedAmount;
            var amount = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre = this.CorrectionAmount == null ? 0 : this.CorrectionAmount;
            var open = expect - others - amount - corre;
            this.EntityPM.OpenAmount = Tools_1.AppTool.Round(open, 2);
        }
    };
    APInvoiceLineShortItem.prototype.ComputeOtherAmounts = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.ForiegnCurrencyAmount)) {
            this.LocalCurrencyAmount = null;
            this.ProfitCurrencyAmount = null;
            this.EntityPM.InvoiceCurrencyAmount = null;
        }
        else {
            var invoiceAmount = 0;
            this.LocalCurrencyAmount = this.ForiegnCurrencyAmount * this.EntityPM.ForiegnExchangeRate;
            if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
                this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
            }
            else {
                this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.APInvoicePM.ProfitCurrencyExchangeRate;
            }
            if (this.ForiegnCurrencyId == this.InvoiceCurrencyId) {
                invoiceAmount = this.ForiegnCurrencyAmount;
            }
            else {
                invoiceAmount = this.LocalCurrencyAmount / this.APInvoicePM.InvoiceCurrencyExchangeRate;
            }
            this.EntityPM.InvoiceCurrencyAmount = Tools_1.AppTool.Round(invoiceAmount, 2);
        }
        this.fatherComponent.ComputeTotals();
    };
    return APInvoiceLineShortItem;
}(BaseComponent_1.BaseComponent));
exports.APInvoiceLineShortItem = APInvoiceLineShortItem;
//# sourceMappingURL=EditMultipleShipmentComponent.js.map