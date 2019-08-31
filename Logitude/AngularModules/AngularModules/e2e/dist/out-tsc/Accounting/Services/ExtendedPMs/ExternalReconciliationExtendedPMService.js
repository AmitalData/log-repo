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
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ExternalReconciliationExtendedPMService = /** @class */ (function () {
    function ExternalReconciliationExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExternalReconciliationExtended';
    }
    //delsertDraftLedgerTransaction(transactions: LedgerTransactionPM[]) {
    //    return Observable.defer(() => {
    //        var authHeader = new Headers();
    //        authHeader.append('Token', SessionInfo.Token);
    //        authHeader.append('Content-Type', 'application/json');
    //        var serviceResponse: ServiceResponse;
    //        serviceResponse = new ServiceResponse();
    //        return this._http.put(this._apiUrl + '/PutDelsertDraftLedgerTransaction/', JSON.stringify(transactions), { headers: authHeader })
    //            .map((res) => {
    //                serviceResponse.Result = res.json();
    //                return serviceResponse;
    //            })
    //            .catch(ServiceHelper.HandleServiceError);
    //    }
    //    );
    //}
    //MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ExternalReconciliationPM = null) {
    //    if (!entityPM) {
    //        entityPM = new ExternalReconciliationPM();
    //    }
    //    var jsonPMKeys = Object.keys(jsonPM);
    //    for (var key in jsonPMKeys) {
    //        if (jsonPMKeys[key] === "UIProperties") {
    //            continue;
    //        }
    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];
    //    }
    //    var oldReconciliationLines: ReconciliationLinePM[] = [];
    //    if (entityPM.OldEntityPM && !mapParent) {
    //        oldReconciliationLines = entityPM.OldEntityPM.ExternalReconciliationLines;
    //    }
    //    entityPM.ExternalReconciliationLines = new Array<ReconciliationLinePM>();
    //    for (var item in jsonPM.ReconciliationLines) {
    //        var jItem = jsonPM.ReconciliationLines[item];
    //        if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
    //            continue;
    //        }
    //        var newReconciliationLinePM: ReconciliationLinePM;
    //        if (mapParent) {
    //            newReconciliationLinePM = new ReconciliationLinePM(entityPM);
    //        }
    //        else {
    //            newReconciliationLinePM = new ReconciliationLinePM(null);
    //        }
    //        var pmKeysArray = Object.keys(jItem);
    //        for (var pmKey in pmKeysArray) {
    //            if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
    //                continue;
    //            }
    //            var pmProperty = pmKeysArray[pmKey];
    //            newReconciliationLinePM[pmProperty] = jItem[pmProperty];
    //        }
    //        newReconciliationLinePM.IsDirty = false;
    //        if (mapParent) {
    //            newReconciliationLinePM.OldEntityPM = this.clone(newReconciliationLinePM);
    //            newReconciliationLinePM.UniqueKey = Guid.newGuid();
    //            newReconciliationLinePM.ChangeSetOp = "None";
    //            jItem.ChangeSetOp = "None";
    //        }
    //        else {
    //            if (newReconciliationLinePM.UniqueKey) {
    //                if (jItem.IsDirty)
    //                    newReconciliationLinePM.ChangeSetOp = "Update";
    //            }
    //            else {
    //                newReconciliationLinePM.ChangeSetOp = "Insert";
    //            }
    //            newReconciliationLinePM.OldEntityPM = null;
    //            newReconciliationLinePM.EntityParentPM = null;
    //        }
    //        entityPM.ExternalReconciliationLines.push(newReconciliationLinePM);
    //    }
    //    if (oldReconciliationLines) {
    //        for (var itemKey in oldReconciliationLines) {
    //            if (entityPM.ExternalReconciliationLines.filter(p => p.UniqueKey === oldReconciliationLines[itemKey].UniqueKey).length === 0) {
    //                if (oldReconciliationLines[itemKey]) {
    //                    oldReconciliationLines[itemKey].ChangeSetOp = "Delete";
    //                    entityPM.ExternalReconciliationLines.push(oldReconciliationLines[itemKey]);
    //                }
    //            }
    //        }
    //    }
    //    entityPM.IsDirty = false;
    //    if (mapParent) {
    //        entityPM.OldEntityPM = this.clone(entityPM);
    //        entityPM.OldEntityPM.ExternalReconciliationLines = [];
    //        for (var m in entityPM.ExternalReconciliationLines) {
    //            entityPM.OldEntityPM.ExternalReconciliationLines.push(this.clone(entityPM.ExternalReconciliationLines[m]));
    //        }
    //    }
    //    else {
    //        entityPM.OldEntityPM = null;
    //    }
    //    return entityPM;
    //}
    ExternalReconciliationExtendedPMService.prototype.clone = function (jsonPM) {
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
    ExternalReconciliationExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ExternalReconciliationExtendedPMService);
    return ExternalReconciliationExtendedPMService;
}());
exports.ExternalReconciliationExtendedPMService = ExternalReconciliationExtendedPMService;
//# sourceMappingURL=ExternalReconciliationExtendedPMService.js.map