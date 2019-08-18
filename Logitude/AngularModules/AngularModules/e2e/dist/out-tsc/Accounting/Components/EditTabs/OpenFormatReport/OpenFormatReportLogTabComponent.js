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
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentsFilingViewsExtService_1 = require("../../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var OpenFormatReportLogTabComponent = /** @class */ (function (_super) {
    __extends(OpenFormatReportLogTabComponent, _super);
    function OpenFormatReportLogTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "OpenFormatReport";
        _this.isRTL = false;
        _this.showLocals = false;
        _this._DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService_1.DocumentsFilingViewsExtService();
        _this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.showLocals = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        _this.UIProperties.SetEnabled("ErrorMessage", "OpenFormatReport", false);
        return _this;
    }
    Object.defineProperty(OpenFormatReportLogTabComponent.prototype, "ErrorMessage", {
        get: function () { return this.entityPM.ErrorMessage; },
        enumerable: true,
        configurable: true
    });
    OpenFormatReportLogTabComponent.prototype.DownloadButtonClicked = function () {
        this.GetDocument();
    };
    OpenFormatReportLogTabComponent.prototype.GetDocument = function () {
        var _this = this;
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.entityPM.Id, objectTable.Id).subscribe(function (myResult) {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                _this.docFilingPM = mm.Result;
                DownloadManager_1.DownloadManager.DownloadPage(null, _this.docFilingPM.SecurityId);
            }
        });
    };
    OpenFormatReportLogTabComponent = __decorate([
        core_1.Component({
            selector: 'OpenFormatReportLogTabComponent',
            moduleId: module.id,
            templateUrl: './OpenFormatReportLogTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OpenFormatReportLogTabComponent);
    return OpenFormatReportLogTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OpenFormatReportLogTabComponent = OpenFormatReportLogTabComponent;
//# sourceMappingURL=OpenFormatReportLogTabComponent.js.map