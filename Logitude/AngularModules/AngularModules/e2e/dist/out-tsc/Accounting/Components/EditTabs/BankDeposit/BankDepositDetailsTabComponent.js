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
var ARPaymentChequeListService_1 = require("./../../../Services/StandardLists/ARPaymentChequeListService");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var BankDepositLinePM_1 = require("../../../EntityPMs/BankDepositLinePM");
var CashBookPMService_1 = require("../../../Services/StandardPMs/CashBookPMService");
var RatesTableExtendedListService_1 = require("../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BankDepositExtendedPMService_1 = require("../../../Services/ExtendedPMs/BankDepositExtendedPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var BankDepositDetailsTabComponent = /** @class */ (function (_super) {
    __extends(BankDepositDetailsTabComponent, _super);
    function BankDepositDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "BankDeposit";
        _this.DataContext = _this;
        _this.CashBookTotal = 0;
        _this.SelectedTotal = 0;
        _this.NoCashBookRows = false;
        _this.IsLinesSelection = false;
        _this.searchText = "";
        _this._CashBookPMService = new CashBookPMService_1.CashBookPMService();
        _this.ratesTableExtendedListService = new RatesTableExtendedListService_1.RatesTableExtendedListService();
        _this.currencyListService = new CurrencyListService_1.CurrencyListService();
        _this._BankDepositExtendedPMService = new BankDepositExtendedPMService_1.BankDepositExtendedPMService();
        _this._ARPaymentChequeListService = new ARPaymentChequeListService_1.ARPaymentChequeListService();
        _this.isRTL = false;
        _this.IsReturnChequeEnabled = false;
        _this.txt_NewDeposit = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");
        _this.txt_DepositDetails = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.DepositDetails");
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Alert
        _this.showAlert = false;
        _this.isAllSelected = false;
        //#region Filter Methods
        _this.FilterSelectedValue = 'cash';
        _this.tenantCurrencyCode = "";
        _this.currencyRate = 1;
        _this.isCurrencyRateLoaded = false;
        //#endregion
        _this.CashCount = 0;
        _this.PostdatesCount = 0;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) { // selection mode
            _this.IsLinesSelection = true;
            _this.GetCashBook();
            _this.BankDepositLines = _this.EntityPM.BankDepositLines;
            _this.BankDepositLines = [];
        }
        else { // view mode
            _this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
            _this.GetCashBook();
            _this.BankDepositLines = _this.EntityPM.BankDepositLines;
            _this.SetUIProperty();
            console.log("Deposit: ", _this.EntityPM);
        }
        _this.Listen();
        // Dim fields
        //this.SetUIProperty(); // do it after getting cashbook (isCashDeposit)
        // Get Default Value
        _this.GetDefaultValues();
        return _this;
    }
    BankDepositDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        // Save
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
            if (isSaveSuccess) {
                console.log("Deposited Success", _this.EntityPM);
                _this.RedrawScreen();
                _this.ShowAlert();
            }
        });
        // Reload
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                console.log("Entity Reloaded");
                console.log("Deposited Success", _this.EntityPM);
                _this.RedrawScreen();
            }
        });
    };
    BankDepositDetailsTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EntityId = this.EntityPM.Id; // Set entity id in edit component to reload
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); // reloading
    };
    BankDepositDetailsTabComponent.prototype.RedrawScreen = function () {
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        console.log("Deposit Reloaded: ", this.EntityPM);
        this.IsLinesSelection = !this.EntityPM.Id;
        // Redraw UI
        this.IsLinesSelection = false;
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.CashBookLines = [];
        this.BankDepositLines = this.EntityPM.BankDepositLines;
        this.CalculateTotals();
        this.GetCashBook();
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
        this.SetUIProperty();
    };
    BankDepositDetailsTabComponent.prototype.ShowAlert = function () {
        this.showAlert = true;
        //this.timerToken = setTimeout(() => { // Turn off after 5 second
        //    this.showAlert = false;
        //}, 5000);
    };
    BankDepositDetailsTabComponent.prototype.CloseAlert = function () {
        this.showAlert = false;
    };
    //#endregion
    BankDepositDetailsTabComponent.prototype.OpenJournal = function (id) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                _this.showAlert = false;
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    BankDepositDetailsTabComponent.prototype.OpenARPayment = function (id) {
        // open ARPayment screen
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARPayment' });
            });
        }
        else {
            console.warn("No ID for this payment!", id);
        }
    };
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "DepositNumber", {
        //#region Properties
        get: function () { return this.EntityPM.DepositNumber; },
        set: function (value) {
            if (this.EntityPM.DepositNumber != value) {
                this.EntityPM.DepositNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "DepositDate", {
        get: function () { return this.EntityPM.DepositDate; },
        set: function (value) {
            if (this.EntityPM.DepositDate != value) {
                this.EntityPM.DepositDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "DepositCurrencyId", {
        get: function () { return this.EntityPM.DepositCurrencyId; },
        set: function (value) {
            if (this.EntityPM.DepositCurrencyId != value) {
                this.EntityPM.DepositCurrencyId = value;
                this.GetCurrencyRate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "LocalDepositAmount", {
        get: function () { return this.EntityPM.LocalDepositAmount; },
        set: function (value) {
            if (this.EntityPM.LocalDepositAmount != value) {
                this.EntityPM.LocalDepositAmount = value;
                //if (this.EntityPM.IsCashDeposit && value != null) {
                //    if (value > this.CashBookPM.TotalAmount) {
                //        this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, false, "Amount must be less than Cashbook total");
                //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Deposit Amount must be less than Cashbook total");
                //        this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                //    } else {
                //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                //        this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, true, "");
                //        this.CurrentSession.CurrentEditComponent.IsEditValid = true;
                //        this.CalculateForeign(value);
                //    }
                //}
                //if (this.IsLinesSelection && value != null) {
                //    this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, false);
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "ForeignAmount", {
        get: function () { return this.EntityPM.ForeignAmount; },
        set: function (value) {
            if (this.EntityPM.ForeignAmount != value) {
                this.EntityPM.ForeignAmount = value;
                //
                if (this.EntityPM.IsCashDeposit && value != null) {
                    if (value > this.CashBookPM.TotalAmount) {
                        this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, false, "Amount must be less than Cashbook total");
                        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Deposit Amount must be less than Cashbook total");
                        //this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                    }
                    else {
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                        this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, true, "");
                        //this.CurrentSession.CurrentEditComponent.IsEditValid = true;
                        this.CalculateLocal(value);
                    }
                }
                if (this.IsLinesSelection && value != null) {
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "CashBookId", {
        get: function () { return this.EntityPM.CashBookId; },
        set: function (value) {
            if (this.EntityPM.CashBookId != value) {
                this.EntityPM.CashBookId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "DepositBankAccountId", {
        get: function () { return this.EntityPM.DepositBankAccountId; },
        set: function (value) {
            if (this.EntityPM.DepositBankAccountId != value) {
                this.EntityPM.DepositBankAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "_CashbookTotal", {
        get: function () { return this.CashBookTotal; },
        set: function (value) {
            if (this.CashBookTotal != value) {
                this.CashBookTotal = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "IsAllSelected", {
        get: function () { return this.isAllSelected; },
        set: function (value) {
            if (this.isAllSelected != value) {
                this.isAllSelected = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankDepositDetailsTabComponent.prototype, "TenantCurrency", {
        get: function () { return this.tenantCurrency; },
        set: function (value) {
            if (this.tenantCurrency != value) {
                this.tenantCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    BankDepositDetailsTabComponent.prototype.SetUIProperty = function () {
        if (this.EntityPM.IsCashDeposit) {
            this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
            this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
            this.UIProperties.SetEnabled("DepositBankAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("_CashbookTotal", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalDepositAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
        }
        else {
            this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalDepositAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DepositBankAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("_CashbookTotal", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DepositCurrencyId", this.ObjectTableName, false);
        }
    };
    BankDepositDetailsTabComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
            this.IsAllSelected = false;
        }
    };
    BankDepositDetailsTabComponent.prototype.FilterLines = function () {
        var _this = this;
        this.BankDepositLines = [];
        this.EntityPM.BankDepositLines = [];
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var lines = this.CashBookPM.CashBookLines;
        // Filtering
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FilterSelectedValue)) {
            lines = lines.filter(function (el) {
                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (_this.FilterSelectedValue == 'postdated') {
                        if (date > todayDate) {
                            return true; //postdated
                        }
                    }
                    else if (_this.FilterSelectedValue == 'cash') {
                        if (date <= todayDate) {
                            return true;
                        }
                    }
                }
                return false;
            });
        }
        this.CashBookLines = lines;
        // remove deposited lines
        this.RemoveDepositedLines();
        if (this.CashBookLines.length > 0) {
            this.NoCashBookRows = false;
        }
        else {
            this.NoCashBookRows = true;
        }
        this.CalculateTotals();
    };
    BankDepositDetailsTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        // Deposited cheque
        if (!this.IsLinesSelection && !this.EntityPM.IsCashDeposit) {
            this.timerToken = setTimeout(function () {
                _this.searchText = searchtext;
                _this.FilterChequeDeposits();
            }, 500);
        }
    };
    BankDepositDetailsTabComponent.prototype.FilterChequeDeposits = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            this.BankDepositLines = this.EntityPM.BankDepositLines;
        }
        else {
            var filteredDepositedCheques = [];
            //filteredDepositedCheques = this.EntityPM.BankDepositLines.filter(d => d.ChequeNumber.toLowerCase().includes(this.searchText.toLowerCase()));
            filteredDepositedCheques = this.EntityPM.BankDepositLines.filter(function (d) { return d.SearchFields.toLowerCase().includes(_this.searchText.toLowerCase()); });
            this.BankDepositLines = filteredDepositedCheques;
        }
    };
    //#endregion
    //#region Get Methods
    BankDepositDetailsTabComponent.prototype.GetCashBook = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CashBookPMService.get(this.EntityPM.CashBookId).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.CashBookPM = myResponse.Result;
                console.log("Cashbook: ", _this.CashBookPM);
                if (_this.IsLinesSelection) {
                    if (_this.CashBookPM.CashBookTypeCode == "1") // 1- Cash
                     {
                        _this.SetUIProperty();
                        _this.EntityPM.IsCashDeposit = true;
                        _this._CashbookTotal = _this.CashBookPM.TotalAmount;
                        //copy amount
                        //this.EntityPM.LocalDepositAmount = this.CashBookPM.TotalAmount;
                        _this.EntityPM.ForeignAmount = _this.CashBookPM.TotalAmount;
                        if (_this.isCurrencyRateLoaded)
                            _this.CalculateLocal(_this.ForeignAmount);
                        _this.UIProperties.SetValidity("LocalDepositAmount", _this.ObjectTableName, true, "");
                        _this.UIProperties.SetValidity("ForeignAmount", _this.ObjectTableName, true, "");
                    }
                    else { // Cheque
                        _this.CashBookLines = _this.CashBookPM.CashBookLines;
                        _this.ComputeTotals();
                        _this.RemoveDepositedLines();
                        _this.CalculateTotals();
                        _this.FilterLines();
                        if (_this.CashBookLines.length > 0) {
                            _this.NoCashBookRows = false;
                        }
                        else {
                            _this.NoCashBookRows = true;
                        }
                        _this.SetUIProperty();
                        _this.EntityPM.IsCashDeposit = false;
                    }
                }
                else {
                    _this._CashbookTotal = _this.CashBookPM.TotalAmount;
                    _this.CalculateTotals();
                }
            }
        }, function (error) {
        });
    };
    BankDepositDetailsTabComponent.prototype.GetDefaultValues = function () {
        var _this = this;
        // Tenant currency
        var defaultCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.tenantCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
        this.currencyListService.getSingle(defaultCurrencyId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.TenantCurrency = myResponse.Result;
                    console.log(">>Tenant Currency: ", myResponse.Result);
                    _this.GetCurrencyRate();
                }
            }
        });
    };
    BankDepositDetailsTabComponent.prototype.GetCurrencyRate = function () {
        var _this = this;
        if (this.IsLinesSelection) {
            if (this.TenantCurrency.Id == this.DepositCurrencyId) {
                // local currecny
                this.currencyRate = 1;
                this.CalculateLocal(this.EntityPM.ForeignAmount);
                this.isCurrencyRateLoaded = true;
            }
            else {
                this.ratesTableExtendedListService.getClosestRate(this.TenantCurrency.Id, this.DepositCurrencyId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            if (myResponse.Result != undefined && myResponse.Result != null) {
                                var rate = myResponse.Result;
                                _this.currencyRate = rate.Rate;
                                _this.isCurrencyRateLoaded = true;
                                //this.EntityPM.ForeignAmount = (this.SelectedTotal * this.currencyRate);
                                //this.CalculateForeign(this.EntityPM.LocalDepositAmount);
                                _this.CalculateLocal(_this.EntityPM.ForeignAmount);
                                console.log(">Exchange Rate for " + _this.TenantCurrency.Code + " : ", _this.currencyRate);
                            }
                            else {
                                _this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NoExchangeRateForLocalCurrency"));
                                //console.warn("The selected currency does not have Exchange Rate!");
                            }
                        }
                    }
                });
            }
        }
    };
    //#endregion
    BankDepositDetailsTabComponent.prototype.CalculateForeign = function (local) {
        if (!Tools_1.AppTool.IsNullOrEmpty(local) && !Tools_1.AppTool.IsNullOrEmpty(this.currencyRate)) {
            this.EntityPM.ForeignAmount = +(local / this.currencyRate).toFixed(2);
        }
        else {
            console.warn("No value or rate to caclulate forigen amount!");
        }
    };
    BankDepositDetailsTabComponent.prototype.CalculateLocal = function (foriegn) {
        if (!Tools_1.AppTool.IsNullOrEmpty(foriegn) && !Tools_1.AppTool.IsNullOrEmpty(this.currencyRate)) {
            this.EntityPM.LocalDepositAmount = +(foriegn * this.currencyRate).toFixed(2);
        }
        else {
            console.warn("No value or rate to caclulate local amount!");
        }
    };
    BankDepositDetailsTabComponent.prototype.CalculateTotals = function () {
        this.CashBookTotal = 0;
        if (this.CashBookPM.CashBookTypeCode == "1") {
            this.CashBookTotal = this.CashBookPM.TotalAmount;
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CashBookLines)) {
                for (var _i = 0, _a = this.CashBookLines; _i < _a.length; _i++) {
                    var line = _a[_i];
                    this.CashBookTotal += line.ForeignAmount == null ? 0 : line.ForeignAmount;
                }
            }
        }
        this.SelectedTotal = 0;
        var localSum = 0.0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BankDepositLines)) {
            for (var _b = 0, _c = this.BankDepositLines; _b < _c.length; _b++) {
                var line2 = _c[_b];
                localSum += line2.LocalAmount;
                this.SelectedTotal += line2.ForeignAmount == null ? 0 : line2.ForeignAmount;
            }
            if (this.IsLinesSelection) {
                if (this.IsLinesSelection && !this.EntityPM.IsCashDeposit) {
                    this.EntityPM.LocalDepositAmount = localSum;
                    this.EntityPM.ForeignAmount = this.SelectedTotal;
                }
                else if (this.IsLinesSelection) {
                    this.EntityPM.ForeignAmount = this.SelectedTotal;
                    //this.CalculateForeign(this.SelectedTotal);
                    this.CalculateLocal(this.SelectedTotal);
                }
            }
        }
    };
    BankDepositDetailsTabComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    BankDepositDetailsTabComponent.prototype.RemoveDepositedLines = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CashBookLines)) {
            var nonDepositedlines = [];
            for (var i = 0; i < this.CashBookLines.length; i++) {
                if (this.CashBookLines[i].IsDeposited == false) {
                    nonDepositedlines.push(this.CashBookLines[i]);
                }
            }
            // Filtering
            nonDepositedlines = nonDepositedlines.filter(function (el) {
                if (el.ARPChequeStatusCode == "5")
                    return false;
                else
                    return true;
            }); //// 5- Returned to Customer
            this.CashBookLines = nonDepositedlines;
        }
    };
    BankDepositDetailsTabComponent.prototype.SelectAll = function (event) {
        this.IsAllSelected = event;
        this.BankDepositLines = [];
        this.EntityPM.BankDepositLines = [];
        if (event == true) {
            for (var _i = 0, _a = this.CashBookLines; _i < _a.length; _i++) {
                var line = _a[_i];
                this.PushBankDeposit(line);
            }
        }
        this.CalculateTotals();
    };
    BankDepositDetailsTabComponent.prototype.LineSelection = function (cashbookLine, event) {
        if (event == true) {
            this.PushBankDeposit(cashbookLine);
        }
        else if (event == false) {
            this.PopBankDeposit(cashbookLine);
        }
        this.CalculateTotals();
    };
    BankDepositDetailsTabComponent.prototype.PushBankDeposit = function (cashbookLine) {
        if (!Tools_1.AppTool.IsNullOrEmpty(cashbookLine)) {
            // Get Counter
            var lastRow = this.BankDepositLines[this.BankDepositLines.length - 1];
            if (!Tools_1.AppTool.IsNullOrEmpty(lastRow)) {
                var lineCounter = this.BankDepositLines[this.BankDepositLines.length - 1].Line + 1;
            }
            else {
                var lineCounter = 1;
            }
            // New Row
            var depositLine = new BankDepositLinePM_1.BankDepositLinePM(this.EntityPM);
            depositLine.CompositId = cashbookLine.CashBookId + ',' + cashbookLine.ARPChequeId;
            depositLine.Line = lineCounter;
            depositLine.Tenant = this.EntityPM.Tenant;
            depositLine.ARPaymentChequeId = cashbookLine.ARPChequeId;
            depositLine.AccountNumber = cashbookLine.AccountNumber;
            depositLine.IsOutOfDeposit = false;
            depositLine.LocalAmount = cashbookLine.LocalAmount;
            depositLine.ForeignAmount = cashbookLine.ForeignAmount;
            this.EntityPM.AddBankDepositLine(depositLine);
            this.BankDepositLines.push(depositLine);
        }
    };
    BankDepositDetailsTabComponent.prototype.PopBankDeposit = function (cashbookLine) {
        if (!Tools_1.AppTool.IsNullOrEmpty(cashbookLine)) {
            var id = cashbookLine.CashBookId + ',' + cashbookLine.ARPChequeId;
            var item = this.BankDepositLines.find(function (d) { return d.CompositId == id; });
            var index = this.BankDepositLines.findIndex(function (d) { return d.CompositId == id; });
            this.BankDepositLines.splice(index, 1);
            this.EntityPM.RemoveBankDepositLine(item);
        }
    };
    //#region Out of Deposit
    BankDepositDetailsTabComponent.prototype.ReturnChequeButtonClicked = function (line) {
        // validate redeemed cheque
        if (line.ChequeStatusCode == "6") { // 6- Redeemed
            // var msg = new MessageWindow();
            // msg.Show(TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));
            return;
        }
        else {
            this.showReturnChequeWindow(line);
        }
    };
    BankDepositDetailsTabComponent.prototype.showReturnChequeWindow = function (line) {
        var _this = this;
        // new code
        var windowArgs = {};
        //windowArgs.ReconciliationPM = entity;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 215;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.OutOfDeposit");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/OutOfDepositMessage');
        logitudeWindow.WindowClosed.subscribe(function (event) {
            if (event && event != "Cancel") {
                var sp = event.split(";");
                var type = sp[0];
                var notes = sp[1];
                _this.ReturnCheque(line.ARPaymentChequeId, type, notes);
            }
        });
    };
    BankDepositDetailsTabComponent.prototype.ReturnCheque = function (chequeId, returnType, notes) {
        var _this = this;
        //
        // returnType: Customer / Cashbook
        //
        this.CurrentSession.StartBusyIndicatorLoading();
        this._BankDepositExtendedPMService.returnCheque(this.EntityPM.Id, chequeId, returnType, notes).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.RedrawScreen();
            }
            else {
                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    BankDepositDetailsTabComponent.prototype.ComputeTotals = function () {
        this.RemoveDepositedLines();
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.CashCount = this.CashBookLines.filter(function (el) {
            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date <= todayDate) {
                    return true;
                }
                return false;
            }
            return false;
        }).length;
        this.PostdatesCount = this.CashBookLines.filter(function (el) {
            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date > todayDate) {
                    return true;
                }
                return false;
            }
            return false;
        }).length;
    };
    BankDepositDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BankDepositDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BankDepositDetailsTabComponent);
    return BankDepositDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BankDepositDetailsTabComponent = BankDepositDetailsTabComponent;
//# sourceMappingURL=BankDepositDetailsTabComponent.js.map