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
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var CustomPickListPM_1 = require("../EntityPMs/CustomPickListPM");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var PickListGeneralEntitiesArgs_1 = require("../DataContracts/PickListGeneralEntitiesArgs");
var EntityPMServiceResponse_1 = require("../DataContracts/EntityPMServiceResponse");
var ObjectFieldPM_1 = require("../EntityPMs/ObjectFieldPM");
var ObjectFieldValidationPM_1 = require("../EntityPMs/ObjectFieldValidationPM");
var Guid_1 = require("../Utilities/Guid");
var PropertyChangedArgs_1 = require("../EventEmitterArgs/PropertyChangedArgs");
var ScreenLayoutArgs_1 = require("../DataContracts/ScreenLayoutArgs");
var GeneralDomainService = /** @class */ (function () {
    function GeneralDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/GeneralDomain';
    }
    GeneralDomainService.prototype.GetTranslationsByParam = function (typeCode, tableId, translationLanguageCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTranslationsByParam?typeCode=' + typeCode + '&tableId=' + tableId + '&translationLanguageCode=' + translationLanguageCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapFieldsTranslations(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetStandardFieldsByTableId = function (tableId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetStandardFieldsByTableId?tableId=' + tableId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                //var listMapped: Array<ObjectFieldPM> = [];
                //for (var itemJeson in listJason) {
                //    var itemMapped: ObjectFieldPM = this.MapJsonToObjectFieldPM(listJason[itemJeson]);
                //    listMapped.push(itemMapped);
                //}
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listJason;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetCustomFieldsByTableId = function (tableId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomFieldsByTableId?tableId=' + tableId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetFieldDataTypes = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetFieldDataTypes';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetTranslationsList = function (typeCode) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTranslationsList?typeCode=' + typeCode;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapFieldsTranslations(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.UpdateFieldsTranslations = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToFieldsUpdateHelper(entity, false);
            return _this._http.put(_this._apiUrl + "/PutFieldsTranslations", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToFieldsUpdateHelper(myJsonResult, true, entity);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetTextCodeTypes = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetTextCodeTypes';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapTextCodeType(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.LoadAllFieldsTranslations = function (translationLanguageCode, objectTableId, textCodeType) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetAllFieldsTranslations?language=' + translationLanguageCode + '&objectTableId=' + objectTableId + '&textCodeType=' + textCodeType;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapFieldsTranslations(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetCustomPickListsByCode = function (Code) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomPickListsByCode?code=' + Code;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapFieldsPickList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                //var serviceResponse = new ServiceResponse();
                //serviceResponse.Result = listMapped;
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.insertPickListGeneralEntities = function (entities) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(entities, false);
            return _this._http.post(_this._apiUrl + "/PostPickList", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    //var mappedResult: GeneralEntitiesArgs;
                    //mappedResult = this.MapJsonToEntityPM(pm, true, entities);
                    response.Result = pm;
                }
                return response;
            });
        });
    };
    GeneralDomainService.prototype.updatePickListGeneralEntities = function (entities) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(entities, false);
            return _this._http.put(_this._apiUrl + "/PutPickList", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    //var mappedResult: PickListGeneralEntitiesArgs;
                    //mappedResult = this.MapJsonToEntityPM(pm, true, entities);
                    response.Result = pm;
                }
                return response;
            });
        });
    };
    GeneralDomainService.prototype.updateScreenFields = function (entities) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToScreenFieldsPM(entities, false);
            return _this._http.put(_this._apiUrl + "/PutScreenFields", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    //var mappedResult: PickListGeneralEntitiesArgs;
                    //mappedResult = this.MapJsonToEntityPM(pm, true, entities);
                    response.Result = pm;
                }
                return response;
            });
        });
    };
    GeneralDomainService.prototype.GetScreenModificationByScreenId = function (ScreenId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetScreenModificationByScreenId?ScreenId=' + ScreenId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetSingleObjectFieldFromZeroTenant = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleObjectFieldFromZeroTenant?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToObjectFieldPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.GetSingleObjectFieldByFieldNameAndTableId = function (fieldName, tableId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetSingleObjectFieldByFieldNameAndTableId?fieldName=' + fieldName + '&tableId=' + tableId;
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToObjectFieldPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GeneralDomainService.prototype.MapFieldsTranslations = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new FieldsTranslations();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    GeneralDomainService.prototype.MapFieldsPickList = function (jsonList) {
        var entityList;
        entityList = new CustomPickListPM_1.CustomPickListPM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GeneralDomainService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entities) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entities === void 0) { entities = null; }
        if (!entities) {
            entities = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
            entities.CustomPickListPMs = [];
            entities.RemovedCustomPickListPMs = [];
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entities[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entities;
    };
    GeneralDomainService.prototype.MapJsonToScreenFieldsPM = function (jsonPM, getCallMap, entities) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entities === void 0) { entities = null; }
        if (!entities) {
            entities = new ScreenLayoutArgs_1.ScreenLayoutArgs();
            entities.ScreenFields = [];
            entities.RemovedScreenFields = [];
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entities[property] = jsonPM[property];
        }
        return entities;
    };
    GeneralDomainService.prototype.MapTextCodeType = function (jsonList) {
        var entityList;
        entityList = new TextCodeType();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    GeneralDomainService.prototype.MapJsonToFieldsUpdateHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new FieldsUpdateHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else if (property === "Items") {
                entityPM.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM;
                    newItemPM = this.MapFieldsTranslations(jItem);
                    entityPM.Items.push(newItemPM);
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    GeneralDomainService.prototype.MapJsonToObjectFieldPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ObjectFieldPM_1.ObjectFieldPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapObjectFieldValidations(entityPM, jsonPM, mapParent);
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.ObjectFieldValidations = [];
            for (var item in entityPM.ObjectFieldValidations) {
                var myObjectFieldValidationPM = entityPM.ObjectFieldValidations[item];
                var newObjectFieldValidationPM = this.clone(myObjectFieldValidationPM);
                entityPM.OldEntityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    GeneralDomainService.prototype.MapObjectFieldValidations = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldObjectFieldValidations = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldObjectFieldValidations = entityPM.OldEntityPM.ObjectFieldValidations;
        }
        entityPM.ObjectFieldValidations = new Array();
        for (var item in jsonPM.ObjectFieldValidations) {
            var jItem = jsonPM.ObjectFieldValidations[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newObjectFieldValidationPM;
            if (mapParent) {
                newObjectFieldValidationPM = new ObjectFieldValidationPM_1.ObjectFieldValidationPM(entityPM);
            }
            else {
                newObjectFieldValidationPM = new ObjectFieldValidationPM_1.ObjectFieldValidationPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newObjectFieldValidationPM[pmProperty] = jItem[pmProperty];
            }
            newObjectFieldValidationPM.IsDirty = false;
            if (mapParent) {
                newObjectFieldValidationPM.UniqueKey = Guid_1.Guid.newGuid();
                newObjectFieldValidationPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newObjectFieldValidationPM.OldEntityPM = this.clone(newObjectFieldValidationPM);
            }
            else {
                if (newObjectFieldValidationPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newObjectFieldValidationPM.ChangeSetOp = "Update";
                }
                else {
                    newObjectFieldValidationPM.ChangeSetOp = "Insert";
                }
                newObjectFieldValidationPM.OldEntityPM = null;
                newObjectFieldValidationPM.EntityParentPM = null;
            }
            entityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
        }
        if (oldObjectFieldValidations) {
            for (var itemKey in oldObjectFieldValidations) {
                if (entityPM.ObjectFieldValidations.filter(function (p) { return p.UniqueKey === oldObjectFieldValidations[itemKey].UniqueKey; }).length === 0) {
                    if (oldObjectFieldValidations[itemKey]) {
                        var oldItemJson = oldObjectFieldValidations[itemKey];
                        var deletedPM = new ObjectFieldValidationPM_1.ObjectFieldValidationPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.ObjectFieldValidations.push(deletedPM);
                    }
                }
            }
        }
    };
    GeneralDomainService.prototype.clone = function (jsonPM) {
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
    GeneralDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GeneralDomainService);
    return GeneralDomainService;
}());
exports.GeneralDomainService = GeneralDomainService;
var FieldsTranslations = /** @class */ (function () {
    function FieldsTranslations() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.IsDirty = false;
    }
    Object.defineProperty(FieldsTranslations.prototype, "TranslatedText", {
        get: function () { return this.translatedText; },
        set: function (newValue) { if (this.translatedText != newValue) {
            this.translatedText = newValue;
            this.MarkAsDirty("TranslatedText");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldsTranslations.prototype, "TranslatedTextPlural", {
        get: function () { return this.translatedTextPlural; },
        set: function (newValue) { if (this.translatedTextPlural != newValue) {
            this.translatedTextPlural = newValue;
            this.MarkAsDirty("TranslatedTextPlural");
        } },
        enumerable: true,
        configurable: true
    });
    FieldsTranslations.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], FieldsTranslations.prototype, "PropertyChanged", void 0);
    return FieldsTranslations;
}());
exports.FieldsTranslations = FieldsTranslations;
var TranslationArgs = /** @class */ (function () {
    function TranslationArgs() {
    }
    return TranslationArgs;
}());
exports.TranslationArgs = TranslationArgs;
var TextCodeType = /** @class */ (function () {
    function TextCodeType() {
    }
    return TextCodeType;
}());
exports.TextCodeType = TextCodeType;
var FieldsUpdateHelper = /** @class */ (function () {
    function FieldsUpdateHelper() {
        this.Items = [];
    }
    return FieldsUpdateHelper;
}());
exports.FieldsUpdateHelper = FieldsUpdateHelper;
//# sourceMappingURL=GeneralDomainService.js.map