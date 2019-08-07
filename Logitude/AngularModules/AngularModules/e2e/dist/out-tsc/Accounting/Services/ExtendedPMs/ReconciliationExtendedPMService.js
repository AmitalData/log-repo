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
var ReconciliationPM_1 = require("../../EntityPMs/ReconciliationPM");
var LedgerTransactionPM_1 = require("../../EntityPMs/LedgerTransactionPM");
var ReconciliationLinePM_1 = require("../../EntityPMs/ReconciliationLinePM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReconciliationExtendedPMService = /** @class */ (function () {
    function ReconciliationExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }
    ReconciliationExtendedPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            //var validator: ClassLevelValidator;
            //validator = new ClassLevelValidator();
            //var errorsArray = validator.Validate("ReconciliationPM", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            //if (errorsArray.length == 0) {
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var _callBack = res.json();
                if (_callBack) {
                    serviceResponse.Result = _callBack;
                    // if(_callBack.isSplitted)
                    // {
                    //     serviceResponse.Result = _callBack;
                    // }
                    // else
                    // {
                    //     // var pm = _callBack.reconciliationPM;
                    //     // if (pm) {
                    //     //     var mappedResult: ReconciliationPM;
                    //     //     //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    //     //     serviceResponse.Result = pm;
                    //     // }
                    // }
                }
                else {
                    console.log("[WARNING!!] no callback for reconciliation!");
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            //}
            //else {
            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;
            //    return Observable.of(serviceResponse);
            //}
        });
    };
    ReconciliationExtendedPMService.prototype.delsertDraftLedgerTransaction = function (transactions) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.put(_this._apiUrl + '/PutDelsertDraftLedgerTransaction/', JSON.stringify(transactions), { headers: authHeader })
                .map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            })
                .catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ReconciliationExtendedPMService.prototype.deleteResetDraftOpenReconciliation = function (gLAccountId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.delete(_this._apiUrl + '/DeleteResetDraftOpenReconciliation?gLAccountId=' + gLAccountId + '&tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant, { headers: authHeader })
                .map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            })
                .catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ReconciliationExtendedPMService.prototype.getDraftReconciliations = function (gLAccountId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetDraftReconciliations?gLAccountId=' + gLAccountId, { headers: authHeader })
                    .map(function (response) {
                    var transactions = response.json();
                    var _mappedListsArray = [];
                    if (transactions) {
                        for (var key in transactions) {
                            var entity;
                            entity = _this.MapJsonToLedgerTransactionPM(transactions[key]);
                            _mappedListsArray.push(entity);
                        }
                    }
                    var serviceResponse = new ServiceResponse_1.ServiceResponse();
                    serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    ReconciliationExtendedPMService.prototype.CreateJournalReconcile = function (myReconciliationLines, TheAccountId, AdjustAccountId, AccountDate, Ref1, Ref2, Ref3, Remarks) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            //var validator: ClassLevelValidator;
            //validator = new ClassLevelValidator();
            //var errorsArray = validator.Validate("ReconciliationPM", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            //if (errorsArray.length == 0) {
            //var mappedEntity: ReconciliationPM[];
            return _this._http.post(_this._apiUrl + "/PostCreateJournalReconcile?"
                + "&TheAccountId=" + TheAccountId
                + "&AdjustAccountId=" + AdjustAccountId
                + "&AccountDate=" + AccountDate
                + "&Ref1=" + Ref1
                + "&Ref2=" + Ref2
                + "&Ref3=" + Ref3
                + "&Remarks=" + Remarks, JSON.stringify(myReconciliationLines), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = pm;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            //}
            //else {
            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;
            //    return Observable.of(serviceResponse);
            //}
        });
    };
    ReconciliationExtendedPMService.prototype.getByNumber = function (number) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetByNumber?number=' + number, { headers: authHeader })
                    .map(function (response) {
                    var entity = response.json();
                    var serviceResponse = new ServiceResponse_1.ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    ReconciliationExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ReconciliationPM_1.ReconciliationPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldReconciliationLines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldReconciliationLines = entityPM.OldEntityPM.ReconciliationLines;
        }
        entityPM.ReconciliationLines = new Array();
        for (var item in jsonPM.ReconciliationLines) {
            var jItem = jsonPM.ReconciliationLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newReconciliationLinePM;
            if (mapParent) {
                newReconciliationLinePM = new ReconciliationLinePM_1.ReconciliationLinePM(entityPM);
            }
            else {
                newReconciliationLinePM = new ReconciliationLinePM_1.ReconciliationLinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newReconciliationLinePM[pmProperty] = jItem[pmProperty];
            }
            newReconciliationLinePM.IsDirty = false;
            if (mapParent) {
                newReconciliationLinePM.OldEntityPM = this.clone(newReconciliationLinePM);
                newReconciliationLinePM.UniqueKey = Guid_1.Guid.newGuid();
                newReconciliationLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newReconciliationLinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newReconciliationLinePM.ChangeSetOp = "Update";
                }
                else {
                    newReconciliationLinePM.ChangeSetOp = "Insert";
                }
                newReconciliationLinePM.OldEntityPM = null;
                newReconciliationLinePM.EntityParentPM = null;
            }
            entityPM.ReconciliationLines.push(newReconciliationLinePM);
        }
        if (oldReconciliationLines) {
            for (var itemKey in oldReconciliationLines) {
                if (entityPM.ReconciliationLines.filter(function (p) { return p.UniqueKey === oldReconciliationLines[itemKey].UniqueKey; }).length === 0) {
                    if (oldReconciliationLines[itemKey]) {
                        oldReconciliationLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.ReconciliationLines.push(oldReconciliationLines[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.ReconciliationLines = [];
            for (var m in entityPM.ReconciliationLines) {
                entityPM.OldEntityPM.ReconciliationLines.push(this.clone(entityPM.ReconciliationLines[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    ReconciliationExtendedPMService.prototype.MapJsonToLedgerTransactionPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new LedgerTransactionPM_1.LedgerTransactionPM();
        }
        var customFields = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    //var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    //entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    ReconciliationExtendedPMService.prototype.clone = function (jsonPM) {
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
    ReconciliationExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ReconciliationExtendedPMService);
    return ReconciliationExtendedPMService;
}());
exports.ReconciliationExtendedPMService = ReconciliationExtendedPMService;
//# sourceMappingURL=ReconciliationExtendedPMService.js.map