System.register(['@angular/core', '@angular/http', 'rxjs/add/operator/map', 'rxjs/Rx', '../../EntityLists/PrepaidCollectList'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, http_1, Rx_1, PrepaidCollectList_1;
    var PrepaidCollectListService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (_1) {},
            function (Rx_1_1) {
                Rx_1 = Rx_1_1;
            },
            function (PrepaidCollectList_1_1) {
                PrepaidCollectList_1 = PrepaidCollectList_1_1;
            }],
        execute: function() {
            PrepaidCollectListService = (function () {
                function PrepaidCollectListService() {
                    this._apiUrl = 'http://localhost:9996/api/prepaidcollectviews';
                }
                PrepaidCollectListService.prototype.setServiceArgs = function (serviceArgs) {
                    this._serviceArgs = serviceArgs;
                    this._http = serviceArgs.http;
                };
                PrepaidCollectListService.prototype.getSingle = function (id) {
                    var _this = this;
                    console.log('--------------------------------------> calling getSingle:');
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    return Rx_1.default.Observable.defer(function () {
                        return _this._http.get(_this._apiUrl + '?' + 'id=' + id, {
                            headers: authHeader
                        }).map(function (response) {
                            var list = response.json();
                            var entity;
                            entity = _this.MapJsonToEntityList(list);
                            return list;
                        });
                    });
                };
                PrepaidCollectListService.prototype.getAll = function () {
                    var _this = this;
                    console.log('--------------------------------------> calling getAllEntityListsFromServer:');
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    Rx_1.default.Observable.defer(function () {
                        return _this._http.get(_this._apiUrl, {
                            headers: authHeader
                        }).map(function (response) {
                            var allLists = response.json();
                            var _mappedListsArray = [];
                            for (var item in allLists) {
                                var entity;
                                entity = _this.MapJsonToEntityList(item);
                                _mappedListsArray.push(item);
                            }
                            return _mappedListsArray;
                        });
                    });
                };
                PrepaidCollectListService.prototype.getByFilters = function (filters) {
                    var _this = this;
                    console.log('--------------------------------------> calling the server with filters:');
                    var urlparameters = '?';
                    var mykeys = Object.keys(filters);
                    var addtionalFiltersValues = null;
                    for (var i in mykeys) {
                        var propName = mykeys[i];
                        var propValue = filters[propName];
                        var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
                        if (urlparameters != "?") {
                            urlparameters = urlparameters.concat('&');
                        }
                        if (!ignoreFilter)
                            urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
                        if (propName == "AdditionalFilters" && propValue.length > 0)
                            addtionalFiltersValues = JSON.stringify(propValue);
                    }
                    if (addtionalFiltersValues) {
                        urlparameters = urlparameters.concat("AdditionalFilters=").concat(JSON.stringify(propValue));
                    }
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    var callUrl = this._apiUrl.concat(urlparameters); //
                    console.log("Calling Url:" + callUrl);
                    var authHeader = new http_1.Headers();
                    authHeader.append('Token', '2acc0a3d-a948-41fe-8a19-51a6d787e4fc');
                    Rx_1.default.Observable.defer(function () {
                        return _this._http.get(callUrl, {
                            headers: authHeader
                        }).map(function (response) {
                            var allLists = response.json();
                            var _mappedListsArray = [];
                            for (var item in allLists) {
                                var entity;
                                entity = _this.MapJsonToEntityList(item);
                                _mappedListsArray.push(item);
                            }
                            return _mappedListsArray;
                        });
                    });
                };
                PrepaidCollectListService.prototype.MapJsonToEntityList = function (jsonList) {
                    var entityList;
                    entityList = new PrepaidCollectList_1.PrepaidCollectList();
                    var jsonListKeys = Object.keys(jsonList);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        entityList[property] = jsonList[property];
                    }
                    return entityList;
                };
                PrepaidCollectListService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [])
                ], PrepaidCollectListService);
                return PrepaidCollectListService;
            })();
            exports_1("PrepaidCollectListService", PrepaidCollectListService);
        }
    }
});
//# sourceMappingURL=PrepaidCollectListService.js.map