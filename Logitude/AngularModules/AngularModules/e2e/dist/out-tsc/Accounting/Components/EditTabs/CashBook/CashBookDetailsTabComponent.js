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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var GLAccountListService_1 = require("../../../Services/StandardLists/GLAccountListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CashBookDetailsTabComponent = /** @class */ (function (_super) {
    __extends(CashBookDetailsTabComponent, _super);
    function CashBookDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "CashBook";
        _this.DataContext = _this;
        _this.TotalSum = 0;
        _this.NoRows = false;
        _this.tenantCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
        _this.searchText = "";
        _this.isRTL = false;
        _this._GLAccountListService = new GLAccountListService_1.GLAccountListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        //#region Filter Methods
        _this.FilterSelectedValue = 'all';
        //#endregion
        _this.CashCount = 0;
        _this.PostdatesCount = 0;
        _this.Listen();
        _this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //this.ItemSource = this.EntityPM.CashBookLines;
        _this.LoadScreen();
        //if (this.TotalSum > 0) {
        //    this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
        //}
        _this.SetUIProperties();
        return _this;
    }
    CashBookDetailsTabComponent.prototype.LoadScreen = function () {
        var _this = this;
        this.RemoveDepositedLines();
        this.CalculateTotals();
        this.ComputeFilterTotals();
        // toggle GLAccount editability
        if (this.EntityPM.AccountId)
            this._GLAccountListService.getSingle(this.EntityPM.AccountId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var glaccount = myResponse.Result;
                        var glaBalance = glaccount.BalanceInLocalCurrency;
                        if (_this.TotalSum == 0 && (!glaBalance || glaBalance == 0)) {
                            _this.UIProperties.SetEnabled("AccountId", _this.ObjectTableName, true);
                        }
                        else {
                            _this.UIProperties.SetEnabled("AccountId", _this.ObjectTableName, false);
                        }
                        _this.SetUIProperties();
                    }
                }
            });
    };
    CashBookDetailsTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false); // always dim, WI 41740
        if (this.EntityPM.Inactive) {
            this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
            //this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookTypeCode", this.ObjectTableName, false);
        }
    };
    CashBookDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadScreen();
                        _this.SetUIProperties();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        _this.LoadScreen();
                        _this.SetUIProperties();
                    }
                });
            }
        }
    };
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "LocalName", {
        //#region Properties
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "AccountId", {
        get: function () { return this.EntityPM.AccountId; },
        set: function (value) {
            if (this.EntityPM.AccountId != value) {
                this.EntityPM.AccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "BranchId", {
        get: function () { return this.EntityPM.BranchId; },
        set: function (value) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CashBookDetailsTabComponent.prototype, "CashBookTypeCode", {
        get: function () { return this.EntityPM.CashBookTypeCode; },
        set: function (value) {
            if (this.EntityPM.CashBookTypeCode != value) {
                this.EntityPM.CashBookTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CashBookDetailsTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        this.timerToken = setTimeout(function () {
            _this.searchText = searchtext;
            _this.FilterLines();
        }, 500);
    };
    CashBookDetailsTabComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    CashBookDetailsTabComponent.prototype.OpenARPayment = function (id) {
        // open ARPayment screen
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARPayment' });
            });
        }
    };
    CashBookDetailsTabComponent.prototype.CalculateTotals = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemSource)) {
            this.TotalSum = 0;
            if (this.CashBookTypeCode == "1") { //1-cash
                this.TotalSum = this.EntityPM.TotalAmount;
            }
            else {
                for (var _i = 0, _a = this.ItemSource; _i < _a.length; _i++) {
                    var line = _a[_i];
                    this.TotalSum += line.ForeignAmount;
                }
            }
        }
    };
    CashBookDetailsTabComponent.prototype.FilterLines = function () {
        var _this = this;
        this.FilterCheques();
        var lines = this.ItemSource;
        // Filtering
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            lines = lines.filter(function (el) {
                if (el.ChequeNumber != null)
                    if (el.ChequeNumber.toLowerCase().includes(_this.searchText.toLowerCase()))
                        return true;
                if (el.AccountNumber != null)
                    if (el.AccountNumber.toLowerCase().includes(_this.searchText.toLowerCase()))
                        return true;
                return false;
            });
        }
        this.ItemSource = lines;
        this.ChequesList = new ObservableCollection_1.ObservableCollection([]);
        this.ChequesList.InsertCollection(lines, true);
        this.NoRows = lines.length == 0;
    };
    CashBookDetailsTabComponent.prototype.RemoveDepositedLines = function () {
        var lines = this.EntityPM.CashBookLines;
        // Filtering
        lines = lines.filter(function (el) {
            if (el.ARPChequeStatusCode == "5")
                return false; // 5- Returned to Customer
            else if (el.IsDeposited == true)
                return false;
            else
                return true;
        });
        this.FilteredLines = lines;
        this.ItemSource = this.FilteredLines;
        this.ChequesList = new ObservableCollection_1.ObservableCollection([]);
        this.ChequesList.InsertCollection(this.FilteredLines, true);
        this.NoRows = this.FilteredLines.length == 0;
    };
    CashBookDetailsTabComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            //this.FilterCheques();
            this.FilterLines(); // set search then filter
        }
    };
    CashBookDetailsTabComponent.prototype.FilterCheques = function () {
        var originalCheques = this.FilteredLines;
        var filteredQuery = originalCheques;
        var today = new Date();
        this.NoRows = false;
        if (this.FilterSelectedValue == 'cash') {
            filteredQuery = originalCheques.filter(function (el) {
                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (date <= today) {
                        return true;
                    }
                    return false;
                }
                return false;
            }); // cash cheques
        }
        else if (this.FilterSelectedValue == 'postdated') {
            filteredQuery = originalCheques.filter(function (el) {
                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (date > today) {
                        return true;
                    }
                    return false;
                }
                return false;
            }); // postdated cheques
        }
        this.ItemSource = filteredQuery;
        this.ChequesList = new ObservableCollection_1.ObservableCollection([]);
        this.ChequesList.InsertCollection(filteredQuery, true);
        this.CalculateTotals();
        this.ComputeFilterTotals();
    };
    CashBookDetailsTabComponent.prototype.ComputeFilterTotals = function () {
        this.CashCount = 0;
        this.PostdatesCount = 0;
        var todayDate = new Date();
        this.CashCount = this.FilteredLines.filter(function (el) {
            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date <= todayDate) {
                    return true;
                }
                return false;
            }
            return false;
        }).length;
        this.PostdatesCount = this.FilteredLines.filter(function (el) {
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
    CashBookDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CashBookDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CashBookDetailsTabComponent);
    return CashBookDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CashBookDetailsTabComponent = CashBookDetailsTabComponent;
//# sourceMappingURL=CashBookDetailsTabComponent.js.map