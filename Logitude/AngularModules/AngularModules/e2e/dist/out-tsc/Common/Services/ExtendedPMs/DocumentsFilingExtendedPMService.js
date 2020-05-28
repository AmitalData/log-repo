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
require("rxjs/add/operator/map");
var DocumentsFilingPM_1 = require("../../EntityPMs/DocumentsFilingPM");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DocumentsFilingMetaDataValuePM_1 = require("../../EntityPMs/DocumentsFilingMetaDataValuePM");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var DocumentsFilingExtendedPMService = /** @class */ (function () {
    function DocumentsFilingExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DocumentsFilingExtended';
    }
    DocumentsFilingExtendedPMService.prototype.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode = function (entityId, childEntityId, objectTableId, directionCode, tenant, withDocuments) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentsFilingPMLists;
            DocumentsFilingPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable = function (entityId, childEntityId, objectTableId, directionCode, tenant, withDocuments) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentsFilingPMLists;
            DocumentsFilingPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.getDocumentsFilingsByEntityIdAndObjectTable = function (entityId, childEntityId, objectTableId, directionCode, tenant, withDocuments) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentsFilingPMLists;
            DocumentsFilingPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.getAllDocumentsFilingsByEntityIdAndObjectTable = function (entityId, objectTableId, directionCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetAllDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentsFilingPMLists;
            DocumentsFilingPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.getRequestedDocumentsFilingsByEntityIdAndObjectTable = function (entityId, objectTableId, directionCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetRequestedDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentsFilingPMLists;
            DocumentsFilingPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.GetFileSizeAndUnit = function (fileBytes) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?fileBytes=' + fileBytes, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.CreateDocumentsFiling = function (documentTypeId, entityId, childEntityId, childReference, objectTableId, directionCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCreateDocumentsFiling" + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.CreateDocumentShipmentEvent = function (entityId, objectTableName, Notes) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCreateDocumentShipmentEvent" + '?entityId=' + entityId + '&objectTableName=' + entityId + '&Notes=' + Notes, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.GetDocumentById = function (documentId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?documentId=' + documentId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.GetDocumentsFilingByDocumentType = function (documentTypeId, objectTableId, entityId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingByDocumentType" + '?documentTypeId=' + documentTypeId + "&objectTableId=" + objectTableId + "&entityId=" + entityId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.IsEntityHasSharedDocs = function (entityId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?entityId=' + entityId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.GetSingleDocumentsFilingByChild = function (documentTypeId, paymentNumber, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetSingleDocumentsFilingByChild" + '?documentTypeId=' + documentTypeId + '&paymentNumber=' + paymentNumber + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            if (result != null) {
                entity = _this.MapJsonToEntityPM(result);
            }
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService.prototype.ShareDocumentsWithAgent = function (Ids) {
        var _this = this;
        // Send request
        return Rx_1.Observable.defer(function () {
            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(function (el) {
                    IdsParameterString += 'Ids=' + el + '&';
                });
            }
            else {
                console.log("[ERROR] cannot Archive Shipments without Ids!", Ids);
                return;
            }
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetShareDocumentsWithAgent/?" + IdsParameterString, { headers: authHeader }).map(function (response) {
                //var res = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DocumentsFilingExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new DocumentsFilingPM_1.DocumentsFilingPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    DocumentsFilingExtendedPMService.prototype.AddEditMapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DocumentsFilingPM_1.DocumentsFilingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldDocumentsFilingMetaDataValues = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldDocumentsFilingMetaDataValues = entityPM.OldEntityPM.DocumentsFilingMetaDataValues;
        }
        entityPM.DocumentsFilingMetaDataValues = new Array();
        for (var item in jsonPM.DocumentsFilingMetaDataValues) {
            var jItem = jsonPM.DocumentsFilingMetaDataValues[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newDocumentsFilingMetaDataValuePM;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM_1.DocumentsFilingMetaDataValuePM(entityPM);
            }
            else {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM_1.DocumentsFilingMetaDataValuePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newDocumentsFilingMetaDataValuePM[pmProperty] = jItem[pmProperty];
            }
            newDocumentsFilingMetaDataValuePM.IsDirty = false;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM.OldEntityPM = this.clone(newDocumentsFilingMetaDataValuePM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid_1.Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newDocumentsFilingMetaDataValuePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Update";
                }
                else {
                    newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Insert";
                }
                newDocumentsFilingMetaDataValuePM.OldEntityPM = null;
                newDocumentsFilingMetaDataValuePM.EntityParentPM = null;
            }
            entityPM.DocumentsFilingMetaDataValues.push(newDocumentsFilingMetaDataValuePM);
        }
        if (oldDocumentsFilingMetaDataValues) {
            for (var itemKey in oldDocumentsFilingMetaDataValues) {
                if (entityPM.DocumentsFilingMetaDataValues.filter(function (p) { return p.UniqueKey === oldDocumentsFilingMetaDataValues[itemKey].UniqueKey; }).length === 0) {
                    if (oldDocumentsFilingMetaDataValues[itemKey]) {
                        oldDocumentsFilingMetaDataValues[itemKey].ChangeSetOp = "Delete";
                        entityPM.DocumentsFilingMetaDataValues.push(oldDocumentsFilingMetaDataValues[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.DocumentsFilingMetaDataValues = [];
            for (var m in entityPM.DocumentsFilingMetaDataValues) {
                entityPM.OldEntityPM.DocumentsFilingMetaDataValues.push(this.clone(entityPM.DocumentsFilingMetaDataValues[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    DocumentsFilingExtendedPMService.prototype.insert = function (entityPM, DontUseComposition) {
        var _this = this;
        if (DontUseComposition === void 0) { DontUseComposition = false; }
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("DocumentsFiling", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                if (DontUseComposition == true) {
                    mappedEntity = _this.CustomMapJsonToEntityPM(entityPM, false);
                }
                else {
                    mappedEntity = _this.AddEditMapJsonToEntityPM(entityPM, false);
                }
                return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.AddEditMapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(serviceResponse);
            }
        });
    };
    DocumentsFilingExtendedPMService.prototype.update = function (entityPM, DontUseComposition) {
        var _this = this;
        if (DontUseComposition === void 0) { DontUseComposition = false; }
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("DocumentsFiling", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                if (DontUseComposition == true) {
                    mappedEntity = _this.CustomMapJsonToEntityPM(entityPM, false);
                }
                else {
                    mappedEntity = _this.AddEditMapJsonToEntityPM(entityPM, false);
                }
                return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.AddEditMapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(serviceResponse);
            }
        });
    };
    DocumentsFilingExtendedPMService.prototype.CustomMapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DocumentsFilingPM_1.DocumentsFilingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldDocumentsFilingMetaDataValues = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldDocumentsFilingMetaDataValues = entityPM.OldEntityPM.DocumentsFilingMetaDataValues;
        }
        entityPM.DocumentsFilingMetaDataValues = new Array();
        for (var item in jsonPM.DocumentsFilingMetaDataValues) {
            var jItem = jsonPM.DocumentsFilingMetaDataValues[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newDocumentsFilingMetaDataValuePM;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM_1.DocumentsFilingMetaDataValuePM(entityPM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid_1.Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM_1.DocumentsFilingMetaDataValuePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newDocumentsFilingMetaDataValuePM[pmProperty] = jItem[pmProperty];
            }
            newDocumentsFilingMetaDataValuePM.IsDirty = false;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM.OldEntityPM = this.clone(newDocumentsFilingMetaDataValuePM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid_1.Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newDocumentsFilingMetaDataValuePM.ChangeSetOp) {
                }
                else {
                    if (newDocumentsFilingMetaDataValuePM.UniqueKey) {
                        if (jItem.IsDirty)
                            newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Update";
                    }
                    else {
                        newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Insert";
                    }
                }
                newDocumentsFilingMetaDataValuePM.OldEntityPM = null;
                newDocumentsFilingMetaDataValuePM.EntityParentPM = null;
            }
            entityPM.DocumentsFilingMetaDataValues.push(newDocumentsFilingMetaDataValuePM);
        }
        if (oldDocumentsFilingMetaDataValues) {
            for (var itemKey in oldDocumentsFilingMetaDataValues) {
                if (entityPM.DocumentsFilingMetaDataValues.filter(function (p) { return p.UniqueKey === oldDocumentsFilingMetaDataValues[itemKey].UniqueKey; }).length === 0) {
                    if (oldDocumentsFilingMetaDataValues[itemKey]) {
                        oldDocumentsFilingMetaDataValues[itemKey].ChangeSetOp = "Delete";
                        entityPM.DocumentsFilingMetaDataValues.push(oldDocumentsFilingMetaDataValues[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.DocumentsFilingMetaDataValues = [];
            for (var m in entityPM.DocumentsFilingMetaDataValues) {
                entityPM.OldEntityPM.DocumentsFilingMetaDataValues.push(this.clone(entityPM.DocumentsFilingMetaDataValues[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    DocumentsFilingExtendedPMService.prototype.clone = function (jsonPM) {
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
    DocumentsFilingExtendedPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new DocumentsFilingPM_1.DocumentsFilingPM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    DocumentsFilingExtendedPMService.prototype.GetLogBoxConnectedDocs = function (SourceEntityId, DestEntityId, ObjectTableId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?SourceEntityId=' + SourceEntityId + "&DestEntityId=" + DestEntityId + "&ObjectTableId=" + ObjectTableId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentsFilingExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DocumentsFilingExtendedPMService);
    return DocumentsFilingExtendedPMService;
}());
exports.DocumentsFilingExtendedPMService = DocumentsFilingExtendedPMService;
//# sourceMappingURL=DocumentsFilingExtendedPMService.js.map