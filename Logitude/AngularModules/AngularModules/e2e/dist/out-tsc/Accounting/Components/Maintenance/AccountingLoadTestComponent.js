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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../Infrastructure/Tools");
var JournalOpService_1 = require("../../Services/Others/JournalOpService");
var AccountingLoadTestComponent = /** @class */ (function (_super) {
    __extends(AccountingLoadTestComponent, _super);
    function AccountingLoadTestComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Journal";
        _this._JournalOpService = new JournalOpService_1.JournalOpService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._SelectedIndexActionTypeItem = 1;
        _this._SelectedIndexEveryMinuteItem = 1;
        _this._SelectedActionTypeValue = "";
        _this._SelectedEveryMinuteValue = 1;
        _this._closeWin = true;
        _this.ActionTypeItems = [];
        _this.ActionTypeItems.push({ Id: /*0, Code:*/ "", Name: "" });
        _this.ActionTypeItems.push({ Id: /*1, Code: */ "CreateVendors", Name: "Create Vendors " });
        _this.ActionTypeItems.push({ Id: /*2, Code: */ "CreateCustomers", Name: "Create Customers" });
        _this.ActionTypeItems.push({ Id: /*3, Code: */ "CreateJournal", Name: "Create Journal " });
        _this.ActionTypeItems.push({ Id: /*4, Code: */ "CreateJournalEvery", Name: "Create Journal Every" });
        _this.EveryMinuteItems = [1, 5, 30, 60, 90, 120];
        _this.clearScreen();
        return _this;
    }
    AccountingLoadTestComponent.prototype.clearScreen = function () {
        this.Year = (new Date().getFullYear()) - 1;
        this._SelectedIndexActionTypeItem = 1;
        this._SelectedIndexEveryMinuteItem = 1;
        this.Amount = 10;
    };
    AccountingLoadTestComponent.prototype.ActionItemSelectionChanged = function (selectControl) {
        this._SelectedActionTypeValue = selectControl.value;
        this.IsCreateJournal = (this._SelectedActionTypeValue == "CreateJournal");
        this.IsCreateJournalEvery = (this._SelectedActionTypeValue == "CreateJournalEvery");
        if (this.IsCreateJournalEvery) {
            this._SelectedIndexEveryMinuteItem = 3;
        }
    };
    AccountingLoadTestComponent.prototype.EveryMinuteItemSelectionChanged = function (selectControl) {
        this._SelectedEveryMinuteValue = selectControl.value;
    };
    Object.defineProperty(AccountingLoadTestComponent.prototype, "Amount", {
        get: function () { return this.amount; },
        set: function (value) {
            if (this.amount != value) {
                this.amount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingLoadTestComponent.prototype, "Year", {
        get: function () { return this.year; },
        set: function (value) {
            if (this.year != value) {
                this.year = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountingLoadTestComponent.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (this.Amount < 1) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Amount must be greater then zero ");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this._SelectedActionTypeValue)) {
            this.ValidationErrorsList.push("Selected Action Type is nothing");
        }
        if (this._SelectedActionTypeValue == "CreateJournalEvery" || "CreateJournal" == this._SelectedActionTypeValue) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.year)) {
                //this.Year = new Date().getFullYear();
                this.ValidationErrorsList.push("Year Field is Required");
            }
            if (this.year > (new Date().getFullYear())) {
                this.ValidationErrorsList.push("Future Year!");
            }
            else if (this.year < 1900) {
            }
        }
        if (this._SelectedActionTypeValue == "CreateJournalEvery") {
            if (this._SelectedEveryMinuteValue < 1) {
                this.ValidationErrorsList.push("Selected Every Minute Value must be greater then zero ");
            }
        }
    };
    AccountingLoadTestComponent.prototype.SetWindowArgs = function (args) {
        //this.EntityPM = args.EntityPM;
    };
    AccountingLoadTestComponent.prototype.SendButtonClicked = function () {
        this._closeWin = true;
        this.JustSend();
    };
    AccountingLoadTestComponent.prototype.SendandNewButtonClicked = function () {
        this._closeWin = false;
        this.JustSend();
    };
    AccountingLoadTestComponent.prototype.JustSend = function () {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        this._JournalOpService
            .GetTaskLoadTest(SessionLocator_1.SessionLocator.Tenant, this._SelectedActionTypeValue, this.Amount, this._SelectedEveryMinuteValue, this.Year)
            .subscribe(function (res) {
            if (res.HasError) {
                _this.ValidationErrorsList = res.ErrorsArray;
            }
            else {
                //this._JournalPM = res.Result;
                if (_this._closeWin) {
                    _this.CancelButtonClicked();
                }
                else {
                    _this.clearScreen();
                }
            }
        }, function (err) {
            alert(err);
        }, function () {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    AccountingLoadTestComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingLoadTestComponent.prototype.ngAfterViewInit = function () {
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AccountingLoadTestComponent.prototype, "AllLocations", void 0);
    AccountingLoadTestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AccountingLoadTestComponent',
            templateUrl: './AccountingLoadTestComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountingLoadTestComponent);
    return AccountingLoadTestComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingLoadTestComponent = AccountingLoadTestComponent;
//# sourceMappingURL=AccountingLoadTestComponent.js.map