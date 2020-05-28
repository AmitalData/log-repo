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
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var JournalPM_1 = require("../../EntityPMs/JournalPM");
var JournalLinePM_1 = require("../../EntityPMs/JournalLinePM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var JournalExtendedPMService = /** @class */ (function () {
    function JournalExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + /*'api/journalviews'*/ 'api/journalop';
    }
    JournalExtendedPMService.prototype.VoidJournal = function (tenant, JournalId, AccountingEntityCode, AccountingEntityId, AccountingEntityReference) {
        //http://localhost:9996/api/JournalOp?JournalOp=void&JournalId=1-93808&tenant=1071&AccountingEntityCode=7&AccountingEntityId=Deposit1212&AccountingEntityReference=Cash%20Deposit%207
        var _this = this;
        var url = this._apiUrl + '?JournalOp=void&JournalId=' + JournalId + '&tenant=' + tenant + '&AccountingEntityCode=' + AccountingEntityCode + '&AccountingEntityId=' + AccountingEntityId + '&AccountingEntityReference=' + AccountingEntityReference;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);
            return _this._http.delete(url, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                if (pm) {
                    var mappedResult;
                    //   mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //GetByAccountingEntityId(accountingEntityId) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', SessionInfo.Token);
    //    var url = this._apiUrl + '/GetByAccountingEntityId?accountingEntityId=' + accountingEntityId;
    //    return Observable.defer(() => {
    //        return this._http.get(url, { headers: authHeader }).map(response => {
    //            var result = response.json();
    //            var entity: JournalPM;
    //            if (result) {
    //                entity = this.MapJsonToEntityPM(result);
    //            }
    //            var serviceResponse: ServiceResponse;
    //            serviceResponse = new ServiceResponse();
    //            serviceResponse.Result = entity;
    //            return serviceResponse;
    //        }).catch(ServiceHelper.HandleServiceError);
    //    });
    //}
    JournalExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new JournalPM_1.JournalPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldJournalLines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldJournalLines = entityPM.OldEntityPM.JournalLines;
        }
        entityPM.JournalLines = new Array();
        for (var item in jsonPM.JournalLines) {
            var jItem = jsonPM.JournalLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newJournalLinePM;
            if (mapParent) {
                newJournalLinePM = new JournalLinePM_1.JournalLinePM(entityPM);
            }
            else {
                newJournalLinePM = new JournalLinePM_1.JournalLinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newJournalLinePM[pmProperty] = jItem[pmProperty];
            }
            newJournalLinePM.IsDirty = false;
            if (mapParent) {
                newJournalLinePM.OldEntityPM = this.clone(newJournalLinePM);
                newJournalLinePM.UniqueKey = Guid_1.Guid.newGuid();
                newJournalLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newJournalLinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newJournalLinePM.ChangeSetOp = "Update";
                }
                else {
                    newJournalLinePM.ChangeSetOp = "Insert";
                }
                newJournalLinePM.OldEntityPM = null;
                newJournalLinePM.EntityParentPM = null;
            }
            entityPM.JournalLines.push(newJournalLinePM);
        }
        if (oldJournalLines) {
            for (var itemKey in oldJournalLines) {
                if (entityPM.JournalLines.filter(function (p) { return p.UniqueKey === oldJournalLines[itemKey].UniqueKey; }).length === 0) {
                    if (oldJournalLines[itemKey]) {
                        oldJournalLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.JournalLines.push(oldJournalLines[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.JournalLines = [];
            for (var m in entityPM.JournalLines) {
                entityPM.OldEntityPM.JournalLines.push(this.clone(entityPM.JournalLines[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    JournalExtendedPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    JournalExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], JournalExtendedPMService);
    return JournalExtendedPMService;
}());
exports.JournalExtendedPMService = JournalExtendedPMService;
//# sourceMappingURL=JournalExtendedPMService.js.map