"use strict";
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
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var BackUpService_1 = require("../../../../Infrastructure/Services/WebServices/BackUpService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DatabaseBackupComponent = /** @class */ (function () {
    function DatabaseBackupComponent(_backUpService) {
        this._backUpService = _backUpService;
        this.DownloadBackupBtnDisable = true;
        this.IsStopTimer = false;
        this.Backupsub = null;
    }
    DatabaseBackupComponent.prototype.ngOnInit = function () {
    };
    DatabaseBackupComponent.prototype.SetDataContext = function (data) {
        this.PreparingTextBlock = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.PressBuildBackupButton");
    };
    DatabaseBackupComponent.prototype.BackUpTimer = function () {
        return Rx_1.Observable.interval(10000).timeInterval();
    };
    DatabaseBackupComponent.prototype.StartBackUpTimer = function () {
        var _this = this;
        this.Backupsub = this.BackUpTimer().subscribe(function (res) {
            if (!_this.IsStopTimer) {
                _this._backUpService.CheckIfDatabaseBackupIsBuilt(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var result = pmResponse.Result;
                        if (result) {
                            _this.IsStopTimer = true;
                            // this.Backupsub.unsubscribe();
                            _this.PreparingTextBlock = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.YourDataIsReady");
                            _this.IsShowProgressLoading = false;
                            _this.DownloadBackupBtnDisable = false;
                        }
                    }
                });
            }
        });
    };
    DatabaseBackupComponent.prototype.BuildDatabaseBackup = function () {
        var _this = this;
        this.PreparingTextBlock = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.PreparingYourData");
        this._backUpService.SetDatabaseDataBackupNotReady(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            _this.IsShowProgressLoading = true;
            _this.DownloadBackupBtnDisable = true;
            _this.IsStopTimer = false;
            _this.BackUpForClientDataTables();
        });
    };
    DatabaseBackupComponent.prototype.BackUpForClientDataTables = function () {
        var _this = this;
        this._backUpService.BackUpForClientData(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var result = res;
            _this.StartBackUpTimer();
        });
    };
    DatabaseBackupComponent.prototype.DownloadDatabaseBackup = function () {
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DataBackupDownloadPage.aspx?tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        window.open(url);
    };
    DatabaseBackupComponent = __decorate([
        core_1.Component({
            selector: 'DatabaseBackup',
            moduleId: module.id,
            templateUrl: './DatabaseBackupComponent.html',
            providers: [BackUpService_1.BackUpService],
        }),
        __metadata("design:paramtypes", [BackUpService_1.BackUpService])
    ], DatabaseBackupComponent);
    return DatabaseBackupComponent;
}());
exports.DatabaseBackupComponent = DatabaseBackupComponent;
//# sourceMappingURL=DatabaseBackupComponent.js.map