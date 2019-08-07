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
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SignStationListTemplate = /** @class */ (function () {
    function SignStationListTemplate(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ShowDate = true;
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
    }
    SignStationListTemplate.prototype.setVariables = function (signStationList, fieldName) {
        ///console.log(rowData);
        this._SignStationList = signStationList;
        this.fieldName = fieldName;
        //#region Set Icons
        switch (this._SignStationList.Status) {
            case "Failure":
                {
                    this.StatusHebrew = "כישלון";
                }
                break;
            case "Waitingtoenterapassword":
                {
                    this.StatusHebrew = "ממתין להזנת סיסמא";
                }
                break;
            case "NoDefinition":
                {
                    this.StatusHebrew = "ללא הגדרה";
                }
                break;
            case "OK":
                {
                    this.StatusHebrew = "תקין";
                }
                break;
            case "Incorrectcard":
                {
                    this.StatusHebrew = "נבחר כרטיס שגוי";
                }
                break;
            case "IncorrectCard":
                {
                    this.StatusHebrew = "נבחר כרטיס שגוי";
                }
                break;
            default:
                {
                    this.StatusHebrew = this._SignStationList.Status;
                }
                break;
        }
        this._ShowDate = (this._SignStationList.LastSignAt.toString() != "0001-01-01T00:00:00");
        //#endregion 
        if (this._SignStationList.IsOk === true) {
            this._Color = "green";
        }
        else if (this._SignStationList.IsOk === false) {
            this._Color = "red";
        }
        else {
            this._Color = "orange";
        }
        this.CD.detectChanges();
    };
    SignStationListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SignStationListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SignStationListTemplate);
    return SignStationListTemplate;
}());
exports.SignStationListTemplate = SignStationListTemplate;
//# sourceMappingURL=SignStationListTemplate.js.map