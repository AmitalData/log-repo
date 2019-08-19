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
var TaxReportLinePMService_1 = require("./../../Services/StandardPMs/TaxReportLinePMService");
var TaxReportPMService_1 = require("./../../Services/StandardPMs/TaxReportPMService");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TaxReportListTemplate = /** @class */ (function () {
    function TaxReportListTemplate(CD) {
        this.CD = CD;
        this.isRTL = false;
        this.showLocal = false;
        this._TaxReportPMService = new TaxReportPMService_1.TaxReportPMService();
        this._TaxReportLinePMService = new TaxReportLinePMService_1.TaxReportLinePMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
    }
    TaxReportListTemplate.prototype.setVariables = function (rowData, fieldName, MyAdditionalData) {
        this.rowData = rowData;
        this.AdditionalData = MyAdditionalData;
        if (fieldName.includes(';')) {
            var temp = fieldName.split(';');
            if (temp.length == 2) {
                this.fieldName = temp[0];
                this.taxReportStatusCode = temp[1];
            }
        }
        else {
            this.fieldName = fieldName;
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    TaxReportListTemplate.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    TaxReportListTemplate.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    TaxReportListTemplate.prototype.EditLine = function () {
        var _this = this;
        var lineEntity = this.rowData;
        if (lineEntity) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + lineEntity.Line;
            this._TaxReportPMService.get(lineEntity.TaxReportId).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var report = mm.Result;
                    _this._TaxReportLinePMService.get(report.Id, lineEntity.Line).subscribe(function (myResult) {
                        var mm = myResult;
                        if (!mm.HasError) {
                            _this.CurrentSession.StopBusyIndicator();
                            var linePM = mm.Result;
                            var windowArgs = {};
                            windowArgs.TaxReportPM = report;
                            windowArgs.TaxReportLinePM = linePM;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 450;
                            logWindow.Height = 350;
                            logWindow.Title = windowTitle;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.WindowClosed.subscribe(function (event) {
                                if (event == "ok")
                                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            });
                            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    });
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    TaxReportListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaxReportListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], TaxReportListTemplate);
    return TaxReportListTemplate;
}());
exports.TaxReportListTemplate = TaxReportListTemplate;
//# sourceMappingURL=TaxReportListTemplate.js.map