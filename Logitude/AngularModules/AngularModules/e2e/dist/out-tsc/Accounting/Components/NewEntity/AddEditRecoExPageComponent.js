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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var ReconcileExternalPagePM_1 = require("../../EntityPMs/ReconcileExternalPagePM");
var ReconcileExternalPageLinePM_1 = require("../../EntityPMs/ReconcileExternalPageLinePM");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ReconcileExternalPagePMService_1 = require("../../Services/StandardPMs/ReconcileExternalPagePMService");
var CurrencyPMService_1 = require("../../../Common/Services/StandardPMs/CurrencyPMService");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var ReconcileExternalPageExtendedPMService_1 = require("../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AddEditRecoExPageComponent = /** @class */ (function (_super) {
    __extends(AddEditRecoExPageComponent, _super);
    function AddEditRecoExPageComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "ReconcileExternalPage";
        _this.ValidationErrorsList = [];
        _this.isNewEntity = false;
        _this.IsCancelApprovedEnabled = false;
        _this.IsDisplayOnly = false;
        _this.IsMultiCurrency = false;
        _this.AMOUNT_TEXT = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
        _this.CreditAMOUNT_TEXT = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.CreditAmount");
        _this.DebitAMOUNT_TEXT = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.DebitAmount");
        _this.isRTL = false;
        _this.TotalSum = 0.0;
        _this.Difference = 0.0;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._ReconcileExternalPagePMService = new ReconcileExternalPagePMService_1.ReconcileExternalPagePMService();
        _this._ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService_1.ReconcileExternalPageExtendedPMService();
        _this._CurrencyPMService = new CurrencyPMService_1.CurrencyPMService();
        _this.currencyListService = new CurrencyListService_1.CurrencyListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isApprovedButtonClicked = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.PageLinesList = new ObservableCollection_1.ObservableCollection([]);
        _this.SetUIProperties();
        return _this;
    }
    AddEditRecoExPageComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            var prevPageNo;
            this.BankAccountPM = args.BankAccount;
            this.GetDefaultValues();
            if (args.entity) {
                // EDIT Mode
                this.ReconcileExternalPagePM = args.entity;
                this.isNewEntity = false;
                this.GetPrevPage(this.ReconcileExternalPagePM.PageNo);
                if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "3")
                    this.IsDisplayOnly = true;
                this.SetUIProperties();
            }
            else {
                // NEW Entity
                //get prev page
                prevPageNo = this.BankAccountPM.LastPageNumber;
                this.GetPage(prevPageNo);
                this.isNewEntity = true;
                var newEntity = new ReconcileExternalPagePM_1.ReconcileExternalPagePM();
                newEntity.Tenant = SessionLocator_1.SessionLocator.Tenant;
                //newEntity.CreateDate = new Date();
                //if (SessionLocator.LoggedUserPM)
                //{
                //    newEntity.CreatedByUserId = SessionLocator.LoggedUserPM.Id;
                //    newEntity.CreatedByUserName = SessionLocator.LoggedUserPM.LocalName;
                //}
                if (args.BankAccount.LastPageEndDate) {
                    var lastDate = new Date(args.BankAccount.LastPageEndDate);
                    var lastDatePlusOne = new Date(lastDate.setDate(lastDate.getDate() + 1));
                    var date = Tools_1.DateTool.GetDate(lastDatePlusOne.getFullYear(), lastDatePlusOne.getMonth(), lastDatePlusOne.getDate(), 0, 0, 0);
                    newEntity.FromDate = date;
                }
                newEntity.StatusCode = "1"; // 1- Draft
                newEntity.PageNo = 0;
                newEntity.BankAccountId = args.BankAccountId;
                newEntity.GLAccountId = args.GLAccountId;
                newEntity.EntryTypeCode = "1"; // 1- Manual
                this.ReconcileExternalPagePM = newEntity;
            }
            if (this.isNewEntity) {
                this.IsCancelApprovedEnabled = false;
            }
            else if (this.ReconcileExternalPagePM.StatusCode == "3") { // 3- Cancelled
                this.IsCancelApprovedEnabled = false;
            }
            else if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "1") { // 1- Draft, 2- Approved
                this.IsCancelApprovedEnabled = true;
            }
            this.CalculateTotals();
            this.FillGridsData();
        }
    };
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "FromDate", {
        //#region Properties
        get: function () { return this.ReconcileExternalPagePM.FromDate; },
        set: function (value) {
            if (this.ReconcileExternalPagePM.FromDate != value) {
                this.ReconcileExternalPagePM.FromDate = value;
                var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                if (value > todayDate) {
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
                }
                else {
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "ok");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "ToDate", {
        get: function () { return this.ReconcileExternalPagePM.ToDate; },
        set: function (value) {
            if (this.ReconcileExternalPagePM.ToDate != value) {
                this.ReconcileExternalPagePM.ToDate = value;
                var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                if (value > todayDate) {
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
                }
                else {
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "ok");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "StartBalance", {
        get: function () { return this.ReconcileExternalPagePM.StartBalance; },
        set: function (value) {
            if (this.ReconcileExternalPagePM.StartBalance != value) {
                this.ReconcileExternalPagePM.StartBalance = value;
                this.CalculateTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "CloseBalance", {
        get: function () { return this.ReconcileExternalPagePM.CloseBalance; },
        set: function (value) {
            if (this.ReconcileExternalPagePM.CloseBalance != value) {
                this.ReconcileExternalPagePM.CloseBalance = value;
                this.CalculateTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "StatusCode", {
        get: function () { return this.ReconcileExternalPagePM.StatusCode; },
        set: function (value) {
            if (this.ReconcileExternalPagePM.StatusCode != value) {
                this.ReconcileExternalPagePM.StatusCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditRecoExPageComponent.prototype, "TenantCurrency", {
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
    //#region Buttons Handlers
    AddEditRecoExPageComponent.prototype.SaveAsDraftButtonClicked = function () {
        this.ReconcileExternalPagePM.StatusCode = '1'; // 1- Draft
        this.SaveEntity();
    };
    AddEditRecoExPageComponent.prototype.ApproveButtonClicked = function () {
        if (this.PageLinesList.Length == 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Nolineswereenteredonbank"));
            return;
        }
        this.isApprovedButtonClicked = true;
        this.ReconcileExternalPagePM.StatusCode = '2'; // 2- Approved
        this.SaveEntity();
    };
    AddEditRecoExPageComponent.prototype.CancelApprovalButtonClicked = function () {
        var errors = [];
        if (this.ReconcileExternalPagePM.StatusCode == "2") { // 2- Approved
            // * Check last approved page
            var isLastApprovedPage = this.BankAccountPM.LastPageNumber == this.ReconcileExternalPagePM.PageNo + "";
            if (isLastApprovedPage) {
                // continue...
            }
            else {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("BankAccounts.O.CantCancelItsNotLastApproved"));
                this.ValidationErrorsList = errors;
                return;
            }
            // * Check reconciled page lines
            var hasReconciledLines = false;
            var lines = this.PageLinesList.Collection;
            if (lines.length > 0) {
                lines.forEach(function (line) {
                    if (line.IsReconciled) {
                        hasReconciledLines = true;
                    }
                });
            }
            if (hasReconciledLines) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("BankAccounts.O.CanCancelItsTransactionsReconciled"));
                this.ValidationErrorsList = errors;
                return;
            }
        }
        this.ReconcileExternalPagePM.StatusCode = "3"; // 3-Canceled
        this.SaveEntity();
    };
    AddEditRecoExPageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    //* grid handlers in seperate region
    //#endregion
    AddEditRecoExPageComponent.prototype.SaveEntity = function () {
        var errors = [];
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (this.ToDate > todayDate || this.FromDate > todayDate) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
        }
        if (this.isNewEntity) {
            this.ReconcileExternalPagePM.CreateDate = new Date();
            if (SessionLocator_1.SessionLocator.LoggedUserPM) {
                this.ReconcileExternalPagePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                this.ReconcileExternalPagePM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.LocalName;
            }
        }
        // Class Validator
        Validator_1.Validator.TryValidateObject(this.ReconcileExternalPagePM, this.ObjectTableName, errors);
        // validate empty lines
        //if (this.ReconcileExternalPagePM.StatusCode != "1") {
        var lines = this.ReconcileExternalPagePM.ReconcileExternalPageLines;
        if (lines.length > 0) {
            lines.forEach(function (line) {
                if (!line.ReferenceDate) {
                    var error = "";
                    error = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line");
                    error += (line.LineNumber + ": ");
                    error += TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.ReferenceDateIsRequired");
                    errors.push(error);
                }
                if (Tools_1.AppTool.IsNullOrEmpty(line.CreditAmount) && Tools_1.AppTool.IsNullOrEmpty(line.DebitAmount)) {
                    var error = "";
                    error = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Line");
                    error += (line.LineNumber + ": ");
                    error += TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AmountIsRequired");
                    errors.push(error);
                }
            });
        }
        //}
        //// Custom Validation
        //if (DateTool.IsDateSmaller(this.FromDate,this.PrevBankPagePM.ToDate)) {
        ////if (this.FromDate < this.PrevBankPagePM.ToDate) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate"));
        //}
        //if (DateTool.IsDateSmaller(this.ToDate,this.FromDate)) {
        ////if (this.ToDate < this.FromDate) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate"));
        //}
        //if (Number(this.StartBalance) != Number(this.PrevBankPagePM.CloseBalance)) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance"));
        //}
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
        else {
            if (this.isApprovedButtonClicked) {
                this.isApprovedButtonClicked = false;
                this.ReconcileExternalPagePM.StatusCode = '1'; // 1- Draft
            }
        }
    };
    AddEditRecoExPageComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.isNewEntity) {
            this._ReconcileExternalPagePMService.insert(this.ReconcileExternalPagePM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    if (_this.isApprovedButtonClicked) {
                        _this.isApprovedButtonClicked = false;
                        _this.ReconcileExternalPagePM.StatusCode = '1'; // 1- Draft
                    }
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
        else {
            this._ReconcileExternalPagePMService.update(this.ReconcileExternalPagePM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    if (_this.isApprovedButtonClicked) {
                        _this.isApprovedButtonClicked = false;
                        _this.ReconcileExternalPagePM.StatusCode = '1'; // 1- Draft
                    }
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AddEditRecoExPageComponent.prototype.GetCurrency = function () {
        var _this = this;
        if (this.BankAccountPM.GLAccountCurrencyId && this.BankAccountPM.GLAccountCurrencyId == "multi") {
            this.IsMultiCurrency = true;
            this.AmountColHeader = this.AMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
            this.CreditAmountColHeader = this.CreditAMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
            this.DebitAmountColHeader = this.DebitAMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
        }
        else if (this.BankAccountPM.GLAccountCurrencyId) {
            this._CurrencyPMService.get(this.BankAccountPM.GLAccountCurrencyId).subscribe(function (myResult) {
                var currency = myResult.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(currency)) {
                    _this.currency = currency;
                    _this.AmountColHeader = _this.AMOUNT_TEXT + " (" + _this.currency.Code + ")";
                    _this.CreditAmountColHeader = _this.CreditAMOUNT_TEXT + " (" + _this.currency.Code + ")";
                    _this.DebitAmountColHeader = _this.DebitAMOUNT_TEXT + " (" + _this.currency.Code + ")";
                }
                else {
                    console.log("[!] Cannot find the currency !!");
                    _this.AmountColHeader = _this.AMOUNT_TEXT;
                    _this.CreditAmountColHeader = _this.CreditAMOUNT_TEXT;
                    _this.DebitAmountColHeader = _this.DebitAMOUNT_TEXT;
                }
            });
        }
    };
    //#region Prev Bank Page
    AddEditRecoExPageComponent.prototype.OpenPrevPage = function () {
        console.log(this.PrevBankPagePM);
        this.OpenBankPageWindow(this.PrevBankPagePM);
    };
    AddEditRecoExPageComponent.prototype.OpenBankPageWindow = function (entity) {
        if (entity === void 0) { entity = null; }
        var windowTitle = entity ? (TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + entity.PageNo) : TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewPage");
        var windowArgs = {};
        windowArgs.entity = entity;
        windowArgs.BankAccountId = this.BankAccountPM.Id;
        windowArgs.GLAccountId = this.BankAccountPM.GLAccountId;
        windowArgs.BankAccount = this.BankAccountPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
        });
        logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');
    };
    AddEditRecoExPageComponent.prototype.GetPage = function (pageNo) {
        var _this = this;
        if (pageNo) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetBankPageByPageNo(pageNo, this.BankAccountPM.Id).subscribe(function (myResult) {
                var bankPage = myResult.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(bankPage)) {
                    _this.PrevBankPagePM = bankPage;
                }
                else {
                    console.log("[!] Cannot find the Prev Bank Page !!");
                }
            });
        }
        else {
            console.log("[!!!] No pageNo provided");
        }
    };
    AddEditRecoExPageComponent.prototype.GetPrevPage = function (prevPageNo) {
        var _this = this;
        if (prevPageNo) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetPrevPageByPageNo(prevPageNo, this.BankAccountPM.Id).subscribe(function (myResult) {
                var bankPage = myResult.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(bankPage)) {
                    _this.PrevBankPagePM = bankPage;
                }
                else {
                    console.log("[!] Cannot find the Prev Bank Page !!");
                }
            });
        }
        else {
            console.log("[!!!] No prevPageNo provided");
        }
    };
    //#endregion
    AddEditRecoExPageComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StartBalance", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CloseBalance", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    };
    //#region Lines Grid
    AddEditRecoExPageComponent.prototype.FillGridsData = function () {
        this.PageLinesList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ReconcileExternalPagePM)) {
            for (var _i = 0, _a = this.ReconcileExternalPagePM.ReconcileExternalPageLines.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; }); _i < _a.length; _i++) {
                var item = _a[_i];
                this.PageLinesList.Insert(new PageLineModel(item, this));
            }
            this.CalculateTotals();
        }
    };
    AddEditRecoExPageComponent.prototype.AddButtonClicked = function () {
        if (this.IsDisplayOnly)
            return;
        var line = 0;
        if (this.ReconcileExternalPagePM.ReconcileExternalPageLines.length > 0) {
            var line = this.ReconcileExternalPagePM.ReconcileExternalPageLines.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current; }).LineNumber;
        }
        line++; // last no.
        var pageLine = new ReconcileExternalPageLinePM_1.ReconcileExternalPageLinePM(this.ReconcileExternalPagePM);
        //pageLine.Id = "new";
        //pageLine.ChangeSetOp = "insert";
        pageLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
        pageLine.ReconcileExternalPageId = this.isNewEntity ? "new" : this.ReconcileExternalPagePM.Id;
        pageLine.LineNumber = line;
        pageLine.IsReconciled = false;
        this.ReconcileExternalPagePM.AddReconcileExternalPageLine(pageLine);
        var item = new PageLineModel(pageLine, this);
        this.PageLinesList.Insert(item);
    };
    AddEditRecoExPageComponent.prototype.RemoveLineClicked = function (item) {
        if (item) {
            this.ReconcileExternalPagePM.RemoveReconcileExternalPageLine(item.pageLinePM);
            this.PageLinesList.Remove(item);
        }
        //resequence consignments
        for (var i = 0; i < this.ReconcileExternalPagePM.ReconcileExternalPageLines.length; i++) {
            var line = this.ReconcileExternalPagePM.ReconcileExternalPageLines[i];
            line.LineNumber = i + 1;
        }
        this.CalculateTotals();
    };
    AddEditRecoExPageComponent.prototype.OnRowEnded = function ($event) {
        if (this.StatusCode == "2")
            return;
        if (($event) == this.PageLinesList.Length) {
            this.AddButtonClicked();
        }
    };
    AddEditRecoExPageComponent.prototype.OnFocus = function () {
        if (this.StatusCode == "2")
            return;
        if (this.PageLinesList.Length == 0) {
            this.AddButtonClicked();
        }
    };
    //#endregion
    AddEditRecoExPageComponent.prototype.GetDefaultValues = function () {
        var _this = this;
        // Tenant currency
        var defaultCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.currencyListService.getSingle(defaultCurrencyId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.TenantCurrency = myResponse.Result;
                    console.log(">>Tenant Currency: ", myResponse.Result);
                    _this.GetCurrency();
                }
                else
                    _this.GetCurrency();
            }
            else
                _this.GetCurrency();
        });
    };
    AddEditRecoExPageComponent.prototype.CalculateTotals = function () {
        var sum = 0.0;
        if (this.PageLinesList.Length > 0) {
            for (var _i = 0, _a = this.PageLinesList.Collection; _i < _a.length; _i++) {
                var line = _a[_i];
                //debit
                sum -= !line.DebitAmount ? 0 : line.DebitAmount;
                sum += !line.CreditAmount ? 0 : line.CreditAmount;
            }
        }
        this.TotalSum = sum;
        if (this.StartBalance) {
            var GrandTotal = this.TotalSum + +this.StartBalance;
            this.Difference = this.CloseBalance - GrandTotal;
        }
    };
    AddEditRecoExPageComponent = __decorate([
        core_1.Component({
            selector: 'AddEditRecoExPageComponent',
            moduleId: module.id,
            providers: [EntityListService_1.EntityListService],
            templateUrl: './AddEditRecoExPageComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], AddEditRecoExPageComponent);
    return AddEditRecoExPageComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditRecoExPageComponent = AddEditRecoExPageComponent;
var PageLineModel = /** @class */ (function (_super) {
    __extends(PageLineModel, _super);
    function PageLineModel(pageLinePM, parent) {
        var _this = _super.call(this) || this;
        _this.pageLinePM = pageLinePM;
        _this.parent = parent;
        _this.ObjectTableName = "ReconcileExternalPageLine";
        _this.DataContext = _this;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.CreditAmount))
            _this.CreditAmount = 0;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.DebitAmount))
            _this.DebitAmount = 0;
        return _this;
    }
    Object.defineProperty(PageLineModel.prototype, "LineNumber", {
        //#region Properties
        get: function () { return this.pageLinePM.LineNumber; },
        set: function (value) {
            if (this.pageLinePM.LineNumber != value) {
                this.pageLinePM.LineNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "ReferenceDate", {
        get: function () { return this.pageLinePM.ReferenceDate; },
        set: function (value) {
            if (this.pageLinePM.ReferenceDate != value) {
                this.pageLinePM.ReferenceDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "Amount", {
        get: function () { return this.pageLinePM.Amount; },
        set: function (value) {
            if (this.pageLinePM.Amount != value) {
                this.pageLinePM.Amount = value;
                this.parent.CalculateTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "CreditAmount", {
        get: function () { return this.pageLinePM.CreditAmount; },
        set: function (value) {
            if (this.pageLinePM.CreditAmount != value) {
                this.pageLinePM.CreditAmount = value;
                this.parent.CalculateTotals();
                if (Tools_1.AppTool.IsNullOrEmpty(value))
                    this.CreditAmount = 0;
                // if(value != 0)
                //     this.DebitAmount = 0;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "DebitAmount", {
        get: function () { return this.pageLinePM.DebitAmount; },
        set: function (value) {
            if (this.pageLinePM.DebitAmount != value) {
                this.pageLinePM.DebitAmount = value;
                this.parent.CalculateTotals();
                if (Tools_1.AppTool.IsNullOrEmpty(value))
                    this.DebitAmount = 0;
                // if(value != 0)
                //     this.CreditAmount = 0;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "Reference", {
        get: function () { return this.pageLinePM.Reference; },
        set: function (value) {
            if (this.pageLinePM.Reference != value) {
                this.pageLinePM.Reference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "IsReconciled", {
        get: function () { return this.pageLinePM.IsReconciled; },
        set: function (value) {
            if (this.pageLinePM.IsReconciled != value) {
                this.pageLinePM.IsReconciled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "ReconciliationNumber", {
        get: function () { return this.pageLinePM.ReconciliationNumber; },
        set: function (value) {
            if (this.pageLinePM.ReconciliationNumber != value) {
                this.pageLinePM.ReconciliationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageLineModel.prototype, "Notes", {
        get: function () { return this.pageLinePM.Notes; },
        set: function (value) {
            if (this.pageLinePM.Notes != value) {
                this.pageLinePM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    PageLineModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return PageLineModel;
}(BaseComponent_1.BaseComponent));
exports.PageLineModel = PageLineModel;
//# sourceMappingURL=AddEditRecoExPageComponent.js.map