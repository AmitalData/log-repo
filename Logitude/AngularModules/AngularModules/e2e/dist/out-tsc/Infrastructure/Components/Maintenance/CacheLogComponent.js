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
var CacheLogService_1 = require("./../../Services/ExtendedLists/CacheLogService");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var EntityArgs_1 = require("../../DataContracts/EntityArgs");
var Tools_1 = require("../../Tools");
var CardPMService_1 = require("../../../Common/Services/StandardPMs/CardPMService");
//
var CacheLogComponent = /** @class */ (function (_super) {
    __extends(CacheLogComponent, _super);
    function CacheLogComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.cacheLogService = new CacheLogService_1.CacheLogService();
        _this.cardPMService = new CardPMService_1.CardPMService();
        _this.OriginalCacheKeys = [];
        _this.CacheKeys = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Properties
        _this.enableLog = false;
        _this.searchText = "";
        _this.GetKeys();
        _this.GetIsLoggerEnabled();
        return _this;
    }
    CacheLogComponent.prototype.GetIsLoggerEnabled = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.cacheLogService.IsLoggerEnabled().subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var result = myResult.Result;
            console.log("cacheLogService.IsLoggerEnabled", myResult);
            _this.enableLog = result;
        });
    };
    CacheLogComponent.prototype.GetKeys = function () {
        var _this = this;
        this.cacheLogService.getKeys().subscribe(function (myResult) {
            var result = myResult.Result;
            console.log("cacheLogService.getKeys", myResult);
            if (!Tools_1.AppTool.IsNullOrEmpty(result) && result.length > 0) {
                _this.OriginalCacheKeys = result;
                result.sort(function (a, b) { return (a.Count === b.Count) ? 0 : (a.Count < b.Count) ? 1 : -1; });
                _this.CacheKeys = result;
                _this.TextChanged(_this.searchText);
            }
            else {
                _this.OriginalCacheKeys = [];
                _this.CacheKeys = [];
            }
        });
    };
    Object.defineProperty(CacheLogComponent.prototype, "EnableLog", {
        get: function () {
            return this.enableLog;
        },
        set: function (v) {
            this.ToggleLogEnabled(v);
            this.enableLog = v;
        },
        enumerable: true,
        configurable: true
    });
    CacheLogComponent.prototype.TextChanged = function (searchtext) {
        this.searchText = searchtext;
        var lines = this.OriginalCacheKeys;
        // Filtering
        if (!Tools_1.AppTool.IsNullOrEmpty(searchtext)) {
            lines = lines.filter(function (el) {
                if (el.Key != null)
                    if (el.Key.toLowerCase().includes(searchtext.toLowerCase()))
                        return true;
                return false;
            });
        }
        this.CacheKeys = lines;
    };
    //#endregion
    CacheLogComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CacheLogComponent.prototype.ResetButtonClicked = function () {
        var _this = this;
        this.cacheLogService.ResetLog().subscribe(function (myResult) {
            var result = myResult.Result;
            console.log("cacheLogService.ResetLog", myResult);
            _this.RefreshButtonClicked();
        });
    };
    CacheLogComponent.prototype.RefreshButtonClicked = function () {
        this.GetKeys();
    };
    CacheLogComponent.prototype.GetCardButtonClicked = function () {
        this.cardPMService.get(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (myResult) {
            var result = myResult.Result;
            console.log("cardPMService.get", myResult);
        });
    };
    CacheLogComponent.prototype.ToggleLogEnabled = function (enable) {
        this.cacheLogService.EnableLog(enable).subscribe(function (myResult) {
            var result = myResult.Result;
            console.log("cacheLogService.EnableLog", myResult);
            // if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
            //     // this.EnableLog =
            // } else {
            //     this.OriginalCacheKeys = [];
            //     this.CacheKeys = [];
            // }
        });
    };
    CacheLogComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CacheLogComponent',
            templateUrl: './CacheLogComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CacheLogComponent);
    return CacheLogComponent;
}(BaseComponent_1.BaseComponent));
exports.CacheLogComponent = CacheLogComponent;
var CacheKey = /** @class */ (function () {
    function CacheKey() {
    }
    return CacheKey;
}());
exports.CacheKey = CacheKey;
//# sourceMappingURL=CacheLogComponent.js.map