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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var PaymentChequePM_1 = require("../../EntityPMs/PaymentChequePM");
var PaymentChequeLinePM_1 = require("../../EntityPMs/PaymentChequeLinePM");
var PaymentChequeExtendedPMService = /** @class */ (function () {
    function PaymentChequeExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/PaymentChequeViews';
    }
    PaymentChequeExtendedPMService.prototype.getPaymentChequeByChequeNumber = function (chequeNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetPaymentChequeByChequeNumber?' + 'ChequeNumber=' + chequeNumber, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PaymentChequeExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new PaymentChequePM_1.PaymentChequePM();
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
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        this.MapPaymentChequeLines(entityPM, jsonPM, mapParent); // Call composition tables map methods
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.PaymentChequeLines = [];
            for (var item in entityPM.PaymentChequeLines) {
                var myPaymentChequeLinePM = entityPM.PaymentChequeLines[item];
                var newPaymentChequeLinePM = this.clone(myPaymentChequeLinePM);
                entityPM.OldEntityPM.PaymentChequeLines.push(newPaymentChequeLinePM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    PaymentChequeExtendedPMService.prototype.MapPaymentChequeLines = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldPaymentChequeLines = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPaymentChequeLines = entityPM.OldEntityPM.PaymentChequeLines;
        }
        entityPM.PaymentChequeLines = new Array();
        for (var item in jsonPM.PaymentChequeLines) {
            var jItem = jsonPM.PaymentChequeLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPaymentChequeLinePM;
            if (mapParent) {
                newPaymentChequeLinePM = new PaymentChequeLinePM_1.PaymentChequeLinePM(entityPM);
            }
            else {
                newPaymentChequeLinePM = new PaymentChequeLinePM_1.PaymentChequeLinePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPaymentChequeLinePM[pmProperty] = jItem[pmProperty];
            }
            if (mapParent) {
                newPaymentChequeLinePM.UniqueKey = Guid_1.Guid.newGuid();
                newPaymentChequeLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newPaymentChequeLinePM.OldEntityPM = this.clone(newPaymentChequeLinePM);
            }
            else {
                if (newPaymentChequeLinePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newPaymentChequeLinePM.ChangeSetOp = "Update";
                }
                else {
                    newPaymentChequeLinePM.ChangeSetOp = "Insert";
                }
                newPaymentChequeLinePM.OldEntityPM = null;
                newPaymentChequeLinePM.EntityParentPM = null;
            }
            newPaymentChequeLinePM.IsDirty = false;
            entityPM.PaymentChequeLines.push(newPaymentChequeLinePM);
        }
        if (oldPaymentChequeLines) {
            for (var itemKey in oldPaymentChequeLines) {
                if (entityPM.PaymentChequeLines.filter(function (p) { return p.UniqueKey === oldPaymentChequeLines[itemKey].UniqueKey; }).length === 0) {
                    if (oldPaymentChequeLines[itemKey]) {
                        //oldPaymentChequeLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.PaymentChequeLines.push(oldPaymentChequeLines[itemKey]);
                        var oldItemJson = oldPaymentChequeLines[itemKey];
                        var deletedPM = new PaymentChequeLinePM_1.PaymentChequeLinePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.PaymentChequeLines.push(deletedPM);
                    }
                }
            }
        }
    };
    PaymentChequeExtendedPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    PaymentChequeExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PaymentChequeExtendedPMService);
    return PaymentChequeExtendedPMService;
}());
exports.PaymentChequeExtendedPMService = PaymentChequeExtendedPMService;
//# sourceMappingURL=PaymentChequeExtendedPMService.js.map